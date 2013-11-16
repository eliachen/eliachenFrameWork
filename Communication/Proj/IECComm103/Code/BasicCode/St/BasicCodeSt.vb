Imports EliachenFw.Comm.Proj.Comm_IEC103.Code.AdvCode

Namespace Communication.Proj.Comm_IEC103.Code.BasicCode
    '基本的编码结构
    Public Module BasicCodeSt
        '固定帧的结构体
        Public Class Recv_StableFrame
            Implements Communication.CommInterface.ICommBufferElement

            '控制域
            Public oriCode As Byte
            '地址域
            Public Addr As Byte

            Public nCode As Comm_IEC103.Code.AdvCode.CodeStS

            '检查定长帧
            Public Function CheckBufferObj(ByVal ArrByte() As Byte) As Boolean Implements CommInterface.ICommBufferElement.CheckBufferElement
                Return Check_StableFrame(ArrByte)
            End Function

            '获取定长帧
            Public Function GetBufferObj(ByVal ArrByte() As Byte) As CommInterface.ICommBufferElement Implements CommInterface.ICommBufferElement.GetBufferElement
                Dim TmpCode As Byte = ArrByte(1)
                nCode = DeCode_CodeS(TmpCode)
                Addr = ArrByte(2)
                Return Me
            End Function
        End Class

        '非固定帧结构
        Public Class Recv_unStableFrame
            Implements Communication.CommInterface.ICommBufferElement

            '控制域
            Public oriCode As Byte
            '地址域
            Public Addr As Byte
            '链路用户数据
            Public oriASDU As Byte()

            'nASDU
            Public nASDU As ASDUSt
            'nCode
            Public nCode As CodeStS

            '检查不定长帧
            Public Function CheckBufferObj(ByVal ArrByte() As Byte) As Boolean Implements CommInterface.ICommBufferElement.CheckBufferElement
                Return Check_unStableFrame(ArrByte)
            End Function

            '返回不定长帧
            Public Function GetBufferObj(ByVal ArrByte() As Byte) As CommInterface.ICommBufferElement Implements CommInterface.ICommBufferElement.GetBufferElement
                '解码控制域
                Dim tmpCode As Byte = ArrByte(4)
                nCode = DeCode_CodeS(tmpCode)
                '获取地址
                Addr = ArrByte(5)

                '解码ASDU
                Dim tmpAsdu As Byte()
                ReDim tmpAsdu(ArrByte.Length - 9)
                Array.Copy(ArrByte, 6, tmpAsdu, 0, tmpAsdu.Length)
                nASDU = Decode_Asdu(tmpAsdu)
                Return Me
            End Function
        End Class
    End Module

End Namespace

