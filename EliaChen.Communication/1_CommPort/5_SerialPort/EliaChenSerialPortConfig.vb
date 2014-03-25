Namespace Comm.CommPort.Port
    Partial Public Class EliaChenSerialPort
        Public Class EliaChenSerialPortConfig
            '端口
            Public eCom As String = "COM1"
            '波特率
            Public eBaud As Integer = 9600
            '数据位
            Public eDataBits As Integer = 8
            '校验位
            Public eParity As IO.Ports.Parity = IO.Ports.Parity.None
            '停止位
            Public eStopBits As IO.Ports.StopBits = IO.Ports.StopBits.One
        End Class
    End Class
End Namespace

