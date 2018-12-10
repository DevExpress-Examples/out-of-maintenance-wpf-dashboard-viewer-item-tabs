Imports System
Imports System.Collections.Generic
Imports System.IO

Namespace WpfDashboardTabSupportExample
    Public NotInheritable Class DataGenerator

        Private Sub New()
        End Sub

        Public Shared Function GenerateTestData() As List(Of DataRow)
            Dim data As New List(Of DataRow)()
            Dim rand As New Random(Date.Now.Second)

            Dim countries = New String() { "USA", "Canada", "Argentina", "Brazil" }
            For Each country As String In countries
                For i As Integer = 0 To 99
                    data.Add(New DataRow With { _
                        .Country = country, _
                        .Sales = rand.NextDouble() * 100 * i, _
                        .SalesTarget = rand.NextDouble() * 100 * i, _
                        .SalesDate = Date.Now.AddDays(((If(i Mod 2 = 0, -1, 1)) * i) + rand.Next(10, 40)) _
                    })
                Next i
            Next country
            Return data
        End Function

        Public Shared Sub SaveTestData()
            Dim data As List(Of DataRow) = GenerateTestData()
            Using stream = New StreamWriter("data.csv") With {.NewLine = vbLf}
                stream.WriteLine("Country,Sales,SalesTarget,SalesDate")
                For Each datarow In data
                    stream.WriteLine(datarow.ToCsv())
                Next datarow
            End Using
        End Sub

    End Class

    Public Class DataRow
        Public Property Country() As String
        Public Property Sales() As Double
        Public Property SalesTarget() As Double
        Public Property SalesDate() As Date

        Public Function ToCsv() As String
            Return String.Join(",", Me.Country, Me.Sales.ToString(), Me.SalesTarget.ToString(), Me.SalesDate.ToString())
        End Function
    End Class
End Namespace