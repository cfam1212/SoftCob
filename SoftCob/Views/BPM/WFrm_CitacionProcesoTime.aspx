<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_CitacionProcesoTime.aspx.cs" Inherits="SoftCob.Views.BPM.WFrm_CitacionProcesoTime" %>

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

    <script>
        function asegurar() {
            rc = confirm("¿Seguro Cambiar Fecha Citación?");
            return rc;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
            </div>
            <asp:UpdatePanel ID="UpdError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-body">
                <asp:UpdatePanel ID="UpdPrincipal" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="ImgExportar" runat="server" ImageUrl="~/Botones/excel.png" Width="40px" Height="30px" OnClick="ImgExportar_Click" />
                                    <asp:Label ID="lblExportar" runat="server" Text="Exportar"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="GrdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                        ShowHeaderWhenEmpty="True" DataKeyNames="CodigoCITA,CodigoPERS,CodigoCLDE,Terreno,Email,Whastapp" OnRowDataBound="GrdvDatos_RowDataBound">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="FechaCita" HeaderText="Fecha" />
                                            <asp:BoundField DataField="HoraCita" HeaderText="Hora" />
                                            <asp:BoundField DataField="Identificacion" HeaderText="Identificación" />
                                            <asp:BoundField DataField="Cliente" HeaderText="Cliente"></asp:BoundField>
                                            <asp:BoundField DataField="Observacion" HeaderText="Observación"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Terreno">
                                                <ItemTemplate>
                                                    <asp:Image ID="ImgTerreno" runat="server" Height="20px" ImageUrl="~/Botones/vistonegro.png" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Email">
                                                <ItemTemplate>
                                                    <asp:Image ID="ImgEmail" runat="server" Height="20px" ImageUrl="~/Botones/vistonegro.png" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Whastapp">
                                                <ItemTemplate>
                                                    <asp:Image ID="ImgWhastapp" runat="server" Height="20px" ImageUrl="~/Botones/vistonegro.png" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Generar">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgCitacion" runat="server" Height="20px" ImageUrl="~/Botones/btncitaprocesobg.png" OnClick="ImgCitacion_Click" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cambiar">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgCambiar" runat="server" Height="20px" ImageUrl="~/Botones/citamedicabg.png" OnClientClick="return asegurar();" OnClick="ImgCambiar_Click" />
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
                                            $('#GrdvDatos').dataTable();
                                        });
                                    </script>
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
