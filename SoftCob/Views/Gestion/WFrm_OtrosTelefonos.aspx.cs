namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Threading;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_OtrosTelefonos : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        ImageButton _imgphone = new ImageButton();
        Thread _thrmarcar;
        string _respuesta = "", _mensaje = "";
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
                    Lbltitulo.Text = "Telefonos Encontrados";
                    ViewState["Cedula"] = Request["Cedula"];

                    PnlDatos.Height = 120;
                    PnlTelefonos.Height = 150;

                    FunCargarDatos();

                    
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
        private void FunCargarDatos()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(203, 0, 0, 0, "", ViewState["Cedula"].ToString(), "", Session["Conectar"].ToString());
                GrdvDatos.DataSource = _dts.Tables[0];
                GrdvDatos.DataBind();

                GrdvTelefonos.DataSource = _dts.Tables[1];
                GrdvTelefonos.DataBind();
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
        protected void ImgTelefono_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            foreach (GridViewRow fr in GrdvTelefonos.Rows)
            {
                fr.Cells[0].BackColor = System.Drawing.Color.White;
            }
            GrdvTelefonos.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
            ViewState["DialerNumber"] = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Telefono"].ToString();
            _imgphone = (ImageButton)(gvRow.Cells[2].FindControl("ImgTelefono"));
            _imgphone.ImageUrl = "~/Botones/call_small_disabled.png";
            _thrmarcar = new Thread(new ThreadStart(FunDial));
            _thrmarcar.Start();
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.close();", true);
        }
        #endregion
    }
}