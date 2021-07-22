namespace ControllerSoftCob
{
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    public class SpeechDAO
    {
        #region Variables
        DataSet _dts = new DataSet();
        SoftCobEntities _dtb = new SoftCobEntities();
        SqlDataAdapter _dap = new SqlDataAdapter();
        List<CatalogosDTO> _catalogo = new List<CatalogosDTO>();
        string _mensaje = "";
        #endregion

        #region Procedimientos y Funciones
        public List<SoftCob_CAMPOS_SPEECH> FunGetCamposSpeech()
        {
            List<SoftCob_CAMPOS_SPEECH> _campos = new List<SoftCob_CAMPOS_SPEECH>();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _campos = _db.SoftCob_CAMPOS_SPEECH.Where(t => t.casp_estado == true).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _campos;
        }

        public DataSet FunGetArbolNewAccion(int _codigocpce)
        {
            List<SoftCob_ARBOL_ACCION> _campos = new List<SoftCob_ARBOL_ACCION>();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _campos = _db.SoftCob_ARBOL_ACCION.Where(t => t.CPCE_CODIGO == _codigocpce && t.arac_estado).OrderBy(t =>
                    t.arac_descripcion).ToList();
                }

                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = "--Seleccione Accion--",
                    Codigo = "0"
                });

                foreach (SoftCob_ARBOL_ACCION _xdat in _campos)
                {
                    _catalogo.Add(new CatalogosDTO()
                    {
                        Descripcion = _xdat.arac_descripcion,
                        Codigo = _xdat.ARAC_CODIGO.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new FuncionesDAO().FunCambiarDataSet(_catalogo);
        }

        public DataSet FunGetArbolNewEfecto(int _codigoarac)
        {
            List<SoftCob_ARBOL_EFECTO> _campos = new List<SoftCob_ARBOL_EFECTO>();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _campos = _db.SoftCob_ARBOL_EFECTO.Where(t => t.ARAC_CODIGO == _codigoarac && 
                    t.aref_estado).OrderBy(t => t.aref_descripcion).ToList();
                }

                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = "--Seleccione Efecto--",
                    Codigo = "0"
                });

                foreach (SoftCob_ARBOL_EFECTO _xdat in _campos)
                {
                    _catalogo.Add(new CatalogosDTO()
                    {
                        Descripcion = _xdat.aref_descripcion,
                        Codigo = _xdat.AREF_CODIGO.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new FuncionesDAO().FunCambiarDataSet(_catalogo);
        }

        public DataSet FunGetArbolNewRespuesta(int _codigoaref)
        {
            List<SoftCob_ARBOL_RESPUESTA> _campos = new List<SoftCob_ARBOL_RESPUESTA>();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _campos = _db.SoftCob_ARBOL_RESPUESTA.Where(t => t.AREF_CODIGO == _codigoaref && 
                    t.arre_estado).OrderBy(t => t.arre_descripcion).ToList();
                }

                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = "--Seleccione Respuesta--",
                    Codigo = "0"
                });

                foreach (SoftCob_ARBOL_RESPUESTA _xdat in _campos)
                {
                    _catalogo.Add(new CatalogosDTO()
                    {
                        Descripcion = _xdat.arre_descripcion,
                        Codigo = _xdat.ARRE_CODIGO.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new FuncionesDAO().FunCambiarDataSet(_catalogo);
        }

        public DataSet FunGetArbolNewContacto(int _codigoarre)
        {
            List<SoftCob_ARBOL_CONTACTO> _campos = new List<SoftCob_ARBOL_CONTACTO>();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _campos = _db.SoftCob_ARBOL_CONTACTO.Where(t => t.ARRE_CODIGO == _codigoarre && 
                    t.arco_estado).OrderBy(t => t.arco_descripcion).ToList();
                }

                _catalogo.Add(new CatalogosDTO()
                {
                    Descripcion = "--Seleccione Contacto--",
                    Codigo = "0"
                });

                foreach (SoftCob_ARBOL_CONTACTO _xdat in _campos)
                {
                    _catalogo.Add(new CatalogosDTO()
                    {
                        Descripcion = _xdat.arco_descripcion,
                        Codigo = _xdat.ARCO_CODIGO.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new FuncionesDAO().FunCambiarDataSet(_catalogo);
        }
        public string FunCrearSpeech(int codigospeech, int cedecodigo, int cpcecodigo, string speechbv, bool estado, string auxv1, string auxv2, int auxi1, int auxi2, int usucodigo, string terminal, DataTable dtbSpeech, string sp, string conexion)
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
                        comm.Parameters.AddWithValue("@in_codigospeech", codigospeech);
                        comm.Parameters.AddWithValue("@in_cedecodigo", cedecodigo);
                        comm.Parameters.AddWithValue("@in_cpcecodigo", cpcecodigo);
                        comm.Parameters.AddWithValue("@in_speechbv", speechbv);
                        comm.Parameters.AddWithValue("@in_estado", estado);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_usucodigo", usucodigo);
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
                        comm.Parameters.AddWithValue("@TablaSpeech", dtbSpeech);
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

        public string FunDelSpeech(int codigospde)
        {
            using (SoftCobEntities _db = new SoftCobEntities())
            {
                var _speech = _db.SoftCob_SPEECH_DETALLE.Where(s => s.SPDE_CODIGO == codigospde).FirstOrDefault();

                if (_speech != null)
                {
                    _db.SoftCob_SPEECH_DETALLE.Attach(_speech);
                    _db.SoftCob_SPEECH_DETALLE.Remove(_speech);
                    _db.SaveChanges();
                }

                return "Borrado";
            }
        }
        public DataSet FunGetSpeechDetaArbol(int codigocede, int codigocpce, int codigoarac, int codigoaref, int codigoarre,
            int codigoarco)
        {
            try
            {
                var query = from SPD in _dtb.SoftCob_SPEECH_DETALLE
                            join SPC in _dtb.SoftCob_SPEECH_CABECERA on SPD.SPCA_CODIGO equals SPC.SPCA_CODIGO
                            where SPC.spca_cedecodigo == codigocede && SPC.spca_cpcecodigo == codigocpce &&
                            SPD.spde_araccodigo == codigoarac && SPD.spde_arefcodigo == codigoaref && SPD.spde_arrecodigo == codigoarre &&
                            SPD.spde_arcocodigo == codigoarco
                            select new SpeechCabeceraDTO
                            {
                                CodigoSpeech = SPD.SPDE_CODIGO,
                                Speechbv = SPD.spde_speechad,
                                Estado = SPD.spde_estado == true ? "Activo" : "Inactivo"
                            };

                return new FuncionesDAO().FunCambiarDataSet(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunGetSpeechCabecera(int codigocede, int codigocpce)
        {
            try
            {
                var query = from Speech in _dtb.SoftCob_SPEECH_CABECERA
                            where Speech.spca_cedecodigo == codigocede && Speech.spca_cpcecodigo == codigocpce
                            select new SpeechCabeceraDTO
                            {
                                CodigoSpeech = Speech.SPCA_CODIGO,
                                Speechbv = Speech.spca_speechbv,
                                Estado = Speech.spca_estado == true ? "Activo" : "Inactivo"
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
