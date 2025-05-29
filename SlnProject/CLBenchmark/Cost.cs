using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLBenchmark
{
    public class Cost
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public string CostType { get; set; }
        public string Category { get; set; }
        public int YearreportId { get; set; }
    }
}
