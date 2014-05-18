
Imports System.Data.Entity
Imports System.Reflection

Namespace DbContext

    Public Interface IDbContextStateByEntityName


        '获取实体的类型
        Function GetEntityType(ByVal _entityname As String) As Type

        '获取新实体
        Function GetNewEntity(ByVal _entityname As String) As Object

        '获取所有实体名称
        Function GetAllEntityNames() As List(Of String)

    End Interface
End Namespace

