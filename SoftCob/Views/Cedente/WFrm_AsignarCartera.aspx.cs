namespace SoftCob.Views.Cedente
{
    using ControllerSoftCob;
    using System;
    using System.Collections;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_AsignarCartera : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtbasignar = new DataTable();
        DataTable _dtbgestores = new DataTable();
        DataTable _dtbagregar = new DataTable();
        DataTable _dtblogicos = new DataTable();
        ListItem _citem = new ListItem();
        ListItem _ditem = new ListItem();
        int _idciudad = 0, _operaciones = 0, _gestores = 0, _totalasignar = 0, _diferencia = 0, _suma = 0, _resultado = 0, _contar = 0;
        string _sql = "", _sqlcli = "", _mensaje = "";
        bool _lexiste = false, _asignado = false;
        DataRow[] _rows;
        DataRow _filagre;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                //if (Session["IN-CALL"].ToString() == "SI")
                //{
                //    new ElastixDAO().ElastixHangUp(Session["IPLocalAdress"].ToString(), 9999);
                //    Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                //    return;
                //}

                _dtbasignar.Columns.Add("DiasMora");
                _dtbasignar.Columns.Add("Exigible");
                _dtbasignar.Columns.Add("SqlFormado");
                ViewState["AsignarSQL"] = _dtbasignar;

                _dtbgestores.Columns.Add("GestorCodigo");
                _dtbgestores.Columns.Add("Gestor");
                _dtbgestores.Columns.Add("Operaciones");
                _dtbgestores.Columns.Add("SqlFormado");
                ViewState["GestoresAsignados"] = _dtbgestores;

                _dtblogicos.Columns.Add("DiasMora");
                _dtblogicos.Columns.Add("Exigible");
                ViewState["Logicos"] = _dtblogicos;

                _citem.Text = "--Seleccione Cartera--";
                _citem.Value = "0";
                DdlCatalogo.Items.Add(_citem);

                _ditem.Text = "--Seleccione Dia--";
                _ditem.Value = "0";
                DdlDiasMora.Items.Add(_ditem);

                Lbltitulo.Text = "Asignar Cartera";

               
                if (Request["MensajeRetornado"] != null)
                {
                    _mensaje = Request["MensajeRetornado"];
                    new FuncionesDAO().FunShowJSMessage(_mensaje, this, "S", "R");
                }
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

        private void FunCargarCombos(int tipo, int codigo)
        {
            try
            {
                switch (tipo)
                {
                    case 0:
                        DdlCatalogo.DataSource = new ConsultaDatosDAO().FunConsultaDatos(13, codigo, 0, 0, "", "", "",
                            Session["Conectar"].ToString());
                        DdlCatalogo.DataTextField = "Descripcion";
                        DdlCatalogo.DataValueField = "Codigo";
                        DdlCatalogo.DataBind();
                        break;
                    case 1:
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
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunClearObjects()
        {
            lblTotal.InnerText = "";
            DdlDiasMora.Items.Clear();
            DdlDiasMora1.Items.Clear();
            _ditem.Text = "--Seleccione Dia--";
            _ditem.Value = "0";
            DdlDiasMora.Items.Add(_ditem);
            DdlDiasMora1.Items.Add(_ditem);
            TxtExigible.Text = "1";
            txtExigible0.Text = "1";
            ChkLogico1.Checked = false;
            ChkLogico2.Checked = false;
            trLogico1.Visible = false;
            trLogico2.Visible = false;
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
            ImgAgregar.Enabled = true;
            RdbOpera.Checked = false;
            RdbOpera.Enabled = false;
            RdbDeudor.Checked = false;
            RdbClientes.Checked = false;
            RdbDeudor.Enabled = false;
            RdbClientes.Enabled = false;
            RdbIdentificacion.Enabled = false;
            RdbIdentificacion.Checked = false;
            _dtbasignar = (DataTable)ViewState["AsignarSQL"];
            //DataRow[] rows;
            _rows = _dtbasignar.Select();

            foreach (DataRow row in _rows)
            {
                _dtbasignar.Rows.Remove(row);
            }

            ViewState["AsignarSQL"] = _dtbasignar;
            GrdvDatos.DataSource = _dtbasignar;
            GrdvDatos.DataBind();
            _dtbgestores = null;
            GrdvGestores.DataSource = _dtbgestores;
            GrdvGestores.DataBind();
            GrdvResultado.DataSource = null;
            GrdvResultado.DataBind();
            updCabecera.Update();
        }

        private void FunConsultarDatos()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(14, int.Parse(DdlCatalogo.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString());
                DdlDiasMora.DataSource = _dts;
                DdlDiasMora.DataTextField = "Descripcion";
                DdlDiasMora.DataValueField = "Codigo";
                DdlDiasMora.DataBind();
                DdlDiasMora1.DataSource = _dts;
                DdlDiasMora1.DataTextField = "Descripcion";
                DdlDiasMora1.DataValueField = "Codigo";
                DdlDiasMora1.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(47, int.Parse(DdlCatalogo.SelectedValue), 0, 0, "", "", "",
                    Session["Conectar"].ToString());
                GrdvResultado.DataSource = _dts;
                GrdvResultado.DataBind();

                if (_dts.Tables[0].Rows.Count > 0) RdbClientes.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
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
                    DataTable dtbBuscar = (DataTable)ViewState["GestoresAsignados"];
                    DataRow result = dtbBuscar.Select("GestorCodigo='" + codigoGestor + "'").FirstOrDefault();
                    if (result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Gestor ya está agregado..!", this, "E", "C");
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
            TreeView arbol = (TreeView)(sender);
            TreeNode node = arbol.SelectedNode;
            try
            {
                switch (node.Depth)
                {
                    case 3:
                        TrvCedentes.SelectedNode.Expand();
                        string[] pathRoot = node.ValuePath.Split(new char[] { '/' });
                        ViewState["CodigoCedente"] = pathRoot[3].ToString();
                        FunCargarCombos(0, int.Parse(pathRoot[4].ToString()));
                        FunCargarCombos(1, int.Parse(pathRoot[3].ToString()));
                        ImgAgregar.Enabled = true;
                        FunClearObjects();
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FunClearObjects();
                FunConsultarDatos();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAgregar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlCatalogo.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cartera Producto..!", this, "W", "C");
                    return;
                }

                if (DdlDiasMora.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Días Mora..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtExigible.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese valor exigible >=1..!", this, "W", "C");
                    return;
                }

                _dtbagregar = (DataTable)ViewState["AsignarSQL"];
                _filagre = _dtbagregar.NewRow();

                if (ChkLogico1.Checked)
                {
                    if (DdlDiasMora1.SelectedValue == "0")
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione Días Mora..!", this, "W", "C");
                        return;
                    }

                    _filagre["DiasMora"] = DdlOperador1.SelectedItem.ToString() + " " + DdlDiasMora.SelectedItem.ToString() + " and" + 
                        DdlOperador3.SelectedValue.ToString() + " " + DdlDiasMora1.SelectedItem.ToString(); ViewState["diasmora"] = 
                        "CD.ctde_diasmora" + DdlOperador1.SelectedItem.ToString() + " " + DdlDiasMora.SelectedItem.ToString() + " and " +
                        "CD.ctde_diasmora" + DdlOperador3.SelectedItem.ToString() + " " + DdlDiasMora1.SelectedItem.ToString();
                }
                else
                {
                    _filagre["DiasMora"] = DdlOperador1.SelectedItem.ToString() + " " + DdlDiasMora.SelectedItem.ToString();
                    ViewState["diasmora"] = "CD.ctde_diasmora" + DdlOperador1.SelectedItem.ToString() + " " + DdlDiasMora.SelectedItem.ToString();
                }

                if (ChkLogico2.Checked)
                {
                    if (string.IsNullOrEmpty(txtExigible0.Text.Trim()))
                    {
                        new FuncionesDAO().FunShowJSMessage("Ingrese valor exigible >=1..!", this, "W", "C");
                        return;
                    }

                    _filagre["Exigible"] = DdlOperador2.SelectedItem.ToString() + " " + TxtExigible.Text.Trim() + " and " +
                        "CD.ctde_valorexigible" + DdlOperador4.SelectedItem.ToString() + " " + txtExigible0.Text.Trim();
                    ViewState["exigible"] = "CD.ctde_valorexigible" + DdlOperador2.SelectedItem.ToString() + " " + TxtExigible.Text.Trim() + " and " + "CD.ctde_valorexigible" + DdlOperador4.SelectedItem.ToString() + " " + txtExigible0.Text.Trim();
                }
                else
                {
                    _filagre["Exigible"] = DdlOperador2.SelectedItem.ToString() + " " + TxtExigible.Text.Trim();
                    ViewState["exigible"] = "CD.ctde_valorexigible" + DdlOperador2.SelectedItem.ToString() + " " + TxtExigible.Text.Trim();
                }

                _sql = "Select count(*) Operaciones from SoftCob_CUENTA_DEUDOR CD (nolock) ";
                _sql += "INNER JOIN SoftCob_CLIENTE_DEUDOR CL (nolock) ON CD.CLDE_CODIGO=CL.CLDE_CODIGO ";
                _sql += "where CL.CPCE_CODIGO=" + DdlCatalogo.SelectedValue.ToString() + " and CD.ctde_estado=1 and CD.ctde_gestorasignado=0 and ";
                _sql += ViewState["diasmora"] + " and " + ViewState["exigible"];

                _filagre["SqlFormado"] = _sql;
                _dtbagregar.Rows.Add(_filagre);
                ViewState["AsignarSQL"] = _dtbagregar;
                GrdvDatos.DataSource = _dtbagregar;
                GrdvDatos.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql, "", "", Session["Conectar"].ToString());
                lblTotal.InnerText = _dts.Tables[0].Rows[0]["Operaciones"].ToString();
                ViewState["TotalOperaciones"] = lblTotal.InnerText;

                if (int.Parse(ViewState["TotalOperaciones"].ToString()) > 0)
                {
                    ImgAgregar.Enabled = false;
                    RdbOpera.Enabled = true;
                    RdbClientes.Enabled = true;
                }
                else new FuncionesDAO().FunShowJSMessage("No existen operaciones..!", this, "E", "C");
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
                _dtbagregar = (DataTable)ViewState["AsignarSQL"];
                _rows = _dtbagregar.Select();

                foreach (DataRow _row in _rows)
                {
                    _dtbagregar.Rows.Remove(_row);
                }

                ViewState["AsignarSQL"] = _dtbagregar;
                GrdvDatos.DataSource = _dtbagregar;
                GrdvDatos.DataBind();
                FunClearObjects();
                DdlCatalogo.SelectedValue = "0";
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
                lblTotal.InnerText = (int.Parse(ViewState["TotalOperaciones"].ToString()) - _suma).ToString();
            }
        }

        protected void ImgDelGestor_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _dtbagregar = (DataTable)ViewState["GestoresAsignados"];
                _dtbagregar.Rows.RemoveAt(_gvrow.RowIndex);
                lblTotal.InnerText = ViewState["TotalOperaciones"].ToString();
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
                lblTotal.InnerText = ViewState["TotalOperaciones"].ToString();

                if (string.IsNullOrEmpty(lblTotal.InnerText))
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cartera para tener asignación..!", this, "W", "C");
                    return;
                }

                _operaciones = int.Parse(lblTotal.InnerText);
                _dts = new ConsultaDatosDAO().FunConsultaDatos(12, int.Parse(ViewState["CodigoCedente"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());

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
                        _sql += "Update SoftCob_CUENTA_DEUDOR set ctde_gestorasignado=" + _dr[1].ToString();
                        _sql += " from (select top " + _totalasignar.ToString() + " CD.ctde_gestorasignado, CL.CLDE_CODIGO,CD.ctde_operacion ";
                        _sql += "from SoftCob_CUENTA_DEUDOR CD (nolock) ";
                        _sql += "INNER JOIN SoftCob_CLIENTE_DEUDOR CL (nolock) ON CD.CLDE_CODIGO=CL.CLDE_CODIGO ";
                        _sql += "where " + ViewState["diasmora"] + " and " + ViewState["exigible"] + " and ";
                        _sql += "CD.ctde_gestorasignado=0 and CL.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " and Cl.clde_estado=1 and ";
                        _sql += "CD.ctde_estado=1" + ") as d ";
                        _sql += "where d.CLDE_CODIGO = SoftCob_CUENTA_DEUDOR.CLDE_CODIGO and d.ctde_operacion=SoftCob_CUENTA_DEUDOR.ctde_operacion";
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
                RdbTodos.Checked = false;
                FunClearGestores();
                lblTotal.InnerText = ViewState["TotalOperaciones"].ToString();
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
                    new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtOperaciones.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese No. operaciones a asignar..!", this, "W", "C");
                    return;
                }

                if (int.Parse(TxtOperaciones.Text) == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese cantidad de operaciones..!", this, "W", "C");
                    return;
                }

                if (int.Parse(TxtOperaciones.Text) > int.Parse(lblTotal.InnerText))
                {
                    new FuncionesDAO().FunShowJSMessage("La cantidad a asignar es mayor a las operaciones totales..!", this, "E", "C");
                    return;
                }

                _sql = "";
                _sql += "Update SoftCob_CUENTA_DEUDOR set ctde_gestorasignado=" + DdlGestores.SelectedValue;
                _sql += " from (select top " + TxtOperaciones.Text.Trim() + " CD.ctde_gestorasignado, CL.CLDE_CODIGO,CD.ctde_operacion ";
                _sql += "from SoftCob_CUENTA_DEUDOR CD (nolock) ";
                _sql += "INNER JOIN SoftCob_CLIENTE_DEUDOR CL (nolock) ON CD.CLDE_CODIGO=CL.CLDE_CODIGO ";
                _sql += "where " + ViewState["diasmora"] + " and " + ViewState["exigible"] + " and ";
                _sql += "CD.ctde_gestorasignado=0 and CL.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " and Cl.clde_estado=1 and ";
                _sql += "CD.ctde_estado=1" + ") as d ";
                _sql += "where d.CLDE_CODIGO = SoftCob_CUENTA_DEUDOR.CLDE_CODIGO and d.ctde_operacion=SoftCob_CUENTA_DEUDOR.ctde_operacion";
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

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (RdbOpera.Checked)
                {
                    _dtbgestores = (DataTable)ViewState["GestoresAsignados"];

                    if (_dtbgestores.Rows.Count > 0)
                    {
                        foreach (DataRow _dr in _dtbgestores.Rows)
                        {
                            new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _dr[3].ToString(), "", "", Session["Conectar"].ToString());
                        }
                        _asignado = true;
                    }
                    else new FuncionesDAO().FunShowJSMessage("No existen datos para asignar..!", this, "E", "C");
                }

                if (RdbClientes.Checked)
                {
                    _asignado = true;

                    if (LstDestino.Items.Count == 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione algún cliente para Asignar..!", this, "W", "C");
                        _asignado = false;
                        return;
                    }

                    foreach (ListItem _camposdestino in LstDestino.Items)
                    {
                        _sqlcli = "";
                        _sqlcli = "update SoftCob_CUENTA_DEUDOR set ctde_gestorasignado=" + int.Parse(DdlGestorCli.SelectedValue) + " where ";
                        _sqlcli += "CTDE_CODIGO=" + _camposdestino.Value;
                        new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sqlcli, "", "", Session["Conectar"].ToString());
                    }
                }

                FunClearObjects();
                FunConsultarDatos();

                if (_asignado) new FuncionesDAO().FunShowJSMessage("Cartera Asignada..!", this, "S", "R");
                else new FuncionesDAO().FunShowJSMessage("No existen Datos para Asignar..!", this, "E", "C");

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgBuscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtBuscar.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Criterio de busqueda..!", this, "W", "C");
                    return;
                }

                if (DdlGestorCli.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this, "W", "C");
                    return;
                }

                if (!RdbDeudor.Checked && !RdbIdentificacion.Checked)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Busqueda Deudor o Identificación..!", this, "W", "C");
                    return;
                }

                _sqlcli = "select Cliente = PE.pers_primerapellido+' '+PE.pers_segundoapellido+' '+PE.pers_primernombre+' '+PE.pers_segundonombre,";
                _sqlcli += "CodigoCTDE = CD.CTDE_CODIGO from SoftCob_CUENTA_DEUDOR CD (nolock) INNER JOIN SoftCob_CLIENTE_DEUDOR CL (nolock) ON CD.CLDE_CODIGO=CL.CLDE_CODIGO ";
                _sqlcli += "INNER JOIN SoftCob_PERSONA PE ON CL.PERS_CODIGO=PE.PERS_CODIGO where CL.CPCE_CODIGO=";
                _sqlcli += DdlCatalogo.SelectedValue + " and CD.ctde_gestorasignado=0 and CD.ctde_estado=1 and Cl.clde_estado=1 and ";

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
                ArrayList ElementosEliminar = new ArrayList();

                foreach (ListItem item in LstOrigen.Items)
                {
                    if (item.Selected == true)
                    {
                        LstDestino.Items.Add(new ListItem(item.Text, item.Value));
                        ElementosEliminar.Add(item);
                    }
                }

                new FuncionesDAO().FunRemoverElement(LstOrigen, ElementosEliminar);
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
                ArrayList ElementosEliminar = new ArrayList();

                foreach (ListItem items in LstDestino.Items)
                {
                    if (items.Selected == true)
                    {
                        LstOrigen.Items.Add(new ListItem(items.Text, items.Value));
                        ElementosEliminar.Add(items);
                    }
                }

                new FuncionesDAO().FunRemoverElement(LstDestino, ElementosEliminar);
                new FuncionesDAO().FunOrdenar(LstOrigen);
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
            lblTotal.InnerText = ViewState["TotalOperaciones"].ToString();
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

        protected void RdbDeudor_CheckedChanged(object sender, EventArgs e)
        {
            RdbIdentificacion.Checked = false;
        }

        protected void RdbIdentificacion_CheckedChanged(object sender, EventArgs e)
        {
            RdbDeudor.Checked = false;
        }

        protected void GrdvResultado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                _resultado += Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "Operacion"));
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Total";
                e.Row.Cells[1].Text = _resultado.ToString();
                e.Row.Font.Bold = true;
            }
        }
        protected void ChkLogico1_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkLogico1.Checked) trLogico1.Visible = true;
            else trLogico1.Visible = false;
        }

        protected void ChkLogico2_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkLogico2.Checked) trLogico2.Visible = true;
            else trLogico2.Visible = false;
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
        }
        #endregion
    }
}