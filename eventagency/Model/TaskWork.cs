using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eventagency.Model
{
    public class TaskWork
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Term { get; set; }
        public string Assigned { get; set; }
        public string Status { get; set; }
        public Event Event { get; set; }
    }
}
