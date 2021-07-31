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
    public partial class WFrm_SupervisorCedente : Page
    {
        #region Variables
        DataTable _dtbsupervisor = new DataTable();
        DataTable _dtbbuscar = new DataTable();
        DataTable _dtbagregar = new DataTable();
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        DataRow _result, _filagre;
        bool _existe = false;
        int _maxcodigo = 0, _codigosupervisor = 0, _codigocedente = 0;
        Label _lblest = new Label();
        CheckBox _chkest = new CheckBox();
        ImageButton _editar = new ImageButton();
        string _mensaje = "", _response = "", _codigousuario = "", _mensajes = "";
        bool _lexiste = false;
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

                Lbltitulo.Text = "Registro de Supervisores";
                _dtbsupervisor.Columns.Add("CodigoSupervisor");
                _dtbsupervisor.Columns.Add("Cedente");
                _dtbsupervisor.Columns.Add("Supervisor");
                _dtbsupervisor.Columns.Add("CedenteCodigo");
                _dtbsupervisor.Columns.Add("UsuarioCodigo");
                _dtbsupervisor.Columns.Add("Estado");
                ViewState["Supervisores"] = _dtbsupervisor;
                FunCargarCombos(0);

                
                if (Request["MensajeRetornado"] != null)
                {
                    _mensajes = Request["MensajeRetornado"];
                    new FuncionesDAO().FunShowJSMessage(_mensaje, this, "S", "R");
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(7, int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", 
                    Session["Conectar"].ToString());
                ViewState["Supervisores"] = _dts.Tables[0];
                GrdvSupervisores.DataSource = _dts;
                GrdvSupervisores.DataBind();
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
                        ListItem item = new ListItem();
                        item.Text = "--Seleccione Supervisor--";
                        item.Value = "0";
                        DdlSupervisor.Items.Add(item);
                        DdlCedente.DataSource = new ConsultaDatosDAO().FunConsultaDatos(5, 0, 0, 0, "", "", "",
                            Session["Conectar"].ToString());
                        DdlCedente.DataTextField = "Descripcion";
                        DdlCedente.DataValueField = "Codigo";
                        DdlCedente.DataBind();
                        break;
                    case 1:
                        DdlSupervisor.Items.Clear();
                        DdlSupervisor.DataSource = new ConsultaDatosDAO().FunConsultaDatos(4, int.Parse(DdlCedente.SelectedValue),
                            0, 0, "", "", "", Session["Conectar"].ToString());
                        DdlSupervisor.DataTextField = "Descripcion";
                        DdlSupervisor.DataValueField = "Codigo";
                        DdlSupervisor.DataBind();
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

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                SoftCob_CEDENTE _cedente = new SoftCob_CEDENTE();
                _dtb = (DataTable)ViewState["Supervisores"];

                if (_dtb.Rows.Count > 0)
                {
                    List<SoftCob_SUPERVISORES> _supervisores = new List<SoftCob_SUPERVISORES>();

                    foreach (DataRow _dr in _dtb.Rows)
                    {
                        _codigosupervisor = new CedenteDAO().FunGetCodigoSupervisor(int.Parse(_dr[3].ToString()), 
                            int.Parse(_dr[0].ToString()));

                        _supervisores.Add(new SoftCob_SUPERVISORES()
                        {
                            SUPE_CODIGO = _codigosupervisor,
                            CEDE_CODIGO = int.Parse(_dr[3].ToString()),
                            USUA_CODIGO = int.Parse(_dr[4].ToString()),
                            supe_estado = _dr[5].ToString() == "Activo" ? true : false,
                            supe_auxv1 = "",
                            supe_auxv2 = "",
                            supe_auxi1 = 0,
                            supe_auxi2 = 0,
                            supe_fechacreacion = DateTime.Now,
                            supe_usuariocreacion = int.Parse(Session["usuCodigo"].ToString()),
                            supe_terminalcreacion = Session["MachineName"].ToString(),
                            supe_fum = DateTime.Now,
                            supe_uum = int.Parse(Session["usuCodigo"].ToString()),
                            supe_tum = Session["MachineName"].ToString()
                        });
                    }

                    _cedente.SoftCob_SUPERVISORES = new List<SoftCob_SUPERVISORES>();

                    foreach (SoftCob_SUPERVISORES _addsupervisor in _supervisores)
                    {
                        _cedente.SoftCob_SUPERVISORES.Add(_addsupervisor);
                    }

                    _mensaje = new CedenteDAO().FunRegistroSupervisor(_cedente);

                    _response = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");

                    if (_mensaje == "") Response.Redirect(_response, false);
                    else Lblerror.Text = _mensaje;
                    DdlCedente.Enabled = true;
                }
                else new FuncionesDAO().FunShowJSMessage("No existen datos ingresados..!", this, "E", "C");
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvSupervisores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkest = (CheckBox)(e.Row.Cells[1].FindControl("chkEstSupervisor"));
                    _editar = (ImageButton)(e.Row.Cells[2].FindControl("imgEdiSupervisor"));
                    _codigosupervisor = int.Parse(GrdvSupervisores.DataKeys[e.Row.RowIndex].Values["CodigoSupervisor"].ToString());
                    _codigocedente = int.Parse(GrdvSupervisores.DataKeys[e.Row.RowIndex].Values["CedenteCodigo"].ToString());

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(6, _codigocedente, _codigosupervisor, 0, "", "", "", Session["Conectar"].ToString());

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

        protected void ImgAddSupervisor_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this, "W", "C");
                    return;
                }

                if (DdlSupervisor.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Supervisor..!", this, "W", "C");
                    return;
                }

                if (ViewState["Supervisores"] != null)
                {
                    _dtbbuscar = (DataTable)ViewState["Supervisores"];

                    if (_dtbbuscar.Rows.Count > 0)
                        _maxcodigo = _dtbbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["CodigoSupervisor"]));
                    else _maxcodigo = 0;

                    _result = _dtbbuscar.Select("CedenteCodigo='" + DdlCedente.SelectedValue + "' and UsuarioCodigo='" + 
                        DdlSupervisor.SelectedValue + "'").FirstOrDefault();

                    if (_result != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Supervisor ya está ingresado..!", this, "E", "C");
                    return;
                }

                _dtbagregar = (DataTable)ViewState["Supervisores"];
                _filagre = _dtbagregar.NewRow();
                _filagre["CodigoSupervisor"] = _maxcodigo + 1;
                _filagre["Cedente"] = DdlCedente.SelectedItem.ToString();
                _filagre["Supervisor"] = DdlSupervisor.SelectedItem.ToString();
                _filagre["CedenteCodigo"] = DdlCedente.SelectedValue;
                _filagre["UsuarioCodigo"] = DdlSupervisor.SelectedValue;
                _filagre["Estado"] = "Activo";
                _dtbagregar.Rows.Add(_filagre);
                _dtbagregar.DefaultView.Sort = "Supervisor";
                ViewState["Supervisores"] = _dtbagregar;
                GrdvSupervisores.DataSource = _dtbagregar;
                GrdvSupervisores.DataBind();
                DdlSupervisor.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEstSupervisor_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkest = (CheckBox)(_gvrow.Cells[2].FindControl("chkEstSupervisor"));
                _dtbsupervisor = (DataTable)ViewState["Supervisores"];
                _codigosupervisor = int.Parse(GrdvSupervisores.DataKeys[_gvrow.RowIndex].Values["CodigoSupervisor"].ToString());
                _result = _dtbsupervisor.Select("CodigoSupervisor='" + _codigosupervisor + "'").FirstOrDefault();
                _result["Estado"] = _chkest.Checked ? "Activo" : "Inactivo";
                _dtbsupervisor.AcceptChanges();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEdiSupervisor_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                ViewState["CodSupervisor"] = GrdvSupervisores.DataKeys[gvRow.RowIndex].Values["CodigoSupervisor"].ToString();
                _dtbbuscar = (DataTable)ViewState["Supervisores"];
                _result = _dtbbuscar.Select("CodigoSupervisor='" + ViewState["CodSupervisor"].ToString() + "'").FirstOrDefault();
                DdlCedente.SelectedValue = _result[3].ToString();
                DdlSupervisor.SelectedValue = _result[4].ToString();
                ViewState["Cedente"] = _result[1].ToString();
                ViewState["Supervisor"] = _result[2].ToString();
                ViewState["Estado"] = _result[5].ToString();
                ImgAddSupervisor.Enabled = false;
                ImgEditSupervisor.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelSupervisor_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigosupervisor = int.Parse(GrdvSupervisores.DataKeys[_gvrow.RowIndex].Values["CodigoSupervisor"].ToString());
                _codigocedente = int.Parse(GrdvSupervisores.DataKeys[_gvrow.RowIndex].Values["CedenteCodigo"].ToString());
                _codigousuario = GrdvSupervisores.DataKeys[_gvrow.RowIndex].Values["UsuarioCodigo"].ToString();
                _mensaje = new CedenteDAO().FunDelSupervisor(_codigocedente, int.Parse(_codigousuario));

                if (_mensaje == "")
                {
                    _dtbsupervisor = (DataTable)ViewState["Supervisores"];
                    _result = _dtbsupervisor.Select("CodigoSupervisor='" + _codigosupervisor + "'").First();
                    _result.Delete();
                    _dtbsupervisor.AcceptChanges();
                    ViewState["Supervisores"] = _dtbsupervisor;
                    GrdvSupervisores.DataSource = _dtbsupervisor;
                    GrdvSupervisores.DataBind();
                }
                else new FuncionesDAO().FunShowJSMessage(_mensaje, this);

                DdlSupervisor.SelectedIndex = 0;
                ImgAddSupervisor.Enabled = true;
                ImgEditSupervisor.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEditSupervisor_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this, "W", "C");
                    return;
                }

                if (DdlSupervisor.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Supervisor..!", this, "W", "C");
                    return;
                }

                if (ViewState["Supervisores"] != null)
                {
                    _dtbbuscar = (DataTable)ViewState["Supervisores"];
                    _dtbbuscar = (DataTable)ViewState["Supervisores"];
                    _result = _dtbbuscar.Select("CedenteCodigo='" + DdlCedente.SelectedValue + "' and UsuarioCodigo='" + DdlSupervisor.SelectedValue + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Cedente o Supervisor ya está asignado..!", this, "E", "C");
                    return;
                }

                _dtbagregar = (DataTable)ViewState["Supervisores"];
                _result = _dtbagregar.Select("CodigoSupervisor='" + ViewState["CodSupervisor"].ToString() + "'").FirstOrDefault();
                _result["CodigoSupervisor"] = ViewState["CodSupervisor"].ToString();
                _result["Cedente"] = DdlCedente.SelectedItem.ToString();
                _filagre["Supervisor"] = DdlSupervisor.SelectedItem.ToString();
                _filagre["CedenteCodigo"] = DdlCedente.SelectedValue;
                _filagre["UsuarioCodigo"] = DdlSupervisor.SelectedValue;
                _filagre["Estado"] = ViewState["Estado"].ToString();
                _dtbagregar.DefaultView.Sort = "Supervisor";
                ViewState["Supervisores"] = _filagre;
                GrdvSupervisores.DataSource = _filagre;
                GrdvSupervisores.DataBind();
                DdlSupervisor.SelectedIndex = 0;
                ImgAddSupervisor.Enabled = true;
                ImgEditSupervisor.Enabled = false;
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