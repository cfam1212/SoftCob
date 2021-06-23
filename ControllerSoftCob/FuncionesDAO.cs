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
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public class FuncionesDAO
    {
        #region Variables
        DataSet dts = new DataSet();
        DataTable dtb = new DataTable();
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

        public bool IsDate(string strFecha)
        {
            try
            {
                DateTime myDT = DateTime.ParseExact(strFecha, "MM/dd/yyyy", CultureInfo.InvariantCulture);
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
        #endregion

    }
}
