namespace SoftCob.Views.BPM
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ListaSeguimientoTime : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        string _codigo = "", _codigoclde = "", _codigopers = "", _estadocodigo = "", _terreno = "", _email = "", _whastapp = "",
            _terrenofin = "", _emailfin = "", _whastappfin = "", _numdocumento = "";
        ImageButton _imgterreno = new ImageButton();
        ImageButton _imgemail = new ImageButton();
        ImageButton _imgwhatsapp = new ImageButton();
        ImageButton _imgcambiar = new ImageButton();
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
                    Lbltitulo.Text = "Lista Clientes NOTIFICACIONES";
                    FunCargarMantenimiento();

                    if (Request["MensajeRetornado"] != null)
                        new FuncionesDAO().FunShowJSMessage(Request["MensajeRetornado"], this, "S", "L");
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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(191, int.Parse(Session["usuCodigo"].ToString()), 0, 0, "", "",
                    "", Session["Conectar"].ToString());

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
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgterreno = (ImageButton)(e.Row.Cells[7].FindControl("ImgTerreno"));
                    _imgemail = (ImageButton)(e.Row.Cells[8].FindControl("ImgEmail"));
                    _imgwhatsapp = (ImageButton)(e.Row.Cells[9].FindControl("ImgWhatsapp"));
                    _imgcambiar = (ImageButton)(e.Row.Cells[10].FindControl("ImgCambiar"));

                    _estadocodigo = GrdvDatos.DataKeys[e.Row.RowIndex].Values["CodigoESTA"].ToString();
                    _terreno = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Terreno"].ToString();
                    _email = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Email"].ToString();
                    _whastapp = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Whatsapp"].ToString();
                    _terrenofin = GrdvDatos.DataKeys[e.Row.RowIndex].Values["TerrenoFin"].ToString();
                    _emailfin = GrdvDatos.DataKeys[e.Row.RowIndex].Values["EmailFin"].ToString();
                    _whastappfin = GrdvDatos.DataKeys[e.Row.RowIndex].Values["WhatsappFin"].ToString();

                    switch (_estadocodigo)
                    {
                        case "CSL":
                            e.Row.Cells[3].BackColor = System.Drawing.Color.DarkOrange;
                            break;
                        case "CMI":
                            e.Row.Cells[3].BackColor = System.Drawing.Color.Red;
                            break;
                        case "CPR":
                            e.Row.Cells[3].BackColor = System.Drawing.Color.Coral;
                            break;
                        case "CGE":
                            e.Row.Cells[3].BackColor = System.Drawing.Color.Cyan;
                            break;
                        case "CRE":
                            e.Row.Cells[3].BackColor = System.Drawing.Color.Bisque;
                            break;
                        case "CCV":
                            e.Row.Cells[3].BackColor = System.Drawing.Color.AliceBlue;
                            break;
                        case "CCS":
                            e.Row.Cells[3].BackColor = System.Drawing.Color.LimeGreen;
                            break;
                        case "CNA":
                            e.Row.Cells[3].BackColor = System.Drawing.Color.Beige;
                            break;
                        case "CSV":
                            e.Row.Cells[3].BackColor = System.Drawing.Color.SeaGreen;
                            break;
                        case "CAS":
                            e.Row.Cells[3].BackColor = System.Drawing.Color.Gold;
                            break;
                    }

                    if (_terreno == "SI")
                    {
                        _imgterreno.ImageUrl = "~/Botones/casa.png";

                        if (_terrenofin == "SI") e.Row.Cells[7].BackColor = System.Drawing.Color.DarkKhaki;
                    }

                    if (_email == "SI")
                    {
                        _imgemail.ImageUrl = "~/Botones/agendarmailbg.png";

                        if (_emailfin == "SI") e.Row.Cells[8].BackColor = System.Drawing.Color.DarkKhaki;
                    }

                    if (_whastapp == "SI")
                    {
                        _imgwhatsapp.ImageUrl = "~/Botones/btnwhastapp.png";

                        if (_whastappfin == "SI") e.Row.Cells[9].BackColor = System.Drawing.Color.DarkKhaki;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDetalle_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                _codigo = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString();
                _codigoclde = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["CodigoCLDE"].ToString();
                _codigopers = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["CodigoPERS"].ToString();
                _numdocumento = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["Identificacion"].ToString();

                Response.Redirect("WFrm_SeguimientoCitacionTime.aspx?CodigoCITA=" + _codigo + "&CodigoPERS=" + _codigopers +
                    "&CodigoCLDE=" + _codigoclde + "&NumDocumento=" + _numdocumento, true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}