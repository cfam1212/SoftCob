namespace SoftCob.Views.Breanch
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_BrenchPorGestor : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        Label lbltext = new Label();
        decimal _porcumplido = 0;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                Lbltitulo.Text = "Administrar BRENCH";
                Session["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                Lbltitulo.Text = "Agregar Nuevo Brench";
                PnlBrenchGlobal.Height = 100;
                PnlBrenchPagos.Height = 100;
                FunCargaMantenimiento();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargaMantenimiento()
        {
            _dts = new ConsultaDatosDAO().FunConsultaDatos(213, int.Parse(Session["usuCodigo"].ToString()),
                int.Parse(Session["CedeCodigo"].ToString()), 0, "", "", "", Session["Conectar"].ToString());
            PnlBrenchGlobal.GroupingText = "Presupuesto ( " + _dts.Tables[0].Rows[0]["Anio"].ToString() + " )" + " - ( " +
                _dts.Tables[0].Rows[0]["Mes"].ToString() + " ) Por Compromiso de Pago";
            GrdvBrenchGestor.DataSource = _dts;
            GrdvBrenchGestor.DataBind();

            _dts = new ConsultaDatosDAO().FunConsultaDatos(214, int.Parse(Session["usuCodigo"].ToString()),
                int.Parse(Session["CedeCodigo"].ToString()), 0, "", "", "", Session["Conectar"].ToString());
            PnlBrenchPagos.GroupingText = "Presupuesto ( " + _dts.Tables[0].Rows[0]["Anio"].ToString() + " )" + " - ( " +
                _dts.Tables[0].Rows[0]["Mes"].ToString() + " ) Pagos Reigistrados";
            GrdvBrenchPago.DataSource = _dts;
            GrdvBrenchPago.DataBind();

        }
        #endregion

        #region Botones y Eventos
        protected void GrdvBrenchGestor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _porcumplido = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PorCumplido"));

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(216, int.Parse(Session["codigoCPCE"].ToString()), 1,
                        0, "", _porcumplido.ToString(), "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        e.Row.Cells[6].Text = _dts.Tables[0].Rows[0]["Etiqueta"].ToString();
                        e.Row.Cells[6].ForeColor = System.Drawing.ColorTranslator.FromHtml(_dts.Tables[0].Rows[0]["Color"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvBrenchPago_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _porcumplido = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PorCumplido"));

                    if (_porcumplido >= 0 && _porcumplido <= 60)
                    {
                        e.Row.Cells[7].Text = "TOTAL APLICACIÓN";
                        e.Row.Cells[7].ForeColor = System.Drawing.Color.Red;
                    }

                    if (_porcumplido >= 61 && _porcumplido <= 70)
                    {
                        e.Row.Cells[7].Text = "MAS EMPEÑO";
                        e.Row.Cells[7].ForeColor = System.Drawing.Color.Tomato;
                    }

                    if (_porcumplido >= 71 && _porcumplido <= 80)
                    {
                        e.Row.Cells[7].Text = "SE PUEDE MEJORAR";
                        e.Row.Cells[7].ForeColor = System.Drawing.Color.Crimson;
                    }

                    if (_porcumplido >= 81 && _porcumplido <= 90)
                    {
                        e.Row.Cells[7].Text = "POR BUEN CAMINO";
                        e.Row.Cells[7].ForeColor = System.Drawing.Color.Cyan;
                    }

                    if (_porcumplido >= 91 && _porcumplido <= 100)
                    {
                        e.Row.Cells[7].Text = "FELICITACIONES!!!";
                        e.Row.Cells[7].ForeColor = System.Drawing.Color.Green;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgVer_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void GrdvBrenchRango_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}