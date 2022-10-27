using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketSelling.Services
{
    public interface IEmailService
    {
        bool AddToEmail(Customer customer, Event evt);
    }
}
