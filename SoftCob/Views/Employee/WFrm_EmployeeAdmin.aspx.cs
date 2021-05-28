namespace SoftCob.Views.Employee
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_EmployeeAdmin : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        ImageButton _imgasigna = new ImageButton();
        ImageButton _imgquita = new ImageButton();
        string _codigoeployee = "0", _codigousu = "0", _mensaje = "0", _redirect = "0";
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

                Lbltitulo.Text = "Administrar Empleados";
                FunCargarMantenimiento();

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", 
                    Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
               _dts = new EmployeeDAO().FunGetEmployeeAdmin();

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    GrdvDatos.DataSource = _dts;
                    GrdvDatos.DataBind();
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
        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_NuevoEmployee.aspx?Tipo=N" + "&Codigo=0");
        }
        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgasigna = (ImageButton)(e.Row.Cells[5].FindControl("imgAsignarUsu"));
                    _imgquita = (ImageButton)(e.Row.Cells[6].FindControl("imgQuitarUsu"));

                    int valor = int.Parse(GrdvDatos.DataKeys[e.Row.RowIndex].Values["Asignado"].ToString());

                    if (valor == 0)
                    {
                        _imgasigna.ImageUrl = "~/Botones/agregar_usuario.jpg";
                        _imgasigna.Enabled = true;
                        _imgquita.ImageUrl = "~/Botones/sin_usuario.png";
                        _imgquita.Enabled = false;
                    }
                    else
                    {
                        _imgasigna.ImageUrl = "~/Botones/con_usuario.jpg";
                        _imgasigna.Enabled = false;
                        _imgquita.ImageUrl = "~/Botones/quitar_usuario.jpg";
                        _imgquita.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAsignarUsu_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigoeployee = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString();
            _codigousu = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["CodigoUsu"].ToString();
            Response.Redirect("WFrm_NuevoUsuarioEmployee.aspx?CodigoEmployee=" + _codigoeployee + "&CodigoUsuario=" + _codigousu);
        }

        protected void ImgQuitarUsu_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigoeployee = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString();
            _codigousu = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["CodigoUsu"].ToString();

            SoftCob_USUARIO _user = new SoftCob_USUARIO();
            {
                _user.USUA_CODIGO = int.Parse(_codigousu);
                _user.empl_codigo = 0;
            }

            _mensaje = new EmployeeDAO().FunEditarUsuarioEmployee(_user);
            _redirect = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Exito..");
            Response.Redirect(_redirect);
        }
        #endregion
    }
}