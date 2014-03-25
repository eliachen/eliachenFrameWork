
Namespace Communication.Proj.Comm_IEC103.Code.BasicCode
    Module DeCode

#Region "固定帧"
        ''' <summary>
        ''' 检测是否符合要求的固定帧
        ''' </summary>
        ''' <param name="_ByteArr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Check_StableFrame(ByVal _ByteArr As Byte()) As Boolean
            Try
                Dim BoolCheck As Boolean = False
                If _ByteArr.Length > 4 Then
                    '获取头部
                    Dim StartStr As Byte = _ByteArr(0)
                    '获取校验和
                    Dim Cs0 As Byte = _ByteArr(1) + _ByteArr(2)
                    '收到的校验和
                    Dim Cs1 As Byte = _ByteArr(3)
                    '获取尾部
                    Dim EndStr As Byte = _ByteArr(4)
                    '校验成功
                    If StartStr = &H10 AndAlso _
                       Cs0 = Cs1 AndAlso _
                       EndStr AndAlso _
                       EndStr = &H16 _
                                            Then

                        BoolCheck = True
                    End If
                End If
                Return BoolCheck
            Catch ex As Exception
                Return False
            End Try

        End Function
        ''' <summary>
        ''' 对于固定帧的解码
        ''' </summary>
        ''' <param name="_ByteArr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeCode_StableFrame(ByVal _ByteArr As Byte()) As Recv_StableFrame
            Dim sf As New Recv_StableFrame
            sf.oriCode = _ByteArr(1)
            'sf.Code = DeCode_Code(_ByteArr(1))
            sf.Addr = _ByteArr(2)
            Return sf
        End Function
#End Region

#Region "不定帧"
        ''' <summary>
        ''' 检测不定帧结构是否正确
        ''' </summary>
        ''' <param name="_ByteArr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Check_unStableFrame(ByVal _ByteArr As Byte()) As Boolean
            Try
                Dim BoolCheck As Boolean = False

                '获取头部1
                Dim StartStr1 As Byte = _ByteArr(0)
                '获取长度1
                Dim Length1 As Byte = _ByteArr(1)
                '获取长度2
                Dim Length2 As Byte = _ByteArr(2)
                '获取头部2
                Dim StartStr2 As Byte = _ByteArr(3)
                '控制域
                Dim Code As Byte = _ByteArr(4)
                '地址域
                Dim Addr As Byte = _ByteArr(5)
                'ASDU
                Dim ASDU As Byte()
                ReDim ASDU(_ByteArr.Length - 9)
                Array.Copy(_ByteArr, 6, ASDU, 0, ASDU.Length)

                '获取校验和
                Dim Cs0 As Byte
                Dim sum As Integer = 0

                For Each s As Byte In ASDU
                    sum = sum + s
                Next

                Cs0 = (Code + Addr + sum) Mod 256

                '收到的校验和
                Dim Cs1 As Byte = _ByteArr(_ByteArr.Length - 2)
                '结束
                Dim EndStr As Byte = _ByteArr(_ByteArr.Length - 1)

                If StartStr1 = &H68 AndAlso _
                   StartStr2 = &H68 AndAlso _
                   Length1 = Length2 AndAlso _
                   _ByteArr.Length = Length1 + 6 AndAlso _
                   Cs1 = Cs0 AndAlso _
                    EndStr = &H16 Then
                    BoolCheck = True
                End If

                Return BoolCheck
            Catch ex As Exception
                Return False
            End Try

        End Function
        ''' <summary>
        ''' 对于不定帧解码的结果
        ''' </summary>
        ''' <param name="_ByteArr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeCode_unStableFrame(ByVal _ByteArr As Byte()) As Recv_unStableFrame
            Dim usf As New Recv_unStableFrame
            usf.oriCode = _ByteArr(4)
            'usf.Code = DeCode_Code(_ByteArr(4))
            usf.Addr = _ByteArr(5)

            ReDim usf.oriASDU(_ByteArr.Length - 9)
            Array.Copy(_ByteArr, 6, usf.oriASDU, 0, usf.oriASDU.Length)
            Return usf
        End Function
#End Region

    End Module
End Namespace


