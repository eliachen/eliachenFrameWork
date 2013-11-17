Imports System.Collections.Concurrent
Imports System.Collections.Generic
    ''' <summary>
''' 面向Linq的基本功能集合
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <remarks></remarks>
Public Interface IBufferList(Of T)

    '添加'
    Sub Add(item As T)

    '查询
    Function Find(ByVal selector As IEnumerable(Of T)) As IEnumerable(Of T)

    '删除
    Sub RemoveAll(ByVal selector As IEnumerable(Of T))

    '清空
    Sub Clear()

End Interface
