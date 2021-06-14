namespace SoftCob.Views.Cedente
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_UpdateGarante : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtbcorreogarante = new DataTable();
        DataTable _dtbdirgarante = new DataTable();
        DataTable _tblbuscar = new DataTable();
        DataRow _result;
        string _codigo = ""; 
        bool _lexiste = false;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                ViewState["CedulaTitular"] = Request["CedulaTitular"];
                ViewState["CedulaGarante"] = Request["CedulaGarante"];
                ViewState["CodigoGARA"] = Request["CodigoGARA"];
                Lbltitulo.Text = "Actualizar Datos << GARANTE/CODEUDOR >>";
                FunCargarCombos(0);
                FunCargarMantenimiento();

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarMantenimiento()
        {
            _dts = new ConsultaDatosDAO().FunConsultaDatos(209, int.Parse(ViewState["CodigoGARA"].ToString()), 0, 0, "",
                ViewState["CedulaTitular"].ToString(), ViewState["CedulaGarante"].ToString(), Session["Conectar"].ToString());

            TxtNumeroDocumento.Text = _dts.Tables[0].Rows[0]["Cedula"].ToString();
            TxtNombres.Text = _dts.Tables[0].Rows[0]["Nombres"].ToString();
            TxtApellidos.Text = _dts.Tables[0].Rows[0]["Apellidos"].ToString();
            TxtOperacion.Text = _dts.Tables[0].Rows[0]["Operacion"].ToString();
            DdlTipo.SelectedValue = _dts.Tables[0].Rows[0]["CodigoTipo"].ToString();

            _dts = new ConsultaDatosDAO().FunConsultaDatos(208, 3, 0, 0, "", ViewState["CedulaGarante"].ToString(), "",
                Session["Conectar"].ToString());

            ViewState["DireccionGarante"] = _dts.Tables[0];
            GrdvDirecGarante.DataSource = _dts;
            GrdvDirecGarante.DataBind();

            _dts = new ConsultaDatosDAO().FunConsultaDatos(208, 4, 0, 0, "", ViewState["CedulaGarante"].ToString(), "",
                Session["Conectar"].ToString());

            ViewState["CorreoGarante"] = _dts.Tables[0];
            GrdvMailGarante.DataSource = _dts;
            GrdvMailGarante.DataBind();
        }

        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    DdlTipo.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO REFERENCIA", "--Seleccione Tipo--", "S");
                    DdlTipo.DataTextField = "Descripcion";
                    DdlTipo.DataValueField = "Codigo";
                    DdlTipo.DataBind();

                    DdlDirGarante.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO DIRECCION",
                        "--Seleccione Tipo--", "S");
                    DdlDirGarante.DataTextField = "Descripcion";
                    DdlDirGarante.DataValueField = "Codigo";
                    DdlDirGarante.DataBind();

                    DdlMailGarante.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO CORREO",
                        "--Seleccione Tipo--", "S");
                    DdlMailGarante.DataTextField = "Descripcion";
                    DdlMailGarante.DataValueField = "Codigo";
                    DdlMailGarante.DataBind();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void ImgAddDirGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlDirGarante.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Direccion..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtDirGarante.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Direccion..!", this);
                    return;
                }

                if (ViewState["DireccionGarante"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["DireccionGarante"];

                    _result = _tblbuscar.Select("Direccion='" + TxtDirGarante.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Direccion ya Existe..!", this);
                    return;
                }

                _dts = new ConsultaDatosDAO().FunNewDireccionEmail(0, 0, TxtNumeroDocumento.Text.Trim(),
                    "DIRECCION", DdlTipo.SelectedValue, DdlDirGarante.SelectedValue, TxtDirGarante.Text.Trim().ToUpper(),
                    TxtRefGarante.Text.Trim().ToUpper(), "", "", "", "", "", 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                    Session["MachineName"].ToString(), Session["Conectar"].ToString());

                ViewState["DireccionGarante"] = _dts.Tables[0];
                GrdvDirecGarante.DataSource = _dts;
                GrdvDirecGarante.DataBind();

                DdlDirGarante.SelectedValue = "0";
                TxtDirGarante.Text = "";
                TxtRefGarante.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModDirGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlDirGarante.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Direccion..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtDirGarante.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Direccion..!", this);
                    return;
                }

                _dtbdirgarante = (DataTable)ViewState["DireccionGarante"];

                if (ViewState["DirecGarante"].ToString() != TxtDirGarante.Text.Trim())
                {
                    _result = _dtbdirgarante.Select("Direccion='" + TxtDirGarante.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;

                    if (_lexiste)
                    {
                        new FuncionesDAO().FunShowJSMessage("Direccion ya Existe..!", this);
                        return;
                    }
                }

                _dts = new ConsultaDatosDAO().FunNewDireccionEmail(1, int.Parse(ViewState["CodigoDIGT"].ToString()), "",
                    "DIRECCION", DdlTipo.SelectedValue, DdlDirGarante.SelectedValue, TxtDirGarante.Text.Trim().ToUpper(),
                    TxtRefGarante.Text.Trim().ToUpper(), "", "", "", "", "", 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                    Session["MachineName"].ToString(), Session["Conectar"].ToString());

                _dts = new ConsultaDatosDAO().FunNewDireccionEmail(5, int.Parse(ViewState["CodigoDIGT"].ToString()), "",
                    "", "", "", "", "", "", "TIPO DIRECCION", "", "", "", 0, 0, 0, 0, 0, "", Session["Conectar"].ToString());

                ViewState["DireccionGarante"] = _dts.Tables[0];
                GrdvDirecGarante.DataSource = _dts;
                GrdvDirecGarante.DataBind();

                ImgAddDirGarante.Enabled = true;
                ImgModDirGarante.Enabled = false;
                DdlDirGarante.SelectedValue = "0";
                TxtDirGarante.Text = "";
                TxtRefGarante.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSelecDirGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvDirecGarante.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvDirecGarante.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _codigo = GrdvDirecGarante.DataKeys[_gvrow.RowIndex].Values["CodigoDIGT"].ToString();
                _dtbdirgarante = (DataTable)ViewState["DireccionGarante"];
                _result = _dtbdirgarante.Select("CodigoDIGT='" + _codigo + "'").FirstOrDefault();
                ViewState["CodigoDIGT"] = _codigo;
                ViewState["DirecGarante"] = _result["Direccion"].ToString();
                DdlDirGarante.SelectedValue = _result["Definicion"].ToString();
                TxtDirGarante.Text = _result["Direccion"].ToString();
                TxtRefGarante.Text = _result["Referencia"].ToString();

                ImgAddDirGarante.Enabled = false;
                ImgModDirGarante.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliDirGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvDirecGarante.DataKeys[_gvrow.RowIndex].Values["CodigoDIGT"].ToString();


                _dts = new ConsultaDatosDAO().FunNewDireccionEmail(2, int.Parse(_codigo), "",
                    "", "", "", "", "", "", "", "", "", "", 0, 0, 0, 0, 0, "", Session["Conectar"].ToString());

                _dts = new ConsultaDatosDAO().FunNewDireccionEmail(5, int.Parse(_codigo), "",
                    "", "", "", "", "", "TIPO DIRECCION", "", "", "", "", 0, 0, 0, 0, 0, "", Session["Conectar"].ToString());

                ViewState["DireccionGarante"] = _dts.Tables[0];
                GrdvDirecGarante.DataSource = _dts;
                GrdvDirecGarante.DataBind();

                DdlDirGarante.SelectedValue = "0";
                TxtDirGarante.Text = "";
                TxtRefGarante.Text = "";
                ImgAddDirGarante.Enabled = true;
                ImgModDirGarante.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddMailGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlMailGarante.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Email..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtMailGarante.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese E-Mail..!", this);
                    return;
                }

                if (!new FuncionesDAO().Email_bien_escrito(TxtMailGarante.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Email Incorrecto Garante..!", this);
                    return;
                }

                if (ViewState["CorreoGarante"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["CorreoGarante"];

                    _result = _tblbuscar.Select("Email='" + TxtMailGarante.Text.Trim().ToLower() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Correo ya Existe..!", this);
                    return;
                }

                _dts = new ConsultaDatosDAO().FunNewDireccionEmail(3, 0, ViewState["CedulaGarante"].ToString(),
                    "CORREO", DdlTipo.SelectedValue, DdlMailGarante.SelectedValue, "", "",
                    TxtMailGarante.Text.Trim().ToLower(), "", "", "", "", 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                    Session["MachineName"].ToString(), Session["Conectar"].ToString());

                ViewState["CorreoGarante"] = _dts.Tables[0];
                GrdvMailGarante.DataSource = _dts;
                GrdvMailGarante.DataBind();

                DdlMailGarante.SelectedValue = "0";
                TxtMailGarante.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModMailGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlMailGarante.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Email..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtMailGarante.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese E-Mail..!", this);
                    return;
                }

                if (!new FuncionesDAO().Email_bien_escrito(TxtMailGarante.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Email Incorrecto Titular..!", this);
                    return;
                }

                _dtbcorreogarante = (DataTable)ViewState["CorreoGarante"];

                if (ViewState["EMailGarante"].ToString() != TxtMailGarante.Text.Trim())
                {
                    _result = _dtbcorreogarante.Select("Email='" + TxtMailGarante.Text.Trim().ToLower() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;

                    if (_lexiste)
                    {
                        new FuncionesDAO().FunShowJSMessage("E-Mail ya Existe..!", this);
                        return;
                    }
                }

                _dts = new ConsultaDatosDAO().FunNewDireccionEmail(4, int.Parse(ViewState["CodigoDIGT"].ToString()), "",
                    "CORREO", DdlTipo.SelectedValue, DdlMailGarante.SelectedValue, "", "", TxtMailGarante.Text.Trim().ToLower(),
                    "", "", "", "", 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                    Session["Conectar"].ToString());

                _dts = new ConsultaDatosDAO().FunNewDireccionEmail(5, int.Parse(ViewState["CodigoDIGT"].ToString()), "",
                    "", "", "", "", "", "TIPO CORREO", "", "", "", "", 0, 0, 0, 0, 0, "", Session["Conectar"].ToString());

                ViewState["CorreoGarante"] = _dts.Tables[0];
                GrdvMailGarante.DataSource = _dts;
                GrdvMailGarante.DataBind();

                DdlMailGarante.SelectedValue = "0";
                TxtMailGarante.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSelecMailGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvMailGarante.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvMailGarante.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _codigo = GrdvMailGarante.DataKeys[_gvrow.RowIndex].Values["CodigoDIGT"].ToString();
                _dtbcorreogarante = (DataTable)ViewState["CorreoGarante"];
                _result = _dtbcorreogarante.Select("CodigoDIGT='" + _codigo + "'").FirstOrDefault();
                ViewState["CodigoDIGT"] = _codigo;
                ViewState["EMailGarante"] = _result["Email"].ToString();
                DdlMailGarante.SelectedValue = _result["Definicion"].ToString();
                TxtMailGarante.Text = _result["Email"].ToString();

                ImgAddMailGarante.Enabled = false;
                ImgModMailGarante.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliMailGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvMailGarante.DataKeys[_gvrow.RowIndex].Values["CodigoDIGT"].ToString();

                _dts = new ConsultaDatosDAO().FunNewDireccionEmail(2, int.Parse(_codigo), "",
                    "", "", "", "", "", "", "", "", "", "", 0, 0, 0, 0, 0, "", Session["Conectar"].ToString());

                _dts = new ConsultaDatosDAO().FunNewDireccionEmail(5, int.Parse(_codigo), "", "", "", "", "", "", "TIPO CORREO",
                    "", "", "", "", 0, 0, 0, 0, 0, "", Session["Conectar"].ToString());

                ViewState["CorreoGarante"] = _dts.Tables[0];
                GrdvMailGarante.DataSource = _dts;
                GrdvMailGarante.DataBind();

                DdlMailGarante.SelectedValue = "0";
                TxtMailGarante.Text = "";
                ImgAddMailGarante.Enabled = true;
                ImgModMailGarante.Enabled = false;
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
                if (string.IsNullOrEmpty(TxtNumeroDocumento.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese No. Documento..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtNombres.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombres..!", this);
                    return;
                }

                if (ViewState["CedulaGarante"].ToString() != TxtNumeroDocumento.Text.Trim())
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(107, 0, 0, 0, "", TxtNumeroDocumento.Text.Trim(),
                        "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("No. de Documento ya Existe..!", this);
                        TxtNumeroDocumento.Text = ViewState["CedulaGarante"].ToString();
                        return;
                    }
                }

                _dts = new ConsultaDatosDAO().FunActualizarDatos(0, int.Parse(ViewState["CodigoGARA"].ToString()),
                    TxtNumeroDocumento.Text.Trim(), DdlTipo.SelectedValue, TxtNombres.Text.Trim().ToUpper(), TxtApellidos.Text.Trim().ToUpper(),
                    TxtOperacion.Text, ViewState["CedulaGarante"].ToString(), "", "", 0, 0, 0, Session["Conectar"].ToString());

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