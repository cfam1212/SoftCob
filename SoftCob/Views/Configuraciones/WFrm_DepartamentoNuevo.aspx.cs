namespace SoftCob.Views.Configuraciones
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Web.UI;
    public partial class WFrm_DepartamentoNuevo : Page
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
                    ViewState["CodigoDEPA"] = Request["Codigo"];
                    ViewState["NomDepa"] = "";
                    if (ViewState["CodigoDEPA"].ToString() == "0") Lbltitulo.Text = "Agregar Nuevo Departamento";
                    else
                    {
                        LblEstado.Visible = true;
                        ChkEstado.Visible = true;
                        Lbltitulo.Text = "Editar Departamento";
                        FunCargarMantenimiento(int.Parse(ViewState["CodigoDEPA"].ToString()));
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
        private void FunCargarMantenimiento(int _Codigo)
        {
            try
            {
                SoftCob_DEPARTAMENTO _depar = new ControllerDAO().FunGetDepartamentoPorCodigo(_Codigo,
                    int.Parse(Session["CodigoEMPR"].ToString()));
                TxtDepartamento.Text = _depar.depa_descripcion;
                ChkEstado.Text = _depar.depa_estado ? "Activo" : "Inactivo";
                ChkEstado.Checked = _depar.depa_estado;
                ChkEvalua.Checked = _depar.depa_auxi1 == 1 ? true : false;
                ChkEvalua.Text = _depar.depa_auxi1 == 1 ? "SI" : "NO";
                ViewState["NomDepa"] = TxtDepartamento.Text.ToUpper();
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
            ChkEstado.Text = ChkEstado.Checked ? "Activo" : "Inactivo";
        }

        protected void ChkEvalua_CheckedChanged(object sender, EventArgs e)
        {
            ChkEvalua.Text = ChkEvalua.Checked ? "SI" : "NO";
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtDepartamento.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese nombre del Departamento..!", this);
                    return;
                }

                if (ViewState["NomDepa"].ToString() != TxtDepartamento.Text.Trim().ToUpper())
                {
                    if (!string.IsNullOrEmpty(new ControllerDAO().FunConsultaDepartamento(TxtDepartamento.Text.Trim().ToUpper(),
                        int.Parse(Session["CodigoEMPR"].ToString()))))
                    {
                        new FuncionesDAO().FunShowJSMessage("Departamento ya está creado..!", this);
                        return;
                    }
                }

                SoftCob_DEPARTAMENTO _depar = new SoftCob_DEPARTAMENTO();
                {
                    _depar.DEPA_CODIGO = int.Parse(ViewState["CodigoDEPA"].ToString());
                    _depar.empr_codigo = int.Parse(Session["CodigoEMPR"].ToString());
                    _depar.depa_descripcion = TxtDepartamento.Text.Trim().ToUpper();
                    _depar.depa_estado = ChkEstado.Checked;
                    _depar.depa_auxi1 = ChkEvalua.Checked ? 1 : 0;
                    _depar.depa_auxi2 = 0;
                    _depar.depa_auxi3 = 0;
                    _depar.depa_auxv1 = "";
                    _depar.depa_auxv2 = "";
                    _depar.depa_auxv3 = "";
                    _depar.depa_fechacreacion = DateTime.Now;
                    _depar.depa_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                    _depar.depa_terminalcreacion = Session["MachineName"].ToString();
                    _depar.depa_fum = DateTime.Now;
                    _depar.depa_uum = int.Parse(Session["usuCodigo"].ToString());
                    _depar.depa_tum = Session["MachineName"].ToString();
                }

                if (_depar.DEPA_CODIGO == 0) new ControllerDAO().FunCrearDepartamento(_depar);
                else new ControllerDAO().FunEditarDepartamento(_depar, int.Parse(Session["CodigoEMPR"].ToString()));
                Response.Redirect("WFrm_DepartamentoAdmin.aspx?MensajeRetornado=Guardado con Éxito");
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_DepartamentoAdmin.aspx", true);
        }
        #endregion
    }
}