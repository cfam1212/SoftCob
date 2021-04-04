namespace SoftCob.Views.Perfil
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Web.UI;
    public partial class WFrm_PerfilNuevo : Page
    {
        #region Variables
        int _codigo = 0; 
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                Lbltitulo.Text = "Agregar Nuevo Perfil";
            }
        }
        #endregion

        #region Botones y Eventos
        protected void ChkCrear_CheckedChanged(object sender, EventArgs e)
        {
            ChkCrear.Text = ChkCrear.Checked == true ? "Si" : "No";
        }

        protected void ChkModificar_CheckedChanged(object sender, EventArgs e)
        {
            ChkModificar.Text = ChkModificar.Checked == true ? "Si" : "No";
        }

        protected void ChkEliminar_CheckedChanged(object sender, EventArgs e)
        {
            ChkEliminar.Text = ChkEliminar.Checked == true ? "Si" : "No";
        }

        protected void ChkPerfil_CheckedChanged(object sender, EventArgs e)
        {
            ChkPerfil.Text = ChkPerfil.Checked == true ? "Si" : "No";
        }

        protected void ChkEstilos_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstilos.Text = ChkEstilos.Checked == true ? "Si" : "No";
        }

        protected void ChkMetaprogramas_CheckedChanged(object sender, EventArgs e)
        {
            ChkMetaprogramas.Text = ChkMetaprogramas.Checked == true ? "Si" : "No";
        }

        protected void ChkModalidad_CheckedChanged(object sender, EventArgs e)
        {
            ChkModalidad.Text = ChkModalidad.Checked == true ? "Si" : "No";
        }

        protected void ChkEstadosdelYo_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstadosdelYo.Text = ChkEstadosdelYo.Checked == true ? "Si" : "No";
        }

        protected void ChkImpulsores_CheckedChanged(object sender, EventArgs e)
        {
            ChkImpulsores.Text = ChkImpulsores.Checked == true ? "Si" : "No";
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtPerfil.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombre del Perfil..!", this);
                    return;
                }

                if (new ControllerDAO().FunConsultaPerfil(TxtPerfil.Text.Trim().ToUpper(), int.Parse(Session["CodigoEMPR"].ToString())) > 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Nombre del Perfil ya Existe..!", this);
                    return;
                }

                SoftCob_PERFIL _pernew = new SoftCob_PERFIL();
                {
                    _pernew.PERF_CODIGO = 0;
                    _pernew.empr_codigo = int.Parse(Session["CodigoEMPR"].ToString());
                    _pernew.perf_descripcion = TxtPerfil.Text.ToUpper();
                    _pernew.perf_observacion = TxtDescripcion.Text.ToUpper();
                    _pernew.perf_estado = true;
                    _pernew.perf_crearparametro = ChkCrear.Checked;
                    _pernew.perf_modiparametro = ChkModificar.Checked;
                    _pernew.perf_eliminaparametro = ChkEliminar.Checked;
                    _pernew.perf_perfilactitudinal = ChkPerfil.Checked;
                    _pernew.perf_estilosnegociacion = ChkEstilos.Checked;
                    _pernew.perf_metaprogramas = ChkMetaprogramas.Checked;
                    _pernew.perf_modalidades = ChkModalidad.Checked;
                    _pernew.perf_estadosdelyo = ChkEstadosdelYo.Checked;
                    _pernew.perf_impulsores = ChkImpulsores.Checked;
                    _pernew.perf_auxb1 = false;
                    _pernew.perf_auxb2 = false;
                    _pernew.perf_auxb3 = false;
                    _pernew.perf_auxv1 = "";
                    _pernew.perf_auxv2 = "";
                    _pernew.perf_auxv3 = "";
                    _pernew.perf_auxi1 = 0;
                    _pernew.perf_auxi2 = 0;
                    _pernew.perf_auxi3 = 0;
                    _pernew.perf_fechacreacion = DateTime.Now;
                    _pernew.perf_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                    _pernew.perf_terminalcreacion = Session["MachineName"].ToString();
                    _pernew.perf_fum = DateTime.Now;
                    _pernew.perf_uum = int.Parse(Session["usuCodigo"].ToString());
                    _pernew.perf_tum = Session["MachineName"].ToString();
                }

                _codigo = new ControllerDAO().FunCrearPerfil(_pernew);

                if (_codigo > 0)
                {
                    Response.Redirect("WFrm_PerfilEdit.aspx?Codigo=" + _codigo, true);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_PerfilAdmin.aspx", true);
        } 
        #endregion
    }
}