Namespace Communication.Proj.Comm_IEC103
    Public Class Comm103DevState
        '设备名称
        Public Name As String = ""
        '设备型号
        Public Edition As String = ""

        '设备地址
        Public Addr As Byte
        '设备通讯参数
        Public Fcb As Byte
        '录波最大地址
        Public AccSum As Byte
        '系数
        Public PT As Double = 1
        Public CT As Double = 1

        '电流系数
        Public parI As Double = 1
        '电压系数
        Public parU As Double = 1
        '功率因数
        Public parP As Double = 1

        Public VAL4 As Double = 1
        Public VAL5 As Double = 1
        Public VAL6 As Double = 1
        Public VAL7 As Double = 1
        Public VAL8 As Double = 1


        Sub New(ByVal _Addr As Byte)
            Me.Addr = _Addr
        End Sub

        Sub New(ByVal _Addr As Byte, ByVal _PT As Double, ByVal _CT As Double)
            Me.Addr = _Addr
            Me.PT = _PT
            Me.CT = _CT
        End Sub

        '传入型号
        Sub New(ByVal _Name As String, ByVal _Addr As Byte, ByVal _PT As Double, ByVal _CT As Double)
            Me.Addr = _Addr
            Me.PT = _PT
            Me.CT = _CT
            Me.Name = _Name
        End Sub

        Public Function exFcb() As Byte
            If Fcb = 0 Then
                Fcb = 1
            ElseIf Fcb = 1 Then
                Fcb = 0
            End If
            Return Fcb
        End Function
        '对于结果的输出
        Public Class ValState
            Public Addr As String
            Public RelName As String
            Public RelVal As String
            Sub New(ByVal _Addr As String, ByVal _relName As String, ByVal _relVal As String)
                Addr = _Addr
                RelName = _relName
                RelVal = _relVal
            End Sub
        End Class

    End Class
End Namespace

