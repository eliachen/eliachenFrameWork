Namespace Communication.Proj.Comm_IEC103
    Partial Public Class CommIEC103
        Inherits Communication.CommInterface.Basic_Comm
        Partial Public Class Recver_Station
            '设置：通信接口模型--延迟模型
            Private Class CommPortWayConfigFor103
                Inherits Communication.CommPort.ComPortWay.Model_Delay
                Sub New()
                    Me.TimeDelaySend = 0
                    Me.TimeOutRecv = 250
                End Sub
            End Class
            '设置：通信参数模型
            Private Class CommWayConfigFor103
                Inherits Communication.CommInterface.myCommWay.Basic_WayConfig
                Sub New()
                    '重读次数
                    Me.CountReadFailReTry = 5
                    '命令延迟时间
                    Me.TimeOrder = 500
                    '轮训时间设置
                    Me.TimeScan = 0
                End Sub
            End Class
        End Class

        '103自定义设置
        Private Class CommPriConfig103
            'ACD双FCB处理
            Public Shared AcdDouble As Integer = 150
            '检测缓冲区ACD=0的命令驱动次数
            Public Shared AcdSet0Count As Integer = 5
        End Class

        Public Class CommConfig

            Public TimeDelaySend As Integer
            Public TimeOutRecv As Integer
            Public CountReadFailReTry As Integer
            Public TimeOrder As Integer
            Public TimeScan As Integer

            Public Sub New(ByVal _TimeDelaySend As Integer, ByVal _TimeOutRecv As Integer, _
                            ByVal _CountReadFailReTry As Integer, ByVal _TimeOrder As Integer, _
                            ByVal _TimeScan As Integer)


                TimeDelaySend = _TimeDelaySend
                TimeOutRecv = _TimeOutRecv
                CountReadFailReTry = _CountReadFailReTry
                TimeOrder = _TimeOrder
                TimeScan = _TimeScan
            End Sub

        End Class
    End Class
End Namespace