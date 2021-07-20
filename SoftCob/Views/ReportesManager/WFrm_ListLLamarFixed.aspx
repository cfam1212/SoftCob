<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_ListLLamarFixed.aspx.cs" Inherits="SoftCob.Views.ReportesManager.WFrm_ListLLamarFixed" %>

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
                <asp:Label ID="LblTitulo" runat="server"></asp:Label>
            </div>
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="LblError" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-body">
                <asp:UpdatePanel ID="updPrincipal" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="ImgExportar" runat="server" ImageUrl="~/Botones/excelbg.png" Width="40px" Height="30px" OnClick="ImgExportar_Click" />
                                    <asp:Label ID="lblExportar" runat="server" Text="Exportar"></asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Panel runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="GrdvDatos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="FechaLlamar,HoraLlamar" OnRowDataBound="GrdvDatos_RowDataBound" ShowHeaderWhenEmpty="True" Width="100%">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha_Registro" />
                                            <asp:BoundField DataField="FechaLlama" HeaderText="Fecha_Llamar">
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                            <asp:BoundField DataField="Identificacion" HeaderText="Identificación" />
                                            <asp:BoundField DataField="Cliente" HeaderText="Cliente">
                                            <ItemStyle Wrap="True" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Operacion" HeaderText="Operación" />
                                            <asp:BoundField DataField="Exigible" HeaderText="Exigible">
                                            <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FechaUltGestion" HeaderText="Fec.Ult_Gestión">
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Gestor" HeaderText="Gestor" />
                                        </Columns>
                                        <RowStyle Font-Size="X-Small" />
                                    </asp:GridView>
                                    <script>

                                        $(document).ready(function () {
                                            $('#GrdvDatos').dataTable();
                                        });
                                    </script>
                                </td>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel panel-default">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 25%"></td>
                        <td style="width: 25%"></td>
                        <td style="width: 25%"></td>
                        <td style="width: 25%"></td>
                        <td style="width: 25%"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td>
                            <asp:Button ID="BtnConsultar" runat="server" Text="Consultar" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnConsultar_Click" />
                        </td>
                        <td>
                            <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" />
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
