
Namespace CommPort.ComPortWay
    ''' <summary>
    ''' -> Basic
    ''' 直接模式
    ''' 直接返回通讯口当前能读到的所有数据
    ''' 使用：无设置
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Model_Straight
        Inherits Model_Basic
        Sub New()
            MyBase.New()
            Me.CommDeal = Function(CommPort As ICommPort) As Byte()
                              Try
                                  Return CommPort.Receive()
                              Catch ex As Exception
                                  Throw ex
                                  '错误的话就抛出一个空数组
                                  Return New Byte() {}
                              End Try
                          End Function
        End Sub
    End Class
End Namespace

