namespace SoftCob.Views.Perfil
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_PerfilEdit : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        CheckBox _chkagre = new CheckBox();
        string _estado = "", _mensaje = "";
        int _codigometa = 0, _codigotare = 0, _codigoperfil = 0;
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
                    ViewState["CodigoPERF"] = Request["Codigo"];
                    if (ViewState["CodigoPERF"].ToString() == "0") Lbltitulo.Text = "Agregar Nuevo Perfil";
                    else
                    {
                        FunCargarCabecera(int.Parse(ViewState["CodigoPERF"].ToString()));
                        Lblestado.Visible = true;
                        ChkEstado.Visible = true;
                        Lbltitulo.Text = "Editar Perfil";
                    }
                    FunCargaMantenimiento();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        } 
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarCabecera(int _codigo)
        {
            SoftCob_PERFIL _perfilactual = new ControllerDAO().FunGetPerfilPorID(_codigo, int.Parse(Session["CodigoEMPR"].ToString()));
            TxtPerfil.Text = _perfilactual.perf_descripcion;
            TxtDescripcion.Text = _perfilactual.perf_observacion;
            ViewState["Perfil"] = TxtPerfil.Text;
            ChkCrear.Checked = (bool)_perfilactual.perf_crearparametro;
            ChkCrear.Text = (bool)_perfilactual.perf_crearparametro ? "Si" : "No";
            ChkModificar.Checked = (bool)_perfilactual.perf_modiparametro;
            ChkModificar.Text = (bool)_perfilactual.perf_modiparametro ? "Si" : "No";
            ChkEliminar.Checked = (bool)_perfilactual.perf_eliminaparametro;
            ChkEliminar.Text = (bool)_perfilactual.perf_eliminaparametro ? "Si" : "No";
            ChkPerfil.Checked = (bool)_perfilactual.perf_perfilactitudinal;
            ChkPerfil.Text = (bool)_perfilactual.perf_perfilactitudinal ? "Si" : "No";
            ChkEstilos.Checked = (bool)_perfilactual.perf_estilosnegociacion;
            ChkEstilos.Text = (bool)_perfilactual.perf_estilosnegociacion ? "Si" : "No";
            ChkMetaprogramas.Checked = (bool)_perfilactual.perf_metaprogramas;
            ChkMetaprogramas.Text = (bool)_perfilactual.perf_metaprogramas ? "Si" : "No";
            ChkModalidad.Checked = (bool)_perfilactual.perf_modalidades;
            ChkModalidad.Text = (bool)_perfilactual.perf_modalidades ? "Si" : "No";
            ChkEstadosdelYo.Checked = (bool)_perfilactual.perf_estadosdelyo;
            ChkEstadosdelYo.Text = (bool)_perfilactual.perf_estadosdelyo ? "Si" : "No";
            ChkImpulsores.Text = (bool)_perfilactual.perf_impulsores ? "Si" : "No";
            ChkImpulsores.Checked = (bool)_perfilactual.perf_impulsores;
            ChkEstado.Checked = (bool)_perfilactual.perf_estado;
            ChkEstado.Text = (bool)_perfilactual.perf_estado ? "Activo" : "Inactivo";
        }

        private void FunCargaMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatosNew(13, int.Parse(Session["CodigoEMPR"].ToString()),
                    "", "", "", "", "", "", int.Parse(ViewState["CodigoPERF"].ToString()), 0, 0, 0, 0, 0, Session["Conectar"].ToString());

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
        protected void ChkCrear_CheckedChanged(object sender, EventArgs e)
        {
            ChkCrear.Text = ChkCrear.Checked == true ? "Si" : "No";
            _codigoperfil = int.Parse(ViewState["CodigoPERF"].ToString());

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                 SoftCob_PERFIL _original = _db.SoftCob_PERFIL.Where(x => x.PERF_CODIGO == _codigoperfil).FirstOrDefault();
                _db.SoftCob_PERFIL.Attach(_original);
                _original.perf_crearparametro = ChkCrear.Checked;
                _db.SaveChanges();
            }
            FunCargaMantenimiento();
        }

        protected void ChkModificar_CheckedChanged(object sender, EventArgs e)
        {
            ChkModificar.Text = ChkModificar.Checked == true ? "Si" : "No";
            _codigoperfil = int.Parse(ViewState["CodigoPERF"].ToString());

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                SoftCob_PERFIL _original = _db.SoftCob_PERFIL.Where(x => x.PERF_CODIGO == _codigoperfil).FirstOrDefault();
                _db.SoftCob_PERFIL.Attach(_original);
                _original.perf_modiparametro = ChkModificar.Checked;
                _db.SaveChanges();
            }
            FunCargaMantenimiento();
        }

        protected void ChkEliminar_CheckedChanged(object sender, EventArgs e)
        {
            ChkEliminar.Text = ChkEliminar.Checked == true ? "Si" : "No";
            _codigoperfil = int.Parse(ViewState["CodigoPERF"].ToString());

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                SoftCob_PERFIL _original = _db.SoftCob_PERFIL.Where(x => x.PERF_CODIGO == _codigoperfil).FirstOrDefault();
                _db.SoftCob_PERFIL.Attach(_original);
                _original.perf_eliminaparametro = ChkEliminar.Checked;
                _db.SaveChanges();
            }
            FunCargaMantenimiento();
        }

        protected void ChkPerfil_CheckedChanged(object sender, EventArgs e)
        {
            ChkPerfil.Text = ChkPerfil.Checked == true ? "Si" : "No";
            _codigoperfil = int.Parse(ViewState["CodigoPERF"].ToString());

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                SoftCob_PERFIL _original = _db.SoftCob_PERFIL.Where(x => x.PERF_CODIGO == _codigoperfil).FirstOrDefault();
                _db.SoftCob_PERFIL.Attach(_original);
                _original.perf_perfilactitudinal = ChkPerfil.Checked;
                _db.SaveChanges();
            }
            FunCargaMantenimiento();
        }

        protected void ChkEstilos_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstilos.Text = ChkEstilos.Checked == true ? "Si" : "No";
            _codigoperfil = int.Parse(ViewState["CodigoPERF"].ToString());

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                SoftCob_PERFIL _original = _db.SoftCob_PERFIL.Where(x => x.PERF_CODIGO == _codigoperfil).FirstOrDefault();
                _db.SoftCob_PERFIL.Attach(_original);
                _original.perf_estilosnegociacion = ChkEstilos.Checked;
                _db.SaveChanges();
            }
            FunCargaMantenimiento();
        }

        protected void ChkMetaprogramas_CheckedChanged(object sender, EventArgs e)
        {
            ChkMetaprogramas.Text = ChkMetaprogramas.Checked == true ? "Si" : "No";
            _codigoperfil = int.Parse(ViewState["CodigoPERF"].ToString());

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                SoftCob_PERFIL _original = _db.SoftCob_PERFIL.Where(x => x.PERF_CODIGO == _codigoperfil).FirstOrDefault();
                _db.SoftCob_PERFIL.Attach(_original);
                _original.perf_metaprogramas = ChkMetaprogramas.Checked;
                _db.SaveChanges();
            }
            FunCargaMantenimiento();
        }

        protected void ChkModalidad_CheckedChanged(object sender, EventArgs e)
        {
            ChkModalidad.Text = ChkModalidad.Checked == true ? "Si" : "No";
            _codigoperfil = int.Parse(ViewState["CodigoPERF"].ToString());

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                SoftCob_PERFIL _original = _db.SoftCob_PERFIL.Where(x => x.PERF_CODIGO == _codigoperfil).FirstOrDefault();
                _db.SoftCob_PERFIL.Attach(_original);
                _original.perf_modalidades = ChkModalidad.Checked;
                _db.SaveChanges();
            }
            FunCargaMantenimiento();
        }

        protected void ChkEstadosdelYo_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstadosdelYo.Text = ChkEstadosdelYo.Checked == true ? "Si" : "No";
            _codigoperfil = int.Parse(ViewState["CodigoPERF"].ToString());

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                SoftCob_PERFIL _original = _db.SoftCob_PERFIL.Where(x => x.PERF_CODIGO == _codigoperfil).FirstOrDefault();
                _db.SoftCob_PERFIL.Attach(_original);
                _original.perf_estadosdelyo = ChkEstadosdelYo.Checked;
                _db.SaveChanges();
            }
            FunCargaMantenimiento();
        }

        protected void ChkImpulsores_CheckedChanged(object sender, EventArgs e)
        {
            ChkImpulsores.Text = ChkImpulsores.Checked == true ? "Si" : "No";
            _codigoperfil = int.Parse(ViewState["CodigoPERF"].ToString());

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                SoftCob_PERFIL _original = _db.SoftCob_PERFIL.Where(x => x.PERF_CODIGO == _codigoperfil).FirstOrDefault();
                _db.SoftCob_PERFIL.Attach(_original);
                _original.perf_impulsores = ChkImpulsores.Checked;
                _db.SaveChanges();
            }
            FunCargaMantenimiento();
        }

        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstado.Text = ChkEstado.Checked == true ? "Activo" : "Inactivo";
            _codigoperfil = int.Parse(ViewState["CodigoPERF"].ToString());

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                SoftCob_PERFIL _original = _db.SoftCob_PERFIL.Where(x => x.PERF_CODIGO == _codigoperfil).FirstOrDefault();
                _db.SoftCob_PERFIL.Attach(_original);
                _original.perf_estado = ChkEstado.Checked;
                _db.SaveChanges();
            }
            FunCargaMantenimiento();
        }
        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkagre = (CheckBox)(e.Row.Cells[3].FindControl("ChkAgregar"));
                    _estado = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();
                    _codigotare = int.Parse(GrdvDatos.DataKeys[e.Row.RowIndex].Values["CodigoTARE"].ToString());
                    if (_estado == "Activo") _chkagre.Checked = true;

                    if (Session["usuPerfil"].ToString() != "1")
                    {
                        if (_codigotare == 10001 || _codigotare == 10002 || _codigotare == 10003 || _codigotare == 10004)
                        {
                            _chkagre.Enabled = false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ChkAgregar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkagre = (CheckBox)(_gvrow.Cells[3].FindControl("ChkAgregar"));
                _codigometa = int.Parse(GrdvDatos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());

                _dts = new ConsultaDatosDAO().FunConsultaDatosNew(14, int.Parse(Session["CodigoEMPR"].ToString()),
                    "", _chkagre.Checked ? "SI" : "NO", "", "", "", "", int.Parse(ViewState["CodigoPERF"].ToString()), _codigometa, 0,
                    0, 0, 0, Session["Conectar"].ToString());

                FunCargaMantenimiento();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtPerfil.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombre del Perfil..!", this, "W", "C");
                    return;
                }

                if (ViewState["Perfil"].ToString() != TxtPerfil.Text.Trim().ToUpper())
                {
                    if (new ControllerDAO().FunConsultaPerfil(TxtPerfil.Text.Trim().ToUpper(), int.Parse(Session["CodigoEMPR"].ToString())) > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Nombre del Perfil ya Existe..!", this, "E", "C");
                        return;
                    }
                }

                SoftCob_PERFIL _pernew = new SoftCob_PERFIL();
                {
                    _pernew.PERF_CODIGO = int.Parse(ViewState["CodigoPERF"].ToString());
                    _pernew.perf_descripcion = TxtPerfil.Text.ToUpper();
                    _pernew.perf_observacion = TxtDescripcion.Text.ToUpper();
                    //_pernew.perf_estado = ChkEstado.Checked;
                    //_pernew.perf_crearparametro = ChkCrear.Checked;
                    //_pernew.perf_modiparametro = ChkModificar.Checked;
                    //_pernew.perf_eliminaparametro = ChkEliminar.Checked;
                    //_pernew.perf_perfilactitudinal = ChkPerfil.Checked;
                    //_pernew.perf_estilosnegociacion = ChkEstilos.Checked;
                    //_pernew.perf_metaprogramas = ChkMetaprogramas.Checked;
                    //_pernew.perf_modalidades = ChkModalidad.Checked;
                    //_pernew.perf_estadosdelyo = ChkEstadosdelYo.Checked;
                    //_pernew.perf_impulsores = ChkImpulsores.Checked;
                    _pernew.perf_fum = DateTime.Now;
                    _pernew.perf_uum = int.Parse(Session["usuCodigo"].ToString());
                    _pernew.perf_tum = Session["MachineName"].ToString();
                }

                _mensaje = new ControllerDAO().FunUpdatePerfil(_pernew);
                if (_mensaje == "") Response.Redirect("WFrm_PerfilAdmin.aspx?MensajeRetornado=Guardado con Éxito", true);
                else Lblerror.Text = _mensaje;
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