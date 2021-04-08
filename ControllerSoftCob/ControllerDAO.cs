namespace ControllerSoftCob
{
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Windows.Forms;
    public class ControllerDAO
    {
        #region Variables
        bool? _usuarioLogueado = false;
        string _terminal = "", _mensaje = "", _strmenp = "", _strnombre = "", _strnomobj = "", _strres = "", _strfullPath = "", _strconect = "",
             _nomtarea = "", _milogin = "";
        SoftCob_USUARIO _user = new SoftCob_USUARIO();
        DataSet _dts = new DataSet();        
        SqlDataAdapter _dta = new SqlDataAdapter();
        List<CatalogosDTO> _catalogo = new List<CatalogosDTO>();
        SoftCobEntities _db = new SoftCobEntities();
        List<ParametroDetalle> _detalle = new List<ParametroDetalle>();
        public string gstrusuario = "";
        public string gstrterminal = "";
        public Form objform = new Form();
        private string[,] stramenus = new string[1, 3];
        private string[,] stramenustemp;
        int intcontmen = 0, intindex = 0, inttam = 0;
        int? _codigo = 0;
        int _xcodigo = 0;
        #endregion

        #region Procedimientos y Funciones USUARIOS
        public int FunGetLogin(int emprcodigo, string usuario, string password)
        {
            using (SoftCobEntities _db = new SoftCobEntities())
            {
                _user = _db.SoftCob_USUARIO.Include("SoftCob_PERFIL").Where(x => x.empr_codigo == emprcodigo && x.usua_login == usuario
                    && x.usua_estado == true && x.SoftCob_PERFIL.perf_estado == true).FirstOrDefault();
            }

            if (_user == null) return 0;
            if (new FuncionesDAO().FunDesencripta(_user.usua_password) == password) return _user.USUA_CODIGO;
            else return 0;
        }

        public SoftCob_USUARIO FunGetUsuario(string login)
        {
            try
            {
                using (SoftCobEntities db = new SoftCobEntities())
                {
                    _user = db.SoftCob_USUARIO.Include("SoftCob_PERFIL").Where(x => x.usua_login == login && x.usua_estado == true && 
                    x.SoftCob_PERFIL.perf_estado == true).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _user;
        }

        public bool FunUsuarioLogeado(int usuacodigo)
        {
            try
            {
                using (SoftCobEntities db = new SoftCobEntities())
                {
                    _usuarioLogueado = db.SoftCob_USUARIO.Where(x => x.USUA_CODIGO == usuacodigo && x.usua_estado == true)
                        .FirstOrDefault().usua_statuslogin;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _usuarioLogueado ?? false;
        }

        public bool FunTerminalLogueada(int usucodigo)
        {
            try
            {
                using (SoftCobEntities db = new SoftCobEntities())
                {
                    _terminal = db.SoftCob_USUARIO.Where(x => x.USUA_CODIGO == usucodigo && x.usua_estado == true).FirstOrDefault().usua_terminallogin;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (_terminal == "") return false;
            else return true;
        }

        public SoftCob_USUARIO FunGetUsuarioPorID(int usuacodigo)
        {
            try
            {
                using (SoftCobEntities db = new SoftCobEntities())
                {
                    _user = db.SoftCob_USUARIO.Include("SoftCob_PERFIL").Where(x => x.USUA_CODIGO == usuacodigo && x.usua_estado == true && 
                    x.SoftCob_PERFIL.perf_estado == true).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _user;
        }

        public void FunUpdateLogueo(SoftCob_USUARIO _user)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_USUARIO _original = _db.SoftCob_USUARIO.Where(x => x.USUA_CODIGO == _user.USUA_CODIGO).FirstOrDefault();
                    _db.SoftCob_USUARIO.Attach(_original);
                    _original.usua_statuslogin = _user.usua_statuslogin;
                    _original.usua_terminallogin = _user.usua_terminallogin;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SoftCob_USUARIO> FunGetUsuariosAdmin()
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    return _db.SoftCob_USUARIO.Include("SoftCob_DEPARTAMENTO").Include("SoftCob_PERFIL").OrderBy(x => x.usua_apellidos).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SoftCob_USUARIO FunConsultarUsuarioPorCodigo(int usuacodigo)
        {
            SoftCob_USUARIO _user = new SoftCob_USUARIO();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _user = _db.SoftCob_USUARIO.Where(x => x.USUA_CODIGO == usuacodigo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _user;
        }

        public string FunConsultaLogin(string _login, int _emprcodigo)
        {
            List<SoftCob_USUARIO> _users = new List<SoftCob_USUARIO>();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _users = _db.SoftCob_USUARIO.Where(x => x.usua_login == _login && x.empr_codigo == _emprcodigo).ToList();
                    if (_users == null || _users.Count == 0) _mensaje = "";
                    else
                        _mensaje = _db.SoftCob_USUARIO.Where(x => x.usua_login == _login).FirstOrDefault().usua_login;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _mensaje;
        }

        public void FunCrearUsuario(SoftCob_USUARIO _user)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_USUARIO.Add(_user);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex /*DbEntityValidationException ex*/)
            {
                //foreach (var validationErrors in ex.EntityValidationErrors)
                //{
                //    foreach (var validationError in validationErrors.ValidationErrors)
                //    {
                //        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                //    }
                //}
                throw ex;
            }
        }

        public void FunEditarUsuario(SoftCob_USUARIO _user)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_USUARIO _original = _db.SoftCob_USUARIO.Where(x => x.USUA_CODIGO == _user.USUA_CODIGO).FirstOrDefault();
                    _db.SoftCob_USUARIO.Attach(_original);
                    _original.USUA_CODIGO = _user.USUA_CODIGO;
                    _original.PERF_CODIGO = _user.PERF_CODIGO;
                    _original.DEPA_CODIGO = _user.DEPA_CODIGO;
                    _original.usua_tipousuario = _user.usua_tipousuario;
                    _original.usua_nombres = _user.usua_nombres;
                    _original.usua_apellidos = _user.usua_apellidos;
                    _original.usua_login = _user.usua_login;
                    _original.usua_password = _user.usua_password;
                    _original.usua_estado = _user.usua_estado;
                    _original.usua_permisosespeciales = _user.usua_permisosespeciales;
                    _original.usua_caducapass = _user.usua_caducapass;
                    _original.usua_fechacaduca = _user.usua_fechacaduca;
                    _original.usua_cambiarpass = _user.usua_cambiarpass;
                    _original.usua_permisosespeciales = _user.usua_permisosespeciales;
                    _original.usua_logobienvenida = _user.usua_logobienvenida;
                    _original.usua_logomenu = _user.usua_logomenu;
                    _original.usua_mail = _user.usua_mail;
                    _original.usua_celular = _user.usua_celular;
                    _original.usua_auxv1 = _user.usua_auxv1;
                    _original.usua_auxv2 = _user.usua_auxv2;
                    _original.usua_auxv3 = _user.usua_auxv3;
                    _original.usua_auxv4 = _user.usua_auxv4;
                    _original.usua_auxv5 = _user.usua_auxv5;
                    _original.usua_auxi1 = _user.usua_auxi1;
                    _original.usua_auxi2 = _user.usua_auxi2;
                    _original.usua_auxi3 = _user.usua_auxi3;
                    _original.usua_auxi4 = _user.usua_auxi4;
                    _original.usua_auxi5 = _user.usua_auxi5;
                    _original.usua_fum = _user.usua_fum;
                    _original.usua_uum = _user.usua_uum;
                    _original.usua_tum = _user.usua_tum;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunConsultarMenuPorUsuario(int tipo, int usuacodigo, int emprcodigo, string auxv1, string auxv2,
            int auxi1, int auxi2,
            string conexion)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conexion))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = con;
                        comm.CommandTimeout = 9000;
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = "sp_ConsultaMenu";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_usuacodigo", usuacodigo);
                        comm.Parameters.AddWithValue("@in_emprcodigo", emprcodigo);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        _dta.SelectCommand = comm;
                        _dta.Fill(_dts);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _dts;
        }
        public int FunGetContador(string _login, int _emprcodigo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    return _db.SoftCob_USUARIO.Where(x => x.usua_login == _login
                    && x.empr_codigo == _emprcodigo).FirstOrDefault().usua_contador ?? 0;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void FunChangePassword(SoftCob_USUARIO user)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_USUARIO _original = _db.SoftCob_USUARIO.Where(x => x.USUA_CODIGO == user.USUA_CODIGO).FirstOrDefault();
                    _db.SoftCob_USUARIO.Attach(_original);
                    _original.usua_password = user.usua_password;
                    _original.usua_fum = user.usua_fum;
                    _original.usua_uum = user.usua_uum;
                    _original.usua_tum = user.usua_tum;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int FunGetGestor(int _usucodigo)
        {
            List<SoftCob_GESTOR_SUPERVISOR> _usua = new List<SoftCob_GESTOR_SUPERVISOR>();

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                _usua = _db.SoftCob_GESTOR_SUPERVISOR.Where(u => u.USUA_CODIGO == _usucodigo).ToList();

                if (_usua == null || _usua.Count == 0) _codigo = 0;
                else
                    _codigo = _db.SoftCob_GESTOR_SUPERVISOR.Where(u => u.USUA_CODIGO == _usucodigo).FirstOrDefault().gest_auxi1;
            }

            return _codigo ?? 0;
        }
        public void FunCrearLogueoTiempos(SoftCob_LOGUEO_TIEMPOS _logueo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_LOGUEO_TIEMPOS.Add(_logueo);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetUsuarioSinAsignar()
        {
            List<SoftCob_USUARIO> _usuar = null;

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                _usuar = _db.SoftCob_USUARIO.Where(u => u.usua_estado && u.empl_codigo == 0 && u.USUA_CODIGO != 1).OrderBy(x => 
                x.usua_nombres).ToList();
            }

            _catalogo.Add(new CatalogosDTO()
            {
                Descripcion = "",
                Codigo = "-1"
            });

            _catalogo.Add(new CatalogosDTO()
            {
                Descripcion = "--Crear Nuevo Usuario--",
                Codigo = "0"
            });

            foreach (SoftCob_USUARIO _usu in _usuar)
            {
                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = _usu.usua_nombres + ' ' + _usu.usua_apellidos,
                    Codigo = _usu.USUA_CODIGO.ToString()
                });
            }

            return new FuncionesDAO().FunCambiarDataSet(_catalogo);
        }
        public string FunConsultaLogin(string _login)
        {
            List<SoftCob_USUARIO> _users = new List<SoftCob_USUARIO>();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _users = _db.SoftCob_USUARIO.Where(u => u.usua_login == _login).ToList();

                    if (_users == null || _users.Count == 0) _milogin = "";
                    else
                        _milogin = _db.SoftCob_USUARIO.Where(u => u.usua_login == _login).FirstOrDefault().usua_login;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _milogin;
        }
        public string FunEditarUsuarioEmployee(SoftCob_USUARIO _usuario)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_USUARIO _original = _db.SoftCob_USUARIO.Where(u => u.USUA_CODIGO == _usuario.USUA_CODIGO).FirstOrDefault();
                    _db.SoftCob_USUARIO.Attach(_original);
                    _original.empl_codigo = _usuario.empl_codigo;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _mensaje;
        }
        public DataSet FunGetPerfilUsuarios(int percodigo)
        {
            try
            {
                var query = from Perfiles in _db.SoftCob_PERFIL
                            where Perfiles.PERF_CODIGO == percodigo
                            select new PerfilUsuarios
                            {
                                CrearParametro = (bool)Perfiles.perf_crearparametro,
                                ModifParametro = (bool)Perfiles.perf_modiparametro,
                                ElimiParametro = (bool)Perfiles.perf_eliminaparametro,
                                PerfActitudinal = (bool)Perfiles.perf_perfilactitudinal,
                                EstilosNegocia = (bool)Perfiles.perf_estilosnegociacion,
                                Metaprogramas = (bool)Perfiles.perf_metaprogramas,
                                Modalidades = (bool)Perfiles.perf_modalidades,
                                EstadosYo = (bool)Perfiles.perf_estadosdelyo,
                                Impulsores = (bool)Perfiles.perf_impulsores
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Procedimientos y Funciones MENU_PRINCIPAL
        public void Crea_menu(DataTable pdtDatos, ref MainMenu objmenu)
        {
            MenuItem objmenp = new MenuItem();
            MenuItem objmenh = new MenuItem();
            foreach (DataRow dr in pdtDatos.Rows)
            {
                //Creo menus principales 
                if (_strmenp != dr["MENU_CODIGO"].ToString())
                {
                    if (dr["MENU_NIVEL"].ToString() == "0")
                    {
                        _strmenp = dr["MENU_CODIGO"].ToString();
                        objmenp = new MenuItem();
                        intindex = objmenu.MenuItems.Add(objmenp);
                        objmenp.Text = dr["MENU_DESCRIPCION"].ToString();
                        Ingresamenu(intindex, _strmenp, "");
                    }
                    else
                    {
                        _strmenp = dr["MENU_CODIGO"].ToString();
                        objmenp = new MenuItem();
                        intindex = int.Parse(Buscamenu(dr["MEN_PADRE"].ToString(), 1, 0));
                        intindex = objmenu.MenuItems[intindex].MenuItems.Add(objmenp);
                        objmenp.Text = dr["MENU_DESCRIPCION"].ToString();
                        Ingresamenu(intindex, _strmenp, "");
                    }
                }
                //Creo tareas de menu 
                objmenh = new MenuItem();
                intindex = objmenp.MenuItems.Add(objmenh);
                objmenh.Text = dr["TARE_DESCRIPCION"].ToString();
                Ingresamenu(intindex, dr["TARE_DESCRIPCION"].ToString(), dr["TARE_PROGRAMA"].ToString());
                objmenh.Click += Click_menus;
            }
            _strmenp = "alir";
            objmenp = new MenuItem();
            intindex = objmenu.MenuItems.Add(objmenp);
            objmenp.Text = "Salir";
            Ingresamenu(intindex, _strmenp, "");
            //Creo Salida
            //Creo tareas de menu 
            objmenh = new MenuItem();
            intindex = objmenp.MenuItems.Add(objmenh);
            objmenh.Text = "Salir de SICAFM";
            Ingresamenu(intindex, "Salir de SICAFM", "UnLoad");
            objmenh.Click += Click_menus;
        }

        public void Crea_menuweb(DataTable pdtDatos, ref System.Web.UI.WebControls.TreeView objmenu)
        {
            System.Web.UI.WebControls.TreeNode objmenp = new System.Web.UI.WebControls.TreeNode();
            System.Web.UI.WebControls.TreeNode objmenh = new System.Web.UI.WebControls.TreeNode();

            _strmenp = ""; /*strpadre = "";*/
            objmenu.Nodes.Clear();
            intindex = 0; /*intmenupadre = 0;*/
            foreach (DataRow dr in pdtDatos.Rows)
            {
                if (_strmenp != dr["MenuCodigo"].ToString())
                {
                    if ((dr["MenuNivel"].ToString() == "0"))
                    {
                        objmenu.SelectedNodeChanged += new EventHandler(this.Click_menuweb);
                        _strmenp = dr["MenuCodigo"].ToString();
                        objmenp = new System.Web.UI.WebControls.TreeNode();
                        objmenu.Nodes.Add(objmenp);
                        intindex = objmenp.ChildNodes.IndexOf(objmenp);
                        objmenp.Text = dr["MenuDescripcion"].ToString();
                        objmenu.SelectedNodeChanged += new EventHandler(this.Click_menuweb);
                        objmenp.Collapse();
                        Ingresamenu(intindex, _strmenp, "");
                    }
                    else
                    {
                        objmenu.SelectedNodeChanged += new EventHandler(this.Click_menuweb);
                        _strmenp = dr["MenuCodigo"].ToString();
                        objmenp = new System.Web.UI.WebControls.TreeNode();
                        intindex = int.Parse(Buscamenu(dr["MepaCodigo"].ToString(), 1, 0));
                        objmenu.Nodes[intindex].ChildNodes.Add(objmenp);
                        intindex = objmenp.ChildNodes.IndexOf(objmenp);
                        objmenp.Text = dr["MepaDescripcion"].ToString();
                        objmenu.SelectedNodeChanged += new EventHandler(this.Click_menuweb);
                        objmenp.Collapse();
                    }

                }
                //Creo tareas de menu 
                objmenu.SelectedNodeChanged += new EventHandler(this.Click_menuweb);
                objmenh = new System.Web.UI.WebControls.TreeNode();
                objmenp.ChildNodes.Add(objmenh);
                intindex = objmenp.ChildNodes.IndexOf(objmenh);
                objmenh.Text = dr["TareDescripcion"].ToString();
                objmenh.SelectAction = System.Web.UI.WebControls.TreeNodeSelectAction.Select;
                if (dr["TarePrograma"].ToString() != "")
                {
                    objmenh.NavigateUrl = dr["TarePrograma"].ToString();
                    objmenh.Target = dr["Pantalla"].ToString(); //"CENTRO";
                    objmenu.SelectedNodeChanged += new EventHandler(this.Click_menuweb);
                }
            }
        }

        private void Ingresamenu(int intindex, string strnombre, string strproceso)
        {
            try
            {
                if (intcontmen > 0)
                {
                    stramenustemp = new string[intcontmen + 1, 3];
                    Array.Copy(stramenus, stramenustemp, Math.Min(stramenus.Length, stramenustemp.Length));
                    stramenus = stramenustemp;
                }

                stramenus[intcontmen, 0] = intindex.ToString();
                stramenus[intcontmen, 1] = strnombre;
                stramenus[intcontmen, 2] = strproceso;
                intcontmen = intcontmen + 1;
            }
            catch
            {
                throw;
            }
        }

        private string Buscamenu(string strdato, int intcampo, int intretorno)
        {
            inttam = 0;
            _strres = "";
            try
            {
                while (inttam < intcontmen)
                {

                    if (stramenus[inttam, intcampo] == strdato)
                    {
                        _strres = stramenus[inttam, intretorno].ToString();
                        inttam = stramenus.Length;
                    }
                    else
                    {
                        inttam = inttam + 1;
                    }
                }
                return _strres;
            }
            catch
            {
                return "-1";
            }
        }

        private void Click_menus(object sender, EventArgs e)
        {
            _strnombre = "";
            _strnomobj = "";
            _strres = "";
            _strfullPath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            _strconect = _strfullPath.Substring(0, _strfullPath.LastIndexOf("\\"));
            try
            {
                _strnombre = "";
                if (sender is MenuItem)
                {
                    _strnombre = ((MenuItem)sender).Text;
                }

                if (_strnombre == "Salir ")
                {
                    objform.Close();
                }
                else
                {
                    object objUser;
                    _strnomobj = Buscamenu(_strnombre, 1, 2);
                    Type oType = Type.GetTypeFromProgID(_strnomobj);
                    objUser = Activator.CreateInstance(oType);
                    oType.InvokeMember("Main", System.Reflection.BindingFlags.InvokeMethod, null, objform, null);
                    objform.Show();
                }
            }
            catch (Exception er)
            {
                _strres = er.Message;
            }
        }

        private void Click_menuweb(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.TreeView objtrvmenu = new System.Web.UI.WebControls.TreeView();
            if (sender is System.Web.UI.WebControls.TreeView)
            {
                objtrvmenu = (System.Web.UI.WebControls.TreeView)sender;
                if (objtrvmenu.SelectedNode.NavigateUrl == "")
                {
                    objtrvmenu.CollapseAll();
                    objtrvmenu.SelectedNode.Expand();
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones TAREA
        public List<MenuNewDTO> FunGetTareas(int emprcodigo)
        {
            try
            {
                var query = from tar in _db.SoftCob_TAREA
                            where tar.empr_codigo == emprcodigo
                            orderby tar.tare_descripcion
                            select new MenuNewDTO
                            {
                                Codigo = tar.TARE_CODIGO,
                                Descripcion = tar.tare_descripcion,
                                RutaPagina = tar.tare_programa,
                                Estado = tar.tare_estado ? "Activo" : "Inactivo",
                                Selecc = "NO",
                                Orden = (int)tar.tare_orden
                            };
                return query.ToList().OrderBy(o => o.Orden).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SoftCob_TAREA> FunGetTareaAdmin()
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    return _db.SoftCob_TAREA.OrderBy(t => t.tare_orden).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SoftCob_TAREA FunGetTareaPorCodigo(int _codigo)
        {
            SoftCob_TAREA _tarea = new SoftCob_TAREA();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    return _db.SoftCob_TAREA.Where(t => t.TARE_CODIGO == _codigo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FunCrearTarea(SoftCob_TAREA _tarea)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_TAREA.Add(_tarea);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FunUpdateTarea(SoftCob_TAREA _tarea)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_TAREA _original = _db.SoftCob_TAREA.Where(t => t.TARE_CODIGO == _tarea.TARE_CODIGO).FirstOrDefault();
                    _db.SoftCob_TAREA.Attach(_original);
                    _original.TARE_CODIGO = _tarea.TARE_CODIGO;
                    _original.tare_descripcion = _tarea.tare_descripcion;
                    _original.tare_programa = _tarea.tare_programa;
                    _original.tare_estado = _tarea.tare_estado;
                    _original.tare_auxv1 = _tarea.tare_auxv1;
                    _original.tare_auxv2 = _tarea.tare_auxv2;
                    _original.tare_auxv3 = _tarea.tare_auxv3;
                    _original.tare_auxi1 = _tarea.tare_auxi1;
                    _original.tare_auxi2 = _tarea.tare_auxi2;
                    _original.tare_auxi3 = _tarea.tare_auxi3;
                    _original.tare_fum = _tarea.tare_fum;
                    _original.tare_uum = _tarea.tare_uum;
                    _original.tare_tum = _tarea.tare_tum;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string FunConsultaTarea(string _tarea, int emprcodigo)
        {
            List<SoftCob_TAREA> _listareas = new List<SoftCob_TAREA>();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _listareas = _db.SoftCob_TAREA.Where(t => t.empr_codigo==emprcodigo && t.tare_descripcion == _tarea).ToList();

                    if (_listareas == null || _listareas.Count == 0) _nomtarea = "";
                    else _nomtarea = _tarea;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _nomtarea;
        }

        public int FunGetOrdenTarea()
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    return _db.SoftCob_TAREA.Max(t => (int)t.tare_orden + 1);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Procedimientos y Funciones (MENU)
        public int FunCrearMenu(SoftCob_MENU _menu)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_MENU.Add(_menu);
                    _db.SaveChanges();
                    _xcodigo = _menu.MENU_CODIGO;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _xcodigo;
        }
        public int FunGetOrdenMenu(int _emprcodigo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    return _db.SoftCob_MENU.Where(x => x.empr_codigo == _emprcodigo).Max(x => x.menu_orden + 1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string FunCrearMenuTarea(List<MenuNew> _menutarea, SoftCob_MENU _menu, int _emprcodigo)
        {
            try
            {
                if (FunConsultaMenu(_menu.menu_descripcion, _emprcodigo) > 0) _mensaje = "Menú ya existe Creado..!";
                else
                {
                    SoftCob_MENU _newmenu = new SoftCob_MENU();
                    {
                        _newmenu.menu_descripcion = _menu.menu_descripcion;
                        _newmenu.menu_nivel = _menu.menu_nivel;
                        _newmenu.menu_estado = _menu.menu_estado;
                        _newmenu.menu_orden = _menu.menu_orden;
                        _newmenu.empr_codigo = _menu.empr_codigo;
                        _newmenu.menu_auxv1 = _menu.menu_auxv1;
                        _newmenu.menu_auxv2 = _menu.menu_auxv2;
                        _newmenu.menu_auxv3 = _menu.menu_auxv3;
                        _newmenu.menu_auxi1 = _menu.menu_auxi1;
                        _newmenu.menu_auxi2 = _menu.menu_auxi2;
                        _newmenu.menu_auxi3 = _menu.menu_auxi3;
                        _newmenu.menu_fechacreacion = _menu.menu_fechacreacion;
                        _newmenu.menu_usuariocreacion = _menu.menu_usuariocreacion;
                        _newmenu.menu_terminalcreacion = _menu.menu_terminalcreacion;
                        _newmenu.menu_fum = _menu.menu_fum;
                        _newmenu.menu_uum = _menu.menu_uum;
                        _newmenu.menu_tum = _menu.menu_tum;
                        _newmenu.SoftCob_MENU_TAREA = new System.Data.Objects.DataClasses.EntityCollection<SoftCob_MENU_TAREA>();
                    }
                    _menutarea.ForEach(mta => _newmenu.SoftCob_MENU_TAREA.Add(new SoftCob_MENU_TAREA()
                    {
                        MENU_CODIGO = mta.MenCodigo,
                        TARE_CODIGO = mta.TarCodigo,
                        empr_codigo = mta.EmprCodigo,
                        meta_estado = true,
                        meta_orden = mta.MenOrden,
                        meta_auxv1 = "",
                        meta_auxv2 = "",
                        meta_auxv3 = "",
                        meta_auxi1 = 0,
                        meta_auxi2 = 0,
                        meta_auxi3 = 0,
                        meta_fechacreacion = mta.FechaCreacion,
                        meta_usuariocreacion = mta.UsuarioCreacion,
                        meta_terminalcreacion = mta.TerminalCreacion,
                        meta_fum = mta.Fum,
                        meta_tum = mta.Tum,
                        meta_uum = mta.Uum,
                    }));

                    FunCrearNewMenuTarea(_newmenu);
                    _mensaje = "";
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();

            }
            return _mensaje;
        }

        public int FunConsultaMenu(string _menu, int _emprcodigo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_MENU> _listamenu = _db.SoftCob_MENU.Where(x => x.menu_descripcion == _menu
                    && x.empr_codigo == _emprcodigo).ToList();
                    return _listamenu.Count();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FunCrearNewMenuTarea(SoftCob_MENU _menutarea)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_MENU.Add(_menutarea);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SoftCob_MENU FunGetMenuPorID(int _codigomenu, int _emprcodigo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    return _db.SoftCob_MENU.Where(x => x.MENU_CODIGO == _codigomenu && x.empr_codigo == _emprcodigo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MenuNewDTO> FunGetMenuNewEdit(int _codigomenu, int _emprcodigo)
        {
            try
            {
                var query = from tar in _db.SoftCob_TAREA
                            join mta in _db.SoftCob_MENU_TAREA on tar.TARE_CODIGO equals mta.TARE_CODIGO
                            where mta.MENU_CODIGO == _codigomenu && mta.empr_codigo == _emprcodigo && tar.empr_codigo == _emprcodigo
                            select new MenuNewDTO
                            {
                                Codigo = tar.TARE_CODIGO,
                                Descripcion = tar.tare_descripcion,
                                RutaPagina = tar.tare_programa,
                                Estado = tar.tare_estado ? "Activo" : "Inactivo",
                                Selecc = "SI",
                                Orden = mta.meta_orden
                            };

                var query_1 = from tar in _db.SoftCob_TAREA
                              where !(from mta in _db.SoftCob_MENU_TAREA
                                      where mta.MENU_CODIGO == _codigomenu && mta.empr_codigo == _emprcodigo
                                      select mta.TARE_CODIGO).Contains(tar.TARE_CODIGO) && tar.empr_codigo == _emprcodigo
                              select new MenuNewDTO
                              {
                                  Codigo = tar.TARE_CODIGO,
                                  Descripcion = tar.tare_descripcion,
                                  RutaPagina = tar.tare_programa,
                                  Estado = tar.tare_estado ? "Activo" : "Inactivo",
                                  Selecc = "NO",
                                  Orden = 50000
                              };
                return query.Union(query_1).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string FunUpdateMenu(SoftCob_MENU _menuedit)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_MENU _original = _db.SoftCob_MENU.Where(m => m.MENU_CODIGO == _menuedit.MENU_CODIGO).FirstOrDefault();
                    _db.SoftCob_MENU.Attach(_original);
                    _original.menu_descripcion = _menuedit.menu_descripcion;
                    _original.menu_estado = _menuedit.menu_estado;
                    _original.menu_fum = _menuedit.menu_fum;
                    _original.menu_uum = _menuedit.menu_uum;
                    _original.menu_tum = _menuedit.menu_tum;
                    _db.SaveChanges();
                    _mensaje = "";
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _mensaje;
        }

        #endregion

        #region Procedimientos y Funciones (PERFIL)
        public DataSet FunGetPerfil()
        {
            List<SoftCob_PERFIL> _perfil = null;

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                _perfil = _db.SoftCob_PERFIL.Where(p => p.perf_estado == true).OrderBy(p => p.perf_descripcion).ToList();
            }

            _catalogo.Add(new CatalogosDTO()
            {
                Descripcion = "--Seleccione Perfil--",
                Codigo = "0"
            });

            foreach (SoftCob_PERFIL _per in _perfil)
            {
                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = _per.perf_descripcion,
                    Codigo = _per.PERF_CODIGO.ToString()
                });
            }

            _dts = new FuncionesDAO().FunCambiarDataSet(_catalogo);
            return _dts;
        }
        public SoftCob_PERFIL FunGetPerfilPorID(int _codigoperfil, int _emprcodigo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    return _db.SoftCob_PERFIL.Where(x => x.PERF_CODIGO == _codigoperfil && x.empr_codigo == _emprcodigo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int FunConsultaPerfil(string _perfil, int _emprcodigo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_PERFIL> _listaperfil = _db.SoftCob_PERFIL.Where(x => x.perf_descripcion == _perfil
                    && x.empr_codigo == _emprcodigo).ToList();
                    return _listaperfil.Count();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int FunCrearPerfil(SoftCob_PERFIL _perfil)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_PERFIL.Add(_perfil);
                    _db.SaveChanges();
                    _xcodigo = _perfil.PERF_CODIGO;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _xcodigo;
        }
        public string FunUpdatePerfil(SoftCob_PERFIL _perfiles)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_PERFIL _original = _db.SoftCob_PERFIL.Where(x => x.PERF_CODIGO == _perfiles.PERF_CODIGO).FirstOrDefault();
                    _db.SoftCob_PERFIL.Attach(_original);
                    _original.perf_descripcion = _perfiles.perf_descripcion;
                    _original.perf_observacion = _perfiles.perf_observacion;
                    _original.perf_estado = _perfiles.perf_estado;
                    _original.perf_crearparametro = _perfiles.perf_crearparametro;
                    _original.perf_modiparametro = _perfiles.perf_modiparametro;
                    _original.perf_eliminaparametro = _perfiles.perf_eliminaparametro;
                    _original.perf_auxb1 = _perfiles.perf_auxb1;
                    _original.perf_auxb2 = _perfiles.perf_auxb2;
                    _original.perf_auxb3 = _perfiles.perf_auxb3;
                    _original.perf_auxi1 = _perfiles.perf_auxi1;
                    _original.perf_auxi2 = _perfiles.perf_auxi2;
                    _original.perf_auxi3 = _perfiles.perf_auxi3;
                    _original.perf_auxv1 = _perfiles.perf_auxv1;
                    _original.perf_auxv2 = _perfiles.perf_auxv2;
                    _original.perf_auxv3 = _perfiles.perf_auxv3;
                    _original.perf_fum = _perfiles.perf_fum;
                    _original.perf_uum = _perfiles.perf_uum;
                    _original.perf_tum = _perfiles.perf_tum;
                    _db.SaveChanges();
                    _mensaje = "";
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _mensaje;
        }
        #endregion

        #region Procedimientos y Funciones (DEPARTAMENTO)
        public DataSet FunGetDepartamento()
        {
            List<SoftCob_DEPARTAMENTO> _depar = null;

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                _depar = _db.SoftCob_DEPARTAMENTO.Where(d => d.depa_estado == true).OrderBy(d => d.depa_descripcion).ToList();
            }

            _catalogo.Add(new CatalogosDTO()
            {
                Descripcion = "--Seleccione Departamento--",
                Codigo = "0"
            });

            foreach (SoftCob_DEPARTAMENTO _dep in _depar)
            {
                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = _dep.depa_descripcion,
                    Codigo = _dep.DEPA_CODIGO.ToString()
                });
            }

            _dts = new FuncionesDAO().FunCambiarDataSet(_catalogo);
            return _dts;
        }
        public SoftCob_DEPARTAMENTO FunGetDepartamentoPorCodigo(int _codigodepa, int _emprcodigo)
        {
            SoftCob_DEPARTAMENTO _datos = new SoftCob_DEPARTAMENTO();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _datos = _db.SoftCob_DEPARTAMENTO.Where(x => x.DEPA_CODIGO == _codigodepa && x.empr_codigo == _emprcodigo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _datos;
        }
        public string FunConsultaDepartamento(string _descripcion, int _emprcodigo)
        {

            List<SoftCob_DEPARTAMENTO> _datos = new List<SoftCob_DEPARTAMENTO>();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _datos = _db.SoftCob_DEPARTAMENTO.Where(x => x.depa_descripcion == _descripcion && x.empr_codigo == _emprcodigo).ToList();
                    if (_datos == null || _datos.Count == 0) _mensaje = "";
                    else
                        _mensaje = _db.SoftCob_DEPARTAMENTO.Where(x => x.depa_descripcion == _descripcion
                        && x.empr_codigo == _emprcodigo).FirstOrDefault().depa_descripcion;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _mensaje;
        }

        public void FunCrearDepartamento(SoftCob_DEPARTAMENTO _datos)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_DEPARTAMENTO.Add(_datos);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FunEditarDepartamento(SoftCob_DEPARTAMENTO _datos, int _emprcodigo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_DEPARTAMENTO _original = _db.SoftCob_DEPARTAMENTO.Where(x => x.DEPA_CODIGO == _datos.DEPA_CODIGO
                    && x.empr_codigo == _emprcodigo).FirstOrDefault();
                    _db.SoftCob_DEPARTAMENTO.Attach(_original);
                    _original.depa_descripcion = _datos.depa_descripcion;
                    _original.depa_estado = _datos.depa_estado;
                    _original.depa_auxv1 = _datos.depa_auxv1;
                    _original.depa_auxv2 = _datos.depa_auxv2;
                    _original.depa_auxv3 = _datos.depa_auxv3;
                    _original.depa_auxi1 = _datos.depa_auxi1;
                    _original.depa_auxi2 = _datos.depa_auxi2;
                    _original.depa_auxi3 = _datos.depa_auxi3;
                    _original.depa_fum = _datos.depa_fum;
                    _original.depa_uum = _datos.depa_uum;
                    _original.depa_tum = _datos.depa_tum;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Procedimientos Y Funciones (REGION)
        public DataSet FunGetProvincia()
        {
            List<SoftCob_PROVINCIA> _datos = new List<SoftCob_PROVINCIA>();

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                _datos = _db.SoftCob_PROVINCIA.Where(x => x.prov_estado == true).OrderBy(x => x.prov_nombre).ToList();
            }

            _catalogo.Add(new CatalogosDTO()
            {
                Descripcion = "--Seleccione Provincia--",
                Codigo = "0"
            });

            foreach (SoftCob_PROVINCIA _xdat in _datos)
            {
                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = _xdat.prov_nombre,
                    Codigo = _xdat.PROV_CODIGO.ToString()
                });
            }
            return new FuncionesDAO().FunCambiarDataSet(_catalogo);
        }

        public DataSet FunGetCiudadPorProvincia(int _codigo)
        {
            List<SoftCob_CIUDAD> _datos = new List<SoftCob_CIUDAD>();

            using (SoftCobEntities _db = new SoftCobEntities())
            {
               _datos = _db.SoftCob_CIUDAD.Where(x => x.ciud_estado == true && x.PROV_CODIGO == _codigo).ToList();
            }

            _catalogo.Add(new CatalogosDTO()
            {
                Descripcion = "--Seleccione Ciudad--",
                Codigo = "0"
            });

            foreach (SoftCob_CIUDAD _xdat in _datos)
            {
                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = _xdat.ciud_nombre,
                    Codigo = _xdat.CIUD_CODIGO.ToString()
                });
            }
            return new FuncionesDAO().FunCambiarDataSet(_catalogo);
        }
        public DataSet FunGetCiudadPorID(int _codprov)
        {
            List<SoftCob_CIUDAD> _ciudad = null;

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                _ciudad = _db.SoftCob_CIUDAD.Where(c => c.ciud_estado == true && c.PROV_CODIGO == _codprov).ToList();
            }

            _catalogo.Add(new CatalogosDTO()
            {
                Descripcion = "--Seleccione Ciudad--",
                Codigo = "0"
            });

            foreach (SoftCob_CIUDAD _ciu in _ciudad)
            {
                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = _ciu.ciud_nombre,
                    Codigo = _ciu.CIUD_CODIGO.ToString()
                });
            }

            return new FuncionesDAO().FunCambiarDataSet(_catalogo);
        }
        #endregion

        #region Procedimientos y Funciones (PARAMETROS)
        public string FunCrearParametro(List<ParametroNew> _listadetalle, SoftCob_PARAMETRO_CABECERA _datos)
        {
            try
            {
                if (FunConsultaParametro(_datos.para_nombre) > 0) _mensaje = "Párametro ya está creado..!";
                else
                {
                    SoftCob_PARAMETRO_CABECERA _listadatos = new SoftCob_PARAMETRO_CABECERA();
                    _listadatos.para_nombre = _datos.para_nombre;
                    _listadatos.para_descripcion = _datos.para_descripcion;
                    _listadatos.para_estado = _datos.para_estado;
                    _listadatos.para_auxv1 = _datos.para_auxv1;
                    _listadatos.para_auxiv2 = _datos.para_auxiv2;
                    _listadatos.para_auxii1 = _datos.para_auxii1;
                    _listadatos.para_auxii2 = _datos.para_auxii2;
                    _listadatos.para_fechacreacion = _datos.para_fechacreacion;
                    _listadatos.para_usuariocreacion = _datos.para_usuariocreacion;
                    _listadatos.para_terminalcreacion = _datos.para_terminalcreacion;
                    _listadatos.para_fum = _datos.para_fechacreacion;
                    _listadatos.para_uum = _datos.para_usuariocreacion;
                    _listadatos.para_tum = _datos.para_terminalcreacion;
                    _listadatos.SoftCob_PARAMETRO_DETALLE = new System.Data.Objects.DataClasses.EntityCollection<SoftCob_PARAMETRO_DETALLE>();

                    _listadetalle.ForEach(x => _listadatos.SoftCob_PARAMETRO_DETALLE.Add(new SoftCob_PARAMETRO_DETALLE()
                    {
                        PARA_CODIGO = x.Codigo,
                        pade_nombre = x.Nombre,
                        pade_valorV = x.ValorV,
                        pade_valorI = x.ValorI,
                        pade_orden = x.Orden,
                        pade_estado = x.Estado,
                        pade_auxv1 = "",
                        pade_auxv2 = "",
                        pade_auxi1 = 0,
                        pade_auxi2 = 0,
                    }));

                    FunCrearParametroNew(_listadatos);
                    _mensaje = "";
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }

            return _mensaje;
        }

        public int FunConsultaParametro(string _valor)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_PARAMETRO_CABECERA> _listadatos = _db.SoftCob_PARAMETRO_CABECERA.Where(p => p.para_nombre == _valor).ToList();
                    return _listadatos.Count();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FunCrearParametroNew(SoftCob_PARAMETRO_CABECERA _datos)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_PARAMETRO_CABECERA.Add(_datos);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string FunParametroDetalle(SoftCob_PARAMETRO_CABECERA _datos, DataSet _dts)
        {
            try
            {
                if (FunUpdateParametro(_datos) == "")
                {
                    foreach (DataRow _dr in _dts.Tables[0].Rows)
                    {
                        using (SoftCobEntities _db = new SoftCobEntities())
                        {
                            _db.FunUpdateParametroDetalle(_datos.PARA_CODIGO, int.Parse(_dr["Codigo"].ToString()), _dr["Nombre"].ToString(),
                            _dr["ValorV"].ToString(), int.Parse(_dr["ValorI"].ToString()), int.Parse(_dr["Orden"].ToString()),
                            bool.Parse(_dr["Estado"].ToString()));
                        }
                    }
                }
                _mensaje = "";
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }

            return _mensaje;
        }

        public string FunUpdateParametro(SoftCob_PARAMETRO_CABECERA _datos)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_PARAMETRO_CABECERA _original = _db.SoftCob_PARAMETRO_CABECERA.Where(p => p.PARA_CODIGO == _datos.PARA_CODIGO).FirstOrDefault();
                    _db.SoftCob_PARAMETRO_CABECERA.Attach(_original);
                    _original.para_nombre = _datos.para_nombre;
                    _original.para_descripcion = _datos.para_descripcion;
                    _original.para_estado = _datos.para_estado;
                    _original.para_auxv1 = _datos.para_auxv1;
                    _original.para_auxiv2 = _datos.para_auxiv2;
                    _original.para_auxii1 = _datos.para_auxii1;
                    _original.para_auxii2 = _datos.para_auxii2;
                    _original.para_fum = _datos.para_fum;
                    _original.para_uum = _datos.para_uum;
                    _original.para_tum = _datos.para_tum;
                    _db.SaveChanges();
                    _mensaje = "";
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }

            return _mensaje;
        }

        public DataSet FunGetParametroDetalle(string _parametro, string _descripcion, string _valor)
        {
            List<SoftCob_PARAMETRO_DETALLE> _datos = new List<SoftCob_PARAMETRO_DETALLE>();

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                _datos = _db.SoftCob_PARAMETRO_DETALLE.Where(x => x.pade_estado &&
                    x.PARA_CODIGO == x.SoftCob_PARAMETRO_CABECERA.PARA_CODIGO && x.SoftCob_PARAMETRO_CABECERA.para_nombre == _parametro
                    && x.SoftCob_PARAMETRO_CABECERA.para_estado).OrderBy(x => x.pade_nombre).ToList();
            }

            _catalogo.Add(new CatalogosDTO()
            {
                Descripcion = _descripcion,
                Codigo = "0"
            });

            if (_valor == "S")
            {
                foreach (SoftCob_PARAMETRO_DETALLE _xdat in _datos)
                {
                    _catalogo.Add(new CatalogosDTO()
                    {
                        Descripcion = _xdat.pade_nombre,
                        Codigo = _xdat.pade_valorV
                    });
                }
            }

            if (_valor == "I")
            {
                foreach (SoftCob_PARAMETRO_DETALLE _xdat in _datos)
                {
                    _catalogo.Add(new CatalogosDTO()
                    {
                        Descripcion = _xdat.pade_nombre,
                        Codigo = _xdat.pade_valorI.ToString()
                    });
                }
            }

            return new FuncionesDAO().FunCambiarDataSet(_catalogo);
        }
        public DataSet FunGetDatosParametroDet(string _paranombre)
        {
            List<SoftCob_PARAMETRO_DETALLE> _tablas = null;

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                _tablas = _db.SoftCob_PARAMETRO_DETALLE.Where(x => x.pade_estado == true && x.PARA_CODIGO == 
                x.SoftCob_PARAMETRO_CABECERA.PARA_CODIGO && x.SoftCob_PARAMETRO_CABECERA.para_nombre == _paranombre && 
                x.SoftCob_PARAMETRO_CABECERA.para_estado == true).ToList();
            }

            foreach (SoftCob_PARAMETRO_DETALLE _tab in _tablas)
            {
                _detalle.Add(new ParametroDetalle()
                {
                    Prametro = _tab.pade_nombre,
                    ValorV = _tab.pade_valorV,
                    ValorI = _tab.pade_valorI
                });
            }

            return new FuncionesDAO().FunCambiarDataSet(_detalle);
        }
        #endregion

        #region Procedimientos y Funciones CONSULTA DATOS
        public DataSet FunGetConsultasCatalogo(int _tipo, string _descripcion, int _int1, int _int2, int _int3, string _str1,
            string _str2, string _str3, string _conexion)
        {
            _dts = new ConsultaDatosDAO().FunConsultaDatos(_tipo, _int1, _int2, _int3, _str1, _str2, _str3, _conexion);

            _catalogo.Add(new CatalogosDTO()
            {
                Descripcion = _descripcion,
                Codigo = "0"
            });

            foreach (DataRow _dr in _dts.Tables[0].Rows)
            {
                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = _dr[0].ToString(),
                    Codigo = _dr[1].ToString()
                });
            }

            return new FuncionesDAO().FunCambiarDataSet(_catalogo);
        }
        public DataSet FunGetTablasBDD()
        {
            List<SoftCob_TABLAS_BDD> _tablas = null;

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                _tablas = _db.SoftCob_TABLAS_BDD.Where(t => t.tadb_estado == true).ToList();
            }

            _catalogo.Add(new CatalogosDTO()
            {
                Descripcion = "--Seleccione Tabla--",
                Codigo = ""
            });

            foreach (SoftCob_TABLAS_BDD _tab in _tablas)
            {
                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = _tab.tadb_nombre,
                    Codigo = _tab.TABD_CODIGO.ToString()
                });
            }

            return new FuncionesDAO().FunCambiarDataSet(_catalogo);
        }
        #endregion
    }
}
