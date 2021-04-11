<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_AccesoArbolGAdmin.aspx.cs" Inherits="SoftCob.Views.Configuraciones.WFrm_AccesoArbolGAdmin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nueva Estrategia</title>
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
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
                        numberOfMonths: 1,
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
    <script>
        function asegurar() {
            rc = confirm("¿Seguro que desea Eliminar?");
            return rc;
        }
    </script>
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
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updBotones">
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
                            <td></td>
                            <td>
                                <h5>Cedente:</h5>
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlCedente" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlCedente_SelectedIndexChanged" TabIndex="1" Width="100%">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <h5>Todos:</h5>
                            </td>
                            <td>
                                <asp:CheckBox ID="ChkTodos" runat="server" CssClass="form-control" AutoPostBack="True" Checked="True" OnCheckedChanged="ChkTodos_CheckedChanged" TabIndex="2" />
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Panel ID="pnlDivision" runat="server" Height="20px"></asp:Panel>
                            </td>
                        </tr>
                    </table>
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
                            <td></td>
                            <td colspan="4">
                                <asp:Panel ID="PnlListaGestores" runat="server" Height="380px" ScrollBars="Vertical" GroupingText="Lista de Gestores" TabIndex="3" Visible="False">
                                    <asp:GridView ID="GrdvGestores" runat="server" AutoGenerateColumns="False"
                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                        DataKeyNames="Codigo,VerLista" ForeColor="#333333"
                                        OnRowDataBound="GrdvGestores_RowDataBound" PageSize="50" TabIndex="11" Width="100%">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="Gestor" HeaderText="Gestor" />
                                            <asp:BoundField DataField="UsuarioCambio" HeaderText="Usuario_Registro" />
                                            <asp:TemplateField HeaderText="Ver_Arbol">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkLista" runat="server" OnCheckedChanged="ChkLista_CheckedChanged" AutoPostBack="True" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
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
            <asp:UpdatePanel ID="updBotones" runat="server">
                <ContentTemplate>
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: right; width: 45%">
                                <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="BtnGrabar_Click" TabIndex="4" />
                            </td>
                            <td style="width: 10%"></td>
                            <td style="text-align: left; width: 45%">
                                <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="5" />
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
