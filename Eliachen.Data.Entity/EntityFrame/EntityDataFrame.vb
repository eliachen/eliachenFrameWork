Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Reflection
Public Class EntityDataFrame
    '属性名称
    Public Property Name() As String
    '显示名称
    Public Property DisplayName() As String
    '类型名称
    Public Property TypeName() As String
    '是否主键
    Public Property isKey() As Boolean
    '是否必须
    Public Property isRequire() As Boolean
    Private m_isRequire As Boolean
    '校验规则:正则表达式
    Public Property Exp() As String

    Public Function GetFrame(_pros As PropertyInfo()) As List(Of EntityDataFrame)
        Dim l As New List(Of EntityDataFrame)()

        For Each pro In _pros
            Dim tmp As New EntityDataFrame
            '获取属性本身名称
            tmp.Name = pro.Name
            '获取类型名称
            tmp.TypeName = pro.PropertyType.ToString()
            '获取自定义属性
            Dim ProDis = pro.GetCustomAttributes(True)

            '显示名称设置
            Dim getdisplayname = (From s In ProDis Where s.[GetType]().FullName = "System.ComponentModel.DataAnnotations.DisplayAttribute").FirstOrDefault()
            If getdisplayname IsNot Nothing Then

                tmp.DisplayName = getdisplayname.[GetType]().GetProperty("Name").GetValue(getdisplayname, Nothing).ToString()
            Else
                tmp.DisplayName = tmp.Name
            End If

            '获取主键
            Dim getkey = (From s In ProDis Where s.[GetType]().FullName = "System.ComponentModel.DataAnnotations.KeyAttribute").FirstOrDefault()
            '主键设置
            If getkey IsNot Nothing Then
                tmp.isKey = True
                tmp.isRequire = True
            Else
                tmp.isKey = False
            End If

            '必填项目设置
            If Not tmp.isKey Then
                Dim getrequire = (From s In ProDis Where s.[GetType]().FullName = "System.ComponentModel.DataAnnotations.RequiredAttribute").FirstOrDefault()
                If getrequire IsNot Nothing Then
                    tmp.isRequire = True
                Else
                    tmp.isRequire = False
                End If
            End If


            '正则表达式设置
            Dim getreg = (From s In ProDis Where s.[GetType]().FullName = "System.ComponentModel.DataAnnotations.RegularExpressionAttribute").FirstOrDefault()
            If getreg IsNot Nothing Then
                tmp.Exp = getreg.[GetType]().GetProperty("Pattern").GetValue(getreg, Nothing).ToString()
            Else
                tmp.Exp = ""
            End If


            l.Add(tmp)
        Next
        Return l
    End Function
End Class
