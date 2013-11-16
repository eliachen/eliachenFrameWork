Namespace CommPort.Log
    Public Class CommPortLogElement(Of T)

        ''' <summary>
        ''' 通信接口配置
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CommPortConfig As String

        ''' <summary>
        ''' 通信接口类型
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommPortType As String


        ''' <summary>
        ''' 通信的连接状态
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommPortIsConnected As Boolean


        ''' <summary>
        ''' 消息的类型
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property MsgType As MsgTypeModel

        ''' <summary>
        ''' 通信的阶段过程
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property MsgProcessDescription As MsgProcessDescriptionModel


        ''' <summary>
        ''' 自定义的消息详情:自定义消息的模式
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Msg As T


        ''' <summary>
        ''' 消息时间
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property MsgTime As Date = Now



        Public Enum MsgProcessDescriptionModel
            optionset
            connect
            disconnect
            reconnect
            recv
            send
            other
        End Enum

        Public Enum MsgTypeModel
            success = 1
            fail = -1
        End Enum

        Sub New()
            MyBase.New()
        End Sub

    End Class
End Namespace

  