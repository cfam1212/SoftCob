namespace SoftCob.Views.BPM
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ListaTipoConvenio : Page
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
                    Lbltitulo.Text = "Consulta General << NOTIFICACIONES >>";
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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(199, 1, 0, 0, "", "CCV", "", Session["Conectar"].ToString());
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
        protected void ImgDetalle_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                _codigoclde = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCLDE"].ToString();
                _codigopers = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPERS"].ToString();
                _numdocumento = GrdvDatos.DataKeys[gvRow.RowIndex].Values["NumDocumento"].ToString();

                Response.Redirect("WFrm_SeguimientoConvenio.aspx?CodigoCITA=" + _codigo + "&CodigoPERS=" + _codigopers +
                    "&CodigoCLDE=" + _codigoclde + "&CodigoGEST=" + _codigogest + "&NumDocumento=" + _numdocumento, true);

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
                        case "CCV":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.AliceBlue;
                            break;
                        case "CCS":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.LimeGreen;
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