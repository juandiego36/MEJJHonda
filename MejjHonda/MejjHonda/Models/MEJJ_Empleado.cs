//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MejjHonda.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class MEJJ_Empleado
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MEJJ_Empleado()
        {
            this.MEJJ_FacturaEnca = new HashSet<MEJJ_FacturaEnca>();
        }
    
        public int IdEmpleado { get; set; }
        [Required(ErrorMessage = "Este campo es nesesario")]
        [StringLength(15, ErrorMessage = "Máximo 15 dígitos")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Este campo es nesesario")]
        [Range(0, int.MaxValue, ErrorMessage = "Solo números")]
        [DisplayName("Cédula")]
        public string Cedula { get; set; }
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        [StringLength(30, ErrorMessage = "Máximo 30 dígitos")]
        [Required(ErrorMessage = "Este campo es nesesario")]
        [DisplayName("Correo")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "Este campo es nesesario")]
        [DataType(DataType.PhoneNumber)]
        [Range(0, int.MaxValue, ErrorMessage = "Solo números")]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "Este campo es nesesario")]
        [DisplayName("Dirección ")]
        [StringLength(70, ErrorMessage = "Máximo 70 dígitos")]
        public string Direccion { get; set; }
        public Nullable<int> IdRol { get; set; }
        [Required(ErrorMessage = "Este campo es nesesario")]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string Contraseña { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MEJJ_FacturaEnca> MEJJ_FacturaEnca { get; set; }

        public string LoginErrorMessage { get; set; }
        public string Codigo_Contrasena { get; set; }
    }
}
