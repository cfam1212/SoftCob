<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_DatosEquifax.aspx.cs" Inherits="SoftCob.Views.Gestion.WFrm_DatosEquifax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nueva Nota</title>
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>

    <script type="text/javascript">
        function pageLoad(sender, arg) {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#TxtFechaRecordatorio').datepicker(
                    {
                        inline: true,
                        dateFormat: "mm/dd/yy",
                        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                        numberOfMonths: 1,
                        showButtonPanel: true,
                        changeMonth: true,
                        changeYear: true,
                        yearRange: "-100:+5"
                    });
            });
        }
    </script>
    <style type="text/css">
        legend {
            color: darkblue;
            font-size: 14px;
            font-weight: bold;
        }

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
                <table style="width: 100%">
                    <tr>
                        <td style="width: 10%"></td>
                        <td style="width: 80%"></td>
                        <td style="width: 10%"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Panel ID="Panel1" runat="server" Height="350px" GroupingText="Datos">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 60%"></td>
                                        <td style="width: 5%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h2>No. Código:</h2>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:Label ID="LblCodigo" runat="server" Font-Bold="True" Font-Size="20pt" ForeColor="#337AB7" Text="SIN DATO"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h2>Físico:</h2>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:Label ID="LblFisico" runat="server" Font-Bold="True" Font-Size="20pt" ForeColor="#337AB7" Text="SIN DATO"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h2>Producto:</h2>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:Label ID="LblProducto" runat="server" Font-Bold="True" Font-Size="20pt" ForeColor="#337AB7" Text="SIN DATO"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td><h2>Fec. Concesión:</h2></td>
                                        <td style="text-align: center">
                                            <asp:Label ID="LblFecha" runat="server" Font-Bold="True" Font-Size="20pt" ForeColor="#337AB7" Text="SIN DATO"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="updBotones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right; width: 45%"></td>
                                <td style="width: 10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="5" OnClick="BtnSalir_Click" />
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