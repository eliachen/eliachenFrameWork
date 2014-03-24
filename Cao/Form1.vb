
Imports System.Threading.Tasks
Imports EliaChen.CommPort

Public Class Form1
    Private l As New System.Collections.Concurrent.ConcurrentBag(Of EliaChen.CommPort.TcpClient)

    Dim Sp1 As New EliaChen.CommPort.SerialPort("COM1,9600,N,8,1")
    Dim Sp2 As New EliaChen.CommPort.SerialPort("COM2,9600,N,8,1")

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        AddHandler Sp1.RecvDataEvent, Sub(ByRef ea As ICommPort, RecvArrByte() As Byte)

                                      End Sub

        AddHandler Sp2.RecvDataEvent, Sub(ByRef ea As ICommPort, RecvArrByte() As Byte)

                                      End Sub
        Sp1.Open()
        Sp2.Open()


        For index = 0 To 100
            'Sp1.Open()
            'Sp1.Write(New Byte() {1, 2, 3, 4})
            'Sp1.Close()
        Next

        Dim rel = (From s In Sp1.CommLog
                  Where s.MsgType = EliaChen.CommPort.Log.CommPortLogElement(Of String).MsgTypeModel.fail
                     Select s).ToList()

        If rel.Count > 0 Then

        End If
        'System.Threading.ThreadPool.QueueUserWorkItem(Sub()
        '                                                  While True
        '                                                      Try
        '                                                          Dim rd As New Random
        '                                                          Parallel.For(1, 100 * 1000, Sub(index)
        '                                                                                          Dim tmp As New Communication.CommPort.TcpClient("223.86.105.239", 804)
        '                                                                                          tmp.Connect()
        '                                                                                          Dim d As Byte()
        '                                                                                          ReDim d(rd.Next(10000, 50000))
        '                                                                                          tmp.Send(d)
        '                                                                                          tmp.DisConnect()
        '                                                                                          tmp.Dispose()
        '                                                                                      End Sub)
        '                                                      Catch ex As Exception

        '                                                      End Try

        '                                                  End While
        '                                              End Sub)

    End Sub


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

    End Sub
End Class
