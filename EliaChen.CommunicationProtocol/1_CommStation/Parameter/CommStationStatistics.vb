Namespace CommProtocol
    '设备通讯过程中的统计参数
    Public Class CommStationStatistics



        '次数：接收的次数（来自通信接口）
        Property CountRecv As Integer

        '次数:收到正确数据的总次数（符合基本解码）
        Property CountRecvRight As Integer

        '次数:收到错误数据的总次数（不符合基本解码）
        Property CountRecvFalse As Integer



        '次数:对话
        Property CountTrip As Integer

        '次数:超时次数
        Property CountTripTimeOut As Integer

        '次数:对话正确次数
        Property CountTripRight As Integer

        '次数:对话错误次数
        Property CountTripFalse As Integer


        '次数：对话超时次数

        ''时间:上次响应时间
        'Property LastRecvTime As Date
        ''时间:上次成功接收时间
        'Property LastRecvRightTime As Date




        '#Region "当前实现"

        '        ''' <summary>
        '        ''' 次数：通讯正确的总次数（符合条件的正确回应,具有正确的交互）
        '        ''' </summary>
        '        ''' <value></value>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Property CountRecvActive As Integer

        '        ''' <summary>
        '        '''时间: 上次正确读取命令的时间（编码）
        '        ''' </summary>
        '        ''' <value></value>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Property LastTimeRecv As Date

        '        ''' <summary>
        '        '''时间: 上次正确读取命令的时间（逻辑）
        '        ''' </summary>
        '        ''' <value></value>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Property LastTimeReadActive As Date

        '        ''' <summary>
        '        ''' 次数：命令发送次数
        '        ''' </summary>
        '        ''' <value></value>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Property CountSend As Integer

        '        ''' <summary>
        '        ''' 时间：上次发送时间
        '        ''' </summary>
        '        ''' <value></value>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Property LastTimeSend As Date

        '        ''' <summary>
        '        ''' 次数：超时次数(命令下发后没有在规定时间内响应)
        '        ''' </summary>
        '        ''' <value></value>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Property CountRecvTimeOut As Integer

        '        ''' <summary>
        '        ''' 次数:重试总次数（返回错误命令或者超时的重试次数）
        '        ''' </summary>
        '        ''' <value></value>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Property CountRetrie As Integer

        '        ''' <summary>
        '        ''' 次数：错误总次数（程序处理上的错误）
        '        ''' </summary>
        '        ''' <value></value>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Property CountAppErr As Integer
        '#End Region



        '        ''' <summary>
        '        ''' 次数：等待处理的队列个数
        '        ''' </summary>
        '        ''' <value></value>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Property CoutReadQueue As Integer


        '        ''' <summary>
        '        ''' 次数：下置的次数
        '        ''' </summary>
        '        ''' <value></value>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Property CountWrite As Integer

        '        Property CountData As Integer
        '        Property CountConn As Integer
        '        Property CountOverruns As Integer
    End Class
End Namespace

