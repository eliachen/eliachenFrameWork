

Namespace Communication.CommInterface
    ''' <summary>
    ''' 实现：用于通讯数据的存取与发送的缓存
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ICommBufferElement

        ''' <summary>
        ''' 进行BufferElement的检查,加入缓存前的检查
        ''' </summary>
        ''' <param name="ArrByte"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function CheckBufferElement(ByVal ArrByte As Byte()) As Boolean

        ''' <summary>
        ''' 获取BufferElement类的每一个项，解码前
        ''' </summary>
        ''' <param name="ArrByte"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetBufferElement(ByVal ArrByte As Byte()) As ICommBufferElement

    End Interface
End Namespace