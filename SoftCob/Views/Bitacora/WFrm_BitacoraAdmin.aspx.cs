namespace SoftCob.Views.Bitacora
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_BitacoraAdmin : Page
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
                if (Session["IN-CALL"].ToString() == "SI")
                {
                    new ElastixDAO().ElastixHangUp(Session["IPLocalAdress"].ToString(), 9999);
                    Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                    return;
                }

                ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(173, 0, 0, 0, "", "", "", ViewState["Conectar"].ToString());
                _dts = new ConsultaDatosDAO().FunConsultaDatos(171, 0, 0, 0, "", "", "", ViewState["Conectar"].ToString());

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

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_NuevaBitacora.aspx?Bitacora=&Estado=Activo&Fecha=" + DateTime.Now.ToString("yyyy-MM-dd"), true);
        }

        protected void Btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _bitacora = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Bitacora"].ToString();
            _estado = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Estado"].ToString();
            _fecha = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Fecha"].ToString();
            Response.Redirect("WFrm_NuevaBitacora.aspx?Bitacora=" + _bitacora + "&Estado=" + _estado +
                "&Fecha=" + _fecha, true);
        }
        #endregion
    }
}