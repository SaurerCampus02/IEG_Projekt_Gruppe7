using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace IEGKeyVaultGruppe7.Pages
{
    public class IndexModel : PageModel
    {
        public string Message { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration = null;

        public void OnGet()
        {
            Message = "Geheimtext = " + _configuration["Nachricht"];
        }
    }
}
