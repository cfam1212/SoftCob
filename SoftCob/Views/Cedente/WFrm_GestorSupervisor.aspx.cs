namespace SoftCob.Views.Cedente
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_GestorSupervisor : Page
    {
        #region Variables
        DataTable _dtbgestores = new DataTable();
        DataTable _dtbbuscar = new DataTable();
        DataTable _dtbagregar = new DataTable();
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        DataRow _result, _filagre;
        bool _existe = false;
        int _maxcodigo = 0, _codigosupervisor = 0, _codigogestor = 0, _gestorcodigo = 0;
        Label _lblest = new Label();
        CheckBox _chkest = new CheckBox();
        ImageButton _editar = new ImageButton();
        ListItem _items = new ListItem();
        ListItem _itemg = new ListItem();
        string _mensaje = "", _response = "";
        bool _lexiste = false;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                Lbltitulo.Text = "Registro de Gestores";
                _dtbgestores.Columns.Add("CodigoGestor");
                _dtbgestores.Columns.Add("Gestor");
                _dtbgestores.Columns.Add("CedenteCodigo");
                _dtbgestores.Columns.Add("CodigoSupervisor");
                _dtbgestores.Columns.Add("GestorCodigo");
                _dtbgestores.Columns.Add("Estado");
                ViewState["Gestores"] = _dtbgestores;
                FunCargarCombos(0);

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", 
                    Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(10, int.Parse(DdlSupervisor.SelectedValue), 0, 0, "", "", "", 
                    Session["Conectar"].ToString());
                ViewState["Gestores"] = _dts.Tables[0];
                GrdvGestores.DataSource = _dts;
                GrdvGestores.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void FunCargarCombos(int tipo)
        {
            try
            {
                switch (tipo)
                {
                    case 0:
                        _items.Text = "--Seleccione Supervisor--";
                        _items.Value = "0";
                        DdlSupervisor.Items.Add(_items);
                        _itemg.Text = "--Seleccione Gestor--";
                        _itemg.Value = "0";
                        DdlGestor.Items.Add(_itemg);
                        DdlCedente.DataSource = new ConsultaDatosDAO().FunConsultaDatos(5, 0, 0, 0, "", "", "", 
                            Session["Conectar"].ToString());
                        DdlCedente.DataTextField = "Descripcion";
                        DdlCedente.DataValueField = "Codigo";
                        DdlCedente.DataBind();
                        break;
                    case 1:
                        DdlSupervisor.Items.Clear();
                        DdlGestor.Items.Clear();
                        _itemg.Text = "--Seleccione Gestor--";
                        _itemg.Value = "0";
                        DdlGestor.Items.Add(_itemg);
                        DdlSupervisor.DataSource = new ConsultaDatosDAO().FunConsultaDatos(8, int.Parse(DdlCedente.SelectedValue), 0, 0,
                            "", "", "", Session["Conectar"].ToString());
                        DdlSupervisor.DataTextField = "Descripcion";
                        DdlSupervisor.DataValueField = "Codigo";
                        DdlSupervisor.DataBind();
                        break;
                    case 2:
                        DdlGestor.Items.Clear();
                        DdlGestor.DataSource = new ConsultaDatosDAO().FunConsultaDatos(9, int.Parse(DdlSupervisor.SelectedValue), 0, 0,
                            "", "", "", Session["Conectar"].ToString()); 
                        DdlGestor.DataTextField = "Descripcion";
                        DdlGestor.DataValueField = "Codigo";
                        DdlGestor.DataBind();
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
        protected void DdlCedente_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(1);
            FunCargarMantenimiento();
        }

        protected void DdlSupervisor_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(2);
            FunCargarMantenimiento();
        }

        protected void ChkEstGestor_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkest = (CheckBox)(_gvrow.Cells[1].FindControl("chkEstGestor"));
                _lblest.Text = _chkest.Checked ? "Activo" : "Inactivo";
                _dtbgestores = (DataTable)ViewState["Gestores"];
                _codigogestor = int.Parse(GrdvGestores.DataKeys[_gvrow.RowIndex].Values["CodigoGestor"].ToString());
                _result = _dtbgestores.Select("CodigoGestor='" + _codigogestor + "'").FirstOrDefault();
                _result["Estado"] = _chkest.Checked ? "Activo" : "Inactivo";
                _dtbgestores.AcceptChanges();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEdiGestor_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                ViewState["CodGestor"] = GrdvGestores.DataKeys[_gvrow.RowIndex].Values["CodigoGestor"].ToString();
                _dtbbuscar = (DataTable)ViewState["Gestores"];
                _result = _dtbbuscar.Select("CodigoGestor='" + ViewState["CodGestor"].ToString() + "'").FirstOrDefault();
                DdlCedente.SelectedValue = _result[2].ToString();
                DdlSupervisor.SelectedValue = _result[3].ToString();
                DdlGestor.SelectedValue = _result[4].ToString();
                ViewState["Estado"] = _result[5].ToString();
                ImgAddGestor.Enabled = false;
                ImgEditarGestor.Enabled = true;
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
                SoftCob_SUPERVISORES supervisor = new SoftCob_SUPERVISORES();
                _dtb = (DataTable)ViewState["Gestores"];

                if (_dtb.Rows.Count > 0)
                {
                    List<SoftCob_GESTOR_SUPERVISOR> gestores = new List<SoftCob_GESTOR_SUPERVISOR>();

                    foreach (DataRow _dr in _dtb.Rows)
                    {
                        _codigogestor = new CedenteDAO().FunGetCodigoGestor(int.Parse(_dr["CodigoSupervisor"].ToString()), 
                            int.Parse(_dr["GestorCodigo"].ToString()));

                        gestores.Add(new SoftCob_GESTOR_SUPERVISOR()
                        {
                            GEST_CODIGO = _codigogestor,
                            SUPE_CODIGO = int.Parse(_dr["CodigoSupervisor"].ToString()),
                            USUA_CODIGO = int.Parse(_dr["GestorCodigo"].ToString()),
                            gest_estado = _dr[5].ToString() == "Activo" ? true : false,
                            gest_auxv1 = "",
                            gest_auxv2 = "",
                            gest_auxv3 = "",
                            gest_auxi1 = int.Parse(DdlCedente.SelectedValue),
                            gest_auxi2 = 0,
                            gest_auxi3 = 0,
                            gest_fechacreacion = DateTime.Now,
                            gest_usuariocreacion = int.Parse(Session["usuCodigo"].ToString()),
                            gest_terminalcreacion = Session["MachineName"].ToString(),
                            gest_fum = DateTime.Now,
                            gest_uum = int.Parse(Session["usuCodigo"].ToString()),
                            gest_tum = Session["MachineName"].ToString()
                        });
                    }

                    supervisor.SoftCob_GESTOR_SUPERVISOR = new List<SoftCob_GESTOR_SUPERVISOR>();

                    foreach (SoftCob_GESTOR_SUPERVISOR addGestores in gestores)
                    {
                        supervisor.SoftCob_GESTOR_SUPERVISOR.Add(addGestores);
                    }

                    _mensaje = new CedenteDAO().FunRegistroGestor(supervisor);
                    _response = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");

                    if (_mensaje == "") Response.Redirect(_response, false);
                    else Lblerror.Text = _mensaje;

                    DdlCedente.Enabled = true;
                    DdlSupervisor.Enabled = true;
                }
                else new FuncionesDAO().FunShowJSMessage("No existen datos ingresados..!", this);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void GrdvGestores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkest = (CheckBox)(e.Row.Cells[1].FindControl("chkEstGestor"));
                    _editar = (ImageButton)(e.Row.Cells[2].FindControl("imgEdiGestor"));
                    _codigosupervisor = int.Parse(GrdvGestores.DataKeys[e.Row.RowIndex].Values["CodigoSupervisor"].ToString());
                    _codigogestor = int.Parse(GrdvGestores.DataKeys[e.Row.RowIndex].Values["CodigoGestor"].ToString());

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(11, _codigosupervisor, _codigogestor, 0, "", "", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        _chkest.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                        _editar.ImageUrl = "~/Botones/editargris.png";
                        _editar.Height = 20;
                        _editar.Enabled = false;
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

        protected void ImgEditarGestor_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this);
                    return;
                }

                if (DdlSupervisor.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Supervisor..!", this);
                    return;
                }

                if (DdlGestor.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this);
                    return;
                }

                if (ViewState["Gestores"] != null)
                {
                    _dtbbuscar = (DataTable)ViewState["Gestores"];
                    _result = _dtbbuscar.Select("CodigoSupervisor='" + DdlSupervisor.SelectedValue + "' and GestorCodigo='" + DdlGestor.SelectedValue + "'").FirstOrDefault();
                    _lexiste = _result == null ? false : true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Gestor ya está asignado al Supervisor..!", this);
                    return;
                }

                _dtbagregar = new DataTable();
                _dtbagregar = (DataTable)ViewState["Gestores"];
                _result = _dtbagregar.Select("CodigoGestor='" + ViewState["CodGestor"].ToString() + "'").FirstOrDefault();
                _result["CodigoGestor"] = ViewState["CodGestor"].ToString();
                _result["Gestor"] = DdlGestor.SelectedItem.ToString();
                _result["CedenteCodigo"] = DdlCedente.SelectedValue;
                _result["CodigoSupervisor"] = DdlSupervisor.SelectedValue;
                _result["GestorCodigo"] = DdlGestor.SelectedValue;
                _result["Estado"] = ViewState["Estado"].ToString();
                _dtbagregar.DefaultView.Sort = "Gestor";
                ViewState["Gestores"] = _dtbagregar;
                GrdvGestores.DataSource = _dtbagregar;
                GrdvGestores.DataBind();
                DdlGestor.SelectedIndex = 0;
                ImgAddGestor.Enabled = true;
                ImgEditarGestor.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddGestor_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this);
                    return;
                }

                if (DdlSupervisor.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Supervisor..!", this);
                    return;
                }

                if (DdlGestor.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this);
                    return;
                }

                if (ViewState["Gestores"] != null)
                {
                    _dtbbuscar = (DataTable)ViewState["Gestores"];
                    _result = _dtbbuscar.Select("CodigoSupervisor='" + DdlSupervisor.SelectedValue + "' and GestorCodigo='" + DdlGestor.SelectedValue + "'").FirstOrDefault();

                    if (_result != null) _existe = true;

                    if (_dtbbuscar.Rows.Count > 0)
                    {
                        _maxcodigo = _dtbbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["CodigoGestor"]));
                    }
                    else _maxcodigo = 0;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Gestor ya está asignado al Supervisor..!", this);
                    return;
                }

                _dtbagregar = (DataTable)ViewState["Gestores"];
                _filagre = _dtbagregar.NewRow();
                _filagre["CodigoGestor"] = _maxcodigo + 1;
                _filagre["Gestor"] = DdlGestor.SelectedItem.ToString();
                _filagre["CedenteCodigo"] = DdlCedente.SelectedValue;
                _filagre["CodigoSupervisor"] = DdlSupervisor.SelectedValue;
                _filagre["GestorCodigo"] = DdlGestor.SelectedValue;
                _filagre["Estado"] = "Activo";
                _dtbagregar.Rows.Add(_filagre);
                _dtbagregar.DefaultView.Sort = "Gestor";
                ViewState["Gestores"] = _dtbagregar;
                GrdvGestores.DataSource = _dtbagregar;
                GrdvGestores.DataBind();
                DdlGestor.SelectedIndex = 0;
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
                _codigosupervisor = int.Parse(GrdvGestores.DataKeys[_gvrow.RowIndex].Values["CodigoSupervisor"].ToString());
                _codigogestor = int.Parse(GrdvGestores.DataKeys[_gvrow.RowIndex].Values["CodigoGestor"].ToString());
                _gestorcodigo = int.Parse(GrdvGestores.DataKeys[_gvrow.RowIndex].Values["GestorCodigo"].ToString());
                _mensaje = new CedenteDAO().FunDelGestor(_codigosupervisor, _codigogestor);
                _dts = new ConsultaDatosDAO().FunConsultaDatos(229, int.Parse(DdlCedente.SelectedValue), _gestorcodigo,
                    0, "", "", "", Session["Conectar"].ToString());

                if (_mensaje == "")
                {
                    _dtbgestores = (DataTable)ViewState["Gestores"];
                    _result = _dtbgestores.Select("CodigoGestor='" + _codigogestor + "'").FirstOrDefault();
                    _result.Delete();
                    ViewState["Gestores"] = _dtbgestores;
                    GrdvGestores.DataSource = _dtbgestores;
                    GrdvGestores.DataBind();
                }
                else new FuncionesDAO().FunShowJSMessage(_mensaje, this);

                FunCargarCombos(2);
                ImgAddGestor.Enabled = true;
                ImgEditarGestor.Enabled = false;
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