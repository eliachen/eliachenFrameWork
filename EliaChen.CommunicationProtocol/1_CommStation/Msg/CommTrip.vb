Namespace CommProtocol
    '应对复杂的需求交互流程
    Public MustInherit Class CommTrip(Of Tbuffer)
        '结果
        Public Property TripRel As Boolean = False

        '结果列表
        Public Property TripRelList As List(Of Tbuffer) = New List(Of Tbuffer)

        '模式
        Public Property TripModel As TripTypeModel = CommTrip(Of Tbuffer).TripTypeModel.noContinue


        '1：发送的Message  3：预期回复是否加入缓存
        Public Property ListMessageTask As New List(Of Tuple(Of SendCommMessage,  _
                                                    Func(Of IList(Of Tbuffer), Boolean)))

        Public Enum TripTypeModel
            '中间出错仍旧继续
            [Continue]
            '出错立即退出
            noContinue
        End Enum
    End Class
End Namespace

