
Namespace Communication.Proj.Comm_IEC103.Code.AdvCode
    'Asdu解码信息元素
    Public Module AsduInf
        ''' <summary>
        ''' Asdu5 复位通讯单元
        ''' </summary>
        ''' <remarks></remarks>
        Structure Asdu_5_Inf
            '解码出来的AscII编码
            Public AscIIStr As String
            '版本
            Public Edition As String
        End Structure
        ''' <summary>
        ''' Asdu1 遥信
        ''' </summary>
        ''' <remarks></remarks>
        Structure Asdu_1_Inf
            ' IV NT BL SB 0 0 DPI	DPI信息元素
            Public IV As Byte
            Public NT As Byte
            Public BL As Byte
            Public SB As Byte
            '遥信状态 (1分，2合,0和3不确定)
            Public FarCommState As Byte
            '时间
            Public Time As Date
            '附加信息
            Public SIN As Byte
        End Structure
        ''' <summary>
        ''' ASDU_2 事故信号的数据格式
        ''' </summary>
        ''' <remarks></remarks>
        Structure Asdu_2_Inf
            ' IV NT BL SB 0 0 DPI	DPI信息元素
            Public IV As Byte
            Public NT As Byte
            Public BL As Byte
            Public SB As Byte
            '遥信状态 (1分，2合,0和3不确定)
            Public FarCommState As Byte
            '故障号
            Public FAN As Int16
            '相对时间
            Public Time1 As Int16
            '时间
            Public Time As Date
            '附加信息
            Public SIN As Byte
        End Structure
        ''' <summary>
        ''' Asdu23 扰动数据表
        ''' </summary>
        ''' <remarks></remarks>
        Structure Asdu_23_Inf

            '故障序号FAN
            Public FAN As Int16
            '故障的状态SOF
            Public SOF As Byte
            'SOF的具体意义
            Public TP As Byte
            Public TM As Byte
            Public TEST As Byte
            Public OTEV As Byte
            '时间
            Public Time As Date
        End Structure

        ''' <summary>
        ''' ASDU28（1CH） 带标志的状态变位传输准备就绪
        ''' </summary>
        ''' <remarks></remarks>
        Structure Asdu_28_Inf

            '故障序号FAN
            Public FAN As Int16
        End Structure

        ''' <summary>
        ''' ASDU29（1DH） 带标志的状态变位传输
        ''' </summary>
        ''' <remarks></remarks>
        Structure Asdu_29_Inf

            '故障序号FAN
            Public FAN As Int16
            '带标志的状态变位的数目(NOT)
            Public NOT0 As Byte
            '带标志的位置(TAP)
            Public TAP As Byte()
            Public TAPStr As String
            '功能类型(FUN)
            Public FUN As Byte
            '信息序号(INF)
            Public INF As Byte
            '双点信息DPI
            Public DPI As Byte
            '双点信息
            Public DPIStr As String
        End Structure


        ''' <summary>
        ''' ASDU27（1BH） 被记录的通道传输准备就绪,获取额定值
        ''' </summary>
        ''' <remarks></remarks>
        Structure Asdu_27_Inf

            '扰动值的类型(TOV)
            Public TOV As Byte
            '故障序号FAN
            Public FAN As Int16
            ' 通道序号
            Public ACC As Byte

            '参数用16进制字符串存储

            '一次额定值
            Public Value1 As Double
            '二次额定值
            Public Value2 As Double
            '参比因子
            Public Value3 As Double

            Public Function GetValue(ByVal _ArrByte As Byte()) As Double
                '反序
                Dim ByteStack As New Stack(Of Byte)
                For Each _each As Byte In _ArrByte
                    ByteStack.Push(_each)
                Next
                _ArrByte = ByteStack.ToArray

                Dim BinaryStr As String = ""

                For Each _each As Byte In _ArrByte
                    Dim TempStr As String = Convert.ToString(_each, 2)
                    Dim len As Byte = 8 - TempStr.Length
                    If len <> 0 Then
                        Dim Zero As New String("0", len)
                        TempStr = Zero & TempStr
                    End If
                    BinaryStr = BinaryStr & TempStr
                Next

                '确定符号位
                Dim Sp As String = Mid(BinaryStr, 1, 1)
                Dim S As SByte
                If Sp = "0" Then
                    S = 1
                Else
                    S = -1
                End If
                '获取指数
                Dim Ep As String = Mid(BinaryStr, 2, 8)
                Dim Ex As Integer = Convert.ToInt32(Ep, 2) - 127
                '获取小数
                Dim Fp As String = Mid(BinaryStr, 10, 23)
                Fp = "1" & Fp

                '整数计算
                Dim Zp As String = Mid(Fp, 1, Ex + 1)
                Dim Z As Integer = Convert.ToInt32(Zp, 2)
                '小数计算
                Dim Xp As String = Mid(Fp, Ex + 2, Fp.Length - Ex - 1)
                Dim X As Double = 0
                For i = 1 To Xp.Length
                    X = X + Val(Xp(i - 1)) * 2 ^ (-i)
                Next

                Dim Result As Double = Z + X

                Return Result
                Return 0
            End Function
        End Structure

        ''' <summary>
        ''' ASDU30（1EH）传输扰动值,通道数据
        ''' </summary>
        ''' <remarks></remarks>
        Structure Asdu_30_Inf

            '扰动值的类型(TOV)
            Public TOV As Byte
            '故障序号FAN
            Public FAN As Int16
            ' 通道序号
            Public ACC As Byte
            '每个运用服务数据单元有关扰动值的数目(NDV)
            Public NDV As Byte
            '运用服务数据单元的第一个信息元素的序号(NFE)
            Public NFE As Byte
            '单扰动值
            Public ListSDV As List(Of Integer)
            '单扰动值16进制字符串存储
            Public StrSDV As String
            '参数值
            'Public p As Asdu_27_Inf

            '获取SDV值
            Public Function GetSdv(ByVal _ArrByte As Byte()) As Integer
                '   Dim _ArrByte As Byte() = New Byte() {&H42, &H98, &HA3, &HD7}
                Return 0
            End Function

        End Structure


        ''ASDU9 遥测值
        'Structure Asdu_9_Inf
        '   
        '    '信息元素字符串
        '    Public Value As String
        '    Public ListValue As List(Of ValueState)


        'End Structure
        'Structure ValueState
        '    Public OV As Byte
        '    Public ER As Byte
        '    Public Value As Integer
        'End StructureASDU30（1EH）传输扰动值,通道数据
        ''' <summary>
        ''' ASDU50（32H）遥测值
        ''' </summary>
        ''' <remarks></remarks>
        Structure Asdu_9_50_Inf

            '信息元素字符串
            Public Value As String
            Public ListValue As List(Of ValueState)
            Structure ValueState
                Public OV As Byte
                Public ER As Byte
                Public Value As Integer
            End Structure
        End Structure

        ''' <summary>
        ''' ASDU36（24H）电能脉冲上传
        ''' </summary>
        ''' <remarks></remarks>
        Structure Asdu_36_Inf

            Public ListPulseState As List(Of PulseState)

            Structure PulseState
                Public Value As Integer
                Public Index As Integer
            End Structure
        End Structure

        ''' <summary>
        ''' ASDU(0AH)通用信息单元
        ''' </summary>
        ''' <remarks></remarks>
        Structure Asdu_10_Inf

            'Rii
            Public Rii As Byte

            '条目个数
            Public NGDSum As Byte

            '值
            Public ListValue As List(Of Value)

            Structure Value
                '组号
                Public GinGroupNum As Byte
                '条目号
                Public GinGroupIndex As Byte
                '描述类别KOD
                Public KOD As Byte
                '值
                Public ValArrByte As Byte()
            End Structure

        End Structure


    End Module
End Namespace

