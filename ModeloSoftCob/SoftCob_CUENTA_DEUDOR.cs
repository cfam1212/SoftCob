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
    
    public partial class SoftCob_CUENTA_DEUDOR
    {
        public int CTDE_CODIGO { get; set; }
        public int CLDE_CODIGO { get; set; }
        public string ctde_operacion { get; set; }
        public decimal ctde_totaldeuda { get; set; }
        public int ctde_diasmora { get; set; }
        public decimal ctde_capitalporvencer { get; set; }
        public decimal ctde_capitalvencido { get; set; }
        public decimal ctde_capitalmora { get; set; }
        public decimal ctde_valorexigible { get; set; }
        public System.DateTime ctde_fechaobligacion { get; set; }
        public System.DateTime ctde_fechavencimiento { get; set; }
        public System.DateTime ctde_fechaultimopago { get; set; }
        public int ctde_gestorasignado { get; set; }
        public bool ctde_estado { get; set; }
        public string ctde_auxv1 { get; set; }
        public string ctde_auxv2 { get; set; }
        public string ctde_auxv3 { get; set; }
        public string ctde_auxv4 { get; set; }
        public Nullable<decimal> ctde_auxd1 { get; set; }
        public Nullable<decimal> ctde_auxd2 { get; set; }
        public Nullable<decimal> ctde_auxd3 { get; set; }
        public Nullable<decimal> ctde_auxd4 { get; set; }
        public Nullable<int> ctde_auxi1 { get; set; }
        public Nullable<int> ctde_auxi2 { get; set; }
        public Nullable<int> ctde_auxi3 { get; set; }
        public Nullable<int> ctde_auxi4 { get; set; }
        public Nullable<System.DateTime> ctde_auxf1 { get; set; }
        public Nullable<System.DateTime> ctde_auxf2 { get; set; }
        public Nullable<System.DateTime> ctde_auxf3 { get; set; }
        public Nullable<System.DateTime> ctde_auxf4 { get; set; }
    
        public virtual SoftCob_CLIENTE_DEUDOR SoftCob_CLIENTE_DEUDOR { get; set; }
    }
}
