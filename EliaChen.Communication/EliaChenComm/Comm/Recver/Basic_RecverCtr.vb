Imports System.Threading.Thread
Imports System.Threading

Namespace Communication.CommInterface.myCommWay
    Public MustInherit Class Basic_RecverElement
        Implements ICommRecver

        '通讯接口
        WithEvents _CommPort As CommPort.ICommPort
        '通讯接收缓存
        'Private _CommRecvBuffer As EliaChen.Buffer.SycList(Of ICommBufferElement)

        '用于处理过程的使用
        Private _CommDone As New ManualResetEvent(False)

        Public Property CommConfig As ICommWayConfig Implements ICommRecver.CommWayConfig

        Public Property CommStatistics As ICommWayStatistics Implements ICommRecver.CommStatistics

        Public Property CommId As Integer Implements ICommRecver.CommId

        Public Property CommParallelable As Boolean Implements ICommRecver.CommParallelable


        Public Property CommPort As CommPort.ICommPort Implements ICommRecver.CommPort
            Get
                Return _CommPort
            End Get
            Set(ByVal value As CommPort.ICommPort)
                _CommPort = value
            End Set
        End Property


        Public Property CommRecvBuffer As EliachenFw.Buffer.IBufferList(Of ICommBufferElement) Implements ICommRecver.CommRecvBuffer


        '收到数据的处理:接口传入
        Public Sub CommRecvData(ByVal ArrByte() As Byte) Handles _CommPort.RecvDataEvent
            Try
                ''统计：接收次数
                '_CommStatistics.CountRecv = _CommStatistics.CountRecv + 1

                '处理
                If Excute(ArrByte) Then
                    '统计:收到符合格式的数据次数
                    _CommStatistics.CountRecvRight = _CommStatistics.CountRecvRight + 1
                    _CommStatistics.LastTimeRecv = Now
                End If

            Catch
                '统计：软件运行错误次数
                _CommStatistics.CountAppErr = _CommStatistics.CountAppErr + 1
            Finally
                _CommDone.Set()
            End Try
        End Sub


        '接收到的处理函数
        Public Overridable Function Excute(ByVal ArrByte() As Byte) As Boolean Implements ICommRecver.Excute
            Return True
        End Function

        '通讯读取
        Public Function CommOrder_Read(ByVal _Order As ICommOrder) As Boolean Implements ICommRecver.CommOrder_Read
            ''前先清空缓存
            _CommRecvBuffer.Clear()

            Dim boolTimeOut As Boolean = False

            '记录:命令下发时间
            Me.CommStatistics.LastTimeSend = Now

            '设置：读取失败次数
            For count = 1 To _CommConfig.CountReadFailReTry
                Try
                    '上锁
                    _CommDone.Reset()
                    '统计：命令发送次数
                    Me._CommStatistics.CountSend = Me._CommStatistics.CountSend + 1
                    '发送读命令
                    _CommPort.Send(_Order.OrderRead)
                    '等待命令反馈:超时或者延时的反馈
                    If Not _CommDone.WaitOne(_CommConfig.TimeOrderOut) Then '统计超时
                        _CommStatistics.CountRecvTimeOut = _CommStatistics.CountRecvTimeOut + 1
                        '///超时处理///
                        _Order.OrderWhen_TimeOut()
                        boolTimeOut = True
                    End If

                    '返回结果
                    If _Order.OrderCheck(_CommRecvBuffer) Then
                        '统计：通讯正确
                        _CommStatistics.CountRecvActive = _CommStatistics.CountRecvActive + 1
                        '统计：通信正确时间
                        _CommStatistics.LastTimeReadActive = Now
                        Return True
                    Else

                        '统计:重试次数
                        _CommStatistics.CountRetrie = _CommStatistics.CountRetrie + 1
                        ' ///逻辑层错误处理:单次///
                        _Order.OrderWhen_OrderErr_Each()
                        '到了次数且命令检查结果错误
                        If count = _CommConfig.CountReadFailReTry Then
                            ' ///逻辑层错误处理:最终///
                            _Order.OrderWhen_OrderErr_Final()
                            _CommPort.Send(_Order.OrderRead)
                            Return False
                        End If
                    End If
                Catch
                    '统计：错误次数,指程序错误
                    _CommStatistics.CountAppErr = _CommStatistics.CountAppErr + 1
                    Return False
                End Try
            Next

            Return False
        End Function
        '通讯下发
        Public Function CommOrder_Write(ByVal _Order As ICommOrder) As Boolean Implements ICommRecver.CommOrder_Write
            ''前先清空缓存
            '_CommRecvBuffer.sycClear()

            ''按照设置的读失败重试次数
            'For count = 1 To _CommConfig.CountWriteFailReTry
            '    Try
            '        '上锁
            '        _CommDone.Reset()
            '        '发送写命令
            '        _CommPort.Send(_Order.OrderWrite)
            '        '等待命令反馈:超时或者延时的反馈
            '        If Not _CommDone.WaitOne(_CommConfig.TimeOrder) Then '统计超时
            '            _CommStatistics.CountTimeOut = _CommStatistics.CountTimeOut + 1
            '            '处理:超时处理函数
            '            Comm_WhenErr_Read_OverTime()
            '        End If

            '        '返回结果
            '        If _Order.OrderCheck(_CommRecvBuffer) Then
            '            '统计：通讯正确
            '            _CommStatistics.CountActive = _CommStatistics.CountActive + 1
            '            Return True
            '        Else
            '            '处理：错误接收
            '            Comm_WhenErr_Read_ErrRecv()
            '            '到了次数且命令检查结果错误
            '            If count = _CommConfig.CountWriteFailReTry Then
            '                Return False
            '            End If
            '        End If
            '        '统计:重试次数
            '        _CommStatistics.CountRetrie = _CommStatistics.CountRetrie + 1
            '    Catch
            '        '统计：错误次数,指程序错误
            '        _CommStatistics.CountAppErr = _CommStatistics.CountAppErr + 1
            '    End Try
            'Next
            'Return False
        End Function



#Region "通讯容错性"
        Public Overridable Function Comm_WhenErr_Read_OverTime() As Object Implements ICommRecver.Comm_WhenErr_Read_OverTime
            Return Nothing
        End Function

        Public Overridable Function Comm_WhenErr_Read_ErrRecv() As Object Implements ICommRecver.Comm_WhenErr_Read_ErrRecv
            Return Nothing
        End Function
#End Region

#Region "通讯控制"
        Public Overridable Function CommInitial() As Object Implements ICommRecver.CommInitial
            Return Nothing
        End Function

        Public Overridable Function CommReset() As Object Implements ICommRecver.CommReset
            Return Nothing
        End Function

        Public Overridable Function CommStop() As Object Implements ICommRecver.CommDispose
            Return Nothing
        End Function

#End Region

    End Class
End Namespace

