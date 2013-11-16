Imports System.Net
Imports System.Net.Sockets
Namespace CommPort
    Partial Public Class eTcpSever

        '添加事件
        Private Delegate Sub DelRegMemBer(ByRef Mem As TcpClientMember)
        Private Event EventRegMember As DelRegMemBer
        Public Sub Start() Implements ICommSever(Of TcpClientMember).Start
            Try
                If Not Me.CommSeverState = ICommSever(Of TcpClientMember).StateModel.running Then
                    '若停止后需求重新创建
                    If Me.CommSeverSocket Is Nothing Then
                        InitialByStr()
                    End If
                    If eListen() = SocketError.Success Then
                        '更新当前状态:运行
                        Me.CommSeverState = ICommSever(Of TcpClientMember).StateModel.running
                        eAccept()
                        '记录
                        CommSeverLog.LogSuccess(Log.CommSeverLogElement(Of String, eTcpSever).MsgProcessDescriptionModel.start)
                    Else
                        '记录
                        CommSeverLog.LogError(Log.CommSeverLogElement(Of String, eTcpSever).MsgProcessDescriptionModel.stop)
                    End If
                End If
            Catch ex As Exception
                '更新当前状态:错误
                Me.CommSeverState = ICommSever(Of TcpClientMember).StateModel.error
                '记录当前状态
                CommSeverLog.LogError(Log.CommSeverLogElement(Of String, eTcpSever).MsgProcessDescriptionModel.start, ex.ToString)
            End Try
        End Sub

        Private Function eListen() As SocketError
            '记录

            Try
                Dim tmpCig As String() = Me.CommSeverConfigStr.Split(",")
                '绑定
                CommSeverSocket.Bind(New IPEndPoint(IPAddress.Parse(Me.CommSeverConfigStr.Split(",")(0)), _
                                                   Integer.Parse(Me.CommSeverConfigStr.Split(",")(1))))
                '监听
                CommSeverSocket.Listen(Integer.Parse(Me.CommSeverConfigStr.Split(",")(1)))


            Catch Skex As SocketException
                '更新
                Me.CommSeverSocketState = Skex.SocketErrorCode
                '记录
                CommSeverLog.LogError(Log.CommSeverLogElement(Of String, eTcpSever).MsgProcessDescriptionModel.start, "SkErr:" & Skex.ToString)
                Return Skex.SocketErrorCode
            Catch ex As Exception
                '记录
                CommSeverLog.LogError(Log.CommSeverLogElement(Of String, eTcpSever).MsgProcessDescriptionModel.start, "SkErr:" & ex.ToString)
            End Try
            Return SocketError.Success
        End Function

        '接收
        <STAThread> _
        Private Sub eAccept()
            If Me.CommSeverState = ICommSever(Of TcpClientMember).StateModel.running _
                Or Me.CommSeverState = ICommSever(Of TcpClientMember).StateModel.error Then
                CommSeverSocket.BeginAccept(New AsyncCallback(Sub(Iar As IAsyncResult)
                                                                  Try

                                                                      '接收新的连接
                                                                      Dim TmpRecvSk As Socket = CommSeverSocket.EndAccept(Iar)
                                                                      '组建
                                                                      Dim RecvSt As TcpClientMember = New TcpClientMember(TmpRecvSk)

                                                                      '暂停状态,断开连接
                                                                      If Me.CommSeverState = ICommSever(Of TcpClientMember).StateModel.pause Then
                                                                          RecvSt.DisConnect()
                                                                          Exit Sub
                                                                      End If

                                                                      Try
                                                                          '开始接收数据
                                                                          RecvSt.CirclRecv()
                                                                          '注册事件
                                                                          AddHandler RecvSt.RecvDataEvent, Sub(ByRef sender As Object, Arrbyte As Byte())
                                                                                                               RaiseEvent RecvData(DirectCast(sender, TcpClientMember), Arrbyte)
                                                                                                           End Sub
                                                                          '登记:队列模型
                                                                          RaiseEvent EventRegMember(RecvSt)
                                                                      Catch Skex As SocketException
                                                                          '更新
                                                                          Me.CommSeverSocketState = Skex.SocketErrorCode
                                                                          '记录
                                                                          CommSeverLog.LogError(Log.CommSeverLogElement(Of String, eTcpSever).MsgProcessDescriptionModel.recvconn, "SkErr:" & Skex.ToString)
                                                                      Catch ex As Exception
                                                                          '记录
                                                                          CommSeverLog.LogError(Log.CommSeverLogElement(Of String, eTcpSever).MsgProcessDescriptionModel.recvconn, ex.ToString)
                                                                      End Try
                                                                  Catch ex As Exception
                                                                      Exit Sub
                                                                  Finally
                                                                      '循环执行接入函数
                                                                      eAccept()
                                                                  End Try
                                                              End Sub), Nothing)
            End If
        End Sub

        Private Sub RegMemberAction(ByRef mem As TcpClientMember) Handles Me.EventRegMember
            If Me.ClientList.Count = 0 Then
                mem.RecvId = 0
            Else
                mem.RecvId = (Aggregate p In Me.ClientList Into Max(p.RecvId)) + 1
            End If
            Me.ClientList.Add(mem)
            '记录
            CommSeverLog.LogSuccess(Log.CommSeverLogElement(Of String, eTcpSever).MsgProcessDescriptionModel.recvconn, mem.CommCofigStr)
        End Sub
    End Class
End Namespace

