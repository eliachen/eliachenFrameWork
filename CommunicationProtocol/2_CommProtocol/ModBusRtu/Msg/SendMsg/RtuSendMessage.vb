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

            ''' <summary>
            '''  'Function 3 - Read Registers
            ''' </summary>
            ''' <param name="_SlaveID">测站ID</param>
            ''' <param name="_StartRegIndex">开始寄存器编号</param>
            ''' <param name="_RegCount">寄存器个数</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Function SendFc3(_SlaveID As Byte, _StartRegIndex As UShort, _RegCount As UShort) As SendCommMessage
                'Function 3 request is always 8 bytes:
                Dim message As Byte() = New Byte(7) {}
                'Function 3 response buffer:
                Dim response As Byte() = New Byte(5 + (2 * _RegCount - 1)) {}
                'Build outgoing modbus message:
                BuildMessage(_SlaveID, CByte(3), _StartRegIndex, _RegCount)
                Return Me
            End Function
        End Class
    End Class
End Namespace


