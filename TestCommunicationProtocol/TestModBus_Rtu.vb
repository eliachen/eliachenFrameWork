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

        Dim rel = MbRtu.SendMessage(New Rtu.RtuSendMessage().SendFc3(1, 0, 10), From s In MbRtu.CommRecvBuffer
                                                                                   Select s)


        If rel Is Nothing Then
            Assert.Fail()
        Else
            Dim relsingle = rel.FirstOrDefault
            Dim RelList As New List(Of Object)
            RelList.Add(Rtu.RtuSendMessage.GetFun3_NumberList(Of Char)(relsingle))
            RelList.Add(Rtu.RtuSendMessage.GetFun3_NumberList(Of Int16)(relsingle))
            RelList.Add(Rtu.RtuSendMessage.GetFun3_NumberList(Of UInt16)(relsingle))
            RelList.Add(Rtu.RtuSendMessage.GetFun3_NumberList(Of Int32)(relsingle))
            RelList.Add(Rtu.RtuSendMessage.GetFun3_NumberList(Of UInt32)(relsingle))
            RelList.Add(Rtu.RtuSendMessage.GetFun3_NumberList(Of Single)(relsingle))
            RelList.Add(Rtu.RtuSendMessage.GetFun3_NumberList(Of Int64)(relsingle))
            RelList.Add(Rtu.RtuSendMessage.GetFun3_NumberList(Of UInt64)(relsingle))
            RelList.Add(Rtu.RtuSendMessage.GetFun3_NumberList(Of Double)(relsingle))
            RelList.Add(Rtu.RtuSendMessage.GetFun3_BitList(relsingle))
        End If
    End Sub

    <TestMethod()>
    Public Sub TestFc16()
        '开串口
        Sp.Open()
        '串口与协议绑定
        MbRtu.CommPort = Sp

        Dim rel
        rel = MbRtu.SendMessage(New Rtu.RtuSendMessage().SendFc16(1, 0, 3, New Short() {1, 2, 3}),
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
