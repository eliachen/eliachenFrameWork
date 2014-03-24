Imports System.Text
Imports EliaChen.CommPort

''' <summary>
''' 本机没有串口,需要装虚拟串口软件
''' </summary>
''' <remarks></remarks>
<TestClass()>
Public Class UnitTestSerialPort
    Dim Sp As New EliaChen.CommPort.SerialPort("COM1,9600,N,8,1")
    ''' <summary>
    ''' 测试打开串口并且发送,最后关闭串口
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()>
    Public Sub TestSingle()
        For index = 0 To 100
            Sp.Open()
            Sp.Write(New Byte() {1, 2, 3, 4})
            Sp.Close()
        Next

        Dim rel = (From s In Sp.CommLog
                  Where s.MsgType = EliaChen.CommPort.Log.CommPortLogElement(Of String).MsgTypeModel.fail
                     Select s).ToList()

        If rel.Count > 0 Then
            Assert.Fail("连续发送错误")
        End If
    End Sub


    Dim Sp1 As New EliaChen.CommPort.SerialPort("COM1,9600,N,8,1")
    Dim Sp2 As New EliaChen.CommPort.SerialPort("COM2,9600,N,8,1")

    ''' <summary>
    ''' 测试打开串口并且发送
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()>
    Public Sub TestDouble()

        'AddHandler Sp1.RecvDataEvent, Sub(ByRef sender As ICommPort, RecvArrByte() As Byte)

        '                              End Sub

        AddHandler Sp2.RecvDataEvent, Sub(ByRef sender As ICommPort, rb As Byte())

                                      End Sub
        Sp1.Open()
        Sp2.Open()

        Dim SendArrByte As Byte() = New Byte() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10}
        Sp1.Write(SendArrByte)
        Dim RecvArrByte As Byte() = Sp2.ReceiveAsWay(Sp2.CommPortWay)

        If Not (SendArrByte.Length = RecvArrByte.Length) Then
            Assert.Fail("收发失败")
        End If
    End Sub

End Class
