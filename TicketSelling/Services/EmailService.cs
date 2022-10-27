using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketSelling.Services
{
    public class EmailService : IEmailService
    {
        bool IEmailService.AddToEmail(Customer customer, Event evt)
        {
            return true;
        }
    }
}
