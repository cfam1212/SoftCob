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
    
    public partial class SoftCob_CEDENTE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SoftCob_CEDENTE()
        {
            this.SoftCob_PRODUCTOS_CEDENTE = new HashSet<SoftCob_PRODUCTOS_CEDENTE>();
            this.SoftCob_SUPERVISORES = new HashSet<SoftCob_SUPERVISORES>();
        }
    
        public int CEDE_CODIGO { get; set; }
        public int cede_provcod { get; set; }
        public int cede_ciudcod { get; set; }
        public string cede_nombre { get; set; }
        public string cede_direccion { get; set; }
        public string cede_ruc { get; set; }
        public string cede_url { get; set; }
        public string cede_telefono1 { get; set; }
        public string cede_telefono2 { get; set; }
        public string cede_fax { get; set; }
        public bool cede_estado { get; set; }
        public string cede_auxv1 { get; set; }
        public string cede_auxiv2 { get; set; }
        public string cede_auxiv3 { get; set; }
        public Nullable<int> cede_auxi1 { get; set; }
        public Nullable<int> cede_auxi2 { get; set; }
        public Nullable<int> cede_auxi3 { get; set; }
        public System.DateTime cede_fechacreacion { get; set; }
        public int cede_usuariocreacion { get; set; }
        public string cede_terminalcreacion { get; set; }
        public System.DateTime cede_fum { get; set; }
        public int cede_uum { get; set; }
        public string cede_tum { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SoftCob_PRODUCTOS_CEDENTE> SoftCob_PRODUCTOS_CEDENTE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SoftCob_SUPERVISORES> SoftCob_SUPERVISORES { get; set; }
    }
}
