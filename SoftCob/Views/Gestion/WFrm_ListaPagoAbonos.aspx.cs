namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ListaPagoAbonos : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        string _identificacion = "", _operacion = "", _codigocede = "", _codigocpce = "", _codigoclde = "", _codigopers = "",
        _mensaje = "";
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
                    if (Session["IN-CALL"].ToString() == "SI")
                    {
                        new FuncionesDAO().FunShowJSMessage("Se encuentra en Llamada, en cuanto termine la gestión podrá salir de la Lista de Trabajo..!", this);
                        Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                    }

                    Lbltitulo.Text = "Lista de Clientes <<-- ABONOS - PAGOS -->>";
                    ViewState["FechaActual"] = DateTime.Now.ToString("yyyy-MM-dd");
                    ViewState["HoraActual"] = DateTime.Now.ToString("HH:mm");
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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(138, int.Parse(Session["usuCodigo"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    GrdvDatos.DataSource = _dts;
                    GrdvDatos.DataBind();
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                ViewState["grdvDatos"] = GrdvDatos.DataSource;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnProcesar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigocede = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCEDE"].ToString();
                _codigocpce = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCPCE"].ToString();
                _codigoclde = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCLDE"].ToString();
                _codigopers = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPERS"].ToString();
                _identificacion = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Identificacion"].ToString();
                _operacion = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Operacion"].ToString();
                Response.Redirect("WFrm_RegLlamadaEntrante.aspx?CodigoCEDE=" + _codigocede + "&CodigoCPCE=" + _codigocpce + "&codigoCLDE=" + _codigoclde + "&CodigoPERS=" + _codigopers + "&NumeroDocumento=" + _identificacion + "&Operacion=" + _operacion + "&CodigoLTCA=0&CodigoUSU=0&Retornar=4", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}