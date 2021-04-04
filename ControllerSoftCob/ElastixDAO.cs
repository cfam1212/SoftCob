namespace ControllerSoftCob
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    public class ElastixDAO
    {
        #region Variables
        string _respuesta = "";
        #endregion

        #region Procedimientos y Funciones ELASTIX
        public string ElastixDial(string IP, int PORT, string phone)
        {
            TcpClient socketForServer = new TcpClient();
            try
            {
                if (!socketForServer.Connected)
                    socketForServer.Connect(IP, PORT);

                socketForServer.NoDelay = true;
                using (NetworkStream networkStream = socketForServer.GetStream())
                using (StreamWriter strWriter = new StreamWriter(networkStream))
                using (StreamReader strReader = new StreamReader(networkStream))
                {
                    _respuesta = strReader.ReadLine();
                    strWriter.WriteLine(String.Format("DIAL {0}", phone));
                    strWriter.Flush();
                    _respuesta = strReader.ReadLine();
                    strWriter.WriteLine("RESTART");
                    strWriter.Flush();
                    strReader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _respuesta;
        }

        public string ElastixHangUp(string IP, int PORT)
        {
            TcpClient socketForServer = new TcpClient();
            try
            {
                if (!socketForServer.Connected)
                    socketForServer.Connect(IP, PORT);

                socketForServer.NoDelay = true;
                using (NetworkStream networkStream = socketForServer.GetStream())
                using (StreamWriter strWriter = new StreamWriter(networkStream))
                using (StreamReader strReader = new StreamReader(networkStream))
                {
                    _respuesta = strReader.ReadLine();
                    strWriter.WriteLine(String.Format("HUNGUP"));
                    strWriter.Flush();
                    _respuesta = strReader.ReadLine();
                    strWriter.WriteLine("RESTART");
                    strWriter.Flush();
                    strReader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _respuesta;
        }

        public string ElastixStatus(string IP, int PORT)
        {
            TcpClient socketForServer = new TcpClient();
            try
            {
                if (!socketForServer.Connected)
                    socketForServer.Connect(IP, PORT);

                socketForServer.NoDelay = true;
                using (NetworkStream networkStream = socketForServer.GetStream())
                using (StreamWriter strWriter = new StreamWriter(networkStream))
                using (StreamReader strReader = new StreamReader(networkStream))
                {
                    _respuesta = strReader.ReadLine();
                    strWriter.WriteLine(String.Format("STATUS"));
                    strWriter.Flush();
                    _respuesta = strReader.ReadLine();
                    strWriter.WriteLine("RESTART");
                    strWriter.Flush();
                    strReader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _respuesta;
        }

        public string ElastixForce(string IP, int PORT, string Option) //READY, NOT_READY, PRESENCE
        {
            TcpClient socketForServer = new TcpClient();
            try
            {
                if (!socketForServer.Connected)
                    socketForServer.Connect(IP, PORT);

                socketForServer.NoDelay = true;
                using (NetworkStream networkStream = socketForServer.GetStream())
                using (StreamWriter strWriter = new StreamWriter(networkStream))
                using (StreamReader strReader = new StreamReader(networkStream))
                {
                    _respuesta = strReader.ReadLine();
                    strWriter.WriteLine(String.Format(Option));
                    strWriter.Flush();
                    _respuesta = strReader.ReadLine();
                    strWriter.WriteLine("RESTART");
                    strWriter.Flush();
                    strReader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _respuesta;
        }
        #endregion
    }
}
