
Namespace CommPort
    Partial Public Class eTcpClient
        Public Overloads Sub Dispose()
            Me.CommSocket.Dispose()
            '释放资源
            Me.ActivatedSocketState = Nothing
            '取消事件的关联
            RemoveHandler mEventSend, AddressOf eSend_Queue
            '接收释放
            Recv_Dispose()
            '设置释放
            Config_Dispose()
        End Sub
    End Class
End Namespace
'释放资源

