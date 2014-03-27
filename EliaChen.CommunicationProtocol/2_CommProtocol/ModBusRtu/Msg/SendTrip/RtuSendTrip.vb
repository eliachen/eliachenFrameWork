Imports EliaChen.CommProtocol.ModBus.Rtu

Namespace CommProtocol.ModBus
    Partial Public Class Rtu
        Public Class RtuSendTrip
            Inherits CommTrip(Of RtuRecvMessage)

            Sub New()
                '出错后仍旧继续
                Me.TripModel = CommTrip(Of Rtu.RtuRecvMessage).TripTypeModel.noContinue
            End Sub

            '下置连续寄存器
            Public Function SendFunc16_Trip(_SlaveID As Byte, _StartRegIndex As UShort, _RegCount As UShort, _Values As Short()) As RtuSendTrip
                Me.ListMessageTask.Add(Tuple.Create(Of SendCommMessage, 
                                       Func(Of IList(Of RtuRecvMessage), Boolean)) _
                                      (New RtuSendMessage().SendFc16(_SlaveID, _StartRegIndex, _RegCount, _Values),
                                            Function(l) As Boolean
                                                '匹配
                                                Dim rel = (From s In l
                                                           Let recvstartindex = BitConverter.ToUInt16(s.RecvMsg.Data.Take(2).Reverse().ToArray(), 0),
                                                               recvregcount = BitConverter.ToUInt16(s.RecvMsg.Data.Skip(2).Reverse().ToArray(), 0)
                                                            Where s.RecvMsg.Address = _SlaveID _
                                                                   And s.RecvMsg.Function = 16 _
                                                                    And recvstartindex = _StartRegIndex _
                                                                    And recvregcount = _RegCount
                                                                Select s).FirstOrDefault

                                                If rel IsNot Nothing Then
                                                    Return True
                                                Else
                                                    Return False
                                                End If
                                            End Function))

                Return Me
            End Function

        End Class
    End Class
    
End Namespace


