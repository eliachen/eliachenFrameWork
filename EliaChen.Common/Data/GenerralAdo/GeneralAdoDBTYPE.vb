
Namespace Data
    Partial Public Class GeneralAdo

        Public Enum GeneralAdoDBTYPE
            DB_ODBC = 0
            DB_SQL = 1
            DB_OLEDB = 2
            DB_ORCALE = 3
            DB_OTHER = 4
        End Enum
        ''' <summary>
        ''' 获取对应数据库类型的命名空间全称
        ''' </summary>
        ''' <param name="_DBType">数据库类型</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function GetProvName(ByVal _DBType As GeneralAdoDBTYPE) As String
            Select Case _DBType
                Case GeneralAdoDBTYPE.DB_ODBC
                    Return "System.Data.Odbc"
                Case GeneralAdoDBTYPE.DB_SQL
                    Return "System.Data.SqlClient"
                Case GeneralAdoDBTYPE.DB_OLEDB
                    Return "System.Data.OleDb"
                Case GeneralAdoDBTYPE.DB_ORCALE
                    Return "System.Data.OracleClient"
                Case GeneralAdoDBTYPE.DB_OTHER
                    Return ""
                Case Else
                    Return ""
            End Select
        End Function
    End Class
End Namespace

