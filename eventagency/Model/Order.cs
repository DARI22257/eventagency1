using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eventagency.Model
{
    public class Order
    {
        public Client Client { get; set; }
        public Event Event { get; set; }
        public TaskWork Task { get; set; }
        public Contractor Contractor { get; set; }

        public EventContractor EventContractor { get; set; } = new();
    }
}
