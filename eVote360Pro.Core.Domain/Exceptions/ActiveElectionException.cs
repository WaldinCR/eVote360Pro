using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Domain.Exceptions
{
    public class ActiveElectionException : DomainValidationException
    {
        public ActiveElectionException(string message = "No se puede realizar esta acción mientras exista una elección activa.")
            : base(message) { }
    }
}