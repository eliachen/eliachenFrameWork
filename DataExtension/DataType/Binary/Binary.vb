Namespace Data.DataType
    ''' <summary>
    ''' 声明几位2进制
    ''' </summary>
    ''' <remarks></remarks>
    Public Class EliaBinary

        '结构体中的位置,position(0)为最低位
        Public Position As Byte()


        Property MaxPos As Byte

        Property MinPos As Byte

        Property Length As Integer



        '结构体初始化,二进制的个数
        Sub New(ByVal count As Byte)
            ReDim Position(count - 1)
        End Sub

        Sub New()

        End Sub
        ''' <summary>
        ''' 获取关于2进制的数组,将一个byte转化为Binary
        ''' </summary>
        ''' <param name="_Byte"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ByteToBinay(ByVal _Byte As Byte) As EliaBinary
            Dim eb As New EliaBinary(8)
            Dim BinaryStr As Char()

            '转化为2进制的字符数组
            BinaryStr = Convert.ToString(_Byte, 2).Reverse.ToArray

            Dim rel = From s In BinaryStr
                         Let cb = Convert.ToByte(s, 10)
                                Select cb


            Dim exrel As Byte() = rel.ToArray

            Array.Copy(exrel.ToArray, eb.Position, exrel.ToArray.Length)
            Return eb
        End Function
        ''' <summary>
        ''' 将2进制转化成Byte
        ''' </summary>
        ''' <param name="_eb"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function BinayToByte(ByVal _eb As EliaBinary) As Byte
            Dim ByteStr As String = Nothing
            For i = 0 To 7
                ByteStr = _eb.Position(i).ToString + ByteStr
            Next
            Return Convert.ToByte(ByteStr, 2)
        End Function
        ''' <summary>
        ''' 读取从指定位置开始的2进制,并指定长度，最低为0位
        ''' </summary>
        ''' <param name="index"></param>
        ''' <param name="len"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function BinaryReadRange(ByVal _eb As EliaBinary, ByVal index As Byte, ByVal len As Byte) As EliaBinary
            Dim eb As New EliaBinary(8)
            Array.Copy(_eb.Position, index, eb.Position, index, len)
            Return eb
        End Function

        ''' <summary>
        '''  将一个二进制的字符串转换成对应的整型（非标准长度）
        ''' </summary>
        ''' <param name="_BinaryStr">二进制字符串</param>
        ''' <param name="_T">数据类型:有符号类型</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function BinaryStrToInt(ByVal _BinaryStr As String, ByVal _T As Type) As Object
            If _BinaryStr.Chars(0) = "0" Then '正数
                Select Case _T
                    Case GetType(Int16)
                        Return Convert.ToInt16(_BinaryStr, 2)
                    Case GetType(Int32)
                        Return Convert.ToInt32(_BinaryStr, 2)
                    Case GetType(Int64)
                        Return Convert.ToInt64(_BinaryStr, 2)
                End Select
            Else '负数
                Select Case _T
                    Case GetType(Int16)
                        Return Convert.ToInt16(New String("1", 16 - _BinaryStr.Length) & _BinaryStr, 2)
                    Case GetType(Int32)
                        Return Convert.ToInt32(New String("1", 32 - _BinaryStr.Length) & _BinaryStr, 2)
                    Case GetType(Int64)
                        Return Convert.ToInt64(New String("1", 64 - _BinaryStr.Length) & _BinaryStr, 2)
                End Select
            End If
        End Function

        ''' <summary>
        ''' 将字节数组转换为二进制字符串
        ''' (从低到高的转换，有补零)
        ''' </summary>
        ''' <param name="_ArrByte"></param>
        ''' <returns></returns>
        ''' <remarks>ss</remarks>
        Public Shared Function ArrByteToStr(ByVal _ArrByte As Byte()) As String
            Dim BinaryStr As String = ""
            For Each _each As Byte In _ArrByte
                Dim TempStr As String = Convert.ToString(_each, 2)
                Dim len As Byte = 8 - TempStr.Length
                If len <> 0 Then
                    Dim Zero As New String("0", len)
                    TempStr = Zero & TempStr
                End If
                BinaryStr = BinaryStr & TempStr
            Next

            Return BinaryStr
        End Function
    End Class
End Namespace

