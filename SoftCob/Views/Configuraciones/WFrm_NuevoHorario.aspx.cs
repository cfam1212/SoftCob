namespace SoftCob.Views.Configuraciones
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    public partial class WFrm_NuevoHorario : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtbhoras = new DataTable();
        DataRow _filagre;
        TimeSpan _intervalo, _intervalminu, _horainicio, _horafin;
        int _inter = 0, _codigo = 0, _orden = 0;
        string _horainicial, _horafinal = "";
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
                    ViewState["Codigo"] = Request["Codigo"];
                    ViewState["Intervalo"] = "";
                    ViewState["Horario"] = "";
                    FunLlenarCombos(0);
                    FunCargaMantenimiento();

                    if (ViewState["Codigo"].ToString() == "0") Lbltitulo.Text = "Nuevo Horario";
                    else Lbltitulo.Text = "Editar Horario";
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargaMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunHorariobpm(1, int.Parse(ViewState["Codigo"].ToString()), "", "", "",
                    "", "", "", 0, "", "", "", 0, 0, 0, int.Parse((Session["usuCodigo"].ToString())), Session["MachineName"].ToString(),
                    Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    TxtHorario.Text = _dts.Tables[0].Rows[0]["Horario"].ToString();
                    TxtDescripcion.Text = _dts.Tables[0].Rows[0]["Descripcion"].ToString();
                    DdlIntervalo.SelectedValue = _dts.Tables[0].Rows[0]["Intervalo"].ToString();
                    DdlHoraIni.SelectedValue = _dts.Tables[0].Rows[0]["HoraIni"].ToString().Substring(0, 2);
                    DdlMinutoIni.SelectedValue = _dts.Tables[0].Rows[0]["HoraIni"].ToString().Substring(3, 2);
                    DdlHoraFin.SelectedValue = _dts.Tables[0].Rows[0]["HoraHasta"].ToString().Substring(0, 2);
                    DdlMinutoFin.SelectedValue = _dts.Tables[0].Rows[0]["HoraHasta"].ToString().Substring(3, 2);
                    ChkEstado.Text = _dts.Tables[0].Rows[0]["Estado"].ToString();
                    ChkEstado.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                    ViewState["Intervalo"] = _dts.Tables[0].Rows[0]["Intervalo"].ToString();
                    ViewState["Horario"] = _dts.Tables[0].Rows[0]["Horario"].ToString();
                }

                if (_dts.Tables[1].Rows.Count > 0) Panel1.Visible = true;

                ViewState["DatosHoras"] = _dts.Tables[1];
                GrdvDatos.DataSource = _dts.Tables[1];
                GrdvDatos.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunLlenarCombos(int opcion)
        {
            try
            {
                switch (opcion)
                {
                    case 0:
                        DdlIntervalo.DataSource = new ControllerDAO().FunGetParametroDetalle("INTERVALO",
                            "--Seleccione Intervalor--","S");
                        DdlIntervalo.DataTextField = "Descripcion";
                        DdlIntervalo.DataValueField = "Codigo";
                        DdlIntervalo.DataBind();

                        new FuncionesDAO().FunCargarComboHoraMinutos(DdlHoraIni, "HORAS");
                        new FuncionesDAO().FunCargarComboHoraMinutos(DdlMinutoIni, "MINUTOS");
                        new FuncionesDAO().FunCargarComboHoraMinutos(DdlHoraFin, "HORAS");
                        new FuncionesDAO().FunCargarComboHoraMinutos(DdlMinutoFin, "MINUTOS");
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunCargarHorasMinutos(string HoraInicial, string HoraFinal, int orden)
        {
            try
            {
                _dtbhoras = (DataTable)ViewState["DatosHoras"];
                _filagre = _dtbhoras.NewRow();
                _filagre["HoraInicio"] = HoraInicial;
                _filagre["HoraFin"] = HoraFinal;
                _filagre["Orden"] = orden;
                _dtbhoras.Rows.Add(_filagre);
                ViewState["DatosHoras"] = _dtbhoras;
                GrdvDatos.DataSource = _dtbhoras;
                GrdvDatos.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DdlIntervalo_SelectedIndexChanged(object sender, EventArgs e)
        {
            _dtbhoras = (DataTable)ViewState["DatosHoras"];
            _dtbhoras.Clear();
            GrdvDatos.DataSource = _dtbhoras;
            GrdvDatos.DataBind();
            Panel1.Visible = false;
        }

        protected void ImgProcesar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlIntervalo.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Intervalo..!", this);
                    return;
                }

                _horainicio = TimeSpan.Parse(DdlHoraIni.SelectedItem.ToString() + ":" + DdlMinutoIni.SelectedItem.ToString());
                _horafin = TimeSpan.Parse(DdlHoraFin.SelectedItem.ToString() + ":" + DdlMinutoFin.SelectedItem.ToString());

                if (_horainicio > _horafin)
                {
                    new FuncionesDAO().FunShowJSMessage("La Hora inicio no puede ser menor a la Hora fin..!", this);
                    return;
                }

                if (_horainicio == _horafin)
                {
                    new FuncionesDAO().FunShowJSMessage("La Hora inicio y la Hora fin son iguales..!", this);
                    return;
                }

                _intervalo = _horafin - _horainicio;
                _inter = (int)_intervalo.TotalMinutes;

                if (int.Parse(DdlIntervalo.SelectedValue) > _inter)
                {
                    new FuncionesDAO().FunShowJSMessage("El intervalo entre las horas es menor al valor del Intervalo..!", this);
                    return;
                }

                Panel1.Visible = true;
                _intervalminu = TimeSpan.FromMinutes(int.Parse(DdlIntervalo.SelectedValue));
                FunCargarHorasMinutos(_horainicio.ToString(@"hh\:mm"), _horainicio.Add(_intervalminu).ToString(@"hh\:mm"), 0);
                _horainicio = _horainicio.Add(_intervalminu);
                _orden = 0;

                while (_horainicio != _horafin)
                {
                    if (_horainicio > _horafin)
                    {
                        _horainicio = _horafin;
                        DdlMinutoFin.SelectedValue = _horafinal.Substring(3, 2);
                    }
                    else
                    {
                        _intervalo = _horafin - _horainicio;
                        _inter = (int)_intervalo.TotalMinutes;

                        if (int.Parse(DdlIntervalo.SelectedValue) <= _inter)
                        {
                            _horainicial = _horainicio.ToString(@"hh\:mm");
                            _horainicio = _horainicio.Add(_intervalminu);
                            _horafinal = _horainicio.ToString(@"hh\:mm");
                            _orden++;
                            FunCargarHorasMinutos(_horainicial, _horafinal, _orden);
                        }
                        else _horainicio = _horainicio.Add(_intervalminu);
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstado.Text = ChkEstado.Checked ? "Activo" : "Inactivo";
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtHorario.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombre del Horario..!", this);
                    return;
                }

                _dtbhoras = (DataTable)ViewState["DatosHoras"];

                if (_dtbhoras.Rows.Count == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Procese horarios..!", this);
                    return;
                }

                if (ViewState["Intervalo"].ToString() != DdlIntervalo.SelectedValue)
                {

                    _dts = new ConsultaDatosDAO().FunHorariobpm(2, int.Parse(ViewState["Codigo"].ToString()), "", "",
                        DdlIntervalo.SelectedValue, "", "", "", 0, "", "", "", 0, 0, 0, int.Parse((Session["usuCodigo"].ToString())), Session["MachineName"].ToString(), Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 1)
                    {
                        new FuncionesDAO().FunShowJSMessage("Ya existe un horario con ese intervalo..!", this);
                        return;
                    }
                }

                if (ViewState["Horario"].ToString() != TxtHorario.Text.Trim().ToUpper())
                {

                    _dts = new ConsultaDatosDAO().FunHorariobpm(1, int.Parse(ViewState["Codigo"].ToString()),
                        TxtHorario.Text.Trim().ToUpper(), "", "", "", "", "", 0, "", "", "", 1, 0, 0,
                        int.Parse((Session["usuCodigo"].ToString())), Session["MachineName"].ToString(),
                        Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 1)
                    {
                        new FuncionesDAO().FunShowJSMessage("Ya existe nombre del horario..!", this);
                        return;
                    }
                }

                _dts = new ConsultaDatosDAO().FunHorariobpm(3, int.Parse(ViewState["Codigo"].ToString()),
                    TxtHorario.Text.Trim().ToUpper(), TxtDescripcion.Text.Trim().ToUpper(), DdlIntervalo.SelectedValue,
                    DdlHoraIni.SelectedItem.ToString() + ":" + DdlMinutoIni.SelectedItem.ToString(),
                    DdlHoraFin.SelectedItem.ToString() + ":" + DdlMinutoFin.SelectedItem.ToString(),
                    ChkEstado.Text, 0, "", "", "", 0, 0, 0, int.Parse((Session["usuCodigo"].ToString())),
                    Session["MachineName"].ToString(), Session["Conectar"].ToString());

                _codigo = int.Parse(_dts.Tables[0].Rows[0]["Codigo"].ToString());

                foreach (DataRow _drfila in _dtbhoras.Rows)
                {
                    _dts = new ConsultaDatosDAO().FunHorariobpm(4, _codigo, "", "", "", _drfila["HoraInicio"].ToString(), _drfila["HoraFin"].ToString(), "", int.Parse(_drfila["Orden"].ToString()), "", "", "", 0, 0, 0, int.Parse((Session["usuCodigo"].ToString())), Session["MachineName"].ToString(), Session["Conectar"].ToString());
                }

                Response.Redirect("WFrm_HorariosAdmin.aspx?MensajeRetornado=Guardado con Éxito", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_HorariosAdmin.aspx", true);
        }
        #endregion
    }
}