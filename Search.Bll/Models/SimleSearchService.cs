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
        public SimleSearchService(IEnumerable<string> directories, string searchData) 
            : base(directories, searchData)
        {
        }

        public override IEnumerable GetYield()
        {
            Queue<string> queue = new Queue<string>();
            foreach (var directory in Directories)
            {
                queue.Enqueue(directory);
                while (queue.Count > 0)
                {
                    string[] fileInfo;
                    var currentDirPath = queue.Dequeue();
                    try
                    {
                        fileInfo = Directory.GetFiles(currentDirPath);
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                    var searchingFiles = fileInfo.Where(x => x.ToLower().Contains(SearchData.ToLower()));
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
                        queue.Enqueue(childDir);
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

       
    }
}
