using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace UnitTestsPOC.Domain
{
    [Authorize]
    public class MiniOrder : Order, IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
