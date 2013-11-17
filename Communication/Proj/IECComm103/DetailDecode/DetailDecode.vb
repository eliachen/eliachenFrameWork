
Namespace Communication.Proj.Comm_IEC103
    Public Class DetailDecode

        Public Shared Function GetDetailData(ByVal dev As Comm103DevState, ByVal obj As Object) As List(Of Comm103DevState.ValState)
            Select Case dev.Name.ToUpper
                Case "Wfb_821A"
                Case "Wbh_822A"
                Case "Wcb_822A"
                Case "Wxh_822A"
                Case "MMPR610HB"
                    Return GetData_MMPR_610Hb(dev, obj)
                Case "MLPR610HB"
                    Return GetData_MLPR_610Hb(dev, obj)
                Case "MTPR610HB"
                    Return GetData_MMPR_610Hb(dev, obj)
                Case "MTPR650HB"
                    Return GetData_MTPR_650Hb(dev, obj)
                Case Else
                    Return Nothing
            End Select
        End Function


#Region "许继"
        Private Shared Function GetData_Wfb_821A(ByVal dev As Comm103DevState, ByVal obj As Object) As Object
            If obj.bool Then

                Dim ListObj As New List(Of Object)

                For i = 1 To obj.rel.count
                    Dim val As Int16 = obj.rel(i - 1)
                    Select Case True
                        'ia~ic
                        Case i >= 1 And i <= 3
                            ListObj.Add(val * dev.CT * dev.parI)
                            'ua~uca
                        Case i >= 4 And i <= 9
                            ListObj.Add(val * dev.PT * dev.parU)
                            'p,q,s
                        Case i = 10 Or i = 11 Or i = 13
                            ListObj.Add(val * dev.PT * dev.CT * dev.parP)
                            'f
                        Case i = 12
                            ListObj.Add(val * dev.PT * dev.CT * dev.VAL4)
                            'cos
                        Case i = 14
                            ListObj.Add(val * dev.VAL5)
                        Case Else
                            ListObj.Add(val)
                    End Select
                Next

                Return ListObj
            End If
            Return Nothing
        End Function

        Private Shared Function GetData_Wbh_822A(ByVal dev As Comm103DevState, ByVal obj As Object) As Object
            If obj.bool Then

                Dim ListObj As New List(Of Object)

                For i = 1 To obj.rel.count
                    Dim val As Int16 = obj.rel(i - 1)
                    Select Case True
                        'ia~ic
                        Case i >= 1 And i <= 3
                            ListObj.Add(val * dev.CT * dev.parI)
                            'ua~uca
                        Case i >= 4 And i <= 9
                            ListObj.Add(val * dev.PT * dev.parU)
                            'p,q,s
                        Case i = 10 Or i = 11 Or i = 13
                            ListObj.Add(val * dev.PT * dev.CT * dev.parP)
                            'f
                        Case i = 12
                            ListObj.Add(val * dev.PT * dev.CT * dev.VAL4)
                            'cos
                        Case i = 14
                            ListObj.Add(val * dev.VAL5)
                        Case Else
                            ListObj.Add(val)
                    End Select
                Next

                Return ListObj
            End If
            Return Nothing
        End Function

        Private Shared Function GetData_Wcb_822A(ByVal dev As Comm103DevState, ByVal obj As Object) As Object
            If obj.bool Then

                Dim ListObj As New List(Of Object)

                For i = 1 To obj.rel.count
                    Dim val As Int16 = obj.rel(i - 1)
                    Select Case True
                        'ia~ic
                        Case i >= 1 And i <= 3
                            ListObj.Add(val * dev.CT * dev.parI)
                            'ua~uc
                        Case i >= 4 And i <= 6
                            ListObj.Add(val * dev.PT * dev.parU)
                            'uab~uca
                        Case i >= 11 And i <= 13
                            ListObj.Add(val * dev.PT * dev.parU)
                            'p,q
                        Case i = 7 Or i = 8
                            ListObj.Add(val * dev.PT * dev.CT * dev.parP)
                            'f
                        Case i = 9
                            ListObj.Add(val * dev.PT * dev.CT * dev.VAL4)
                            'cos
                        Case i = 10
                            ListObj.Add(val * dev.VAL5)
                        Case Else
                            ListObj.Add(val)
                    End Select
                Next

                Return ListObj
            End If
            Return Nothing
        End Function

        Private Shared Function GetData_Wxh_822A(ByVal dev As Comm103DevState, ByVal obj As Object) As Object
            If obj.bool Then

                Dim ListObj As New List(Of Object)

                For i = 1 To obj.rel.count
                    Dim val As Int16 = obj.rel(i - 1)
                    Select Case True
                        'ia~ic
                        Case i >= 1 And i <= 3
                            ListObj.Add(val * dev.CT * dev.parI)
                            'ua~uca
                        Case i >= 4 And i <= 9
                            ListObj.Add(val * dev.PT * dev.parU)
                            'p,q,s
                        Case i = 10 Or i = 11 Or i = 13
                            ListObj.Add(val * dev.PT * dev.CT * dev.parP)
                            'f
                        Case i = 12
                            ListObj.Add(val * dev.PT * dev.CT * dev.VAL4)
                            'cos
                        Case i = 14
                            ListObj.Add(val * dev.VAL5)
                        Case Else
                            ListObj.Add(val)
                    End Select
                Next

                Return ListObj
            End If
            Return Nothing
        End Function



#End Region

