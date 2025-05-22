using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eventagency.Model
{
    public class Event
    {

        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Place { get; set; }
        public int Budget { get; set; }
        public string Status { get; set; }
    }
}
