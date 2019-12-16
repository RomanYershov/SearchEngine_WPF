using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Search.Bll.Abstraction;

namespace Search.Bll.Models
{
    public class SimleSearchService : SearchServiceBase
    {
        private readonly Queue<string> _queue;

        public SimleSearchService(IEnumerable<string> directories, string searchData) 
            : base(directories, searchData)
        {
        }

        public override IEnumerable GetYield()
        {
            foreach (var directory in Directories)
            {
                _queue.Enqueue(directory);
                while (_queue.Count > 0)
                {
                    string[] fileInfo;
                    var currentDirPath = _queue.Dequeue();
                    try
                    {
                        fileInfo = Directory.GetFiles(currentDirPath);
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                    var searchingFiles = fileInfo.Where(x => x.ToLower().Contains(SearchData));
                    if (searchingFiles.Any())
                    {
                        foreach (var searchingFile in searchingFiles)
                        {
                            yield return searchingFile;
                        }
                    }
                    var childDirectories = Directory.GetDirectories(currentDirPath);
                    foreach (var childDir in childDirectories)
                    {
                        _queue.Enqueue(childDir);
                    }
                }
            }
        }

        public override async Task<IEnumerable<string>> GetAsync(string currentDir)
        {
            List<string> buffer = new List<string>();


            await Task.Run(async () =>
            {
                string[] fileInfo = new string[] { };

                try
                {
                    fileInfo = Directory.GetFiles(currentDir);
                }
                catch (Exception e)
                {
                    return;
                }
                var searchingFiles = fileInfo.Where(x => x.ToLower().Contains(SearchData));
                if (searchingFiles.Any())
                {
                    foreach (var searchingFile in searchingFiles)
                    {
                        buffer.Add(searchingFile);
                            // return buffer;
                        }
                }
                var childDirectories = Directory.GetDirectories(currentDir);
                foreach (var childDir in childDirectories)
                {
                    var list = await GetAsync(childDir);
                    if (list != null)
                        buffer.AddRange(list);
                }
            });


            return buffer.Count > 0 ? buffer : null;

        }

        private async Task GetDir(string dir)
        {

        }
    }
}
