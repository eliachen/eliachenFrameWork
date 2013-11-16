Namespace CommPort
    ''' <summary>
    ''' 通讯的统计
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ICommPortStatitics

#Region "必要信息"
        ''' <summary>
        ''' 通信接口:数据接收次数
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CountRecv As Integer

        ''' <summary>
        ''' 通信接口：数据发送次数
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CountSend As Integer

        ''' <summary>
        ''' 统计总发送字节数
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property SumSend As Integer

        ''' <summary>
        ''' 统计总接收字节数
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property SumRecv As Integer
#End Region

#Region "可选信息"

#End Region

    End Interface
End Namespace

