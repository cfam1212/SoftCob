namespace SoftCob.Views.Usuarios
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_UsuarioAdmin : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        string _mensaje = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                    Response.Redirect("~/Reload.html");

                if (!IsPostBack)
                {
                    Lbltitulo.Text = "Administrar Usuario";
                    FunCargarMantenimiento();

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
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                dts = new ConsultaDatosDAO().FunConsultaDatosNew(9, int.Parse(Session["CodigoEMPR"].ToString()),
                    "", "", "", "", "", "", 0, 0, 0, 0, 0, 0, Session["Conectar"].ToString());
                if (dts.Tables[0].Rows.Count > 0)
                {
                    GrdvDatos.DataSource = dts;
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
            Response.Redirect("WFrm_UsuarioNuevo.aspx?Codigo=0", true);
        }
        #endregion
    }
}