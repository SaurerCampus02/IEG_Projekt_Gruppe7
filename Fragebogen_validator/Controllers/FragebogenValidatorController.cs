﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fragebogen_creator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fragebogen_validator.Controllers
{
    //Fragebogen JSON zum Testen
//    [
//     {
//      "fragebogenId":0,
//        "fragen":[
//           {
//            "fragenId":2,
//              "frageText":"Warum Sie mit der Dauer der Lieferung zufrieden?",
//              "antwort":3
//         },
//         {
//            "fragenId":1,
//            "frageText":"Warum Sie mit der Qualität des Produktes zufrieden?",
//            "antwort":4
//         }
//      ]
//   },
//   {
//      "fragebogenId":1,
//      "fragen":[
//         {
//            "fragenId":2,
//            "frageText":"Warum Sie mit der Dauer der Lieferung zufrieden?",
//            "antwort":4
//         },
//         {
//            "fragenId":1,
//            "frageText":"Warum Sie mit der Qualität des Produktes zufrieden?",
//            "antwort":5
//         }
//      ]
//   }
//]


    [Route("api/[controller]")]
    [ApiController]
    public class FragebogenValidatorController : ControllerBase
    {
       [HttpGet]
        public List<KeyValuePair<int, double>> Get([FromBody]Fragebogen[] frageboegen)
        {
            List<Frage> alleFragen = new List<Frage>();

            foreach (Fragebogen fragebogen in frageboegen)
            {
                foreach (Frage frage in fragebogen.Fragen)
                {
                    alleFragen.Add(frage);
                }
               
            }

            var groupedFragen = alleFragen.GroupBy(frage => frage.FragenId);

            var validatedFragen = new List<KeyValuePair<int, double>>();

            foreach (var groupedFrage in groupedFragen)
            {
                double sum = 0;
                foreach (Frage singleFrage in groupedFrage)
                {
                    sum += singleFrage.Antwort;
                }

                int length = alleFragen.FindAll(f => f.FragenId == groupedFrage.Key).Count;

               validatedFragen.Add(new KeyValuePair<int, double>(groupedFrage.Key, sum / length));

            }
            return validatedFragen;
        }
    }
}