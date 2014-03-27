Imports System.Text
Imports EliaChen.CommPort
Imports EliaChen.CommProtocol.ModBus
Imports System.Linq
Imports EliaChen.CommProtocol.ModBus.Rtu
Imports EliaChen.CommProtocol.Common


<TestClass()>
Public Class TestModBus_Rtu

    Dim Sp As New EliaChen.CommPort.SerialPort("COM1,9600,N,8,1")
    Dim MbRtu As New EliaChen.CommProtocol.ModBus.Rtu

    <TestMethod()>
    Public Sub TestFc1()
        '开串口
        Sp.Open()
        '串口与协议绑定
        MbRtu.CommPort = Sp

        Dim rel = MbRtu.SendMessage(New RtuSendMessage().SendFc01(1, 0, 10), From s In MbRtu.CommRecvBuffer
                                                                                   Select s)


        If rel Is Nothing Then
            Assert.Fail()
        Else
            Dim relsingle = rel.FirstOrDefault
            Dim RelList As New List(Of Object)
            'RelList.Add(DataHelper.GetNumberList(Of Char)(relsingle.RecvMsg.Data))
            'RelList.Add(DataHelper.GetNumberList(Of Int16)(relsingle.RecvMsg.Data))
            'RelList.Add(DataHelper.GetNumberList(Of UInt16)(relsingle.RecvMsg.Data))
            'RelList.Add(DataHelper.GetNumberList(Of Int32)(relsingle.RecvMsg.Data))
            'RelList.Add(DataHelper.GetNumberList(Of UInt32)(relsingle.RecvMsg.Data))
            'RelList.Add(DataHelper.GetNumberList(Of Single)(relsingle.RecvMsg.Data))
            'RelList.Add(DataHelper.GetNumberList(Of Int64)(relsingle.RecvMsg.Data))
            'RelList.Add(DataHelper.GetNumberList(Of UInt64)(relsingle.RecvMsg.Data))
            'RelList.Add(DataHelper.GetNumberList(Of Double)(relsingle.RecvMsg.Data))
            RelList.Add(DataHelper.GetBitList(relsingle.RecvMsg.Data))
        End If
    End Sub

    '数据获取与数据类型转换
    <TestMethod()>
    Public Sub TestFc3()
        '开串口
        Sp.Open()
        '串口与协议绑定
        MbRtu.CommPort = Sp

        Dim rel = MbRtu.SendMessage(New RtuSendMessage().SendFc03(1, 0, 10), From s In MbRtu.CommRecvBuffer
                                                                                   Select s)


        If rel Is Nothing Then
            Assert.Fail()
        Else
            Dim relsingle = rel.FirstOrDefault
            Dim RelList As New List(Of Object)
            RelList.Add(DataHelper.GetNumberList(Of Char)(relsingle.RecvMsg.Data))
            RelList.Add(DataHelper.GetNumberList(Of Int16)(relsingle.RecvMsg.Data))
            RelList.Add(DataHelper.GetNumberList(Of UInt16)(relsingle.RecvMsg.Data))
            RelList.Add(DataHelper.GetNumberList(Of Int32)(relsingle.RecvMsg.Data))
            RelList.Add(DataHelper.GetNumberList(Of UInt32)(relsingle.RecvMsg.Data))
            RelList.Add(DataHelper.GetNumberList(Of Single)(relsingle.RecvMsg.Data))
            RelList.Add(DataHelper.GetNumberList(Of Int64)(relsingle.RecvMsg.Data))
            RelList.Add(DataHelper.GetNumberList(Of UInt64)(relsingle.RecvMsg.Data))
            RelList.Add(DataHelper.GetNumberList(Of Double)(relsingle.RecvMsg.Data))
            RelList.Add(DataHelper.GetBitList(relsingle.RecvMsg.Data))
        End If
    End Sub

    <TestMethod()>
    Public Sub TestFc16()
        '开串口
        Sp.Open()
        '串口与协议绑定
        MbRtu.CommPort = Sp

        Dim rel
        rel = MbRtu.SendMessage(New RtuSendMessage().SendFc16(1, 0, 3, New Short() {1, 2, 3}),
                                                                        From s In MbRtu.CommRecvBuffer
                                                                                                    Select s)
        Dim rel2 = New RtuSendTrip().SendFunc16_Trip(1, 0, 3, New Short() {1, 3, 4})
        MbRtu.SendTrip(rel2)


        If rel Is Nothing Then
            Assert.Fail()
        Else
            Dim relsingle = rel.FirstOrDefault
        End If

    End Sub
End Class
