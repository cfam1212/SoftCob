<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_CompararBrench.aspx.cs" Inherits="SoftCob.Views.Breanch.WFrm_CompararBrench" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>

    <style type="text/css">
        legend {
            color: darkblue;
            font-size: 14px;
            font-weight: bold;
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
            <div class="panel-heading" style="background-color: beige; text-align: left; font-size: 25px">
                <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
            </div>

            <div class="panel panel-primary">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 10%"></td>
                        <td style="width: 80%"></td>
                        <td style="width: 10%"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Panel ID="PnlHeaderPagos" runat="server">
                                Pagos Registrados
                                <asp:Image runat="server" ID="ImgCollapsePagos" ImageUrl="~/Botones/collapseopen.png"
                                    Height="20px" />
                            </asp:Panel>
                            <asp:Panel ID="PnlDatosPagos" runat="server" CssClass="panel-primary" Height="200px"
                                GroupingText="Comparando mis Pagos con Todos" ScrollBars="Auto">
                                <asp:GridView ID="GrdvPagos" runat="server"
                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                    ForeColor="#333333" PageSize="3" TabIndex="2" Width="100%" AutoGenerateColumns="False"
                                    DataKeyNames="CodigoGEST,PorCumplido" OnRowDataBound="GrdvPagos_RowDataBound">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="Gestor" HeaderText="Gestor" />
                                        <asp:BoundField DataField="Presupuesto" HeaderText="Presupuesto">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ValCumplido" HeaderText="Pagos">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PorCumplido" HeaderText="% Cumplido">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Calificación"></asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                    <RowStyle Font-Size="X-Small" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                            </asp:Panel>
                            <asp:CollapsiblePanelExtender ID="PnlDatosPagos_CollapsiblePanelExtender" runat="server"
                                Enabled="True" TargetControlID="PnlDatosPagos" ExpandControlID="PnlHeaderPagos"
                                CollapsedImage="~/Botones/collapseopen.png"
                                ExpandedImage="~/Botones/collapseclose.png" CollapsedText="Mostrando Pagos..."
                                ImageControlID="ImgCollapsePagos" SuppressPostBack="True" Collapsed="True"
                                CollapseControlID="PnlHeaderPagos" ExpandedText="Ocultar Pagos">
                            </asp:CollapsiblePanelExtender>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>

            <div class="panel panel-primary">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 10%"></td>
                        <td style="width: 80%"></td>
                        <td style="width: 10%"></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Panel ID="Panel1" runat="server" Height="20px"></asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Panel ID="PnlHeaderCompromiso" runat="server">
                                Compromisos
                                <asp:Image runat="server" ID="ImgCollapseCompromisos" ImageUrl="~/Botones/collapseopen.png" Height="20px" />
                            </asp:Panel>
                            <asp:Panel ID="PnlDatosCompromiso" runat="server" CssClass="panel-primary" Height="200px"
                                GroupingText="Comparando mis Compromisos con Todos" ScrollBars="Auto">
                                <asp:GridView ID="GrdvCompromisos" runat="server"
                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                    ForeColor="#333333" PageSize="3" TabIndex="2" Width="100%" AutoGenerateColumns="False"
                                    DataKeyNames="CodigoGEST,PorCumplido" OnRowDataBound="GrdvCompromisos_RowDataBound">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="Gestor" HeaderText="Gestor" />
                                        <asp:BoundField DataField="Presupuesto" HeaderText="Presupuesto">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ValCumplido" HeaderText="Compromisos">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PorCumplido" HeaderText="% Cumplido">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Calificación"></asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                    <RowStyle Font-Size="X-Small" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                            </asp:Panel>
                            <asp:CollapsiblePanelExtender ID="PnlDatosCompromiso_CollapsiblePanelExtender" runat="server"
                                Enabled="True" TargetControlID="PnlDatosCompromiso" ExpandControlID="PnlHeaderCompromiso"
                                CollapsedImage="~/Botones/collapseopen.png"
                                ExpandedImage="~/Botones/collapseclose.png" CollapsedText="Mostrando Compromisos..."
                                ImageControlID="ImgCollapseCompromisos" SuppressPostBack="True" Collapsed="True"
                                CollapseControlID="PnlHeaderCompromiso" ExpandedText="Ocultar Compromisos">
                            </asp:CollapsiblePanelExtender>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>

            <div class="panel panel-default">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 25%"></td>
                        <td style="width: 50%"></td>
                        <td style="width: 25%"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="text-align: center">
                            <asp:Button ID="BtnSalir" runat="server" CssClass="button" OnClick="BtnSalir_Click" TabIndex="42" Text="Salir" Width="120px" />
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
