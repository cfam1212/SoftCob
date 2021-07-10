<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_NuevoBrench.aspx.cs" Inherits="SoftCob.Views.Breanch.WFrm_NuevoBrench" %>

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
        <div class="panel panel-primary">
            <div class="panel-heading" style="background-color: #79BBB8;">
                <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
            </div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
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
                <asp:UpdatePanel ID="updCabecera" runat="server" UpdateMode="Conditional">
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
                                    <asp:DropDownList ID="DdlCedente" CssClass="form-control" runat="server" Width="100%" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="DdlCedente_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>Catálogo:</h5>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="DdlCatalogo" CssClass="form-control" runat="server" AutoPostBack="True" TabIndex="2" Width="100%" OnSelectedIndexChanged="DdlCatalogo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Rango Días:</h5>
                                </td>
                                <td>
                                    <h5>Inicial:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtRinicio" runat="server" CssClass="form-control alinearDerecha" Width="100%" TabIndex="3" MaxLength="4"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtRinicio_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtRinicio">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <h5>Final:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtRFin" runat="server" CssClass="form-control alinearDerecha" Width="100%" TabIndex="4" MaxLength="4"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtRFin_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtRFin">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5 runat="server" id="lblEstado" visible="false">Estado:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkEstadoBrench" runat="server" AutoPostBack="True" Checked="True" OnCheckedChanged="ChkEstadoBrench_CheckedChanged" TabIndex="10" Text="Activo" Visible="False" />
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <asp:Panel ID="pnlDiv1" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:ImageButton ID="ImgAgregar" runat="server" Height="25px" ImageUrl="~/Botones/agregarbg.png" TabIndex="5" OnClick="ImgAgregar_Click" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="ImgModificar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/modificarbg.png" TabIndex="6" OnClick="ImgModificar_Click" />
                                </td>
                                <td></td>
                            </tr>
                        </table>
                        <div class="panel-info">
                            <asp:Panel ID="pnlRangoDias" runat="server" ScrollBars="Vertical" Height="280px" GroupingText="Brench">
                                <asp:GridView ID="GrdvBrench" runat="server" Width="100%"
                                    CssClass="table table-condensed table-bordered table-hover table-responsive" EmptyDataText="No existen datos para mostrar" TabIndex="7" AutoGenerateColumns="False" PageSize="12" DataKeyNames="Codigo,Orden" OnRowDataBound="GrdvBrench_RowDataBound">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="RangoIni" HeaderText="R.Inicial">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RangoFin" HeaderText="R.Final">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Etiqueta" HeaderText="Etiqueta" />
                                        <asp:TemplateField HeaderText="Estado">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkEstado" runat="server" OnCheckedChanged="ChkEstado_CheckedChanged" AutoPostBack="True" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Subir Nivel" Visible="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgSubir" runat="server" Height="20px" ImageUrl="~/Botones/activada_up.png" OnClick="ImgSubir_Click" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Eliminar">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgEliminar" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgEliminar_Click" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Selecc">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgSelecc" runat="server" Height="20px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSelecc_Click" />
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
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            
            <div class="panel panel-default">
                <asp:UpdatePanel ID="updBotones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right; width: 45%">
                                    <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CausesValidation="False" CssClass="button" TabIndex="7" OnClick="BtnGrabar_Click" />
                                </td>
                                <td style="width: 10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="8" OnClick="BtnSalir_Click" />
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
