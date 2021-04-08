<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_SeleccionListaTrabajo.aspx.cs" Inherits="SoftCob.Views.Gestion.WFrm_SeleccionListaTrabajo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <title>Selección Lista Activa</title>
    <script src="../../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../../Scripts/jquery-ui.min.js"></script>

    <script>
        function SmartPhone(cliente, servidor) {
            try {
                var linea;
                switch (cliente) {
                    case "ELASTIX":
                        linea = servidor;
                        break;
                    default:
                        alert('No Existe Cliente de Telefonía..!');
                        break;
                }

                program = new ActiveXObject("WScript.Shell");

                program.run(linea);
                $("#divRedirect").attr("style", "visibility:visible");

            }
            catch (e) {
                alert("Configure los objetos ActiveX, Solicite al administrador. \n" + e.message + ' ' + e.stack);
            }
        }
    </script>
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
                        <td style="width: 5%"></td>
                        <td style="width: 15%"></td>
                        <td style="width: 55%"></td>
                        <td style="width: 20%"></td>
                        <td style="width: 5%"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td>
                            <asp:DropDownList ID="DdlListaTrabajo" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlListaTrabajo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <div id="divRedirect" style="visibility: hidden">
                                <asp:Button ID="BtnRedirect" runat="server" Text="Gestionar" CssClass="button" OnClick="BtnRedirect_Click" OnClientClick="LevantarProgressModal();" />
                            </div>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="divException" class="alert-danger" style="visibility: hidden">
            <asp:Literal ID="litMensajeEx" runat="server">

            </asp:Literal>
        </div>

        <div id="divWaitToPhoneClient" class="modal">
            <div class="panel panel-body">
                <h2>Se está iniciando el cliente de telefonía, espere por favor...</h2>
                <img src="../../Images/load.gif" alt="Sistema trabajando..." style="width: 100px; height: 100px; margin-left: auto; margin-right: auto; left: 0; right: 0" />
            </div>

        </div>
    </form>
</body>
</html>
<script>
    function funMensajeException() {
        $("#divException").attr("style", "visibility:visible");
    }
    function LevantarProgressModal() {
        $("#divWaitToPhoneClient").show();
    }
</script>
