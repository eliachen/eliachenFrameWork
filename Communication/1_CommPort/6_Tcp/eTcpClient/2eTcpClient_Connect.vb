Imports System.Net.Sockets

Namespace CommPort
    Partial Public Class eTcpClient
#Region "Connect"
        Private Function eConnect() As SocketError
            Try
                Dim tmpEndPoint As New System.Net.IPEndPoint(Net.IPAddress.Parse(Me.CommCofigStr.Split(",")(0)), _
                                                             Me.CommCofigStr.Split(",")(1))
                Me.CommSocket.Connect(tmpEndPoint)

                '记录：
                CommLog.LogSuccess(Log.CommPortLogElement(Of eTcpClientLog).MsgProcessDescriptionModel.connect,
                                   New eTcpClientLog(Me.ActivatedSocketState, ""))
                'exSk：记录Socket的错误并且保存状态
            Catch exSk As SocketException
                Me.ActivatedSocketState = exSk.SocketErrorCode
                CommLog.LogError(Log.CommPortLogElement(Of eTcpClientLog).MsgProcessDescriptionModel.connect,
                                    New eTcpClientLog(exSk.SocketErrorCode))
                Return exSk.SocketErrorCode
                'ex:抛出别的错误
            Catch ex As Exception
                CommLog.LogError(Log.CommPortLogElement(Of eTcpClientLog).MsgProcessDescriptionModel.connect,
                                    New eTcpClientLog(Me.ActivatedSocketState, ex.ToString))
            End Try

            '无错误则返回成功
            Return SocketError.Success
        End Function
#End Region
    End Class
End Namespace


