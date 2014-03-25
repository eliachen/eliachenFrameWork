Namespace Communication.Proj.ModBusSlave

    '接收结构体基本结构体
    Public Class MB_Recv
        '地址
        Public addr As Byte
        '功能号
        Public fun As Byte
        '接收的个数
        Public count As Byte
        '原始数据区
        Public ListData As New List(Of Byte)
        '校验方式
        Public CrcFormat As String = "LH"
    End Class

    '接收结构体基本结构体
    Public Class MB_Recv_Adv
        '基本结构
        Public _StBasic As MB_Recv
        '数据区
        Public ListValue As New List(Of Object)
    End Class

End Namespace

