using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilerySearch.ApplicationCore.Services.FilerySearchService
{
    public class MatchedSentence
    {
        /// <summary>
        /// 
        /// </summary>
        public string MatchedWord { get; set; } //list of words
        /// <summary>
        /// 
        /// </summary>
        public int MatchedSentenceNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MatchedSentenceContent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> MatchedWords { get; set; }


    }
}
