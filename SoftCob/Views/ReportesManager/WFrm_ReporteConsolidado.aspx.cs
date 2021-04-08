namespace SoftCob.Views.ReportesManager
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ReporteConsolidado : Page
    {
        #region Variables
        int _totalOperaciones = 0;
        decimal _totalSaldos = 0.00M;
        string _codigocpce = "", _codigocede = "";
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["Conexion"] = ConfigurationManager.AppSettings["SqlConn"];
                Lbltitulo.Text = "Reporte Consolidado CEDENTES";
                FunCargarMantenimiento();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunGerReporteConsolidado(1, 0, 0, 0, "", "", 0, 0, ViewState["Conexion"].ToString());
                if (_dts.Tables[0].Rows.Count > 0)
                {
                    _dtb = _dts.Tables[0];
                    _totalOperaciones = int.Parse(_dtb.Compute("Sum(Operaciones)", "").ToString());
                    _totalSaldos = decimal.Parse(_dtb.Compute("Sum(SumSaldo)", "").ToString());
                    GrdvDatos.DataSource = _dts;
                    GrdvDatos.DataBind();
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                    lblOperaciones.InnerText = _totalOperaciones.ToString("##,###.##");
                    lblSaldos.InnerText = "$" + string.Format("{0:n}", _totalSaldos);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void Btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigocpce = GrdvDatos.DataKeys[gvRow.RowIndex].Values["codigoCPCE"].ToString();
                _codigocede = GrdvDatos.DataKeys[gvRow.RowIndex].Values["codigoCEDE"].ToString();
                Response.Redirect("WFrm_ReporteGestorCedente.aspx?codigoCPCE=" + _codigocpce + "&codigoCEDE=" + _codigocede);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}