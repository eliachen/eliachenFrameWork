Namespace CommProtocol.ModBus
    Partial Public Class Rtu
        Public Class RtuRecvMessage
            Inherits RecvCommMessage

            Public Property RecvMsg As RtuBasicMessage

            Sub New()
                MyBase.new()
                Me.MessageType = MessageTypeModel.Cache_noAck
            End Sub

            Public Overrides Property MessageResponse As Byte()

            Public Shared Function Decode(ByVal _ArrByte As Byte()) As RecvCommMessage
                Return New RtuRecvMessage With {.RecvMsg = RtuBasicMessage.Decode(_ArrByte)}
            End Function
        End Class
    End Class
End Namespace

