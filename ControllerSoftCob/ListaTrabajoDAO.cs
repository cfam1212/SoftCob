﻿namespace ControllerSoftCob
{
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    public class ListaTrabajoDAO
    {
        #region Variables
        DataSet _dts = new DataSet();
        SqlDataAdapter _dap = new SqlDataAdapter();
        SoftCobEntities _db = new SoftCobEntities();
        string _mensaje = "";
        bool _respuesta = false;
        int _codigo = 0;
        #endregion

        #region Procedimientos y Funciones
        public DataSet FunSpeechConvert(string speech, int cldcodigo, int gestorasingado, string conexion)
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
                        comm.CommandText = "sp_ConsultaSpeech";
                        comm.Parameters.AddWithValue("@in_speech", speech);
                        comm.Parameters.AddWithValue("@in_cldecodigo", cldcodigo);
                        comm.Parameters.AddWithValue("@in_gestorasignado", gestorasingado);
                        _dap.SelectCommand = comm;
                        _dap.Fill(_dts);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _dts;
        }

        public bool FunGetValorRespuesta(int _arrecodigo, int _opcion)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    if (_opcion == 0) _respuesta = _db.SoftCob_RESPUESTA.Where(r => r.ARRE_CODIGO == _arrecodigo && r.arre_estado).FirstOrDefault().arre_pago;

                    if (_opcion == 1) _respuesta = _db.SoftCob_RESPUESTA.Where(r => r.ARRE_CODIGO == _arrecodigo && r.arre_estado).FirstOrDefault().arre_llamar;

                    if (_opcion == 2) _respuesta = bool.Parse(_db.SoftCob_RESPUESTA.Where(r => r.ARRE_CODIGO == _arrecodigo && r.arre_estado == true).FirstOrDefault().arre_auxv2);
                }
            }
            catch (Exception)
            {
                _respuesta = false;
            }
            return _respuesta;
        }

        public int FunGetExisteTelefono(int _cedecodigo, int _perscodigo, string _telefono)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    List<SoftCob_TELEFONOS_CEDENTE> _dato = _db.SoftCob_TELEFONOS_CEDENTE.Where(c => c.tece_cedecodigo == _cedecodigo && c.tece_perscodigo == _perscodigo && c.tece_numero == _telefono).ToList();

                    if (_dato.Count > 0) _codigo = 1;
                    else _codigo = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _codigo;
        }

        public void FunRegistrarVolverLLamar(SoftCob_REGISTRO_VOLVERALLAMAR _regllamar)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_REGISTRO_VOLVERALLAMAR _original = _db.SoftCob_REGISTRO_VOLVERALLAMAR.Where(r => r.REVL_CODIGO == _regllamar.REVL_CODIGO && r.revl_cldecodigo == _regllamar.revl_cldecodigo).FirstOrDefault();

                    _db.SoftCob_REGISTRO_VOLVERALLAMAR.Attach(_original);
                    _original.revl_gestionado = _regllamar.revl_gestionado;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int FunGetPersCodigoListaDetalle(int _codigoltca, int _codigoclde, int _gestorasignado)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    return _db.SoftCob_LISTATRABAJO_DETALLE.Where(x => x.LTCA_CODIGO == _codigoltca && x.ltde_cldecodigo == _codigoclde && x.ltde_gestorasignado == _gestorasignado).FirstOrDefault().ltde_perscodigo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FunActualizarListaActiva(SoftCob_LISTATRABAJO_ACTIVAS _listatrab)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_LISTATRABAJO_ACTIVAS _original = _db.SoftCob_LISTATRABAJO_ACTIVAS.Where(x => x.lsac_listatrabajo == _listatrab.lsac_listatrabajo && x.lsac_gestorasignado == _listatrab.lsac_gestorasignado).FirstOrDefault();

                    _db.SoftCob_LISTATRABAJO_ACTIVAS.Attach(_original);
                    _original.lsac_estado = false;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FunActualizarListaDetalle(SoftCob_LISTATRABAJO_DETALLE _listadetalle)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_LISTATRABAJO_DETALLE _original = _db.SoftCob_LISTATRABAJO_DETALLE.Where(x => x.LTCA_CODIGO == _listadetalle.LTCA_CODIGO && x.LTDE_CODIGO == _listadetalle.LTDE_CODIGO).FirstOrDefault();
                    _db.SoftCob_LISTATRABAJO_DETALLE.Attach(_original);
                    _original.ltde_estado = _listadetalle.ltde_estado;
                    _original.ltde_gestionado = _listadetalle.ltde_gestionado;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FunUpdateListaTrabajo(SoftCob_LISTATRABAJO_CABECERA _listacabecera)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_LISTATRABAJO_CABECERA _original = _db.SoftCob_LISTATRABAJO_CABECERA.Where(x => x.LTCA_CODIGO == _listacabecera.LTCA_CODIGO).FirstOrDefault();
                    _db.SoftCob_LISTATRABAJO_CABECERA.Attach(_original);
                    _original.ltca_estado = _listacabecera.ltca_estado;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
        }

        public SoftCob_BRENCH FunGetBrenchAdminPorID(int _codigo)
        {
            SoftCob_BRENCH _datos = new SoftCob_BRENCH();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _datos = _db.SoftCob_BRENCH.Where(x => x.BRCH_CODIGO == _codigo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _datos;
        }

        public DataSet FunNewLstADE(int tipo, string sql, int cedecodigo, int cpcecodigo, int lstacodigo, int tipogestion,
            int efectiva, int araccodigo, int arefcodigo, int arrecodigo, int arcocodigo, int gestorasignado,
            string fechadesde, string fechahasta, string var1, string var2, string var3,
            string var4, int int1, int int2, int int3, int int4, DataTable dtbsave, string conexion)
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
                        comm.CommandText = "sp_NewListasTrabajoADE";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_sql", sql);
                        comm.Parameters.AddWithValue("@in_cedecodigo", cedecodigo);
                        comm.Parameters.AddWithValue("@in_cpcecodigo", cpcecodigo);
                        comm.Parameters.AddWithValue("@in_lstacodigo", lstacodigo);
                        comm.Parameters.AddWithValue("@in_tipogestion", tipogestion);
                        comm.Parameters.AddWithValue("@in_efectiva", efectiva);
                        comm.Parameters.AddWithValue("@in_araccodigo", araccodigo);
                        comm.Parameters.AddWithValue("@in_arefcodigo", arefcodigo);
                        comm.Parameters.AddWithValue("@in_arrecodigo", arrecodigo);
                        comm.Parameters.AddWithValue("@in_arcocodigo", arcocodigo);
                        comm.Parameters.AddWithValue("@in_gestorasignado", gestorasignado);
                        comm.Parameters.AddWithValue("@in_fechadesde", fechadesde);
                        comm.Parameters.AddWithValue("@in_fechahasta", fechahasta);
                        comm.Parameters.AddWithValue("@in_var1", var1);
                        comm.Parameters.AddWithValue("@in_var2", var2);
                        comm.Parameters.AddWithValue("@in_var3", var3);
                        comm.Parameters.AddWithValue("@in_var4", var4);
                        comm.Parameters.AddWithValue("@in_int1", int1);
                        comm.Parameters.AddWithValue("@in_int2", int2);
                        comm.Parameters.AddWithValue("@in_int3", int3);
                        comm.Parameters.AddWithValue("@in_int4", int4);
                        comm.Parameters.AddWithValue("@TablaGstSave", dtbsave);
                        _dap.SelectCommand = comm;
                        _dap.Fill(_dts);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _dts;
        }
        public DataSet FunGetBrenchAdmin()
        {
            try
            {
                var query = from datos in _db.SoftCob_BRENCH
                            join Cedente in _db.SoftCob_CEDENTE on datos.brch_cedecodigo equals Cedente.CEDE_CODIGO
                            join Catalogo in _db.SoftCob_CATALOGO_PRODUCTOS_CEDENTE on datos.brch_cpcecodigo equals Catalogo.CPCE_CODIGO
                            select new BrenchAdminDTO
                            {
                                Codigo = datos.BRCH_CODIGO,
                                Cedente = Cedente.cede_nombre,
                                Catalogo = Catalogo.cpce_producto,
                                Estado = datos.brch_estado == true ? "Activo" : "Inactivo",
                                auxv1 = datos.brch_auxv1,
                                auxv2 = datos.brch_auxv2,
                                auxv3 = datos.brch_auxv3,
                                auxi1 = (int)datos.brch_auxi1,
                                auxi2 = (int)datos.brch_auxi2,
                                auxi3 = (int)datos.brch_auxi3
                            };

                return _dts = new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
