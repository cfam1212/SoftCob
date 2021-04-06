namespace ControllerSoftCob
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    public class BitacoraDAO
    {
        #region Variables
        DataSet _dts = new DataSet();
        SqlDataAdapter _da = new SqlDataAdapter();
        #endregion

        #region Procedimientos y Funciones
        public DataSet FunNewBitacora(int tipo, string nombrebt, string tipobt, int codigobt, string observaciongen,
            int gestor, string observacionbt, string fechabt, string horabt, string turnoact, string turnonue,
            int firma, string auxv1, string auxv2, string auxv3, string auxv4, string auxv5, int auxi1, int auxi2,
            int auxi3, int auxi4, int auxi5, string terminal, string conexion)
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
                        comm.CommandText = "sp_NewBitacora";
                        comm.Parameters.AddWithValue("@in_tipo", tipo);
                        comm.Parameters.AddWithValue("@in_nombrebt", nombrebt);
                        comm.Parameters.AddWithValue("@in_tipobitacora", tipobt);
                        comm.Parameters.AddWithValue("@in_codigobitacora", codigobt);
                        comm.Parameters.AddWithValue("@in_observaciongen", observaciongen);
                        comm.Parameters.AddWithValue("@in_gestor", gestor);
                        comm.Parameters.AddWithValue("@in_observacionbt", observacionbt);
                        comm.Parameters.AddWithValue("@in_fechabitacora", fechabt);
                        comm.Parameters.AddWithValue("@in_horabitacora", horabt);
                        comm.Parameters.AddWithValue("@in_turnoactual", turnoact);
                        comm.Parameters.AddWithValue("@in_turnonuevo", turnonue);
                        comm.Parameters.AddWithValue("@in_firma", firma);
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
                        comm.Parameters.AddWithValue("@in_terminal", terminal);
                        _da.SelectCommand = comm;
                        _da.Fill(_dts);
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
