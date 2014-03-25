Imports EliachenFw.Comm.Proj.Comm_IEC103.Code.BasicCode
Imports EliachenFw.Comm.Proj.Comm_IEC103.Code.AdvCode

Namespace Communication.Proj.Comm_IEC103.Order
    '主—>从
    Module OrderMasterToSlave

#Region "固定帧命令"
        '功能码	    帧类型	           功能描述	 FCV状态
        '0	发送/确认帧 C_RCU_NA_3	 复位通信单元	0
        '3	发送/确认帧	              传送数据	    1
        '4	发送/无回答帧	         传送数据	    0
        '7	复位帧计数位C_RFB_NA_3	传送数据	    0
        '9	请求/响应帧C_RLK_NA_3	召唤链路状态	0
        '10	请求/响应帧C_PL1_NA_3	召唤1 级数据	1
        '11	请求/响应帧C_PL2_NA_3	召唤2 级数据	1


        Private Function CommonConstituteS(ByVal _Addr As Byte, ByVal _Fcb As Byte, _
                                          ByVal _Fun As Byte, ByVal _Fcv As Byte) As Byte()
            '控制域
            Dim FunCode As New CodeStM
            '固定帧
            Dim StableFra As New Recv_StableFrame

            FunCode.FunctionCode = _Fun
            FunCode.Fcv = _Fcv
            FunCode.Fcb = _Fcb
            '表示主->从，Prm置1
            FunCode.Prm = 1
            StableFra.Addr = _Addr
            StableFra.oriCode = Encode_Code(FunCode)
            Return Encode_StableFrame(StableFra)
        End Function

        ''' <summary>
        ''' 复位通信单元
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CommReset(ByVal _Addr As Byte, ByVal _Fcb As Byte) As Byte()
            Return CommonConstituteS(_Addr, _Fcb, 0, 0)
        End Function
        ''' <summary>
        ''' 召唤链路状态
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ListenState(ByVal _Addr As Byte, ByVal _Fcb As Byte) As Byte()
            Return CommonConstituteS(_Addr, _Fcb, 9, 0)
        End Function
        ''' <summary>
        ''' 召唤1级数据
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function AskDataOne(ByVal _Addr As Byte, ByVal _Fcb As Byte) As Byte()
            Return CommonConstituteS(_Addr, _Fcb, 10, 1)
        End Function
        ''' <summary>
        ''' 召唤2级数据
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function AskDataTwo(ByVal _Addr As Byte, ByVal _Fcb As Byte) As Byte()
            Return CommonConstituteS(_Addr, _Fcb, 11, 1)
        End Function
#End Region

