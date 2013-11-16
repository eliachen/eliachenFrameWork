Imports EliachenFw.Data.DataType.EliaBinary
Imports EliachenFw.Data.DataType
Namespace Communication.Proj.Comm_IEC103.Code.AdvCode
    Module DeCode
        '解码控制域Code主，没意义
        Public Function DeCode_CodeM(ByVal _Byte As Byte) As CodeStM
            Dim Cs As New CodeStM
            Dim CsBin As New EliaBinary
            CsBin = ByteToBinay(_Byte)
            'D7      D6    D5     D4      D3 D2 D1 D0
            '备用    PRM   FCB     FCV    FUNCTION CODE  
            '0或1     1                  功能码
            Cs.FunctionCode = BinayToByte(BinaryReadRange(CsBin, 0, 4))
            Cs.Fcv = CsBin.Position(4)
            Cs.Fcb = CsBin.Position(5)
            Cs.Prm = CsBin.Position(6)
            Cs.Backup = CsBin.Position(7)
            Return Cs
        End Function

        '解码控制域CodeS，从
        Public Function DeCode_CodeS(ByVal _Byte As Byte) As CodeStS
            'D7     D6   D5  D4   D3 D2 D1 D0 
            '备用  PRM  ACD DFC FUNCTION CODE 
            Dim Cs As New CodeStS
            Dim CsBin As New EliaBinary
            CsBin = ByteToBinay(_Byte)
            Cs.FunctionCode = BinayToByte(BinaryReadRange(CsBin, 0, 4))
            Cs.DFC = CsBin.Position(4)
            Cs.ACD = CsBin.Position(5)
            Cs.Prm = CsBin.Position(6)
            Cs.Backup = CsBin.Position(7)
            Return Cs
        End Function

        '解码ASDU
        Public Function Decode_Asdu(ByVal _Byte As Byte()) As ASDUSt
            '                TYPE 	ASDU类别标识 （1byte）
            'S 	VSQ （i）	可变结构限定词 
            '                COT(传输原因)
            '                COMADDR(数据单元公共地址)
            '                FUN(功能类型)
            '                INF(信息序号)
            '                信息元素数据(信息元素)
            '     …………… 信息元素 
            Dim AsduSt As New ASDUSt
            AsduSt.Type = _Byte(0)
            AsduSt.Vsq = _Byte(1)
            AsduSt.Cot = _Byte(2)
            AsduSt.Comaddr = _Byte(3)
            AsduSt.Fun = _Byte(4)
            AsduSt.Inf = _Byte(5)
            ReDim AsduSt.InfData(_Byte.Length - 6 - 1)
            Array.Copy(_Byte, 6, AsduSt.InfData, 0, AsduSt.InfData.Length)
            Return AsduSt
        End Function
    End Module
End Namespace

