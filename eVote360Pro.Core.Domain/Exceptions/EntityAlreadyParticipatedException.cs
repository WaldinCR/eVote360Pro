using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Domain.Exceptions
{
    public class EntityAlreadyParticipatedException : DomainValidationException
    {
        public EntityAlreadyParticipatedException(string message = "No se pueden modificar los datos principales porque la entidad ya participó en una elección.")
            : base(message) { }
    }
}