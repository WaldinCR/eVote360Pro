namespace eVote360Pro.Core.Domain.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        // Aquí más adelante podríamos agregar campos de auditoría si el negocio lo requiere
        // como CreatedBy, CreatedDate, etc.
    }
}

