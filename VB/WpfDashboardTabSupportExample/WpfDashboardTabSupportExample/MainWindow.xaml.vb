Imports DevExpress.DashboardCommon
Imports DevExpress.Xpf.Bars
Imports System
Imports System.ComponentModel
Imports System.Windows
Imports System.Windows.Controls

Namespace WpfDashboardTabSupportExample

    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            Me.InitializeComponent()
            DataContext = Me
            Me.dashboardControl.Dashboard = CreateSimpleDashboard()
            AddHandler Me.dashboardControl.Dashboard.DataLoading, AddressOf Me.Dashboard_DataLoading
        End Sub

        Private Sub Dashboard_DataLoading(ByVal sender As Object, ByVal e As DashboardDataLoadingEventArgs)
            e.Data = GenerateTestData()
        End Sub

        Private Function CreateSimpleDashboard() As Dashboard
            Dim dashboard As Dashboard = New Dashboard()
            Dim dataSource As DashboardObjectDataSource = New DashboardObjectDataSource()
            dashboard.DataSources.Add(dataSource)
            Dim gridItem As GridDashboardItem = New GridDashboardItem() With {.ComponentName = "grid1"}
            gridItem.DataSource = dataSource
            gridItem.Columns.Add(New GridDimensionColumn(New Dimension("Country")))
            gridItem.Columns.Add(New GridMeasureColumn(New Measure("Sales")))
            Dim pieItem As PieDashboardItem = New PieDashboardItem() With {.ComponentName = "pie1"}
            pieItem.DataSource = dataSource
            pieItem.Values.Add(New Measure("Sales"))
            pieItem.Arguments.Add(New Dimension("Country"))
            dashboard.Items.AddRange(gridItem, pieItem)
            Return dashboard
        End Function

        Private Sub btnCreate_ItemClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            Dim dashboard As Dashboard = New Dashboard()
            Dim dataSource As DashboardObjectDataSource = New DashboardObjectDataSource()
            dashboard.BeginUpdate()
            dashboard.DataSources.Add(dataSource)
            dashboard.Items.Add(GenerateCardItem(dataSource, "card1"))
            dashboard.Items.Add(GenerateGridItem(dataSource, "grid1"))
            dashboard.Items.Add(GeneratePieItem(dataSource, "pie1"))
            dashboard.Items.Add(GenerateListBoxItem(dataSource, "list1"))
            Dim tabContainer As TabContainerDashboardItem = New TabContainerDashboardItem()
            tabContainer.TabPages.Add(New DashboardTabPage() With {.Name = "Tab Page One", .ComponentName = "page1"})
            tabContainer.TabPages("page1").AddRange(dashboard.Items("grid1"), dashboard.Items("pie1"))
            Dim secondTabPage As DashboardTabPage = tabContainer.CreateTabPage()
            secondTabPage.Name = "Tab Page Two"
            secondTabPage.Add(dashboard.Items("list1"))
            secondTabPage.ShowItemAsTabPage = True
            dashboard.Items.Add(tabContainer)
            AddHandler dashboard.DataLoading, AddressOf Dashboard_DataLoading
            dashboard.EndUpdate()
            Me.dashboardControl.Dashboard = dashboard
        End Sub

        Private Sub btnModify_ItemClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            Dim dashboard As Dashboard = New Dashboard()
            dashboard.LoadFromXDocument(Me.dashboardControl.Dashboard.SaveToXDocument())
            dashboard.BeginUpdate()
            Try
                dashboard.Items("card1").ParentContainer = dashboard.Items("list1").ParentContainer
            Catch __unusedNullReferenceException1__ As NullReferenceException
            End Try

            dashboard.Items.ForEach(Sub(ByVal item)
                If TypeOf item Is PieDashboardItem Then
                    CType(item, PieDashboardItem).PieType = PieType.Donut
                End If
            End Sub)
            AddHandler dashboard.DataLoading, AddressOf Dashboard_DataLoading
            ' The RebuildLayout call is required because the WPF viewer control contains a dashboard whose layout is already built.
            ' If the layout is null (dashboardControl.Dashboard = null or dashboardControl.Dashboard.LayoutRoot = null), 
            ' the RebuildLayout is called automatically after assigning a dashboard.
            dashboard.RebuildLayout()
            dashboard.EndUpdate()
            Me.dashboardControl.Dashboard = dashboard
        End Sub
    End Class
End Namespace
