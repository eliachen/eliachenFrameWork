﻿Namespace CommProtocol.ModBus
    Public Class RtuBasicMessage
        Property Address As Byte
        Property [Function] As Byte
        Property Data As Byte()
        Property Check As Byte()

        '编码
        Public Function Encode(ByVal _RtuMsg As RtuBasicMessage) As Byte()
            Dim message As New List(Of Byte)

            message.Add(_RtuMsg.Address)
            message.Add(_RtuMsg.Function)
            message.AddRange(_RtuMsg.Data)

            Dim CRC As Byte() = New Byte(1) {}
            Common.MathHelper.CRC16(message.ToArray(), CRC(0), CRC(1))

            message.AddRange(CRC)
            Return message.ToArray()
        End Function

        '解码
        Public Shared Function Decode(ByVal _ArrByte As Byte()) As RtuBasicMessage
            Try
                Dim rbm As New RtuBasicMessage
                rbm.Address = _ArrByte(0)
                rbm.Function = _ArrByte(1)

                ReDim rbm.Data(_ArrByte.Length - 1 - 1 - 2)
                Array.Copy(_ArrByte, 2, rbm.Data, 0, rbm.Data.Length - 1)

                ReDim rbm.Check(1)
                Array.Copy(_ArrByte, _ArrByte.Length - 2, rbm.Check, 0, rbm.Check.Length - 1)
                Return rbm
            Catch ex As Exception
                Return Nothing
            End Try
        End Function
    End Class
End Namespace

