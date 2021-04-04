namespace ControllerSoftCob
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    public class ConsultaDatosDAO
    {
        #region Variables
        DataSet _dts = new DataSet();
        SqlDataAdapter _dap = new SqlDataAdapter();
        string _mensaje = "";
        #endregion

        #region Procedimientos y Funciones STORE PROCEDURE
        public DataSet FunConsultaDatos(int tipo, int int1, int int2, int int3, string str1, string str2, string str3, string conexion)
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
                        comm.CommandText = "sp_ConsultaDatos";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_int1", int1);
                        comm.Parameters.AddWithValue("@in_int2", int2);
                        comm.Parameters.AddWithValue("@in_int3", int3);
                        comm.Parameters.AddWithValue("@in_var1", str1);
                        comm.Parameters.AddWithValue("@in_var2", str2);
                        comm.Parameters.AddWithValue("@in_var3", str3);
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

        public DataSet FunConsultaDatosNew(int tipo, int emprcodigo, string auxv1, string auxv2, string auxv3, string auxv4, string auxv5,
            string auxv6, int auxi1, int auxi2, int auxi3, int auxi4, int auxi5, int auxi6, string conexion)
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
                        comm.CommandText = "sp_ConsultaDatosNew";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_emprcodigo", emprcodigo);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxv4", auxv4);
                        comm.Parameters.AddWithValue("@in_auxv5", auxv5);
                        comm.Parameters.AddWithValue("@in_auxv6", auxv6);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        comm.Parameters.AddWithValue("@in_auxi4", auxi4);
                        comm.Parameters.AddWithValue("@in_auxi5", auxi5);
                        comm.Parameters.AddWithValue("@in_auxi6", auxi6);
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

        public DataSet FunRegReasignaCartera(int tipo, int codigocedente, int cpcecodigo, string motivo, string observacion, int gestorasignado, int diasmora, string auxv1, string auxv2, string auxv3, int auxi1, int auxi2, int auxi3, int usucodigo, string terminal, string conexion)
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
                        comm.CommandText = "sp_NewReasignarCartera";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_cedecodigo", codigocedente);
                        comm.Parameters.AddWithValue("@in_cpcecodigo", cpcecodigo);
                        comm.Parameters.AddWithValue("@in_motivo", motivo);
                        comm.Parameters.AddWithValue("@in_observacion", observacion);
                        comm.Parameters.AddWithValue("@in_gestorasignado", gestorasignado);
                        comm.Parameters.AddWithValue("@in_diasmora", diasmora);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        comm.Parameters.AddWithValue("@in_usucodigo", usucodigo);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
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

        public string FunEditarTelefonos(int tipo, int perscodigo, int cldecodigo, string tipotelefono, string propietario,
            string nombreref, string apellidoref, string telefono, string prefijo, string telefonoanterior, string auxv1, string auxv2, string auxv3, int auxi1, int auxi2, int auxi3, int usucodigo, string terminal, string conexion)
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
                        comm.CommandText = "sp_ModificarTelefonos";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_perscodigo", perscodigo);
                        comm.Parameters.AddWithValue("@in_cldecodigo", cldecodigo);
                        comm.Parameters.AddWithValue("@in_tipotelefono", tipotelefono);
                        comm.Parameters.AddWithValue("@in_propietario", propietario);
                        comm.Parameters.AddWithValue("@in_nombrereferencia", nombreref);
                        comm.Parameters.AddWithValue("@in_apellidoreferencia", apellidoref);
                        comm.Parameters.AddWithValue("@in_telefono", telefono);
                        comm.Parameters.AddWithValue("@in_prefijo", prefijo);
                        comm.Parameters.AddWithValue("@in_telefonoanterior", telefonoanterior);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
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

        public string FunProcesoBrenchGestor(int tipo, int brmccodigo, int brmdcodigo, int gestorasignado,
            decimal porcentaje, decimal valortotal, string fecharegistro, int usuario, string terminal, string auxv1,
            string auxv2, string auxv3, string auxv4, string auxv5, int auxi1, int auxi2, int auxi3, int auxi4,
            int auxi5, string conexion)
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
                        comm.CommandText = "sp_NewBrenchGestor";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_brmccodigo", brmccodigo);
                        comm.Parameters.AddWithValue("@in_brmdcodigo", brmdcodigo);
                        comm.Parameters.AddWithValue("@in_gestor", gestorasignado);
                        comm.Parameters.AddWithValue("@in_porcentaje", porcentaje);
                        comm.Parameters.AddWithValue("@in_valortotal", valortotal);
                        comm.Parameters.AddWithValue("@in_fecharegistro", fecharegistro);
                        comm.Parameters.AddWithValue("@in_usucodigo", usuario);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxv4", auxv3);
                        comm.Parameters.AddWithValue("@in_auxv5", auxv3);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        comm.Parameters.AddWithValue("@in_auxi4", auxi3);
                        comm.Parameters.AddWithValue("@in_auxi5", auxi3);
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

        public DataSet FunReporteAccionTelefono(int tipo, int gestor, string buscarpor, string criterio, int porfecha,
            string fechadesde, string fechahasta, int motivo, string auxv1, string auxv2, string auxv3, int auxi1, int auxi2,
            int auxi3, string conexion)
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
                        comm.CommandText = "sp_AccionTelefono";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_gestor", gestor);
                        comm.Parameters.AddWithValue("@in_buscarpor", buscarpor);
                        comm.Parameters.AddWithValue("@in_criterio", criterio);
                        comm.Parameters.AddWithValue("@in_porfecha", porfecha);
                        comm.Parameters.AddWithValue("@in_fechadesde", fechadesde);
                        comm.Parameters.AddWithValue("@in_fechahasta", fechahasta);
                        comm.Parameters.AddWithValue("@in_motivo", motivo);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        _dap.SelectCommand = comm;
                        _dap.Fill(_dts);
                    }
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _dts;
        }

        public DataSet FunNewProtocolo(int tipo, int codigoprev, int codigoprotocolo, int codigopadre,
            string descripcion, string estado, int calificacion, string nivel, string auxv1, string auxv2, string auxv3,
            string auxv4, int auxi1, int auxi2, int auxi3, int auxi4, int usucodigo, string terminal, string conexion)
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
                        comm.CommandText = "sp_NewProtocolos";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_codigoprev", codigoprev);
                        comm.Parameters.AddWithValue("@in_codigoprotocolo", codigoprotocolo);
                        comm.Parameters.AddWithValue("@in_codigopadre", codigopadre);
                        comm.Parameters.AddWithValue("@in_descripcion", descripcion);
                        comm.Parameters.AddWithValue("@in_estado", estado);
                        comm.Parameters.AddWithValue("@in_califica", calificacion);
                        comm.Parameters.AddWithValue("@in_nivel", nivel);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxv4", auxv4);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        comm.Parameters.AddWithValue("@in_auxi4", auxi4);
                        comm.Parameters.AddWithValue("@in_usucodigo", usucodigo);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
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

        public DataSet FunNewEvaluacion(int tipo, int codigoevca, string nombre, string descripcion, string fechainicio,
            string fechafin, string estado, string auxv1, string auxv2, string auxv3, int auxi1, int auxi2,
            int auxi3, int usucodigo, string terminal, string conexion)
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
                        comm.CommandText = "sp_NewEvaluacion";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_codigoevca", codigoevca);
                        comm.Parameters.AddWithValue("@in_nombre", nombre);
                        comm.Parameters.AddWithValue("@in_descripcion", descripcion);
                        comm.Parameters.AddWithValue("@in_fechainicio", fechainicio);
                        comm.Parameters.AddWithValue("@in_fechafin", fechafin);
                        comm.Parameters.AddWithValue("@in_estado", estado);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        comm.Parameters.AddWithValue("@in_usucodigo", usucodigo);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
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

        public DataSet FunNewEvaluacionDesempenio(int tipo, int codigoevca, int codigopadre, int codigoprotocolo,
            int codigodepartamento, int califica, string auxv1, string auxv2, string auxv3, int auxi1, int auxi2,
            int auxi3, int usucodigo, string terminal, string conexion)
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
                        comm.CommandText = "sp_NewEvaluacionDesem";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_evcacodigo", codigoevca);
                        comm.Parameters.AddWithValue("@in_codigopadre", codigopadre);
                        comm.Parameters.AddWithValue("@in_codigoprotocolo", codigoprotocolo);
                        comm.Parameters.AddWithValue("@in_codigodepartamento", codigodepartamento);
                        comm.Parameters.AddWithValue("@in_califica", califica);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        comm.Parameters.AddWithValue("@in_usucodigo", usucodigo);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
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

        public DataSet FunCrearNuevoDeudor(int tipo, string tipodocumento, string numerodocumento,
            string nombres, string apellidos, string fechanac, string genero, int provincia, int ciudad,
            string estadocivil, string estado, int cpcecodigo, string operacion, string totaldeuda, string exigible,
            int diasmora, int gestor, string tipooperacion, string definicion, string tipotelefono, string prefijo,
            string telefono, string propietario, string garante, string domicilio, string refdomicilio, string trabajo,
            string reftrabajo, int usucodigo, string terminal, string auxv1, string auxv2, string auxv3, int auxi1, int auxi2,
            int auxi3, string conexion)
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
                        comm.CommandText = "sp_CrearNuevoDeudor";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_tipodocumento", tipodocumento);
                        comm.Parameters.AddWithValue("@in_numerodocumento", numerodocumento);
                        comm.Parameters.AddWithValue("@in_nombres", nombres);
                        comm.Parameters.AddWithValue("@in_apellidos", apellidos);
                        comm.Parameters.AddWithValue("@in_fechanaci", fechanac);
                        comm.Parameters.AddWithValue("@in_genero", genero);
                        comm.Parameters.AddWithValue("@in_provincia", provincia);
                        comm.Parameters.AddWithValue("@in_ciudad", ciudad);
                        comm.Parameters.AddWithValue("@in_estadocivil", estadocivil);
                        comm.Parameters.AddWithValue("@in_estado", estado);
                        comm.Parameters.AddWithValue("@in_cpcecodigo", cpcecodigo);
                        comm.Parameters.AddWithValue("@in_operacion", operacion);
                        comm.Parameters.AddWithValue("@in_totaldeuda", totaldeuda.Replace(",", "."));
                        comm.Parameters.AddWithValue("@in_exigible", exigible.Replace(",", "."));
                        comm.Parameters.AddWithValue("@in_diasmora", diasmora);
                        comm.Parameters.AddWithValue("@in_gestor", gestor);
                        comm.Parameters.AddWithValue("@in_tipooperacion", tipooperacion);
                        comm.Parameters.AddWithValue("@in_definicion", definicion);
                        comm.Parameters.AddWithValue("@in_tipotelefono", tipotelefono);
                        comm.Parameters.AddWithValue("@in_prefijo", prefijo);
                        comm.Parameters.AddWithValue("@in_telefono", telefono);
                        comm.Parameters.AddWithValue("@in_propietario", propietario);
                        comm.Parameters.AddWithValue("@in_garante", garante);
                        comm.Parameters.AddWithValue("@in_domicilio", domicilio);
                        comm.Parameters.AddWithValue("@in_refdomicilio", refdomicilio);
                        comm.Parameters.AddWithValue("@in_trabajo", trabajo);
                        comm.Parameters.AddWithValue("@in_reftrabajo", reftrabajo);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        comm.Parameters.AddWithValue("@in_usucodigo", usucodigo);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
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

        public DataSet FunHorariobpm(int tipo, int horacodigo, string horario, string descripcion, string intervalo,
            string horadesde, string horahasta, string estado, int orden, string auxv1, string auxv2, string auxv3,
            int auxi1, int auxi2, int auxi3, int usucodigo, string terminal, string conexion)
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
                        comm.CommandText = "sp_Horariobmp";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_horacodigo", horacodigo);
                        comm.Parameters.AddWithValue("@in_horario", horario);
                        comm.Parameters.AddWithValue("@in_descripcion", descripcion);
                        comm.Parameters.AddWithValue("@in_intervalo", intervalo);
                        comm.Parameters.AddWithValue("@in_horadesde", horadesde);
                        comm.Parameters.AddWithValue("@in_horahasta", horahasta);
                        comm.Parameters.AddWithValue("@in_estado", estado);
                        comm.Parameters.AddWithValue("@in_orden", orden);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        comm.Parameters.AddWithValue("@in_usucodigo", usucodigo);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
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

        public DataSet FunAgendaCitaciones(int tipo, int cldecodigo, int perscodigo, string fecha, int horacodigo,
            string valor, int usuario, string observacion, string tipoconvenio, string direccion, string referencia,
            string telefono, string celular, string xwhastapp, string xemail, string xterreno, string email,
            byte[] documento, string pathdocumento, string nomdocumento, string extdocumento, string contentdocumento,
            string totalconvenio, string cuotas, int numcuotas, string numjuicio, int plazo,
            string auxv1, string auxv2, string auxv3, string auxv4, string auxv5, string auxv6, string auxv7, string auxv8,
            string auxv9, string auxv10, int auxi1, int auxi2, int auxi3, int auxi4, int auxi5, int usuacodigo,
            string terminal, string conexion)
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
                        comm.CommandText = "sp_AgendaCitaciones";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_cldecodigo", cldecodigo);
                        comm.Parameters.AddWithValue("@in_perscodigo", perscodigo);
                        comm.Parameters.AddWithValue("@in_fecha", fecha);
                        comm.Parameters.AddWithValue("@in_horacodigo", horacodigo);
                        comm.Parameters.AddWithValue("@in_valor", valor);
                        comm.Parameters.AddWithValue("@in_usuario", usuario);
                        comm.Parameters.AddWithValue("@in_observacion", observacion);
                        comm.Parameters.AddWithValue("@in_tipoconvenio", tipoconvenio);
                        comm.Parameters.AddWithValue("@in_direccion", direccion);
                        comm.Parameters.AddWithValue("@in_referencia", referencia);
                        comm.Parameters.AddWithValue("@in_telefono", telefono);
                        comm.Parameters.AddWithValue("@in_celular", celular);
                        comm.Parameters.AddWithValue("@in_xwhatsapp", xwhastapp);
                        comm.Parameters.AddWithValue("@in_xemail", xemail);
                        comm.Parameters.AddWithValue("@in_xterreno", xterreno);
                        comm.Parameters.AddWithValue("@in_email", email);
                        comm.Parameters.AddWithValue("@in_documento", documento);
                        comm.Parameters.AddWithValue("@in_pathdocumento", pathdocumento);
                        comm.Parameters.AddWithValue("@in_nomdocumento", nomdocumento);
                        comm.Parameters.AddWithValue("@in_extdocumento", extdocumento);
                        comm.Parameters.AddWithValue("@in_contentdocumento", contentdocumento);
                        comm.Parameters.AddWithValue("@in_totalconvenio", totalconvenio);
                        comm.Parameters.AddWithValue("@in_cuotas", cuotas);
                        comm.Parameters.AddWithValue("@in_numcuotas", numcuotas);
                        comm.Parameters.AddWithValue("@in_numjuicio", numjuicio);
                        comm.Parameters.AddWithValue("@in_plazo", plazo);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxv4", auxv4);
                        comm.Parameters.AddWithValue("@in_auxv5", auxv5);
                        comm.Parameters.AddWithValue("@in_auxv6", auxv6);
                        comm.Parameters.AddWithValue("@in_auxv7", auxv7);
                        comm.Parameters.AddWithValue("@in_auxv8", auxv8);
                        comm.Parameters.AddWithValue("@in_auxv9", auxv9);
                        comm.Parameters.AddWithValue("@in_auxv10", auxv10);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        comm.Parameters.AddWithValue("@in_auxi4", auxi4);
                        comm.Parameters.AddWithValue("@in_auxi5", auxi5);
                        comm.Parameters.AddWithValue("@in_usuacodigo", usuacodigo);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
                        _dap.SelectCommand = comm;
                        _dap.Fill(_dts);
                    }
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _dts;
        }

        public DataSet FunUpdateDeudor(int tipo, int codigopers, string tipodocumento, string numerodocumento,
            string nombres, string apellidos, string fechanac, string genero, string estadocivil, int provincia, int ciudad,
            string dirdomicilio, string refdomicilio, string dirtrabajo, string reftrabajo, string email,
            string auxv1, string auxv2, string auxv3, int auxi1, int auxi2, int auxi3, int usucodigo,
            string terminal, string conexion)
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
                        comm.CommandText = "sp_UpdateDeudor";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_codigopers", codigopers);
                        comm.Parameters.AddWithValue("@in_tipodocumento", tipodocumento);
                        comm.Parameters.AddWithValue("@in_numerodocumento", numerodocumento);
                        comm.Parameters.AddWithValue("@in_nombres", nombres);
                        comm.Parameters.AddWithValue("@in_apellidos", apellidos);
                        comm.Parameters.AddWithValue("@in_fecnaci", fechanac);
                        comm.Parameters.AddWithValue("@in_genero", genero);
                        comm.Parameters.AddWithValue("@in_estcivil", estadocivil);
                        comm.Parameters.AddWithValue("@in_provincia", provincia);
                        comm.Parameters.AddWithValue("@in_ciudad", ciudad);
                        comm.Parameters.AddWithValue("@in_dirdomicilio", dirdomicilio);
                        comm.Parameters.AddWithValue("@in_refdomicilio", refdomicilio);
                        comm.Parameters.AddWithValue("@in_dirtrabajo", dirtrabajo);
                        comm.Parameters.AddWithValue("@in_reftrabajo", reftrabajo);
                        comm.Parameters.AddWithValue("@in_email", email);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        comm.Parameters.AddWithValue("@in_usucodigo", usucodigo);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
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

        public DataSet FunNewArbolDecision(int tipo, int cpcecodigo, int araccodigo, int arefcodigo, int arrecodigo,
            int arcocodigo, string descripcion, string estado, string efectivo, int respuesta, string comisiona,
            string auxv1, string auxv2, string auxv3, string auxv4, string auxv5, int auxi1, int auxi2, int auxi3,
            int auxi4, int auxi5, int usucodigo, string terminal, string conexion)
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
                        comm.Parameters.AddWithValue("@in_cpcecodigo", cpcecodigo);
                        comm.Parameters.AddWithValue("@in_araccodigo", araccodigo);
                        comm.Parameters.AddWithValue("@in_arefcodigo", arefcodigo);
                        comm.Parameters.AddWithValue("@in_arrecodigo", arrecodigo);
                        comm.Parameters.AddWithValue("@in_arcocodigo", arcocodigo);
                        comm.Parameters.AddWithValue("@in_descripcion", descripcion);
                        comm.Parameters.AddWithValue("@in_estado", estado);
                        comm.Parameters.AddWithValue("@in_efectivo", efectivo);
                        comm.Parameters.AddWithValue("@in_respuesta", respuesta);
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
                        comm.Parameters.AddWithValue("@in_usucodigo", usucodigo);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
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

        public DataSet FunRepGerencialG(int tipo, int cedecodigo, int cpcecodigo, string fechadesde, string fechahasta, int gestor, string sp, string auxv1, string auxv2, string auxv3, string auxv4, string auxv5, string auxv6, int auxi1, int auxi2, int auxi3, int auxi4, int auxi5, int auxi6, string conexion)
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
                        comm.Parameters.AddWithValue("@in_cedecodigo", cedecodigo);
                        comm.Parameters.AddWithValue("@in_cpcecodigo", cpcecodigo);
                        comm.Parameters.AddWithValue("@in_fechadesde", fechadesde);
                        comm.Parameters.AddWithValue("@in_fechahasta", fechahasta);
                        comm.Parameters.AddWithValue("@in_gestor", gestor);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxv4", auxv4);
                        comm.Parameters.AddWithValue("@in_auxv5", auxv5);
                        comm.Parameters.AddWithValue("@in_auxv6", auxv6);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        comm.Parameters.AddWithValue("@in_auxi4", auxi4);
                        comm.Parameters.AddWithValue("@in_auxi5", auxi5);
                        comm.Parameters.AddWithValue("@in_auxi6", auxi6);
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
        public DataSet FunActualizarDatos(int tipo, int cambio, int perscodigo, string cedulatitular, string cedulagarante,
            string email, string direccion, string referencia, string dirtrabajo, string reftrabajo, string mailtrabajo,
            string auxv1, string auxv2, string auxv3, int auxi1, int auxi2, int auxi3, string conexion)
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
                        comm.CommandText = "sp_ActualizarDatos";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_cambio", cambio);
                        comm.Parameters.AddWithValue("@in_perscodigo", perscodigo);
                        comm.Parameters.AddWithValue("@in_cedulatitular", cedulatitular);
                        comm.Parameters.AddWithValue("@in_cedulagarante", cedulagarante);
                        comm.Parameters.AddWithValue("@in_email", email);
                        comm.Parameters.AddWithValue("@in_direccion", direccion);
                        comm.Parameters.AddWithValue("@in_referencia", referencia);
                        comm.Parameters.AddWithValue("@in_dirtrabajo", dirtrabajo);
                        comm.Parameters.AddWithValue("@in_reftrabajo", reftrabajo);
                        comm.Parameters.AddWithValue("@in_emailtrabajo", mailtrabajo);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
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
        public DataSet FunDatosAdicionales(int codigocede, int codigocpce, int codigopers, string operacion,
            string auxv1, string auxv2, string auxv3, int auxi1, int auxi2, int auxi3, string conexion)
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
                        comm.CommandText = "sp_DatosAdicionales";
                        comm.Parameters.AddWithValue("@in_cedecodigo", codigocede);
                        comm.Parameters.AddWithValue("@in_cpccodigo", codigocpce);
                        comm.Parameters.AddWithValue("@in_perscodigo", codigopers);
                        comm.Parameters.AddWithValue("@in_operacion", operacion);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
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
        public DataSet FunMenuEditUpdate(string _check, int _codigomenu, int _codigotarea, int _emprcodigo, string _auxv1, string _auxv2, 
            string _auxv3, int _auxi1, int _auxi2, int _auxi3, int _usucodigo, string _terminal, string _conexion)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_conexion))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = con;
                        comm.CommandTimeout = 9000;
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = "sp_MenuEditUpdate";
                        comm.Parameters.AddWithValue("@in_check", _check);
                        comm.Parameters.AddWithValue("@in_codigomenu", _codigomenu);
                        comm.Parameters.AddWithValue("@in_codigotarea", _codigotarea);
                        comm.Parameters.AddWithValue("@in_emprcodigo", _emprcodigo);
                        comm.Parameters.AddWithValue("@in_auxv1", _auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", _auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", _auxv3);
                        comm.Parameters.AddWithValue("@in_auxi1", _auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", _auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", _auxi3);
                        comm.Parameters.AddWithValue("@in_usucodigo", _usucodigo);
                        comm.Parameters.AddWithValue("@in_terminal", _terminal);
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
        public string FunCrearArbolDecision(int codigocatalago, int codigoaccion, string descripcion, bool estado, bool contacto, string auxv1,
            string auxv2, int auxi1, int auxi2, int usucodigo, string terminal, DataTable dtbEfecto, DataTable dtbRespuesta, DataTable dtbContacto,
            string sp, string conexion)
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
                        comm.Parameters.AddWithValue("@in_codigocatalogo", codigocatalago);
                        comm.Parameters.AddWithValue("@in_codigoaccion", codigoaccion);
                        comm.Parameters.AddWithValue("@in_descripcion", descripcion);
                        comm.Parameters.AddWithValue("@in_estado", estado);
                        comm.Parameters.AddWithValue("@in_contacto", contacto);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_usucodigo", usucodigo);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
                        comm.Parameters.AddWithValue("@TablaEfecto", dtbEfecto);
                        comm.Parameters.AddWithValue("@TablaRespuesta", dtbRespuesta);
                        comm.Parameters.AddWithValue("@TablaContacto", dtbContacto);
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
        public string FunCrearArbolScore(int cedecodigo, int cpcecodigo, string auxv1, string auxv2, int auxi1, int auxi2,
            int usucodigo, string terminal, DataTable dtbArbolScore, string sp, string conexion)
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
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_usucodigo", usucodigo);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
                        comm.Parameters.AddWithValue("@TablaScore", dtbArbolScore);
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
        #endregion
    }
}
