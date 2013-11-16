Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.InteropServices
Imports System.Timers.Timer

Namespace CommPort
    Partial Public Class eTcpSever
        Implements ICommSever(Of TcpClientMember)
        Implements IDisposable

        '服务器Socket
        Private CommSeverSocket As Socket

        '服务器Socket状态:被动推送
        Public CommSeverSocketState As SocketError

        '服务器状态
        Public Property CommSeverState As ICommSever(Of TcpClientMember).StateModel = ICommSever(Of TcpClientMember).StateModel.stopped _
                                        Implements ICommSever(Of TcpClientMember).CommSeverState
        Public Property ClientList As Buffer.BufferList(Of TcpClientMember) _
                                            = New Buffer.BufferList(Of TcpClientMember) _
                                                 Implements ICommSever(Of TcpClientMember).CommClientList

        Public Property CommSeverConfigStr As String Implements ICommSever(Of TcpClientMember).CommConfigStr

        Public ReadOnly Property CommSeverType As String Implements ICommSever(Of TcpClientMember).CommSeverType
            Get
                Return "TcpServer"
            End Get
        End Property


        '服务器日志
        Public Property CommSeverLog As Log.CommSeverLogManager(Of String, TcpClientMember) _
                                                                            = New Log.CommSeverLogManager(Of String, TcpClientMember)(Me)

        '接收-事件
        Public Event RecvData(Client As TcpClientMember, RecvData() As Byte) Implements ICommSever(Of TcpClientMember).RecvData


        Sub New()
            MyBase.New()
        End Sub

        Sub New(ByRef _Sk As Socket)
            Me.CommSeverSocket = _Sk
            Me.CommSeverConfigStr = _Sk.LocalEndPoint.ToString.Replace(":", ",")

        End Sub

        Sub New(ByVal _CigStr As String)
            Me.New()
            Me.CommSeverConfigStr = _CigStr
            InitialByStr()
        End Sub

        Sub New(ByVal _Ip As String, ByVal _Port As Integer)
            Me.New()
            Me.CommSeverSocket = New Socket(New IPEndPoint(IPAddress.Parse(_Ip), _Port).AddressFamily, _
                                            SocketType.Stream, ProtocolType.Tcp)
            Me.CommSeverConfigStr = _Ip + "," + _Port.ToString
        End Sub
        Sub New(ByVal _IpAddress As IPAddress, ByVal _Port As Integer)
            Me.New()
            '建立对应类型的Socket,由IP地址自适应网络类型
            CommSeverSocket = New Socket(New IPEndPoint(_IpAddress, _Port).AddressFamily, SocketType.Stream, ProtocolType.Tcp)
            Me.CommSeverConfigStr = _IpAddress.ToString + "," + _Port.ToString

            '设置Socket的属性
            Configure_eTcpSeverSocket(Me.CommSeverSocket)
        End Sub


        '通过字符串初始化
        Private Sub InitialByStr()
            Me.CommSeverSocket = New Socket(New IPEndPoint(IPAddress.Parse(Me.CommSeverConfigStr.Split(",")(0)), Me.CommSeverConfigStr.Split(",")(1)).AddressFamily, SocketType.Stream, ProtocolType.Tcp)
        End Sub


        Public Sub FreshClientList() Implements ICommSever(Of TcpClientMember).FreshClientList

        End Sub



    End Class
End Namespace


