Imports System.Text
Imports EliachenFw.Data.DataType.EliaBinary
Imports EliachenFw.Data.DataType
Namespace Communication.Proj.Comm_IEC103.Code.AdvCode

    '公用，解码时间
    '解码asdu中信息元素
    Module DeCode_AsduInf

        '获取原始数据
        Private Function GetHexStr(ByVal _ArrByte As Byte()) As String
            Dim str As String = ""
            For Each s As Byte In _ArrByte
                Dim TempStr As String = Hex(s)
                If TempStr.Length = 1 Then
                    TempStr = "0" & TempStr
                End If
                str = str & TempStr
            Next
            Return str
        End Function

        '公用，解码时间
        Private Function GetTime(ByVal _ArrByte As Byte()) As Date

            '解码时间
            Dim MsLow As Byte = _ArrByte(0)
            Dim Mshig As Byte = _ArrByte(1)
            Dim Min As Byte = _ArrByte(2)
            Dim Hour As Byte = _ArrByte(3)


            Dim Ms As Integer = Mshig * 256 + MsLow

            If _ArrByte.Length = 4 Then
                '时间战时只精确到秒
                Return New Date(Now.Year, Now.Month, Now.Day, Hour, Min, CInt(Ms / 1000), 0)
            ElseIf _ArrByte.Length = 7 Then
                Dim Day As Byte = _ArrByte(4)
                Dim Month As Byte = _ArrByte(5)

                Dim NowYearStr As String = Now.Year.ToString
                If NowYearStr.Length > 2 Then
                    NowYearStr = Left(NowYearStr, NowYearStr.Length - 2)
                End If
                Dim Year As Integer = CInt(NowYearStr & _ArrByte(6).ToString)
                'CInt(Ms / 1000)
                Return New Date(Year, CInt(Month), CInt(Day), CInt(Hour), CInt(Min), CInt(Ms / 1000))
            End If

        End Function

        ''' <summary>
        ''' 解码asdu5,响应复位通讯
        ''' </summary>
        ''' <param name="_Asdu"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeCode_Asdu_5(ByVal _Asdu As ASDUSt) As Asdu_5_Inf
            Dim Asdu5 As New Asdu_5_Inf

            Dim AsciiArrByte As Byte()
            ReDim AsciiArrByte(7)
            Array.Copy(_Asdu.InfData, 1, AsciiArrByte, 0, AsciiArrByte.Length)

            Asdu5.AscIIStr = Encoding.ASCII.GetString(AsciiArrByte)

            Asdu5.Edition = Hex(_Asdu.InfData(10)) & "." & Hex(_Asdu.InfData(9))
            Return Asdu5
        End Function

        ''' <summary>
        ''' 解码asdu1（01H）,遥信信息
        ''' </summary>
        ''' <param name="_Asdu"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeCode_Asdu_1(ByVal _Asdu As ASDUSt) As Asdu_1_Inf
            Dim Asdu1 As New Asdu_1_Inf

            'DPI信息解码
            Dim DPI As New EliaBinary(8)
            DPI = ByteToBinay(_Asdu.InfData(0))
            Asdu1.IV = DPI.Position(7)
            Asdu1.NT = DPI.Position(6)
            Asdu1.BL = DPI.Position(5)
            Asdu1.SB = DPI.Position(4)
            Asdu1.FarCommState = DPI.Position(0) * 1 + DPI.Position(1) * 2


            '解码时间
            Dim MsLow As Byte = _Asdu.InfData(1)
            Dim Mshig As Byte = _Asdu.InfData(2)
            Dim Min As Byte = _Asdu.InfData(3)
            Dim Hour As Byte = _Asdu.InfData(4)

            Dim MsStrHex As String = "&H" & Hex(Mshig) & Hex(MsLow)
            Dim Ms As Integer = Val(MsStrHex)
            '时间战时只精确到秒
            Asdu1.Time = New Date(Now.Year, Now.Month, Now.Day, Hour, Min, CInt(Min / 1000), 0)
            '附加位
            Asdu1.SIN = _Asdu.InfData(5)
            Return Asdu1
        End Function
        ''' <summary>
        ''' 解码asdu1（02H）,事故信号
        ''' </summary>
        ''' <param name="_Asdu"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeCode_Asdu_2(ByVal _Asdu As ASDUSt) As Asdu_2_Inf
            Dim Asdu2 As New Asdu_2_Inf

            'DPI信息解码
            Dim DPI As New EliaBinary(8)
            DPI = ByteToBinay(_Asdu.InfData(0))
            Asdu2.IV = DPI.Position(7)
            Asdu2.NT = DPI.Position(6)
            Asdu2.BL = DPI.Position(5)
            Asdu2.SB = DPI.Position(4)
            Asdu2.FarCommState = DPI.Position(0) * 1 + DPI.Position(1) * 2

            '相对时间
            Asdu2.Time1 = BitConverter.ToInt16(New Byte() {_Asdu.InfData(1), _Asdu.InfData(2)}, 0)
            '故障号
            Asdu2.FAN = BitConverter.ToInt16(New Byte() {_Asdu.InfData(3), _Asdu.InfData(4)}, 0)
            '解码时间
            Dim MsLow As Byte = _Asdu.InfData(5)
            Dim Mshig As Byte = _Asdu.InfData(6)
            Dim Min As Byte = _Asdu.InfData(7)
            Dim Hour As Byte = _Asdu.InfData(8)

            Dim MsStrHex As String = "&H" & Hex(Mshig) & Hex(MsLow)
            Dim Ms As Integer = Val(MsStrHex)
            '时间战时只精确到秒
            Asdu2.Time = New Date(Now.Year, Now.Month, Now.Day, Hour, Min, CInt(Min / 1000), 0)
            '附加位
            Asdu2.SIN = _Asdu.InfData(9)
            Return Asdu2
        End Function

        ''' <summary>
        ''' 解码asdu23(17H),扰动表
        ''' </summary>
        ''' <param name="_Asdu"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeCode_Asdu_23(ByVal _Asdu As ASDUSt) As List(Of Asdu_23_Inf)
            Dim Asdu23 As New Asdu_23_Inf

            '存储信息体的list
            Dim ListAsdu23 As New List(Of Asdu_23_Inf)

            '信息体的个数
            Dim InfLen As Integer = _Asdu.InfData.Length / 10

            For i = 1 To InfLen
                '索引
                Dim index As Integer = (i - 1) * 10
                '故障序号FAN
                'Dim low As Byte = _Asdu.InfData(0 + index)
                'Dim hig As Byte = _Asdu.InfData(1 + index)
                'Asdu23.FAN = low + 256 * hig
                Asdu23.FAN = BitConverter.ToInt16(New Byte() {_Asdu.InfData(0 + index), _Asdu.InfData(1 + index)}, 0)
                '故障状态SOF
                Asdu23.SOF = _Asdu.InfData(2 + index)
                Dim SofBin As EliaBinary = ByteToBinay(Asdu23.SOF)
                Asdu23.TP = SofBin.Position(0)
                Asdu23.TM = SofBin.Position(1)
                Asdu23.TEST = SofBin.Position(2)
                Asdu23.OTEV = SofBin.Position(3)
                '解码时间
                Dim TimeArrByte(6) As Byte
                Array.Copy(_Asdu.InfData, 3 + index, TimeArrByte, 0, 7)
                Asdu23.Time = GetTime(TimeArrByte)
                ListAsdu23.Add(Asdu23)
            Next
            Return ListAsdu23
        End Function

        ''' <summary>
        ''' 解码ASDU28（1CH） 带标志的状态变位传输准备就绪
        ''' </summary>
        ''' <param name="_Asdu"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeCode_Asdu_28(ByVal _Asdu As ASDUSt) As Asdu_28_Inf
            Dim Asdu28 As New Asdu_28_Inf

            '故障序号FAN
            'Dim low As String = Convert.ToString(_Asdu.InfData(3))
            'Dim hig As String = Convert.ToString(_Asdu.InfData(4))
            'Asdu28.FAN = Val("&H" & hig & low)
            Asdu28.FAN = BitConverter.ToInt16(New Byte() {_Asdu.InfData(3), _Asdu.InfData(4)}, 0)

            Return Asdu28
        End Function

        ''' <summary>
        ''' 解码ASDU29（1DH） 带标志的状态变位传输准备就绪
        ''' </summary>
        ''' <param name="_Asdu"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeCode_Asdu_29(ByVal _Asdu As ASDUSt) As List(Of Asdu_29_Inf)
            Dim Asdu29 As New Asdu_29_Inf

            '存储信息体的list
            Dim ListAsdu29 As New List(Of Asdu_29_Inf)

            '故障序号FAN
            'Dim low As String = Convert.ToString(_Asdu.InfData(0))
            'Dim hig As String = Convert.ToString(_Asdu.InfData(1))
            'Asdu29.FAN = Val("&H" & hig & low)
            Asdu29.FAN = BitConverter.ToInt16(New Byte() {_Asdu.InfData(0), _Asdu.InfData(1)}, 0)

            '故障数目
            Asdu29.NOT0 = _Asdu.InfData(2)
            'TAP
            ReDim Asdu29.TAP(1)
            Asdu29.TAP(0) = _Asdu.InfData(3)
            Asdu29.TAP(1) = _Asdu.InfData(4)

            Asdu29.TAPStr = GetHexStr(Asdu29.TAP)
            For i = 1 To Asdu29.NOT0
                '索引
                Dim index As Integer = (i - 1) * 3
                '功能类型FUN
                Asdu29.FUN = _Asdu.InfData(5 + index)
                '信息序号
                Asdu29.INF = _Asdu.InfData(6 + index)
                '遥信值
                Asdu29.DPI = _Asdu.InfData(7 + index)

                ListAsdu29.Add(Asdu29)
            Next
            Return ListAsdu29
        End Function

        ''' <summary>
        ''' 解码ASDU27（1BH） 被记录的通道传输准备就绪,获取额定值
        ''' </summary>
        ''' <param name="_Asdu"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeCode_Asdu_27(ByVal _Asdu As ASDUSt) As Asdu_27_Inf
            Dim Asdu27 As New Asdu_27_Inf

            Asdu27.TOV = _Asdu.InfData(1)
            '故障序号FAN
            'Dim low As String = Convert.ToString(_Asdu.InfData(2))
            'Dim hig As String = Convert.ToString(_Asdu.InfData(3))
            'Asdu27.FAN = Val("&H" & hig & low)
            Asdu27.FAN = BitConverter.ToInt16(New Byte() {_Asdu.InfData(2), _Asdu.InfData(3)}, 0)

            '通道序号
            Asdu27.ACC = _Asdu.InfData(4)

            Dim Value As Byte()
            ReDim Value(3)

            Array.Copy(_Asdu.InfData, 5, Value, 0, Value.Length)
            Asdu27.Value1 = Asdu27.GetValue(Value)
            ' Asdu27.Value1 = GetHexStr(Value)

            Array.Copy(_Asdu.InfData, 5 + 4, Value, 0, Value.Length)
            'Asdu27.Value2 = GetHexStr(Value)
            Asdu27.Value2 = Asdu27.GetValue(Value)
            Array.Copy(_Asdu.InfData, 5 + 4 + 4, Value, 0, Value.Length)
            ' Asdu27.Value3 = GetHexStr(Value)
            Asdu27.Value3 = Asdu27.GetValue(Value)

            Return Asdu27
        End Function

        ''' <summary>
        ''' 解码ASDU30（1EH）传输扰动值,通道数据
        ''' </summary>
        ''' <param name="_Asdu"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeCode_Asdu_30(ByVal _Asdu As ASDUSt) As Asdu_30_Inf
            Dim Asdu30 As New Asdu_30_Inf
            '存储参数
            'Asdu30.p = p

            Asdu30.TOV = _Asdu.InfData(1)
            '故障序号FAN
            'Dim low As String = Convert.ToString(_Asdu.InfData(2))
            'Dim hig As String = Convert.ToString(_Asdu.InfData(3))
            'Asdu30.FAN = Val("&H" & hig & low)
            Asdu30.FAN = BitConverter.ToInt16(New Byte() {_Asdu.InfData(2), _Asdu.InfData(3)}, 0)
            '通道序号
            Asdu30.ACC = _Asdu.InfData(4)
            '每个运用服务数据单元有关扰动值的数目(NDV)
            Asdu30.NDV = _Asdu.InfData(5)
            '运用服务数据单元的第一个信息元素的序号(NFE)
            Asdu30.NFE = _Asdu.InfData(6)

            '获取定值
            Asdu30.ListSDV = New List(Of Integer)

            '这里有一个扰动值得获取
            Dim ArrByteSdv As Byte()
            ReDim ArrByteSdv(1)
            Dim index As Integer
            For i = 1 To Asdu30.NDV
                index = (i - 1) * 2
                Array.Copy(_Asdu.InfData, 7 + index, ArrByteSdv, 0, 2)
                Asdu30.ListSDV.Add(Asdu30.GetSdv(ArrByteSdv))
            Next

            '获取16进制SDV
            ReDim ArrByteSdv(_Asdu.InfData.Length - 8)
            Array.Copy(_Asdu.InfData, 7, ArrByteSdv, 0, ArrByteSdv.Length)
            Asdu30.StrSDV = GetHexStr(ArrByteSdv)

            Return Asdu30
        End Function

        ''' <summary>
        ''' 解码ASDU50（32H）遥测值的获取
        ''' </summary>
        ''' <param name="_Asdu"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeCode_Asdu_9_50(ByVal _Asdu As ASDUSt, ByVal t As Type) As Asdu_9_50_Inf
            Dim Asdu50 As New Asdu_9_50_Inf
            '初始化值存储
            Asdu50.ListValue = New List(Of Asdu_9_50_Inf.ValueState)

            Dim len As Integer = _Asdu.InfData.Length

            For index = 0 To len - 1 Step 2
                Dim tempArryByrte As Byte()
                ReDim tempArryByrte(1)
                tempArryByrte(0) = _Asdu.InfData(index + 1)
                tempArryByrte(1) = _Asdu.InfData(index)
                'tempArryByrte(0) = _Asdu.InfData(index)
                'tempArryByrte(1) = _Asdu.InfData(index + 1)
                '获取2进制字符串
                Dim BinaryStr As String = ArrByteToStr(tempArryByrte)

                Dim TempVS As New Asdu_9_50_Inf.ValueState
                TempVS.ER = Val(BinaryStr.Chars(14))
                TempVS.OV = Val(BinaryStr.Chars(15))
                '计算值
                TempVS.Value = BinaryStrToInt((Mid(BinaryStr, 1, 13)), t)
                'TempVS.Value = BitConverter.ToInt16(tempArryByrte, 0)
                '添加结果
                Asdu50.ListValue.Add(TempVS)
            Next


            Asdu50.Value = GetHexStr(_Asdu.InfData)
            Return Asdu50
        End Function

        ''' <summary>
        ''' 解码ASDU36(24H) 电度值的获取
        ''' </summary>
        ''' <param name="_Asdu"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeCode_Asdu_36(ByVal _Asdu As ASDUSt) As Asdu_36_Inf
            Dim Asdu36 As New Asdu_36_Inf
            '初始化值存储
            Asdu36.ListPulseState = New List(Of Asdu_36_Inf.PulseState)

            Dim len As Integer = _Asdu.InfData.Length

            For i = 0 To len - 1 Step 5
                Try
                    Dim tempArryByrte As Byte()
                    ReDim tempArryByrte(4)
                    Array.Copy(_Asdu.InfData, i, tempArryByrte, 0, 5)

                    Dim TempVS As New Asdu_36_Inf.PulseState
                    '顺序号
                    TempVS.Index = tempArryByrte(4)
                    '计算值
                    TempVS.Value = BitConverter.ToInt32(New Byte() {tempArryByrte(0), tempArryByrte(1), _
                                                                  tempArryByrte(2), tempArryByrte(3)}, 0)

                    '添加结果
                    Asdu36.ListPulseState.Add(TempVS)
                Catch ex As Exception

                End Try
            Next
            Return Asdu36
        End Function


        ''' <summary>
        ''' 解码ASDU10(0AH)通用分类
        ''' </summary>
        ''' <param name="_Asdu"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeCode_Asdu_10(ByVal _Asdu As ASDUSt) As Asdu_10_Inf
            Dim Asdu10 As New Asdu_10_Inf
            '储存值
            Asdu10.ListValue = New List(Of Asdu_10_Inf.Value)

            '获取Rii
            Asdu10.Rii = _Asdu.InfData(0)
            '获取个数
            Asdu10.NGDSum = Val("&H" & Strings.Right(Convert.ToString(_Asdu.InfData(1), 16), 1))


            '获取真实数据区
            Dim ListData As New List(Of Byte)
            ListData = _Asdu.InfData.ToList
            '移除头部两个数据
            ListData.RemoveRange(0, 2)


            '必读的数据长度
            Dim Len As Integer = 6

            '循环解码数据知道没有数据
            While ListData.Count > 0
                Dim RelVal As New Asdu_10_Inf.Value

                RelVal.GinGroupNum = ListData(0)
                RelVal.GinGroupIndex = ListData(1)
                RelVal.KOD = ListData(2)

                'index=3跳过不解码

                '数据宽度
                Dim DataLen As Byte = ListData(4)

                '只解码rs2332
                Select Case DataLen
                    Case 1
                        'RelVal.Val = ListData(5)
                        '移除已经处理的数据
                        ListData.RemoveRange(0, Len + 1)
                    Case 4
                        Dim RsArr As Byte()
                        ReDim RsArr(3)
                        '存储生数据，即为原始数组
                        Array.Copy(ListData.ToArray, 6, RsArr, 0, RsArr.Length)
                        RelVal.ValArrByte = RsArr
                        '移除已经处理的数据
                        ListData.RemoveRange(0, Len + 4)
                    Case Else
                        '移除已经处理的数据
                        ListData.RemoveRange(0, Len + DataLen)
                End Select

                '对象保存
                Asdu10.ListValue.Add(RelVal)

            End While

            Return Asdu10
        End Function

    End Module
End Namespace


