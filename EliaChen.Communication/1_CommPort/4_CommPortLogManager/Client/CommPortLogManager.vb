Imports EliaChen
Imports EliaChen.Collection

Namespace CommPort.Log
    ''' <summary>
    ''' 日志管理
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <remarks></remarks>
    Public Class CommPortLogManager(Of T)
        Inherits BufferList(Of CommPortLogElement(Of T))
        Private Property Commport As ICommPort

        Sub New()
            MyBase.New()
        End Sub
        Sub New(ByVal _CommPort As ICommPort)
            Me.Commport = _CommPort
        End Sub


        Public Overridable Sub Log(item As CommPortLogElement(Of T))
            MyBase.Add(item)
        End Sub

        Public Shadows Add

        Public Overridable Sub LogSuccess(ByVal _MsgDes As Log.CommPortLogElement(Of T).MsgProcessDescriptionModel, _
                                          Optional ByVal _Msg As T = Nothing)


            Me.Log(New Log.CommPortLogElement(Of T) With {.CommPortConfig = Commport.CommCofigStr, .CommPortType = Commport.CommPortType, .CommPortIsConnected = Commport.IsConnected,
                                                                     .MsgType = CommPortLogElement(Of T).MsgTypeModel.success, .MsgTime = Now,
                                                                     .MsgProcessDescription = _MsgDes, .Msg = _Msg})

        End Sub

        Public Overridable Sub LogError(ByVal _MsgDes As Log.CommPortLogElement(Of T).MsgProcessDescriptionModel, _
                                        Optional ByVal _Msg As T = Nothing)
            Me.Log(New Log.CommPortLogElement(Of T) With {.CommPortConfig = Commport.CommCofigStr, .CommPortType = Commport.CommPortType, .CommPortIsConnected = Commport.IsConnected,
                                                                     .MsgType = CommPortLogElement(Of T).MsgTypeModel.fail, .MsgTime = Now,
                                                                     .MsgProcessDescription = _MsgDes, .Msg = _Msg})
        End Sub
    End Class



End Namespace

