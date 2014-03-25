Namespace Communication.CommInterface.myCommWay
    Public Class Basic_WayStatistics
        Implements ICommWayStatistics

        Public Property CountActive As Integer Implements ICommWayStatistics.CountRecvActive
        Public Property CountAppErr As Integer Implements ICommWayStatistics.CountAppErr
        Public Property CountConn As Integer Implements ICommWayStatistics.CountConn
        Public Property CountData As Integer Implements ICommWayStatistics.CountData
        Public Property CountOverruns As Integer Implements ICommWayStatistics.CountOverruns
        Public Property CountRecv As Integer Implements ICommWayStatistics.CountRecvRight
        Public Property CountRetrie As Integer Implements ICommWayStatistics.CountRetrie
        Public Property CountTimeOut As Integer Implements ICommWayStatistics.CountRecvTimeOut
        Public Property CountTransmit As Integer Implements ICommWayStatistics.CountRecv
        Public Property CountWrite As Integer Implements ICommWayStatistics.CountWrite
        Public Property CoutReadQueue As Integer Implements ICommWayStatistics.CoutReadQueue
        Public Property LastTime_Read As Date Implements ICommWayStatistics.LastTimeRecv
        Public Property LastTime_ReadActive As Date Implements ICommWayStatistics.LastTimeReadActive
        Public Property CountSend As Integer Implements ICommWayStatistics.CountSend
        Public Property LastTimeSend As Date Implements ICommWayStatistics.LastTimeSend

    End Class
End Namespace

