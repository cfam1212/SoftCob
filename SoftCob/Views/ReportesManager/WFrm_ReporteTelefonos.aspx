<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_ReporteTelefonos.aspx.cs" Inherits="SoftCob.Views.ReportesManager.WFrm_ReporteTelefonos" %>

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
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>
    <script src="../../JS/alertify.min.js"></script>
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
                <asp:UpdatePanel ID="updCabecera" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 25%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 25%"></td>
                                <td style="width: 10%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Tipo Acción:</h5>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="DdlTipoAccion" runat="server" CssClass="form-control" Width="100%" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="DdlTipoAccion_SelectedIndexChanged">
                                        <asp:ListItem Value="0">--Seleccione Acción--</asp:ListItem>
                                        <asp:ListItem Value="1">Eliminados</asp:ListItem>
                                        <asp:ListItem Value="2">Modificados</asp:ListItem>
                                        <asp:ListItem Value="3">No Cargados</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>Gestor:</h5>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="DdlGestor" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="2" Width="100%" OnSelectedIndexChanged="DdlGestor_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Buscar Por:</h5>
                                </td>
                                <td colspan="2">

                                    <asp:DropDownList ID="DdlBuscar" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlBuscar_SelectedIndexChanged" TabIndex="3" Width="100%">
                                        <asp:ListItem Value="T">Todo</asp:ListItem>
                                        <asp:ListItem Value="I">Identificacion</asp:ListItem>
                                        <asp:ListItem Value="F">Teléfono</asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:TextBox ID="TxtCriterio" runat="server" CssClass="form-control" Enabled="False" TabIndex="4" Width="100%"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>

                                    <asp:CheckBox ID="ChkPorFecha" runat="server" Font-Size="8pt" Text="Por Fecha" TabIndex="5" AutoPostBack="True" OnCheckedChanged="ChkPorFecha_CheckedChanged" />

                                </td>
                                <td>
                                    <h5>Desde:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtFechaIni" runat="server" CssClass="form-control" TabIndex="6" Width="100%"></asp:TextBox>
                                </td>
                                <td>
                                    <h5>Hasta:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtFechaFin" runat="server" CssClass="form-control" Width="100%" TabIndex="7"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                        <div class="panel panel-default">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 45%">
                                        <asp:Button ID="BtnProcesar" runat="server" Text="Procesar" Width="120px" CssClass="button" OnClick="BtnProcesar_Click" TabIndex="8" />
                                    </td>
                                    <td style="width: 10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="9" OnClick="BtnSalir_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="panel panel-default">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left">
                                        <asp:ImageButton ID="ImgExportar" runat="server" ImageUrl="~/Botones/excelbg.png" Width="40px" Height="30px" Visible="false" TabIndex="10" OnClick="ImgExportar_Click" />
                                        <asp:Label ID="LblExportar" runat="server" Text="Exportar" Visible="False"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                        <div class="panel-info">
                            <asp:Panel ID="PnlTelefonoEli" runat="server" Width="100%" Height="280px" ScrollBars="Auto" Visible="false">
                                <asp:GridView ID="GrdvTelefonoE" runat="server" Width="100%"
                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                    ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" AllowPaging="True" TabIndex="11" AutoGenerateColumns="False" OnPageIndexChanging="GrdvTelefonoE_PageIndexChanging">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="FechaAccion" HeaderText="Fecha_Acción" />
                                        <asp:BoundField DataField="Identificacion" HeaderText="Identificación" />
                                        <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                                        <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
                                        <asp:BoundField DataField="Motivo" HeaderText="Motivo" />
                                        <asp:BoundField DataField="Gestor" HeaderText="Gestor" />
                                    </Columns>
                                    <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                    <RowStyle Font-Size="X-Small" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                            </asp:Panel>
                            <asp:Panel ID="PnlTelefonoMod" runat="server" Width="100%" Height="250px" ScrollBars="Auto" Visible="false">
                                <asp:GridView ID="GrdvTelefonoM" runat="server" Width="100%"
                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                    ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" AllowPaging="True" TabIndex="12" AutoGenerateColumns="False" OnPageIndexChanging="GrdvTelefonoM_PageIndexChanging">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="FechaAccion" HeaderText="Fecha_Accion" />
                                        <asp:BoundField DataField="Identificacion" HeaderText="Indetifiación" />
                                        <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                                        <asp:BoundField DataField="FonoAnterior" HeaderText="Fono_Anterior" />
                                        <asp:BoundField DataField="FonoActual" HeaderText="Fono_Actual" />
                                        <asp:BoundField DataField="Motivo" HeaderText="Motivo" />
                                        <asp:BoundField DataField="Gestor" HeaderText="Gestor" />
                                    </Columns>
                                    <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                    <RowStyle Font-Size="X-Small" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                            </asp:Panel>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
