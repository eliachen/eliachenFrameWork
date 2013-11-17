Namespace Communication.Proj.Comm_IEC103
    Partial Public Class CommIEC103
        Inherits Communication.CommInterface.Basic_Comm
        Partial Public Class Recver_Station
            '统计：通信过程统计(使用默认)
            Private Class CommStaticsFor103
                Inherits Communication.CommInterface.myCommWay.Basic_WayStatistics
                Sub New()
                End Sub
            End Class
        End Class
    End Class
End Namespace
