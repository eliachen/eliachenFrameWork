
Imports System.Net.Sockets
Imports EliaChen
Imports EliaChen.Collection

Namespace CommPort
    Partial Public Class TcpSever
        Public Class TcpClientMember
            Inherits CommPort.TcpClient

            '接收编号
            Property RecvId As Integer

            '存储这个RecvSocket的状态
            Sub New(ByVal _RecvSocket As Socket)
                MyBase.New(_RecvSocket)
            End Sub


            Shadows connect()
            Shadows reconnect()


            '获取编号
            Private Shared Function GetId(ByVal _RecvList As BufferList(Of TcpClientMember)) As Integer
                If _RecvList.Count = 0 Then
                    Return 0
                Else
                    Return _RecvList.Max(Function(eState) As Integer
                                             Return eState.RecvId
                                         End Function) _
                                                    + 1
                End If
            End Function



        End Class
    End Class
End Namespace




