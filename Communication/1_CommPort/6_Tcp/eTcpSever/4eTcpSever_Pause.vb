Imports System.Net
Imports System.Net.Sockets
Imports System
Imports System.Threading.Tasks.Parallel
Namespace CommPort
    Partial Public Class eTcpSever
        Public Sub Pause() Implements ICommSever(Of TcpClientMember).Pause
            If Me.CommSeverState = ICommSever(Of TcpClientMember).StateModel.error Or _
                Me.CommSeverState = ICommSever(Of TcpClientMember).StateModel.running Then
                '记录

                Me.CommSeverState = ICommSever(Of TcpClientMember).StateModel.pause
            End If
        End Sub
    End Class
End Namespace

