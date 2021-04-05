namespace SoftCob.Views.ArbolDesicion
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
    public partial class WFrm_NuevoSpeech : Page
    {
        #region Variable
        DataSet _dts = new DataSet();
        DataTable _dtbspeech = new DataTable();
        ListItem _efecto = new ListItem();
        ListItem _respuesta = new ListItem();
        ListItem _contacto = new ListItem();
        DataTable _tblbuscar = new DataTable();
        DataTable _tblagre = new DataTable();
        CheckBox _chkestado = new CheckBox();
        DataRow _filagre;
        DataRow _result;
        int _idciudad = 0, _maxcodigo = 0, _codigo = 0, i = 0;
        bool _lexiste = false;
        string _response = "";
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

                ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];

                _dtbspeech.Columns.Add("Codigo");
                _dtbspeech.Columns.Add("codigoARAC");
                _dtbspeech.Columns.Add("Accion");
                _dtbspeech.Columns.Add("codigoAREF");
                _dtbspeech.Columns.Add("Efecto");
                _dtbspeech.Columns.Add("codigoARRE");
                _dtbspeech.Columns.Add("Respuesta");
                _dtbspeech.Columns.Add("codigoARCO");
                _dtbspeech.Columns.Add("Contacto");
                _dtbspeech.Columns.Add("Speech");
                _dtbspeech.Columns.Add("Observacion");
                _dtbspeech.Columns.Add("Estado");
                _dtbspeech.Columns.Add("auxv1");
                _dtbspeech.Columns.Add("auxv2");
                _dtbspeech.Columns.Add("auxi1");
                _dtbspeech.Columns.Add("auxi2");
                ViewState["SpeechArbol"] = _dtbspeech;
                ViewState["codigoCPCE"] = "0";
                ViewState["codigoCEDE"] = "0";
                FunCargarCombos(0);
                FunCargarCombos(1);

                Lbltitulo.Text = "Administrar Speech Catálogo";

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
                    foreach (DataRow dr in _dts.Tables[0].Rows)
                    {
                        _idciudad = int.Parse(dr[1].ToString());
                        TreeNode node = new TreeNode(dr[0].ToString(), dr[1].ToString());
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
                    foreach (DataRow drFila in _dts.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(drFila[0].ToString(), drFila[1].ToString());
                        unNode = FunAgregarProductoCedentes(unNode, int.Parse(drFila[1].ToString()));
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
                    foreach (DataRow drFila in _dts.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(drFila[0].ToString(), drFila[1].ToString());
                        unNode = FunAgregarCatalogoProductos(unNode, int.Parse(drFila[1].ToString()));
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
                    foreach (DataRow drFila in _dts.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(drFila[0].ToString(), drFila[1].ToString());
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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(180, int.Parse(ViewState["codigoCEDE"].ToString()), 0, 0,
                    "", "", "", ViewState["Conectar"].ToString());

                ViewState["NivelArbol"] = _dts.Tables[0].Rows[0]["Nivel"].ToString();


                ViewState["CodigoSpeechCab"] = "0";

                lblEstado.Visible = false;
                ChkEstadoB.Visible = false;
                ChkEstadoB.Checked = true;
                TxtEditor1.Content = "";
                TxtEditor2.Content = "";
                ImgAddSpeech.Enabled = true;
                ImgModiSpeech.Enabled = false;
                GrdvSpeech.DataSource = null;
                GrdvSpeech.DataBind();

                SoftCob_SPEECH_CABECERA _datos = new ArbolDecisionDAO().FunGetSpeechPorID(int.Parse(ViewState["codigoCPCE"].ToString()));

                if (_datos != null)
                {
                    ViewState["CodigoSpeechCab"] = _datos.SPCA_CODIGO;

                    TxtEditor1.Content = _datos.spca_speechbv;
                    ChkEstadoB.Visible = true;
                    lblEstado.Visible = true;
                    ChkEstadoB.Checked = _datos.spca_estado;
                    ChkEstadoB.Text = _datos.spca_estado ? "Activo" : "Inactivo";
                    ViewState["usucreacion"] = _datos.spca_usuariocreacion;
                    ViewState["fechacreacion"] = _datos.spca_fechacreacion;
                    ViewState["terminalcreacion"] = _datos.spca_terminalcreacion;

                    switch (ViewState["NivelArbol"].ToString())
                    {
                        case "3":
                            _dts = new ArbolDecisionDAO().FunGetArbolSpeechDet1(_datos.SPCA_CODIGO);
                            break;
                        case "4":
                            _dts = new ArbolDecisionDAO().FunGetArbolSpeechDet(_datos.SPCA_CODIGO);
                            break;
                    }

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        ViewState["SpeechArbol"] = _dts.Tables[0];
                        GrdvSpeech.DataSource = _dts;
                        GrdvSpeech.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void FunLimpiarObjects()
        {
            try
            {
                ViewState["codigoCPCE"] = "0";
                ViewState["codigoCEDE"] = "0";

                _dtbspeech.Clear();
                _dtbspeech.Columns.Add("Codigo");
                _dtbspeech.Columns.Add("codigoARAC");
                _dtbspeech.Columns.Add("Accion");
                _dtbspeech.Columns.Add("codigoAREF");
                _dtbspeech.Columns.Add("Efecto");
                _dtbspeech.Columns.Add("codigoARRE");
                _dtbspeech.Columns.Add("Respuesta");
                _dtbspeech.Columns.Add("codigoARCO");
                _dtbspeech.Columns.Add("Contacto");
                _dtbspeech.Columns.Add("Speech");
                _dtbspeech.Columns.Add("Observacion");
                _dtbspeech.Columns.Add("Estado");
                _dtbspeech.Columns.Add("auxv1");
                _dtbspeech.Columns.Add("auxv2");
                _dtbspeech.Columns.Add("auxi1");
                _dtbspeech.Columns.Add("auxi2");

                ViewState["SpeechArbol"] = _dtbspeech;
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
                    case 1:
                        LstCamposA.DataSource = new SpeechDAO().FunGetCamposSpeech();
                        LstCamposA.DataTextField = "casp_etiqueta";
                        LstCamposA.DataBind();

                        LstCamposB.DataSource = new SpeechDAO().FunGetCamposSpeech();
                        LstCamposB.DataTextField = "casp_etiqueta";
                        LstCamposB.DataBind();
                        break;
                    case 2:
                        DdlAccion.DataSource = new SpeechDAO().FunGetArbolAccion(int.Parse(ViewState["codigoCPCE"].ToString()));
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
                        DdlEfecto.DataSource = new SpeechDAO().FunGetArbolEfecto(int.Parse(DdlAccion.SelectedValue));
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
                        DdlRespuesta.DataSource = new SpeechDAO().FunGetArbolRespuesta(int.Parse(DdlEfecto.SelectedValue));
                        DdlRespuesta.DataTextField = "Descripcion";
                        DdlRespuesta.DataValueField = "Codigo";
                        DdlRespuesta.DataBind();
                        break;
                    case 5:
                        DdlContacto.Items.Clear();
                        _contacto.Text = "--Seleccione Contacto--";
                        _contacto.Value = "0";
                        DdlContacto.Items.Add(_contacto);
                        DdlContacto.DataSource = new SpeechDAO().FunGetArbolContacto(int.Parse(DdlRespuesta.SelectedValue));
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
        protected void TrvCedentes_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            switch (e.Node.Depth)
            {
                case 0:
                    FunLlenarCiudadCedente(e.Node);
                    break;
            }
        }

        protected void TrvCedentes_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeView arbolPres = (TreeView)(sender);
            TreeNode node = arbolPres.SelectedNode;
            try
            {
                ViewState["codigoCPCE"] = "0";

                switch (node.Depth)
                {
                    case 1:
                        ViewState["codigoCPCE"] = "0";
                        Lbltitulo.Text = "SPEECH Catálogo: ";
                        break;
                    case 2:
                        ViewState["codigoCPCE"] = "0";
                        break;
                    case 3:
                        ViewState["codigoCPCE"] = "0";
                        break;
                    case 4:
                        FunLimpiarObjects();
                        string[] pathRoot = node.ValuePath.Split(new char[] { '/' });
                        ViewState["codigoCPCE"] = pathRoot[5].ToString();
                        ViewState["codigoCEDE"] = pathRoot[3].ToString();
                        lblCatalogo.InnerText = "SPEECH Catálogo >>" + new CedenteDAO().FunGetNameCatalogoporID(int.Parse(pathRoot[5].ToString()));
                        FunCargarCombos(2);
                        FunCargarMantenimiento();
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void BtnPasar0_Click(object sender, EventArgs e)
        {
            i = LstCamposA.SelectedIndex;

            if (i >= 0)
            {
                TxtEditor1.Content += LstCamposA.Items[i].ToString();
            }
            else new FuncionesDAO().FunShowJSMessage("Seleccione Campo..", this);
        }

        protected void BtnPasar1_Click(object sender, EventArgs e)
        {
            i = LstCamposB.SelectedIndex;

            if (i >= 0)
            {
                TxtEditor2.Content += LstCamposB.Items[i].ToString();
            }
            else new FuncionesDAO().FunShowJSMessage("Seleccione Campo..", this);
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

        protected void ImgAddSpeech_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (int.Parse(ViewState["codigoCPCE"].ToString()) == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Catálogo del Cedente..", this);
                    return;
                }

                if (DdlAccion.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Acción..", this);
                    return;
                }

                if (DdlEfecto.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Efecto..", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtEditor2.Content) && string.IsNullOrEmpty(txtObservacion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Observación o Speech..", this);
                    return;
                }

                if (ViewState["SpeechArbol"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["SpeechArbol"];
                    _result = _tblbuscar.Select("codigoARAC='" + DdlAccion.SelectedValue + "' and codigoAREF='" + DdlEfecto.SelectedValue + "' and codigoARRE='" + DdlRespuesta.SelectedValue + "' and codigoARCO='" + DdlContacto.SelectedValue + "'").FirstOrDefault();

                    _tblbuscar.DefaultView.Sort = "Codigo";

                    if (_result != null) _lexiste = true;
                    else
                    {
                        if (_tblbuscar.Rows.Count > 0)
                            _maxcodigo = _tblbuscar.AsEnumerable()
                                .Max(row => int.Parse((string)row["Codigo"]));
                        else _maxcodigo = 0;
                    }
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Ya existe definido Speech..", this);
                    return;
                }

                _tblagre = new DataTable();
                _tblagre = (DataTable)ViewState["SpeechArbol"];
                _filagre = _tblagre.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["codigoARAC"] = DdlAccion.SelectedValue;
                _filagre["Accion"] = DdlAccion.SelectedItem.ToString();
                _filagre["codigoAREF"] = DdlEfecto.SelectedValue;
                _filagre["Efecto"] = int.Parse(DdlEfecto.SelectedValue) == 0 ? "" : DdlEfecto.SelectedItem.ToString();
                _filagre["codigoARRE"] = DdlRespuesta.SelectedValue;
                _filagre["Respuesta"] = int.Parse(DdlRespuesta.SelectedValue) == 0 ? "" : DdlRespuesta.SelectedItem.ToString();
                _filagre["codigoARCO"] = DdlContacto.SelectedValue;
                _filagre["Contacto"] = int.Parse(DdlContacto.SelectedValue) == 0 ? "" : DdlContacto.SelectedItem.ToString();
                _filagre["Speech"] = TxtEditor2.Content;
                _filagre["Observacion"] = txtObservacion.Text.Trim().ToUpper();
                _filagre["Estado"] = "Activo";
                _filagre["auxv1"] = "";
                _filagre["auxv2"] = "";
                _filagre["auxi1"] = "0";
                _filagre["auxi2"] = "0";
                _tblagre.Rows.Add(_filagre);
                _tblagre.DefaultView.Sort = "Accion";
                _tblagre = _tblagre.DefaultView.ToTable();
                ViewState["SpeechArbol"] = _tblagre;
                GrdvSpeech.DataSource = _tblagre;
                GrdvSpeech.DataBind();
                TxtEditor2.Content = "";
                txtObservacion.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvSpeech_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkestado = (CheckBox)(e.Row.Cells[4].FindControl("chkEstadoD"));
                    _codigo = int.Parse(GrdvSpeech.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString());
                    _dts = new ArbolDecisionDAO().FunGetArbolSpeechDetPorID(int.Parse(ViewState["CodigoSpeechCab"].ToString()), _codigo);

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        _chkestado.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
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

        protected void ChkEstadoD_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkestado = (CheckBox)(gvRow.Cells[4].FindControl("chkEstadoD"));
                _dtbspeech = (DataTable)ViewState["SpeechArbol"];
                _codigo = int.Parse(GrdvSpeech.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                _result = _dtbspeech.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result["Estado"] = _chkestado.Checked ? "Activo" : "Inactivo";
                _dtbspeech.AcceptChanges();
                ViewState["SpeechArbol"] = _dtbspeech;
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

                foreach (GridViewRow _fr in GrdvSpeech.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvSpeech.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _dtbspeech = (DataTable)ViewState["SpeechArbol"];
                _codigo = int.Parse(GrdvSpeech.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                ViewState["CodigoSpeechDeta"] = _codigo;
                _result = _dtbspeech.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtEditor2.Content = _result["Speech"].ToString();
                txtObservacion.Text = _result["Observacion"].ToString();
                DdlAccion.SelectedValue = _result["codigoARAC"].ToString();
                FunCargarCombos(3);
                DdlEfecto.SelectedValue = _result["codigoAREF"].ToString();
                FunCargarCombos(4);
                DdlRespuesta.SelectedValue = _result["codigoARRE"].ToString();
                FunCargarCombos(5);
                DdlContacto.SelectedValue = _result["codigoARCO"].ToString();
                ViewState["CodigoAccion"] = _result["codigoARAC"].ToString();
                ViewState["CodigoEfecto"] = _result["codigoAREF"].ToString();
                ViewState["CodigoRespuesta"] = _result["codigoARRE"].ToString();
                ViewState["CodigoContacto"] = _result["codigoARCO"].ToString();
                ImgAddSpeech.Enabled = false;
                ImgModiSpeech.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModiSpeech_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlAccion.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Acción..", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtEditor2.Content) && string.IsNullOrEmpty(txtObservacion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Observación o Speech..", this);
                    return;
                }

                if (ViewState["CodigoAccion"].ToString() != DdlAccion.SelectedValue || ViewState["CodigoEfecto"].ToString() != DdlEfecto.SelectedValue
                    || ViewState["CodigoRespuesta"].ToString() != DdlRespuesta.SelectedValue || ViewState["CodigoContacto"].ToString() != DdlContacto.SelectedValue)
                {
                    _tblbuscar = (DataTable)ViewState["SpeechArbol"];
                    _result = _tblbuscar.Select("codigoARAC='" + DdlAccion.SelectedValue + "' and codigoAREF='" + DdlEfecto.SelectedValue + "' and codigoARRE='" + DdlRespuesta.SelectedValue + "' and codigoARCO='" + DdlContacto.SelectedValue + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Ya existe definido Speech..", this);
                    return;
                }

                _dtbspeech = (DataTable)ViewState["SpeechArbol"];
                _result = _dtbspeech.Select("Codigo='" + ViewState["CodigoSpeechDeta"].ToString() + "'").FirstOrDefault();
                _result["codigoARAC"] = DdlAccion.SelectedValue;
                _result["Accion"] = DdlAccion.SelectedItem.ToString();
                _result["codigoAREF"] = DdlEfecto.SelectedValue;
                _result["Efecto"] = DdlEfecto.SelectedItem.ToString();
                _result["codigoARRE"] = DdlRespuesta.SelectedValue;
                _result["Respuesta"] = int.Parse(DdlRespuesta.SelectedValue) == 0 ? "" : DdlRespuesta.SelectedItem.ToString();
                _result["codigoARCO"] = DdlContacto.SelectedValue;
                _result["Contacto"] = int.Parse(DdlContacto.SelectedValue) == 0 ? "" : DdlContacto.SelectedItem.ToString();
                _result["Speech"] = TxtEditor2.Content;
                _result["Observacion"] = txtObservacion.Text.Trim().ToUpper();
                _dtbspeech.AcceptChanges();
                ViewState["SpeechArbol"] = _dtbspeech;
                GrdvSpeech.DataSource = _dtbspeech;
                GrdvSpeech.DataBind();
                FunCargarCombos(0);
                DdlAccion.SelectedValue = "0";
                TxtEditor2.Content = "";
                txtObservacion.Text = "";
                ImgAddSpeech.Enabled = true;
                ImgModiSpeech.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigo = int.Parse(GrdvSpeech.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());

            _response = new SpeechDAO().FunDelSpeech(_codigo);

            _dtbspeech = (DataTable)ViewState["SpeechArbol"];
            _result = _dtbspeech.Select("Codigo='" + _codigo + "'").FirstOrDefault();
            _result.Delete();
            _dtbspeech.AcceptChanges();
            ViewState["SpeechArbol"] = _dtbspeech;
            GrdvSpeech.DataSource = _dtbspeech;
            GrdvSpeech.DataBind();
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtEditor1.Content.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Speech de BienVenida..", this);
                    return;
                }

                if (int.Parse(ViewState["codigoCPCE"].ToString()) == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Catálogo del Cedente..", this);
                    return;
                }

                SoftCob_SPEECH_CABECERA _datos = new SoftCob_SPEECH_CABECERA();
                {
                    _datos.SPCA_CODIGO = int.Parse(ViewState["CodigoSpeechCab"].ToString());
                    _datos.spca_cedecodigo = int.Parse(ViewState["codigoCEDE"].ToString());
                    _datos.spca_cpcecodigo = int.Parse(ViewState["codigoCPCE"].ToString());
                    _datos.spca_speechbv = TxtEditor1.Content;
                    _datos.spca_estado = ChkEstadoB.Checked;
                    _datos.spca_auxv1 = "";
                    _datos.spca_auxv2 = "";
                    _datos.spca_auxi1 = 0;
                    _datos.spca_auxi2 = 0;
                    _datos.spca_fum = DateTime.Now;
                    _datos.spca_uum = int.Parse(Session["usuCodigo"].ToString());
                    _datos.spca_tum = Session["MachineName"].ToString();
                }

                if (int.Parse(ViewState["CodigoSpeechCab"].ToString()) == 0)
                {
                    _datos.spca_fechacreacion = DateTime.Now;
                    _datos.spca_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                    _datos.spca_terminalcreacion = Session["MachineName"].ToString();
                }
                else
                {
                    _datos.spca_fechacreacion = DateTime.Parse(ViewState["fechacreacion"].ToString());
                    _datos.spca_usuariocreacion = int.Parse(ViewState["usucreacion"].ToString());
                    _datos.spca_terminalcreacion = ViewState["terminalcreacion"].ToString();
                }

                _dtbspeech = (DataTable)ViewState["SpeechArbol"];

                if (_dtbspeech.Rows.Count > 0)
                {
                    List<SoftCob_SPEECH_DETALLE> datos1 = new List<SoftCob_SPEECH_DETALLE>();
                    foreach (DataRow dr in _dtbspeech.Rows)
                    {
                        _codigo = new ArbolDecisionDAO().FunGetCodigoSpeechDet(int.Parse(ViewState["CodigoSpeechCab"].ToString()),
                            int.Parse(dr["Codigo"].ToString()));

                        datos1.Add(new SoftCob_SPEECH_DETALLE()
                        {
                            SPDE_CODIGO = _codigo,
                            SPCA_CODIGO = int.Parse(ViewState["CodigoSpeechCab"].ToString()),
                            spde_araccodigo = int.Parse(dr["codigoARAC"].ToString()),
                            spde_arefcodigo = int.Parse(dr["codigoAREF"].ToString()),
                            spde_arrecodigo = int.Parse(dr["codigoARRE"].ToString()),
                            spde_arcocodigo = int.Parse(dr["codigoARCO"].ToString()),
                            spde_speechad = dr["Speech"].ToString(),
                            spde_observacion = dr["Observacion"].ToString(),
                            spde_estado = dr["Estado"].ToString() == "Activo" ? true : false,
                            spde_auxv1 = dr["auxv1"].ToString(),
                            spde_auxv2 = dr["auxv2"].ToString(),
                            spde_auxi1 = int.Parse(dr["auxi1"].ToString()),
                            spde_auxi2 = int.Parse(dr["auxi2"].ToString())
                        });
                    }

                    _datos.SoftCob_SPEECH_DETALLE = new List<SoftCob_SPEECH_DETALLE>();
                    foreach (SoftCob_SPEECH_DETALLE addDatos in datos1)
                    {
                        _datos.SoftCob_SPEECH_DETALLE.Add(addDatos);
                    }
                }

                if (_datos.SPCA_CODIGO == 0)
                {
                    _codigo = new ArbolDecisionDAO().FunCrearArbolSpeech(_datos);
                    ViewState["CodigoSpeechCab"] = _codigo;
                }
                else _codigo = new ArbolDecisionDAO().FunEditArbolSpeech(_datos);

                if (_codigo == -1) Lblerror.Text = "Error Inserción datos Arbol Sepeech..";
                else
                {
                    _response = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");
                    Response.Redirect(_response, true);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEstadoB_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstadoB.Text = ChkEstadoB.Checked ? "Activo" : "Inactivo";
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
        }
        #endregion
    }
}