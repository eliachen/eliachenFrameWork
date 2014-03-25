Namespace Communication.Proj.ModBusSlave
    Partial Public Class MB_Recv
        '检测一帧数据的可靠性
        Shared Function CheckData(ByVal Arrbyte As Byte(), ByVal _CrcFormat As String) As Boolean
            Dim tmphi As Byte
            Dim tmplow As Byte

            '获取数据区部分
            Dim DataArr As Byte()
            ReDim DataArr(Arrbyte.Length - 3)
            Array.Copy(Arrbyte, DataArr, Arrbyte.Length - 2)

            CRC16(DataArr, tmplow, tmphi)

            '校验模式不同
            If _CrcFormat = "HL" Then

                If tmplow = Arrbyte(Arrbyte.Length - 1) And tmphi = Arrbyte(Arrbyte.Length - 2) Then
                    Return True
                Else
                    Return False
                End If
            ElseIf _CrcFormat = "LH" Then
                If tmphi = Arrbyte(Arrbyte.Length - 1) And tmplow = Arrbyte(Arrbyte.Length - 2) Then
                    Return True
                Else
                    Return False
                End If

            End If
        End Function


#Region "编码"
        '编码原始数据区(byte())
        Shared Function EncodeRecvBasic(ByVal _StBasRecv As MB_Recv) As Byte()
            Dim ListData As New List(Of Byte)
            '地址
            ListData.Add(_StBasRecv.addr)
            '功能号
            ListData.Add(_StBasRecv.fun)
            '统计数
            ListData.Add(_StBasRecv.count)
            '数据区
            ListData.AddRange(_StBasRecv.ListData)
            'CRC校验
            Dim hi As Byte
            Dim lo As Byte
            CRC16(ListData.ToArray, lo, hi)

            If _StBasRecv.CrcFormat = "LH" Then
                ListData.Add(lo)
                ListData.Add(hi)
            ElseIf _StBasRecv.CrcFormat = "HL" Then
                ListData.Add(hi)
                ListData.Add(lo)
            End If
            Return ListData.ToArray
        End Function

        '将编码数据进行存储
        Shared Function EncodeRecvBasic(ByVal _StAdvRecv As MB_Recv_Adv) As MB_Recv
            Dim tmpStBas As New MB_Recv
            tmpStBas = _StAdvRecv._StBasic

            '先编码出结果
            For Each _edata In _StAdvRecv.ListValue
                tmpStBas.ListData.AddRange(BitConverter.GetBytes(_edata))
            Next

            '再高低位置
            For index = 0 To tmpStBas.ListData.Count - 1 Step 2
                tmpStBas.ListData.Reverse(index, 2)
            Next
            Return tmpStBas
        End Function


#End Region

#Region "解码原始数据"
        '解码原始数据区(byte())
        Shared Function DecodeRecvBasic(ByVal Arrbyte As Byte()) As MB_Recv
            Dim tmpst As MB_Recv = Nothing
            tmpst.addr = Arrbyte(0)
            tmpst.fun = Arrbyte(1)
            tmpst.count = Arrbyte(2)

            '获取数据区的内容
            For index = 1 To Arrbyte.Length - 5 Step 1
                tmpst.ListData.Add(Arrbyte(index + 2))
            Next
            Return tmpst
        End Function

        ''' <summary>
        ''' 用于还原数据区
        ''' </summary>
        ''' <param name="StBas">基本数据结构</param>
        ''' <param name="NumCount">单个数据长度</param>
        ''' <param name="Type"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function DecodeRecvAdv(ByVal StBas As MB_Recv, ByVal NumCount As Byte, ByVal Type As Type) As MB_Recv_Adv
            Dim tmpst As New MB_Recv_Adv
            tmpst._StBasic = StBas

            '进行两两反转
            For index = 0 To StBas.ListData.Count - 1 Step 2
                StBas.ListData.Reverse(index, 2)
            Next

            '临时数组
            Dim DataArrByte As Byte() = New Byte(NumCount - 1) {}
            '循环获取数据
            For index = 0 To StBas.ListData.Count - 1 Step NumCount
                StBas.ListData.CopyTo(index, DataArrByte, 0, NumCount)
                tmpst.ListValue.Add(GetValue(Type, DataArrByte))
            Next
            Return tmpst
        End Function
        '解码
        Private Shared Function GetValue(ByVal Type As Type, ByVal ArrByte As Byte()) As Object
            Select Case Type.Name
                Case "Char"
                    Return BitConverter.ToChar(ArrByte, 0)
                Case "Int16"
                    Return BitConverter.ToInt16(ArrByte, 0)
                Case "Int32"
                    Return BitConverter.ToInt32(ArrByte, 0)
                Case "Int64"
                    Return BitConverter.ToInt64(ArrByte, 0)
                Case "UInt16"
                    Return BitConverter.ToUInt16(ArrByte, 0)
                Case "UInt32"
                    Return BitConverter.ToUInt32(ArrByte, 0)
                Case "UInt64"
                    Return BitConverter.ToUInt64(ArrByte, 0)
                Case "Single"
                    Return BitConverter.ToSingle(ArrByte, 0)
                Case "Double"
                    Return BitConverter.ToDouble(ArrByte, 0)
                Case Else
                    Return Nothing
            End Select
        End Function

#End Region

    End Class
End Namespace

