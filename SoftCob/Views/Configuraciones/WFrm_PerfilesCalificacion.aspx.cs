namespace SoftCob.Views.Configuraciones
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_PerfilesCalificacion : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _tblbuscar = new DataTable();
        DataTable _dtbperfil = new DataTable();
        DataTable _tblagre = new DataTable();
        DataRow _result, _filagre;
        DataRow _rows;
        int _maxcodigo = 0, _codigoperfil = 0;
        bool _existe = false;
        ImageButton _imgeliminar = new ImageButton();
        CheckBox _chkestado = new CheckBox();
        string _mensaje = "", _redirect = "", _mensajes = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                _dtbperfil.Columns.Add("CodigoPer");
                _dtbperfil.Columns.Add("Descripcion");
                _dtbperfil.Columns.Add("Estado");
                _dtbperfil.Columns.Add("auxv1");
                _dtbperfil.Columns.Add("auxv2");
                _dtbperfil.Columns.Add("auxv3");
                _dtbperfil.Columns.Add("auxi1");
                _dtbperfil.Columns.Add("auxi2");
                _dtbperfil.Columns.Add("auxi3");
                ViewState["Perfil"] = _dtbperfil;
                Lbltitulo.Text = "Administrar Perfiles de Calificación";
                FunCargarCombos(0);

                if (Request["MensajeRetornado"] != null)
                {
                    _mensajes = Request["MensajeRetornado"];
                    ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                        "'top-center'); alertify.success('" + _mensajes + "', 5, function(){console.log('dismissed');});", true);
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(103, _codigoperfil, 0, 0, DdlPerfiles.SelectedValue, "", "", 
                    Session["Conectar"].ToString());
                ViewState["Perfil"] = _dts.Tables[0];
                GrdvDatos.DataSource = _dts;
                GrdvDatos.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void FunCargarCombos(int tipo)
        {
            try
            {
                switch (tipo)
                {
                    case 0:
                        _dts = new ControllerDAO().FunGetParametroDetalle("TIPO PERFIL", "--Seleccione Perfil--", "S");
                        DdlPerfiles.DataSource = _dts;
                        DdlPerfiles.DataTextField = "Descripcion";
                        DdlPerfiles.DataValueField = "Codigo";
                        DdlPerfiles.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void ImgAdd_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlPerfiles.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Perfil..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtDescripcion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Perfil..!", this);
                    return;
                }

                if (ViewState["Perfil"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["Perfil"];
                    _result = _tblbuscar.Select("Descripcion='" + TxtDescripcion.Text.Trim().ToUpper() + "'").FirstOrDefault();
                    _existe = _result != null ? true : false;

                    if (_tblbuscar.Rows.Count > 0)
                    {
                        _maxcodigo = _tblbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["CodigoPer"]));
                    }
                    else _maxcodigo = 0;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Descripción ya existe..!", this);
                    return;
                }

                _dtbperfil = (DataTable)ViewState["Perfil"];
                _filagre = _dtbperfil.NewRow();
                _filagre["CodigoPer"] = _maxcodigo + 1;
                _filagre["Descripcion"] = TxtDescripcion.Text.Trim().ToUpper();
                _filagre["Estado"] = "Activo";
                _filagre["auxv1"] = "";
                _filagre["auxv2"] = "";
                _filagre["auxv3"] = "";
                _filagre["auxi1"] = "0";
                _filagre["auxi2"] = "0";
                _filagre["auxi3"] = "0";
                _dtbperfil.Rows.Add(_filagre);
                _dtbperfil.DefaultView.Sort = "Descripcion";
                ViewState["Perfil"] = _dtbperfil;
                GrdvDatos.DataSource = _dtbperfil;
                GrdvDatos.DataBind();
                TxtDescripcion.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlPerfiles.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Perfil..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtDescripcion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Perfil..!", this);
                    return;
                }

                if (ViewState["Perfil"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["Perfil"];
                    _result = _tblbuscar.Select("Descripcion='" + TxtDescripcion.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_result != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Descripción ya existe..!", this);
                    return;
                }

                _tblagre = (DataTable)ViewState["Perfil"];
                _rows = _tblagre.Select("CodigoPer='" + ViewState["CodigoPerfil"].ToString() + "'").FirstOrDefault();
                _rows["Descripcion"] = TxtDescripcion.Text.Trim().ToUpper();
                _tblagre.AcceptChanges();
                _tblagre.DefaultView.Sort = "Descripcion";
                ViewState["Perfil"] = _tblagre;
                GrdvDatos.DataSource = _tblagre;
                GrdvDatos.DataBind();
                TxtDescripcion.Text = "";
                ImgAdd.Enabled = true;
                ImgEdit.Enabled = false;
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
                _chkestado = (CheckBox)(_gvrow.Cells[1].FindControl("chkEstado"));
                _dtbperfil = (DataTable)ViewState["Perfil"];
                _codigoperfil = int.Parse(GrdvDatos.DataKeys[_gvrow.RowIndex].Values["CodigoPer"].ToString());
                _result = _dtbperfil.Select("CodigoPer='" + _codigoperfil + "'").FirstOrDefault();
                _result["Estado"] = _chkestado.Checked ? "Activo" : "Inactivo";
                _dtbperfil.AcceptChanges();
                ViewState["Perfil"] = _dtbperfil;
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
                ViewState["CodigoPerfil"] = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["CodigoPer"].ToString();
                _tblbuscar = (DataTable)ViewState["Perfil"];
                _result = _tblbuscar.Select("CodigoPer='" + ViewState["CodigoPerfil"].ToString() + "'").FirstOrDefault();
                TxtDescripcion.Text = _result["Descripcion"].ToString();
                ImgAdd.Enabled = false;
                ImgEdit.Enabled = true;
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
                _dtbperfil = (DataTable)ViewState["Perfil"];

                if (_dtbperfil.Rows.Count > 0)
                {
                    _mensaje = new GestionTelefonicaDAO().FunRegistrarPerfilCalifica(0, DdlPerfiles.SelectedValue, "", "", 0, 0, 0, 
                        int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), _dtbperfil, 
                        Session["Conectar"].ToString());

                    if (_mensaje == "")
                    {
                        _redirect = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, 
                            "Perfil de Calificación Guardado con Éxito..!");
                        Response.Redirect(_redirect, true);
                    }
                    else new FuncionesDAO().FunShowJSMessage(_mensaje, this);
                }
                else new FuncionesDAO().FunShowJSMessage("No existen datos ingresados..!", this);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
        }

        protected void DdlPerfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            GrdvDatos.DataSource = null;
            GrdvDatos.DataBind();

            if (DdlPerfiles.SelectedValue != "0") FunCargarMantenimiento();
        }

        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkestado = (CheckBox)(e.Row.Cells[1].FindControl("chkEstado"));
                    _imgeliminar = (ImageButton)(e.Row.Cells[3].FindControl("imgDel"));
                    _codigoperfil = int.Parse(GrdvDatos.DataKeys[e.Row.RowIndex].Values["CodigoPer"].ToString());

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(104, _codigoperfil, 0, 0, DdlPerfiles.SelectedValue, "", "",
                        Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        _chkestado.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                        _imgeliminar.ImageUrl = "~/Botones/eliminargris.png";
                        _imgeliminar.Enabled = false;
                    }
                    else
                    {
                        _chkestado.Checked = true;
                        _chkestado.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _dtbperfil = (DataTable)ViewState["Perfil"];
                _dtbperfil.Rows.RemoveAt(_gvrow.RowIndex);
                ViewState["Perfil"] = _dtbperfil;
                GrdvDatos.DataSource = _dtbperfil;
                GrdvDatos.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}