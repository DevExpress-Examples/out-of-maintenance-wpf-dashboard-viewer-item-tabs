using DevExpress.DashboardCommon;

namespace WpfDashboardTabSupportExample
{
    public static class DashboardItemGenerator
    {
        public static DashboardItem GenerateCardItem(IDashboardDataSource dataSource, string itemComponentName) {
            CardDashboardItem cardItem = new CardDashboardItem() {ComponentName= itemComponentName};
            cardItem.SeriesDimensions.Add(new Dimension("Country"));
            cardItem.SparklineArgument = new Dimension("SalesDate", DateTimeGroupInterval.DayMonthYear);
            cardItem.DataSource = dataSource;
            cardItem.ShowCaption = false;

            Measure actualValue = new Measure("Sales");
            actualValue.NumericFormat.FormatType = DataItemNumericFormatType.Currency;
            Measure targetValue = new Measure("SalesTarget");
            targetValue.NumericFormat.FormatType = DataItemNumericFormatType.Currency;
            Card card = new Card(actualValue, targetValue);
            card.LayoutTemplate = new CardCompactLayoutTemplate();
            cardItem.Cards.Add(card);

            return cardItem;
        }

        public static DashboardItem GenerateListBoxItem(IDashboardDataSource dataSource, string itemComponentName)
        {
            ListBoxDashboardItem listBoxItem = new ListBoxDashboardItem() { ComponentName = itemComponentName };
            listBoxItem.Name = "Countries";
            listBoxItem.DataSource = dataSource;
            listBoxItem.FilterDimensions.Add(new Dimension("Country"));
            listBoxItem.InteractivityOptions.IgnoreMasterFilters = false;

            return listBoxItem;
        }

        public static DashboardItem GeneratePieItem(IDashboardDataSource dataSource, string itemComponentName)
        {
            PieDashboardItem pieItem = new PieDashboardItem() { ComponentName = itemComponentName };
            pieItem.DataSource = dataSource;
            pieItem.Values.Add(new Measure("Sales"));
            pieItem.Arguments.Add(new Dimension("Country"));
            pieItem.SeriesDimensions.Add(new Dimension("SalesDate"));

            return pieItem;
        }

        public static DashboardItem GenerateGridItem(IDashboardDataSource dataSource, string itemComponentName)
        {
            GridDashboardItem gridItem = new GridDashboardItem() { ComponentName = itemComponentName };
            gridItem.DataSource = dataSource;

            gridItem.Columns.Add(new GridDimensionColumn(new Dimension("Country")));
            gridItem.Columns.Add(new GridMeasureColumn(new Measure("Sales")));
            gridItem.Columns.Add(new GridDeltaColumn(new Measure("Sales"), new Measure("SalesTarget")));
            gridItem.Columns.Add(new GridSparklineColumn(new Measure("Sales")));
            gridItem.SparklineArgument = new Dimension("SalesDate", DateTimeGroupInterval.MonthYear);

            return gridItem;
        }



    }
}
