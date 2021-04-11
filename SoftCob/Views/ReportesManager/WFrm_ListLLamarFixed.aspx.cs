namespace SoftCob.Views.ReportesManager
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ListLLamarFixed : Page
    {
        #region Variables
        string _sql = "", _fechallamar = "", _horallamar = "";
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        DateTime _fechaactual, _fechallamada;
        TimeSpan _horaactual, _horallamada;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.ImgExportar);
                if (!IsPostBack)
                {
                    ViewState["CodigoCEDE"] = Request["CodigoCEDE"];
                    ViewState["CodigoCPCE"] = Request["CodigoCPCE"];
                    ViewState["FechaDesde"] = Request["FechaDesde"];
                    ViewState["FechaHasta"] = Request["FechaHasta"];
                    ViewState["Gestor"] = Request["Gestor"];
                    ViewState["Tipo"] = Request["Tipo"];
                    ViewState["FechaActual"] = DateTime.Now.ToString("yyyy-MM-dd");
                    ViewState["HoraActual"] = DateTime.Now.ToString("HH:mm");
                    LblTitulo.Text = "Lista de Seguimientos << VOLVER A LLAMAR >> ";
                    FunCargarMantenimiento();
                }
            }
            catch (Exception ex)
            {
                LblError.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                _sql = "SELECT FechaRegistro = CONVERT(varchar(19),revl_fechacreacion,121),FechaLlama = CONVERT(varchar(10),revl_fechallamar,121)+ ' '+CONVERT(varchar(5),revl_horallamar,108), Producto = ISNULL((SELECT pade_nombre FROM SoftCob_PARAMETRO_DETALLE (NOLOCK) WHERE pade_valorV=ctde_auxv1 and PARA_CODIGO in(SELECT PARA_CODIGO FROM SoftCob_PARAMETRO_CABECERA (NOLOCK) WHERE para_nombre='TIPO PRODUCTO')),(SELECT CT.cpce_producto FROM SoftCob_CATALOGO_PRODUCTOS_CEDENTE CT (NOLOCK) WHERE CT.CPCE_CODIGO=3)),Identificacion = PER.pers_numerodocumento,Cliente = PER.pers_nombrescompletos,Operacion = CDE.ctde_operacion,Exigible = CDE.ctde_valorexigible,FechaUltGestion = CDE.ctde_auxv3,FechaLlamar = CONVERT(varchar(10),revl_fechallamar,121),HoraLlamar = CONVERT(varchar(5),revl_horallamar,108),Gestor = (SELECT USU.usu_Nombres+' '+USU.usu_Apellidos FROM USUARIO USU (NOLOCK) WHERE USU.USU_CODIGO=CDE.ctde_gestorasignado) ";
                _sql += "FROM SoftCob_REGISTRO_VOLVERALLAMAR VLL (NOLOCK) INNER JOIN SoftCob_CUENTA_DEUDOR CDE (NOLOCK) ON VLL.revl_cldecodigo=CDE.CLDE_CODIGO INNER JOIN SoftCob_CLIENTE_DEUDOR CLI (nolock) ON CDE.CLDE_CODIGO = CLI.CLDE_CODIGO INNER JOIN SoftCob_PERSONA PER (NOLOCK) ON PER.PERS_CODIGO=VLL.revl_perscodigo WHERE ";

                switch (ViewState["Tipo"].ToString())
                {
                    case "0":
                    case "1":
                        _sql += "CONVERT(DATE,VLL.revl_fechacreacion,101) ";
                        break;
                    case "2":
                    case "3":
                        _sql += "CONVERT(DATE,VLL.revl_fechallamar,101) ";
                        break;
                }

                _sql += "BETWEEN CONVERT(DATE,'" + ViewState["FechaDesde"].ToString() + "',101) AND CONVERT(DATE,'" + ViewState["FechaHasta"].ToString() + "',101) AND CLI.CPCE_CODIGO=" + ViewState["CodigoCPCE"].ToString() + " AND VLL.revl_gestionado='NO' AND CDE.ctde_estado=1 ";

                if (ViewState["Tipo"].ToString() == "1" || ViewState["Tipo"].ToString() == "3")
                {
                    _sql += "AND revl_gestorasignado=" + ViewState["Gestor"].ToString();
                }

                //_sql += "ORDER BY VLL.revl_fechallamar,VLL.revl_horallamar";
                _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql, "", "", Session["Conectar"].ToString());

                ViewState["GrdvDatos"] = _dts.Tables[0];

                if (_dts.Tables[0].Rows.Count == 0)
                {
                    ImgExportar.Visible = false;
                    lblExportar.Visible = false;
                }

                GrdvDatos.DataSource = _dts;
                GrdvDatos.DataBind();
                GrdvDatos.UseAccessibleHeader = true;
                GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblError.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void ImgExportar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtb = (DataTable)ViewState["GrdvDatos"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    string FileName = "VolveraLlamar" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                LblError.Text = ex.ToString();
            }
        }

        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _fechallamar = GrdvDatos.DataKeys[e.Row.RowIndex].Values["FechaLlamar"].ToString();
                    _horallamar = GrdvDatos.DataKeys[e.Row.RowIndex].Values["HoraLlamar"].ToString();

                    _fechallamada = DateTime.ParseExact(_fechallamar, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    _horallamada = TimeSpan.Parse(_horallamar);

                    _fechaactual = DateTime.ParseExact(ViewState["FechaActual"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    _horaactual = TimeSpan.Parse(ViewState["HoraActual"].ToString());

                    if (_fechallamada == _fechaactual)
                    {
                        e.Row.Cells[1].BackColor = System.Drawing.Color.Coral;

                        if (_horallamada == _horaactual) e.Row.Cells[1].BackColor = System.Drawing.Color.Aquamarine;

                        if (_horallamada < _horaactual) e.Row.Cells[1].BackColor = System.Drawing.Color.Beige;
                    }

                    if (_fechallamada < _fechaactual) e.Row.Cells[1].BackColor = System.Drawing.Color.Cyan;
                }
            }
            catch (Exception ex)
            {
                LblError.Text = ex.ToString();
            }
        }

        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_ReporteListVolveraLlamar.aspx", true);
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
        }
        #endregion
    }
}