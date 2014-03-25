﻿Namespace Communication
    Module CommonMethod

        ''' <summary>
        ''' CRC16校验编码
        ''' </summary>
        ''' <param name="data"></param>
        ''' <param name="ReturnDatalo"></param>
        ''' <param name="ReturnDatahi"></param>
        ''' <remarks></remarks>
        Public Function CRC16(ByVal data() As Byte, ByRef ReturnDatalo As Byte, ByRef ReturnDatahi As Byte) As Boolean
            Try
                Dim CRC16Lo As Byte, CRC16Hi As Byte      'CRC寄存器
                Dim CL As Byte, CH As Byte                '多项式码&HA001
                Dim SaveHi As Byte, SaveLo As Byte
                Dim Flag As Integer
                CRC16Lo = &HFF
                CRC16Hi = &HFF
                CL = &H1
                CH = &HA0
                For i = 0 To UBound(data)
                    CRC16Lo = CRC16Lo Xor data(i) '每一个数据与CRC寄存器进行异或
                    For Flag = 0 To 7
                        SaveHi = CRC16Hi
                        SaveLo = CRC16Lo
                        CRC16Hi = CRC16Hi \ 2            '高位右移一位
                        CRC16Lo = CRC16Lo \ 2            '低位右移一位
                        If ((SaveHi And &H1) = &H1) Then '如果高位字节最后一位为1
                            CRC16Lo = CRC16Lo Or &H80      '则低位字节右移后前面补1
                        End If                           '否则自动补0
                        If ((SaveLo And &H1) = &H1) Then '如果LSB为1，则与多项式码进行异或
                            CRC16Hi = CRC16Hi Xor CH
                            CRC16Lo = CRC16Lo Xor CL
                        End If
                    Next Flag
                Next i
                ReturnDatahi = CRC16Hi              'CRC高位
                ReturnDatalo = CRC16Lo              'CRC低位
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function


    End Module
End Namespace

