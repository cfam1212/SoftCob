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
    
    public partial class SoftCob_LISTATRABAJO_CABECERA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SoftCob_LISTATRABAJO_CABECERA()
        {
            this.SoftCob_LISTATRABAJO_DETALLE = new HashSet<SoftCob_LISTATRABAJO_DETALLE>();
        }
    
        public int LTCA_CODIGO { get; set; }
        public string ltca_lista { get; set; }
        public string ltca_descripcion { get; set; }
        public System.DateTime ltca_fechainicio { get; set; }
        public System.DateTime ltca_fechafin { get; set; }
        public int ltca_escacodigo { get; set; }
        public int ltca_cedecodigo { get; set; }
        public int ltca_cpcecodigo { get; set; }
        public bool ltca_estado { get; set; }
        public string ltca_tipomarcado { get; set; }
        public string ltca_campania { get; set; }
        public Nullable<int> ltca_porgestion { get; set; }
        public string ltca_tipogestion { get; set; }
        public Nullable<int> ltca_porarbol { get; set; }
        public Nullable<int> ltca_araccodigo { get; set; }
        public Nullable<int> ltca_porfecha { get; set; }
        public string ltca_fechadesde { get; set; }
        public string ltca_fechahasta { get; set; }
        public string ltca_auxvi1 { get; set; }
        public string ltca_auxv2 { get; set; }
        public string ltca_auxv3 { get; set; }
        public Nullable<int> ltca_auxi1 { get; set; }
        public Nullable<int> ltca_auxi2 { get; set; }
        public Nullable<int> ltca_auxi3 { get; set; }
        public System.DateTime ltca_fechacreacion { get; set; }
        public int ltca_usuariocreacion { get; set; }
        public string ltca_terminalcreacion { get; set; }
        public System.DateTime ltca_fum { get; set; }
        public int ltca_uum { get; set; }
        public string ltca_tum { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SoftCob_LISTATRABAJO_DETALLE> SoftCob_LISTATRABAJO_DETALLE { get; set; }
    }
}
