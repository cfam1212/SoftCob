namespace SoftCob.Views.Evaluacion
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_EvaluacionAdmin : Page
    {
        #region Variables
        DataTable _dtbprotocolos = new DataTable();
        DataSet _dts = new DataSet();
        DataRow[] _result;
        DataRow _resultado;
        string[] _pathroot;
        int _maxcodigo = 0, _orden = 0;
        bool _existe = false;
        string _descripanterior = "", _response = "";
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
                    Lbltitulo.Text = "Administrar Protocolos << EVALUACIÓN - DESEMPEÑO >>";
                    FunProtocolos();

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
        private void FunProtocolos()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(156, 0, 0, 0, "", "", "", Session["Conectar"].ToString());
                ViewState["Protocolos"] = _dts.Tables[0];
                TrvProtocolos.Nodes.Clear();
                TreeNode node = new TreeNode("Protocolo-Evaluación", "0");
                node = FunLlenarProcolos(node);
                TrvProtocolos.Nodes.Add(node);
                TrvProtocolos.CollapseAll();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private TreeNode FunLlenarProcolos(TreeNode node)
        {
            try
            {
                _dtbprotocolos = (DataTable)ViewState["Protocolos"];
                _result = _dtbprotocolos.Select("CodigoPADRE='0'");

                foreach (DataRow _drfila in _result)
                {
                    TreeNode unnode = new TreeNode(_drfila["Descripcion"].ToString(), _drfila["CodigoPROTOCOLO"].ToString());
                    unnode = FunLlenarCalifacion(unnode, int.Parse(_drfila["CodigoPROTOCOLO"].ToString()));
                    node.ChildNodes.Add(unnode);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return node;
        }

        private TreeNode FunLlenarCalifacion(TreeNode node, int codigopadre)
        {
            try
            {
                _dtbprotocolos = (DataTable)ViewState["Protocolos"];
                _result = _dtbprotocolos.Select("CodigoPADRE='" + codigopadre + "'");

                foreach (DataRow _drfila in _result)
                {
                    TreeNode unnode = new TreeNode(_drfila["Descripcion"].ToString(), _drfila["CodigoProtocolo"].ToString());
                    unnode.Collapse();
                    node.ChildNodes.Add(unnode);
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
                _dtbprotocolos = (DataTable)ViewState["Protocolos"];
                _resultado = _dtbprotocolos.Select("CodigoPROTOCOLO='" + ViewState["CodigoProtocolo"].ToString() + "'").FirstOrDefault();
                TxtCalificacion.Text = _resultado["Calificacion"].ToString();
                LblEstado.Visible = true;
                ChkEtado.Visible = true;
                ChkEtado.Text = _resultado["Estado"].ToString();
                ChkEtado.Checked = _resultado["Estado"].ToString() == "Activo" ? true : false;
                _resultado = _dtbprotocolos.Select("CodigoPROTOCOLO='" + ViewState["CodigoPadre"].ToString() + "'").FirstOrDefault();
                LblProtocolo.InnerText = _resultado["Descripcion"].ToString();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void TrvProtocolos_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                TreeView treepress = (TreeView)(sender);
                TreeNode node = treepress.SelectedNode;
                TxtDescripcion.Text = "";
                TxtCalificacion.Text = "0";
                ImgAdd.Visible = false;
                ImgMod.Visible = false;
                ImgDel.Visible = false;

                switch (node.Depth)
                {
                    case 0:
                        _pathroot = node.ValuePath.Split(new char[] { '/' });
                        LblProtocolo.InnerText = _pathroot[0].ToString();
                        break;
                    case 1:
                        _pathroot = node.ValuePath.Split(new char[] { '/' });
                        ViewState["CodigoPadre"] = _pathroot[1].ToString();
                        LblProtocolo.InnerText = node.Text;
                        ImgAdd.Visible = true;
                        break;
                    case 2:
                        _pathroot = node.ValuePath.Split(new char[] { '/' });
                        ImgMod.Visible = true;
                        ImgDel.Visible = true;
                        ViewState["CodigoPadre"] = _pathroot[1].ToString();
                        ViewState["CodigoProtocolo"] = _pathroot[2].ToString();
                        TxtDescripcion.Text = node.Text;
                        FunCargarMantenimiento();
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEtado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEtado.Text = ChkEtado.Checked ? "Activo" : "Inactivo";
        }

        protected void ImgAdd_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //Obtener el siguiente Codigod de Protocolo
                if (string.IsNullOrEmpty(TxtDescripcion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Descripción..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtCalificacion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Calificacion..!", this);
                    return;
                }

                _dtbprotocolos = (DataTable)ViewState["Protocolos"];
                _resultado = _dtbprotocolos.Select("CodigoPADRE='" + ViewState["CodigoPadre"].ToString() + "' and " +
                    "Descripcion='" + TxtDescripcion.Text.Trim() + "'").FirstOrDefault();

                if (_resultado != null) _existe = true;

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Descripción ya Existe definida..!", this);
                    return;
                }

                if (_dtbprotocolos.Rows.Count > 0)
                {
                    _maxcodigo = _dtbprotocolos.AsEnumerable()
                        .Max(row => int.Parse((string)row["CodigoPROTOCOLO"])) + 1;
                    _orden = _dtbprotocolos.AsEnumerable()
                        .Max(row => int.Parse((string)row["Orden"])) + 1;
                }
                else
                {
                    _maxcodigo = 1;
                    _orden = 1;
                }

                _resultado = _dtbprotocolos.NewRow();
                _resultado["CodigoPREV"] = 0;
                _resultado["CodigoPROTOCOLO"] = _maxcodigo;
                _resultado["CodigoPADRE"] = ViewState["CodigoPadre"].ToString();
                _resultado["Descripcion"] = TxtDescripcion.Text.Trim();
                _resultado["Estado"] = "Activo";
                _resultado["Calificacion"] = TxtCalificacion.Text.Trim();
                _resultado["Nivel"] = "CAL";
                _resultado["Orden"] = _orden;
                _dtbprotocolos.Rows.Add(_resultado);
                _dtbprotocolos.DefaultView.Sort = "Orden";
                _dtbprotocolos = _dtbprotocolos.DefaultView.ToTable();
                ViewState["Protocolos"] = _dtbprotocolos;
                TreeNode node = new TreeNode(TxtDescripcion.Text.Trim(), _maxcodigo.ToString());
                TrvProtocolos.SelectedNode.ChildNodes.Add(node);
                TxtDescripcion.Text = "";
                TxtCalificacion.Text = "0";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgMod_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtDescripcion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Descripción..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtCalificacion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Calificacion..!", this);
                    return;
                }

                _dtbprotocolos = (DataTable)ViewState["Protocolos"];

                _resultado = _dtbprotocolos.Select("CodigoPROTOCOLO='" + ViewState["CodigoProtocolo"].ToString() + "'").FirstOrDefault();

                _descripanterior = _resultado["Descripcion"].ToString();

                if (_descripanterior != TxtDescripcion.Text.Trim())
                {
                    _resultado = _dtbprotocolos.Select("CodigoPADRE='" + ViewState["CodigoPadre"].ToString() + "' and " +
                        "Descripcion='" + TxtDescripcion.Text.Trim() + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;

                    if (_existe)
                    {
                        new FuncionesDAO().FunShowJSMessage("Descripción ya Existe definida..!", this);
                        return;
                    }
                }

                _resultado = _dtbprotocolos.Select("CodigoPROTOCOLO='" + ViewState["CodigoProtocolo"].ToString() + "'").FirstOrDefault();

                _resultado["Descripcion"] = TxtDescripcion.Text.Trim();
                _resultado["Estado"] = ChkEtado.Checked ? "Activo" : "Inactivo";
                _resultado["Calificacion"] = TxtCalificacion.Text.Trim();
                _dtbprotocolos.AcceptChanges();
                ViewState["Protocolos"] = _dtbprotocolos;

                TrvProtocolos.Nodes.Clear();
                TreeNode node = new TreeNode("Protocolo-Evaluación", "0");
                node = FunLlenarProcolos(node);
                TrvProtocolos.Nodes.Add(node);
                TrvProtocolos.ExpandAll();
                TxtDescripcion.Text = "";
                TxtCalificacion.Text = "0";
                LblProtocolo.InnerText = "";
                ImgMod.Visible = false;
                ImgDel.Visible = false;
                LblEstado.Visible = false;
                ChkEtado.Visible = false;
                ChkEtado.Checked = true;
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
                foreach (TreeNode nodes in TrvProtocolos.Nodes[0].ChildNodes)
                {
                    if (nodes.Value == ViewState["CodigoPadre"].ToString())
                    {
                        nodes.ChildNodes.Remove(TrvProtocolos.SelectedNode);
                        break;
                    }
                }

                _dtbprotocolos = (DataTable)ViewState["Protocolos"];

                _resultado = _dtbprotocolos.Select("CodigoPADRE='" + ViewState["CodigoPadre"].ToString() + "' and CodigoPROTOCOLO='" + ViewState["CodigoProtocolo"].ToString() + "'").FirstOrDefault();

                _resultado.Delete();
                _dtbprotocolos.AcceptChanges();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(157, int.Parse(ViewState["CodigoPadre"].ToString()),
                    int.Parse(ViewState["CodigoProtocolo"].ToString()), 0, "", "", "", Session["Conectar"].ToString());
                //TrvProtocolos.Nodes[0].ChildNodes[2].ChildNodes.Remove(TrvProtocolos.SelectedNode);
                TxtDescripcion.Text = "";
                TxtCalificacion.Text = "0";
                ImgAdd.Visible = false;
                ImgDel.Visible = false;
                ImgMod.Visible = false;
                LblProtocolo.InnerText = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnGragar_Click(object sender, EventArgs e)
        {
            try
            {
                _dtbprotocolos = (DataTable)ViewState["Protocolos"];

                foreach (DataRow _drfila in _dtbprotocolos.Rows)
                {
                    _dts = new ConsultaDatosDAO().FunNewProtocolo(0, int.Parse(_drfila["CodigoPREV"].ToString()),
                        int.Parse(_drfila["CodigoPROTOCOLO"].ToString()), int.Parse(_drfila["CodigoPADRE"].ToString()), _drfila["Descripcion"].ToString(), _drfila["Estado"].ToString(), int.Parse(_drfila["Calificacion"].ToString()), _drfila["Nivel"].ToString(), "", "", "", "", 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), Session["Conectar"].ToString());
                }

                _response = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");
                Response.Redirect(_response, false);
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