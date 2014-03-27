Namespace CommProtocol.Common
    Public Class DataHelper

        Public Shared Function CheckTwoByte(_d1 As Byte(), _d2 As Byte()) As Boolean
            If _d1.Length = _d2.Length Then
                For index = 0 To _d1.Length - 1
                    If _d1(index) = _d2(index) Then
                        Continue For
                    Else
                        Return False
                    End If
                Next
            Else
                Return False
            End If
            Return True
        End Function
        '获取数据区数据
        Public Shared Function GetNumberList(Of T)(ByVal _RtuRecvMsg As Byte()) As IList(Of T)
            '头两个展示长度,丢掉
            Dim AllArrByte As Byte() = New Byte(_RtuRecvMsg.Length - 1 - 1) {}
            Array.Copy(_RtuRecvMsg, 1, AllArrByte, 0, AllArrByte.Length)

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

        Public Shared Function GetBitList(ByVal _RtuRecvMsg As Byte()) As List(Of List(Of Boolean))
            '头两个展示长度,丢掉
            Dim AllArrByte As Byte() = New Byte(_RtuRecvMsg.Length - 1 - 1) {}
            Array.Copy(_RtuRecvMsg, 1, AllArrByte, 0, AllArrByte.Length)

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
    End Class
End Namespace

