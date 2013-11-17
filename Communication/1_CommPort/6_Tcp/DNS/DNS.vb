
Imports System.Net
Imports System.Net.Sockets
Namespace CommPort
    Public Class eDNS

        ''' <summary>
        ''' 通过DNS解析对应的IP地址
        ''' </summary>
        ''' <param name="Str"></param>
        ''' <returns>远程地址</returns>
        ''' <remarks></remarks>
        Public Shared Function GetAddr(ByVal Str As String) As IPAddress()
            Return Dns.GetHostAddresses(Str)
        End Function

        ''' <summary>
        ''' 通过DNS获取本机的ip地址
        ''' </summary>
        ''' <returns>本机地址的数组</returns>
        ''' <remarks></remarks>
        Public Shared Function GetLocalAddr() As IPAddress()
            Return Dns.GetHostAddresses(Dns.GetHostName)
        End Function
    End Class
End Namespace