#Region "不固定帧命令"
        Private Function CommonConstituteUnS(ByVal _Addr As Byte, ByVal _Fcb As Byte, _
                                             ByVal _Asdu As ASDUSt) As Byte()
            'Code
            Dim FunCode As New CodeStM
            'Asdu
            Dim Asdu As ASDUSt
            '固定帧
            Dim unStableFra As New Recv_unStableFrame

            'Code设置
            FunCode.Prm = 1

            FunCode.Fcv = 1
            FunCode.Fcb = _Fcb
            FunCode.FunctionCode = 3

            'Asdu传入
            Asdu = _Asdu
            ''调试的是都是1
            'Asdu.Comaddr = 1

            '构建不定帧
            unStableFra.Addr = _Addr
            unStableFra.oriASDU = Encode_ASDU(Asdu)
            unStableFra.oriCode = Encode_Code(FunCode)

            Return Encode_unStableFrame(unStableFra)
        End Function

        ''' <summary>
        ''' 对时,系统时间Now
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function AskTimeSyn(ByVal _Addr As Byte, ByVal _Fcb As Byte) As Byte()
            '6H(类别标识)
            '81H(可变结构限定词)
            '8H(传输原因)
            'COMADDR(单元公共地址)
            '255:   功能类型()
            '0H(信息序号)
            'Ms(毫秒低字节) 	时间 
            '            Ms(毫秒高字节)
            'IV 0 分 2进制(六位) 
            'SU 0 0 时 2进制(五位) 
            '0 0 0 日 2进制(五位) 
            '0 0 0 0 月 2进制(四位) 
            '0 年 2进制(七位) 


            'Asdu
            Dim Asdu As New ASDUSt
            Asdu.Type = &H6
            Asdu.Vsq = &H81
            Asdu.Cot = &H8
            Asdu.Comaddr = &HFF
            Asdu.Fun = &HFF
            Asdu.Inf = &H0

            ReDim Asdu.InfData(6)

            '当前时间毫秒
            Dim NowTime As Integer = Now.Second * 1000
            Dim TimeStr As String = Convert.ToString(NowTime, 16)

            '补零
            If TimeStr.Length < 4 Then
                Dim tempStr As New String("0", 4 - TimeStr.Length)
                TimeStr = tempStr & TimeStr
            End If

            Dim hig As String = "&H" & Mid(TimeStr, 1, 2)
            Dim low As String = "&H" & Mid(TimeStr, 3, 2)

            Asdu.InfData(0) = CByte(low)
            Asdu.InfData(1) = CByte(hig)

            Asdu.InfData(2) = CByte(Now.Minute)
            Asdu.InfData(3) = CByte(Now.Hour)
            Asdu.InfData(4) = CByte(Now.Day)
            Asdu.InfData(5) = CByte(Now.Month)

            Dim YearStr As String = Now.Year.ToString

            If YearStr.Length > 1 Then
                YearStr = Right(YearStr, 2)
            End If

            Asdu.InfData(6) = CByte(YearStr)

            '改地址了
            Return CommonConstituteUnS(_Addr, _Fcb, Asdu)
        End Function

        ''' <summary>
        ''' 总召唤->获取遥信信息
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <param name="_Sectors"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function AskTotally(ByVal _Addr As Byte, ByVal _Fcb As Byte, ByVal _Sectors As Byte) As Byte()
            '7:          H(类别标识)
            '81:         H(可变结构限定词)
            '9:          传输原因()
            '            COMADDR(单元公共地址)
            '255:        功能类型()
            '0:          H(信息序号)
            '            总召唤序号(信息元素)


            'Asdu
            Dim Asdu As New ASDUSt
            Asdu.Type = &H7
            Asdu.Vsq = &H81
            Asdu.Cot = &H9
            '不知道几个公共地址&H1
            Asdu.Comaddr = _Sectors
            Asdu.Fun = &HFF
            Asdu.Inf = &H0


            ReDim Asdu.InfData(0)
            '许继
            'Asdu.InfData(0) = &H0
            '万利达
            Asdu.InfData(0) = &H15
            Return CommonConstituteUnS(_Addr, _Fcb, Asdu)
        End Function

