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
    
    public partial class SoftCob_HORARIOS_CAB
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SoftCob_HORARIOS_CAB()
        {
            this.SoftCob_HORARIOS_DET = new HashSet<SoftCob_HORARIOS_DET>();
        }
    
        public int HORA_CODIGO { get; set; }
        public string hora_nombre { get; set; }
        public string hora_descripcion { get; set; }
        public string hora_intervalo { get; set; }
        public System.TimeSpan hora_desde { get; set; }
        public System.TimeSpan hora_hasta { get; set; }
        public bool hora_estado { get; set; }
        public string hora_auxv1 { get; set; }
        public string hora_auxv2 { get; set; }
        public string hora_auxv3 { get; set; }
        public Nullable<int> hora_auxi1 { get; set; }
        public Nullable<int> hora_auxi2 { get; set; }
        public Nullable<int> hora_auxi3 { get; set; }
        public System.DateTime hora_fechacreacion { get; set; }
        public int hora_usuariocreacion { get; set; }
        public string hora_terminalcreacion { get; set; }
        public System.DateTime hora_fum { get; set; }
        public int hora_uum { get; set; }
        public string hora_tum { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SoftCob_HORARIOS_DET> SoftCob_HORARIOS_DET { get; set; }
    }
}
