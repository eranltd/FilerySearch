using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilerySearch.ApplicationCore.Services.FilerySearchService;
using FilerySearch.Models;
using FilerySearchService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FilerySearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilerySearchController : ControllerBase
    {

        [HttpGet]
        public ActionResult Hello()
        {
            return Ok("Hello from FilerySearchController");
        }



        [HttpPost]
        public async Task<ActionResult<IEnumerable< FileProps>>> PostSearchInFiles(FilerySearchRequestItem item)
        {
            IEnumerable<FileProps> result = await FileryService.SearchOccurrences(item.SearchWords,
                                                                                  item.FromDate, item.ToDate,
                                                                                  item.DirectoryPath, item.MustMatchAllSearchWords,
                                                                                  '.'+item.FileType);
            return Ok(result);
        }
        /*Call from the outside with this JSON*/
        /*POST https://localhost:44330/api/filerysearch/ */
        /*
        {
               "SearchWords":["DateTime", "x.Refresh", "keySelector"],
               "FromDate": "10/3/2019 00:00:00",
               "ToDate": "10/3/2019 23:59:59",
                "DirectoryPath": "C:\\Projects\\FilerySearch\\SampleTXTFiles",
                "MustMatchAllSearchWords": false,
                "FileType" : "txt"
         }
        
         
         */

    }
}