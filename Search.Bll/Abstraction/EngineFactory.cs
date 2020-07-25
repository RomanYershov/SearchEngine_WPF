using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search.Bll.Abstraction
{
   public abstract class EngineFactory
   {
       public abstract SearchServiceBase Create();
   }
}
