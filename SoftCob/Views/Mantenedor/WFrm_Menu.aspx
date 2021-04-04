<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_Menu.aspx.cs" Inherits="SoftCob.Views.Mantenedor.WFrm_Menu" Theme="admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/jscript">
        function salir() {
            if (confirm("Esta seguro que desea cerrar la sesion???"))
                window.parent.location = "../../WFrm_login.aspx";
        }

        function hideLeftPanel() {
            var oculto = '280px,*';

            if (oculto == parent.document.getElementById('MenuDetalle').cols) {
                parent.document.getElementById('MenuDetalle').cols = '40px,*'
            } else {
                parent.document.getElementById('MenuDetalle').cols = '280px,*'
            }
        }
    </script>
</head>
<body style="text-align: left; height: 100%; margin-bottom: 15px;">
    <form id="form1" runat="server">
        <div style="text-align: left; background-color: #79BBB8;">
            <table border="0" cellpadding="0" cellspacing="0" style="border: thick solid #6699FF; height: 100%; width: 280px;">
                <tr>
                    <td style="width:40px"></td>
                    <td style="width:200px"></td>
                    <td style="width:40px"></td>
                </tr>
                <tr>
                    <td>
                        <asp:ImageButton ID="ImgContraer" runat="server" OnClientClick="return hideLeftPanel()" ImageUrl="~/Botones/ocultarFrame.png" Width="20px" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td style="text-align: center">
                        <asp:LinkButton ID="LnkCerrar" runat="server" Font-Size="20pt" ForeColor="#0066FF" OnClick="LnkCerrar_Click">Cerrar Sesión</asp:LinkButton>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Panel ID="Panel1" runat="server" Height="20px"></asp:Panel>
                    </td>
                </tr>
                <tr> 
                    <td></td>
                    <td style="text-align: center; font-weight: bold; color: #000000; font-size: medium;">
                        <h3 runat="server" style="color:darkblue">Usuario</h3>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td style="text-align: center">
                        <asp:Label ID="LblUsuario" runat="server" Text="Label" Font-Bold="True" Font-Size="14pt" ForeColor="#0066FF"></asp:Label>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Panel ID="PnlDiv0" runat="server" Height="10px"></asp:Panel>
                    </td>
                </tr>
                <tr> 
                    <td></td>
                    <td style="vertical-align: top; height: 100%; text-align: left;">
                        <asp:TreeView ID="Trvmenu" runat="server" NodeIndent="0" NodeWrap="True"
                            ShowExpandCollapse="False" ForeColor="Black" OnSelectedNodeChanged="trvmenu_SelectedNodeChanged">
                            <SelectedNodeStyle BackColor="#D2CFD8" BorderStyle="Inset" BorderWidth="1px" Height="20px"
                                Width="200px" Font-Size="Small" ForeColor="Black" />
                            <NodeStyle ForeColor="Black" Font-Size="Small" />
                            <ParentNodeStyle BackColor="#20365F" BorderStyle="Outset" BorderWidth="1px" Font-Bold="True"
                                Height="20px" Width="200px" Font-Size="Small" ForeColor="Black" />
                            <RootNodeStyle ForeColor="White" BackColor="#20365F" BorderStyle="Outset" BorderWidth="1px" Font-Bold="True"
                                Font-Italic="False" Height="20px" Width="200px" Font-Size="Small" />
                            <LeafNodeStyle ForeColor="Black" BackColor="#99CCFF" BorderStyle="Inset" BorderWidth="1px" Height="15px"
                                Width="180px" HorizontalPadding="10px" Font-Size="Small" />
                        </asp:TreeView>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel2" Height="30px" runat="server"></asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td style="text-align: center">
                        <asp:Image ID="ImgLogo" runat="server" Width="100%" />
                    </td>
                    <td></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
