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
    
    public partial class SoftCob_PROYECCION_DET
    {
        public int PRDE_CODIGO { get; set; }
        public int PRCB_CODIGO { get; set; }
        public int prde_codigoresp { get; set; }
        public string prde_cedula { get; set; }
        public string prde_cliente { get; set; }
        public Nullable<System.DateTime> prde_fechapago { get; set; }
        public Nullable<decimal> prde_valor { get; set; }
        public string prde_estadopago { get; set; }
        public string prde_observacion { get; set; }
        public Nullable<bool> prde_eliminado { get; set; }
        public Nullable<int> prde_codigoarre { get; set; }
        public string prde_auxv1 { get; set; }
        public string prde_auxv2 { get; set; }
        public string prde_auxv3 { get; set; }
        public Nullable<int> prde_auxi1 { get; set; }
        public Nullable<int> prde_auxi2 { get; set; }
        public Nullable<int> prde_auxi3 { get; set; }
    
        public virtual SoftCob_PROYECCION_CAB SoftCob_PROYECCION_CAB { get; set; }
    }
}
