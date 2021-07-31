namespace SoftCob.Views.ArbolDesicion
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ArbolDecisionNew : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtbaccion = new DataTable();
        DataTable _dtbefecto = new DataTable();
        DataTable _dtbrespuesta = new DataTable();
        DataTable _dtbcontacto = new DataTable();
        string[] _pathroot;
        DataRow _resultado;
        string response = "", _codigocpce = "", _namecpce = "", _codigoarac = "", _namearac = "", _codigoaref = "",
            _namearef = "", _codigoarre = "", _namearre = "", _codigoarco = "", _mensaje = "", _namearco = "", _mensajes = "";
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
                    ViewState["CodigoCPCEaux"] = "";
                    Lbltitulo.Text = "Administrar Arbol Decision";
                    TrvCedenteArbol.Nodes.Clear();
                    TreeNode node = new TreeNode("Cedente-Arbol", "0");
                    node = FunLlenarCiudadCedente(node);
                    TrvCedenteArbol.Nodes.Add(node);
                    TrvCedenteArbol.CollapseAll();

                    if (Request["MensajeRetornado"] != null)
                    {
                        _mensajes = Request["MensajeRetornado"];
                        ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                            "'top-center'); alertify.success('" + _mensajes + "', 5, function(){console.log('dismissed');});", true);
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
        private TreeNode FunLlenarCiudadCedente(TreeNode node)
        {
            try
            {
                _dts = new CedenteDAO().FunGetCiuadesCedentes();

                if (_dts != null && node != null)
                {
                    foreach (DataRow _drfila in _dts.Tables[0].Rows)
                    {
                        TreeNode unnode = new TreeNode(_drfila["Descripcion"].ToString(), _drfila["Codigo"].ToString());
                        unnode = FunCedentesCatalogo(unnode, int.Parse(_drfila["Codigo"].ToString()));
                        node.ChildNodes.Add(unnode);
                    }
                    TrvCedenteArbol.CollapseAll();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return node;
        }

        private TreeNode FunCedentesCatalogo(TreeNode node, int ciudadcod)
        {
            try
            {
                _dts = new CedenteDAO().FunGetCatalogoPorCiudad(ciudadcod);

                if (_dts != null && _dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _drFila in _dts.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(_drFila["Descripcion"].ToString(), _drFila["Codigo"].ToString());
                        unNode = FunArbolAccion(unNode, int.Parse(_drFila["Codigo"].ToString()), int.Parse(_drFila["Nivel"].ToString()));
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

        private TreeNode FunArbolAccion(TreeNode node, int cpcecodigo, int nivel)
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(16, cpcecodigo, 0, 1, "", "", "", Session["Conectar"].ToString());

                if (_dts != null && _dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _drfila in _dts.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(_drfila["Descripcion"].ToString(), _drfila["Codigo"].ToString());
                        unNode = FunArbolEfecto(unNode, int.Parse(_drfila["Codigo"].ToString()), cpcecodigo, nivel);
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

        private TreeNode FunArbolEfecto(TreeNode node, int araccodigo, int cpcecodigo, int nivel)
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(17, araccodigo, cpcecodigo, 1, "", "", "", Session["Conectar"].ToString());

                if (_dts != null && _dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _drfila in _dts.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(_drfila["Descripcion"].ToString(), _drfila["Codigo"].ToString());
                        unNode = FunArbolRespuesta(unNode, int.Parse(_drfila["Codigo"].ToString()), cpcecodigo, nivel);
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

        private TreeNode FunArbolRespuesta(TreeNode node, int arefcodigo, int cpcecodigo, int nivel)
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(18, arefcodigo, cpcecodigo, 1, "", "", "", Session["Conectar"].ToString());

                if (_dts != null && _dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _drfila in _dts.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(_drfila["Descripcion"].ToString(), _drfila["Codigo"].ToString());
                        if (nivel == 4)
                        {
                            unNode = FunArbolContacto(unNode, int.Parse(_drfila["Codigo"].ToString()), cpcecodigo);
                        }
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

        private TreeNode FunArbolContacto(TreeNode node, int arrecodigo, int cpcecodigo)
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(19, arrecodigo, cpcecodigo, 1, "", "", "", Session["Conectar"].ToString());

                if (_dts != null && _dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _drfila in _dts.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(_drfila["Descripcion"].ToString(), _drfila["Codigo"].ToString());
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

        private void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(182, int.Parse(ViewState["CodigoCPCE"].ToString()),
                    1, 0, "", "", "", Session["Conectar"].ToString());

                ViewState["ArbolAccion"] = _dts.Tables[0];
                ViewState["ArbolEfecto"] = _dts.Tables[1];
                ViewState["ArbolRespuesta"] = _dts.Tables[2];
                ViewState["ArbolContacto"] = _dts.Tables[3];

                //if (ViewState["CodigoCPCEaux"].ToString() != ViewState["CodigoCPCE"].ToString())
                //{
                //    ViewState["ArbolAccion"] = null;
                //    ViewState["ArbolEfecto"] = null;
                //    ViewState["ArbolRespuesta"] = null;
                //    ViewState["ArbolContacto"] = null;
                //}

                //if (ViewState["ArbolAccion"] == null)
                    

                //if (ViewState["ArbolEfecto"] == null)
                    

                //if (ViewState["ArbolRespuesta"] == null)
                    

                //if (ViewState["ArbolContacto"] == null)
                    

                //ViewState["CodigoCPCEaux"] = ViewState["CodigoCPCE"];

                _dts = new ConsultaDatosDAO().FunConsultaDatos(180, int.Parse(ViewState["CodigoCPCE"].ToString()),
                    0, 0, "", "", "", Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                    ViewState["Nivel"] = _dts.Tables[0].Rows[0]["Nivel"].ToString();
                else ViewState["Nivel"] = 4;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunActivarPorArbol(string arbol, string codigo)
        {
            try
            {
                PnlOpciones.Visible = true;
                ImgAdd.Visible = true;
                LblDescripcion.Visible = true;
                TxtDescripcion.Visible = true;
                LblArbol.Visible = false;
                TxtArbol.Visible = false;
                ImgModificar.Visible = false;
                ImgDelete.Visible = true;
                LblEstado.Visible = false;
                ChkEstado.Visible = false;
                ChkLlamar.Visible = false;
                ChkPago.Visible = false;
                ChkEfectivo.Visible = false;
                ChkComisiona.Visible = false;
                ChkContacto.Visible = false;
                ChkPago.Checked = false;
                ChkLlamar.Checked = false;
                ChkEfectivo.Checked = false;
                ChkEfectivo.Checked = false;
                switch (arbol)
                {
                    case "CEDENTE":
                        LblDescripcion.InnerText = "Accion:";
                        ChkContacto.Visible = true;
                        break;
                    case "ACCION":
                        LblArbol.InnerText = "Accion:";
                        LblDescripcion.InnerText = "Efecto:";
                        _dtbaccion = (DataTable)ViewState["ArbolAccion"];
                        _resultado = _dtbaccion.Select("CodigoCPCE='" + ViewState["CodigoCPCE"].ToString() +
                            "' and CodigoARAC ='" + codigo + "'").FirstOrDefault();
                        break;
                    case "EFECTO":
                        ChkPago.Visible = true;
                        ChkLlamar.Visible = true;
                        ChkEfectivo.Visible = true;
                        ChkComisiona.Visible = true;
                        LblArbol.InnerText = "Efecto:";
                        LblDescripcion.InnerText = "Respuesta:";
                        _dtbefecto = (DataTable)ViewState["ArbolEfecto"];
                        _resultado = _dtbefecto.Select("CodigoCPCE='" + ViewState["CodigoCPCE"].ToString() +
                            "' and CodigoAREF='" + codigo + "'").FirstOrDefault();
                        break;
                    case "RESPUESTA":
                        ChkPago.Visible = true;
                        LblArbol.InnerText = "Respuesta:";
                        LblDescripcion.InnerText = "Contacto:";
                        _dtbrespuesta = (DataTable)ViewState["ArbolRespuesta"];
                        _resultado = _dtbrespuesta.Select("CodigoCPCE='" + ViewState["CodigoCPCE"].ToString() +
                            "' and CodigoARRE='" + codigo + "'").FirstOrDefault();

                        if (ViewState["Nivel"].ToString() == "3")
                        {
                            LblDescripcion.Visible = false;
                            TxtDescripcion.Visible = false;
                            ImgAdd.Visible = false;
                        }

                        if (ViewState["Nivel"].ToString() == "4")
                        {
                            ChkPago.Visible = true;
                            ChkLlamar.Visible = true;
                            ChkEfectivo.Visible = true;
                            ChkComisiona.Visible = true;
                            ChkPago.Checked = _resultado["Pago"].ToString() == "SI" ? true : false;
                            ChkLlamar.Checked = _resultado["Llamar"].ToString() == "SI" ? true : false;
                            ChkEfectivo.Checked = _resultado["Efectivo"].ToString() == "SI" ? true : false;
                            ChkEfectivo.Text = _resultado["Efectivo"].ToString() == "SI" ? "Efectivo" : "No Efectivo";
                            ChkComisiona.Checked = _resultado["Comisiona"].ToString() == "SI" ? true : false;
                            ChkContacto.Visible = true;
                        }

                        break;
                    case "CONTACTO":
                        LblArbol.InnerText = "Contacto:";
                        ImgAdd.Visible = false;
                        LblDescripcion.Visible = false;
                        TxtDescripcion.Visible = false;
                        _dtbcontacto = (DataTable)ViewState["ArbolContacto"];
                        _resultado = _dtbcontacto.Select("CodigoCPCE='" + ViewState["CodigoCPCE"].ToString() +
                            "' and CodigoARCO='" + codigo + "'").FirstOrDefault();
                        ChkContacto.Checked = _resultado["Pago"].ToString() == "SI" ? true : false;
                        ChkContacto.Text = _resultado["Pago"].ToString() == "SI" ? "Directo" : "Indirecto";
                        ChkContacto.Visible = true;
                        break;
                }

                if (_resultado != null)
                {
                    if (_resultado["Nuevo"].ToString() == "NO")
                    {
                        LblArbol.Visible = true;
                        TxtArbol.Visible = true;
                        TxtArbol.Text = _resultado["Descripcion"].ToString();
                        ImgModificar.Visible = true;
                        LblEstado.Visible = true;
                        ChkEstado.Visible = true;
                        ChkEstado.Checked = _resultado["Estado"].ToString() == "Activo" ? true : false;
                        ChkEstado.Text = _resultado["Estado"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void TrvCedenteArbol_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                TreeView treepress = (TreeView)(sender);
                TreeNode node = treepress.SelectedNode;
                PnlOpciones.Visible = false;
                ChkPago.Visible = false;
                ChkLlamar.Visible = false;
                ChkEfectivo.Visible = false;
                ChkComisiona.Visible = false;
                LblEstado.Visible = false;
                ChkEstado.Visible = false;
                TxtDescripcion.Text = "";

                    switch (node.Depth)
                {
                    case 0:
                        _pathroot = node.ValuePath.Split(new char[] { '/' });
                        PnlOpciones.Visible = true;
                        break;
                    case 1:
                        _pathroot = node.ValuePath.Split(new char[] { '/' });
                        ViewState["CodigoPadre"] = _pathroot[1].ToString();
                        break;
                    case 2:
                        _pathroot = node.ValuePath.Split(new char[] { '/' });
                        ViewState["CodigoArbol"] = "CEDENTE";
                        ViewState["CodigoCPCE"] = _pathroot[2].ToString();
                        LblDefinicion.InnerText = node.Text;
                        ChkContacto.Visible = true;
                        FunCargarMantenimiento();
                        FunActivarPorArbol(ViewState["CodigoArbol"].ToString(), ViewState["CodigoCPCE"].ToString());
                        break;
                    case 3:
                        _pathroot = node.ValuePath.Split(new char[] { '/' });
                        ViewState["CodigoArbol"] = "ACCION";
                        ViewState["CodigoCPCE"] = _pathroot[2].ToString();
                        ViewState["CodigoARAC"] = _pathroot[3].ToString();
                        FunCargarMantenimiento();
                        LblDefinicion.InnerText = node.Text;
                        FunActivarPorArbol(ViewState["CodigoArbol"].ToString(), ViewState["CodigoARAC"].ToString());
                        break;
                    case 4:
                        _pathroot = node.ValuePath.Split(new char[] { '/' });
                        ViewState["CodigoArbol"] = "EFECTO";
                        ViewState["CodigoCPCE"] = _pathroot[2].ToString();
                        ViewState["CodigoARAC"] = _pathroot[3].ToString();
                        ViewState["CodigoAREF"] = _pathroot[4].ToString();
                        FunCargarMantenimiento();
                        LblDefinicion.InnerText = node.Text;
                        FunActivarPorArbol(ViewState["CodigoArbol"].ToString(), ViewState["CodigoAREF"].ToString());
                        break;
                    case 5:
                        _pathroot = node.ValuePath.Split(new char[] { '/' });
                        ViewState["CodigoArbol"] = "RESPUESTA";
                        ViewState["CodigoCPCE"] = _pathroot[2].ToString();
                        ViewState["CodigoARAC"] = _pathroot[3].ToString();
                        ViewState["CodigoAREF"] = _pathroot[4].ToString();
                        ViewState["CodigoARRE"] = _pathroot[5].ToString();
                        FunCargarMantenimiento();
                        LblDefinicion.InnerText = node.Text;
                        FunActivarPorArbol(ViewState["CodigoArbol"].ToString(), ViewState["CodigoARRE"].ToString());
                        break;
                    case 6:
                        _pathroot = node.ValuePath.Split(new char[] { '/' });
                        ViewState["CodigoArbol"] = "CONTACTO";
                        ViewState["CodigoCPCE"] = _pathroot[2].ToString();
                        ViewState["CodigoARAC"] = _pathroot[3].ToString();
                        ViewState["CodigoAREF"] = _pathroot[4].ToString();
                        ViewState["CodigoARRE"] = _pathroot[5].ToString();
                        ViewState["CodigoARCO"] = _pathroot[6].ToString();
                        FunCargarMantenimiento();
                        LblDefinicion.InnerText = node.Text;
                        FunActivarPorArbol(ViewState["CodigoArbol"].ToString(), ViewState["CodigoARCO"].ToString());
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
                if (string.IsNullOrEmpty(TxtDescripcion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Descripcion..!", this);
                    return;
                }

                switch (ViewState["CodigoArbol"].ToString())
                {
                    case "CEDENTE":
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(181, 1, int.Parse(ViewState["CodigoCPCE"].ToString()), 0,
                            TxtDescripcion.Text.Trim().ToUpper(), "", "", Session["Conectar"].ToString());

                        if (_dts.Tables[0].Rows.Count > 0)                        
                        {
                            new FuncionesDAO().FunShowJSMessage("Descripcion ACCION ya existe..!", this);
                            return;
                        }

                        _dts = new ArbolDecisionDAO().FunNewArbolDecision(0, 0, int.Parse(ViewState["CodigoCPCE"].ToString()), 0, 0, 0,
                            0, TxtDescripcion.Text.Trim().ToUpper(), "Activo", ChkContacto.Checked ? "SI" : "NO", "", "", "", "", "", "",
                            "", "", "", 0, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                            Session["Conectar"].ToString());

                        TreeNode unnodeaccion = new TreeNode(_dts.Tables[0].Rows[0]["Descripcion"].ToString(),
                            _dts.Tables[0].Rows[0]["Codigo"].ToString());
                        TrvCedenteArbol.SelectedNode.ChildNodes.Add(unnodeaccion);
                        break;
                    case "ACCION":
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(181, 2, int.Parse(ViewState["CodigoCPCE"].ToString()),
                            int.Parse(ViewState["CodigoARAC"].ToString()), TxtDescripcion.Text.Trim().ToUpper(), "", "",
                            Session["Conectar"].ToString());

                        if (_dts.Tables[0].Rows.Count > 0)
                        {
                            new FuncionesDAO().FunShowJSMessage("Descripcion EFECTO ya existe..!", this);
                            return;
                        }

                        _dts = new ArbolDecisionDAO().FunNewArbolDecision(1, 0, int.Parse(ViewState["CodigoCPCE"].ToString()),
                            int.Parse(ViewState["CodigoARAC"].ToString()), 0, 0, 0, TxtDescripcion.Text.Trim().ToUpper(), "Activo",
                            ChkContacto.Checked ? "SI" : "NO", "", "", "", "", "", "", "", "", "", 0, 0, 0, 0, 0, 0, "",
                            Session["Conectar"].ToString());

                        TreeNode unnodeafecto = new TreeNode(TxtDescripcion.Text.Trim().ToUpper(), 
                            _dts.Tables[0].Rows[0]["Codigo"].ToString());
                        TrvCedenteArbol.SelectedNode.ChildNodes.Add(unnodeafecto);
                        break;
                    case "EFECTO":
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(181, 3, int.Parse(ViewState["CodigoCPCE"].ToString()),
                            int.Parse(ViewState["CodigoAREF"].ToString()), TxtDescripcion.Text.Trim().ToUpper(), "", "",
                            Session["Conectar"].ToString());

                        if (_dts.Tables[0].Rows.Count > 0)
                        {
                            new FuncionesDAO().FunShowJSMessage("Descripcion RESPUESTA ya existe..!", this);
                            return;
                        }

                        _dts = new ArbolDecisionDAO().FunNewArbolDecision(2, 0, int.Parse(ViewState["CodigoCPCE"].ToString()),
                            0, int.Parse(ViewState["CodigoAREF"].ToString()), 0, 0, TxtDescripcion.Text.Trim().ToUpper(), "Activo",
                            "", ChkPago.Checked ? "SI" : "NO", ChkLlamar.Checked ? "SI" : "NO", ChkEfectivo.Checked ? "SI" : "NO",
                            ChkComisiona.Checked ? "SI" : "NO", "", "", "", "", "", 0, 0, 0, 0, 0, 0, "", Session["Conectar"].ToString());

                        TreeNode unnoderespuesta = new TreeNode(TxtDescripcion.Text.Trim().ToUpper(), 
                            _dts.Tables[0].Rows[0]["Codigo"].ToString());
                        TrvCedenteArbol.SelectedNode.ChildNodes.Add(unnoderespuesta);
                        break;
                    case "RESPUESTA":
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(181, 4, int.Parse(ViewState["CodigoCPCE"].ToString()),
                            int.Parse(ViewState["CodigoARRE"].ToString()), TxtDescripcion.Text.Trim().ToUpper(), "", "",
                            Session["Conectar"].ToString());

                        if (_dts.Tables[0].Rows.Count > 0)
                        {
                            new FuncionesDAO().FunShowJSMessage("Descripcion CONTACTO ya existe..!", this);
                            return;
                        }

                        _dts = new ArbolDecisionDAO().FunNewArbolDecision(3, 0, int.Parse(ViewState["CodigoCPCE"].ToString()),
                            0, 0, int.Parse(ViewState["CodigoARRE"].ToString()), 0, TxtDescripcion.Text.Trim().ToUpper(), "Activo",
                            "", ChkContacto.Checked ? "SI" : "NO", "", "", "", "", "", "", "", "", 0, 0, 0, 0, 0, 0, "",
                            Session["Conectar"].ToString());

                        TreeNode unnodecontacto = new TreeNode(TxtDescripcion.Text.Trim().ToUpper(), _dts.Tables[0].Rows[0]["Codigo"].ToString());
                        TrvCedenteArbol.SelectedNode.ChildNodes.Add(unnodecontacto);
                        break;
                }

                TxtDescripcion.Text = "";
                ChkPago.Checked = false;
                ChkLlamar.Checked = false;
                ChkEfectivo.Checked = false;
                ChkEfectivo.Text = "No Efectivo";

                //_dts = new ConsultaDatosDAO().FunConsultaDatos(182, int.Parse(ViewState["CodigoCPCE"].ToString()),
                //    1, 0, "", "", "", Session["Conectar"].ToString());

                //if (ViewState["ArbolAccion"] == null)
                //    ViewState["ArbolAccion"] = _dts.Tables[0];

                //if (ViewState["ArbolEfecto"] == null)
                //    ViewState["ArbolEfecto"] = _dts.Tables[1];

                //if (ViewState["ArbolRespuesta"] == null)
                //    ViewState["ArbolRespuesta"] = _dts.Tables[2];

                //if (ViewState["ArbolContacto"] == null)
                //    ViewState["ArbolContacto"] = _dts.Tables[3];
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
                switch (ViewState["CodigoArbol"].ToString())
                {
                    case "ACCION":
                        _dts = new ConsultaDatosDAO().FunNewArbolDecision(0, 0, int.Parse(ViewState["CodigoCPCE"].ToString()),
                            int.Parse(ViewState["CodigoARAC"].ToString()), 0, 0, 0, TxtArbol.Text.Trim().ToUpper(),
                            ChkEstado.Checked ? "Activo" : "Inactivo", ChkContacto.Checked ? "SI" : "NO", "", "", "", "", "", "", "", "", "",
                            1, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                            Session["Conectar"].ToString());
                        break;
                    case "EFECTO":
                        _dts = new ConsultaDatosDAO().FunNewArbolDecision(1, 0, int.Parse(ViewState["CodigoCPCE"].ToString()),
                            0, int.Parse(ViewState["CodigoAREF"].ToString()), 0, 0, TxtArbol.Text.Trim().ToUpper(),
                            ChkEstado.Checked ? "Activo" : "Inactivo", "", "", "", "", "", "", "", "", "",
                            "", 1, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                            Session["Conectar"].ToString());
                        break;
                    case "RESPUESTA":
                        _dts = new ConsultaDatosDAO().FunNewArbolDecision(2, 0, int.Parse(ViewState["CodigoCPCE"].ToString()),
                            0, 0, int.Parse(ViewState["CodigoARRE"].ToString()), 0, TxtArbol.Text.Trim().ToUpper(),
                            ChkEstado.Checked ? "Activo" : "Inactivo", "", ChkPago.Checked ? "SI" : "NO",
                            ChkLlamar.Checked ? "SI" : "NO", ChkEfectivo.Checked ? "SI" : "NO", ChkComisiona.Checked ? "SI" : "NO",
                            "", "", "", "", "", 1, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                            Session["Conectar"].ToString());
                        break;
                    case "CONTACTO":
                        _dts = new ConsultaDatosDAO().FunNewArbolDecision(3, 0, int.Parse(ViewState["CodigoCPCE"].ToString()),
                            0, 0, 0, int.Parse(ViewState["CodigoARRE"].ToString()), TxtArbol.Text.Trim().ToUpper(),
                            ChkEstado.Checked ? "Activo" : "Inactivo", "", ChkContacto.Checked ? "SI" : "NO", "", "", "",
                            "", "", "", "", "", 1, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                            Session["MachineName"].ToString(), Session["Conectar"].ToString());
                        break;
                }

                response = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");
                Response.Redirect(response, false);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ViewState["CodigoArbol"].ToString() == "CEDENTE")
                {
                    new FuncionesDAO().FunShowJSMessage("No se puede eliminar Cedente..!", this, "E", "C");
                    return;
                }

                //_dtbaccion = (DataTable)ViewState["ArbolAccion"];
                //_dtbefecto = (DataTable)ViewState["ArbolEfecto"];
                //_dtbrespuesta = (DataTable)ViewState["ArbolRespuesta"];
                //_dtbcontacto = (DataTable)ViewState["ArbolContacto"];

                if (ViewState["CodigoArbol"].ToString() == "ACCION")
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(122, 0, int.Parse(ViewState["CodigoARAC"].ToString()), 0,
                        "", "", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("No se puede Eliminar, existen gestiones registradas..!", this, "E", "C");
                        return;
                    }

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(142, 0, int.Parse(ViewState["CodigoARAC"].ToString()), 0,
                        "", "", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Elimine primero los Efectos Registrados..!", this, "E", "C");
                        return;
                    }
                }

                if (ViewState["CodigoArbol"].ToString() == "EFECTO")
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(122, 1, int.Parse(ViewState["CodigoAREF"].ToString()), 0,
                        "", "", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("No se puede Eliminar, existen gestiones registradas..!", this, "E", "C");
                        return;
                    }

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(142, 1, int.Parse(ViewState["CodigoAREF"].ToString()), 0,
                        "", "", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Elimine primero las Respuestas Registradas..!", this, "E", "C");
                        return;
                    }
                }

                if (ViewState["CodigoArbol"].ToString() == "RESPUESTA")
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(122, 2, int.Parse(ViewState["CodigoARRE"].ToString()), 0,
                        "", "", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("No se puede Eliminar, existen gestiones registradas..!", this, "E", "C");
                        return;
                    }

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(142, 2, int.Parse(ViewState["CodigoARRE"].ToString()), 0,
                        "", "", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Elimine primero los Contacto Registrados..!", this, "E", "C");
                        return;
                    }
                }

                if (ViewState["CodigoArbol"].ToString() == "CONTACTO")
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(122, 3, int.Parse(ViewState["CodigoARCO"].ToString()), 0,
                        "", "", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("No se puede Eliminar, existen gestiones registradas..!", this, "E", "C");
                        return;
                    }
                }

                foreach (TreeNode node in TrvCedenteArbol.Nodes[0].ChildNodes)
                {
                    foreach (TreeNode childcpce in node.ChildNodes)
                    {
                        _codigocpce = childcpce.Value;
                        _namecpce = childcpce.Text;

                        if (_codigocpce == ViewState["CodigoCPCE"].ToString())
                        {
                            foreach (TreeNode childaccion in childcpce.ChildNodes)
                            {
                                _namearac = childaccion.Text;
                                _codigoarac = childaccion.Value;
                                if (_codigoarac == ViewState["CodigoARAC"].ToString())
                                {
                                    if (ViewState["CodigoArbol"].ToString() == "ACCION")
                                    {
                                        childcpce.ChildNodes.Remove(TrvCedenteArbol.SelectedNode);
                                        _mensaje = new ArbolDecisionDAO().FunDelAccion(int.Parse(ViewState["CodigoARAC"].ToString()),
                                            int.Parse(ViewState["CodigoCPCE"].ToString()));
                                        break;
                                    }

                                    foreach (TreeNode childefecto in childaccion.ChildNodes)
                                    {
                                        _codigoaref = childefecto.Value;
                                        _namearef = childefecto.Text;
                                        if (_codigoaref == ViewState["CodigoAREF"].ToString())
                                        {
                                            if (ViewState["CodigoArbol"].ToString() == "EFECTO")
                                            {
                                                childaccion.ChildNodes.Remove(TrvCedenteArbol.SelectedNode);
                                                _mensaje = new ArbolDecisionDAO().FunDelEfecto(int.Parse(ViewState["CodigoAREF"].ToString()),
                                                    int.Parse(ViewState["CodigoCPCE"].ToString()));
                                                break;
                                            }

                                            foreach (TreeNode childrespuesta in childefecto.ChildNodes)
                                            {
                                                _codigoarre = childrespuesta.Value;
                                                _namearre = childrespuesta.Text;

                                                if (_codigoarre == ViewState["CodigoARRE"].ToString())
                                                {
                                                    if (ViewState["CodigoArbol"].ToString() == "RESPUESTA")
                                                    {
                                                        childefecto.ChildNodes.Remove(TrvCedenteArbol.SelectedNode);
                                                        _mensaje = new ArbolDecisionDAO().FunDelRespuesta(int.Parse(ViewState["CodigoARRE"].ToString()),
                                                            int.Parse(ViewState["CodigoCPCE"].ToString()));
                                                        break;
                                                    }
                                                }

                                                foreach (TreeNode childcontacto in childrespuesta.ChildNodes)
                                                {
                                                    _codigoarco = childcontacto.Value;
                                                    _namearco = childcontacto.Text;

                                                    if (_codigoarco == ViewState["CodigoARCO"].ToString())
                                                    {
                                                        if (ViewState["CodigoArbol"].ToString() == "CONTACTO")
                                                        {
                                                            childrespuesta.ChildNodes.Remove(TrvCedenteArbol.SelectedNode);
                                                            _mensaje = new ArbolDecisionDAO().FunDelContacto(int.Parse(ViewState["CodigoARCO"].ToString()),
                                                                int.Parse(ViewState["CodigoCPCE"].ToString()));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                ImgDelete.Visible = false;
                LblDefinicion.InnerText = "";
                PnlOpciones.Visible = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ChkPago_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkPago.Checked) ChkLlamar.Checked = false;
        }

        protected void ChkLlamar_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkLlamar.Checked) ChkPago.Checked = false;
        }

        protected void ChkContacto_CheckedChanged(object sender, EventArgs e)
        {
            ChkContacto.Text = ChkContacto.Checked ? "Directo" : "Indirecto";
        }
        protected void ChkEfectivo_CheckedChanged(object sender, EventArgs e)
        {
            ChkEfectivo.Text = ChkEfectivo.Checked ? "Efectivo" : "No Efectivo";
        }

        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstado.Text = ChkEstado.Checked ? "Activo" : "Inactivo";
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    _dtbaccion = (DataTable)ViewState["ArbolAccion"];
            //    _dtbefecto = (DataTable)ViewState["ArbolEfecto"];
            //    _dtbrespuesta = (DataTable)ViewState["ArbolRespuesta"];
            //    _dtbcontacto = (DataTable)ViewState["ArbolContacto"];

            //    foreach (DataRow _draccion in _dtbaccion.Rows)
            //    {
            //        _codigocpce = _draccion["CodigoCPCE"].ToString();
            //        _codigoarac = _draccion["CodigoARAC"].ToString();
            //        _descripcion = _draccion["Descripcion"].ToString();
            //        _estado = _draccion["Estado"].ToString();

            //        _dts = new ConsultaDatosDAO().FunNewArbolDecision(0, int.Parse(_codigocpce), int.Parse(_codigoarac),
            //            0, 0, 0, _descripcion, _estado, "", 0, "", "", "", "", "", "", 0, 0, 0, 0, 0,
            //            int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
            //            Session["Conectar"].ToString());

            //        _codigoaracnew = _dts.Tables[0].Rows[0]["CodigoARAC"].ToString();
            //        _efecto = _dtbefecto.Select("CodigoARAC='" + _codigoarac + "'");

            //        foreach (DataRow drefecto in _efecto)
            //        {
            //            _codigoaref = drefecto["CodigoAREF"].ToString();
            //            _descripcion = drefecto["Descripcion"].ToString();
            //            _estado = drefecto["Estado"].ToString();

            //            _dts = new ConsultaDatosDAO().FunNewArbolDecision(1, int.Parse(_codigocpce), int.Parse(_codigoaracnew),
            //                int.Parse(_codigoaref), 0, 0, _descripcion, _estado, "", 0, "", "", "", "", "", "", 0, 0, 0, 0, 0,
            //                int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
            //                Session["Conectar"].ToString());

            //            _codigoarefnew = _dts.Tables[0].Rows[0]["CodigoAREF"].ToString();
            //            _respuesta = _dtbrespuesta.Select("CodigoAREF='" + _codigoaref + "'");

            //            foreach (DataRow drrespuesta in _respuesta)
            //            {
            //                _codigoarre = drrespuesta["CodigoARRE"].ToString();
            //                _descripcion = drrespuesta["Descripcion"].ToString();
            //                _estado = drrespuesta["Estado"].ToString();
            //                _efectivo = drrespuesta["Efectivo"].ToString();
            //                _tiporespuesta = drrespuesta["Tipo"].ToString();
            //                _comisiona = drrespuesta["Comisiona"].ToString();

            //                _dts = new ConsultaDatosDAO().FunNewArbolDecision(2, int.Parse(_codigocpce), int.Parse(_codigoaracnew),
            //                    int.Parse(_codigoarefnew), int.Parse(_codigoarre), 0, _descripcion, _estado, _efectivo,
            //                    int.Parse(_tiporespuesta), _comisiona, "", "", "", "", "", 0, 0, 0, 0, 0,
            //                    int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
            //                    Session["Conectar"].ToString());

            //                _codigoarrenew = _dts.Tables[0].Rows[0]["CodigoARRE"].ToString();
            //                _contacto = _dtbcontacto.Select("CodigoARRE='" + _codigoarre + "'");

            //                foreach (DataRow drcontacto in _contacto)
            //                {
            //                    _codigoarco = drcontacto["CodigoARCO"].ToString();
            //                    _descripcion = drcontacto["Descripcion"].ToString();
            //                    _estado = drcontacto["Estado"].ToString();

            //                    _dts = new ConsultaDatosDAO().FunNewArbolDecision(3, int.Parse(_codigocpce), int.Parse(_codigoaracnew), 
            //                        int.Parse(_codigoarefnew), int.Parse(_codigoarrenew), int.Parse(_codigoarco), _descripcion, _estado,
            //                        "", 0, "", "", "", "", "", "", 0, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), 
            //                        Session["MachineName"].ToString(), Session["Conectar"].ToString());
            //                }
            //            }
            //        }
            //    }
            //    response = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");
            //    Response.Redirect(response, false);
            //}
            //catch (Exception ex)
            //{
            //    Lblerror.Text = ex.ToString();
            //}
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
        }
        #endregion
    }
}