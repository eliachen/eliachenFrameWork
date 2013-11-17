Imports EliachenFw.Data.DataType.EliaBinary
Imports EliachenFw.Data.DataType
Namespace Communication.Proj.Comm_IEC103.Code.AdvCode
    '高级信息编码
    Module EnCode
        '组织控制域
        Public Function Encode_Code(ByVal _Cs As CodeStM) As Byte

            'D7      D6    D5     D4      D3 D2 D1 D0
            '备用    PRM   FCB     FCV    FUNCTION CODE  
            '0或1     1                  功能码
            Dim BinaryCode As New EliaBinary(8)
            BinaryCode = ByteToBinay(_Cs.FunctionCode)
            BinaryCode.Position(4) = _Cs.Fcv
            BinaryCode.Position(5) = _Cs.Fcb
            BinaryCode.Position(6) = _Cs.Prm
            BinaryCode.Position(7) = _Cs.Backup
            Return BinayToByte(BinaryCode)
        End Function


        Public Function test() As List(Of Asdu_10_Inf)
            Dim l As New List(Of Asdu_10_Inf)
            For index = 0 To 9


                Dim a As New Asdu_10_Inf
                a.NGDSum = Rnd() * 10 + 1
                a.ListValue = New List(Of Asdu_10_Inf.Value)

                Dim b As Asdu_10_Inf.Value
                b.GinGroupIndex = Rnd() * 2 + 1
                b.ValArrByte = New Byte() {2, 3, 4, 2}
                a.ListValue.Add(b)

                l.Add(a)
            Next
            Return l


        End Function

        '组织ASDU
        Public Function Encode_ASDU(ByVal _AsduSt As ASDUSt) As Byte()
            Dim AsduList As New List(Of Byte)
            AsduList.Add(_AsduSt.Type)
            AsduList.Add(_AsduSt.Vsq)
            AsduList.Add(_AsduSt.Cot)
            AsduList.Add(_AsduSt.Comaddr)
            AsduList.Add(_AsduSt.Fun)
            AsduList.Add(_AsduSt.Inf)
            AsduList.AddRange(_AsduSt.InfData)
            Return AsduList.ToArray
        End Function
    End Module
End Namespace


'Namespace Code
'    Namespace AdvCode

'    End Namespace
'End Namespace