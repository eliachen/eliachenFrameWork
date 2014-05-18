Namespace DbService
    Public Class DbServiceCommonForObject
        Implements IDbServiceForObject(Of Object)

        Private repo As DbRepository.DbRepositoryForObject
        Private eq As IEqualEntity = New EqualEntityCommon()

        Sub New(ByVal _repo As DbRepository.DbRepositoryForObject)
            Me.repo = _repo
        End Sub
        Sub New(ByVal _repo As DbRepository.DbRepositoryForObject, ByVal _eq As IEqualEntity)
            Me.repo = _repo
            Me.eq = _eq
        End Sub

        Public Function SetData(_entityname As String, _entitymodel As DataModel, _entity As Object) As Boolean Implements IDbServiceForObject(Of Object).SetData
            Me.repo.EntityName = _entityname
            Select Case _entitymodel
                Case DataModel.creat
                    Return Me.repo.Creat(_entity)
                Case DataModel.delete
                    Return Me.repo.Delete(_entity)
                Case DataModel.update
                    Return Me.repo.Update(_entity)
                Case Else
                    Return True
            End Select
        End Function

        Public Function SetDatas(_entityname As String, _entitymodel As DataModel, _entitys As System.Collections.Generic.IList(Of Object)) As Boolean Implements IDbServiceForObject(Of Object).SetData
            Me.repo.EntityName = _entityname
            Select Case _entitymodel
                Case DataModel.creat
                    Return Me.repo.CreatRanges(_entitys)
                Case DataModel.delete
                    Return Me.repo.DeleteRanges(_entitys)
                Case DataModel.update
                    Return Me.repo.UpdateRanges(_entitys)
                Case Else
                    Return True
            End Select
        End Function

        Public Function GetData(_entityname As String, Optional whereLambda As System.Linq.Expressions.Expression(Of System.Func(Of Object, Boolean)) = Nothing) As System.Collections.Generic.IEnumerable(Of Object) Implements IDbServiceForObject(Of Object).GetData
            '设置实体
            Me.repo.EntityName = _entityname
            '若为nothing则返回全部,否则按照表达式查询
            If whereLambda Is Nothing Then
                Return Me.repo.Query(Function(d) True)
            Else
                Return Me.repo.Query(whereLambda)
            End If
        End Function

        Public Function GetDataByPage(_entityname As String, pageSize As Integer, pageIndex As Integer, ByRef total As Integer, Optional whereLambda As System.Linq.Expressions.Expression(Of System.Func(Of Object, Boolean)) = Nothing) As System.Collections.Generic.IEnumerable(Of Object) Implements IDbServiceForObject(Of Object).GetDataByPage
            '设置实体
            Me.repo.EntityName = _entityname
            '若为nothing则返回全部的查询，否则按照表达式查询
            If whereLambda Is Nothing Then
                Return Me.repo.QueryForPage(pageSize, pageIndex, total, Function(d) True)
            Else
                Return Me.repo.QueryForPage(pageSize, pageIndex, total, whereLambda)
            End If
        End Function

        Public Function GetDataBySingleObject(_entityname As String, query As Object) As System.Collections.Generic.IEnumerable(Of Object) Implements IDbServiceForObject(Of Object).GetDataBySingleObject
            '设置实体
            Me.repo.EntityName = _entityname
            Me.eq.SetEqualSingle(query)
            Return Me.repo.Query(Function(v) Me.eq.EqualSingle(v))
        End Function

        Public Function GetDataBySingleObjectAndPage(_entityname As String, pageSize As Integer, pageIndex As Integer, ByRef total As Integer, query As Object) As System.Collections.Generic.IEnumerable(Of Object) Implements IDbServiceForObject(Of Object).GetDataBySingleObjectAndPage
            '设置实体
            Me.repo.EntityName = _entityname
            Me.eq.SetEqualSingle(query)
            Return Me.repo.QueryForPage(pageSize, pageIndex, total, Function(v) Me.eq.EqualSingle(v))
        End Function

        Public Function GetEntityNames() As System.Collections.Generic.IList(Of String) Implements IDbServiceForObject(Of Object).GetEntityNames
            Return Me.repo.GetAllEntityNames()
        End Function

        Public Function GetEntityType(_entityname As String) As System.Type Implements IDbServiceForObject(Of Object).GetEntityType
            Return Me.repo.GetEntityType(_entityname)
        End Function

        Public Function GetEntityFrame(_entityname As String) As IList(Of EntityDataFrame) Implements IDbServiceForObject(Of Object).GetEntityFrame
            Return New EntityDataFrame().GetFrame(GetEntityType(_entityname).GetProperties())
        End Function

      
    End Class
End Namespace

