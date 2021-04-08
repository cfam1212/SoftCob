<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_MonitorTimeGestor.aspx.cs" Inherits="SoftCob.Views.Gestion.WFrm_MonitorTimeGestor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../../Scripts/external/jquery/jquery.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>
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
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-body">
                <asp:UpdatePanel ID="updCabecera" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="panel-info">
                            <asp:Panel ID="pnlCallAnswerd" runat="server" ScrollBars="Vertical" Height="230px" GroupingText="Registros Gestión">
                                <asp:GridView ID="GrdvEfectivas" runat="server" Width="100%"
                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                    ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" TabIndex="1" AutoGenerateColumns="False" PageSize="12" ShowFooter="True" OnRowDataBound="GrdvEfectivas_RowDataBound">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="TipoRegistro" HeaderText="Registro" />
                                        <asp:BoundField DataField="Operaciones" HeaderText="Operaciones">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TotalGestion" HeaderText="T.Gestión">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TotalLlamada" HeaderText="T.Llamada">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                    </Columns>
                                    <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                    <RowStyle Font-Size="X-Small" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                            </asp:Panel>
                        </div>
                        <div class="panel-info">
                            <asp:Panel ID="pnlMaxLlamada" runat="server" ScrollBars="Vertical" Height="200px" GroupingText="Máximo Llamada-Gestión">
                                <asp:GridView ID="GrdvMaxLlamada" runat="server" Width="100%"
                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                    ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" TabIndex="2" AutoGenerateColumns="False" PageSize="12">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="Tipo" HeaderText="Registro" />
                                        <asp:BoundField DataField="MaximoGestion" HeaderText="T.Max.Gestión">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MaximoLlamada" HeaderText="T.Max.LLamada">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                                        <asp:BoundField DataField="Identificacion" HeaderText="Identificación" />
                                        <asp:BoundField DataField="Operacion" HeaderText="Operación" />
                                        <asp:BoundField DataField="TelefonoGestionado" HeaderText="Telefono" />
                                    </Columns>
                                    <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                    <RowStyle Font-Size="X-Small" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                            </asp:Panel>
                        </div>
                        <div class="panel-info">
                            <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Height="200px" GroupingText="Challenger - Efectivas">
                                <asp:GridView ID="GrdvChallenger" runat="server" Width="100%"
                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                    ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" TabIndex="3" AutoGenerateColumns="False" PageSize="12" OnRowDataBound="GrdvChallenger_RowDataBound">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="Gestor" HeaderText="Gestor" />
                                        <asp:BoundField DataField="Operacion" HeaderText="Operaciones">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TotalGestion" HeaderText="T.Gestión">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TotalLlamada" HeaderText="T.Llamada">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Calificación">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCalifica" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                    <RowStyle Font-Size="X-Small" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                            </asp:Panel>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
