using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eventagency.Model
{
    public class EventContractor
    {
        public int ID { get; set; }
        public int Price { get; set; }

        public int idTask { get; set; }
        public string DescriptionService { get; set; }
    }
}
