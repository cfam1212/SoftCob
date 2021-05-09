namespace ControllerSoftCob
{
    using ModeloSoftCob;
    using System;
    using System.Data;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Data.SqlClient;
    using System.Linq;
    public class ArbolDecisionDAO
    {
        #region Variables
        SoftCobEntities _dtb = new SoftCobEntities();
        DataSet _dts = new DataSet();
        SqlDataAdapter _dta = new SqlDataAdapter();
        string _mensaje = "";
        int _codigo = 0;
        #endregion

        #region Procedimientos y Funciones
        public string FunDelAccion(int _codigo, int _codigocpce)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_ARBOL_ACCION _arbol = _db.SoftCob_ARBOL_ACCION.SingleOrDefault(x => x.CPCE_CODIGO == _codigocpce 
                    && x.ARAC_CODIGO == _codigo);

                    if (_arbol != null)
                    {
                        _db.Entry(_arbol).State = System.Data.Entity.EntityState.Deleted;
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

        public string FunDelEfecto(int _codigo, int _codigocpce)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_ARBOL_EFECTO _arbol = _db.SoftCob_ARBOL_EFECTO.SingleOrDefault(x => x.AREF_CODIGO == _codigo && 
                    x.cpcecodigo == _codigocpce);

                    if (_arbol != null)
                    {
                        _db.Entry(_arbol).State = System.Data.Entity.EntityState.Deleted;
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

        public string FunDelRespuesta(int _codigo, int _codigocpce)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_ARBOL_RESPUESTA _arbol = _db.SoftCob_ARBOL_RESPUESTA.SingleOrDefault(x => x.ARRE_CODIGO == _codigo 
                    && x.cpcecodigo == _codigocpce);

                    if (_arbol != null)
                    {
                        _db.Entry(_arbol).State = System.Data.Entity.EntityState.Deleted;
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

        public string FunDelContacto(int _codigo, int _codigocpce)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_ARBOL_CONTACTO _arbol = _db.SoftCob_ARBOL_CONTACTO.SingleOrDefault(x => x.ARCO_CODIGO == _codigo 
                    && x.cpcecodigo == _codigocpce);

                    if (_arbol != null)
                    {
                        _db.Entry(_arbol).State = System.Data.Entity.EntityState.Deleted;
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

        public bool FunGetAbonoPago(int _arrecodigo, int _codigocpce)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    return _db.SoftCob_ARBOL_RESPUESTA.Where(x => x.ARRE_CODIGO == _arrecodigo 
                    && x.cpcecodigo == _codigocpce).FirstOrDefault().arre_pago;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool FunGetLlamar(int _arrecodigo, int _codigocpce)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    return _db.SoftCob_ARBOL_RESPUESTA.Where(x => x.ARRE_CODIGO == _arrecodigo 
                    && x.cpcecodigo == _codigocpce).FirstOrDefault().arre_llamar;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool FunGetEfectivo(int _araccodigo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    return _db.SoftCob_ARBOL_ACCION.Where(x => x.ARAC_CODIGO == _araccodigo).FirstOrDefault().arac_contacto;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FunEditArbolAccion(SoftCob_ACCION _datos)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_ARBOL_ACCION _original = _db.SoftCob_ARBOL_ACCION.Where(x => x.CPCE_CODIGO == _datos.CPCE_CODIGO 
                    && x.ARAC_CODIGO == _datos.ARAC_CODIGO).FirstOrDefault();
                    _db.SoftCob_ARBOL_ACCION.Attach(_original);
                    _original.arac_descripcion = _datos.arac_descripcion;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SoftCob_SPEECH_CABECERA FunGetSpeechPorID(int _codigocpce)
        {
            SoftCob_SPEECH_CABECERA _datos = new SoftCob_SPEECH_CABECERA();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _datos = _db.SoftCob_SPEECH_CABECERA.Where(x => x.spca_cpcecodigo == _codigocpce).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _datos;
        }

        public int FunGetCodigoSpeechDet(int _codigospca, int _codigospde)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_SPEECH_DETALLE> datos = _db.SoftCob_SPEECH_DETALLE.Where(x => x.SPCA_CODIGO == _codigospca 
                    && x.SPDE_CODIGO == _codigospde).ToList();

                    if (datos.Count > 0)
                    {
                        foreach (var datosx in datos)
                        {
                            _codigo = datosx.SPDE_CODIGO;
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
        public int FunCrearArbolSpeech(SoftCob_SPEECH_CABECERA _datos)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_SPEECH_CABECERA.Add(_datos);
                    _db.SaveChanges();
                    _codigo = _datos.SPCA_CODIGO;
                }
            }
            catch (Exception)
            {
                _codigo = -1;
            }
            return _codigo;
        }
        public int FunEditArbolSpeech(SoftCob_SPEECH_CABECERA _datos)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_SPEECH_CABECERA.Add(_datos);
                    _db.Entry(_datos).State = System.Data.Entity.EntityState.Modified;

                    foreach (SoftCob_SPEECH_DETALLE datos1 in _datos.SoftCob_SPEECH_DETALLE)
                    {
                        if (datos1.SPDE_CODIGO != 0) _db.Entry(datos1).State = System.Data.Entity.EntityState.Modified;
                        else _db.Entry(datos1).State = System.Data.Entity.EntityState.Added;
                    }

                    _db.SaveChanges();
                    _codigo = 0;
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                _codigo = -1;
            }
            return _codigo;
        }
        public int FunGetCodigoAccion(int _codigocpce, string _descripcion)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_ARBOL_ACCION> _accion = _db.SoftCob_ARBOL_ACCION.Where(a => a.CPCE_CODIGO == _codigocpce 
                    && a.arac_descripcion == _descripcion).ToList();

                    if (_accion.Count > 0)
                    {
                        foreach (var _datos in _accion)
                        {
                            _codigo = _datos.ARAC_CODIGO;
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
        public DataSet FunGetArbolSpeechDet(int codigoSPCA)
        {
            try
            {

                var query = from SPD in _dtb.SoftCob_SPEECH_DETALLE
                            join ACC in _dtb.SoftCob_ARBOL_ACCION on SPD.spde_araccodigo equals ACC.ARAC_CODIGO
                            join EFE in _dtb.SoftCob_ARBOL_EFECTO on SPD.spde_arefcodigo equals EFE.AREF_CODIGO
                            join ARE in _dtb.SoftCob_ARBOL_RESPUESTA on SPD.spde_arrecodigo equals ARE.ARRE_CODIGO
                            join ACO in _dtb.SoftCob_ARBOL_CONTACTO on SPD.spde_arcocodigo equals ACO.ARCO_CODIGO
                            where SPD.SPCA_CODIGO == codigoSPCA
                            orderby ACC.arac_descripcion
                            select new ArbolSpeechDTO
                            {
                                Codigo = SPD.SPDE_CODIGO.ToString(),
                                CodigoARAC = SPD.spde_araccodigo,
                                Accion = ACC.arac_descripcion,
                                CodigoAREF = SPD.spde_arefcodigo,
                                Efecto = EFE.aref_descripcion,
                                CodigoARRE = SPD.spde_arrecodigo,
                                Respuesta = ARE.arre_descripcion,
                                CodigoARCO = SPD.spde_arcocodigo,
                                Contacto = ACO.arco_descripcion,
                                Speech = SPD.spde_speechad,
                                Observacion = SPD.spde_observacion,
                                Estado = SPD.spde_estado ? "Activo" : "Inactivo",
                                Auxv1 = SPD.spde_auxv1,
                                Auxv2 = SPD.spde_auxv2,
                                Auxi1 = (int)SPD.spde_auxi1,
                                Auxi2 = (int)SPD.spde_auxi2
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FunGetArbolSpeechDet1(int codigoSPCA)
        {
            try
            {
                var query = from SPD in _dtb.SoftCob_SPEECH_DETALLE
                            join ACC in _dtb.SoftCob_ACCION on SPD.spde_araccodigo equals ACC.ARAC_CODIGO
                            join EFE in _dtb.SoftCob_EFECTO on SPD.spde_arefcodigo equals EFE.AREF_CODIGO
                            join ARE in _dtb.SoftCob_RESPUESTA on SPD.spde_arrecodigo equals ARE.ARRE_CODIGO
                            where SPD.SPCA_CODIGO == codigoSPCA
                            orderby ACC.arac_descripcion
                            select new ArbolSpeechDTO
                            {
                                Codigo = SPD.SPDE_CODIGO.ToString(),
                                CodigoARAC = SPD.spde_araccodigo,
                                Accion = ACC.arac_descripcion,
                                CodigoAREF = SPD.spde_arefcodigo,
                                Efecto = EFE.aref_descripcion,
                                CodigoARRE = SPD.spde_arrecodigo,
                                Respuesta = ARE.arre_descripcion,
                                CodigoARCO = SPD.spde_arcocodigo,
                                Contacto = SPD.spde_arcocodigo > 0 ? "CON CATALOGO" : "",
                                Speech = SPD.spde_speechad,
                                Observacion = SPD.spde_observacion,
                                Estado = SPD.spde_estado ? "Activo" : "Inactivo",
                                Auxv1 = SPD.spde_auxv1,
                                Auxv2 = SPD.spde_auxv2,
                                Auxi1 = (int)SPD.spde_auxi1,
                                Auxi2 = (int)SPD.spde_auxi2
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FunGetArbolSpeechDetPorID(int codigoSPCA, int codigoSPDE)
        {
            try
            {
                var query = from SPD in _dtb.SoftCob_SPEECH_DETALLE
                            where SPD.SPCA_CODIGO == codigoSPCA && SPD.SPDE_CODIGO == codigoSPDE
                            select new ArbolSpeechDTO
                            {
                                Estado = SPD.spde_estado ? "Activo" : "Inactivo"
                            };
                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunNewArbolDecision(int tipo, int cedecodigo, int cpcecodigo, int araccodigo, int arefcodigo, int arrecodigo,
            int arcocodigo, string descripcion, string estado, string contacto, string pago, string llamar, string efectivo,
            string comisiona, string auxv1, string auxv2, string auxv3, string auxv4, string auxv5, int auxi1, int auxi2, int auxi3,
            int auxi4, int auxi5, int usuacodigo, string terminal, string conexion)
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
                        comm.CommandText = "sp_NewArbolDecision";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_cedecodigo", cedecodigo);
                        comm.Parameters.AddWithValue("@in_cpcecodigo", cpcecodigo);
                        comm.Parameters.AddWithValue("@in_araccodigo", araccodigo);
                        comm.Parameters.AddWithValue("@in_arefcodigo", arefcodigo);
                        comm.Parameters.AddWithValue("@in_arrecodigo", arrecodigo);
                        comm.Parameters.AddWithValue("@in_arcocodigo", arcocodigo);
                        comm.Parameters.AddWithValue("@in_descripcion", descripcion);
                        comm.Parameters.AddWithValue("@in_estado", estado);
                        comm.Parameters.AddWithValue("@in_contacto", contacto);
                        comm.Parameters.AddWithValue("@in_pago", pago);
                        comm.Parameters.AddWithValue("@in_llamar", llamar);
                        comm.Parameters.AddWithValue("@in_efectivo", efectivo);
                        comm.Parameters.AddWithValue("@in_comisiona", comisiona);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxv4", auxv4);
                        comm.Parameters.AddWithValue("@in_auxv5", auxv5);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        comm.Parameters.AddWithValue("@in_auxi4", auxi4);
                        comm.Parameters.AddWithValue("@in_auxi5", auxi5);
                        comm.Parameters.AddWithValue("@in_usuacodigo", usuacodigo);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
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
        #endregion
    }
}
