namespace SoftCob.Views.Evaluacion
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_NewEvaluacion : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        CheckBox _chkselecc = new CheckBox();
        string _estado = "", _codigo = "", _fechaactual = "";
        DateTime _dtmfechainicio, _dtmfechafin, _dtmfechaactual;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TxtFechaInicio.Text = DateTime.Now.ToString("MM/dd/yyyy");
                TxtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
                FunCargarCombos(0);
                ViewState["CodigoEVCA"] = Request["CodigoEVCA"];

                if (ViewState["CodigoEVCA"].ToString() == "0") Lbltitulo.Text = "Administrar Nueva Evaluación Desempeño";
                else
                {
                    Lbltitulo.Text = "Administrar Evaluación Desempeño";
                    LblEstado.Visible = true;
                    ChkEstado.Visible = true;
                    TxtFechaInicio.Enabled = false;
                    FunCargarMantenimiento();
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(159, int.Parse(ViewState["CodigoEVCA"].ToString()), 0, 0,
                    "", "", "", Session["Conectar"].ToString());
                TxtEvaluacion.Text = _dts.Tables[0].Rows[0]["Evaluacion"].ToString();
                TxtDescripcion.Text = _dts.Tables[0].Rows[0]["Descripcion"].ToString();
                TxtFechaInicio.Text = _dts.Tables[0].Rows[0]["FechaInicio"].ToString();
                TxtFechaFin.Text = _dts.Tables[0].Rows[0]["FechaFin"].ToString();
                ChkEstado.Text = _dts.Tables[0].Rows[0]["Estado"].ToString();
                ChkEstado.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunCargarCombos(int opcion)
        {
            try
            {
                switch (opcion)
                {
                    case 0:
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(158, 0, 0, 0, "", "", "", Session["Conectar"].ToString());
                        GrdvDepartamentos.DataSource = _dts;
                        GrdvDepartamentos.DataBind();
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
        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstado.Text = ChkEstado.Checked ? "Activo" : "Inactivo";
        }

        protected void ChkEvaluar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkselecc = (CheckBox)(_gvrow.Cells[1].FindControl("ChkEvaluar"));
                _codigo = GrdvDepartamentos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString();

                if (_chkselecc.Checked) _gvrow.Cells[0].BackColor = System.Drawing.Color.LightSeaGreen;
                else _gvrow.Cells[0].BackColor = System.Drawing.Color.Beige;

                _dts = new ConsultaDatosDAO().FunConsultaDatos(160, int.Parse(_codigo), _chkselecc.Checked ? 1 : 0, 0, "", ""
                    , "", Session["Conectar"].ToString());
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvDepartamentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkselecc = (CheckBox)(e.Row.Cells[1].FindControl("ChkEvaluar"));
                    _estado = GrdvDepartamentos.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();

                    if (_estado == "Activo")
                    {
                        e.Row.Cells[0].BackColor = System.Drawing.Color.LightSeaGreen;
                        _chkselecc.Checked = true;
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
                if (string.IsNullOrEmpty(TxtEvaluacion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombre de la Evaluación..!", this);
                    return;
                }

                if (ViewState["CodigoEVCA"].ToString() == "0")
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(162, 0, 0, 0, "",
                        TxtEvaluacion.Text.Trim().ToUpper(), TxtFechaInicio.Text.Trim(), Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Nombre de la Evaluacion ya Existe..!", this);
                        return;
                    }
                }

                if (!new FuncionesDAO().IsDate(TxtFechaInicio.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("Formato Fecha Incorrecta..!", this);
                    return;
                }

                if (!new FuncionesDAO().IsDate(TxtFechaFin.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("Formato Fecha Incorrecta..!", this);
                    return;
                }

                _fechaactual = DateTime.Now.ToString("MM/dd/yyyy");
                _dtmfechainicio = DateTime.ParseExact(TxtFechaInicio.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                _dtmfechafin = DateTime.ParseExact(TxtFechaFin.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                _dtmfechaactual = DateTime.ParseExact(_fechaactual, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                if (_dtmfechafin < _dtmfechainicio)
                {
                    new FuncionesDAO().FunShowJSMessage("Fecha Inicio no puede ser mayor a Fecha Fin..!", this);
                    return;
                }

                if (int.Parse(ViewState["CodigoEVCA"].ToString()) == 0)
                {
                    if (_dtmfechainicio < _dtmfechaactual)
                    {
                        new FuncionesDAO().FunShowJSMessage("Fecha Inicio no puede ser menor a la Fecha Actual..!", this);
                        return;
                    }
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(163, 0, 0, 0, TxtEvaluacion.Text.Trim().ToUpper(), "", "",
                    Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Nombre de Evaluación ya Existe..!", this);
                    return;
                }

                _dts = new ConsultaDatosDAO().FunNewEvaluacion(0, int.Parse(ViewState["CodigoEVCA"].ToString()),
                    TxtEvaluacion.Text.Trim().ToUpper(), TxtDescripcion.Text.Trim().ToUpper(), TxtFechaInicio.Text.Trim(),
                    TxtFechaFin.Text.Trim(), ChkEstado.Text, "", "", "", 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                    Session["MachineName"].ToString(), Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows[0][0].ToString() == "OK") Response.Redirect("WFrm_ListaEvaluacionAdmin.aspx?MensajeRetornado='Grabado con Exito'", true);
                else Response.Redirect("WFrm_ListaEvaluacionAdmin.aspx?MensajeRetornado='No se pudo grabar correctamente'", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_ListaEvaluacionAdmin.aspx", true);
        }
        #endregion
    }
}