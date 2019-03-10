using FilerySearch.ApplicationCore.Services.FilerySearchService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FilerySearchService
{
    public class FileProps
    {
        /// <summary>
        /// 
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime FileDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public FileR2NetDateFormat FileNameDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<MatchedSentence> MatchedLines { get; set; } //matchedWord, lineNumber, lineContent



        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchWords"></param>
        /// <param name="mustMatchAllSearchWords"></param>
        public void FindAndSetDictOccrencesInFile(List<string> searchWords, bool mustMatchAllSearchWords = false)
        {
            int lineCount = 1;
            if (File.Exists(FilePath))
            {
                // Find sentences that contain all the terms in the wordsToMatch array.  
                // Note that the number of terms to match is not specified at compile time.  
                var sentencesContainsAnyOccrencesQuery = from sentence in File.ReadAllLines(FilePath)

                                                         let sentenceCount = lineCount++

                                                         let w = sentence.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' },
                                                         StringSplitOptions.RemoveEmptyEntries)

                                                         //we are iterating now on each word of the sentence
                                                         from d in searchWords


                                                             //all Sentences that Contain a Specified Set of Words
                                                         where (mustMatchAllSearchWords == true && w.Distinct().Intersect(searchWords).Count() == searchWords.Count()) //search duo to boolean flag.
                                                         || (mustMatchAllSearchWords == false && sentence.Contains(d))

                                                         select new MatchedSentence()
                                                         {
                                                             MatchedSentenceNum = sentenceCount,
                                                             MatchedWord = d,
                                                             MatchedWords = searchWords,
                                                             MatchedSentenceContent = sentence
                                                         };


                MatchedLines = sentencesContainsAnyOccrencesQuery.ToList();


            }
        }
    }
}
