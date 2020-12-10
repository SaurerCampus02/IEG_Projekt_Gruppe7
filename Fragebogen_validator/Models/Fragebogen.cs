using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fragebogen_creator.Models
{
    public class Fragebogen
    {
        public int FragebogenId { get; set; }
        public Frage[] Fragen { get; set; }

    }
}
