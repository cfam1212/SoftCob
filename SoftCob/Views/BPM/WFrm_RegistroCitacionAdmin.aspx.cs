namespace SoftCob.Views.BPM
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_RegistroCitacionAdmin : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        string _codigo = "", _codigoclde = "", _codigopers = "", _estadocodigo = "", _codigogest = "",
            _numdocumento = "", _cliente = "";
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
                    
                    Lbltitulo.Text = "Lista Registro de Notificaciones";
                    FunCargarMantenimiento();

                    if (Request["MensajeRetornado"] != null) new FuncionesDAO().FunShowJSMessage(Request["MensajeRetornado"],
                        this, "S", "C");
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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(199, 0, 0, 0, "", "", "", Session["Conectar"].ToString());
                GrdvDatos.DataSource = _dts;
                GrdvDatos.DataBind();

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void ImgCitacion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                _codigoclde = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCLDE"].ToString();
                _codigopers = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPERS"].ToString();
                _codigogest = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoGEST"].ToString();
                _estadocodigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoESTA"].ToString();
                _numdocumento = GrdvDatos.DataKeys[gvRow.RowIndex].Values["NumDocumento"].ToString();
                _cliente = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Cliente"].ToString();

                switch (_estadocodigo)
                {
                    case "CCS":
                        Response.Redirect("WFrm_RegistrarConvenio.aspx?CodigoCITA=" + _codigo + "&CodigoPERS=" + _codigopers +
                            "&CodigoCLDE=" + _codigoclde + "&CodigoGEST=" + _codigogest + "&NumDocumento=" + _numdocumento, true);
                        break;
                    case "CAS":
                        Response.Redirect("WFrm_RegistroPagos.aspx?CodigoCITA=" + _codigo + "&CodigoPERS=" + _codigopers + "&CodigoCLDE=" +
                           _codigoclde + "&CodigoGEST=" + _codigogest + "&NumDocumento=" + _numdocumento +
                           "&Documento=" + _numdocumento + "&Nombres=" + _cliente, true);
                        break;
                }
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
                    _estadocodigo = GrdvDatos.DataKeys[e.Row.RowIndex].Values["CodigoESTA"].ToString();

                    switch (_estadocodigo)
                    {
                        case "CCS":
                            e.Row.Cells[2].BackColor = System.Drawing.Color.LimeGreen;
                            break;
                        case "CAS":
                            e.Row.Cells[2].BackColor = System.Drawing.Color.Gold;
                            break;
                    }
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