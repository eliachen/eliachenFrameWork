Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.InteropServices
Imports System.Timers.Timer

Namespace CommPort
    Partial Public Class TcpClient
        Implements ICommPort



        '当前的连接状态
        Public Property ActivatedSocketState As SocketError


        Public ReadOnly Property IsConnected As Boolean Implements ICommPort.IsConnected
            Get
                Try
                    If Not Me.CommSocket.Connected Then
                        Return False
                    Else
                        Return IIf(CheckState() = SocketError.Success, True, False)
                    End If
                Catch ex As SocketException
                    If Not Me.CommSocket.Connected Then
                        Return False
                    Else
                        Return IIf(ex.ErrorCode = SocketError.Success, True, False)
                    End If
                End Try
            End Get
        End Property

        '接收-事件
        Public Event RecvDataEvent(ByRef sender As ICommPort, RecvArrByte() As Byte) Implements ICommPort.RecvDataEvent

        '发送-事件（私有）
        Private Delegate Sub DelgSend(ByRef obj As Object, ByVal Arrbyte As Byte())
        Private Event mEventSend As DelgSend

        Public Property CommCofigStr As String Implements ICommPort.CommCofigStr

        Public Property CommPortWay As ICommPortWay = _
                                                        New CommPort.ComPortWay.Model_Straight _
                                                          Implements ICommPort.CommPortWay

        Public Property CommStatics As ICommPortStatitics = _
                                                             New CommPort.ComPortStatics.Model_BasicStatics _
                                                               Implements ICommPort.CommStatics

        Public Property CommLog As Log.CommPortLogManager(Of TcpClientLog) = _
                                                                New Log.CommPortLogManager(Of TcpClientLog)(Me)


        Public Property CommSocket As Net.Sockets.Socket


        Public ReadOnly Property CommPortType As String Implements ICommPort.CommPortType
            Get
                Return "TcpC"
            End Get
        End Property
        Sub New()
            MyBase.New()
        End Sub

        Sub New(ByVal adFamily As AddressFamily)
            Me.CommSocket = New Socket(adFamily, SocketType.Stream, ProtocolType.Tcp)
        End Sub
        '直接配置形式
        Sub New(ByVal IpStr As String, ByVal Port As Integer)
            Me.New(New IPEndPoint(IPAddress.Parse(IpStr), Port).AddressFamily)
            Me.CommCofigStr = IpStr & "," & Port.ToString
        End Sub

        '字符串形式：127.0.0.1,8080
        Sub New(ByVal ConfigStr As String)
            Me.New(ConfigStr.Split(",")(0), ConfigStr.Split(",")(1))
            Me.CommCofigStr = ConfigStr
            If String.IsNullOrEmpty(Me.CommCofigStr) Then
                CommLog.LogError(Log.CommPortLogElement(Of TcpClientLog).MsgProcessDescriptionModel.optionset,
                                 New TcpClientLog(Me.ActivatedSocketState, "字符串配置"))

                Exit Sub
            End If
        End Sub

        Sub New(ByRef _Sk As Net.Sockets.Socket)
            Me.CommSocket = _Sk
            Me.CommCofigStr = _Sk.RemoteEndPoint.ToString
        End Sub


        '字符串形式的IP地址，兼容IPv4与IPv6
        Private Sub SkInitial()
            Try

                '设置Socket的属性,需要在一定时间内保活
                Dim dummy As UInteger = 0
                '设置属性
                Dim inOptionValues As Byte() = New Byte(Marshal.SizeOf(dummy) * 3 - 1) {}

                '启用Keep-Alive
                BitConverter.GetBytes(CUInt(1)).CopyTo(inOptionValues, 0)
                '第一次探测的时间：5000ms
                BitConverter.GetBytes(CUInt(Config_KeepaLiveTime)).CopyTo(inOptionValues, Marshal.SizeOf(dummy))
                '时间间隔:5000ms
                BitConverter.GetBytes(CUInt(Config_KeepaLiveInterval)).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2)

                Dim ouOptionValues As Byte() = New Byte(Marshal.SizeOf(dummy) * 3 - 1) {}

                Me.CommSocket.IOControl(IOControlCode.KeepAliveValues, inOptionValues, ouOptionValues)

                '绑定本地端口
                'eTcpClient.Bind(New IPEndPoint(IPAddress.Parse("0.0.0.0"), Now.Millisecond))
                CommLog.LogSuccess(Log.CommPortLogElement(Of TcpClientLog).MsgProcessDescriptionModel.optionset,
                                  New TcpClientLog(Me.ActivatedSocketState, "保活设置"))
            Catch exSk As SocketException
                CommLog.LogError(Log.CommPortLogElement(Of TcpClientLog).MsgProcessDescriptionModel.optionset,
                                 New TcpClientLog(exSk.SocketErrorCode, "保活设置"))
            Catch ex As Exception
                CommLog.LogError(Log.CommPortLogElement(Of TcpClientLog).MsgProcessDescriptionModel.optionset,
                                 New TcpClientLog(Me.ActivatedSocketState, "保活设置:" & ex.ToString))

            End Try
        End Sub


#Region "状态检查"
        Private Function CheckState() As SocketError
            Dim listBool As New List(Of Boolean)

            listBool.AddRange(New Boolean() {Me.CommSocket.Poll(500, SelectMode.SelectError), _
                                             Me.CommSocket.Poll(500, SelectMode.SelectRead), _
                                             Me.CommSocket.Poll(500, SelectMode.SelectWrite)})

            'Dim ias As Byte() = New Byte() {0, 1, 2}

            'Dim x As Boolean() = New Boolean() {False}

            Select Case True
                'FFT:连接正常,无数据
                Case listBool(0) = False And listBool(1) = False And listBool(2) = True
                    Return SocketError.Success
                    'FTT:已经断开
                    'Case listBool(0) = False And listBool(1) = True And listBool(1) = True
                    '    Return SocketError.ConnectionAborted
                Case Else
                    Return SocketError.ConnectionAborted
            End Select

        End Function

#End Region


        Public Overloads Sub Connect() Implements ICommPort.Connect
            '基本配置
            SkInitial()
            '连接成功 -> 进行接收数据
            If Me.eConnect() = SocketError.Success Then
                CirclRecv()
            End If
        End Sub

        Public Overloads Sub DisConnect() Implements ICommPort.DisConnect
            eDisConnect()
        End Sub

        Public Sub ReConnect() Implements ICommPort.ReConnect
            If eDisConnect() = SocketError.Success Then
                '记录：
                CommLog.LogSuccess(Log.CommPortLogElement(Of TcpClientLog).MsgProcessDescriptionModel.reconnect,
                                   New TcpClientLog(Me.ActivatedSocketState, "断开连接"))
                Me.Connect()
            Else
                CommLog.LogError(Log.CommPortLogElement(Of TcpClientLog).MsgProcessDescriptionModel.reconnect,
                                    New TcpClientLog(Me.ActivatedSocketState, "断开连接"))
            End If
        End Sub






    End Class
End Namespace

