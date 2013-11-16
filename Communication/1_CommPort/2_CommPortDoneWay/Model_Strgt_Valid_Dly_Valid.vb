
Namespace CommPort.ComPortWay
    ''' <summary>
    ''' -> Basic
    ''' 直接模式+校验模式+延迟模式+校验模式
    ''' 使用：组合模式：对象,先进行S_V，然后进行D_V
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Model_Strgt_Valid_Dly_Valid
        Inherits Model_Basic

        Public Property S_V As Model_Strgt_Valid

        Public Property D_V As Model_Dly_Valid

       
        Sub New()
            MyBase.New()
            S_V = New Model_Strgt_Valid() With {.TimeDelaySend = Me.TimeDelaySend, .TimeOutRecv = Me.TimeOutRecv}
            D_V = New Model_Dly_Valid() With {.TimeDelaySend = Me.TimeDelaySend, .TimeOutRecv = Me.TimeOutRecv}
            '组合S_V和D_V进行CommDeal的操作
            Me.CommDeal = Function(CommPort As ICommPort) As Byte()
                              Try
                                  Dim S_V_Rel As Byte() = S_V.CommDeal(CommPort)
                                  If S_V_Rel.Count > 0 Then
                                      Return S_V_Rel
                                  Else
                                      Dim D_V_Rel As Byte() = D_V.CommDeal(CommPort)
                                      If D_V_Rel.Count > 0 Then
                                          Return D_V_Rel
                                      Else
                                          '无结果，返回空数组
                                          Return New Byte() {}
                                      End If
                                  End If
                              Catch ex As Exception
                                  Throw ex
                                  '错误的话就抛出一个空数组
                                  Return New Byte() {}
                              End Try
                          End Function

        End Sub

        
    End Class
End Namespace

