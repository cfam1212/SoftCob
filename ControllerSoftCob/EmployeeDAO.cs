namespace ControllerSoftCob
{
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    public class EmployeeDAO
    {
        #region Variables
        SoftCobEntities _dtb = new SoftCobEntities();
        string _mensaje = "";
        bool _existe = false;
        #endregion

        #region Procedimientos y Funciones
        public string FunCrearEmployee(SoftCob_EMPLOYEE _employee)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_EMPLOYEE.Add(_employee);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _mensaje;
        }

        public string FunDelOtrosEstudios(int _codigo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_OTROSESTUDIOS otrosestudios = _db.SoftCob_OTROSESTUDIOS.SingleOrDefault(o => o.OTES_CODIGO == _codigo);
                    if (otrosestudios != null)
                    {
                        _db.Entry(otrosestudios).State = System.Data.Entity.EntityState.Deleted;
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _mensaje = "";
        }

        public string FunDelIdiomas(int _codigo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_IDIOMAS _idiomas = _db.SoftCob_IDIOMAS.SingleOrDefault(i => i.IDIO_CODIGO == _codigo);

                    if (_idiomas != null)
                    {
                        _db.Entry(_idiomas).State = System.Data.Entity.EntityState.Deleted;
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _mensaje;
        }

        public string FunDelExperiencia(int _codigo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_EXPERIENCIA _experiencia = _db.SoftCob_EXPERIENCIA.SingleOrDefault(e => e.EXPE_CODIGO == _codigo);

                    if (_experiencia != null)
                    {
                        _db.Entry(_experiencia).State = System.Data.Entity.EntityState.Deleted;
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _mensaje;
        }

        public string FunDelRefLaboral(int _codigo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_REFERENCIASLABORALES _reflaboral = _db.SoftCob_REFERENCIASLABORALES.SingleOrDefault(l => l.RELA_CODIGO == _codigo);

                    if (_reflaboral != null)
                    {
                        _db.Entry(_reflaboral).State = System.Data.Entity.EntityState.Deleted;
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _mensaje;
        }

        public string FunDelRefPersonal(int _codigo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_REFERENCIASPERSONALES _refpersonal = _db.SoftCob_REFERENCIASPERSONALES.SingleOrDefault(p => p.REPE_CODIGO == _codigo);

                    if (_refpersonal != null)
                    {
                        _db.Entry(_refpersonal).State = System.Data.Entity.EntityState.Deleted;
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _mensaje;
        }

        public SoftCob_EMPLOYEE FunGetEmployeePorCodigo(int _codigoemployee)
        {
            SoftCob_EMPLOYEE _employee = new SoftCob_EMPLOYEE();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _employee = _db.SoftCob_EMPLOYEE.Where(e => e.EMPL_CODIGO == _codigoemployee).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _employee;
        }

        public bool FunConsultarOtrosEstudios(int _codigoemployee, int _codigootrosestudios)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_OTROSESTUDIOS> _otrosestu = _db.SoftCob_OTROSESTUDIOS.Where(o => o.EMPL_CODIGO == _codigoemployee && o.OTES_CODIGO == _codigootrosestudios).ToList();

                    if (_otrosestu.Count > 0) _existe = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _existe;
        }

        public bool FunConsultarIdiomas(int _codigoemployee, int _codigoidioma)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_IDIOMAS> _idioma = _db.SoftCob_IDIOMAS.Where(i => i.EMPL_CODIGO == _codigoemployee && i.IDIO_CODIGO == _codigoidioma).ToList();

                    if (_idioma.Count > 0) _existe = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _existe;
        }

        public bool FunConsultarExperiencia(int _codigoemployee, int _codigoexperiencia)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_EXPERIENCIA> _expe = _db.SoftCob_EXPERIENCIA.Where(e => e.EMPL_CODIGO == _codigoemployee && e.EXPE_CODIGO == _codigoexperiencia).ToList();

                    if (_expe.Count > 0) _existe = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _existe;
        }

        public bool FunConsultarRefLaboral(int _codigoemployee, int _codigoreflaboral)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_REFERENCIASLABORALES> _refl = _db.SoftCob_REFERENCIASLABORALES.Where(l => l.EMPL_CODIGO == _codigoemployee && l.RELA_CODIGO == _codigoreflaboral).ToList();

                    if (_refl.Count > 0) _existe = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _existe;
        }

        public bool FunConsultarRefPersonal(int _codigoemployee, int _codigorefpersonal)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_REFERENCIASPERSONALES> _refp = _db.SoftCob_REFERENCIASPERSONALES.Where(p => p.EMPL_CODIGO == _codigoemployee && p.REPE_CODIGO == _codigorefpersonal).ToList();

                    if (_refp.Count > 0) _existe = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _existe;
        }

        public bool FunConsultarEmpleadoPorIdentificacion(string _identificacion)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_EMPLOYEE> _emple = _db.SoftCob_EMPLOYEE.Where(e => e.empl_identificacion == _identificacion).ToList();

                    if (_emple.Count > 0) _existe = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _existe;
        }

        public string FunEditEmployee(SoftCob_EMPLOYEE _employee)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_EMPLOYEE.Add(_employee);
                    _db.Entry(_employee).State = System.Data.Entity.EntityState.Modified;

                    foreach (SoftCob_ESTUDIOS _estu in _employee.SoftCob_ESTUDIOS)
                    {
                        if (_estu.ESTU_CODIGO != 0) _db.Entry(_estu).State = System.Data.Entity.EntityState.Modified;
                        else _db.Entry(_estu).State = System.Data.Entity.EntityState.Added;
                    }

                    foreach (SoftCob_OTROSESTUDIOS _otros in _employee.SoftCob_OTROSESTUDIOS)
                    {
                        if (_otros.OTES_CODIGO == 0) _db.Entry(_otros).State = System.Data.Entity.EntityState.Added;
                    }

                    foreach (SoftCob_IDIOMAS _idiomas in _employee.SoftCob_IDIOMAS)
                    {
                        if (_idiomas.IDIO_CODIGO == 0) _db.Entry(_idiomas).State = System.Data.Entity.EntityState.Added;
                    }

                    foreach (SoftCob_EXPERIENCIA _experiencia in _employee.SoftCob_EXPERIENCIA)
                    {
                        if (_experiencia.EXPE_CODIGO == 0) _db.Entry(_experiencia).State = System.Data.Entity.EntityState.Added;
                    }

                    foreach (SoftCob_REFERENCIASLABORALES _reflaboral in _employee.SoftCob_REFERENCIASLABORALES)
                    {
                        if (_reflaboral.RELA_CODIGO == 0) _db.Entry(_reflaboral).State = System.Data.Entity.EntityState.Added;
                    }

                    foreach (SoftCob_REFERENCIASPERSONALES _refpersonal in _employee.SoftCob_REFERENCIASPERSONALES)
                    {
                        if (_refpersonal.REPE_CODIGO == 0) _db.Entry(_refpersonal).State = System.Data.Entity.EntityState.Added;
                    }

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
                _mensaje = ex.ToString();
            }
            return _mensaje;
        }

        public string FunGrabarMailPersona(int codigopers, string email)
        {
            SoftCob_PERSONA _original = new SoftCob_PERSONA();

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                _original = _db.SoftCob_PERSONA.Where(p => p.PERS_CODIGO == codigopers).FirstOrDefault();
                _original.pers_auxiv2 = email;
                _db.SaveChanges();
            }

            return "OK";
        }

        public string FunGrabarMailEmpre(int codigoclde, string email)
        {
            SoftCob_CLIENTE_DEUDOR _original = new SoftCob_CLIENTE_DEUDOR();

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                _original = _db.SoftCob_CLIENTE_DEUDOR.Where(c => c.CLDE_CODIGO == codigoclde && c.clde_estado).FirstOrDefault();
                _original.clde_auxv1 = email;
                _db.SaveChanges();
            }

            return "OK";
        }

        public string FunGrabarMailGarante(int codigogara, string email, string tipo)
        {
            SoftCob_GARANTES _original = new SoftCob_GARANTES();

            //using (SoftCobEntities _db = new SoftCobEntities())
            //{
            //    _original = _db.SoftCob_GARANTES.Where(g => g.GART_CODIGO == codigogara).FirstOrDefault();
            //    if (tipo == "P")
            //    {
            //        _original.gara_ = email;
            //    }

            //    if (tipo == "T")
            //    {
            //        _original.EMAIL_TRABAJO = email;
            //    }
            //    _db.SaveChanges();
            //}

            return "OK";
        }
        public DataSet FunGetEmployeeAdmin()
        {
            try
            {
                var query = from Employee in _dtb.SoftCob_EMPLOYEE
                            join Usuario in _dtb.SoftCob_USUARIO on Employee.EMPL_CODIGO equals Usuario.empl_codigo
                            join Departamento in _dtb.SoftCob_DEPARTAMENTO on Employee.DEPA_CODIGO equals Departamento.DEPA_CODIGO
                            select new EmployeeAdminDTO
                            {
                                CodigoUsu = Usuario.USUA_CODIGO,
                                Codigo = Employee.EMPL_CODIGO,
                                Identificacion = Employee.empl_identificacion,
                                Apellidos = Employee.empl_apellidos,
                                Nombres = Employee.empl_nombres,
                                Departamento = Departamento.depa_descripcion,
                                Estado = Employee.empl_estado ? "Activo" : "Inactivo",
                                Asignado = 1,
                                Urllink= "WFrm_NuevoEmployee.aspx?Tipo=E&Codigo="+ Employee.EMPL_CODIGO.ToString()
                            };

                var query_1 = from Employee in _dtb.SoftCob_EMPLOYEE
                              join Departamento in _dtb.SoftCob_DEPARTAMENTO on Employee.DEPA_CODIGO equals Departamento.DEPA_CODIGO
                              where !(from Usuario in _dtb.SoftCob_USUARIO
                                      where Employee.EMPL_CODIGO == Usuario.empl_codigo
                                      select Usuario.empl_codigo).Contains(Employee.EMPL_CODIGO)
                              select new EmployeeAdminDTO
                              {
                                  CodigoUsu = 0,
                                  Codigo = Employee.EMPL_CODIGO,
                                  Identificacion = Employee.empl_identificacion,
                                  Apellidos = Employee.empl_apellidos,
                                  Nombres = Employee.empl_nombres,
                                  Departamento = Departamento.depa_descripcion,
                                  Estado = Employee.empl_estado ? "Activo" : "Inactivo",
                                  Asignado = 0,
                                  Urllink = "WFrm_NuevoEmployee.aspx?Tipo=E&Codigo=" + Employee.EMPL_CODIGO.ToString()
                              };

                return new FuncionesDAO().FunCambiarDataSet(query.Union(query_1).ToList());
            }
            catch (Exception ex)
            {
                throw ex;

            }
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
        public DataSet FunGetEstudios(int codigoemployee)
        {
            try
            {
                var query = from Estudios in _dtb.SoftCob_ESTUDIOS
                            where Estudios.EMPL_CODIGO.Equals(codigoemployee)
                            select new EmployeeEstudios
                            {
                                Codigo = Estudios.ESTU_CODIGO,
                                primaria = Estudios.estu_primaria,
                                apdesde = Estudios.estu_apdesde,
                                aphasta = Estudios.estu_aphasta,
                                secundaria = Estudios.estu_secundaria,
                                asdesde = Estudios.estu_asdesde,
                                ashasta = Estudios.estu_ashasta,
                                titulos = Estudios.estu_titulosecundaria,
                                superior = Estudios.estu_superior,
                                sudesde = Estudios.estu_audesde,
                                suhasta = Estudios.estu_auhasta,
                                titulop = Estudios.estu_titulosuperior
                            };
                
                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetOtrosEstudios(int codigoemployee)
        {
            try
            {
                var query = from OtrosEstudios in _dtb.SoftCob_OTROSESTUDIOS
                            where OtrosEstudios.EMPL_CODIGO.Equals(codigoemployee)
                            select new EmployeeOtrosEstudios
                            {
                                Codigo = OtrosEstudios.OTES_CODIGO.ToString(),
                                Institucion = OtrosEstudios.otes_institucion,
                                FechaDesde = OtrosEstudios.otes_oidesde.ToString(),
                                FechaHasta = OtrosEstudios.otes_oihasta.ToString(),
                                Titulo = OtrosEstudios.otes_tituloinstituto
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetIdiomas(int codigoemployee)
        {
            try
            {
                var query = from Idiomas in _dtb.SoftCob_IDIOMAS
                            join ParaDet in _dtb.SoftCob_PARAMETRO_DETALLE on Idiomas.idio_idioma equals ParaDet.pade_valorV
                            join ParaCab in _dtb.SoftCob_PARAMETRO_CABECERA on ParaDet.PARA_CODIGO equals ParaCab.PARA_CODIGO
                            where Idiomas.EMPL_CODIGO.Equals(codigoemployee) && ParaCab.para_nombre == "IDIOMAS"
                            select new EmployeeIdiomas
                            {
                                Codigo = Idiomas.IDIO_CODIGO.ToString(),
                                Idioma = ParaDet.pade_nombre,
                                NivelH = Idiomas.idio_nivelH,
                                NivelE = Idiomas.idio_nivelE,
                                CodigoIdioma = Idiomas.idio_idioma
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetExperiencia(int codigoemployee)
        {
            try
            {
                var query = from Experiencia in _dtb.SoftCob_EXPERIENCIA
                            join ParaDet in _dtb.SoftCob_PARAMETRO_DETALLE on Experiencia.expe_motivo equals ParaDet.pade_valorV
                            join ParaCab in _dtb.SoftCob_PARAMETRO_CABECERA on ParaDet.PARA_CODIGO equals ParaCab.PARA_CODIGO
                            where Experiencia.EMPL_CODIGO.Equals(codigoemployee) && ParaCab.para_nombre == "MOTIVOS SALIDA"
                            select new EmployeeExperiencia
                            {
                                Codigo = Experiencia.EXPE_CODIGO.ToString(),
                                Empresa = Experiencia.expe_empresa,
                                FecInicio = Experiencia.expe_desde.ToString(),
                                FecFin = Experiencia.expe_hasta.ToString(),
                                Cargo = Experiencia.expe_cargo,
                                Descripcion = Experiencia.expe_descripcion,
                                Motivo = ParaDet.pade_nombre,
                                CodigoMotivo = Experiencia.expe_motivo
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetRefLaboral(int codigoemployee)
        {
            try
            {
                var query = from RefLaboral in _dtb.SoftCob_REFERENCIASLABORALES
                            where RefLaboral.EMPL_CODIGO.Equals(codigoemployee)
                            select new EmployeeRefLaboral
                            {
                                Codigo = RefLaboral.RELA_CODIGO.ToString(),
                                Nombre = RefLaboral.rela_nombre,
                                Empresa = RefLaboral.rela_empresa,
                                Cargo = RefLaboral.rela_cargo,
                                Telefono = RefLaboral.rela_telefono,
                                Celular = RefLaboral.rela_celular,
                                Email = RefLaboral.rela_email
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetRefPersonal(int codigoemployee)
        {
            try
            {
                var query = from refperso in _dtb.SoftCob_REFERENCIASPERSONALES
                            join ParaDet in _dtb.SoftCob_PARAMETRO_DETALLE on refperso.repe_parentesco equals ParaDet.pade_valorV
                            join ParaCab in _dtb.SoftCob_PARAMETRO_CABECERA on ParaDet.PARA_CODIGO equals ParaCab.PARA_CODIGO
                            where refperso.EMPL_CODIGO.Equals(codigoemployee) && ParaCab.para_nombre == "PARENTESCO"
                            select new EmpoyeeRefPersonal
                            {
                                Codigo = refperso.REPE_CODIGO.ToString(),
                                Nombre = refperso.repe_nomrefe,
                                Parentesco = ParaDet.pade_nombre,
                                Telefono = refperso.repe_telefono,
                                Celular = refperso.repe_celular,
                                CodigoParen = refperso.repe_parentesco
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
