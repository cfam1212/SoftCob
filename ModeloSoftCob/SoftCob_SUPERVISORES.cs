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
    
    public partial class SoftCob_SUPERVISORES
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SoftCob_SUPERVISORES()
        {
            this.SoftCob_GESTOR_SUPERVISOR = new HashSet<SoftCob_GESTOR_SUPERVISOR>();
        }
    
        public int SUPE_CODIGO { get; set; }
        public int CEDE_CODIGO { get; set; }
        public int USUA_CODIGO { get; set; }
        public bool supe_estado { get; set; }
        public string supe_auxv1 { get; set; }
        public string supe_auxv2 { get; set; }
        public Nullable<int> supe_auxi1 { get; set; }
        public Nullable<int> supe_auxi2 { get; set; }
        public System.DateTime supe_fechacreacion { get; set; }
        public int supe_usuariocreacion { get; set; }
        public string supe_terminalcreacion { get; set; }
        public System.DateTime supe_fum { get; set; }
        public int supe_uum { get; set; }
        public string supe_tum { get; set; }
    
        public virtual SoftCob_CEDENTE SoftCob_CEDENTE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SoftCob_GESTOR_SUPERVISOR> SoftCob_GESTOR_SUPERVISOR { get; set; }
        public virtual SoftCob_USUARIO SoftCob_USUARIO { get; set; }
    }
}
