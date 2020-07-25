using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Search.Bll.Abstraction;

namespace Search.Bll.Models
{
    public class SearchEngine
    {
        private readonly SearchServiceBase searchService;
        public SearchEngine(IEngineFactory engineFactory, List<string> directories, string searchText)
        {
            searchService = engineFactory.Create();
            searchService.Directories = directories;
            searchService.SearchData = searchText;
        }

        public  IEnumerable GetData()
        {
           return searchService.GetYield();
        }
    }
}
