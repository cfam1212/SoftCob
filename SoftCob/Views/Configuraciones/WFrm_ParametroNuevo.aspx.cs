namespace SoftCob.Views.Configuraciones
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ParametroNuevo : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtbparametro = new DataTable();
        ImageButton _imgsubir = new ImageButton();
        ImageButton _imgbajar = new ImageButton();
        ImageButton _imgeliminar = new ImageButton();
        int _orden = 0, _maxcodigo = 0, _codigo = 0, _codigoant = 0, _ordenant = 0, _contar = 0;
        string _mensaje = "", _nuevo = "";
        DataRow[] _result;
        DataRow _resultado, _filagre;
        bool _lexiste = false;
        DataTable _dtbagre;
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
                    ViewState["CodigoParametro"] = Request["Codigo"];
                    FunCargarMantenimiento();
                    if (ViewState["CodigoParametro"].ToString() == "0") Lbltitulo.Text = "Agregar Nuevo Parámetro";
                    else
                    {
                        LblEstadoPar.Visible = true;
                        ChkEstadoPar.Visible = true;
                        Lbltitulo.Text = "Editar Parámetro";
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
                _dts = new ConsultaDatosDAO().FunConsultaDatosNew(16, 0, "", "", "", "", "", "", int.Parse(ViewState["CodigoParametro"].ToString()), 0, 0, 0, 0, 0, Session["Conectar"].ToString());
                ViewState["DatosParametros"] = _dts.Tables[1];

                GrdvDatos.DataSource = _dts.Tables[1];
                GrdvDatos.DataBind();

                new FuncionesDAO().SetearGrid(GrdvDatos, _imgsubir, 5, _imgbajar, 6, _dts.Tables[1]);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    TxtParametro.Text = _dts.Tables[0].Rows[0]["Paranametro"].ToString();
                    TxtDescripcion.Text = _dts.Tables[0].Rows[0]["Descripcion"].ToString();
                    LblEstadoPar.Visible = true;
                    ChkEstadoPar.Text = _dts.Tables[0].Rows[0]["Estado"].ToString();
                    ChkEstadoPar.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                    ViewState["NombreParametro"] = _dts.Tables[0].Rows[0]["Paranametro"].ToString();
                    ViewState["DatosParametros"] = _dts.Tables[1];
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void Funclearobject(int opcion)
        {
            try
            {
                switch (opcion)
                {
                    case 0:
                        TxtDetalle.Text = "";
                        TxtValorV.Text = "";
                        TxtValorI.Text = "";
                        ImgModificar.Enabled = false;
                        ImgAgregar.Enabled = true;
                        ImgCancelar.Enabled = false;
                        LblEstadoDet.Visible = false;
                        ChkEstadoDet.Visible = false;
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
        protected void ChkEstadoPar_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstadoPar.Text = ChkEstadoPar.Checked ? "Activo" : "Pasivo";
        }

        protected void ChkEstadoDet_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstadoDet.Text = ChkEstadoDet.Checked ? "Activo" : "Inactivo";
        }

        protected void ImgAgregar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtDetalle.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Detalle del Parámetro..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtValorV.Text.Trim()) && string.IsNullOrEmpty(TxtValorI.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Valor del Parámetro..!", this, "W", "C");
                    return;
                }

                if (!string.IsNullOrEmpty(TxtValorV.Text.Trim()) && !string.IsNullOrEmpty(TxtValorI.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Defina solo un tipo de valor..!", this, "E", "C");
                    return;
                }

                if (ViewState["DatosParametros"] != null)
                {
                    _dtbparametro = (DataTable)ViewState["DatosParametros"];

                    if (_dtbparametro.Rows.Count > 0)
                    {
                        _maxcodigo = _dtbparametro.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                        _orden = _dtbparametro.AsEnumerable()
                            .Max(row => int.Parse((string)row["Orden"]));
                    }

                    if (!string.IsNullOrEmpty(TxtValorV.Text.Trim()))
                    {
                        _resultado = _dtbparametro.Select("Detalle='" + TxtDetalle.Text.ToUpper() + "' AND ValorV='" +
                            TxtValorV.Text.Trim() + "'").FirstOrDefault();
                    }

                    if (!string.IsNullOrEmpty(TxtValorI.Text.Trim()))
                    {
                        _resultado = _dtbparametro.Select("Detalle='" + TxtDetalle.Text.ToUpper() + "' AND ValorI='" +
                            TxtValorI.Text.Trim() + "'").FirstOrDefault();
                    }

                    if (_resultado != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Ya esta definido un Detalle/Valor para el parámetro..!", this, "E", "C");
                    return;
                }

                _dtbagre = (DataTable)ViewState["DatosParametros"];
                _filagre = _dtbagre.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Detalle"] = TxtDetalle.Text.ToUpper().Trim();
                _filagre["ValorV"] = TxtValorV.Text.Trim();
                _filagre["ValorI"] = TxtValorI.Text.Trim() == "" ? "0" : TxtValorI.Text.Trim();
                _filagre["Estado"] = "Activo";
                _filagre["Orden"] = _orden + 1;
                _filagre["Nuevo"] = "SI";
                _dtbagre.Rows.Add(_filagre);
                ViewState["DatosParametros"] = _dtbagre;
                GrdvDatos.DataSource = _dtbagre;
                GrdvDatos.DataBind();
                new FuncionesDAO().SetearGrid(GrdvDatos, _imgsubir, 5, _imgbajar, 6, _dtbagre);
                TxtDetalle.Text = "";
                TxtValorV.Text = "";
                TxtValorI.Text = "";
                ImgCancelar.Enabled = true;
                Panel1.Visible = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModificar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtDetalle.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Detalle del Parámetro..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtValorV.Text.Trim()) && string.IsNullOrEmpty(TxtValorI.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Valor del Parámetro..!", this, "W", "C");
                    return;
                }

                if (!string.IsNullOrEmpty(TxtValorV.Text.Trim()) && !string.IsNullOrEmpty(TxtValorI.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Defina solo un tipo de valor..!", this, "E", "C");
                    return;
                }

                if (ViewState["DatosParametros"] != null)
                {
                    _dtbparametro = (DataTable)ViewState["DatosParametros"];
                    if (!string.IsNullOrEmpty(ViewState["ValorV"].ToString()))
                    {
                        if (ViewState["DetalleAnterior"].ToString() != TxtDetalle.Text.ToUpper() &&
                            ViewState["ValorV"].ToString() == TxtValorV.Text.Trim())
                        {
                            _result = _dtbparametro.Select("Detalle='" + TxtDetalle.Text.ToUpper().Trim() + "'");
                            _lexiste = _result.Length == 0 ? false : true;
                        }

                        if (ViewState["DetalleAnterior"].ToString() != TxtDetalle.Text.ToUpper() &&
                            ViewState["ValorV"].ToString() != TxtValorV.Text.Trim())
                        {
                            if (!string.IsNullOrEmpty(TxtValorV.Text.Trim()))
                            {
                                _result = _dtbparametro.Select("Detalle='" + TxtDetalle.Text.ToUpper().Trim() +
                                    "' and ValorV='" + TxtValorV.Text.Trim() + "'");
                                _lexiste = _result.Length == 0 ? false : true;
                            }
                        }

                        if (ViewState["DetalleAnterior"].ToString() == TxtDetalle.Text.ToUpper().Trim() &&
                            ViewState["ValorV"].ToString() != TxtValorV.Text.Trim())
                        {
                            if (!string.IsNullOrEmpty(TxtValorV.Text.Trim()))
                            {
                                _result = _dtbparametro.Select("ValorV='" + TxtValorV.Text.Trim() + "'");
                                _lexiste = _result.Length == 0 ? false : true;
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(ViewState["ValorI"].ToString()))
                    {
                        if (ViewState["DetalleAnterior"].ToString() != TxtDetalle.Text.ToUpper().Trim() &&
                            ViewState["ValorI"].ToString() == TxtValorI.Text.Trim())
                        {
                            _result = _dtbparametro.Select("Detalle='" + TxtDetalle.Text.ToUpper().Trim() + "'");
                            _lexiste = _result.Length == 0 ? false : true;
                        }

                        if (ViewState["DetalleAnterior"].ToString() != TxtDetalle.Text.ToUpper().Trim() &&
                            ViewState["ValorI"].ToString() != TxtValorI.Text.Trim())
                        {
                            _result = _dtbparametro.Select("Detalle='" + TxtDetalle.Text.ToUpper().Trim() +
                                "' and ValorI='" + TxtValorI.Text.Trim() + "'");
                            _lexiste = _result.Length == 0 ? false : true;
                        }

                        if (ViewState["DetalleAnterior"].ToString() == TxtDetalle.Text.ToUpper().Trim() &&
                            ViewState["ValorI"].ToString() != TxtValorI.Text.Trim())
                        {
                            _result = _dtbparametro.Select("ValorI='" + TxtValorI.Text.Trim() + "'");
                            _lexiste = _result.Length == 0 ? false : true;
                        }
                    }
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Ya esta definido un Detalle/Valor para el parámetro..!", this, "E", "C");
                    return;
                }

                _dtbparametro = (DataTable)ViewState["DatosParametros"];
                _resultado = _dtbparametro.Select("Codigo='" + ViewState["CodigoPADE"].ToString() + "'").FirstOrDefault();
                _resultado["Detalle"] = TxtDetalle.Text.ToUpper().Trim();
                _resultado["ValorV"] = TxtValorV.Text.Trim();
                _resultado["ValorI"] = TxtValorI.Text.Trim() == "" ? "0" : TxtValorI.Text.Trim();
                _resultado["Estado"] = ChkEstadoDet.Checked ? "Activo" : "Inactivo";
                _dtbparametro.AcceptChanges();
                ViewState["DatosParametros"] = _dtbparametro;
                _dtbparametro.DefaultView.Sort = "Orden ASC";
                _dtbparametro = _dtbparametro.DefaultView.ToTable();
                GrdvDatos.DataSource = _dtbparametro;
                GrdvDatos.DataBind();
                new FuncionesDAO().SetearGrid(GrdvDatos, _imgsubir, 5, _imgbajar, 6, _dtbparametro);
                Funclearobject(0);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgCancelar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Funclearobject(0);
                FunCargarMantenimiento();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
                throw;
            }
        }

        protected void ImgSubirNivel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                _dtbparametro = (DataTable)ViewState["DatosParametros"];
                _codigo = int.Parse(GrdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                _orden = int.Parse(GrdvDatos.DataKeys[gvRow.RowIndex].Values["Orden"].ToString());

                _resultado = _dtbparametro.Select("Codigo=" + _codigo).FirstOrDefault();
                _resultado["Orden"] = _orden - 1;
                _dtbparametro.AcceptChanges();

                _codigoant = int.Parse(GrdvDatos.DataKeys[gvRow.RowIndex - 1].Values["Codigo"].ToString());
                _ordenant = int.Parse(GrdvDatos.DataKeys[gvRow.RowIndex - 1].Values["Orden"].ToString());
                _resultado = _dtbparametro.Select("Codigo=" + _codigoant).FirstOrDefault();
                _resultado["Orden"] = _ordenant + 1;
                _dtbparametro.AcceptChanges();

                _dtbparametro.Select("Orden=0", "Orden ASC");
                _dtbparametro.DefaultView.Sort = "Orden ASC";
                GrdvDatos.DataSource = _dtbparametro;
                GrdvDatos.DataBind();
                new FuncionesDAO().SetearGrid(GrdvDatos, _imgsubir, 5, _imgbajar, 6, _dtbparametro);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void ImgBajarNivel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                _dtbparametro = (DataTable)ViewState["DatosParametros"];
                _codigo = int.Parse(GrdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                _orden = int.Parse(GrdvDatos.DataKeys[gvRow.RowIndex].Values["Orden"].ToString());

                _resultado = _dtbparametro.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _resultado["Orden"] = _orden + 1;
                _dtbparametro.AcceptChanges();

                _codigoant = int.Parse(GrdvDatos.DataKeys[gvRow.RowIndex + 1].Values["Codigo"].ToString());
                _ordenant = int.Parse(GrdvDatos.DataKeys[gvRow.RowIndex + 1].Values["Orden"].ToString());
                _resultado = _dtbparametro.Select("Codigo='" + _codigoant + "'").FirstOrDefault();
                _resultado["Orden"] = _ordenant - 1;
                _dtbparametro.AcceptChanges();

                _dtbparametro.Select("Orden=0", "Orden ASC");
                _dtbparametro.DefaultView.Sort = "Orden ASC";
                GrdvDatos.DataSource = _dtbparametro;
                GrdvDatos.DataBind();
                new FuncionesDAO().SetearGrid(GrdvDatos, _imgsubir, 5, _imgbajar, 6, _dtbparametro);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _dtbparametro = (DataTable)ViewState["DatosParametros"];
                ViewState["CodigoDetalle"] = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                _resultado = _dtbparametro.Select("Codigo='" + ViewState["CodigoDetalle"].ToString() + "'").FirstOrDefault();
                _resultado.Delete();
                _dtbparametro.AcceptChanges();
                ViewState["DatosParametros"] = _dtbparametro;
                GrdvDatos.DataSource = _dtbparametro;
                GrdvDatos.DataBind();
                new FuncionesDAO().SetearGrid(GrdvDatos, _imgsubir, 5, _imgbajar, 6, _dtbparametro);
                Funclearobject(0);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow fr in GrdvDatos.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }
                GrdvDatos.Rows[GrdvDatos.SelectedIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                ViewState["CodigoPADE"] = int.Parse(GrdvDatos.DataKeys[GrdvDatos.SelectedIndex].Values["Codigo"].ToString());
                _dtbparametro = (DataTable)ViewState["DatosParametros"];
                _resultado = _dtbparametro.Select("Codigo='" + ViewState["CodigoPADE"].ToString() + "'").FirstOrDefault();
                TxtDetalle.Text = _resultado["Detalle"].ToString();
                TxtValorV.Text = _resultado["ValorV"].ToString();
                TxtValorI.Text = _resultado["ValorI"].ToString() == "0" ? "" : _resultado["ValorI"].ToString();
                ViewState["DetalleAnterior"] = TxtDetalle.Text.Trim();
                ViewState["ValorV"] = _resultado["ValorV"].ToString();
                ViewState["ValorI"] = _resultado["ValorI"].ToString();
                LblEstadoDet.Visible = true;
                ChkEstadoDet.Visible = true;
                ChkEstadoDet.Text = _resultado["Estado"].ToString();
                ChkEstadoDet.Checked = _resultado["Estado"].ToString() == "Activo" ? true : false;
                ImgAgregar.Enabled = false;
                ImgModificar.Enabled = true;
                ImgCancelar.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgeliminar = (ImageButton)(e.Row.Cells[7].FindControl("ImgEliminar"));
                    _nuevo = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Nuevo"].ToString();
                    if (_nuevo == "SI")
                    {
                        _imgeliminar.Enabled = true;
                        _imgeliminar.ImageUrl = "~/Botones/eliminar.png";
                    }
                }
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
                if (string.IsNullOrEmpty(TxtParametro.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese nombre del Parámetro..!", this, "W", "C");
                    return;
                }

                _dtbparametro = (DataTable)ViewState["DatosParametros"];

                if (_dtbparametro.Rows.Count > 0)
                {
                    List<ParametroNew> _newparametro = new List<ParametroNew>();
                    foreach (DataRow _drfila in _dtbparametro.Rows)
                    {
                        _newparametro.Add(new ParametroNew()
                        {
                            Codigo = int.Parse(_drfila["Codigo"].ToString()),
                            Nombre = _drfila["Detalle"].ToString(),
                            ValorV = _drfila["ValorV"].ToString(),
                            ValorI = int.Parse(_drfila["ValorI"].ToString()),
                            Orden = int.Parse(_drfila["Orden"].ToString()),
                            Estado = _drfila["Estado"].ToString() == "Activo" ? true : false
                        });
                    }

                    _dts = new FuncionesDAO().FunCambiarDataSet(_newparametro);

                    SoftCob_PARAMETRO_CABECERA _parametro = new SoftCob_PARAMETRO_CABECERA();
                    {
                        _parametro.PARA_CODIGO = int.Parse(ViewState["CodigoParametro"].ToString());
                        _parametro.para_nombre = TxtParametro.Text.Trim().ToUpper();
                        _parametro.para_descripcion = TxtDescripcion.Text.Trim().ToUpper();
                        _parametro.para_estado = ChkEstadoPar.Checked;
                        _parametro.para_auxv1 = "";
                        _parametro.para_auxiv2 = "";
                        _parametro.para_auxii1 = 0;
                        _parametro.para_auxii2 = 0;
                        _parametro.para_fechacreacion = DateTime.Now;
                        _parametro.para_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                        _parametro.para_terminalcreacion = Session["MachineName"].ToString();
                        _parametro.para_fum = DateTime.Now;
                        _parametro.para_uum = int.Parse(Session["usuCodigo"].ToString());
                        _parametro.para_tum = Session["MachineName"].ToString();
                    }
                    if (ViewState["CodigoParametro"].ToString() == "0")
                    {
                        _mensaje = new ControllerDAO().FunCrearParametro(_newparametro, _parametro);
                        if (_mensaje != "") Lblerror.Text = _mensaje;
                        else Response.Redirect("WFrm_ParametroAdmin.aspx?MensajeRetornado=Guardado con Éxito");
                    }
                    else
                    {
                        if (ViewState["NombreParametro"].ToString() != TxtParametro.Text.Trim().ToUpper())
                            _contar = new ControllerDAO().FunConsultaParametro(TxtParametro.Text.Trim().ToUpper());

                        if (_contar == 0)
                        {
                            _mensaje = new ControllerDAO().FunParametroDetalle(_parametro, _dts);

                            if (_mensaje != "") Lblerror.Text = _mensaje;
                            else Response.Redirect("WFrm_ParametroAdmin.aspx?MensajeRetornado=Guardado con Éxito", true);
                        }
                    }
                }
                else new FuncionesDAO().FunShowJSMessage("Ingrese al menos un parámetro Detalle..!", this, "W", "C");
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_ParametroAdmin.aspx", true);
        }
        #endregion
    }
}