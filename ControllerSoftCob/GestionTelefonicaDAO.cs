namespace ControllerSoftCob
{
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    public class GestionTelefonicaDAO
    {
        #region Variables
        DataSet _dts = new DataSet();
        SoftCobEntities _dtb = new SoftCobEntities();
        SqlDataAdapter _dap = new SqlDataAdapter();
        string _mensaje = "";
        int _codigo = 0;
        #endregion

        #region Procedimientos y Funciones
        public string FunRegistrarGestionTelefonica(int cedecodigo, int cpcecodigo, int ltcacodigo, int ltdecodigo,
            int cldecodigo, int perscodigo, string numerodocumento, int gestorasignado, string telefonogestionado,
            string tipotelefono, string propietario, int araccodigo, int arefcodigo, int arrecodigo, int arcocodigo,
            string observacion, bool efectivo, string fechagestion, string horagestion, string tracknum, string tiempogestion,
            string tiempollamada, int totalgestion, int totalllamada, string pago, string fechapago, string valorpago,
            string llamar, string fechallamar, string horallamar, string telefonollamar, int perfilactitu, int estilosnego,
            int metraprogramas, int modalidad, int estadosyo, int impulsores, string auxv1, string auxv2, string auxv3,
            string auxv4, int auxi1, int auxi2, int auxi3, int auxi4, DataTable dtbDatos, DataTable dtbTelefonos,
            int usucodigo, string terminal, string sp, string conexion)
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
                        comm.Parameters.AddWithValue("@in_cedecodigo", cedecodigo);
                        comm.Parameters.AddWithValue("@in_cpcecodigo", cpcecodigo);
                        comm.Parameters.AddWithValue("@in_ltcacodigo", ltcacodigo);
                        comm.Parameters.AddWithValue("@in_ltdecodigo", ltdecodigo);
                        comm.Parameters.AddWithValue("@in_cldecodigo", cldecodigo);
                        comm.Parameters.AddWithValue("@in_perscodigo", perscodigo);
                        comm.Parameters.AddWithValue("@in_numerodocumento", numerodocumento);
                        comm.Parameters.AddWithValue("@in_gestorasignado", gestorasignado);
                        comm.Parameters.AddWithValue("@in_telefonogestionado", telefonogestionado);
                        comm.Parameters.AddWithValue("@in_tipotelefono", tipotelefono);
                        comm.Parameters.AddWithValue("@in_propietario", propietario);
                        comm.Parameters.AddWithValue("@in_araccodigo", araccodigo);
                        comm.Parameters.AddWithValue("@in_arefcodigo", arefcodigo);
                        comm.Parameters.AddWithValue("@in_arrecodigo", arrecodigo);
                        comm.Parameters.AddWithValue("@in_arcocodigo", arcocodigo);
                        comm.Parameters.AddWithValue("@in_observacion", observacion);
                        comm.Parameters.AddWithValue("@in_efectivo", efectivo);
                        comm.Parameters.AddWithValue("@in_fechagestion", fechagestion);
                        comm.Parameters.AddWithValue("@in_horagestion", horagestion);
                        comm.Parameters.AddWithValue("@in_tracknum", tracknum);
                        comm.Parameters.AddWithValue("@in_tiempogestion", tiempogestion);
                        comm.Parameters.AddWithValue("@in_tiempollamada", tiempollamada);
                        comm.Parameters.AddWithValue("@in_totalgestion", totalgestion);
                        comm.Parameters.AddWithValue("@in_totalllamada", totalllamada);
                        comm.Parameters.AddWithValue("@in_pago", pago);
                        comm.Parameters.AddWithValue("@in_fechapago", fechapago);
                        comm.Parameters.AddWithValue("@in_valorpago", valorpago);
                        comm.Parameters.AddWithValue("@in_llamar", llamar);
                        comm.Parameters.AddWithValue("@in_fechallamar", fechallamar);
                        comm.Parameters.AddWithValue("@in_horallamar", horallamar);
                        comm.Parameters.AddWithValue("@in_telefonollamar", telefonollamar);
                        comm.Parameters.AddWithValue("@in_perfilactitudinal", perfilactitu);
                        comm.Parameters.AddWithValue("@in_estilosnegocia", estilosnego);
                        comm.Parameters.AddWithValue("@in_metraprograma", metraprogramas);
                        comm.Parameters.AddWithValue("@in_modalidad", modalidad);
                        comm.Parameters.AddWithValue("@in_estadosyo", estadosyo);
                        comm.Parameters.AddWithValue("@in_impulsores", impulsores);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxv4", auxv4);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        comm.Parameters.AddWithValue("@in_auxi4", auxi4);
                        comm.Parameters.AddWithValue("@TablaDatos", dtbDatos);
                        comm.Parameters.AddWithValue("@in_usucodigo", usucodigo);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
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

        public string FunRegistrarGestionEntrante(int cedecodigo, int cpcecodigo, int ltcacodigo, int ltdecodigo, int cldecodigo, int perscodigo, string numerodocumento, string operacion, int gestorasignado, string telefonogestionado, string tipotelefono, string propietario, int araccodigo, int arefcodigo, int arrecodigo, int arcocodigo, string observacion, bool efectivo, string fechagestion,
            string horagestion, string tracknum, string tiempogestion, string tiempollamada, int totalgestion, int totalllamada,
            string pago, string fechapago, string valorpago, string llamar, string fechallamar, string horallamar, string telefonollamar, int perfilactitu, int estilosnego, int metraprogramas, int modalidad, int estadosyo, int impulsores, string auxv1, string auxv2, string auxv3, string auxv4, int auxi1, int auxi2, int auxi3, int auxi4, DataTable dtbDatos, int usucodigo, string terminal, string sp, string conexion)
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
                        comm.Parameters.AddWithValue("@in_cedecodigo", cedecodigo);
                        comm.Parameters.AddWithValue("@in_cpcecodigo", cpcecodigo);
                        comm.Parameters.AddWithValue("@in_ltcacodigo", ltcacodigo);
                        comm.Parameters.AddWithValue("@in_ltdecodigo", ltdecodigo);
                        comm.Parameters.AddWithValue("@in_cldecodigo", cldecodigo);
                        comm.Parameters.AddWithValue("@in_perscodigo", perscodigo);
                        comm.Parameters.AddWithValue("@in_numerodocumento", numerodocumento);
                        comm.Parameters.AddWithValue("@in_operacion", operacion);
                        comm.Parameters.AddWithValue("@in_gestorasignado", gestorasignado);
                        comm.Parameters.AddWithValue("@in_telefonogestionado", telefonogestionado);
                        comm.Parameters.AddWithValue("@in_tipotelefono", tipotelefono);
                        comm.Parameters.AddWithValue("@in_propietario", propietario);
                        comm.Parameters.AddWithValue("@in_araccodigo", araccodigo);
                        comm.Parameters.AddWithValue("@in_arefcodigo", arefcodigo);
                        comm.Parameters.AddWithValue("@in_arrecodigo", arrecodigo);
                        comm.Parameters.AddWithValue("@in_arcocodigo", arcocodigo);
                        comm.Parameters.AddWithValue("@in_observacion", observacion);
                        comm.Parameters.AddWithValue("@in_efectivo", efectivo);
                        comm.Parameters.AddWithValue("@in_fechagestion", fechagestion);
                        comm.Parameters.AddWithValue("@in_horagestion", horagestion);
                        comm.Parameters.AddWithValue("@in_tracknum", tracknum);
                        comm.Parameters.AddWithValue("@in_tiempogestion", tiempogestion);
                        comm.Parameters.AddWithValue("@in_tiempollamada", tiempollamada);
                        comm.Parameters.AddWithValue("@in_totalgestion", totalgestion);
                        comm.Parameters.AddWithValue("@in_totalllamada", totalllamada);
                        comm.Parameters.AddWithValue("@in_pago", pago);
                        comm.Parameters.AddWithValue("@in_fechapago", fechapago);
                        comm.Parameters.AddWithValue("@in_valorpago", valorpago);
                        comm.Parameters.AddWithValue("@in_llamar", llamar);
                        comm.Parameters.AddWithValue("@in_fechallamar", fechallamar);
                        comm.Parameters.AddWithValue("@in_horallamar", horallamar);
                        comm.Parameters.AddWithValue("@in_telefonollamar", telefonollamar);
                        comm.Parameters.AddWithValue("@in_perfilactitudinal", perfilactitu);
                        comm.Parameters.AddWithValue("@in_estilosnegocia", estilosnego);
                        comm.Parameters.AddWithValue("@in_metraprograma", metraprogramas);
                        comm.Parameters.AddWithValue("@in_modalidad", modalidad);
                        comm.Parameters.AddWithValue("@in_estadosyo", estadosyo);
                        comm.Parameters.AddWithValue("@in_impulsores", impulsores);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxv4", auxv4);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        comm.Parameters.AddWithValue("@in_auxi4", auxi4);
                        comm.Parameters.AddWithValue("@TablaDatos", dtbDatos);
                        comm.Parameters.AddWithValue("@in_usucodigo", usucodigo);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
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

        public void FunCrearTelefonoCedente(SoftCob_TELEFONOS_CEDENTE _telefono)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_TELEFONOS_CEDENTE.Add(_telefono);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FunEditarTelefonoCedente(SoftCob_TELEFONOS_CEDENTE _telefonocedente)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_TELEFONOS_CEDENTE _original = _db.SoftCob_TELEFONOS_CEDENTE.Where(u => u.TECE_CODIGO == 
                    _telefonocedente.TECE_CODIGO).FirstOrDefault();

                    _db.SoftCob_TELEFONOS_CEDENTE.Attach(_original);
                    _original.TECE_CODIGO = _telefonocedente.TECE_CODIGO;
                    _original.tece_numero = _telefonocedente.tece_numero;
                    _original.tece_tipo = _telefonocedente.tece_tipo;
                    _original.tece_propietario = _telefonocedente.tece_propietario;
                    _original.tece_auxv1 = _telefonocedente.tece_auxv1;
                    _original.tece_auxv3 = _telefonocedente.tece_auxv3;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int FunCrearTelefonoReferencia(SoftCob_DEUDOR_REFERENCIAS _telefonoref)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_DEUDOR_REFERENCIAS.Add(_telefonoref);
                    _db.SaveChanges();
                    _codigo = _telefonoref.DERE_CODIGO;
                }
            }
            catch
            {
                _codigo = 0;
            }

            return _codigo;
        }

        public void FunModificarDeudorReferen(SoftCob_DEUDOR_REFERENCIAS _deudorref)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_DEUDOR_REFERENCIAS _original = _db.SoftCob_DEUDOR_REFERENCIAS.Where(d => d.DERE_CODIGO ==_deudorref.DERE_CODIGO &&
                    d.dere_numdocumento==_deudorref.dere_numdocumento).FirstOrDefault();

                    _db.SoftCob_DEUDOR_REFERENCIAS.Attach(_original);
                    _original.dere_tiporeferencia = _deudorref.dere_tiporeferencia;
                    _original.dere_nombrereferencia = _deudorref.dere_nombrereferencia;
                    _original.dere_apellidoreferencia = _deudorref.dere_apellidoreferencia;
                    _original.dere_fum = _deudorref.dere_fum;
                    _original.dere_uum = _deudorref.dere_uum;
                    _original.dere_tum = _deudorref.dere_tum;
                    _db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string FunRegistrarPerfilCalifica(int tipo, string auxv1, string auxv2, string auxv3, int auxi1,
            int auxi2, int auxi3, int usucodigo, string terminal, DataTable dtbPerfiles, string conexion)
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
                        comm.CommandText = "sp_NewPerfilesCalifica";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        comm.Parameters.AddWithValue("@in_usucodigo", usucodigo);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
                        comm.Parameters.AddWithValue("@TablaPerfiles", dtbPerfiles);
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

        public string FunConsultarFonoEstado(int cedecodigo, int codpersona, string telefono)
        {
            try
            {
                using (SoftCobEntities db = new SoftCobEntities())
                {
                    List<SoftCob_TELEFONOS_CEDENTE> phone = db.SoftCob_TELEFONOS_CEDENTE.Where(o => o.tece_cedecodigo == cedecodigo
                    && o.tece_perscodigo == codpersona && o.tece_numero == telefono && o.tece_estado == false).ToList();

                    if (phone.Count > 0) _mensaje = "Existe";
                    else _mensaje = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _mensaje;
        }

        public void FunCambiarEstadoTelefono(SoftCob_TELEFONOS_CEDENTE _telefonocedente)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_TELEFONOS_CEDENTE _original = _db.SoftCob_TELEFONOS_CEDENTE.Where(u => u.TECE_CODIGO ==
                    _telefonocedente.TECE_CODIGO).FirstOrDefault();

                    _db.SoftCob_TELEFONOS_CEDENTE.Attach(_original);
                    _original.tece_estado = _telefonocedente.tece_estado;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FunCrearNotasGestion(SoftCob_NOTAS_GESTION _datos)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_NOTAS_GESTION.Add(_datos);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void FunEditarNotasGestion(SoftCob_NOTAS_GESTION _datos)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_NOTAS_GESTION _original = _db.SoftCob_NOTAS_GESTION.Where(x => x.NOTA_CODIGO == 
                    _datos.NOTA_CODIGO).FirstOrDefault();

                    _db.SoftCob_NOTAS_GESTION.Attach(_original);
                    _original.nota_descripcion = _datos.nota_descripcion;
                    _original.nota_revisarfecha = _datos.nota_revisarfecha;
                    _original.nota_mantener = _datos.nota_mantener;
                    _original.nota_auxv1 = _datos.nota_auxv1;
                    _original.nota_auxv2 = _datos.nota_auxv2;
                    _original.nota_auxv3 = _datos.nota_auxv3;
                    _original.nota_auxi1 = _datos.nota_auxi1;
                    _original.nota_auxi2 = _datos.nota_auxi2;
                    _original.nota_auxi3 = _datos.nota_auxi3;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void FunDelNotasGestion(int _codigo)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_NOTAS_GESTION _datos = _db.SoftCob_NOTAS_GESTION.SingleOrDefault(x => x.NOTA_CODIGO == _codigo);

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
        }
        public DataSet FunGetNotasGestion(int codigoCEDE, int codigoCPCE, int codigoPERS, int codigoUSU)
        {
            try
            {
                var query = from NTG in _dtb.SoftCob_NOTAS_GESTION
                            where NTG.nota_cedecodigo == codigoCEDE && NTG.nota_cpcecodigo == codigoCPCE && 
                            NTG.nota_perscodigo == codigoPERS && NTG.nota_gestor == codigoUSU
                            select new NotasGestion
                            {
                                Codigo = NTG.NOTA_CODIGO,
                                Descripcion = NTG.nota_descripcion,
                                Fecha = NTG.nota_revisarfecha.Month.ToString().Length == 1 ? "0" + NTG.nota_revisarfecha.Month.ToString()
                                + "/" + NTG.nota_revisarfecha.Day.ToString() + "/" + NTG.nota_revisarfecha.Year.ToString() :
                                NTG.nota_revisarfecha.Month.ToString() + "/" + NTG.nota_revisarfecha.Day.ToString() + "/" + 
                                NTG.nota_revisarfecha.Year.ToString(),
                                Mantener = NTG.nota_mantener
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
