
namespace SoftCob.Views.Menu
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_MenuAdmin : Page
    {
        #region Variables
        ImageButton _imgsubir = new ImageButton();
        ImageButton _imgbajar = new ImageButton();
        DataSet _dts = new DataSet();
        string _codigo = "";
        int _codigomenu = 0, _lastrow = 0;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                Lbltitulo.Text = "Administrar Menú";
                FunCargaMantenimiento();

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargaMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatosNew(1, int.Parse(Session["CodigoEMPR"].ToString()),
                    "", "", "", "", "", "", 0, 0, 0, 0, 0, 0, Session["Conectar"].ToString());
                GrdvDatos.DataSource = _dts;
                GrdvDatos.DataBind();
                GrdvDatos.UseAccessibleHeader = true;
                GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;

                _lastrow = GrdvDatos.Rows.Count - 1;
                _imgsubir = (ImageButton)GrdvDatos.Rows[0].Cells[2].FindControl("ImgSubirNivel");
                _imgsubir.ImageUrl = "~/Botones/desactivada_up.png";
                _imgsubir.Enabled = false;

                _imgbajar = (ImageButton)GrdvDatos.Rows[_lastrow].Cells[2].FindControl("ImgBajarNivel");
                _imgbajar.ImageUrl = "~/Botones/desactivada_down.png";
                _imgbajar.Enabled = false;

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_MenuNuevo.aspx", true);
        }

        protected void Btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
            Response.Redirect("WFrm_MenuEditar.aspx?Codigo=" + _codigo);
        }

        protected void ImgSubirNivel_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigomenu = int.Parse(GrdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
            _dts = new ConsultaDatosDAO().FunConsultaDatosNew(2, int.Parse(Session["CodigoEMPR"].ToString()),
                "", "", "", "", "", "", _codigomenu, 0, 0, 0, 0, 0, Session["Conectar"].ToString());
            FunCargaMantenimiento();
        }

        protected void ImgBajarNivel_Click(object sender, ImageClickEventArgs e)
        {            
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigomenu = int.Parse(GrdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
            _dts = new ConsultaDatosDAO().FunConsultaDatosNew(3, int.Parse(Session["CodigoEMPR"].ToString()),
                "", "", "", "", "", "", _codigomenu, 0, 0, 0, 0, 0, Session["Conectar"].ToString());
            FunCargaMantenimiento();
        }
        #endregion
    }
}