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
    
    public partial class SoftCob_EVALUACION_DET
    {
        public int EVDE_CODIGO { get; set; }
        public int EVCA_CODIGO { get; set; }
        public int evde_codigopadre { get; set; }
        public int evde_codigoprotocolo { get; set; }
        public int evde_codigodepartamento { get; set; }
        public int evde_califica { get; set; }
        public int evde_contador { get; set; }
        public string evde_auxv1 { get; set; }
        public string evde_auxv2 { get; set; }
        public string evde_auxv3 { get; set; }
        public Nullable<int> evde_auxi1 { get; set; }
        public Nullable<int> evde_auxi2 { get; set; }
        public Nullable<int> evde_auxi3 { get; set; }
    
        public virtual SoftCob_EVALUACION_CAB SoftCob_EVALUACION_CAB { get; set; }
    }
}
