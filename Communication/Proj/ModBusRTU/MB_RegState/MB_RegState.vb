Namespace Communication.Proj.ModBusSlave
    Partial Friend Class ModBusSlave
        Public Class ModBusSlaveReg

            '通讯的ID
            Public RegSlveID As Short = 1

            '寄存器类型
            Public RegFunReg As FunctionType

            '地址
            Public RegAddr As Byte = 1

            '寄存器个数
            Public RegQuantity As UInt16 = 10

            '维护的缓存
            Private RegList As New EliachenFw.Buffer.BufferList(Of Byte)


            '初始化寄存器
            Sub New(ByVal _Fun As FunctionType, ByVal _Addr As Byte, ByVal _Quantity As UInt16)

                RegFunReg = _Fun
                RegAddr = _Addr
                RegQuantity = _Quantity

                Dim TmpArr As Byte() = New Byte() {0, 0}
                '初始化寄存器,全为空
                For tmpi = 1 To RegQuantity
                    RegList.AddRange(TmpArr)
                Next

            End Sub

            ''' <summary>
            ''' 修改寄存器中的值,校验方式
            ''' </summary>
            ''' <param name="ListVal"></param>
            ''' <param name="FrtVal"></param>
            ''' <remarks></remarks>
            Public Sub Reg_Ex(ByVal ListVal As List(Of Object), ByVal FrtVal As String)

                '临时值列表
                Dim TmpValList As New List(Of Byte)

                For Each e In ListVal
                    '获取字节数组
                    Dim tmpArr As Byte() = BitConverter.GetBytes(e)
                    '按需更改
                    If FrtVal = "HL" Then
                        Array.Reverse(tmpArr)
                    ElseIf FrtVal = "LH" Then

                    End If
                    TmpValList.AddRange(tmpArr)
                Next
                '可能有线程危险
                RegList.SyncBufferList = TmpValList
            End Sub

            '返回开始位置,个数的地址
            Public Function Reg_Read(ByVal _start As Byte, ByVal _Regcount As Byte) As Byte()
                Dim RecvSt As New MB_Recv
                RecvSt.addr = RegAddr
                RecvSt.fun = RegFunReg

                '读寄存器的个数,所以反应到字节数需要乘以2
                RecvSt.count = _Regcount * 2

                '读出对应的值
                For index = _start * 2 To _start * 2 + RecvSt.count - 1
                    RecvSt.ListData.Add(RegList.SyncBufferList(index))
                Next
                '校验方式
                RecvSt.CrcFormat = "LH"
                '返回结果
                Return MB_Recv.EncodeRecvBasic(RecvSt)
            End Function



            '读取范围内的值并返回一个字节数组
            Public Shared Function ReadRegRange(ByVal ListReg As List(Of ModBusSlaveReg), ByVal Start As UInt16, ByVal Count As UInt16) As Byte()
                'Dim ListByte As New List(Of Byte)
                'For i = Start To Count
                '    ListByte.AddRange(ListReg(i).RegArrByte)
                'Next
                'Return ListByte.ToArray
            End Function

            'Slave中的类型
            Enum FunctionType
                CoilStatus
                InputStatus
                HoldingReg = 3
                InputReg
            End Enum
        End Class
    End Class
End Namespace

