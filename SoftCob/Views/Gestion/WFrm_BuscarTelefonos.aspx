<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_BuscarTelefonos.aspx.cs" Inherits="SoftCob.Views.Gestion.WFrm_BuscarTelefonos" %>

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
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdCabecera">
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
                <asp:UpdatePanel ID="UpdCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 10%">
                                    <h5>Buscar Por:</h5>
                                </td>
                                <td style="width: 35%">
                                    <asp:DropDownList ID="DdlBuscarPor" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="1" Width="100%">
                                        <asp:ListItem Value="0">--Seleccione Buscar--</asp:ListItem>
                                        <asp:ListItem Value="1">Identificación</asp:ListItem>
                                        <asp:ListItem Value="2">Teléfono</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 10%">
                                    <h5>Criterio:</h5>
                                </td>
                                <td style="width: 35%">
                                    <asp:TextBox ID="TxtCriterio" runat="server" CssClass="form-control upperCase" MaxLength="150" TabIndex="2" Width="100%"></asp:TextBox>
                                </td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:Button ID="BtnBuscar" runat="server" CausesValidation="False" CssClass="btn-sm" OnClick="BtnBuscar_Click" TabIndex="3" Text="Buscar" Width="120px" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Panel ID="pnlDivision" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Teléfonos CLARO</h3>
                        <asp:Panel ID="PnlTelefonoClaro" runat="server" Height="180px" Visible="false">
                            <asp:GridView ID="GrdvCelular" runat="server" AutoGenerateColumns="False"
                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                DataKeyNames="Telefono" ForeColor="#333333" PageSize="5"
                                TabIndex="4" Width="100%">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
                                    <asp:BoundField DataField="Cedula" HeaderText="Cédula" />
                                    <asp:BoundField DataField="Nombres" HeaderText="Nombres" />
                                    <asp:BoundField DataField="TipoPlan" HeaderText="Plan" />
                                    <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                    <asp:TemplateField HeaderText="Marcar">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgCelular" runat="server" Height="15px" ImageUrl="~/Botones/call_small.png" OnClick="ImgCelular_Click" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle Font-Size="Small" />
                                <RowStyle Font-Size="X-Small" />
                            </asp:GridView>
                        </asp:Panel>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel1" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Teléfonos CNT</h3>
                        <asp:Panel ID="PnlTelefonoCNT" runat="server" Height="180px" Visible="false">
                            <asp:GridView ID="GrdvTelefonos" runat="server" AutoGenerateColumns="False"
                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                DataKeyNames="Telefono" ForeColor="#333333" PageSize="5"
                                TabIndex="5" Width="100%">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
                                    <asp:BoundField DataField="Cedula" HeaderText="Cedula" />
                                    <asp:BoundField DataField="Nombres" HeaderText="Nombres" />
                                    <asp:BoundField DataField="Direccion" HeaderText="Direccion" />
                                    <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                    <asp:BoundField DataField="Provincia" HeaderText="Provincia" />
                                    <asp:TemplateField HeaderText="Marcar">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgTelefono" runat="server" Height="15px" ImageUrl="~/Botones/call_small.png" OnClick="ImgTelefono_Click" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle Font-Size="Small" />
                                <RowStyle Font-Size="X-Small" />
                            </asp:GridView>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="UpdBotones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: center; width: 45%">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="6" />
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
