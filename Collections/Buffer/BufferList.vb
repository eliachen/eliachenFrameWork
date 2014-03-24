Imports System.Threading


Namespace Collection
    ''' <summary>
    ''' 基于List的缓存类
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <remarks></remarks>
    Public Class BufferList(Of T)
        Inherits List(Of T)
        Implements IBufferList(Of T)

        Sub New()
            MyBase.New()
        End Sub

        Public Overloads Sub Add(item As T) Implements IBufferList(Of T).Add
            SyncLock (Me)
                MyBase.Add(item)
            End SyncLock
        End Sub

        Public Overloads Sub Clear() Implements IBufferList(Of T).Clear
            SyncLock (Me)
                MyBase.Clear()
            End SyncLock
        End Sub

        Public Overloads Function Find(selector As IEnumerable(Of T)) As IEnumerable(Of T) Implements IBufferList(Of T).Find
            SyncLock (Me)
                Return selector
            End SyncLock
        End Function


        Public Overloads Function Remove(item As T) As Boolean
            SyncLock (Me)
                Return MyBase.Remove(item)
            End SyncLock
        End Function


        Public Overloads Sub RemoveAll(selector As IEnumerable(Of T)) Implements IBufferList(Of T).RemoveAll
            SyncLock (Me)
                For Each _e In selector
                    MyBase.Remove(_e)
                Next
            End SyncLock
        End Sub

    End Class
End Namespace



