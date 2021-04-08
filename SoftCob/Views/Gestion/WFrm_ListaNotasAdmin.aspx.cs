﻿namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ListaNotasAdmin : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        int _codigonota = 0;
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
                    new FuncionesDAO().FunShowJSMessage("Se encuentra en Llamada, en cuanto termine la gestión podrá salir de la Lista de Trabajo..!", this);
                    Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                }
                ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                Lbltitulo.Text = "Lista de Notas <<-- NOTAS GESTION -->>";
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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(125, int.Parse(Session["usuCodigo"].ToString()), 0, 0, "", "", "", ViewState["Conectar"].ToString());

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
        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigonota = int.Parse(GrdvDatos.DataKeys[gvRow.RowIndex].Values["codigoNOTA"].ToString());
                new GestionTelefonicaDAO().FunDelNotasGestion(_codigonota);
                Response.Redirect(Request.Url.AbsolutePath, true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}