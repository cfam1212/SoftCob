namespace SoftCob.Views.BPM
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_RegistrarCitacionTime : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        ImageButton _imglogo = new ImageButton();
        string _whastapp = "", _email = "", _terreno = "", _canal = "", _ext = "", _nombre = "", _horacodigo = "",
            _horacita = "", _contentType = "", _ruta = "", _path = "", _archivo = "", _totalname = "", _codigocita = "",
            _estadocodigo = "", _codigogest = "", _codigopers = "";
        decimal _totalExigible = 0.00M, _totalDeuda = 0.00M;
        DataTable _dtbcitaciones = new DataTable();
        DataTable _tblagre = new DataTable();
        DataRow _filagre;
        GridView _grdvCanales;
        ImageButton _imgcitacion = new ImageButton();
        DateTime _dtmcitacion, _dtmactual;
        TextBox _txtfecha = new TextBox();
        string[] _formatos;
        int _opcion = 0, _minutos = 0;
        bool _continuar = false;
        TimeSpan _horainicio, _horafin;
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
                    ViewState["CodigoGEST"] = Request["CodigoGEST"];
                    ViewState["NumDocumento"] = Request["NumDocumento"];
                    ViewState["Retornar"] = Request["Retornar"];
                    ViewState["Agendado"] = "NO";

                    ViewState["Fechaactual"] = DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy",
                        CultureInfo.InvariantCulture);

                    _dtbcitaciones.Columns.Add("CodigoCITA");
                    _dtbcitaciones.Columns.Add("Canal");
                    ViewState["Citaciones"] = _dtbcitaciones;

                    Lbltitulo.Text = "Registrar Citacion";
                    PnlDatosDeudor.Height = 100;
                    PnlDatosGetion.Height = 120;
                    PnlDatosGarante.Height = 120;
                    PnlCitaciones.Height = 230;

                    CalenCitaciones.SelectedDate = DateTime.Now;
                    FunCargaMantenimiento();
                    FunCitacionHoras(CalenCitaciones.SelectedDate.ToString("MM/dd/yyyy"), 0);
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

                _dts = new ConsultaDatosDAO().FunConsultaDatos(242, int.Parse(ViewState["CodigoCITA"].ToString()), 0, 0,
                    "", "", "", Session["Conectar"].ToString());

                LblObservacion.Text = _dts.Tables[0].Rows[0]["Observacion"].ToString();
                LblValor.Text = _dts.Tables[0].Rows[0]["ValorCitacion"].ToString();

                if (_dts.Tables[0].Rows[0]["Estado"].ToString() == "REV") ViewState["Agendado"] = "SI";

                _whastapp = _dts.Tables[0].Rows[0]["Whastapp"].ToString();
                _email = _dts.Tables[0].Rows[0]["Email"].ToString();
                _terreno = _dts.Tables[0].Rows[0]["Terreno"].ToString();

                _tblagre = new DataTable();
                _tblagre = (DataTable)ViewState["Citaciones"];

                if (!string.IsNullOrEmpty(_whastapp))
                {
                    _filagre = _tblagre.NewRow();
                    _filagre["CodigoCITA"] = ViewState["CodigoCITA"].ToString();
                    _filagre["Canal"] = _whastapp;
                    _tblagre.Rows.Add(_filagre);
                }

                if (!string.IsNullOrEmpty(_email))
                {
                    _filagre = _tblagre.NewRow();
                    _filagre["CodigoCITA"] = ViewState["CodigoCITA"].ToString();
                    _filagre["Canal"] = _email;
                    _tblagre.Rows.Add(_filagre);
                }

                if (!string.IsNullOrEmpty(_terreno))
                {
                    _filagre = _tblagre.NewRow();
                    _filagre["CodigoCITA"] = ViewState["CodigoCITA"].ToString();
                    _filagre["Canal"] = _terreno;
                    _tblagre.Rows.Add(_filagre);
                }

                ViewState["Citaciones"] = _tblagre;
                GrdvCitaciones.DataSource = _tblagre;
                GrdvCitaciones.DataBind();

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private string FunGuardarArchivo(HttpPostedFile file)
        {
            // Se carga la ruta física de la carpeta temp del sitio
            _ruta = Server.MapPath("~/citacion/" + ViewState["NumDocumento"].ToString());

            if (file.FileName.ToString().Length > 80)
            {                
                new FuncionesDAO().FunShowJSMessage("Nombre del archivo debe contener hasta 80 caracteres..!", this, "E", "R");
                return _ruta = "";
            }

            // Si el directorio no existe, crearlo
            if (!Directory.Exists(_ruta))
                Directory.CreateDirectory(_ruta);

            _archivo = String.Format("{0}\\{1}", _ruta, file.FileName);

            // Verificar que el archivo no exista
            if (File.Exists(_archivo)) File.Delete(_archivo);

            file.SaveAs(_archivo);
            return _ruta;
        }

        private void FunCitacionHoras(string fecha, int horacodigo)
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunAgendaCitaciones(0, int.Parse(ViewState["CodigoCLDE"].ToString()),
                    int.Parse(ViewState["CodigoPERS"].ToString()), fecha, horacodigo, "0", int.Parse(Session["usuCodigo"].ToString()),
                    "", "", "", "", "", "", "", "", "", "", new byte[0], "", "", "", "", "", "0", 0, "", 0, "", "", "",
                    "", "", "", "", "", "", "", 0, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                    Session["MachineName"].ToString(), Session["Conectar"].ToString());

                ViewState["Datoscitaciones"] = _dts.Tables[0];
                GrdvHorarios.DataSource = _dts;
                GrdvHorarios.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
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
        protected void GrdvHorarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgcitacion = (ImageButton)(e.Row.Cells[0].FindControl("Imgcitacion"));
                    _estadocodigo = GrdvHorarios.DataKeys[e.Row.RowIndex].Values["EstadoCodigo"].ToString();
                    _horainicio = TimeSpan.Parse(GrdvHorarios.DataKeys[e.Row.RowIndex].Values["HoraInicio"].ToString());
                    _horafin = TimeSpan.Parse(GrdvHorarios.DataKeys[e.Row.RowIndex].Values["HoraFin"].ToString());
                    _codigogest = GrdvHorarios.DataKeys[e.Row.RowIndex].Values["CodigoUSUA"].ToString();
                    _codigopers = GrdvHorarios.DataKeys[e.Row.RowIndex].Values["CodigoPERS"].ToString();

                    switch (_estadocodigo)
                    {
                        case "DIS":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.MediumSeaGreen;
                            _imgcitacion.Enabled = true;

                            if (DateTime.ParseExact(CalenCitaciones.SelectedDate.ToString("MM/dd/yyyy"), "MM/dd/yyyy",
                                CultureInfo.InvariantCulture) == (DateTime)ViewState["Fechaactual"])
                            {
                                if (!new FuncionesDAO().Between(TimeSpan.Parse(DateTime.Now.ToString("HH:mm")), _horainicio, _horafin))
                                {
                                    _imgcitacion.ImageUrl = "~/Botones/citamedicadesac.png";
                                    _imgcitacion.Enabled = false;
                                    _imgcitacion.Height = 15;
                                    e.Row.Cells[1].BackColor = System.Drawing.Color.SlateGray;
                                }
                            }
                            break;
                        case "REV":
                            if (_codigogest != Session["usuCodigo"].ToString()) _imgcitacion.ImageUrl = "~/Botones/btncitagrisbg.png";
                            else
                            {
                                if (_codigopers != ViewState["CodigoPERS"].ToString()) _imgcitacion.ImageUrl = "~/Botones/btncitagrisbg.png";
                                else
                                {
                                    _imgcitacion.ImageUrl = "~/Botones/btncitaeliminabg.png";
                                    _imgcitacion.Enabled = true;
                                }
                            }
                            e.Row.Cells[1].BackColor = System.Drawing.Color.Tomato;
                            break;
                        case "CSL":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.DarkOrange;
                            _imgcitacion.ImageUrl = "~/Botones/btncitasolicitabg.png";
                            break;
                        case "CPR":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.Coral;
                            _imgcitacion.ImageUrl = "~/Botones/btncitaprocesobg.png";
                            break;
                        case "CRE":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.Bisque;
                            _imgcitacion.ImageUrl = "~/Botones/btncitarealizada.png";
                            break;
                        case "CMI":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.Red;
                            _imgcitacion.ImageUrl = "~/Botones/btnnoemailbg.png";
                            break;
                        case "CGE":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.Cyan;
                            _imgcitacion.ImageUrl = "~/Botones/btncitageneradabg.png";
                            break;
                        case "CCS":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.LimeGreen;
                            _imgcitacion.ImageUrl = "~/Botones/btntodosbg.png";
                            break;
                        case "CNA":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.BurlyWood;
                            _imgcitacion.ImageUrl = "~/Botones/quitar_usuariobg.png";
                            break;
                        case "CSV":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.SeaGreen;
                            _imgcitacion.ImageUrl = "~/Botones/btncitagrisbg.png";
                            break;
                        case "CLI":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.Gray;
                            break;
                        case "CCV":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.AliceBlue;
                            break;
                        case "CAS":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.Gold;
                            break;
                        case "CLT":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.DarkSalmon;
                            _imgcitacion.ImageUrl = "~/Botones/btncitaeliminabg.png";
                            break;
                    }
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
                if (e.Row.RowIndex >= 0)
                {
                    _imglogo = (ImageButton)(e.Row.Cells[1].FindControl("ImgLogo"));
                    _canal = GrdvCitaciones.DataKeys[e.Row.RowIndex].Values["Canal"].ToString();

                    switch (_canal)
                    {
                        case "Whatsapp":
                            _imglogo.ImageUrl = "~/Botones/btnwhastapp.png";
                            break;
                        case "Email":
                            _imglogo.ImageUrl = "~/Botones/btnemailcitacionbg.png";
                            break;
                        case "Terreno":
                            _imglogo.ImageUrl = "~/Botones/casabg.png";
                            _txtfecha.Enabled = true;
                            break;
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    _canal = GrdvCitaciones.DataKeys[e.Row.RowIndex].Values["Canal"].ToString();
                    _codigocita = GrdvCitaciones.DataKeys[e.Row.RowIndex].Values["CodigoCITA"].ToString();
                    _grdvCanales = e.Row.FindControl("GrdvCanales") as GridView;

                    switch (_canal)
                    {
                        case "Whatsapp":
                            _grdvCanales.Columns[1].Visible = false;
                            _grdvCanales.Columns[3].Visible = false;
                            _grdvCanales.Columns[4].Visible = false;
                            _grdvCanales.Columns[5].Visible = false;
                            _grdvCanales.Columns[6].Visible = false;
                            _grdvCanales.Columns[7].Visible = false;
                            _opcion = 0;
                            break;
                        case "Email":
                            _grdvCanales.Columns[2].Visible = false;
                            _grdvCanales.Columns[4].Visible = false;
                            _grdvCanales.Columns[5].Visible = false;
                            _grdvCanales.Columns[6].Visible = false;
                            _grdvCanales.Columns[7].Visible = false;
                            _opcion = 1;
                            break;
                        case "Terreno":
                            _grdvCanales.Columns[2].Visible = false;
                            _grdvCanales.Columns[3].Visible = false;
                            _opcion = 2;
                            break;
                    }

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(246, _opcion, int.Parse(_codigocita), 0, "", _canal,
                        "VIS", Session["Conectar"].ToString());

                    if (_canal == "Terreno") ViewState["CitasTerreno"] = _dts.Tables[0];

                    _grdvCanales.DataSource = _dts;
                    _grdvCanales.DataBind();                    
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void Imgcitacion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                FunCitacionHoras(CalenCitaciones.SelectedDate.ToString("MM/dd/yyyy"), 0);
                _horacodigo = GrdvHorarios.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                _horacita = GrdvHorarios.DataKeys[gvRow.RowIndex].Values["HoraInicio"].ToString();
                _estadocodigo = GrdvHorarios.DataKeys[gvRow.RowIndex].Values["EstadoCodigo"].ToString();

                if (_estadocodigo == "REV")
                {
                    _dts = new ConsultaDatosDAO().FunAgendaCitaciones(4, int.Parse(ViewState["CodigoCLDE"].ToString()),
                        int.Parse(ViewState["CodigoPERS"].ToString()), CalenCitaciones.SelectedDate.ToString("MM/dd/yyyy"),
                        int.Parse(_horacodigo), "0", int.Parse(Session["usuCodigo"].ToString()),
                        "", "", "", "", "", "", "", "", "", "", new byte[0], "",
                        "", "", "", "", "0", 0, "", 0, "CSL", "", "", "", "", "", "", "", "", "", int.Parse(ViewState["CodigoCITA"].ToString())
                        , 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                        Session["Conectar"].ToString());

                    ViewState["Agendado"] = "NO";
                }
                else
                {
                    _dts = new ConsultaDatosDAO().FunAgendaCitaciones(3, 0, int.Parse(ViewState["CodigoPERS"].ToString()),
                        "", 0, "0", 0, "", "", "", "", "", "", "", "", "", "", new byte[0], "", "", "", "", "", "0", 0,
                        "", 0, "", "", "", "", "", "", "", "", "", "", 0, 0, 0, 0, 0, 0, "",
                        Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        _estadocodigo = _dts.Tables[0].Rows[0]["Estado"].ToString();

                        if (_estadocodigo != "DIS")
                        {
                            new FuncionesDAO().FunShowJSMessage("Cliente ya tiene NOTIFICACION RESERVADA Fecha: " +
                                _estadocodigo, this, "W", "C");
                            return;
                        }
                    }

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(132, 1, 0, 0, "MINUTOS CITACION", "TIEMPOS PROGRAMADOS", "",
                        Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                        _minutos = int.Parse(_dts.Tables[0].Rows[0]["Valor"].ToString());
                    else _minutos = 60;

                    _horacita = DateTime.Parse(_horacita).ToString("HH:mm");

                    _dtmcitacion = DateTime.ParseExact(String.Format("{0}", CalenCitaciones.SelectedDate.ToString("MM/dd/yyyy")),
                        "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    _dtmactual = DateTime.ParseExact(String.Format("{0}", DateTime.Now.ToString("MM/dd/yyyy")), "MM/dd/yyyy",
                        CultureInfo.InvariantCulture);

                    if (_dtmcitacion <= _dtmactual)
                    {
                        new FuncionesDAO().FunShowJSMessage("Fecha Citacion, No debe ser Menor o igual a la fecha actual", this,
                            "W", "C");
                        return;
                    }

                    _dts = new ConsultaDatosDAO().FunAgendaCitaciones(1, int.Parse(ViewState["CodigoCLDE"].ToString()),
                        int.Parse(ViewState["CodigoPERS"].ToString()), CalenCitaciones.SelectedDate.ToString("MM/dd/yyyy"),
                        int.Parse(_horacodigo), "0", int.Parse(Session["usuCodigo"].ToString()),
                        "", "", "", "", "", "", "NO", "NO", "NO", "", new byte[0], "", "", "", "", "", "0", 0, "", 0,
                        "REV", "", "", "", "", "", "", "", "", "", int.Parse(ViewState["CodigoGEST"].ToString()),
                        int.Parse(ViewState["CodigoCITA"].ToString()), 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                        Session["MachineName"].ToString(), Session["Conectar"].ToString());

                    ViewState["Agendado"] = "SI";
                }

                FunCitacionHoras(CalenCitaciones.SelectedDate.ToString("MM/dd/yyyy"), int.Parse(_horacodigo));
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void CalenCitaciones_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                Panel1.GroupingText = "Horarios Citacion <<" + CalenCitaciones.SelectedDate.ToString("MM/dd/yyyy") + ">>";
                FunCitacionHoras(CalenCitaciones.SelectedDate.ToString("MM/dd/yyyy"), 0);
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
                if (ViewState["Agendado"].ToString() == "NO")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Fecha de Citacion", this, "W", "C");
                    return;
                }

                if (FileUpload1.HasFile)
                {
                    _ext = FileUpload1.PostedFile.FileName;
                    _ext = _ext.Substring(_ext.LastIndexOf(".") + 1).ToLower();
                    _formatos = new string[] { "jpg", "jpeg", "bmp", "png", "gif", "pdf" };

                    if (Array.IndexOf(_formatos, _ext) < 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Formato de imagen inválido..!", this, "W", "C");
                        return;
                    }

                    _nombre = FileUpload1.FileName.Substring(0, FileUpload1.FileName.LastIndexOf("."));
                    _contentType = FileUpload1.PostedFile.ContentType;
                    _ruta = FunGuardarArchivo(FileUpload1.PostedFile);
                    _path = "~/citacion/" + ViewState["NumDocumento"].ToString();

                    _totalname = _path + "/" + _nombre + "." + _ext;
                }
                else
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione un archivo..!", this, "W", "C");
                    return;
                }

                _dts = new ConsultaDatosDAO().FunAgendaCitaciones(10, int.Parse(ViewState["CodigoCLDE"].ToString()),
                    int.Parse(ViewState["CodigoPERS"].ToString()), "", 0, "", 0, "", "", "", "", "", "", "", "", "", "",
                    new byte[0], _path, _nombre, _ext, _contentType, "", "", 0, "", 0, "CGE", "", "", "", "", "", "", "",
                    "", "", int.Parse(ViewState["CodigoCITA"].ToString()), 0, 0, 0, 0,
                    int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                    Session["Conectar"].ToString());

                _dts = new ConsultaDatosDAO().FunAgendaCitaciones(9, 0, 0, "", 0, "", 0, "REGISTRO CITACION", "", "",
                    "", "", "", "", "", "", "", new byte[0], _totalname, "", "", _contentType, "", "", 0, "", 0,
                    "CGE", "", "", "", "", "", "", "", "", "", int.Parse(ViewState["CodigoCITA"].ToString()),
                    0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                    Session["Conectar"].ToString());

                switch (ViewState["Retornar"].ToString())
                {
                    case "0":
                        Response.Redirect("WFrm_ListaSolicitudGestores.aspx?MensajeRetornado=Notificación Generada", true);
                        break;
                    case "1":
                        Response.Redirect("WFrm_CitacionProcesoTime.aspx?MensajeRetornado=Notificación Generada", true);
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            switch (ViewState["Retornar"].ToString())
            {
                case "0":
                    Response.Redirect("WFrm_ListaSolicitudGestores.aspx", true);
                    break;
                case "1":
                    Response.Redirect("WFrm_CitacionProcesoTime.aspx", true);
                    break;
            }
        }
        #endregion
    }
}