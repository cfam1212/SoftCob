namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ArbolGenealogicoRecursivo : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        string _cedula = "", _redirect = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                    LblTitulo.Text = "CONSULTA RECURSIVA << ARBOL GENEALOGICO >>";
                    ViewState["Cedula"] = Request["Cedula"];
                    LblCedula.InnerText = ViewState["Cedula"].ToString();
                    FunCargarDatos(ViewState["Cedula"].ToString());
                }
            }
            catch (Exception ex)
            {
                LblError.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarDatos(string numerodocumento)
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(184, 0, 0, 0, "", numerodocumento.Substring(0, 4),
                    ViewState["Cedula"].ToString(), ViewState["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    LblNombres.Text = _dts.Tables[0].Rows[0]["Nombres"].ToString();
                    LblFecNaci.Text = _dts.Tables[0].Rows[0]["FechaNac"].ToString();
                    LblEdad.Text = _dts.Tables[0].Rows[0]["Edad"].ToString();
                    LblEsCivil.Text = _dts.Tables[0].Rows[0]["EstCivil"].ToString();
                    LblTipoCedula.Text = _dts.Tables[0].Rows[0]["TipoCedula"].ToString();
                    LblNivelEstudios.Text = _dts.Tables[0].Rows[0]["Nivel"].ToString();
                    LblProfesion.Text = _dts.Tables[0].Rows[0]["Profesion"].ToString();
                    LblConyuge.Text = _dts.Tables[0].Rows[0]["Conyuge"].ToString();
                    LblCedConyuge.Text = _dts.Tables[0].Rows[0]["Cedulacy"].ToString();
                    LblPadre.Text = _dts.Tables[0].Rows[0]["Padre"].ToString();
                    LblCedPadre.Text = _dts.Tables[0].Rows[0]["Cedulapa"].ToString();
                    LblMadre.Text = _dts.Tables[0].Rows[0]["Madre"].ToString();
                    LblCedMadre.Text = _dts.Tables[0].Rows[0]["Cedulama"].ToString();

                    if (_dts.Tables[1].Rows.Count > 0)
                    {
                        DivTitulo.Visible = true;
                        DivDatos.Visible = true;
                        GrdvDatosIESS.DataSource = _dts.Tables[1];
                        GrdvDatosIESS.DataBind();
                    }
                    else DivDatos.Visible = false;

                    if (_dts.Tables[2].Rows.Count > 0)
                    {
                        DivArbol.Visible = true;
                        GrdvDatos.DataSource = _dts.Tables[2];
                        GrdvDatos.DataBind();
                    }
                    else DivArbol.Visible = false;
                }
            }
            catch (Exception ex)
            {
                LblError.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void ImgSelecc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _cedula = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Cedula"].ToString();
                _redirect = string.Format("{0}?Cedula={1}", Request.Url.AbsolutePath, _cedula);
                Response.Redirect(_redirect);
            }
            catch (Exception ex)
            {
                LblError.Text = ex.ToString();
            }
        }

        protected void BtnRegresar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _redirect = string.Format("{0}?Cedula={1}", Request.Url.AbsolutePath, Session["CedulaCookie"].ToString());
                Response.Redirect(_redirect);
            }
            catch (Exception ex)
            {
                LblError.Text = ex.ToString();
                throw;
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.close();", true);
        }
        #endregion
    }
}