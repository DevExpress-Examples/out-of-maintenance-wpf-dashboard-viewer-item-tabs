Imports System
Imports System.Collections.Generic
Imports System.IO

Namespace WpfDashboardTabSupportExample

    Public Module DataGenerator

        Public Function GenerateTestData() As List(Of WpfDashboardTabSupportExample.DataRow)
            Dim data As System.Collections.Generic.List(Of WpfDashboardTabSupportExample.DataRow) = New System.Collections.Generic.List(Of WpfDashboardTabSupportExample.DataRow)()
            Dim rand As System.Random = New System.Random(System.DateTime.Now.Second)
            Dim countries = New String() {"USA", "Canada", "Argentina", "Brazil"}
            For Each country As String In countries
                For i As Integer = 0 To 100 - 1
                    data.Add(New WpfDashboardTabSupportExample.DataRow With {.Country = country, .Sales = rand.NextDouble() * 100 * i, .SalesTarget = rand.NextDouble() * 100 * i, .SalesDate = System.DateTime.Now.AddDays(((If(i Mod 2 = 0, -1, 1)) * i) + rand.[Next](10, 40))})
                Next
            Next

            Return data
        End Function

        Public Sub SaveTestData()
            Dim data As System.Collections.Generic.List(Of WpfDashboardTabSupportExample.DataRow) = WpfDashboardTabSupportExample.DataGenerator.GenerateTestData()
            Using stream = New System.IO.StreamWriter("data.csv") With {.NewLine = Global.Microsoft.VisualBasic.Constants.vbLf}
                stream.WriteLine("Country,Sales,SalesTarget,SalesDate")
                For Each datarow In data
                    stream.WriteLine(datarow.ToCsv())
                Next
            End Using
        End Sub
    End Module

    Public Class DataRow

        Public Property Country As String

        Public Property Sales As Double

        Public Property SalesTarget As Double

        Public Property SalesDate As DateTime

        Public Function ToCsv() As String
            Return System.[String].Join(",", Me.Country, Me.Sales.ToString(), Me.SalesTarget.ToString(), Me.SalesDate.ToString())
        End Function
    End Class
End Namespace
