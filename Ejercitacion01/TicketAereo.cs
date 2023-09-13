using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercitacion01
{
    public class TicketAereo
    {
        public string NroTicket { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public Pasajero pasajero { get; set; }
        public Avion avion { get; set; }
        public DateTime FechaDeVuelo { get; set; }
    }
}
