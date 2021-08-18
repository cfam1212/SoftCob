namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ListaVolveraLlamar : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        string _identificacion = "", _operacion = "", _codigocede = "", _codigocpce = "", _codigoclde = "", _codigopers = "", 
        _fechallamar = "", _horallamar = "", _mensaje = "";
        DateTime _fechaactual, _fechallamada;
        TimeSpan _horaactual, _horallamada;
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
                    //if (Session["IN-CALL"].ToString() == "SI")
                    //{
                    //    new FuncionesDAO().FunShowJSMessage("Se encuentra en Llamada, en cuanto termine la gestión podrá salir de la Lista de Trabajo..!", this);
                    //    Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                    //}

                    Lbltitulo.Text = "Lista de Clientes <<-- VOLVER A LLAMAR -->>";
                    ViewState["FechaActual"] = DateTime.Now.ToString("yyyy-MM-dd");
                    ViewState["HoraActual"] = DateTime.Now.ToString("HH:mm");
                    FunCargarMantenimiento();

                    
                    if (Request["MensajeRetornado"] != null)
                    {
                        _mensaje = Request["MensajeRetornado"];
                        new FuncionesDAO().FunShowJSMessage(_mensaje, this, "S", "R");
                    }
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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(120, int.Parse(Session["usuCodigo"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    GrdvDatos.DataSource = _dts;
                    GrdvDatos.DataBind();
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }

                ViewState["grdvDatos"] = GrdvDatos.DataSource;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnProcesar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigocede = GrdvDatos.DataKeys[gvRow.RowIndex].Values["codigoCEDE"].ToString();
                _codigocpce = GrdvDatos.DataKeys[gvRow.RowIndex].Values["codigoCPCE"].ToString();
                _codigoclde = GrdvDatos.DataKeys[gvRow.RowIndex].Values["codigoCLDE"].ToString();
                _codigopers = GrdvDatos.DataKeys[gvRow.RowIndex].Values["codigoPERS"].ToString();
                _identificacion = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Identificacion"].ToString();
                _operacion = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Operacion"].ToString();
                Response.Redirect("WFrm_RegLlamadaEntrante.aspx?CodigoCEDE=" + _codigocede + "&CodigoCPCE=" + _codigocpce + "&codigoCLDE=" + 
                    _codigoclde + "&CodigoPERS=" + _codigopers + "&NumeroDocumento=" + _identificacion + "&Operacion=" + _operacion + 
                    "&CodigoLTCA=0&CodigoUSU=0&Retornar=2", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _fechallamar = GrdvDatos.DataKeys[e.Row.RowIndex].Values["FechaLlamar"].ToString();
                    _horallamar = GrdvDatos.DataKeys[e.Row.RowIndex].Values["HoraLlamar"].ToString();

                    _fechallamada = DateTime.ParseExact(_fechallamar, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    _horallamada = TimeSpan.Parse(_horallamar);

                    _fechaactual = DateTime.ParseExact(ViewState["FechaActual"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    _horaactual = TimeSpan.Parse(ViewState["HoraActual"].ToString());

                    if (_fechallamada == _fechaactual)
                    {
                        e.Row.Cells[7].BackColor = System.Drawing.Color.Coral;

                        if (_horallamada == _horaactual) e.Row.Cells[8].BackColor = System.Drawing.Color.Aquamarine;

                        if (_horallamada < _horaactual) e.Row.Cells[8].BackColor = System.Drawing.Color.Beige;
                    }

                    if (_fechallamada < _fechaactual) e.Row.Cells[7].BackColor = System.Drawing.Color.Cyan;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}