namespace SoftCob.Views.Bitacora
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_NuevaBitacora : Page
    {
        #region Variables
        DataTable _dtbatrasos = new DataTable();
        DataTable _dtbfaltasj = new DataTable();
        DataTable _dtbfaltasi = new DataTable();
        DataTable _dtbpermisos = new DataTable();
        DataTable _dtbcambioturno = new DataTable();
        DataTable _dtbvarios = new DataTable();
        DataTable _dtbnovedades = new DataTable();
        DataTable _dtbrefuerzos = new DataTable();
        DataTable _dtbterreno = new DataTable();
        DataTable _dtbsistemas = new DataTable();
        DataTable _dtbpagos = new DataTable();
        DataTable _dtbadicional = new DataTable();
        int _maxcodigo = 0;
        DataRow _resultado, _filagre;
        DataSet _dts = new DataSet();
        bool _existe = false;
        string _codigo = "", _horaactual = "", _codigogestor, _response;
        TimeSpan _hora, _hatraso;
        ImageButton _imgselecc;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {

                ViewState["Bitacora"] = Request["Bitacora"];
                ViewState["Estado"] = Request["Estado"];
                ViewState["Fecha"] = Request["Fecha"];
                if (ViewState["Bitacora"].ToString() == "")
                {
                    Lbltitulo.Text = "Ingreso Bitacora << " + "Bitacora_" + ViewState["Fecha"].ToString() + " >>";
                }
                else
                {
                    Lbltitulo.Text = "Editando Bitacora << " + ViewState["Bitacora"].ToString() + " >>";
                }

                ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                TxtHoraAT.Text = DateTime.Now.ToString("HH:mm");
                TxtFechaPE.Text = DateTime.Now.ToString("MM/dd/yyyy");
                ViewState["FechaActual"] = DateTime.Now.ToString("MM/dd/yyyy");
                LblFecha.InnerText = "FECHA SUPERVISION: " + ViewState["Fecha"].ToString();
                FunCargarCombos(0);
                FunCargarMantenimiento();
                TabDatosBitacora.ActiveTabIndex = 0;
                PnlDatosSupervisor.Height = 100;

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                if (ViewState["Estado"].ToString() == "Inactivo")
                {
                    TabDatosBitacora.Enabled = false;
                    DdlGestor.Enabled = false;
                    BtnGrabar.Enabled = false;
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(169, int.Parse(Session["usuCodigo"].ToString()),
                    0, 0, "", Session["MachineName"].ToString(), ViewState["Bitacora"].ToString(), ViewState["Conectar"].ToString());

                ViewState["Bitacora"] = _dts.Tables[0].Rows[0][0].ToString();

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(170, 0, 0, 0, "", ViewState["Bitacora"].ToString(), "",
                        ViewState["Conectar"].ToString());

                    GrdvSupervisores.DataSource = _dts.Tables[0];
                    GrdvSupervisores.DataBind();

                    ViewState["Atrasos"] = _dts.Tables[1];

                    if (_dts.Tables[1].Rows.Count > 0)
                    {
                        Pnl3.Visible = true;
                        GrdvAtrasos.DataSource = _dts.Tables[1];
                        GrdvAtrasos.DataBind();
                    }

                    ViewState["FaltasJ"] = _dts.Tables[2];

                    if (_dts.Tables[2].Rows.Count > 0)
                    {
                        Pnl4.Visible = true;
                        GrdvFaltasJ.DataSource = _dts.Tables[2];
                        GrdvFaltasJ.DataBind();
                    }

                    ViewState["FaltasI"] = _dts.Tables[3];

                    if (_dts.Tables[3].Rows.Count > 0)
                    {
                        Pnl5.Visible = true;
                        GrdvFaltasI.DataSource = _dts.Tables[3];
                        GrdvFaltasI.DataBind();
                    }

                    ViewState["Permiso"] = _dts.Tables[4];

                    if (_dts.Tables[4].Rows.Count > 0)
                    {
                        Pnl6.Visible = true;
                        GrdvPermisos.DataSource = _dts.Tables[4];
                        GrdvPermisos.DataBind();
                    }

                    ViewState["CambioTurno"] = _dts.Tables[5];

                    if (_dts.Tables[5].Rows.Count > 0)
                    {
                        Pnl7.Visible = true;
                        GrdvCambioTurno.DataSource = _dts.Tables[5];
                        GrdvCambioTurno.DataBind();
                    }

                    ViewState["Varios"] = _dts.Tables[6];

                    if (_dts.Tables[6].Rows.Count > 0)
                    {
                        Pnl8.Visible = true;
                        GrdvVarios.DataSource = _dts.Tables[6];
                        GrdvVarios.DataBind();
                    }

                    ViewState["Novedades"] = _dts.Tables[7];

                    if (_dts.Tables[7].Rows.Count > 0)
                    {
                        Pnl9.Visible = true;
                        GrdvNovedad.DataSource = _dts.Tables[7];
                        GrdvNovedad.DataBind();
                    }

                    ViewState["Refuerzos"] = _dts.Tables[8];

                    if (_dts.Tables[8].Rows.Count > 0)
                    {
                        Pnl10.Visible = true;
                        GrdvRefuerzo.DataSource = _dts.Tables[8];
                        GrdvRefuerzo.DataBind();
                    }

                    ViewState["Terreno"] = _dts.Tables[9];

                    if (_dts.Tables[9].Rows.Count > 0)
                    {
                        Pnl11.Visible = true;
                        GrdvTerreno.DataSource = _dts.Tables[9];
                        GrdvTerreno.DataBind();
                    }

                    ViewState["Sistemas"] = _dts.Tables[10];

                    if (_dts.Tables[10].Rows.Count > 0)
                    {
                        Pnl12.Visible = true;
                        GrdvSistemas.DataSource = _dts.Tables[10];
                        GrdvSistemas.DataBind();
                    }

                    ViewState["Pagos"] = _dts.Tables[11];

                    if (_dts.Tables[11].Rows.Count > 0)
                    {
                        Pnl13.Visible = true;
                        GrdvPagos.DataSource = _dts.Tables[11];
                        GrdvPagos.DataBind();
                    }

                    ViewState["Adicionales"] = _dts.Tables[12];

                    if (_dts.Tables[12].Rows.Count > 0)
                    {
                        Pnl14.Visible = true;
                        GrdvAdicionales.DataSource = _dts.Tables[12];
                        GrdvAdicionales.DataBind();
                    }
                }
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
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(168, 0, 0, 0, "", "", "", ViewState["Conectar"].ToString());
                        DdlGestor.DataSource = _dts;
                        DdlGestor.DataTextField = "Descripcion";
                        DdlGestor.DataValueField = "Codigo";
                        DdlGestor.DataBind();

                        _dts = new CatalogosDAO().FunGetParametroDetalle("TIPO TURNO", "--Seleccione Turno--");
                        DdlTurnoA.DataSource = _dts;
                        DdlTurnoA.DataTextField = "Descripcion";
                        DdlTurnoA.DataValueField = "Codigo";
                        DdlTurnoA.DataBind();

                        DdlTurnoN.DataSource = _dts;
                        DdlTurnoN.DataTextField = "Descripcion";
                        DdlTurnoN.DataValueField = "Codigo";
                        DdlTurnoN.DataBind();

                        break;
                    case 1:
                        DdlGestor.SelectedValue = "0";
                        TxtDescripAT.Text = "";
                        TxtObservaAT.Text = "";
                        TxtHoraAT.Text = DateTime.Now.ToString("HH:mm");
                        ImgAddAT.Visible = true;
                        ImgModAT.Visible = false;
                        ImgDelAT.Visible = false;
                        break;
                    case 2:
                        DdlGestor.SelectedValue = "0";
                        TxtDescripFJ.Text = "";
                        TxtObservaFJ.Text = "";
                        ImgAddFJ.Visible = true;
                        ImgModFJ.Visible = false;
                        ImgDelFJ.Visible = false;
                        break;
                    case 3:
                        DdlGestor.SelectedValue = "0";
                        TxtDescripFI.Text = "";
                        TxtObservaFI.Text = "";
                        ImgAddFI.Visible = true;
                        ImgModFI.Visible = false;
                        ImgDelFI.Visible = false;
                        break;
                    case 4:
                        DdlGestor.SelectedValue = "0";
                        TxtDescripPE.Text = "";
                        TxtObservaPE.Text = "";
                        ImgAddPE.Visible = true;
                        ImgModPE.Visible = false;
                        ImgDelPE.Visible = false;
                        break;
                    case 5:
                        DdlGestor.SelectedValue = "0";
                        TxtDescripCT.Text = "";
                        TxtObservaCT.Text = "";
                        DdlTurnoA.SelectedValue = "0";
                        DdlTurnoN.SelectedValue = "0";
                        ImgAddCT.Visible = true;
                        ImgModCT.Visible = false;
                        ImgDelCT.Visible = false;
                        break;
                    case 6:
                        DdlGestor.SelectedValue = "0";
                        TxtDescripVA.Text = "";
                        TxtObservaVA.Text = "";
                        ImgAddVA.Visible = true;
                        ImgModVA.Visible = false;
                        ImgDelVA.Visible = false;
                        break;
                    case 7:
                        TxtDescripNV.Text = "";
                        TxtObservaNV.Text = "";
                        ImgAddNV.Visible = true;
                        ImgModNV.Visible = false;
                        ImgDelNV.Visible = false;
                        break;
                    case 8:
                        DdlGestor.SelectedValue = "0";
                        TxtDescripRE.Text = "";
                        TxtObservaRE.Text = "";
                        ImgAddRE.Visible = true;
                        ImgModRE.Visible = false;
                        ImgDelRE.Visible = false;
                        break;
                    case 9:
                        TxtDescripGT.Text = "";
                        TxtObservaGT.Text = "";
                        ImgAddGT.Visible = true;
                        ImgModGT.Visible = false;
                        ImgDelGT.Visible = false;
                        break;
                    case 10:
                        TxtDescripNS.Text = "";
                        TxtObservaNS.Text = "";
                        ImgAddNS.Visible = true;
                        ImgModNS.Visible = false;
                        ImgDelNS.Visible = false;
                        break;
                    case 11:
                        TxtDescripCP.Text = "";
                        TxtObservaCP.Text = "";
                        ImgAddCP.Visible = true;
                        ImgModCP.Visible = false;
                        ImgDelCP.Visible = false;
                        break;
                    case 12:
                        TxtDescripAD.Text = "";
                        TxtObservaAD.Text = "";
                        ImgAddAD.Visible = true;
                        ImgModAD.Visible = false;
                        ImgDelAD.Visible = false;
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
        protected void GrdvSupervisores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _codigo = GrdvSupervisores.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString();

                    if (_codigo == Session["usuCodigo"].ToString()) e.Row.Cells[0].BackColor = Color.LightSeaGreen;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddAT_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlGestor.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Colaborador..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtDescripAT.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrse Descripcion..!", this);
                    return;
                }

                if (!new FuncionesDAO().IsHour(TxtHoraAT.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Hora ingresada incorrecta..!", this);
                    return;
                }

                _horaactual = DateTime.Now.ToString("HH:mm");
                _hora = TimeSpan.Parse(_horaactual);
                _hatraso = TimeSpan.Parse(TxtHoraAT.Text.Trim());

                if (_hatraso > _hora)
                {
                    new FuncionesDAO().FunShowJSMessage("Hora de Atraso no puede ser mayor a Hora Actual..!", this);
                    return;
                }

                if (ViewState["Atrasos"] != null)
                {
                    _dtbatrasos = (DataTable)ViewState["Atrasos"];

                    if (_dtbatrasos.Rows.Count > 0)
                        _maxcodigo = _dtbatrasos.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbatrasos.Select("CodigoGestorAT='" + DdlGestor.SelectedValue + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Gestor ya existe ingresado..!", this);
                    return;
                }

                _filagre = _dtbatrasos.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Gestor"] = DdlGestor.SelectedItem.ToString();
                _filagre["CodigoGestorAT"] = DdlGestor.SelectedValue;
                _filagre["Hora"] = TxtHoraAT.Text.Trim();
                _filagre["Descripcion"] = TxtDescripAT.Text.Trim().ToUpper();
                _filagre["Observacion"] = TxtObservaAT.Text.Trim().ToUpper();
                _filagre["Firma"] = Session["usuNombres"].ToString();
                _filagre["FirmaCodigo"] = Session["usuCodigo"].ToString();
                _dtbatrasos.Rows.Add(_filagre);
                _dtbatrasos.DefaultView.Sort = "Gestor";
                _dtbatrasos = _dtbatrasos.DefaultView.ToTable();
                ViewState["Atrasos"] = _dtbatrasos;
                GrdvAtrasos.DataSource = _dtbatrasos;
                GrdvAtrasos.DataBind();
                //LENAR UN TEXTBOX EN UN GRID
                //int rowsnumber = GrdvAtrasos.Rows.Count;
                //int i = 0;
                //foreach (GridViewRow row in GrdvAtrasos.Rows)
                //{
                //    if (row.RowType == DataControlRowType.DataRow)
                //    {
                //        if (row.RowIndex == i)
                //        {
                //            TextBox text = (TextBox)row.FindControl("TxtDescrip");
                //            text.Text = TxtDescripAT.Text.Trim().ToUpper();
                //        }
                //    }
                //    i++;
                //}
                FunCargarCombos(1);
                Pnl3.Visible = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelAT_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtbatrasos = (DataTable)ViewState["Atrasos"];
                _resultado = _dtbatrasos.Select("Codigo='" + ViewState["CodigoAtraso"].ToString() + "'").FirstOrDefault();
                _resultado.Delete();
                _dtbatrasos.AcceptChanges();
                _dts = new ConsultaDatosDAO().FunConsultaDatos(174, int.Parse(ViewState["CodigoAtraso"].ToString()),
                    0, 0, "", "ATR", ViewState["Bitacora"].ToString(), ViewState["Conectar"].ToString());
                GrdvAtrasos.DataSource = _dtbatrasos;
                GrdvAtrasos.DataBind();

                if (_dtbatrasos.Rows.Count == 0) Pnl3.Visible = false;

                FunCargarCombos(1);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSeleccAT_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow fr in GrdvAtrasos.Rows)
                {
                    fr.Cells[0].BackColor = Color.White;
                }

                GrdvAtrasos.Rows[gvRow.RowIndex].Cells[0].BackColor = Color.Coral;
                _dtbatrasos = (DataTable)ViewState["Atrasos"];
                _codigo = GrdvAtrasos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                _codigogestor = GrdvAtrasos.DataKeys[gvRow.RowIndex].Values["CodigoGestorAT"].ToString();
                ViewState["CodigoAtraso"] = _codigo;
                ViewState["CodigoGestorAT"] = _codigogestor;
                _resultado = _dtbatrasos.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtHoraAT.Text = _resultado["Hora"].ToString();
                TxtDescripAT.Text = _resultado["Descripcion"].ToString();
                TxtObservaAT.Text = _resultado["Observacion"].ToString();
                ImgAddAT.Visible = false;
                ImgModAT.Visible = true;
                ImgDelAT.Visible = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModAT_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtDescripAT.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrse Descripcion..!", this);
                    return;
                }

                if (!new FuncionesDAO().IsHour(TxtHoraAT.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Hora ingresada incorrecta..!", this);
                    return;
                }

                _horaactual = DateTime.Now.ToString("HH:mm");
                _hora = TimeSpan.Parse(_horaactual);
                _hatraso = TimeSpan.Parse(TxtHoraAT.Text.Trim());

                if (_hatraso > _hora)
                {
                    new FuncionesDAO().FunShowJSMessage("Hora de Atraso no puede ser mayor a Hora Actual..!", this);
                    return;
                }

                _dtbatrasos = (DataTable)ViewState["Atrasos"];
                _resultado = _dtbatrasos.Select("Codigo='" + ViewState["CodigoAtraso"].ToString() + "'").FirstOrDefault();
                _resultado["Hora"] = TxtHoraAT.Text.Trim();
                _resultado["Descripcion"] = TxtDescripAT.Text.Trim().ToUpper();
                _resultado["Observacion"] = TxtObservaAT.Text.Trim().ToUpper();
                _dtbatrasos.AcceptChanges();
                GrdvAtrasos.DataSource = _dtbatrasos;
                GrdvAtrasos.DataBind();
                FunCargarCombos(1);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvAtrasos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgselecc = (ImageButton)(e.Row.Cells[4].FindControl("ImgSeleccAT"));
                    _codigo = GrdvAtrasos.DataKeys[e.Row.RowIndex].Values["FirmaCodigo"].ToString();

                    if (_codigo != Session["usuCodigo"].ToString())
                    {
                        _imgselecc.ImageUrl = "~/Botones/editargris.png";
                        _imgselecc.Enabled = false;
                        e.Row.Cells[4].BackColor = Color.SlateGray;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddFJ_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlGestor.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Colaborador..!", this);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtDescripFJ.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Descripción..!", this);
                    return;
                }

                if (ViewState["FaltasJ"] != null)
                {
                    _dtbfaltasj = (DataTable)ViewState["FaltasJ"];

                    if (_dtbfaltasj.Rows.Count > 0)
                        _maxcodigo = _dtbfaltasj.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbfaltasj.Select("CodigoGestorFJ='" + DdlGestor.SelectedValue + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Gestor ya existe ingresado..!", this);
                    return;
                }

                _filagre = _dtbfaltasj.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Gestor"] = DdlGestor.SelectedItem.ToString();
                _filagre["CodigoGestorFJ"] = DdlGestor.SelectedValue;
                _filagre["Descripcion"] = TxtDescripFJ.Text.Trim().ToUpper();
                _filagre["Observacion"] = TxtObservaFJ.Text.Trim().ToUpper();
                _filagre["Firma"] = Session["usuNombres"].ToString();
                _filagre["FirmaCodigo"] = Session["usuCodigo"].ToString();
                _dtbfaltasj.Rows.Add(_filagre);
                _dtbfaltasj.DefaultView.Sort = "Gestor";
                _dtbfaltasj = _dtbfaltasj.DefaultView.ToTable();
                ViewState["FaltasJ"] = _dtbfaltasj;
                GrdvFaltasJ.DataSource = _dtbfaltasj;
                GrdvFaltasJ.DataBind();
                Pnl4.Visible = true;
                FunCargarCombos(2);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSeleccFJ_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow fr in GrdvFaltasJ.Rows)
                {
                    fr.Cells[0].BackColor = Color.White;
                }

                GrdvFaltasJ.Rows[gvRow.RowIndex].Cells[0].BackColor = Color.Coral;
                _dtbfaltasj = (DataTable)ViewState["FaltasJ"];
                _codigo = GrdvFaltasJ.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                _codigogestor = GrdvFaltasJ.DataKeys[gvRow.RowIndex].Values["CodigoGestorFJ"].ToString();
                ViewState["CodigoFaltaJ"] = _codigo;
                ViewState["CodigoGestorFJ"] = _codigogestor;
                _resultado = _dtbfaltasj.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtDescripFJ.Text = _resultado["Descripcion"].ToString();
                TxtObservaFJ.Text = _resultado["Observacion"].ToString();
                ImgAddFJ.Visible = false;
                ImgModFJ.Visible = true;
                ImgDelFJ.Visible = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModFJ_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtDescripFJ.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Descripción..!", this);
                    return;
                }

                _dtbfaltasj = (DataTable)ViewState["FaltasJ"];
                _resultado = _dtbfaltasj.Select("Codigo='" + ViewState["CodigoFaltaJ"].ToString() + "'").FirstOrDefault();
                _resultado["Descripcion"] = TxtDescripFJ.Text.Trim().ToUpper();
                _resultado["Observacion"] = TxtObservaFJ.Text.Trim().ToUpper();
                _dtbfaltasj.AcceptChanges();
                GrdvFaltasJ.DataSource = _dtbfaltasj;
                GrdvFaltasJ.DataBind();
                FunCargarCombos(2);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelFJ_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtbfaltasj = (DataTable)ViewState["FaltasJ"];
                _resultado = _dtbfaltasj.Select("Codigo='" + ViewState["CodigoFaltaJ"].ToString() + "'").FirstOrDefault();
                _resultado.Delete();
                _dtbfaltasj.AcceptChanges();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(174, int.Parse(ViewState["CodigoFaltaJ"].ToString()),
                    0, 0, "", "FTJ", ViewState["Bitacora"].ToString(), ViewState["Conectar"].ToString());
                GrdvFaltasJ.DataSource = _dtbfaltasj;
                GrdvFaltasJ.DataBind();

                if (_dtbfaltasj.Rows.Count == 0) Pnl4.Visible = false;

                FunCargarCombos(2);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvFaltasJ_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgselecc = (ImageButton)(e.Row.Cells[3].FindControl("ImgSeleccFJ"));
                    _codigo = GrdvFaltasJ.DataKeys[e.Row.RowIndex].Values["FirmaCodigo"].ToString();

                    if (_codigo != Session["usuCodigo"].ToString())
                    {
                        _imgselecc.ImageUrl = "~/Botones/editargris.png";
                        _imgselecc.Enabled = false;
                        e.Row.Cells[3].BackColor = Color.SlateGray;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddFI_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlGestor.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Colaborador..!", this);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtDescripFI.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Descripción..!", this);
                    return;
                }

                if (ViewState["FaltasI"] != null)
                {
                    _dtbfaltasi = (DataTable)ViewState["FaltasI"];

                    if (_dtbfaltasi.Rows.Count > 0)
                        _maxcodigo = _dtbfaltasi.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbfaltasi.Select("CodigoGestorFI='" + DdlGestor.SelectedValue + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Gestor ya existe ingresado..!", this);
                    return;
                }

                _filagre = _dtbfaltasi.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Gestor"] = DdlGestor.SelectedItem.ToString();
                _filagre["CodigoGestorFI"] = DdlGestor.SelectedValue;
                _filagre["Descripcion"] = TxtDescripFI.Text.Trim().ToUpper();
                _filagre["Observacion"] = TxtObservaFI.Text.Trim().ToUpper();
                _filagre["Firma"] = Session["usuNombres"].ToString();
                _filagre["FirmaCodigo"] = Session["usuCodigo"].ToString();
                _dtbfaltasi.Rows.Add(_filagre);
                _dtbfaltasi.DefaultView.Sort = "Gestor";
                _dtbfaltasi = _dtbfaltasi.DefaultView.ToTable();
                ViewState["FaltasI"] = _dtbfaltasi;
                GrdvFaltasI.DataSource = _dtbfaltasi;
                GrdvFaltasI.DataBind();
                Pnl5.Visible = true;
                FunCargarCombos(3);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModFI_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtDescripFI.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Descripción..!", this);
                    return;
                }

                _dtbfaltasi = (DataTable)ViewState["FaltasI"];
                _resultado = _dtbfaltasi.Select("Codigo='" + ViewState["CodigoFaltaI"].ToString() + "'").FirstOrDefault();
                _resultado["Descripcion"] = TxtDescripFI.Text.Trim().ToUpper();
                _resultado["Observacion"] = TxtObservaFI.Text.Trim().ToUpper();
                _dtbfaltasi.AcceptChanges();
                GrdvFaltasI.DataSource = _dtbfaltasi;
                GrdvFaltasI.DataBind();
                FunCargarCombos(3);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelFI_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtbfaltasi = (DataTable)ViewState["FaltasI"];
                _resultado = _dtbfaltasi.Select("Codigo='" + ViewState["CodigoFaltaI"].ToString() + "'").FirstOrDefault();
                _resultado.Delete();
                _dtbfaltasi.AcceptChanges();
                _dts = new ConsultaDatosDAO().FunConsultaDatos(174, int.Parse(ViewState["CodigoFaltaI"].ToString()),
                    0, 0, "", "FTI", ViewState["Bitacora"].ToString(), ViewState["Conectar"].ToString());
                GrdvFaltasI.DataSource = _dtbfaltasi;
                GrdvFaltasI.DataBind();

                if (_dtbfaltasi.Rows.Count == 0) Pnl5.Visible = false;

                FunCargarCombos(3);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSeleccFI_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow fr in GrdvFaltasI.Rows)
                {
                    fr.Cells[0].BackColor = Color.White;
                }

                GrdvFaltasI.Rows[gvRow.RowIndex].Cells[0].BackColor = Color.Coral;
                _dtbfaltasi = (DataTable)ViewState["FaltasI"];
                _codigo = GrdvFaltasI.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                _codigogestor = GrdvFaltasI.DataKeys[gvRow.RowIndex].Values["CodigoGestorFI"].ToString();
                ViewState["CodigoFaltaI"] = _codigo;
                ViewState["CodigoGestorFI"] = _codigogestor;
                _resultado = _dtbfaltasi.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtDescripFI.Text = _resultado["Descripcion"].ToString();
                TxtObservaFI.Text = _resultado["Observacion"].ToString();
                ImgAddFI.Visible = false;
                ImgModFI.Visible = true;
                ImgDelFI.Visible = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvFaltasI_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgselecc = (ImageButton)(e.Row.Cells[3].FindControl("ImgSeleccFI"));
                    _codigo = GrdvFaltasI.DataKeys[e.Row.RowIndex].Values["FirmaCodigo"].ToString();

                    if (_codigo != Session["usuCodigo"].ToString())
                    {
                        _imgselecc.ImageUrl = "~/Botones/editargris.png";
                        _imgselecc.Enabled = false;
                        e.Row.Cells[3].BackColor = Color.SlateGray;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddPE_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlGestor.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Colaborador..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtDescripPE.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Descripcion..!", this);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtFechaPE.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Fecha de Permiso Incorrecta..!", this);
                    return;
                }

                if (!new FuncionesDAO().IsDate(TxtFechaPE.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Fecha de Permiso Incorrecta..!", this);
                    return;
                }

                DateTime fechaactual = DateTime.ParseExact(String.Format("{0}", DateTime.Now.ToString("MM/dd/yyyy")), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime fechapermiso = DateTime.ParseExact(String.Format("{0}", TxtFechaPE.Text), "MM/dd/yyyy", CultureInfo.InvariantCulture);

                if (fechapermiso < fechaactual)
                {
                    new FuncionesDAO().FunShowJSMessage("La Fecha Permiso no puede ser menor a la actual..!", this);
                    return;
                }

                if (ViewState["Permiso"] != null)
                {
                    _dtbpermisos = (DataTable)ViewState["Permiso"];

                    if (_dtbpermisos.Rows.Count > 0)
                        _maxcodigo = _dtbpermisos.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbpermisos.Select("CodigoGestorPE='" + DdlGestor.SelectedValue + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Gestor ya existe ingresado..!", this);
                    return;
                }

                _filagre = _dtbpermisos.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Gestor"] = DdlGestor.SelectedItem.ToString();
                _filagre["CodigoGestorPE"] = DdlGestor.SelectedValue;
                _filagre["Descripcion"] = TxtDescripPE.Text.Trim().ToUpper();
                _filagre["Observacion"] = TxtObservaPE.Text.Trim().ToUpper();
                _filagre["Firma"] = Session["usuNombres"].ToString();
                _filagre["FirmaCodigo"] = Session["usuCodigo"].ToString();
                _filagre["FechaPermiso"] = TxtFechaPE.Text.Trim();
                _dtbpermisos.Rows.Add(_filagre);
                _dtbpermisos.DefaultView.Sort = "Gestor";
                _dtbpermisos = _dtbpermisos.DefaultView.ToTable();
                ViewState["Permiso"] = _dtbpermisos;
                GrdvPermisos.DataSource = _dtbpermisos;
                GrdvPermisos.DataBind();
                Pnl6.Visible = true;
                FunCargarCombos(4);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModPE_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtDescripPE.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Descripcion..!", this);
                    return;
                }

                DateTime fechaactual = DateTime.ParseExact(String.Format("{0}", DateTime.Now.ToString("MM/dd/yyyy")), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime fechapermiso = DateTime.ParseExact(String.Format("{0}", TxtFechaPE.Text), "MM/dd/yyyy", CultureInfo.InvariantCulture);

                if (fechapermiso < fechaactual)
                {
                    new FuncionesDAO().FunShowJSMessage("La Fecha Permiso no puede ser menor a la actual..!", this);
                    return;
                }

                _dtbpermisos = (DataTable)ViewState["Permiso"];
                _resultado = _dtbpermisos.Select("Codigo='" + ViewState["CodigoPermiso"].ToString() + "'").FirstOrDefault();
                _resultado["FechaPermiso"] = TxtFechaPE.Text.Trim();
                _resultado["Descripcion"] = TxtDescripPE.Text.Trim().ToUpper();
                _resultado["Observacion"] = TxtObservaPE.Text.Trim().ToUpper();
                _dtbpermisos.AcceptChanges();
                GrdvPermisos.DataSource = _dtbpermisos;
                GrdvPermisos.DataBind();
                FunCargarCombos(4);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelPE_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtbpermisos = (DataTable)ViewState["Permiso"];
                _resultado = _dtbpermisos.Select("Codigo='" + ViewState["CodigoPermiso"].ToString() + "'").FirstOrDefault();
                _resultado.Delete();
                _dtbpermisos.AcceptChanges();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(174, int.Parse(ViewState["CodigoPermiso"].ToString()),
                    0, 0, "", "PER", ViewState["Bitacora"].ToString(), ViewState["Conectar"].ToString());

                GrdvPermisos.DataSource = _dtbpermisos;
                GrdvPermisos.DataBind();

                if (_dtbpermisos.Rows.Count == 0) Pnl6.Visible = false;

                FunCargarCombos(4);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSeleccPE_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow fr in GrdvPermisos.Rows)
                {
                    fr.Cells[0].BackColor = Color.White;
                }

                GrdvPermisos.Rows[gvRow.RowIndex].Cells[0].BackColor = Color.Coral;
                _dtbpermisos = (DataTable)ViewState["Permiso"];
                _codigo = GrdvPermisos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                _codigogestor = GrdvPermisos.DataKeys[gvRow.RowIndex].Values["CodigoGestorPE"].ToString();
                ViewState["CodigoPermiso"] = _codigo;
                ViewState["CodigoGestorPE"] = _codigogestor;
                _resultado = _dtbpermisos.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtFechaPE.Text = _resultado["FechaPermiso"].ToString();
                TxtDescripPE.Text = _resultado["Descripcion"].ToString();
                TxtObservaPE.Text = _resultado["Observacion"].ToString();
                ImgAddPE.Visible = false;
                ImgModPE.Visible = true;
                ImgDelPE.Visible = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvPermisos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgselecc = (ImageButton)(e.Row.Cells[4].FindControl("ImgSeleccPE"));
                    _codigo = GrdvPermisos.DataKeys[e.Row.RowIndex].Values["FirmaCodigo"].ToString();

                    if (_codigo != Session["usuCodigo"].ToString())
                    {
                        _imgselecc.ImageUrl = "~/Botones/editargris.png";
                        _imgselecc.Enabled = false;
                        e.Row.Cells[4].BackColor = Color.SlateGray;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddCT_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlGestor.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Colaborador..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtDescripCT.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Descripcion..!", this);
                    return;
                }

                if (DdlTurnoA.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Turno Actual..!", this);
                    return;
                }

                if (DdlTurnoN.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Turno Cambio..!", this);
                    return;
                }

                if (DdlTurnoA.SelectedValue == DdlTurnoN.SelectedValue)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione diferentes turnos..!", this);
                    return;
                }

                if (ViewState["CambioTurno"] != null)
                {
                    _dtbcambioturno = (DataTable)ViewState["CambioTurno"];

                    if (_dtbcambioturno.Rows.Count > 0)
                        _maxcodigo = _dtbcambioturno.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbcambioturno.Select("CodigoGestorCT='" + DdlGestor.SelectedValue + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Gestor ya existe ingresado..!", this);
                    return;
                }

                _filagre = _dtbcambioturno.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Gestor"] = DdlGestor.SelectedItem.ToString();
                _filagre["CodigoGestorCT"] = DdlGestor.SelectedValue;
                _filagre["Descripcion"] = TxtDescripCT.Text.Trim().ToUpper();
                _filagre["Observacion"] = TxtObservaCT.Text.Trim().ToUpper();
                _filagre["Firma"] = Session["usuNombres"].ToString();
                _filagre["FirmaCodigo"] = Session["usuCodigo"].ToString();
                _filagre["TurnoA"] = DdlTurnoA.SelectedValue;
                _filagre["TurnoN"] = DdlTurnoN.SelectedValue;
                _filagre["TurnoActual"] = DdlTurnoA.SelectedItem.ToString();
                _filagre["TurnoNuevo"] = DdlTurnoN.SelectedItem.ToString();
                _dtbcambioturno.Rows.Add(_filagre);
                _dtbcambioturno.DefaultView.Sort = "Gestor";
                _dtbcambioturno = _dtbcambioturno.DefaultView.ToTable();
                ViewState["CambioTurno"] = _dtbcambioturno;
                GrdvCambioTurno.DataSource = _dtbcambioturno;
                GrdvCambioTurno.DataBind();
                Pnl7.Visible = true;
                FunCargarCombos(5);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvCambioTurno_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgselecc = (ImageButton)(e.Row.Cells[4].FindControl("ImgSeleccCT"));
                    _codigo = GrdvCambioTurno.DataKeys[e.Row.RowIndex].Values["FirmaCodigo"].ToString();

                    if (_codigo != Session["usuCodigo"].ToString())
                    {
                        _imgselecc.ImageUrl = "~/Botones/editargris.png";
                        _imgselecc.Enabled = false;
                        e.Row.Cells[5].BackColor = Color.SlateGray;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModCT_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtDescripCT.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Descripcion..!", this);
                    return;
                }

                if (DdlTurnoA.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Turno Actual..!", this);
                    return;
                }

                if (DdlTurnoN.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Turno Cambio..!", this);
                    return;
                }

                if (DdlTurnoA.SelectedValue == DdlTurnoN.SelectedValue)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione diferentes turnos..!", this);
                    return;
                }

                _dtbcambioturno = (DataTable)ViewState["CambioTurno"];
                _resultado = _dtbcambioturno.Select("Codigo='" + ViewState["CodigoCambioT"].ToString() + "'").FirstOrDefault();
                _resultado["TurnoA"] = DdlTurnoA.SelectedValue;
                _resultado["TurnoN"] = DdlTurnoN.SelectedValue;
                _resultado["TurnoActual"] = DdlTurnoA.SelectedItem.ToString();
                _resultado["TurnoNuevo"] = DdlTurnoN.SelectedItem.ToString();
                _resultado["Descripcion"] = TxtDescripCT.Text.Trim().ToUpper();
                _resultado["Observacion"] = TxtObservaCT.Text.Trim().ToUpper();
                _dtbcambioturno.AcceptChanges();
                GrdvCambioTurno.DataSource = _dtbcambioturno;
                GrdvCambioTurno.DataBind();
                FunCargarCombos(5);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelCT_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtbcambioturno = (DataTable)ViewState["CambioTurno"];
                _resultado = _dtbcambioturno.Select("Codigo='" + ViewState["CodigoCambioT"].ToString() + "'").FirstOrDefault();
                _resultado.Delete();
                _dtbcambioturno.AcceptChanges();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(174, int.Parse(ViewState["CodigoCambioT"].ToString()),
                    0, 0, "", "CTU", ViewState["Bitacora"].ToString(), ViewState["Conectar"].ToString());

                GrdvCambioTurno.DataSource = _dtbcambioturno;
                GrdvCambioTurno.DataBind();

                if (_dtbcambioturno.Rows.Count == 0) Pnl7.Visible = false;

                FunCargarCombos(5);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSeleccCT_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow fr in GrdvCambioTurno.Rows)
                {
                    fr.Cells[0].BackColor = Color.White;
                }

                GrdvCambioTurno.Rows[gvRow.RowIndex].Cells[0].BackColor = Color.Coral;
                _dtbcambioturno = (DataTable)ViewState["CambioTurno"];
                _codigo = GrdvCambioTurno.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                _codigogestor = GrdvCambioTurno.DataKeys[gvRow.RowIndex].Values["CodigoGestorCT"].ToString();
                ViewState["CodigoCambioT"] = _codigo;
                ViewState["CodigoGestorCT"] = _codigogestor;
                _resultado = _dtbcambioturno.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                DdlTurnoA.SelectedValue = _resultado["TurnoA"].ToString();
                DdlTurnoN.SelectedValue = _resultado["TurnoN"].ToString();
                TxtDescripCT.Text = _resultado["Descripcion"].ToString();
                TxtObservaCT.Text = _resultado["Observacion"].ToString();
                ImgAddCT.Visible = false;
                ImgModCT.Visible = true;
                ImgDelCT.Visible = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }


        protected void ImgAddVA_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlGestor.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Colaborador..!", this);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtDescripVA.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese alguna Descripción..!", this);
                    return;
                }

                if (ViewState["Varios"] != null)
                {
                    _dtbvarios = (DataTable)ViewState["Varios"];
                    if (_dtbvarios.Rows.Count > 0)
                        _maxcodigo = _dtbvarios.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbvarios.Select("CodigoGestorVA='" + DdlGestor.SelectedValue + "'and Descripcion='" +
                        TxtDescripVA.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Colaborador ya existe ingresado..!", this);
                    return;
                }

                _filagre = _dtbvarios.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Gestor"] = DdlGestor.SelectedItem.ToString();
                _filagre["CodigoGestorVA"] = DdlGestor.SelectedValue;
                _filagre["Descripcion"] = TxtDescripVA.Text.Trim().ToUpper();
                _filagre["Observacion"] = TxtObservaVA.Text.Trim().ToUpper();
                _filagre["Firma"] = Session["usuNombres"].ToString();
                _filagre["FirmaCodigo"] = Session["usuCodigo"].ToString();
                _dtbvarios.Rows.Add(_filagre);
                _dtbvarios.DefaultView.Sort = "Gestor";
                _dtbvarios = _dtbvarios.DefaultView.ToTable();
                ViewState["Varios"] = _dtbvarios;
                GrdvVarios.DataSource = _dtbvarios;
                GrdvVarios.DataBind();
                Pnl8.Visible = true;
                FunCargarCombos(6);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModVA_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtDescripVA.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese alguna Descripción..!", this);
                    return;
                }

                _dtbvarios = (DataTable)ViewState["Varios"];
                _resultado = _dtbvarios.Select("Codigo='" + ViewState["CodigoVarios"].ToString() + "'").FirstOrDefault();
                _resultado["Descripcion"] = TxtDescripVA.Text.Trim().ToUpper();
                _resultado["Observacion"] = TxtObservaVA.Text.Trim().ToUpper();
                _dtbvarios.AcceptChanges();
                GrdvVarios.DataSource = _dtbvarios;
                GrdvVarios.DataBind();
                FunCargarCombos(6);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelVA_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtbvarios = (DataTable)ViewState["Varios"];
                _resultado = _dtbvarios.Select("Codigo='" + ViewState["CodigoVarios"].ToString() + "'").FirstOrDefault();
                _resultado.Delete();
                _dtbvarios.AcceptChanges();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(174, int.Parse(ViewState["CodigoVarios"].ToString()),
                    0, 0, "", "VAR", ViewState["Bitacora"].ToString(), ViewState["Conectar"].ToString());

                GrdvVarios.DataSource = _dtbvarios;
                GrdvVarios.DataBind();

                if (_dtbvarios.Rows.Count == 0) Pnl8.Visible = false;

                FunCargarCombos(6);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSeleccVA_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow fr in GrdvVarios.Rows)
                {
                    fr.Cells[0].BackColor = Color.White;
                }

                GrdvVarios.Rows[gvRow.RowIndex].Cells[0].BackColor = Color.Coral;
                _dtbvarios = (DataTable)ViewState["Varios"];
                _codigo = GrdvVarios.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                _codigogestor = GrdvVarios.DataKeys[gvRow.RowIndex].Values["CodigoGestorVA"].ToString();
                ViewState["CodigoVarios"] = _codigo;
                ViewState["CodigoGestorVA"] = _codigogestor;
                _resultado = _dtbvarios.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtDescripVA.Text = _resultado["Descripcion"].ToString();
                TxtObservaVA.Text = _resultado["Observacion"].ToString();
                ImgAddVA.Visible = false;
                ImgModVA.Visible = true;
                ImgDelVA.Visible = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvVarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgselecc = (ImageButton)(e.Row.Cells[3].FindControl("ImgSeleccVA"));
                    _codigo = GrdvVarios.DataKeys[e.Row.RowIndex].Values["FirmaCodigo"].ToString();

                    if (_codigo != Session["usuCodigo"].ToString())
                    {
                        _imgselecc.ImageUrl = "~/Botones/editargris.png";
                        _imgselecc.Enabled = false;
                        e.Row.Cells[3].BackColor = Color.SlateGray;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddNV_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtDescripNV.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese alguna Descripción..!", this);
                    return;
                }

                if (ViewState["Novedades"] != null)
                {
                    _dtbnovedades = (DataTable)ViewState["Novedades"];

                    if (_dtbnovedades.Rows.Count > 0)
                        _maxcodigo = _dtbnovedades.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbnovedades.Select("Descripcion='" + TxtDescripNV.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Descripción ya existe ingresado..!", this);
                    return;
                }

                _filagre = _dtbnovedades.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Descripcion"] = TxtDescripNV.Text.Trim().ToUpper();
                _filagre["Observacion"] = TxtObservaNV.Text.Trim().ToUpper();
                _filagre["Firma"] = Session["usuNombres"].ToString();
                _filagre["FirmaCodigo"] = Session["usuCodigo"].ToString();
                _dtbnovedades.Rows.Add(_filagre);
                ViewState["Novedades"] = _dtbnovedades;
                GrdvNovedad.DataSource = _dtbnovedades;
                GrdvNovedad.DataBind();
                Pnl9.Visible = true;
                FunCargarCombos(7);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSeleccNV_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow fr in GrdvNovedad.Rows)
                {
                    fr.Cells[0].BackColor = Color.White;
                }

                GrdvNovedad.Rows[gvRow.RowIndex].Cells[0].BackColor = Color.Coral;
                _dtbnovedades = (DataTable)ViewState["Novedades"];
                _codigo = GrdvNovedad.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                ViewState["CodigoNovedad"] = _codigo;
                _resultado = _dtbnovedades.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtDescripNV.Text = _resultado["Descripcion"].ToString();
                TxtObservaNV.Text = _resultado["Observacion"].ToString();
                ImgAddNV.Visible = false;
                ImgModNV.Visible = true;
                ImgDelNV.Visible = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModNV_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtDescripNV.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese alguna Descripción..!", this);
                    return;
                }

                _dtbnovedades = (DataTable)ViewState["Novedades"];
                _resultado = _dtbnovedades.Select("Codigo='" + ViewState["CodigoNovedad"].ToString() + "'").FirstOrDefault();
                _resultado["Descripcion"] = TxtDescripNV.Text.Trim().ToUpper();
                _resultado["Observacion"] = TxtObservaNV.Text.Trim().ToUpper();
                _dtbnovedades.AcceptChanges();
                GrdvNovedad.DataSource = _dtbnovedades;
                GrdvNovedad.DataBind();
                FunCargarCombos(7);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelNV_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtbnovedades = (DataTable)ViewState["Novedades"];
                _resultado = _dtbnovedades.Select("Codigo='" + ViewState["CodigoNovedad"].ToString() + "'").FirstOrDefault();
                _resultado.Delete();
                _dtbnovedades.AcceptChanges();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(174, int.Parse(ViewState["CodigoNovedad"].ToString()),
                    0, 0, "", "NOV", ViewState["Bitacora"].ToString(),
                    ViewState["Conectar"].ToString());

                GrdvNovedad.DataSource = _dtbvarios;
                GrdvNovedad.DataBind();

                if (_dtbnovedades.Rows.Count == 0) Pnl9.Visible = false;

                FunCargarCombos(7);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvNovedad_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgselecc = (ImageButton)(e.Row.Cells[2].FindControl("ImgSeleccNV"));
                    _codigo = GrdvNovedad.DataKeys[e.Row.RowIndex].Values["FirmaCodigo"].ToString();

                    if (_codigo != Session["usuCodigo"].ToString())
                    {
                        _imgselecc.ImageUrl = "~/Botones/editargris.png";
                        _imgselecc.Enabled = false;
                        e.Row.Cells[2].BackColor = Color.SlateGray;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddRE_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlGestor.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Colaborador..!", this);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtDescripRE.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese alguna Descripción..!", this);
                    return;
                }

                if (ViewState["Refuerzos"] != null)
                {
                    _dtbrefuerzos = (DataTable)ViewState["Refuerzos"];

                    if (_dtbrefuerzos.Rows.Count > 0)
                        _maxcodigo = _dtbrefuerzos.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbrefuerzos.Select("CodigoGestorRE='" + DdlGestor.SelectedValue + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Colaborador ya existe ingresado..!", this);
                    return;
                }

                _filagre = _dtbrefuerzos.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Gestor"] = DdlGestor.SelectedItem.ToString();
                _filagre["CodigoGestorRE"] = DdlGestor.SelectedValue;
                _filagre["Descripcion"] = TxtDescripRE.Text.Trim().ToUpper();
                _filagre["Observacion"] = TxtObservaRE.Text.Trim().ToUpper();
                _filagre["Firma"] = Session["usuNombres"].ToString();
                _filagre["FirmaCodigo"] = Session["usuCodigo"].ToString();
                _dtbrefuerzos.Rows.Add(_filagre);
                ViewState["Refuerzos"] = _dtbrefuerzos;
                GrdvRefuerzo.DataSource = _dtbrefuerzos;
                GrdvRefuerzo.DataBind();
                Pnl10.Visible = true;
                FunCargarCombos(8);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModRE_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtDescripRE.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese alguna Descripción..!", this);
                    return;
                }

                _dtbrefuerzos = (DataTable)ViewState["Refuerzos"];
                _resultado = _dtbrefuerzos.Select("Codigo='" + ViewState["CodigoRefuerzo"].ToString() + "'").FirstOrDefault();
                _resultado["Descripcion"] = TxtDescripRE.Text.Trim().ToUpper();
                _resultado["Observacion"] = TxtObservaRE.Text.Trim().ToUpper();
                _dtbrefuerzos.AcceptChanges();
                GrdvRefuerzo.DataSource = _dtbrefuerzos;
                GrdvRefuerzo.DataBind();
                FunCargarCombos(8);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelRE_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtbrefuerzos = (DataTable)ViewState["Refuerzos"];
                _resultado = _dtbrefuerzos.Select("Codigo='" + ViewState["CodigoRefuerzo"].ToString() + "'").FirstOrDefault();
                _resultado.Delete();
                _dtbrefuerzos.AcceptChanges();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(174, int.Parse(ViewState["CodigoRefuerzo"].ToString()),
                    0, 0, "", "REF", ViewState["Bitacora"].ToString(), ViewState["Conectar"].ToString());

                GrdvRefuerzo.DataSource = _dtbrefuerzos;
                GrdvRefuerzo.DataBind();

                if (_dtbrefuerzos.Rows.Count == 0) Pnl10.Visible = false;

                FunCargarCombos(8);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSeleccRE_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow fr in GrdvRefuerzo.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvRefuerzo.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _dtbrefuerzos = (DataTable)ViewState["Refuerzos"];
                _codigo = GrdvRefuerzo.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                _codigogestor = GrdvRefuerzo.DataKeys[gvRow.RowIndex].Values["CodigoGestorRE"].ToString();
                ViewState["CodigoRefuerzo"] = _codigo;
                ViewState["CodigoGestorRE"] = _codigogestor;
                _resultado = _dtbrefuerzos.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtDescripRE.Text = _resultado["Descripcion"].ToString();
                TxtObservaRE.Text = _resultado["Observacion"].ToString();
                ImgAddRE.Visible = false;
                ImgModRE.Visible = true;
                ImgDelRE.Visible = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvRefuerzo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgselecc = (ImageButton)(e.Row.Cells[3].FindControl("ImgSeleccRE"));
                    _codigo = GrdvRefuerzo.DataKeys[e.Row.RowIndex].Values["FirmaCodigo"].ToString();

                    if (_codigo != Session["usuCodigo"].ToString())
                    {
                        _imgselecc.ImageUrl = "~/Botones/editargris.png";
                        _imgselecc.Enabled = false;
                        e.Row.Cells[3].BackColor = Color.SlateGray;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddGT_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtDescripGT.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese alguna Descripción..!", this);
                    return;
                }

                if (ViewState["Terreno"] != null)
                {
                    _dtbterreno = (DataTable)ViewState["Terreno"];

                    if (_dtbterreno.Rows.Count > 0)
                        _maxcodigo = _dtbterreno.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbterreno.Select("Descripcion='" + TxtDescripGT.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Descripción ya existe ingresado..!", this);
                    return;
                }

                _filagre = _dtbterreno.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Descripcion"] = TxtDescripGT.Text.Trim().ToUpper();
                _filagre["Observacion"] = TxtObservaGT.Text.Trim().ToUpper();
                _filagre["Firma"] = Session["usuNombres"].ToString();
                _filagre["FirmaCodigo"] = Session["usuCodigo"].ToString();
                _dtbterreno.Rows.Add(_filagre);
                ViewState["Terreno"] = _dtbterreno;
                GrdvTerreno.DataSource = _dtbterreno;
                GrdvTerreno.DataBind();
                Pnl11.Visible = true;
                FunCargarCombos(9);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModGT_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtDescripGT.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese alguna Descripción..!", this);
                    return;
                }

                _dtbterreno = (DataTable)ViewState["Terreno"];
                _resultado = _dtbterreno.Select("Codigo='" + ViewState["CodigoTerreno"].ToString() + "'").FirstOrDefault();
                _resultado["Descripcion"] = TxtDescripGT.Text.Trim().ToUpper();
                _resultado["Observacion"] = TxtObservaGT.Text.Trim().ToUpper();
                _dtbterreno.AcceptChanges();
                GrdvTerreno.DataSource = _dtbterreno;
                GrdvTerreno.DataBind();
                FunCargarCombos(9);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelGT_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtbterreno = (DataTable)ViewState["Terreno"];
                _resultado = _dtbterreno.Select("Codigo='" + ViewState["CodigoTerreno"].ToString() + "'").FirstOrDefault();
                _resultado.Delete();
                _dtbterreno.AcceptChanges();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(174, int.Parse(ViewState["CodigoTerreno"].ToString()),
                    0, 0, "", "GTE", ViewState["Bitacora"].ToString(), ViewState["Conectar"].ToString());

                GrdvTerreno.DataSource = _dtbterreno;
                GrdvTerreno.DataBind();

                if (_dtbterreno.Rows.Count == 0) Pnl11.Visible = false;

                FunCargarCombos(9);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSeleccGT_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow fr in GrdvTerreno.Rows)
                {
                    fr.Cells[0].BackColor = Color.White;
                }

                GrdvTerreno.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _dtbterreno = (DataTable)ViewState["Terreno"];
                _codigo = GrdvTerreno.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                ViewState["CodigoTerreno"] = _codigo;
                _resultado = _dtbterreno.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtDescripGT.Text = _resultado["Descripcion"].ToString();
                TxtObservaGT.Text = _resultado["Observacion"].ToString();
                ImgAddGT.Visible = false;
                ImgModGT.Visible = true;
                ImgDelGT.Visible = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvTerreno_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgselecc = (ImageButton)(e.Row.Cells[2].FindControl("ImgSeleccGT"));
                    _codigo = GrdvTerreno.DataKeys[e.Row.RowIndex].Values["FirmaCodigo"].ToString();

                    if (_codigo != Session["usuCodigo"].ToString())
                    {
                        _imgselecc.ImageUrl = "~/Botones/editargris.png";
                        _imgselecc.Enabled = false;
                        e.Row.Cells[2].BackColor = Color.SlateGray;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddNS_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtDescripNS.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese alguna Descripción..!", this);
                    return;
                }

                if (ViewState["Sistemas"] != null)
                {
                    _dtbsistemas = (DataTable)ViewState["Sistemas"];

                    if (_dtbsistemas.Rows.Count > 0)
                        _maxcodigo = _dtbsistemas.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbsistemas.Select("Descripcion='" + TxtDescripNS.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Descripción ya existe ingresada..!", this);
                    return;
                }

                _filagre = _dtbsistemas.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Descripcion"] = TxtDescripNS.Text.Trim().ToUpper();
                _filagre["Observacion"] = TxtObservaNS.Text.Trim().ToUpper();
                _filagre["Firma"] = Session["usuNombres"].ToString();
                _filagre["FirmaCodigo"] = Session["usuCodigo"].ToString();
                _dtbsistemas.Rows.Add(_filagre);

                ViewState["Sistemas"] = _dtbsistemas;
                GrdvSistemas.DataSource = _dtbsistemas;
                GrdvSistemas.DataBind();
                Pnl12.Visible = true;
                FunCargarCombos(10);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModNS_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtDescripNS.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese alguna Descripción..!", this);
                    return;
                }

                _dtbsistemas = (DataTable)ViewState["Sistemas"];
                _resultado = _dtbsistemas.Select("Codigo='" + ViewState["CodigoSistemas"].ToString() + "'").FirstOrDefault();
                _resultado["Descripcion"] = TxtDescripNS.Text.Trim().ToUpper();
                _resultado["Observacion"] = TxtObservaNS.Text.Trim().ToUpper();
                _dtbsistemas.AcceptChanges();
                GrdvSistemas.DataSource = _dtbsistemas;
                GrdvSistemas.DataBind();
                FunCargarCombos(10);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelNS_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtbsistemas = (DataTable)ViewState["Sistemas"];
                _resultado = _dtbsistemas.Select("Codigo='" + ViewState["CodigoSistemas"].ToString() + "'").FirstOrDefault();
                _resultado.Delete();
                _dtbsistemas.AcceptChanges();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(174, int.Parse(ViewState["CodigoSistemas"].ToString()),
                    0, 0, "", "SIS", ViewState["Bitacora"].ToString(), ViewState["Conectar"].ToString());

                GrdvSistemas.DataSource = _dtbsistemas;
                GrdvSistemas.DataBind();

                if (_dtbsistemas.Rows.Count == 0) Pnl12.Visible = false;

                FunCargarCombos(10);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSeleccNS_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow fr in GrdvSistemas.Rows)
                {
                    fr.Cells[0].BackColor = Color.White;
                }

                GrdvSistemas.Rows[gvRow.RowIndex].Cells[0].BackColor = Color.Coral;
                _dtbsistemas = (DataTable)ViewState["Sistemas"];
                _codigo = GrdvSistemas.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                ViewState["CodigoSistemas"] = _codigo;
                _resultado = _dtbsistemas.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtDescripNS.Text = _resultado["Descripcion"].ToString();
                TxtObservaNS.Text = _resultado["Observacion"].ToString();
                ImgAddNS.Visible = false;
                ImgModNS.Visible = true;
                ImgDelNS.Visible = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvSistemas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgselecc = (ImageButton)(e.Row.Cells[2].FindControl("ImgSeleccNS"));
                    _codigo = GrdvSistemas.DataKeys[e.Row.RowIndex].Values["FirmaCodigo"].ToString();

                    if (_codigo != Session["usuCodigo"].ToString())
                    {
                        _imgselecc.ImageUrl = "~/Botones/editargris.png";
                        _imgselecc.Enabled = false;
                        e.Row.Cells[2].BackColor = Color.SlateGray;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddCP_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtDescripCP.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese alguna Descripción..!", this);
                    return;
                }

                if (ViewState["Pagos"] != null)
                {
                    _dtbpagos = (DataTable)ViewState["Pagos"];

                    if (_dtbpagos.Rows.Count > 0)
                        _maxcodigo = _dtbpagos.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbpagos.Select("Descripcion='" + TxtDescripCP.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Descripción ya existe ingresada..!", this);
                    return;
                }

                _filagre = _dtbpagos.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Descripcion"] = TxtDescripCP.Text.Trim().ToUpper();
                _filagre["Observacion"] = TxtObservaCP.Text.Trim().ToUpper();
                _filagre["Firma"] = Session["usuNombres"].ToString();
                _filagre["FirmaCodigo"] = Session["usuCodigo"].ToString();
                _dtbpagos.Rows.Add(_filagre);

                ViewState["Pagos"] = _dtbpagos;
                GrdvPagos.DataSource = _dtbpagos;
                GrdvPagos.DataBind();
                Pnl13.Visible = true;
                FunCargarCombos(11);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModCP_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtDescripCP.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese alguna Descripción..!", this);
                    return;
                }

                _dtbpagos = (DataTable)ViewState["Pagos"];
                _resultado = _dtbpagos.Select("Codigo='" + ViewState["CodigoPagos"].ToString() + "'").FirstOrDefault();
                _resultado["Descripcion"] = TxtDescripCP.Text.Trim().ToUpper();
                _resultado["Observacion"] = TxtObservaCP.Text.Trim().ToUpper();
                _dtbpagos.AcceptChanges();
                GrdvPagos.DataSource = _dtbpagos;
                GrdvPagos.DataBind();
                FunCargarCombos(11);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelCP_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtbpagos = (DataTable)ViewState["Pagos"];
                _resultado = _dtbpagos.Select("Codigo='" + ViewState["CodigoPagos"].ToString() + "'").FirstOrDefault();
                _resultado.Delete();
                _dtbpagos.AcceptChanges();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(174, int.Parse(ViewState["CodigoPagos"].ToString()),
                    0, 0, "", "CPA", ViewState["Bitacora"].ToString(), ViewState["Conectar"].ToString());

                GrdvPagos.DataSource = _dtbpagos;
                GrdvPagos.DataBind();

                if (_dtbpagos.Rows.Count == 0) Pnl13.Visible = false;

                FunCargarCombos(11);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSeleccCP_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow fr in GrdvPagos.Rows)
                {
                    fr.Cells[0].BackColor = Color.White;
                }

                GrdvPagos.Rows[gvRow.RowIndex].Cells[0].BackColor = Color.Coral;
                _dtbpagos = (DataTable)ViewState["Pagos"];
                _codigo = GrdvPagos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                ViewState["CodigoPagos"] = _codigo;
                _resultado = _dtbpagos.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtDescripCP.Text = _resultado["Descripcion"].ToString();
                ImgAddCP.Visible = false;
                ImgModCP.Visible = true;
                ImgDelCP.Visible = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgselecc = (ImageButton)(e.Row.Cells[2].FindControl("ImgSeleccCP"));
                    _codigo = GrdvPagos.DataKeys[e.Row.RowIndex].Values["FirmaCodigo"].ToString();

                    if (_codigo != Session["usuCodigo"].ToString())
                    {
                        _imgselecc.ImageUrl = "~/Botones/editargris.png";
                        _imgselecc.Enabled = false;
                        e.Row.Cells[2].BackColor = Color.SlateGray;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddAD_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtDescripAD.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese alguna Descripción..!", this);
                    return;
                }

                if (ViewState["Adicionales"] != null)
                {
                    _dtbadicional = (DataTable)ViewState["Adicionales"];

                    if (_dtbadicional.Rows.Count > 0)
                        _maxcodigo = _dtbadicional.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbadicional.Select("Descripcion='" + TxtDescripAD.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesDAO().FunShowJSMessage("Descripción ya existe ingresada..!", this);
                    return;
                }

                _filagre = _dtbadicional.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Descripcion"] = TxtDescripAD.Text.Trim().ToUpper();
                _filagre["Observacion"] = TxtObservaAD.Text.Trim().ToUpper();
                _filagre["Firma"] = Session["usuNombres"].ToString();
                _filagre["FirmaCodigo"] = Session["usuCodigo"].ToString();
                _dtbadicional.Rows.Add(_filagre);

                ViewState["Adicionales"] = _dtbadicional;
                GrdvAdicionales.DataSource = _dtbadicional;
                GrdvAdicionales.DataBind();
                Pnl14.Visible = true;
                FunCargarCombos(12);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModAD_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtDescripAD.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese alguna Descripción..!", this);
                    return;
                }

                _dtbadicional = (DataTable)ViewState["Adicionales"];
                _resultado = _dtbadicional.Select("Codigo='" + ViewState["CodigoAdicional"].ToString() + "'").FirstOrDefault();
                _resultado["Descripcion"] = TxtDescripAD.Text.Trim().ToUpper();
                _resultado["Observacion"] = TxtObservaAD.Text.Trim().ToUpper();
                _dtbadicional.AcceptChanges();
                GrdvAdicionales.DataSource = _dtbadicional;
                GrdvAdicionales.DataBind();
                FunCargarCombos(12);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelAD_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtbadicional = (DataTable)ViewState["Adicionales"];
                _resultado = _dtbadicional.Select("Codigo='" + ViewState["CodigoAdicional"].ToString() + "'").FirstOrDefault();
                _resultado.Delete();
                _dtbadicional.AcceptChanges();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(174, int.Parse(ViewState["CodigoAdicional"].ToString()),
                    0, 0, "", "ADI", ViewState["Bitacora"].ToString(), ViewState["Conectar"].ToString());
                GrdvAdicionales.DataSource = _dtbadicional;
                GrdvAdicionales.DataBind();

                if (_dtbadicional.Rows.Count == 0) Pnl14.Visible = false;

                FunCargarCombos(12);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSeleccAD_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow fr in GrdvAdicionales.Rows)
                {
                    fr.Cells[0].BackColor = Color.White;
                }

                GrdvAdicionales.Rows[gvRow.RowIndex].Cells[0].BackColor = Color.Coral;
                _dtbadicional = (DataTable)ViewState["Adicionales"];
                _codigo = GrdvAdicionales.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                ViewState["CodigoAdicional"] = _codigo;
                _resultado = _dtbadicional.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtDescripAD.Text = _resultado["Descripcion"].ToString();
                TxtObservaAD.Text = _resultado["Observacion"].ToString();
                ImgAddAD.Visible = false;
                ImgModAD.Visible = true;
                ImgDelAD.Visible = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvAdicionales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgselecc = (ImageButton)(e.Row.Cells[2].FindControl("ImgSeleccAD"));
                    _codigo = GrdvAdicionales.DataKeys[e.Row.RowIndex].Values["FirmaCodigo"].ToString();

                    if (_codigo != Session["usuCodigo"].ToString())
                    {
                        _imgselecc.ImageUrl = "~/Botones/editargris.png";
                        _imgselecc.Enabled = false;
                        e.Row.Cells[2].BackColor = Color.SlateGray;
                    }
                }
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
                _dtbatrasos = (DataTable)ViewState["Atrasos"];

                foreach (DataRow _drfila in _dtbatrasos.Rows)
                {
                    _dts = new BitacoraDAO().FunNewBitacora(0, ViewState["Bitacora"].ToString(), "ATR",
                        int.Parse(_drfila["Codigo"].ToString()), _drfila["Observacion"].ToString(),
                        int.Parse(_drfila["CodigoGestorAT"].ToString()), _drfila["Descripcion"].ToString(),
                        ViewState["FechaActual"].ToString(), _drfila["Hora"].ToString(), "", "",
                        int.Parse(Session["usuCodigo"].ToString()), "", "", "", "", "", 0, 0, 0, 0, 0,
                        Session["MachineName"].ToString(), ViewState["Conectar"].ToString());
                }

                _dtbfaltasj = (DataTable)ViewState["FaltasJ"];

                foreach (DataRow _drfila in _dtbfaltasj.Rows)
                {
                    _dts = new BitacoraDAO().FunNewBitacora(0, ViewState["Bitacora"].ToString(), "FTJ",
                        int.Parse(_drfila["Codigo"].ToString()), _drfila["Observacion"].ToString(),
                        int.Parse(_drfila["CodigoGestorFJ"].ToString()), _drfila["Descripcion"].ToString(),
                        DateTime.Now.ToString("MM/dd/yyyy"), DateTime.Now.ToString("HH:mm"), "", "",
                        int.Parse(Session["usuCodigo"].ToString()), "", "", "", "", "", 0, 0, 0, 0, 0,
                        Session["MachineName"].ToString(), ViewState["Conectar"].ToString());
                }

                _dtbfaltasi = (DataTable)ViewState["FaltasI"];

                foreach (DataRow _drfila in _dtbfaltasi.Rows)
                {
                    _dts = new BitacoraDAO().FunNewBitacora(0, ViewState["Bitacora"].ToString(), "FTI",
                        int.Parse(_drfila["Codigo"].ToString()), _drfila["Observacion"].ToString(),
                        int.Parse(_drfila["CodigoGestorFI"].ToString()), _drfila["Descripcion"].ToString(),
                        DateTime.Now.ToString("MM/dd/yyyy"), DateTime.Now.ToString("HH:mm"), "", "",
                        int.Parse(Session["usuCodigo"].ToString()), "", "", "", "", "", 0, 0, 0, 0, 0,
                        Session["MachineName"].ToString(), ViewState["Conectar"].ToString());
                }

                _dtbpermisos = (DataTable)ViewState["Permiso"];

                foreach (DataRow _drfila in _dtbpermisos.Rows)
                {
                    _dts = new BitacoraDAO().FunNewBitacora(0, ViewState["Bitacora"].ToString(), "PER",
                        int.Parse(_drfila["Codigo"].ToString()), _drfila["Observacion"].ToString(),
                        int.Parse(_drfila["CodigoGestorPE"].ToString()), _drfila["Descripcion"].ToString(),
                        _drfila["FechaPermiso"].ToString(), DateTime.Now.ToString("HH:mm"), "", "",
                        int.Parse(Session["usuCodigo"].ToString()), "", "", "", "", "", 0, 0, 0, 0, 0,
                        Session["MachineName"].ToString(), ViewState["Conectar"].ToString());
                }

                _dtbcambioturno = (DataTable)ViewState["CambioTurno"];

                foreach (DataRow _drfila in _dtbcambioturno.Rows)
                {
                    _dts = new BitacoraDAO().FunNewBitacora(0, ViewState["Bitacora"].ToString(), "CTU",
                        int.Parse(_drfila["Codigo"].ToString()), _drfila["Observacion"].ToString(),
                        int.Parse(_drfila["CodigoGestorCT"].ToString()), _drfila["Descripcion"].ToString(),
                        DateTime.Now.ToString("MM/dd/yyyy"), DateTime.Now.ToString("HH:mm"), _drfila["TurnoA"].ToString(), _drfila["TurnoN"].ToString(), int.Parse(Session["usuCodigo"].ToString()), "", "", "", "", "", 0, 0, 0, 0, 0, Session["MachineName"].ToString(), ViewState["Conectar"].ToString());
                }

                _dtbvarios = (DataTable)ViewState["Varios"];

                foreach (DataRow _drfila in _dtbvarios.Rows)
                {
                    _dts = new BitacoraDAO().FunNewBitacora(0, ViewState["Bitacora"].ToString(), "VAR",
                        int.Parse(_drfila["Codigo"].ToString()), _drfila["Observacion"].ToString(),
                        int.Parse(_drfila["CodigoGestorVA"].ToString()), _drfila["Descripcion"].ToString(),
                        DateTime.Now.ToString("MM/dd/yyyy"), DateTime.Now.ToString("HH:mm"), "", "",
                        int.Parse(Session["usuCodigo"].ToString()), "", "", "", "", "", 0, 0, 0, 0, 0,
                        Session["MachineName"].ToString(), ViewState["Conectar"].ToString());
                }

                _dtbrefuerzos = (DataTable)ViewState["Refuerzos"];

                foreach (DataRow _drfila in _dtbrefuerzos.Rows)
                {
                    _dts = new BitacoraDAO().FunNewBitacora(0, ViewState["Bitacora"].ToString(), "REF",
                        int.Parse(_drfila["Codigo"].ToString()), _drfila["Observacion"].ToString(),
                        int.Parse(_drfila["CodigoGestorRE"].ToString()), _drfila["Descripcion"].ToString(),
                        DateTime.Now.ToString("MM/dd/yyyy"), DateTime.Now.ToString("HH:mm"), "", "",
                        int.Parse(Session["usuCodigo"].ToString()), "", "", "", "", "", 0, 0, 0, 0, 0,
                        Session["MachineName"].ToString(), ViewState["Conectar"].ToString());
                }

                _dtbnovedades = (DataTable)ViewState["Novedades"];

                foreach (DataRow _drfila in _dtbnovedades.Rows)
                {
                    _dts = new BitacoraDAO().FunNewBitacora(1, ViewState["Bitacora"].ToString(), "NOV",
                        int.Parse(_drfila["Codigo"].ToString()), _drfila["Observacion"].ToString(),
                        0, _drfila["Descripcion"].ToString(), DateTime.Now.ToString("MM/dd/yyyy"),
                        DateTime.Now.ToString("HH:mm"), "", "", int.Parse(Session["usuCodigo"].ToString()), "", "", "", "", "", 0, 0, 0, 0, 0, Session["MachineName"].ToString(), ViewState["Conectar"].ToString());
                }

                _dtbterreno = (DataTable)ViewState["Terreno"];

                foreach (DataRow _drfila in _dtbterreno.Rows)
                {
                    _dts = new BitacoraDAO().FunNewBitacora(1, ViewState["Bitacora"].ToString(), "GTE",
                        int.Parse(_drfila["Codigo"].ToString()), _drfila["Observacion"].ToString(),
                        0, _drfila["Descripcion"].ToString(), DateTime.Now.ToString("MM/dd/yyyy"),
                        DateTime.Now.ToString("HH:mm"), "", "", int.Parse(Session["usuCodigo"].ToString()), "", "", "", "", "", 0, 0, 0, 0, 0, Session["MachineName"].ToString(), ViewState["Conectar"].ToString());
                }

                _dtbsistemas = (DataTable)ViewState["Sistemas"];

                foreach (DataRow _drfila in _dtbsistemas.Rows)
                {
                    _dts = new BitacoraDAO().FunNewBitacora(1, ViewState["Bitacora"].ToString(), "SIS",
                        int.Parse(_drfila["Codigo"].ToString()), _drfila["Observacion"].ToString(),
                        0, _drfila["Descripcion"].ToString(), DateTime.Now.ToString("MM/dd/yyyy"), DateTime.Now.ToString("HH:mm"),
                        "", "", int.Parse(Session["usuCodigo"].ToString()), "", "", "", "", "", 0, 0, 0, 0, 0,
                        Session["MachineName"].ToString(), ViewState["Conectar"].ToString());
                }

                _dtbpagos = (DataTable)ViewState["Pagos"];

                foreach (DataRow _drfila in _dtbpagos.Rows)
                {
                    _dts = new BitacoraDAO().FunNewBitacora(1, ViewState["Bitacora"].ToString(), "CPA",
                        int.Parse(_drfila["Codigo"].ToString()), _drfila["Observacion"].ToString(),
                        0, _drfila["Descripcion"].ToString(), DateTime.Now.ToString("MM/dd/yyyy"), DateTime.Now.ToString("HH:mm"), "", "", int.Parse(Session["usuCodigo"].ToString()), "", "", "", "", "", 0, 0, 0, 0, 0, Session["MachineName"].ToString(), ViewState["Conectar"].ToString());
                }

                _dtbadicional = (DataTable)ViewState["Adicionales"];

                foreach (DataRow _drfila in _dtbadicional.Rows)
                {
                    _dts = new BitacoraDAO().FunNewBitacora(1, ViewState["Bitacora"].ToString(), "ADI",
                        int.Parse(_drfila["Codigo"].ToString()), _drfila["Observacion"].ToString(),
                        0, _drfila["Descripcion"].ToString(), DateTime.Now.ToString("MM/dd/yyyy"), DateTime.Now.ToString("HH:mm"),
                        "", "", int.Parse(Session["usuCodigo"].ToString()), "", "", "", "", "", 0, 0, 0, 0, 0,
                        Session["MachineName"].ToString(), ViewState["Conectar"].ToString());
                }

                _response = string.Format("{0}?MensajeRetornado={1}&Bitacora={2}&Estado={3}&Fecha={4}",
                    Request.Url.AbsolutePath, "Guardado con Éxito", ViewState["Bitacora"].ToString(),
                    ViewState["Estado"].ToString(), ViewState["Fecha"].ToString());

                Response.Redirect(_response, false);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_BitacoraAdmin.aspx", true);
        }
        #endregion
    }
}