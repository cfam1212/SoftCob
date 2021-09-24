namespace SoftCob.Views.BPM
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_RegistroCitacionTerreno : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        DropDownList _ddlrepuesta = new DropDownList();
        TextBox _txtobservacion = new TextBox();
        string _ext = "", _path = "", _codigo = "", _redirect = "", _codigoterreno = "", _respuesta = "",
             _observacion = "", _codiomatd = "", _fechavisita = "";
        decimal _totalExigible = 0.00M, _totalDeuda = 0.00M;
        int _totalregistros = 0, _contar = 0;
        DataTable _dtbcitaciones = new DataTable();
        DataTable _tblagre = new DataTable();
        DataRow[] _resultado;
        DateTime _dtmfechavisita;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                    Response.Redirect("~/Reload.html");

                Page.Form.Attributes.Add("enctype", "multipart/form-data");

                if (!IsPostBack)
                {
                    ViewState["CodigoCITA"] = Request["CodigoCITA"];
                    ViewState["CodigoPERS"] = Request["CodigoPERS"];
                    ViewState["CodigoCLDE"] = Request["CodigoCLDE"];
                    ViewState["NumDocumento"] = Request["NumDocumento"];

                    _dtbcitaciones.Columns.Add("CodigoCITA");
                    _dtbcitaciones.Columns.Add("Canal");
                    ViewState["Citaciones"] = _dtbcitaciones;

                    Lbltitulo.Text = "Registrar Entrega Citacion << TERRENO >>";
                    PnlDatosDeudor.Height = 100;
                    PnlDatosGetion.Height = 120;
                    PnlDatosGarante.Height = 120;
                    PnlCitaciones.Height = 230;
                    FunCargaMantenimiento();

                    if (Request["MensajeRetornado"] != null) new FuncionesDAO().FunShowJSMessage(Request["MensajeRetornado"],
                        this, "W", "R");
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargaMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(32, int.Parse(ViewState["CodigoPERS"].ToString()), 0, 0,
                    "", "", "", Session["Conectar"].ToString().ToString());

                GrdvDatosDeudor.DataSource = _dts;
                GrdvDatosDeudor.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(33, 0, 0, int.Parse(ViewState["CodigoCLDE"].ToString()),
                    "", "", "", Session["Conectar"].ToString());

                GrdvDatosObligacion.DataSource = _dts;
                GrdvDatosObligacion.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(45, 0, 0, 0, "", ViewState["NumDocumento"].ToString(), "",
                    Session["Conectar"].ToString().ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    TrGarantes.Visible = true;
                    GrdvDatosGarante.DataSource = _dts;
                    GrdvDatosGarante.DataBind();
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(246, 2, int.Parse(ViewState["CodigoCITA"].ToString()), 0,
                    "", "Terreno", "VIS", Session["Conectar"].ToString());

                ViewState["Citaciones"] = _dts.Tables[0];
                GrdvCitaciones.DataSource = _dts;
                GrdvCitaciones.DataBind();

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        //private string Funguardararchivo(HttpPostedFile file)
        //{
        //    _ruta = Server.MapPath("~/citacion/" + ViewState["NumDocumento"].ToString());

        //    if (file.FileName.ToString().Length > 80)
        //    {
        //        new FuncionesBAS().FunShowJSMessage("Nombre del archivo debe contener hasta 80 caracteres..!", this);
        //        return _ruta = "";
        //    }

        //    if (!Directory.Exists(_ruta))
        //        Directory.CreateDirectory(_ruta);

        //    _archivo = String.Format("{0}\\{1}", _ruta, file.FileName);

        //    if (File.Exists(_archivo)) File.Delete(_archivo);

        //    file.SaveAs(_archivo);
        //    return _ruta;
        //}
        #endregion

        #region Botones
        protected void GrdvDatosObligacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    _totalExigible += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Exigible"));
                    _totalDeuda += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ValorDeuda"));
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[2].Text = "TOTAL:";
                    e.Row.Cells[3].Text = _totalDeuda.ToString();
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[4].Text = _totalExigible.ToString();
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgCitacion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(242, int.Parse(ViewState["CodigoCITA"].ToString()), 0,
                    0, "", "", "", Session["Conectar"].ToString());

                _path = _dts.Tables[0].Rows[0]["Ruta"].ToString();
                _ext = _dts.Tables[0].Rows[0]["Extension"].ToString();

                switch (_ext)
                {
                    case "png":
                    case "jpg":
                    case "jpeg":
                    case "bmp":
                    case "gif":
                        ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); " +
                            "posicion_y=(screen.height/2)-(600/2); window.open('WFrm_DocumentosView.aspx?Path=" + _path +
                            "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=950px, height=550px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
                        break;
                    case "pdf":
                        ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); " +
                            "posicion_y=(screen.height/2)-(600/2); window.open('WFrm_ViewPdf.aspx?Path=" + _path +
                            "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=980px, height=550px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvCitaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList _ddlrespuesta = (e.Row.FindControl("DdlRespuesta") as DropDownList);
                    _dts = new ControllerDAO().FunGetParametroDetalle("TIPO TERRENO", "--Seleccione Opcion--", "S");

                    _ddlrespuesta.DataSource = _dts;
                    _ddlrespuesta.DataTextField = "Descripcion";
                    _ddlrespuesta.DataValueField = "Codigo";
                    _ddlrespuesta.DataBind();
                }

                if (e.Row.RowIndex >= 0)
                {
                    _respuesta = GrdvCitaciones.DataKeys[e.Row.RowIndex].Values["Respuesta"].ToString();
                    _observacion = GrdvCitaciones.DataKeys[e.Row.RowIndex].Values["Registro"].ToString();
                    _fechavisita = GrdvCitaciones.DataKeys[e.Row.RowIndex].Values["FechaVisita"].ToString();

                    _ddlrepuesta = (DropDownList)(e.Row.Cells[3].FindControl("DdlRespuesta"));
                    _txtobservacion = (TextBox)(e.Row.Cells[4].FindControl("TxtObservacion"));

                    _dtmfechavisita = DateTime.ParseExact(_fechavisita, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    if (DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture) >=
                        _dtmfechavisita)
                    {
                        _ddlrepuesta.Enabled = true;
                        _txtobservacion.Enabled = true;
                    }

                    //FileUpload _file = (FileUpload)(e.Row.Cells[5].FindControl("FileUpload1"));

                    if (_respuesta != "")
                    {
                        _ddlrepuesta.Enabled = false;
                        _txtobservacion.ReadOnly = true;
                        //_file.Enabled = false;
                    }

                    _ddlrepuesta.SelectedValue = _respuesta;
                    _txtobservacion.Text = _observacion;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                _dtb = (DataTable)ViewState["Citaciones"];
                _resultado = _dtb.Select("Respuesta=''");

                _totalregistros = _resultado.Count();

                foreach (DataRow _drfila in _resultado)
                {
                    _codigo = _drfila["CodigoTERR"].ToString();

                    foreach (GridViewRow i_row in GrdvCitaciones.Rows)
                    {
                        _ddlrepuesta = (DropDownList)GrdvCitaciones.Rows[i_row.RowIndex].Cells[4].FindControl("DdlRespuesta");
                        _txtobservacion = (TextBox)GrdvCitaciones.Rows[i_row.RowIndex].Cells[4].FindControl("TxtObservacion");
                        _codigoterreno = GrdvCitaciones.DataKeys[i_row.RowIndex]["CodigoTERR"].ToString();
                        _codiomatd = GrdvCitaciones.DataKeys[i_row.RowIndex]["CodigoMATD"].ToString();

                        if (_ddlrepuesta.SelectedValue != "0")
                        {
                            //FileUpload _file = (FileUpload)GrdvCitaciones.Rows[i_row.RowIndex].Cells[5].FindControl("FileUpload1");

                            //if (_file.HasFile)
                            //{
                            //    _ext = _file.PostedFile.FileName;
                            //    _ext = _ext.Substring(_ext.LastIndexOf(".") + 1).ToLower();
                            //    _formatos = new string[] { "jpg", "jpeg", "bmp", "png", "gif", "pdf" };

                            //    if (Array.IndexOf(_formatos, _ext) < 0)
                            //    {
                            //        new FuncionesBAS().FunShowJSMessage("Formato de imagen inválido..!", this);
                            //        return;
                            //    }

                            //    _nombre = _file.FileName.Substring(0, _file.FileName.LastIndexOf("."));
                            //    _contentType = _file.PostedFile.ContentType;
                            //    _ruta = Funguardararchivo(_file.PostedFile);
                            //    _path = "~/citacion/" + ViewState["NumDocumento"].ToString();

                            //    _totalname = _path + " " + _nombre + " " + _ext;
                            //}

                            if (_codigo == _codigoterreno)
                            {
                                _dts = new ConsultaDatosDAO().FunAgendaCitaciones(15, int.Parse(ViewState["CodigoCLDE"].ToString()),
                                    0, "", 0, "0", 0, _txtobservacion.Text.Trim().ToUpper(), "", "", "", "", "", "", "", "", "",
                                    new byte[0], "", "", "", "", "0", "0", 0, "", 0, _ddlrepuesta.SelectedValue,
                                    "Terreno", "", "", "", "", "", "", "", "", int.Parse(ViewState["CodigoCITA"].ToString()),
                                    int.Parse(_codigoterreno), int.Parse(_codiomatd), 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                                    Session["MachineName"].ToString(), Session["Conectar"].ToString());

                                _contar++;
                            }
                        }
                    }
                }

                if (_totalregistros == _contar)
                    Response.Redirect("WFrm_CitacionProcesoTerreno.aspx?MensajeRetornado=Gestion Terreno Realizada..!");
                else
                {
                    _redirect = string.Format("{0}?CodigoCITA={1}&CodigoPERS={2}&CodigoCLDE={3}&NumDocumento={4}" +
                        "&MensajeRetornado={5}", Request.Url.AbsolutePath, ViewState["CodigoCITA"].ToString(),
                        ViewState["CodigoPERS"].ToString(), ViewState["CodigoCLDE"].ToString(),
                        ViewState["NumDocumento"].ToString(), "Falta Gestionar Terrenos");

                    Response.Redirect(_redirect);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_CitacionProcesoTerreno.aspx", true);
        }

        #endregion
    }
}