Namespace CommProtocol
    '设备通讯的参数设置
    Public Class CommStationConfig

        ''' <summary>
        ''' 测站并行性：True-并行招;False-同步招
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommParallelable As Boolean = False

        ' ''' <summary>
        ' ''' 测站轮询时间
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Property TimeScan As Integer = 0


        ''' <summary>
        ''' 命令超时时间(ms)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property MessageTimeOut As Integer = 500

      


        ''' <summary>
        ''' 命令回应错误重试次数
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property MessageErrReTryCount As Integer = 5





        ' ''' <summary>
        ' ''' 故障恢复的时间
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Property TimeErrResume As Integer


        ' ''' <summary>
        ' ''' 一次下置失败后的重试次数
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Property CountWriteFailReTry As Integer


        ' ''' <summary>
        ' ''' 判断通讯失败的次数
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Property CountCommErr As Integer
    End Class
End Namespace

