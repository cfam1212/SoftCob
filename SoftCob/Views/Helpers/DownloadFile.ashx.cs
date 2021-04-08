namespace SoftCob.Views.Helpers
{
    using System;
    using System.IO;
    using System.Web;
    /// <summary>
    /// Descripción breve de DownloadFile
    /// </summary>
    public class DownloadFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Stream stream = null;
            try
            {
                string FileLocation = HttpContext.Current.Request.QueryString["file"];
                stream = new FileStream(FileLocation, FileMode.Open, FileAccess.Read, FileShare.Read);
                long bytesToRead = stream.Length;
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(FileLocation));
                while (bytesToRead > 0)
                {
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        byte[] buffer = new Byte[10000];
                        int length = stream.Read(buffer, 0, 10000);
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                        HttpContext.Current.Response.Flush();
                        bytesToRead = bytesToRead - length;
                    }
                    else
                    {
                        bytesToRead = -1;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}