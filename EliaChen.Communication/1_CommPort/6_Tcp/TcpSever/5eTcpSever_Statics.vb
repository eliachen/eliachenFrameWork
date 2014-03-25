Imports System.Net
Imports System.Net.Sockets
Imports System
Imports System.Threading.Tasks.Parallel
Namespace CommPort
    Partial Public Class TcpSever
#Region "统计"
        Public Property CountRecv As Integer Implements ICommPortStatitics.CountRecv
            Get
                Return Aggregate p In Me.ClientList.AsParallel Into Sum(p.CommStatics.CountRecv)
            End Get
            Set(value As Integer)

            End Set
        End Property

        Public Property CountSend As Integer Implements ICommPortStatitics.CountSend
            Get
                Return Aggregate p In Me.ClientList.AsParallel Into Sum(p.CommStatics.CountSend)
            End Get
            Set(value As Integer)

            End Set
        End Property

        Public Property SumRecv As Integer Implements ICommPortStatitics.SumRecv
            Get
                Return Aggregate p In Me.ClientList.AsParallel Into Sum(p.CommStatics.SumRecv)
            End Get
            Set(value As Integer)

            End Set
        End Property

        Public Property SumSend As Integer Implements ICommPortStatitics.SumSend
            Get
                Return Aggregate p In Me.ClientList.AsParallel Into Sum(p.CommStatics.SumSend)
            End Get
            Set(value As Integer)

            End Set
        End Property
#End Region
    End Class
End Namespace

