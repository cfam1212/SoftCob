namespace SoftCob.Views.ArbolDesicion
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ArbolScoreTelefono : Page
    {
        #region Variable
        DataSet _dts = new DataSet();
        DataTable _dtbscore = new DataTable();
        ListItem _accion = new ListItem();
        ListItem _efecto = new ListItem();
        ListItem _respuesta = new ListItem();
        ListItem _contacto = new ListItem();
        DataTable _tblbuscar = new DataTable();
        DataTable _tblagre = new DataTable();
        CheckBox _chkestado = new CheckBox();
        DataRow _filagre, _result;
        DataRow[] _change;
        int _idciudad = 0, _maxcodigo = 0, _codigo = 0;
        bool _lexiste = false;
        string _sql = "", _mensaje = "", _response = "";
        string[] _pathroot, _columnas;
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

                ViewState["codigoCatalogo"] = "0";
                ViewState["codigoCedente"] = "0";
                Lbltitulo.Text = "Administrar Árbol Score Telefónico";
                _accion.Text = "--Seleccione Acción--";
                _accion.Value = "0";
                DdlAccion.Items.Add(_accion);
                FunCargarCombos(0);

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunLlenarCiudadCedente(TreeNode treenode)
        {
            try
            {
                _dts = new CedenteDAO().FunGetCiuadesCedentes();

                if (_dts != null && treenode != null)
                {
                    foreach (DataRow _fila in _dts.Tables[0].Rows)
                    {
                        _idciudad = int.Parse(_fila[1].ToString());
                        TreeNode node = new TreeNode(_fila[0].ToString(), _fila[1].ToString());
                        node = FunAgregarCedentes(node, _idciudad);
                        treenode.ChildNodes.Add(node);
                    }
                    TrvCedentes.CollapseAll();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private TreeNode FunAgregarCedentes(TreeNode node, int idCiudad)
        {
            try
            {
                _dts = new CedenteDAO().FunGetCedentesporIDCiudad(idCiudad);

                if (_dts != null && _dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _drfila in _dts.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(_drfila[0].ToString(), _drfila[1].ToString());
                        unNode = FunAgregarProductoCedentes(unNode, int.Parse(_drfila[1].ToString()));
                        node.ChildNodes.Add(unNode);
                    }
                }
                return node;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
                return node;
            }
        }

        private TreeNode FunAgregarProductoCedentes(TreeNode node, int idCedente)
        {
            try
            {
                _dts = new CedenteDAO().FunGetProductosporIDCedente(idCedente);

                if (_dts != null && _dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _drfila in _dts.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(_drfila[0].ToString(), _drfila[1].ToString());
                        unNode = FunAgregarCatalogoProductos(unNode, int.Parse(_drfila[1].ToString()));
                        node.ChildNodes.Add(unNode);
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return node;
        }

        private TreeNode FunAgregarCatalogoProductos(TreeNode node, int idProducto)
        {
            try
            {
                _dts = new CedenteDAO().FunGetCatalogoProductosporIDProducto(idProducto);

                if (_dts != null && _dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _drfila in _dts.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(_drfila[0].ToString(), _drfila[1].ToString());
                        node.ChildNodes.Add(unNode);
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return node;
        }

        protected void FunCargarMantenimiento(int codigocedente, int codigocatalogo)
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(50, codigocedente, codigocatalogo, 0, "", "", "",
                    Session["Conectar"].ToString());
                ViewState["ArbolScore"] = _dts.Tables[0];
                GrdvArbolScore.DataSource = _dts;
                GrdvArbolScore.DataBind();

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void FunCargarCombos(int opcion)
        {
            try
            {
                switch (opcion)
                {
                    case 0:
                        DdlEfecto.Items.Clear();
                        _efecto.Text = "--Seleccione Efecto--";
                        _efecto.Value = "0";
                        DdlEfecto.Items.Add(_efecto);
                        DdlRespuesta.Items.Clear();
                        _respuesta.Text = "--Seleccione Respuesta--";
                        _respuesta.Value = "0";
                        DdlRespuesta.Items.Add(_respuesta);
                        DdlContacto.Items.Clear();
                        _contacto.Text = "--Seleccione Contacto--";
                        _contacto.Value = "0";
                        DdlContacto.Items.Add(_contacto);
                        break;
                    case 2:
                        DdlAccion.DataSource = new SpeechDAO().FunGetArbolNewAccion(int.Parse(ViewState["codigoCatalogo"].ToString()));
                        DdlAccion.DataTextField = "Descripcion";
                        DdlAccion.DataValueField = "Codigo";
                        DdlAccion.DataBind();
                        break;
                    case 3:
                        DdlEfecto.Items.Clear();
                        _efecto.Text = "--Seleccione Efecto--";
                        _efecto.Value = "0";
                        DdlEfecto.Items.Add(_efecto);
                        DdlRespuesta.Items.Clear();
                        _respuesta.Text = "--Seleccione Respuesta--";
                        _respuesta.Value = "0";
                        DdlRespuesta.Items.Add(_respuesta);
                        DdlContacto.Items.Clear();
                        _contacto.Text = "--Seleccione Contacto--";
                        _contacto.Value = "0";
                        DdlContacto.Items.Add(_contacto);
                        DdlEfecto.DataSource = new SpeechDAO().FunGetArbolNewEfecto(int.Parse(DdlAccion.SelectedValue));
                        DdlEfecto.DataTextField = "Descripcion";
                        DdlEfecto.DataValueField = "Codigo";
                        DdlEfecto.DataBind();
                        break;
                    case 4:
                        DdlRespuesta.Items.Clear();
                        _respuesta.Text = "--Seleccione Respuesta--";
                        _respuesta.Value = "0";
                        DdlRespuesta.Items.Add(_respuesta);
                        DdlContacto.Items.Clear();
                        _contacto.Text = "--Seleccione Contacto--";
                        _contacto.Value = "0";
                        DdlContacto.Items.Add(_contacto);
                        DdlRespuesta.DataSource = new SpeechDAO().FunGetArbolNewRespuesta(int.Parse(DdlEfecto.SelectedValue));
                        DdlRespuesta.DataTextField = "Descripcion";
                        DdlRespuesta.DataValueField = "Codigo";
                        DdlRespuesta.DataBind();
                        break;
                    case 5:
                        DdlContacto.Items.Clear();
                        _contacto.Text = "--Seleccione Contacto--";
                        _contacto.Value = "0";
                        DdlContacto.Items.Add(_contacto);
                        DdlContacto.DataSource = new SpeechDAO().FunGetArbolNewContacto(int.Parse(DdlRespuesta.SelectedValue));
                        DdlContacto.DataTextField = "Descripcion";
                        DdlContacto.DataValueField = "Codigo";
                        DdlContacto.DataBind();
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
        protected void TrvCedentes_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                TreeView arbolPres = (TreeView)(sender);
                TreeNode node = arbolPres.SelectedNode;
                Lblerror.Text = "";
                ViewState["codigoCedente"] = "0";
                ViewState["codigoCatalogo"] = "0";
                switch (node.Depth)
                {
                    case 4:
                        _pathroot = node.ValuePath.Split(new char[] { '/' });
                        ViewState["codigoCedente"] = _pathroot[3].ToString();
                        ViewState["codigoCatalogo"] = _pathroot[5].ToString();
                        lblCatalogo.InnerText = "Árbol Catálogo >>" + new CedenteDAO().FunGetNameCatalogoporID(int.Parse(_pathroot[5].ToString()));
                        FunCargarCombos(0);
                        FunCargarCombos(2);
                        FunCargarMantenimiento(int.Parse(ViewState["codigoCedente"].ToString()), int.Parse(ViewState["codigoCatalogo"].ToString()));
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlAccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(3);
        }

        protected void DdlEfecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(4);
        }

        protected void DdlRespuesta_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(5);
        }

        protected void TrvCedentes_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            switch (e.Node.Depth)
            {
                case 0:
                    FunLlenarCiudadCedente(e.Node);
                    break;
            }
        }

        protected void DdlCalifica_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DdlValor.Items.Clear();
                switch (DdlCalifica.SelectedValue)
                {
                    case "Score":
                        new FuncionesDAO().FunLlenarCombosValues(DdlValor, 1, 10);
                        break;
                    case "Estado":
                        new FuncionesDAO().FunLlenarCombosValues(DdlValor, 0, 1);
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAdd_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (int.Parse(ViewState["codigoCatalogo"].ToString()) == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Catálogo del Cedente..!", this);
                    return;
                }

                if (DdlAccion.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Acción..!", this);
                    return;
                }

                if (DdlCalifica.SelectedValue == "N")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Califica..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtCantidad.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese cantidad a validar..!", this);
                    return;
                }

                if (DdlValor.SelectedValue == "-1")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Valor..!", this);
                    return;
                }

                if (ViewState["ArbolScore"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["ArbolScore"];

                    if (_tblbuscar.Rows.Count > 0)
                        _maxcodigo = _tblbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _result = _tblbuscar.Select("AracCodigo='" + DdlAccion.SelectedValue + "' and ArefCodigo='" + DdlEfecto.SelectedValue + "' and ArreCodigo='" + DdlRespuesta.SelectedValue + "' and ArcoCodigo='" + DdlContacto.SelectedValue + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Ya existe definido Árbol Score..!", this);
                    return;
                }

                _tblagre = new DataTable();
                _tblagre = (DataTable)ViewState["ArbolScore"];
                _filagre = _tblagre.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["AracCodigo"] = DdlAccion.SelectedValue;
                _filagre["Accion"] = DdlAccion.SelectedItem.ToString();
                _filagre["ArefCodigo"] = DdlEfecto.SelectedValue;
                _filagre["Efecto"] = int.Parse(DdlEfecto.SelectedValue) == 0 ? "" : DdlEfecto.SelectedItem.ToString();
                _filagre["ArreCodigo"] = DdlRespuesta.SelectedValue;
                _filagre["Respuesta"] = int.Parse(DdlRespuesta.SelectedValue) == 0 ? "" : DdlRespuesta.SelectedItem.ToString();
                _filagre["ArcoCodigo"] = DdlContacto.SelectedValue;
                _filagre["Contacto"] = int.Parse(DdlContacto.SelectedValue) == 0 ? "" : DdlContacto.SelectedItem.ToString();
                _filagre["Califica"] = DdlCalifica.SelectedItem.ToString();
                _filagre["Cantidad"] = TxtCantidad.Text.Trim();
                _filagre["Valor"] = DdlValor.SelectedValue;
                _filagre["Estado"] = "Activo";
                _filagre["auxv1"] = "";
                _filagre["auxv2"] = "";
                _filagre["auxi1"] = "0";
                _filagre["auxi2"] = "0";
                _tblagre.Rows.Add(_filagre);
                _tblagre.DefaultView.Sort = "Accion";
                _tblagre = _tblagre.DefaultView.ToTable();
                ViewState["ArbolScore"] = _tblagre;
                GrdvArbolScore.DataSource = _tblagre;
                GrdvArbolScore.DataBind();
                TxtCantidad.Text = "1";
                DdlValor.SelectedValue = "-1";
                DdlCalifica.SelectedValue = "N";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvArbolScore_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkestado = (CheckBox)(e.Row.Cells[4].FindControl("chkEstado"));
                    _codigo = int.Parse(GrdvArbolScore.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString());
                    _sql = "Select Estado=case arst_estado when 1 then 'Activo' else 'Inactivo' end from SoftCob_ARBOL_SCORETELEFONO where ";
                    _sql += "arst_cedecodigo=" + int.Parse(ViewState["codigoCedente"].ToString()) + " and arst_cpcecodigo=" + int.Parse(ViewState["codigoCatalogo"].ToString());
                    _sql += "and ARST_CODIGO=" + _codigo;

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql, "", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count == 0)
                    {
                        _chkestado.Checked = true;
                        _chkestado.Enabled = false;
                    }

                    _dtbscore = (DataTable)ViewState["ArbolScore"];
                    _result = _dtbscore.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                    _chkestado.Checked = _result["Estado"].ToString() == "Activo" ? true : false;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModi_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlAccion.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Acción..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtCantidad.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese cantidad a validar..!", this);
                    return;
                }

                if (DdlValor.SelectedValue == "-1")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Valor..!", this);
                    return;
                }

                if (ViewState["Accion"].ToString() != DdlAccion.SelectedValue || ViewState["Efecto"].ToString() != DdlEfecto.SelectedValue
                    || ViewState["Respuesta"].ToString() != DdlRespuesta.SelectedValue || ViewState["Contacto"].ToString() != DdlContacto.SelectedValue)
                {
                    _tblbuscar = (DataTable)ViewState["ArbolScore"];
                    _maxcodigo = _tblbuscar.AsEnumerable()
                        .Max(row => int.Parse((string)row["Codigo"]));
                    _result = _tblbuscar.Select("AracCodigo='" + DdlAccion.SelectedValue + "' and ArefCodigo='" + DdlEfecto.SelectedValue + "' and ArreCodigo='" + DdlRespuesta.SelectedValue + "' and ArcoCodigo='" + DdlContacto.SelectedValue + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Ya existe definido Árbol Score..!", this);
                    return;
                }

                _dtbscore = (DataTable)ViewState["ArbolScore"];
                _change = _dtbscore.Select("Codigo='" + ViewState["CodigoScore"].ToString() + "'");
                _change[0]["AracCodigo"] = DdlAccion.SelectedValue;
                _change[0]["Accion"] = DdlAccion.SelectedItem.ToString();
                _change[0]["ArefCodigo"] = DdlEfecto.SelectedValue;
                _change[0]["Efecto"] = int.Parse(DdlEfecto.SelectedValue) == 0 ? "" : DdlEfecto.SelectedItem.ToString();
                _change[0]["ArreCodigo"] = DdlRespuesta.SelectedValue;
                _change[0]["Respuesta"] = int.Parse(DdlRespuesta.SelectedValue) == 0 ? "" : DdlRespuesta.SelectedItem.ToString();
                _change[0]["ArcoCodigo"] = DdlContacto.SelectedValue;
                _change[0]["Contacto"] = int.Parse(DdlContacto.SelectedValue) == 0 ? "" : DdlContacto.SelectedItem.ToString();
                _change[0]["Califica"] = DdlCalifica.SelectedItem.ToString();
                _change[0]["Cantidad"] = TxtCantidad.Text.Trim();
                _change[0]["Valor"] = DdlValor.SelectedValue;
                _dtbscore.AcceptChanges();
                ViewState["ArbolScore"] = _dtbscore;
                GrdvArbolScore.DataSource = _dtbscore;
                GrdvArbolScore.DataBind();
                FunCargarCombos(0);
                DdlAccion.SelectedValue = "0";
                DdlCalifica.SelectedValue = "N";
                DdlValor.SelectedValue = "-1";
                ImgAdd.Enabled = true;
                ImgModi.Enabled = false;
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
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                _dtbscore = (DataTable)ViewState["ArbolScore"];
                _codigo = int.Parse(GrdvArbolScore.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                ViewState["CodigoScore"] = _codigo;
                _result = _dtbscore.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                DdlCalifica.SelectedValue = _result["Califica"].ToString();
                DdlValor.Items.Clear();

                if (DdlCalifica.SelectedValue == "Score") new FuncionesDAO().FunLlenarCombosValues(DdlValor, 1, 10);

                if (DdlCalifica.SelectedValue == "Estado") new FuncionesDAO().FunLlenarCombosValues(DdlValor, 0, 1);

                TxtCantidad.Text = _result["Cantidad"].ToString();
                DdlValor.SelectedValue = _result["Valor"].ToString();
                DdlAccion.SelectedValue = _result["AracCodigo"].ToString();
                FunCargarCombos(3);
                DdlEfecto.SelectedValue = _result["ArefCodigo"].ToString();
                FunCargarCombos(4);
                DdlRespuesta.SelectedValue = _result["ArreCodigo"].ToString();
                FunCargarCombos(5);
                DdlContacto.SelectedValue = _result["ArcoCodigo"].ToString();
                ViewState["Accion"] = _result["AracCodigo"].ToString();
                ViewState["Efecto"] = _result["ArefCodigo"].ToString();
                ViewState["Respuesta"] = _result["ArreCodigo"].ToString();
                ViewState["Contacto"] = _result["ArcoCodigo"].ToString();
                ImgAdd.Enabled = false;
                ImgModi.Enabled = true;
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
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkestado = (CheckBox)(gvRow.Cells[6].FindControl("chkEstado"));
                _dtbscore = (DataTable)ViewState["ArbolScore"];
                _codigo = int.Parse(GrdvArbolScore.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                _result = _dtbscore.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result["Estado"] = _chkestado.Checked ? "Activo" : "Inactivo";
                _dtbscore.AcceptChanges();
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
                _columnas = new[] { "Codigo", "AracCodigo","ArefCodigo", "ArreCodigo", "ArcoCodigo", "Califica", "Cantidad",
                "Valor","Estado","auxv1","auxv2","auxi1","auxi2"};
                _dtbscore = (DataTable)ViewState["ArbolScore"];

                DataView _view = new DataView(_dtbscore);
                _dtbscore = _view.ToTable(true, _columnas);

                if (_dtbscore.Rows.Count == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("No existen datos para grabar..!", this);
                    return;
                }

                _mensaje = new ConsultaDatosDAO().FunCrearArbolScore(int.Parse(ViewState["codigoCedente"].ToString()), int.Parse(ViewState["codigoCatalogo"].ToString()), "", "", 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), _dtbscore, "sp_NewArbolScore", Session["Conectar"].ToString());

                _response = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");

                if (_mensaje == "") Response.Redirect(_response, false);
                else Lblerror.Text = _mensaje;
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
        #endregion
    }
}