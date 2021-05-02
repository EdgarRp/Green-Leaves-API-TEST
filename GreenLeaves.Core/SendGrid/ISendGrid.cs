using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GreenLeaves.Core.SendGrid {
    public interface ISendGrid {
        Task<bool> SendMailAsync( string key, string address, string message, string subject, string mailAddress );
    }
}
