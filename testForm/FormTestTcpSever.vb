Imports Communication
Imports Communication.CommPort.eTcpClient
Imports System.Threading.Tasks.Task
Public Class FormTestTcpSever
    'Dim testTcpSever As New Communication.CommPort.eTcpSever((From p In Communication.CommPort.eDNS.GetLocalAddr
    '               Where p.AddressFamily = Net.Sockets.AddressFamily.InterNetwork
    '                 Select p).FirstOrDefault, 8080)
    Dim testTcpSever As New Communication.CommPort.eTcpSever((From p In Communication.CommPort.eDNS.GetLocalAddr
                  Where p.AddressFamily = Net.Sockets.AddressFamily.InterNetwork
                    Select p).FirstOrDefault, 8080)
    '120.83.32.3

    Dim testTcpClients As New System.Collections.Concurrent.BlockingCollection(Of Communication.CommPort.eTcpClient)

    'Dim timerList As New Timers.Timer(1000)
    Private Async Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        'AddHandler timerList.Elapsed, Sub()

        '                              End Sub
        'timerList.Start()

        Dim x = Await sumx()

    End Sub

    Public Async Function sumx() As Threading.Tasks.Task(Of Integer)
        Dim l As New System.Collections.Concurrent.BlockingCollection(Of Integer)
        Await System.Threading.Tasks.Task.Run(Sub()

                                                  System.Threading.Tasks.Parallel.For(0, 10, Sub(index)
                                                                                                 l.Add(New Random(2).Next(2, 5))
                                                                                                 System.Threading.Thread.Sleep(500)
                                                                                             End Sub)
                                              End Sub)

        Await System.Threading.Tasks.Task.Run(Sub()

                                                  System.Threading.Tasks.Parallel.For(0, 20, Sub(index)
                                                                                                 l.Add(New Random(2).Next(2, 5))
                                                                                                 System.Threading.Thread.Sleep(50)
                                                                                             End Sub)
                                              End Sub)

        Return Aggregate p In l.AsParallel Into Sum(p)
    End Function

    Private Async Function GetCliensRel() As Threading.Tasks.Task
        Await Run(Sub()
                      If testTcpClients.Count > 0 Then
                          Dim RelOnline = From s In testTcpClients.AsParallel
                                      Where s.IsConnected = True And s.CommSocket.LocalEndPoint IsNot Nothing
                                             Select s.CommSocket.LocalEndPoint
                          Dim RelDropped = From s In testTcpClients.AsParallel
                                       Where s.IsConnected = False And s.CommSocket.LocalEndPoint IsNot Nothing
                                              Select s.CommSocket.LocalEndPoint

                          Me.BeginInvoke(Sub()
                                             Me.ListBox4.Items.Clear()
                                             Me.ListBox5.Items.Clear()
                                             Me.ListBox6.Items.Clear()
                                             Me.ListBox8.Items.Clear()

                                             If RelOnline.ToArray.Count > 0 Then
                                                 Me.ListBox4.Items.AddRange(RelOnline.ToArray)
                                             End If

                                             If RelDropped.ToArray.Count > 0 Then
                                                 Me.ListBox5.Items.AddRange(RelDropped.ToArray)
                                             End If

                                             Me.ListBox6.Items.Add("在线:" & RelOnline.Count)
                                             Me.ListBox6.Items.Add("掉线:" & RelDropped.Count)
                                             Me.ListBox6.Items.Add("---")
                                             Me.ListBox6.Items.Add("发送次数：" & testTcpClients.Sum(Function(p) p.CommStatics.CountSend))
                                             Me.ListBox6.Items.Add("发送字节：" & testTcpClients.Sum(Function(p) p.CommStatics.SumSend))
                                             Me.ListBox6.Items.Add("---")
                                             Me.ListBox6.Items.Add("接收次数：" & testTcpClients.Sum(Function(p) p.CommStatics.CountRecv))
                                             Me.ListBox6.Items.Add("接收字节：" & testTcpClients.Sum(Function(p) p.CommStatics.SumRecv))

                                             Dim RelLogSuccess = From s In testTcpClients.AsParallel
                                                                       From p In s.CommLog
                                                                         Where p.MsgType = CommPort.Log.CommPortLogElement(Of eTcpClientLog).MsgTypeModel.success
                                                                               Select p.CommPortConfig, p.MsgProcessDescription, p.Msg.SkErr, p.Msg.Msg, conn = IIf(p.CommPortIsConnected, "在线", "断线"), p.MsgTime


                                             Dim RelLogError = From s In testTcpClients.AsParallel
                                                                       From p In s.CommLog
                                                                         Where p.MsgType = CommPort.Log.CommPortLogElement(Of eTcpClientLog).MsgTypeModel.fail
                                                                               Select p.CommPortConfig, p.MsgProcessDescription, p.Msg.SkErr, p.Msg.Msg, conn = IIf(p.CommPortIsConnected, "在线", "断线"), p.MsgTime

                                             IIf(RelLogSuccess.ToArray.Count > 0, Sub()
                                                                                      'Me.ListBox8.Items.Add("Success")
                                                                                      'Me.ListBox8.Items.AddRange(RelLogSuccess.ToArray)
                                                                                  End Sub, Sub() Me.ListBox8.Items.Add("Success")).invoke()

                                             IIf(RelLogError.ToArray.Count > 0, Sub()
                                                                                    Me.ListBox8.Items.Add("Error")
                                                                                    Me.ListBox8.Items.AddRange(RelLogError.ToArray)
                                                                                End Sub, Sub() Me.ListBox8.Items.Add("Error")).invoke()
                                         End Sub)


                      End If
                  End Sub)

    End Function
    Private Async Function GetSeverRel() As Threading.Tasks.Task

        Await System.Threading.Tasks.Task.Run(Sub()
                                                  Dim RelOnline = From s In testTcpSever.ClientList.AsParallel
                                                                            Where s.IsConnected = True
                                                                                   Select s.RecvId & Space(1) & s.CommCofigStr
                                                  Dim RelDropped = From s In testTcpSever.ClientList.AsParallel
                                                                     Where s.IsConnected = False
                                                                            Select s.RecvId & Space(1) & s.CommCofigStr

                                                  Me.BeginInvoke(Function()
                                                                     Try
                                                                         Me.ListBox1.Items.Clear()
                                                                         Me.ListBox2.Items.Clear()
                                                                         Me.ListBox3.Items.Clear()
                                                                         Me.ListBox7.Items.Clear()


                                                                         IIf(RelOnline.ToArray.Count > 0, Sub()
                                                                                                              Me.ListBox1.Items.AddRange(RelOnline.ToArray)
                                                                                                          End Sub, Sub() Me.ListBox1.Items.Add("无")).Invoke()

                                                                         IIf(RelDropped.ToArray.Count > 0, Sub()
                                                                                                               Me.ListBox2.Items.AddRange(RelDropped.ToArray)
                                                                                                           End Sub, Sub() Me.ListBox2.Items.Add("无")).Invoke()

                                                                         Me.ListBox3.Items.Add("运行状态:" & testTcpSever.CommSeverState)
                                                                         Me.ListBox3.Items.Add("在线:" & RelOnline.Count)
                                                                         Me.ListBox3.Items.Add("掉线:" & RelDropped.Count)
                                                                         Me.ListBox3.Items.Add("---")
                                                                         Me.ListBox3.Items.Add("发送次数:" & testTcpSever.CountSend)
                                                                         Me.ListBox3.Items.Add("发送字节:" & testTcpSever.SumSend)
                                                                         Me.ListBox3.Items.Add("---")
                                                                         Me.ListBox3.Items.Add("接收次数:" & testTcpSever.CountRecv)
                                                                         Me.ListBox3.Items.Add("接收字节:" & testTcpSever.SumRecv)

                                                                         Dim RelLogSuccess = From s In testTcpSever.CommSeverLog.AsParallel
                                                                                                Where s.MsgType = Communication.CommPort.Log.CommSeverLogElement(Of String, TcpClientMember).MsgTypeModel.success
                                                                                                      Select s.CommSeverState, s.MsgDes, s.Msg

                                                                         Dim RelLogError = From s In testTcpSever.CommSeverLog.AsParallel
                                                                                               Where s.MsgType = Communication.CommPort.Log.CommSeverLogElement(Of String, TcpClientMember).MsgTypeModel.fail
                                                                                                      Select s.CommSeverState, s.MsgDes, s.Msg

                                                                         IIf(RelLogSuccess.ToArray.Count > 0, Sub()
                                                                                                                  Me.ListBox7.Items.Add("Success")
                                                                                                                  Me.ListBox7.Items.AddRange(RelLogSuccess.ToArray)
                                                                                                              End Sub, Sub() Me.ListBox7.Items.Add("Success")).Invoke()

                                                                         IIf(RelLogError.ToArray.Count > 0, Sub()
                                                                                                                Me.ListBox7.Items.Add("Error")
                                                                                                                Me.ListBox7.Items.AddRange(RelLogError.ToArray)
                                                                                                            End Sub, Sub() Me.ListBox7.Items.Add("Error")).Invoke()
                                                                     Catch ex As Exception

                                                                     End Try

                                                                 End Function)
                                              End Sub)


    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        testTcpSever.Start()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        testTcpSever.Pause()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        testTcpSever.Stop()
    End Sub

    Private Async Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Await GetSeverRel()
        'Dim i = Me.testTcpSever.CountRecv
    End Sub
    Private Async Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Await testMulConnect()
    End Sub

    Private Async Function testMulConnect() As Threading.Tasks.Task
        Await Run(Sub()
                      Dim ip As String = (From p In Communication.CommPort.eDNS.GetLocalAddr
                                                Where p.AddressFamily = Net.Sockets.AddressFamily.InterNetwork
                                                 Select p).FirstOrDefault.ToString

                      ip = Communication.CommPort.eDNS.GetAddr("gwtcn.vicp.net")(0).ToString
                      Dim cig As String = ip & "," & "80"

                      testTcpClients = New System.Collections.Concurrent.BlockingCollection(Of Communication.CommPort.eTcpClient)
                      Threading.ThreadPool.QueueUserWorkItem(Sub()
                                                                 System.Threading.Tasks.Parallel.For(1, New Random().Next(1000, 2000), Sub(index)
                                                                                                                                           Dim tmp As New Communication.CommPort.eTcpClient(cig)
                                                                                                                                           tmp.Connect()
                                                                                                                                           testTcpClients.Add(tmp)
                                                                                                                                       End Sub)
                                                             End Sub)
                  End Sub)

    End Function
    Private Async Function testMulSend() As Threading.Tasks.Task
        Await Run(Sub()
                      System.Threading.Tasks.Parallel.ForEach(testTcpClients, Sub(s As Communication.CommPort.eTcpClient)
                                                                                  If s.IsConnected Then
                                                                                      Dim romdom As New Random()
                                                                                      Dim TmpArr As Byte()
                                                                                      ReDim TmpArr(romdom.Next(5000, 10 * 1000))
                                                                                      s.Send(TmpArr)
                                                                                  End If
                                                                              End Sub)

                  End Sub)
    End Function

    Private Sub testMulDisconnect()
        System.Threading.Tasks.Parallel.ForEach(testTcpClients, Sub(s As Communication.CommPort.eTcpClient)
                                                                    Dim romdom As New Random()
                                                                    If romdom.Next(1, 300) Mod 2 = 0 Then
                                                                        s.DisConnect()
                                                                    End If
                                                                End Sub)
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        GetCliensRel()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        testMulDisconnect()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim tmpTask As New System.Threading.Tasks.Task(Sub()
                                                           testMulSend()
                                                       End Sub)
        tmpTask.Start()
    End Sub

    Private Async Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click

        '测试50次

        While True
            Await testMulConnect()
            Await testMulSend()
            '刷新信息
            Me.Invoke(Async Function()
                          'Await GetCliensRel()
                          'Await GetSeverRel()
                      End Function)

        End While

                                  
    End Sub
End Class
