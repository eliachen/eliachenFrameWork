Imports System.Text
Imports EliaChen.CommPort
Imports EliaChen.CommProtocol.ModBus
Imports System.Linq



<TestClass()>
Public Class TestModBus_Rtu

    Dim Sp As New EliaChen.CommPort.SerialPort("COM1,9600,N,8,1")
    Dim MbRtu As New EliaChen.CommProtocol.ModBus.Rtu

    '数据获取与数据类型转换
    <TestMethod()>
    Public Sub TestFc3()
        '开串口
        Sp.Open()
        '串口与协议绑定
        MbRtu.CommPort = Sp

        Dim rel = MbRtu.Send(New Rtu.RtuSendMessage().SendFc3(1, 0, 10),
                                                         From s In MbRtu.CommRecvBuffer
                                                                            Select s)


        If rel Is Nothing Then
            Assert.Fail()
        Else
            Dim relsingle = rel.FirstOrDefault
            Dim RelList As New List(Of Object)
            RelList.Add(MbRtu.GetNumberList(Of Char)(relsingle))
            RelList.Add(MbRtu.GetNumberList(Of Int16)(relsingle))
            RelList.Add(MbRtu.GetNumberList(Of UInt16)(relsingle))
            RelList.Add(MbRtu.GetNumberList(Of Int32)(relsingle))
            RelList.Add(MbRtu.GetNumberList(Of UInt32)(relsingle))
            RelList.Add(MbRtu.GetNumberList(Of Single)(relsingle))
            RelList.Add(MbRtu.GetNumberList(Of Int64)(relsingle))
            RelList.Add(MbRtu.GetNumberList(Of UInt64)(relsingle))
            RelList.Add(MbRtu.GetNumberList(Of Double)(relsingle))
            RelList.Add(MbRtu.GetBitList(relsingle))
        End If
    End Sub

End Class
