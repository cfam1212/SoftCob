<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_Login.aspx.cs" Inherits="SoftCob.WFrm_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="css/Login.css" rel="stylesheet" type="text/css" />
    <link href="css/Estilos.css" rel="stylesheet" type="text/css" />
    <link href="~/Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="~/Bootstrap/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Panel ID="pnlLogueo" runat="server">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 20%"></td>
                        <td style="width: 40%"></td>
                        <td style="width: 40%"></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Panel ID="pnlDiv1" runat="server" Height="50px"></asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="2">
                            <asp:Panel ID="pnlLogin" runat="server" Height="350px" Width="100%">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 50%"></td>
                                        <td style="width: 25%"></td>
                                        <td style="width: 25%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="2">
                                            <asp:Panel ID="pnlDatosLogin" runat="server" Height="150px" Width="100%">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 20%"></td>
                                                        <td style="width: 80%"></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Panel ID="pnlDiv2" runat="server" Height="195px"></asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <h5 style="color:#607399; font-weight:bold">Usuario:</h5>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtUsuario" runat="server" CssClass="form-control" MaxLength="80" TabIndex="1" Width="60%"></asp:TextBox>                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <h5 style="color:#607399;font-weight:bold;">Password:</h5>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtClave" runat="server" CssClass="form-control" MaxLength="20" TabIndex="2" Width="60%" TextMode="Password"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="text-align: center">
                                                            <asp:Label ID="LblError" runat="server" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="text-align: center">
                                                            <asp:Label ID="Lblmensaje" runat="server" BackColor="Gold" ForeColor="Black"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:ImageButton ID="BtnIngresar" runat="server" ImageUrl="~/Botones/btnInciarsesion.png" OnClick="BtnIngresar_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Panel ID="pnlDiv0" runat="server" Height="120px"></asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: center">
                            <asp:Label ID="LblPie1" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="20pt" ForeColor="#607399" Text="Sistema Gestión de Cobranza"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
