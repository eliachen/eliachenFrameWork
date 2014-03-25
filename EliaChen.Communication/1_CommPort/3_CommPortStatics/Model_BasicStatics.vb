
Namespace CommPort.ComPortStatics

    ''' <summary>
    ''' 基类实现通讯统计的基本模式
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Model_BasicStatics
        Implements ICommPortStatitics
        Public Property CountRecv As Integer = 0 Implements ICommPortStatitics.CountRecv

        Public Property CountSend As Integer = 0 Implements ICommPortStatitics.CountSend

        Public Property SumRecv As Integer = 0 Implements ICommPortStatitics.SumRecv

        Public Property SumSend As Integer = 0 Implements ICommPortStatitics.SumSend

        Sub New()
            MyBase.New()
        End Sub


    End Class
End Namespace