namespace SoftCob.Views.Menu
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_MenuEdit : Page
    {
        #region Variables
        ImageButton _imgsubir = new ImageButton();
        ImageButton _imgbajar = new ImageButton();
        CheckBox _chkagregar = new CheckBox();
        DataSet _dts = new DataSet();
        int _contar = 0, _fila = 0, _codigotarea = 0;
        string _mensaje = ""; 
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                Lbltitulo.Text = "Editar Menú";
                ViewState["CodigoMENU"] = Request["Codigo"].ToString();
                FunCargarMantenimiento();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarMantenimiento()
        {
            SoftCob_MENU _menu = new ControllerDAO().FunGetMenuPorID(int.Parse(ViewState["CodigoMENU"].ToString()),
                int.Parse(Session["CodigoEMPR"].ToString()));
            TxtNombreMenu.Text = _menu.menu_descripcion;
            ChkEstado.Checked = _menu.menu_estado;
            ChkEstado.Text = _menu.menu_estado ? "Activo" : "Inactivo";
            ViewState["NombreMenu"] = TxtNombreMenu.Text;

            List<MenuNewDTO> _listamenuNew = new ControllerDAO().FunGetMenuNewEdit(int.Parse(ViewState["CodigoMENU"].ToString()),
                int.Parse(Session["CodigoEMPR"].ToString()));

            _dts = new FuncionesDAO().FunCambiarDataSet(_listamenuNew);
            
            DataView _dvw = _dts.Tables[0].DefaultView;
            _dvw.Sort = "Orden Asc";
            GrdvDatos.DataSource = _dvw;
            GrdvDatos.DataBind();

            GrdvDatos.UseAccessibleHeader = true;
            GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;

            _imgsubir = (ImageButton)GrdvDatos.Rows[0].Cells[3].FindControl("ImgSubirNivel");
            _imgsubir.ImageUrl = "~/Botones/desactivada_up.png";
            _imgsubir.Enabled = false;

            foreach (GridViewRow _row in GrdvDatos.Rows)
            {
                _imgsubir = _row.FindControl("ImgSubirNivel") as ImageButton;
                _imgbajar = _row.FindControl("ImgBajarNivel") as ImageButton;
                _chkagregar = _row.FindControl("ChkAgregar") as CheckBox;
                if (GrdvDatos.DataKeys[_row.RowIndex].Values["Selecc"].ToString() == "SI") _chkagregar.Checked = true;
                else _chkagregar.Checked = false;
                if (_chkagregar.Checked == false)
                {
                    _imgsubir.ImageUrl = "~/Botones/desactivada_up.png";
                    _imgsubir.Enabled = false;
                    _imgbajar.ImageUrl = "~/Botones/desactivada_down.png";
                    _imgbajar.Enabled = false;
                }
                else _fila = _row.RowIndex;
            }
            _imgbajar = (ImageButton)GrdvDatos.Rows[_fila].FindControl("imgBajarNivel");
            _imgbajar.ImageUrl = "~/Botones/desactivada_down.png";
            _imgbajar.Enabled = false;
        }
        #endregion

        #region Botones y Eventos
        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstado.Text = ChkEstado.Checked ? "Activo" : "Inactivo";
        }

        protected void ImgSubirNivel_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigotarea = int.Parse(GrdvDatos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
            _dts = new ConsultaDatosDAO().FunConsultaDatosNew(6, int.Parse(Session["CodigoEMPR"].ToString()),
                "", "", "", "", "", "", int.Parse(ViewState["CodigoMENU"].ToString()), _codigotarea, 0, 0, 0, 0,
                Session["Conectar"].ToString());
            FunCargarMantenimiento();
        }

        protected void ImgBajarNivel_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigotarea = int.Parse(GrdvDatos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
            _dts = new ConsultaDatosDAO().FunConsultaDatosNew(7, int.Parse(Session["CodigoEMPR"].ToString()),
                "", "", "", "", "", "", int.Parse(ViewState["CodigoMENU"].ToString()), _codigotarea, 0, 0, 0, 0,
                Session["Conectar"].ToString());
            FunCargarMantenimiento();
        }
        protected void ChkAgregar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkagregar = (CheckBox)(_gvrow.Cells[5].FindControl("ChkAgregar"));
                _codigotarea = int.Parse(GrdvDatos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _dts = new ConsultaDatosDAO().FunMenuEditUpdate(_chkagregar.Checked ? "SI" : "NO", int.Parse(ViewState["CodigoMENU"].ToString()),
                    _codigotarea, int.Parse(Session["CodigoEMPR"].ToString()), "", "", "", 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                    Session["MachineName"].ToString(), Session["Conectar"].ToString());
                FunCargarMantenimiento();
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
                if (string.IsNullOrEmpty(TxtNombreMenu.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese nombre del Menú..!", this);
                    return;
                }

                if (ViewState["NombreMenu"].ToString() != TxtNombreMenu.Text)
                {
                    _contar = new ControllerDAO().FunConsultaMenu(TxtNombreMenu.Text, int.Parse(Session["CodigoEMPR"].ToString()));

                    if (_contar > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Nombre del Menú ya Existe..!", this);
                        return;
                    }
                }

                SoftCob_MENU _menunew = new SoftCob_MENU();
                {
                    _menunew.MENU_CODIGO = int.Parse(ViewState["CodigoMENU"].ToString());
                    _menunew.menu_descripcion = TxtNombreMenu.Text.Trim();
                    _menunew.menu_estado = ChkEstado.Checked ? true : false;
                    _menunew.menu_fum = DateTime.Now;
                    _menunew.menu_uum = int.Parse(Session["usuCodigo"].ToString());
                    _menunew.menu_tum = Session["MachineName"].ToString();
                }

                _mensaje = new ControllerDAO().FunUpdateMenu(_menunew);
                if (_mensaje == "") Response.Redirect("WFrm_MenuAdmin.aspx?MensajeRetornado=Guardado con Éxito", true);
                else Lblerror.Text = _mensaje;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_MenuAdmin.aspx", true);
        }
        #endregion
    }
}