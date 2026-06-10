using eVote360Pro.Core.Domain.Common;
using eVote360Pro.Core.Domain.Common.Enums;

namespace eVote360Pro.Core.Domain.Entities
{
    public class User : BaseEntity
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required UserRol Role { get; set; }
        public bool IsActive { get; set; } = true;

        //navigation properties
        //public DirigentePolitico? DirigentePolitico { get; set; }
    }

}

