Namespace Communication.CommInterface
    Public Interface ICommRecver


        ''' <summary>
        ''' 通讯的标识：唯一的编号
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommId As Integer


        ''' <summary>
        ''' 通讯的并行性：可以同时召测
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommParallelable As Boolean

        ''' <summary>
        ''' 通讯接口
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommPort As Communication.CommPort.ICommPort


        ''' <summary>
        ''' 通讯:接收信息缓存列表
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommRecvBuffer As EliachenFw.Buffer.IBufferList(Of ICommBufferElement)


        ''' <summary>
        ''' 通信策略:参数设置
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommWayConfig As Communication.CommInterface.ICommWayConfig

        ''' <summary>
        ''' 通信策略:结果参数
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommStatistics As Communication.CommInterface.ICommWayStatistics



        ''' <summary>
        ''' 通讯初始化
        ''' </summary>
        ''' <remarks></remarks>
        Function CommInitial() As Object


        ''' <summary>
        ''' 通讯复位
        ''' </summary>
        ''' <remarks></remarks>
        Function CommReset() As Object


        ''' <summary>
        ''' 通讯
        ''' </summary>
        ''' <remarks></remarks> 
        Function CommDispose() As Object


        ''' <summary>
        ''' 通讯错误处理函数：超时
        ''' </summary>
        ''' <remarks></remarks>
        Function Comm_WhenErr_Read_OverTime() As Object

        ''' <summary>
        ''' 通讯错误处理函数：无正确的返回
        ''' </summary>
        ''' <remarks></remarks>
        Function Comm_WhenErr_Read_ErrRecv() As Object


        ''' <summary>
        ''' 接收到数据的处理方法
        ''' </summary>
        ''' <param name="ArrByte"></param>
        ''' <remarks></remarks>
        Function Excute(ByVal ArrByte As Byte()) As Boolean


        ''' <summary>
        ''' 读命令：单次命令下发的结果
        ''' </summary>
        ''' <param name="_Order"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function CommOrder_Read(ByVal _Order As ICommOrder) As Boolean

        ''' <summary>
        ''' 写命令：单次命令下发的结果
        ''' </summary>
        ''' <param name="_Order"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function CommOrder_Write(ByVal _Order As ICommOrder) As Boolean

    End Interface
End Namespace

