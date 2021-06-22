namespace SoftCob.Views.Cedente
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_NuevoCedente : Page
    {
        #region Variables
        DataTable _dtbcontactos = new DataTable();
        DataTable _dtbproductos = new DataTable();
        DataTable _dtbcatalogoproducto = new DataTable();
        DataTable _dtbagencias = new DataTable();
        DataTable _dtbbuscar = new DataTable();
        DataTable _dtbagregar = new DataTable();
        DataTable _tblbuscar;
        DataRow _filagre;
        DataSet _dts = new DataSet();
        DataSet _dts1 = new DataSet();
        DataSet _dts2 = new DataSet();
        DataSet _dts3 = new DataSet();
        DataTable _dtb = new DataTable();
        DataRow _resultado;
        bool _existe = false;
        int _con = 0, _maxcodigo = 0, _codigo = 0, _codigoproducto = 0, _codigocatalogo = 0, _respuesta = 0;
        bool _lexiste = false;
        string _mensaje = "", _producto = "";
        Label _lblest = new Label();
        CheckBox _chkest = new CheckBox();
        ListItem _product;
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
                    new ElastixDAO().ElastixHangUp(Session["IPLocalAdress"].ToString(), 9999);
                    Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                    return;
                }

                ViewState["CodigoCedente"] = Request["CodigoCedente"];
                ViewState["ConectarEnterprise"] = ConfigurationManager.AppSettings["SqlEnterprise"];

                _dtbcontactos.Columns.Add("CodigoContacto");
                _dtbcontactos.Columns.Add("Contacto");
                _dtbcontactos.Columns.Add("CodigoCargo");
                _dtbcontactos.Columns.Add("Cargo");
                _dtbcontactos.Columns.Add("Ext");
                _dtbcontactos.Columns.Add("Celular");
                _dtbcontactos.Columns.Add("Email1");
                _dtbcontactos.Columns.Add("Email2");
                ViewState["Contactos"] = _dtbcontactos;

                _dtbproductos.Columns.Add("CodigoProducto");
                _dtbproductos.Columns.Add("Producto");
                _dtbproductos.Columns.Add("Descripcion");
                _dtbproductos.Columns.Add("Estado");
                ViewState["Productos"] = _dtbproductos;

                _dtbcatalogoproducto.Columns.Add("Producto");
                _dtbcatalogoproducto.Columns.Add("CodigoCatalogo");
                _dtbcatalogoproducto.Columns.Add("CodigoProducto");
                _dtbcatalogoproducto.Columns.Add("CatalogoProducto");
                _dtbcatalogoproducto.Columns.Add("CodigoFamilia");
                _dtbcatalogoproducto.Columns.Add("Familia");
                _dtbcatalogoproducto.Columns.Add("Estado");
                _dtbcatalogoproducto.Columns.Add("CodProducto");
                ViewState["CatalogoProductos"] = _dtbcatalogoproducto;

                _dtbagencias.Columns.Add("AgenCodigo");
                _dtbagencias.Columns.Add("CodigoAgencia");
                _dtbagencias.Columns.Add("Agencia");
                _dtbagencias.Columns.Add("Sucursal");
                _dtbagencias.Columns.Add("Zona");
                _dtbagencias.Columns.Add("Estado");
                _dtbagencias.Columns.Add("CodigoSucursal");
                _dtbagencias.Columns.Add("CodigoZona");
                ViewState["Agencias"] = _dtbagencias;

                FunCargarCombos(0);
                FunCargarCombos(2);
                FunCargarCombos(3);
                FunCargarCombos(4);
                FunCargarCombos(5);

                ListItem item = new ListItem();
                {
                    item.Text = "--Seleccione Ciudad--";
                    item.Value = "0";
                }

                DdlCiudad.Items.Add(item);

                if (int.Parse(ViewState["CodigoCedente"].ToString()) > 0)
                {
                    Lbltitulo.Text = "Editar Cedente";
                    lblEstado.Visible = true;
                    ChkEstado.Visible = true;
                    FunCargarMantenimiento();
                }
                else Lbltitulo.Text = "Nuevo Cedente";
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarCombos(int tipo)
        {
            switch (tipo)
            {
                case 0:
                    DdlProvincia.DataSource = new ControllerDAO().FunGetProvincia();
                    DdlProvincia.DataTextField = "Descripcion";
                    DdlProvincia.DataValueField = "Codigo";
                    DdlProvincia.DataBind();
                    break;
                case 1:
                    DdlCiudad.DataSource = new ControllerDAO().FunGetCiudadPorID(int.Parse(DdlProvincia.SelectedValue));
                    DdlCiudad.DataTextField = "Descripcion";
                    DdlCiudad.DataValueField = "Codigo";
                    DdlCiudad.DataBind();
                    break;
                case 2:
                    DdlCargo.DataSource = new ControllerDAO().FunGetParametroDetalle("CARGOS", "--Seleccione Cargo--", "S");
                    DdlCargo.DataTextField = "Descripcion";
                    DdlCargo.DataValueField = "Codigo";
                    DdlCargo.DataBind();
                    break;
                case 3:
                    DdlProducto.DataSource = new CedenteDAO().FunGetProductoPorID(int.Parse(ViewState["CodigoCedente"].ToString()));
                    DdlProducto.DataTextField = "Descripcion";
                    DdlProducto.DataValueField = "Codigo";
                    DdlProducto.DataBind();
                    break;
                case 4:
                    DdlSucursal.DataSource = new ControllerDAO().FunGetParametroDetalle("SUCURSAL", "--Seleccione Sucursal--", "S");
                    DdlSucursal.DataTextField = "Descripcion";
                    DdlSucursal.DataValueField = "Codigo";
                    DdlSucursal.DataBind();
                    break;
                case 5:
                    DdlZona.DataSource = new ControllerDAO().FunGetParametroDetalle("ZONAS", "--Seleccione Zona--", "S");
                    DdlZona.DataTextField = "Descripcion";
                    DdlZona.DataValueField = "Codigo";
                    DdlZona.DataBind();
                    break;

            }
        }

        protected void FunCargarComboProductos()
        {
            DdlProducto.Items.Clear();

            if (ViewState["Productos"] != null)
            {
                _dtb = (DataTable)ViewState["Productos"];
                //_product.Text = "--Seleccione Producto--";
                //_product.Value = "0";
                //DdlProducto.Items.Add(_product);

                if (_dtb.Rows.Count > 0)
                {
                    foreach (DataRow _dr in _dtb.Rows)
                    {
                        ListItem _nuevoproducto = new ListItem();
                        _con++;
                        _nuevoproducto.Text = _dr[1].ToString();
                        _nuevoproducto.Value = _dr[0].ToString();
                        _product = DdlProducto.Items.FindByText(_dr[1].ToString());
                        if (_product == null) DdlProducto.Items.Add(_nuevoproducto);
                    }
                }
            }
        }

        protected void FunCargarMantenimiento()
        {
            SoftCob_CEDENTE cedente = new CedenteDAO().FunGetCedentePorID(int.Parse(ViewState["CodigoCedente"].ToString()));
            DdlProvincia.SelectedValue = cedente.cede_provcod.ToString();
            FunCargarCombos(1);
            DdlCiudad.SelectedValue = cedente.cede_ciudcod.ToString();
            TxtCedente.Text = cedente.cede_nombre;
            ViewState["Cedente"] = cedente.cede_nombre;
            TxtRuc.Text = cedente.cede_ruc;
            TxtDireccion.Text = cedente.cede_direccion;
            TxtTelefono1.Text = cedente.cede_telefono1;
            TxtTelefono2.Text = cedente.cede_telefono2;
            TxtFax.Text = cedente.cede_fax;
            TxtUrl.Text = cedente.cede_url;
            ChkEstado.Text = cedente.cede_estado ? "Activo" : "Inactivo";
            ChkEstado.Checked = cedente.cede_estado;
            DdlNivelArbol.SelectedValue = cedente.cede_auxi1.ToString();
            ViewState["usucreacion"] = cedente.cede_usuariocreacion;
            ViewState["fechacreacion"] = cedente.cede_fechacreacion;
            ViewState["terminalcreacion"] = cedente.cede_terminalcreacion;

            _dts = new CedenteDAO().FunGetContactos(cedente.CEDE_CODIGO);
            GrdvContactos.DataSource = _dts;
            GrdvContactos.DataBind();
            ViewState["Contactos"] = _dts.Tables[0];

            _dts1 = new CedenteDAO().FunGetProductos(cedente.CEDE_CODIGO);
            GrdvProductos.DataSource = _dts1;
            GrdvProductos.DataBind();
            ViewState["Productos"] = _dts1.Tables[0];

            _dts2 = new CedenteDAO().FunGetCatalogoProducto(cedente.CEDE_CODIGO);
            GrdvCatalogoProd.DataSource = _dts2;
            GrdvCatalogoProd.DataBind();
            ViewState["CatalogoProductos"] = _dts2.Tables[0];

            _dts3 = new ConsultaDatosDAO().FunConsultaDatos(2, cedente.CEDE_CODIGO, 0, 0, "", "", "", Session["Conectar"].ToString());
            GrdvAgencias.DataSource = _dts3;
            GrdvAgencias.DataBind();
            ViewState["Agencias"] = _dts3.Tables[0];
        }

        protected void FunLimpiarCatalogoProducto()
        {
            try
            {
                _dts = new CedenteDAO().FunGetCatalogoProducto(int.Parse(ViewState["CodigoCedente"].ToString()));
                GrdvCatalogoProd.DataSource = _dts;
                GrdvCatalogoProd.DataBind();
                ViewState["CatalogoProductos"] = _dts.Tables[0];
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void ImgNewContacto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlCargo.SelectedValue == "")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cargo..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtContacto.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombre del contacto..!", this);
                    return;
                }

                if (!string.IsNullOrEmpty(TxtEmail1.Text))
                {
                    if (new FuncionesDAO().Email_bien_escrito(TxtEmail1.Text) == false)
                    {
                        new FuncionesDAO().FunShowJSMessage("Ingrese un e-mail válido..!", this);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(TxtEmail2.Text))
                {
                    if (new FuncionesDAO().Email_bien_escrito(TxtEmail2.Text) == false)
                    {
                        new FuncionesDAO().FunShowJSMessage("Ingrese un e-mail válido..!", this);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(TxtCelular.Text.Trim()))
                {
                    if (TxtCelular.Text.Trim().Length < 10)
                    {
                        new FuncionesDAO().FunShowJSMessage("No. de celular incompleto..!", this);
                        return;
                    }
                    if (TxtCelular.Text.Trim().Substring(0, 2) != "09")
                    {
                        new FuncionesDAO().FunShowJSMessage("No. de celular incorrecto..!", this);
                        return;
                    }
                }

                if (ViewState["Contactos"] != null)
                {
                    _dtbbuscar = (DataTable)ViewState["Contactos"];
                    if (_dtbbuscar.Rows.Count > 0)
                        _maxcodigo = _dtbbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["CodigoContacto"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbbuscar.Select("Contacto='" + TxtContacto.Text.Trim().ToUpper() + "'").FirstOrDefault();
                    _existe = _resultado == null ? false : true;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Contacto ya está ingresado..!", this);
                    return;
                }

                _dtbagregar = (DataTable)ViewState["Contactos"];
                _filagre = _dtbagregar.NewRow();
                _filagre["CodigoContacto"] = _maxcodigo + 1;
                _filagre["Contacto"] = TxtContacto.Text.Trim().ToUpper();
                _filagre["CodigoCargo"] = DdlCargo.SelectedValue;
                _filagre["Cargo"] = DdlCargo.SelectedItem.ToString();
                _filagre["Ext"] = TxtExt.Text.Trim();
                _filagre["Celular"] = TxtCelular.Text.Trim();
                _filagre["Email1"] = TxtEmail1.Text.Trim();
                _filagre["Email2"] = TxtEmail2.Text.Trim();
                _dtbagregar.Rows.Add(_filagre);
                _dtbagregar.DefaultView.Sort = "Contacto";
                _dtbagregar = _dtbagregar.DefaultView.ToTable();
                ViewState["Contactos"] = _dtbagregar;
                GrdvContactos.DataSource = _dtbagregar;
                GrdvContactos.DataBind();
                TxtContacto.Text = "";
                DdlCargo.SelectedIndex = 0;
                TxtCelular.Text = "";
                TxtExt.Text = "";
                TxtEmail1.Text = "";
                TxtEmail2.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(1);
        }

        protected void ImgEdiContacto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                ViewState["CodigoContacto"] = GrdvContactos.DataKeys[_gvrow.RowIndex].Values["CodigoContacto"].ToString();
                _dtbbuscar = (DataTable)ViewState["Contactos"];
                _resultado = _dtbbuscar.Select("CodigoContacto='" + ViewState["CodigoContacto"].ToString() + "'").FirstOrDefault();
                TxtContacto.Text = _resultado[1].ToString();
                ViewState["Contacto"] = TxtContacto.Text.Trim();
                DdlCargo.SelectedValue = _resultado[2].ToString();
                TxtExt.Text = _resultado[4].ToString();
                TxtCelular.Text = _resultado[5].ToString();
                TxtEmail1.Text = _resultado[6].ToString();
                TxtEmail2.Text = _resultado[7].ToString();
                ImgNewContacto.Enabled = false;
                ImgEditarContacto.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEditarContacto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlCargo.SelectedValue == "")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cargo..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtContacto.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombre del contacto..!", this);
                    return;
                }

                if (!string.IsNullOrEmpty(TxtEmail1.Text))
                {
                    if (new FuncionesDAO().Email_bien_escrito(TxtEmail1.Text) == false)
                    {
                        new FuncionesDAO().FunShowJSMessage("Ingrese un e-mail válido..!", this);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(TxtEmail2.Text))
                {
                    if (new FuncionesDAO().Email_bien_escrito(TxtEmail2.Text) == false)
                    {
                        new FuncionesDAO().FunShowJSMessage("Ingrese un e-mail válido..!", this);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(TxtCelular.Text.Trim()))
                {
                    if (TxtCelular.Text.Trim().Length < 10)
                    {
                        new FuncionesDAO().FunShowJSMessage("No. de celular incompleto..!", this);
                        return;
                    }
                    if (TxtCelular.Text.Trim().Substring(0, 2) != "09")
                    {
                        new FuncionesDAO().FunShowJSMessage("No. de celular incorrecto..!", this);
                        return;
                    }
                }

                if (ViewState["Contactos"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["Contactos"];

                    if (ViewState["Contacto"].ToString() != TxtContacto.Text.Trim())
                    {
                        _resultado = _tblbuscar.Select("Contacto='" + TxtContacto.Text.ToUpper().Trim() + "'").FirstOrDefault();
                        _lexiste = _resultado == null ? false : true;
                    }
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Nombre de Contacto ya existe..!", this);
                    return;
                }

                _dtbagregar = (DataTable)ViewState["Contactos"];
                _resultado = _dtbagregar.Select("CodigoContacto='" + ViewState["CodigoContacto"].ToString() + "'").FirstOrDefault();
                _resultado["CodigoContacto"] = ViewState["CodigoContacto"].ToString();
                _resultado["Contacto"] = TxtContacto.Text.Trim().ToUpper();
                _resultado["CodigoCargo"] = DdlCargo.SelectedValue;
                _resultado["Cargo"] = DdlCargo.SelectedItem.ToString();
                _resultado["Ext"] = TxtExt.Text.Trim();
                _resultado["Celular"] = TxtCelular.Text.Trim();
                _resultado["Email1"] = TxtEmail1.Text.Trim();
                _resultado["Email2"] = TxtEmail2.Text.Trim();
                _dtbagregar.DefaultView.Sort = "Contacto";
                ViewState["Contactos"] = _dtbagregar;
                GrdvContactos.DataSource = _dtbagregar;
                GrdvContactos.DataBind();
                TxtContacto.Text = "";
                DdlCargo.SelectedIndex = 0;
                TxtCelular.Text = "";
                TxtExt.Text = "";
                TxtEmail1.Text = "";
                TxtEmail2.Text = "";
                ImgNewContacto.Enabled = true;
                ImgEditarContacto.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgDelContacto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = int.Parse(GrdvContactos.DataKeys[_gvrow.RowIndex].Values["CodigoContacto"].ToString());

                if (new CedenteDAO().FunDelContacto(int.Parse(ViewState["CodigoCedente"].ToString()), _codigo) == "")
                {
                    _dtbcontactos = (DataTable)ViewState["Contactos"];
                    _resultado = _dtbcontactos.Select("CodigoContacto='" + _codigo + "'").FirstOrDefault();
                    _resultado.Delete();
                    ViewState["Contactos"] = _dtbcontactos;
                    GrdvContactos.DataSource = _dtbcontactos;
                    GrdvContactos.DataBind();
                }
                TxtContacto.Text = "";
                DdlCargo.SelectedIndex = 0;
                TxtCelular.Text = "";
                TxtExt.Text = "";
                TxtEmail1.Text = "";
                TxtEmail2.Text = "";
                ImgNewContacto.Enabled = true;
                ImgEditarContacto.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgNewProducto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtProducto.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese producto..!", this);
                    return;
                }

                if (ViewState["Productos"] != null)
                {
                    _dtbbuscar = (DataTable)ViewState["Productos"];

                    if (_dtbbuscar.Rows.Count > 0)
                        _maxcodigo = _dtbbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["CodigoProducto"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbbuscar.Select("Producto='" + TxtProducto.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Producto ya está ingresado..!", this);
                    return;
                }

                _dtbagregar = (DataTable)ViewState["Productos"];
                _filagre = _dtbagregar.NewRow();
                _filagre["CodigoProducto"] = _maxcodigo + 1;
                _filagre["Producto"] = TxtProducto.Text.Trim().ToUpper();
                _filagre["Descripcion"] = TxtDescripcion.Text.Trim().ToUpper();
                _filagre["Estado"] = "Activo";
                _dtbagregar.Rows.Add(_filagre);
                _dtbagregar.DefaultView.Sort = "Producto";
                _dtbagregar = _dtbagregar.DefaultView.ToTable();
                ViewState["Productos"] = _dtbagregar;
                GrdvProductos.DataSource = _dtbagregar;
                GrdvProductos.DataBind();
                TxtProducto.Text = "";
                TxtDescripcion.Text = "";
                FunCargarComboProductos();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void GrdvProductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkest = (CheckBox)(e.Row.Cells[1].FindControl("chkEstProducto"));
                    _codigo = int.Parse(GrdvProductos.DataKeys[e.Row.RowIndex].Values["CodigoProducto"].ToString());
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(0, int.Parse(ViewState["CodigoCedente"].ToString()), _codigo, 0, "", "", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        _chkest.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                    }
                    else
                    {
                        _chkest.Enabled = false;
                        _chkest.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgEdiProducto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                ViewState["CodigoProducto"] = GrdvProductos.DataKeys[_gvrow.RowIndex].Values["CodigoProducto"].ToString();
                _dtbbuscar = (DataTable)ViewState["Productos"];
                _resultado = _dtbbuscar.Select("CodigoProducto='" + ViewState["CodigoProducto"].ToString() + "'").FirstOrDefault();
                TxtProducto.Text = _resultado[1].ToString();
                ViewState["Producto"] = TxtProducto.Text.Trim();
                ViewState["EstadoProducto"] = _resultado[3].ToString();
                TxtDescripcion.Text = _resultado[2].ToString();
                ImgNewProducto.Enabled = false;
                ImgEditarProducto.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgEditarProducto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtProducto.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese producto..!", this);
                    return;
                }

                if (ViewState["Productos"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["Productos"];

                    if (ViewState["Producto"].ToString() != TxtProducto.Text.Trim())
                    {
                        _resultado = _tblbuscar.Select("Producto='" + TxtProducto.Text.ToUpper().Trim() + "'").FirstOrDefault();
                        _lexiste = _resultado != null ? false : true;
                    }
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Nombre de Producto ya existe..!", this);
                    return;
                }

                _dtbagregar = (DataTable)ViewState["Productos"];
                _resultado = _dtbagregar.Select("CodigoProducto='" + ViewState["CodigoProducto"].ToString() + "'").FirstOrDefault();
                _resultado["CodigoProducto"] = ViewState["CodigoProducto"].ToString();
                _resultado["Producto"] = TxtProducto.Text.Trim().ToUpper();
                _resultado["Descripcion"] = TxtDescripcion.Text.Trim().ToUpper();
                _resultado["Estado"] = ViewState["EstadoProducto"].ToString();
                _dtbagregar.DefaultView.Sort = "Producto";
                _dtbagregar.AcceptChanges();
                ViewState["Productos"] = _dtbagregar;
                GrdvProductos.DataSource = _dtbagregar;
                GrdvProductos.DataBind();
                TxtProducto.Text = "";
                TxtDescripcion.Text = "";
                ImgNewProducto.Enabled = true;
                ImgEditarProducto.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgDelProducto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = int.Parse(GrdvProductos.DataKeys[_gvrow.RowIndex].Values["CodigoProducto"].ToString());
                _mensaje = new CedenteDAO().FunDelProducto(int.Parse(ViewState["CodigoCedente"].ToString()), _codigo);

                if (_mensaje == "")
                {
                    _dtbproductos = (DataTable)ViewState["Productos"];
                    _resultado = _dtbproductos.Select("CodigoProducto='" + _codigo + "'").FirstOrDefault();
                    _resultado.Delete();
                    _dtbproductos.AcceptChanges();
                    ViewState["Productos"] = _dtbproductos;
                    GrdvProductos.DataSource = _dtbproductos;
                    GrdvProductos.DataBind();
                    FunLimpiarCatalogoProducto();
                    FunCargarComboProductos();
                }
                else new FuncionesDAO().FunShowJSMessage(_mensaje, this);

                TxtProducto.Text = "";
                TxtDescripcion.Text = "";
                ImgNewProducto.Enabled = true;
                ImgEditarProducto.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ChkEstProducto_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkest = (CheckBox)(_gvrow.Cells[1].FindControl("chkEstProducto"));
                _dtbproductos = (DataTable)ViewState["Productos"];
                _codigo = int.Parse(GrdvProductos.DataKeys[_gvrow.RowIndex].Values["CodigoProducto"].ToString());
                _resultado = _dtbproductos.Select("CodigoProducto='" + _codigo + "'").FirstOrDefault();
                _resultado["Estado"] = _chkest.Checked ? "Activo" : "Inactivo";
                _dtbproductos.AcceptChanges();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgNewCatalogo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlProducto.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Producto..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtCodigoProd.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Código del Producto..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtCatalgoProducto.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombre del Catálogo..!", this);
                    return;
                }

                if (ViewState["CatalogoProductos"] != null)
                {
                    _dtbbuscar = (DataTable)ViewState["CatalogoProductos"];
                    _resultado = _dtbbuscar.Select("Producto='" + DdlProducto.SelectedItem.ToString() + "' and CodigoProducto='" + TxtCodigoProd.Text.Trim() + "'").FirstOrDefault();

                    if (_resultado != null)
                    {
                        new FuncionesDAO().FunShowJSMessage("Código Catálogo ya existe..!", this);
                        return;
                    }

                    _resultado = _dtbbuscar.Select("Producto='" + DdlProducto.SelectedItem.ToString() + "' and CatalogoProducto='" + TxtCatalgoProducto.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_resultado != null)
                    {
                        new FuncionesDAO().FunShowJSMessage("Catálogo Producto ya existe..!", this);
                        return;
                    }

                    _dtbbuscar.DefaultView.Sort = "CodigoCatalogo";

                    if (_dtbbuscar.Rows.Count > 0)
                        _maxcodigo = _dtbbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["CodigoCatalogo"]));
                    else _maxcodigo = 0;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Catálogo Producto ya está ingresado..!", this);
                    return;
                }

                _dtbagregar = (DataTable)ViewState["CatalogoProductos"];
                _filagre = _dtbagregar.NewRow();
                _filagre["Producto"] = DdlProducto.SelectedItem.ToString();
                _filagre["CodigoCatalogo"] = _maxcodigo + 1;
                _filagre["CodigoProducto"] = TxtCodigoProd.Text.Trim();
                _filagre["CatalogoProducto"] = TxtCatalgoProducto.Text.Trim().ToUpper();
                _filagre["CodigoFamilia"] = TxtCodigoFamilia.Text.Trim();
                _filagre["Familia"] = TxtFamilia.Text.Trim().ToUpper();
                _filagre["Estado"] = "Activo";
                _filagre["CodProducto"] = DdlProducto.SelectedValue;
                _dtbagregar.Rows.Add(_filagre);
                _dtbagregar.DefaultView.Sort = "Producto";
                _dtbagregar = _dtbagregar.DefaultView.ToTable();
                ViewState["CatalogoProductos"] = _dtbagregar;
                GrdvCatalogoProd.DataSource = _dtbagregar;
                GrdvCatalogoProd.DataBind();
                DdlProducto.SelectedIndex = 0;
                TxtCodigoProd.Text = "";
                TxtCatalgoProducto.Text = "";
                TxtCodigoFamilia.Text = "";
                TxtFamilia.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();

            }
        }
        protected void GrdvCatalogoProd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkest = (CheckBox)(e.Row.Cells[4].FindControl("chkEstCatalogo"));
                    _codigo = int.Parse(GrdvCatalogoProd.DataKeys[e.Row.RowIndex].Values["CodigoCatalogo"].ToString());
                    _producto = GrdvCatalogoProd.DataKeys[e.Row.RowIndex].Values["Producto"].ToString();
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(1, int.Parse(ViewState["CodigoCedente"].ToString()), _codigo, 0, _producto, "", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        _chkest.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                    }
                    else
                    {
                        _chkest.Enabled = false;
                        _chkest.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgEdiCatalogo_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
            ViewState["CodigoCatalogo"] = GrdvCatalogoProd.DataKeys[_gvrow.RowIndex].Values["CodigoCatalogo"].ToString();
            _dtbbuscar = (DataTable)ViewState["CatalogoProductos"];
            _resultado = _dtbbuscar.Select("CodigoCatalogo='" + ViewState["CodigoCatalogo"].ToString() + "'").FirstOrDefault();
            DdlProducto.SelectedValue = _resultado[7].ToString();
            TxtCodigoProd.Text = _resultado[2].ToString();
            TxtCatalgoProducto.Text = _resultado[3].ToString();
            TxtCodigoFamilia.Text = _resultado[4].ToString();
            TxtFamilia.Text = _resultado[5].ToString();
            ViewState["ddlProducto"] = DdlProducto.SelectedItem.ToString();
            ViewState["CodCatalago"] = TxtCodigoProd.Text.Trim();
            ViewState["ProductoCatalogo"] = TxtCatalgoProducto.Text.Trim();
            ImgNewCatalogo.Enabled = false;
            ImgEditarCatalogo.Enabled = true;
        }
        protected void ImgDelCatalogo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigocatalogo = int.Parse(GrdvCatalogoProd.DataKeys[_gvrow.RowIndex].Values["CodigoCatalogo"].ToString());
                _codigoproducto = int.Parse(GrdvCatalogoProd.DataKeys[_gvrow.RowIndex].Values["CodProducto"].ToString());
                _mensaje = new CedenteDAO().FunDelCatalogoProducto(int.Parse(ViewState["CodigoCedente"].ToString()), _codigoproducto, _codigocatalogo);

                if (_mensaje == "")
                {
                    _dtbcatalogoproducto = (DataTable)ViewState["CatalogoProductos"];
                    _resultado = _dtbcatalogoproducto.Select("CodigoCatalogo='" + _codigocatalogo + "'").FirstOrDefault();
                    _resultado.Delete();
                    ViewState["CatalogoProductos"] = _dtbcatalogoproducto;
                    GrdvCatalogoProd.DataSource = _dtbcatalogoproducto;
                    GrdvCatalogoProd.DataBind();
                }
                else new FuncionesDAO().FunShowJSMessage(_mensaje, this);

                ImgNewCatalogo.Enabled = true;
                ImgEditarCatalogo.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgEditarCatalogo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlProducto.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Producto..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtCodigoProd.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Código del Producto..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtCatalgoProducto.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombre del Catálogo..!", this);
                    return;
                }

                if (ViewState["CatalogoProductos"] != null)
                {
                    _dtbbuscar = (DataTable)ViewState["CatalogoProductos"];

                    if (ViewState["ddlProducto"].ToString() != DdlProducto.SelectedItem.ToString())
                    {
                        _resultado = _dtbbuscar.Select("Producto='" + DdlProducto.SelectedItem.ToString() + "' and CodigoProducto='" + TxtCodigoProd.Text.Trim() + "'").FirstOrDefault();

                        if (_resultado != null)
                        {
                            new FuncionesDAO().FunShowJSMessage("Código Catálogo ya existe..!", this);
                            return;
                        }

                        _resultado = _dtbbuscar.Select("Producto='" + DdlProducto.SelectedItem.ToString() + "' and CatalogoProducto='" + TxtCatalgoProducto.Text.Trim().ToUpper() + "'").FirstOrDefault();

                        if (_resultado != null)
                        {
                            new FuncionesDAO().FunShowJSMessage("Catálogo Producto ya existe..!", this);
                            return;
                        }
                    }

                    if (ViewState["ddlProducto"].ToString() == DdlProducto.SelectedItem.ToString())
                    {
                        if (ViewState["CodCatalago"].ToString() != TxtCodigoProd.Text.Trim())
                        {
                            _resultado = _dtbbuscar.Select("Producto='" + DdlProducto.SelectedItem.ToString() + "' and CodigoProducto='" + TxtCodigoProd.Text.Trim() + "'").FirstOrDefault();

                            if (_resultado != null)
                            {
                                new FuncionesDAO().FunShowJSMessage("Código Catálogo ya existe..!", this);
                                return;
                            }
                        }

                        if (ViewState["ProductoCatalogo"].ToString() != TxtCatalgoProducto.Text.Trim().ToUpper())
                        {
                            _resultado = _dtbbuscar.Select("Producto='" + DdlProducto.SelectedItem.ToString() + "' and CatalogoProducto='" + TxtCatalgoProducto.Text.Trim().ToUpper() + "'").FirstOrDefault();

                            if (_resultado != null)
                            {
                                new FuncionesDAO().FunShowJSMessage("Catálogo Producto ya existe..!", this);
                                return;
                            }
                        }
                    }
                }

                _dtbagregar = (DataTable)ViewState["CatalogoProductos"];
                _resultado = _dtbagregar.Select("CodigoCatalogo='" + ViewState["CodigoCatalogo"].ToString() + "'").FirstOrDefault();
                _resultado["Producto"] = DdlProducto.SelectedItem.ToString();
                _resultado["CodigoCatalogo"] = int.Parse(ViewState["CodigoCatalogo"].ToString());
                _resultado["CodigoProducto"] = TxtCodigoProd.Text.Trim();
                _resultado["CatalogoProducto"] = TxtCatalgoProducto.Text.Trim().ToUpper();
                _resultado["CodigoFamilia"] = TxtCodigoFamilia.Text.Trim();
                _resultado["Familia"] = TxtFamilia.Text.Trim().ToUpper();
                _resultado["Estado"] = "Activo";
                _resultado["CodProducto"] = DdlProducto.SelectedValue;
                _dtbagregar.AcceptChanges();
                _dtbagregar.DefaultView.Sort = "CatalogoProducto";
                ViewState["CatalogoProductos"] = _dtbagregar;
                GrdvCatalogoProd.DataSource = _dtbagregar;
                GrdvCatalogoProd.DataBind();
                DdlProducto.SelectedIndex = 0;
                TxtCodigoProd.Text = "";
                TxtCatalgoProducto.Text = "";
                TxtCodigoFamilia.Text = "";
                TxtFamilia.Text = "";
                ImgNewCatalogo.Enabled = true;
                ImgEditarCatalogo.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstado.Text = ChkEstado.Checked ? "Activo" : "Inactivo";
        }
        protected void ChkEstCatalogo_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
            _chkest = (CheckBox)(_gvrow.Cells[4].FindControl("chkEstCatalogo"));
            _dtbcatalogoproducto = (DataTable)ViewState["CatalogoProductos"];
            _codigo = int.Parse(GrdvCatalogoProd.DataKeys[_gvrow.RowIndex].Values["CodigoCatalogo"].ToString());
            _resultado = _dtbcatalogoproducto.Select("CodigoCatalogo='" + _codigo + "'").FirstOrDefault();
            _resultado["Estado"] = _chkest.Checked ? "Activo" : "Inactivo";
            _dtbcatalogoproducto.AcceptChanges();
        }
        protected void ImgNewAgencia_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtCodigoAgencia.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Código de Agencia..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtAgencia.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombre de la Agencia..!", this);
                    return;
                }

                if (ViewState["Agencias"] != null)
                {
                    _dtbbuscar = (DataTable)ViewState["Agencias"];

                    if (_dtbbuscar.Rows.Count > 0)
                        _maxcodigo = _dtbbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["AgenCodigo"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbbuscar.Select("Agencia='" + TxtAgencia.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Agencia ya está ingresada..!", this);
                    return;
                }

                _dtbagregar = (DataTable)ViewState["Agencias"];
                _filagre = _dtbagregar.NewRow();
                _filagre["AgenCodigo"] = _maxcodigo + 1;
                _filagre["CodigoAgencia"] = TxtCodigoAgencia.Text.Trim();
                _filagre["Agencia"] = TxtAgencia.Text.Trim().ToUpper();
                _filagre["Sucursal"] = DdlSucursal.SelectedItem.ToString();
                _filagre["Zona"] = DdlZona.SelectedItem.ToString();
                _filagre["Estado"] = "Activo";
                _filagre["CodigoSucursal"] = DdlSucursal.SelectedValue;
                _filagre["CodigoZona"] = DdlZona.SelectedValue;
                _dtbagregar.Rows.Add(_filagre);
                _dtbagregar.DefaultView.Sort = "Agencia";
                ViewState["Agencias"] = _dtbagregar;
                GrdvAgencias.DataSource = _dtbagregar;
                GrdvAgencias.DataBind();
                TxtCodigoAgencia.Text = "";
                TxtAgencia.Text = "";
                DdlSucursal.SelectedIndex = 0;
                DdlZona.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgEditarAgencia_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtCodigoAgencia.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Código de Agencia..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtAgencia.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombre de la Agencia..!", this);
                    return;
                }

                if (ViewState["Agencias"] != null)
                {
                    _dtbbuscar = (DataTable)ViewState["Agencias"];

                    if (ViewState["Agencia"].ToString() != TxtAgencia.Text.Trim().ToUpper() && ViewState["CodigoAgencia"].ToString() == TxtCodigoAgencia.Text.Trim().ToUpper())
                    {
                        _resultado = _dtbbuscar.Select("Agencia='" + TxtAgencia.Text.Trim().ToUpper() + "'").FirstOrDefault();

                        if (_resultado != null)
                        {
                            new FuncionesDAO().FunShowJSMessage("Nombre de Agencia ya existe..!", this);
                            return;
                        }
                    }

                    if (ViewState["Agencia"].ToString() != TxtAgencia.Text.Trim().ToUpper() && ViewState["CodigoAgencia"].ToString() != TxtCodigoAgencia.Text.Trim().ToUpper())
                    {
                        _resultado = _dtbbuscar.Select("Agencia='" + TxtAgencia.Text.Trim().ToUpper() + "' and CodigoAgencia='" + TxtCodigoAgencia.Text.Trim().ToUpper() + "'").FirstOrDefault();

                        if (_resultado != null)
                        {
                            new FuncionesDAO().FunShowJSMessage("Datos de Agencia ya están ingresados..!", this);
                            return;
                        }
                    }

                    if (ViewState["Agencia"].ToString() == TxtAgencia.Text.Trim().ToUpper() && ViewState["CodigoAgencia"].ToString() != TxtCodigoAgencia.Text.Trim().ToUpper())
                    {
                        _resultado = _dtbbuscar.Select("CodigoAgencia='" + TxtCodigoAgencia.Text.Trim().ToUpper() + "'").FirstOrDefault();

                        if (_resultado != null)
                        {
                            new FuncionesDAO().FunShowJSMessage("Código de Agencia ya existe..!", this);
                            return;
                        }
                    }
                }

                _dtbagregar = (DataTable)ViewState["Agencias"];
                _resultado = _dtbagregar.Select("AgenCodigo='" + ViewState["AgenCodigo"].ToString() + "'").FirstOrDefault();
                _resultado["AgenCodigo"] = int.Parse(ViewState["AgenCodigo"].ToString());
                _resultado["CodigoAgencia"] = TxtCodigoAgencia.Text.Trim();
                _resultado["Agencia"] = TxtAgencia.Text.Trim().ToUpper();
                _resultado["Sucursal"] = DdlSucursal.SelectedItem.ToString();
                _resultado["Zona"] = DdlZona.SelectedItem.ToString();
                _resultado["Estado"] = "Activo";
                _resultado["CodigoSucursal"] = DdlSucursal.SelectedValue;
                _resultado["CodigoZona"] = DdlZona.SelectedValue;
                _dtbagregar.AcceptChanges();
                _dtbagregar.DefaultView.Sort = "Agencia";
                ViewState["Agencias"] = _dtbagregar;
                GrdvAgencias.DataSource = _dtbagregar;
                GrdvAgencias.DataBind();
                TxtCodigoAgencia.Text = "";
                TxtAgencia.Text = "";
                DdlSucursal.SelectedIndex = 0;
                DdlZona.SelectedIndex = 0;
                ImgNewAgencia.Enabled = true;
                ImgEditarAgencia.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ChkEstAgencias_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
            _chkest = (CheckBox)(_gvrow.Cells[4].FindControl("chkEstAgencias"));
            _dtbagencias = (DataTable)ViewState["Agencias"];
            _codigo = int.Parse(GrdvAgencias.DataKeys[_gvrow.RowIndex].Values["AgenCodigo"].ToString());
            _resultado = _dtbagencias.Select("AgenCodigo='" + _codigo + "'").FirstOrDefault();
            _resultado["Estado"] = _chkest.Checked ? "Activo" : "Inactivo";
            _dtbagencias.AcceptChanges();
        }
        protected void ImgEdiAgencias_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
            ViewState["AgenCodigo"] = GrdvAgencias.DataKeys[_gvrow.RowIndex].Values["AgenCodigo"].ToString();
            _dtbbuscar = (DataTable)ViewState["Agencias"];
            _resultado = _dtbbuscar.Select("AgenCodigo='" + ViewState["AgenCodigo"].ToString() + "'").FirstOrDefault();
            TxtCodigoAgencia.Text = _resultado[1].ToString();
            TxtAgencia.Text = _resultado[2].ToString();
            DdlSucursal.SelectedValue = _resultado[6].ToString();
            DdlZona.SelectedValue = _resultado[7].ToString();
            ViewState["CodigoAgencia"] = TxtCodigoAgencia.Text;
            ViewState["Agencia"] = TxtAgencia.Text;
            ImgNewAgencia.Enabled = false;
            ImgEditarAgencia.Enabled = true;
        }
        protected void ImgDelAgencias_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = int.Parse(GrdvAgencias.DataKeys[_gvrow.RowIndex].Values["AgenCodigo"].ToString());
                _mensaje = new CedenteDAO().FunDelAgencia(int.Parse(ViewState["CodigoCedente"].ToString()), _codigo);

                if (_mensaje == "")
                {
                    _dtbagencias = (DataTable)ViewState["Agencias"];
                    _resultado = _dtbagencias.Select("AgenCodigo='" + _codigo + "'").FirstOrDefault();
                    _resultado.Delete();
                    ViewState["Agencias"] = _dtbagencias;
                    GrdvAgencias.DataSource = _dtbagencias;
                    GrdvAgencias.DataBind();
                }
                else new FuncionesDAO().FunShowJSMessage(_mensaje, this);

                TxtCodigoAgencia.Text = "";
                TxtAgencia.Text = "";
                DdlSucursal.SelectedIndex = 0;
                DdlZona.SelectedIndex = 0;
                ImgNewAgencia.Enabled = true;
                ImgEditarAgencia.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void GrdvAgencias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkest = (CheckBox)(e.Row.Cells[4].FindControl("chkEstAgencias"));
                    _codigo = int.Parse(GrdvAgencias.DataKeys[e.Row.RowIndex].Values["AgenCodigo"].ToString());
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(3, int.Parse(ViewState["CodigoCedente"].ToString()), _codigo, 0, "", "", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        _chkest.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                    }
                    else
                    {
                        _chkest.Enabled = false;
                        _chkest.Checked = true;
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
                if (DdlProvincia.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Provincia..!", this);
                    return;
                }

                if (DdlCiudad.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Ciudad..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtCedente.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Cedente..!", this);
                    return;
                }

                if (DdlNivelArbol.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Nivel del Árbol..!", this);
                    return;
                }

                if (ViewState["Cedente"] != null)
                {
                    if (ViewState["Cedente"].ToString() != TxtCedente.Text.Trim().ToUpper())
                    {
                        if (new CedenteDAO().FunConsultarCedenteporNombre(int.Parse(DdlCiudad.SelectedValue), TxtCedente.Text.Trim().ToUpper()))
                        {
                            new FuncionesDAO().FunShowJSMessage("Cedente ya se encuentra registrado..!", this);
                            return;
                        }
                    }
                }
                else
                {
                    if (new CedenteDAO().FunConsultarCedenteporNombre(int.Parse(DdlCiudad.SelectedValue), TxtCedente.Text.Trim().ToUpper()))
                    {
                        new FuncionesDAO().FunShowJSMessage("Cedente ya se encuentra registrado..!", this);
                        return;
                    }
                }

                SoftCob_CEDENTE cedente = new SoftCob_CEDENTE();
                {
                    cedente.CEDE_CODIGO = int.Parse(ViewState["CodigoCedente"].ToString());
                    cedente.cede_provcod = int.Parse(DdlProvincia.SelectedValue);
                    cedente.cede_ciudcod = int.Parse(DdlCiudad.SelectedValue);
                    cedente.cede_nombre = TxtCedente.Text.Trim().ToUpper();
                    cedente.cede_direccion = TxtDireccion.Text.Trim().ToUpper();
                    cedente.cede_ruc = TxtRuc.Text.Trim();
                    cedente.cede_url = TxtUrl.Text.Trim();
                    cedente.cede_telefono1 = TxtTelefono1.Text;
                    cedente.cede_telefono2 = TxtTelefono2.Text;
                    cedente.cede_fax = TxtFax.Text;
                    cedente.cede_estado = ChkEstado.Checked;
                    cedente.cede_auxi1 = int.Parse(DdlNivelArbol.SelectedValue);
                    cedente.cede_auxi2 = 0;
                    cedente.cede_auxi3 = 0;
                    cedente.cede_auxv1 = "";
                    cedente.cede_auxiv2 = "";
                    cedente.cede_auxiv3 = "";
                    cedente.cede_fum = DateTime.Now;
                    cedente.cede_uum = int.Parse(Session["usuCodigo"].ToString());
                    cedente.cede_tum = Session["MachineName"].ToString();

                    if (int.Parse(ViewState["CodigoCedente"].ToString()) == 0)
                    {
                        cedente.cede_fechacreacion = DateTime.Now;
                        cedente.cede_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                        cedente.cede_terminalcreacion = Session["MachineName"].ToString();
                    }
                    else
                    {
                        if (ViewState["fechacreacion"] == null) ViewState["fechacreacion"] = DateTime.Now;
                        if (ViewState["usucreacion"] == null) ViewState["usucreacion"] = Session["usuCodigo"].ToString();
                        if (ViewState["terminalcreacion"] == null) ViewState["terminalcreacion"] = Session["MachineName"].ToString();

                        cedente.cede_fechacreacion = DateTime.Parse(ViewState["fechacreacion"].ToString());
                        cedente.cede_usuariocreacion = int.Parse(ViewState["usucreacion"].ToString());
                        cedente.cede_terminalcreacion = ViewState["terminalcreacion"].ToString();
                    }
                }

                _dtb = (DataTable)ViewState["Contactos"];

                if (_dtb.Rows.Count > 0)
                {
                    List<SoftCob_CONTACTOS_CEDENTE> contactosCedente = new List<SoftCob_CONTACTOS_CEDENTE>();
                    foreach (DataRow _dr in _dtb.Rows)
                    {
                        _codigo = new CedenteDAO().FunGetCodigoContacto(int.Parse(ViewState["CodigoCedente"].ToString()), 
                            int.Parse(_dr[0].ToString()));

                        contactosCedente.Add(new SoftCob_CONTACTOS_CEDENTE()
                        {
                            CEDE_CODIGO = int.Parse(ViewState["CodigoCedente"].ToString()),
                            COCE_CODIGO = _codigo,
                            coce_contacto = _dr[1].ToString(),
                            coce_cargo = _dr[2].ToString(),
                            coce_extension = _dr[4].ToString(),
                            coce_celular = _dr[5].ToString(),
                            coce_email1 = _dr[6].ToString(),
                            coce_email2 = _dr[7].ToString(),
                            coce_auxi1 = 0,
                            coce_auxv1 = "",
                        });
                    }

                    cedente.SoftCob_CONTACTOS_CEDENTE = new List<SoftCob_CONTACTOS_CEDENTE>();

                    foreach (SoftCob_CONTACTOS_CEDENTE addContactos in contactosCedente)
                    {
                        cedente.SoftCob_CONTACTOS_CEDENTE.Add(addContactos);
                    }
                }

                _dtb = (DataTable)ViewState["Productos"];

                if (_dtb.Rows.Count > 0)
                {
                    List<SoftCob_PRODUCTOS_CEDENTE> _productoscedente = new List<SoftCob_PRODUCTOS_CEDENTE>();

                    foreach (DataRow _dr in _dtb.Rows)
                    {
                        _codigo = new CedenteDAO().FunGetCodigoProducto(int.Parse(ViewState["CodigoCedente"].ToString()), 
                            int.Parse(_dr[0].ToString()));

                        _productoscedente.Add(new SoftCob_PRODUCTOS_CEDENTE()
                        {
                            CEDE_CODIGO = int.Parse(ViewState["CodigoCedente"].ToString()),
                            PRCE_CODIGO = _codigo,
                            prce_producto = _dr[1].ToString(),
                            prce_descripcion = _dr[2].ToString(),
                            prce_estado = _dr[3].ToString() == "Activo" ? true : false,
                            prce_auxv1 = "",
                            prce_auxv2 = "",
                            prce_auxi1 = 0,
                            prce_auxi2 = 0
                        });
                    }

                    cedente.SoftCob_PRODUCTOS_CEDENTE = new List<SoftCob_PRODUCTOS_CEDENTE>();

                    foreach (SoftCob_PRODUCTOS_CEDENTE _addproductos in _productoscedente)
                    {
                        cedente.SoftCob_PRODUCTOS_CEDENTE.Add(_addproductos);
                    }
                }

                _dtb = (DataTable)ViewState["Agencias"];

                if (_dtb.Rows.Count > 0)
                {
                    List<SoftCob_AGENCIAS_CEDENTE> _agenciascedente = new List<SoftCob_AGENCIAS_CEDENTE>();

                    foreach (DataRow _dr in _dtb.Rows)
                    {
                        _codigo = new CedenteDAO().FunGetCodigoAgencia(int.Parse(ViewState["CodigoCedente"].ToString()), int.Parse(_dr[0].ToString()));

                        _agenciascedente.Add(new SoftCob_AGENCIAS_CEDENTE()
                        {
                            CEDE_CODIGO = int.Parse(ViewState["CodigoCedente"].ToString()),
                            AGEN_CODIGO = _codigo,
                            agen_codigoagencia = _dr[1].ToString(),
                            agen_nombreagencia = _dr[2].ToString(),
                            agen_sucursal = _dr[6].ToString(),
                            agen_zona = _dr[7].ToString(),
                            agen_estado = _dr[5].ToString() == "Activo" ? true : false,
                            agen_auxv1 = "",
                            agen_auxv2 = "",
                            agen_auxi1 = 0,
                            agen_auxi2 = 0
                        });
                    }

                    cedente.SoftCob_AGENCIAS_CEDENTE = new List<SoftCob_AGENCIAS_CEDENTE>();

                    foreach (SoftCob_AGENCIAS_CEDENTE _addagencias in _agenciascedente)
                    {
                        cedente.SoftCob_AGENCIAS_CEDENTE.Add(_addagencias);
                    }
                }

                if (cedente.CEDE_CODIGO == 0)
                {
                    _respuesta = new CedenteDAO().FunCrearCedente(cedente);
                    ViewState["CodigoCedente"] = _respuesta;
                }
                else _respuesta = new CedenteDAO().FunEditCedente(cedente);

                _dtb = (DataTable)ViewState["CatalogoProductos"];

                if (_dtb.Rows.Count > 0)
                {
                    foreach (DataRow _dr in _dtb.Rows)
                    {
                        _mensaje = new CedenteDAO().FunCatalogoProductos(0, int.Parse(ViewState["CodigoCedente"].ToString()), _dr[0].ToString(),
                            int.Parse(_dr[1].ToString()), _dr[2].ToString(), _dr[3].ToString(), _dr[4].ToString(), _dr[5].ToString(), 
                            _dr[6].ToString(), "", "", 0, 0, Session["Conectar"].ToString());
                    }
                }

                if (_respuesta == -1) Lblerror.Text = "Error Inserción datos Cedente..";
                else
                {
                    _mensaje = new CedenteDAO().FunCrearTablaHistorico(TxtCedente.Text.Trim().ToUpper().Replace(" ", "") + "_" + 
                        ViewState["CodigoCedente"].ToString(), ViewState["ConectarEnterprise"].ToString());
                    Response.Redirect("WFrm_CedenteAdmin.aspx?MensajeRetornado=Guardado con Éxito", true);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_CedenteAdmin.aspx", true);
        }
        #endregion
    }
}