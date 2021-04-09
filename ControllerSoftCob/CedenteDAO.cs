namespace ControllerSoftCob
{
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    public class CedenteDAO
    {
        #region Variables
        DataSet _dts = new DataSet();
        SqlDataAdapter _dap = new SqlDataAdapter();
        SoftCobEntities _dtb = new SoftCobEntities();
        List<CatalogosDTO> _catalogo = new List<CatalogosDTO>();
        string _mensaje = "", _query = "";
        int _codigo = 0, _codcatalago = 0;
        bool _eliminar = false, _existe = false;
        #endregion

        #region Procedimientos y Funciones CEDENTE
        public DataSet FunGetCiuadesCedentes()
        {
            try
            {
                var query = (from Cedente in _dtb.SoftCob_CEDENTE
                             join Ciudad in _dtb.SoftCob_CIUDAD on Cedente.cede_ciudcod equals Ciudad.CIUD_CODIGO
                             where Cedente.cede_estado == true
                             select new CatalogosDTO
                             {
                                 Descripcion = Ciudad.ciud_nombre,
                                 Codigo = Ciudad.CIUD_CODIGO.ToString()
                             }).Distinct();

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public DataSet FunGetCedentesporIDCiudad(int _idciudad)
        {
            try
            {
                var query = from Cedente in _dtb.SoftCob_CEDENTE
                            where Cedente.cede_ciudcod.Equals(_idciudad) && Cedente.cede_estado == true
                            select new CatalogosDTO
                            {
                                Descripcion = Cedente.cede_nombre,
                                Codigo = Cedente.CEDE_CODIGO.ToString()
                            };
                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FunGetProductosporIDCedente(int _idcedente)
        {
            try
            {
                var query = from ProductoCedente in _dtb.SoftCob_PRODUCTOS_CEDENTE
                            where ProductoCedente.CEDE_CODIGO.Equals(_idcedente) && ProductoCedente.prce_estado == true
                            select new CatalogosDTO
                            {
                                Descripcion = ProductoCedente.prce_producto,
                                Codigo = ProductoCedente.PRCE_CODIGO.ToString()
                            };
                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetCatalogoProductosporIDProducto(int _idproducto)
        {
            try
            {
                var query = from CatalogoProducto in _dtb.SoftCob_CATALOGO_PRODUCTOS_CEDENTE
                            where CatalogoProducto.PRCE_CODIGO.Equals(_idproducto) && CatalogoProducto.cpce_estado == true
                            select new CatalogosDTO
                            {
                                Descripcion = CatalogoProducto.cpce_producto,
                                Codigo = CatalogoProducto.CPCE_CODIGO.ToString()
                            };
                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SoftCob_CEDENTE FunGetCedentePorID(int _codcedente)
        {
            SoftCob_CEDENTE _cedente = new SoftCob_CEDENTE();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _cedente = _db.SoftCob_CEDENTE.Where(e => e.CEDE_CODIGO == _codcedente).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _cedente;
        }
        public string FunGetNameCatalogoporID(int _codigocatalogo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _mensaje = _db.SoftCob_CATALOGO_PRODUCTOS_CEDENTE.Where(c => c.CPCE_CODIGO == _codigocatalogo).FirstOrDefault().cpce_producto;
                }
            }
            catch
            {
                _mensaje = "";
            }
            return _mensaje;
        }
        public DataSet FunGetCatalogoPorCiudad(int _codigociudad)
        {
            try
            {
                var query = from CTP in _dtb.SoftCob_CATALOGO_PRODUCTOS_CEDENTE
                            join PRC in _dtb.SoftCob_PRODUCTOS_CEDENTE on CTP.PRCE_CODIGO equals PRC.PRCE_CODIGO
                            join CED in _dtb.SoftCob_CEDENTE on PRC.CEDE_CODIGO equals CED.CEDE_CODIGO
                            where CED.cede_ciudcod.Equals(_codigociudad) && CTP.cpce_estado == true && PRC.prce_estado == true
                            && CED.cede_estado == true
                            orderby CTP.cpce_producto
                            select new CatalogosDTO
                            {
                                Descripcion = CTP.cpce_producto,
                                Codigo = CTP.CPCE_CODIGO.ToString(),
                                Nivel = CED.cede_auxi1
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetCedentes()
        {
            List<SoftCob_CEDENTE> _cedente = new List<SoftCob_CEDENTE>();

            _cedente = _dtb.SoftCob_CEDENTE.Where(pd => pd.cede_estado).OrderBy(o => o.cede_nombre).ToList();

            _catalogo.Add(new CatalogosDTO()
            {
                Descripcion = "--Seleccione Cedente--",
                Codigo = "0"
            });

            foreach (SoftCob_CEDENTE _tab in _cedente)
            {
                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = _tab.cede_nombre,
                    Codigo = _tab.CEDE_CODIGO.ToString()
                });
            }

            return new FuncionesDAO().FunCambiarDataSet(_catalogo);
        }
        public DataSet FunGetCatalogoProducto(int _codigocedente)
        {
            try
            {
                var query = from Productos in _dtb.SoftCob_PRODUCTOS_CEDENTE
                            from Catalogo in _dtb.SoftCob_CATALOGO_PRODUCTOS_CEDENTE
                            where Productos.CEDE_CODIGO.Equals(_codigocedente) && Catalogo.PRCE_CODIGO.Equals(Productos.PRCE_CODIGO)
                            orderby Catalogo.CPCE_CODIGO
                            select new CatalogoProductos
                            {
                                Producto = Productos.prce_producto,
                                CodigoCatalogo = Catalogo.CPCE_CODIGO.ToString(),
                                CodigoProducto = Catalogo.cpce_codigoproducto,
                                CatalogoProducto = Catalogo.cpce_producto,
                                CodigoFamilia = Catalogo.cpce_codigofamilia,
                                Familia = Catalogo.cpce_familia,
                                Estado = Catalogo.cpce_estado ? "Activo" : "Inactivo",
                                CodProducto = Catalogo.PRCE_CODIGO.ToString()
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetBrenchDet(int _codigocede, int _codigocpce)
        {
            try
            {
                var query = from BHD in _dtb.SoftCob_BRENCHDET
                            join BCH in _dtb.SoftCob_BRENCH on BHD.BRCH_CODIGO equals (BCH.BRCH_CODIGO)
                            where BCH.brch_cedecodigo == _codigocede && BCH.brch_cpcecodigo == _codigocpce
                            && BCH.brch_estado == true && BHD.brde_estado == true
                            orderby BHD.brde_orden
                            select new BrenchDET
                            {
                                Codigo = BHD.BRDE_CODIGO,
                                RangoIni = BHD.brde_rangoinicial,
                                RangoFin = BHD.brde_rangofinal,
                                Etiqueta = BHD.brde_etiqueta,
                                Orden = BHD.brde_orden
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void FunCrearBrenchDet(SoftCob_BRENCHMESCAB _datos)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_BRENCHMESCAB.Add(_datos);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void FunEditBrenchDet(SoftCob_BRENCHMESCAB _datos)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_BRENCHMESCAB.Add(_datos);
                    _db.Entry(_datos).State = System.Data.Entity.EntityState.Modified;

                    foreach (SoftCob_BRENCHMESDET _datos1 in _datos.SoftCob_BRENCHMESDET)
                    {
                        if (_datos1.BRMD_CODIGO != 0) _db.Entry(_datos1).State = System.Data.Entity.EntityState.Modified;
                        else _db.Entry(_datos1).State = System.Data.Entity.EntityState.Added;
                    }
                    _db.SaveChanges();
                }
            }
            catch (Exception ex/*DbEntityValidationException ex*/)
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
        public int FunGetCodigoBrench(int _codigocede, int _codigocpce, int _codigobrch)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_BRENCH> _dato = _db.SoftCob_BRENCH.Where(x => x.brch_cedecodigo == _codigocede && x.brch_cpcecodigo == _codigocpce && x.BRCH_CODIGO == _codigobrch).ToList();

                    if (_dato.Count > 0) _codigo = _dato.FirstOrDefault().BRCH_CODIGO;
                    else _codigo = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _codigo;
        }
        public int FunGetCodigoBrenchDet(int _codigobrch, int _codigobrde)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_BRENCHDET> _dato = _db.SoftCob_BRENCHDET.Where(x => x.BRCH_CODIGO == _codigobrch
                        && x.BRDE_CODIGO == _codigobrde).ToList();

                    if (_dato.Count > 0) _codigo = _dato.FirstOrDefault().BRDE_CODIGO;
                    else _codigo = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _codigo;
        }
        public void FunCrearBrench(SoftCob_BRENCH _datos)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_BRENCH.Add(_datos);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void FunEditBrench(SoftCob_BRENCH _datos)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_BRENCH.Add(_datos);
                    _db.Entry(_datos).State = System.Data.Entity.EntityState.Modified;

                    foreach (SoftCob_BRENCHDET _datos1 in _datos.SoftCob_BRENCHDET)
                    {
                        if (_datos1.BRDE_CODIGO != 0) _db.Entry(_datos1).State = System.Data.Entity.EntityState.Modified;
                        else _db.Entry(_datos1).State = System.Data.Entity.EntityState.Added;
                    }
                    _db.SaveChanges();
                }
            }
            catch (Exception ex/*DbEntityValidationException ex*/)
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
        public DataSet FunGetListaCedentes()
        {
            try
            {
                var query = from Cedente in _dtb.SoftCob_CEDENTE
                            join Ciudad in _dtb.SoftCob_CIUDAD on Cedente.cede_ciudcod equals Ciudad.CIUD_CODIGO
                            select new CedenteAdminDTO
                            {
                                Codigo = Cedente.CEDE_CODIGO,
                                Ciudad = Ciudad.ciud_nombre,
                                Cedente = Cedente.cede_nombre,
                                Telefono = Cedente.cede_telefono1,
                                Estado = Cedente.cede_estado ? "Activo" : "Inactivo",
                                Urllink = "WFrm_NuevoCedente.aspx?CodigoCedente=" + Cedente.CEDE_CODIGO.ToString()
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int FunGetCodigoGestor(int _codigosupervisor, int _codigogestor)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_GESTOR_SUPERVISOR> _gest = _db.SoftCob_GESTOR_SUPERVISOR.Where(g => g.SUPE_CODIGO == _codigosupervisor 
                    && g.USUA_CODIGO == _codigogestor).ToList();

                    if (_gest.Count > 0)
                    {
                        foreach (var datos in _gest)
                        {
                            _codigo = datos.GEST_CODIGO;
                        }
                    }
                    else _codigo = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _codigo;
        }
        public string FunRegistroGestor(SoftCob_SUPERVISORES _supervisor)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    foreach (SoftCob_GESTOR_SUPERVISOR _gest in _supervisor.SoftCob_GESTOR_SUPERVISOR)
                    {
                        if (_gest.GEST_CODIGO != 0) _db.Entry(_gest).State = System.Data.Entity.EntityState.Modified;
                        else _db.Entry(_gest).State = System.Data.Entity.EntityState.Added;
                    }
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
        public string FunDelGestor(int _codigosupervisor, int _codigogestor)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_GESTOR_SUPERVISOR _gestor = _db.SoftCob_GESTOR_SUPERVISOR.SingleOrDefault(g => g.SUPE_CODIGO==_codigosupervisor
                    && g.GEST_CODIGO == _codigogestor);

                    if (_gestor != null)
                    {
                        _db.Entry(_gestor).State = System.Data.Entity.EntityState.Deleted;
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
        public DataSet FunGetProductoPorID(int _codcedente)
        {
            List<SoftCob_PRODUCTOS_CEDENTE> _produ = null;

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                _produ = _db.SoftCob_PRODUCTOS_CEDENTE.Where(p => p.prce_estado == true && p.CEDE_CODIGO == _codcedente).ToList();
            }

            _catalogo.Add(new CatalogosDTO()
            {
                Descripcion = "--Seleccione Producto--",
                Codigo = ""
            });

            foreach (SoftCob_PRODUCTOS_CEDENTE _pro in _produ)
            {
                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = _pro.prce_producto,
                    Codigo = _pro.PRCE_CODIGO.ToString()
                });
            }

            return new FuncionesDAO().FunCambiarDataSet(_catalogo);
        }
        public DataSet FunGetContactos(int _codigocedente)
        {
            try
            {
                var query = from Contactos in _dtb.SoftCob_CONTACTOS_CEDENTE
                            from PDetalle in _dtb.SoftCob_PARAMETRO_DETALLE
                            from PCabecera in _dtb.SoftCob_PARAMETRO_CABECERA
                            where Contactos.CEDE_CODIGO.Equals(_codigocedente) &&
                            PDetalle.PARA_CODIGO.Equals(PCabecera.PARA_CODIGO) && PCabecera.para_nombre.Equals("CARGOS") &&
                            Contactos.coce_cargo.Equals(PDetalle.pade_valorV)
                            orderby Contactos.COCE_CODIGO
                            select new CedenteContactos
                            {
                                CodigoContacto = Contactos.COCE_CODIGO,
                                Contacto = Contactos.coce_contacto,
                                CodigoCargo = Contactos.coce_cargo,
                                Cargo = PDetalle.pade_nombre,
                                Ext = Contactos.coce_extension,
                                Celular = Contactos.coce_celular,
                                Email1 = Contactos.coce_email1,
                                Email2 = Contactos.coce_email2
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetProductos(int _codigocedente)
        {
            try
            {
                var query = from Productos in _dtb.SoftCob_PRODUCTOS_CEDENTE
                            where Productos.CEDE_CODIGO.Equals(_codigocedente)
                            orderby Productos.PRCE_CODIGO
                            select new CedenteProductos
                            {
                                CodigoProducto = Productos.PRCE_CODIGO.ToString(),
                                Producto = Productos.prce_producto,
                                Descripcion = Productos.prce_descripcion,
                                Estado = Productos.prce_estado ? "Activo" : "Inactivo"
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string FunDelContacto(int _codcedente, int _cocecod)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_CONTACTOS_CEDENTE _contacto = _db.SoftCob_CONTACTOS_CEDENTE.SingleOrDefault(c => c.CEDE_CODIGO == 
                    _codcedente && c.COCE_CODIGO == _cocecod);

                    if (_contacto != null)
                    {
                        _db.Entry(_contacto).State = System.Data.Entity.EntityState.Deleted;
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
        public string FunDelProducto(int _codigocedente, int _codigoproducto)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_PRODUCTOS_CEDENTE> _producto = _db.SoftCob_PRODUCTOS_CEDENTE.Where(p => p.CEDE_CODIGO == 
                    _codigocedente && p.PRCE_CODIGO == _codigoproducto).ToList();

                    if (_producto.Count > 0) _codigo = _codigoproducto;
                    else _codigo = 0;
                }

                if (_codigo > 0)
                {
                    using (SoftCobEntities _db = new SoftCobEntities())
                    {
                        SoftCob_CATALOGO_PRODUCTOS_CEDENTE _catalogoproducto = 
                            _db.SoftCob_CATALOGO_PRODUCTOS_CEDENTE.SingleOrDefault(c => c.PRCE_CODIGO == _codigo);

                        if (_catalogoproducto != null) _mensaje = "Existe un Catálogo asociado a este Producto..!";
                        else _eliminar = true;
                    }
                }

                if (_eliminar)
                {
                    using (SoftCobEntities _db = new SoftCobEntities())
                    {
                        SoftCob_PRODUCTOS_CEDENTE _producto = _db.SoftCob_PRODUCTOS_CEDENTE.SingleOrDefault(p => p.CEDE_CODIGO == 
                        _codigocedente && p.PRCE_CODIGO == _codigoproducto);

                        if (_producto != null)
                        {
                            _db.Entry(_producto).State = System.Data.Entity.EntityState.Deleted;
                            _db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _mensaje;
        }
        public string FunDelCatalogoProducto(int _codigocedente, int _codigoproducto, int _codigocatalago)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_PRODUCTOS_CEDENTE> _producto = _db.SoftCob_PRODUCTOS_CEDENTE.Where(p => p.CEDE_CODIGO == _codigocedente && p.PRCE_CODIGO == _codigoproducto).ToList();

                    if (_producto.Count > 0) _codigo = _codigoproducto;
                    else _codigo = 0;
                }

                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_CATALOGO_PRODUCTOS_CEDENTE> _catalogo = _db.SoftCob_CATALOGO_PRODUCTOS_CEDENTE.Where(a => a.PRCE_CODIGO == _codigo && a.CPCE_CODIGO == _codigocatalago).ToList();

                    if (_catalogo.Count > 0) _codcatalago = _codigocatalago;
                    else _codigocatalago = 0;
                }

                if (_codcatalago > 0)
                {
                    using (SoftCobEntities _db = new SoftCobEntities())
                    {
                        SoftCob_CLIENTE_DEUDOR _cliente = _db.SoftCob_CLIENTE_DEUDOR.FirstOrDefault(c => c.CPCE_CODIGO == _codcatalago);

                        if (_cliente != null) _mensaje = "Existe un Cliente asociado a este Catálogo..!";
                        else _eliminar = true;
                    }
                }

                if (_eliminar)
                {
                    using (SoftCobEntities _db = new SoftCobEntities())
                    {
                        SoftCob_CATALOGO_PRODUCTOS_CEDENTE _catalogoproducto = 
                            _db.SoftCob_CATALOGO_PRODUCTOS_CEDENTE.SingleOrDefault(p => p.CPCE_CODIGO == _codcatalago);

                        if (_catalogoproducto != null)
                        {
                            _db.Entry(_catalogoproducto).State = System.Data.Entity.EntityState.Deleted;
                            _db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _mensaje;
        }
        public string FunDelAgencia(int _codigocedente, int _codigoagencia)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_AGENCIAS_CEDENTE> _agencia = _db.SoftCob_AGENCIAS_CEDENTE.Where(a => a.CEDE_CODIGO == _codigocedente 
                    && a.AGEN_CODIGO == _codigoagencia).ToList();

                    if (_agencia.Count > 0) _codigo = _codigoagencia;
                    else _codigo = 0;
                }

                if (_codigo > 0)
                {
                    using (SoftCobEntities _db = new SoftCobEntities())
                    {
                        SoftCob_AGENCIAS_CEDENTE _agencia = _db.SoftCob_AGENCIAS_CEDENTE.SingleOrDefault(a => a.CEDE_CODIGO ==
                        _codigocedente && a.AGEN_CODIGO == _codigo);

                        if (_agencia != null)
                        {
                            _db.Entry(_agencia).State = System.Data.Entity.EntityState.Deleted;
                            _db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _mensaje;
        }
        public bool FunConsultarCedenteporNombre(int _codciudad, string _nombrecedente)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_CEDENTE> _cedente = _db.SoftCob_CEDENTE.Where(c => c.cede_ciudcod == _codciudad && 
                    c.cede_nombre == _nombrecedente).ToList();

                    if (_cedente.Count > 0) _existe = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _existe;
        }
        public int FunGetCodigoContacto(int _codcedente, int _cocecod)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_CONTACTOS_CEDENTE> _cedente = _db.SoftCob_CONTACTOS_CEDENTE.Where(c => c.CEDE_CODIGO == _codcedente 
                    && c.COCE_CODIGO == _cocecod).ToList();

                    if (_cedente.Count > 0)
                    {
                        foreach (var _datos in _cedente)
                        {
                            _codigo = _datos.COCE_CODIGO;
                        }
                    }
                    else _codigo = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _codigo;
        }
        public int FunGetCodigoProducto(int _codcedente, int _prcecod)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_PRODUCTOS_CEDENTE> _producto = _db.SoftCob_PRODUCTOS_CEDENTE.Where(p => p.CEDE_CODIGO == _codcedente 
                    && p.PRCE_CODIGO == _prcecod).ToList();

                    if (_producto.Count > 0)
                    {
                        foreach (var datos in _producto)
                        {
                            _codigo = datos.PRCE_CODIGO;
                        }
                    }
                    else _codigo = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _codigo;
        }
        public int FunGetCodigoAgencia(int _codigocedente, int _codigoagencia)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_AGENCIAS_CEDENTE> _agencias = _db.SoftCob_AGENCIAS_CEDENTE.Where(a => a.CEDE_CODIGO == _codigocedente 
                    && a.AGEN_CODIGO == _codigoagencia).ToList();

                    if (_agencias.Count > 0)
                    {
                        foreach (var _datos in _agencias)
                        {
                            _codigo = _datos.AGEN_CODIGO;
                        }
                    }
                    else _codigo = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _codigo;
        }
        public int FunCrearCedente(SoftCob_CEDENTE _cedente)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_CEDENTE.Add(_cedente);
                    _db.SaveChanges();
                    _codigo = _cedente.CEDE_CODIGO;
                }
            }
            catch (Exception)
            {
                _codigo = -1;
            }
            return _codigo;
        }
        public int FunEditCedente(SoftCob_CEDENTE _cedente)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_CEDENTE.Add(_cedente);
                    _db.Entry(_cedente).State = System.Data.Entity.EntityState.Modified;

                    foreach (SoftCob_CONTACTOS_CEDENTE _contac in _cedente.SoftCob_CONTACTOS_CEDENTE)
                    {
                        if (_contac.COCE_CODIGO != 0) _db.Entry(_contac).State = System.Data.Entity.EntityState.Modified;
                        else _db.Entry(_contac).State = System.Data.Entity.EntityState.Added;
                    }

                    foreach (SoftCob_PRODUCTOS_CEDENTE _produ in _cedente.SoftCob_PRODUCTOS_CEDENTE)
                    {
                        if (_produ.PRCE_CODIGO != 0) _db.Entry(_produ).State = System.Data.Entity.EntityState.Modified;
                        else _db.Entry(_produ).State = System.Data.Entity.EntityState.Added;
                    }

                    foreach (SoftCob_AGENCIAS_CEDENTE _agencias in _cedente.SoftCob_AGENCIAS_CEDENTE)
                    {
                        if (_agencias.AGEN_CODIGO != 0) _db.Entry(_agencias).State = System.Data.Entity.EntityState.Modified;
                        else _db.Entry(_agencias).State = System.Data.Entity.EntityState.Added;
                    }

                    _db.SaveChanges();
                    _codigo = 0;
                }
            }
            catch (Exception /*DbEntityValidationException ex*/)
            {
                //foreach (var validationErrors in ex.EntityValidationErrors)
                //{
                //    foreach (var validationError in validationErrors.ValidationErrors)
                //    {
                //        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                //    }
                //}
                _codigo = -1;
            }
            return _codigo;
        }
        public string FunCatalogoProductos(int tipo, int codigocedente, string producto, int codigocatalogo, string codigoproducto, 
            string catalogoproducto, string codigofamilia, string familia, string estado, string str1, string str2, int int1, 
            int int2, string conexion)
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
                        comm.CommandText = "sp_InsertarCatalogoProductos";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_codigocedente", codigocedente);
                        comm.Parameters.AddWithValue("@in_producto", producto);
                        comm.Parameters.AddWithValue("@in_codigocatalago", codigocatalogo);
                        comm.Parameters.AddWithValue("@in_codigoproducto", codigoproducto);
                        comm.Parameters.AddWithValue("@in_catalogoproducto", catalogoproducto);
                        comm.Parameters.AddWithValue("@in_codigofamilia", codigofamilia);
                        comm.Parameters.AddWithValue("@in_familia", familia);
                        comm.Parameters.AddWithValue("@in_estado", estado);
                        comm.Parameters.AddWithValue("@in_auxv1", str1);
                        comm.Parameters.AddWithValue("@in_auxv2", str2);
                        comm.Parameters.AddWithValue("@in_auxi1", int1);
                        comm.Parameters.AddWithValue("@in_auxi2", int2);
                        _dap.SelectCommand = comm;
                        _dap.Fill(_dts);
                        _mensaje = _dts.Tables[0].Rows[0][0].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _mensaje;
        }
        public string FunCrearTablaHistorico(string _cedente, string _conexion)
        {
            try
            {
                _query = "IF OBJECT_ID('dbo.SALDOS_" + _cedente.Replace(".", "") + "', 'U') IS NULL ";
                _query += "BEGIN ";
                _query += "CREATE TABLE [dbo].[SALDOS_" + _cedente.Replace(".", "") + "](";
                _query += "[sald_diasmora][int] NOT NULL,";
                _query += "[sald_totaloperacion][int] NOT NULL,";
                _query += "[sald_valorexigible][decimal](12,2) NOT NULL,";
                _query += "[sald_totaldeuda][decimal](12,2) NOT NULL,";
                _query += "[sald_fechaproceso] [date] NOT NULL)";
                _query += " END";

                using (SqlConnection con = new SqlConnection(_conexion))
                {
                    using (SqlCommand comm = new SqlCommand(_query))
                    {
                        comm.Connection = con;
                        con.Open();
                        comm.CommandTimeout = 9000;
                        comm.ExecuteNonQuery();
                        con.Close();
                    }
                }

                _query = "";
                _query = "IF OBJECT_ID('dbo.HISTORICO_" + _cedente.Replace(".", "") + "', 'U') IS NULL ";
                _query += "BEGIN ";
                _query += "CREATE TABLE [dbo].[HISTORICO_" + _cedente.Replace(".", "") + "](";
                _query += "[hiop_fechaproceso] [date] NOT NULL,";
                _query += "[hiop_identificacion][varchar](20) NOT NULL,";
                _query += "[hiop_operacion][varchar](50) NOT NULL,";
                _query += "[hiop_valorexigible][decimal](12,2) NOT NULL,";
                _query += "[hiop_diasmora][int] NOT NULL,";
                _query += "[hiop_totaldeuda][decimal](12,2) NOT NULL,";
                _query += "[hiop_auxv1][varchar](50) NULL,";
                _query += "[hiop_auxv2][varchar](50) NULL,";
                _query += "[hiop_auxi1][int] NULL,";
                _query += "[hiop_auxi2][int] NULL,";
                _query += "[hiop_auxd1][decimal](12,2) NOT NULL,";
                _query += "[hiop_auxd2][decimal](12,2) NOT NULL,";
                _query += "[hiop_auxd3][decimal](12,2) NOT NULL,";
                _query += "[hiop_auxd4][decimal](12,2) NOT NULL)";
                _query += " END";

                using (SqlConnection con = new SqlConnection(_conexion))
                {
                    using (SqlCommand comm = new SqlCommand(_query))
                    {
                        comm.Connection = con;
                        con.Open();
                        comm.CommandTimeout = 9000;
                        comm.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _mensaje;
        }
        public int FunGetCodigoSupervisor(int _codigocedente, int _codigosupervisor)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_SUPERVISORES> _super = _db.SoftCob_SUPERVISORES.Where(s => s.CEDE_CODIGO == _codigocedente && 
                    s.SUPE_CODIGO == _codigosupervisor).ToList();

                    if (_super.Count > 0)
                    {
                        foreach (var _datos in _super)
                        {
                            _codigo = _datos.SUPE_CODIGO;
                        }
                    }
                    else _codigo = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _codigo;
        }
        public string FunRegistroSupervisor(SoftCob_CEDENTE _supervisores)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    foreach (SoftCob_SUPERVISORES _super in _supervisores.SoftCob_SUPERVISORES)
                    {
                        if (_super.SUPE_CODIGO != 0) _db.Entry(_super).State = System.Data.Entity.EntityState.Modified;
                        else _db.Entry(_super).State = System.Data.Entity.EntityState.Added;
                    }
                    _db.SaveChanges();
                    _mensaje = "";
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
        public string FunDelSupervisor(int _codigocedente, int _codigosupervisor)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _codigo = _db.SoftCob_SUPERVISORES.Where(s => s.CEDE_CODIGO == _codigocedente &&
                        s.USUA_CODIGO == _codigosupervisor) == null ? 0 : _db.SoftCob_SUPERVISORES.Where(s => s.CEDE_CODIGO ==
                        _codigocedente && s.USUA_CODIGO == _codigosupervisor).FirstOrDefault().SUPE_CODIGO;
                }

                if (_codigo > 0)
                {
                    using (SoftCobEntities _db = new SoftCobEntities())
                    {
                        if (_db.SoftCob_GESTOR_SUPERVISOR.Where(g => g.SUPE_CODIGO == _codigo).ToList().Count == 0)
                        {
                            _eliminar = true;
                        }
                        else
                        {
                            _mensaje = "Existe asociado un Gestor al Supervisor..!";
                        }
                    }
                }

                if (_eliminar)
                {
                    using (SoftCobEntities _db = new SoftCobEntities())
                    {
                        SoftCob_SUPERVISORES _supervisor = _db.SoftCob_SUPERVISORES.SingleOrDefault(s => s.CEDE_CODIGO == 
                        _codigocedente && s.SUPE_CODIGO == _codigo);

                        if (_supervisor != null)
                        {
                            _db.Entry(_supervisor).State = System.Data.Entity.EntityState.Deleted;
                            _db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _mensaje;
        }
        public void FunInsertarAccionGestion(SoftCob_ACCIONGESTION _accionges)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_ACCIONGESTION.Add(_accionges);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetSegmentoCabecera(int _codigocede, int _codigocpce, int _tipo)
        {
            try
            {
                var query = from SCA in _dtb.SoftCob_SEGMENTO_CABECERA
                            join CDE in _dtb.SoftCob_CEDENTE on SCA.sgca_cedecodigo equals CDE.CEDE_CODIGO
                            join CTP in _dtb.SoftCob_CATALOGO_PRODUCTOS_CEDENTE on SCA.sgca_cpcecodigo equals CTP.CPCE_CODIGO
                            where SCA.sgca_cedecodigo == _codigocede && SCA.sgca_cpcecodigo == _codigocpce
                            && SCA.sgca_auxi1 == _tipo
                            orderby SCA.sgca_valorinicial
                            select new SegmentoAdmin
                            {
                                Codigo = SCA.SGCA_CODIGO.ToString(),
                                Segmento = SCA.sgca_segmento,
                                Descripcion = SCA.sgca_descripcion,
                                ValorI = SCA.sgca_valorinicial,
                                ValorF = SCA.sgca_valorfinal,
                                Estado = SCA.sgca_estado ? "Activo" : "Inactivo",
                                Auxv1 = SCA.sgca_auxv1,
                                Auxv2 = SCA.sgca_auxv2,
                                Auxv3 = SCA.sgca_auxv3,
                                Auxi1 = (int)SCA.sgca_auxi1,
                                Auxi2 = (int)SCA.sgca_auxi2,
                                Auxi3 = (int)SCA.sgca_auxi3
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string FunDelSegmento(int _codigocede, int _codigocpce, int _codigosgca, int _tipo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_SEGMENTO_CABECERA _datos = _db.SoftCob_SEGMENTO_CABECERA.SingleOrDefault(x => x.sgca_cedecodigo == _codigocede
                    && x.sgca_cpcecodigo == _codigocpce && x.SGCA_CODIGO == _codigosgca && x.sgca_auxi1 == _tipo);

                    if (_datos != null)
                    {
                        _db.Entry(_datos).State = System.Data.Entity.EntityState.Deleted;
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
        public int FunGetCodigoSegmento(int _codigocede, int _codigocpce, int _codigosgca, int _tipo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_SEGMENTO_CABECERA> _dato = _db.SoftCob_SEGMENTO_CABECERA.Where(x => x.sgca_cedecodigo == _codigocede
                    && x.sgca_cpcecodigo == _codigocpce && x.SGCA_CODIGO == _codigosgca && x.sgca_auxi1 == _tipo).ToList();

                    if (_dato.Count > 0) _codigo = _dato.FirstOrDefault().SGCA_CODIGO;
                    else _codigo = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _codigo;
        }
        public void FunCrearSegmento(SoftCob_SEGMENTO_CABECERA _datos)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_SEGMENTO_CABECERA.Add(_datos);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void FunEditSegmento(SoftCob_SEGMENTO_CABECERA _datos)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_SEGMENTO_CABECERA.Add(_datos);
                    _db.Entry(_datos).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex/*DbEntityValidationException ex*/)
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
        public DataSet FunGetEstrategiaCab()
        {
            List<SoftCob_ESTRATEGIA_CABECERA> _tablas = null;

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                _tablas = _db.SoftCob_ESTRATEGIA_CABECERA.Where(pd => pd.esca_estado).ToList();
            }

            _catalogo.Add(new CatalogosDTO()
            {
                Descripcion = "--Seleccione Estrategia--",
                Codigo = "0"
            });

            foreach (SoftCob_ESTRATEGIA_CABECERA _tab in _tablas)
            {
                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = _tab.esca_estrategia,
                    Codigo = _tab.ESCA_CODIGO.ToString()
                });
            }

            return new FuncionesDAO().FunCambiarDataSet(_catalogo);
        }
        #endregion
    }
}
