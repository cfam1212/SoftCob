<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_ConsultaAccion.aspx.cs" Inherits="SoftCob.Views.ConsultasManager.WFrm_ConsultaAccion" %>

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
                $('#txtFechaInicio').datepicker(
                    {
                        inline: true,
                        dateFormat: "mm/dd/yy",
                        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                        numberOfMonths: 1,
                        showButtonPanel: true,
                        changeMonth: true,
                        changeYear: true,
                        yearRange: "-100:+5"
                    });
            });

            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#txtFechaFin').datepicker(
                    {
                        inline: true,
                        dateFormat: "mm/dd/yy",
                        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                        numberOfMonths: 1,
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
            <div class="panel panel-primary">
                <asp:UpdatePanel ID="updCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 35%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 35%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel1" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Cedente:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlCedente" runat="server" CssClass="form-control" Width="100%" OnSelectedIndexChanged="DdlCedente_SelectedIndexChanged" TabIndex="1" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>Catálogo/Producto:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlCatalogo" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="2" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Acción:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlAccion" runat="server" CssClass="form-control" Width="100%" TabIndex="3">
                                        <asp:ListItem Value="0">Todas</asp:ListItem>
                                        <asp:ListItem Value="C">CAMBIAR OPERACION</asp:ListItem>
                                        <asp:ListItem Value="G">ELIMINAR GESTION</asp:ListItem>
                                        <asp:ListItem Value="T">ELIMINAR TELEFONO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>Buscar Por:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlBuscarPor" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlBuscarPor_SelectedIndexChanged" Width="100%" TabIndex="4">
                                        <asp:ListItem Value="0">Todo</asp:ListItem>
                                        <asp:ListItem Value="I">Identificación</asp:ListItem>
                                        <asp:ListItem Value="O">Operación</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:TextBox ID="TxtBuscarPor" CssClass="form-control" runat="server" Enabled="False" MaxLength="20" Width="100%" TabIndex="5"></asp:TextBox>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="6" style="text-align: center">
                                    <asp:Button ID="BtnBuscar" runat="server" CausesValidation="False" CssClass="button" OnClick="BtnBuscar_Click" Text="Buscar" Width="120px" TabIndex="6" />
                                </td>
                            </tr>
                        </table>
                        <div class="panel panel-default">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left">
                                        <asp:ImageButton ID="ImgExportar" runat="server" ImageUrl="~/Botones/excelbg.png" Width="40px" Height="30px" Visible="false" OnClick="ImgExportar_Click" TabIndex="7" />
                                        <asp:Label ID="LblExportar" runat="server" Text="Exportar" Visible="False"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlDatos" runat="server" Height="350px" ScrollBars="Vertical">
                                            <asp:GridView ID="GrdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar"
                                                AllowPaging="True" OnPageIndexChanging="GrdvDatos_PageIndexChanging"
                                                TabIndex="8">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField HeaderText="Catalogo" DataField="Catalogo" />
                                                    <asp:BoundField DataField="Identificacion" HeaderText="Identificación" />
                                                    <asp:BoundField HeaderText="Operación" DataField="Operacion" />
                                                    <asp:BoundField HeaderText="Acción" DataField="Accion" />
                                                    <asp:BoundField HeaderText="Motivo" DataField="Motivo" />
                                                    <asp:BoundField HeaderText="Observación" DataField="Observacion" />
                                                    <asp:BoundField HeaderText="Gestor_Anterior" DataField="GestorAnterior" />
                                                    <asp:BoundField DataField="GestorActual" HeaderText="Gestor_Actual" />
                                                    <asp:BoundField DataField="DatoAnterior" HeaderText="Dato" />
                                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                                    <asp:BoundField HeaderText="Usuario" DataField="Usuario" />
                                                </Columns>
                                                <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                                <RowStyle Font-Size="X-Small" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="updBotones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: center; width: 100%">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="9" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
