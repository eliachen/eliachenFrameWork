Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices

Namespace EliaChen.Config
    Public Class IniFile
        '文件的地址
        Public filePath As String
        '程序运行的地址,结尾时"\"
        Public Shared App As String = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase

        <DllImport("kernel32")> _
        Private Shared Function WritePrivateProfileString(ByVal section As String, ByVal key As String, ByVal val As String, ByVal filePath As String) As Long
        End Function

        <DllImport("kernel32")> _
        Private Shared Function GetPrivateProfileString(ByVal section As String, ByVal key As String, ByVal def As String, ByVal retVal As StringBuilder, ByVal size As Integer, ByVal filePath As String) As Integer
        End Function
        Public Sub New(ByVal iniPath As String)
            filePath = iniPath
        End Sub

        Public Sub WriteIniValue(ByVal Section As String, ByVal Key As String, ByVal value As String)
            WritePrivateProfileString(Section, Key, value, Me.filePath)
        End Sub

        Public Function ReadIniValue(ByVal Section As String, ByVal Key As String) As String
            Dim temp As New StringBuilder(255)
            Dim i As Integer = GetPrivateProfileString(Section, Key, "", temp, 255, Me.filePath)
            Return temp.ToString()
        End Function
    End Class
End Namespace
