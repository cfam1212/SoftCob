namespace SoftCob.Views.Cedente
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    public partial class WFrm_UpdateGarante : Page
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
                ViewState["CedulaTitular"] = Request["CedulaTitular"];
                ViewState["CedulaGarante"] = Request["CedulaGarante"];
                ViewState["CodigoGARA"] = Request["CodigoGARA"];
                Lbltitulo.Text = "Actualizar Datos << GARANTE/CODEUDOR >>";
                FunCargarMantenimiento();

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarMantenimiento()
        {
            _dts = new ConsultaDatosDAO().FunConsultaDatos(209, int.Parse(ViewState["CodigoGARA"].ToString()), 0, 0, "",
                ViewState["CedulaTitular"].ToString(), ViewState["CedulaGarante"].ToString(), Session["Conectar"].ToString());

            TxtNumeroDocumento.Text = _dts.Tables[0].Rows[0]["Documento"].ToString();
            TxtNombres.Text = _dts.Tables[0].Rows[0]["Nombres"].ToString();
            DdlTipo.SelectedValue = _dts.Tables[0].Rows[0]["Tipo"].ToString();
            TxtOperacion.Text = _dts.Tables[0].Rows[0]["Operacion"].ToString();
            TxtDirDomicilio.Text = _dts.Tables[0].Rows[0]["DirDom"].ToString();
            TxtRefDomicilio.Text = _dts.Tables[0].Rows[0]["RefDom"].ToString();
            TxtDirTrabajo.Text = _dts.Tables[0].Rows[0]["DirTra"].ToString();
            TxtRefTrabajo.Text = _dts.Tables[0].Rows[0]["RefTra"].ToString();
            TxtMailPersonal.Text = _dts.Tables[0].Rows[0]["MailPersonal"].ToString();
            TxtMailEmpresa.Text = _dts.Tables[0].Rows[0]["MailTrabajo"].ToString();
        }
        #endregion

        #region Botones y Eventos
        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtNumeroDocumento.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese No. Documento..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtNombres.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombres..!", this);
                    return;
                }

                if (!string.IsNullOrEmpty(TxtMailPersonal.Text.Trim()))
                {
                    if (!new FuncionesDAO().Email_bien_escrito(TxtMailPersonal.Text.Trim()))
                    {
                        new FuncionesDAO().FunShowJSMessage("Email Personal Incorrecto..!", this);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(TxtMailEmpresa.Text.Trim()))
                {
                    if (!new FuncionesDAO().Email_bien_escrito(TxtMailEmpresa.Text.Trim()))
                    {
                        new FuncionesDAO().FunShowJSMessage("Email Empresa Incorrecto..!", this);
                        return;
                    }
                }

                if (ViewState["CedulaGarante"].ToString() != TxtNumeroDocumento.Text.Trim())
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(209, 0, 0, 0, "", ViewState["CedulaTitular"].ToString(),
                        TxtNumeroDocumento.Text.Trim(), Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("No. de Documento ya Existe..!", this);
                        TxtNumeroDocumento.Text = ViewState["CedulaGarante"].ToString();
                        return;
                    }
                }

                _dts = new ConsultaDatosDAO().FunActualizarDatos(2, 0, 0, ViewState["CedulaGarante"].ToString(),
                    TxtNumeroDocumento.Text.Trim(), TxtMailPersonal.Text.Trim().ToLower(), TxtDirDomicilio.Text.Trim().ToUpper(),
                    TxtRefDomicilio.Text.Trim().ToUpper(), TxtDirTrabajo.Text.Trim().ToUpper(),
                    TxtRefTrabajo.Text.Trim().ToUpper(), TxtMailEmpresa.Text.Trim().ToLower(),
                    TxtOperacion.Text.Trim(), TxtNombres.Text.Trim().ToUpper(), "", int.Parse(ViewState["CodigoGARA"].ToString()), 0, 0,
                    Session["Conectar"].ToString());

                ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.close();", true);
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