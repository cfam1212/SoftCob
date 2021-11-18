namespace SoftCob.Views.BPM
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_SeguimientoConvenio : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        ImageButton _imglogo = new ImageButton();
        ImageButton _imgtabla = new ImageButton();
        string _whastapp = "", _email = "", _terreno = "", _canal = "", _contentType = "", _ruta = "", _path = "",
            _codigocita = "", _estadocodigo = "";
        decimal _totalExigible = 0.00M, _totalDeuda = 0.00M;
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
                    ViewState["NumDocumento"] = Request["NumDocumento"];

                    _dtbcitaciones.Columns.Add("CodigoCITA");
                    _dtbcitaciones.Columns.Add("Canal");
                    ViewState["Citaciones"] = _dtbcitaciones;
                    ViewState["VerHistorial"] = "0";

                    Lbltitulo.Text = "Segumiento Citacion << CONVENIO REALIZADO >>";
                    PnlDatosDeudor.Height = 100;
                    PnlDatosGetion.Height = 120;
                    PnlDatosGarante.Height = 120;
                    PnlCitaciones.Height = 230;

                    FunCargaMantenimiento();
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

                FunHistorial(0);

                _dts = new ConsultaDatosDAO().FunConsultaDatos(242, int.Parse(ViewState["CodigoCITA"].ToString()), 0, 0,
                    "", "", "", Session["Conectar"].ToString());

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
        private void FunHistorial(int opcion)
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(262, int.Parse(ViewState["CodigoCITA"].ToString()),
                    opcion, 0, "", "", "", Session["Conectar"].ToString().ToString());

                GrdvDetalle.DataSource = _dts;
                GrdvDetalle.DataBind();
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
                            _grdvCanales.Columns[7].Visible = false;
                            _grdvCanales.Columns[9].Visible = false;
                            _opcion = 4;
                            break;
                        case "Email":
                            _grdvCanales.Columns[2].Visible = false;
                            _grdvCanales.Columns[4].Visible = false;
                            _grdvCanales.Columns[5].Visible = false;
                            _grdvCanales.Columns[6].Visible = false;
                            _grdvCanales.Columns[7].Visible = false;
                            _grdvCanales.Columns[9].Visible = false;
                            _opcion = 5;
                            break;
                        case "Terreno":
                            _grdvCanales.Columns[2].Visible = false;
                            _grdvCanales.Columns[3].Visible = false;
                            _opcion = 6;
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

        protected void LnkHistorial_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["VerHistorial"].ToString() == "1")
                {
                    ViewState["VerHistorial"] = "0";
                    LnkHistorial.Text = "Todo el Historial";
                    FunHistorial(0);
                }
                else
                {
                    FunHistorial(1);
                    ViewState["VerHistorial"] = "1";
                    LnkHistorial.Text = "Últimos Diez Registros";
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvDetalle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imglogo = (ImageButton)(e.Row.Cells[4].FindControl("ImgArchivo"));
                    _imgtabla = (ImageButton)(e.Row.Cells[5].FindControl("ImgTabla"));

                    _path = GrdvDetalle.DataKeys[e.Row.RowIndex].Values["PathArchivo"].ToString();
                    _estadocodigo = GrdvDetalle.DataKeys[e.Row.RowIndex].Values["CodigoESTA"].ToString();

                    if (!string.IsNullOrEmpty(_path))
                    {
                        _imglogo.ImageUrl = "~/Botones/busqueda.png";
                        _imglogo.Enabled = true;
                    }

                    switch (_estadocodigo)
                    {
                        case "CSL":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.DarkOrange;
                            break;
                        case "CMI":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.Red;
                            break;
                        case "CPR":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.Coral;
                            break;
                        case "CGE":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.Cyan;
                            break;
                        case "CRE":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.Bisque;
                            break;
                        case "CCV":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.AliceBlue;
                            _imgtabla.Visible = true;
                            break;
                        case "CCS":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.LimeGreen;
                            break;
                        case "CNA":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.Beige;
                            break;
                        case "CSV":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.SeaGreen;
                            break;
                        case "CAS":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.Gold;
                            break;
                        case "SEND":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.LightBlue;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgArchivo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                _contentType = GrdvDetalle.DataKeys[gvRow.RowIndex].Values["Content"].ToString();
                _path = GrdvDetalle.DataKeys[gvRow.RowIndex].Values["PathArchivo"].ToString();

                switch (_contentType)
                {
                    case "application/pdf":
                        ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); " +
                            "posicion_y=(screen.height/2)-(600/2); window.open('WFrm_ViewPdf.aspx?Path=" + _path +
                            "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=980px, height=550px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
                        break;
                    default:
                        ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); " +
                            "posicion_y=(screen.height/2)-(600/2); window.open('WFrm_DocumentosView.aspx?Path=" + _path +
                            "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=950px, height=550px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgTabla_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); " +
                "posicion_y=(screen.height/2)-(600/2); window.open('WFrm_TablaPagosConvenio.aspx?CodigoCITA=" +
                 ViewState["CodigoCITA"].ToString() + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=650px, height=600px, " +
                "status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_ListaTipoConvenio.aspx", true);
        }
        #endregion
    }
}