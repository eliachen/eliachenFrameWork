Imports System.Threading
Imports System.Text
Namespace Communication.CommPort.Port
    Public Class DDpSever
        Private mCommPortWay As ICommPortWay
        Private winFormHandle As IntPtr

        Public serv_reply As Boolean = True
        Public serv_start As Integer = 0
        Public serv_ip As String
        Public connect_time As Integer
        Public refresh_time As Integer
        Public serv_port As Integer
        Public serv_type As Integer
        Public serv_mode As Integer
        Public sign As Integer
        Public recvdata As Boolean
        Public threadrun As Boolean
        '控制线程
        Public thread As Thread
        '阻塞模式读取数据线程

        Public listUser As New EliachenFw.Buffer.BufferList(Of DDpClient)

        Delegate Sub Del_DataRecv(ByVal DdpData As DDpClient)
        Public Event Event_DataRecv As Del_DataRecv

        Sub New(ByVal _hw As IntPtr)
            winFormHandle = _hw
        End Sub

        Public Sub StartSever()

            Try
                serv_start = 0
                Dim sign As Int32 = 9999
                '设置环节
                serv_ip = DNS.GetLocalAddr(0).ToString
                connect_time = 30
                refresh_time = 3
                serv_port = 3645
                serv_type = 0
                serv_mode = 2



                SetCustomIP(inet_addr(serv_ip))
                SetWorkMode(serv_mode) '开发包函数，设置服务模式：2-消息，0-阻塞，1-非阻塞
                SelectProtocol(serv_type) '开发包函数，设置服务类型：0-UDP，1-TCP
                Dim mess As New StringBuilder(1000)

                If serv_mode = 2 Then
                    '开发包函数，消息模式启动服务
                    sign = start_net_service(winFormHandle, WM_DTU, serv_port, mess)
                Else
                    '开发包函数，非消息模式启动服务
                    sign = start_net_service(winFormHandle, 0, serv_port, mess)
                    '需启动定时器来收
                End If

                '为0启动成功
                If sign = 0 Then
                    serv_start = 1
                Else
                    serv_start = 0
                End If
            Catch ex As Exception

            End Try


        End Sub

        Public Sub EndSever()
            Dim mess As New StringBuilder(1000)

            '停止服务
            do_close_all_user2(mess) '开发包函数，使所有DTU下线

            If serv_mode <> 2 Then
                'timer3.Enabled = false;
            End If

            If serv_mode = 0 Then
                cancel_read_block() '阻塞模式下取消阻塞读取
                threadrun = False '退出线程处理函数
                thread.Abort() '终止线程
            End If

            '服务停止成功
            If stop_net_service(mess) = 0 Then '开发包函数，停止服务

            End If
        End Sub

        '非阻塞模式下数据处理
        Private Function GetData0() As Byte()
            Try
                Dim recdPtr As New GPRS_DATA_RECORD()
                Dim mess As New StringBuilder(100)

                'If serv_mode = 1 Then
                If do_read_proc(recdPtr, mess, serv_reply) >= 0 Then
                    Select Case recdPtr.m_data_type
                        Case &H1
                            Dim user_info As New GPRS_USER_INFO()
                            Dim usPort As UInt16 = 0
                            If get_user_info(recdPtr.m_userid, user_info) = 0 Then
                                Dim tmp As New DDpClient
                                tmp.Data = Nothing
                                tmp.User = user_info
                                '添加用户
                                AddUser(tmp)
                            End If
                        Case &H2     '收到注销包，刷新终端登陆列表
                            DeleteUser(recdPtr.m_userid)
                        Case &H4    '无效包
                            Exit Select
                        Case &H9     '数据包

                            Dim tmpold As DDpClient = GetUser(recdPtr.m_userid)
                            '获取数据
                            Dim tmpnew As New DDpClient
                            tmpnew.User = tmpold.User
                            tmpnew.Data = recdPtr

                            '更新回去
                            tmpold.Update(tmpnew)

                            '临时事件
                            Dim Dosth = Sub()
                                            '按照设定接收
                                            tmpnew.Data.m_data_buf = tmpnew.ReceiveAsWay(tmpnew.CommPortWay)
                                            '触发接收数据事件
                                            RaiseEvent Event_DataRecv(tmpnew)
                                        End Sub

                            '使用系统的线程
                            If ThreadPool.QueueUserWorkItem(Sub()
                                                                Dosth()
                                                            End Sub) Then

                            Else
                                Dim t As New Thread(Sub()
                                                        Dosth()
                                                    End Sub)
                                t.Start()
                            End If

                        Case &HD '参数设置成功

                        Case &HB '参数查询成功

                        Case &H6 '断开PPP连接成功

                        Case &H7 '停止向DSC发送数据

                        Case &H8 '允许向DSC发送数据

                        Case &HA '丢弃用户数据

                    End Select
                End If
                'End If

                Return New Byte() {}
            Catch ex As Exception
                Return New Byte() {}
            End Try
        End Function

        '阻塞模式下数据处理
        Private Function GetData1() As Byte()
            Return GetData0()
        End Function

        '消息模式时定义消息的响应函数
        Public Function GetData2(ByVal m As Message) As Byte()
            If m.Msg = WM_DTU Then
                Return GetData0()
            Else
                Return New Byte() {}
            End If
        End Function
#Region "用户列表维护"

        '获取用户
        Private Function GetUser(ByVal t_id As String) As DDpClient
            Return (From s In listUser.SyncBufferList Where t_id = s.User.m_userid
                       Select s).First
        End Function

        '添加或更新用户
        Private Sub AddUser(ByVal t As DDpClient)
            Dim Rel = From s In listUser.SyncBufferList Where t.User.m_userid = s.User.m_userid
                       Select s

            If Rel.ToArray.Length > 0 Then
                '更新
                Rel(0).Data = t.Data
                Rel(0).User = t.User
            Else
                '添加
                Me.listUser.Add(t)
            End If
        End Sub
        '删除用户
        Private Sub DeleteUser(ByVal t_id As String)
            listUser.SyncBufferList.RemoveAll(Function(s As DDpClient)
                                                  If s.User.m_userid = t_id Then
                                                      Return True
                                                  Else
                                                      Return False
                                                  End If
                                              End Function)

        End Sub
#End Region
    End Class
End Namespace