#Region "录波"
        ''' <summary>
        ''' 获取扰动表
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <param name="_Sectors"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function AskDisturbanceDataTable(ByVal _Addr As Byte, ByVal _Fcb As Byte, ByVal _Sectors As Byte) As Byte()
            'Asdu
            Dim Asdu As New ASDUSt
            Asdu.Type = &H18
            Asdu.Vsq = &H81
            Asdu.Cot = &H1F
            Asdu.Comaddr = _Sectors
            Asdu.Fun = &H0
            Asdu.Inf = &H0

            ReDim Asdu.InfData(4)
            '命令类型TOO
            Asdu.InfData(0) = &H18
            '扰动值类型TOV
            Asdu.InfData(1) = &H1
            '故障序号FAN（2个字节）
            Asdu.InfData(2) = &H0
            Asdu.InfData(3) = &H0
            '实际通道序号(ACC)：全局
            Asdu.InfData(4) = &H0
            Return CommonConstituteUnS(_Addr, _Fcb, Asdu)
        End Function

        ''' <summary>
        ''' 获取扰动数据
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <param name="_FaultNum"></param>
        ''' <param name="_Sectors"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function AskDisturbanceData(ByVal _Addr As Byte, ByVal _Fcb As Byte, ByVal _FaultNum As Int16, ByVal _Sectors As Byte) As Byte()
            'Asdu
            Dim Asdu As New ASDUSt
            Asdu.Type = &H18
            Asdu.Vsq = &H81
            Asdu.Cot = &H1F
            Asdu.Comaddr = _Sectors
            Asdu.Fun = &H0
            Asdu.Inf = &H0


            ReDim Asdu.InfData(4)
            '命令类型TOO
            Asdu.InfData(0) = &H2
            '扰动值类型TOV
            Asdu.InfData(1) = &H1
            '故障序号FAN（2个字节）
            Dim FaultStr As String = Hex(_FaultNum)
            If FaultStr.Length < 3 Then
                Asdu.InfData(2) = _FaultNum   '故障低位
                Asdu.InfData(3) = &H0         '故障高位
            Else
                Dim FaultLow As String = "&H" & Right(FaultStr, 2)
                Dim FaultHig As String = "&H" & Left(FaultStr, FaultStr.Length - 2)
                Asdu.InfData(2) = Val(FaultLow)   '故障低位
                Asdu.InfData(3) = Val(FaultHig)   '故障高位
            End If

            '实际通道序号(ACC)：全局
            Asdu.InfData(4) = &H0
            Return CommonConstituteUnS(_Addr, _Fcb, Asdu)
        End Function

        ''' <summary>
        ''' 获取状态变位
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <param name="_FaultNum"></param>
        ''' <param name="_Sectors"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function AskStateChange(ByVal _Addr As Byte, ByVal _Fcb As Byte, ByVal _FaultNum As Int16, ByVal _Sectors As Byte) As Byte()
            'Asdu
            Dim Asdu As New ASDUSt
            Asdu.Type = &H18
            Asdu.Vsq = &H81
            Asdu.Cot = &H1F
            Asdu.Comaddr = _Sectors
            Asdu.Fun = &H0
            Asdu.Inf = &H0

            ReDim Asdu.InfData(4)
            '命令类型TOO
            Asdu.InfData(0) = &H10
            '扰动值类型TOV
            Asdu.InfData(1) = &H1
            '故障序号FAN（2个字节）
            Dim FaultStr As String = Hex(_FaultNum)
            If FaultStr.Length < 3 Then
                Asdu.InfData(2) = _FaultNum   '故障低位
                Asdu.InfData(3) = &H0         '故障高位
            Else
                Dim FaultLow As String = "&H" & Right(FaultStr, 2)
                Dim FaultHig As String = "&H" & Left(FaultStr, FaultStr.Length - 2)
                Asdu.InfData(2) = Val(FaultLow)   '故障低位
                Asdu.InfData(3) = Val(FaultHig)   '故障高位
            End If

            '实际通道序号(ACC)：全局
            Asdu.InfData(4) = &H0
            Return CommonConstituteUnS(_Addr, _Fcb, Asdu)
        End Function


        ''' <summary>
        ''' 状态确认，获取额定因子
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <param name="_FaultNum"></param>
        ''' <param name="_Sectors"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function AskStateConfirm(ByVal _Addr As Byte, ByVal _Fcb As Byte, ByVal _FaultNum As Int16, ByVal _Sectors As Byte) As Byte()
            'Asdu
            Dim Asdu As New ASDUSt
            Asdu.Type = &H19
            Asdu.Vsq = &H81
            Asdu.Cot = &H1F
            Asdu.Comaddr = _Sectors
            Asdu.Fun = &H0
            Asdu.Inf = &H0

            ReDim Asdu.InfData(4)
            '命令类型TOO
            Asdu.InfData(0) = &H44
            '扰动值类型TOV
            Asdu.InfData(1) = &H1
            '故障序号FAN（2个字节）
            Dim FaultStr As String = Hex(_FaultNum)
            If FaultStr.Length < 3 Then
                Asdu.InfData(2) = _FaultNum   '故障低位
                Asdu.InfData(3) = &H0         '故障高位
            Else
                Dim FaultLow As String = "&H" & Right(FaultStr, 2)
                Dim FaultHig As String = "&H" & Left(FaultStr, FaultStr.Length - 2)
                Asdu.InfData(2) = Val(FaultLow)   '故障低位
                Asdu.InfData(3) = Val(FaultHig)   '故障高位
            End If

            '实际通道序号(ACC)：全局
            Asdu.InfData(4) = &H0
            Return CommonConstituteUnS(_Addr, _Fcb, Asdu)
        End Function

        ''' <summary>
        ''' 获取通道数据
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <param name="_Sectors"></param>
        ''' <param name="_FaultNum"></param>
        ''' <param name="_Acc"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function AskAccData(ByVal _Addr As Byte, ByVal _Fcb As Byte, ByVal _Sectors As Byte, ByVal _FaultNum As Int16, ByVal _Acc As Byte) As Byte()
            'Asdu
            Dim Asdu As New ASDUSt
            Asdu.Type = &H18
            Asdu.Vsq = &H81
            Asdu.Cot = &H1F
            Asdu.Comaddr = _Sectors
            Asdu.Fun = &H0
            Asdu.Inf = &H0

            ReDim Asdu.InfData(4)
            '命令类型TOO
            Asdu.InfData(0) = &H8
            '扰动值类型TOV
            Asdu.InfData(1) = &H1
            '故障序号FAN（2个字节）
            Dim FaultStr As String = Hex(_FaultNum)
            If FaultStr.Length < 3 Then
                Asdu.InfData(2) = _FaultNum   '故障低位
                Asdu.InfData(3) = &H0         '故障高位
            Else
                Dim FaultLow As String = "&H" & Right(FaultStr, 2)
                Dim FaultHig As String = "&H" & Left(FaultStr, FaultStr.Length - 2)
                Asdu.InfData(2) = Val(FaultLow)   '故障低位
                Asdu.InfData(3) = Val(FaultHig)   '故障高位
            End If

            '实际通道序号(ACC)：全局
            Asdu.InfData(4) = _Acc
            Return CommonConstituteUnS(_Addr, _Fcb, Asdu)
        End Function

        ''' <summary>
        ''' 通道认可
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <param name="_Sectors"></param>
        ''' <param name="_FaultNum"></param>
        ''' <param name="_Acc"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function AskAccConfirm(ByVal _Addr As Byte, ByVal _Fcb As Byte, ByVal _Sectors As Byte, ByVal _FaultNum As Int16, ByVal _Acc As Byte) As Byte()
            'Asdu
            Dim Asdu As New ASDUSt
            Asdu.Type = &H19
            Asdu.Vsq = &H81
            Asdu.Cot = &H1F
            Asdu.Comaddr = &H1
            Asdu.Fun = &H0
            Asdu.Inf = &H0

            ReDim Asdu.InfData(4)
            '命令类型TOO
            Asdu.InfData(0) = &H42
            '扰动值类型TOV
            Asdu.InfData(1) = &H1
            '故障序号FAN（2个字节）
            Dim FaultStr As String = Hex(_FaultNum)
            If FaultStr.Length < 3 Then
                Asdu.InfData(2) = _FaultNum   '故障低位
                Asdu.InfData(3) = &H0         '故障高位
            Else
                Dim FaultLow As String = "&H" & Right(FaultStr, 2)
                Dim FaultHig As String = "&H" & Left(FaultStr, FaultStr.Length - 2)
                Asdu.InfData(2) = Val(FaultLow)   '故障低位
                Asdu.InfData(3) = Val(FaultHig)   '故障高位
            End If

            '实际通道序号(ACC)：全局
            Asdu.InfData(4) = _Acc
            Return CommonConstituteUnS(_Addr, _Fcb, Asdu)
        End Function

        ''' <summary>
        ''' 扰动确认
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <param name="_Sectors"></param>
        ''' <param name="_FaultNum"></param>
        ''' <param name="_Acc"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function AskDisConfirm(ByVal _Addr As Byte, ByVal _Fcb As Byte, ByVal _Sectors As Byte, ByVal _FaultNum As Int16, ByVal _Acc As Byte) As Byte()
            'Asdu
            Dim Asdu As New ASDUSt
            Asdu.Type = &H19
            Asdu.Vsq = &H81
            Asdu.Cot = &H1F
            Asdu.Comaddr = _Sectors
            Asdu.Fun = &H0
            Asdu.Inf = &H0

            ReDim Asdu.InfData(4)
            '命令类型TOO
            Asdu.InfData(0) = &H40
            '扰动值类型TOV
            Asdu.InfData(1) = &H1
            '故障序号FAN（2个字节）
            Dim FaultStr As String = Hex(_FaultNum)
            If FaultStr.Length < 3 Then
                Asdu.InfData(2) = _FaultNum   '故障低位
                Asdu.InfData(3) = &H0         '故障高位
            Else
                Dim FaultLow As String = "&H" & Right(FaultStr, 2)
                Dim FaultHig As String = "&H" & Left(FaultStr, FaultStr.Length - 2)
                Asdu.InfData(2) = Val(FaultLow)   '故障低位
                Asdu.InfData(3) = Val(FaultHig)   '故障高位
            End If

            '实际通道序号(ACC)：全局
            Asdu.InfData(4) = _Acc
            Return CommonConstituteUnS(_Addr, _Fcb, Asdu)
        End Function
#End Region


        ''' <summary>
        ''' 获取遥测
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <param name="_Sectors"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function AskFarComm(ByVal _Addr As Byte, ByVal _Fcb As Byte, ByVal _Sectors As Byte) As Byte()
            'Asdu
            Dim Asdu As New ASDUSt
            Asdu.Type = 93
            Asdu.Vsq = &H81
            Asdu.Cot = 20
            Asdu.Comaddr = _Sectors
            Asdu.Fun = &H0
            Asdu.Inf = &H0

            ReDim Asdu.InfData(0)
            Asdu.InfData(0) = 0
            Return CommonConstituteUnS(_Addr, _Fcb, Asdu)
        End Function

        ''' <summary>
        ''' 召唤电镀量
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <param name="_Sectors"></param>
        ''' <param name="_Fun"></param>
        ''' <param name="_order"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Order_AskEleMeasure(ByVal _Addr As Byte, ByVal _Fcb As Byte, ByVal _Sectors As Byte, ByVal _Fun As Byte, ByVal _order As Byte) As Byte()
            Dim Asdu As New ASDUSt
            Asdu.Type = 88
            Asdu.Vsq = &H1
            Asdu.Cot = &H2
            Asdu.Comaddr = _Sectors
            Asdu.Fun = 1
            Asdu.Inf = &H0

            ReDim Asdu.InfData(1)
            '冻结带复位
            Asdu.InfData(0) = _order

            '电度量复位

            'Asdu.InfData(0) = &H45
            Asdu.InfData(1) = 0
            Return CommonConstituteUnS(_Addr, _Fcb, Asdu)
        End Function

#Region "遥控"
        ''' <summary>
        ''' 遥控选择
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <param name="_Sectors"></param>
        ''' <param name="_Fun"></param>
        ''' <param name="_Inf"></param>
        ''' <param name="On"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Order_AskControl_Choice(ByVal _Addr As Byte, ByVal _Fcb As Byte, ByVal _Sectors As Byte, _
                                         ByVal _Fun As Byte, ByVal _Inf As Byte, ByVal [On] As Byte) As Byte()
            Dim Asdu As New ASDUSt
            Asdu.Type = &H40
            Asdu.Vsq = &H81
            Asdu.Cot = 12
            Asdu.Comaddr = _Sectors
            Asdu.Fun = _Fun
            Asdu.Inf = _Inf

            ReDim Asdu.InfData(1)

            If [On] = 1 Then '合
                Asdu.InfData(0) = &H82

            Else             '分
                Asdu.InfData(0) = &H81
            End If

            Asdu.InfData(1) = 0
            Return CommonConstituteUnS(_Addr, _Fcb, Asdu)
        End Function
        ''' <summary>
        ''' 遥控执行
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <param name="_Sectors"></param>
        ''' <param name="_Fun"></param>
        ''' <param name="_Inf"></param>
        ''' <param name="On"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Order_AskControl_Excute(ByVal _Addr As Byte, ByVal _Fcb As Byte, ByVal _Sectors As Byte, _
                                         ByVal _Fun As Byte, ByVal _Inf As Byte, ByVal [On] As Byte) As Byte()
            Dim Asdu As New ASDUSt
            Asdu.Type = &H40
            Asdu.Vsq = &H81
            Asdu.Cot = 12
            Asdu.Comaddr = _Sectors
            Asdu.Fun = _Fun
            Asdu.Inf = _Inf


            ReDim Asdu.InfData(1)

            If [On] = 1 Then '合
                Asdu.InfData(0) = &H2

            Else             '分
                Asdu.InfData(0) = &H1
            End If

            Asdu.InfData(1) = 0
            Return CommonConstituteUnS(_Addr, _Fcb, Asdu)
        End Function
        ''' <summary>
        ''' 遥控撤销
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <param name="_Sectors"></param>
        ''' <param name="_Fun"></param>
        ''' <param name="_Inf"></param>
        ''' <param name="On"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Order_AskControl_Pause(ByVal _Addr As Byte, ByVal _Fcb As Byte, ByVal _Sectors As Byte, _
                                         ByVal _Fun As Byte, ByVal _Inf As Byte, ByVal [On] As Byte) As Byte()
            Dim Asdu As New ASDUSt
            Asdu.Type = &H40
            Asdu.Vsq = &H81
            Asdu.Cot = 12
            Asdu.Comaddr = _Sectors
            Asdu.Fun = _Fun
            Asdu.Inf = _Inf


            ReDim Asdu.InfData(1)

            If [On] = 1 Then '合
                Asdu.InfData(0) = &HC2

            Else             '分
                Asdu.InfData(0) = &HC1
            End If

            Asdu.InfData(1) = 0
            Return CommonConstituteUnS(_Addr, _Fcb, Asdu)
        End Function

#End Region

        ''' <summary>
        ''' 通用分类读取
        ''' </summary>
        ''' <param name="_Addr"></param>
        ''' <param name="_Fcb"></param>
        ''' <param name="_Sectors"></param>
        ''' <param name="_GroupNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Order_AskComm(ByVal _Addr As Byte, ByVal _Fcb As Byte, ByVal _Sectors As Byte, ByVal _GroupNum As Byte) As Byte()
            'Asdu
            Dim Asdu As New ASDUSt
            Asdu.Type = &H15
            Asdu.Vsq = &H81
            Asdu.Cot = &H2A
            Asdu.Comaddr = _Sectors
            Asdu.Fun = &HFE
            Asdu.Inf = &HF1

            ReDim Asdu.InfData(4)
            'Rii
            Asdu.InfData(0) = 1
            '通用分类标识数目
            Asdu.InfData(1) = 11
            '组号
            Asdu.InfData(2) = _GroupNum
            '条目号
            Asdu.InfData(3) = 0
            'KOD
            Asdu.InfData(4) = 1
            Return CommonConstituteUnS(_Addr, _Fcb, Asdu)
        End Function
#End Region
    End Module
End Namespace

