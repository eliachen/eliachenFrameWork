Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Linq.Expressions
Namespace DbService
    Public Interface IDbServiceForObject(Of T)
        '数据操作
        Function SetData(ByVal _entityname As String _
                             , ByVal _entitymodel As DataModel _
                                , ByVal _entity As T) As Boolean

        '数据操作
        Function SetData(ByVal _entityname As String _
                             , ByVal _entitymodel As DataModel _
                                , ByVal _entitys As IList(Of T)) As Boolean




        '获取所有数据
        Function GetData(ByVal _entityname As String, Optional ByVal whereLambda As Expression(Of Func(Of T, Boolean)) = Nothing) As IEnumerable(Of T)


        '获取所有数据:基于基本查询
        Function GetDataBySingleObject(ByVal _entityname As String,
                                       ByVal query As Object) As IEnumerable(Of T)

        '获取所有数据:基于分页
        Function GetDataByPage(ByVal _entityname As String, ByVal pageSize As Integer, ByVal pageIndex As Integer, _
                         ByRef total As Integer, Optional ByVal whereLambda As Expression(Of Func(Of T, Boolean)) = Nothing) As IEnumerable(Of T)

        '获取所有数据:基于基本查询与分页
        Function GetDataBySingleObjectAndPage(ByVal _entityname As String, ByVal pageSize As Integer, ByVal pageIndex As Integer, ByRef total As Integer, _
                                       ByVal query As Object) As IEnumerable(Of T)


        '获取实体类型
        Function GetEntityType(ByVal _entityname As String) As Type

        '获取实体结构
        Function GetEntityFrame(ByVal _entityname As String) As IList(Of EntityDataFrame)

        '所有实体的名称集
        Function GetEntityNames() As IList(Of String)

    End Interface
End Namespace

