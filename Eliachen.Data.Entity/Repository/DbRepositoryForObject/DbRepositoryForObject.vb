Imports System.Data.Entity.DbContext
Imports System.Reflection
Namespace DbRepository

    Public Class DbRepositoryForObject
        Implements IDbRepository(Of Object)

        Private Property Db As System.Data.Entity.DbContext

        Private mEntityName As String
        Public Property EntityName As String
            Get
                Return mEntityName
            End Get
            Set(value As String)
                Me.mEntityName = value
                Me.mEntityType = GetEntityType(value)
            End Set
        End Property

        Private mEntityType As System.Type
        Public Property EntityType As System.Type
            Get
                Return mEntityType
            End Get
            Set(value As System.Type)
                mEntityType = value
            End Set
        End Property

        Sub New()

        End Sub

        Sub New(ByVal _db As System.Data.Entity.DbContext)
            Me.Db = _db
        End Sub

        Sub New(ByVal _db As System.Data.Entity.DbContext, ByVal _entityname As String)
            Me.Db = _db
            Me.EntityName = _entityname
        End Sub

        Public Function Creat(entity As Object) As Boolean Implements IDbRepository(Of Object).Creat
            Db.Entry(entity).State = EntityState.Added
            Return Db.SaveChanges() > 0
        End Function

        Public Function Query(exp As System.Linq.Expressions.Expression(Of System.Func(Of Object, Boolean))) As System.Collections.Generic.IEnumerable(Of Object) Implements IDbRepository(Of Object).Query
            Return Db.Database.SqlQuery(mEntityType, "Select * from " & Me.EntityName).Cast(Of Object).Where(exp.Compile())
        End Function

        Public Function Update(entity As Object) As Boolean Implements IDbRepository(Of Object).Update
            Db.Set(mEntityType).Attach(entity)
            Db.Entry(entity).State = EntityState.Modified
            Return Db.SaveChanges() > 0
        End Function

        Public Function Delete(entity As Object) As Boolean Implements IDbRepository(Of Object).Delete
            Db.Set(mEntityType).Attach(entity)
            Db.Entry(entity).State = EntityState.Deleted
            Return Db.SaveChanges() > 0
        End Function

        Public Function QueryForPage(pageSize As Integer, pageIndex As Integer, _
                                     ByRef total As Integer, _
                                     whereLambda As System.Linq.Expressions.Expression(Of System.Func(Of Object, Boolean))) As System.Collections.Generic.IEnumerable(Of Object) Implements IDbRepository(Of Object).QueryForPage
            '查询
            Dim tempData = Me.Query(whereLambda)
            total = tempData.Count()
            '分页
            tempData = tempData.Skip(pageSize * (pageIndex - 1)).Take(pageSize)
            Return tempData
        End Function

        Public Overridable Function GetEntityType(ByVal _entityname As String) As System.Type
            Dim assblyname As String = TryCast(Db, Object).GetType().Assembly.GetName().Name
            Return (Assembly.Load(assblyname).GetType(assblyname + "." + _entityname))
        End Function

        Public Overridable Function GetNewEntity(ByVal _entityname As String) As Object
            Return Activator.CreateInstance(GetEntityType(_entityname))
        End Function

        Public Overridable Function GetAllEntityNames() As IList(Of String)
            Dim pros = TryCast(Db, Object).GetType().GetProperties()
            Dim rel = (From s In pros
                        Where s.PropertyType.FullName.Contains("DbSet")
                            Select s.PropertyType.FullName.Replace("System.Data.Entity.DbSet`1[[" + TryCast(Db, Object).GetType().Assembly.GetName().Name + ".", "") _
                                                         .Split(New Char() {","})(0))
            Return rel.ToList()
        End Function

        Public Function CreatRanges(entitys As IEnumerable(Of Object)) As Integer Implements IDbRepository(Of Object).CreatRanges
            For Each e In entitys
                Db.Entry(e).State = EntityState.Added
            Next
            Return Db.SaveChanges()
        End Function

        Public Function DeleteRanges(entitys As IEnumerable(Of Object)) As Integer Implements IDbRepository(Of Object).DeleteRanges
            For Each e In entitys
                Db.Set(mEntityType).Attach(e)
                Db.Entry(e).State = EntityState.Deleted
            Next
            Return Db.SaveChanges()
        End Function

        Public Function UpdateRanges(entitys As IEnumerable(Of Object)) As Integer Implements IDbRepository(Of Object).UpdateRanges
            For Each e In entitys
                Db.Set(mEntityType).Attach(e)
                Db.Entry(e).State = EntityState.Modified
            Next
            Return Db.SaveChanges()
        End Function
    End Class
End Namespace

