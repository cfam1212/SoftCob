namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Threading;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_BuscarTelefonos : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        ImageButton _imgmarcar = new ImageButton();
        Thread _thrmarcar;
        string _respuesta;
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
                    Lbltitulo.Text = "Buscar Telefonos";

                    if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", 
                        Request["MensajeRetornado"].ToString());
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        public void FunDial()
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
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (DdlBuscarPor.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Opcion de Busqueda..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtCriterio.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Crierio para buscar..!", this);
                    return;
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(181, int.Parse(DdlBuscarPor.SelectedValue), 0, 0, "",
                    TxtCriterio.Text.Trim(), "", Session["Conectar"].ToString());
                GrdvCelular.DataSource = _dts.Tables[0];
                GrdvCelular.DataBind();

                if (_dts.Tables[0].Rows.Count > 0) PnlTelefonoClaro.Visible = true;
                else PnlTelefonoClaro.Visible = false;

                GrdvTelefonos.DataSource = _dts.Tables[1];
                GrdvTelefonos.DataBind();

                if (_dts.Tables[1].Rows.Count > 0) PnlTelefonoCNT.Visible = true;
                else PnlTelefonoCNT.Visible = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgCelular_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                foreach (GridViewRow fr in GrdvCelular.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }
                GrdvCelular.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                ViewState["DialerNumber"] = GrdvCelular.DataKeys[_gvrow.RowIndex].Values["Telefono"].ToString();
                _imgmarcar = (ImageButton)(_gvrow.Cells[5].FindControl("ImgCelular"));
                _imgmarcar.ImageUrl = "~/Botones/call_small_disabled.png";
                _thrmarcar = new Thread(new ThreadStart(FunDial));
                _thrmarcar.Start();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgTelefono_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                foreach (GridViewRow fr in GrdvCelular.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }
                GrdvTelefonos.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                ViewState["DialerNumber"] = GrdvTelefonos.DataKeys[_gvrow.RowIndex].Values["Telefono"].ToString();
                _imgmarcar = (ImageButton)(_gvrow.Cells[6].FindControl("ImgTelefono"));
                _imgmarcar.ImageUrl = "~/Botones/call_small_disabled.png";
                _thrmarcar = new Thread(new ThreadStart(FunDial));
                _thrmarcar.Start();
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