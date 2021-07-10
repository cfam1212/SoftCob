<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_EmployeeAdmin.aspx.cs" Inherits="SoftCob.Views.Employee.WFrm_EmployeeAdmin" %>

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
                                            class="glyphicon glyphicon-plus" style="left: 1px; top: -3px"></span>
                                    </button>
                                </td>
                            </tr>
                        </table>
                        <table class="table table-bordered table-condensed table-hover table-responsive">
                            <tr>
                                <td>
                                    <asp:GridView ID="GrdvDatos" runat="server" Width="100%"
                                        AutoGenerateColumns="False" DataKeyNames="Codigo,Asignado,CodigoUsu"
                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                        ShowHeaderWhenEmpty="True" EmptyDataText="No existen usuario creados" OnRowDataBound="GrdvDatos_RowDataBound">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:HyperLinkField DataNavigateUrlFields="Urllink" DataTextField="Identificacion" HeaderText="Identificación">
                                            <ControlStyle ForeColor="Black" />
                                            </asp:HyperLinkField>
                                            <asp:HyperLinkField DataNavigateUrlFields="Urllink" DataTextField="Nombres" HeaderText="Nombres">
                                            <ControlStyle ForeColor="Black" />
                                            </asp:HyperLinkField>
                                            <asp:HyperLinkField DataNavigateUrlFields="Urllink" DataTextField="Apellidos" HeaderText="Apellidos">
                                            <ControlStyle ForeColor="Black" />
                                            </asp:HyperLinkField>
                                            <asp:HyperLinkField DataNavigateUrlFields="Urllink" DataTextField="Departamento" HeaderText="Departamento">
                                            <ControlStyle ForeColor="Black" />
                                            </asp:HyperLinkField>
                                            <asp:HyperLinkField DataNavigateUrlFields="Urllink" DataTextField="Estado" HeaderText="Estado">
                                            <ControlStyle ForeColor="Black" />
                                            </asp:HyperLinkField>
                                            <asp:TemplateField HeaderText="Asignar Usuario">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgAsignarUsu" runat="server" Height="20px" ImageUrl="~/Botones/agregar_usuariobg.png" OnClick="ImgAsignarUsu_Click" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Desasignar Usuario">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgQuitarUsu" runat="server" Height="20px" ImageUrl="~/Botones/quitar_usuariobg.png" OnClick="ImgQuitarUsu_Click" />
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
