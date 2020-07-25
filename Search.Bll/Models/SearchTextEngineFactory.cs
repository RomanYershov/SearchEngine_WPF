using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Search.Bll.Abstraction;

namespace Search.Bll.Models
{
   public class SearchTextEngineFactory : EngineFactory
    {
        public override SearchServiceBase Create()
        {
            return new TextSearchService();
        }
    }
}
