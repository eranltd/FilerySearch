﻿using FilerySearchService;
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
        //public FileryService()
        //{
        //}
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
        public static async Task<IEnumerable<FileProps>> SearchOccurrences(string[] searchWords, DateTime fromDate, DateTime toDate, string directoryPath = null, bool mustMatchAllSearchWords = false, string fileType = ".csv", SearchOption searchOption = SearchOption.AllDirectories)
        {
            return await Task.Run(() =>

            {
                List<string> searchWordsList = new List<string>(searchWords);
                var filesGroup = GetAllFilesInRange(directoryPath, fromDate, toDate, fileType, searchOption);

                //search for each file the line which the word appear, and return dictionary of FileMatch
                var found = FindAllOccurrencesAsync(searchWordsList, filesGroup, mustMatchAllSearchWords);
                return found;
            });
        }




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
        private static IEnumerable<FileProps> GetAllFilesInRange(string directoryPath, DateTime fromDate, DateTime toDate, string fileType, SearchOption searchOption = SearchOption.AllDirectories)
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
                                    let fileName = Path.GetFileNameWithoutExtension(filePath)

                                    where extension == fileType
                                    
                                    select new FileProps
                                    {
                                        FilePath = filePath,
                                        FileName = fileName,
                                        FileDate = File.GetLastWriteTime(filePath),
                                    };


                //filter only those who has matching date range.
                supplierFiles = supplierFiles.Where(item => item.FileDate >= fromDate && item.FileDate <= toDate);
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

        private static IEnumerable<FileProps> FindAllOccurrencesAsync(List<string> searchWords, IEnumerable<FileProps> filesInRange, bool mustMatchAllSearchWords)
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
