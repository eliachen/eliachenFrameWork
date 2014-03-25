Imports Comm.Proj.Comm_IEC103.DetailDecode

Namespace Communication.Proj.Comm_IEC103
    Public Class CommIEC103
        Inherits Communication.CommInterface.Basic_Comm

        Public Delegate Sub DelShow(ByVal obj As Object)
        Event EventShow As DelShow

        Sub New(ByVal _ListDev As Buffer.BufferList(Of Recver_Station))
            '添加任务
            Me.CommEngine.EnginJob = New CircleCall(_ListDev)
        End Sub

        '内部执行通过自定义
        Public Class CircleCall
            Implements Engine.IEngineJob

            Public Event DataCircleRel(ByVal obj As Object) Implements Engine.IEngineJob.myEvent

            Private mL As New Buffer.BufferList(Of Recver_Station)

            Sub New(ByVal _L As Buffer.BufferList(Of Recver_Station))
                mL = _L
            End Sub

            Public Sub Excute() Implements Engine.IEngineJob.Excute
                For Each e In mL
                    e.CommCall_ElcCharge()
                Next
            End Sub
        End Class

    End Class
End Namespace

