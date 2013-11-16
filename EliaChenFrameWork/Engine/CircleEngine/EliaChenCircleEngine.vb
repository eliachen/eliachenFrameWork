Imports System.Threading.Tasks
Imports System.Threading
Namespace Engine
    ''' <summary>
    ''' 循环引擎用于轮询一个任务
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CircleEngine

        '需要执行的任务
        Public EnginJob As IEngineJob

        Private mTokenSource As New CancellationTokenSource()
        Private mToken As CancellationToken = mTokenSource.Token
        Private mTask As Task

        '任务开始
        Public Sub Start()
            mTokenSource = New CancellationTokenSource()
            mToken = mTokenSource.Token
            mTask = New Task(Sub()
                                 While True
                                     Try
                                         '取消则不再执行
                                         If Not mTokenSource.IsCancellationRequested Then
                                             EnginJob.Excute()
                                         Else
                                             Exit While
                                         End If
                                         '任务取消的错误
                                     Catch ex As OperationCanceledException
                                         Throw ex
                                     End Try
                                 End While
                             End Sub, mToken)
            mTask.Start()
        End Sub
        ' 任务结束
        Public Sub [Stop]()
            mTokenSource.Cancel()
            mToken.WaitHandle.WaitOne()
        End Sub
    End Class
End Namespace

