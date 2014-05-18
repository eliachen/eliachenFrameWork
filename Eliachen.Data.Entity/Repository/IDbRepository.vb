Imports System.Linq
Imports System.Linq.Expressions

Namespace DbRepository
    Public Interface IDbRepository(Of T As Class)
        Function Creat(entity As T) As [Boolean]
        Function Query(exp As Expression(Of Func(Of T, Boolean))) As IEnumerable(Of T)
        Function Update(entity As T) As [Boolean]
        Function Delete(entity As T) As [Boolean]

        Function CreatRanges(entitys As IEnumerable(Of T)) As Integer
        Function UpdateRanges(entitys As IEnumerable(Of T)) As Integer
        Function DeleteRanges(entitys As IEnumerable(Of T)) As Integer

        Function QueryForPage(pageSize As Integer, pageIndex As Integer, ByRef total As Integer, whereLambda As Expression(Of Func(Of T, Boolean))) As IEnumerable(Of T)
    End Interface
End Namespace

