namespace ControllerSoftCob
{
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    public class ArbolDecisionDAO
    {
        #region Variables
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
                    SoftCob_ACCION _arbol = _db.SoftCob_ACCION.SingleOrDefault(x => x.CPCE_CODIGO == _codigocpce && x.ARAC_CODIGO == _codigo);

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

        public string FunDelEfecto(int _codigo, int _araccodigo, int _codigocpce)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_EFECTO _arbol = _db.SoftCob_EFECTO.SingleOrDefault(x => x.AREF_CODIGO == _codigo && x.ARAC_CODIGO == _araccodigo && x.aref_auxi1 == _codigocpce);

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

        public string FunDelRespuesta(int _codigo, int _arefcodigo, int _codigocpce)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_RESPUESTA _arbol = _db.SoftCob_RESPUESTA.SingleOrDefault(x => x.ARRE_CODIGO == _codigo && x.AREF_CODIGO == _arefcodigo && x.arre_auxi1 == _codigocpce);

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

        public string FunDelContacto(int _codigo, int _arrecodigo, int _codigocpce)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_CONTACTO _arbol = _db.SoftCob_CONTACTO.SingleOrDefault(x => x.ARCO_CODIGO == _codigo && x.ARRE_CODIGO == _arrecodigo && x.arco_auxi1 == _codigocpce);

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
                    return _db.SoftCob_RESPUESTA.Where(x => x.ARRE_CODIGO == _arrecodigo && x.arre_auxi1 == _codigocpce).FirstOrDefault().arre_pago;
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
                    return _db.SoftCob_RESPUESTA.Where(x => x.ARRE_CODIGO == _arrecodigo && x.arre_auxi1 == _codigocpce).FirstOrDefault().arre_llamar;
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
                    return _db.SoftCob_ACCION.Where(x => x.ARAC_CODIGO == _araccodigo).FirstOrDefault().arac_contacto;
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
                    SoftCob_ACCION _original = _db.SoftCob_ACCION.Where(x => x.CPCE_CODIGO == _datos.CPCE_CODIGO && x.ARAC_CODIGO == _datos.ARAC_CODIGO).FirstOrDefault();
                    _db.SoftCob_ACCION.Attach(_original);
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
                    List<SoftCob_SPEECH_DETALLE> datos = _db.SoftCob_SPEECH_DETALLE.Where(x => x.SPCA_CODIGO == _codigospca && x.SPDE_CODIGO == _codigospde).ToList();

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
                    List<SoftCob_ACCION> _accion = _db.SoftCob_ACCION.Where(a => a.CPCE_CODIGO == _codigocpce && a.arac_descripcion == _descripcion).ToList();

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
        #endregion
    }
}
