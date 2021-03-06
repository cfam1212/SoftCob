<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_MonitorCheckProceso.aspx.cs" Inherits="SoftCob.Views.ReportesManager.WFrm_MonitorCheckProceso" %>

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
            <div class="panel-body">
                <asp:UpdatePanel ID="updPrincipal" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 25%"></td>
                                <td style="width: 25%"></td>
                                <td style="width: 25%"></td>
                                <td style="width: 25%"></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ImageButton ID="ImgExportar" runat="server" ImageUrl="~/Botones/excelbg.png" Width="40px" Height="30px" OnClick="ImgExportar_Click" />
                                    <asp:Label ID="LblExportar" runat="server" Text="Exportar"></asp:Label>
                                </td>
                                <td>
                                    <asp:Button ID="BtnConsultar" runat="server" CausesValidation="False" CssClass="button" OnClick="BtnConsultar_Click" Text="Consultar" Width="120px" />
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Panel ID="PnlDiv0" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Panel ID="Panel1" runat="server" Height="450px" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvDatos" runat="server" Width="100%"
                                            AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" DataKeyNames="CodigoLista,CodigoGestor,PorGestionar,Estado" OnRowDataBound="GrdvDatos_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="Gestor" HeaderText="Gestor">
                                                    <ItemStyle Wrap="True" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Lista" HeaderText="Lista">
                                                    <ItemStyle Wrap="True" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FechaIni" HeaderText="Fecha_Inicio" DataFormatString="{0:d}">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FechaFin" HeaderText="Fecha_Fin" DataFormatString="{0:d}">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TipoMarcado" HeaderText="Tipo" />
                                                <asp:BoundField DataField="Operaciones" HeaderText="Operaciones">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PorGestionar" HeaderText="xGestionar">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Efectivas" HeaderText="Efectivas">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="Chkall" AutoPostBack="true" runat="server" OnCheckedChanged="Chckchanged" Checked="true" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Chkselecc" runat="server" Checked="true" OnCheckedChanged="Chkselecc_CheckedChanged" />
                                                    </ItemTemplate>
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
