namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    public partial class WFrm_SpeechBV : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataSet _dtsx = new DataSet();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["CodigoCEDE"] = Request["CodigoCEDE"];
                ViewState["CodigoCPCE"] = Request["CodigoCPCE"];
                Lbltitulo.Text = "<<< SPEECH DE BIENVENIDA >>>";
                FunCargaMantenimiento();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargaMantenimiento()
        {
            try
            {
                _dts = new SpeechDAO().FunGetSpeechCabecera(int.Parse(ViewState["CodigoCEDE"].ToString()),
                    int.Parse(ViewState["CodigoCPCE"].ToString()));

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    _dtsx = new ListaTrabajoDAO().FunSpeechConvert(_dts.Tables[0].Rows[0]["Speechbv"].ToString(), 0
                        , int.Parse(Session["usuCodigo"].ToString()), Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        Repeater1.DataSource = _dtsx;
                        Repeater1.DataBind();
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