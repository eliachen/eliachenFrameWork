Namespace CommProtocol.ModBus
    Partial Public Class Rtu
        Inherits CommStation(Of RtuRecvMessage)


        Sub New()
            MyBase.New()
        End Sub

        '校验
        Public Overrides Function Validate(_ArrByte As Byte()) As Boolean
            If _ArrByte.Length < 3 Then
                Return False
            End If

            Dim CRC As Byte() = New Byte(1) {}
            Dim CheckArrByte As Byte()
            ReDim CheckArrByte(_ArrByte.Length - 2 - 1)
            Array.Copy(_ArrByte, CheckArrByte, CheckArrByte.Length)

            Common.MathHelper.CRC16(CheckArrByte, CRC(0), CRC(1))
            If CRC(0) = _ArrByte(_ArrByte.Length - 2) AndAlso CRC(1) = _ArrByte(_ArrByte.Length - 1) Then
                Return True
            Else
                Return False
            End If
        End Function
        '基本解码
        Public Overrides Function Decode(_ArrByte() As Byte) As RtuRecvMessage
            Return RtuRecvMessage.Decode(_ArrByte)
        End Function

    End Class
End Namespace

