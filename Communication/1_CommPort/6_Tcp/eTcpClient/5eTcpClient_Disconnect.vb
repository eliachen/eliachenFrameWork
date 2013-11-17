Imports System.Net.Sockets

Namespace CommPort
    Partial Public Class eTcpClient

        '断开连接
        Private Function eDisConnect() As SocketError
            Try
                Me.CommSocket.Shutdown(SocketShutdown.Both)
                'Me.CommSocket.Disconnect(True)

                Me.CommSocket.Disconnect(False)


                'Me.CommSocket.Close()
                'Me.CommSocket = Nothing
                '记录
                CommLog.LogSuccess(Log.CommPortLogElement(Of eTcpClientLog).MsgProcessDescriptionModel.disconnect,
                                                                  New eTcpClientLog(Me.ActivatedSocketState))
            Catch exSk As SocketException
                '更新
                ActivatedSocketState = exSk.SocketErrorCode
                '记录
                CommLog.LogError(Log.CommPortLogElement(Of eTcpClientLog).MsgProcessDescriptionModel.disconnect,
                                                                  New eTcpClientLog(exSk.ErrorCode))

                Return exSk.SocketErrorCode
            Catch ex As Exception
                '记录
                CommLog.LogError(Log.CommPortLogElement(Of eTcpClientLog).MsgProcessDescriptionModel.disconnect,
                                                                  New eTcpClientLog(Me.ActivatedSocketState, ex.ToString))

            End Try
            Return SocketError.Success
        End Function
    End Class
End Namespace


