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
    
    public partial class SoftCob_PROYECCION_CAB
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SoftCob_PROYECCION_CAB()
        {
            this.SoftCob_PROYECCION_DET = new HashSet<SoftCob_PROYECCION_DET>();
        }
    
        public int PRCB_CODIGO { get; set; }
        public int prcb_cedecodigo { get; set; }
        public int prcb_cpcecodigo { get; set; }
        public int prcb_gestor { get; set; }
        public Nullable<int> prcb_year { get; set; }
        public Nullable<int> prcb_mes { get; set; }
        public string prcb_meslabel { get; set; }
        public Nullable<decimal> prcb_presupuesto { get; set; }
        public Nullable<decimal> prcb_proyeccion { get; set; }
        public string prcb_auxv1 { get; set; }
        public string prcb_auxv2 { get; set; }
        public string prcb_auxv3 { get; set; }
        public Nullable<int> prcb_auxi1 { get; set; }
        public Nullable<int> prcb_auxi2 { get; set; }
        public Nullable<int> prcb_auxi3 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SoftCob_PROYECCION_DET> SoftCob_PROYECCION_DET { get; set; }
    }
}