#Region "万利达"
        Private Shared Function GetData_MMPR_610Hb(ByVal dev As Comm103DevState, ByVal obj As Object) As List(Of Comm103DevState.ValState)
            If obj.bool And obj.rel.count >= 13 Then
                '先移除首位,无用
                'obj.rel.RemoveAt(0)
                Dim ListVal As New List(Of Comm103DevState.ValState)
                Dim index As Integer = 0
                For Each ea In obj.rel
                    Select Case index
                        'Ia
                        Case 0
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Ia", ea * dev.CT))
                            'Ib
                        Case 1
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Ib", ea * dev.CT))
                            'Ic
                        Case 2
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Ic", ea * dev.CT))
                            'Uab
                        Case 3
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Uab", ea * dev.PT))
                            'Ubc
                        Case 4
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Ubc", ea * dev.PT))
                            'Uca
                        Case 5
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Uca", ea * dev.PT))
                            'U0
                        Case 6
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "U0", ea * dev.PT))
                            'I0
                        Case 7
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "I0", ea * dev.CT))
                            '零序方向
                        Case 8
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Zero", ea))
                            'P有功
                        Case 9
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "P", ea * dev.PT * dev.CT))
                            'Pm无功
                        Case 10
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Pm", ea * dev.PT * dev.CT))
                            'Pp功率因数
                        Case 11
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Pp", ea * dev.PT * dev.CT))
                            'F
                        Case 12
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "F", ea))
                    End Select
                    index = index + 1
                Next
                Return ListVal
            Else
                Return Nothing
            End If
        End Function

        Private Shared Function GetData_MLPR_610Hb(ByVal dev As Comm103DevState, ByVal obj As Object) As List(Of Comm103DevState.ValState)
            If obj.bool And obj.rel.count >= 15 Then
                ''先移除首位,无用
                'obj.rel.RemoveAt(0)
                Dim ListVal As New List(Of Comm103DevState.ValState)
                Dim index As Integer = 0
                For Each ea In obj.rel
                    Select Case index
                        'Ia
                        Case 0
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Ia", ea * dev.CT))
                            'Ib
                        Case 1
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Ib", ea * dev.CT))
                            'Ic
                        Case 2
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Ic", ea * dev.CT))
                            'Uab
                        Case 3
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Uab", ea * dev.PT))
                            'Ubc
                        Case 4
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Ubc", ea * dev.PT))
                            'Uca
                        Case 5
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Uca", ea * dev.PT))
                            'U0
                        Case 6
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "U0", ea * dev.PT))
                            'UL
                        Case 7
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "UL", ea * dev.PT))
                            'P有功
                        Case 8
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "P", ea * dev.PT * dev.CT))
                            'Pm无功
                        Case 9
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Pm", ea * dev.PT * dev.CT))
                            'Pp功率因数
                        Case 10
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Pp", ea * dev.PT * dev.CT))
                            'F1母线频率
                        Case 11
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "F1", ea))
                            'F2线路频率
                        Case 12
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "F2", ea))
                            'I0
                        Case 13
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "I0", ea * dev.CT))
                            '零序方向Zero
                        Case 14
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Zero", ea))
                    End Select

                    index = index + 1
                Next
                Return ListVal
            Else
                Return Nothing
            End If
        End Function


        Private Shared Function GetData_MTPR_610Hb(ByVal dev As Comm103DevState, ByVal obj As Object) As List(Of Comm103DevState.ValState)
            Return GetData_MMPR_610Hb(dev, obj)
        End Function

        Private Shared Function GetData_MTPR_650Hb(ByVal dev As Comm103DevState, ByVal obj As Object) As List(Of Comm103DevState.ValState)
            If obj.bool And obj.rel.count >= 19 Then
                '先移除首位,无用
                'obj.rel.RemoveAt(0)
                Dim ListVal As New List(Of Comm103DevState.ValState)
                Dim index As Integer = 0
                For Each ea In obj.rel
                    Select Case index
                        'Ia
                        Case 0
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Ia", ea * dev.CT))
                            'Ib
                        Case 1
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Ib", ea * dev.CT))
                            'Ic
                        Case 2
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Ic", ea * dev.CT))
                            'I0
                        Case 3
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "I0", ea * dev.CT))
                            'F
                        Case 4
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "F", ea))
                            'IA
                        Case 5
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "IA", ea * dev.CT))
                            'IB
                        Case 6
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "IB", ea * dev.CT))
                            'IC
                        Case 7
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "IC", ea * dev.CT))
                            'Ua
                        Case 8
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Ua", ea * dev.PT))
                            'Ub
                        Case 9
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Ub", ea * dev.PT))
                            'Uc
                        Case 10
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Uc", ea * dev.PT))
                            'Uab
                        Case 11
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Uab", ea * dev.PT))
                            'Ubc
                        Case 12
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Ubc", ea * dev.PT))
                            'Uca
                        Case 13
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Uca", ea * dev.PT))
                            'U2
                        Case 14
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "U2", ea * dev.PT))
                            'U0
                        Case 15
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "U0", ea * dev.PT))
                            'P
                        Case 16
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "P", ea * dev.PT * dev.CT))
                            'Q
                        Case 17
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "Q", ea * dev.PT * dev.CT))
                            'Cos
                        Case 18
                            ListVal.Add(New Comm103DevState.ValState(dev.Addr.ToString, "COS", ea))
                    End Select

                    index = index + 1
                Next
                Return ListVal
            Else
                Return Nothing
            End If
        End Function
#End Region

    End Class
End Namespace

