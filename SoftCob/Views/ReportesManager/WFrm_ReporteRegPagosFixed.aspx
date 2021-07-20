<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_ReporteRegPagosFixed.aspx.cs" Inherits="SoftCob.Views.ReportesManager.WFrm_ReporteRegPagosFixed" %>

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
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <script src="../../JS/alertify.min.js"></script>
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
                        <div class="panel panel-default">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 25%"></td>
                                    <td style="width: 25%"></td>
                                    <td style="width: 25%"></td>
                                    <td style="width: 25%"></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">
                                        <asp:ImageButton ID="ImgExportar" runat="server" ImageUrl="~/Botones/excelbg.png" Width="40px" Height="30px" TabIndex="1" OnClick="ImgExportar_Click" />
                                        <asp:Label ID="LblExportar" runat="server" Text="Exportar"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="BtnConsultar" runat="server" CausesValidation="False" CssClass="button" OnClick="BtnConsultar_Click" Text="Consultar" Width="120px" TabIndex="2" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                        <div class="panel-info">
                            <asp:Panel ID="pnlGestiones" runat="server" Width="100%" Height="500px" ScrollBars="Auto">
                                <asp:GridView ID="GrdvDatos" runat="server" Width="100%"
                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                    AutoGenerateColumns="False"
                                    ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" PageSize="50"
                                    TabIndex="3">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="Cliente" HeaderText="Cliente">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Identificacion" HeaderText="Identificación">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Operacion" HeaderText="Operación">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Documento" HeaderText="No.Documento">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FechaPago" HeaderText="Fecha_Pago">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TotalDeuda" HeaderText="Total_Deuda">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Exigible" HeaderText="Exigible">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Pago" HeaderText="Pago">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
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
                            </asp:Panel>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
