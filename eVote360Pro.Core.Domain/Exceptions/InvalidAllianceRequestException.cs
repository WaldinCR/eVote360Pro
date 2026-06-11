using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Domain.Exceptions
{
    public class InvalidAllianceRequestException : DomainValidationException
    {
        public InvalidAllianceRequestException(string message = "La solicitud de alianza no es válida.")
            : base(message) { }
    }
}