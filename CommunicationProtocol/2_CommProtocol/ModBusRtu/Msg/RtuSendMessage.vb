Namespace CommProtocol.ModBus
    Partial Public Class Rtu
        Public Class RtuSendMessage
            Inherits SendCommMessage

            Sub New()
                MyBase.new()
                Me.MessageType = MessageTypeModel.Ack
            End Sub

            Private Sub BuildMessage(address As Byte, type As Byte, start As UShort, registers As UShort)
                'Array to receive CRC bytes:
                Dim message As New List(Of Byte)
                message.Add(address)
                message.Add(type)
                message.Add(CByte(start >> 8))
                message.Add(CByte(start))
                message.Add(CByte(registers >> 8))
                message.Add(CByte(registers))

                Dim CRC As Byte() = New Byte(1) {}
                Common.MathHelper.CRC16(message.ToArray(), CRC(0), CRC(1))
                message.AddRange(CRC)

                Me.MessageBody = message.ToArray
            End Sub

            'Function 3 - Read Registers
            Public Function SendFc3(address As Byte, start As UShort, registers As UShort) As SendCommMessage
                'Function 3 request is always 8 bytes:
                Dim message As Byte() = New Byte(7) {}
                'Function 3 response buffer:
                Dim response As Byte() = New Byte(5 + (2 * registers - 1)) {}
                'Build outgoing modbus message:
                BuildMessage(address, CByte(3), start, registers)
                Return Me
            End Function
        End Class
    End Class
End Namespace


