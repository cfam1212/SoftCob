<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_ReporteConsolidado.aspx.cs" Inherits="SoftCob.Views.ReportesManager.WFrm_ReporteConsolidado" %>

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

        .auto-style1 {
            width: 100%;
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
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updPrincipal">
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
                <asp:UpdatePanel ID="updPrincipal" runat="server">
                    <ContentTemplate>
                        <table class="table table-bordered table-condensed table-hover table-responsive">
                            <tr>
                                <td>
                                    <asp:GridView ID="GrdvDatos" runat="server" Width="100%"
                                        AutoGenerateColumns="False" DataKeyNames="codigoCPCE,codigoCEDE"
                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                        ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="Catalago" HeaderText="Catálogo/Producto" />
                                            <asp:BoundField DataField="Operaciones" HeaderText="Operaciones">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Saldo" HeaderText="Saldos">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Ver">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Btnselecc" runat="server" Height="20px" ImageUrl="~/Botones/Buscar.png" OnClick="Btnselecc_Click" />
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
                        <td style="text-align: right">
                            <h5 style="color: blue">TOTAL:<span></span></h5>
                        </td>
                        <td style="text-align: right">
                            <h5 runat="server" id="lblOperaciones" style="color: blue"></h5>
                        </td>
                        <td style="text-align: right">
                            <h5 runat="server" id="lblSaldos" style="color: blue">$</h5>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
