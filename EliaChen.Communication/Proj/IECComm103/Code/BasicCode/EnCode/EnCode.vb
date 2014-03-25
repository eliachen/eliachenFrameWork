
Namespace Communication.Proj.Comm_IEC103.Code.BasicCode
    Module EnCode
        '固定帧的启动字符
        Private StartStr_StableFrame As Byte = &H10

        '非固定帧的启动字符
        Private StartStr_unStableFrame As Byte = &H68

        '公共的结束字符
        Private EndStr As Byte = &H16


        '构建固定帧
        Public Function Encode_StableFrame(ByVal _StableFrame As Recv_StableFrame) As Byte()
            Dim FrameByteArray As Byte()
            ReDim FrameByteArray(4)
            '启动字符
            FrameByteArray(0) = StartStr_StableFrame
            '控制域
            FrameByteArray(1) = _StableFrame.oriCode
            '地址域
            FrameByteArray(2) = _StableFrame.Addr
            '校验和
            FrameByteArray(3) = _StableFrame.oriCode + _StableFrame.Addr
            '结束字符
            FrameByteArray(4) = EndStr
            Return FrameByteArray
        End Function

        '构建不固定帧
        Public Function Encode_unStableFrame(ByVal _unStableFrame As Recv_unStableFrame) As Byte()
            Dim ListFrameByteArray As New List(Of Byte)
            Dim Length As Byte
            Dim CS As Byte



            '启动字符1
            ListFrameByteArray.Add(StartStr_unStableFrame)

            '长度1= Length=ASDU 链路用户数据包的字节数+2(2为控制域、地址域的长度)
            Length = _unStableFrame.oriASDU.Length + 2
            ListFrameByteArray.Add(Length)
            '长度2=长度1，重复
            ListFrameByteArray.Add(Length)

            '启动字符2=启动字符1，重复
            ListFrameByteArray.Add(StartStr_unStableFrame)


            '控制域
            ListFrameByteArray.Add(_unStableFrame.oriCode)
            '地址域
            ListFrameByteArray.Add(_unStableFrame.Addr)
            '链路用户数据
            ListFrameByteArray.AddRange(_unStableFrame.oriASDU)
            '校验和= 控制域+地址域+链路用户数据八位位组的算术和（不考虑溢出位即256的模和）
            Dim sum As Integer = 0
            For Each s As Byte In _unStableFrame.oriASDU
                sum = sum + s
            Next
            Dim tempSumCs As Integer = (CInt(_unStableFrame.oriCode) + CInt(_unStableFrame.Addr) + CInt(sum))
            CS = tempSumCs Mod 256
            ListFrameByteArray.Add(CS)

            '结束字符
            ListFrameByteArray.Add(EndStr)


            Return ListFrameByteArray.ToArray
        End Function

    End Module
End Namespace
