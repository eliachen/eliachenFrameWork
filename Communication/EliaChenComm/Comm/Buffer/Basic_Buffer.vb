Namespace Communication.CommInterface.myCommWay
    Public MustInherit Class Basic_Buffer
        Implements Communication.CommInterface.ICommBufferElement

        Public Overridable Function CheckBufferObj(ByVal ArrByte() As Byte) As Boolean Implements ICommBufferElement.CheckBufferElement

        End Function

        Public Overridable Function GetBufferObj(ByVal ArrByte() As Byte) As ICommBufferElement Implements ICommBufferElement.GetBufferElement

        End Function
    End Class
End Namespace

