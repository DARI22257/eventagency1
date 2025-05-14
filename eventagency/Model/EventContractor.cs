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
        public int idClient { get; set; }
        public int idEvents { get; set; }
        public int idContractor { get; set; }
        public string DescriptionService { get; set; }

        public Client Client { get; set; }
        public Event Event { get; set; }
        public TaskWork Task { get; set; }
        public Contractor Contractor { get; set; }
    }
}
