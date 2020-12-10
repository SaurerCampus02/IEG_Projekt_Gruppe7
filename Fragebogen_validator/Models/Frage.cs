using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fragebogen_creator.Models
{
    public class Frage
    {
        public int FragenId { get; set; }
        public string FrageText { get; set; }

        //Anwort 1-5 1..zutreffend - 5..nicht zutreffend 
        public int Antwort { get; set; }

    }
}
