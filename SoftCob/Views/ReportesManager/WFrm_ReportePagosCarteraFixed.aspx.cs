namespace SoftCob.Views.ReportesManager
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ReportePagosCarteraFixed : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        string _sql = "", _fileName = "";
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
                    ViewState["Catalogo"] = Request["Catalogo"];
                    ViewState["CodigoCPCE"] = Request["CodigoCPCE"];
                    ViewState["FechaDesde"] = Request["FechaDesde"];
                    ViewState["FechaHasta"] = Request["FechaHasta"];
                    ViewState["Accion"] = Request["Accion"];
                    ViewState["Efecto"] = Request["Efecto"];
                    ViewState["Respuesta"] = Request["Respuesta"];
                    ViewState["Contacto"] = Request["Contacto"];
                    ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                    Lbltitulo.Text = "Reporte Gestiones << " + ViewState["Catalogo"].ToString() + " >>";
                    FunCargarMantenimiento();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos Y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                _sql = "select Cliente = PE.pers_nombrescompletos,Identificacion = PE.pers_numerodocumento,Operacion = CD.ctde_operacion,";
                _sql += "Documento = AP.rpab_auxv2,FechaPago = convert(date,AP.rpab_fechapago,103),ValorPago = AP.rpab_valorpago,";
                _sql += "Gestor = (select US.usu_Nombres+' '+US.usu_Apellidos from USUARIO US where US.USU_CODIGO=AP.rpab_gestorasignado),";
                _sql += "Accion = (select AC.arac_descripcion from GSBPO_ACCION AC where AC.ARAC_CODIGO=AP.rpab_araccodigo),";
                _sql += "Efecto = (select EF.aref_descripcion from GSBPO_EFECTO EF where EF.AREF_CODIGO=AP.rpab_arefcodigo),";
                _sql += "Respuesta = (select RE.arre_descripcion from GSBPO_RESPUESTA RE where RE.ARRE_CODIGO=AP.rpab_arrecodigo),";
                _sql += "Contacto = (select CO.arco_descripcion from GSBPO_CONTACTO CO where CO.ARCO_CODIGO=AP.rpab_arcocodigo) ";
                _sql += "from GSBPO_REGISTRO_ABONOSPAGO AP (nolock) INNER JOIN GSBPO_CLIENTE_DEUDOR CL (nolock) ON AP.rpab_cldecodigo=CL.CLDE_CODIGO ";
                _sql += "INNER JOIN GSBPO_CUENTA_DEUDOR CD (nolock) ON CL.CLDE_CODIGO=CD.CLDE_CODIGO INNER JOIN GSBPO_PERSONA PE (nolock) ON CL.PERS_CODIGO=PE.PERS_CODIGO ";
                _sql += "where CL.CPCE_CODIGO=" + ViewState["CodigoCPCE"].ToString() + " and AP.rpab_fechapago between CONVERT(date,'" + ViewState["FechaDesde"].ToString() + "',101) and CONVERT(date,'";
                _sql += ViewState["FechaHasta"].ToString() + "',101) and ";

                if (ViewState["Accion"].ToString() != "0") _sql += "AP.rpab_araccodigo=" + ViewState["Accion"].ToString() + " and ";

                if (ViewState["Efecto"].ToString() != "0") _sql += "AP.rpab_arefcodigo=" + ViewState["Efecto"].ToString() + " and ";

                if (ViewState["Respuesta"].ToString() != "0") _sql += "AP.rpab_arrecodigo=" + ViewState["Respuesta"].ToString() + " and ";

                if (ViewState["Contacto"].ToString() != "0") _sql += "AP.rpab_arcocodigo=" + ViewState["Contacto"].ToString() + " and ";

                _sql = _sql.Remove(_sql.Length - 4);
                _sql += " order by AP.rpab_fechapago";
                _dts = new ConsultaDatosDAO().FunGetRerporteGestiones(1, int.Parse(ViewState["CodigoCEDE"].ToString()),
                    int.Parse(ViewState["CodigoCPCE"].ToString()), ViewState["FechaDesde"].ToString(),
                    ViewState["FechaHasta"].ToString(), "", "", _sql, "", 0, 0, ViewState["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ImgExportar.Visible = true;
                    LblExportar.Visible = true;
                    ViewState["GrdvDatos"] = _dts.Tables[0];
                    GrdvDatos.DataSource = _dts;
                    GrdvDatos.DataBind();
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
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
                    _fileName = "Reporte_PagosCartera_" + ViewState["Catalogo"].ToString() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + _fileName);
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
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_ReportePagosCartera.aspx?CodigoCEDE=" + ViewState["CodigoCEDE"].ToString() +
                "&CodigoCPCE=" + ViewState["CodigoCPCE"].ToString() + "&FechaDesde=" + ViewState["FechaDesde"].ToString() +
                "&FechaHasta=" + ViewState["FechaHasta"].ToString() + "&Accion=" + ViewState["Accion"].ToString() +
                "&Efecto=" + ViewState["Efecto"].ToString() + "&Respuesta=" + ViewState["Respuesta"].ToString() +
                "&Contacto=" + ViewState["Contacto"].ToString(), true);
        }
        #endregion
    }
}