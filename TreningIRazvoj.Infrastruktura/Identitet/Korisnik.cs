using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace TreningIRazvoj.Infrastruktura.Identitet
{
    public class Korisnik : IdentityUser
    {
        public string Ime { get; set; } = string.Empty;

        public string Prezime { get; set; } = string.Empty;
    }
}
