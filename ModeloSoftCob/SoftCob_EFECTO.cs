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
    
    public partial class SoftCob_EFECTO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SoftCob_EFECTO()
        {
            this.SoftCob_RESPUESTA = new HashSet<SoftCob_RESPUESTA>();
        }
    
        public int AREF_CODIGO { get; set; }
        public int ARAC_CODIGO { get; set; }
        public string aref_descripcion { get; set; }
        public bool aref_estado { get; set; }
        public string aref_auxv1 { get; set; }
        public string aref_auxv2 { get; set; }
        public Nullable<int> aref_auxi1 { get; set; }
        public Nullable<int> aref_auxi2 { get; set; }
    
        public virtual SoftCob_ACCION SoftCob_ACCION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SoftCob_RESPUESTA> SoftCob_RESPUESTA { get; set; }
    }
}
