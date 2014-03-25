Imports System.Threading
Namespace CommPort
    ''' <summary>
    ''' 通讯收到数据后基本处理方式,核心是为了判定一次通讯的结束
    ''' 两种模型：1、延迟停等 2、字节流判断
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ICommPortWay

        ''' <summary>
        ''' 时间:发送前的延迟
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property TimeDelaySend As Integer

        ''' <summary>
        ''' 时间:接收数据触发后延迟检测缓冲区
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property TimeOutRecv As Integer

        ''' <summary>
        ''' 1
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommDone As ManualResetEvent



        ''' <summary>
        ''' 当次通讯获取的方法
        ''' </summary>
        ''' <param name="Comm"></param>
        ''' <remarks></remarks>
        Property CommDeal As Func(Of ICommPort, Byte())


        ''' <summary>
        ''' 具有特定格式帧的校验
        ''' </summary>
        ''' <param name="ArrByte"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommValidate As Func(Of Byte(), Boolean)
        'Delegate Function CommDealHandle(ByVal Comm As ICommPort) As Byte


        'Function CommDeal(ByRef Comm As ICommPort) As Byte()


        'Delegate Function CommJugHandle(ByVal ArrByte As Byte()) As Boolean

        
        'Function CommValidate(ByVal ArrByte As Byte()) As Boolean


    End Interface
End Namespace

