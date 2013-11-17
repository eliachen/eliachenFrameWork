Imports System.Threading
Namespace Communication.Proj.ModBusSlave
    Partial Friend Class ModBusSlave
        '通讯口
        Public WithEvents CommMB_SP As New Communication.CommPort.Port.SerialPort()

        '通讯延迟时间
        Private CommMBDone As New ManualResetEvent(False)

        '关联数据接收事件
        Private Sub MB_RecvData(ByVal Arrbyte As Byte()) Handles CommMB_SP.RecvDataEvent
            Try
                '接收到的字节数组
                Dim Sendst As MB_Read
                Sendst = MB_Read.Decode_Read(Arrbyte, CrcFormat_Send)

                Me.CommMB_SP.Write(MbReg.Reg_Read(Sendst.start, Sendst.count))
            Catch ex As Exception
                Exit Sub
            End Try
        End Sub



        '通讯接口数据处理
        Private Class MB_CommSpWay
            Inherits CommPort.ComPortWay.Model_Strgt_Jud_Dly_Jud

            '发送及接收延迟设置
            Sub New()
                Me.TimeDelaySend = 0
                Me.TimeOutRecv = 200
            End Sub

            '接口

            '重载数据判断
            Public Overrides Function CommJug(ByVal ArrByte As Byte()) As Boolean
                Return MB_Recv.CheckData(ArrByte, "LH")
            End Function

        End Class
    End Class
End Namespace

