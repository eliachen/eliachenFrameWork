Namespace CommProtocol
    Public MustInherit Class RecvCommMessage
        '默认带缓存
        Public Property MessageType As MessageTypeModel = MessageTypeModel.Cache_noAck
        '默认回应
        Public MustOverride Property MessageResponse As Byte()



        Public Enum MessageTypeModel
            '缓存,回复
            Cache_Ack
            '缓存,不回复
            Cache_noAck

            '不缓存,回复
            noCache_Ack
            '不缓存,不回复
            noCache_noAck
        End Enum
    End Class
End Namespace

