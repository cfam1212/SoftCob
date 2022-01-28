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
    public partial class WFrm_RegistrarConvenio : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        ImageButton _imglogo = new ImageButton();
        string _whastapp = "", _email = "", _terreno = "", _canal = "", _documento = "", _nombres = "", _ruta = "",
             _archivo = "", _codigocita = "";
        DataTable _dtbcitaciones = new DataTable();
        DataTable _tblagre = new DataTable();
        DataRow _filagre;
        GridView _grdvCanales;
        TextBox _txtfecha = new TextBox();
        int _opcion = 0;
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
                    FunCargaMantenimiento();
                    FunCargarCombos(0);
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

        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    DdlRegistro.DataSource = new ControllerDAO().FunGetParametroDetalle("REGISTRO CITACION", "--Seleccione Sector--", "S");
                    DdlRegistro.DataTextField = "Descripcion";
                    DdlRegistro.DataValueField = "Codigo";
                    DdlRegistro.DataBind();

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(207, 1, 0, 0, "", ViewState["NumDocumento"].ToString(),
                        "", Session["Conectar"].ToString());

                    DdlConvenio.DataSource = _dts;
                    DdlConvenio.DataTextField = "Descripcion";
                    DdlConvenio.DataValueField = "Codigo";
                    DdlConvenio.DataBind();
                    break;
            }
        }

        private string Funguardararchivo(HttpPostedFile file)
        {
            // Se carga la ruta física de la carpeta temp del sitio
            _ruta = Server.MapPath("~/citacion/" + ViewState["NumDocumento"].ToString());

            if (file.FileName.ToString().Length > 80)
            {
                new FuncionesDAO().FunShowJSMessage("Nombre del archivo debe contener hasta 80 caracteres..!", this, "W", "C");
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


        #endregion

        #region Botones y Eventos
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
                            _imglogo.ImageUrl = "~/Botones/btnemailcitacion.png";
                            break;
                        case "Terreno":
                            _imglogo.ImageUrl = "~/Botones/casa.png";
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
                            _opcion = 0;
                            break;
                        case "Email":
                            _grdvCanales.Columns[2].Visible = false;
                            _grdvCanales.Columns[4].Visible = false;
                            _grdvCanales.Columns[5].Visible = false;
                            _grdvCanales.Columns[6].Visible = false;
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

        protected void DdlRegistro_SelectedIndexChanged(object sender, EventArgs e)
        {
            TrConevio.Visible = false;
            DdlConvenio.SelectedValue = "0";
            if (DdlRegistro.SelectedValue == "CCV") TrConevio.Visible = true;
        }

        protected void DdlConvenio_SelectedIndexChanged(object sender, EventArgs e)
        {
            TrDatos.Visible = false;
            TxtDocumento.Text = "";
            TxtNombres.Text = "";
            if (DdlConvenio.SelectedValue == "1") TrDatos.Visible = true;
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (DdlRegistro.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Registro", this, "W", "C");
                    return;
                }
                else
                {
                    if (DdlRegistro.SelectedValue == "CCV")
                    {
                        if (DdlConvenio.SelectedValue == "0")
                        {
                            new FuncionesDAO().FunShowJSMessage("Seleccione Convenio..!", this, "W", "C");
                            return;
                        }
                    }

                    if (DdlConvenio.SelectedValue == "1")
                    {
                        if (string.IsNullOrEmpty(TxtDocumento.Text.Trim()) && string.IsNullOrEmpty(TxtNombres.Text.Trim()))
                        {
                            new FuncionesDAO().FunShowJSMessage("Ingrese No. Documento y Nombres", this, "W", "C");
                            return;
                        }
                    }
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(244, int.Parse(ViewState["CodigoCITA"].ToString()),
                    int.Parse(ViewState["CodigoCLDE"].ToString()), 0, "", DdlRegistro.SelectedValue == "CCV" ? "CAS" :
                    DdlRegistro.SelectedValue, "", Session["Conectar"].ToString());

                _dts = new ConsultaDatosDAO().FunConsultaDatos(261, int.Parse(ViewState["CodigoCITA"].ToString()),
                    int.Parse(Session["usuCodigo"].ToString()), 0, "CITACION ASISTE", DdlRegistro.SelectedValue == "CCV" ? "CAS" :
                    DdlRegistro.SelectedValue, Session["MachineName"].ToString(), Session["Conectar"].ToString());

                _nombres = DdlConvenio.SelectedItem.ToString() == "OTRO DATO" ?
                        TxtNombres.Text.Trim().ToUpper() : DdlConvenio.SelectedItem.ToString();

                _documento = DdlConvenio.SelectedItem.ToString() == "OTRO DATO" ?
                        TxtDocumento.Text.Trim().ToUpper() : DdlConvenio.SelectedValue;

                if (DdlRegistro.SelectedValue == "CCV")
                    Response.Redirect("WFrm_RegistroPagos.aspx?CodigoCITA=" + ViewState["CodigoCITA"].ToString() +
                        "&CodigoPERS=" + ViewState["CodigoPERS"].ToString() + "&CodigoCLDE=" +
                        ViewState["CodigoCLDE"].ToString() + "&CodigoGEST=" + ViewState["CodigoGEST"].ToString() +
                        "&NumDocumento=" + ViewState["NumDocumento"].ToString() + "&Documento=" +
                        _documento + "&Nombres=" + _nombres, true);
                else Response.Redirect("WFrm_RegistroCitaAdmin.aspx?MensajeRetornado=Registro Realizado", true);

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_RegistroCitacionAdmin.aspx", true);
        }
        #endregion
    }
}