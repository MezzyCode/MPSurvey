using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JsonModels.Master
{
    public class JsonChart
    {
        public string Label { get; set; }
        public int Count { get; set; }
    }

    public class JsonChartLabel
    {
        public string XLabel { get; set; }
        public List<JsonChart> Charts { get; set; } = new List<JsonChart>();
    }

}
