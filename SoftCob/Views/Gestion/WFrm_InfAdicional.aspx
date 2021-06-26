<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_InfAdicional.aspx.cs" Inherits="SoftCob.Views.Gestion.WFrm_InfAdicional" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <link href="../../JS/css/alertify.min.css" rel="stylesheet" />

    <script type="text/javascript" src="../../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../JS/alertify.min.js"></script>
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
                        <td style="width: 5%"></td>
                        <td style="width: 90%"></td>
                        <td style="width: 5%"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Panel ID="PnlDatos" runat="server" CssClass="panel-primary" Height="200px"
                                GroupingText="Datos Adicionales" ScrollBars="Auto">
                                <asp:GridView ID="GrdvDatos" runat="server"
                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                    ForeColor="#333333" PageSize="3" TabIndex="2" Width="100%">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                    <RowStyle Font-Size="X-Small" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                            </asp:Panel>
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
