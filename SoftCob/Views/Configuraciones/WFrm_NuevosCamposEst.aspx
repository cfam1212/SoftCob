<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_NuevosCamposEst.aspx.cs" Inherits="SoftCob.Views.Configuraciones.WFrm_NuevosCamposEst" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nuevos Campos Estrategia</title>
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <style type="text/css">
        legend {
            color: darkblue;
            font-size: 12px;
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
                            <td style="width: 10%">
                                <h5>Tablas:</h5>
                            </td>
                            <td style="width: 40%">
                                <asp:DropDownList ID="DdlTablas" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlTablas_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 10%"></td>
                            <td style="width: 5%"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <h5>Campos:</h5>
                            </td>
                            <td>
                                <asp:ListBox ID="LstOrigen" runat="server" CssClass="form-control" Height="180px" SelectionMode="Multiple" Width="100%"></asp:ListBox>
                            </td>
                            <td style="text-align: center">
                                <asp:ImageButton ID="ImgAddCampo" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAddCampo_Click" TabIndex="29" />
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Panel ID="pnlDivision" runat="server" Height="30px"></asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="3">
                                <asp:GridView ID="GrdvCampos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,CodigoTabla" ForeColor="#333333" PageSize="5" TabIndex="31" Width="100%" OnRowDataBound="GrdvCampos_RowDataBound">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="Campo" HeaderText="Campo" />
                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                        <asp:TemplateField HeaderText="Activo">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkEstado" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEstado_CheckedChanged" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Eliminar">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgDelCampo" runat="server" Height="20px" ImageUrl="~/Botones/eliminargris.png" OnClick="ImgDelCampo_Click" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                    <RowStyle Font-Size="X-Small" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
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
                                <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="BtnGrabar_Click" />
                            </td>
                            <td style="width: 5%"></td>
                            <td style="text-align: left; width: 45%">
                                <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" />
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
