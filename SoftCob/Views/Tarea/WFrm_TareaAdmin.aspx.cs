namespace SoftCob.Views.Tarea
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class WFrm_TareaAdmin : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        string _codigo = "", _mensaje = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                Lbltitulo.Text = "Administrar Tareas";
                FunCargaMantenimiento();

                //if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", 
                //    Request["MensajeRetornado"].ToString());

                if (Request["MensajeRetornado"] != null)
                {
                    _mensaje = Request["MensajeRetornado"];
                    ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                        "'top-center'); alertify.success('" + _mensaje + "', 5, function(){console.log('dismissed');});", true);
                }

            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargaMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatosNew(0, int.Parse(Session["CodigoEMPR"].ToString()),
                    "", "", "", "", "", "", 0, 0, 0, 0, 0, 0, Session["Conectar"].ToString());
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
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_TareaNueva.aspx?Codigo=0", true);
        }

        protected void Btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
            Response.Redirect("WFrm_TareaNueva.aspx?Codigo=" + _codigo, true);
        }
        #endregion
    }
}