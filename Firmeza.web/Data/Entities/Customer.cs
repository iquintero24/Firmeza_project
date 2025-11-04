// Firmeza.web.Data.Entities/Customer.cs (VERSIÓN CORREGIDA Y COMPLETA)

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firmeza.web.Data.Entities
{
    public class Customer
    {
        // Primary Key
        [Key]
        public int Id { get; set; }

        // ... (Propiedades de Negocio existentes) ...
        [Required(ErrorMessage = "The customer's name is mandatory.")]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "The document/identification is mandatory.")]
        [StringLength(20)]
        public string Document { get; set; }  = string.Empty;
        
        [Required(ErrorMessage = "The email address is mandatory.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(150)]
        public string Email { get; set; }  = string.Empty;
        
        [Phone(ErrorMessage = "Invalid phone format.")]
        [StringLength(15)]
        [Display(Name = "Phone")]
        public string Phone { get; set; } = string.Empty;

        // =======================================================
        // PROPIEDADES PARA VINCULAR CON IDENTITY (FK a AspNetUsers)
        // =======================================================
        
        // 1. Clave Foránea (FK)
        // string es el tipo de dato del campo Id en la tabla AspNetUsers
        public string? IdentityUserId { get; set; } 
        
        // 2. Propiedad de Navegación
        [ForeignKey("IdentityUserId")]
        // Asumo que tu clase de usuario de Identity se llama ApplicationUser
        public ApplicationUser? IdentityUser { get; set; } 
        
        // =======================================================

        // Relationship: A customer can have many sales (1:N)
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}