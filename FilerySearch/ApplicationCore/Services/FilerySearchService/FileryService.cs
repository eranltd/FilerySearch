using FilerySearchService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilerySearch.ApplicationCore.Services.FilerySearchService
{
    /// <summary>
     /// This is a little helper service, to help us search for sub-string inside supplier csv file(s)
     /// </summary>
        public class FileryService
        {
            #region Constructor
            /// <summary>
            /// 
            /// </summary>
            public FileryService()
            {
            }
            #endregion
            #region Public Methods
            /// <summary>
            /// main Function Orch, should return list of FileMatch
            /// </summary>
            /// <param name="searchWords"></param>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <param name="directoryPath"></param>
            /// <param name="fileType"></param>
            /// <param name="pageNum"></param>
            /// <param name="pageSize"></param>
            /// <param name="searchOption"></param>
            /// <param name="mustMatchAllSearchWords"></param>
            /// <returns></returns>
            //public IEnumerable<FileProps> SearchOccurrences(string[] searchWords, DateTime fromDate, DateTime toDate, string directoryPath = null, bool mustMatchAllSearchWords = true, SearchOption searchOption = SearchOption.AllDirectories, string fileType = ".csv")
            //{
            //    List<string> searchWordsList = new List<string>(searchWords);
            //    var filesGroup = GetAllFilesInRange(directoryPath, fromDate, toDate, fileType, searchOption);

            //    //search for each file the line which the word appear, and return dictionary of FileMatch
            //    var found = FindAllOccurrences(searchWordsList, filesGroup, mustMatchAllSearchWords);
            //    return found;

            //}




            /// <summary>
            /// 
            /// </summary>
            /// <param name="directoryPath"></param>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <param name="fileType"></param>
            /// <param name="searchOption"></param>
            /// <returns></returns>
            public int GetNumberOfFilesInRange(string directoryPath, DateTime fromDate, DateTime toDate, string fileType = ".csv", SearchOption searchOption = SearchOption.AllDirectories)
            {
                IEnumerable<FileProps> filesInRange = GetAllFilesInRange(directoryPath, fromDate, toDate, fileType, searchOption);
                return filesInRange != null ? filesInRange.Count() : 0;
            }
            #endregion

            #region Private Methods
            /// <summary>
            /// Returns all files in given dates from the supplier
            /// </summary>
            /// <param name="directoryPath"></param>
            /// <param name="fromDate"></param>
            /// <param name="toDate"></param>
            /// <param name="fileType"></param>
            /// <param name="searchOption"></param>
            /// <returns></returns>
            public IEnumerable<FileProps> GetAllFilesInRange(string directoryPath, DateTime fromDate, DateTime toDate, string fileType = ".csv", SearchOption searchOption = SearchOption.AllDirectories)
            {
                List<FileProps> filesInRange = default(List<FileProps>);

                try
                {
                    //enumarate all files in directory, including sub-directoris
                    var files = Directory.EnumerateFiles(directoryPath, "*.*", searchOption);

                    //build from all data above List of FileProps objects.

                    var supplierFiles = from filePath in files.AsParallel() // Use AsOrdered to preserve source ordering
                                                                            //filter all desired files
                                        let extension = Path.GetExtension(filePath)
                                        where extension == fileType

                                        let fileName = Path.GetFileNameWithoutExtension(filePath)

                                        select new FileProps
                                        {
                                            FilePath = filePath,
                                            FileName = fileName,
                                            FileDate = File.GetLastWriteTime(filePath),
                                            //FileNameDate = new FileR2NetDateFormat(fileName)
                                        };


                    //filter only those who has matching date range.
                    //supplierFiles = supplierFiles.Where(item => item.FileNameDate.dateTime >= fromDate && item.FileNameDate.dateTime < toDate);
                    filesInRange = supplierFiles.ToList();

                }

                catch (AggregateException ae)
                {
                    ae.Handle((ex) =>
                    {
                        if (ex is UnauthorizedAccessException)
                        {
                            //Console.WriteLine(ex.Message);
                            throw ex;
                            //return true;
                        }
                        return false;
                    });
                }

                return filesInRange;
            }

            public IEnumerable<FileProps> FindAllOccurrences(List<string> searchWords, IEnumerable<FileProps> filesInRange, bool mustMatchAllSearchWords)
            {
                //Best Performance, exploit CPU power.
                Parallel.ForEach(filesInRange, (currentFile) =>
                {

                    //// The more computational work you do here, the greater 
                    //// the speedup compared to a sequential foreach loop.
                    currentFile.FindAndSetDictOccrencesInFile(searchWords, mustMatchAllSearchWords);
                });

                return filesInRange.Where(file => file.MatchedLines.Count > 0).ToList();
            }


            #endregion

        }
    }
