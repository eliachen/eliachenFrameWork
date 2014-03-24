Imports System.Runtime.InteropServices
Imports System.Net.Sockets

Namespace CommPort
    Partial Public Class TcpSever
#Region "New"
        '多长时间后开始第一次探测:ms
        Private Config_KeepaLiveTime As Integer = 5000
        '探测时间间隔:ms
        Private Config_KeepaLiveInterval As Integer = 1000
#End Region


#Region "Recv"
        '接收定时器的间隔：ms
        'Private Config_TimerRecvInk As Integer = 10
#End Region


        '设置Socket
        Private Sub Configure_eTcpSeverSocket(ByRef eTcpSocket As Socket)

            '设置Socket的属性,需要在一定时间内保活
            Dim dummy As UInteger = 0
            '设置属性
            Dim inOptionValues As Byte() = New Byte(Marshal.SizeOf(dummy) * 3 - 1) {}

            '启用Keep-Alive
            BitConverter.GetBytes(CUInt(1)).CopyTo(inOptionValues, 0)
            '第一次探测的时间：5000ms
            BitConverter.GetBytes(CUInt(Config_KeepaLiveTime)).CopyTo(inOptionValues, Marshal.SizeOf(dummy))
            '时间间隔:5000ms
            BitConverter.GetBytes(CUInt(Config_KeepaLiveInterval)).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2)

            Dim ouOptionValues As Byte() = New Byte(Marshal.SizeOf(dummy) * 3 - 1) {}

            eTcpSocket.IOControl(IOControlCode.KeepAliveValues, inOptionValues, ouOptionValues)


            ' True:不复用  False:复用
            eTcpSocket.ExclusiveAddressUse = True

            'The socket will linger for 10 seconds after 
            'Socket.Close is called.
            'eTcpSocket.LingerState = New LingerOption(True, 10)

            ' Disable the Nagle Algorithm for this tcp socket.
            eTcpSocket.NoDelay = True

            'Set the receive buffer size to 8k
            eTcpSocket.ReceiveBufferSize = 8192

            ' Set the timeout for synchronous receive methods to 
            ' 1 second (1000 milliseconds.)
            'eTcpSocket.ReceiveTimeout = 1000

            'Set the send buffer size to 8k.
            eTcpSocket.SendBufferSize = 8192

            'Set the timeout for synchronous send methods
            ' to 1 second (1000 milliseconds.)
            'eTcpSocket.SendTimeout = 1000

            'Set the Time To Live (TTL) to 42 router hops.
            'eTcpSocket.Ttl = 42


        End Sub

        'Dispose
        Private Sub Config_Dispose()
            Config_KeepaLiveTime = vbNull
            Config_KeepaLiveInterval = vbNull
            'Config_TimerRecvInk = vbNull
        End Sub

    End Class
End Namespace
'内部一些参数的设置

