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
    public class ParametroDetalle
    {
        public string Prametro { get; set; }
        public string ValorV { get; set; }
        public int ValorI { get; set; }
    }
    #endregion
    public class BrenchAdminDTO
    {
        public int Codigo { get; set; }
        public string Cedente { get; set; }
        public string Catalogo { get; set; }
        public string Brench { get; set; }
        public string Estado { get; set; }
        public string Auxv1 { get; set; }
        public string Auxv2 { get; set; }
        public string Auxv3 { get; set; }
        public int Auxi1 { get; set; }
        public int Auxi2 { get; set; }
        public int Auxi3 { get; set; }
    }
    public class CatalogoProductos
    {
        public string Producto { get; set; }
        public string CodigoCatalogo { get; set; }
        public string CodigoProducto { get; set; }
        public string CatalogoProducto { get; set; }
        public string CodigoFamilia { get; set; }
        public string Familia { get; set; }
        public string Estado { get; set; }
        public string CodProducto { get; set; }
    }
    public class SpeechAdminDTO
    {
        public int Codigo { get; set; }
        public int AracCodigo { get; set; }
        public string Accion { get; set; }
        public int ArefCodigo { get; set; }
        public string Efecto { get; set; }
        public int ArreCodigo { get; set; }
        public string Respuesta { get; set; }
        public int ArcoCodigo { get; set; }
        public string Contacto { get; set; }
        public string Speech { get; set; }
        public string Estado { get; set; }
        public string Auxv1 { get; set; }
        public string Auxv2 { get; set; }
        public int Auxi1 { get; set; }
        public int Auxi2 { get; set; }
    }
    public class SpeechCabeceraDTO
    {
        public int CodigoSpeech { get; set; }
        public string Speechbv { get; set; }
        public string Estado { get; set; }
    }
    public class ArbolSpeechDTO
    {
        public string Codigo { get; set; }
        public int CodigoARAC { get; set; }
        public string Accion { get; set; }
        public int CodigoAREF { get; set; }
        public string Efecto { get; set; }
        public int CodigoARRE { get; set; }
        public string Respuesta { get; set; }
        public int CodigoARCO { get; set; }
        public string Contacto { get; set; }
        public string Speech { get; set; }
        public string Observacion { get; set; }
        public string Estado { get; set; }
        public string Auxv1 { get; set; }
        public string Auxv2 { get; set; }
        public int Auxi1 { get; set; }
        public int Auxi2 { get; set; }
    }
}
