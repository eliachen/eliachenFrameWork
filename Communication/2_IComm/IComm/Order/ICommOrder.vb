Namespace Communication.CommInterface
    '设备通讯过程中的命令响应
    Public Interface ICommOrder

        ''' <summary>
        ''' 读命令的字节数组：用于通讯端口发送
        ''' </summary>
        ''' <remarks></remarks>
        Function OrderRead() As Byte()


        ''' <summary>
        ''' 写命令的字节数组：用于通讯端口发送
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function OrderWrite() As Byte()

        ''' <summary>
        ''' 超时处理
        ''' </summary>
        ''' <remarks></remarks>
        Sub OrderWhen_TimeOut()

        ''' <summary>
        ''' 命令错误处理:单次，失败后的处理
        ''' </summary>
        ''' <remarks></remarks>
        Sub OrderWhen_OrderErr_Each()

        ''' <summary>
        ''' 命令错误处理:最终，最终处理失败后结果
        ''' </summary>
        ''' <remarks></remarks>
        Sub OrderWhen_OrderErr_Final()


        ''' <summary>
        ''' 对于命令结果进行检测：目的是获取是否有符合要求的结果已进行随后的工作
        ''' </summary>
        ''' <param name="BufferList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function OrderCheck(ByVal BufferList As EliachenFw.Buffer.BufferList(Of ICommBufferElement)) As Boolean

    End Interface
End Namespace

