namespace ControllerSoftCob
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;    
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    //using MimeKit;
    //using MailKit.Net.Smtp;
    //using MailKit.Security;
    //using MimeKit.Utils;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Net.Mail;

    public class FuncionesDAO
    {
        #region Variables
        DataSet dts = new DataSet();
        DataTable dtb = new DataTable();
        string _mensaje = "", _body = "";
        int valorx = 0, valory = 0;
        bool bValid;
        TripleDESCryptoServiceProvider mCSP = new TripleDESCryptoServiceProvider();
        #endregion

        #region Procedimientos y Funciones Seguridad
        public string FunDesencripta(string bynario)
        {
            mCSP.Key = new Byte[] { Convert.ToByte("71"), Convert.ToByte("24"), Convert.ToByte("103"), Convert.ToByte("58"), Convert.ToByte("162"), Convert.ToByte("235"), Convert.ToByte("211"), Convert.ToByte("130"), Convert.ToByte("134"), Convert.ToByte("212"), Convert.ToByte("56"), Convert.ToByte("119"), Convert.ToByte("70"), Convert.ToByte("108"), Convert.ToByte("91"), Convert.ToByte("113"), Convert.ToByte("189"), Convert.ToByte("247"), Convert.ToByte("9"), Convert.ToByte("17"), Convert.ToByte("157"), Convert.ToByte("9"), Convert.ToByte("65"), Convert.ToByte("35") };
            mCSP.IV = new Byte[] { Convert.ToByte("230"), Convert.ToByte("128"), Convert.ToByte("180"), Convert.ToByte("179"), Convert.ToByte("98"), Convert.ToByte("247"), Convert.ToByte("139"), Convert.ToByte("137") };
            try
            {
                ICryptoTransform ictEncriptado;
                MemoryStream mstMemoria;
                CryptoStream cytFlujo;
                byte[] bytArreglo;

                ictEncriptado = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);

                bytArreglo = Convert.FromBase64String(bynario);

                mstMemoria = new MemoryStream();
                cytFlujo = new CryptoStream(mstMemoria, ictEncriptado, CryptoStreamMode.Write);
                cytFlujo.Write(bytArreglo, 0, bytArreglo.Length);
                cytFlujo.FlushFinalBlock();

                cytFlujo.Close(); cytFlujo = null;
                ictEncriptado.Dispose(); ictEncriptado = null;

                return Encoding.UTF8.GetString(mstMemoria.ToArray());
            }
            catch (Exception ex)
            {
                return "ERROR:" + ex.Message;
            }
        }

        public string FunEncripta(string Texto)
        {
            mCSP.Key = new Byte[] { Convert.ToByte("71"), Convert.ToByte("24"), Convert.ToByte("103"), Convert.ToByte("58"), Convert.ToByte("162"), Convert.ToByte("235"), Convert.ToByte("211"), Convert.ToByte("130"), Convert.ToByte("134"), Convert.ToByte("212"), Convert.ToByte("56"), Convert.ToByte("119"), Convert.ToByte("70"), Convert.ToByte("108"), Convert.ToByte("91"), Convert.ToByte("113"), Convert.ToByte("189"), Convert.ToByte("247"), Convert.ToByte("9"), Convert.ToByte("17"), Convert.ToByte("157"), Convert.ToByte("9"), Convert.ToByte("65"), Convert.ToByte("35") };
            mCSP.IV = new Byte[] { Convert.ToByte("230"), Convert.ToByte("128"), Convert.ToByte("180"), Convert.ToByte("179"), Convert.ToByte("98"), Convert.ToByte("247"), Convert.ToByte("139"), Convert.ToByte("137") };
            try
            {
                ICryptoTransform ictEncriptado;
                MemoryStream mstMemoria;
                CryptoStream cytFlujo;
                byte[] bytArreglo;

                ictEncriptado = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);

                bytArreglo = Encoding.UTF8.GetBytes(Texto);

                mstMemoria = new MemoryStream();
                cytFlujo = new CryptoStream(mstMemoria, ictEncriptado, CryptoStreamMode.Write);
                cytFlujo.Write(bytArreglo, 0, bytArreglo.Length);
                cytFlujo.FlushFinalBlock();

                cytFlujo.Close(); cytFlujo = null;
                ictEncriptado.Dispose(); ictEncriptado = null;

                return Convert.ToBase64String(mstMemoria.ToArray());
            }
            catch (Exception ex)
            {
                return "ERROR:" + ex.Message;
            }
        }
        #endregion

        #region Procedimientos y Funciones FUNCIONES
        public void FunShowJSMessage(string message, Control pagina)
        {
            //ScriptManager.RegisterClientScriptBlock(pagina, pagina.GetType(), "alert", "alert('" + message + "');", true);

            ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                "'top-right'); alertify.warning('" + message + "', 5, function(){console.log('dismissed');});", true);
        }

        public void FunShowJSMessage(string message, Control pagina, string tipo, string position)
        {
            switch (tipo)
            {
                case "S":
                    if (position == "C")
                        ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                            "'top-center'); alertify.success('" + message + "', 5, function(){console.log('dismissed');});", true);

                    if (position == "R")
                        ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                            "'bottom-right'); alertify.success('" + message + "', 5, function(){console.log('dismissed');});", true);

                    if (position == "L")
                        ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                            "'top-left'); alertify.success('" + message + "', 5, function(){console.log('dismissed');});", true);

                    break;
                case "W":
                    if (position == "C")
                        ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                            "'top-center'); alertify.warning('" + message + "', 5, function(){console.log('dismissed');});", true);

                    if (position == "R")
                        ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                            "'top-right'); alertify.warning('" + message + "', 5, function(){console.log('dismissed');});", true);

                    if (position == "L")
                        ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                            "'top-left'); alertify.warning('" + message + "', 5, function(){console.log('dismissed');});", true);

                    break;
                case "E":
                    if (position == "C")
                        ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                            "'bottom-center'); alertify.error('" + message + "', 5, function(){console.log('dismissed');});", true);

                    if (position == "R")
                        ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                            "'top-right'); alertify.error('" + message + "', 5, function(){console.log('dismissed');});", true);

                    if (position == "L")
                        ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                            "'top-left'); alertify.error('" + message + "', 5, function(){console.log('dismissed');});", true);

                    break;
                case "M":
                    if (position == "C")
                        ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                            "'top-center'); alertify.message('" + message + "', 5, function(){console.log('dismissed');});", true);

                    if (position == "R")
                        ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                            "'top-right'); alertify.message('" + message + "', 5, function(){console.log('dismissed');});", true);

                    if (position == "L")
                        ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                            "'top-left'); alertify.message('" + message + "', 5, function(){console.log('dismissed');});", true);

                    break;
                case "N":
                    if (position == "C")
                        ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                            "'top-center'); alertify.notify('" + message + "', 5, function(){console.log('dismissed');});", true);

                    if (position == "R")
                        ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                            "'top-right'); alertify.notify('" + message + "', 5, function(){console.log('dismissed');});", true);

                    if (position == "L")
                        ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                            "'top-left'); alertify.notify('" + message + "', 5, function(){console.log('dismissed');});", true);

                    break;
            }
        }
        public int FunBetween(int valorI, int valorF, int rinicio, int rfinal)
        {
            if (rinicio >= valorI && rinicio <= valorF) valorx = 1;
            if (rfinal >= valorI && rfinal <= valorF) valory = 1;
            return valorx + valory;
        }

        public int FunBetweenUno(int valorI, int valorF, int rango)
        {
            if (rango >= valorI && rango <= valorF) return 1;
            else return 0;
        }

        public bool IsDate(string strFecha, string formato)
        {
            try
            {
                DateTime myDT = DateTime.ParseExact(strFecha, formato, CultureInfo.InvariantCulture);
                bValid = true;
            }
            catch
            {
                bValid = false;
            }

            return bValid;
        }

        public bool IsHour(string hora)
        {
            try
            {
                DateTime myHR = DateTime.ParseExact(hora, "HH:mm", CultureInfo.InstalledUICulture);
                bValid = true;
            }
            catch (Exception)
            {
                bValid = false;
            }
            return bValid;
        }

        public bool Ruta_bien_escrita(string rutaPagina)
        {
            if (rutaPagina.Length > 5)
                if (rutaPagina.Substring(rutaPagina.Length - 5, 5) == ".aspx")
                    return true;
                else
                    return false;
            else
                return false;
        }

        public bool Email_bien_escrito(string email)
        {
            if (email.Length > 0)
            {
                String expresion;
                expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                if (Regex.IsMatch(email, expresion))
                {
                    if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else return true;
        }

        public bool CedulaBienEscrita(string cedula)
        {
            int isNumeric;
            var total = 0;
            const int tamanoLongitudCedula = 10;
            int[] coeficientes = { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            const int numeroProvincias = 24;
            const int tercerDigito = 6;

            if (int.TryParse(cedula, out isNumeric) && cedula.Length == tamanoLongitudCedula)
            {
                var provincia = Convert.ToInt32(string.Concat(cedula[0], cedula[1], string.Empty));
                var digitoTres = Convert.ToInt32(cedula[2] + string.Empty);
                if ((provincia > 0 && provincia <= numeroProvincias) && digitoTres < tercerDigito)
                {
                    var digitoVerificadorRecibido = Convert.ToInt32(cedula[9] + string.Empty);
                    for (var f = 0; f < coeficientes.Length; f++)
                    {
                        var valor = Convert.ToInt32(coeficientes[f] + string.Empty) * Convert.ToInt32(cedula[f] + string.Empty);
                        total = valor >= 10 ? total + (valor - 9) : total + valor;
                    }
                    var digitoVerificadorObtenido = total >= 10 ? (total % 10) != 0 ? 10 - (total % 10) : (total % 10) : total;
                    return digitoVerificadorObtenido == digitoVerificadorRecibido;
                }
                return false;
            }
            return false;
        }

        public void FunOrdenar(ListBox lstbox)
        {
            SortedList sorted = new SortedList();
            foreach (ListItem litem in lstbox.Items)
            {
                sorted.Add(litem.Value, litem.Text);
            }
            lstbox.Items.Clear();
            foreach (String key in sorted.Keys)
            {
                lstbox.Items.Add(new ListItem(sorted[key].ToString(), key.ToString()));
            }
        }

        public void FunRemoverElement(ListBox lstBox, ArrayList aryElement)
        {
            foreach (ListItem item in aryElement)
            {
                lstBox.Items.Remove(item);
            }
        }

        public void SetearGrid(GridView grvGrid, ImageButton imgSubir, int cells, ImageButton imgBajar, int cellb, DataTable dtTable)
        {
            if (grvGrid.Rows.Count > 0)
            {
                imgSubir = (ImageButton)grvGrid.Rows[0].Cells[cells].FindControl("ImgSubirNivel");
                imgSubir.ImageUrl = "~/Botones/desactivada_up.png";
                imgSubir.Enabled = false;

                if (dtTable.Rows.Count == 1)
                {
                    imgBajar = (ImageButton)grvGrid.Rows[0].Cells[cellb].FindControl("ImgBajarNivel");
                    imgBajar.ImageUrl = "~/Botones/desactivada_down.png";
                    imgBajar.Enabled = false;
                }

                if (dtTable.Rows.Count > 1)
                {
                    imgBajar = (ImageButton)grvGrid.Rows[dtTable.Rows.Count - 1].Cells[cellb].FindControl("ImgBajarNivel");
                    imgBajar.ImageUrl = "~/Botones/desactivada_down.png";
                    imgBajar.Enabled = false;
                }
            }
        }
        public void SetearGrid(GridView grvGrid, ImageButton imgSubir, int cells, DataTable dtTable)
        {
            if (dtTable.Rows.Count > 0)
            {
                imgSubir = (ImageButton)grvGrid.Rows[0].Cells[cells].FindControl("imgSubir");
                imgSubir.ImageUrl = "~/Botones/desactivada_up.png";
                imgSubir.Enabled = false;
            }
        }

        public void FunLlenarCombosValues(DropDownList dropList, int valorI, int valorF)
        {
            ListItem item = new ListItem();
            {
                item.Text = "--Seleccione Valor--";
                item.Value = "-1";
            }
            dropList.Items.Add(item);
            for (int i = valorI; i <= valorF; i++)
            {
                item = new ListItem();
                {
                    item.Text = i.ToString();
                    item.Value = i.ToString();
                    dropList.Items.Add(item);
                }
            }
        }

        public DataSet FunCambiarDataSet<T>(List<T> list)
        {
            try
            {
                Type elementType = typeof(T);
                dts.Tables.Add(dtb);
                foreach (var propInfo in elementType.GetProperties())
                {
                    Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                    dtb.Columns.Add(propInfo.Name, ColType);
                }
                foreach (T item in list)
                {
                    DataRow row = dtb.NewRow();

                    foreach (var propInfo in elementType.GetProperties())
                    {
                        row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                    }
                    dtb.Rows.Add(row);
                }
                return dts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void FunCargarComboHoraMinutos(DropDownList ddlCombo, string tipo)
        {
            switch (tipo)
            {
                case "HORAS":
                    for (int x = 7; x <= 18; x++)
                    {
                        ListItem itemDatos = new ListItem();
                        itemDatos.Text = x < 10 ? "0" + x.ToString() : x.ToString();
                        itemDatos.Value = x < 10 ? "0" + x.ToString() : x.ToString();
                        ddlCombo.Items.Add(itemDatos);
                    }
                    break;
                case "MINUTOS":
                    for (int x = 0; x <= 59; x++)
                    {
                        ListItem itemDatos = new ListItem();
                        itemDatos.Text = x < 10 ? "0" + x.ToString() : x.ToString();
                        itemDatos.Value = x < 10 ? "0" + x.ToString() : x.ToString();
                        ddlCombo.Items.Add(itemDatos);
                    }
                    break;
                case "YEARS":
                    int year = DateTime.Now.Year;
                    for (int i = year - 5; i <= year + 5; i++)
                    {
                        ListItem li = new ListItem(i.ToString());
                        ddlCombo.Items.Add(li);
                    }
                    ddlCombo.Items.FindByText(year.ToString()).Selected = true;
                    break;
            }
        }
        public bool IsNumber(string number)
        {
            try
            {
                int myNB = int.Parse(number);
                bValid = true;
            }
            catch (Exception)
            {
                bValid = false;
            }
            return bValid;
        }
        public bool Between(TimeSpan hora, TimeSpan desde, TimeSpan hasta)
        {
            //if (desde > hasta) throw new ArgumentException("dtFrom may not be after dtThru", "dtFrom");
            bool isBetween = (desde >= hora && hasta >= hora);
            return isBetween;
        }
        public string FunEnviarMail(string mailsTo, string subject, object[] objBody, object[] objPie, string emailTemplate,
            string host, int port, bool enableSSl, string usuario, string password, string ePathAttach, string ePathLogo,
            string eAlterMail, string conexion)
        {
            try
            {
                _body = ReplaceBody(objBody, objPie, emailTemplate);
                _mensaje = SendHtmlEmail(mailsTo, subject, _body, host, port, enableSSl, usuario, password,
                    ePathAttach, ePathLogo, eAlterMail, conexion);
            }
            catch (Exception ex)
            {
                new ConsultaDatosDAO().FunConsultaDatos(269, 1, 0, 0, ex.ToString(), "SendMail", "", conexion);
                _mensaje = ex.Message;
            }
            return _mensaje;
        }

        private string ReplaceBody(object[] oBody, object[] oPie, string eTemplate)
        {
            using (StreamReader reader = new StreamReader(eTemplate))
            {
                _body = reader.ReadToEnd();
            }
            _body = _body.Replace("{Notificar}", oBody[0].ToString());
            _body = _body.Replace("{Leyenda}", oPie[4].ToString());
            _body = _body.Replace("{Pie1}", oPie[0].ToString());
            _body = _body.Replace("{Pie2}", oPie[1].ToString());
            _body = _body.Replace("{Pie3}", oPie[2].ToString());
            _body = _body.Replace("{Pie4}", oPie[3].ToString());
            return _body;
        }

        private string SendHtmlEmail(string mailTO, string subject, string body, string ehost, int eport, bool eEnableSSL,
            string eusername, string epassword, string pathAttach, string pathLogo, string mailAlter, string conexion)
        {

            using (MailMessage mailMessage = new MailMessage())
            {
                try
                {
                    Attachment archivo = new Attachment(pathAttach);
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                    LinkedResource theEmailImage = new LinkedResource(pathLogo);
                    {
                        theEmailImage.ContentId = "myImageID";
                    }
                    htmlView.LinkedResources.Add(theEmailImage);
                    mailMessage.AlternateViews.Add(htmlView);
                    mailMessage.From = new MailAddress(eusername);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;

                    if (!string.IsNullOrEmpty(mailTO))
                    {
                        string[] manyMails = mailTO.Split(',');
                        foreach (string toMails in manyMails)
                        {
                            mailMessage.To.Add(new MailAddress(toMails));
                        }
                    }

                    if (!string.IsNullOrEmpty(mailAlter))
                    {
                        string[] alterMails = mailAlter.Split(',');
                        foreach (string alMalis in alterMails)
                        {
                            mailMessage.CC.Add(alMalis);
                        }
                    }

                    mailMessage.Attachments.Add(archivo);

                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                    {
                        NetworkCred.UserName = eusername;
                        NetworkCred.Password = epassword;
                    }

                    SmtpClient smtp = new SmtpClient();
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = NetworkCred;
                        smtp.Host = ehost;
                        smtp.Port = eport;
                        smtp.EnableSsl = eEnableSSL;
                        smtp.Send(mailMessage);
                    }
                    _mensaje = "";
                }
                catch (Exception ex)
                {
                    new ConsultaDatosDAO().FunConsultaDatos(269, 1, 0, 0, ex.ToString(), "SendMail", "", conexion);
                    _mensaje = ex.Message;
                }
                return _mensaje;
            }
        }
        //private string SendHtmlEmail(string mailTO, string subject, string body, string ehost, int eport, bool eEnableSSL,
        //    string eusername, string epassword, string pathAttach, string pathLogo, string mailAlter,
        //    string conexion)
        //{
        //    try
        //    {
        //        _mensaje = "";
        //        var mailMessage = new MimeMessage();
        //        mailMessage.From.Add(new MailboxAddress("Departamento Jurídico", eusername));

        //        if (!string.IsNullOrEmpty(mailTO))
        //        {
        //            string[] manyMails = mailTO.Split(',');
        //            foreach (string toMails in manyMails)
        //            {
        //                mailMessage.To.Add(new MailboxAddress("", toMails.ToString().Trim()));
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(mailAlter))
        //        {
        //            string[] alterMails = mailAlter.Split(',');
        //            foreach (string alMalis in alterMails)
        //            {
        //                mailMessage.Cc.Add(new MailboxAddress("", alMalis.ToString().Trim()));
        //            }
        //        }

        //        mailMessage.Subject = subject;
        //        var builder = new BodyBuilder();

        //        var image = builder.LinkedResources.Add(pathLogo);
        //        image.ContentId = MimeUtils.GenerateMessageId();
        //        body = body.Replace("cid:myImageID", string.Format(@"'cid:{0}'", image.ContentId));
        //        //builder.HtmlBody = string.Format(@" < img src='cid:{0}'><br>{1}", image.ContentId, body);
        //        builder.HtmlBody = body;
        //        builder.Attachments.Add(pathAttach);
        //        mailMessage.Body = builder.ToMessageBody();

        //        using (var client = new SmtpClient())
        //        {
        //            //client.CheckCertificateRevocation = false;
        //            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
        //            //client.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;

        //            //client.Connect(ehost, eport, SecureSocketOptions.SslOnConnect);
        //            client.Connect(ehost, eport, false);
        //            client.Authenticate(eusername, epassword);

        //            client.Send(mailMessage);
        //            client.Disconnect(true);
        //        }

        //        _mensaje = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        new ConsultaDatosDAO().FunConsultaDatos(269, 1, 0, 0, ex.ToString(), "SendMail", "", conexion);
        //        _mensaje = ex.Message;
        //    }
        //    return _mensaje;
        //}

        #endregion

    }
}
