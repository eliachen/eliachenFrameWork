Namespace DbService
    ''' <summary>
    ''' 非null值搜索;字符串去空;
    ''' 多属性匹配其一
    ''' </summary>
    ''' <remarks></remarks>
    Public Class EqualEntityCommon
        Implements IEqualEntity
        '将比较对象
        Private singleeq As Object
        '将比较属性名称
        Private singlekeynames As Dictionary(Of String, Object)

        Sub New()
            MyBase.New()
        End Sub
       
        Public Sub SetEqualSingle(queryel As Object) Implements IEqualEntity.SetEqualSingle
            '获取属性名称以及非空值
            Me.singlekeynames = (From s In queryel.GetType().GetProperties()
                                    Let r = queryel.GetType().GetProperty(s.Name).GetValue(queryel, Nothing)
                                        Where r IsNot Nothing
                                            Select New With {.name = s.Name, .value = r}). _
                                                    ToDictionary(Function(d) d.name, Function(d) d.value)

        End Sub

        Public Function EqualSingle(dataeqel As Object) As Boolean Implements IEqualEntity.EqualSingle
            For Each qel In Me.singlekeynames
                Dim querydata = qel.Value

                '字符串处理
                If TypeOf (qel.Value) Is String Then
                    querydata = qel.Value.ToString().Trim()
                    dataeqel = dataeqel.GetType().GetProperty(qel.Key).GetValue(dataeqel, Nothing).ToString().Trim()
                End If
                If Not (querydata.Equals(dataeqel)) Then
                    Return False
                End If
            Next
            Return True
        End Function
    End Class
End Namespace

