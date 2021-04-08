﻿namespace SoftCob.Views.ReportesManager
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web.UI.WebControls;
    using System.Web.UI;
    public partial class WFrm_ReportePagosCartera : Page
    {
        #region Variables
        ListItem _itemc = new ListItem();
        DataSet _dts = new DataSet();
        ListItem _accion = new ListItem();
        ListItem _efecto = new ListItem();
        ListItem _respuesta = new ListItem();
        ListItem _contacto = new ListItem();
        string _sql = "";
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
                    Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                    return;
                }

                ViewState["CodigoCEDE"] = Request["CodigoCEDE"];
                ViewState["CodigoCPCE"] = Request["CodigoCPCE"];
                ViewState["FechaDesde"] = Request["FechaDesde"];
                ViewState["FechaHasta"] = Request["FechaHasta"];
                ViewState["Accion"] = Request["Accion"];
                ViewState["Efecto"] = Request["Efecto"];
                ViewState["Respuesta"] = Request["Respuesta"];
                ViewState["Contacto"] = Request["Contacto"];
                ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                TxtFechaIni.Text = DateTime.Now.ToString("MM/dd/yyyy");
                TxtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
                Lbltitulo.Text = "Reportes Pagos de Cartera";
                FunCargarCombos(0);

                if (ViewState["CodigoCEDE"] != null) FunCargarMantenimiento();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    DdlCedente.DataSource = new CedenteDAO().FunGetCedentes();
                    DdlCedente.DataTextField = "Descripcion";
                    DdlCedente.DataValueField = "Codigo";
                    DdlCedente.DataBind();

                    _itemc.Text = "--Seleccione Catálago/Producto--";
                    _itemc.Value = "0";
                    DdlCatalogo.Items.Add(_itemc);

                    DdlAccion.Items.Clear();
                    _accion.Text = "--Seleccione Acción--";
                    _accion.Value = "0";
                    DdlAccion.Items.Add(_accion);

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
                    DdlAccion.DataSource = new SpeechDAO().FunGetArbolAccion(int.Parse(DdlCedente.SelectedValue));
                    DdlAccion.DataTextField = "Descripcion";
                    DdlAccion.DataValueField = "Codigo";
                    DdlAccion.DataBind();
                    break;
                case 2:
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
                case 3:
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
                case 4:
                    DdlContacto.Items.Clear();
                    _contacto.Text = "--Seleccione Contacto--";
                    _contacto.Value = "0";
                    DdlContacto.Items.Add(_contacto);
                    DdlContacto.DataSource = new SpeechDAO().FunGetArbolContacto(int.Parse(DdlRespuesta.SelectedValue));
                    DdlContacto.DataTextField = "Descripcion";
                    DdlContacto.DataValueField = "Codigo";
                    DdlContacto.DataBind();
                    break;
                case 5:
                    DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataTextField = "CatalogoProducto";
                    DdlCatalogo.DataValueField = "CodigoCatalogo";
                    DdlCatalogo.DataBind();
                    break;
            }
        }

        protected void FunCargarMantenimiento()
        {
            try
            {
                DdlCedente.SelectedValue = ViewState["CodigoCEDE"].ToString();
                FunCargarCombos(1);
                FunCargarCombos(5);
                DdlAccion.SelectedValue = ViewState["Accion"].ToString();
                FunCargarCombos(2);
                DdlEfecto.SelectedValue = ViewState["Efecto"].ToString();
                FunCargarCombos(3);
                DdlRespuesta.SelectedValue = ViewState["Respuesta"].ToString();
                FunCargarCombos(4);
                DdlContacto.SelectedValue = ViewState["Contacto"].ToString();
                DdlCatalogo.SelectedValue = ViewState["CodigoCPCE"].ToString();
                TxtFechaIni.Text = ViewState["FechaDesde"].ToString();
                TxtFechaFin.Text = ViewState["FechaHasta"].ToString();
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
            try
            {
                FunCargarCombos(1);
                FunCargarCombos(5);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlAccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(2);
        }

        protected void DdlEfecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(3);
        }

        protected void DdlRespuesta_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(4);
        }

        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this);
                    return;
                }

                _sql = "";
                _sql = "Select Contar = COUNT(1) ";
                _sql += "from GSBPO_REGISTRO_ABONOSPAGO AP (nolock) INNER JOIN GSBPO_CLIENTE_DEUDOR CL (nolock) ON AP.rpab_cldecodigo=CL.CLDE_CODIGO ";
                _sql += "INNER JOIN GSBPO_CUENTA_DEUDOR CD (nolock) ON CL.CLDE_CODIGO=CD.CLDE_CODIGO INNER JOIN GSBPO_PERSONA PE (nolock) ON CL.PERS_CODIGO=PE.PERS_CODIGO ";
                _sql += "where CL.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " and AP.rpab_fechapago between CONVERT(date,'" + TxtFechaIni.Text + "',101) and CONVERT(date,'";
                _sql += TxtFechaFin.Text + "',101) and ";

                if (DdlAccion.SelectedValue != "0") _sql += "AP.rpab_araccodigo=" + DdlAccion.SelectedValue + " and ";

                if (DdlEfecto.SelectedValue != "0") _sql += "AP.rpab_arefcodigo=" + DdlEfecto.SelectedValue + " and ";

                if (DdlRespuesta.SelectedValue != "0") _sql += "AP.rpab_arrecodigo=" + DdlRespuesta.SelectedValue + " and ";

                if (DdlContacto.SelectedValue != "0") _sql += "AP.rpab_arcocodigo=" + DdlContacto.SelectedValue + " and ";

                _sql = _sql.Remove(_sql.Length - 4);
                _dts = new ConsultaDatosDAO().FunGetRerporteGestiones(1, int.Parse(DdlCedente.SelectedValue), int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text, TxtFechaFin.Text, "", "", _sql, "", 0, 0, ViewState["Conectar"].ToString());

                if (int.Parse(_dts.Tables[0].Rows[0]["Contar"].ToString()) > 0)
                {
                    Response.Redirect("WFrm_ReportePagosCarteraFixed.aspx?CodigoCEDE=" + DdlCedente.SelectedValue +
                        "&Catalogo=" + DdlCatalogo.SelectedItem.ToString() + "&CodigoCPCE=" + DdlCatalogo.SelectedValue +
                        "&FechaDesde=" + TxtFechaIni.Text.Trim() + "&FechaHasta=" + TxtFechaFin.Text.Trim() +
                        "&Accion=" + DdlAccion.SelectedValue + "&Efecto=" + DdlEfecto.SelectedValue +
                        "&Respuesta=" + DdlRespuesta.SelectedValue + "&Contacto=" + DdlContacto.SelectedValue, true);
                    //ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "View",
                    //    "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); " +
                    //    "posicion_y=(screen.height/2)-(600/2); window.open('WFrm_ReportePagosCarteraFixed.aspx?CodigoCEDE=" +
                    //    ddlCedente.SelectedValue + "&Catalogo=" + ddlCatalogo.SelectedItem.ToString() + "&CodigoCPCE=" + ddlCatalogo.SelectedValue +
                    //    "&FechaDesde=" + txtFechaIni.Text.Trim() + "&FechaHasta=" + txtFechaFin.Text.Trim() +
                    //    "&Sql=" + sql +
                    //    "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=1024px, height=600px, " +
                    //    "status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');",
                    //    true);
                }
                else new FuncionesDAO().FunShowJSMessage("No Existen Datos para Mostrar..!", this);
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