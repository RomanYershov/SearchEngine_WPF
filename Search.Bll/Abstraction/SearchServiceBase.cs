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
        
        public  List<string> Directories { get; set; }
        public string SearchData { get; set; }
       protected SearchServiceBase() => Directories = new List<string>();
        public abstract IEnumerable GetYield();
    }
}
