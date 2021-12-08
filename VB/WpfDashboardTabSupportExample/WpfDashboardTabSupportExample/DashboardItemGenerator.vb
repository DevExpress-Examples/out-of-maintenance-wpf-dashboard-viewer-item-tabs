Imports DevExpress.DashboardCommon

Namespace WpfDashboardTabSupportExample

    Public Module DashboardItemGenerator

        Public Function GenerateCardItem(ByVal dataSource As IDashboardDataSource, ByVal itemComponentName As String) As DashboardItem
            Dim cardItem As CardDashboardItem = New CardDashboardItem() With {.ComponentName = itemComponentName}
            cardItem.SeriesDimensions.Add(New Dimension("Country"))
            cardItem.SparklineArgument = New Dimension("SalesDate", DateTimeGroupInterval.DayMonthYear)
            cardItem.DataSource = dataSource
            cardItem.ShowCaption = False
            Dim actualValue As Measure = New Measure("Sales")
            actualValue.NumericFormat.FormatType = DataItemNumericFormatType.Currency
            Dim targetValue As Measure = New Measure("SalesTarget")
            targetValue.NumericFormat.FormatType = DataItemNumericFormatType.Currency
            Dim card As Card = New Card(actualValue, targetValue)
            card.LayoutTemplate = New CardCompactLayoutTemplate()
            cardItem.Cards.Add(card)
            Return cardItem
        End Function

        Public Function GenerateListBoxItem(ByVal dataSource As IDashboardDataSource, ByVal itemComponentName As String) As DashboardItem
            Dim listBoxItem As ListBoxDashboardItem = New ListBoxDashboardItem() With {.ComponentName = itemComponentName}
            listBoxItem.Name = "Countries"
            listBoxItem.DataSource = dataSource
            listBoxItem.FilterDimensions.Add(New Dimension("Country"))
            listBoxItem.InteractivityOptions.IgnoreMasterFilters = False
            Return listBoxItem
        End Function

        Public Function GeneratePieItem(ByVal dataSource As IDashboardDataSource, ByVal itemComponentName As String) As DashboardItem
            Dim pieItem As PieDashboardItem = New PieDashboardItem() With {.ComponentName = itemComponentName}
            pieItem.DataSource = dataSource
            pieItem.Values.Add(New Measure("Sales"))
            pieItem.Arguments.Add(New Dimension("Country"))
            pieItem.SeriesDimensions.Add(New Dimension("SalesDate"))
            Return pieItem
        End Function

        Public Function GenerateGridItem(ByVal dataSource As IDashboardDataSource, ByVal itemComponentName As String) As DashboardItem
            Dim gridItem As GridDashboardItem = New GridDashboardItem() With {.ComponentName = itemComponentName}
            gridItem.DataSource = dataSource
            gridItem.Columns.Add(New GridDimensionColumn(New Dimension("Country")))
            gridItem.Columns.Add(New GridMeasureColumn(New Measure("Sales")))
            gridItem.Columns.Add(New GridDeltaColumn(New Measure("Sales"), New Measure("SalesTarget")))
            gridItem.Columns.Add(New GridSparklineColumn(New Measure("Sales")))
            gridItem.SparklineArgument = New Dimension("SalesDate", DateTimeGroupInterval.MonthYear)
            Return gridItem
        End Function
    End Module
End Namespace
