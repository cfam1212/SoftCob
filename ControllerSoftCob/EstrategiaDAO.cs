namespace ControllerSoftCob
{
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    public class EstrategiaDAO
    {
        #region Variables
        string _mensaje = "";
        SoftCobEntities _dtb = new SoftCobEntities();
        DataSet _dts = new DataSet();
        SqlDataAdapter _dap = new SqlDataAdapter();
        int _codigo = 0;
        #endregion

        #region Procedimientos y Funciones
        public string FunCrearCamposEstrategia(int tipo, string auxv1, string auxv2, string auxv3, int auxi1, int auxi2, int auxi3,
            int usucodigo, string terminal, DataTable dtbCampos, string sp, string conexion)
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
                        comm.CommandText = sp;
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        comm.Parameters.AddWithValue("@in_usucodigo", usucodigo);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
                        comm.Parameters.AddWithValue("@TablaCampos", dtbCampos);
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

        public List<SoftCob_CAMPOS_ESTRATEGIA> FunGetCamposComboEstrategia()
        {
            List<SoftCob_CAMPOS_ESTRATEGIA> _campos = new List<SoftCob_CAMPOS_ESTRATEGIA>();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _campos = _db.SoftCob_CAMPOS_ESTRATEGIA.Where(t => t.caes_estado == true).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _campos;
        }

        public int FunGetCodigoCabEstrategia(int _codigocabecera, int _codigodetalle)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_ESTRATEGIA_DETALLE> _estdetalle = _db.SoftCob_ESTRATEGIA_DETALLE.Where(p => p.ESCA_CODIGO ==
                    _codigocabecera && p.ESDE_CODIGO == _codigodetalle).ToList();

                    if (_estdetalle.Count > 0)
                    {
                        foreach (var _datos in _estdetalle)
                        {
                            _codigo = _datos.ESDE_CODIGO;
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

        public string FunCrearEstrategia(SoftCob_ESTRATEGIA_CABECERA _estrategia)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_ESTRATEGIA_CABECERA.Add(_estrategia);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _mensaje;
        }

        public string FunEditEstrategia(SoftCob_ESTRATEGIA_CABECERA _estrategia)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_ESTRATEGIA_CABECERA.Add(_estrategia);
                    _db.Entry(_estrategia).State = System.Data.Entity.EntityState.Modified;

                    foreach (SoftCob_ESTRATEGIA_DETALLE _deta in _estrategia.SoftCob_ESTRATEGIA_DETALLE)
                    {
                        if (_deta.ESDE_CODIGO != 0) _db.Entry(_deta).State = System.Data.Entity.EntityState.Modified;
                        else _db.Entry(_deta).State = System.Data.Entity.EntityState.Added;
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

        public string FunCrearListaTrabajo(int codigolista, string lista, string descripcion, string fechainicio, string fechafin, 
            int escacodigo, int cedecodigo, int cpccodigo, bool estado, string tipomarcado, string campania, int porgestion, 
            string tipogestion, int porarbol, int codigoarac, int porfecha, string fechadesde, string fechahasta, string auxv1, 
            string auxv2, string auxv3, int auxi1, int auxi2, int auxi3,
            int usucodigo, string terminal, DataTable dtbLista, DataTable dtbEstrategia, string sp, string conexion)
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
                        comm.CommandText = sp;
                        comm.Parameters.AddWithValue("@in_codigolista", codigolista);
                        comm.Parameters.AddWithValue("@in_lista", lista);
                        comm.Parameters.AddWithValue("@in_descripcion", descripcion);
                        comm.Parameters.AddWithValue("@in_fechainicio", fechainicio);
                        comm.Parameters.AddWithValue("@in_fechafin", fechafin);
                        comm.Parameters.AddWithValue("@in_escacodigo", escacodigo);
                        comm.Parameters.AddWithValue("@in_cedecodigo", cedecodigo);
                        comm.Parameters.AddWithValue("@in_cpccodigo", cpccodigo);
                        comm.Parameters.AddWithValue("@in_estado", estado);
                        comm.Parameters.AddWithValue("@in_tipomarcado", tipomarcado);
                        comm.Parameters.AddWithValue("@in_campania", campania);
                        comm.Parameters.AddWithValue("@in_porgestion", porgestion);
                        comm.Parameters.AddWithValue("@in_tipogestion", tipogestion);
                        comm.Parameters.AddWithValue("@in_porarbol", porarbol);
                        comm.Parameters.AddWithValue("@in_araccodigo", codigoarac);
                        comm.Parameters.AddWithValue("@in_porfecha", porfecha);
                        comm.Parameters.AddWithValue("@in_fechadesde", fechadesde);
                        comm.Parameters.AddWithValue("@in_fechahasta", fechahasta);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        comm.Parameters.AddWithValue("@in_usucodigo", usucodigo);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
                        comm.Parameters.AddWithValue("@TablaLista", dtbLista);
                        comm.Parameters.AddWithValue("@TablaEstra", dtbEstrategia);
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

        public void FunDelEstrategiaDet(int _codigoesca, int _codigoesde)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_ESTRATEGIA_DETALLE _borrar = _db.SoftCob_ESTRATEGIA_DETALLE.SingleOrDefault(x => x.ESCA_CODIGO ==
                    _codigoesca && x.ESDE_CODIGO == _codigoesde);

                    if (_borrar != null)
                    {
                        _db.Entry(_borrar).State = System.Data.Entity.EntityState.Deleted;
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetEstrategiaCabecera()
        {
            try
            {
                var query = from Estrategia in _dtb.SoftCob_ESTRATEGIA_CABECERA
                            select new EstrategiaCabecera
                            {
                                Codigo = Estrategia.ESCA_CODIGO,
                                Estrategia = Estrategia.esca_estrategia,
                                Descripcion = Estrategia.esca_descripcion,
                                Estado = Estrategia.esca_estado == true ? "Activo" : "Inactivo",
                                Urllink = "WFrm_NuevaEstrategia.aspx?CodigoEstrategia=" + Estrategia.ESCA_CODIGO.ToString()
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetEstrategiaDetalle(int codigocabecera)
        {
            try
            {
                var query = from Detalle in _dtb.SoftCob_ESTRATEGIA_DETALLE
                            join Campos in _dtb.SoftCob_CAMPOS_ESTRATEGIA on Detalle.esde_caescodigo equals Campos.CAES_CODIGO
                            where Detalle.ESCA_CODIGO == codigocabecera
                            orderby Detalle.esde_prioridad
                            select new EstrategiaDetalle
                            {
                                Codigo = Detalle.ESDE_CODIGO.ToString(),
                                CodigoCampo = Campos.CAES_CODIGO,
                                Campo = Campos.caes_nombre,
                                Operacion = Detalle.esde_operacion,
                                Valor = Detalle.esde_valor,
                                Orden = Detalle.esde_orden,
                                Prioridad = Detalle.esde_prioridad.ToString(),
                                Estado = Detalle.esde_estado ? "Activo" : "Inactivo",
                                Auxv1 = Campos.caes_auxv1,
                                Auxv2 = Campos.caes_auxv2,
                                Auxv3 = Campos.caes_auxv3,
                                Auxi1 = (int)Campos.caes_auxi1,
                                Auxi2 = (int)Campos.caes_auxi2,
                                Auxi3 = (int)Campos.caes_auxi3
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetDatosCampos(int codigotabla, int codigocampos)
        {
            try
            {
                var query = from Campos in _dtb.SoftCob_CAMPOS_ESTRATEGIA
                            where Campos.TABD_CODIGO == codigotabla && Campos.CAES_CODIGO == codigocampos
                            select new EstrategiaAdminDTO
                            {
                                Codigo = Campos.CAES_CODIGO,
                                CodigoTabla = Campos.TABD_CODIGO,
                                Campo = Campos.caes_nombre,
                                Tipo = Campos.caes_tipo,
                                Estado = Campos.caes_estado ? "Activo" : "Inactivo",
                                Auxv1 = Campos.caes_auxv1,
                                Auxv2 = Campos.caes_auxv2,
                                Auxv3 = Campos.caes_auxv3,
                                Auxi1 = (int)Campos.caes_auxi1,
                                Auxi2 = (int)Campos.caes_auxi2,
                                Auxi3 = (int)Campos.caes_auxi3,
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetDatosCampos(int caescodigo)
        {
            try
            {
                var query = from Campos in _dtb.SoftCob_CAMPOS_ESTRATEGIA
                            where Campos.CAES_CODIGO == caescodigo
                            select new EstrategiaAdminDTO
                            {
                                Codigo = Campos.CAES_CODIGO,
                                CodigoTabla = Campos.TABD_CODIGO,
                                Campo = Campos.caes_nombre,
                                Tipo = Campos.caes_tipo,
                                Estado = Campos.caes_estado ? "Activo" : "Inactivo",
                                Auxv1 = Campos.caes_auxv1,
                                Auxv2 = Campos.caes_auxv2,
                                Auxv3 = Campos.caes_auxv3,
                                Auxi1 = (int)Campos.caes_auxi1,
                                Auxi2 = (int)Campos.caes_auxi2,
                                Auxi3 = (int)Campos.caes_auxi3,
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetCamposEstrategia(int codigotabla)
        {
            try
            {
                var query = from Campos in _dtb.SoftCob_CAMPOS_ESTRATEGIA
                            where Campos.TABD_CODIGO == codigotabla
                            select new EstrategiaAdminDTO
                            {
                                Codigo = Campos.CAES_CODIGO,
                                CodigoTabla = Campos.TABD_CODIGO,
                                Campo = Campos.caes_nombre,
                                Tipo = Campos.caes_tipo,
                                Estado = Campos.caes_estado ? "Activo" : "Inactivo",
                                Auxv1 = Campos.caes_auxv1,
                                Auxv2 = Campos.caes_auxv2,
                                Auxv3 = Campos.caes_auxv3,
                                Auxi1 = (int)Campos.caes_auxi1,
                                Auxi2 = (int)Campos.caes_auxi2,
                                Auxi3 = (int)Campos.caes_auxi3,
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
