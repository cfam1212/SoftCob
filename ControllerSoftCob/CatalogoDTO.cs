namespace ControllerSoftCob
{
    using System;

    #region CatalogosDTO
    public class CatalogosDTO
    {
        public string Descripcion { get; set; }
        public string Codigo { get; set; }
        public int? Nivel { get; set; }
    }
    #endregion

    #region Catalagos MENU
    public class MenuNew
    {
        public int MenCodigo { get; set; }
        public int TarCodigo { get; set; }
        public int EmprCodigo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCreacion { get; set; }
        public string TerminalCreacion { get; set; }
        public DateTime Fum { get; set; }
        public int Uum { get; set; }
        public string Tum { get; set; }
        public int MenOrden { get; set; }
    }

    public class MenuNewDTO
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public string RutaPagina { get; set; }
        public string Estado { get; set; }
        public string Selecc { get; set; }
        public int Orden { get; set; }
    }

    public class MenuNewEdit
    {
        public int TareCodigo { get; set; }
        public string Check { get; set; }
        public string Estado { get; set; }
    }
    #endregion

    #region Catalagos PERFIL
    public class PerfilNewDTO
    {
        public int Codigo { get; set; }
        public string Menu { get; set; }
        public string SubMenu { get; set; }
        public string Estado { get; set; }
        public bool ChkAgregar { get; set; }
    }

    public class PerfilNew
    {
        public int CodigoPerf { get; set; }
        public int CodigoMeta { get; set; }
        public bool Estado { get; set; }
    }
    #endregion

    #region Catalogos PAREMETROS
    public class ParametroNew
    {
        public int Codigo { get; set; }
        public int Emprcodigo { get; set; }
        public string Nombre { get; set; }
        public string ValorV { get; set; }
        public int ValorI { get; set; }
        public int Orden { get; set; }
        public bool Estado { get; set; }
    }
    #endregion
    public class BrenchAdminDTO
    {
        public int Codigo { get; set; }
        public string Cedente { get; set; }
        public string Catalogo { get; set; }
        public string Brench { get; set; }
        public string Estado { get; set; }
        public string auxv1 { get; set; }
        public string auxv2 { get; set; }
        public string auxv3 { get; set; }
        public int auxi1 { get; set; }
        public int auxi2 { get; set; }
        public int auxi3 { get; set; }
    }

}
