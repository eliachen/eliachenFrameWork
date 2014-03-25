Imports System.Runtime.InteropServices
Imports System.Text
Namespace Communication.CommPort.Port
    Public Module DdpAPI
        '结构定义
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
        Public Structure GPRS_DATA_RECORD
            '终端模块号码
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=12)>
            Public m_userid As String
            '接收到数据包的时间 '这里做了修改，转换时由ByValTStr变为ByValArray类型
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=20)>
            Public m_recv_date As String
            '存储接收到的数据 
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=1024)>
            Public m_data_buf As Byte()
            '接收到的数据包长度 
            Public m_data_len As UShort
            '接收到的数据包类型 
            Public m_data_type As Byte

            Public Sub Initialize()
                '初始化byte[]的字段 
                m_data_buf = New Byte(1023) {}
                'UnmanagedType.LPStr
            End Sub
        End Structure


        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
        Public Structure GPRS_USER_INFO
            '终端模块号码  
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=12)>
            Public m_userid As String

            '终端模块进入Internet的代理主机IP地址
            Public m_sin_addr As UInteger
            '终端模块进入Internet的代理主机IP端口
            Public m_sin_port As UShort

            '终端模块在移动网内IP地址 
            Public m_local_addr As UInteger
            '终端模块在移动网内IP端口 
            Public m_local_port As UShort
            '终端模块登录时间 '这里做了修改，转换时由ByValTStr变为ByValArray类型
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=20)>
            Public m_logon_date As String
            '终端用户更新时间 
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=20)>
            Public m_update_time As Byte()
            '终端模块状态, 1 在线 0 不在线 
            '[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)] 
            ' public string m_update_time;
            Public m_status As Byte

            Public Sub Initialize()
                '初始化byte[]的字段  
                m_update_time = New Byte(19) {}
            End Sub

        End Structure


        '启动服务
        <DllImport(".\wcomm_dll.dll")>
        Public Function start_gprs_server(ByVal hWnd As IntPtr, _
                                                 ByVal wMsg As Integer, _
                                                 ByVal nServerPort As Integer, _
                                                 <MarshalAs(UnmanagedType.LPStr)> _
                                                 ByVal mess As StringBuilder) As Integer
        End Function

        '启动服务
        <DllImport(".\wcomm_dll.dll")>
        Public Function start_net_service(ByVal hWnd As IntPtr, _
                                                 ByVal wMsg As Integer, _
                                                 ByVal nServerPort As Integer, _
                                                 <MarshalAs(UnmanagedType.LPStr)> _
                                                 ByVal mess As StringBuilder) As Integer
        End Function


        '停止服务
        <DllImport(".\wcomm_dll.dll")>
        Public Function stop_gprs_server(<MarshalAs(UnmanagedType.LPStr)> ByVal mess As StringBuilder) As Integer
        End Function

        '停止服务
        <DllImport(".\wcomm_dll.dll")>
        Public Function stop_net_service(<MarshalAs(UnmanagedType.LPStr)> ByVal mess As StringBuilder) As Integer
        End Function

        '读取数据
        <DllImport(".\wcomm_dll.dll")>
        Public Function do_read_proc(ByRef recdPtr As GPRS_DATA_RECORD, _
                                            <MarshalAs(UnmanagedType.LPStr)> ByVal mess As StringBuilder, _
                                            ByVal reply As Boolean) As Integer
        End Function

        '发送数据
        '[MarshalAs(UnmanagedType.LPStr)]'string data,
        <DllImport(".\wcomm_dll.dll")>
        Public Function do_send_user_data(<MarshalAs(UnmanagedType.LPStr)> ByVal userid As String, _
                                                 ByVal data As Byte(), _
                                                 ByVal len As Integer, _
                                                 <MarshalAs(UnmanagedType.LPStr)> ByVal mess As StringBuilder) As Integer
        End Function

        '获取终端信息
        <DllImport(".\wcomm_dll.dll")>
        Public Function get_user_info(<MarshalAs(UnmanagedType.LPStr)> ByVal userid As String, _
                                             ByRef infoPtr As GPRS_USER_INFO) As Integer
        End Function

        '设置服务模式
        <DllImport(".\wcomm_dll.dll")>
        Public Function SetWorkMode(ByVal nWorkMode As Integer) As Integer
        End Function
        '取消阻塞读取
        <DllImport(".\wcomm_dll.dll")>
        Public Sub cancel_read_block()
        End Sub
        '使某个DTU下线
        <DllImport(".\wcomm_dll.dll")>
        Public Function do_close_one_user(<MarshalAs(UnmanagedType.LPStr)> ByVal userid As String, _
                                         <MarshalAs(UnmanagedType.LPStr)> ByVal mess As StringBuilder) As Integer
        End Function
        '使所有DTU下线
        <DllImport(".\wcomm_dll.dll")>
        Public Function do_close_all_user(<MarshalAs(UnmanagedType.LPStr)> ByVal mess As StringBuilder) As Integer
        End Function
        '使某个DTU下线
        <DllImport(".\wcomm_dll.dll")>
        Public Function do_close_one_user2(<MarshalAs(UnmanagedType.LPStr)> ByVal userid As String, _
                                          <MarshalAs(UnmanagedType.LPStr)> ByVal mess As StringBuilder) As Integer
        End Function
        '使所有DTU下线<DllImport(".\wcomm_dll.dll")> 
        Public Function do_close_all_user2(<MarshalAs(UnmanagedType.LPStr)> ByVal mess As StringBuilder) As Integer
        End Function
        '设置服务类型
        <DllImport(".\wcomm_dll.dll")>
        Public Function SelectProtocol(ByVal nProtocol As Integer) As Integer
        End Function
        '指定服务IP
        <DllImport(".\wcomm_dll.dll")>
        Public Sub SetCustomIP(ByVal IP As Integer)
        End Sub
        '获得最大DTU连接数量
        <DllImport(".\wcomm_dll.dll")>
        Public Function get_max_user_amount() As UInteger
        End Function
        '获取终端信息<DllImport(".\wcomm_dll.dll")> 
        Public Function get_user_at(ByVal index As UInteger, ByRef infoPtr As GPRS_USER_INFO) As Integer
        End Function
        '定义一些SOCKET API函数
        <DllImport("Ws2_32.dll")>
        Public Function inet_addr(ByVal ip As String) As Int32
        End Function
        <DllImport("Ws2_32.dll")>
        Public Function inet_ntoa(ByVal ip As UInteger) As String
        End Function
        <DllImport("Ws2_32.dll")>
        Public Function htonl(ByVal ip As UInteger) As UInteger
        End Function
        <DllImport("Ws2_32.dll")>
        Public Function ntohl(ByVal ip As UInteger) As UInteger
        End Function
        <DllImport("Ws2_32.dll")>
        Public Function htons(ByVal ip As UShort) As UShort
        End Function
        <DllImport("Ws2_32.dll")>
        Public Function ntohs(ByVal ip As UShort) As UShort
        End Function
        Public Const WM_DTU As Integer = &H400 + 100
    End Module
End Namespace

