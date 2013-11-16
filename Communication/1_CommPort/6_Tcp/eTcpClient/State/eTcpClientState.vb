
Public Class eTcpClientState
    Implements IDisposable

    '缓冲的长度
    Public ArrBuffer As Byte()
    '缓冲区的长度:默认1024
    Private ArrBufferLen As Integer = 1024

    Public CommDone As New System.Threading.ManualResetEvent(False)

    '套接字错误的枚举
    Public ErrSocket As New System.Net.Sockets.SocketError

#Region "Recv"
    '用于接收,默认接收缓冲区
    Sub New()
        '定义缓冲的长度
        ReDim ArrBuffer(ArrBufferLen - 1)
    End Sub
    '用于接收，自定义缓冲区
    Sub New(ByVal SetArrBufferLen As Integer)
        '定义缓冲的长度
        ReDim ArrBuffer(SetArrBufferLen - 1)
    End Sub
#End Region


#Region "Send"
    '用于发送
    Sub New(ByVal ArrByte As Byte())
        ArrBuffer = ArrByte
    End Sub
#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 检测冗余的调用

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)。
                ArrBuffer = Nothing
                CommDone = Nothing
            End If

            ' TODO: 释放非托管资源(非托管对象)并重写下面的 Finalize()。
            ' TODO: 将大型字段设置为 null。
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: 仅当上面的 Dispose(ByVal disposing As Boolean)具有释放非托管资源的代码时重写 Finalize()。
    'Protected Overrides Sub Finalize()
    '    ' 不要更改此代码。请将清理代码放入上面的 Dispose(ByVal disposing As Boolean)中。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' Visual Basic 添加此代码是为了正确实现可处置模式。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。请将清理代码放入上面的 Dispose(ByVal disposing As Boolean)中。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class


