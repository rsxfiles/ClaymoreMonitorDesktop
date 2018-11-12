using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaymoreMonitorDesktop
{
    public class Gpu
    {
        public int Id { get; set; }
        public int EthHash { get; set; }
        public int Temperature { get; set; }
        public int Fan { get; set; }
    }
}
