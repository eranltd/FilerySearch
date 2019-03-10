using FilerySearch.ApplicationCore.Services.FilerySearchService;
using System;
using System.Collections.Generic;
using Xunit;

namespace FilerySearchEngineTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            string directoryPath = @"C:\Projects\FilerySearch\SampleTXTFiles";

            var fromDate = new DateTime(2019, 3, 10);
            var toDate = new DateTime(2019, 3, 10, 23,59,59);

            var searchParams = new List<string> { "DateTime", "x.Refresh", "keySelector" };

            var result = FileryService.SearchOccurrences(searchParams.ToArray(), fromDate, toDate, directoryPath, false,".txt");

        }
    }
}
