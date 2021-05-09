<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_ProyeccionAdmin.aspx.cs" Inherits="SoftCob.Views.Breanch.WFrm_ProyeccionAdmin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../css/Estilos.css" rel="stylesheet" />

    <link href="../../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../../Scripts/external/jquery/jquery.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>

    <script type="text/javascript">
        function pageLoad(sender, arg) {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#TxtFechaIni').datepicker(
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
            <div class="panel-heading">
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
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updCabecera">
                    <ProgressTemplate>
                        <div class="overlay" />
                        <div class="overlayContent">
                            <h2>Procesando..</h2>
                            <img src="../../Images/load.gif" alt="Loading" border="1" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel ID="updCabecera" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 10%"></td>
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
                                <td colspan="2">
                                    <asp:DropDownList ID="DdlCatalogo" runat="server" CssClass="form-control" TabIndex="2" Width="100%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Gestor:</h5>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="DdlGestor" runat="server" CssClass="form-control" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>Año:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlYear" runat="server" CssClass="form-control" OnSelectedIndexChanged="DdlBuscar_SelectedIndexChanged" TabIndex="5" Width="100%">
                                        <asp:ListItem Value="0">Todo</asp:ListItem>
                                        <asp:ListItem Value="I">Identificacion</asp:ListItem>
                                        <asp:ListItem Value="O">Operacion</asp:ListItem>
                                        <asp:ListItem Value="C">Cliente</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Tipo Pago:</h5>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="DdlTipoPago" runat="server" CssClass="form-control" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>Reporte:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlReporte" runat="server" CssClass="form-control" OnSelectedIndexChanged="DdlBuscar_SelectedIndexChanged" TabIndex="5" Width="100%">
                                        <asp:ListItem Value="0">Todo</asp:ListItem>
                                        <asp:ListItem Value="I">Identificacion</asp:ListItem>
                                        <asp:ListItem Value="O">Operacion</asp:ListItem>
                                        <asp:ListItem Value="C">Cliente</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td colspan="2"></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:Button ID="BtnProcesar" runat="server" CssClass="button" OnClick="BtnProcesar_Click" TabIndex="7" Text="Procesar" Width="120px" />
                                </td>
                                <td></td>
                            </tr>
                        </table>
                        <div class="panel panel-default">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left">
                                        <asp:ImageButton ID="ImgExportar" runat="server" ImageUrl="~/Botones/excel.png" Width="40px" Height="30px" OnClick="ImgExportar_Click" Visible="false" TabIndex="8" />
                                        <asp:Label ID="lblExportar" runat="server" Text="Exportar" Visible="false"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                        <div class="panel-info" runat="server" id="DivProyecc" visible="false">
                            <asp:GridView ID="GrdvDatos" runat="server" Width="100%"
                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                EmptyDataText="No Existen Registros" TabIndex="9" AutoGenerateColumns="False"
                                ForeColor="#333333" OnRowDataBound="GrdvDatos_RowDataBound" PageSize="5" DataKeyNames="CodigoRESP,EstadoPago,CodigoPRCB,Respuesta,Cedula">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="EstadoPago" HeaderText="Gestor"></asp:BoundField>
                                    <asp:BoundField DataField="Cedula" HeaderText="Prom.Pres">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Enero" HeaderText="Ene">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Febrero" HeaderText="Feb">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Marzo" HeaderText="Mar">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Abr" DataField="Abril">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="May" DataField="Mayo">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Jun" DataField="Junio">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Jul" DataField="Julio">
                                        <ItemStyle HorizontalAlign="Justify" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Ago" DataField="Agosto">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Sep" DataField="Septiembre">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Oct" DataField="Octubre">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Nov" DataField="Noviembre">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Dic" DataField="Diciembre">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                <RowStyle Font-Size="X-Small" />
                                <EditRowStyle BackColor="#2461BF" />
                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                            </asp:GridView>
                            <script>
                                var prm = Sys.WebForms.PageRequestManager.getInstance();

                                prm.add_endRequest(function () {
                                    createDataTable();
                                });

                                createDataTable();

                                function createDataTable() {
                                    $('#<%= GrdvDatos.ClientID %>').DataTable();
                                }
                            </script>
                        </div>

                        <div class="panel panel-default">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 45%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="8" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>

