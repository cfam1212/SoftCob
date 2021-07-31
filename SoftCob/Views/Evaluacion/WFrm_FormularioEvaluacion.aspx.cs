namespace SoftCob.Views.Evaluacion
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_FormularioEvaluacion : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        CheckBox _chkselecc = new CheckBox();
        DataTable _dtbdepco = new DataTable();
        DataTable _dtbdepli = new DataTable();
        DataTable _dtbdepmo = new DataTable();
        DataTable _dtbdepac = new DataTable();
        DataTable _dtbdepsp = new DataTable();
        DataTable _dtbdepat = new DataTable();
        DataTable _dtbdepcp = new DataTable();
        DataTable _dtbdeptp = new DataTable();
        string _coddepar = "", _codprotocolo = "", _codpadre = "", _califica = "", _selecc = "", _response = "", _mensaje = "";
        DataRow _resultado;
        DataRow[] _result;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                //if (Session["IN-CALL"].ToString() == "SI")
                //{
                //    new FuncionesDAO().FunShowJSMessage("Se encuentra en Llamada, en cuanto termine la gestión podrá salir de la Lista de Trabajo..!", this);
                //    Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                //}

                ViewState["MesActual"] = DateTime.Today.Month;
                ViewState["AnioActual"] = DateTime.Today.Year;
                Lbltitulo.Text = "Formulario EVALUACION - DESEMPEÑO << " + DateTime.Now.ToString("MM-dd-yyyy") + " >>";
                FunCargarCombos(0);

                
                if (Request["MensajeRetornado"] != null)
                {
                    _mensaje = Request["MensajeRetornado"];
                    new FuncionesDAO().FunShowJSMessage(_mensaje, this, "S", "R");
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarCombos(int opcion)
        {
            try
            {
                switch (opcion)
                {
                    case 0:
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(172, 0, 0, 0, "", "", "", Session["Conectar"].ToString());
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(164, int.Parse(Session["usuCodigo"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());

                        DddlEvaluacion.DataSource = _dts;
                        DddlEvaluacion.DataTextField = "Descripcion";
                        DddlEvaluacion.DataValueField = "Codigo";
                        DddlEvaluacion.DataBind();
                        break;
                    case 1:
                        TblActitud.Visible = false;
                        TblAmbiente.Visible = false;
                        TblCapacidad.Visible = false;
                        TblComunicacion.Visible = false;
                        TblCostos.Visible = false;
                        TblLiderazgo.Visible = false;
                        TblMotivacion.Visible = false;
                        TblSolucion.Visible = false;
                        break;
                    case 2:
                        TblActitud.Visible = true;
                        TblAmbiente.Visible = true;
                        TblCapacidad.Visible = true;
                        TblComunicacion.Visible = true;
                        TblCostos.Visible = true;
                        TblLiderazgo.Visible = true;
                        TblMotivacion.Visible = true;
                        TblSolucion.Visible = true;

                        _dts = new ConsultaDatosDAO().FunConsultaDatos(165, 0, 0, 0, "", "", "", Session["Conectar"].ToString());

                        GrdvEvaluacionCO.DataSource = _dts.Tables[0];
                        GrdvEvaluacionCO.DataBind();
                        ViewState["DepartamentoCO"] = _dts.Tables[1];
                        GrdvDepartamentoCO.DataSource = _dts.Tables[1];
                        GrdvDepartamentoCO.DataBind();

                        GrdvEvaluacionLI.DataSource = _dts.Tables[2];
                        GrdvEvaluacionLI.DataBind();
                        ViewState["DepartamentoLI"] = _dts.Tables[3];
                        GrdvDepartamentoLI.DataSource = _dts.Tables[3];
                        GrdvDepartamentoLI.DataBind();

                        GrdvEvaluacionMO.DataSource = _dts.Tables[4];
                        GrdvEvaluacionMO.DataBind();
                        ViewState["DepartamentoMO"] = _dts.Tables[5];
                        GrdvDepartamentoMO.DataSource = _dts.Tables[5];
                        GrdvDepartamentoMO.DataBind();

                        GrdvEvaluacionAC.DataSource = _dts.Tables[6];
                        GrdvEvaluacionAC.DataBind();
                        ViewState["DepartamentoAC"] = _dts.Tables[7];
                        GrdvDepartamentoAC.DataSource = _dts.Tables[7];
                        GrdvDepartamentoAC.DataBind();

                        GrdvEvaluacionSP.DataSource = _dts.Tables[8];
                        GrdvEvaluacionSP.DataBind();
                        ViewState["DepartamentoSP"] = _dts.Tables[9];
                        GrdvDepartamentoSP.DataSource = _dts.Tables[9];
                        GrdvDepartamentoSP.DataBind();

                        GrdvEvaluacionAT.DataSource = _dts.Tables[10];
                        GrdvEvaluacionAT.DataBind();
                        ViewState["DepartamentoAT"] = _dts.Tables[11];
                        GrdvDepartamentoAT.DataSource = _dts.Tables[11];
                        GrdvDepartamentoAT.DataBind();

                        GrdvCapacidadP.DataSource = _dts.Tables[12];
                        GrdvCapacidadP.DataBind();
                        ViewState["DepartamentoCP"] = _dts.Tables[13];
                        GrdvDepartamentoCP.DataSource = _dts.Tables[13];
                        GrdvDepartamentoCP.DataBind();

                        GrdvCostoP.DataSource = _dts.Tables[14];
                        GrdvCostoP.DataBind();
                        ViewState["DepartamentoTP"] = _dts.Tables[15];
                        GrdvDepartamentoTP.DataSource = _dts.Tables[15];
                        GrdvDepartamentoTP.DataBind();

                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DddlEvaluacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DddlEvaluacion.SelectedValue == "0") FunCargarCombos(1);
            else FunCargarCombos(2);
        }

        protected void ImgSelecCO_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvEvaluacionCO.Rows)
                {
                    _fr.Cells[2].BackColor = System.Drawing.Color.White;
                }

                _codprotocolo = GrdvEvaluacionCO.DataKeys[_gvrow.RowIndex].Values["CodigoProtocolo"].ToString();
                _codpadre = GrdvEvaluacionCO.DataKeys[_gvrow.RowIndex].Values["CodigoPadre"].ToString();
                _califica = GrdvEvaluacionCO.DataKeys[_gvrow.RowIndex].Values["Calificacion"].ToString();
                _dtbdepco = (DataTable)ViewState["DepartamentoCO"];
                _resultado = _dtbdepco.Select("Selecc='SI'").FirstOrDefault();

                if (_resultado == null) new FuncionesDAO().FunShowJSMessage("Seleccione Departamento a Calificar..!", this);
                else
                {
                    GrdvEvaluacionCO.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.LightSeaGreen;
                    _resultado["CodigoPadre"] = _codpadre;
                    _resultado["CodigoProtocolo"] = _codprotocolo;
                    _resultado["Calificacion"] = _califica;
                    _dtbdepco.AcceptChanges();

                    GrdvDepartamentoCO.DataSource = _dtbdepco;
                    GrdvDepartamentoCO.DataBind();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkDepCO_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvEvaluacionCO.Rows)
                {
                    _fr.Cells[2].BackColor = System.Drawing.Color.White;
                }

                foreach (GridViewRow _row in GrdvDepartamentoCO.Rows)
                {
                    (_row.FindControl("ChkDepCO") as CheckBox).Checked = false;
                    _row.Cells[2].BackColor = System.Drawing.Color.White;
                }

                _chkselecc = (CheckBox)(_gvrow.Cells[0].FindControl("ChkDepCO"));
                _chkselecc.Checked = true;
                _coddepar = GrdvDepartamentoCO.DataKeys[_gvrow.RowIndex].Values["CodigoDepartamento"].ToString();
                GrdvDepartamentoCO.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.Coral;
                _dtbdepco = (DataTable)ViewState["DepartamentoCO"];
                _result = _dtbdepco.Select("CodigoDepartamento>0");

                foreach (DataRow _drfila in _result)
                {
                    _drfila["Selecc"] = "NO";
                }

                _resultado = _dtbdepco.Select("CodigoDepartamento='" + _coddepar + "'").FirstOrDefault();
                _resultado["Selecc"] = _chkselecc.Checked ? "SI" : "NO";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSelecLI_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvEvaluacionLI.Rows)
                {
                    _fr.Cells[2].BackColor = System.Drawing.Color.White;
                }

                _codprotocolo = GrdvEvaluacionLI.DataKeys[_gvrow.RowIndex].Values["CodigoProtocolo"].ToString();
                _codpadre = GrdvEvaluacionLI.DataKeys[_gvrow.RowIndex].Values["CodigoPadre"].ToString();
                _califica = GrdvEvaluacionLI.DataKeys[_gvrow.RowIndex].Values["Calificacion"].ToString();
                _dtbdepli = (DataTable)ViewState["DepartamentoLI"];
                _resultado = _dtbdepli.Select("Selecc='SI'").FirstOrDefault();

                if (_resultado == null) new FuncionesDAO().FunShowJSMessage("Seleccione Departamento a Calificar..!", this, "W", "C");
                else
                {
                    GrdvEvaluacionLI.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.LightSeaGreen;
                    _resultado["CodigoPadre"] = _codpadre;
                    _resultado["CodigoProtocolo"] = _codprotocolo;
                    _resultado["Calificacion"] = _califica;
                    _dtbdepli.AcceptChanges();
                    GrdvDepartamentoLI.DataSource = _dtbdepli;
                    GrdvDepartamentoLI.DataBind();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkDepLI_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvEvaluacionLI.Rows)
                {
                    _fr.Cells[2].BackColor = System.Drawing.Color.White;
                }

                foreach (GridViewRow _row in GrdvDepartamentoLI.Rows)
                {
                    (_row.FindControl("ChkDepLI") as CheckBox).Checked = false;
                    _row.Cells[2].BackColor = System.Drawing.Color.White;
                }

                _chkselecc = (CheckBox)(_gvrow.Cells[0].FindControl("ChkDepLI"));
                _chkselecc.Checked = true;
                _coddepar = GrdvDepartamentoLI.DataKeys[_gvrow.RowIndex].Values["CodigoDepartamento"].ToString();
                GrdvDepartamentoLI.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.Coral;
                _dtbdepli = (DataTable)ViewState["DepartamentoLI"];

                _result = _dtbdepli.Select("CodigoDepartamento>0");

                foreach (DataRow _drfila in _result)
                {
                    _drfila["Selecc"] = "NO";
                }

                _resultado = _dtbdepli.Select("CodigoDepartamento='" + _coddepar + "'").FirstOrDefault();
                _resultado["Selecc"] = _chkselecc.Checked ? "SI" : "NO";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSelecMO_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvEvaluacionMO.Rows)
                {
                    _fr.Cells[2].BackColor = System.Drawing.Color.White;
                }

                _codprotocolo = GrdvEvaluacionMO.DataKeys[_gvrow.RowIndex].Values["CodigoProtocolo"].ToString();
                _codpadre = GrdvEvaluacionMO.DataKeys[_gvrow.RowIndex].Values["CodigoPadre"].ToString();
                _califica = GrdvEvaluacionMO.DataKeys[_gvrow.RowIndex].Values["Calificacion"].ToString();
                _dtbdepmo = (DataTable)ViewState["DepartamentoMO"];
                _resultado = _dtbdepmo.Select("Selecc='SI'").FirstOrDefault();

                if (_resultado == null) new FuncionesDAO().FunShowJSMessage("Seleccione Departamento a Calificar..!", this, "W", "C");
                else
                {
                    GrdvEvaluacionMO.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.LightSeaGreen;
                    _resultado["CodigoPadre"] = _codpadre;
                    _resultado["CodigoProtocolo"] = _codprotocolo;
                    _resultado["Calificacion"] = _califica;
                    _dtbdepmo.AcceptChanges();
                    GrdvDepartamentoMO.DataSource = _dtbdepmo;
                    GrdvDepartamentoMO.DataBind();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkDepMO_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvEvaluacionMO.Rows)
                {
                    _fr.Cells[2].BackColor = System.Drawing.Color.White;
                }

                foreach (GridViewRow _row in GrdvDepartamentoMO.Rows)
                {
                    (_row.FindControl("ChkDepMO") as CheckBox).Checked = false;
                    _row.Cells[2].BackColor = System.Drawing.Color.White;
                }

                _chkselecc = (CheckBox)(_gvrow.Cells[0].FindControl("ChkDepMO"));
                _chkselecc.Checked = true;
                _coddepar = GrdvDepartamentoMO.DataKeys[_gvrow.RowIndex].Values["CodigoDepartamento"].ToString();
                GrdvDepartamentoMO.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.Coral;
                _dtbdepmo = (DataTable)ViewState["DepartamentoMO"];
                _result = _dtbdepmo.Select("CodigoDepartamento>0");

                foreach (DataRow _drfila in _result)
                {
                    _drfila["Selecc"] = "NO";
                }

                _resultado = _dtbdepmo.Select("CodigoDepartamento='" + _coddepar + "'").FirstOrDefault();
                _resultado["Selecc"] = _chkselecc.Checked ? "SI" : "NO";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSelecAC_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow fr in GrdvEvaluacionAC.Rows)
                {
                    fr.Cells[2].BackColor = System.Drawing.Color.White;
                }

                _codprotocolo = GrdvEvaluacionAC.DataKeys[_gvrow.RowIndex].Values["CodigoProtocolo"].ToString();
                _codpadre = GrdvEvaluacionAC.DataKeys[_gvrow.RowIndex].Values["CodigoPadre"].ToString();
                _califica = GrdvEvaluacionAC.DataKeys[_gvrow.RowIndex].Values["Calificacion"].ToString();
                _dtbdepac = (DataTable)ViewState["DepartamentoAC"];
                _resultado = _dtbdepac.Select("Selecc='SI'").FirstOrDefault();

                if (_resultado == null) new FuncionesDAO().FunShowJSMessage("Seleccione Departamento a Calificar..!", this, "W", "C");
                else
                {
                    GrdvEvaluacionAC.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.LightSeaGreen;
                    _resultado["CodigoPadre"] = _codpadre;
                    _resultado["CodigoProtocolo"] = _codprotocolo;
                    _resultado["Calificacion"] = _califica;
                    _dtbdepac.AcceptChanges();
                    GrdvDepartamentoAC.DataSource = _dtbdepac;
                    GrdvDepartamentoAC.DataBind();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkDepAC_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvEvaluacionAC.Rows)
                {
                    _fr.Cells[2].BackColor = System.Drawing.Color.White;
                }

                foreach (GridViewRow _row in GrdvDepartamentoAC.Rows)
                {
                    (_row.FindControl("ChkDepAC") as CheckBox).Checked = false;
                    _row.Cells[2].BackColor = System.Drawing.Color.White;
                }

                _chkselecc = (CheckBox)(_gvrow.Cells[0].FindControl("ChkDepAC"));
                _chkselecc.Checked = true;
                _coddepar = GrdvDepartamentoAC.DataKeys[_gvrow.RowIndex].Values["CodigoDepartamento"].ToString();
                GrdvDepartamentoAC.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.Coral;
                _dtbdepac = (DataTable)ViewState["DepartamentoAC"];
                _result = _dtbdepac.Select("CodigoDepartamento>0");

                foreach (DataRow _drfila in _result)
                {
                    _drfila["Selecc"] = "NO";
                }

                _resultado = _dtbdepac.Select("CodigoDepartamento='" + _coddepar + "'").FirstOrDefault();
                _resultado["Selecc"] = _chkselecc.Checked ? "SI" : "NO";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSelecSP_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvEvaluacionSP.Rows)
                {
                    _fr.Cells[2].BackColor = System.Drawing.Color.White;
                }

                _codprotocolo = GrdvEvaluacionSP.DataKeys[_gvrow.RowIndex].Values["CodigoProtocolo"].ToString();
                _codpadre = GrdvEvaluacionSP.DataKeys[_gvrow.RowIndex].Values["CodigoPadre"].ToString();
                _califica = GrdvEvaluacionSP.DataKeys[_gvrow.RowIndex].Values["Calificacion"].ToString();
                _dtbdepsp = (DataTable)ViewState["DepartamentoSP"];
                _resultado = _dtbdepsp.Select("Selecc='SI'").FirstOrDefault();

                if (_resultado == null) new FuncionesDAO().FunShowJSMessage("Seleccione Departamento a Calificar..!", this, "W", "C");
                else
                {
                    GrdvEvaluacionSP.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.LightSeaGreen;
                    _resultado["CodigoPadre"] = _codpadre;
                    _resultado["CodigoProtocolo"] = _codprotocolo;
                    _resultado["Calificacion"] = _califica;
                    _dtbdepsp.AcceptChanges();
                    GrdvDepartamentoSP.DataSource = _dtbdepsp;
                    GrdvDepartamentoSP.DataBind();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkDepSP_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvEvaluacionSP.Rows)
                {
                    _fr.Cells[2].BackColor = System.Drawing.Color.White;
                }

                foreach (GridViewRow _row in GrdvDepartamentoSP.Rows)
                {
                    (_row.FindControl("ChkDepSP") as CheckBox).Checked = false;
                    _row.Cells[2].BackColor = System.Drawing.Color.White;
                }

                _chkselecc = (CheckBox)(_gvrow.Cells[0].FindControl("ChkDepSP"));
                _chkselecc.Checked = true;
                _coddepar = GrdvDepartamentoSP.DataKeys[_gvrow.RowIndex].Values["CodigoDepartamento"].ToString();
                GrdvDepartamentoSP.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.Coral;
                _dtbdepsp = (DataTable)ViewState["DepartamentoSP"];
                _result = _dtbdepsp.Select("CodigoDepartamento>0");

                foreach (DataRow _drfila in _result)
                {
                    _drfila["Selecc"] = "NO";
                }

                _resultado = _dtbdepsp.Select("CodigoDepartamento='" + _coddepar + "'").FirstOrDefault();
                _resultado["Selecc"] = _chkselecc.Checked ? "SI" : "NO";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSelecAT_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvEvaluacionAT.Rows)
                {
                    _fr.Cells[2].BackColor = System.Drawing.Color.White;
                }

                _codprotocolo = GrdvEvaluacionAT.DataKeys[_gvrow.RowIndex].Values["CodigoProtocolo"].ToString();
                _codpadre = GrdvEvaluacionAT.DataKeys[_gvrow.RowIndex].Values["CodigoPadre"].ToString();
                _califica = GrdvEvaluacionAT.DataKeys[_gvrow.RowIndex].Values["Calificacion"].ToString();
                _dtbdepat = (DataTable)ViewState["DepartamentoAT"];
                _resultado = _dtbdepat.Select("Selecc='SI'").FirstOrDefault();

                if (_resultado == null) new FuncionesDAO().FunShowJSMessage("Seleccione Departamento a Calificar..!", this, "W", "C");
                else
                {
                    GrdvEvaluacionAT.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.LightSeaGreen;
                    _resultado["CodigoPadre"] = _codpadre;
                    _resultado["CodigoProtocolo"] = _codprotocolo;
                    _resultado["Calificacion"] = _califica;
                    _dtbdepat.AcceptChanges();
                    GrdvDepartamentoAT.DataSource = _dtbdepat;
                    GrdvDepartamentoAT.DataBind();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkDepAT_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvEvaluacionAT.Rows)
                {
                    _fr.Cells[2].BackColor = System.Drawing.Color.White;
                }

                foreach (GridViewRow _row in GrdvDepartamentoAT.Rows)
                {
                    (_row.FindControl("ChkDepAT") as CheckBox).Checked = false;
                    _row.Cells[2].BackColor = System.Drawing.Color.White;
                }

                _chkselecc = (CheckBox)(_gvrow.Cells[0].FindControl("ChkDepAT"));
                _chkselecc.Checked = true;
                _coddepar = GrdvDepartamentoAT.DataKeys[_gvrow.RowIndex].Values["CodigoDepartamento"].ToString();
                GrdvDepartamentoAT.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.Coral;
                _dtbdepat = (DataTable)ViewState["DepartamentoAT"];
                _result = _dtbdepat.Select("CodigoDepartamento>0");

                foreach (DataRow _drfila in _result)
                {
                    _drfila["Selecc"] = "NO";
                }

                _resultado = _dtbdepat.Select("CodigoDepartamento='" + _coddepar + "'").FirstOrDefault();
                _resultado["Selecc"] = _chkselecc.Checked ? "SI" : "NO";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSelecCP_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvCapacidadP.Rows)
                {
                    _fr.Cells[2].BackColor = System.Drawing.Color.White;
                }

                _codprotocolo = GrdvCapacidadP.DataKeys[_gvrow.RowIndex].Values["CodigoProtocolo"].ToString();
                _codpadre = GrdvCapacidadP.DataKeys[_gvrow.RowIndex].Values["CodigoPadre"].ToString();
                _califica = GrdvCapacidadP.DataKeys[_gvrow.RowIndex].Values["Calificacion"].ToString();
                _dtbdepcp = (DataTable)ViewState["DepartamentoCP"];
                _resultado = _dtbdepcp.Select("Selecc='SI'").FirstOrDefault();

                if (_resultado == null) new FuncionesDAO().FunShowJSMessage("Seleccione Departamento a Calificar..!", this, "W", "C");
                else
                {
                    GrdvCapacidadP.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.LightSeaGreen;
                    _resultado["CodigoPadre"] = _codpadre;
                    _resultado["CodigoProtocolo"] = _codprotocolo;
                    _resultado["Calificacion"] = _califica;
                    _dtbdepcp.AcceptChanges();
                    GrdvDepartamentoCP.DataSource = _dtbdepcp;
                    GrdvDepartamentoCP.DataBind();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkDepCP_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvCapacidadP.Rows)
                {
                    _fr.Cells[2].BackColor = System.Drawing.Color.White;
                }

                foreach (GridViewRow _row in GrdvDepartamentoCP.Rows)
                {
                    (_row.FindControl("ChkDepCP") as CheckBox).Checked = false;
                    _row.Cells[2].BackColor = System.Drawing.Color.White;
                }

                _chkselecc = (CheckBox)(_gvrow.Cells[0].FindControl("ChkDepCP"));
                _chkselecc.Checked = true;
                _coddepar = GrdvDepartamentoCP.DataKeys[_gvrow.RowIndex].Values["CodigoDepartamento"].ToString();
                GrdvDepartamentoCP.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.Coral;
                _dtbdepcp = (DataTable)ViewState["DepartamentoCP"];
                _result = _dtbdepcp.Select("CodigoDepartamento>0");

                foreach (DataRow _drfila in _result)
                {
                    _drfila["Selecc"] = "NO";
                }

                _resultado = _dtbdepcp.Select("CodigoDepartamento='" + _coddepar + "'").FirstOrDefault();
                _resultado["Selecc"] = _chkselecc.Checked ? "SI" : "NO";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSelecTP_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvCostoP.Rows)
                {
                    _fr.Cells[2].BackColor = System.Drawing.Color.White;
                }

                _codprotocolo = GrdvCostoP.DataKeys[_gvrow.RowIndex].Values["CodigoProtocolo"].ToString();
                _codpadre = GrdvCostoP.DataKeys[_gvrow.RowIndex].Values["CodigoPadre"].ToString();
                _califica = GrdvCostoP.DataKeys[_gvrow.RowIndex].Values["Calificacion"].ToString();
                _dtbdeptp = (DataTable)ViewState["DepartamentoTP"];
                _resultado = _dtbdeptp.Select("Selecc='SI'").FirstOrDefault();

                if (_resultado == null) new FuncionesDAO().FunShowJSMessage("Seleccione Departamento a Calificar..!", this, "W", "C");
                else
                {
                    GrdvCostoP.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.LightSeaGreen;
                    _resultado["CodigoPadre"] = _codpadre;
                    _resultado["CodigoProtocolo"] = _codprotocolo;
                    _resultado["Calificacion"] = _califica;
                    _dtbdeptp.AcceptChanges();
                    GrdvDepartamentoTP.DataSource = _dtbdeptp;
                    GrdvDepartamentoTP.DataBind();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkDepTP_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvCostoP.Rows)
                {
                    _fr.Cells[2].BackColor = System.Drawing.Color.White;
                }

                foreach (GridViewRow _row in GrdvDepartamentoTP.Rows)
                {
                    (_row.FindControl("ChkDepTP") as CheckBox).Checked = false;
                    _row.Cells[2].BackColor = System.Drawing.Color.White;
                }

                _chkselecc = (CheckBox)(_gvrow.Cells[0].FindControl("ChkDepTP"));
                _chkselecc.Checked = true;
                _coddepar = GrdvDepartamentoTP.DataKeys[_gvrow.RowIndex].Values["CodigoDepartamento"].ToString();
                GrdvDepartamentoTP.Rows[_gvrow.RowIndex].Cells[2].BackColor = System.Drawing.Color.Coral;
                _dtbdeptp = (DataTable)ViewState["DepartamentoTP"];
                _result = _dtbdeptp.Select("CodigoDepartamento>0");

                foreach (DataRow _drfila in _result)
                {
                    _drfila["Selecc"] = "NO";
                }

                _resultado = _dtbdeptp.Select("CodigoDepartamento='" + _coddepar + "'").FirstOrDefault();
                _resultado["Selecc"] = _chkselecc.Checked ? "SI" : "NO";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvDepartamentoLI_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex >= 0)
            {
                _chkselecc = (CheckBox)(e.Row.Cells[0].FindControl("ChkDepLI"));
                _selecc = GrdvDepartamentoLI.DataKeys[e.Row.RowIndex].Values["Selecc"].ToString();

                if (_selecc == "SI")
                {
                    e.Row.Cells[2].BackColor = System.Drawing.Color.Coral;
                    _chkselecc.Checked = true;
                }
            }
        }

        protected void GrdvDepartamentoMO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex >= 0)
            {
                _chkselecc = (CheckBox)(e.Row.Cells[0].FindControl("ChkDepMO"));
                _selecc = GrdvDepartamentoMO.DataKeys[e.Row.RowIndex].Values["Selecc"].ToString();

                if (_selecc == "SI")
                {
                    e.Row.Cells[2].BackColor = System.Drawing.Color.Coral;
                    _chkselecc.Checked = true;
                }
            }
        }

        protected void GrdvDepartamentoAC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex >= 0)
            {
                _chkselecc = (CheckBox)(e.Row.Cells[0].FindControl("ChkDepAC"));
                _selecc = GrdvDepartamentoAC.DataKeys[e.Row.RowIndex].Values["Selecc"].ToString();

                if (_selecc == "SI")
                {
                    e.Row.Cells[2].BackColor = System.Drawing.Color.Coral;
                    _chkselecc.Checked = true;
                }
            }
        }

        protected void GrdvDepartamentoSP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex >= 0)
            {
                _chkselecc = (CheckBox)(e.Row.Cells[0].FindControl("ChkDepSP"));
                _selecc = GrdvDepartamentoSP.DataKeys[e.Row.RowIndex].Values["Selecc"].ToString();

                if (_selecc == "SI")
                {
                    e.Row.Cells[2].BackColor = System.Drawing.Color.Coral;
                    _chkselecc.Checked = true;
                }
            }
        }

        protected void GrdvDepartamentoAT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex >= 0)
            {
                _chkselecc = (CheckBox)(e.Row.Cells[0].FindControl("ChkDepAT"));
                _selecc = GrdvDepartamentoAT.DataKeys[e.Row.RowIndex].Values["Selecc"].ToString();

                if (_selecc == "SI")
                {
                    e.Row.Cells[2].BackColor = System.Drawing.Color.Coral;
                    _chkselecc.Checked = true;
                }
            }
        }

        protected void GrdvDepartamentoCP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex >= 0)
            {
                _chkselecc = (CheckBox)(e.Row.Cells[0].FindControl("ChkDepCP"));
                _selecc = GrdvDepartamentoCP.DataKeys[e.Row.RowIndex].Values["Selecc"].ToString();

                if (_selecc == "SI")
                {
                    e.Row.Cells[2].BackColor = System.Drawing.Color.Coral;
                    _chkselecc.Checked = true;
                }
            }
        }

        protected void GrdvDepartamentoTP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex >= 0)
            {
                _chkselecc = (CheckBox)(e.Row.Cells[0].FindControl("ChkDepTP"));
                _selecc = GrdvDepartamentoTP.DataKeys[e.Row.RowIndex].Values["Selecc"].ToString();

                if (_selecc == "SI")
                {
                    e.Row.Cells[2].BackColor = System.Drawing.Color.Coral;
                    _chkselecc.Checked = true;
                }
            }
        }

        protected void GrdvDepartamentoCO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex >= 0)
            {
                _chkselecc = (CheckBox)(e.Row.Cells[0].FindControl("ChkDepCO"));
                _selecc = GrdvDepartamentoCO.DataKeys[e.Row.RowIndex].Values["Selecc"].ToString();

                if (_selecc == "SI")
                {
                    e.Row.Cells[2].BackColor = System.Drawing.Color.Coral;
                    _chkselecc.Checked = true;
                }
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (DddlEvaluacion.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Evaluación Desempeño..!", this, "W", "C");
                    return;
                }

                _dtbdepco = (DataTable)ViewState["DepartamentoCO"];
                _resultado = _dtbdepco.Select("Calificacion=0").FirstOrDefault();

                if (_resultado != null)
                {
                    new FuncionesDAO().FunShowJSMessage("Termine Calificación Protocolo COMUNICACION..!", this, "E", "C");
                    return;
                }

                _dtbdepli = (DataTable)ViewState["DepartamentoLI"];
                _resultado = _dtbdepli.Select("Calificacion=0").FirstOrDefault();

                if (_resultado != null)
                {
                    new FuncionesDAO().FunShowJSMessage("Termine Calificación Protocolo LIDERAZGO..!", this, "E", "C");
                    return;
                }

                _dtbdepmo = (DataTable)ViewState["DepartamentoMO"];
                _resultado = _dtbdepmo.Select("Calificacion=0").FirstOrDefault();

                if (_resultado != null)
                {
                    new FuncionesDAO().FunShowJSMessage("Termine Calificación Protocolo MOTIVACION..!", this, "E", "C");
                    return;
                }

                _dtbdepac = (DataTable)ViewState["DepartamentoAC"];
                _resultado = _dtbdepac.Select("Calificacion=0").FirstOrDefault();

                if (_resultado != null)
                {
                    new FuncionesDAO().FunShowJSMessage("Termine Calificación Protocolo ACTITUD Y COLABORACION..!", this, "E", "C");
                    return;
                }

                _dtbdepsp = (DataTable)ViewState["DepartamentoSP"];
                _resultado = _dtbdepsp.Select("Calificacion=0").FirstOrDefault();

                if (_resultado != null)
                {
                    new FuncionesDAO().FunShowJSMessage("Termine Calificación Protocolo SOLUCION DE PROBLEMAS..!", this, "E", "C");
                    return;
                }

                _dtbdepat = (DataTable)ViewState["DepartamentoAT"];
                _resultado = _dtbdepat.Select("Calificacion=0").FirstOrDefault();

                if (_resultado != null)
                {
                    new FuncionesDAO().FunShowJSMessage("Termine Calificación Protocolo AMBIENTE DE TRABAJO..!", this, "E", "C");
                    return;
                }

                _dtbdepcp = (DataTable)ViewState["DepartamentoCP"];
                _resultado = _dtbdepcp.Select("Calificacion=0").FirstOrDefault();

                if (_resultado != null)
                {
                    new FuncionesDAO().FunShowJSMessage("Termine Calificación Protocolo CAPACIDAD PERSONAL..!", this, "E", "C");
                    return;
                }

                _dtbdeptp = (DataTable)ViewState["DepartamentoTP"];
                _resultado = _dtbdeptp.Select("Calificacion=0").FirstOrDefault();

                if (_resultado != null)
                {
                    new FuncionesDAO().FunShowJSMessage("Termine Calificación Protocolo COSTOS Y PRODUCTIVIDAD..!", this, "E", "C");
                    return;
                }

                foreach (DataRow _drfila in _dtbdepco.Rows)
                {
                    _dts = new ConsultaDatosDAO().FunNewEvaluacionDesempenio(0, int.Parse(DddlEvaluacion.SelectedValue),
                        int.Parse(_drfila["CodigoPadre"].ToString()), int.Parse(_drfila["CodigoProtocolo"].ToString()),
                        int.Parse(_drfila["CodigoDepartamento"].ToString()), int.Parse(_drfila["Calificacion"].ToString()),
                        "", "", "", 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                        Session["Conectar"].ToString());
                }

                foreach (DataRow _drfila in _dtbdepli.Rows)
                {
                    _dts = new ConsultaDatosDAO().FunNewEvaluacionDesempenio(0, int.Parse(DddlEvaluacion.SelectedValue),
                        int.Parse(_drfila["CodigoPadre"].ToString()), int.Parse(_drfila["CodigoProtocolo"].ToString()),
                        int.Parse(_drfila["CodigoDepartamento"].ToString()), int.Parse(_drfila["Calificacion"].ToString()),
                        "", "", "", 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                        Session["Conectar"].ToString());
                }

                foreach (DataRow _drfila in _dtbdepmo.Rows)
                {
                    _dts = new ConsultaDatosDAO().FunNewEvaluacionDesempenio(0, int.Parse(DddlEvaluacion.SelectedValue),
                        int.Parse(_drfila["CodigoPadre"].ToString()), int.Parse(_drfila["CodigoProtocolo"].ToString()),
                        int.Parse(_drfila["CodigoDepartamento"].ToString()), int.Parse(_drfila["Calificacion"].ToString()),
                        "", "", "", 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                        Session["Conectar"].ToString());
                }

                foreach (DataRow _drfila in _dtbdepac.Rows)
                {
                    _dts = new ConsultaDatosDAO().FunNewEvaluacionDesempenio(0, int.Parse(DddlEvaluacion.SelectedValue),
                        int.Parse(_drfila["CodigoPadre"].ToString()), int.Parse(_drfila["CodigoProtocolo"].ToString()),
                        int.Parse(_drfila["CodigoDepartamento"].ToString()), int.Parse(_drfila["Calificacion"].ToString()),
                        "", "", "", 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                        Session["Conectar"].ToString());
                }

                foreach (DataRow _drfila in _dtbdepsp.Rows)
                {
                    _dts = new ConsultaDatosDAO().FunNewEvaluacionDesempenio(0, int.Parse(DddlEvaluacion.SelectedValue),
                        int.Parse(_drfila["CodigoPadre"].ToString()), int.Parse(_drfila["CodigoProtocolo"].ToString()),
                        int.Parse(_drfila["CodigoDepartamento"].ToString()), int.Parse(_drfila["Calificacion"].ToString()),
                        "", "", "", 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                        Session["Conectar"].ToString());
                }

                foreach (DataRow _drfila in _dtbdepat.Rows)
                {
                    _dts = new ConsultaDatosDAO().FunNewEvaluacionDesempenio(0, int.Parse(DddlEvaluacion.SelectedValue),
                        int.Parse(_drfila["CodigoPadre"].ToString()), int.Parse(_drfila["CodigoProtocolo"].ToString()),
                        int.Parse(_drfila["CodigoDepartamento"].ToString()), int.Parse(_drfila["Calificacion"].ToString()),
                        "", "", "", 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                        Session["Conectar"].ToString());
                }

                foreach (DataRow _drfila in _dtbdepcp.Rows)
                {
                    _dts = new ConsultaDatosDAO().FunNewEvaluacionDesempenio(0, int.Parse(DddlEvaluacion.SelectedValue),
                        int.Parse(_drfila["CodigoPadre"].ToString()), int.Parse(_drfila["CodigoProtocolo"].ToString()),
                        int.Parse(_drfila["CodigoDepartamento"].ToString()), int.Parse(_drfila["Calificacion"].ToString()),
                        "", "", "", 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                        Session["Conectar"].ToString());
                }

                foreach (DataRow _drfila in _dtbdeptp.Rows)
                {
                    _dts = new ConsultaDatosDAO().FunNewEvaluacionDesempenio(0, int.Parse(DddlEvaluacion.SelectedValue),
                        int.Parse(_drfila["CodigoPadre"].ToString()), int.Parse(_drfila["CodigoProtocolo"].ToString()),
                        int.Parse(_drfila["CodigoDepartamento"].ToString()), int.Parse(_drfila["Calificacion"].ToString()),
                        "", "", "", 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                        Session["Conectar"].ToString());
                }

                _response = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");
                Response.Redirect(_response, false);
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