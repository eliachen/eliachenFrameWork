
Namespace CommPort
    Public Interface ICommSever(Of T)
        Inherits ICommPortStatitics

        ''' <summary>
        ''' 客户端列表
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommClientList As Buffer.BufferList(Of T)


        ''' <summary>
        ''' 字符串配置
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommConfigStr As String

        ''' <summary>
        ''' 服当前状态
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommSeverState As StateModel


        ''' <summary>
        ''' 服务的类型
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property CommSeverType As String


        Delegate Sub Del_RecvData(ByVal Client As T, ByVal RecvData As Byte())
        '接收事件
        Event RecvData As Del_RecvData

        ''' <summary>
        ''' 开始服务:开始监听与接收传入连接
        ''' </summary>
        ''' <remarks></remarks>
        Sub Start()

        ''' <summary>
        ''' 服务暂停：保持监听状态，断开传入连接
        ''' </summary>
        ''' <remarks></remarks>
        Sub Pause()

        ''' <summary>
        ''' 服务停止：断开所有传入连接，服务停止
        ''' </summary>
        ''' <remarks></remarks>
        Sub [Stop]()

        ''' <summary>
        ''' 刷新列表
        ''' </summary>
        ''' <remarks></remarks>
        Sub FreshClientList()

        Enum StateModel
            running
            pause
            stopped
            [error]
        End Enum
    End Interface
End Namespace

