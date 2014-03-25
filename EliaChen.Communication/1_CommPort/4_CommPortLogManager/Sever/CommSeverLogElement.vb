Namespace CommPort.Log
    Public Class CommSeverLogElement(Of TMsg, TSever)

        ''' <summary>
        ''' 通信服务器配置
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CommSeverConfig As String

        ''' <summary>
        ''' 通信服务类型
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommSeverType As String


        ''' <summary>
        ''' 通信服务状态
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CommSeverState As ICommSever(Of TSever).StateModel



        ''' <summary>
        ''' 消息的类型
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property MsgType As MsgTypeModel


        Property MsgDes As MsgProcessDescriptionModel

        ''' <summary>
        ''' 自定义的消息详情:自定义消息的模式
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Msg As TMsg


        ''' <summary>
        ''' 消息时间
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property MsgTime As Date = Now



        Public Enum MsgProcessDescriptionModel
            start
            recvconn
            [stop]
            pause
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

  