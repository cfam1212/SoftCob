//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ModeloSoftCob
{
    using System;
    using System.Collections.Generic;
    
    public partial class SoftCob_CATALOGO_PRODUCTOS_CEDENTE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SoftCob_CATALOGO_PRODUCTOS_CEDENTE()
        {
            this.SoftCob_ACCION = new HashSet<SoftCob_ACCION>();
        }
    
        public int CPCE_CODIGO { get; set; }
        public int PRCE_CODIGO { get; set; }
        public string cpce_codigoproducto { get; set; }
        public string cpce_producto { get; set; }
        public string cpce_codigofamilia { get; set; }
        public string cpce_familia { get; set; }
        public bool cpce_estado { get; set; }
        public string cpce_auxv1 { get; set; }
        public string cpce_auxv2 { get; set; }
        public Nullable<int> cpce_auxi1 { get; set; }
        public Nullable<int> cpce_auxi2 { get; set; }
    
        public virtual SoftCob_PRODUCTOS_CEDENTE SoftCob_PRODUCTOS_CEDENTE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SoftCob_ACCION> SoftCob_ACCION { get; set; }
    }
}
