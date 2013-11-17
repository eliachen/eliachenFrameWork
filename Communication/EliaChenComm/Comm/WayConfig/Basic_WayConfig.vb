Namespace Communication.CommInterface.myCommWay
    Public MustInherit Class Basic_WayConfig
        Implements ICommWayConfig
        Public Property CountCommErr As Integer Implements ICommWayConfig.CountCommErr
        Public Property CountReadFailReTry As Integer Implements ICommWayConfig.CountReadFailReTry
        Public Property CountWriteFailReTry As Integer Implements ICommWayConfig.CountWriteFailReTry
        Public Property TimeErrResume As Integer Implements ICommWayConfig.TimeErrResume
        Public Property TimeOrder As Integer Implements ICommWayConfig.TimeOrderOut
        Public Property TimeScan As Integer Implements ICommWayConfig.TimeScan
    End Class
End Namespace

