

namespace SoftCob.Views.Configuraciones
{
    
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_AccesoListasAdmin : Page
    {
        #region Variables
        ListItem _itemc = new ListItem();
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        CheckBox _chklista = new CheckBox();
        DataRow[] _resul;
        DataRow _resultado;
        string _verlista = "", _codigo = "", _response = "";
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
                    Response.Redirect("../Gestion/WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                    return;
                }
                ViewState["GestoresAcceso"] = null;
                ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                Lbltitulo.Text = "Administrar Permisos Acceso Lista Clientes";

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());
                FunCargarCombos(0);
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    DdlCedente.DataSource = new CatalogosDTO().FunGetCedentes();
                    DdlCedente.DataTextField = "Descripcion";
                    DdlCedente.DataValueField = "Codigo";
                    DdlCedente.DataBind();
                    break;
                case 1:
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(153, int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "",
                        ViewState["Conectar"].ToString());
                    ViewState["GestoresAcceso"] = _dts.Tables[0];
                    GrdvGestores.DataSource = _dts;
                    GrdvGestores.DataBind();

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drfila in _dts.Tables[0].Rows)
                        {
                            if (drfila["VerLista"].ToString() == "NO")
                            {
                                ChkTodos.Checked = false;
                                break;
                            }
                        }

                        PnlListaGestores.Visible = true;
                    }
                    else PnlListaGestores.Visible = false;
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DdlCedente_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(1);
        }

        protected void ChkTodos_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesBAS().FunShowJSMessage("Seleccione Cedente..!", this);
                    return;
                }

                _dtb = (DataTable)ViewState["GestoresAcceso"];
                _resul = _dtb.Select("Codigo>'0'");

                foreach (DataRow _drfila in _resul)
                {
                    if (ChkTodos.Checked)
                        _drfila["VerLista"] = "SI";
                    else _drfila["VerLista"] = "NO";
                }

                _dtb.AcceptChanges();
                ViewState["GestoresAcceso"] = _dtb;
                GrdvGestores.DataSource = _dtb;
                GrdvGestores.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkLista_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _chklista = (CheckBox)(gvRow.Cells[1].FindControl("ChkLista"));
                _dtb = (DataTable)ViewState["GestoresAcceso"];
                _codigo = GrdvGestores.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                _resultado = _dtb.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _resultado["VerLista"] = _chklista.Checked ? "SI" : "NO";
                _dtb.AcceptChanges();
                ViewState["GestoresAcceso"] = _dtb;

                if (_chklista.Checked) gvRow.Cells[0].BackColor = System.Drawing.Color.LightSeaGreen;
                else gvRow.Cells[0].BackColor = System.Drawing.Color.Beige;

                _resultado = _dtb.Select("VerLista='NO'").FirstOrDefault();

                if (_resultado == null) ChkTodos.Checked = true;
                else ChkTodos.Checked = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvGestores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chklista = (CheckBox)(e.Row.Cells[1].FindControl("ChkLista"));
                    _verlista = GrdvGestores.DataKeys[e.Row.RowIndex].Values["VerLista"].ToString();

                    if (_verlista == "SI")
                    {
                        e.Row.Cells[0].BackColor = System.Drawing.Color.LightSeaGreen;
                        _chklista.Checked = true;
                    }
                    else e.Row.Cells[0].BackColor = System.Drawing.Color.Beige;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["GestoresAcceso"] != null)
                {
                    _dtb = (DataTable)ViewState["GestoresAcceso"];

                    foreach (DataRow _drfila in _dtb.Rows)
                    {
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(154, int.Parse(DdlCedente.SelectedValue),
                            int.Parse(_drfila["Codigo"].ToString()), int.Parse(Session["usuCodigo"].ToString()), "",
                            _drfila["VerLista"].ToString(), Session["MachineName"].ToString(), ViewState["Conectar"].ToString());
                    }

                    _response = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");
                    Response.Redirect(_response, true);
                }
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