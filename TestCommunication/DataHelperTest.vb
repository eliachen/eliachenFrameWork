Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports EliaChen.CommProtocol.Common



'''<summary>
'''这是 DataHelperTest 的测试类，旨在
'''包含所有 DataHelperTest 单元测试
'''</summary>
<TestClass()> _
Public Class DataHelperTest


    Private testContextInstance As TestContext

    '''<summary>
    '''获取或设置测试上下文，上下文提供
    '''有关当前测试运行及其功能的信息。
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(value As TestContext)
            testContextInstance = Value
        End Set
    End Property

#Region "附加测试特性"
    '
    '编写测试时，还可使用以下特性:
    '
    '使用 ClassInitialize 在运行类中的第一个测试前先运行代码
    '<ClassInitialize()>  _
    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    'End Sub
    '
    '使用 ClassCleanup 在运行完类中的所有测试后再运行代码
    '<ClassCleanup()>  _
    'Public Shared Sub MyClassCleanup()
    'End Sub
    '
    '使用 TestInitialize 在运行每个测试前先运行代码
    '<TestInitialize()>  _
    'Public Sub MyTestInitialize()
    'End Sub
    '
    '使用 TestCleanup 在运行完每个测试后运行代码
    '<TestCleanup()>  _
    'Public Sub MyTestCleanup()
    'End Sub
    '
#End Region


    '''<summary>
    '''CheckTwoByte 的测试
    '''</summary>
    <TestMethod()> _
    Public Sub CheckTwoByteTest()
        Dim _d1() As Byte = New Byte() {1, 2, 3} ' TODO: 初始化为适当的值
        Dim _d2() As Byte = New Byte() {1, 2, 3} ' TODO: 初始化为适当的值
        Dim expected As Boolean = False ' TODO: 初始化为适当的值
        Dim actual As Boolean
        actual = DataHelper.CheckTwoByte(_d1, _d2)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("验证此测试方法的正确性。")
    End Sub
End Class
