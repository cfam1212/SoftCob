namespace ControllerSoftCob
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
        SoftCobEntities _dtb = new SoftCobEntities();
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
                    if (_opcion == 0) _respuesta = _db.SoftCob_RESPUESTA.Where(r => r.ARRE_CODIGO == _arrecodigo 
                    && r.arre_estado).FirstOrDefault().arre_pago;

                    if (_opcion == 1) _respuesta = _db.SoftCob_RESPUESTA.Where(r => r.ARRE_CODIGO == _arrecodigo 
                    && r.arre_estado).FirstOrDefault().arre_llamar;

                    if (_opcion == 2) _respuesta = bool.Parse(_db.SoftCob_RESPUESTA.Where(r => r.ARRE_CODIGO == _arrecodigo 
                    && r.arre_estado == true).FirstOrDefault().arre_auxv2);
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
                    List<SoftCob_TELEFONOS_CEDENTE> _dato = _db.SoftCob_TELEFONOS_CEDENTE.Where(c => c.tece_cedecodigo == _cedecodigo
                    && c.tece_perscodigo == _perscodigo && c.tece_numero == _telefono).ToList();

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
                    SoftCob_REGISTRO_VOLVERALLAMAR _original = _db.SoftCob_REGISTRO_VOLVERALLAMAR.Where(r => 
                    r.REVL_CODIGO == _regllamar.REVL_CODIGO && r.revl_cldecodigo == _regllamar.revl_cldecodigo).FirstOrDefault();

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
                    return _db.SoftCob_LISTATRABAJO_DETALLE.Where(x => x.LTCA_CODIGO == _codigoltca && x.ltde_cldecodigo == _codigoclde
                    && x.ltde_gestorasignado == _gestorasignado).FirstOrDefault().ltde_perscodigo;
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
                    SoftCob_LISTATRABAJO_ACTIVAS _original = _db.SoftCob_LISTATRABAJO_ACTIVAS.Where(x => x.lsac_listatrabajo ==
                    _listatrab.lsac_listatrabajo && x.lsac_gestorasignado == _listatrab.lsac_gestorasignado).FirstOrDefault();

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
                    SoftCob_LISTATRABAJO_DETALLE _original = _db.SoftCob_LISTATRABAJO_DETALLE.Where(x => x.LTCA_CODIGO ==
                    _listadetalle.LTCA_CODIGO && x.LTDE_CODIGO == _listadetalle.LTDE_CODIGO).FirstOrDefault();
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
                    SoftCob_LISTATRABAJO_CABECERA _original = _db.SoftCob_LISTATRABAJO_CABECERA.Where(x => x.LTCA_CODIGO == 
                    _listacabecera.LTCA_CODIGO).FirstOrDefault();
                    _db.SoftCob_LISTATRABAJO_CABECERA.Attach(_original);
                    _original.ltca_estado = _listacabecera.ltca_estado;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                var query = from datos in _dtb.SoftCob_BRENCH
                            join Cedente in _dtb.SoftCob_CEDENTE on datos.brch_cedecodigo equals Cedente.CEDE_CODIGO
                            join Catalogo in _dtb.SoftCob_CATALOGO_PRODUCTOS_CEDENTE on datos.brch_cpcecodigo equals Catalogo.CPCE_CODIGO
                            select new BrenchAdminDTO
                            {
                                Codigo = datos.BRCH_CODIGO,
                                Cedente = Cedente.cede_nombre,
                                Catalogo = Catalogo.cpce_producto,
                                Estado = datos.brch_estado == true ? "Activo" : "Inactivo",
                                Auxv1 = datos.brch_auxv1,
                                Auxv2 = datos.brch_auxv2,
                                Auxv3 = datos.brch_auxv3,
                                Auxi1 = (int)datos.brch_auxi1,
                                Auxi2 = (int)datos.brch_auxi2,
                                Auxi3 = (int)datos.brch_auxi3,
                                Urllink = "WFrm_NuevoBrench.aspx?CodigoBrench=" + datos.BRCH_CODIGO
                            };

                return _dts = new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetBrenchDet(int codigo)
        {
            try
            {
                var query = from datos in _dtb.SoftCob_BRENCHDET
                            where datos.BRCH_CODIGO == codigo
                            orderby datos.brde_orden
                            select new NuevoBrenchDTO
                            {
                                Codigo = datos.BRDE_CODIGO.ToString(),
                                RangoIni = datos.brde_rangoinicial,
                                RangoFin = datos.brde_rangofinal,
                                Etiqueta = datos.brde_etiqueta,
                                Orden = datos.brde_orden.ToString(),
                                Estado = datos.brde_estado == true ? "Activo" : "Inactivo",
                                Auxv1 = datos.brde_auxv1,
                                Auxv2 = datos.brde_auxv2,
                                Auxv3 = datos.brde_auxv3,
                                Auxi1 = (int)datos.brde_auxi1,
                                Auxi2 = (int)datos.brde_auxi1,
                                Auxi3 = (int)datos.brde_auxi3
                            };

                return _dts = new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetBrenchDetPorID(int codigoBRCH, int codigoBRDE)
        {
            try
            {
                var query = from datos in _dtb.SoftCob_BRENCHDET
                            where datos.BRCH_CODIGO == codigoBRCH && datos.BRDE_CODIGO == codigoBRDE
                            select new NuevoBrenchDTO
                            {
                                Estado = datos.brde_estado == true ? "Activo" : "Inactivo"
                            };

                return _dts = new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetArbolRespuesta(int cpcecodigo)
        {
            try
            {
                var query = from Respuesta in _dtb.SoftCob_RESPUESTA
                            where Respuesta.arre_estado == true &&
                            Respuesta.arre_auxi1 == cpcecodigo
                            select new ArbolContactoEfectivo
                            {
                                Codigo = Respuesta.ARRE_CODIGO,
                                Descripcion = Respuesta.arre_descripcion,
                                Contacto = Respuesta.arre_auxi2 == 1 ? true : false
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetListaTrabajo(int idListaTrabajo)
        {
            try
            {
                var query = from Lista in _dtb.SoftCob_LISTATRABAJO_CABECERA
                            join Cedente in _dtb.SoftCob_CEDENTE on Lista.ltca_cedecodigo equals Cedente.CEDE_CODIGO
                            join Catalogo in _dtb.SoftCob_CATALOGO_PRODUCTOS_CEDENTE on Lista.ltca_cpcecodigo equals Catalogo.CPCE_CODIGO
                            join Producto in _dtb.SoftCob_PRODUCTOS_CEDENTE on Catalogo.PRCE_CODIGO equals Producto.PRCE_CODIGO
                            where Lista.LTCA_CODIGO == idListaTrabajo && Lista.ltca_estado == true
                            select new ListaTrabajoAdminDTO
                            {
                                Codigo = Lista.LTCA_CODIGO,
                                Lista = Lista.ltca_lista,
                                FechaInicio = Lista.ltca_fechainicio.ToString(),
                                FechaFin = Lista.ltca_fechafin.ToString(),
                                CedeCodigo = Lista.ltca_cedecodigo,
                                Cedente = Cedente.cede_nombre,
                                PrceCodigo = Producto.PRCE_CODIGO,
                                Producto = Producto.prce_producto,
                                CpceCodigo = Lista.ltca_cpcecodigo,
                                Catalogo = Catalogo.cpce_producto,
                                TipoMarcado = Lista.ltca_tipomarcado,
                                Operaciones = (int)Lista.ltca_auxi1,
                                GestorApoyo = Lista.ltca_auxv3
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FunGetTelefonoPredicitivoPorId(int cedecodigo, int perscodigo)
        {
            try
            {
                var query = from Telefono in _dtb.SoftCob_TELEFONOS_CEDENTE
                            where Telefono.tece_cedecodigo == cedecodigo && Telefono.tece_perscodigo == perscodigo && Telefono.tece_estado
                            orderby Telefono.tece_score descending, Telefono.tece_numero ascending
                            select new TelefonoPredictivo
                            {
                                Telefono = Telefono.tece_numero,
                                Tipo = Telefono.tece_tipo,
                                Propietario = Telefono.tece_propietario,
                                Score = Telefono.tece_score,
                                Prefijo = Telefono.tece_auxv1
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FunGetSpeech(int tipo, int cedecodigo, int cpcecodigo, int araccodigo, int arefcodigo,
            int arrecodigo, int arcocodigo, int spcacodigo)
        {
            try
            {
                switch (tipo)
                {
                    case 0:
                        var query = from Speech in _dtb.SoftCob_SPEECH_CABECERA
                                    where Speech.spca_cedecodigo == cedecodigo && Speech.spca_cpcecodigo == cpcecodigo
                                    select new SpeechGenerado
                                    {
                                        Texto = Speech.spca_speechbv,
                                        Observa = "",
                                        CodigoSpeech = Speech.SPCA_CODIGO
                                    };

                        _dts = new FuncionesDAO().FunCambiarDataSet(query.ToList());
                        break;
                    case 1:
                        var query1 = from SpeechDeta in _dtb.SoftCob_SPEECH_DETALLE
                                     where SpeechDeta.SPCA_CODIGO == spcacodigo && SpeechDeta.spde_araccodigo == araccodigo
                                     && SpeechDeta.spde_arefcodigo == arefcodigo && SpeechDeta.spde_arrecodigo == arrecodigo
                                     && SpeechDeta.spde_arcocodigo == arcocodigo
                                     select new SpeechGenerado
                                     {
                                         Texto = SpeechDeta.spde_speechad,
                                         Observa = SpeechDeta.spde_observacion,
                                         CodigoSpeech = SpeechDeta.SPDE_CODIGO
                                     };
                        _dts = new FuncionesDAO().FunCambiarDataSet(query1.ToList());
                        break;
                }

                return _dts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetArbolContacto(int cpcecodigo)
        {
            try
            {
                var query = from Accion in _dtb.SoftCob_ACCION
                            where Accion.arac_estado == true
                            select new ArbolContactoEfectivo
                            {
                                Codigo = Accion.ARAC_CODIGO,
                                Descripcion = Accion.arac_descripcion,
                                Contacto = Accion.arac_contacto
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FunGetPerfilActitudinal()
        {
            try
            {
                var query = from Actitudinal in _dtb.SoftCob_PERFIL_ACTITUDINAL
                            where Actitudinal.peac_estado == true
                            select new VariablesBlandas
                            {
                                Codigo = Actitudinal.PEAC_CODIGO,
                                Descripcion = Actitudinal.peac_descripcion
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FunGetEstilosNegociacion()
        {
            try
            {
                var query = from Estilos in _dtb.SoftCob_ESTILOS_NEGOCIACION
                            where Estilos.peen_estado == true
                            select new VariablesBlandas
                            {
                                Codigo = Estilos.PEEN_CODIGO,
                                Descripcion = Estilos.peen_descripcion
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FunGetMetaprogramas()
        {
            try
            {
                var query = from Metap in _dtb.SoftCob_METAPROGRAMAS
                            where Metap.pemp_estado == true
                            select new VariablesBlandas
                            {
                                Codigo = Metap.PEMP_CODIGO,
                                Descripcion = Metap.pemp_descripcion
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FunGetModalidades()
        {
            try
            {
                var query = from Modali in _dtb.SoftCob_MODALIDADES
                            where Modali.pemo_estado == true
                            select new VariablesBlandas
                            {
                                Codigo = Modali.PEMO_CODIGO,
                                Descripcion = Modali.pemo_descripcion
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FunGetEstadosYo()
        {
            try
            {
                var query = from Estados in _dtb.SoftCob_ESTADOS_DELYO
                            where Estados.peey_estado == true
                            select new VariablesBlandas
                            {
                                Codigo = Estados.PEEY_CODIGO,
                                Descripcion = Estados.peey_descripcion
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FunGetImpulsores()
        {
            try
            {
                var query = from Impulsores in _dtb.SoftCob_IMPULSORES
                            where Impulsores.peip_estado == true
                            select new VariablesBlandas
                            {
                                Codigo = Impulsores.PEIP_CODIGO,
                                Descripcion = Impulsores.peip_descripcion
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FunGetPerfilDeudor(int perscodigo)
        {
            try
            {
                var query = from Deudor in _dtb.SoftCob_PERFIL_DEUDOR
                            where Deudor.pede_perscodigo == perscodigo
                            select new PerfilDeudor
                            {
                                Codigo = Deudor.PEDE_CODIGO,
                                PerfActitudinal = Deudor.pede_peaccodigo,
                                EstilosNegocia = Deudor.pede_peencodigo,
                                Metaprogramas = Deudor.pede_pempcodigo,
                                Modalidades = Deudor.pede_pemocodigo,
                                EstadosYo = Deudor.pede_peeycodigo,
                                Impulsores = Deudor.pede_peipcodigo
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        #endregion
    }
}
