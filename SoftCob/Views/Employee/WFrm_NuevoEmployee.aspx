<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_NuevoEmployee.aspx.cs" Inherits="SoftCob.Views.Employee.WFrm_NuevoEmployee" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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

    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>

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

    <script>
        $(function () {
            $("#acordionParametro").accordion();
        });
    </script>

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

            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#TxtFecIniOtrosE').datepicker(
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

            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#TxtFecFinOtrosE').datepicker(
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

            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#TxtFecIniEmpre').datepicker(
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

            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#TxtFecFinEmpre').datepicker(
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
            var cedula = document.getElementById("<%=TxtIdentificacion.ClientID%>").value;
            var digito_region = cedula.substring(0, 2);
            if (ddltipodoc == "C") {
                arreglo = cedula.split("");
                num = cedula.length;
                if (digito_region >= 1 && digito_region <= 24) {
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
                            document.getElementById("<%=TxtIdentificacion.ClientID%>").value = "";
                            return false;
                        }
                    }
                    else {
                        alert("Cédula Incorrecta");
                        document.getElementById("<%=TxtIdentificacion.ClientID%>").value = "";
                        document.getElementById("<%=TxtIdentificacion.ClientID%>").focus;
                    }
                }
                else {
                    document.getElementById("<%=TxtIdentificacion.ClientID%>").value = "";
                    document.getElementById("<%=TxtIdentificacion.ClientID%>").focus;
                    alert("Cédula Incorrecta");
                }
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
            <%-- <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updOpciones">
                    <ProgressTemplate>
                        <div class="overlay" />
                        <div class="overlayContent">
                            <h2>Procesando..</h2>
                            <img src="../../Images/load.gif" alt="Loading" border="1" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>--%>
            <div class="table-responsive">
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">DATOS PERSONALES</h3>
                <asp:UpdatePanel ID="updDatosPersonales" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 15%">
                                    <h5>Tipo Documento:</h5>
                                </td>
                                <td style="width: 30%">
                                    <asp:DropDownList ID="DdlTipoDocumento" runat="server" CssClass="form-control" Width="100%" OnSelectedIndexChanged="DdlTipoDocumento_SelectedIndexChanged" AutoPostBack="True" TabIndex="1">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 15%">
                                    <h5>No. Documento:</h5>
                                </td>
                                <td style="width: 30%">
                                    <asp:TextBox ID="TxtIdentificacion" runat="server" Width="100%" MaxLength="20" CssClass="form-control upperCase" TabIndex="2"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtIdentificacion_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtIdentificacion">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Nombres:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtNombres" runat="server" Width="100%" MaxLength="80" CssClass="form-control upperCase" TabIndex="3"></asp:TextBox>
                                </td>
                                <td>
                                    <h5>Apellidos:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtApellidos" runat="server" Width="100%" MaxLength="80" CssClass="form-control upperCase" TabIndex="4"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Género:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlGenero" runat="server" CssClass="form-control" Width="100%" TabIndex="5">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>Est. Civil:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlEstadoCivil" runat="server" CssClass="form-control" Width="100%" TabIndex="6">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Nacionalidad:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlNacionalidad" runat="server" CssClass="form-control" Width="100%" TabIndex="7">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>Fecha_Nacimiento:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtFechaNacimiento" runat="server" CssClass="form-control" Width="100%" TabIndex="8"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Departamento:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlDepartamento" runat="server" CssClass="form-control" Width="100%" TabIndex="9">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>Tipo Sangre:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlTipoSangre" runat="server" CssClass="form-control" Width="100%" TabIndex="10">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Dirección:</h5>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="TxtDireccion" runat="server" onkeydown="return (event.keyCode!=13);" Width="100%" MaxLength="255" CssClass="form-control upperCase" TextMode="MultiLine" Height="40px" TabIndex="11"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Telefono_1:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtTelefono1" runat="server" CssClass="form-control" Width="100%" MaxLength="10" TabIndex="12"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtTelefono1_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtTelefono1">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <h5>Teléfono_2:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtTelefono2" runat="server" CssClass="form-control" Width="100%" MaxLength="10" TabIndex="13"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtTelefono2_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtTelefono2">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Celular_1:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtCelular1" runat="server" CssClass="form-control" Width="100%" MaxLength="10" TabIndex="14"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtCelular1_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtCelular1">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <h5>Celular_2:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtCelular2" runat="server" CssClass="form-control" Width="100%" MaxLength="10" TabIndex="15"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtCelular2_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtCelular2">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Email_1:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtEmail1" runat="server" Width="100%" MaxLength="80" CssClass="form-control lowCase" TabIndex="16"></asp:TextBox>
                                </td>
                                <td>
                                    <h5>Email_2:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtEmail2" runat="server" Width="100%" MaxLength="80" CssClass="form-control lowCase" TabIndex="17"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5 id="lblEstado" runat="server" visible="false">Estado:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkEstado" runat="server" CssClass="form-control" AutoPostBack="True" Checked="True" Text="Activo" Visible="False" OnCheckedChanged="ChkEstado_CheckedChanged" TabIndex="18" />
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>

                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel-body">
                <div id="acordionParametro">
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Estudios</h3>
                    <asp:UpdatePanel ID="updEstudios" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 1%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 9%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Primaria:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="TxtPrimaria" runat="server" CssClass="form-control upperCase" MaxLength="150" Width="100%" TabIndex="19"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Fecha Inicio:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlFecIniPrimaria" runat="server" CssClass="form-control" Width="100%" TabIndex="20">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: center;">
                                            <h5>Fecha Fin:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlFecFinPrimaria" runat="server" CssClass="form-control" Width="100%" TabIndex="21">
                                            </asp:DropDownList>

                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:Panel ID="Panel1" runat="server" Height="20px">
                                            </asp:Panel>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Secundaria:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="TxtSecundaria" runat="server" CssClass="form-control upperCase" MaxLength="150" Width="100%" TabIndex="22"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Fecha Inicio:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlFecIniSecundaria" runat="server" CssClass="form-control" Width="100%" TabIndex="23">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: center;">
                                            <h5>Fecha Fin:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlFecFinSecundaria" runat="server" CssClass="form-control" Width="100%" TabIndex="24">
                                            </asp:DropDownList>

                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Titulo Bachiller:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="TxtTituloS" runat="server" CssClass="form-control upperCase" MaxLength="150" Width="100%" TabIndex="25"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:Panel ID="Panel2" runat="server" Height="20px">
                                            </asp:Panel>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Superior:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="TxtSuperior" runat="server" CssClass="form-control upperCase" MaxLength="150" Width="100%" TabIndex="26"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Fecha Inicio:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlFecIniSuperior" runat="server" CssClass="form-control" Width="100%" TabIndex="27">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: center;">
                                            <h5>Fecha Fin:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlFecFinSuperior" runat="server" CssClass="form-control" Width="100%" TabIndex="28">
                                            </asp:DropDownList>

                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Titulo Superior:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="TxtTituloR" runat="server" CssClass="form-control upperCase" MaxLength="150" Width="100%" TabIndex="29"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Otros Estudios</h3>
                    <asp:UpdatePanel ID="updOtrosEstudios" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 1%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 9%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Institución:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="TxtInstitucion" runat="server" CssClass="form-control upperCase" MaxLength="150" Width="100%" TabIndex="30"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Fecha Inicio:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtFecIniOtrosE" runat="server" CssClass="form-control" Width="100%" TabIndex="31"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center;">
                                            <h5>Fecha Fin:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtFecFinOtrosE" runat="server" CssClass="form-control" Width="100%" TabIndex="32"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:ImageButton ID="ImgAgregarOtrosEstudios" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAgregarOtrosEstudios_Click" TabIndex="34" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Título:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="TxtTituloOtrosEstudios" runat="server" CssClass="form-control upperCase" MaxLength="150" Width="100%" TabIndex="33"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:Panel ID="Panel3" runat="server" Height="30px">
                                            </asp:Panel>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="4">
                                            <asp:Panel ID="pnlOtrosEstudios" runat="server" ScrollBars="Vertical" Height="230px">
                                                <asp:GridView ID="GrdvOtrosEstudios" runat="server" AutoGenerateColumns="False"
                                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                    DataKeyNames="Codigo" ForeColor="#333333" PageSize="5" Width="100%" TabIndex="35">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Institucion" HeaderText="Institución" />
                                                        <asp:BoundField DataField="FechaDesde" HeaderText="Fecha Inicio" />
                                                        <asp:BoundField DataField="FechaHasta" HeaderText="Fecha Fin" />
                                                        <asp:BoundField HeaderText="Título" DataField="Titulo" />
                                                        <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgDelOtrosEstudios" runat="server" Height="20px" ImageUrl="~/Botones/eliminar.png" Width="15px" OnClick="ImgDelOtrosEstudios_Click" />
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
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Idiomas</h3>
                    <asp:UpdatePanel ID="updIdiomas" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 1%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 9%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Idioma:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DdlIdiomas" runat="server" CssClass="form-control" Width="100%" TabIndex="36">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Nivel Escrito %:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlNivelE" runat="server" CssClass="form-control" Width="100%" TabIndex="37">
                                                <asp:ListItem>5%</asp:ListItem>
                                                <asp:ListItem>20%</asp:ListItem>
                                                <asp:ListItem>30%</asp:ListItem>
                                                <asp:ListItem>50%</asp:ListItem>
                                                <asp:ListItem>80%</asp:ListItem>
                                                <asp:ListItem>90%</asp:ListItem>
                                                <asp:ListItem>100%</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: center;">
                                            <h5>Nivel Hablado %:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlNivelH" runat="server" CssClass="form-control" Width="100%" TabIndex="38">
                                                <asp:ListItem>5%</asp:ListItem>
                                                <asp:ListItem>20%</asp:ListItem>
                                                <asp:ListItem>30%</asp:ListItem>
                                                <asp:ListItem>50%</asp:ListItem>
                                                <asp:ListItem>80%</asp:ListItem>
                                                <asp:ListItem>90%</asp:ListItem>
                                                <asp:ListItem>100%</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ImgAgregarIdiomas" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAgregarIdiomas_Click" TabIndex="39" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:Panel ID="Panel4" runat="server" Height="30px">
                                            </asp:Panel>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="4">
                                            <asp:Panel ID="pnlIdiomas" runat="server" ScrollBars="Vertical" Height="230px">
                                                <asp:GridView ID="GrdvIdiomas" runat="server" AutoGenerateColumns="False"
                                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                    DataKeyNames="Codigo,CodigoIdioma" ForeColor="#333333" PageSize="5" Width="100%" TabIndex="40">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Idioma" HeaderText="Idioma" />
                                                        <asp:BoundField DataField="NivelH" HeaderText="Hablado(%)" />
                                                        <asp:BoundField DataField="NivelE" HeaderText="Escrito(%)" />
                                                        <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgDelIdiomas" runat="server" Height="15px" ImageUrl="~/Botones/eliminar.png" Width="20px" OnClick="ImgDelIdiomas_Click" />
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
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Experiencia Laboral</h3>
                    <asp:UpdatePanel ID="updExperienciaLaboral" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 1%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 9%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Empresa:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="TxtEmpresa" runat="server" CssClass="form-control upperCase" MaxLength="100" Width="100%" TabIndex="41"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Fecha Inicio:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtFecIniEmpre" runat="server" CssClass="form-control" Width="100%" TabIndex="42"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center;">
                                            <h5>Fecha Fin:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtFecFinEmpre" runat="server" CssClass="form-control" Width="100%" TabIndex="43"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Cargo:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="TxtCargo" runat="server" CssClass="form-control upperCase" MaxLength="50" Width="100%" TabIndex="44"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Descripción:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="TxtDescripcion" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="form-control upperCase" MaxLength="150" Width="100%" TextMode="MultiLine" Height="50px" TabIndex="45"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Motivo Salida:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DdlMotivo" runat="server" CssClass="form-control" Width="100%" TabIndex="46">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:ImageButton ID="ImgAgregarExperiencia" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAgregarExperiencia_Click" TabIndex="47" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:Panel ID="Panel5" runat="server" Height="30px">
                                            </asp:Panel>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="4">
                                            <asp:Panel ID="pnlExperiencia" runat="server" ScrollBars="Vertical" Height="230px">
                                                <asp:GridView ID="GrdvExperiencia" runat="server" AutoGenerateColumns="False"
                                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                    DataKeyNames="Codigo" ForeColor="#333333" PageSize="5" Width="100%" TabIndex="48">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Empresa" HeaderText="Empresa" />
                                                        <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                                                        <asp:BoundField DataField="Motivo" HeaderText="Motivo Salida" />
                                                        <asp:BoundField DataField="FecInicio" HeaderText="Fecha Inicio" />
                                                        <asp:BoundField DataField="FecFin" HeaderText="Fecha Fin" />
                                                        <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgDelExpeLaboral" runat="server" Height="20px" ImageUrl="~/Botones/eliminar.png" Width="15px" OnClick="ImgDelExpeLaboral_Click" />
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
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Referencias Laborales</h3>
                    <asp:UpdatePanel ID="updReferenciaLaboral" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 1%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 9%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Empresa:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DdlEmpresa" runat="server" CssClass="form-control" Width="100%" TabIndex="49">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Referencia:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtNombreRefe" runat="server" CssClass="form-control upperCase" MaxLength="100" Width="100%" TabIndex="50"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center;">
                                            <h5>Cargo:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtCargoRefe" runat="server" CssClass="form-control upperCase" MaxLength="50" Width="100%" TabIndex="51"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Teléfono:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtTelefonoRefe" runat="server" CssClass="form-control upperCase" MaxLength="10" Width="100%" TabIndex="52"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtTelefonoRefe_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtTelefonoRefe">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td style="text-align: center;">
                                            <h5>Celular:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtCelularRefe" runat="server" CssClass="form-control upperCase" MaxLength="10" Width="100%" TabIndex="53"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtCelularRefe_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtCelularRefe">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Email:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="TxtEmailRefe" runat="server" CssClass="form-control lowCase" MaxLength="80" Width="100%" TabIndex="54"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:ImageButton ID="ImgAgregarRefLaboral" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAgregarRefLaboral_Click" TabIndex="55" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:Panel ID="Panel6" runat="server" Height="30px">
                                            </asp:Panel>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="4">
                                            <asp:Panel ID="Panel7" runat="server" ScrollBars="Vertical" Height="230px">
                                                <asp:GridView ID="GrdvRefLaboral" runat="server" AutoGenerateColumns="False"
                                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                    DataKeyNames="Codigo" ForeColor="#333333" PageSize="5" Width="100%" TabIndex="56">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Empresa" HeaderText="Empresa" />
                                                        <asp:BoundField DataField="Nombre" HeaderText="Referencia" />
                                                        <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                                                        <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
                                                        <asp:BoundField DataField="Celular" HeaderText="Celular" />
                                                        <asp:BoundField DataField="Email" HeaderText="Email" />
                                                        <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgDelRefLaboral" runat="server" Height="20px" ImageUrl="~/Botones/eliminar.png" Width="15px" OnClick="ImgDelRefLaboral_Click" />
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
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="3"></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Referencias Personales</h3>
                    <asp:UpdatePanel ID="updReferenciaPersonal" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 1%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 9%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Referencia:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtRefePersonal" runat="server" CssClass="form-control upperCase" MaxLength="100" Width="100%" TabIndex="57"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center;">
                                            <h5>Parentesco:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlParentesco" runat="server" CssClass="form-control" Width="100%" TabIndex="58">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Teléfono:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtTelefonoRefPersonal" runat="server" CssClass="form-control upperCase" MaxLength="10" Width="100%" TabIndex="59"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtTelefonoRefPersonal_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtTelefonoRefPersonal">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td style="text-align: center;">
                                            <h5>Celular:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtCelularRefPersonal" runat="server" CssClass="form-control upperCase" MaxLength="10" Width="100%" TabIndex="60"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtCelularRefPersonal_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtCelularRefPersonal">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:ImageButton ID="ImgAgregarRefPersonal" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAgregarRefPersonal_Click" TabIndex="61" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:Panel ID="Panel8" runat="server" Height="30px">
                                            </asp:Panel>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="4">
                                            <asp:Panel ID="Panel9" runat="server" ScrollBars="Vertical" Height="230px">
                                                <asp:GridView ID="GrdvRefPersonal" runat="server" AutoGenerateColumns="False"
                                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                    DataKeyNames="Codigo,CodigoParen" ForeColor="#333333" PageSize="5" Width="100%" TabIndex="62">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Nombre" HeaderText="Referencia" />
                                                        <asp:BoundField DataField="Parentesco" HeaderText="Parentesco" />
                                                        <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
                                                        <asp:BoundField DataField="Celular" HeaderText="Celular" />
                                                        <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgDelRefPersonal" runat="server" Height="15px" ImageUrl="~/Botones/eliminar.png" Width="20px" OnClick="ImgDelRefPersonal_Click" />
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
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="3"></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="updOpciones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 45%; text-align: right;">
                                    <asp:Button ID="BtnGrabar" runat="server" CssClass="button" OnClick="BtnGrabar_Click" Text="Grabar" Width="120px" TabIndex="63" />
                                </td>
                                <td style="width: 5%"></td>
                                <td style="width: 45%; text-align: left;">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="64" />
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

