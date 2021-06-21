namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ListaClientesAdmin : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        string _identificacion = "", _operacion = "", _codigocede = "", _codigocpce = "", _codigoclde = "", _codigopers = "", _fechapago = "", _volverllamar = "", _listaactiva = "", _mensaje="";
        DateTime _nuevafecha;
        int _anio = 0, _mes = 0;
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
                    new FuncionesDAO().FunShowJSMessage("Se encuentra en Llamada, en cuanto termine la gestión podrá salir de la Lista de Trabajo..!", this);
                    Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                }

                ViewState["MesActual"] = DateTime.Today.Month;
                ViewState["AnioActual"] = DateTime.Today.Year;
                Lbltitulo.Text = "Lista de Clientes";
                FunConsultarAgendamiento();
                FunCargarMantenimiento();

                //if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());
                if (Request["MensajeRetornado"] != null)
                {
                    _mensaje = Request["MensajeRetornado"];
                    ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                        "'top-center'); alertify.success('" + _mensaje + "', 5, function(){console.log('dismissed');});", true);
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(155, int.Parse(Session["usuCodigo"].ToString()), 0, 0, "", "", "", 
                    Session["Conectar"].ToString());
                _listaactiva = _dts.Tables[0].Rows[0][0].ToString();

                if (_listaactiva == "SI")
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(82, int.Parse(Session["usuCodigo"].ToString()), 0, 0,
                        "", "", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        GrdvDatos.DataSource = _dts;
                        GrdvDatos.DataBind();
                        GrdvDatos.UseAccessibleHeader = true;
                        GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }

                    ViewState["grdvDatos"] = GrdvDatos.DataSource;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunConsultarAgendamiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(39, 0, int.Parse(Session["usuCodigo"].ToString()), 0, "", "", "", 
                    Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["FechaLlamar"] = _dts.Tables[0].Rows[0]["FechaLlamar"].ToString();
                    ViewState["HoraLlamar"] = _dts.Tables[0].Rows[0]["HoraLlamar"].ToString();

                    if (int.Parse(_dts.Tables[0].Rows[0]["Codigo"].ToString()) > 0)
                    {
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(32, int.Parse(_dts.Tables[0].Rows[0]["Perscodigo"].ToString()), 
                            0, 0, "", "", "", Session["Conectar"].ToString().ToString());

                        if (_dts.Tables[0].Rows.Count > 0)
                        {
                            Lblerror.Text = String.Format(@"Estimado Gestor, por favor realice la gestión al cliente:  Sr./Sra: {0} Agendado el: {1}", 
                                _dts.Tables[0].Rows[0]["Cliente"].ToString(),
                              ViewState["FechaLlamar"].ToString() + " " + ViewState["HoraLlamar"].ToString());
                        }
                    }
                    else ViewState["CodigoLlamar"] = "0";
                }
                else ViewState["CodigoLlamar"] = "0";
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
                Response.Redirect("WFrm_RegLlamadaEntrante.aspx?CodigoCEDE=" + _codigocede + "&CodigoCPCE=" + _codigocpce + "&CodigoCLDE=" + _codigoclde + "&CodigoPERS=" + _codigopers + "&NumeroDocumento=" + _identificacion + "&Operacion=" + _operacion + "&CodigoLTCA=0&CodigoUSU=0&Retornar=1", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void grdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _fechapago = GrdvDatos.DataKeys[e.Row.RowIndex].Values["FechaPago"].ToString();
                    _volverllamar = GrdvDatos.DataKeys[e.Row.RowIndex].Values["VolverLLamar"].ToString();

                    if (_volverllamar == "SI")
                    {
                        e.Row.Cells[1].BackColor = System.Drawing.Color.BurlyWood;
                    }

                    if (!string.IsNullOrEmpty(_fechapago))
                    {
                        _nuevafecha = DateTime.ParseExact(_fechapago, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        _anio = _nuevafecha.Year;
                        _mes = _nuevafecha.Month;

                        if (_anio == int.Parse(ViewState["AnioActual"].ToString()))
                        {
                            if (_mes == int.Parse(ViewState["MesActual"].ToString()))
                            {
                                e.Row.Cells[7].BackColor = System.Drawing.Color.Coral;
                                e.Row.Cells[8].BackColor = System.Drawing.Color.Coral;
                            }
                            if (_mes > int.Parse(ViewState["MesActual"].ToString()))
                            {
                                e.Row.Cells[7].BackColor = System.Drawing.Color.MediumAquamarine;
                                e.Row.Cells[8].BackColor = System.Drawing.Color.MediumAquamarine;
                            }
                            if (_mes < int.Parse(ViewState["MesActual"].ToString()))
                            {
                                e.Row.Cells[7].Text = "";
                                e.Row.Cells[8].Text = "0.00";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgCitacion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigoclde = GrdvDatos.DataKeys[gvRow.RowIndex].Values["codigoCLDE"].ToString();
                _codigopers = GrdvDatos.DataKeys[gvRow.RowIndex].Values["codigoPERS"].ToString();
                Response.Redirect("../BPM/WFrm_NuevaCitacion.aspx?CodigoPERS=" + _codigopers + "&CodigoCLDE="
                    + _codigoclde + "&Retornar=0", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}