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
    
    public partial class MEJJ_Articulo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MEJJ_Articulo()
        {
            this.MEJJ_FacturaDeta = new HashSet<MEJJ_FacturaDeta>();
        }
    
        public int IdArticulo { get; set; }
        public string Descripcion { get; set; }
        public string Modelo { get; set; }
        public Nullable<decimal> Precio { get; set; }
        public string Color { get; set; }
        public string Tamanio { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MEJJ_FacturaDeta> MEJJ_FacturaDeta { get; set; }
    }
}