<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_ConsultaBitacoraAdmin.aspx.cs" Inherits="SoftCob.Views.Bitacora.WFrm_ConsultaBitacoraAdmin" %>

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
        <div class="panel panel-primary">
            <div class="panel-heading" style="background-color: #79BBB8;">
                <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
            </div>
            <div class="panel-body">
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                </asp:ToolkitScriptManager>
                <asp:UpdatePanel ID="UpdPrincipal" runat="server">
                    <ContentTemplate>
                        <table class="table table-bordered table-condensed table-hover table-responsive">
                            <tr>
                                <td>
                                    <asp:GridView ID="GrdvDatos" runat="server" Width="100%"
                                        AutoGenerateColumns="False" DataKeyNames="Codigo,Bitacora,Estado,Fecha"
                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                        ShowHeaderWhenEmpty="True" EmptyDataText="No existen usuario creados" OnRowDataBound="GrdvDatos_RowDataBound">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="Bitacora" HeaderText="Bitacora" />
                                            <asp:BoundField DataField="Usuario" HeaderText="Supervisor" />
                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha_Bitacora" />
                                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                            <asp:TemplateField HeaderText="Detalle">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="BtnDetalle" runat="server" Height="20px" ImageUrl="~/Botones/btnnotepad.png" OnClick="BtnDetalle_Click" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                        <RowStyle Font-Size="X-Small" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                    <script>
                                        $(document).ready(function () {
                                            $('#GrdvDatos').dataTable(
                                                {
                                                    "order": [0,'desc']
                                                });
                                        });
                                    </script>
                                </td>
                            </tr>
                        </table>

                        <table style="width:100%">
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td style="text-align: right">
                                    <asp:Button ID="BtnSalir" runat="server" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="4" Text="Salir" Width="120px" />
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
