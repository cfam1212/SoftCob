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
    
    public partial class SoftCob_PAGOSBRENCH_DET
    {
        public int PBDE_CODIGO { get; set; }
        public int PBCA_CODIGO { get; set; }
        public string pbde_operacion { get; set; }
        public int pbde_diasmora { get; set; }
        public System.DateTime pbde_fechapago { get; set; }
        public string pbde_brench { get; set; }
        public int pbde_rangoinicial { get; set; }
        public int pbde_rangofinal { get; set; }
        public string pbde_auxv1 { get; set; }
        public string pbde_auxv2 { get; set; }
        public string pbde_auxv3 { get; set; }
        public Nullable<int> pbde_auxi1 { get; set; }
        public Nullable<int> pbde_auxi2 { get; set; }
        public Nullable<int> pbde_auxi3 { get; set; }
    
        public virtual SoftCob_PAGOSBRENCH_CAB SoftCob_PAGOSBRENCH_CAB { get; set; }
    }
}