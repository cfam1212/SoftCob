<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_ConsultasAdmin.aspx.cs" Inherits="SoftCob.Views.ConsultasManager.WFrm_ConsultasAdmin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Consulta Admin</title>
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../../Scripts/external/jquery/jquery.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>

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
                <asp:UpdatePanel ID="updCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 35%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Buscar Por:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlBuscarPor" runat="server" CssClass="form-control" Width="100%">
                                        <asp:ListItem Value="I">Identificación</asp:ListItem>
                                        <asp:ListItem Value="C">Cliente</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: center">
                                    <h5>Criterio:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtCriterio" runat="server" CssClass="form-control" MaxLength="500" Width="100%"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="BtnBuscar" runat="server" CausesValidation="False" CssClass="button" OnClick="BtnBuscar_Click" Text="Buscar" Width="120px" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkArbol" runat="server" Text="En el Árbol" />
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                        <div runat="server" id="DivDatos" class="panel-body" visible="false">
                            <table class="table table-bordered table-condensed table-hover table-responsive">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlDatos" runat="server" Height="350px" ScrollBars="Vertical">
                                            <asp:GridView ID="GrdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                                CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="CodigoCEDE,CodigoCPCE,CodigoCLDE,CodigoPERS"
                                                ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" AllowPaging="True" OnPageIndexChanging="GrdvDatos_PageIndexChanging">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="Catalogo" HeaderText="Catalogo/Producto" />
                                                    <asp:BoundField HeaderText="Identificación" DataField="Identificacion" />
                                                    <asp:BoundField HeaderText="Cliente" DataField="Cliente" />
                                                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                    <asp:TemplateField HeaderText="Selecc">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgSelecc" runat="server" Height="20px" ImageUrl="~/Botones/Buscar.png" OnClick="ImgSelecc_Click" />
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
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" Visible="False" />
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
