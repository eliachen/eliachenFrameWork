
Namespace CommPort.ComPortWay
    ''' <summary>
    ''' ->Basic
    ''' 直接模式+校验模式
    ''' 直接模式基础上,进行数据校验
    ''' 使用：设置验证函数+设置延迟参数
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Model_Strgt_Valid
        Inherits Model_Basic

        Sub New()
            MyBase.New()
            Me.CommDeal = Function(CommPort As ICommPort) As Byte()
                              Try
                                  '直接获取数据
                                  Dim TmpByte As Byte() = CommPort.Receive()

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

'匹配通讯处理方式
'                Select Case CommPortWay.CommDoneModel
'                    Case IEliaChenCommPortWay.CommDoneModelSelect.DelayStop
'                        RaiseEvent RecvDataEvent(GetData("DELAY"))
'                    Case IEliaChenCommPortWay.CommDoneModelSelect.DataJug
'Dim tmpArrByte As Byte() = GetData("STRAIGHT")
'                        If meDoneWay.CommDoneWay.Invoke(tmpArrByte) Then '检查数据结果:真
'                            RaiseEvent RecvDataEvent(tmpArrByte)
'                        Else                                                      '检查数据结果:假
'                            Exit Sub
'                        End If
'                    Case IEliaChenCommPortWay.CommDoneModelSelect.Mix_ALL
''缓冲获取数据
'Dim tmpArrByte As Byte() = GetData("DELAY")
'                        If meDoneWay.CommDoneWay.Invoke(tmpArrByte) Then '检查数据结果:真
'                            RaiseEvent RecvDataEvent(tmpArrByte)
'                        Else                                                      '检查数据结果:假
'                            Exit Sub
'                        End If
'                    Case IEliaChenCommPortWay.CommDoneModelSelect.Mix_Jug
'Dim tmpArrByte As Byte() = GetData("STRAIGHT")
'Dim tmpListByte As New List(Of Byte)
'                        If meDoneWay.CommDoneWay.Invoke(tmpArrByte) Then '检查数据结果:真,直接传出数据
'                            RaiseEvent RecvDataEvent(tmpArrByte)
'                        Else                                                      '检查数据结果:假,则转为延迟获取
'                            tmpListByte.AddRange(tmpArrByte)
'                            tmpArrByte = GetData("DELAY")
'                            tmpListByte.AddRange(tmpArrByte)
'                            If meDoneWay.CommDoneWay.Invoke(tmpArrByte) Then '检查数据结果:真,传出
'                                RaiseEvent RecvDataEvent(tmpListByte.ToArray)
'                            Else
'                                Exit Sub
'                            End If
'                        End If
'                End Select