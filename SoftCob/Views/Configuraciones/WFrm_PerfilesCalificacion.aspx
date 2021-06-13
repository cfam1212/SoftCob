<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_PerfilesCalificacion.aspx.cs" Inherits="SoftCob.Views.Configuraciones.WFrm_PerfilesCalificacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nuevo Usuario</title>
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
                            <td style="width: 45%" colspan="2"></td>
                            <td style="width: 5%"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <h5>Tipo Perfil:</h5>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="DdlPerfiles" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlPerfiles_SelectedIndexChanged" TabIndex="1">
                                </asp:DropDownList>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <h5>Descripción:</h5>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="TxtDescripcion" runat="server" CssClass="form-control upperCase" MaxLength="80" TabIndex="2" Width="100%"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Panel ID="pnlDivision" runat="server" Height="25px">
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td style="text-align: center">
                                <asp:ImageButton ID="ImgAdd" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAdd_Click" TabIndex="3" />
                            </td>
                            <td>
                                <asp:ImageButton ID="ImgEdit" runat="server" Enabled="False" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgEdit_Click" TabIndex="4" />
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Panel ID="Panel1" runat="server" Height="25px">
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="3">
                                <asp:GridView ID="GrdvDatos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="CodigoPer" ForeColor="#333333" PageSize="5" TabIndex="5" Width="100%" OnRowDataBound="GrdvDatos_RowDataBound">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Activo">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkEstado" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEstado_CheckedChanged" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Selecc">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgSelecc" runat="server" Height="20px" ImageUrl="~/Botones/seleccbg.png" OnClick="ImgSelecc_Click" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Eliminar">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgDel" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" Width="20px" OnClick="ImgDel_Click" />
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
                                <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="BtnGrabar_Click" TabIndex="6" />
                            </td>
                            <td style="width: 10%"></td>
                            <td style="text-align: left; width: 45%">
                                <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="7" />
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
