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
    
    public partial class SoftCob_BRENCH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SoftCob_BRENCH()
        {
            this.SoftCob_BRENCHDET = new HashSet<SoftCob_BRENCHDET>();
        }
    
        public int BRCH_CODIGO { get; set; }
        public int brch_cedecodigo { get; set; }
        public int brch_cpcecodigo { get; set; }
        public bool brch_estado { get; set; }
        public string brch_auxv1 { get; set; }
        public string brch_auxv2 { get; set; }
        public string brch_auxv3 { get; set; }
        public Nullable<int> brch_auxi1 { get; set; }
        public Nullable<int> brch_auxi2 { get; set; }
        public Nullable<int> brch_auxi3 { get; set; }
        public System.DateTime brch_fechacreacion { get; set; }
        public int brch_usuariocreacion { get; set; }
        public string brch_terminalcreacion { get; set; }
        public System.DateTime brch_fum { get; set; }
        public int brch_uum { get; set; }
        public string brch_tum { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SoftCob_BRENCHDET> SoftCob_BRENCHDET { get; set; }
    }
}
