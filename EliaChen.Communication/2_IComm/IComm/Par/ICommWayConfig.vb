Namespace Communication.CommInterface
    '设备通讯的参数设置
    Public Interface ICommWayConfig
        ''' <summary>
        ''' 设备的扫描时间（轮询过程中设备的停等时间）
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property TimeScan As Integer


        ''' <summary>
        ''' 命令与命令间的间隔时间（等待一条命令超时的时间）
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property TimeOrderOut As Integer




        ''' <summary>
        ''' 一次采集失败后的重试次数
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CountReadFailReTry As Integer



        ''' <summary>
        ''' 一次下置失败后的重试次数
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CountWriteFailReTry As Integer


        ''' <summary>
        ''' 判断通讯失败的次数
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CountCommErr As Integer



        ''' <summary>
        ''' 故障恢复的时间
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property TimeErrResume As Integer

    End Interface
End Namespace

