<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_ListaTrabajoAdminFDA.aspx.cs" Inherits="SoftCob.Views.ListaTrabajo.WFrm_ListaTrabajoAdminFDA" %>

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
                        <table class="table table-bordered table-condensed table-hover table-responsive">
                            <tr>
                                <td>
                                    <button id="BtnNuevo" runat="server" type="submit" class="btn btn-primary"
                                        onserverclick="BtnNuevo_Click">
                                        <span aria-hidden="true"
                                            class="glyphicon glyphicon-plus"></span>
                                    </button>
                                </td>
                            </tr>
                        </table>
                        <table class="table table-bordered table-condensed table-hover table-responsive">
                            <tr>
                                <td>
                                    <asp:GridView ID="GrdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                        ShowHeaderWhenEmpty="True" DataKeyNames="Codigo">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="Lista" HeaderText="Lista Trabajo" />
                                            <asp:BoundField DataField="FechaInicio" HeaderText="Fecha Inicio" />
                                            <asp:BoundField DataField="FechaFin" HeaderText="Fecha Fin" />
                                            <asp:BoundField DataField="Gestor" HeaderText="Gestor" />
                                            <asp:BoundField DataField="Marcado" HeaderText="Tipo Marcado" />
                                            <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                            <asp:TemplateField HeaderText="Editar">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Btnselecc" runat="server" Height="20px" ImageUrl="~/Botones/modificarbg.png" OnClick="Btnselecc_Click" />
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
