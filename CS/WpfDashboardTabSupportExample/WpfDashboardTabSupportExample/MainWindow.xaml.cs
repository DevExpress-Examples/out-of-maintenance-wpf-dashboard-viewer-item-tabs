using DevExpress.DashboardCommon;
using DevExpress.Xpf.Bars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDashboardTabSupportExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            dashboardControl.Dashboard = CreateSimpleDashboard();
            dashboardControl.Dashboard.DataLoading += Dashboard_DataLoading;
        }

        private void Dashboard_DataLoading(object sender, DashboardDataLoadingEventArgs e)
        {
            e.Data = DataGenerator.GenerateTestData();
        }

        private Dashboard CreateSimpleDashboard()
        {
            Dashboard dashboard = new Dashboard();
            DashboardObjectDataSource dataSource = new DashboardObjectDataSource();
            dashboard.DataSources.Add(dataSource);

            GridDashboardItem gridItem = new GridDashboardItem() { ComponentName = "grid1" };
            gridItem.DataSource = dataSource;
            gridItem.Columns.Add(new GridDimensionColumn(new Dimension("Country")));
            gridItem.Columns.Add(new GridMeasureColumn(new Measure("Sales")));

            PieDashboardItem pieItem = new PieDashboardItem() { ComponentName = "pie1" };
            pieItem.DataSource = dataSource;
            pieItem.Values.Add(new Measure("Sales"));
            pieItem.Arguments.Add(new Dimension("Country"));

            dashboard.Items.AddRange(gridItem, pieItem);

            return dashboard;
        }

        private void btnCreate_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            DashboardObjectDataSource dataSource = new DashboardObjectDataSource();

            dashboard.BeginUpdate();
            dashboard.DataSources.Add(dataSource);

            dashboard.Items.Add(DashboardItemGenerator.GenerateCardItem(dataSource, "card1"));
            dashboard.Items.Add(DashboardItemGenerator.GenerateGridItem(dataSource, "grid1"));
            dashboard.Items.Add(DashboardItemGenerator.GeneratePieItem(dataSource, "pie1"));
            dashboard.Items.Add(DashboardItemGenerator.GenerateListBoxItem(dataSource, "list1"));

            TabContainerDashboardItem tabContainer = new TabContainerDashboardItem();
            tabContainer.TabPages.Add(new DashboardTabPage() { Name = "Tab Page One", ComponentName = "page1" });
            tabContainer.TabPages["page1"].AddRange(dashboard.Items["grid1"], dashboard.Items["pie1"]);

            DashboardTabPage secondTabPage = tabContainer.CreateTabPage();
            secondTabPage.Name = "Tab Page Two";
            secondTabPage.Add(dashboard.Items["list1"]);
            secondTabPage.ShowItemAsTabPage = true;

            dashboard.Items.Add(tabContainer);

            dashboard.DataLoading += Dashboard_DataLoading;

            dashboard.EndUpdate();

            dashboardControl.Dashboard = dashboard;
        }

        private void btnModify_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.LoadFromXDocument(dashboardControl.Dashboard.SaveToXDocument());

            dashboard.BeginUpdate();
            try
            {
                dashboard.Items["card1"].ParentContainer = dashboard.Items["list1"].ParentContainer;
            }
            catch (NullReferenceException) { };

            dashboard.Items.ForEach(delegate (DashboardItem item)
            {
                if (item is PieDashboardItem)
                {
                    ((PieDashboardItem)item).PieType = PieType.Donut;
                }
            });

            dashboard.DataLoading += Dashboard_DataLoading;
            // The RebuildLayout call is required because the WPF viewer control contains a dashboard whose layout is already built.
            // If the layout is null (dashboardControl.Dashboard = null or dashboardControl.Dashboard.LayoutRoot = null), 
            // the RebuildLayout is called automatically after assigning a dashboard.
            dashboard.RebuildLayout();
            dashboard.EndUpdate();          
            
            dashboardControl.Dashboard = dashboard;
        }
    }
}
