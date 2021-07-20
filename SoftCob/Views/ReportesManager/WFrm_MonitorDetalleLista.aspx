<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_MonitorDetalleLista.aspx.cs" Inherits="SoftCob.Views.ReportesManager.WFrm_MonitorDetalleLista" %>

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
                        <table class="table table-bordered table-condensed table-hover table-responsive">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="ImgExportar" runat="server" ImageUrl="~/Botones/excelbg.png" Width="40px" Height="30px" OnClick="ImgExportar_Click" />
                                    <asp:Label ID="lblExportar" runat="server" Text="Exportar"></asp:Label>
                                </td>                         
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <asp:Panel runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="GrdvDatos" runat="server" Width="100%"
                                        AutoGenerateColumns="False"
                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                        ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="Identificacion" HeaderText="Identificacion" />
                                            <asp:BoundField DataField="Operacion" HeaderText="Operación"></asp:BoundField>
                                            <asp:BoundField DataField="Cliente" HeaderText="Cliente"></asp:BoundField>
                                            <asp:BoundField DataField="Sucursal" HeaderText="Sucursal" />
                                            <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                            <asp:BoundField DataField="DiasMora" HeaderText="Dias_Mora" />
                                            <asp:BoundField DataField="TipoCartera" HeaderText="Tipo_Cartera" />
                                            <asp:BoundField DataField="Exigible" HeaderText="Exigible" />
                                        </Columns>
                                        <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                        <RowStyle Font-Size="X-Small" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                    <script>
                                        $(document).ready(function () {
                                            $('#GrdvDatos').dataTable();
                                        });
                                    </script>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel-body">
                <div class="panel-default">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 25%"></td>
                            <td style="width: 25%"></td>
                            <td style="width: 25%"></td>
                            <td style="width: 25%; text-align: right;">
                                <asp:Button ID="BtnRegresar" runat="server" CausesValidation="False" CssClass="button" OnClick="BtnRegresar_Click" Text="Regresar" Width="120px" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
