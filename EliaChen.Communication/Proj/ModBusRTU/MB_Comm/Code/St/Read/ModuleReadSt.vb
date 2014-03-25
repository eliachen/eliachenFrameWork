Namespace Communication.Proj.ModBusSlave

    '发送结构体
    Public Class MB_Read
        '地址
        Public addr As Byte
        '功能号
        Public fun As Byte
        '发送起始
        Public start As UInt16
        '读取的个数
        Public count As UInt16
        '最后一位的CRC校验位置 HL,LH
        Public CrcFormat As String = "LH"

        '编码:请求命令
        Public Shared Function Encode_Read(ByVal _StRead As MB_Read) As Byte()

            Dim ListSendData As New List(Of Byte)

            ListSendData.Add(_StRead.addr)
            ListSendData.Add(_StRead.fun)

            '寄存器开始位置
            ListSendData.AddRange(BitConverter.GetBytes(_StRead.start).Reverse)

            '寄存器结束位置
            ListSendData.AddRange(BitConverter.GetBytes(_StRead.count).Reverse)

            'CRC进行校验
            Dim CRChi As Byte
            Dim CRClow As Byte
            CRC16(ListSendData.ToArray, CRClow, CRChi)

            If _StRead.CrcFormat = "HL" Then
                ListSendData.Add(CRChi)
                ListSendData.Add(CRClow)
            ElseIf _StRead.CrcFormat = "LH" Then
                ListSendData.Add(CRClow)
                ListSendData.Add(CRChi)
            End If

            Return ListSendData.ToArray
        End Function





        '解码:请求命令
        Public Shared Function Decode_Read(ByVal _Arrbyte As Byte(), ByVal CrcFrt As String) As MB_Read
            Dim Mb_R As New MB_Read
            Mb_R.addr = _Arrbyte(0)
            Mb_R.fun = _Arrbyte(1)

            '校验模式处理
            If CrcFrt = "LH" Then
                '需要反转
                Array.Reverse(_Arrbyte, 2, 2)
                Array.Reverse(_Arrbyte, 4, 2)
            ElseIf CrcFrt = "HL" Then

            End If

            '开始位置
            Mb_R.start = BitConverter.ToUInt16(_Arrbyte, 2)
            '读取的个数
            Mb_R.count = BitConverter.ToUInt16(_Arrbyte, 4)

            Return Mb_R
        End Function
    End Class
End Namespace

