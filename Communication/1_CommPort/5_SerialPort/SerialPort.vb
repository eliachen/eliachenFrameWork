Imports System.IO.Ports
Imports System.Threading.Thread
Imports Communication.CommPort.ComPortStatics
Imports Communication.CommPort.ComPortWay

Namespace CommPort
    Public Class eSerialPort
        Inherits IO.Ports.SerialPort
        Implements ICommPort




        Public Property CommStatics As ICommPortStatitics Implements ICommPort.CommStatics

        Public Property CommPortWay As ICommPortWay Implements ICommPort.CommPortWay

        Public Property CommCofigStr As String Implements ICommPort.CommCofigStr

        Public Property CommLog As CommPort.Log.CommPortLogManager(Of String)


        Public ReadOnly Property IsConnected As Boolean Implements ICommPort.IsConnected
            Get
                Return MyBase.IsOpen
            End Get
        End Property

        Public ReadOnly Property CommPortType As String Implements ICommPort.CommPortType
            Get
                Return "SP"
            End Get
        End Property

        '发送处理：事件队列
        Private Delegate Sub mDataSended(ByVal _SendData As Byte())
        Private Event mEventSended As mDataSended

        ' 数据接收事件
        Public Event RecvDataEvent(ByRef sender As ICommPort, RecvArrByte() As Byte) Implements ICommPort.RecvDataEvent

        '非字符串配置方式
        Sub New()
            MyBase.New()
            '配置统计参数
            Me.CommStatics = IIf(Me.CommStatics IsNot Nothing, Me.CommStatics, _
                                New Model_BasicStatics)
            '配置接收模式
            Me.CommPortWay = IIf(Me.CommPortWay IsNot Nothing, Me.CommPortWay, _
                                 New Model_Delay With {.TimeDelaySend = 0, .TimeOutRecv = 150})

            '字符串配置
            Me.CommCofigStr = GetSpConfigStr(Me)

            '配置记录模式
            Me.CommLog = New CommPort.Log.CommPortLogManager(Of String)(Me)
        End Sub

        '字符串配置：COM1，9600，N，8，1
        Sub New(ByVal ConfigStr As String)
            Me.New()
            Me.CommCofigStr = CommCofigStr
            If String.IsNullOrEmpty(Me.CommCofigStr.Length) Then
                CommLog.LogError(Log.CommPortLogElement(Of String).MsgProcessDescriptionModel.optionset, _
                                  "串口字符串未配置")
            Else
                Try
                    InitialByConfigStr(Me)
                Catch ex As Exception
                    CommLog.LogError(Log.CommPortLogElement(Of String).MsgProcessDescriptionModel.optionset, _
                                  "串口字符串配置错误:" & ex.ToString)
                End Try
            End If
        End Sub


        Private Sub InitialByConfigStr(ByRef _OriSp As IO.Ports.SerialPort)
            Dim tmpConfigStr As String() = Me.CommCofigStr.Split(",")
            With _OriSp
                '串口号
                .PortName = tmpConfigStr(0)
                '波特率
                .BaudRate = Integer.Parse(tmpConfigStr(1))
                '校验方式
                Select Case tmpConfigStr(2).ToUpper
                    Case "N"
                        .Parity = IO.Ports.Parity.None
                    Case "E"
                        .Parity = IO.Ports.Parity.Even
                    Case "O"
                        .Parity = IO.Ports.Parity.Odd
                    Case "M"
                        .Parity = IO.Ports.Parity.Mark
                    Case "S"
                        .Parity = IO.Ports.Parity.Space
                End Select
                '数据位
                .DataBits = Integer.Parse(tmpConfigStr(3))
                '停止位
                .StopBits = Integer.Parse(tmpConfigStr(4))
            End With
        End Sub

        Private Function GetSpConfigStr(ByVal _OriSp As IO.Ports.SerialPort) As String
            Dim TmpStr As New Text.StringBuilder
            'COM X
            TmpStr.Append(_OriSp.PortName)
            TmpStr.Append("," & _OriSp.BaudRate)
            Select Case _OriSp.Parity
                Case IO.Ports.Parity.None
                    TmpStr.Append("," & "N")
                Case IO.Ports.Parity.Even
                    TmpStr.Append("," & "E")
                Case IO.Ports.Parity.Odd
                    TmpStr.Append("," & "O")
                Case IO.Ports.Parity.Mark
                    TmpStr.Append("," & "M")
                Case IO.Ports.Parity.Space
                    TmpStr.Append("," & "S")
            End Select
            TmpStr.Append("," & _OriSp.DataBits)
            TmpStr.Append("," & _OriSp.StopBits)
            Return TmpStr.ToString
        End Function

        '通讯建立:打开串口
        Public Overloads Sub Open() Implements ICommPort.Connect
            Try
                '串口在没打开情况下打开
                If Not MyBase.IsOpen Then
                    MyBase.Open()
                    CommLog.LogSuccess(Log.CommPortLogElement(Of String).MsgProcessDescriptionModel.connect, _
                                       "串口打开成功")

                End If
            Catch ex As Exception
                CommLog.LogError(Log.CommPortLogElement(Of String).MsgProcessDescriptionModel.connect, _
                                   "串口打开错误：" & ex.ToString)
            End Try
        End Sub

        '通讯结束:关闭串口
        Public Overloads Sub Close() Implements ICommPort.DisConnect
            Try
                If MyBase.IsOpen Then
                    MyBase.Close()
                    CommLog.LogSuccess(Log.CommPortLogElement(Of String).MsgProcessDescriptionModel.disconnect, _
                                      "串口关闭成功")
                End If
            Catch ex As Exception
                CommLog.LogError(Log.CommPortLogElement(Of String).MsgProcessDescriptionModel.disconnect, _
                                  "串口关闭错误：" & ex.ToString)
            End Try
        End Sub

        '通讯重连：关闭后再打开
        Public Sub ReConnect() Implements ICommPort.ReConnect
            Try
                Me.Close()
                Me.Open()
                CommLog.LogSuccess(Log.CommPortLogElement(Of String).MsgProcessDescriptionModel.reconnect, _
                                      "串口重开成功")
            Catch ex As Exception
                CommLog.LogError(Log.CommPortLogElement(Of String).MsgProcessDescriptionModel.reconnect, _
                                     "串口重开错误:" & ex.ToString)
            End Try
        End Sub

        '通讯接收触发：收到数据
        Private Sub SerialPortRecv(ByVal obj As Object, ByVal e As SerialDataReceivedEventArgs) Handles Me.DataReceived
            Try
                '检测串口状态
                If Me.IsOpen Then
                    '接收到的数据
                    Dim TmpArr As Byte() = Me.CommPortWay.CommDeal(Me)
                    If TmpArr.Count > 0 Then
                        RaiseEvent RecvDataEvent(obj, TmpArr)
                    End If
                End If
                '容错处理:直接退出,不再抛出接收的字节数组
            Catch ex As Exception
                CommLog.LogError(Log.CommPortLogElement(Of String).MsgProcessDescriptionModel.recv, _
                                     "串口接收错误:" & ex.ToString)
                Exit Sub
            End Try
        End Sub
