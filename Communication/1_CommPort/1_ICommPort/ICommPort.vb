
Namespace CommPort
    ''' <summary>
    ''' 通讯管理必须的通讯管理接口
    ''' 定义: 数据局接收的接口
    ''' 定义：数据发出的接口
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ICommPort

        ''' <summary>
        ''' 通讯收发的处理参数及其模式
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommPortWay As ICommPortWay

        ''' <summary>
        ''' 通讯过程中的数据统计
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommStatics As ICommPortStatitics

        ''' <summary>
        ''' 通讯定义,利用字符串
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommCofigStr As String


        ''' <summary>
        ''' 通信类型定义
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property CommPortType As String

        'Property CommLog As CommPort.Log.CommPortLogManager(Of CommPort.Log.CommPortLogElement(Of String))

        ''' <summary>
        ''' 表明连接的状态
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property IsConnected As Boolean



        ''' <summary>
        ''' 连接建立
        ''' </summary>
        ''' <remarks></remarks>
        Sub Connect()


        ''' <summary>
        ''' 断开连接
        ''' </summary>
        ''' <remarks></remarks>
        Sub DisConnect()


        ''' <summary>
        ''' 重新连接
        ''' </summary>
        ''' <remarks></remarks>
        Sub ReConnect()




        ''' <summary>
        ''' 委托：数据收到的字节数组
        ''' </summary>
        ''' <param name="sender" >传出的自定义类型</param>
        ''' <param name="RecvArrByte">接收到的字节数组</param>
        ''' <remarks></remarks>
        Delegate Sub RecvDataHandle(Of T)(ByRef sender As T, ByVal RecvArrByte As Byte())

        ''' <summary>
        ''' 事件：数据来到后的字节数组
        ''' </summary>
        ''' <remarks></remarks>
        Event RecvDataEvent As RecvDataHandle(Of ICommPort)


        ''' <summary>
        ''' 发送数据的接口
        ''' </summary>
        ''' <param name="SendArrByte"></param>
        ''' <remarks></remarks>
        Sub Send(ByVal SendArrByte As Byte())

        ''' <summary>
        ''' 接收数据接口（及时的,一次把缓冲区的数据取出来）
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Receive() As Byte()

        ''' <summary>
        ''' 接收数据接口(约定格式)
        ''' </summary>
        ''' <param name="Way">接收模式</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function ReceiveAsWay(ByVal Way As ICommPortWay) As Byte()


    End Interface
End Namespace



