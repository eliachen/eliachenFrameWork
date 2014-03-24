Imports EliaChen
Imports EliaChen.Collection

Namespace CommPort.Log

    Public Class CommSeverLogManager(Of TMsg, TClientMember)
        Inherits BufferList(Of CommSeverLogElement(Of TMsg, TClientMember))

        Private Property CommSever As ICommSever(Of TClientMember)

        Sub New()
            MyBase.New()
        End Sub
        Sub New(ByVal _CommSever As ICommSever(Of TClientMember))
            Me.CommSever = _CommSever
        End Sub

        Public Overridable Sub Log(item As CommSeverLogElement(Of TMsg, TClientMember))
            MyBase.Add(item)
        End Sub

        Public Shadows Add

        Public Overridable Sub LogSuccess(ByVal _MsgDes As Log.CommSeverLogElement(Of TMsg, TClientMember).MsgProcessDescriptionModel, _
                                          Optional ByVal _Msg As TMsg = Nothing)


            Me.Log(New Log.CommSeverLogElement(Of TMsg, TClientMember) With {.CommSeverConfig = Me.CommSever.CommConfigStr, .CommSeverState = Me.CommSever.CommSeverState, _
                                                                      .CommSeverType = Me.CommSever.CommSeverType, .MsgType = CommSeverLogElement(Of TMsg, TClientMember).MsgTypeModel.success, _
                                                                    .MsgDes = _MsgDes, .Msg = _Msg})

        End Sub

        Public Overridable Sub LogError(ByVal _MsgDes As Log.CommSeverLogElement(Of TMsg, TClientMember).MsgProcessDescriptionModel, _
                                         Optional ByVal _Msg As TMsg = Nothing)


            Me.Log(New Log.CommSeverLogElement(Of TMsg, TClientMember) With {.CommSeverConfig = Me.CommSever.CommConfigStr, .CommSeverState = Me.CommSever.CommSeverState, _
                                                                      .CommSeverType = Me.CommSever.CommSeverType, .MsgType = CommSeverLogElement(Of TMsg, TClientMember).MsgTypeModel.fail, _
                                                                    .MsgDes = _MsgDes, .Msg = _Msg})

        End Sub
    End Class



End Namespace

