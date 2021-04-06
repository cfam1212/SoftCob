namespace SoftCob.Views.Cedente
{
    using ControllerSoftCob;
    using System;
    using System.Collections;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ReasignarCartera : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        int _idciudad = 0, _codigogestor = 0, _suma = 0, _operaciones = 0, _gestores = 0, _totalasignar = 0, _diferencia = 0,
            _totaloperaciones = 0, _contar = 0;
        CheckBox _chkselecc = new CheckBox();
        DataTable _dtbgestores = new DataTable();
        DataTable _dtbagregar = new DataTable();
        DataTable _dtbasignar = new DataTable();
        bool _lexiste = false, _asignado = false;
        string _sql = "", _sqlcli = "";
        DataRow[] _rows;
        DataTable _dtbbuscar;
        DataRow _result, _filagre;
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

                _dtbgestores.Columns.Add("GestorCodigo");
                _dtbgestores.Columns.Add("Gestor");
                _dtbgestores.Columns.Add("Operaciones");
                _dtbgestores.Columns.Add("SqlFormado");
                ViewState["GestoresAsignados"] = _dtbgestores;

                _dtbasignar.Columns.Add("DiasMora");
                _dtbasignar.Columns.Add("Operaciones");
                _dtbasignar.Columns.Add("Exigible");
                _dtbasignar.Columns.Add("Deuda");
                ViewState["AsignarSQL"] = _dtbasignar;
                Lbltitulo.Text = "ReAsignar Cartera";

                _dts = new ConsultaDatosDAO().FunConsultaDatos(71, -1, 0, 0, "", "", "", Session["Conectar"].ToString());
                GrdvDatosReasignar.DataSource = _dts;
                GrdvDatosReasignar.DataBind();

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimientos y Funciones
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

        protected void TrvCedentes_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeView arbol = (TreeView)(sender);
            TreeNode node = arbol.SelectedNode;
            try
            {
                switch (node.Depth)
                {
                    case 4:
                        TrvCedentes.SelectedNode.Expand();
                        string[] pathRoot = node.ValuePath.Split(new char[] { '/' });
                        ViewState["CodigoCedente"] = pathRoot[3].ToString();
                        ViewState["CodigoCatalago"] = pathRoot[5].ToString();
                        FunCargarCombos(0, int.Parse(pathRoot[3].ToString()));
                        FunClearObjects(0);
                        FunCargarDatos();
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void FunCargarDatos()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(71, int.Parse(ViewState["CodigoCatalago"].ToString()), 0, 0, "", "", "",
                    Session["Conectar"].ToString());
                GrdvDatosReasignar.DataSource = _dts;
                GrdvDatosReasignar.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunCargarCombos(int tipo, int codigo)
        {
            try
            {
                switch (tipo)
                {
                    case 0:
                        _dts = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--", codigo, 0, 0, "", "", "", 
                            Session["Conectar"].ToString());
                        DdlGestores.DataSource = _dts;
                        DdlGestores.DataTextField = "Descripcion";
                        DdlGestores.DataValueField = "Codigo";
                        DdlGestores.DataBind();

                        DdlGestorCli.DataSource = _dts;
                        DdlGestorCli.DataTextField = "Descripcion";
                        DdlGestorCli.DataValueField = "Codigo";
                        DdlGestorCli.DataBind();

                        DdlMotivo.DataSource = new ControllerDAO().FunGetParametroDetalle("MOTIVOS REASIGNAR", 
                            "--Seleccione Motivo--", "S");
                        DdlMotivo.DataTextField = "Descripcion";
                        DdlMotivo.DataValueField = "Codigo";
                        DdlMotivo.DataBind();

                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void FunClearObjects(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    LblDias.InnerText = "";
                    LblOperaciones.InnerText = "0";
                    RdbOpera.Checked = false;
                    RdbOpera.Enabled = false;
                    RdbClientes.Checked = false;
                    RdbClientes.Enabled = false;
                    RdbTodos.Checked = false;
                    RdbGestor.Checked = false;
                    RdbTodos.Enabled = false;
                    RdbGestor.Enabled = false;
                    DdlGestores.SelectedValue = "0";
                    DdlGestores.Enabled = false;
                    DdlGestorCli.SelectedValue = "0";
                    DdlGestorCli.Enabled = false;
                    TxtBuscar.Text = "";
                    TxtBuscar.Enabled = false;
                    TxtOperaciones.Text = "";
                    TxtOperaciones.Enabled = false;
                    LstOrigen.Items.Clear();
                    LstDestino.Items.Clear();
                    ImgPasar.Enabled = false;
                    ImgBuscar.Enabled = false;
                    ImgPasar1.Enabled = false;
                    ImgQuitar1.Enabled = false;
                    RdbDeudor.Checked = false;
                    RdbClientes.Checked = false;
                    RdbClientes.Enabled = false;
                    RdbIdentificacion.Enabled = false;
                    RdbIdentificacion.Checked = false;
                    _dtbgestores = null;
                    GrdvGestores.DataSource = _dtbgestores;
                    GrdvGestores.DataBind();
                    updCabecera.Update();
                    break;
                case 1:
                    LblCatalogo.InnerText = "";
                    _dtbasignar = null;
                    GrdvDiasAsignar.DataSource = _dtbasignar;
                    GrdvDiasAsignar.DataBind();
                    updCabecera.Update();
                    break;
            }
        }

        private void FunClearGestores()
        {
            _dtbgestores = (DataTable)ViewState["GestoresAsignados"];
            _rows = _dtbgestores.Select();

            foreach (DataRow _row in _rows)
            {
                _dtbgestores.Rows.Remove(_row);
            }

            ViewState["GestoresAsignados"] = _dtbgestores;
            GrdvGestores.DataSource = _dtbgestores;
            GrdvGestores.DataBind();
        }

        private void FunInsertarGestor(int codigoGestor, string gestor, int operaciones, string sqlformado)
        {
            try
            {
                if (ViewState["GestoresAsignados"] != null)
                {
                    _dtbbuscar = (DataTable)ViewState["GestoresAsignados"];
                    _result = _dtbbuscar.Select("GestorCodigo='" + codigoGestor + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Gestor ya está agregado..!", this);
                    return;
                }

                _dtbagregar = (DataTable)ViewState["GestoresAsignados"];
                _filagre = _dtbagregar.NewRow();
                _filagre["GestorCodigo"] = codigoGestor;
                _filagre["Gestor"] = gestor;
                _filagre["Operaciones"] = operaciones;
                _filagre["SqlFormado"] = sqlformado;
                _dtbagregar.Rows.Add(_filagre);
                ViewState["GestoresAsignados"] = _dtbagregar;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void ChkSelecc_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                FunClearObjects(0);
                FunClearObjects(1);

                foreach (GridViewRow _fr in GrdvDatosReasignar.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                    _fr.Cells[1].BackColor = System.Drawing.Color.White;
                    _fr.Cells[2].BackColor = System.Drawing.Color.White;
                    _fr.Cells[3].BackColor = System.Drawing.Color.White;
                    _chkselecc = (CheckBox)(_fr.Cells[4].FindControl("chkSelecc"));
                    _chkselecc.Checked = false;
                }

                _chkselecc = (CheckBox)(_gvrow.Cells[4].FindControl("chkSelecc"));
                _chkselecc.Checked = !_chkselecc.Checked;

                if (_chkselecc.Checked)
                {
                    GrdvDatosReasignar.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Bisque;
                    GrdvDatosReasignar.Rows[_gvrow.RowIndex].Cells[1].BackColor = System.Drawing.Color.Bisque;
                    GrdvDatosReasignar.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.Bisque;
                    GrdvDatosReasignar.Rows[_gvrow.RowIndex].Cells[3].BackColor = System.Drawing.Color.Bisque;
                    _codigogestor = int.Parse(GrdvDatosReasignar.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                    ViewState["CodigoGestor"] = _codigogestor;

                    LblCatalogo.InnerText = "Cartera Gestor: " + GrdvDatosReasignar.DataKeys[_gvrow.RowIndex].Values["Gestor"].ToString();

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(72, int.Parse(ViewState["CodigoCatalago"].ToString()), _codigogestor, 0, "", "", "", Session["Conectar"].ToString());

                    GrdvDiasAsignar.DataSource = _dts;
                    GrdvDiasAsignar.DataBind();
                }
                else
                {
                    GrdvDatosReasignar.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.White;
                    GrdvDatosReasignar.Rows[_gvrow.RowIndex].Cells[1].BackColor = System.Drawing.Color.White;
                    GrdvDatosReasignar.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.White;
                    GrdvDatosReasignar.Rows[_gvrow.RowIndex].Cells[3].BackColor = System.Drawing.Color.White;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void RdbOpera_CheckedChanged(object sender, EventArgs e)
        {
            RdbClientes.Checked = false;
            DdlGestorCli.SelectedValue = "0";
            DdlGestorCli.Enabled = false;
            TxtBuscar.Text = "";
            TxtBuscar.Enabled = false;
            RdbDeudor.Checked = true;
            RdbDeudor.Enabled = false;
            RdbIdentificacion.Checked = false;
            RdbIdentificacion.Enabled = false;
            LstOrigen.Items.Clear();
            LstDestino.Items.Clear();
            ImgPasar1.Enabled = false;
            ImgQuitar1.Enabled = false;
            RdbTodos.Enabled = true;
            RdbGestor.Enabled = true;
        }

        protected void RdbClientes_CheckedChanged(object sender, EventArgs e)
        {
            RdbOpera.Checked = false;
            RdbTodos.Checked = false;
            RdbTodos.Enabled = false;
            RdbGestor.Checked = false;
            RdbGestor.Enabled = false;
            DdlGestores.SelectedValue = "0";
            DdlGestores.Enabled = false;
            TxtOperaciones.Text = "";
            TxtOperaciones.Enabled = false;
            ImgPasar.Enabled = false;
            _dtbgestores = null;
            GrdvGestores.DataSource = _dtbgestores;
            GrdvGestores.DataBind();
            DdlGestorCli.Enabled = true;
            TxtBuscar.Enabled = true;
            ImgBuscar.Enabled = true;
            RdbDeudor.Enabled = true;
            RdbDeudor.Checked = true;
            RdbIdentificacion.Enabled = true;
            RdbIdentificacion.Checked = false;
            ImgPasar1.Enabled = true;
            ImgQuitar1.Enabled = true;
        }

        protected void RdbTodos_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Lblerror.Text = "";
                RdbGestor.Checked = false;
                FunClearGestores();
                DdlGestores.SelectedValue = "0";
                DdlGestores.Enabled = false;
                TxtOperaciones.Text = "";
                TxtOperaciones.Enabled = false;
                ImgPasar.Enabled = false;

                if (string.IsNullOrEmpty(LblOperaciones.InnerText))
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cartera para tener asignación..!", this);
                    return;
                }

                _operaciones = int.Parse(LblOperaciones.InnerText);

                _dts = new ConsultaDatosDAO().FunConsultaDatos(73, int.Parse(ViewState["CodigoCedente"].ToString()), int.Parse(ViewState["CodigoGestor"].ToString()), 0, "", "", "", Session["Conectar"].ToString());

                if (_dts != null && _dts.Tables[0].Rows.Count > 0)
                {
                    _gestores = _dts.Tables[0].Rows.Count;
                    _totalasignar = _operaciones / _gestores;
                    _diferencia = _operaciones - (_totalasignar * _gestores);
                    _contar = 0;

                    foreach (DataRow _dr in _dts.Tables[0].Rows)
                    {
                        _contar++;

                        if (_contar == _gestores) _totalasignar = _totalasignar + _diferencia;

                        _sql = "";
                        _sql += "Update GSBPO_CUENTA_DEUDOR set ctde_gestorasignado=" + _dr[1].ToString();
                        _sql += " from (select top " + _totalasignar.ToString() + " CD.ctde_gestorasignado, CL.CLDE_CODIGO,CD.ctde_operacion ";
                        _sql += "from GSBPO_CUENTA_DEUDOR CD (nolock) ";
                        _sql += "INNER JOIN GSBPO_CLIENTE_DEUDOR CL (nolock) ON CD.CLDE_CODIGO=CL.CLDE_CODIGO ";
                        _sql += "where CD.ctde_diasmora=" + LblDias.InnerText + " and CD.ctde_gestorasignado=" + ViewState["CodigoGestor"].ToString() + " and ";
                        _sql += "CL.CPCE_CODIGO=" + ViewState["CodigoCatalago"].ToString() + " and Cl.clde_estado=1 and ";
                        _sql += "CD.ctde_estado=1" + ") as d ";
                        _sql += "where d.CLDE_CODIGO = GSBPO_CUENTA_DEUDOR.CLDE_CODIGO and d.ctde_operacion=GSBPO_CUENTA_DEUDOR.ctde_operacion";
                        FunInsertarGestor(int.Parse(_dr[1].ToString()), _dr[0].ToString(), _totalasignar, _sql);
                    }

                    _dtbagregar = (DataTable)ViewState["GestoresAsignados"];
                    GrdvGestores.DataSource = _dtbagregar;
                    GrdvGestores.DataBind();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void RdbGestor_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Lblerror.Text = "";
                RdbTodos.Checked = false;
                FunClearGestores();
                LblOperaciones.InnerText = ViewState["TotalOperaciones"].ToString();
                DdlGestores.Enabled = true;
                TxtOperaciones.Enabled = true;
                ImgPasar.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgPasar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlGestores.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtOperaciones.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese No. operaciones a asignar..!", this);
                    return;
                }

                if (int.Parse(TxtOperaciones.Text) == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese cantidad de operaciones..!", this);
                    return;
                }

                if (int.Parse(TxtOperaciones.Text) > int.Parse(LblOperaciones.InnerText))
                {
                    new FuncionesDAO().FunShowJSMessage("La cantidad a asignar es mayor a las operaciones totales..!", this);
                    return;
                }

                _sql = "";
                _sql += "Update GSBPO_CUENTA_DEUDOR set ctde_gestorasignado=" + DdlGestores.SelectedValue;
                _sql += " from (select top " + TxtOperaciones.Text.Trim() + " CD.ctde_gestorasignado, CL.CLDE_CODIGO,CD.ctde_operacion ";
                _sql += "from GSBPO_CUENTA_DEUDOR CD (nolock) ";
                _sql += "INNER JOIN GSBPO_CLIENTE_DEUDOR CL (nolock) ON CD.CLDE_CODIGO=CL.CLDE_CODIGO ";
                _sql += "where CL.CPCE_CODIGO=" + ViewState["CodigoCatalago"].ToString() + " and CD.ctde_diasmora=" + LblDias.InnerText;
                _sql += " and CD.ctde_gestorasignado=" + ViewState["CodigoGestor"].ToString() + " and Cl.clde_estado=1 and ";
                _sql += "CD.ctde_estado=1" + ") as d ";
                _sql += "where d.CLDE_CODIGO = GSBPO_CUENTA_DEUDOR.CLDE_CODIGO and d.ctde_operacion=GSBPO_CUENTA_DEUDOR.ctde_operacion";

                FunInsertarGestor(int.Parse(DdlGestores.SelectedValue), DdlGestores.SelectedItem.ToString(), int.Parse(TxtOperaciones.Text.Trim()), _sql);

                _dtbagregar = (DataTable)ViewState["GestoresAsignados"];
                GrdvGestores.DataSource = _dtbagregar;
                GrdvGestores.DataBind();
                DdlGestores.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelGestor_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _dtbagregar = (DataTable)ViewState["GestoresAsignados"];
                _dtbagregar.Rows.RemoveAt(_gvrow.RowIndex);
                LblOperaciones.InnerText = ViewState["TotalOperaciones"].ToString();
                ViewState["GestoresAsignados"] = _dtbagregar;
                GrdvGestores.DataSource = _dtbagregar;
                GrdvGestores.DataBind();

                if (_dtbagregar.Rows.Count == 0) RdbTodos.Checked = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void RdbDeudor_CheckedChanged(object sender, EventArgs e)
        {
            RdbIdentificacion.Checked = false;
        }

        protected void RdbIdentificacion_CheckedChanged(object sender, EventArgs e)
        {
            RdbDeudor.Checked = false;
        }

        protected void ImgBuscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtBuscar.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Criterio de busqueda..!", this);
                    return;
                }

                if (DdlGestorCli.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this);
                    return;
                }

                if (!RdbDeudor.Checked && !RdbIdentificacion.Checked)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Busqueda Deudor o Identificación..!", this);
                    return;
                }

                _sqlcli = "select Cliente = PE.pers_primerapellido+' '+PE.pers_segundoapellido+' '+PE.pers_primernombre+' '+PE.pers_segundonombre,";
                _sqlcli += "CodigoCTDE = CD.CTDE_CODIGO from GSBPO_CUENTA_DEUDOR CD (nolock) INNER JOIN GSBPO_CLIENTE_DEUDOR CL (nolock) ON CD.CLDE_CODIGO=CL.CLDE_CODIGO ";
                _sqlcli += "INNER JOIN GSBPO_PERSONA PE (nolock) ON CL.PERS_CODIGO=PE.PERS_CODIGO where CL.CPCE_CODIGO=" + ViewState["CodigoCatalago"].ToString();
                _sqlcli += " and CD.ctde_gestorasignado=" + ViewState["CodigoGestor"].ToString() + " and CD.ctde_estado=1 and Cl.clde_estado=1 and ";

                if (RdbDeudor.Checked) _sqlcli += "PE.pers_nombrescompletos like '" + TxtBuscar.Text.Trim().ToUpper() + "%'";

                if (RdbIdentificacion.Checked) _sqlcli += "PE.pers_numerodocumento like '" + TxtBuscar.Text.Trim().ToUpper() + "%'";

                _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sqlcli, "", "", Session["Conectar"].ToString());

                LstOrigen.DataSource = _dts;
                LstOrigen.DataTextField = _dts.Tables[0].Columns[0].ColumnName;
                LstOrigen.DataValueField = _dts.Tables[0].Columns[1].ColumnName;
                LstOrigen.DataBind();
                new FuncionesDAO().FunOrdenar(LstOrigen);
                ViewState["DatosDeudores"] = _dts;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgPasar1_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ArrayList _elementoseliminar = new ArrayList();

                foreach (ListItem _item in LstOrigen.Items)
                {
                    if (_item.Selected == true)
                    {
                        LstDestino.Items.Add(new ListItem(_item.Text, _item.Value));
                        _elementoseliminar.Add(_item);
                    }
                }

                new FuncionesDAO().FunRemoverElement(LstOrigen, _elementoseliminar);
                new FuncionesDAO().FunOrdenar(LstDestino);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgQuitar1_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ArrayList _elementoseliminar = new ArrayList();

                foreach (ListItem _items in LstDestino.Items)
                {
                    if (_items.Selected == true)
                    {
                        LstOrigen.Items.Add(new ListItem(_items.Text, _items.Value));
                        _elementoseliminar.Add(_items);
                    }
                }

                new FuncionesDAO().FunRemoverElement(LstDestino, _elementoseliminar);
                new FuncionesDAO().FunOrdenar(LstOrigen);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvGestores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                _suma += Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "Operaciones"));
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Total";
                e.Row.Cells[1].Text = _suma.ToString();
                e.Row.Font.Bold = true;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                LblOperaciones.InnerText = (int.Parse(ViewState["TotalOperaciones"].ToString()) - _suma).ToString();
            }
        }

        protected void ChkReasig_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                FunClearObjects(0);

                foreach (GridViewRow _fr in GrdvDiasAsignar.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                    _fr.Cells[1].BackColor = System.Drawing.Color.White;
                    _fr.Cells[2].BackColor = System.Drawing.Color.White;
                    _fr.Cells[3].BackColor = System.Drawing.Color.White;
                    _chkselecc = (CheckBox)(_fr.Cells[4].FindControl("chkReasig"));
                    _chkselecc.Checked = false;
                }

                _chkselecc = (CheckBox)(_gvrow.Cells[4].FindControl("chkReasig"));
                _chkselecc.Checked = !_chkselecc.Checked;

                if (_chkselecc.Checked)
                {
                    GrdvDiasAsignar.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Bisque;
                    GrdvDiasAsignar.Rows[_gvrow.RowIndex].Cells[1].BackColor = System.Drawing.Color.Bisque;
                    GrdvDiasAsignar.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.Bisque;
                    GrdvDiasAsignar.Rows[_gvrow.RowIndex].Cells[3].BackColor = System.Drawing.Color.Bisque;
                    LblDias.InnerText = GrdvDiasAsignar.DataKeys[_gvrow.RowIndex].Values["DiasMora"].ToString();
                    LblOperaciones.InnerText = GrdvDiasAsignar.DataKeys[_gvrow.RowIndex].Values["Operaciones"].ToString();
                    ViewState["TotalOperaciones"] = GrdvDiasAsignar.DataKeys[_gvrow.RowIndex].Values["Operaciones"].ToString();
                    RdbOpera.Enabled = true;
                    RdbClientes.Enabled = true;
                }
                else
                {
                    GrdvDiasAsignar.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.White;
                    GrdvDiasAsignar.Rows[_gvrow.RowIndex].Cells[1].BackColor = System.Drawing.Color.White;
                    GrdvDiasAsignar.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.White;
                    GrdvDiasAsignar.Rows[_gvrow.RowIndex].Cells[3].BackColor = System.Drawing.Color.White;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvDatosReasignar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    _totaloperaciones += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Operaciones"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[0].Text = "TOTAL:";
                    e.Row.Cells[1].Text = _totaloperaciones.ToString();
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
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
                if (DdlMotivo.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Motivo..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtObservacion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese observación..!", this);
                    return;
                }

                _dts = new ConsultaDatosDAO().FunRegReasignaCartera(0, int.Parse(ViewState["CodigoCedente"].ToString()), int.Parse(ViewState["CodigoCatalago"].ToString()), DdlMotivo.SelectedValue,
                    TxtObservacion.Text.Trim().ToUpper(), int.Parse(ViewState["CodigoGestor"].ToString()), int.Parse(LblDias.InnerText), "", "", "", 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                Session["MachineName"].ToString(), Session["Conectar"].ToString());

                if (RdbOpera.Checked)
                {
                    _dtbgestores = (DataTable)ViewState["GestoresAsignados"];

                    if (_dtbgestores.Rows.Count > 0)
                    {
                        _asignado = true;

                        foreach (DataRow _dr in _dtbgestores.Rows)
                        {
                            new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _dr[3].ToString(), "", "", 
                                Session["Conectar"].ToString());
                        }
                    }
                    else new FuncionesDAO().FunShowJSMessage("No existen datos para asignar..!", this);
                }

                if (RdbClientes.Checked)
                {
                    _asignado = true;

                    if (LstDestino.Items.Count == 0)
                    {
                        _asignado = false;
                        new FuncionesDAO().FunShowJSMessage("Seleccione algún cliente para Asignar..!", this);
                        return;
                    }

                    foreach (ListItem camposDestino in LstDestino.Items)
                    {
                        _sqlcli = "";
                        _sqlcli = "update GSBPO_CUENTA_DEUDOR set ctde_gestorasignado=" + int.Parse(DdlGestorCli.SelectedValue) + 
                            " where ";
                        _sqlcli += "CTDE_CODIGO=" + camposDestino.Value;
                        new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sqlcli, "", "", Session["Conectar"].ToString());
                    }
                }

                FunClearObjects(0);
                FunClearObjects(1);
                FunCargarDatos();

                if (_asignado) new FuncionesDAO().FunShowJSMessage("Cartera Re-Asignada..!", this);
                else new FuncionesDAO().FunShowJSMessage("No Existen Datos para Re-Asignar..!", this);
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