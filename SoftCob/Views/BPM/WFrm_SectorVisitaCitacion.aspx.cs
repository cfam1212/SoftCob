namespace SoftCob.Views.BPM
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Globalization;
    using System.Threading;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_SectorVisitaCitacion : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        ImageButton _imglogo = new ImageButton();
        string _sector = "", _codigoterreno = "", _fecha = "", _codigo = "";
        decimal _totalExigible = 0.00M, _totalDeuda = 0.00M;
        DataTable _dtb = new DataTable();
        DropDownList _ddlsector = new DropDownList();
        CheckBox _chkvisita = new CheckBox();
        TextBox _txtfecha = new TextBox();
        DataRow _result;
        //DataRow[] _resultado;
        int _codigomatd = 0, _contar = 0, _totalregistro = 0;
        DateTime _dtmvisita, _dtmfechahoy;
        bool _continuar;
        string _mensaje = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                    Response.Redirect("~/Reload.html");

                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-EC");

                if (!IsPostBack)
                {
                    ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                    ViewState["CodigoCITA"] = Request["CodigoCITA"];
                    ViewState["CodigoCPCE"] = Request["CodigoCPCE"];
                    ViewState["CodigoPERS"] = Request["CodigoPERS"];
                    ViewState["CodigoCLDE"] = Request["CodigoCLDE"];
                    ViewState["CodigoGEST"] = Request["CodigoGEST"];
                    ViewState["Retornar"] = Request["Retornar"];

                    Lbltitulo.Text = "Sectorizar Notificaciones << TERRENO >>";
                    PnlDatosDeudor.Height = 100;
                    PnlDatosGetion.Height = 120;
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
                    "", "", "", ViewState["Conectar"].ToString().ToString());

                GrdvDatosDeudor.DataSource = _dts;
                GrdvDatosDeudor.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(33, 0, 0, int.Parse(ViewState["CodigoCLDE"].ToString()),
                    "", "", "", ViewState["Conectar"].ToString());

                GrdvDatosObligacion.DataSource = _dts;
                GrdvDatosObligacion.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(246, 2, int.Parse(ViewState["CodigoCITA"].ToString()), 0,
                    "", "Terreno", "", ViewState["Conectar"].ToString());

                ViewState["Terreno"] = _dts.Tables[0];

                GrdvTerreno.DataSource = _dts;
                GrdvTerreno.DataBind();
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
                    _totalDeuda += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "MontoGSPBO"));
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[3].Text = "TOTAL:";
                    e.Row.Cells[4].Text = _totalDeuda.ToString();
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[5].Text = _totalExigible.ToString();
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvTerreno_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList _ddlsector = (e.Row.FindControl("DdlSector") as DropDownList);
                    _dts = new ControllerDAO().FunGetParametroDetalle("TIPO SECTOR", "--Seleccione Sector--", "S");

                    _ddlsector.DataSource = _dts;
                    _ddlsector.DataTextField = "Descripcion";
                    _ddlsector.DataValueField = "Codigo";
                    _ddlsector.DataBind();
                }

                if (e.Row.RowIndex >= 0)
                {
                    _ddlsector = (DropDownList)(e.Row.Cells[4].FindControl("DdlSector"));
                    _txtfecha = (TextBox)(e.Row.Cells[6].FindControl("TxtFechaVisita"));
                    _sector = GrdvTerreno.DataKeys[e.Row.RowIndex].Values["CodigoSECT"].ToString();

                    _ddlsector.SelectedValue = _sector;
                    _txtfecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkVisitar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                _chkvisita = (CheckBox)(_gvRow.Cells[5].FindControl("ChkVisitar"));
                _txtfecha = (TextBox)(_gvRow.Cells[6].FindControl("TxtFechaVisita"));
                _codigoterreno = GrdvTerreno.DataKeys[_gvRow.RowIndex].Values["CodigoTERR"].ToString();

                if (_chkvisita.Checked) _txtfecha.Enabled = true;
                else _txtfecha.Enabled = false;

                _dtb = (DataTable)ViewState["Terreno"];
                _result = _dtb.Select("CodigoTERR='" + _codigoterreno + "'").FirstOrDefault();
                _result["Visitar"] = _chkvisita.Checked ? "SI" : "NO";
                _dtb.AcceptChanges();
                ViewState["Terreno"] = _dtb;
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
                try
                {
                    _dtb = (DataTable)ViewState["Terreno"];
                    _totalregistro = _dtb.Rows.Count;

                    _continuar = true;

                    foreach (DataRow _drfila in _dtb.Rows)
                    {
                        _codigo = _drfila["CodigoTERR"].ToString();

                        foreach (GridViewRow i_row in GrdvTerreno.Rows)
                        {
                            _ddlsector = (DropDownList)GrdvTerreno.Rows[i_row.RowIndex].Cells[4].FindControl("DdlSector");
                            _txtfecha = (TextBox)GrdvTerreno.Rows[i_row.RowIndex].Cells[2].FindControl("TxtFechaVisita");
                            _codigoterreno = GrdvTerreno.DataKeys[i_row.RowIndex]["CodigoTERR"].ToString();
                            _fecha = _txtfecha.Text;

                            if (_codigo == _codigoterreno)
                            {
                                if (_drfila["Visitar"].ToString() == "SI")
                                {
                                    if (!new FuncionesDAO().IsDate(_fecha, "yyyy-MM-dd"))
                                    {
                                        new FuncionesDAO().FunShowJSMessage("Fecha de Visita Incorrecta..!", this);
                                        _continuar = false;
                                        break;
                                    }

                                    _dtmvisita = DateTime.ParseExact(String.Format("{0}", _fecha), "yyyy-MM-dd",
                                        CultureInfo.InvariantCulture);

                                    _dtmfechahoy = DateTime.ParseExact(String.Format("{0}", DateTime.Now.ToString("yyyy-MM-dd")),
                                        "yyyy-MM-dd", CultureInfo.InvariantCulture);

                                    if (_dtmvisita <= _dtmfechahoy)
                                    {
                                        new FuncionesDAO().FunShowJSMessage("Fecha Visita, No debe ser menor a la fecha Actual", this);
                                        _continuar = false;
                                        break;
                                    }

                                    _contar++;
                                }

                                if (_ddlsector.SelectedValue == "0")
                                {
                                    new FuncionesDAO().FunShowJSMessage("Defina Sector de la Direccion..!", this);
                                    _continuar = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (_continuar)
                    {
                        foreach (GridViewRow i_row in GrdvTerreno.Rows)
                        {
                            _ddlsector = (DropDownList)GrdvTerreno.Rows[i_row.RowIndex].Cells[4].FindControl("DdlSector");
                            _chkvisita = (CheckBox)GrdvTerreno.Rows[i_row.RowIndex].Cells[5].FindControl("ChkVisitar");
                            _txtfecha = (TextBox)GrdvTerreno.Rows[i_row.RowIndex].Cells[6].FindControl("TxtFechaVisita");
                            _codigoterreno = GrdvTerreno.DataKeys[i_row.RowIndex]["CodigoTERR"].ToString();
                            _codigomatd = int.Parse(GrdvTerreno.DataKeys[i_row.RowIndex]["CodigoMATD"].ToString());
                            _fecha = _txtfecha.Text;

                            _dts = new ConsultaDatosDAO().FunAgendaCitaciones(16, int.Parse(ViewState["CodigoCLDE"].ToString()),
                                0, _fecha, 0, "0", 0, "", "", "", "", "", "", "", "", "", "", new byte[0], "", "", "", "", "0",
                                "0", 0, "", 0, "", _ddlsector.SelectedValue, _chkvisita.Checked ? "VIS" : "",
                                "Terreno", "", "", "", "", "", "", int.Parse(ViewState["CodigoCITA"].ToString()),
                                int.Parse(_codigoterreno), _codigomatd, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                                Session["MachineName"].ToString(), ViewState["Conectar"].ToString());
                        }

                        if (_contar == 0)
                            _mensaje = "No se Registro fecha de Visita..";

                        else
                        {
                            _mensaje = "Se registro " + _contar.ToString() + " Visitas Terreno de " + _totalregistro.ToString();
                            //_redirect = string.Format("{0}?CodigoCITA={1}&CodigoPERS={2}&CodigoCLDE={3}&NumDocumento={4}" +
                            //    "&MensajeRetornado={5}", Request.Url.AbsolutePath, ViewState["CodigoCITA"].ToString(),
                            //    ViewState["CodigoPERS"].ToString(), ViewState["CodigoCLDE"].ToString(),
                            //    ViewState["NumDocumento"].ToString(), "Falta Gestionar Terrenos");

                            //Response.Redirect(_redirect);
                        }

                        Response.Redirect("WFrm_ListaSolicitudTerreno.aspx?MensajeRetornado=" + _mensaje, true);
                    }
                }
                catch (Exception ex)
                {
                    Lblerror.Text = ex.ToString();
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
                    Response.Redirect("WFrm_ListaSolicitudTerreno.aspx", true);
                    break;
            }
        }
        #endregion
    }
}