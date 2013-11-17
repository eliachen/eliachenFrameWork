Imports System.Text

<TestClass()>
Public Class UnitTest1

    <TestMethod()>
    Public Sub TestMethod1()
        Dim t As New Communication.CommPort.eTcpSever(Communication.CommPort.eDNS.GetLocalAddr(0), 8080)
        t.eListen()
        t.eAccept()


    End Sub

End Class
