Imports System.Linq
Imports System.Collections.Generic

Namespace Communication.Proj.ModBusSlave
    Friend Class ModBusSlave
        Inherits Communication.CommInterface.myCommWay.Basic_RecverElement

        '通讯的寄存器:03寄存器,起始1,个数10
        Public MbReg As New ModBusSlaveReg(ModBusSlaveReg.FunctionType.HoldingReg, _
                                                 1, 10)


        '通讯策略
        Private MbRtu_CommPortWay As New MB_CommSpWay

        '校验方式
        Public CrcFormat_Recv As String = "LH"
        Public CrcFormat_Send As String = "LH"

        '用于存储通讯结果
        Private Buffer_RecvData As New EliachenFw.Buffer.BufferList(Of MB_Recv)


        '已经连接成功的通信口
        Sub New(ByVal _CommPort As Communication.CommPort.ICommPort)
            'Set1：通信接口设置
            Me.CommPort = _CommPort
            '
            Me.CommPort.CommPortWay = Nothing
        End Sub

#Region "初始化函数及其结束"
        '初始化函数
        Public Sub Initial()
            Try
                '串口通讯策略
                CommMB_SP.CommPortWay = New MB_CommSpWay

                '初始化串口的配置
                'CommMB_SP.CommPortConfig = New Mb_CommSpCofig

                '串口初始化
                CommMB_SP.eOpen()
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
        '结束函数
        Public Sub Over()
            Try
                '串口初始化
                CommMB_SP.eClose()
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
#End Region

#Region "获取通讯结果"
        '通用获取,不区分,只是去拿寄存器的字节数组
        Public Function GetRel(ByVal SendSt As MB_Read, ByRef RecvArrbyte As MB_Recv) As Boolean
            Try
                '清空
                RecvArrbyte = Nothing
                '发送命令
                If AskRead(MB_Read.Encode_Read(SendSt)) Then
                    '获取对应地址号的设备内的信息
                    Dim rel = From s In Buffer_RecvData.SyncBufferList Where s.addr = SendSt.addr Select s
                    '查找结果
                    If rel.ToArray.Count > 0 Then
                        '结果赋值
                        RecvArrbyte = rel.ToArray(0)
                        Return True
                    End If
                End If
                Return False
            Catch ex As Exception
                Return False
            End Try
        End Function
#End Region

    End Class
End Namespace





'





