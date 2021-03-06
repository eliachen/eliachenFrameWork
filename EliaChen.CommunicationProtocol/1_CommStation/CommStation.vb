﻿Imports System.Threading
Imports EliaChen.CommPort
Namespace CommProtocol
    Public MustInherit Class CommStation(Of Tbuffer)

        '通讯的标识：唯一的编号
        Property CommId As Integer = 0

        WithEvents mCommPort As CommPort.ICommPort
        '通讯接口
        Public Property CommPort As EliaChen.CommPort.ICommPort
            Get
                Return mCommPort
            End Get
            Set(value As EliaChen.CommPort.ICommPort)
                mCommPort = value
            End Set
        End Property

        '通信延迟控制
        Private mCommDone As New ManualResetEvent(False)


        ' 通讯:接收信息缓存列表
        Public Property CommRecvBuffer As EliaChen.Collection.BufferList(Of Tbuffer) _
                                                                    = New EliaChen.Collection.BufferList(Of Tbuffer)


        '通信策略:参数设置
        Public Property CommWayConfig As CommStationConfig = New CommStationConfig()


        '通信统计:结果参数
        Public Property CommStatistics As CommStationStatistics = New CommStationStatistics()


        '数据校验
        Public MustOverride Function Validate(ByVal _ArrByte As Byte()) As Boolean
        '数据加缓存
        Public MustOverride Function Decode(ByVal _ArrByte As Byte()) As Tbuffer

        Sub New()
            MyBase.New()
        End Sub

        Sub New(ByVal _CommPort As ICommPort)
            Me.New()
            Me.CommPort = _CommPort
        End Sub

        Public Function SendMessage(ByVal _Msg As SendCommMessage, ByVal _Selector As IEnumerable(Of Tbuffer)) As IList(Of Tbuffer)
            '清空缓存
            Me.CommRecvBuffer.Clear()
            If _Msg.MessageType = SendCommMessage.MessageTypeModel.noAck Then
                Me.CommPort.Send(_Msg.MessageBody)
            ElseIf _Msg.MessageType = SendCommMessage.MessageTypeModel.Ack Then
                '错误重试
                For count = 1 To Me.CommWayConfig.MessageErrReTryCount
                    '记录:对话
                    Me.CommStatistics.CountTrip = +1
                    If Me.CommPort.IsConnected Then
                        Me.mCommDone.Reset()
                        Me.CommPort.Send(_Msg.MessageBody)
                        If Not (Me.mCommDone.WaitOne(Me.CommWayConfig.MessageTimeOut)) Then
                            '记录:对话超时次数
                            Me.CommStatistics.CountTripTimeOut = +1
                        Else
                            Dim Rel = _Selector.ToList()
                            If Rel.Count > 0 Then
                                '记录:对话成功
                                Me.CommStatistics.CountTripRight = +1
                                Return Rel
                            Else
                                '记录:对话失败
                                Me.CommStatistics.CountTripFalse = +1
                            End If
                        End If
                    End If
                Next
            End If
            '无结果Nothing
            Return Nothing
        End Function

        '无结果回调
        'Public Sub SendMessage(ByVal _Msg As SendCommMessage, ByVal _Fun As Action(Of IList(Of Tbuffer)))
        '    Dim CommList = SendMessage(_Msg, From s In Me.CommRecvBuffer
        '                                                            Select s)
        '    _Fun.Invoke(CommList)
        'End Sub

        '有结果的回调
        'Public Sub SendMessage(ByVal _Msg As SendCommMessage, ByVal _Fun As Func(Of IList(Of Tbuffer), Boolean))
        '    Dim CommList = SendMessage(_Msg, From s In Me.CommRecvBuffer
        '                                                        Select s)
        '    _Fun.Invoke(CommList)
        'End Sub


        '基于复杂对话

        Public Sub SendTrip(ByRef _Trip As CommTrip(Of Tbuffer))
            For Each msg In _Trip.ListMessageTask
                Dim rel = Me.SendMessage(msg.Item1, From s In Me.CommRecvBuffer
                                                                         Select s)
                '符合结果
                If msg.Item2(rel) Then
                    _Trip.TripRelList.AddRange(rel)
                Else
                    '流程处理
                    If _Trip.TripModel = CommTrip(Of Tbuffer).TripTypeModel.noContinue Then
                        '结果失败
                        _Trip.TripRel = False
                        '直接退出
                        Exit Sub
                    Else
                        Continue For
                    End If
                End If
            Next
            '所有结束，故对整个过程评价为真
            _Trip.TripRel = True
        End Sub

        Private Sub Recv(ByRef sender As EliaChen.CommPort.ICommPort, ByVal RecvArrByte As Byte()) Handles mCommPort.RecvDataEvent
            Try
                '统计:接收次数
                Me.CommStatistics.CountRecv = +1
                If Validate(RecvArrByte) Then
                    '统计
                    Me.CommStatistics.CountRecvRight = +1
                    '解码
                    Dim rel = Decode(RecvArrByte)
                    Dim recvMsg As RecvCommMessage = TryCast(rel, RecvCommMessage)
                    If recvMsg IsNot Nothing Then
                        '信息处理
                        Select Case recvMsg.MessageType
                            Case RecvCommMessage.MessageTypeModel.Cache_Ack
                                '加入缓存
                                Me.CommRecvBuffer.Add(rel)
                                '回应
                                Me.CommPort.Send(recvMsg.MessageResponse)
                            Case RecvCommMessage.MessageTypeModel.Cache_noAck
                                '加入缓存
                                Me.CommRecvBuffer.Add(rel)
                            Case RecvCommMessage.MessageTypeModel.noCache_Ack
                                Me.CommPort.Send(recvMsg.MessageResponse)
                            Case RecvCommMessage.MessageTypeModel.noCache_noAck
                                Exit Sub
                        End Select
                    End If

                Else
                    '统计
                    Me.CommStatistics.CountRecvFalse = +1
                End If
            Finally
                Me.mCommDone.Set()
            End Try
        End Sub

    End Class
End Namespace

