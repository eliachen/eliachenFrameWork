'Order
Namespace Communication.Proj.ModBusSlave
    Partial Friend Class ModBusSlave
        '在通讯失败情况下重试的次数,默认5次
        Public OrderReCount As Int32 = 5
        '命令停等时间
        Public OrderDelayTime As Integer = 500

        '检查发送命令的结果,多次重复
        Private Function AskRead(ByVal OrderArrByte As Byte()) As Boolean
            '清空缓存
            Buffer_RecvData.Clear()

            '发送命令
            For i = 1 To OrderReCount
                '发送命令
                CommMB_SP.Write(OrderArrByte)
                '上锁
                CommMBDone.Reset()
                '停等命令
                CommMBDone.WaitOne(OrderDelayTime)
                '检查结果
                If Buffer_RecvData.SyncBufferList.Count > 0 Then
                    Return True
                End If
            Next
            Return False
        End Function
    End Class
End Namespace

