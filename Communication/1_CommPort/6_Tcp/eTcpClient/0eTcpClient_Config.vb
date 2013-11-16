Namespace CommPort
    '内部一些参数的设置
    Partial Public Class eTcpClient
#Region "New"
        '多长时间后开始第一次探测:ms
        Private Config_KeepaLiveTime As Integer = 5000
        '探测时间间隔:ms
        Private Config_KeepaLiveInterval As Integer = 1000
#End Region


#Region "Recv"
        '接收定时器的间隔：ms
        Private Config_TimerRecvInk As Integer = 10
#End Region

        'Dispose
        Private Sub Config_Dispose()
            Config_KeepaLiveTime = vbNull
            Config_KeepaLiveInterval = vbNull
            Config_TimerRecvInk = vbNull
        End Sub

    End Class
End Namespace

