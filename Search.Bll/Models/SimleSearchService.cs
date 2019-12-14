using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Search.Bll.Abstraction;

namespace Search.Bll.Models
{
    public class SimleSearchService : SearchServiceBase
    {
        private readonly Queue<string> _queue;
        public SimleSearchService() => _queue = new Queue<string>();
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
    }
}
