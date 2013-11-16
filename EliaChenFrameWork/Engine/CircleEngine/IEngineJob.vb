
Namespace Engine
    ''' <summary>
    ''' 在循环引擎内执行的动作
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IEngineJob
        Sub Excute()
        Event myEvent As Action(Of Object)


    End Interface
End Namespace

