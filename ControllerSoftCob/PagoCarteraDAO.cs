namespace ControllerSoftCob
{
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    public class PagoCarteraDAO
    {
        #region Varaibles
        DataSet _dts = new DataSet();
        SqlDataAdapter _dap = new SqlDataAdapter();
        string _mensaje = "";
        int _codigo = 0;
        #endregion

        #region Procedimientos y Funciones
        public DataSet FunGetPagoCartera(int tipo, int cedecodigo, int cpcecodigo, string identificacion, string operacion,
            string documento, string fechapago, string valorpago, string tipopago, string auxv1, string auxv2, string auxv3,
            int auxi1, int auxi2, int auxi3, int usuCodigo, string terminal, string conexion)
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
                        comm.CommandText = "sp_RegistroPagosCartera";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_cedecodigo", cedecodigo);
                        comm.Parameters.AddWithValue("@in_cpcecodigo", cpcecodigo);
                        comm.Parameters.AddWithValue("@in_numerodocumento", identificacion);
                        comm.Parameters.AddWithValue("@in_operacion", operacion);
                        comm.Parameters.AddWithValue("@in_documento", documento);
                        comm.Parameters.AddWithValue("@in_fechaPago", fechapago);
                        comm.Parameters.AddWithValue("@in_valorpago", valorpago);
                        comm.Parameters.AddWithValue("@in_tipopago", tipopago);
                        comm.Parameters.AddWithValue("@in_auxv1", auxv1);
                        comm.Parameters.AddWithValue("@in_auxv2", auxv2);
                        comm.Parameters.AddWithValue("@in_auxv3", auxv3);
                        comm.Parameters.AddWithValue("@in_auxi1", auxi1);
                        comm.Parameters.AddWithValue("@in_auxi2", auxi2);
                        comm.Parameters.AddWithValue("@in_auxi3", auxi3);
                        comm.Parameters.AddWithValue("@in_usucodigo", usuCodigo);
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

        public string FunInsertarPagoAbono(SoftCob_PAGOSCARTERA _pagocartera)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_PAGOSCARTERA.Add(_pagocartera);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _mensaje;
        }

        public string FunInsertarPagoAbonoCab(SoftCob_PAGOSCARTERA _pagocartera, SoftCob_PAGOSCABECERA _pagocabecera)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _db.SoftCob_PAGOSCARTERA.Add(_pagocartera);
                    _db.SaveChanges();
                    _pagocabecera.PACP_CODIGO = _pagocartera.PACP_CODIGO;
                    _db.SoftCob_PAGOSCABECERA.Add(_pagocabecera);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.ToString();
            }
            return _mensaje;
        }


        public int FunGetIdParametroDetalle(string _paracabecera, string _paradetalle)
        {
            try
            {
                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    _codigo = _db.SoftCob_PARAMETRO_DETALLE.Include("GSBPO_PARAMETRO_CABECERA").
                        Where(pd => pd.pade_nombre == _paradetalle &&
                        pd.PARA_CODIGO == pd.SoftCob_PARAMETRO_CABECERA.PARA_CODIGO &&
                        pd.SoftCob_PARAMETRO_CABECERA.para_nombre == _paracabecera).FirstOrDefault().pade_valorI;
                }
            }
            catch (Exception)
            {
                _codigo = 0;
            }
            return _codigo;
        }

        public string FunGetPagado(string numdocumento, string fechapago)
        {
            DateTime _fechapago = DateTime.ParseExact(fechapago, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            using (SoftCobEntities _db = new SoftCobEntities())
            {
                List<SoftCob_PAGOSCARTERA> _pagos = _db.SoftCob_PAGOSCARTERA.Where(p => p.pacp_numerodocumento == numdocumento &&
                     p.pacp_fechapago == _fechapago).ToList();

                if (_pagos.Count == 0) _mensaje = "";
                else _mensaje = "PAGADO";

                return _mensaje;
            }
        }

        public SoftCob_PAGOSCARTERA FunGetPagados(string numdocumento, string fechapago, string documento)
        {
            DateTime _fechapago = DateTime.ParseExact(fechapago, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            using (SoftCobEntities _db = new SoftCobEntities())
            {
                SoftCob_PAGOSCARTERA _pagos = _db.SoftCob_PAGOSCARTERA.Where(p => p.pacp_numerodocumento == numdocumento &&
                     p.pacp_fechapago == _fechapago && p.pacp_documento == documento).FirstOrDefault();

                return _pagos;
            }
        }
        #endregion
    }
}
