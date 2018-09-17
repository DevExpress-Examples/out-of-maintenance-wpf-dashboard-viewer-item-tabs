using System;
using System.Collections.Generic;
using System.IO;

namespace WpfDashboardTabSupportExample
{
    public static class DataGenerator
    {
        public static List<DataRow> GenerateTestData()
        {
            List<DataRow> data = new List<DataRow>();
            Random rand = new Random(DateTime.Now.Second);

            var countries = new string[] { "USA", "Canada", "Argentina", "Brazil" };
            foreach (string country in countries)
            {
                for (int i = 0; i < 100; i++)
                    data.Add(new DataRow
                    {
                        Country = country,
                        Sales = rand.NextDouble() * 100 * i,
                        SalesTarget = rand.NextDouble() * 100 * i,
                        SalesDate = DateTime.Now.AddDays(((i % 2 == 0 ? -1 : 1) * i) + rand.Next(10, 40))
                    });
            }
            return data;
        }

        public static void SaveTestData()
        {
            List<DataRow> data = GenerateTestData();
            using (var stream = new StreamWriter("data.csv") { NewLine = "\n" })
            {
                stream.WriteLine("Country,Sales,SalesTarget,SalesDate");
                foreach (var datarow in data)
                    stream.WriteLine(datarow.ToCsv());
            }
        }

    }

    public class DataRow
    {
        public string Country { get; set; }
        public double Sales { get; set; }
        public double SalesTarget { get; set; }
        public DateTime SalesDate { get; set; }

        public string ToCsv() {
            return String.Join(",", this.Country, this.Sales.ToString(), this.SalesTarget.ToString(), this.SalesDate.ToString());
        }
    }
}