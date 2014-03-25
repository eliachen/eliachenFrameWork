Imports System.Net.Sockets
Imports Communication.TcpClient

Namespace CommPort
    Partial Public Class TcpClient
        Public Overloads Sub Send(SendArrByte() As Byte) Implements ICommPort.Send
            eSend(SendArrByte)
        End Sub

#Region "Send"

#End Region

        '发送接口
        Private Sub eSend(ByVal ArrByte As Byte())
            RaiseEvent mEventSend(Nothing, ArrByte)
        End Sub

        '利用事件队列处理的发送函数
        Private Sub eSend_Queue(ByRef obj As Object, ByVal ArrByte As Byte()) Handles Me.mEventSend
            Try
                If Me.CommPortWay.TimeDelaySend > 0 Then
                    '延迟发送
                    System.Threading.Thread.Sleep(Me.CommPortWay.TimeDelaySend)
                End If

                '发送
                Dim eSendS As New TcpClientState(ArrByte)
                Me.CommSocket.Send(eSendS.ArrBuffer, 0, eSendS.ArrBuffer.Length, SocketFlags.None, eSendS.ErrSocket)
                '统计
                Me.CommStatics.CountSend = Me.CommStatics.CountSend + 1
                Me.CommStatics.SumSend = Me.CommStatics.SumSend + eSendS.ArrBuffer.Count

                '更新状态
                Me.ActivatedSocketState = eSendS.ErrSocket

                If eSendS.ErrSocket <> SocketError.Success Then
                    '记录
                    CommLog.LogError(Log.CommPortLogElement(Of TcpClientLog).MsgProcessDescriptionModel.send,
                                                                      New TcpClientLog(eSendS.ErrSocket))
                End If
            Catch exSk As SocketException
                '记录
                CommLog.LogError(Log.CommPortLogElement(Of TcpClientLog).MsgProcessDescriptionModel.send,
                                                                  New TcpClientLog(exSk.ErrorCode))
            Catch ex As Exception
                CommLog.LogError(Log.CommPortLogElement(Of TcpClientLog).MsgProcessDescriptionModel.other,
                                                                  New TcpClientLog(Me.ActivatedSocketState, ex.ToString))
            End Try
        End Sub
    End Class
End Namespace