#Region "发送"
        '发送数据,事件响应模型，避免多个共同占用
        Public Overloads Sub Write(ByVal _ArrByte As Byte()) Implements ICommPort.Send
            RaiseEvent mEventSended(_ArrByte)
        End Sub

        '串口发送响应
        Private Sub WriteResponse(ByVal _ArrByte As Byte()) Handles Me.mEventSended
            Try
                If Me.CommPortWay.TimeDelaySend = 0 Then
                    GoTo DirSend
                End If

                '延迟发送
                Sleep(Me.CommPortWay.TimeDelaySend)

                '检测串口状态并发送
DirSend:
                If Me.IsOpen Then
                    Using TmpDone As New Threading.ManualResetEvent(False)
                        TmpDone.Reset()

                        Me.BaseStream.BeginWrite(_ArrByte, 0, _ArrByte.Length, Sub(iar As IAsyncResult)
                                                                                   '统计：发送次数
                                                                                   Me.CommStatics.CountSend = Me.CommStatics.CountSend + 1
                                                                                   '统计：发送字节数
                                                                                   Me.CommStatics.SumSend = Me.CommStatics.SumSend + _ArrByte.Length
                                                                                   DirectCast(TmpDone, Threading.ManualResetEvent).Set()
                                                                               End Sub, TmpDone)
                        TmpDone.WaitOne()
                    End Using
                End If
            Catch ex As Exception
                CommLog.LogError(Log.CommPortLogElement(Of String).MsgProcessDescriptionModel.send, _
                                     "串口发送错误:" & ex.ToString)
            End Try
        End Sub
#End Region

#Region "接收"
        ''' <summary>
        ''' 接收:直接由当前缓存中取数据(没有数据会回一个Length=0的空字节数组)
        ''' </summary>
        ''' <returns></returns>
        Private Function Receive_Stra() As Byte() Implements ICommPort.Receive
            Try
                Dim RecvLen As Integer = MyBase.BytesToRead

                '临时数据数组
                If RecvLen > 0 Then
                    Dim DataArrByte As Byte() = New Byte(RecvLen - 1) {}
                    Dim TmpDone As New Threading.ManualResetEvent(False)
                    TmpDone.Reset()
                    MyBase.BaseStream.BeginRead(DataArrByte, 0, DataArrByte.Length, Sub(iar As IAsyncResult)
                                                                                        '统计：接收次数
                                                                                        Me.CommStatics.CountRecv = Me.CommStatics.CountRecv + 1
                                                                                        '统计：接收字节数
                                                                                        Me.CommStatics.SumRecv = Me.CommStatics.SumRecv + RecvLen
                                                                                        DirectCast(TmpDone, Threading.ManualResetEvent).Set()
                                                                                    End Sub, TmpDone)
                    TmpDone.WaitOne()
                    Return DataArrByte
                Else
                    Return New Byte() {}
                End If
            Catch ex As Exception
                'Throw New CommPortLogElement() With {.PortType = "SP", .PortErrMsg = "接收错误:" & ex.Message}
                Return New Byte() {}
            End Try
        End Function

        ''' <summary>
        ''' 接收:按照接口策略处理
        ''' </summary>
        ''' <param name="Way"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ReceiveAsWay(ByVal Way As ICommPortWay) As Byte() Implements ICommPort.ReceiveAsWay
            Return Way.CommDeal(Me)
        End Function
#End Region


    End Class
End Namespace

