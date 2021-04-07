namespace SoftCob.Views.ArbolDesicion
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ArbolDecision : Page
    {
        #region Variables
        DataSet _dts, _dsaccion, _dsefecto, _dsrespuesta, _dscontacto;
        DataTable _dtbaccion = new DataTable();
        DataTable _dtbefecto = new DataTable();
        DataTable _dtbrespuesta = new DataTable();
        DataTable _dtbcontacto = new DataTable();
        DataTable _dtbefectotemp = new DataTable();
        DataTable _dtbrespuestatemp = new DataTable();
        DataTable _dtbcontactotemp = new DataTable();
        DataTable _tblbuscar = new DataTable();
        DataTable _tblagre = new DataTable();
        DataRow[] _drtemp, _drtempx, _dr;
        DataRow _filagretem, _filagretemx, _filagre, _result, _change;
        int _idciudad = 0, _maxcodigo = 0, _codigo = 0, _codigoarbolaccion = 0, _codigoarbolefecto = 0, _codigoarbolrespuesta = 0;
        bool _lexiste = false, _estadoact = false, _contactoact = false;
        CheckBox _chkestado, _chkcontacto, _chkpago, _chkllamar, _chkcomisiona, _chkefectivo;
        ImageButton _imgborrar;
        string _contacto = "", _sql = "", _mensaje = "", _descripcion = "", _response = "";
        string[] _pathroot;
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
                    Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(),
                        true);
                    return;
                }

                Session["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                _dtbaccion.Columns.Add("Codigo");
                _dtbaccion.Columns.Add("CodigoCatagolo");
                _dtbaccion.Columns.Add("Descripcion");
                _dtbaccion.Columns.Add("Estado");
                _dtbaccion.Columns.Add("Contacto");
                _dtbaccion.Columns.Add("auxv1");
                _dtbaccion.Columns.Add("auxv2");
                _dtbaccion.Columns.Add("auxi1");
                _dtbaccion.Columns.Add("auxi2");
                ViewState["ArbolAccion"] = _dtbaccion;

                _dtbefecto.Columns.Add("Codigo");
                _dtbefecto.Columns.Add("CodigoAccion");
                _dtbefecto.Columns.Add("Descripcion");
                _dtbefecto.Columns.Add("Estado");
                _dtbefecto.Columns.Add("auxv1");
                _dtbefecto.Columns.Add("auxv2");
                _dtbefecto.Columns.Add("auxi1");
                _dtbefecto.Columns.Add("auxi2");
                ViewState["ArbolEfecto"] = _dtbefecto;

                _dtbefectotemp.Columns.Add("Codigo");
                _dtbefectotemp.Columns.Add("CodigoAccion");
                _dtbefectotemp.Columns.Add("Descripcion");
                _dtbefectotemp.Columns.Add("Estado");
                _dtbefectotemp.Columns.Add("auxv1");
                _dtbefectotemp.Columns.Add("auxv2");
                _dtbefectotemp.Columns.Add("auxi1");
                _dtbefectotemp.Columns.Add("auxi2");
                ViewState["ArbolEfectoTemp"] = _dtbefectotemp;

                _dtbrespuesta.Columns.Add("Codigo");
                _dtbrespuesta.Columns.Add("CodigoEfecto");
                _dtbrespuesta.Columns.Add("Descripcion");
                _dtbrespuesta.Columns.Add("Estado");
                _dtbrespuesta.Columns.Add("Pago");
                _dtbrespuesta.Columns.Add("Llamar");
                _dtbrespuesta.Columns.Add("auxv1");
                _dtbrespuesta.Columns.Add("auxv2");
                _dtbrespuesta.Columns.Add("auxi1");
                _dtbrespuesta.Columns.Add("auxi2");
                ViewState["ArbolRespuesta"] = _dtbrespuesta;

                _dtbrespuestatemp.Columns.Add("Codigo");
                _dtbrespuestatemp.Columns.Add("CodigoEfecto");
                _dtbrespuestatemp.Columns.Add("Descripcion");
                _dtbrespuestatemp.Columns.Add("Estado");
                _dtbrespuestatemp.Columns.Add("Pago");
                _dtbrespuestatemp.Columns.Add("Llamar");
                _dtbrespuestatemp.Columns.Add("auxv1");
                _dtbrespuestatemp.Columns.Add("auxv2");
                _dtbrespuestatemp.Columns.Add("auxi1");
                _dtbrespuestatemp.Columns.Add("auxi2");
                ViewState["ArbolRespuestaTemp"] = _dtbrespuestatemp;

                _dtbcontacto.Columns.Add("Codigo");
                _dtbcontacto.Columns.Add("CodigoRespuesta");
                _dtbcontacto.Columns.Add("Descripcion");
                _dtbcontacto.Columns.Add("Estado");
                _dtbcontacto.Columns.Add("auxv1");
                _dtbcontacto.Columns.Add("auxv2");
                _dtbcontacto.Columns.Add("auxi1");
                _dtbcontacto.Columns.Add("auxi2");
                ViewState["ArbolContacto"] = _dtbcontacto;

                _dtbcontactotemp.Columns.Add("Codigo");
                _dtbcontactotemp.Columns.Add("CodigoRespuesta");
                _dtbcontactotemp.Columns.Add("Descripcion");
                _dtbcontactotemp.Columns.Add("Estado");
                _dtbcontactotemp.Columns.Add("auxv1");
                _dtbcontactotemp.Columns.Add("auxv2");
                _dtbcontactotemp.Columns.Add("auxi1");
                _dtbcontactotemp.Columns.Add("auxi2");
                ViewState["ArbolContactoTemp"] = _dtbcontactotemp;

                ViewState["codigoCatalogo"] = "0";
                ViewState["CodigoAccion"] = "0";
                ViewState["CodigoEfecto"] = "0";
                ViewState["CodigoRespuesta"] = "0";
                ViewState["CodigoContacto"] = "0";
                Lbltitulo.Text = "Definir Arbol de Decisión";

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::",
                    Request["MensajeRetornado"].ToString());

            }
        }
        #endregion

        #region Procedimientos y Eventos
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
                    foreach (DataRow _fila in _dts.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(_fila[0].ToString(), _fila[1].ToString());
                        unNode = FunAgregarProductoCedentes(unNode, int.Parse(_fila[1].ToString()));
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
                    foreach (DataRow _fila in _dts.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(_fila[0].ToString(), _fila[1].ToString());
                        unNode = FunAgregarCatalogoProductos(unNode, int.Parse(_fila[1].ToString()));
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
                    foreach (DataRow _fila in _dts.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(_fila[0].ToString(), _fila[1].ToString());
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
                _dsaccion = new ConsultaDatosDAO().FunConsultaDatos(16, int.Parse(ViewState["codigoCatalogo"].ToString()), 0, 0,
                    "", "", "", Session["Conectar"].ToString());

                ViewState["ArbolAccion"] = _dsaccion.Tables[0];
                GrdvAccion.DataSource = _dsaccion;
                GrdvAccion.DataBind();

                if (_dsaccion.Tables[0].Rows.Count > 0)
                {
                    pnlAccion.Visible = true;
                    _dtbefecto = (DataTable)ViewState["ArbolEfecto"];

                    foreach (DataRow _draccion in _dsaccion.Tables[0].Rows)
                    {
                        _codigoarbolaccion = int.Parse(_draccion[0].ToString());
                        _dsefecto = new ConsultaDatosDAO().FunConsultaDatos(17, _codigoarbolaccion, int.Parse(ViewState["codigoCatalogo"].ToString()),
                            0, "", "", "", Session["Conectar"].ToString());

                        foreach (DataRow _fila in _dsefecto.Tables[0].Rows)
                        {
                            _filagre = _dtbefecto.NewRow();
                            _filagre["Codigo"] = _fila[0].ToString();
                            _filagre["CodigoAccion"] = _fila[1].ToString();
                            _filagre["Descripcion"] = _fila[2].ToString();
                            _filagre["Estado"] = _fila[3].ToString();
                            _filagre["auxv1"] = _fila[4].ToString();
                            _filagre["auxv2"] = _fila[5].ToString();
                            _filagre["auxi1"] = _fila[6].ToString();
                            _filagre["auxi2"] = _fila[7].ToString();
                            _dtbefecto.Rows.Add(_filagre);
                        }

                        _dtbefecto.DefaultView.Sort = "Descripcion";
                        _dtbefecto = _dtbefecto.DefaultView.ToTable();
                    }

                    ViewState["ArbolEfecto"] = _dtbefecto;

                    if (_dtbefecto.Rows.Count > 0)
                    {
                        _dtbrespuesta = (DataTable)ViewState["ArbolRespuesta"];

                        foreach (DataRow _drefecto in _dtbefecto.Rows)
                        {
                            _codigoarbolefecto = int.Parse(_drefecto[0].ToString());
                            _dsrespuesta = new ConsultaDatosDAO().FunConsultaDatos(18, _codigoarbolefecto,
                                int.Parse(ViewState["codigoCatalogo"].ToString()), 0, "", "", "",
                                Session["Conectar"].ToString());

                            foreach (DataRow _fila in _dsrespuesta.Tables[0].Rows)
                            {
                                _filagre = _dtbrespuesta.NewRow();
                                _filagre["Codigo"] = _fila[0].ToString();
                                _filagre["CodigoEfecto"] = _fila[1].ToString();
                                _filagre["Descripcion"] = _fila[2].ToString();
                                _filagre["Estado"] = _fila[3].ToString();
                                _filagre["Pago"] = _fila[4].ToString();
                                _filagre["LLamar"] = _fila[5].ToString();
                                _filagre["auxv1"] = _fila[6].ToString();
                                _filagre["auxv2"] = _fila[7].ToString();
                                _filagre["auxi1"] = _fila[8].ToString();
                                _filagre["auxi2"] = _fila[9].ToString();
                                _dtbrespuesta.Rows.Add(_filagre);
                            }

                            _dtbrespuesta.DefaultView.Sort = "Descripcion";
                            _dtbrespuesta = _dtbrespuesta.DefaultView.ToTable();
                        }

                        ViewState["ArbolRespuesta"] = _dtbrespuesta;

                        if (_dtbrespuesta.Rows.Count > 0)
                        {
                            _dtbcontacto = (DataTable)ViewState["ArbolContacto"];

                            foreach (DataRow _drrespuesta in _dtbrespuesta.Rows)
                            {
                                _codigoarbolrespuesta = int.Parse(_drrespuesta[0].ToString());
                                _dscontacto = new ConsultaDatosDAO().FunConsultaDatos(19, _codigoarbolrespuesta,
                                    int.Parse(ViewState["codigoCatalogo"].ToString()), 0, "", "", "",
                                    Session["Conectar"].ToString());

                                foreach (DataRow _fila in _dscontacto.Tables[0].Rows)
                                {
                                    _filagre = _dtbcontacto.NewRow();
                                    _filagre["Codigo"] = _fila[0].ToString();
                                    _filagre["CodigoRespuesta"] = _fila[1].ToString();
                                    _filagre["Descripcion"] = _fila[2].ToString();
                                    _filagre["Estado"] = _fila[3].ToString();
                                    _filagre["auxv1"] = _fila[4].ToString();
                                    _filagre["auxv2"] = _fila[5].ToString();
                                    _filagre["auxi1"] = _fila[6].ToString();
                                    _filagre["auxi2"] = _fila[7].ToString();
                                    _dtbcontacto.Rows.Add(_filagre);
                                }
                                _dtbcontacto.DefaultView.Sort = "Descripcion";
                                _dtbcontacto = _dtbcontacto.DefaultView.ToTable();
                            }
                        }
                        ViewState["ArbolContacto"] = _dtbcontacto;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void FunLimpiarCampos(int opcion)
        {
            _lexiste = false;
            switch (opcion)
            {
                case 0:
                    ViewState["CodigoAccion"] = "0";
                    ViewState["CodigoEfecto"] = "0";
                    ViewState["CodigoRespuesta"] = "0";
                    ViewState["CodigoContacto"] = "0";
                    pnlEfecto.Visible = false;
                    pnlRespuesta.Visible = false;
                    pnlContacto.Visible = false;
                    break;
                case 1:
                    ViewState["CodigoEfecto"] = "0";
                    ViewState["CodigoRespuesta"] = "0";
                    ViewState["CodigoContacto"] = "0";
                    pnlRespuesta.Visible = false;
                    pnlContacto.Visible = false;
                    break;
                case 2:
                    ViewState["CodigoRespuesta"] = "0";
                    ViewState["CodigoContacto"] = "0";
                    pnlContacto.Visible = false;
                    break;
                case 3:
                    ViewState["CodigoContacto"] = "0";
                    break;
            }
        }

        protected void FunLimpiarObjects()
        {
            Lblerror.Text = "";
            ViewState["CodigoAccion"] = "0";
            ViewState["CodigoEfecto"] = "0";
            ViewState["CodigoRespuesta"] = "0";
            ViewState["CodigoContacto"] = "0";

            _dtbaccion.Clear();
            _dtbaccion.Columns.Add("Codigo");
            _dtbaccion.Columns.Add("CodigoCatagolo");
            _dtbaccion.Columns.Add("Descripcion");
            _dtbaccion.Columns.Add("Estado");
            _dtbaccion.Columns.Add("Contacto");
            _dtbaccion.Columns.Add("auxv1");
            _dtbaccion.Columns.Add("auxv2");
            _dtbaccion.Columns.Add("auxi1");
            _dtbaccion.Columns.Add("auxi2");
            ViewState["ArbolAccion"] = _dtbaccion;

            _dtbefecto.Clear();
            _dtbefectotemp.Clear();
            _dtbefecto.Columns.Add("Codigo");
            _dtbefecto.Columns.Add("CodigoAccion");
            _dtbefecto.Columns.Add("Descripcion");
            _dtbefecto.Columns.Add("Estado");
            _dtbefecto.Columns.Add("auxv1");
            _dtbefecto.Columns.Add("auxv2");
            _dtbefecto.Columns.Add("auxi1");
            _dtbefecto.Columns.Add("auxi2");
            ViewState["ArbolEfecto"] = _dtbefecto;

            _dtbrespuesta.Clear();
            _dtbrespuestatemp.Clear();
            _dtbrespuesta.Columns.Add("Codigo");
            _dtbrespuesta.Columns.Add("CodigoEfecto");
            _dtbrespuesta.Columns.Add("Descripcion");
            _dtbrespuesta.Columns.Add("Estado");
            _dtbrespuesta.Columns.Add("Pago");
            _dtbrespuesta.Columns.Add("Llamar");
            _dtbrespuesta.Columns.Add("auxv1");
            _dtbrespuesta.Columns.Add("auxv2");
            _dtbrespuesta.Columns.Add("auxi1");
            _dtbrespuesta.Columns.Add("auxi2");
            ViewState["ArbolRespuesta"] = _dtbrespuesta;

            _dtbcontacto.Clear();
            _dtbcontactotemp.Clear();
            _dtbcontacto.Columns.Add("Codigo");
            _dtbcontacto.Columns.Add("CodigoRespuesta");
            _dtbcontacto.Columns.Add("Descripcion");
            _dtbcontacto.Columns.Add("Estado");
            _dtbcontacto.Columns.Add("auxv1");
            _dtbcontacto.Columns.Add("auxv2");
            _dtbcontacto.Columns.Add("auxi1");
            _dtbcontacto.Columns.Add("auxi2");
            ViewState["ArbolContacto"] = _dtbcontacto;

            pnlAccion.Visible = false;
            pnlEfecto.Visible = false;
            pnlRespuesta.Visible = false;
            pnlContacto.Visible = false;
        }
        #endregion

        #region Botones y Eventos
        protected void TrvCedentes_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeView arbolPres = (TreeView)(sender);
            TreeNode node = arbolPres.SelectedNode;
            try
            {
                ViewState["codigoCatalogo"] = "0";
                switch (node.Depth)
                {
                    case 1:
                        Lbltitulo.Text = "Definir Arbol de Decisión";
                        break;
                    case 4:
                        _pathroot = node.ValuePath.Split(new char[] { '/' });
                        ViewState["codigoCatalogo"] = _pathroot[5].ToString();
                        SoftCob_CEDENTE cedente = new CedenteDAO().FunGetCedentePorID(int.Parse(_pathroot[3].ToString()));
                        ViewState["NivelArbol"] = cedente.cede_auxi1;
                        lblCatalogo.InnerText = "Definir Árbol >>" + new CedenteDAO().FunGetNameCatalogoporID(int.Parse(_pathroot[5].ToString()));
                        FunLimpiarObjects();
                        FunCargarMantenimiento();
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void ImgAddAccion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ViewState["codigoCatalogo"] == null || int.Parse(ViewState["codigoCatalogo"].ToString()) == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Producto/Catálogo..!", this);
                    return;
                }

                if (TxtAccion.Text.Trim() == "")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Definición de Acción..!", this);
                    return;
                }

                if (ViewState["ArbolAccion"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["ArbolAccion"];

                    if (_tblbuscar.Rows.Count > 0)
                        _maxcodigo = _tblbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _result = _tblbuscar.Select("Descripcion='" + TxtAccion.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Ya existe definido Acción..!", this);
                    return;
                }

                pnlAccion.Visible = true;
                _tblagre = new DataTable();
                _tblagre = (DataTable)ViewState["ArbolAccion"];
                _filagre = _tblagre.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["CodigoCatagolo"] = ViewState["codigoCatalogo"].ToString();
                _filagre["Descripcion"] = TxtAccion.Text.Trim().ToUpper();
                _filagre["Estado"] = "Activo";
                _filagre["Contacto"] = "NO";
                _filagre["auxv1"] = "";
                _filagre["auxv2"] = "";
                _filagre["auxi1"] = "0";
                _filagre["auxi2"] = "0";
                _tblagre.Rows.Add(_filagre);
                _tblagre.DefaultView.Sort = "Descripcion";
                _tblagre = _tblagre.DefaultView.ToTable();
                ViewState["ArbolAccion"] = _tblagre;
                GrdvAccion.DataSource = _tblagre;
                GrdvAccion.DataBind();
                TxtAccion.Text = "";
                FunLimpiarCampos(0);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEdiAccion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = int.Parse(GrdvAccion.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                ViewState["CodigoAccion"] = _codigo;
                _dtbaccion = (DataTable)ViewState["ArbolAccion"];
                _result = _dtbaccion.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtAccion.Text = _result["Descripcion"].ToString();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModiAccion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ViewState["CodigoAccion"] == null || int.Parse(ViewState["CodigoAccion"].ToString()) == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione definición de Acción..!", this);
                    return;
                }

                if (TxtAccion.Text.Trim() == "")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Definición de Acción..!", this);
                    return;
                }

                if (ViewState["ArbolAccion"] != null)
                {
                    DataTable tblbuscar = (DataTable)ViewState["ArbolAccion"];
                    _result = tblbuscar.Select("Descripcion='" + TxtAccion.Text.Trim().ToUpper() + "'").FirstOrDefault();
                    tblbuscar.DefaultView.Sort = "Codigo";
                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Ya existe definido Acción..!", this);
                    return;
                }

                _dtbaccion = (DataTable)ViewState["ArbolAccion"];

                SoftCob_ACCION _datos = new SoftCob_ACCION();
                {
                    _datos.ARAC_CODIGO = int.Parse(ViewState["CodigoAccion"].ToString());
                    _datos.CPCE_CODIGO = int.Parse(ViewState["codigoCatalogo"].ToString());
                    _datos.arac_descripcion = TxtAccion.Text.Trim().ToUpper();
                }

                new ArbolDecisionDAO().FunEditArbolAccion(_datos);
                _change = _dtbaccion.Select("Codigo='" + ViewState["CodigoAccion"].ToString() + "'").FirstOrDefault();
                _change["Descripcion"] = TxtAccion.Text.Trim().ToUpper();
                _dtbaccion.AcceptChanges();
                GrdvAccion.DataSource = _dtbaccion;
                GrdvAccion.DataBind();
                TxtAccion.Text = "";
                FunLimpiarCampos(0);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvAccion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkestado = (CheckBox)(e.Row.Cells[1].FindControl("ChkEstAccion"));
                    _chkcontacto = (CheckBox)(e.Row.Cells[3].FindControl("ChkContacto"));
                    _imgborrar = (ImageButton)(e.Row.Cells[5].FindControl("ImgDelAccion"));
                    _contacto = GrdvAccion.DataKeys[e.Row.RowIndex].Values["Contacto"].ToString();
                    _codigo = int.Parse(GrdvAccion.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString());
                    _sql = "Select Estado=case arac_estado when 1 then 'Activo' else 'Inactivo' end,";
                    _sql += "Contacto = case arac_contacto when 1 then 'SI' else 'NO' end from GSBPO_ACCION where ";
                    _sql += "CPCE_CODIGO=" + ViewState["codigoCatalogo"].ToString() + " and ARAC_CODIGO=" + _codigo;
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql, "", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        _chkestado.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                        _chkcontacto.Checked = _dts.Tables[0].Rows[0]["Contacto"].ToString() == "SI" ? true : false;
                        _imgborrar.Enabled = false;
                        _imgborrar.ImageUrl = "~/Botones/eliminargris.png";
                    }
                    else
                    {
                        _chkestado.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEstAccion_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkestado = (CheckBox)(_gvrow.Cells[1].FindControl("ChkEstAccion"));
                _dtbaccion = (DataTable)ViewState["ArbolAccion"];
                _codigo = int.Parse(GrdvAccion.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _result = _dtbaccion.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result["Estado"] = _chkestado.Checked ? "Activo" : "Inactivo";
                _dtbaccion.AcceptChanges();
                ViewState["ArbolAccion"] = _dtbaccion;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ChkContacto_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkcontacto = (CheckBox)(_gvrow.Cells[3].FindControl("ChkContacto"));
                _dtbaccion = (DataTable)ViewState["ArbolAccion"];
                _codigo = int.Parse(GrdvAccion.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _result = _dtbaccion.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result["Contacto"] = _chkcontacto.Checked ? "SI" : "NO";
                _dtbaccion.AcceptChanges();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgSelecAccion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                pnlEfecto.Visible = false;
                pnlRespuesta.Visible = false;
                pnlContacto.Visible = false;
                ViewState["CodigoAccion"] = "0";
                ViewState["CodigoEfecto"] = "0";
                ViewState["CodigoRespuesta"] = "0";
                ViewState["CodigoContacto"] = "0";
                TxtEfecto.Text = "";
                TxtRespuesta.Text = "";
                TxtContacto.Text = "";

                foreach (GridViewRow _fila in GrdvAccion.Rows)
                {
                    _fila.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvAccion.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Gray;
                ViewState["CodigoAccion"] = int.Parse(GrdvAccion.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _dtbefecto = (DataTable)ViewState["ArbolEfecto"];
                _dtbefectotemp = (DataTable)ViewState["ArbolEfectoTemp"];
                _dtbefectotemp.Clear();
                _dr = _dtbefecto.Select("CodigoAccion='" + ViewState["CodigoAccion"].ToString() + "'");

                foreach (DataRow _fila in _dr)
                {
                    _filagre = _dtbefectotemp.NewRow();
                    _filagre["Codigo"] = _fila[0].ToString();
                    _filagre["CodigoAccion"] = _fila[1].ToString();
                    _filagre["Descripcion"] = _fila[2].ToString();
                    _filagre["Estado"] = _fila[3].ToString();
                    _filagre["auxv1"] = _fila[4].ToString();
                    _filagre["auxv2"] = _fila[5].ToString();
                    _filagre["auxi1"] = _fila[6].ToString();
                    _filagre["auxi2"] = _fila[7].ToString();
                    _dtbefectotemp.Rows.Add(_filagre);
                    _dtbefectotemp.DefaultView.Sort = "Descripcion";
                }

                if (_dtbefectotemp.Rows.Count > 0) pnlEfecto.Visible = true;

                GrdvEfecto.DataSource = _dtbefectotemp;
                GrdvEfecto.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgDelAccion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = int.Parse(GrdvAccion.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                //Buscar si no existe Efecto agregado
                _dtbefecto = (DataTable)ViewState["ArbolEfecto"];
                _result = _dtbefecto.Select("CodigoAccion='" + _codigo + "'").FirstOrDefault();

                if (_result == null)
                {
                    _mensaje = new ArbolDecisionDAO().FunDelAccion(_codigo, int.Parse(ViewState["codigoCatalogo"].ToString()));
                    _dtbaccion = (DataTable)ViewState["ArbolAccion"];
                    _result = _dtbaccion.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                    _result.Delete();
                    _dtbaccion.AcceptChanges();
                    ViewState["ArbolAccion"] = _dtbaccion;
                    GrdvAccion.DataSource = _dtbaccion;
                    GrdvAccion.DataBind();
                    //contar si ya no hay registros
                    if (_dtbaccion.Rows.Count == 0)
                    {
                        pnlAccion.Visible = false;
                        FunLimpiarCampos(0);
                    }
                }
                else new FuncionesDAO().FunShowJSMessage("Elimine antes el efecto asociado..!", this);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void GrdvAccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow fr in GrdvAccion.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvAccion.Rows[GrdvAccion.SelectedIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _codigo = int.Parse(GrdvAccion.DataKeys[GrdvAccion.SelectedIndex].Values["Codigo"].ToString());
                ViewState["CodigoAccion"] = _codigo;
                _dtbaccion = (DataTable)ViewState["ArbolAccion"];
                _result = _dtbaccion.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtAccion.Text = _result["Descripcion"].ToString();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEfectivo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkefectivo = (CheckBox)(_gvrow.Cells[4].FindControl("ChkEfectivo"));
                _dtbrespuesta = (DataTable)ViewState["ArbolRespuesta"];
                _codigo = int.Parse(GrdvRespuesta.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _result = _dtbrespuesta.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result["auxi2"] = _chkefectivo.Checked ? "1" : "0";
                _dtbrespuesta.AcceptChanges();
                ViewState["ArbolRespuesta"] = _dtbrespuesta;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgAddEfecto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ViewState["CodigoAccion"] == null || int.Parse(ViewState["CodigoAccion"].ToString()) == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Definición de Acción..!", this);
                    return;
                }

                if (TxtEfecto.Text.Trim() == "")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Definición de Efecto..!", this);
                    return;
                }

                if (ViewState["ArbolEfecto"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["ArbolEfecto"];

                    if (_tblbuscar.Rows.Count > 0)
                        _maxcodigo = _tblbuscar.AsEnumerable().Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _result = _tblbuscar.Select("CodigoAccion='" + ViewState["CodigoAccion"].ToString() + "' and Descripcion='" +
                        TxtEfecto.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Ya existe definido el Efecto..!", this);
                    return;
                }

                pnlEfecto.Visible = true;
                _tblagre = (DataTable)ViewState["ArbolEfecto"];
                _filagre = _tblagre.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["CodigoAccion"] = ViewState["CodigoAccion"].ToString();
                _filagre["Descripcion"] = TxtEfecto.Text.Trim().ToUpper();
                _filagre["Estado"] = "Activo";
                _filagre["auxv1"] = "NO";
                _filagre["auxv2"] = "";
                _filagre["auxi1"] = ViewState["codigoCatalogo"].ToString();
                _filagre["auxi2"] = TxtDiaLatencia.Text;
                _tblagre.Rows.Add(_filagre);
                ViewState["ArbolEfecto"] = _tblagre;
                _dtbefectotemp = (DataTable)ViewState["ArbolEfectoTemp"];
                _dtbefectotemp.Clear();
                _drtemp = _tblagre.Select("CodigoAccion='" + ViewState["CodigoAccion"].ToString() + "'");

                foreach (DataRow _fila in _drtemp)
                {
                    _filagretem = _dtbefectotemp.NewRow();
                    _filagretem["Codigo"] = _fila[0].ToString();
                    _filagretem["CodigoAccion"] = _fila[1].ToString();
                    _filagretem["Descripcion"] = _fila[2].ToString();
                    _filagretem["Estado"] = _fila[3].ToString();
                    _filagre["auxv1"] = _fila[4].ToString();
                    _filagre["auxv2"] = _fila[5].ToString();
                    _filagre["auxi1"] = _fila[6].ToString();
                    _filagre["auxi2"] = _fila[7].ToString();
                    _dtbefectotemp.Rows.Add(_filagretem);
                }

                _dtbefectotemp.DefaultView.Sort = "Descripcion";
                _dtbefectotemp = _dtbefectotemp.DefaultView.ToTable();
                GrdvEfecto.DataSource = _dtbefectotemp;
                GrdvEfecto.DataBind();
                TxtEfecto.Text = "";
                TxtDiaLatencia.Text = "0";
                FunLimpiarCampos(1);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void GrdvEfecto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkestado = (CheckBox)(e.Row.Cells[1].FindControl("ChkEstEfecto"));
                    _chkcomisiona = (CheckBox)(e.Row.Cells[2].FindControl("ChkComisiona"));
                    _imgborrar = (ImageButton)(e.Row.Cells[4].FindControl("ImgDelEfecto"));
                    _codigo = int.Parse(GrdvEfecto.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString());
                    _sql = "Select Comisiona=aref_auxv1,Estado=case aref_estado when 1 then 'Activo' else 'Inactivo' end from GSBPO_EFECTO where ";
                    _sql += "aref_auxi1=" + ViewState["codigoCatalogo"].ToString() + " and AREF_CODIGO=" + _codigo + " and ";
                    _sql += "ARAC_CODIGO=" + ViewState["CodigoAccion"].ToString();
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql, "", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        _chkestado.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                        _imgborrar.Enabled = false;
                        _imgborrar.ImageUrl = "~/Botones/eliminargris.png";
                        _chkcomisiona.Checked = _dts.Tables[0].Rows[0]["Comisiona"].ToString() == "SI" ? true : false;
                        _chkcomisiona.Text = _dts.Tables[0].Rows[0]["Comisiona"].ToString();
                    }
                    else
                    {
                        _chkestado.Checked = true;
                        _chkcomisiona.Checked = false;
                        _chkcomisiona.Text = "NO";
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ChkEstEfecto_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkestado = (CheckBox)(_gvrow.Cells[1].FindControl("ChkEstEfecto"));
                _dtbefecto = (DataTable)ViewState["ArbolEfecto"];
                _codigo = int.Parse(GrdvEfecto.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _result = _dtbefecto.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result["Estado"] = _chkestado.Checked ? "Activo" : "Inactivo";
                _dtbefecto.AcceptChanges();
                ViewState["ArbolEfecto"] = _dtbefecto;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ChkComisiona_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkcomisiona = (CheckBox)(_gvrow.Cells[2].FindControl("ChkComisiona"));
                _dtbefecto = (DataTable)ViewState["ArbolEfecto"];
                _codigo = int.Parse(GrdvEfecto.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _result = _dtbefecto.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result["auxv1"] = _chkcomisiona.Checked ? "SI" : "NO";
                _chkcomisiona.Text = _chkcomisiona.Checked ? "SI" : "NO";
                _dtbefecto.AcceptChanges();
                ViewState["ArbolEfecto"] = _dtbefecto;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEdiEfecto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _dtbefecto = (DataTable)ViewState["ArbolEfecto"];
                _codigo = int.Parse(GrdvEfecto.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                ViewState["CodigoEfecto"] = _codigo;
                _result = _dtbefecto.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtEfecto.Text = _result["Descripcion"].ToString();
                ViewState["DescipcionEfecto"] = _result["Descripcion"].ToString();
                TxtDiaLatencia.Text = _result["auxi2"].ToString();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgSelecEfecto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lblerror.Text = "";
                ViewState["CodigoEfecto"] = "0";
                ViewState["CodigoRespuesta"] = "0";
                ViewState["CodigoContacto"] = "0";
                TxtRespuesta.Text = "";
                TxtContacto.Text = "";

                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                pnlRespuesta.Visible = false;
                pnlContacto.Visible = false;

                foreach (GridViewRow _fr in GrdvEfecto.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvEfecto.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Beige;
                ViewState["CodigoEfecto"] = int.Parse(GrdvEfecto.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());

                _dtbrespuesta = (DataTable)ViewState["ArbolRespuesta"];
                _dtbrespuestatemp = (DataTable)ViewState["ArbolRespuestaTemp"];
                _dtbrespuestatemp.Clear();
                _dr = _dtbrespuesta.Select("CodigoEfecto='" + ViewState["CodigoEfecto"].ToString() + "'");

                foreach (DataRow _fila in _dr)
                {
                    _filagre = _dtbrespuestatemp.NewRow();
                    _filagre["Codigo"] = _fila[0].ToString();
                    _filagre["CodigoEfecto"] = _fila[1].ToString();
                    _filagre["Descripcion"] = _fila[2].ToString();
                    _filagre["Estado"] = _fila[3].ToString();
                    _filagre["Pago"] = _fila[4].ToString();
                    _filagre["LLamar"] = _fila[5].ToString();
                    _filagre["auxv1"] = _fila[6].ToString();
                    _filagre["auxv2"] = _fila[7].ToString();
                    _filagre["auxi1"] = _fila[8].ToString();
                    _filagre["auxi2"] = _fila[9].ToString();
                    _dtbrespuestatemp.Rows.Add(_filagre);
                    _dtbrespuestatemp.DefaultView.Sort = "Descripcion";
                }

                if (_dtbrespuestatemp.Rows.Count > 0) pnlRespuesta.Visible = true;

                GrdvRespuesta.DataSource = _dtbrespuestatemp;
                GrdvRespuesta.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelEfecto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = int.Parse(GrdvEfecto.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _codigoarbolaccion = int.Parse(GrdvEfecto.DataKeys[_gvrow.RowIndex].Values["CodigoAccion"].ToString());
                _dtbrespuesta = (DataTable)ViewState["ArbolRespuesta"];
                _result = _dtbrespuesta.Select("CodigoEfecto='" + _codigo + "'").FirstOrDefault();

                if (_result == null)
                {
                    _mensaje = new ArbolDecisionDAO().FunDelEfecto(_codigo, _codigoarbolaccion, int.Parse(ViewState["codigoCatalogo"].ToString()));

                    _dtbefecto = (DataTable)ViewState["ArbolEfecto"];

                    _result = _dtbefecto.Select("CodigoAccion='" + ViewState["CodigoAccion"].ToString() + "' and Codigo='" +
                        _codigo + "'").FirstOrDefault();

                    _result.Delete();
                    _dtbefecto.AcceptChanges();
                    ViewState["ArbolEfecto"] = _dtbefecto;
                    _dtbefectotemp = (DataTable)ViewState["ArbolEfectoTemp"];
                    _dtbefectotemp.Clear();

                    _dr = _dtbefecto.Select("CodigoAccion='" + ViewState["CodigoAccion"].ToString() + "'");

                    foreach (DataRow _fila in _dr)
                    {
                        _filagre = _dtbefectotemp.NewRow();
                        _filagre["Codigo"] = _fila[0].ToString();
                        _filagre["CodigoAccion"] = _fila[1].ToString();
                        _filagre["Descripcion"] = _fila[2].ToString();
                        _filagre["Estado"] = _fila[3].ToString();
                        _filagre["auxv1"] = _fila[4].ToString();
                        _filagre["auxv2"] = _fila[5].ToString();
                        _filagre["auxi1"] = _fila[6].ToString();
                        _filagre["auxi2"] = _fila[7].ToString();
                        _dtbefectotemp.Rows.Add(_filagre);
                        _dtbefectotemp.DefaultView.Sort = "Descripcion";
                    }

                    GrdvEfecto.DataSource = _dtbefectotemp;
                    GrdvEfecto.DataBind();
                    //contar si ya no hay registros
                    if (_dtbefectotemp.Rows.Count == 0)
                    {
                        pnlEfecto.Visible = false;
                        FunLimpiarCampos(1);
                    }
                }
                else new FuncionesDAO().FunShowJSMessage("Elimne antes la respuesta asociada..!", this);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }
        protected void ImgModiEfecto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ViewState["CodigoEfecto"] == null || int.Parse(ViewState["CodigoEfecto"].ToString()) == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione definición de Efecto..!", this);
                    return;
                }

                if (TxtEfecto.Text.Trim() == "")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Definición de Efecto..!", this);
                    return;
                }

                if (ViewState["ArbolEfecto"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["ArbolEfecto"];

                    if (ViewState["DescipcionEfecto"].ToString() != TxtEfecto.Text.Trim().ToUpper())
                    {
                        _result = _tblbuscar.Select("CodigoAccion='" + ViewState["CodigoAccion"].ToString() + "' and Descripcion='" + TxtEfecto.Text.Trim().ToUpper() + "'").FirstOrDefault();
                    }

                    _tblbuscar.DefaultView.Sort = "Codigo";
                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Ya existe definido Efecto..!", this);
                    return;
                }

                _dtbefecto = (DataTable)ViewState["ArbolEfecto"];
                _change = _dtbefecto.Select("Codigo='" + ViewState["CodigoEfecto"].ToString() + "'").FirstOrDefault();
                _change["Descripcion"] = TxtEfecto.Text.Trim().ToUpper();
                _change["auxi2"] = TxtDiaLatencia.Text;
                _dtbefecto.AcceptChanges();
                ViewState["ArbolEfecto"] = _dtbefecto;
                _dtbefectotemp = (DataTable)ViewState["ArbolEfectoTemp"];
                _dtbefectotemp.Clear();
                _dr = _dtbefecto.Select("CodigoAccion='" + ViewState["CodigoAccion"].ToString() + "'");

                foreach (DataRow _fila in _dr)
                {
                    _filagre = _dtbefectotemp.NewRow();
                    _filagre["Codigo"] = _fila[0].ToString();
                    _filagre["CodigoAccion"] = _fila[1].ToString();
                    _filagre["Descripcion"] = _fila[2].ToString();
                    _filagre["Estado"] = _fila[3].ToString();
                    _filagre["auxv1"] = _fila[4].ToString();
                    _filagre["auxv2"] = _fila[5].ToString();
                    _filagre["auxi1"] = _fila[6].ToString();
                    _filagre["auxi2"] = _fila[7].ToString();
                    _dtbefectotemp.Rows.Add(_filagre);
                    _dtbefectotemp.DefaultView.Sort = "Descripcion";
                }

                GrdvEfecto.DataSource = _dtbefectotemp;
                GrdvEfecto.DataBind();
                TxtEfecto.Text = "";
                TxtDiaLatencia.Text = "0";
                FunLimpiarCampos(1);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgAddRespuesta_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ViewState["CodigoEfecto"] == null || int.Parse(ViewState["CodigoEfecto"].ToString()) == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Definición de Efecto..!", this);
                    return;
                }

                if (TxtRespuesta.Text.Trim() == "")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Definición de Respuesta..!", this);
                    return;
                }

                if (ViewState["ArbolRespuesta"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["ArbolRespuesta"];

                    if (_tblbuscar.Rows.Count > 0)
                        _maxcodigo = _tblbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _result = _tblbuscar.Select("CodigoEfecto='" + ViewState["CodigoEfecto"].ToString() + "' and Descripcion='" +
                        TxtRespuesta.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Ya existe definida Respuesta..!", this);
                    return;
                }

                pnlRespuesta.Visible = true;
                _tblagre = (DataTable)ViewState["ArbolRespuesta"];
                _filagre = _tblagre.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["CodigoEfecto"] = ViewState["CodigoEfecto"].ToString();
                _filagre["Descripcion"] = TxtRespuesta.Text.Trim().ToUpper();
                _filagre["Estado"] = "Activo";
                _filagre["Pago"] = "NO";
                _filagre["Llamar"] = "NO";
                _filagre["auxv1"] = "";
                _filagre["auxv2"] = "";
                _filagre["auxi1"] = ViewState["codigoCatalogo"].ToString();
                _filagre["auxi2"] = "0";
                _tblagre.Rows.Add(_filagre);

                ViewState["ArbolRespuesta"] = _tblagre;
                _dtbrespuestatemp = (DataTable)ViewState["ArbolRespuestaTemp"];
                _dtbrespuestatemp.Clear();
                _drtemp = _tblagre.Select("CodigoEfecto='" + ViewState["CodigoEfecto"].ToString() + "'");

                foreach (DataRow _fila in _drtemp)
                {
                    _filagretem = _dtbrespuestatemp.NewRow();
                    _filagretem["Codigo"] = _fila[0].ToString();
                    _filagretem["CodigoEfecto"] = _fila[1].ToString();
                    _filagretem["Descripcion"] = _fila[2].ToString();
                    _filagretem["Estado"] = _fila[3].ToString();
                    _filagretem["Pago"] = _fila[4].ToString();
                    _filagretem["LLamar"] = _fila[5].ToString();
                    _filagretem["auxv1"] = _fila[6].ToString();
                    _filagretem["auxv2"] = _fila[7].ToString();
                    _filagretem["auxi1"] = _fila[8].ToString();
                    _filagretem["auxi2"] = _fila[9].ToString();
                    _dtbrespuestatemp.Rows.Add(_filagretem);
                }

                _dtbrespuestatemp.DefaultView.Sort = "Descripcion";
                _dtbrespuestatemp = _dtbrespuestatemp.DefaultView.ToTable();
                GrdvRespuesta.DataSource = _dtbrespuestatemp;
                GrdvRespuesta.DataBind();
                TxtRespuesta.Text = "";
                FunLimpiarCampos(2);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgModiRespuesta_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ViewState["CodigoRespuesta"] == null || int.Parse(ViewState["CodigoRespuesta"].ToString()) == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione definición de Respuesta..!", this);
                    return;
                }

                if (TxtRespuesta.Text.Trim() == "")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Definición de Respuesta..!", this);
                    return;
                }

                if (ViewState["ArbolRespuesta"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["ArbolRespuesta"];
                    _result = _tblbuscar.Select("CodigoEfecto='" + ViewState["CodigoEfecto"].ToString() + "' and Descripcion='" +
                        TxtRespuesta.Text.Trim().ToUpper() + "'").FirstOrDefault();
                    _tblbuscar.DefaultView.Sort = "Codigo";

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Ya existe definida Respuesta..!", this);
                    return;
                }

                _dtbrespuesta = (DataTable)ViewState["ArbolRespuesta"];
                _change = _dtbrespuesta.Select("Codigo='" + ViewState["CodigoRespuesta"].ToString() + "'").FirstOrDefault();
                _change["Descripcion"] = TxtRespuesta.Text.Trim().ToUpper();
                _dtbrespuesta.AcceptChanges();
                ViewState["ArbolRespuesta"] = _dtbrespuesta;

                _dtbrespuestatemp = (DataTable)ViewState["ArbolRespuestaTemp"];
                _dtbrespuestatemp.Clear();
                _dr = _dtbrespuesta.Select("CodigoEfecto='" + ViewState["CodigoEfecto"].ToString() + "'");

                foreach (DataRow _fila in _dr)
                {
                    _filagre = _dtbrespuestatemp.NewRow();
                    _filagre["Codigo"] = _fila[0].ToString();
                    _filagre["CodigoEfecto"] = _fila[1].ToString();
                    _filagre["Descripcion"] = _fila[2].ToString();
                    _filagre["Estado"] = _fila[3].ToString();
                    _filagre["Pago"] = _fila[4].ToString();
                    _filagre["LLamar"] = _fila[5].ToString();
                    _filagre["auxv1"] = _fila[6].ToString();
                    _filagre["auxv2"] = _fila[7].ToString();
                    _filagre["auxi1"] = _fila[8].ToString();
                    _filagre["auxi2"] = _fila[9].ToString();
                    _dtbrespuestatemp.Rows.Add(_filagre);
                    _dtbrespuestatemp.DefaultView.Sort = "Descripcion";
                }

                GrdvRespuesta.DataSource = _dtbrespuestatemp;
                GrdvRespuesta.DataBind();
                TxtRespuesta.Text = "";
                FunLimpiarCampos(2);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ChkEstRespuesta_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkestado = (CheckBox)(_gvrow.Cells[1].FindControl("chkEstRespuesta"));
                _dtbrespuesta = (DataTable)ViewState["ArbolRespuesta"];
                _codigo = int.Parse(GrdvRespuesta.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _result = _dtbrespuesta.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result["Estado"] = _chkestado.Checked ? "Activo" : "Inactivo";
                _dtbrespuesta.AcceptChanges();
                ViewState["ArbolRespuesta"] = _dtbrespuesta;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ChkPago_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkpago = (CheckBox)(_gvrow.Cells[2].FindControl("chkPago"));
                _chkllamar = (CheckBox)(_gvrow.Cells[3].FindControl("chkLlamar"));
                _dtbrespuesta = (DataTable)ViewState["ArbolRespuesta"];
                _codigo = int.Parse(GrdvRespuesta.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _result = _dtbrespuesta.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result["Pago"] = _chkpago.Checked ? "SI" : "NO";
                _result["Llamar"] = "NO";
                _chkllamar.Checked = false;
                _dtbrespuesta.AcceptChanges();
                ViewState["ArbolRespuesta"] = _dtbrespuesta;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ChkLlamar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkpago = (CheckBox)(_gvrow.Cells[2].FindControl("chkPago"));
                _chkllamar = (CheckBox)(_gvrow.Cells[3].FindControl("chkLlamar"));
                _dtbrespuesta = (DataTable)ViewState["ArbolRespuesta"];
                _codigo = int.Parse(GrdvRespuesta.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _result = _dtbrespuesta.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result["LLamar"] = _chkllamar.Checked ? "SI" : "NO";
                _result["Pago"] = "NO";
                _chkpago.Checked = false;
                _dtbrespuesta.AcceptChanges();
                ViewState["ArbolRespuesta"] = _dtbrespuesta;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgEdiRespuesta_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _dtbrespuesta = (DataTable)ViewState["ArbolRespuesta"];
                _codigo = int.Parse(GrdvRespuesta.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                ViewState["CodigoRespuesta"] = _codigo;
                _result = _dtbrespuesta.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtRespuesta.Text = _result["Descripcion"].ToString();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgSelecRespuesta_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["CodigoRespuesta"] = "0";
                ViewState["CodigoContacto"] = "0";
                TxtRespuesta.Text = "";
                TxtContacto.Text = "";

                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                pnlContacto.Visible = false;

                foreach (GridViewRow _fr in GrdvRespuesta.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvRespuesta.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                ViewState["CodigoRespuesta"] = int.Parse(GrdvRespuesta.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _dtbcontacto = (DataTable)ViewState["ArbolContacto"];
                _dtbcontactotemp = (DataTable)ViewState["ArbolContactoTemp"];
                _dtbcontactotemp.Clear();
                _dr = _dtbcontacto.Select("CodigoRespuesta='" + ViewState["CodigoRespuesta"].ToString() + "'");

                foreach (DataRow _fila in _dr)
                {
                    _filagre = _dtbcontactotemp.NewRow();
                    _filagre["Codigo"] = _fila[0].ToString();
                    _filagre["CodigoRespuesta"] = _fila[1].ToString();
                    _filagre["Descripcion"] = _fila[2].ToString();
                    _filagre["Estado"] = _fila[3].ToString();
                    _filagre["auxv1"] = _fila[4].ToString();
                    _filagre["auxv2"] = _fila[5].ToString();
                    _filagre["auxi1"] = _fila[6].ToString();
                    _filagre["auxi2"] = _fila[7].ToString();
                    _dtbcontactotemp.Rows.Add(_filagre);
                    _dtbcontactotemp.DefaultView.Sort = "Descripcion";
                }

                if (_dtbcontactotemp.Rows.Count > 0) pnlContacto.Visible = true;

                GrdvContacto.DataSource = _dtbcontactotemp;
                GrdvContacto.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgDelRespuesta_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = int.Parse(GrdvRespuesta.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _codigoarbolefecto = int.Parse(GrdvRespuesta.DataKeys[_gvrow.RowIndex].Values["CodigoEfecto"].ToString());
                _dtbcontacto = (DataTable)ViewState["ArbolContacto"];
                _result = _dtbcontacto.Select("CodigoRespuesta='" + _codigo + "'").FirstOrDefault();

                if (_result == null)
                {
                    _mensaje = new ArbolDecisionDAO().FunDelRespuesta(_codigo, _codigoarbolefecto,
                        int.Parse(ViewState["codigoCatalogo"].ToString()));

                    _dtbrespuesta = (DataTable)ViewState["ArbolRespuesta"];
                    _result = _dtbrespuesta.Select("CodigoEfecto='" + ViewState["CodigoEfecto"].ToString() + "' and Codigo='" +
                        _codigo + "'").FirstOrDefault();

                    _result.Delete();
                    ViewState["ArbolRespuesta"] = _dtbrespuesta;
                    _dtbrespuestatemp = (DataTable)ViewState["ArbolRespuestaTemp"];
                    _dtbrespuestatemp.Clear();

                    _dr = _dtbrespuesta.Select("CodigoEfecto='" + ViewState["CodigoEfecto"].ToString() + "'");

                    foreach (DataRow _fila in _dr)
                    {
                        _filagre = _dtbrespuestatemp.NewRow();
                        _filagre["Codigo"] = _fila[0].ToString();
                        _filagre["CodigoEfecto"] = _fila[1].ToString();
                        _filagre["Descripcion"] = _fila[2].ToString();
                        _filagre["Estado"] = _fila[3].ToString();
                        _filagre["Pago"] = _fila[4].ToString();
                        _filagre["LLamar"] = _fila[5].ToString();
                        _filagre["auxv1"] = _fila[6].ToString();
                        _filagre["auxv2"] = _fila[7].ToString();
                        _filagre["auxi1"] = _fila[8].ToString();
                        _filagre["auxi2"] = _fila[9].ToString();
                        _dtbrespuestatemp.Rows.Add(_filagre);
                        _dtbrespuestatemp.DefaultView.Sort = "Descripcion";
                    }

                    GrdvRespuesta.DataSource = _dtbrespuestatemp;
                    GrdvRespuesta.DataBind();

                    if (_dtbrespuestatemp.Rows.Count == 0)
                    {
                        pnlRespuesta.Visible = false;
                        FunLimpiarCampos(2);
                    }
                }
                else new FuncionesDAO().FunShowJSMessage("Elimine antes el Contacto asociado..!", this);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }
        protected void GrdvRespuesta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkestado = (CheckBox)(e.Row.Cells[1].FindControl("ChkEstRespuesta"));
                    _chkpago = (CheckBox)(e.Row.Cells[2].FindControl("ChkPago"));
                    _chkllamar = (CheckBox)(e.Row.Cells[3].FindControl("ChkLlamar"));
                    _chkefectivo = (CheckBox)(e.Row.Cells[4].FindControl("ChkEfectivo"));
                    _imgborrar = (ImageButton)(e.Row.Cells[7].FindControl("imgDelRespuesta"));
                    _codigo = int.Parse(GrdvRespuesta.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString());
                    _sql = "Select Estado=case arre_estado when 1 then 'Activo' else 'Inactivo' end,";
                    _sql += "Pago = case arre_pago when 1 then 'SI' else 'NO' end,Llamar = case arre_llamar when 1 then 'SI' else 'NO' end,";
                    _sql += "Efectivo = case arre_auxi2 when 1 then 'SI' else 'NO' end ";
                    _sql += "from GSBPO_RESPUESTA where arre_auxi1=" + ViewState["codigoCatalogo"].ToString() + " and ARRE_CODIGO=" + _codigo + " and ";
                    _sql += "AREF_CODIGO=" + ViewState["CodigoEfecto"].ToString();
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql, "", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        _chkestado.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                        _chkpago.Checked = _dts.Tables[0].Rows[0]["Pago"].ToString() == "SI" ? true : false;
                        _chkllamar.Checked = _dts.Tables[0].Rows[0]["Llamar"].ToString() == "SI" ? true : false;
                        _chkefectivo.Checked = _dts.Tables[0].Rows[0]["Efectivo"].ToString() == "SI" ? true : false;
                        _imgborrar.Enabled = false;
                        _imgborrar.ImageUrl = "~/Botones/eliminargris.png";
                    }
                    else
                    {
                        _chkestado.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgAddContacto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ViewState["CodigoRespuesta"] == null || int.Parse(ViewState["CodigoRespuesta"].ToString()) == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Definición de Respuesta..!", this);
                    return;
                }

                if (TxtContacto.Text.Trim() == "")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Definición de Contacto..!", this);
                    return;
                }

                if (ViewState["ArbolContacto"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["ArbolContacto"];

                    if (_tblbuscar.Rows.Count > 0)
                        _maxcodigo = _tblbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _result = _tblbuscar.Select("CodigoRespuesta='" + ViewState["CodigoRespuesta"].ToString() +
                        "' and Descripcion='" + TxtContacto.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Ya existe definido Contacto..!", this);
                    return;
                }

                pnlContacto.Visible = true;
                _tblagre = (DataTable)ViewState["ArbolContacto"];
                _filagre = _tblagre.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["CodigoRespuesta"] = ViewState["CodigoRespuesta"].ToString();
                _filagre["Descripcion"] = TxtContacto.Text.Trim().ToUpper();
                _filagre["Estado"] = "Activo";
                _filagre["auxv1"] = "";
                _filagre["auxv2"] = "";
                _filagre["auxi1"] = ViewState["codigoCatalogo"].ToString();
                _filagre["auxi2"] = "0";
                _tblagre.Rows.Add(_filagre);
                ViewState["ArbolContacto"] = _tblagre;
                _dtbcontactotemp = (DataTable)ViewState["ArbolContactoTemp"];
                _dtbcontactotemp.Clear();
                _drtemp = _tblagre.Select("CodigoRespuesta='" + ViewState["CodigoRespuesta"].ToString() + "'");

                foreach (DataRow _fila in _drtemp)
                {
                    _filagretem = _dtbcontactotemp.NewRow();
                    _filagretem["Codigo"] = _fila[0].ToString();
                    _filagretem["CodigoRespuesta"] = _fila[1].ToString();
                    _filagretem["Descripcion"] = _fila[2].ToString();
                    _filagretem["Estado"] = _fila[3].ToString();
                    _filagretem["auxv1"] = _fila[4].ToString();
                    _filagretem["auxv2"] = _fila[5].ToString();
                    _filagretem["auxi1"] = _fila[6].ToString();
                    _filagretem["auxi2"] = _fila[7].ToString();
                    _dtbcontactotemp.Rows.Add(_filagretem);
                }

                _dtbcontactotemp.DefaultView.Sort = "Descripcion";
                _dtbcontactotemp = _dtbcontactotemp.DefaultView.ToTable();
                GrdvContacto.DataSource = _dtbcontactotemp;
                GrdvContacto.DataBind();
                TxtContacto.Text = "";
                FunLimpiarCampos(3);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgModiContacto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ViewState["CodigoContacto"] == null || int.Parse(ViewState["CodigoContacto"].ToString()) == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione definición de Contacto..!", this);
                    return;
                }

                if (TxtContacto.Text.Trim() == "")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Definición de Contacto..!", this);
                    return;
                }

                if (ViewState["ArbolContacto"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["ArbolContacto"];
                    _result = _tblbuscar.Select("CodigoRespuesta='" + ViewState["CodigoRespuesta"].ToString() +
                        "' and Descripcion='" + TxtContacto.Text.Trim().ToUpper() + "'").FirstOrDefault();
                    _tblbuscar.DefaultView.Sort = "Codigo";

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Ya existe definido Contacto..!", this);
                    return;
                }

                _dtbcontacto = (DataTable)ViewState["ArbolContacto"];
                _change = _dtbcontacto.Select("Codigo='" + ViewState["CodigoContacto"].ToString() + "'").FirstOrDefault();
                _change["Descripcion"] = TxtContacto.Text.Trim().ToUpper();
                _dtbcontacto.AcceptChanges();
                ViewState["ArbolContacto"] = _dtbcontacto;
                _dtbcontactotemp = (DataTable)ViewState["ArbolContactoTemp"];
                _dtbcontactotemp.Clear();
                _dr = _dtbcontacto.Select("CodigoRespuesta='" + ViewState["CodigoRespuesta"].ToString() + "'");

                foreach (DataRow _fila in _dr)
                {
                    _filagre = _dtbcontactotemp.NewRow();
                    _filagre["Codigo"] = _fila[0].ToString();
                    _filagre["CodigoRespuesta"] = _fila[1].ToString();
                    _filagre["Descripcion"] = _fila[2].ToString();
                    _filagre["Estado"] = _fila[3].ToString();
                    _filagre["auxv1"] = _fila[4].ToString();
                    _filagre["auxv2"] = _fila[5].ToString();
                    _filagre["auxi1"] = _fila[6].ToString();
                    _filagre["auxi2"] = _fila[7].ToString();
                    _dtbcontactotemp.Rows.Add(_filagre);
                    _dtbcontactotemp.DefaultView.Sort = "Descripcion";
                }

                GrdvContacto.DataSource = _dtbcontactotemp;
                GrdvContacto.DataBind();
                TxtContacto.Text = "";
                FunLimpiarCampos(3);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEstContacto_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkestado = (CheckBox)(_gvrow.Cells[1].FindControl("chkEstContacto"));
                _dtbcontacto = (DataTable)ViewState["ArbolContacto"];
                _codigo = int.Parse(GrdvContacto.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _result = _dtbcontacto.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result["Estado"] = _chkestado.Checked ? "Activo" : "Inactivo";
                _dtbcontacto.AcceptChanges();
                ViewState["ArbolContacto"] = _dtbcontacto;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEdiContacto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _dtbcontacto = (DataTable)ViewState["ArbolContacto"];
                _codigo = int.Parse(GrdvContacto.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                ViewState["CodigoContacto"] = _codigo;
                _result = _dtbcontacto.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtContacto.Text = _result["Descripcion"].ToString();
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
                _codigo = int.Parse(GrdvContacto.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _codigoarbolrespuesta = int.Parse(GrdvContacto.DataKeys[_gvrow.RowIndex].Values["CodigoRespuesta"].ToString());

                _mensaje = new ArbolDecisionDAO().FunDelContacto(_codigo, _codigoarbolrespuesta,
                    int.Parse(ViewState["codigoCatalogo"].ToString()));

                _dtbcontacto = (DataTable)ViewState["ArbolContacto"];
                _result = _dtbcontacto.Select("CodigoRespuesta='" + ViewState["CodigoRespuesta"].ToString() +
                    "' and Codigo='" + _codigo + "'").FirstOrDefault();
                _result.Delete();
                _dtbcontacto.AcceptChanges();

                ViewState["ArbolContacto"] = _dtbcontacto;
                _dtbcontactotemp = (DataTable)ViewState["ArbolContactoTemp"];
                _dtbcontactotemp.Clear();

                _dr = _dtbcontacto.Select("CodigoRespuesta='" + ViewState["CodigoRespuesta"].ToString() + "'");

                foreach (DataRow _fila in _dr)
                {
                    _filagre = _dtbcontactotemp.NewRow();
                    _filagre["Codigo"] = _fila[0].ToString();
                    _filagre["CodigoRespuesta"] = _fila[1].ToString();
                    _filagre["Descripcion"] = _fila[2].ToString();
                    _filagre["Estado"] = _fila[3].ToString();
                    _filagre["auxv1"] = _fila[4].ToString();
                    _filagre["auxv2"] = _fila[5].ToString();
                    _filagre["auxi1"] = _fila[6].ToString();
                    _filagre["auxi2"] = _fila[7].ToString();
                    _dtbcontactotemp.Rows.Add(_filagre);
                    _dtbcontactotemp.DefaultView.Sort = "Descripcion";
                }

                GrdvContacto.DataSource = _dtbcontactotemp;
                GrdvContacto.DataBind();

                if (_dtbcontactotemp.Rows.Count == 0)
                {
                    pnlContacto.Visible = false;
                    FunLimpiarCampos(3);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void GrdvContacto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkestado = (CheckBox)(e.Row.Cells[1].FindControl("ChkEstContacto"));
                    _imgborrar = (ImageButton)(e.Row.Cells[3].FindControl("ImgDelContacto"));
                    _codigo = int.Parse(GrdvContacto.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString());
                    _sql = "Select Estado=case arco_estado when 1 then 'Activo' else 'Inactivo' end from GSBPO_CONTACTO where ";
                    _sql += "arco_auxi1=" + ViewState["codigoCatalogo"].ToString() + " and ARCO_CODIGO=" + _codigo + " and ";
                    _sql += "ARRE_CODIGO=" + ViewState["CodigoRespuesta"].ToString();
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql, "", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        _chkestado.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                        _imgborrar.Enabled = false;
                        _imgborrar.ImageUrl = "~/Botones/eliminargris.png";
                    }
                    else
                    {
                        _chkestado.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnAsignar_Click(object sender, EventArgs e)
        {
            try
            {
                _dtbaccion = (DataTable)ViewState["ArbolAccion"];

                if (_dtbaccion.Rows.Count == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Agregue al menos definción de Acción..!", this);
                    return;
                }
                else
                {
                    _dtbefecto = (DataTable)ViewState["ArbolEfecto"];
                    _dtbefectotemp = (DataTable)ViewState["ArbolEfectoTemp"];
                    _dtbrespuesta = (DataTable)ViewState["ArbolRespuesta"];
                    _dtbrespuestatemp = (DataTable)ViewState["ArbolRespuestaTemp"];
                    _dtbcontacto = (DataTable)ViewState["ArbolContacto"];
                    _dtbcontactotemp = (DataTable)ViewState["ArbolContactoTemp"];

                    foreach (DataRow _draccion in _dtbaccion.Rows)
                    {
                        _codigoarbolaccion = new ArbolDecisionDAO().FunGetCodigoAccion(int.Parse(ViewState["codigoCatalogo"].ToString()),
                            _draccion[2].ToString());

                        _estadoact = _draccion[3].ToString() == "Activo" ? true : false;
                        _contactoact = _draccion[4].ToString() == "SI" ? true : false;
                        _descripcion = _draccion[2].ToString();
                        _dtbefectotemp.Clear();
                        _dtbrespuestatemp.Clear();
                        _dtbcontactotemp.Clear();

                        _dr = _dtbefecto.Select("CodigoAccion='" + _draccion[0].ToString() + "'");

                        foreach (DataRow _fila in _dr)
                        {
                            _filagre = _dtbefectotemp.NewRow();
                            _filagre["Codigo"] = _fila[0].ToString();
                            _filagre["CodigoAccion"] = _fila[1].ToString();
                            _filagre["Descripcion"] = _fila[2].ToString();
                            _filagre["Estado"] = _fila[3].ToString() == "Activo" ? true : false;
                            _filagre["auxv1"] = _fila[4].ToString();
                            _filagre["auxv2"] = _fila[5].ToString();
                            _filagre["auxi1"] = _fila[6].ToString();
                            _filagre["auxi2"] = _fila[7].ToString();

                            _dtbefectotemp.Rows.Add(_filagre);
                            _dtbefectotemp.DefaultView.Sort = "Descripcion";
                            _drtemp = _dtbrespuesta.Select("CodigoEfecto='" + _fila[0].ToString() + "'");

                            foreach (DataRow _filrow in _drtemp)
                            {
                                _filagretem = _dtbrespuestatemp.NewRow();
                                _filagretem["Codigo"] = _filrow[0].ToString();
                                _filagretem["CodigoEfecto"] = _filrow[1].ToString();
                                _filagretem["Descripcion"] = _filrow[2].ToString();
                                _filagretem["Estado"] = _filrow[3].ToString() == "Activo" ? true : false;
                                _filagretem["Pago"] = _filrow[4].ToString() == "SI" ? true : false;
                                _filagretem["LLamar"] = _filrow[5].ToString() == "SI" ? true : false;
                                _filagretem["auxv1"] = _filrow[6].ToString();
                                _filagretem["auxv2"] = _filrow[7].ToString();
                                _filagretem["auxi1"] = _filrow[8].ToString();
                                _filagretem["auxi2"] = _filrow[9].ToString();
                                _dtbrespuestatemp.Rows.Add(_filagretem);
                                _dtbrespuestatemp.DefaultView.Sort = "Descripcion";
                                _drtempx = _dtbcontacto.Select("CodigoRespuesta='" + _filrow[0].ToString() + "'");

                                foreach (DataRow _filfil in _drtempx)
                                {
                                    _filagretemx = _dtbcontactotemp.NewRow();
                                    _filagretemx["Codigo"] = _filfil[0].ToString();
                                    _filagretemx["CodigoRespuesta"] = _filfil[1].ToString();
                                    _filagretemx["Descripcion"] = _filfil[2].ToString();
                                    _filagretemx["Estado"] = _filfil[3].ToString() == "Activo" ? true : false;
                                    _filagretemx["auxv1"] = _filfil[4].ToString();
                                    _filagretemx["auxv2"] = _filfil[5].ToString();
                                    _filagretemx["auxi1"] = _filfil[6].ToString();
                                    _filagretemx["auxi2"] = _filfil[7].ToString();
                                    _dtbcontactotemp.Rows.Add(_filagretemx);
                                    _dtbcontactotemp.DefaultView.Sort = "Descripcion";
                                }
                            }
                        }

                        _mensaje = new ConsultaDatosDAO().FunCrearArbolDecision(int.Parse(ViewState["codigoCatalogo"].ToString()),
                            _codigoarbolaccion, _descripcion, _estadoact, _contactoact, _draccion[5].ToString(), _draccion[6].ToString(), int.Parse(_draccion[7].ToString()), int.Parse(_draccion[8].ToString()), int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), _dtbefectotemp, _dtbrespuestatemp, _dtbcontactotemp, "sp_NewAccionEfectoResp", Session["Conectar"].ToString());

                        _response = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");

                        if (_mensaje == "") Response.Redirect(_response, false);
                        else Lblerror.Text = _mensaje;
                    }
                }
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