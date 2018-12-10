Imports DevExpress.DashboardCommon
Imports DevExpress.Xpf.Bars
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes

Namespace WpfDashboardTabSupportExample
    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Partial Public Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            DataContext = Me

            dashboardControl.Dashboard = CreateSimpleDashboard()
            AddHandler dashboardControl.Dashboard.DataLoading, AddressOf Dashboard_DataLoading
        End Sub

        Private Sub Dashboard_DataLoading(ByVal sender As Object, ByVal e As DashboardDataLoadingEventArgs)
            e.Data = DataGenerator.GenerateTestData()
        End Sub

        Private Function CreateSimpleDashboard() As Dashboard
            Dim dashboard As New Dashboard()
            Dim dataSource As New DashboardObjectDataSource()
            dashboard.DataSources.Add(dataSource)

            Dim gridItem As New GridDashboardItem() With {.ComponentName = "grid1"}
            gridItem.DataSource = dataSource
            gridItem.Columns.Add(New GridDimensionColumn(New Dimension("Country")))
            gridItem.Columns.Add(New GridMeasureColumn(New Measure("Sales")))

            Dim pieItem As New PieDashboardItem() With {.ComponentName = "pie1"}
            pieItem.DataSource = dataSource
            pieItem.Values.Add(New Measure("Sales"))
            pieItem.Arguments.Add(New Dimension("Country"))

            dashboard.Items.AddRange(gridItem, pieItem)

            Return dashboard
        End Function

        Private Sub btnCreate_ItemClick(ByVal sender As Object, ByVal e As DevExpress.Xpf.Bars.ItemClickEventArgs)
            Dim dashboard As New Dashboard()
            Dim dataSource As New DashboardObjectDataSource()

            dashboard.BeginUpdate()
            dashboard.DataSources.Add(dataSource)

            dashboard.Items.Add(DashboardItemGenerator.GenerateCardItem(dataSource, "card1"))
            dashboard.Items.Add(DashboardItemGenerator.GenerateGridItem(dataSource, "grid1"))
            dashboard.Items.Add(DashboardItemGenerator.GeneratePieItem(dataSource, "pie1"))
            dashboard.Items.Add(DashboardItemGenerator.GenerateListBoxItem(dataSource, "list1"))

            Dim tabContainer As New TabContainerDashboardItem()
            tabContainer.TabPages.Add(New DashboardTabPage() With { _
                .Name = "Tab Page One", _
                .ComponentName = "page1" _
            })
            tabContainer.TabPages("page1").AddRange(dashboard.Items("grid1"), dashboard.Items("pie1"))

            Dim secondTabPage As DashboardTabPage = tabContainer.CreateTabPage()
            secondTabPage.Name = "Tab Page Two"
            secondTabPage.Add(dashboard.Items("list1"))
            secondTabPage.ShowItemAsTabPage = True

            dashboard.Items.Add(tabContainer)

            AddHandler dashboard.DataLoading, AddressOf Dashboard_DataLoading

            dashboard.EndUpdate()

            dashboardControl.Dashboard = dashboard
        End Sub

        Private Sub btnModify_ItemClick(ByVal sender As Object, ByVal e As DevExpress.Xpf.Bars.ItemClickEventArgs)
            Dim dashboard As New Dashboard()
            dashboard.LoadFromXDocument(dashboardControl.Dashboard.SaveToXDocument())

            dashboard.BeginUpdate()
            Try
                dashboard.Items("card1").ParentContainer = dashboard.Items("list1").ParentContainer
            Catch e1 As NullReferenceException
            End Try

            dashboard.Items.ForEach(Sub(item As DashboardItem)
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

            dashboardControl.Dashboard = dashboard
        End Sub
    End Class
End Namespace
