Namespace CommProtocol
    Public MustInherit Class SendCommMessage

        '默认带回复
        Public Property MessageType As MessageTypeModel = MessageTypeModel.Ack
        '默认带
        Public Property MessageBody As Byte()

        Public Enum MessageTypeModel
            '需等回复
            Ack
            '不等回复
            noAck
        End Enum
    End Class
End Namespace

