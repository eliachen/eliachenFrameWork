Namespace CommProtocol.ModBus
    Partial Public Class Rtu
        Public Class RtuSendMessage
            Inherits SendCommMessage

            Sub New()
                MyBase.new()
                Me.MessageType = MessageTypeModel.Ack
            End Sub

            Private Sub BuildMessage(address As Byte, type As Byte, start As UShort, registers As UShort, message As Byte())
                message(0) = address
                message(1) = type
                message(2) = CByte(start >> 8)
                message(3) = CByte(start)
                message(4) = CByte(registers >> 8)
                message(5) = CByte(registers)

                '除去最后两位的校验
                Dim CRC As Byte() = New Byte(1) {}
                Common.MathHelper.CRC16(message.Take(message.Length - 2).ToArray(), CRC(0), CRC(1))
                message(message.Length - 2) = CRC(0)
                message(message.Length - 1) = CRC(1)

                Me.MessageBody = message
            End Sub


            ''' <summary>
            '''  'Function 1 - Read Registers
            ''' </summary>
            ''' <param name="_SlaveID">测站ID</param>
            ''' <param name="_StartRegIndex">开始寄存器编号</param>
            ''' <param name="_RegCount">寄存器个数</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Function SendFc01(_SlaveID As Byte, _StartRegIndex As UShort, _RegCount As UShort) As SendCommMessage
                'Function 1 request is always 8 bytes:
                Dim message As Byte() = New Byte(7) {}
                'Build outgoing modbus message:
                BuildMessage(_SlaveID, CByte(1), _StartRegIndex, _RegCount, message)
                Return Me
            End Function


            ''' <summary>
            '''  'Function 3 - Read Registers
            ''' </summary>
            ''' <param name="_SlaveID">测站ID</param>
            ''' <param name="_StartRegIndex">开始寄存器编号</param>
            ''' <param name="_RegCount">寄存器个数</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Function SendFc03(_SlaveID As Byte, _StartRegIndex As UShort, _RegCount As UShort) As SendCommMessage
                'Function 3 request is always 8 bytes:
                Dim message As Byte() = New Byte(7) {}
                'Build outgoing modbus message:
                BuildMessage(_SlaveID, CByte(3), _StartRegIndex, _RegCount, message)
                Return Me
            End Function


            ''' <summary>
            ''' Function 16 - Write Multiple Registers
            ''' </summary>
            ''' <param name="_SlaveID"></param>
            ''' <param name="_StartRegIndex"></param>
            ''' <param name="registers"></param>
            ''' <param name="values"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Function SendFc16(_SlaveID As Byte, _StartRegIndex As UShort, registers As UShort, values As Short()) As SendCommMessage

                'Message is 1 addr + 1 fcn + 2 start + 2 reg + 1 count + 2 * reg vals + 2 CRC
                Dim message As Byte() = New Byte(9 + (2 * registers - 1)) {}
                'Function 16 response is fixed at 8 bytes
                Dim response As Byte() = New Byte(7) {}

                'Add bytecount to message:
                message(6) = CByte(registers * 2)
                'Put write values into message prior to sending:
                For i As Integer = 0 To registers - 1
                    message(7 + 2 * i) = CByte(values(i) >> 8)
                    message(8 + 2 * i) = CByte(values(i))
                Next
                'Build outgoing message:
                BuildMessage(_SlaveID, CByte(16), _StartRegIndex, registers, message)
                Return Me
            End Function


      
         
        End Class
    End Class
End Namespace


