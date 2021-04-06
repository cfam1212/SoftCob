namespace SoftCob.Views.ConsultasManager
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Data;
    using System.Drawing;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_AccionConsulta : Page
    {
        #region Variables
        string _telefono = "", _identificacion = "", _operacion = "", _descripcion = "", _estado = "";
        int _codigogestor = 0, _codigogete = 0, _codigofono = 0;
        DataSet _dts = new DataSet();
        CheckBox _chkestado = new CheckBox();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                try
                {
                    pnlDatosDeudor.Height = 100;
                    pnlDatosObligacion.Height = 150;
                    Lbltitulo.Text = "Acciones Gestor - Gestión";
                    ViewState["codigoCEDE"] = Request["codigoCEDE"];
                    ViewState["codigoCPCE"] = Request["codigoCPCE"];
                    ViewState["codigoCLDE"] = Request["codigoCLDE"];
                    ViewState["codigoPERS"] = Request["codigoPERS"];
                    FunCargarCombos();
                    FunCargarMatenimiento();
                }
                catch (Exception ex)
                {
                    Lblerror.Text = ex.ToString();
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarCombos()
        {
            try
            {
                _dts = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--", 
                    int.Parse(ViewState["codigoCEDE"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());

                DdlAsignar.DataSource = _dts;
                DdlAsignar.DataTextField = "Descripcion";
                DdlAsignar.DataValueField = "Codigo";
                DdlAsignar.DataBind();

                _dts = new ControllerDAO().FunGetParametroDetalle("CAMBIAR OPERACION", "--Seleccione Motivo--", "S");

                DdlMotivo1.DataSource = _dts;
                DdlMotivo1.DataTextField = "Descripcion";
                DdlMotivo1.DataValueField = "Codigo";
                DdlMotivo1.DataBind();

                DdlMotivo2.DataSource = _dts;
                DdlMotivo2.DataTextField = "Descripcion";
                DdlMotivo2.DataValueField = "Codigo";
                DdlMotivo2.DataBind();

                DdlMotivo3.DataSource = _dts;
                DdlMotivo3.DataTextField = "Descripcion";
                DdlMotivo3.DataValueField = "Codigo";
                DdlMotivo3.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void FunCargarMatenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(32, int.Parse(ViewState["codigoPERS"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString().ToString());

                ViewState["Identificacion"] = _dts.Tables[0].Rows[0]["Cedula"].ToString();
                GrdvDatosDeudor.DataSource = _dts;
                GrdvDatosDeudor.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(58, int.Parse(ViewState["codigoCPCE"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString().ToString());

                ViewState["Catalogo"] = _dts.Tables[0].Rows[0]["Descripcion"].ToString();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(33, int.Parse(ViewState["codigoCEDE"].ToString()), int.Parse(ViewState["codigoCPCE"].ToString()), int.Parse(ViewState["codigoCLDE"].ToString()), ViewState["Catalogo"].ToString(), "", "", Session["Conectar"].ToString().ToString());

                GrdvDatosObligacion.DataSource = _dts;
                GrdvDatosObligacion.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(107, int.Parse(ViewState["codigoCEDE"].ToString()), int.Parse(ViewState["codigoPERS"].ToString()), int.Parse(ViewState["codigoCLDE"].ToString()), "", "", "", Session["Conectar"].ToString());

                GrdvTelefonos.DataSource = _dts;
                GrdvTelefonos.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(96, int.Parse(ViewState["codigoCLDE"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());
                GrdvGestiones.DataSource = _dts;
                GrdvGestiones.DataBind();

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }

        }
        #endregion

        #region Botones y Eventos
        protected void BtnCambiar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["Operacion"] == null)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Operación a Cambiar..!", this);
                    return;
                }

                if (DdlAsignar.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Gestor nuevo Asignado..!", this);
                    return;
                }

                if (DdlMotivo1.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Motivo..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtObservacion1.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Observación..!", this);
                    return;
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(97, int.Parse(ViewState["codigoCLDE"].ToString()), 
                    int.Parse(DdlAsignar.SelectedValue), 0, "", ViewState["Operacion"].ToString(), "", Session["Conectar"].ToString());

                SoftCob_ACCIONGESTION _newaccion = new SoftCob_ACCIONGESTION();
                {
                    _newaccion.acci_tipoaccion = "CAMBIAR OPERACION";
                    _newaccion.acci_idmotivo = int.Parse(DdlMotivo1.SelectedValue);
                    _newaccion.acci_observacion = TxtObservacion1.Text.Trim().ToUpper();
                    _newaccion.acci_codigocpce = int.Parse(ViewState["codigoCPCE"].ToString());
                    _newaccion.acci_identificacion = ViewState["Identificacion"].ToString();
                    _newaccion.acci_operacion = ViewState["Operacion"].ToString();
                    _newaccion.acci_gestoranterior = int.Parse(ViewState["GestorAsignado"].ToString());
                    _newaccion.acci_gestoractual = int.Parse(DdlAsignar.SelectedValue);
                    _newaccion.acci_datoanterior = "";
                    _newaccion.acci_auxv1 = "";
                    _newaccion.acci_auxv2 = "";
                    _newaccion.acci_auxv3 = "";
                    _newaccion.acci_auxi1 = int.Parse(ViewState["codigoPERS"].ToString());
                    _newaccion.acci_auxi2 = 0;
                    _newaccion.acci_auxi3 = 0;
                    _newaccion.acci_fechacreacion = DateTime.Now;
                    _newaccion.acci_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                    _newaccion.acci_terminalcreacion = Session["MachineName"].ToString();
                }

                new CedenteDAO().FunInsertarAccionGestion(_newaccion);
                new FuncionesDAO().FunShowJSMessage("Operacion Cambiada con éxito..!", this);

                _dts = new ConsultaDatosDAO().FunConsultaDatos(33, 0, int.Parse(ViewState["codigoCPCE"].ToString()),
                    int.Parse(ViewState["codigoCLDE"].ToString()), ViewState["Catalogo"].ToString(), "", "", Session["Conectar"].ToString().ToString());
                GrdvDatosObligacion.DataSource = _dts;
                GrdvDatosObligacion.DataBind();
                DdlAsignar.SelectedValue = "0";
                DdlMotivo1.SelectedValue = "0";
                TxtObservacion1.Text = "";
                ViewState["Operacion"] = null;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSelecc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                foreach (GridViewRow _fr in GrdvDatosObligacion.Rows)
                {
                    _fr.Cells[0].BackColor = Color.White;
                    _fr.Cells[1].BackColor = Color.White;
                }
                GrdvDatosObligacion.Rows[_gvrow.RowIndex].Cells[0].BackColor = Color.Coral;
                GrdvDatosObligacion.Rows[_gvrow.RowIndex].Cells[1].BackColor = Color.Coral;
                ViewState["Operacion"] = GrdvDatosObligacion.DataKeys[_gvrow.RowIndex].Values["Operacion"].ToString();
                ViewState["GestorAsignado"] = GrdvDatosObligacion.DataKeys[_gvrow.RowIndex].Values["GestorAsignado"].ToString();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliminaTele_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlMotivo2.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Motivo..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtObservacion2.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Observación..!", this);
                    return;
                }

                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _telefono = GrdvTelefonos.DataKeys[_gvrow.RowIndex].Values["Telefono"].ToString();

                new ConsultaDatosDAO().FunConsultaDatos(88, int.Parse(ViewState["codigoCEDE"].ToString()), int.Parse(ViewState["codigoPERS"].ToString()), int.Parse(ViewState["codigoCLDE"].ToString()), "", _telefono, "", Session["Conectar"].ToString());

                SoftCob_ACCIONGESTION _newaccion = new SoftCob_ACCIONGESTION();
                {
                    _newaccion.acci_tipoaccion = "ELIMINAR TELEFONO";
                    _newaccion.acci_idmotivo = int.Parse(DdlMotivo2.SelectedValue);
                    _newaccion.acci_observacion = TxtObservacion2.Text.Trim().ToUpper();
                    _newaccion.acci_codigocpce = int.Parse(ViewState["codigoCPCE"].ToString());
                    _newaccion.acci_identificacion = ViewState["Identificacion"].ToString();
                    _newaccion.acci_operacion = "";
                    _newaccion.acci_gestoranterior = 0;
                    _newaccion.acci_gestoractual = 0;
                    _newaccion.acci_datoanterior = _telefono;
                    _newaccion.acci_auxv1 = "";
                    _newaccion.acci_auxv2 = "";
                    _newaccion.acci_auxv3 = "";
                    _newaccion.acci_auxi1 = int.Parse(ViewState["codigoPERS"].ToString());
                    _newaccion.acci_auxi2 = 0;
                    _newaccion.acci_auxi3 = 0;
                    _newaccion.acci_fechacreacion = DateTime.Now;
                    _newaccion.acci_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                    _newaccion.acci_terminalcreacion = Session["MachineName"].ToString();
                }
                new CedenteDAO().FunInsertarAccionGestion(_newaccion);
                new FuncionesDAO().FunShowJSMessage("Teléfono eliminado con éxito..!", this);

                _dts = new ConsultaDatosDAO().FunConsultaDatos(35, int.Parse(ViewState["codigoCEDE"].ToString()), int.Parse(ViewState["codigoPERS"].ToString()), int.Parse(ViewState["codigoCLDE"].ToString()), "", "", "", Session["Conectar"].ToString());

                GrdvTelefonos.DataSource = _dts;
                GrdvTelefonos.DataBind();
                DdlMotivo2.SelectedValue = "0";
                TxtObservacion2.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliminaGes_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlMotivo3.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Motivo..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtObservacion3.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Observación..!", this);
                    return;
                }

                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigogete = int.Parse(GrdvGestiones.DataKeys[_gvrow.RowIndex].Values["codigoGETE"].ToString());
                _identificacion = GrdvGestiones.DataKeys[_gvrow.RowIndex].Values["Identificacion"].ToString();
                _operacion = GrdvGestiones.DataKeys[_gvrow.RowIndex].Values["Operacion"].ToString();
                _codigogestor = int.Parse(GrdvGestiones.DataKeys[_gvrow.RowIndex].Values["codigoGESTOR"].ToString());
                _descripcion = GrdvGestiones.DataKeys[_gvrow.RowIndex].Values["Descripcion"].ToString();
                new ConsultaDatosDAO().FunConsultaDatos(98, _codigogete, 0, 0, "", _telefono, "", Session["Conectar"].ToString());

                SoftCob_ACCIONGESTION _newaccion = new SoftCob_ACCIONGESTION();
                {
                    _newaccion.acci_tipoaccion = "ELIMINAR GESTION";
                    _newaccion.acci_idmotivo = int.Parse(DdlMotivo3.SelectedValue);
                    _newaccion.acci_observacion = TxtObservacion3.Text.Trim().ToUpper();
                    _newaccion.acci_codigocpce = int.Parse(ViewState["codigoCPCE"].ToString());
                    _newaccion.acci_identificacion = _identificacion;
                    _newaccion.acci_operacion = _operacion;
                    _newaccion.acci_gestoranterior = _codigogestor;
                    _newaccion.acci_gestoractual = 0;
                    _newaccion.acci_datoanterior = _descripcion;
                    _newaccion.acci_auxv1 = "";
                    _newaccion.acci_auxv2 = "";
                    _newaccion.acci_auxv3 = "";
                    _newaccion.acci_auxi1 = int.Parse(ViewState["codigoPERS"].ToString());
                    _newaccion.acci_auxi2 = 0;
                    _newaccion.acci_auxi3 = 0;
                    _newaccion.acci_fechacreacion = DateTime.Now;
                    _newaccion.acci_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                    _newaccion.acci_terminalcreacion = Session["MachineName"].ToString();
                }
                new CedenteDAO().FunInsertarAccionGestion(_newaccion);
                new FuncionesDAO().FunShowJSMessage("Gestión eliminado con éxito..!", this);

                _dts = new ConsultaDatosDAO().FunConsultaDatos(96, int.Parse(ViewState["codigoCLDE"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());

                GrdvGestiones.DataSource = _dts;
                GrdvGestiones.DataBind();
                DdlMotivo3.SelectedValue = "0";
                TxtObservacion3.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigofono = int.Parse(GrdvTelefonos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _chkestado = (CheckBox)(_gvrow.Cells[1].FindControl("chkEstado"));
                SoftCob_TELEFONOS_CEDENTE telecedente = new SoftCob_TELEFONOS_CEDENTE();
                {
                    telecedente.TECE_CODIGO = _codigofono;
                    telecedente.tece_estado = _chkestado.Checked;
                }
                new GestionTelefonicaDAO().FunCambiarEstadoTelefono(telecedente);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvTelefonos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkestado = (CheckBox)(e.Row.Cells[4].FindControl("chkEstado"));
                    _estado = GrdvTelefonos.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();

                    if (_estado == "Activo") _chkestado.Checked = true;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_ConsultasAdmin.aspx", true);
        }
        #endregion
    }
}