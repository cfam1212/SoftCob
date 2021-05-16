namespace SoftCob.Views.Breanch
{
    using ControllerSoftCob;
    using ClosedXML.Excel;
    using System;
    using System.IO;
    using System.Configuration;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ProyeccionAdmin : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        DataTable _dtbproyecc = new DataTable();
        ListItem _itemc = new ListItem();
        ListItem _itemg = new ListItem();
        DataRow _fileagre;
        decimal _enero = 0.00M, _febrero = 0.00M, _marzo = 0.00M, _abril = 0.00M, _mayo = 0.00M, _junio = 0.00M,
            _julio = 0.00M, _agosto = 0.00M, _septiembre = 0.00M, _octubre = 0.00M, _noviembre = 0.00M, _diciembre = 0.00M;
        int _opcion = 0;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.ImgExportar);
                if (!IsPostBack)
                {
                   Session["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                    Lbltitulo.Text = "Reporte Proyecciones";
                    ViewState["MesActual"] = DateTime.Now.Month;
                    FunCargarCombos(0);
                }
                //else
                //{
                //    GrdvDatos.DataSource = (DataTable)ViewState["Proyeccion"];
                //    GrdvDatos.DataBind();
                //    GrdvDatos.UseAccessibleHeader = true;
                //    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                //}
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimiento y Funciones
        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    DdlCedente.DataSource = new CedenteDAO().FunGetCedentes();
                    DdlCedente.DataTextField = "Descripcion";
                    DdlCedente.DataValueField = "Codigo";
                    DdlCedente.DataBind();

                    _itemc.Text = "--Seleccione Catálago/Producto--";
                    _itemc.Value = "0";
                    DdlCatalogo.Items.Add(_itemc);

                    _itemg.Text = "--Seleccione Gestor--";
                    _itemg.Value = "0";
                    DdlGestor.Items.Add(_itemg);

                    new FuncionesDAO().FunCargarComboHoraMinutos(DdlYear, "YEARS");

                    _dtbproyecc.Columns.Add("Gestor");
                    _dtbproyecc.Columns.Add("Presupuesto");
                    _dtbproyecc.Columns.Add("Enero");
                    _dtbproyecc.Columns.Add("Febrero");
                    _dtbproyecc.Columns.Add("Marzo");
                    _dtbproyecc.Columns.Add("Abril");
                    _dtbproyecc.Columns.Add("Mayo");
                    _dtbproyecc.Columns.Add("Junio");
                    _dtbproyecc.Columns.Add("Julio");
                    _dtbproyecc.Columns.Add("Agosto");
                    _dtbproyecc.Columns.Add("Septiembre");
                    _dtbproyecc.Columns.Add("Octubre");
                    _dtbproyecc.Columns.Add("Noviembre");
                    _dtbproyecc.Columns.Add("Diciembre");

                    _fileagre = _dtbproyecc.NewRow();
                    _fileagre["Gestor"] = "";
                    _fileagre["Presupuesto"] = "0";
                    _fileagre["Enero"] = "0";
                    _fileagre["Febrero"] = "0";
                    _fileagre["Marzo"] = "0";
                    _fileagre["Abril"] = "0";
                    _fileagre["Mayo"] = "0";
                    _fileagre["Junio"] = "0";
                    _fileagre["Julio"] = "0";
                    _fileagre["Agosto"] = "0";
                    _fileagre["Septiembre"] = "0";
                    _fileagre["Octubre"] = "0";
                    _fileagre["Noviembre"] = "0";
                    _fileagre["Diciembre"] = "0";
                    _dtbproyecc.Rows.Add(_fileagre);
                    ViewState["Proyeccion"] = _dtbproyecc;

                    break;
                case 1:
                    DdlGestor.Items.Clear();
                    _itemg.Text = "--Seleccione Gestor--";
                    _itemg.Value = "0";
                    DdlGestor.Items.Add(_itemg);
                    DdlGestor.DataSource = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--",
                        int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "",Session["Conectar"].ToString());
                    DdlGestor.DataTextField = "Descripcion";
                    DdlGestor.DataValueField = "Codigo";
                    DdlGestor.DataBind();
                    break;
                case 2:
                    _dts = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataTextField = "CatalogoProducto";
                    DdlCatalogo.DataValueField = "CodigoCatalogo";
                    DdlCatalogo.DataBind();

                    if (_dts.Tables[0].Rows.Count == 0)
                    {
                        _itemc.Text = "--Seleccione Catálago/Producto--";
                        _itemc.Value = "0";
                        DdlCatalogo.Items.Add(_itemc);
                    }
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DdlCedente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(1);
                FunCargarCombos(2);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this);
                    return;
                }

                if (DdlTipoPago.SelectedValue.ToString() == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Proyeccion..!", this);
                    return;
                }

                if (DdlReporte.SelectedValue.ToString() == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Reporte..!", this);
                    return;
                }

                if (DdlGestor.SelectedValue == "0")
                {
                    _dts = new ConsultaDatosDAO().FunRepGerencialG(19, int.Parse(DdlCedente.SelectedValue),
                        int.Parse(DdlCatalogo.SelectedValue), "", "", 0, "sp_RepGerencialV1", "", "", "", "", "", "",
                        int.Parse(DdlYear.SelectedValue), 0, 0, 0, 0, 0, Session["Conectar"].ToString());

                    foreach (DataRow _drfila in _dts.Tables[0].Rows)
                    {
                        _dts = new ConsultaDatosDAO().FunRepGerencialG(20, int.Parse(DdlCedente.SelectedValue),
                            int.Parse(DdlCatalogo.SelectedValue), "", "", int.Parse(_drfila["Gestor"].ToString()),
                            "sp_RepGerencialV1", "", "", "", "", "", "", int.Parse(DdlYear.SelectedValue),
                            0, 0, 0, 0, 0, Session["Conectar"].ToString());
                    }
                }
                else
                {
                    _dts = new ConsultaDatosDAO().FunRepGerencialG(20, int.Parse(DdlCedente.SelectedValue),
                        int.Parse(DdlCatalogo.SelectedValue), "", "", int.Parse(DdlGestor.SelectedValue),
                        "sp_RepGerencialV1", "", "", "", "", "", "", int.Parse(DdlYear.SelectedValue),
                        0, 0, 0, 0, 0, Session["Conectar"].ToString());
                }

                ImgExportar.Visible = false;
                LblExportar.Visible = false;
                DivProyecc.Visible = false;

                if (DdlTipoPago.SelectedValue == "1") _opcion = 15;
                if (DdlTipoPago.SelectedValue == "2") _opcion = 16;
                if (DdlTipoPago.SelectedValue == "3") _opcion = 17;
                if (DdlTipoPago.SelectedValue == "4") _opcion = 18;

                _dts = new ConsultaDatosDAO().FunRepGerencialG(_opcion, int.Parse(DdlCedente.SelectedValue),
                    int.Parse(DdlCatalogo.SelectedValue), "", "", int.Parse(DdlGestor.SelectedValue), "sp_RepGerencialV1",
                    "", "", "", "", "", "", int.Parse(DdlYear.SelectedValue), int.Parse(ViewState["MesActual"].ToString()),
                    int.Parse(DdlReporte.SelectedValue), 0, 0, 0,Session["Conectar"].ToString());

                ViewState["Proyeccion"] = _dts.Tables[0];

                _enero = 0; _febrero = 0; _marzo = 0; _abril = 0; _mayo = 0; _junio = 0; _julio = 0; _agosto = 0;
                _septiembre = 0; _octubre = 0; _noviembre = 0; _diciembre = 0;

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ImgExportar.Visible = true;
                    LblExportar.Visible = true;
                    DivProyecc.Visible = true;
                    GrdvDatos.DataSource = _dts;
                    GrdvDatos.DataBind();
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                else
                {
                    new FuncionesDAO().FunShowJSMessage("No Existen Registros para Mostrar..!", this);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void ImgExportar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtb = (DataTable)ViewState["Proyeccion"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    string FileName = "Reporte_" + DdlTipoPago.SelectedItem.ToString() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
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
                    switch (DdlReporte.SelectedValue)
                    {
                        case "1":
                            if (ViewState["MesActual"].ToString() == "1")
                                e.Row.Cells[2].BackColor = System.Drawing.Color.Orange;
                            if (ViewState["MesActual"].ToString() == "2")
                                e.Row.Cells[3].BackColor = System.Drawing.Color.Orange;
                            if (ViewState["MesActual"].ToString() == "3")
                                e.Row.Cells[4].BackColor = System.Drawing.Color.Orange;
                            if (ViewState["MesActual"].ToString() == "4")
                                e.Row.Cells[5].BackColor = System.Drawing.Color.Orange;
                            if (ViewState["MesActual"].ToString() == "5")
                                e.Row.Cells[6].BackColor = System.Drawing.Color.Orange;
                            if (ViewState["MesActual"].ToString() == "6")
                                e.Row.Cells[7].BackColor = System.Drawing.Color.Orange;
                            if (ViewState["MesActual"].ToString() == "7")
                                e.Row.Cells[8].BackColor = System.Drawing.Color.Orange;
                            if (ViewState["MesActual"].ToString() == "8")
                                e.Row.Cells[9].BackColor = System.Drawing.Color.Orange;
                            if (ViewState["MesActual"].ToString() == "9")
                                e.Row.Cells[10].BackColor = System.Drawing.Color.Orange;
                            if (ViewState["MesActual"].ToString() == "10")
                                e.Row.Cells[11].BackColor = System.Drawing.Color.Orange;
                            if (ViewState["MesActual"].ToString() == "11")
                                e.Row.Cells[12].BackColor = System.Drawing.Color.Orange;
                            if (ViewState["MesActual"].ToString() == "12")
                                e.Row.Cells[13].BackColor = System.Drawing.Color.Orange;
                            break;
                        case "2":
                            e.Row.Cells[2].BackColor = System.Drawing.Color.Orange;
                            e.Row.Cells[3].BackColor = System.Drawing.Color.AliceBlue;
                            e.Row.Cells[4].BackColor = System.Drawing.Color.Bisque;
                            break;
                        case "3":
                            e.Row.Cells[2].BackColor = System.Drawing.Color.Orange;
                            e.Row.Cells[3].BackColor = System.Drawing.Color.AliceBlue;
                            e.Row.Cells[4].BackColor = System.Drawing.Color.Bisque;
                            e.Row.Cells[5].BackColor = System.Drawing.Color.Gold;
                            e.Row.Cells[6].BackColor = System.Drawing.Color.Coral;
                            e.Row.Cells[7].BackColor = System.Drawing.Color.Cyan;
                            break;
                        case "4":
                            e.Row.Cells[2].BackColor = System.Drawing.Color.Orange;
                            e.Row.Cells[4].BackColor = System.Drawing.Color.AliceBlue;
                            e.Row.Cells[6].BackColor = System.Drawing.Color.Bisque;
                            e.Row.Cells[8].BackColor = System.Drawing.Color.Gold;
                            e.Row.Cells[10].BackColor = System.Drawing.Color.Coral;
                            e.Row.Cells[12].BackColor = System.Drawing.Color.Cyan;
                            break;
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    _enero += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Enero"));
                    _febrero += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Febrero"));
                    _marzo += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Marzo"));
                    _abril += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Abril"));
                    _mayo += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Mayo"));
                    _junio += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Junio"));
                    _julio += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Julio"));
                    _agosto += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Agosto"));
                    _septiembre += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Septiembre"));
                    _octubre += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Octubre"));
                    _noviembre += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Noviembre"));
                    _diciembre += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Diciembre"));
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[1].Text = "TOTAL:";
                    e.Row.Cells[2].Text = "$ " + _enero.ToString("N2");
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[3].Text = "$ " + _febrero.ToString("N2");
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[4].Text = _marzo.ToString("N2");
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[5].Text = _abril.ToString("N2");
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[6].Text = _mayo.ToString("N2");
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[7].Text = _junio.ToString("N2");
                    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[8].Text = _julio.ToString("N2");
                    e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[9].Text = _agosto.ToString("N2");
                    e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[10].Text = _septiembre.ToString("N2");
                    e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[11].Text = _octubre.ToString("N2");
                    e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[12].Text = _noviembre.ToString("N2");
                    e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[13].Text = _diciembre.ToString("N2");
                    e.Row.Cells[13].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;
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