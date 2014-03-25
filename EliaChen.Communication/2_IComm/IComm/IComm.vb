Namespace Communication.CommInterface
    Public Interface IComm



        ''' <summary>
        ''' 通讯的接收对象
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommListRecver As EliachenFw.Buffer.BufferList(Of ICommRecver)


        ''' <summary>
        ''' 通讯的循环引擎
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommEngine As EliachenFw.Engine.CircleEngine



    End Interface
End Namespace

