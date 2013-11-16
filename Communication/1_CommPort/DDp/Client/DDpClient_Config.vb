Namespace Communication.CommPort.Port
    Partial Public Class DDpClient
        Private Class CpWay
            Inherits CommPort.ComPortWay.Model_Straight

            Sub New()
                '发送不延迟
                Me.TimeDelaySend = 0
                '数据报模式，无需停等
                Me.TimeOutRecv = 0
            End Sub

        End Class
    End Class
End Namespace

