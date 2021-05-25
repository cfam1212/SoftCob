namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Linq;
    using System.Web.UI.WebControls;
    public partial class WFrm_MonitorTimeGestor : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        TimeSpan _turnotarde, _turnonoche, _tiempoactual, _diferencia, _tiempo1, _tiempo2;
        string _time1 = "", _time2 = "", _validar = "";
        int _minutoslatencia = 0;
        Label _lblcalifica = new Label();
        DataRow _result;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                _tiempoactual = DateTime.Now.TimeOfDay;
                _dts = new ControllerDAO().FunGetDatosParametroDet("HORARIOS SALIDA");

                _result = _dts.Tables[0].Select("Prametro='VALIDAR'").FirstOrDefault();
                if (_result != null)
                {
                    _validar = _result[1].ToString();
                }

                ViewState["FechaActual"] = DateTime.Now.ToString("MM/dd/yyyy");
                Lbltitulo.Text = "Monitoreo Challenger - Tiempos - Gestión - Llamada";

                if (_validar == "SI")
                {
                    if (_dts.Tables[0].Rows.Count == 0) Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);

                    if (_dts.Tables[0].Rows[0]["Prametro"].ToString() == "SALIDA TARDE") _turnotarde = TimeSpan.Parse(_dts.Tables[0].Rows[0]["ValorV"].ToString());

                    if (_dts.Tables[0].Rows[1]["Prametro"].ToString() == "SALIDA NOCHE") _turnonoche = TimeSpan.Parse(_dts.Tables[0].Rows[1]["ValorV"].ToString());

                    if (_dts.Tables[0].Rows[2]["Prametro"].ToString() == "MINUTOS LATENCIA") _minutoslatencia = int.Parse(_dts.Tables[0].Rows[2]["ValorI"].ToString());

                    if (Session["IN-CALL"].ToString() == "SI" || Session["codigoCPCE"] == null) Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);

                    _diferencia = _turnotarde - _tiempoactual;

                    if (_diferencia.Hours > 0) Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);

                    if (_diferencia.Hours == 0 && _diferencia.Minutes > _minutoslatencia) Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);

                    if (_diferencia.Hours < 0)
                    {
                        _diferencia = _turnonoche - _tiempoactual;

                        if (_diferencia.Hours > 0) Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);

                        if (_diferencia.Hours == 0 && _diferencia.Minutes > _minutoslatencia) Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
                    }
                }

                FunCargarMantenimiento();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {

                if (Session["CodigoCPCE"] == null)
                {
                    new FuncionesDAO().FunShowJSMessage("No ha realizado ninguna gestión para monitorear sus resultados..!", this);
                    return;
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(100, int.Parse(Session["usuCodigo"].ToString()), 0, 0, "",
                    ViewState["FechaActual"].ToString(), ViewState["FechaActual"].ToString(), Session["Conectar"].ToString());
                GrdvEfectivas.DataSource = _dts.Tables[0];
                GrdvEfectivas.DataBind();
                GrdvMaxLlamada.DataSource = _dts.Tables[1];
                GrdvMaxLlamada.DataBind();
                _dts = new ConsultaDatosDAO().FunConsultaDatos(102, int.Parse(Session["CodigoCPCE"].ToString()), 0, 0, "",
                    ViewState["FechaActual"].ToString(), ViewState["FechaActual"].ToString(), Session["Conectar"].ToString());
                ViewState["Efectivas"] = _dts.Tables[0].Rows.Count;
                GrdvChallenger.DataSource = _dts.Tables[0];
                GrdvChallenger.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void GrdvEfectivas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    _time1 = (string)(DataBinder.Eval(e.Row.DataItem, "TotalGestion"));
                    _time2 = (string)(DataBinder.Eval(e.Row.DataItem, "TotalLlamada"));
                    _tiempo1 += TimeSpan.Parse(_time1);
                    _tiempo2 += TimeSpan.Parse(_time2);
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[1].Text = "TOTAL:";
                    e.Row.Cells[2].Text = _tiempo1.Hours.ToString("00") + ":" + _tiempo1.Minutes.ToString("00") + ":" + _tiempo1.Seconds.ToString("00");
                    e.Row.Cells[3].Text = _tiempo2.Hours.ToString("00") + ":" + _tiempo2.Minutes.ToString("00") + ":" + _tiempo2.Seconds.ToString("00");
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void GrdvChallenger_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (int.Parse(ViewState["Efectivas"].ToString()) > 2)
                {
                    _lblcalifica = (Label)(e.Row.Cells[4].FindControl("lblCalifica"));
                    if (e.Row.RowIndex == 0)
                    {
                        _lblcalifica.Text = "EXECELENTE GESTION";
                        e.Row.Cells[0].BackColor = System.Drawing.Color.LawnGreen;
                        e.Row.Cells[4].BackColor = System.Drawing.Color.LawnGreen;
                    }
                    if (e.Row.RowIndex == int.Parse(ViewState["Efectivas"].ToString()) - 1)
                    {
                        _lblcalifica.Text = "PONER MAS EMPEÑO";
                        e.Row.Cells[0].BackColor = System.Drawing.Color.Red;
                        e.Row.Cells[4].BackColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}