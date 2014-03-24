Imports System.Text
Imports EliaChen.CommPort
Imports EliaChen.CommProtocol.ModBus
Imports System.Linq



<TestClass()>
Public Class TestModBus_Rtu

    Dim Sp As New EliaChen.CommPort.SerialPort("COM1,9600,N,8,1")
    Dim ss As New EliaChen.CommProtocol.ModBus.Rtu
    <TestMethod()>
    Public Sub TestFc3()
        Sp.Open()
        ss.CommPort = Sp

 
        Dim rel = ss.Send(New Rtu.RtuSendMessage().SendFc3(1, 0, 10),
                                                From s In ss.CommRecvBuffer
                                                                    Select s)



        If rel Is Nothing Then
            Assert.Fail()
        Else
            Dim l = rel(0).Data.ToList()
            l.RemoveAt(0)
            'l.RemoveAt(0)

            Dim z = BitConverter.ToUInt16(New Byte() {99, 0}, 0)

        End If
    End Sub

End Class
