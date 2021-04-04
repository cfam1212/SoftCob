<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_Detalle.aspx.cs" Inherits="SoftCob.Views.Mantenedor.WFrm_Detalle" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="~/css/Estilos.css" rel="stylesheet" />
    <script type="text/jscript">

        function salir() {
            if (confirm("Esta seguro que desea cerrar la sesion???"))
                window.parent.location = "~/WFrm_Login.aspx";
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="font-size: 14px; font-style: normal; font-weight: bold; color: #000080;">
        </div>
        <asp:Panel ID="Panel1" runat="server" BorderStyle="Double" BorderColor="#99ccff" Height="100%">
            <table style="width: 100%">
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Image ID="ImgLogo" runat="server" Height="450px" Width="100%" />
                    </td>
                </tr>
            </table>
            <h1 style="text-align:center; color:ActiveCaption">BIENVENIDO</h1>
            <h3 style="text-align:center">
                <strong><asp:Label ID="LblUsuario" runat="server" Text="Label"></asp:Label></strong>
            </h3>
            <h4 style="text-align:center">
                <asp:Label ID="Label1" runat="server" CssClass="Label" Text="Sistema de Gestión de Cobranzas ----&gt; CopyRigth 2020"></asp:Label>
                <%--<asp:Label ID="Label1" runat="server" CssClass="Label" Text="SoftCo. ---&gt; System Manager Client ----&gt; CopyRigth "></asp:Label>--%>
            </h4>
        </asp:Panel>
    </form>
</body>
</html>
