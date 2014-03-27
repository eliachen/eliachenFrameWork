Namespace CommProtocol.ModBus
    Partial Public Class Rtu
        Public Class RtuSendMessage
            Inherits SendCommMessage

            Sub New()
                MyBase.new()
                Me.MessageType = MessageTypeModel.Ack
            End Sub

            Private Sub BuildMessage(address As Byte, type As Byte, start As UShort, registers As UShort, message As Byte())
                'Array to receive CRC bytes:
                'Dim tmp As New RtuBasicMessage() With {.Address = address,
                '                                       .Function = type,
                '                                       .Data = New Byte() {CByte(start >> 8), CByte(start),
                '                                                           CByte(registers >> 8), CByte(registers)}}


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
#Region "Function16"
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

#End Region
           
#Region "Function3"
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
                'Build outgoing modbus message:
                BuildMessage(_SlaveID, CByte(3), _StartRegIndex, _RegCount, message)
                Return Me
            End Function

            '获取数据区数据
            Public Shared Function GetFun3_NumberList(Of T)(ByVal _RtuRecvMsg As RtuRecvMessage) As IList(Of T)
                '头两个展示长度,丢掉
                Dim AllArrByte As Byte() = New Byte(_RtuRecvMsg.RecvMsg.Data.Length - 1 - 1) {}
                Array.Copy(_RtuRecvMsg.RecvMsg.Data, 1, AllArrByte, 0, AllArrByte.Length)

                '寄存器交换
                For index = 0 To AllArrByte.Length - 1 Step 2
                    Array.Reverse(AllArrByte, index, 2)
                Next

                '计算数据类型长度
                Dim count As Integer = Function() As Integer
                                           Dim x = GetType(T).Name
                                           Select Case GetType(T).Name
                                               Case "Char"
                                                   Return 2
                                               Case "Int16"
                                                   Return 2
                                               Case "UInt16"
                                                   Return 2

                                               Case "Int32"
                                                   Return 4
                                               Case "UInt32"
                                                   Return 4
                                               Case "Single"
                                                   Return 4

                                               Case "Int64"
                                                   Return 8
                                               Case "UInt64"
                                                   Return 8
                                               Case "Double"
                                                   Return 8
                                               Case Else
                                                   Throw New Exception("无法转换的数据类型")
                                           End Select
                                       End Function.Invoke()


                Dim len As Integer = Function() As Integer
                                         If AllArrByte.Length < count Then
                                             Throw New Exception("寄存器长度不够")
                                         Else
                                             Return AllArrByte.Length - (AllArrByte.Length Mod count)
                                         End If
                                     End Function.Invoke()



                Dim tmpListRel As New List(Of Object)

                '临时数组
                Dim DataArrByte As Byte() = New Byte(count - 1) {}

                For index = 0 To len Step count
                    If index <> len Then
                        Array.Copy(AllArrByte, index, DataArrByte, 0, DataArrByte.Length)
                        tmpListRel.Add(GetNumberValue(GetType(T), DataArrByte))
                    End If
                Next

                '再强制类型转换
                Dim ExList As New List(Of T)
                For Each ea In tmpListRel
                    ExList.Add(DirectCast(ea, T))
                Next
                Return ExList
            End Function

            Private Shared Function GetNumberValue(ByVal Type As Type, ByVal ArrByte As Byte()) As Object
                Select Case Type.Name
                    Case "Char"
                        Return BitConverter.ToChar(ArrByte, 0)
                    Case "Int16"
                        Return BitConverter.ToInt16(ArrByte, 0)
                    Case "Int32"
                        Return BitConverter.ToInt32(ArrByte, 0)
                    Case "Int64"
                        Return BitConverter.ToInt64(ArrByte, 0)
                    Case "UInt16"
                        Return BitConverter.ToUInt16(ArrByte, 0)
                    Case "UInt32"
                        Return BitConverter.ToUInt32(ArrByte, 0)
                    Case "UInt64"
                        Return BitConverter.ToUInt64(ArrByte, 0)
                    Case "Single"
                        Return BitConverter.ToSingle(ArrByte, 0)
                    Case "Double"
                        Return BitConverter.ToDouble(ArrByte, 0)
                    Case Else
                        Return Nothing
                End Select

            End Function

            Public Shared Function GetFun3_BitList(ByVal _RtuRecvMsg As RtuRecvMessage) As List(Of List(Of Boolean))
                '头两个展示长度,丢掉
                Dim AllArrByte As Byte() = New Byte(_RtuRecvMsg.RecvMsg.Data.Length - 1 - 1) {}
                Array.Copy(_RtuRecvMsg.RecvMsg.Data, 1, AllArrByte, 0, AllArrByte.Length)

                Dim relList As New List(Of Tuple(Of BitArray, BitArray))

                '临时数组
                Dim DataArrByte As Byte() = New Byte(1) {}

                For index = 0 To AllArrByte.Length - 1 Step 2
                    If index <> AllArrByte.Length - 1 Then
                        Array.Copy(AllArrByte, index, DataArrByte, 0, DataArrByte.Length - 1)
                        relList.Add(Tuple.Create(Of BitArray, BitArray) _
                                    (New BitArray(New Byte() {DataArrByte(0)}), _
                                     New BitArray(New Byte() {DataArrByte(1)})))

                    End If
                Next

                Dim l As New List(Of List(Of Boolean))

                Dim ex = Function(bitArr As BitArray) As List(Of Boolean)
                             Dim tmplist As New List(Of Boolean)
                             For index = 0 To 7
                                 tmplist.Add(bitArr.Get(index))
                             Next
                             Return tmplist
                         End Function

                For Each ea In relList
                    Dim tmpList As New List(Of Boolean)
                    Dim i As New BitArray(New Byte())
                    tmpList.AddRange(ex(ea.Item1))
                    tmpList.AddRange(ex(ea.Item2))
                    l.Add(tmpList)
                Next
                Return l
            End Function
#End Region


        End Class
    End Class
End Namespace


