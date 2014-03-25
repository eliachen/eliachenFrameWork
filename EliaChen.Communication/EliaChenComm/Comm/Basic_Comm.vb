Imports System.Threading
Imports System.Threading.Tasks
Namespace Communication.CommInterface
    Public MustInherit Class Basic_Comm
        Implements IComm


        '**可能**使用通讯物理接口
        WithEvents mCommPort As Communication.CommPort.ICommPort

        '通讯延迟时间
        Private mCommDone As New ManualResetEvent(False)

        ''基础值存取
        'Private mCommBasicBuffer As EliaChen.Buffer.EliaList(Of Byte())

        '接收对象存储
        Private mCommListRecver As New EliachenFw.Buffer.BufferList(Of ICommRecver)

        '通讯引擎
        Private mCommengine As New EliachenFw.Engine.CircleEngine

        '通讯名单
        Public Property CommListRecver As EliachenFw.Buffer.BufferList(Of ICommRecver) Implements IComm.CommListRecver
            Get
                Return mCommListRecver.SyncBufferList
            End Get
            Set(ByVal value As EliachenFw.Buffer.BufferList(Of ICommRecver))
                mCommListRecver = value
            End Set
        End Property


        ''通讯接口
        'Public Property CommPort As CommPort.ICommPort Implements IComm.CommPort
        '    Get
        '        Return mCommPort
        '    End Get
        '    Set(ByVal value As CommPort.ICommPort)
        '        mCommPort = value
        '    End Set
        'End Property

        ''接收到的数据
        'Private Sub DataRecv(ByVal ArrByte As Byte()) Handles mCommPort.RecvDataEvent
        '    Try
        '        '存储到缓存中去
        '        Me.mCommBasicBuffer.sycAdd(ArrByte)
        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        mCommDone.Set()
        '    End Try
        'End Sub

        Public Property CommEngine As EliachenFw.Engine.CircleEngine Implements IComm.CommEngine
            Get
                Return mCommengine
            End Get
            Set(ByVal value As EliachenFw.Engine.CircleEngine)
                mCommengine = value
            End Set
        End Property
    End Class
End Namespace



