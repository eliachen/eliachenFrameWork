
Namespace CommPort.ComPortWay
    ''' <summary>
    ''' 基类实现通讯的基本模式
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class Model_Basic
        Implements ICommPortWay



        Sub New()
            Me.TimeDelaySend = 0
            Me.TimeOutRecv = 0
            Me.CommDone = New System.Threading.ManualResetEvent(False)
            Me.CommDeal = Nothing
            Me.CommValidate = Nothing
        End Sub

        '发送前的延迟
        Public Property TimeDelaySend As Integer Implements ICommPortWay.TimeDelaySend

        '发送后的延迟
        Public Property TimeOutRecv As Integer Implements ICommPortWay.TimeOutRecv

        '数据接收延迟通告
        Public Property CommDone As System.Threading.ManualResetEvent Implements ICommPortWay.CommDone


        Public Property CommDeal As Func(Of ICommPort, Byte()) Implements ICommPortWay.CommDeal

        Public Property CommValidate As Func(Of Byte(), Boolean) Implements ICommPortWay.CommValidate
    End Class
End Namespace