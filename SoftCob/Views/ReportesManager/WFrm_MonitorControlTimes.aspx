<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_MonitorControlTimes.aspx.cs" Inherits="SoftCob.Views.ReportesManager.WFrm_MonitorControlTimes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../JS/css/alertify.min.css" rel="stylesheet" />

    <script src="../../Scripts/external/jquery/jquery.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../JS/alertify.min.js"></script>
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>

    <script type="text/javascript">
        function pageLoad(sender, arg) {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#TxtFechaInicio').datepicker(
                    {
                        inline: true,
                        dateFormat: "mm/dd/yy",
                        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                        numberOfMonths: 2,
                        showButtonPanel: true,
                        changeMonth: true,
                        changeYear: true,
                        yearRange: "-100:+5"
                    });
            });

            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#TxtFechaFin').datepicker(
                    {
                        inline: true,
                        dateFormat: "mm/dd/yy",
                        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                        numberOfMonths: 2,
                        showButtonPanel: true,
                        changeMonth: true,
                        changeYear: true,
                        yearRange: "-100:+5"
                    });
            });
        }

    </script>
    <style type="text/css">
        legend {
            color: darkblue;
            font-size: 14px;
            font-weight: bold;
        }

        .overlay {
            position: fixed;
            z-index: 98;
            top: 0px;
            left: 0px;
            right: 0px;
            bottom: 0px;
            background-color: #aaa;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }

        .overlayContent {
            z-index: 99;
            margin: 250px auto;
            width: 80px;
            height: 80px;
        }

            .overlayContent h2 {
                font-size: 18px;
                font-weight: bold;
                color: #000;
            }

            .overlayContent img {
                width: 80px;
                height: 80px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div class="panel panel-primary">
            <div class="panel-heading" style="background-color: #79BBB8;">
                <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
            </div>
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdOpciones">
                    <ProgressTemplate>
                        <div class="overlay" />
                        <div class="overlayContent">
                            <h2>Procesando..</h2>
                            <img src="../../Images/load.gif" alt="Loading" border="1" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>

            <div class="panel-default">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 5%"></td>
                        <td style="width: 15%"></td>
                        <td style="width: 15%"></td>
                        <td style="width: 20%"></td>
                        <td style="width: 15%"></td>
                        <td style="width: 25%"></td>
                        <td style="width: 5%"></td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:Panel runat="server" Height="20px"></asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <h5>Cedente:</h5>
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="DdlCedente" runat="server" CssClass="form-control" Width="100%" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="DdlCedente_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <h5>Catálogo:</h5>
                        </td>
                        <td>
                            <asp:DropDownList ID="DdlCatalogo" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="2" Width="100%" OnSelectedIndexChanged="DdlCatalogo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <h5>Fecha Proceso:</h5>
                        </td>
                        <td>
                            <h5>Desde:</h5>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtFechaInicio" runat="server" CssClass="form-control" Width="100%" TabIndex="3"></asp:TextBox>
                        </td>
                        <td>
                            <h5>Hasta:</h5>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtFechaFin" runat="server" CssClass="form-control" Width="100%" TabIndex="4"></asp:TextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <h5>Gestor:</h5>
                        </td>
                        <td colspan="2">

                            <asp:DropDownList ID="DdlGestores" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="5" Width="100%" OnSelectedIndexChanged="DdlGestores_SelectedIndexChanged">
                            </asp:DropDownList>

                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:Panel runat="server" Height="20px"></asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>

            <div class="panel panel-default">
                <asp:UpdatePanel ID="UpdOpciones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:Button ID="BtnProcesar" runat="server" CssClass="button" OnClick="BtnProcesar_Click" TabIndex="6" Text="Procesar" Width="120px" />
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="panel panel-default">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 5%"></td>
                                <td style="width: 55%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="text-align: left">
                                    <asp:ImageButton ID="ImgExportar" runat="server" ImageUrl="~/Botones/excelbg.png" Width="40px" Height="30px" OnClick="ImgExportar_Click" Visible="false" TabIndex="8" />
                                </td>
                                <td>
                                    <h5 runat="server" id="LblExportar" visible="false">Exportar</h5>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="3">
                                    <asp:Panel ID="pnlCallAnswerd" runat="server" ScrollBars="Vertical" Height="250px" GroupingText="Llamadas Efectivas">
                                        <asp:GridView ID="GrdvEfectivas" runat="server" Width="100%"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" TabIndex="9" AutoGenerateColumns="False" PageSize="12">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="TipoRegistro" HeaderText="Registro" />
                                                <asp:BoundField DataField="Operaciones" HeaderText="Operaciones">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TotalGestion" HeaderText="T.Gestión">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TotalLlamada" HeaderText="T.Llamada">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                            </Columns>
                                            <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                            <RowStyle Font-Size="X-Small" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <div class="panel panel-default">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 5%"></td>
                                <td style="width: 55%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="text-align: left">
                                    <asp:ImageButton ID="ImgExportar1" runat="server" ImageUrl="~/Botones/excelbg.png" Width="40px" Height="30px" OnClick="ImgExportar1_Click" Visible="false" TabIndex="10" />
                                </td>
                                <td>
                                    <h5 runat="server" id="LblExportar1" visible="false">Exportar</h5>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="3">
                                    <asp:Panel ID="pnlMaxLlamada" runat="server" ScrollBars="Vertical" Height="250px" GroupingText="Máximo Llamada-Gestión">
                                        <asp:GridView ID="GrdvMaxLlamada" runat="server" Width="100%"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" TabIndex="11" AutoGenerateColumns="False" PageSize="12">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="Tipo" HeaderText="Registro" />
                                                <asp:BoundField DataField="MaximoGestion" HeaderText="T.Max.Gestión">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="MaximoLlamada" HeaderText="T.Max.LLamada">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                                                <asp:BoundField DataField="Identificacion" HeaderText="Identificación" />
                                                <asp:BoundField DataField="Operacion" HeaderText="Operación" />
                                                <asp:BoundField DataField="TelefonoGestionado" HeaderText="Telefono" />
                                            </Columns>
                                            <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                            <RowStyle Font-Size="X-Small" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <div class="panel panel-default">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 5%"></td>
                                <td style="width: 55%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="text-align: left">
                                    <asp:ImageButton ID="ImgExportar2" runat="server" ImageUrl="~/Botones/excelbg.png" Width="40px" Height="30px" OnClick="ImgExportar2_Click" Visible="false" TabIndex="12" />
                                </td>
                                <td>
                                    <h5 runat="server" id="LblExportar2" visible="false">Exportar</h5>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="3">
                                    <asp:Panel ID="pnlLogueos" runat="server" ScrollBars="Vertical" Height="250px" GroupingText="Tiempos - Logueos">
                                        <asp:GridView ID="GrdvLogueos" runat="server" Width="100%"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" TabIndex="13" AutoGenerateColumns="False" PageSize="12">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="TipoLogueo" HeaderText="Tipo Logueo" />
                                                <asp:BoundField DataField="FechaLogueo" HeaderText="Fecha Logueo" DataFormatString="{0:d}">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="HoraLogueo" HeaderText="Hora Logueo">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                            </Columns>
                                            <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                            <RowStyle Font-Size="X-Small" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <div class="panel-default">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 5%"></td>
                        <td style="width: 30%"></td>
                        <td style="width: 30%"></td>
                        <td style="width: 30%"></td>
                        <td style="width: 5%"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td>
                            <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="7" />
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>

        </div>
    </form>
</body>
</html>
