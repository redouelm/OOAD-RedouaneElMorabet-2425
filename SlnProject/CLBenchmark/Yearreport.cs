using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLBenchmark
{
    public class Yearreport
    {
        public int Id;
        public int Year;
        public int Fte;
        public int CompanyId;
    
        public override string ToString()
        {
            return "📅 " + Year;
        }
    }
}