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
    
    public partial class MEJJ_Cliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MEJJ_Cliente()
        {
            this.MEJJ_FacturaEnca = new HashSet<MEJJ_FacturaEnca>();
        }
    
        public int IdCliente { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public Nullable<int> DiasCredito { get; set; }
        public string Nombre { get; set; }
        public string Mail { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MEJJ_FacturaEnca> MEJJ_FacturaEnca { get; set; }
    }
}