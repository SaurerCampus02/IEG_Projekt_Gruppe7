using System.Collections.Generic;
using WebHookService.Models;

namespace WebHookService.Data
{
    public class EmailStorage
    {
        List<Emails> emails;

        public EmailStorage()
        {
            emails = new List<Emails>()
            {
                new Emails() {Id = "1", Name = "Martin", Mail = "martin.schantl@edu.campus02.at"},
                new Emails() {Id = "2", Name = "Matthias", Mail = "matthias.schreiner@edu.campus02.at"}
            };
        }

        public List<Emails> Emails
        {
            get => emails;
            set => emails = value;
        }
    }
}