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
        SqlDataAdapter _dap = new SqlDataAdapter();
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

        public List<SoftCob_ACCION> FunGetArbolAccion(int _codigocpce)
        {
            List<SoftCob_ACCION> _campos = new List<SoftCob_ACCION>();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _campos = _db.SoftCob_ACCION.Where(t => t.CPCE_CODIGO == _codigocpce && t.arac_estado).OrderBy(t => t.arac_descripcion).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _campos;
        }

        public List<SoftCob_EFECTO> FunGetArbolEfecto(int _codigoarac)
        {
            List<SoftCob_EFECTO> _campos = new List<SoftCob_EFECTO>();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _campos = _db.SoftCob_EFECTO.Where(t => t.ARAC_CODIGO == _codigoarac && t.aref_estado).OrderBy(t => t.aref_descripcion).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _campos;
        }

        public List<SoftCob_RESPUESTA> FunGetArbolRespuesta(int _codigoaref)
        {
            List<SoftCob_RESPUESTA> _campos = new List<SoftCob_RESPUESTA>();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _campos = _db.SoftCob_RESPUESTA.Where(t => t.AREF_CODIGO == _codigoaref && t.arre_estado).OrderBy(t => t.arre_descripcion).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _campos;
        }

        public List<SoftCob_CONTACTO> FunGetArbolContacto(int _codigoarre)
        {
            List<SoftCob_CONTACTO> _campos = new List<SoftCob_CONTACTO>();
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _campos = _db.SoftCob_CONTACTO.Where(t => t.ARRE_CODIGO == _codigoarre && t.arco_estado).OrderBy(t => t.arco_descripcion).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _campos;
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
        #endregion
    }
}
