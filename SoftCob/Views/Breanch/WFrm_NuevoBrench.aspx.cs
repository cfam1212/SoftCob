namespace SoftCob.Views.Breanch
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_NuevoBrench : Page
    {
        #region Variables local
        DataSet dts = new DataSet();
        DataSet dtsX = new DataSet();
        DataTable dtb = new DataTable();
        ListItem itemC = new ListItem();
        DataTable dtbBrench = new DataTable();
        int maxCodigo = 0, between = 0, orden = 0, rangox = 0, codigo = 0, codigodet = 0;
        DataRow result, filagre;
        DataRow[] resultado;
        bool lexiste = false;
        string etiqueta = "";
        ImageButton ImgSubir = new ImageButton();
        ImageButton ImgEliminar = new ImageButton();
        CheckBox ChkEstado = new CheckBox();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Lbltitulo.Text = "Administrar BRENCH";
                ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];

                ViewState["CodigoBrench"] = Request["CodigoBrench"];
                FunCargarCombos(0);

                if (ViewState["CodigoBrench"].ToString() == "0")
                {
                    dtbBrench.Columns.Add("Codigo");
                    dtbBrench.Columns.Add("RangoIni");
                    dtbBrench.Columns.Add("RangoFin");
                    dtbBrench.Columns.Add("Etiqueta");
                    dtbBrench.Columns.Add("Orden");
                    dtbBrench.Columns.Add("Estado");
                    dtbBrench.Columns.Add("auxv1");
                    dtbBrench.Columns.Add("auxv2");
                    dtbBrench.Columns.Add("auxv3");
                    dtbBrench.Columns.Add("auxi1");
                    dtbBrench.Columns.Add("auxi2");
                    dtbBrench.Columns.Add("auxi3");
                    ViewState["BrenchDet"] = dtbBrench;
                    Lbltitulo.Text = "Agregar Nuevo Brench";
                }
                else
                {
                    Lbltitulo.Text = "Editar Brench";
                    DdlCedente.Enabled = false;
                    DdlCatalogo.Enabled = false;
                    lblEstado.Visible = true;
                    ChkEstadoBrench.Visible = true;
                    FunCargaMantenimiento();
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargaMantenimiento()
        {
            SoftCob_BRENCH datos = new ListaTrabajoDAO().FunGetBrenchAdminPorID(int.Parse(ViewState["CodigoBrench"].ToString()));
            DdlCedente.SelectedValue = datos.brch_cedecodigo.ToString();
            ChkEstadoBrench.Checked = datos.brch_estado;
            ChkEstadoBrench.Text = datos.brch_estado ? "Activo" : "Inactivo";
            FunCargarCombos(1);
            DdlCatalogo.SelectedValue = datos.brch_cpcecodigo.ToString();
            ViewState["CodCatalogo"] = datos.brch_cpcecodigo.ToString();
            ViewState["fechacreacion"] = datos.brch_fechacreacion.ToString();
            ViewState["usucreacion"] = datos.brch_usuariocreacion;
            ViewState["terminalcreacion"] = datos.brch_terminalcreacion;
            dts = new ListaTrabajoDAO().FunGetBrenchDet(int.Parse(ViewState["CodigoBrench"].ToString()));
            ViewState["BrenchDet"] = dts.Tables[0];
            GrdvBrench.DataSource = dts;
            GrdvBrench.DataBind();
            //if (dts.Tables[0].Rows.Count >= 1)
            //{
            //    imgSubir = (ImageButton)grdvBrench.Rows[0].Cells[4].FindControl("imgSubir");
            //    imgSubir.ImageUrl = "~/Botones/desactivada_up.png";
            //    imgSubir.Enabled = false;
            //}
        }

        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    DdlCedente.DataSource = new CedenteDAO().FunGetCedentes();
                    DdlCedente.DataTextField = "Descripcion";
                    DdlCedente.DataValueField = "Codigo";
                    DdlCedente.DataBind();

                    itemC.Text = "--Seleccione Catálago/Producto--";
                    itemC.Value = "0";
                    DdlCatalogo.Items.Add(itemC);
                    break;
                case 1:
                    DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataTextField = "CatalogoProducto";
                    DdlCatalogo.DataValueField = "CodigoCatalogo";
                    DdlCatalogo.DataBind();
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DdlCedente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dts = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                if (dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["CodigoCedente"] = DdlCedente.SelectedValue;
                    DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataTextField = "CatalogoProducto";
                    DdlCatalogo.DataValueField = "CodigoCatalogo";
                    DdlCatalogo.DataBind();
                    ViewState["CodCatalogo"] = DdlCatalogo.SelectedValue;
                }
                else
                {
                    DdlCatalogo.Items.Clear();
                    itemC.Text = "--Seleccione Catálago/Producto--";
                    itemC.Value = "0";
                    DdlCatalogo.Items.Add(itemC);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["CodCatalogo"] = DdlCatalogo.SelectedValue;
        }

        protected void ChkEstadoBrench_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstadoBrench.Text = ChkEstadoBrench.Checked ? "Activo" : "Inactivo";
        }

        protected void ImgAgregar_Click(object sender, ImageClickEventArgs e)
        {
            Lblerror.Text = "";
            try
            {
                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this);
                    return;
                }
                if (DdlCatalogo.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Catálogo..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(TxtRinicio.Text.Trim()) || TxtRinicio.Text.Trim() == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Rango Inicio..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(TxtRFin.Text.Trim()) || TxtRFin.Text.Trim() == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Rango Fin..!", this);
                    return;
                }
                if (int.Parse(TxtRFin.Text.Trim()) <= int.Parse(TxtRinicio.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Rango Final no puede ser menor o igual al Inicial..!", this);
                    return;
                }
                if (ViewState["BrenchDet"] != null)
                {
                    dtbBrench = (DataTable)ViewState["BrenchDet"];
                    if (dtbBrench.Rows.Count > 0)
                    {
                        maxCodigo = dtbBrench.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                        orden = dtbBrench.AsEnumerable()
                            .Max(row => int.Parse((string)row["Orden"]));
                    }
                    else maxCodigo = 0;
                    if (dtbBrench.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtbBrench.Rows)
                        {
                            between = new FuncionesDAO().FunBetween(int.Parse(dr[1].ToString()), int.Parse(dr[2].ToString()),
                                int.Parse(TxtRinicio.Text), int.Parse(TxtRFin.Text));
                            if (between > 0)
                            {
                                lexiste = true;
                                break;
                            }
                        }
                    }
                }
                if (lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Rango ya existe creado..!", this);
                    return;
                }
                rangox = orden + 1;
                if (orden < 10) etiqueta = "0" + rangox + ") " + TxtRinicio.Text + " a " + TxtRFin.Text;
                else etiqueta = rangox + ") " + TxtRinicio.Text + " a " + TxtRFin.Text;
                dtbBrench = (DataTable)ViewState["BrenchDet"];
                filagre = dtbBrench.NewRow();
                filagre["Codigo"] = maxCodigo + 1;
                filagre["RangoIni"] = TxtRinicio.Text.Trim();
                filagre["RangoFin"] = TxtRFin.Text.Trim();
                filagre["Etiqueta"] = etiqueta;
                filagre["Orden"] = orden + 1;
                filagre["Estado"] = "Activo";
                filagre["auxv1"] = "";
                filagre["auxv2"] = "";
                filagre["auxv3"] = "";
                filagre["auxi1"] = "0";
                filagre["auxi2"] = "0";
                filagre["auxi3"] = "0";
                dtbBrench.Rows.Add(filagre);
                dtbBrench.DefaultView.Sort = "Orden";
                ViewState["BrenchDet"] = dtbBrench;
                GrdvBrench.DataSource = dtbBrench;
                GrdvBrench.DataBind();
                TxtRinicio.Text = "";
                TxtRFin.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSubir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                //int codigoTarea = int.Parse(grdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                //int devuelto = new MenuDTO().funOrdenarMenuTarea(int.Parse(ViewState["menuCodigo"].ToString()), codigoTarea, 0);
                //funCargaMantenimiento(int.Parse(ViewState["menuCodigo"].ToString()));
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvBrench_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    ChkEstado = (CheckBox)(e.Row.Cells[3].FindControl("chkEstado"));
                    ImgEliminar = (ImageButton)(e.Row.Cells[4].FindControl("imgEliminar"));
                    codigo = int.Parse(GrdvBrench.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString());
                    dtsX = new ListaTrabajoDAO().FunGetBrenchDetPorID(int.Parse(ViewState["CodigoBrench"].ToString()), codigo);
                    if (dtsX.Tables[0].Rows.Count > 0)
                    {
                        ImgEliminar.Enabled = false;
                        ImgEliminar.ImageUrl = "~/Botones/eliminargris.png";
                        ChkEstado.Checked = dtsX.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                    }
                    else
                    {
                        ChkEstado.Checked = true;
                        ChkEstado.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModificar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this);
                    return;
                }

                if (DdlCatalogo.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Catálogo..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtRinicio.Text.Trim()) || TxtRinicio.Text.Trim() == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Rango Inicio..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtRFin.Text.Trim()) || TxtRFin.Text.Trim() == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Rango Fin..!", this);
                    return;
                }

                if (int.Parse(TxtRFin.Text.Trim()) <= int.Parse(TxtRinicio.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Rango Final no puede ser menor o igual al Inicial..!", this);
                    return;
                }

                if (ViewState["BrenchDet"] != null)
                {
                    dtbBrench = (DataTable)ViewState["BrenchDet"];
                    resultado = dtbBrench.Select("Codigo<>'" + ViewState["Codigo"].ToString() + "'");
                    foreach (DataRow dr in resultado)
                    {
                        between = new FuncionesDAO().FunBetween(int.Parse(dr["RangoIni"].ToString()), int.Parse(dr["RangoFin"].ToString()),
                            int.Parse(TxtRinicio.Text), int.Parse(TxtRFin.Text));
                        if (between > 0)
                        {
                            lexiste = true;
                            break;
                        }
                    }
                }

                if (lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Rango ya existe creado..!", this);
                    return;
                }

                etiqueta = "0" + ViewState["Orden"].ToString() + ") " + TxtRinicio.Text + " a " + TxtRFin.Text;
                dtbBrench = (DataTable)ViewState["BrenchDet"];
                result = dtbBrench.Select("Codigo='" + int.Parse(ViewState["Codigo"].ToString()) + "'").FirstOrDefault();
                result.Delete();
                dtbBrench.AcceptChanges();
                filagre = dtbBrench.NewRow();
                filagre["Codigo"] = ViewState["Codigo"].ToString();
                filagre["RangoIni"] = TxtRinicio.Text.Trim();
                filagre["RangoFin"] = TxtRFin.Text.Trim();
                filagre["Etiqueta"] = etiqueta;
                filagre["Orden"] = ViewState["Orden"].ToString();
                filagre["Estado"] = ViewState["Estado"].ToString();
                filagre["auxv1"] = "";
                filagre["auxv2"] = "";
                filagre["auxv3"] = "";
                filagre["auxi1"] = "0";
                filagre["auxi2"] = "0";
                filagre["auxi3"] = "0";
                dtbBrench.Rows.Add(filagre);
                dtbBrench.DefaultView.Sort = "Orden";
                ViewState["BrenchDet"] = dtbBrench;
                GrdvBrench.DataSource = dtbBrench;
                GrdvBrench.DataBind();
                TxtRinicio.Text = "";
                TxtRFin.Text = "";
                ImgAgregar.Enabled = true;
                ImgModificar.Enabled = false;
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
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                ChkEstado = (CheckBox)(gvRow.Cells[2].FindControl("chkEstado"));
                dtbBrench = (DataTable)ViewState["BrenchDet"];
                codigo = int.Parse(GrdvBrench.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                result = dtbBrench.Select("Codigo='" + codigo + "'").FirstOrDefault();
                result["Estado"] = ChkEstado.Checked ? "Activo" : "Inactivo";
                dtbBrench.AcceptChanges();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                codigo = int.Parse(GrdvBrench.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                //Buscar si no existe contacto agregado
                dtbBrench = (DataTable)ViewState["BrenchDet"];
                result = dtbBrench.Select("Codigo='" + codigo + "'").FirstOrDefault();
                result.Delete();
                dtbBrench.AcceptChanges();
                //reordenar
                orden = 0;
                foreach (DataRow dr in dtbBrench.Rows)
                {
                    orden++;
                    dr["Orden"] = orden;
                    if (orden < 10) etiqueta = "0" + orden + ") " + dr["RangoIni"].ToString() + " a " + dr["RangoFin"].ToString();
                    else etiqueta = orden + ") " + dr["RangoIni"].ToString() + " a " + dr["RangoFin"].ToString();
                    dr["Etiqueta"] = etiqueta;
                    dtbBrench.AcceptChanges();
                }
                ViewState["BrenchDet"] = dtbBrench;
                GrdvBrench.DataSource = dtbBrench;
                GrdvBrench.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void ImgSelecc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                foreach (GridViewRow fr in GrdvBrench.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }
                GrdvBrench.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Beige;
                ViewState["Codigo"] = int.Parse(GrdvBrench.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                dtbBrench = (DataTable)ViewState["BrenchDet"];
                result = dtbBrench.Select("Codigo='" + ViewState["Codigo"].ToString() + "'").FirstOrDefault();
                TxtRinicio.Text = result["RangoIni"].ToString();
                TxtRFin.Text = result["RangoFin"].ToString();
                ViewState["Orden"] = result["Orden"].ToString();
                ViewState["Estado"] = result["Estado"].ToString();
                ImgAgregar.Enabled = false;
                ImgModificar.Enabled = true;
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
                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this);
                    return;
                }

                if (DdlCatalogo.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Catálogo/Producto..!", this);
                    return;
                }

                dtbBrench = (DataTable)ViewState["BrenchDet"];

                if (dtbBrench.Rows.Count == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese al menos un Rango para Brench..!", this);
                    return;
                }

                System.Threading.Thread.Sleep(300);
                SoftCob_BRENCH datos = new SoftCob_BRENCH();
                {
                    datos.BRCH_CODIGO = int.Parse(ViewState["CodigoBrench"].ToString());
                    datos.brch_cedecodigo = int.Parse(DdlCedente.SelectedValue);
                    datos.brch_cpcecodigo = int.Parse(DdlCatalogo.SelectedValue);
                    datos.brch_estado = ChkEstadoBrench.Checked;
                    datos.brch_auxv1 = "";
                    datos.brch_auxv2 = "";
                    datos.brch_auxv3 = "";
                    datos.brch_auxi1 = 0;
                    datos.brch_auxi2 = 0;
                    datos.brch_auxi3 = 0;
                    datos.brch_fum = DateTime.Now;
                    datos.brch_uum = int.Parse(Session["usuCodigo"].ToString());
                    datos.brch_tum = Session["MachineName"].ToString();
                }

                if (int.Parse(ViewState["CodigoBrench"].ToString()) == 0)
                {
                    datos.brch_fechacreacion = DateTime.Now;
                    datos.brch_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                    datos.brch_terminalcreacion = Session["MachineName"].ToString();
                }
                else
                {
                    datos.brch_fechacreacion = DateTime.Parse(ViewState["fechacreacion"].ToString());
                    datos.brch_usuariocreacion = int.Parse(ViewState["usucreacion"].ToString());
                    datos.brch_terminalcreacion = ViewState["terminalcreacion"].ToString();
                }

                dtb = (DataTable)ViewState["BrenchDet"];

                if (dtb.Rows.Count > 0)
                {
                    codigo = new CedenteDAO().FunGetCodigoBrench(int.Parse(DdlCedente.SelectedValue),
                        int.Parse(DdlCatalogo.SelectedValue), int.Parse(ViewState["CodigoBrench"].ToString()));

                    List<SoftCob_BRENCHDET> datos1 = new List<SoftCob_BRENCHDET>();

                    foreach (DataRow dr in dtb.Rows)
                    {
                        codigodet = new CedenteDAO().FunGetCodigoBrenchDet(codigo, int.Parse(dr["Codigo"].ToString()));
                        datos1.Add(new SoftCob_BRENCHDET()
                        {
                            BRDE_CODIGO = codigodet,
                            BRCH_CODIGO = codigo,
                            brde_rangoinicial = int.Parse(dr["RangoIni"].ToString()),
                            brde_rangofinal = int.Parse(dr["RangoFin"].ToString()),
                            brde_etiqueta = dr["Etiqueta"].ToString(),
                            brde_orden = int.Parse(dr["Orden"].ToString()),
                            brde_estado = dr["Estado"].ToString() == "Activo" ? true : false,
                            brde_auxv1 = dr["auxv1"].ToString(),
                            brde_auxv2 = dr["auxv2"].ToString(),
                            brde_auxv3 = dr["auxv3"].ToString(),
                            brde_auxi1 = int.Parse(DdlCatalogo.SelectedValue),
                            brde_auxi2 = int.Parse(dr["auxi2"].ToString()),
                            brde_auxi3 = int.Parse(dr["auxi3"].ToString()),
                        });
                    }

                    datos.SoftCob_BRENCHDET = new List<SoftCob_BRENCHDET>();
                    foreach (SoftCob_BRENCHDET addDatos in datos1)
                    {
                        datos.SoftCob_BRENCHDET.Add(addDatos);
                    }

                    if (datos.BRCH_CODIGO == 0) new CedenteDAO().FunCrearBrench(datos);
                    else new CedenteDAO().FunEditBrench(datos);

                    Response.Redirect("WFrm_BrenchAdmin.aspx?MensajeRetornado=Guardado con Éxito", true);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_BrenchAdmin.aspx", true);
        }
        #endregion
    }
}