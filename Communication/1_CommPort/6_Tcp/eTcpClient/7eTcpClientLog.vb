
Namespace CommPort
    Partial Public Class eTcpClient
        Public Class eTcpClientLog

            Property SkErr As System.Net.Sockets.SocketError

            Property Msg As String

            Sub New()
                MyBase.New()
            End Sub

            Sub New(ByRef exSkErr As System.Net.Sockets.SocketError)
                Me.SkErr = exSkErr
                Me.Msg = SkErrToMsg(exSkErr)
            End Sub

            Sub New(ByRef exSkErr As System.Net.Sockets.SocketError, ByRef Msg As String)
                Me.SkErr = exSkErr
                Me.Msg = Msg
            End Sub

            Private Function SkErrToMsg(ByVal skErr As System.Net.Sockets.SocketError)
                Return skErr.ToString
            End Function
        End Class
    End Class
End Namespace

