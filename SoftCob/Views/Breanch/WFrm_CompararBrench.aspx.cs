

namespace SoftCob.Views.Breanch
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_CompararBrench : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        decimal _porcumplido = 0.00M;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                Lbltitulo.Text = "Comparar Presupuestos << COMPROMISOS DE PAGO --- PAGOS REALIZADOS >> ";

                ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                ViewState["CodigoCPCE"] = Request["CodigoCPCE"];

                PnlDatosCompromiso_CollapsiblePanelExtender.Collapsed = false;
                PnlDatosPagos_CollapsiblePanelExtender.Collapsed = false;
                FunCargaMantenimiento();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargaMantenimiento()
        {
            _dts = new ConsultaDatosDAO().FunConsultaDatos(218, int.Parse(ViewState["CodigoCPCE"].ToString()),
                0, 0, "", "", "", ViewState["Conectar"].ToString());
            GrdvCompromisos.DataSource = _dts;
            GrdvCompromisos.DataBind();

            _dts = new ConsultaDatosDAO().FunConsultaDatos(219, int.Parse(ViewState["CodigoCPCE"].ToString()),
                0, 0, "", "", "", ViewState["Conectar"].ToString());
            GrdvPagos.DataSource = _dts;
            GrdvPagos.DataBind();
        }
        #endregion

        #region Botones y Eventos
        protected void GrdvCompromisos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _porcumplido = decimal.Parse(GrdvCompromisos.DataKeys[e.Row.RowIndex].Values["PorCumplido"].ToString());

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(216, int.Parse(ViewState["CodigoCPCE"].ToString()), 1,
                        0, "", _porcumplido.ToString(), "", ViewState["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        e.Row.Cells[4].Text = _dts.Tables[0].Rows[0]["Etiqueta"].ToString();
                        e.Row.Cells[4].ForeColor = System.Drawing.ColorTranslator.FromHtml(_dts.Tables[0].Rows[0]["Color"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _porcumplido = decimal.Parse(GrdvPagos.DataKeys[e.Row.RowIndex].Values["PorCumplido"].ToString());

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(216, int.Parse(ViewState["CodigoCPCE"].ToString()), 1,
                        0, "", _porcumplido.ToString(), "", ViewState["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        e.Row.Cells[4].Text = _dts.Tables[0].Rows[0]["Etiqueta"].ToString();
                        e.Row.Cells[4].ForeColor = System.Drawing.ColorTranslator.FromHtml(_dts.Tables[0].Rows[0]["Color"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.close();", true);
        }
        #endregion
    }
}