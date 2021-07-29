<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_ConfigurarAccionEfecto.aspx.cs" Inherits="SoftCob.Views.Configuraciones.WFrm_ConfigurarAccionEfecto" %>

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
            rc = confirm("¿Seguro que desea Copiar?");
            return rc;
        }
    </script>
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
            <div class="panel-body">
                <asp:UpdatePanel ID="updCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 15%">
                                    <h5>Cedente:</h5>
                                </td>
                                <td style="width: 30%">
                                    <asp:DropDownList ID="DdlCedente" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlCedente_SelectedIndexChanged" TabIndex="1" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 15%">
                                    <h5>Catálogo/Producto:</h5>
                                </td>
                                <td style="width: 30%">
                                    <asp:DropDownList ID="DdlCatalogo" runat="server" CssClass="form-control" TabIndex="2" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlCatalogo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Panel ID="pnlDivision" runat="server" Height="30px"></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="3">
                                    <asp:Panel ID="PnlEfectos" runat="server" GroupingText="Acción-Efectos" Height="250px" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvEfecto" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="Codigo,Selecc" ShowHeaderWhenEmpty="True" TabIndex="10" Width="100%"
                                            OnRowDataBound="GrdvEfecto_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Efecto" />
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkSelecc" runat="server" OnCheckedChanged="ChkSelecc_CheckedChanged" AutoPostBack="True" />
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
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Panel ID="Panel1" runat="server" Height="30px"></asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel panel-default">
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: right; width: 45%"></td>
                        <td style="width: 10%"></td>
                        <td style="text-align: left; width: 45%">
                            <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="6" OnClick="BtnSalir_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
