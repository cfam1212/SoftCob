<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_BrenchGestiones.aspx.cs" Inherits="SoftCob.Views.Breanch.WFrm_BrenchGestiones" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/Estilos.css" rel="stylesheet" />
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
                <asp:Panel ID="PnlDatosGestiones" runat="server" CssClass="panel panel-primary" Height="350px"
                    ScrollBars="Vertical">
                    <asp:GridView ID="GrdvGestiones" runat="server" AutoGenerateColumns="False"
                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                        ForeColor="#333333" PageSize="15" TabIndex="39" Width="100%">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha Registro" />
                            <asp:BoundField DataField="Gestor" HeaderText="Gestor" />
                            <asp:BoundField DataField="Operacion" HeaderText="Operación" />
                            <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
                            <asp:BoundField DataField="Respuesta" HeaderText="Respuesta" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                            <asp:BoundField DataField="Efectivo" HeaderText="Efectivo">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                        <RowStyle Font-Size="X-Small" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                    </asp:GridView>
                </asp:Panel>
            </div>
        </div>

    </form>
</body>
</html>
