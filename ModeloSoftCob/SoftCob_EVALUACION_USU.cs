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
    
    public partial class SoftCob_EVALUACION_USU
    {
        public int EVUS_CODIGO { get; set; }
        public int EVCA_CODIGO { get; set; }
        public int evus_usucodigo { get; set; }
        public System.DateTime evus_fechacreacion { get; set; }
        public string evus_terminalcreacion { get; set; }
        public string evus_auxv1 { get; set; }
        public string evus_auxv2 { get; set; }
        public string evus_auxv3 { get; set; }
        public Nullable<int> evus_auxi1 { get; set; }
        public Nullable<int> evus_auxi2 { get; set; }
        public Nullable<int> evus_auxi3 { get; set; }
    
        public virtual SoftCob_EVALUACION_CAB SoftCob_EVALUACION_CAB { get; set; }
    }
}
