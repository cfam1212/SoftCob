namespace SoftCob.Views.Bitacora
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    public partial class WFrm_DatosBitacora : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {

                ViewState["Bitacora"] = Request["Bitacora"];
                ViewState["Fecha"] = Request["Fecha"];
                ViewState["FechaDesde"] = Request["FechaDesde"];
                ViewState["FechaHasta"] = Request["FechaHasta"];
                Lbltitulo.Text = "Consulta Bitacora << " + ViewState["Bitacora"].ToString() + " >>";
                ViewState["FechaActual"] = DateTime.Now.ToString("MM/dd/yyyy");
                LblFecha.InnerText = "FECHA BITACORA: " + ViewState["Fecha"].ToString();
                FunCargarMantenimiento();
                TabDatosBitacora.ActiveTabIndex = 0;
                PnlDatosSupervisor.Height = 100;
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(170, 0, 0, 0, "", ViewState["Bitacora"].ToString(), "",
                    Session["Conectar"].ToString());

                GrdvSupervisores.DataSource = _dts.Tables[0];
                GrdvSupervisores.DataBind();

                if (_dts.Tables[1].Rows.Count > 0)
                {
                    Pnl3.Visible = true;
                    GrdvAtrasos.DataSource = _dts.Tables[1];
                    GrdvAtrasos.DataBind();
                }

                if (_dts.Tables[2].Rows.Count > 0)
                {
                    Pnl4.Visible = true;
                    GrdvFaltasJ.DataSource = _dts.Tables[2];
                    GrdvFaltasJ.DataBind();
                }

                if (_dts.Tables[3].Rows.Count > 0)
                {
                    Pnl5.Visible = true;
                    GrdvFaltasI.DataSource = _dts.Tables[3];
                    GrdvFaltasI.DataBind();
                }

                if (_dts.Tables[4].Rows.Count > 0)
                {
                    Pnl6.Visible = true;
                    GrdvPermisos.DataSource = _dts.Tables[4];
                    GrdvPermisos.DataBind();
                }

                if (_dts.Tables[5].Rows.Count > 0)
                {
                    Pnl7.Visible = true;
                    GrdvCambioTurno.DataSource = _dts.Tables[5];
                    GrdvCambioTurno.DataBind();
                }

                if (_dts.Tables[6].Rows.Count > 0)
                {
                    Pnl8.Visible = true;
                    GrdvVarios.DataSource = _dts.Tables[6];
                    GrdvVarios.DataBind();
                }

                if (_dts.Tables[7].Rows.Count > 0)
                {
                    Pnl9.Visible = true;
                    GrdvNovedad.DataSource = _dts.Tables[7];
                    GrdvNovedad.DataBind();
                }

                if (_dts.Tables[8].Rows.Count > 0)
                {
                    Pnl10.Visible = true;
                    GrdvRefuerzo.DataSource = _dts.Tables[8];
                    GrdvRefuerzo.DataBind();
                }

                if (_dts.Tables[9].Rows.Count > 0)
                {
                    Pnl11.Visible = true;
                    GrdvTerreno.DataSource = _dts.Tables[9];
                    GrdvTerreno.DataBind();
                }

                if (_dts.Tables[10].Rows.Count > 0)
                {
                    Pnl12.Visible = true;
                    GrdvSistemas.DataSource = _dts.Tables[10];
                    GrdvSistemas.DataBind();
                }

                if (_dts.Tables[11].Rows.Count > 0)
                {
                    Pnl13.Visible = true;
                    GrdvPagos.DataSource = _dts.Tables[11];
                    GrdvPagos.DataBind();
                }

                if (_dts.Tables[12].Rows.Count > 0)
                {
                    Pnl14.Visible = true;
                    GrdvAdicionales.DataSource = _dts.Tables[12];
                    GrdvAdicionales.DataBind();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        #endregion

        #region Botones y Eventos
        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_ConsultaBitacoraAdmin.aspx?FechaDesde=" + ViewState["FechaDesde"].ToString() +
                "&FechaHasta=" + ViewState["FechaHasta"].ToString(), true);
        }
        #endregion
    }
}