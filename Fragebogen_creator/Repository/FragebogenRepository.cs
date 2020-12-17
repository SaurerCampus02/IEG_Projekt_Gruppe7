using Fragebogen_creator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fragebogen_creator.Repository
{
    public class FragebogenRepository
    {
        private static List<Fragebogen> _fragebogen = new List<Fragebogen>();
        public FragebogenRepository()
        {
            _fragebogen.Clear();

            Frage frage1 = new Frage();
            frage1.FragenId = 2;
            frage1.FrageText = "Warum Sie mit der Dauer der Lieferung zufrieden?";
            frage1.Antwort = 3;
            Frage frage2 = new Frage();
            frage2.FragenId = 1;
            frage2.FrageText = "Warum Sie mit der Qualität des Produktes zufrieden?";
            frage2.Antwort = 4;

            Fragebogen fragebogen1 = new Fragebogen();
            fragebogen1.FragebogenId = 0;
            fragebogen1.Fragen = new Frage[] { frage1, frage2 };

            Frage frage3 = new Frage();
            frage3.FragenId = 2;
            frage3.FrageText = "Warum Sie mit der Dauer der Lieferung zufrieden?";
            frage3.Antwort = 4;
            Frage frage4 = new Frage();
            frage4.FragenId = 1;
            frage4.FrageText = "Warum Sie mit der Qualität des Produktes zufrieden?";
            frage4.Antwort = 5;

            Fragebogen fragebogen2 = new Fragebogen();
            fragebogen2.FragebogenId = 1;
            fragebogen2.Fragen = new Frage[] { frage3, frage4 };

            _fragebogen.Add(fragebogen1);
            _fragebogen.Add(fragebogen2);
        }


        public IEnumerable<Fragebogen> Get()
        {
            return _fragebogen;
        }

        public Fragebogen Get(int fragenbogenId)
        {
            return _fragebogen.Where(p => p.FragebogenId == fragenbogenId).SingleOrDefault();
        }

        public Fragebogen Add(Fragebogen fragebogen)
        {
            if (_fragebogen.Count != 0)
            {
                fragebogen.FragebogenId = _fragebogen.Max(p => p.FragebogenId) + 1;
            }
            else
            {
                fragebogen.FragebogenId = 1;
            }

            _fragebogen.Add(fragebogen);
            return fragebogen;
        }

        public void Remove(int id)
        {
            Fragebogen fragebogen = Get(id);
            _fragebogen.Remove(fragebogen);
        }
    }
}
