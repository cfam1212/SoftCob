namespace SoftCob.Views.ArbolDesicion
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
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
        int _maxcodigo = 0;
        string[] _pathroot;
        DataRow _resultado;
        DataRow[] _result, _efecto, _respuesta, _contacto;
        string response = "", _codigocpce = "", _namecpce = "", _codigoarac = "", _namearac = "", _codigoaref = "",
            _namearef = "", _codigoarre = "", _namearre = "", _codigoarco = "", _namearco = "", _descripcion = "",
            _estado = "", _codigoaracnew = "", _codigoarefnew = "", _codigoarrenew = "",
            _efectivo = "", _tiporespuesta = "", _comisiona = "";
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
                    Session["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                    ViewState["CodigoCPCEaux"] = "";
                    Lbltitulo.Text = "Administrar Arbol Decision";
                    TrvCedenteArbol.Nodes.Clear();
                    TreeNode node = new TreeNode("Cedente-Arbol", "0");
                    node = FunLlenarCiudadCedente(node);
                    TrvCedenteArbol.Nodes.Add(node);
                    TrvCedenteArbol.CollapseAll();

                    if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());
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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(16, cpcecodigo, 0, 0, "", "", "", Session["Conectar"].ToString());

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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(17, araccodigo, cpcecodigo, 0, "", "", "", Session["Conectar"].ToString());

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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(18, arefcodigo, cpcecodigo, 0, "", "", "", Session["Conectar"].ToString());

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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(19, arrecodigo, cpcecodigo, 0, "", "", "", Session["Conectar"].ToString());

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
                    0, 0, "", "", "", Session["Conectar"].ToString());

                if (ViewState["CodigoCPCEaux"].ToString() != ViewState["CodigoCPCE"].ToString())
                {
                    ViewState["ArbolAccion"] = null;
                    ViewState["ArbolEfecto"] = null;
                    ViewState["ArbolRespuesta"] = null;
                    ViewState["ArbolContacto"] = null;
                }

                if (ViewState["ArbolAccion"] == null)
                    ViewState["ArbolAccion"] = _dts.Tables[0];

                if (ViewState["ArbolEfecto"] == null)
                    ViewState["ArbolEfecto"] = _dts.Tables[1];

                if (ViewState["ArbolRespuesta"] == null)
                    ViewState["ArbolRespuesta"] = _dts.Tables[2];

                if (ViewState["ArbolContacto"] == null)
                    ViewState["ArbolContacto"] = _dts.Tables[3];

                ViewState["CodigoCPCEaux"] = ViewState["CodigoCPCE"];

                _dts = new ConsultaDatosDAO().FunConsultaDatos(180, int.Parse(ViewState["CodigoCPCE"].ToString()),
                    0, 0, "", "", "", Session["Conectar"].ToString());
                ViewState["Nivel"] = _dts.Tables[0].Rows[0]["Nivel"].ToString();
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
                ImgDelete.Visible = false;
                LblEstado.Visible = false;
                ChkEstado.Visible = false;
                ChkLlamar.Visible = false;
                ChkPago.Visible = false;
                ChkContacto.Visible = false;
                ChkComisiona.Visible = false;
                ChkPago.Checked = false;
                ChkLlamar.Checked = false;
                ChkContacto.Checked = false;
                ChkContacto.Checked = false;
                switch (arbol)
                {
                    case "CEDENTE":
                        LblDescripcion.InnerText = "Accion:";
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
                        ChkContacto.Visible = true;
                        ChkComisiona.Visible = true;
                        LblArbol.InnerText = "Efecto:";
                        LblDescripcion.InnerText = "Respuesta:";
                        _dtbefecto = (DataTable)ViewState["ArbolEfecto"];
                        _resultado = _dtbefecto.Select("CodigoCPCE='" + ViewState["CodigoCPCE"].ToString() +
                            "' and CodigoAREF='" + codigo + "'").FirstOrDefault();
                        break;
                    case "RESPUESTA":
                        LblArbol.InnerText = "Respuesta:";
                        LblDescripcion.InnerText = "Contacto:";
                        _dtbrespuesta = (DataTable)ViewState["ArbolRespuesta"];
                        _resultado = _dtbrespuesta.Select("CodigoCPCE='" + ViewState["CodigoCPCE"].ToString() +
                            "' and CodigoARRE='" + codigo + "'").FirstOrDefault();

                        if (_resultado != null)
                        {
                            if (_resultado["Nuevo"].ToString() == "NO" && ViewState["Nivel"].ToString() == "4")
                            {
                                ChkPago.Visible = true;
                                ChkLlamar.Visible = true;
                                ChkContacto.Visible = true;
                                ChkComisiona.Visible = true;
                                ChkPago.Checked = _resultado["Tipo"].ToString() == "1" ? true : false;
                                ChkLlamar.Checked = _resultado["Tipo"].ToString() == "2" ? true : false;
                                ChkContacto.Checked = _resultado["Efectivo"].ToString() == "1" ? true : false;
                                ChkComisiona.Checked = _resultado["Comisiona"].ToString() == "SI" ? true : false;
                            }
                        }

                        if (ViewState["Nivel"].ToString() == "3")
                        {
                            LblDescripcion.Visible = false;
                            TxtDescripcion.Visible = false;
                            ImgAdd.Visible = false;
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
                    else ImgDelete.Visible = true;
                }
                else ImgDelete.Visible = true;
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
                ChkContacto.Visible = false;
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
                        _maxcodigo = 0;
                        _dtbaccion = (DataTable)ViewState["ArbolAccion"];

                        if (_dtbaccion.Rows.Count == 0)
                        {
                            _dts = new ConsultaDatosDAO().FunConsultaDatos(182, int.Parse(ViewState["CodigoCPCE"].ToString()),
                                0, 0, "", "", "", Session["Conectar"].ToString());
                            _dtbaccion = _dts.Tables[0];
                            ViewState["ArbolAccion"] = _dts.Tables[0];
                        }

                        _resultado = _dtbaccion.Select("CodigoCPCE='" + ViewState["CodigoCPCE"].ToString()
                            + "' and Descripcion='" + TxtDescripcion.Text.Trim().ToUpper() + "'").FirstOrDefault();

                        if (_resultado != null)
                        {
                            new FuncionesDAO().FunShowJSMessage("Descripcion ACCION ya existe..!", this);
                            return;
                        }

                        if (_dtbaccion.Rows.Count > 0)
                            _maxcodigo = _dtbaccion.AsEnumerable()
                                .Max(row => int.Parse((string)row["CodigoARAC"]));

                        _maxcodigo++;
                        _resultado = _dtbaccion.NewRow();
                        _resultado["CodigoCPCE"] = ViewState["CodigoCPCE"].ToString();
                        _resultado["CodigoARAC"] = _maxcodigo;
                        _resultado["Descripcion"] = TxtDescripcion.Text.Trim().ToUpper();
                        _resultado["Estado"] = "Activo";
                        _resultado["Nuevo"] = "SI";
                        _dtbaccion.Rows.Add(_resultado);
                        ViewState["ArbolAccion"] = _dtbaccion;
                        TreeNode unnodeaccion = new TreeNode(TxtDescripcion.Text.Trim().ToUpper(), _maxcodigo.ToString());
                        TrvCedenteArbol.SelectedNode.ChildNodes.Add(unnodeaccion);
                        break;
                    case "ACCION":
                        _maxcodigo = 0;
                        _dtbefecto = (DataTable)ViewState["ArbolEfecto"];

                        if (_dtbefecto.Rows.Count == 0)
                        {
                            _dts = new ConsultaDatosDAO().FunConsultaDatos(182, int.Parse(ViewState["CodigoCPCE"].ToString()),
                                0, 0, "", "", "", Session["Conectar"].ToString());
                            _dtbefecto = _dts.Tables[1];
                            ViewState["ArbolEfecto"] = _dts.Tables[1];
                        }

                        _resultado = _dtbefecto.Select("CodigoCPCE='" + ViewState["CodigoCPCE"].ToString() +
                            "' and CodigoARAC='" + ViewState["CodigoARAC"].ToString() + "' and Descripcion ='" +
                            TxtDescripcion.Text.Trim().ToUpper() + "'").FirstOrDefault();

                        if (_resultado != null)
                        {
                            new FuncionesDAO().FunShowJSMessage("Descripcion EFECTO ya existe..!", this);
                            return;
                        }

                        if (_dtbefecto.Rows.Count > 0)
                            _maxcodigo = _dtbefecto.AsEnumerable()
                                .Max(row => int.Parse((string)row["CodigoAREF"]));

                        _maxcodigo++;
                        _resultado = _dtbefecto.NewRow();
                        _resultado["CodigoCPCE"] = ViewState["CodigoCPCE"].ToString();
                        _resultado["CodigoARAC"] = ViewState["CodigoARAC"].ToString();
                        _resultado["CodigoAREF"] = _maxcodigo;
                        _resultado["Descripcion"] = TxtDescripcion.Text.Trim().ToUpper();
                        _resultado["Estado"] = "Activo";
                        _resultado["Nuevo"] = "SI";
                        _dtbefecto.Rows.Add(_resultado);
                        ViewState["ArbolEfecto"] = _dtbefecto;
                        TreeNode unnodeafecto = new TreeNode(TxtDescripcion.Text.Trim().ToUpper(), _maxcodigo.ToString());
                        TrvCedenteArbol.SelectedNode.ChildNodes.Add(unnodeafecto);
                        break;
                    case "EFECTO":
                        _maxcodigo = 0;
                        _dtbrespuesta = (DataTable)ViewState["ArbolRespuesta"];

                        if (_dtbrespuesta.Rows.Count == 0)
                        {
                            _dts = new ConsultaDatosDAO().FunConsultaDatos(182, int.Parse(ViewState["CodigoCPCE"].ToString()),
                                0, 0, "", "", "", Session["Conectar"].ToString());
                            _dtbrespuesta = _dts.Tables[2];
                            ViewState["ArbolRespuesta"] = _dts.Tables[2];
                        }

                        _resultado = _dtbrespuesta.Select("CodigoCPCE='" + ViewState["CodigoCPCE"].ToString() +
                            "' and CodigoAREF='" + ViewState["CodigoAREF"].ToString() + "' and Descripcion='" +
                            TxtDescripcion.Text.Trim().ToUpper() + "'").FirstOrDefault();

                        if (_resultado != null)
                        {
                            new FuncionesDAO().FunShowJSMessage("Descripcion RESPUESTA ya existe..!", this);
                            return;
                        }

                        if (_dtbrespuesta.Rows.Count > 0)
                            _maxcodigo = _dtbrespuesta.AsEnumerable()
                                .Max(row => int.Parse((string)row["CodigoARRE"]));

                        _maxcodigo++;
                        _resultado = _dtbrespuesta.NewRow();
                        _resultado["CodigoCPCE"] = ViewState["CodigoCPCE"].ToString();
                        _resultado["CodigoAREF"] = ViewState["CodigoAREF"].ToString();
                        _resultado["CodigoARRE"] = _maxcodigo;
                        _resultado["Descripcion"] = TxtDescripcion.Text.Trim().ToUpper();
                        _resultado["Estado"] = "Activo";
                        _resultado["Efectivo"] = ChkContacto.Checked ? "1" : "0";
                        _resultado["Tipo"] = ChkPago.Checked ? "1" : ChkLlamar.Checked ? "2" : "0";
                        _resultado["Comisiona"] = ChkComisiona.Checked ? "SI" : "NO";
                        _resultado["Nuevo"] = "SI";
                        _dtbrespuesta.Rows.Add(_resultado);
                        ViewState["ArbolRespuesta"] = _dtbrespuesta;
                        TreeNode unnoderespuesta = new TreeNode(TxtDescripcion.Text.Trim().ToUpper(), _maxcodigo.ToString());
                        TrvCedenteArbol.SelectedNode.ChildNodes.Add(unnoderespuesta);
                        break;
                    case "RESPUESTA":
                        _maxcodigo = 0;
                        _dtbcontacto = (DataTable)ViewState["ArbolContacto"];

                        if (_dtbcontacto.Rows.Count == 0)
                        {
                            _dts = new ConsultaDatosDAO().FunConsultaDatos(182, int.Parse(ViewState["CodigoCPCE"].ToString()),
                                0, 0, "", "", "", Session["Conectar"].ToString());
                            _dtbcontacto = _dts.Tables[3];
                            ViewState["ArbolContacto"] = _dts.Tables[3];
                        }

                        _resultado = _dtbcontacto.Select("CodigoCPCE='" + ViewState["CodigoCPCE"].ToString() +
                            "' and CodigoARRE='" + ViewState["CodigoARRE"].ToString() + "' and Descripcion='" +
                            TxtDescripcion.Text.Trim().ToUpper() + "'").FirstOrDefault();

                        if (_resultado != null)
                        {
                            new FuncionesDAO().FunShowJSMessage("Descripcion CONTACTO ya existe..!", this);
                            return;
                        }

                        if (_dtbcontacto.Rows.Count > 0)
                            _maxcodigo = _dtbcontacto.AsEnumerable()
                                .Max(row => int.Parse((string)row["CodigoARCO"]));

                        _maxcodigo++;
                        _resultado = _dtbcontacto.NewRow();
                        _resultado["CodigoCPCE"] = ViewState["CodigoCPCE"].ToString();
                        _resultado["CodigoARRE"] = ViewState["CodigoARRE"].ToString();
                        _resultado["CodigoARCO"] = _maxcodigo;
                        _resultado["Descripcion"] = TxtDescripcion.Text.Trim().ToUpper();
                        _resultado["Estado"] = "Activo";
                        _resultado["Nuevo"] = "SI";
                        _dtbcontacto.Rows.Add(_resultado);
                        ViewState["ArbolContacto"] = _dtbcontacto;
                        TreeNode unnodecontacto = new TreeNode(TxtDescripcion.Text.Trim().ToUpper(), _maxcodigo.ToString());
                        TrvCedenteArbol.SelectedNode.ChildNodes.Add(unnodecontacto);
                        break;
                }
                TxtDescripcion.Text = "";
                ChkPago.Checked = false;
                ChkLlamar.Checked = false;
                ChkContacto.Checked = false;
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
                        _dts = new ConsultaDatosDAO().FunNewArbolDecision(0, int.Parse(ViewState["CodigoCPCE"].ToString()),
                            int.Parse(ViewState["CodigoARAC"].ToString()), 0, 0, 0, TxtArbol.Text.Trim().ToUpper(),
                            ChkEstado.Checked ? "Activo" : "Inactivo", "", 0, "", "", "", "", "", "", 0, 0, 0, 0, 0,
                            int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                            Session["Conectar"].ToString());
                        break;
                    case "EFECTO":
                        _dts = new ConsultaDatosDAO().FunNewArbolDecision(1, int.Parse(ViewState["CodigoCPCE"].ToString()),
                            int.Parse(ViewState["CodigoARAC"].ToString()), int.Parse(ViewState["CodigoAREF"].ToString()),
                            0, 0, TxtArbol.Text.Trim().ToUpper(), ChkEstado.Checked ? "Activo" : "Inactivo", "", 0, "", "", "", "", "", "", 0, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), Session["Conectar"].ToString());
                        break;
                    case "RESPUESTA":
                        _dts = new ConsultaDatosDAO().FunNewArbolDecision(2, int.Parse(ViewState["CodigoCPCE"].ToString()),
                            int.Parse(ViewState["CodigoARAC"].ToString()), int.Parse(ViewState["CodigoAREF"].ToString()),
                            int.Parse(ViewState["CodigoARRE"].ToString()), 0, TxtArbol.Text.Trim().ToUpper(),
                            ChkEstado.Checked ? "Activo" : "Inactivo", ChkContacto.Checked ? "1" : "0",
                            ChkPago.Checked ? 1 : ChkLlamar.Checked ? 2 : 0, ChkComisiona.Checked ? "SI" : "NO", "", "", "", "", "", 0, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), Session["Conectar"].ToString());
                        break;
                    case "CONTACTO":
                        _dts = new ConsultaDatosDAO().FunNewArbolDecision(3, int.Parse(ViewState["CodigoCPCE"].ToString()),
                            int.Parse(ViewState["CodigoARAC"].ToString()), int.Parse(ViewState["CodigoAREF"].ToString()),
                            int.Parse(ViewState["CodigoARRE"].ToString()), int.Parse(ViewState["CodigoARRE"].ToString()),
                            TxtArbol.Text.Trim().ToUpper(), ChkEstado.Checked ? "Activo" : "Inactivo",
                            "", 0, "", "", "", "", "", "", 0, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), Session["Conectar"].ToString());
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
                    new FuncionesDAO().FunShowJSMessage("No se puede eliminar Cedente..!", this);
                    return;
                }

                _dtbaccion = (DataTable)ViewState["ArbolAccion"];
                _dtbefecto = (DataTable)ViewState["ArbolEfecto"];
                _dtbrespuesta = (DataTable)ViewState["ArbolRespuesta"];
                _dtbcontacto = (DataTable)ViewState["ArbolContacto"];

                if (ViewState["CodigoArbol"].ToString() == "ACCION")
                {
                    _result = _dtbefecto.Select("CodigoCPCE='" + ViewState["CodigoCPCE"].ToString() +
                        "' and CodigoARAC='" + ViewState["CodigoARAC"].ToString() + "'");

                    if (_result.Count() > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Elimine primero los efectos registrados..!", this);
                        return;
                    }
                }

                if (ViewState["CodigoArbol"].ToString() == "EFECTO")
                {
                    _result = _dtbrespuesta.Select("CodigoCPCE='" + ViewState["CodigoCPCE"].ToString() +
                        "' and CodigoAREF='" + ViewState["CodigoAREF"].ToString() + "'");

                    if (_result.Count() > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Elimine primero las respuestas registradas..!", this);
                        return;
                    }
                }

                if (ViewState["CodigoArbol"].ToString() == "RESPUESTA")
                {
                    _result = _dtbcontacto.Select("CodigoCPCE='" + ViewState["CodigoCPCE"].ToString() +
                        "' and CodigoARRE='" + ViewState["CodigoARRE"].ToString() + "'");

                    if (_result.Count() > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Elimine primero los contactos registrados..!", this);
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
                                        _resultado = _dtbaccion.Select("CodigoARAC='" + _codigoarac + "'").FirstOrDefault();
                                        _resultado.Delete();
                                        ViewState["ArbolAccion"] = _dtbaccion;
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
                                                _resultado = _dtbefecto.Select("CodigoAREF='" + _codigoaref + "'").FirstOrDefault();
                                                _resultado.Delete();
                                                ViewState["ArbolEfecto"] = _dtbefecto;
                                                childaccion.ChildNodes.Remove(TrvCedenteArbol.SelectedNode);
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
                                                        _resultado = _dtbrespuesta.Select("CodigoARRE='" + _codigoarre + "'").FirstOrDefault();
                                                        _resultado.Delete();
                                                        ViewState["ArbolRespuesta"] = _dtbrespuesta;
                                                        childefecto.ChildNodes.Remove(TrvCedenteArbol.SelectedNode);
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
                                                            _resultado = _dtbcontacto.Select("CodigoARCO='" + _codigoarco + "'").FirstOrDefault();
                                                            _resultado.Delete();
                                                            ViewState["ArbolContacto"] = _dtbcontacto;
                                                            childrespuesta.ChildNodes.Remove(TrvCedenteArbol.SelectedNode);
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
            ChkContacto.Text = ChkContacto.Checked ? "Efectivo" : "No Efectivo";
        }

        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstado.Text = ChkEstado.Checked ? "Activo" : "Inactivo";
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                _dtbaccion = (DataTable)ViewState["ArbolAccion"];
                _dtbefecto = (DataTable)ViewState["ArbolEfecto"];
                _dtbrespuesta = (DataTable)ViewState["ArbolRespuesta"];
                _dtbcontacto = (DataTable)ViewState["ArbolContacto"];

                foreach (DataRow _draccion in _dtbaccion.Rows)
                {
                    _codigocpce = _draccion["CodigoCPCE"].ToString();
                    _codigoarac = _draccion["CodigoARAC"].ToString();
                    _descripcion = _draccion["Descripcion"].ToString();
                    _estado = _draccion["Estado"].ToString();

                    _dts = new ConsultaDatosDAO().FunNewArbolDecision(0, int.Parse(_codigocpce), int.Parse(_codigoarac),
                        0, 0, 0, _descripcion, _estado, "", 0, "", "", "", "", "", "", 0, 0, 0, 0, 0,
                        int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                        Session["Conectar"].ToString());

                    _codigoaracnew = _dts.Tables[0].Rows[0]["CodigoARAC"].ToString();
                    _efecto = _dtbefecto.Select("CodigoARAC='" + _codigoarac + "'");

                    foreach (DataRow drefecto in _efecto)
                    {
                        _codigoaref = drefecto["CodigoAREF"].ToString();
                        _descripcion = drefecto["Descripcion"].ToString();
                        _estado = drefecto["Estado"].ToString();

                        _dts = new ConsultaDatosDAO().FunNewArbolDecision(1, int.Parse(_codigocpce), int.Parse(_codigoaracnew),
                            int.Parse(_codigoaref), 0, 0, _descripcion, _estado, "", 0, "", "", "", "", "", "", 0, 0, 0, 0, 0,
                            int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                            Session["Conectar"].ToString());

                        _codigoarefnew = _dts.Tables[0].Rows[0]["CodigoAREF"].ToString();
                        _respuesta = _dtbrespuesta.Select("CodigoAREF='" + _codigoaref + "'");

                        foreach (DataRow drrespuesta in _respuesta)
                        {
                            _codigoarre = drrespuesta["CodigoARRE"].ToString();
                            _descripcion = drrespuesta["Descripcion"].ToString();
                            _estado = drrespuesta["Estado"].ToString();
                            _efectivo = drrespuesta["Efectivo"].ToString();
                            _tiporespuesta = drrespuesta["Tipo"].ToString();
                            _comisiona = drrespuesta["Comisiona"].ToString();

                            _dts = new ConsultaDatosDAO().FunNewArbolDecision(2, int.Parse(_codigocpce), int.Parse(_codigoaracnew),
                                int.Parse(_codigoarefnew), int.Parse(_codigoarre), 0, _descripcion, _estado, _efectivo,
                                int.Parse(_tiporespuesta), _comisiona, "", "", "", "", "", 0, 0, 0, 0, 0,
                                int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                                Session["Conectar"].ToString());

                            _codigoarrenew = _dts.Tables[0].Rows[0]["CodigoARRE"].ToString();
                            _contacto = _dtbcontacto.Select("CodigoARRE='" + _codigoarre + "'");

                            foreach (DataRow drcontacto in _contacto)
                            {
                                _codigoarco = drcontacto["CodigoARCO"].ToString();
                                _descripcion = drcontacto["Descripcion"].ToString();
                                _estado = drcontacto["Estado"].ToString();

                                _dts = new ConsultaDatosDAO().FunNewArbolDecision(3, int.Parse(_codigocpce), int.Parse(_codigoaracnew), int.Parse(_codigoarefnew), int.Parse(_codigoarrenew), int.Parse(_codigoarco), _descripcion, _estado, "", 0, "", "", "", "", "", "", 0, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), Session["Conectar"].ToString());
                            }
                        }
                    }
                }
                response = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");
                Response.Redirect(response, false);
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