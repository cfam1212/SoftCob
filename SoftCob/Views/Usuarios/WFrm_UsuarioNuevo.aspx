<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_UsuarioNuevo.aspx.cs" Inherits="SoftCob.Views.Usuarios.WFrm_UsuarioNuevo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nuevo Usuario</title>
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <link href="../../JS/css/alertify.min.css" rel="stylesheet" />

    <script type="text/javascript" src="../../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../JS/alertify.min.js"></script>

    <script type="text/javascript">
        function pageLoad(sender, arg) {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#txtFechaCaduca').datepicker(
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
            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updBotones">
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
                <asp:UpdatePanel ID="updCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 15%">
                                    <h5>Nombres:</h5>
                                </td>
                                <td style="width: 30%">
                                    <asp:TextBox ID="TxtNombres" runat="server" Width="100%" CssClass="form-control upperCase" MaxLength="80" TabIndex="1"></asp:TextBox>
                                </td>
                                <td style="width: 15%">
                                    <h5>Apellidos:</h5>
                                </td>
                                <td style="width: 30%">
                                    <asp:TextBox ID="TxtApellidos" runat="server" Width="100%" CssClass="form-control upperCase" MaxLength="80" TabIndex="2"></asp:TextBox>
                                </td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Login:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtUser" runat="server" CssClass="form-control" Width="100%" MaxLength="16" TabIndex="3"></asp:TextBox>
                                </td>
                                <td>
                                    <h5>Password:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtPassword" runat="server" Width="100%" MaxLength="16" CssClass="form-control" TextMode="Password" TabIndex="4"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Departamento:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlDepartamento" runat="server" Width="100%" CssClass="form-control" TabIndex="5">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>Perfil:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlPerfil" runat="server" Width="100%" CssClass="form-control" TabIndex="6">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Password Caduca:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkCaduca" runat="server" AutoPostBack="True" Text="No" CssClass="form-control" OnCheckedChanged="ChkCaduca_CheckedChanged" TabIndex="7" />
                                </td>
                                <td>
                                    <h5 runat="server" id="Label10">Fecha Caduca:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtFechaCaduca" runat="server" Width="100%" Enabled="false" TabIndex="8"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Cambiar Password:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkCambiar" runat="server" AutoPostBack="True" Text="No" CssClass="form-control" OnCheckedChanged="ChkCambiar_CheckedChanged" TabIndex="9" />
                                </td>
                                <td>
                                    <h5>Permisos Especiales:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkPermisos" runat="server" AutoPostBack="True" CssClass="form-control" Text="No" OnCheckedChanged="ChkPermisos_CheckedChanged" TabIndex="10" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5 runat="server" id="Label7" visible="false">Estado:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkEstado" runat="server" AutoPostBack="True" Visible="False" Text="Activo" Checked="True" CssClass="form-control" OnCheckedChanged="ChkEstado_CheckedChanged" TabIndex="11" />
                                </td>
                                <td>
                                    <h5>Tipo Usuario:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlTipoUsuario" runat="server" CssClass="form-control" Width="100%" TabIndex="12">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="updBotones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right; width: 45%">
                                    <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="BtnGrabar_Click" TabIndex="13" />
                                </td>
                                <td style="width: 10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="14" />
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