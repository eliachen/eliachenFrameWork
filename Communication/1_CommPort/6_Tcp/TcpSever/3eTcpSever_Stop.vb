Imports System.Net
Imports System.Net.Sockets
Imports System
Imports System.Threading.Tasks.Parallel
Namespace CommPort
    Partial Public Class TcpSever
        Public Sub [Stop]() Implements ICommSever(Of TcpClientMember).Stop
            If Not Me.CommSeverState = ICommSever(Of TcpClientMember).StateModel.stopped Then

                Try
                    '所有接口都断开连接
                    ForEach(Of TcpClientMember)(Me.ClientList, Sub(ea)
                                                                   '断开连接
                                                                   ea.DisConnect()
                                                               End Sub)
                    '清空列表
                    Me.ClientList.Clear()
                    '释放
                    Me.CommSeverSocket.Close()
                    Me.CommSeverSocket = Nothing
                    '更新状态
                    Me.CommSeverState = ICommSever(Of TcpClientMember).StateModel.stopped
                    '记录
                    CommSeverLog.LogSuccess(Log.CommSeverLogElement(Of String, TcpSever).MsgProcessDescriptionModel.stop)
                Catch exSk As SocketException
                    '更新状态
                    Me.CommSeverState = ICommSever(Of TcpClientMember).StateModel.error
                    '记录
                    CommSeverLog.LogError(Log.CommSeverLogElement(Of String, TcpSever).MsgProcessDescriptionModel.stop, "SkErr:" & exSk.ErrorCode)
                Catch ex As Exception
                    '更新状态
                    Me.CommSeverState = ICommSever(Of TcpClientMember).StateModel.error
                    '记录
                    CommSeverLog.LogError(Log.CommSeverLogElement(Of String, TcpSever).MsgProcessDescriptionModel.stop, ex.ToString)
                End Try
            End If
        End Sub

    End Class
End Namespace

