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
    
    public partial class SoftCob_CIUDAD
    {
        public int CIUD_CODIGO { get; set; }
        public int PROV_CODIGO { get; set; }
        public string ciud_nombre { get; set; }
        public bool ciud_estado { get; set; }
        public System.DateTime ciud_fechacreacion { get; set; }
        public int ciud_usuariocreacion { get; set; }
        public string ciud_terminalcreacion { get; set; }
        public System.DateTime ciud_fum { get; set; }
        public int ciud_uum { get; set; }
        public string ciud_tum { get; set; }
    
        public virtual SoftCob_PROVINCIA SoftCob_PROVINCIA { get; set; }
    }
}
