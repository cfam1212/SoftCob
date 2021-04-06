

namespace SoftCob.Views.Configuraciones
{
    using ControllerSoftCob;
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_NuevosCamposEst : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtbcampos = new DataTable();
        DataTable _tblagre = new DataTable();
        DataRow _filagre;
        int _codigocampo = 0, _codigotabla = 0, _maxcodigo = 0;
        CheckBox _chkest = new CheckBox();
        ImageButton _imgdelcampo = new ImageButton();
        DataRow[] _rows;
        DataRow _result;
        string _mensaje = "", _response = "";
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
                    new ElastixDAO().ElastixHangUp(Session["IPLocalAdress"].ToString(), 9999);
                    Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                    return;
                }

                ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                Lbltitulo.Text = "Agregar Campos Estrategias";
                FunCargarCombos();

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarCombos()
        {
            try
            {
                DdlTablas.DataSource = new CatalogosDTO().FunGetTablasBDD();
                DdlTablas.DataTextField = "Descripcion";
                DdlTablas.DataValueField = "Codigo";
                DdlTablas.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DdlTablas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _dts = new EstrategiaDTO().FunGetCamposEstrategia(int.Parse(DdlTablas.SelectedValue));
                ViewState["CamposEstrategia"] = _dts.Tables[0];
                GrdvCampos.DataSource = _dts;
                GrdvCampos.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(20, int.Parse(DdlTablas.SelectedValue), 0, 0, DdlTablas.SelectedItem.ToString(), "", "", ViewState["Conectar"].ToString());

                ViewState["dtsCampos"] = _dts.Tables[0];
                LstOrigen.DataSource = _dts;
                LstOrigen.DataTextField = _dts.Tables[0].Columns[0].ColumnName;
                LstOrigen.DataValueField = _dts.Tables[0].Columns[0].ColumnName;
                LstOrigen.DataBind();
                new FuncionesDTO().FunOrdenar(LstOrigen);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddCampo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ArrayList _elementoseliminar = new ArrayList();
                _tblagre = (DataTable)ViewState["CamposEstrategia"];

                if (_tblagre.Rows.Count == 0) _maxcodigo = 0;
                else
                {
                    foreach (DataRow _dr in _tblagre.Rows)
                    {
                        _maxcodigo = int.Parse(_dr[0].ToString());
                    }
                }

                _dtbcampos = (DataTable)ViewState["dtsCampos"];

                foreach (ListItem _item in LstOrigen.Items)
                {
                    if (_item.Selected == true)
                    {
                        _filagre = _tblagre.NewRow();
                        _filagre["Codigo"] = _maxcodigo + 1;
                        _filagre["CodigoTabla"] = DdlTablas.SelectedValue;
                        _filagre["Campo"] = _item.Text;
                        _rows = _dtbcampos.Select("Campo='" + _item.Value + "'");
                        _filagre["Tipo"] = _rows[0]["Tipo"].ToString();
                        _filagre["Estado"] = "Activo";
                        _filagre["auxv1"] = "";
                        _filagre["auxv2"] = "";
                        _filagre["auxv3"] = "";
                        _filagre["auxi1"] = "0";
                        _filagre["auxi2"] = "0";
                        _filagre["auxi3"] = "0";
                        _tblagre.Rows.Add(_filagre);
                        ViewState["CamposEstrategia"] = _tblagre;
                        GrdvCampos.DataSource = _tblagre;
                        GrdvCampos.DataBind();
                        _elementoseliminar.Add(_item);
                        break;
                    }
                }

                new FuncionesDTO().FunRemoverElement(LstOrigen, _elementoseliminar);
                new FuncionesDTO().FunOrdenar(LstOrigen);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvCampos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkest = (CheckBox)(e.Row.Cells[2].FindControl("chkEstado"));
                    _imgdelcampo = (ImageButton)(e.Row.Cells[3].FindControl("imgDelCampo"));

                    _codigocampo = int.Parse(GrdvCampos.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString());
                    _codigotabla = int.Parse(GrdvCampos.DataKeys[e.Row.RowIndex].Values["CodigoTabla"].ToString());

                    _dts = new EstrategiaDTO().FunGetDatosCampos(_codigotabla, _codigocampo);

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        _chkest.Enabled = true;
                        _chkest.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                        _imgdelcampo.Enabled = false;
                    }
                    else
                    {
                        _chkest.Enabled = false;
                        _chkest.Checked = true;
                        _imgdelcampo.ImageUrl = "~/Botones/eliminar.png";
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkest = (CheckBox)(_gvrow.Cells[1].FindControl("chkEstado"));
                _dtbcampos = (DataTable)ViewState["CamposEstrategia"];
                _codigocampo = int.Parse(GrdvCampos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _result = _dtbcampos.Select("Codigo='" + _codigocampo + "'").FirstOrDefault();
                _result["Estado"] = _chkest.Checked ? "Activo" : "Inactivo";
                _dtbcampos.AcceptChanges();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelCampo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigocampo = int.Parse(GrdvCampos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _dtbcampos = (DataTable)ViewState["CamposEstrategia"];
                _result = _dtbcampos.Select("Codigo='" + _codigocampo.ToString() + "'").FirstOrDefault();
                _result.Delete();
                LstOrigen.Items.Add(new ListItem(_result[2].ToString(), _result[2].ToString()));
                _dtbcampos.AcceptChanges();
                ViewState["CamposEstrategia"] = _dtbcampos;
                GrdvCampos.DataSource = _dtbcampos;
                GrdvCampos.DataBind();
                new FuncionesDTO().FunOrdenar(LstOrigen);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["CamposEstrategia"] != null)
                {
                    _dtbcampos = (DataTable)ViewState["CamposEstrategia"];

                    if (_dtbcampos.Rows.Count == 0)
                    {
                        new FuncionesBAS().FunShowJSMessage("Ingrese un campo para la estrategía..!", this);
                        return;
                    }

                    _mensaje = new EstrategiaDAO().FunCrearCamposEstrategia(0, "", "", "", 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), _dtbcampos, "sp_NewCamposEstrategia", ViewState["Conectar"].ToString());

                    _response = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");

                    if (_mensaje == "") Response.Redirect(_response, false);
                    else new FuncionesBAS().FunShowJSMessage(_mensaje, this);
                }
                else new FuncionesBAS().FunShowJSMessage("No existen Datos en las tablas..!", this);
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