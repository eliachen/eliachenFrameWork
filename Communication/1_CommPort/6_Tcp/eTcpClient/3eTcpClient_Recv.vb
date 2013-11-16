Imports System.Threading.Thread
Imports System.Threading
Imports System.Net.Sockets
Imports System.Runtime.InteropServices

Namespace CommPort
    '处理接收
    Partial Public Class eTcpClient
        '一个接收的定时器
        Private Timer_Recv As System.Timers.Timer

#Region " Recv"

        Private priSkip As Boolean = False

        '接收
        Public Sub CirclRecv()
            Try
                '记录：
                CommLog.LogSuccess(Log.CommPortLogElement(Of eTcpClientLog).MsgProcessDescriptionModel.recv,
                                   New eTcpClientLog(Me.ActivatedSocketState, "开始接收"))
                '初始化接收定时器
                Timer_Recv = New System.Timers.Timer
                '设置定时器的时间
                Timer_Recv.Interval = Config_TimerRecvInk
                '关联接收事件
                AddHandler Timer_Recv.Elapsed, Sub(sender As Object, e As Timers.ElapsedEventArgs)
                                                   '避免多次检测
                                                   If Interlocked.Exchange(priSkip, priSkip) Then
                                                       Exit Sub
                                                   End If

                                                   Try
                                                       '获取当前可读取的字节总数
                                                       Dim RecvSum As Integer = Me.CommSocket.Available

                                                       ''///数据来的触发///
                                                       If RecvSum > 0 Then
                                                           '修改标志
                                                           Interlocked.Exchange(priSkip, True)
                                                           Dim RecvByte As Byte() = Me.CommPortWay.CommDeal(Me)

                                                           If RecvByte.Count > 0 Then
                                                               '统计
                                                               Me.CommStatics.CountRecv = Me.CommStatics.CountRecv + 1
                                                               Me.CommStatics.SumRecv = Me.CommStatics.SumRecv + RecvByte.Count
                                                               RaiseEvent RecvDataEvent(Me, RecvByte)

                                                           End If

                                                           '修改标志
                                                           Interlocked.Exchange(priSkip, False)

                                                       End If
                                                   Catch exSk As SocketException
                                                       '记录
                                                       CommLog.LogError(Log.CommPortLogElement(Of eTcpClientLog).MsgProcessDescriptionModel.recv,
                                                                                                         New eTcpClientLog(exSk.SocketErrorCode))
                                                   Catch ex As Exception
                                                       '记录
                                                       CommLog.LogError(Log.CommPortLogElement(Of eTcpClientLog).MsgProcessDescriptionModel.recv,
                                                                 New eTcpClientLog(Me.ActivatedSocketState, ex.ToString))
                                                   End Try
                                               End Sub

                '定时器开始
                Timer_Recv.Start()
                '抛出异常
            Catch ex As Exception
                CommLog.LogError(Log.CommPortLogElement(Of eTcpClientLog).MsgProcessDescriptionModel.other,
                                                                  New eTcpClientLog(Me.ActivatedSocketState, ex.ToString))
            End Try
        End Sub

        Public Overloads Function Receive() As Byte() Implements ICommPort.Receive

            Try
                Using RecvClient As New eTcpClientState
                        ReDim RecvClient.ArrBuffer(Me.CommSocket.Available - 1)
                    RecvClient.CommDone.Reset()

                    Me.CommSocket.BeginReceive(RecvClient.ArrBuffer, 0, RecvClient.ArrBuffer.Length, _
                                               SocketFlags.None, _
                                               RecvClient.ErrSocket, _
                                               New AsyncCallback(Sub(Iar As IAsyncResult)

                                                                     Dim RecLen As Integer = Me.CommSocket.EndReceive(Iar)

                                                                     If Not RecLen > 0 Then
                                                                         '无可读数据则检查Socket状态
                                                                         RecvClient.ErrSocket = CheckState()

                                                                     End If
                                                                     RecvClient.CommDone.Set()
                                                                 End Sub), RecvClient)
                    RecvClient.CommDone.WaitOne()

                    '返回Socket状态
                    Me.ActivatedSocketState = RecvClient.ErrSocket
                    If RecvClient.ErrSocket <> SocketError.Success Then
                        '抛出异常
                        Throw New SocketException(RecvClient.ErrSocket)
                    End If

                    If RecvClient.ArrBuffer.Count > 0 Then
                        Return RecvClient.ArrBuffer
                    Else
                        Return New Byte() {}
                    End If
                End Using
            Catch exSk As SocketException
                '更新状态
                Me.ActivatedSocketState = exSk.ErrorCode
                '抛出异常
                Throw New SocketException(exSk.ErrorCode)
                '返回空字节数组
                Return New Byte() {}
            Catch ex As Exception
                Throw ex
            End Try

            '    '获取当前可读取的字节总数
            '    Dim RecvSum As Integer = Me.CommSocket.Available

            '    '等效事件响应，表示当前具有数据
            '    If RecvSum > 0 Then
            '        '收到的数据进行保存
            '        Dim eRecvS As New eTcpClientState(RecvSum)

            '        '接收数据
            '        Me.CommSocket.Receive(eRecvS.ArrBuffer, 0, eRecvS.ArrBuffer.Length, SocketFlags.None, eRecvS.ErrSocket)

            '        '返回Socket状态
            '        Me.ActivatedSocketState = eRecvS.ErrSocket

            '        '返回数据
            '        Return eRecvS.ArrBuffer
            '    Else
            '        '收到0字节数据有可能出错
            '        Dim tmpState As SocketError = CheckState()
            '        '更新状态
            '        Me.ActivatedSocketState = tmpState
            '        If tmpState <> SocketError.Success Then
            '            Throw New SocketException(tmpState)
            '        End If

            '        '返回空字节数组
            '        Return New Byte() {}
            '    End If
            'Catch exSk As SocketException
            '    '更新状态
            '    Me.ActivatedSocketState = exSk.ErrorCode
            '    '返回空字节数组
            '    Return New Byte() {}
            'Catch ex As Exception
            '    Throw ex
            'End Try
        End Function

        Public Function ReceiveAsWay(Way As ICommPortWay) As Byte() Implements ICommPort.ReceiveAsWay
            Return Me.CommPortWay.CommDeal(Me)
        End Function
#End Region

        'Dispose
        Private Sub Recv_Dispose()
            If Timer_Recv IsNot Nothing Then
                Timer_Recv.Close()
            End If
        End Sub
    End Class
End Namespace

