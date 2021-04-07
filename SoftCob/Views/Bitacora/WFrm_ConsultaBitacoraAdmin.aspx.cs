namespace SoftCob.Views.Bitacora
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ConsultaBitacoraAdmin : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        string _bitacora, _estado, _fecha;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                Session["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                ViewState["FechaDesde"] = Request["FechaDesde"];
                ViewState["FechaHasta"] = Request["FechaHasta"];
                Lbltitulo.Text = "Administrar Bitacora";
                FunCargarMantenimiento();

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(171, 1, 0, 0, "", ViewState["FechaDesde"].ToString(),
                    ViewState["FechaHasta"].ToString(), Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    GrdvDatos.DataSource = _dts;
                    GrdvDatos.DataBind();
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ex)
            {
                Lbltitulo.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnDetalle_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _bitacora = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Bitacora"].ToString();
            _fecha = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Fecha"].ToString();
            Response.Redirect("WFrm_DatosBitacora.aspx?Bitacora=" + _bitacora + "&Fecha=" + _fecha + "&FechaDesde=" +
                ViewState["FechaDesde"].ToString() + "&FechaHasta=" + ViewState["FechaHasta"].ToString(), true);
        }

        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _estado = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();

                    if (_estado == "Activo")
                    {
                        e.Row.Cells[0].BackColor = System.Drawing.Color.Bisque;
                        e.Row.Cells[1].BackColor = System.Drawing.Color.Bisque;
                        e.Row.Cells[2].BackColor = System.Drawing.Color.Bisque;
                        e.Row.Cells[3].BackColor = System.Drawing.Color.Bisque;
                    }
                }
            }
            catch (Exception ex)
            {
                Lbltitulo.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_ConsultaBitacora.aspx", true);
        }
        #endregion
    }
}