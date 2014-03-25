Imports EliachenFw.Comm.Proj.Comm_IEC103.Code.AdvCode
Imports EliachenFw.Comm.Proj.Comm_IEC103.Code.BasicCode

Namespace Communication.Proj.Comm_IEC103.Code.Others
    Module CodeOthers
        '命令中改变fcb/////////////反编译改变命令中的FCB
        '通讯中的需求，每次的命令需要与上一次不同的fcb
        Public Function OrderExFcb(ByVal _ArrByte As Byte()) As Byte()

            If _ArrByte.Length = 5 Then
                Dim sf As Recv_StableFrame = DeCode_StableFrame(_ArrByte)
                Dim codem As CodeStM = DeCode_CodeM(sf.oriCode)

                '改变FCB
                exFcb(codem.Fcb)

                sf.oriCode = Encode_Code(codem)

                Return Encode_StableFrame(sf)
            Else
                Dim usf As Recv_unStableFrame = DeCode_unStableFrame(_ArrByte)
                Dim codeM As CodeStM = DeCode_CodeM(usf.oriCode)

                '改变FCB
                exFcb(codeM.Fcb)

                usf.oriCode = Encode_Code(codeM)
                Return Encode_unStableFrame(usf)
            End If
        End Function

        '改变fcb的值
        Public Function exFcb(ByRef _Fcb As Byte) As Byte
            If _Fcb = 1 Then
                _Fcb = 0
                Return _Fcb
            Else
                _Fcb = 1
                Return _Fcb
            End If
        End Function

    End Module
End Namespace

