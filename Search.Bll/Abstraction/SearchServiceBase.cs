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

       protected IEnumerable<string> Directories { get; }
       protected string SearchData { get; }

       protected SearchServiceBase(IEnumerable<string> directories, string searchData)
       {
           Directories = directories;
           SearchData = searchData;
       }
        public abstract IEnumerable GetYield();
      public abstract Task<IEnumerable<string>> GetAsync(string currentDir);
   }
}
