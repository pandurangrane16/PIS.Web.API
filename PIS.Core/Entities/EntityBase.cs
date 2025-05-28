using System.ComponentModel.DataAnnotations;

namespace PIS.Framework.Entities
{
    public class EntityBase
    {
        // This is the base class for all entities.
        // The DataAccess repositories have this class as constraint for entities that are persisted in the database.

        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
