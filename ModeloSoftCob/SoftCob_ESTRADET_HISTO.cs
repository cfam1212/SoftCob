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
    
    public partial class SoftCob_ESTRADET_HISTO
    {
        public int EDHI_CODIGO { get; set; }
        public int ECHI_CODIGO { get; set; }
        public string edhi_campo { get; set; }
        public string edhi_operacion { get; set; }
        public string edhi_valor { get; set; }
        public string edhi_ordenar { get; set; }
    
        public virtual SoftCob_ESTRACAB_HISTO SoftCob_ESTRACAB_HISTO { get; set; }
    }
}
