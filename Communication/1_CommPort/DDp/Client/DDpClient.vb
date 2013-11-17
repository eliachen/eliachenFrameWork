Imports System.Text
Imports EliachenFw.Comm.CommPort.Port.DDpSever
Imports System.Threading

Namespace CommPort
    Public Class DDpClient
        Implements ICommPort
        Private mCommPortWay As ICommPortWay

        '锁
        Private CacheLock As New ReaderWriterLockSlim

        Public User As GPRS_USER_INFO
        Public Data As GPRS_DATA_RECORD
        '基本信息
        Public Inf As String

        Sub New()
            '设置通信模式
            Me.CommPortWay = New CpWay
        End Sub

        Public Property CommPortWay As ICommPortWay Implements ICommPort.CommPortWay
            Get
                Return mCommPortWay
            End Get
            Set(ByVal value As ICommPortWay)
                mCommPortWay = value
            End Set
        End Property

        Public Property CommStatics As ICommPortStatitics Implements ICommPort.CommStatics
            Get

            End Get
            Set(ByVal value As ICommPortStatitics)

            End Set
        End Property

        Public Event RecvDataEvent(ByVal RecvArrByte() As Byte) Implements ICommPort.RecvDataEvent

        '无
        Public Sub Connect() Implements ICommPort.Connect

        End Sub
        '主动下线
        Public Sub DisConnect() Implements ICommPort.DisConnect
            do_close_one_user2(User.m_userid, Nothing)
        End Sub
        '数据接收
        Public Function Receive() As Byte() Implements ICommPort.Receive
            CacheLock.EnterReadLock()
            Try
                Dim tmpData As Byte() = Data.m_data_buf
                If tmpData.Length > 0 Then
                    '清空
                    Data.m_data_buf = New Byte() {}
                    Return tmpData
                Else
                    Return New Byte() {}
                End If
            Finally
                CacheLock.ExitReadLock()
            End Try
        End Function

        Public Function ReceiveAsWay(ByVal Way As ICommPortWay) As Byte() Implements ICommPort.ReceiveAsWay

            Return Way.CommDeal(Me)
        End Function

        Public Sub Send(ByVal SendArrByte() As Byte) Implements ICommPort.Send
            Dim mess As New StringBuilder(500)
            do_send_user_data(User.m_userid, SendArrByte, SendArrByte.Length, mess)
        End Sub

        '表示数据已经到了
        Public Sub DataComing()
            Dim tmpData As Byte() = Me.CommPortWay.CommDeal(Me)
            If tmpData.Length > 0 Then
                RaiseEvent RecvDataEvent(tmpData)
            End If
        End Sub

        '更新
        Public Sub Update(ByVal new_me As DDpClient)
            CacheLock.EnterWriteLock()
            Try
                Me.User = new_me.User
                Me.Data = new_me.Data
            Finally
                CacheLock.ExitWriteLock()
            End Try
        End Sub


    End Class



End Namespace


