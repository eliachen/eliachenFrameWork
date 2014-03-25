Imports EliachenFw.Comm.Proj.Comm_IEC103.Code.BasicCode.BasicCodeSt
Imports EliachenFw.Comm.Proj.Comm_IEC103.Code.AdvCode.AdvCodeSt

Namespace Communication.Proj.Comm_IEC103
    Partial Public Class CommIEC103
        Inherits Communication.CommInterface.Basic_Comm
        Partial Public Class Recver_Station
            'NO4:维护基本命令模式
            Private Class BasicOrderDeal
                Implements Communication.CommInterface.ICommOrder


                '需求发送的命令
                Private mOrderByte As Byte()

                ''发送的从站信息
                'Private mDevState As Comm103DevState

                Sub New(ByVal _OrderByte As Byte(), ByVal _DevState As Comm103DevState)
                    mOrderByte = _OrderByte
                    'mDevState = _DevState
                End Sub
                '命令确认是否为真
                Public Function OrderCheck(ByVal BufferList As EliachenFw.Buffer.BufferList(Of Comm.CommInterface.ICommBufferElement)) As Boolean Implements Comm.CommInterface.ICommOrder.OrderCheck
                    Dim Rel = From s In BufferList Where TypeOf s Is Recv_StableFrame
                                                     Let Ex_s = TryCast(s, Recv_StableFrame)
                                                        Where Ex_s.nCode.ACD = 1 And Ex_s.nCode.FunctionCode = 0
                                                                              Select Ex_s


                    If Rel.ToArray.Count > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                End Function

                '超时处理：更改命令中的Fcb
                Public Sub OrderWhen_TimeOut() Implements Communication.CommInterface.ICommOrder.OrderWhen_TimeOut
                    mOrderByte = Communication.Proj.Comm_IEC103.Code.Others.OrderExFcb(mOrderByte)
                End Sub
                '命令错误处理（单次）:更改命令中Fcb
                Public Sub OrderWhen_OrderErr() Implements Communication.CommInterface.ICommOrder.OrderWhen_OrderErr_Each
                    ''更改命令
                    'mOrderByte = Comm.Proj.Comm_IEC103.Code.Others.OrderExFcb(mOrderByte)
                End Sub
                '命令错误处理（最终）:发送复位通信
                Public Sub OrderWhen_OrderErr_Final() Implements CommInterface.ICommOrder.OrderWhen_OrderErr_Final
                    'Dim tmpCode = Comm_IEC103.Code.BasicCode.DeCode.DeCode_StableFrame(mOrderByte)
                    'mOrderByte = Comm_IEC103.Order.CommReset(tmpCode.Addr, 1)
                End Sub

                '读命令
                Public Function OrderRead() As Byte() Implements Communication.CommInterface.ICommOrder.OrderRead
                    Return mOrderByte
                End Function
                '写命令
                Public Function OrderWrite() As Byte() Implements Communication.CommInterface.ICommOrder.OrderWrite
                    Return Nothing
                End Function

            End Class
        End Class
    End Class
End Namespace
