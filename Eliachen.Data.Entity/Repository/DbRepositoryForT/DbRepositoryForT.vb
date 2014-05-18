
Namespace DbRepository
    Public Class DbRepositoryForT(Of T As Class)
        Implements IDbRepository(Of T)




        Private db As System.Data.Entity.DbContext

        Public Sub New(_db As System.Data.Entity.DbContext)
            Me.db = _db
        End Sub

        Public Function Creat(entity As T) As Boolean Implements IDbRepository(Of T).Creat
            db.Entry(Of T)(entity).State = EntityState.Added
            Return db.SaveChanges() > 0
        End Function
        Public Function Update(entity As T) As Boolean Implements IDbRepository(Of T).Update
            db.[Set](Of T)().Attach(entity)
            db.Entry(Of T)(entity).State = EntityState.Modified
            Return db.SaveChanges() > 0
        End Function

        Public Function Delete(entity As T) As Boolean Implements IDbRepository(Of T).Delete
            db.[Set](Of T)().Attach(entity)
            db.Entry(Of T)(entity).State = EntityState.Deleted
            Return db.SaveChanges() > 0
        End Function

        Public Function Query(exp As System.Linq.Expressions.Expression(Of System.Func(Of T, Boolean))) As System.Collections.Generic.IEnumerable(Of T) Implements IDbRepository(Of T).Query
            Return db.[Set](Of T)().Where(exp)
        End Function

        Public Function QueryForPage(pageSize As Integer, pageIndex As Integer, ByRef total As Integer, whereLambda As System.Linq.Expressions.Expression(Of System.Func(Of T, Boolean))) As System.Collections.Generic.IEnumerable(Of T) Implements IDbRepository(Of T).QueryForPage
            Dim tempData = db.[Set](Of T)().Where(whereLambda)
            total = tempData.Count()
            tempData = tempData.Skip(pageSize * (pageIndex - 1)).Take(pageSize)
            Return tempData
        End Function


        Public Function CreatRanges(entitys As IEnumerable(Of T)) As Integer Implements IDbRepository(Of T).CreatRanges
            For Each e In entitys
                db.Entry(Of T)(e).State = EntityState.Added
            Next
            Return db.SaveChanges()
        End Function

        Public Function DeleteRanges(entitys As IEnumerable(Of T)) As Integer Implements IDbRepository(Of T).DeleteRanges
            For Each e In entitys
                db.[Set](Of T)().Attach(e)
                db.Entry(Of T)(e).State = EntityState.Deleted
            Next
            Return db.SaveChanges()
        End Function

        Public Function UpdateRanges(entitys As IEnumerable(Of T)) As Integer Implements IDbRepository(Of T).UpdateRanges
            For Each e In entitys
                db.[Set](Of T)().Attach(e)
                db.Entry(Of T)(e).State = EntityState.Modified
            Next
            Return db.SaveChanges()
        End Function
    End Class
End Namespace

