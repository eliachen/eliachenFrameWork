Imports EliachenFw.Comm.Proj.Comm_IEC103.Code.BasicCode
Imports EliachenFw.Comm.Proj.Comm_IEC103.Code.AdvCode
Imports EliachenFw.Comm.Proj.Comm_IEC103.Code.AdvCode.DeCode_AsduInf
Imports EliachenFw.Comm.Proj.Comm_IEC103.Code.AdvCode.DeCode
Imports EliachenFw.Comm.Proj.Comm_IEC103.Code.Others
Imports EliachenFw.Comm.Proj.Comm_IEC103.Order
Imports EliachenFw.Comm.Proj.Comm_IEC103
Imports System.Threading.Thread
Imports System.Threading

Namespace Communication.Proj.Comm_IEC103
    Partial Public Class CommIEC103
        Inherits Communication.CommInterface.Basic_Comm
        Public Class Recver_Station
            Inherits Communication.CommInterface.myCommWay.Basic_RecverElement
            '103通信参数设置
            Public DevState As Comm103DevState
            '103通信规则设置

            'No1:初始化每个设备
            Sub New(ByVal _CommPort As Communication.CommPort.ICommPort, ByVal _DevS As Comm103DevState, ByVal _CommConfig As CommConfig)
                'Set1：通信接口设置
                Me.CommPort = _CommPort
                'Set2:通信接口方法设置
                Me.CommPort.CommPortWay = New CommPortWayConfigFor103()
                Me.CommPort.CommPortWay.TimeDelaySend = _CommConfig.TimeDelaySend
                Me.CommPort.CommPortWay.TimeOutRecv = _CommConfig.TimeOutRecv
                'Set3：——通信设备属性设置
                Me.DevState = _DevS
                'Set4：通信参数设置
                Me.CommConfig = New CommWayConfigFor103()
                Me.CommConfig.TimeOrderOut = _CommConfig.TimeOrder
                Me.CommConfig.TimeScan = _CommConfig.TimeScan
                Me.CommConfig.CountReadFailReTry = _CommConfig.CountReadFailReTry
                'Set5:通信统计
                Me.CommStatistics = New CommStaticsFor103
                'Set6:通信缓存-新建
                Me.CommRecvBuffer = New EliachenFw.Buffer.BufferList(Of Comm.CommInterface.ICommBufferElement)

            End Sub


            Sub New(ByVal _CommPort As Communication.CommPort.ICommPort, ByVal _DevS As Comm103DevState)
                'Set1：通信接口设置
                Me.CommPort = _CommPort
                'Set2:通信接口方法设置
                Me.CommPort.CommPortWay = New CommPortWayConfigFor103()
                'Set3：——通信设备属性设置
                Me.DevState = _DevS
                'Set4：通信参数设置
                Me.CommConfig = New CommWayConfigFor103()
                'Set5:通信统计
                Me.CommStatistics = New CommStaticsFor103
                'Set6:通信缓存-新建
                Me.CommRecvBuffer = New EliachenFw.Buffer.BufferList(Of Comm.CommInterface.ICommBufferElement)
            End Sub

            'NO2:接收处理的函数
            Public Overrides Function Excute(ByVal _ArrByte() As Byte) As Boolean

                Dim RecvList As New List(Of Byte)
                RecvList.AddRange(_ArrByte)
                Try
                    While RecvList.Count > 0
                        '头部
                        Dim Head As Byte = RecvList(0)

                        If Head = &H10 Then
                            ReDim _ArrByte(4)
                            Array.Copy(RecvList.ToArray, 0, _ArrByte, 0, _ArrByte.Length)
                            RecvList.RemoveRange(0, 5)
                            Dim recv_sf As New Recv_StableFrame
                            If recv_sf.CheckBufferObj(_ArrByte) Then
                                Dim tmp As Recv_StableFrame = recv_sf.GetBufferObj(_ArrByte)
                                '地址匹配加入缓存
                                If tmp.Addr = Me.DevState.Addr Then
                                    Me.CommRecvBuffer.add(tmp)
                                    ExcuteCode(recv_sf.nCode)
                                    'Ex统计：正确接收（编码）
                                    Me.CommStatistics.CountRecvRight = Me.CommStatistics.CountRecvRight + 1
                                    Me.CommStatistics.LastTimeRecv = Now
                                    'Return True
                                End If
                            End If
                        ElseIf Head = &H68 Then
                            Dim len As Byte = RecvList(1) + 6
                            ReDim _ArrByte(len - 1)
                            Array.Copy(RecvList.ToArray, 0, _ArrByte, 0, _ArrByte.Length)
                            RecvList.RemoveRange(0, len)
                            Dim recv_usf As New Recv_unStableFrame
                            If recv_usf.CheckBufferObj(_ArrByte) Then
                                Dim tmp As Recv_unStableFrame = recv_usf.GetBufferObj(_ArrByte)
                                '地址匹配加入缓存
                                If tmp.Addr = Me.DevState.Addr Then
                                    Me.CommRecvBuffer.add(recv_usf.GetBufferObj(_ArrByte))
                                    ExcuteCode(recv_usf.nCode)
                                    'Ex统计：正确接收（编码）
                                    Me.CommStatistics.CountRecvRight = Me.CommStatistics.CountRecvRight + 1
                                    Me.CommStatistics.LastTimeRecv = Now
                                    'Return True
                                End If
                            End If
                        Else
                            '无法判断的头部就全部清空
                            RecvList.Clear()
                        End If
                    End While
                Catch
                    Return False
                End Try
                Return False
            End Function

            'NO3:处理后的分支
            Private Sub ExcuteCode(ByVal _RecCode As CodeStS)
                If _RecCode.ACD = 1 Then  '循环召唤一级数据//////////////////////////
                    Me.CommPort.Send(AskDataOne(DevState.Addr, DevState.Fcb))
                    System.Threading.Thread.Sleep(CommPriConfig103.AcdDouble)
                    Me.CommPort.Send(AskDataOne(DevState.Addr, DevState.exFcb))
                End If
            End Sub


            'NO5:维护针对103的基本命令模式:发送命令，等待结果
            Private Function Com103Order(ByVal ArrByte As Byte()) As Boolean
                Try
                    If Me.CommOrder_Read(New BasicOrderDeal(ArrByte, DevState)) Then
                        '命令成功后等待过程结束
                        'No1:先清空不必要的不定长帧ACD=0的项
                        Me.CommRecvBuffer.RemoveAll(Function(e) As Boolean
                                                        If TypeOf e Is Recv_StableFrame Then
                                                            If CType(e, Recv_StableFrame).nCode.ACD = 0 Then
                                                                Return True
                                                            End If
                                                        End If
                                                        Return False
                                                    End Function)

                        'No2:开始检测直到命令中出现ACD=0,表明一级数据召唤完全
                        Dim TmpTimer As New System.Timers.Timer(Me.CommConfig.TimeOrderOut)
                        Dim Tmpalldone As New ManualResetEvent(False)
                        Dim TmpTimeStart As Date = Now

                        '/////SET/////
                        Dim TmpCount As Integer = CommPriConfig103.AcdSet0Count

                        Dim list As New List(Of Integer)


                        AddHandler TmpTimer.Elapsed, Sub(obj As Object, e As Timers.ElapsedEventArgs)

                                                         Dim Rel0 = From s In Me.CommRecvBuffer
                                                                       Where TypeOf s Is Recv_StableFrame
                                                                         Let ex = TryCast(s, Recv_StableFrame)
                                                                                            Where ex.nCode.ACD = 0
                                                                                                     Select ex


                                                         Rel0 = Rel0.ToList

                                                         If Rel0.ToArray.Length > 0 Or TmpCount > CommIEC103.CommPriConfig103.AcdSet0Count Then    '查找结果
                                                             Tmpalldone.Set()
                                                         Else
                                                             Try
                                                                 Dim tmp As Integer = (Now - Me.CommStatistics.LastTimeRecv).TotalMilliseconds
                                                                 '/////SET//////最长命令延迟时间
                                                                 If tmp > 2000 Then
                                                                     TmpCount = TmpCount + 1
                                                                 End If
                                                                 Me.CommPort.Send(AskDataOne(DevState.Addr, DevState.exFcb))
                                                             Catch ex As Exception

                                                             End Try

                                                         End If
                                                     End Sub

                        TmpTimer.Start()
                        '停等延迟
                        Tmpalldone.WaitOne()
                        '结束后释放资源
                        TmpTimer.Stop()

                        Return True
                    Else
                        Return False
                    End If
                Catch ex As Exception
                    Return False
                End Try
            End Function

            'NO6:发送命令
            '通信初始化：103-通信复位
            Public Overrides Function CommInitial() As Object
                '当前已经收到命令
                If Com103Order(Order.CommReset(DevState.Addr, DevState.exFcb)) Then
                    Try
                        Dim Rel = From s In Me.CommRecvBuffer
                                                               Where TypeOf s Is Recv_unStableFrame
                                                                 Let ex = TryCast(s, Recv_unStableFrame)
                                                                    Where ex.nASDU.Cot = 4
                                                                      Let CodeRel = DeCode_Asdu_5(ex.nASDU)
                                                                        Select New With {.DevName = CodeRel.AscIIStr,
                                                                                         .Edition = CodeRel.Edition} Distinct

                        If Rel.ToArray.Count > 0 Then

                            '不更新设备的信息，只是获取
                            ''设备名称
                            'DevState.Name = Rel.ToList.First.DevName
                            ''版本号
                            'DevState.Edition = Rel.ToList.First.Edition

                            Return New With {.bool = True, .Rel = Rel.ToList.First}

                        Else
                            Return New With {.bool = False, .Rel = Nothing}
                        End If

                    Catch ex As Exception
                        Throw ex
                        Return New With {.bool = False, .Rel = Nothing}
                    End Try
                Else
                    Return New With {.bool = False, .Rel = Nothing}
                End If
            End Function
            '通信重置：103-通信复位
            Public Overrides Function CommReset() As Object
                Return CommInitial()
            End Function
            '通信错误处理_通讯延时： 更改参数
            Public Overrides Function Comm_WhenErr_Read_OverTime() As Object
                '////SET////更改通讯的参数来保证通讯的高效
                Me.CommConfig.TimeOrderOut = Me.CommConfig.TimeOrderOut
                Return Nothing
            End Function
            '通讯错误处理_错误接收: 不做处理
            Public Overrides Function Comm_WhenErr_Read_ErrRecv() As Object
                Return MyBase.Comm_WhenErr_Read_ErrRecv()
            End Function

            'NO7:自定义的命令
            '通信：103-对时
            Public Function CommSetTime() As Object
                If Com103Order(Order.AskTimeSyn(DevState.Addr, DevState.exFcb)) Then
                    Return New With {.bool = True, .Rel = Nothing}
                Else
                    Return New With {.bool = False, .Rel = Nothing}
                End If
            End Function

            '通信：103-总召唤(K)
            Public Function CommCall_TotalData() As Object
                If Com103Order(AskTotally(DevState.Addr, DevState.exFcb, 1)) Then
                    'step1:确定总查询结束
                    Dim RelOver = From s In Me.GetUnFrameList
                                     Where s.nASDU.Type = 8 And s.nASDU.Cot = 10 And s.nASDU.Comaddr = 1
                                          Select s


                    If RelOver.ToArray.Length > 0 Then
                        'step2:读取总查询的值
                        Dim RelData = From s In Me.GetUnFrameList
                                     Where s.nASDU.Type = 1 And s.nASDU.Cot = 9 And s.nASDU.Comaddr = 1
                                         Let ex = DeCode_Asdu_1(s.nASDU)
                                            Select ex

                        Return New With {.bool = True, .Rel = RelData.ToList}
                    End If
                End If
                Return New With {.bool = True, .Rel = Nothing}
            End Function

            '通信：103-召唤二级数据
            Public Function CommCall_TwoData() As Object

                '二级数据不采取稳定处理，只要有就更新
                Com103Order(AskDataTwo(DevState.Addr, DevState.exFcb))
                Dim RelData = From s In GetUnFrameList()
                                Where s.nASDU.Type = 9 Or s.nASDU.Type = 50
                                   Group By TmpR = s.nASDU.Type Into TmpGrp = Group
                                      Order By TmpGrp(0).nASDU.Type Ascending
                                                Select r = DeCode_Asdu_9_50(TmpGrp(0).nASDU, GetType(Int16))

                If RelData.ToArray.Length > 0 Then
                    '获取所有的遥测整数
                    Dim ListVal As New List(Of Int16)
                    For Each ea In RelData
                        For Each ea2 In ea.ListValue
                            ListVal.Add(ea2.Value)
                        Next
                    Next
                    Return New With {.bool = True, .Rel = ListVal}
                End If
                'End If
                Return New With {.bool = False, .Rel = Nothing}
            End Function

            '通信:103-召唤遥测数据
            Public Function CommCall_Tele(Optional ByVal Sectors As Byte = 1) As Object
                If Com103Order(AskFarComm(DevState.Addr, DevState.exFcb, Sectors)) Then
                    'Step1:确定遥测结束
                    Dim Rel = From s In GetUnFrameList()
                                   Where s.nASDU.Type = 93 Or s.nASDU.Cot = 10
                                      Select s

                    If Rel.ToArray.Length > 0 Then
                        Dim RelData = From s In GetUnFrameList()
                                           Where s.nASDU.Type = 50 Or s.nASDU.Cot = 64
                                                                  Select s

                        Dim tmpList As New List(Of Object)

                        '处理数据
                        If RelData.ToArray.Count > 0 Then
                            Dim x As Asdu_9_50_Inf = DeCode_Asdu_9_50(RelData(0).nASDU, GetType(Int16))
                            For Each ea In x.ListValue
                                tmpList.Add(ea.Value)
                            Next
                            Return New With {.bool = True, .Rel = tmpList}
                        End If
                    End If
                End If
                Return New With {.bool = False, .Rel = Nothing}
            End Function

            '通信：103-召唤电度
            Public Function CommCall_ElecQ(ByVal Sectors As Byte, ByVal Fun As Byte, Optional ByVal Order As Byte = &H15) As Object
                'Order = &H15
                'Order = &H85
                If Com103Order(Order_AskEleMeasure(DevState.Addr, DevState.exFcb, Sectors, Fun, Order)) Then
                    'Step1:确认信息
                    Dim Rel = From s In GetUnFrameList()
                                 Where s.nASDU.Type = &H58 And s.nASDU.Cot = 2
                                    Select s

                    If Rel.ToArray.Length > 0 Then
                        'Step2:解码数据
                        Dim RelData = From s In GetUnFrameList()
                                           Where s.nASDU.Type = 36 And s.nASDU.InfData.Length > 4
                                               Let data = DeCode_Asdu_36(s.nASDU)
                                                  Select data

                        Return New With {.bool = True, .Rel = RelData.ToList}
                    End If

                End If
                Return New With {.bool = False, .Rel = Nothing}
            End Function

            '通信:103-通用分类服务_电量遥测
            Public Function CommCall_ElcCharge(Optional ByVal GrouNum As Byte = 9, Optional ByVal Sectors As Byte = 1) As Object
                '获取


                If Com103Order(Order_AskComm(DevState.Addr, DevState.exFcb, Sectors, GrouNum)) Then

                    'Dim x = Me.CommRecvBuffer.SycList

                    'Dim Rel1 = From s In GetUnFrameList()
                    '              Where s.nASDU.Type = 10
                    '                Let data = DeCode_Asdu_10(s.nASDU)
                    '                 Select data

                    Dim Rel = From s In GetUnFrameList()
                                  Where s.nASDU.Type = 10
                                     Let data = DeCode_Asdu_10(s.nASDU)
                                       Order By data.NGDSum Descending
                                         Group By data.NGDSum Into GrpData = Group
                                           Select R = New With {.tmpRel = GrpData(0).data}
                                                 From p In R.tmpRel.ListValue
                                                    Where p.ValArrByte IsNot Nothing
                                                        Order By p.GinGroupIndex
                                                            Select p

                    Rel = Rel.ToList

                    Dim Relx = From s In Rel
                                 Group By gindex = s.GinGroupIndex Into grp = Group
                                     Order By gindex
                                                Select grp(0)






                    '必须要满足有起始位1
                    If Relx(0).GinGroupIndex = 1 Then
                        '对应数据类型进行转换成对应类型
                        Dim ExList As New List(Of Object)
                        For Each eData In Relx
                            ExList.Add(BitConverter.ToSingle(eData.ValArrByte, 0))
                        Next
                        Return New With {.bool = True, .Rel = ExList}

                    End If

                End If

                Return New With {.bool = False, .Rel = Nothing}
                'Dim r1 = From s In ListAsdu10
                '                    Order By s.NGDSum Descending
                '                       Select s, s.NGDSum Distinct
                '                        Order By s.ListValue(0).GinGroupIndex

            End Function

            '通信:103-通用分类服务_非电量遥测
            Public Function CommCall_unElcCharge(Optional ByVal GrouNum As Byte = 9, Optional ByVal Sectors As Byte = 1) As Object
                Return CommCall_ElcCharge(&HD, 1)
            End Function


#Region "缓存辅助"
            '辅助：获取不定帧列表
            Private Function GetUnFrameList() As List(Of Recv_unStableFrame)
                Dim Rel = From s In Me.CommRecvBuffer
                             Where TypeOf s Is Recv_unStableFrame
                                 Let ex = TryCast(s, Recv_unStableFrame)
                                  Where ex.Addr = DevState.Addr
                                    Select ex
                'DebugErr:集合已经改变
                Return Rel.ToList
            End Function

            '辅助：获取定帧列表
            Private Function GetFrameList() As List(Of Recv_StableFrame)
                Dim Rel = From s In Me.CommRecvBuffer
                            Where TypeOf s Is Recv_StableFrame
                              Let ex = TryCast(s, Recv_StableFrame)
                                Where ex.Addr = DevState.Addr
                                Select ex

                Return Rel.ToList
            End Function
#End Region




        End Class
    End Class
End Namespace

