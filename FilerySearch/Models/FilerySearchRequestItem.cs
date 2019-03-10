using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilerySearch.Models
{
    public class FilerySearchRequestItem
    {
        public string[] SearchWords { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string DirectoryPath { get; set; }
        public bool MustMatchAllSearchWords { get; set; }
        public string FileType { get; set; }
       
      }
}
