namespace SoftCob.Views.Configuraciones
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_NuevoPresupuesto : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtbsegmento = new DataTable();
        int _idciudad = 0, _maxcodigo = 0, _between = 0, _codigo = 0;
        bool _lexiste = false;
        DataRow _result, _filagre;
        DataRow[] _resultado;
        ImageButton _imgdel = new ImageButton();
        string _redirect = "", _color = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["CodigoCEDE"] = "0";
                ViewState["CodigoCPCE"] = "0";

                Session["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                Lbltitulo.Text = "Definir Porcentaje Presupuesto de Carteras";

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimientos Y Funciones
        protected void TrvCedentes_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            switch (e.Node.Depth)
            {
                case 0:
                    FunLlenarCiudadCedente(e.Node);
                    break;
            }
        }

        private void FunLlenarCiudadCedente(TreeNode treenode)
        {
            try
            {
                _dts = new CedenteDAO().FunGetCiuadesCedentes();

                if (_dts != null && treenode != null)
                {
                    foreach (DataRow _dr in _dts.Tables[0].Rows)
                    {
                        _idciudad = int.Parse(_dr[1].ToString());
                        TreeNode node = new TreeNode(_dr[0].ToString(), _dr[1].ToString());
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
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return node;
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

        protected void FunCargarMantenimiento()
        {
            try
            {
                _dts = new CedenteDAO().FunGetSegmentoCabecera(int.Parse(ViewState["CodigoCEDE"].ToString()),
                    int.Parse(ViewState["CodigoCPCE"].ToString()), 1);
                GrdvSegmento.DataSource = _dts;
                GrdvSegmento.DataBind();
                ViewState["SegmentoCabecera"] = _dts.Tables[0];
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
            TreeView arbolPres = (TreeView)(sender);
            TreeNode node = arbolPres.SelectedNode;
            try
            {
                ViewState["CodigoCEDE"] = "0";
                ViewState["CodigoCPCE"] = "0";

                switch (node.Depth)
                {
                    case 1:
                        ViewState["CodigoCEDE"] = "0";
                        ViewState["CodigoCPCE"] = "0";
                        Lbltitulo.Text = "Definir Arbol de Decisión";
                        break;
                    case 2:
                        ViewState["CodigoCEDE"] = "0";
                        ViewState["CodigoCPCE"] = "0";

                        break;
                    case 3:
                        ViewState["CodigoCEDE"] = "0";
                        ViewState["CodigoCPCE"] = "0";
                        break;
                    case 4:
                        string[] pathRoot = node.ValuePath.Split(new char[] { '/' });
                        ViewState["CodigoCEDE"] = pathRoot[3].ToString();
                        ViewState["CodigoCPCE"] = pathRoot[5].ToString();
                        lblCatalogo.InnerText = "Definir Presupuesto Pocentaje >>" + 
                            new CedenteDAO().FunGetNameCatalogoporID(int.Parse(ViewState["CodigoCPCE"].ToString()));
                        FunCargarMantenimiento();
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void ImgAdd_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtEtiqueta.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Etiqueta..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtValorInicial.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Porcentaje Inicial..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtValorFinal.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Porcentaje Final..!", this);
                    return;
                }

                if (int.Parse(TxtValorFinal.Text.Trim()) <= int.Parse(TxtValorInicial.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Valor Final no puede ser menor o igual al Inicial..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(Request.Form[TxtColor.UniqueID]))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Color de Etiqueta..!", this);
                    return;
                }

                if (ViewState["SegmentoCabecera"] != null)
                {
                    _dtbsegmento = (DataTable)ViewState["SegmentoCabecera"];

                    if (_dtbsegmento.Rows.Count > 0)
                    {
                        _maxcodigo = _dtbsegmento.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    }
                    else _maxcodigo = 0;

                    if (_dtbsegmento.Rows.Count > 0)
                    {
                        foreach (DataRow _dr in _dtbsegmento.Rows)
                        {
                            _between = new FuncionesDAO().FunBetween(int.Parse(_dr[3].ToString()), int.Parse(_dr[4].ToString()),
                                int.Parse(TxtValorInicial.Text), int.Parse(TxtValorFinal.Text));

                            if (_between > 0)
                            {
                                _lexiste = true;
                                break;
                            }
                        }
                    }
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Segmento ya Existe Creado..!", this);
                    return;
                }

                _dtbsegmento = (DataTable)ViewState["SegmentoCabecera"];
                _filagre = _dtbsegmento.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Segmento"] = "PRESUPUESTO";
                _filagre["Descripcion"] = TxtEtiqueta.Text.Trim().ToUpper();
                _filagre["ValorI"] = TxtValorInicial.Text.Trim();
                _filagre["ValorF"] = TxtValorFinal.Text.Trim();
                _filagre["Estado"] = "Activo";
                _filagre["auxv1"] = Request.Form[TxtColor.UniqueID];
                _filagre["auxv2"] = "";
                _filagre["auxv3"] = "";
                _filagre["auxi1"] = "1";
                _filagre["auxi2"] = "0";
                _filagre["auxi3"] = "0";
                _dtbsegmento.Rows.Add(_filagre);
                _dtbsegmento.DefaultView.Sort = "ValorI";
                ViewState["SegmentoCabecera"] = _dtbsegmento;
                GrdvSegmento.DataSource = _dtbsegmento;
                GrdvSegmento.DataBind();
                TxtEtiqueta.Text = "";
                TxtColor.Text = "";
                TxtValorInicial.Text = "";
                TxtValorFinal.Text = "";
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
                if (string.IsNullOrEmpty(TxtEtiqueta.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Descripción Etiqueta..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtValorInicial.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Porcentaje Inicial..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtValorFinal.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Porcentaje Final..!", this);
                    return;
                }

                if (int.Parse(TxtValorFinal.Text.Trim()) <= int.Parse(TxtValorInicial.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Valor Final no puede ser menor o igual al Inicial..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(Request.Form[TxtColor.UniqueID]))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Color de Etiqueta..!", this);
                    return;
                }

                if (ViewState["SegmentoCabecera"] != null)
                {
                    _dtbsegmento = (DataTable)ViewState["SegmentoCabecera"];
                    _resultado = _dtbsegmento.Select("Codigo<>'" + ViewState["Codigo"].ToString() + "'");

                    foreach (DataRow _dr in _resultado)
                    {
                        _between = new FuncionesDAO().FunBetween(int.Parse(_dr[3].ToString()), int.Parse(_dr[4].ToString()),
                            int.Parse(TxtValorInicial.Text), int.Parse(TxtValorFinal.Text));

                        if (_between > 0)
                        {
                            _lexiste = true;
                            break;
                        }
                    }
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Segmento ya Existe Creado..!", this);
                    return;
                }

                _dtbsegmento = (DataTable)ViewState["SegmentoCabecera"];
                _result = _dtbsegmento.Select("Codigo='" + ViewState["Codigo"].ToString() + "'").FirstOrDefault();
                _result["Segmento"] = "PRESUPUESTO";
                _result["Descripcion"] = TxtEtiqueta.Text.Trim().ToUpper();
                _result["ValorI"] = TxtValorInicial.Text.Trim();
                _result["ValorF"] = TxtValorFinal.Text.Trim();
                _result["Estado"] = "Activo";
                _result["auxv1"] = Request.Form[TxtColor.UniqueID];
                _result["auxv2"] = "";
                _result["auxv3"] = "";
                _result["auxi1"] = "1";
                _result["auxi2"] = "0";
                _result["auxi3"] = "0";
                _dtbsegmento.AcceptChanges();
                _dtbsegmento.DefaultView.Sort = "ValorI";
                ViewState["SegmentoCabecera"] = _dtbsegmento;
                GrdvSegmento.DataSource = _dtbsegmento;
                GrdvSegmento.DataBind();
                TxtEtiqueta.Text = "";
                TxtColor.Text = "";
                TxtValorInicial.Text = "";
                TxtValorFinal.Text = "";
                ImgAdd.Enabled = true;
                ImgModi.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void GrdvSegmento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _color = GrdvSegmento.DataKeys[e.Row.RowIndex].Values["auxv1"].ToString();
                    e.Row.Cells[3].ForeColor = System.Drawing.ColorTranslator.FromHtml(_color);
                }
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

                foreach (GridViewRow _fr in GrdvSegmento.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvSegmento.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Beige;
                ViewState["Codigo"] = int.Parse(GrdvSegmento.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _dtbsegmento = (DataTable)ViewState["SegmentoCabecera"];
                _result = _dtbsegmento.Select("Codigo='" + ViewState["Codigo"].ToString() + "'").FirstOrDefault();
                TxtEtiqueta.Text = _result["Descripcion"].ToString();
                TxtValorInicial.Text = _result["ValorI"].ToString();
                TxtValorFinal.Text = _result["ValorF"].ToString();
                TxtColor.Text = _result["auxv1"].ToString();
                ImgAdd.Enabled = false;
                ImgModi.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                ViewState["Codigo"] = int.Parse(GrdvSegmento.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _dtbsegmento = (DataTable)ViewState["SegmentoCabecera"];
                _result = _dtbsegmento.Select("Codigo='" + ViewState["Codigo"].ToString() + "'").FirstOrDefault();
                _result.Delete();
                _dtbsegmento.AcceptChanges();
                ViewState["SegmentoCabecera"] = _dtbsegmento;
                GrdvSegmento.DataSource = _dtbsegmento;
                GrdvSegmento.DataBind();
                new CedenteDAO().FunDelSegmento(int.Parse(ViewState["CodigoCEDE"].ToString()), int.Parse(ViewState["CodigoCPCE"].ToString()),
                    int.Parse(ViewState["Codigo"].ToString()), 1);
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
                if (ViewState["CodigoCEDE"].ToString() == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this);
                    return;
                }

                if (ViewState["CodigoCPCE"].ToString() == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Producto/Catálogo..!", this);
                    return;
                }

                _dtbsegmento = (DataTable)ViewState["SegmentoCabecera"];

                if (_dtbsegmento.Rows.Count == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("No Existen Registros Ingresados..!", this);
                    return;
                }

                _dtbsegmento = (DataTable)ViewState["SegmentoCabecera"];

                SoftCob_SEGMENTO_CABECERA _datos = new SoftCob_SEGMENTO_CABECERA();

                foreach (DataRow _dr in _dtbsegmento.Rows)
                {
                    _codigo = new CedenteDAO().FunGetCodigoSegmento(int.Parse(ViewState["CodigoCEDE"].ToString()),
                        int.Parse(ViewState["CodigoCPCE"].ToString()), int.Parse(_dr[0].ToString()), 1);

                    _datos.SGCA_CODIGO = _codigo;
                    _datos.sgca_cedecodigo = int.Parse(ViewState["CodigoCEDE"].ToString());
                    _datos.sgca_cpcecodigo = int.Parse(ViewState["CodigoCPCE"].ToString());
                    _datos.sgca_segmento = _dr["Segmento"].ToString();
                    _datos.sgca_descripcion = _dr["Descripcion"].ToString();
                    _datos.sgca_valorinicial = int.Parse(_dr["ValorI"].ToString());
                    _datos.sgca_valorfinal = int.Parse(_dr["ValorF"].ToString());
                    _datos.sgca_estado = _dr["Estado"].ToString() == "Activo" ? true : false;
                    _datos.sgca_auxv1 = _dr["auxv1"].ToString();
                    _datos.sgca_auxv2 = _dr["auxv2"].ToString();
                    _datos.sgca_auxv3 = _dr["auxv3"].ToString();
                    _datos.sgca_auxi1 = int.Parse(_dr["auxi1"].ToString());
                    _datos.sgca_auxi2 = int.Parse(_dr["auxi2"].ToString());
                    _datos.sgca_auxi3 = int.Parse(_dr["auxi3"].ToString());
                    _datos.sgca_fum = DateTime.Now;
                    _datos.sgca_uum = int.Parse(Session["usuCodigo"].ToString());
                    _datos.sgca_tum = Session["MachineName"].ToString();
                    _datos.sgca_fechacreacion = DateTime.Now;
                    _datos.sgca_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                    _datos.sgca_terminalcreacion = Session["MachineName"].ToString();

                    if (_codigo == 0) new CedenteDAO().FunCrearSegmento(_datos);
                    else new CedenteDAO().FunEditSegmento(_datos);
                }

                _redirect = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Grabado con Éxito..!");
                Response.Redirect(_redirect, true);
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