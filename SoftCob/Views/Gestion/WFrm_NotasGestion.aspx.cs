namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;
    public partial class WFrm_NotasGestion : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DateTime _dtmfecharecordatorio, _dtmfechaactual;
        string _fechaactual = "";
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
                    Lbltitulo.Text = "Ingreso << Notas -- Observaciones >>";
                    ViewState["codigoCEDE"] = Request["codigoCEDE"];
                    ViewState["codigoCPCE"] = Request["codigoCPCE"];
                    ViewState["codigoPERS"] = Request["codigoPERS"];
                    ViewState["codigoNOTA"] = "0";
                    TxtFechaRecordatorio.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    FunCargarCombos(0);
                    FunCargarMantenimiento();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                _dts = new GestionTelefonicaDAO().FunGetNotasGestion(int.Parse(ViewState["codigoCEDE"].ToString()),
                    int.Parse(ViewState["codigoCPCE"].ToString()), int.Parse(ViewState["codigoPERS"].ToString()),
                    int.Parse(Session["usuCodigo"].ToString()));

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["codigoNOTA"] = int.Parse(_dts.Tables[0].Rows[0]["Codigo"].ToString());
                    TxtFechaRecordatorio.Text = _dts.Tables[0].Rows[0]["Fecha"].ToString();
                    DdlMantener.SelectedValue = _dts.Tables[0].Rows[0]["Mantener"].ToString();
                    TxtDescripcion.Text = _dts.Tables[0].Rows[0]["Descripcion"].ToString();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    _dts = new ControllerDAO().FunGetParametroDetalle("TIPO RECORDATORIO", "--Seleccione Tipo--", "S");
                    DdlMantener.DataSource = _dts;
                    DdlMantener.DataTextField = "Descripcion";
                    DdlMantener.DataValueField = "Codigo";
                    DdlMantener.DataBind();
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                _fechaactual = DateTime.Now.ToString("MM/dd/yyyy");
                _dtmfecharecordatorio = DateTime.ParseExact(TxtFechaRecordatorio.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                _dtmfechaactual = DateTime.ParseExact(_fechaactual, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                if (_dtmfecharecordatorio < _dtmfechaactual)
                {
                    new FuncionesDAO().FunShowJSMessage("Fecha de Recordatorio no puede ser menor a Fecha Actual..!", this, "E", "C");
                    return;
                }

                if (DdlMantener.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo de Recordatorio..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtDescripcion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Descripción..!", this, "W", "C");
                    return;
                }

                SoftCob_NOTAS_GESTION datos = new SoftCob_NOTAS_GESTION();
                {
                    datos.NOTA_CODIGO = int.Parse(ViewState["codigoNOTA"].ToString());
                    datos.nota_cedecodigo = int.Parse(ViewState["codigoCEDE"].ToString());
                    datos.nota_cpcecodigo = int.Parse(ViewState["codigoCPCE"].ToString());
                    datos.nota_perscodigo = int.Parse(ViewState["codigoPERS"].ToString());
                    datos.nota_gestor = int.Parse(Session["usuCodigo"].ToString());
                    datos.nota_descripcion = TxtDescripcion.Text.Trim().ToUpper();
                    datos.nota_revisarfecha = DateTime.ParseExact(TxtFechaRecordatorio.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    datos.nota_mantener = DdlMantener.SelectedValue;
                    datos.nota_auxv1 = "";
                    datos.nota_auxv2 = "";
                    datos.nota_auxv3 = "";
                    datos.nota_auxi1 = 0;
                    datos.nota_auxi2 = 0;
                    datos.nota_auxi3 = 0;
                    datos.nota_fechacreacion = DateTime.Now;
                    datos.nota_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                    datos.nota_terminalcreacion = Session["MachineName"].ToString();
                }

                if (int.Parse(ViewState["codigoNOTA"].ToString()) == 0) new GestionTelefonicaDAO().FunCrearNotasGestion(datos);
                else new GestionTelefonicaDAO().FunEditarNotasGestion(datos);

                ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.close();", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.close();", true);
        }
        #endregion
    }
}