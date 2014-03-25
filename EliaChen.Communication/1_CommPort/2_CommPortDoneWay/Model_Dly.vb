
Namespace CommPort.ComPortWay
    ''' <summary>
    ''' -> Basic
    ''' 延迟模式
    ''' 延迟回检测直到读出所有缓存中的数据
    ''' 使用：设置延迟参数
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Model_Delay
        Inherits Model_Basic

        Sub New()
            MyBase.New()
            '默认延迟设置
            Me.TimeOutRecv = 150
            Me.TimeDelaySend = 0
            '重写延迟
            Me.CommDeal = Function(CommPort As ICommPort) As Byte()
                              Return GetDelayData(CommPort, Me.TimeOutRecv)
                          End Function
        End Sub

        
        ''' <summary>
        ''' 延迟获取数据模式
        ''' </summary>
        ''' <param name="CommPort">通讯端口</param>
        ''' <param name="DelayTime">延迟时间</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetDelayData(ByRef CommPort As ICommPort, ByVal DelayTime As Integer) As Byte()
            Try
                '临时数据存储
                Dim TmpListByte As New List(Of Byte)
                '收到的数据
                Dim TmpArrByte As Byte()

                While True
                    '收到的数据
                    TmpArrByte = CommPort.Receive()
                    If TmpArrByte.Count > 0 Then
                        '加入缓存
                        TmpListByte.AddRange(TmpArrByte)
                    Else
                        Exit While
                    End If
                    '延时
                    Threading.Thread.Sleep(DelayTime)
                End While

                Return TmpListByte.ToArray()
            Catch ex As Exception
                Throw ex
                '错误的话就抛出一个空数组
                Return New Byte() {}
            End Try
        End Function
    End Class
End Namespace