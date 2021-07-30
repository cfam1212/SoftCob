namespace SoftCob.Views.Tarea
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Web.UI;
    public partial class WFrm_TareaNueva : Page
    {
        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                    Response.Redirect("~/Reload.html");
                if (!IsPostBack)
                {
                    ViewState["CodigoTARE"] = Request["Codigo"];
                    ViewState["NomTarea"] = "";
                    if (ViewState["CodigoTARE"].ToString() == "0") Lbltitulo.Text = "Agregar Nueva Tarea";
                    else
                    {
                        LblEstado.Visible = true;
                        ChkEstado.Visible = true;
                        Lbltitulo.Text = "Editar Tarea";
                        FunCargaMantenimiento();
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
        private void FunCargaMantenimiento()
        {
            try
            {
                SoftCob_TAREA _tarea = new SoftCob_TAREA();
                _tarea = new ControllerDAO().FunGetTareaPorCodigo(int.Parse(ViewState["CodigoTARE"].ToString()));
                TxtTarea.Text = _tarea.tare_descripcion;
                TxtRuta.Text = _tarea.tare_programa;
                ChkEstado.Checked = _tarea.tare_estado;
                ChkEstado.Text = _tarea.tare_estado ? "Activo" : "Inactivo";
                ViewState["NomTarea"] = TxtTarea.Text.Trim();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstado.Text = ChkEstado.Checked == true ? "Activo" : "Inactivo";
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtTarea.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombre de la Tarea..!", this, "N", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtRuta.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Ruta/Página de la Tarea..!", this, "N", "C");
                    return;
                }

                if (ViewState["NomTarea"].ToString() != TxtTarea.Text.Trim())
                {
                    if (!string.IsNullOrEmpty(new ControllerDAO().FunConsultaTarea(TxtTarea.Text.Trim(),
                        int.Parse(Session["CodigoEMPR"].ToString()))))
                    {
                        new FuncionesDAO().FunShowJSMessage("Tarea ya existe..!", this, "W", "C");
                        return;
                    }
                }

                if (!new FuncionesDAO().Ruta_bien_escrita(TxtRuta.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Página mal escrita (eje: Views/Contenedor/nombre.aspx)..!", this, "W", "C");
                    return;
                }

                SoftCob_TAREA _tarea = new SoftCob_TAREA();
                {
                    _tarea.TARE_CODIGO = int.Parse(ViewState["CodigoTARE"].ToString());
                    _tarea.empr_codigo = int.Parse(Session["CodigoEMPR"].ToString());
                    _tarea.tare_descripcion = TxtTarea.Text.Trim();
                    _tarea.tare_programa = TxtRuta.Text.Trim();
                    _tarea.tare_estado = ChkEstado.Checked;
                    _tarea.tare_auxv1 = "";
                    _tarea.tare_auxv2 = "";
                    _tarea.tare_auxv3 = "";
                    _tarea.tare_auxi1 = 0;
                    _tarea.tare_auxi2 = 0;
                    _tarea.tare_auxi3 = 0;
                    _tarea.tare_fechacreacion = DateTime.Now;
                    _tarea.tare_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                    _tarea.tare_terminalcreacion = Session["MachineName"].ToString();
                    _tarea.tare_fum = DateTime.Now;
                    _tarea.tare_uum = int.Parse(Session["usuCodigo"].ToString());
                    _tarea.tare_tum = Session["MachineName"].ToString();
                    if (_tarea.TARE_CODIGO == 0) new ControllerDAO().FunCrearTarea(_tarea);
                    else new ControllerDAO().FunUpdateTarea(_tarea);
                }
                Response.Redirect("WFrm_TareaAdmin.aspx?MensajeRetornado=Guardado con Éxito", false);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_TareaAdmin.aspx", true);
        }
        #endregion
    }
}