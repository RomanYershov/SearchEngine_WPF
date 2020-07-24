using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search.Bll.Abstraction
{
   public abstract  class SearchServiceBase
   {

       public IEnumerable<string> Directories { get; set; }
       public string SearchData { get; set; }

       protected SearchServiceBase() { }
       protected SearchServiceBase(IEnumerable<string> directories, string searchData)
       {
           Directories = directories;
           SearchData = searchData;
       }
        public abstract IEnumerable GetYield();
      public abstract Task<IEnumerable<string>> GetAsync(string currentDir);
   }
}
