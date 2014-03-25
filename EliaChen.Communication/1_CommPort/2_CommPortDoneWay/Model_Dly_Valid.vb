
Namespace CommPort.ComPortWay
    ''' <summary>
    ''' -> Dly
    ''' 延迟模式+校验模式
    ''' 延迟读取后进行校验
    ''' 使用：设置验证函数+设置延迟参数
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Model_Dly_Valid
        Inherits Model_Delay

        Sub New()
            MyBase.New()
            Me.CommDeal = Function(CommPort As ICommPort) As Byte()
                              Try
                                  '延迟获取数据
                                  Dim TmpByte As Byte() = MyBase.CommDeal(CommPort)
                                  '数据判断
                                  If Me.CommValidate(TmpByte) Then
                                      Return TmpByte
                                  Else
                                      Return New Byte() {}
                                  End If
                              Catch ex As Exception
                                  Throw ex
                                  '错误的话就抛出一个空数组
                                  Return New Byte() {}
                              End Try
                          End Function

            If Me.CommValidate Is Nothing Then
                Throw New Exception("未设定验证函数")
            End If
        End Sub
    End Class
End Namespace

