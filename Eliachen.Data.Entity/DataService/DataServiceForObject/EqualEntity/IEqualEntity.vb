Namespace DbService
    Public Interface IEqualEntity
        '设置对比对象
        Sub SetEqualSingle(ByVal queryel As Object)

        '单个匹配
        Function EqualSingle(ByVal datael As Object) As Boolean

        '多个匹配

    End Interface
End Namespace

