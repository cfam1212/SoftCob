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
    
    public partial class SoftCob_HORARIOS_DET
    {
        public int CHOR_CODIGO { get; set; }
        public int HORA_CODIGO { get; set; }
        public System.TimeSpan chor_inicio { get; set; }
        public System.TimeSpan chor_fin { get; set; }
        public int chor_orden { get; set; }
        public string chor_auxv1 { get; set; }
        public string chor_auxv2 { get; set; }
        public string chor_auxv3 { get; set; }
        public Nullable<int> chor_auxi1 { get; set; }
        public Nullable<int> chor_auxi2 { get; set; }
        public Nullable<int> chor_auxi3 { get; set; }
    
        public virtual SoftCob_HORARIOS_CAB SoftCob_HORARIOS_CAB { get; set; }
    }
}
