﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using GemBox.Document;
using Search.Bll.Abstraction;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Search.Bll.Models
{
    public class TextSearchEngine : SearchServiceBase
    {
        public TextSearchEngine() : base() { }
        public TextSearchEngine(IEnumerable<string> directories, string searchData) : base(directories, searchData)
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
                    string[] files;
                    var currentDirectory = queue.Dequeue();
                    try
                    {
                        files = Directory.GetFiles(currentDirectory);
                  
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                    var resFiles = files.Where(FindText);

                    foreach (var file in resFiles)
                    {
                        yield return file;
                    }

                    var childDirectories = Directory.GetDirectories(currentDirectory);
                    foreach (var childDirectory in childDirectories)
                    {
                        queue.Enqueue(childDirectory);
                    }
                }
                
            }

            yield return null;
        }

        public override Task<IEnumerable<string>> GetAsync(string currentDir)
        {
            throw new NotImplementedException();
        }

        private bool FindText(string filePath)
        {
            var searchFormatPattern = new [] {"txt", "doc"};
            var format = filePath.Split('.');
            var resF = format.Last();
            if (searchFormatPattern.Contains(resF))
            {
              
                
                try
                {
                    string allText;
                    if (resF == "doc")
                    {
                        ComponentInfo.SetLicense("FREE-LIMITED-KEY");//todo: без лицензии не работает
                         allText = DocumentModel.Load(filePath).Content.ToString();
                    }
                    else
                    {
                        allText = File.ReadAllText(filePath);
                    }
                    
                    return allText.ToLower().Contains(SearchData.ToLower());
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return false;
        }


        private  Stream GetStreamFromUrl(string url)
        {
            byte[] imageData = null;
            using (var wc = new WebClient())
            {
                imageData = wc.DownloadData(url);
            }
            return new MemoryStream(imageData);
        }
    }
}
