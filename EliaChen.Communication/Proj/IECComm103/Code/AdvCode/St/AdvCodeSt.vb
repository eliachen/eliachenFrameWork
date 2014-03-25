Imports EliachenFw.Comm.Proj.Comm_IEC103.Code.BasicCode

Namespace Communication.Proj.Comm_IEC103.Code.AdvCode
    '高级编码信息
    Public Module AdvCodeSt
        ''' <summary>
        ''' 控制域结构,主->从
        ''' </summary>
        ''' <remarks></remarks>
        Structure CodeStM
            '备用 
            Public Backup As Byte
            Public Prm As Byte
            Public Fcb As Byte
            Public Fcv As Byte
            '功能码
            Public FunctionCode As Byte
        End Structure
        ''' <summary>
        ''' 控制域结构,从->主
        ''' </summary>
        ''' <remarks></remarks>
        Public Class CodeStS
            'Implements Comm.myComm.ICommBuffer

            'D7     D6   D5  D4   D3 D2 D1 D0 
            '备用  PRM  ACD DFC FUNCTION CODE 
            '备用 
            Public Backup As Byte
            Public Prm As Byte
            Public ACD As Byte
            Public DFC As Byte
            '功能码
            Public FunctionCode As Byte

            'Public Function CheckBufferObj(ByVal ArrByte() As Byte) As Boolean Implements myComm.ICommBuffer.CheckBufferObj

            'End Function

            'Public Function GetBufferObj(ByVal ArrByte() As Byte) As myComm.ICommBuffer Implements myComm.ICommBuffer.GetBufferObj
            '    Dim tmps As Recv_StableFrame = DeCode_StableFrame(ArrByte)
            'End Function
        End Class
        ''' <summary>
        ''' ASDU结构体
        ''' </summary>
        ''' <remarks></remarks>
        Public Class ASDUSt
            'Implements Comm.myComm.ICommBuffer
            '控制域
            Public Code As CodeStS
            'ASDU类别标识
            Public Type As Byte
            '可变结构限定词
            Public Vsq As Byte
            '传输原因
            Public Cot As Byte
            '数据单元公共地址 
            Public Comaddr As Byte
            '功能类型 
            Public Fun As Byte
            '信息序号 
            Public Inf As Byte
            '信息元素
            Public InfData As Byte()

            'Public Function CheckBufferObj(ByVal ArrByte() As Byte) As Boolean Implements myComm.ICommBuffer.CheckBufferObj

            'End Function

            'Public Function GetBufferObj(ByVal ArrByte() As Byte) As myComm.ICommBuffer Implements myComm.ICommBuffer.GetBufferObj

            'End Function
        End Class

    End Module
End Namespace


