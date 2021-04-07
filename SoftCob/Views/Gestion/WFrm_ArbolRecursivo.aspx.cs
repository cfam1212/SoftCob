﻿namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Threading;
    public partial class WFrm_ArbolRecursivo : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        string _cedula = "", _redirect = "", _estado = "", _respuesta = "";
        ImageButton _imgphone = new ImageButton();
        Thread _thrmarcar;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                    Lbltitulo.Text = "CONSULTA RECURSIVA << ARBOL GENEALOGICO >>";
                    ViewState["Cedula"] = Request["Cedula"];
                    ViewState["PhoneLocalize"] = Request["PhoneLocalize"];
                    LblCedula.InnerText = ViewState["Cedula"].ToString();
                    Pnldatosdeudor.Height = 180;
                    Panel2.Height = 180;
                    LblDireccion.InnerText = "";
                    FunGuardarconsulta(ViewState["Cedula"].ToString());
                    FunCargarDatos(ViewState["Cedula"].ToString());
                    if (ViewState["PhoneLocalize"] != null)
                    {
                        FunPhoneLocalize(ViewState["Cedula"].ToString());
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
        private void FunPhoneLocalize(string numerodocumento)
        {
            _dts = new ConsultaDatosDAO().FunConsultaDatos(203, 0, 0, 0, "", numerodocumento, "", ViewState["Conectar"].ToString());

            if (_dts.Tables[0].Rows.Count > 0)
            {
                LblDireccion.InnerText = _dts.Tables[0].Rows[0]["Direccion"].ToString();
                TblFonos.Visible = true;
                GrdvTelefonos.DataSource = _dts.Tables[1];
                GrdvTelefonos.DataBind();
            }
        }

        private void FunCargarDatos(string numerodocumento)
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(184, 0, 0, 0, "", numerodocumento.Substring(0, 4),
                    ViewState["Cedula"].ToString(), ViewState["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    LblNombres.Text = _dts.Tables[0].Rows[0]["Nombres"].ToString();
                    Grdvdatosper1.DataSource = _dts.Tables[0];
                    Grdvdatosper1.DataBind();

                    Grdvdatosper2.DataSource = _dts.Tables[0];
                    Grdvdatosper2.DataBind();

                    if (_dts.Tables[1].Rows.Count > 0)
                    {
                        Tbldatosiess.Visible = true;
                        Grdvdatosiess1.DataSource = _dts.Tables[1];
                        Grdvdatosiess1.DataBind();

                        Grdvdatosiess2.DataSource = _dts.Tables[1];
                        Grdvdatosiess2.DataBind();
                    }
                    else Tbldatosiess.Visible = false;

                    if (_dts.Tables[2].Rows.Count > 0)
                    {
                        Tblarbol.Visible = true;
                        GrdvDatos.DataSource = _dts.Tables[2];
                        GrdvDatos.DataBind();
                    }
                    else Tblarbol.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunGuardarconsulta(string cedula)
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(196, int.Parse(Session["usuCodigo"].ToString()), 0, 0,
                    "", cedula, "", ViewState["Conectar"].ToString());
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunDial()
        {
            try
            {
                new ElastixDAO().ElastixHangUp(Session["IPLocalAdress"].ToString(), 9999);
                _respuesta = new ElastixDAO().ElastixDial(Session["IPLocalAdress"].ToString(), 9999, ViewState["DialerNumber"].ToString());
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void ImgSelecc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _cedula = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["Cedula"].ToString();
                _redirect = string.Format("{0}?Cedula={1}&PhoneLocalize={2}", Request.Url.AbsolutePath, _cedula, "SI");
                Response.Redirect(_redirect);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgPhone_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                foreach (GridViewRow fr in GrdvTelefonos.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }
                GrdvTelefonos.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                ViewState["DialerNumber"] = GrdvTelefonos.DataKeys[_gvrow.RowIndex].Values["Telefono"].ToString();
                _imgphone = (ImageButton)(_gvrow.Cells[2].FindControl("ImgTelefono"));
                _imgphone.ImageUrl = "~/Botones/call_small_disabled.png";
                _thrmarcar = new Thread(new ThreadStart(FunDial));
                _thrmarcar.Start();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnRegresar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["PhoneLocalize"] = null;
                _redirect = string.Format("{0}?Cedula={1}", Request.Url.AbsolutePath, Session["CedulaCookie"].ToString());
                Response.Redirect(_redirect);
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
                    _estado = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Consultado"].ToString();
                    if (_estado == "SI") e.Row.Cells[0].BackColor = System.Drawing.Color.LightSeaGreen;
                }
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