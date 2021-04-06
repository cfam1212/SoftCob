<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_UpdateDeudor.aspx.cs" Inherits="SoftCob.Views.Cedente.WFrm_UpdateDeudor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/jquery.ui.accordion.css" rel="stylesheet" />
    <script src="../../Scripts/external/jquery/jquery.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <script src="../../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../../Scripts/jquery-ui.min.js"></script>

    <script type="text/javascript">
        function pageLoad(sender, arg) {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#TxtFechaNacimiento').datepicker(
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

        function Validar_Cedula() {
            var ddltipodoc = document.getElementById("<%=DdlTipoDocumento.ClientID%>").value;
            var cedula = document.getElementById("<%=TxtNumeroDocumento.ClientID%>").value;
            var digito_region = cedula.substring(0, 2);
            if (ddltipodoc == "C") {
                arreglo = cedula.split("");
                num = cedula.length;
                if (digito_region >= 1 && digito_region <= 25) {
                    if (num == 10) {
                        //validar cedula
                        digito = (arreglo[9] * 1);
                        total = 0;
                        for (i = 0; i < (num - 1); i++) {
                            if ((i % 2) != 0) {
                                total = total + (arreglo[i] * 1);
                            } else {
                                mult = arreglo[i] * 2;
                                if (mult > 9) {
                                    total = total + (mult - 9);
                                } else {
                                    total = total + mult;
                                }
                            }
                        }
                        decena = total / 10;
                        decena = Math.floor(decena);
                        decena = (decena + 1) * 10;
                        final = (decena - total);
                        if (final == 10) {
                            final = 0;
                        }
                        if (digito == final) {
                            <%--document.getElementById("<%=txtnombre1.ClientID%>").value = ""--%>
                            return true;
                        } else {
                            alert("Cédula Incorrecta");
                            document.getElementById("<%=TxtNumeroDocumento.ClientID%>").value = "";
                            return false;
                        }
                    }
                    else {
                        alert("Cédula Incorrecta");
                        document.getElementById("<%=TxtNumeroDocumento.ClientID%>").value = "";
                        document.getElementById("<%=TxtNumeroDocumento.ClientID%>").focus;
                    }
                }
                else {
                    document.getElementById("<%=TxtNumeroDocumento.ClientID%>").value = "";
                    document.getElementById("<%=TxtNumeroDocumento.ClientID%>").focus;
                    alert("Cédula Incorrecta");
                }
            }
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
        <div class="panel panel-primary">
            <div class="panel-heading" style="background-color: #79BBB8;">
                <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
            </div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdBotones">
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
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">DATOS PERSONALES</h3>
                <asp:UpdatePanel ID="updCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 35%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Tipo Documento:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlTipoDocumento" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlTipoDocumento_SelectedIndexChanged" TabIndex="1" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>No. Documento:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtNumeroDocumento" runat="server" CssClass="upperCase" MaxLength="20" TabIndex="2" Width="100%" OnTextChanged="TxtNumeroDocumento_TextChanged" AutoPostBack="True"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="TxtNumeroDocumento_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="TxtNumeroDocumento">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Nombres:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtNombres" runat="server" Width="100%" CssClass="upperCase" MaxLength="80" TabIndex="3"></asp:TextBox>
                                </td>
                                <td>
                                    <h5>Apellidos:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtApellidos" runat="server" Width="100%" CssClass="upperCase" MaxLength="80" TabIndex="4"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Fec. Nacimiento:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtFechaNacimiento" runat="server" Width="100%" CssClass="form-control" TabIndex="5"></asp:TextBox>
                                </td>
                                <td>
                                    <h5>Género:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlGenero" runat="server" CssClass="form-control" TabIndex="6" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Est. Civil:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlEstCivil" runat="server" CssClass="form-control" TabIndex="7" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>email Personal:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtCorreo" runat="server" CssClass="lowCase" MaxLength="100" TabIndex="8" Width="100%"></asp:TextBox>
                                </td>
                                <td>
                                    <h5>e-Empresa:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtMailEmpresa" runat="server" CssClass="lowCase" MaxLength="100" TabIndex="9" Width="100%"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Provincia:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlProvincia" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlProvincia_SelectedIndexChanged" TabIndex="10" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>Ciudad:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlCiudad" runat="server" CssClass="form-control" TabIndex="11" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Dir.Domicilio:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="Txtdirdomicilio" runat="server" CssClass="fupperCase" Height="50px" MaxLength="150" onkeydown="return (event.keyCode!=13);" TabIndex="12" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                </td>
                                <td>
                                    <h5>Ref.Domicilio:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="Txtrefdomicilio" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="upperCase" Height="50px" MaxLength="150" TabIndex="13" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Dir.Trabajo:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="Txtdirtrabajo" runat="server" CssClass="upperCase" Height="50px" MaxLength="150" onkeydown="return (event.keyCode!=13);" TabIndex="14" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                </td>
                                <td>
                                    <h5>Ref.Trabajo:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtReftrabajo" runat="server" CssClass="upperCase" Height="50px" MaxLength="150" onkeydown="return (event.keyCode!=13);" TabIndex="15" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="UpdBotones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right; width: 45%">
                                    <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="BtnGrabar_Click" TabIndex="15" />
                                </td>
                                <td style="width: 10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="16" />
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
