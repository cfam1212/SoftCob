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
    
    public partial class SoftCob_BRENCHMESCAB
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SoftCob_BRENCHMESCAB()
        {
            this.SoftCob_BRENCHMESDET = new HashSet<SoftCob_BRENCHMESDET>();
        }
    
        public int BRMC_CODIGO { get; set; }
        public int brmc_cedecodigo { get; set; }
        public int brmc_cpcecodigo { get; set; }
        public int brmc_gestorasignado { get; set; }
        public int brmc_presupuestoanio { get; set; }
        public int brmc_presupuestomes { get; set; }
        public string brmc_presumeslabel { get; set; }
        public decimal brmc_totalmonto { get; set; }
        public decimal brmc_totalexigible { get; set; }
        public decimal brmc_presuporcentaje { get; set; }
        public decimal brmc_presupuestototal { get; set; }
        public System.DateTime brmc_presupuestofecha { get; set; }
        public decimal brmc_presuporcencumplido { get; set; }
        public decimal brmc_presutotalcumplido { get; set; }
        public bool brmc_presupuestogenerado { get; set; }
        public System.DateTime brmc_fechacierre { get; set; }
        public int brmc_usuariocierre { get; set; }
        public string brmc_terminalcierre { get; set; }
        public string brmc_auxv1 { get; set; }
        public string brmc_auxv2 { get; set; }
        public string brmc_auxv3 { get; set; }
        public Nullable<int> brmc_auxi1 { get; set; }
        public Nullable<int> brmc_auxi2 { get; set; }
        public Nullable<int> brmc_auxi3 { get; set; }
        public Nullable<decimal> brmc_auxd1 { get; set; }
        public Nullable<decimal> brmc_auxd2 { get; set; }
        public Nullable<decimal> brmc_auxd3 { get; set; }
        public Nullable<System.DateTime> brmc_auxf1 { get; set; }
        public Nullable<System.DateTime> brmc_auxf2 { get; set; }
        public Nullable<System.DateTime> brmc_auxf3 { get; set; }
        public System.DateTime brmc_fechacreacion { get; set; }
        public int brmc_usuariocreacion { get; set; }
        public string brmc_terminalcreacion { get; set; }
        public System.DateTime brmc_fum { get; set; }
        public int brmc_uum { get; set; }
        public string brmc_tum { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SoftCob_BRENCHMESDET> SoftCob_BRENCHMESDET { get; set; }
    }
}
