<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_NewDeudor.aspx.cs" Inherits="SoftCob.Views.Cedente.WFrm_NewDeudor" %>

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
    <link href="../../JS/css/alertify.min.css" rel="stylesheet" />

    <script src="../../Scripts/external/jquery/jquery.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <script src="../../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../../Scripts/jquery-ui.min.js"></script>
    <script src="../../JS/alertify.min.js"></script>
    <link rel="stylesheet" href="../../Style/chosen.css" />

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

        function ValidarDecimales() {
            var numero = document.getElementById("<%=TxtTotalDeuda.ClientID%>").value;
            if (!/^([0-9])*[.]?[0-9]*$/.test(numero)) {
                alert("El valor " + numero + " no es un número válido. Ejemplo 157.68");
                document.getElementById("<%=TxtTotalDeuda.ClientID%>").value = "";
                return false;
            }
        }

        function ValidarDecimales1() {
            var numero = document.getElementById("<%=TxtExigible.ClientID%>").value;
            if (!/^([0-9])*[.]?[0-9]*$/.test(numero)) {
                alert("El valor " + numero + " no es un número válido. Ejemplo 157.68");
                document.getElementById("<%=TxtExigible.ClientID%>").value = "";
                return false;
            }
        }

        function asegurar() {
            rc = confirm("¿Seguro que desea Cambiar Datos?");
            return rc;
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
    <script>
        $(function () {
            $("#acordionParametro").accordion();
        });
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

            <div class="panel-info">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdBuscar">
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
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Datos - TITULAR</h3>
                <asp:UpdatePanel ID="updCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 35%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 35%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Tipo:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlTipoDocumento" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlTipoDocumento_SelectedIndexChanged" TabIndex="1" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>Documento:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtNumeroDocumento" runat="server" CssClass="form-control alinearDerecha" MaxLength="20" TabIndex="2" Width="100%" AutoPostBack="True"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="TxtNumeroDocumento_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="TxtNumeroDocumento">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td style="text-align: center">
                                    <asp:UpdatePanel runat="server" ID="UpdBuscar">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="ImgBuscar" runat="server" ImageUrl="~/Botones/buscarbg.png" OnClick="ImgBuscar_Click" TabIndex="600" Height="30px" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Nombres:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtNombres" runat="server" Width="100%" CssClass="form-control upperCase" MaxLength="80" TabIndex="3"></asp:TextBox>
                                </td>
                                <td>
                                    <h5>Apellidos:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtApellidos" runat="server" Width="100%" CssClass="form-control upperCase" MaxLength="80" TabIndex="4"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Fec.Nacim:</h5>
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
                                    <h5>Provincia:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlProvincia" runat="server" Width="100%" CssClass="form-control chzn-select" TabIndex="7" OnSelectedIndexChanged="DdlProvincia_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>Ciudad:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlCiudad" runat="server" Width="100%" CssClass="form-control chzn-select" TabIndex="8">
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
                                    <asp:DropDownList ID="DdlEstCivil" runat="server" CssClass="form-control" TabIndex="9" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5 runat="server" id="LblEstado">Estado:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkEstado" runat="server" AutoPostBack="True" Checked="True" CssClass="form-control" OnCheckedChanged="ChkEstado_CheckedChanged" TabIndex="10" Text="Activo" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Panel runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                            <tr runat="server" id="TrActualizar" visible="false">
                                <td></td>
                                <td></td>
                                <td colspan="2">
                                    <asp:LinkButton ID="LnkActualizar" runat="server" Font-Bold="True" Font-Size="12pt" OnClick="LnkActualizar_Click" OnClientClick="return asegurar();">Actualizar Datos</asp:LinkButton>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel-body">
                <div id="acordionParametro">
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Dirección - TITULAR</h3>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 5%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 35%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 35%"></td>
                                    <td style="width: 5%"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Tipo:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlDirTitular" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="11" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Dirección:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtDirTitular" runat="server" CssClass="form-control upperCase" Height="50px" MaxLength="250" onkeydown="return (event.keyCode!=13);" TabIndex="12" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                    </td>
                                    <td>
                                        <h5>Referencia:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtRefTitular" runat="server" CssClass="form-control upperCase" Height="50px" MaxLength="250" onkeydown="return (event.keyCode!=13);" TabIndex="13" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Panel ID="Panel10" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td style="text-align: center">
                                        <asp:ImageButton ID="ImgAddDirTitular" runat="server" Height="25px" ImageUrl="~/Botones/agregarbg.png" OnClick="ImgAddDirTitular_Click" TabIndex="14" Enabled="False" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ImgModDirTitular" runat="server" Height="25px" ImageUrl="~/Botones/modificarbg.png" OnClick="ImgModDirTitular_Click" TabIndex="15" Enabled="False" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Panel ID="Panel11" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr runat="server" id="Tr1">
                                    <td colspan="6">
                                        <asp:Panel ID="Panel12" runat="server" Height="180px" ScrollBars="Vertical">
                                            <asp:GridView ID="GrdvDirecTitular" runat="server" AutoGenerateColumns="False"
                                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                DataKeyNames="CodigoDIGT,Nuevo"
                                                ForeColor="#333333" PageSize="7" TabIndex="16" Width="100%">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo"></asp:BoundField>
                                                    <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                                                    <asp:BoundField DataField="Referencia" HeaderText="Referencia"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Selecc">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgSelecDirTitular" runat="server" Height="20px" ImageUrl="~/Botones/seleccbg.png" OnClick="ImgSelecDirTitular_Click" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Eliminar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgEliDirTitular" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgEliDirTitular_Click" OnClientClick="return asegurar();" />
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
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">E-Mail - TITULAR</h3>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 5%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 35%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 35%"></td>
                                    <td style="width: 5%"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Tipo:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlMailTitular" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="17" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <h5>E-mail:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtMailTitular" runat="server" CssClass="form-control lowCase" MaxLength="100" TabIndex="18" Width="100%"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Panel ID="Panel13" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td style="text-align: center">
                                        <asp:ImageButton ID="ImgAddMailTitular" runat="server" Height="25px" ImageUrl="~/Botones/agregarbg.png" OnClick="ImgAddMailTitular_Click" TabIndex="19" Enabled="False" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ImgModMailTitular" runat="server" Height="25px" ImageUrl="~/Botones/modificarbg.png" OnClick="ImgModMailTitular_Click" TabIndex="20" Enabled="False" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Panel ID="Panel14" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr runat="server" id="Tr2">
                                    <td colspan="6">
                                        <asp:Panel ID="Panel15" runat="server" Height="180px" ScrollBars="Vertical">
                                            <asp:GridView ID="GrdvMailTitular" runat="server" AutoGenerateColumns="False"
                                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                DataKeyNames="CodigoDIGT,Nuevo"
                                                ForeColor="#333333" PageSize="7" TabIndex="21" Width="100%">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo"></asp:BoundField>
                                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                                    <asp:TemplateField HeaderText="Selecc">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgSelecMailTitular" runat="server" Height="20px" ImageUrl="~/Botones/seleccbg.png" OnClick="ImgSelecMailTitular_Click" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Eliminar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgEliMailTitular" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgEliMailTitular_Click" OnClientClick="return asegurar();" />
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
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Operaciones</h3>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 5%"></td>
                                    <td style="width: 15%"></td>
                                    <td style="width: 30%"></td>
                                    <td style="width: 15%"></td>
                                    <td style="width: 30%"></td>
                                    <td style="width: 5%"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Cedente:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlCedente" runat="server" Width="100%" CssClass="form-control" TabIndex="22" OnSelectedIndexChanged="DdlCedente_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <h5>Producto/Catalogo:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlProducto" runat="server" CssClass="form-control" TabIndex="23" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Operación:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtOperacion" runat="server" Width="100%" CssClass="form-control upperCase" MaxLength="20" TabIndex="24"></asp:TextBox>
                                    </td>
                                    <td>
                                        <h5>Tipo Operación</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlTipoOperacion" runat="server" Width="100%" CssClass="form-control" TabIndex="25">
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Días Mora:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtDiasMora" runat="server" Width="100%" CssClass="form-control alinearDerecha" MaxLength="4" TabIndex="26">0</asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="TxtDiasMora_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="TxtDiasMora">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <h5>Total Deuda:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtTotalDeuda" runat="server" Width="100%" CssClass="form-control alinearDerecha" MaxLength="10" TabIndex="27">0.00</asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Exigible:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtExigible" runat="server" Width="100%" CssClass="form-control alinearDerecha" MaxLength="10" TabIndex="28">0.00</asp:TextBox>
                                    </td>
                                    <td>
                                        <h5>Asignar A:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlGestor" runat="server" Width="100%" CssClass="form-control" TabIndex="29">
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Panel ID="Panel1" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td style="text-align: center">
                                        <asp:ImageButton ID="ImgAddOperacion" runat="server" Height="25px" ImageUrl="~/Botones/agregarbg.png" OnClick="ImgAddOperacion_Click" TabIndex="30" Enabled="False" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ImgModOperacion" runat="server" Height="25px" ImageUrl="~/Botones/modificarbg.png" OnClick="ImgModOperacion_Click" TabIndex="31" Enabled="False" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Panel ID="Panel2" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr runat="server" id="TrDatos">
                                    <td colspan="6">
                                        <asp:Panel ID="Panel3" runat="server" Height="180px" ScrollBars="Vertical">
                                            <asp:GridView ID="GrdvDatos" runat="server" AutoGenerateColumns="False"
                                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                DataKeyNames="CodigoCPCE,CodigoCTDE,CodigoCLDE,CodigoCEDE,CodigoPERS,Estado,Operacion,Nuevo"
                                                ForeColor="#333333" PageSize="7" TabIndex="32" Width="100%" OnRowDataBound="GrdvDatos_RowDataBound">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="Producto" HeaderText="Producto"></asp:BoundField>
                                                    <asp:BoundField DataField="Operacion" HeaderText="Operación" />
                                                    <asp:BoundField DataField="DiasMora" HeaderText="Días_Mora">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TotalDeuda" HeaderText="Total_Deuda">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Exigible" HeaderText="Exigible">
                                                        <ItemStyle Wrap="True" HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo">
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Selecc">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgSeleccOperacion" runat="server" Height="20px" ImageUrl="~/Botones/seleccbg.png" OnClick="ImgSeleccOperacion_Click" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Estado">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ChkEstOperacion" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEstOperacion_CheckedChanged" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Eliminar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgEliOperacion" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgEliOperacion_Click" OnClientClick="return asegurar();" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Gestionar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgGestionar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/btncitagrisbg.png" OnClick="ImgGestionar_Click" />
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
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Teléfonos</h3>
                    <asp:UpdatePanel ID="UpdTelefonos" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 5%"></td>
                                    <td style="width: 15%"></td>
                                    <td style="width: 30%"></td>
                                    <td style="width: 15%"></td>
                                    <td style="width: 30%"></td>
                                    <td style="width: 5%"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Tipo Teléfono:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlTipoTelefono" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="33" Width="100%" OnSelectedIndexChanged="DdlTipoTelefono_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <h5>Propietario</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlPropietario" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="34" Width="100%" OnSelectedIndexChanged="DdlPropietario_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Prefijo:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlPrefijo" runat="server" CssClass="form-control" TabIndex="35" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <h5>Número:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtTelefono" runat="server" CssClass="form-control alinearDerecha" MaxLength="10" TabIndex="36" Width="100%"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtTelefono_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtTelefono">
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
                                        <asp:TextBox ID="TxtNomReferencia" runat="server" CssClass="form-control upperCase" Enabled="False" MaxLength="80" TabIndex="37" Width="100%"></asp:TextBox>
                                    </td>
                                    <td>
                                        <h5>Apellidos:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtApeReferencia" runat="server" CssClass="form-control upperCase" Enabled="False" MaxLength="80" TabIndex="38" Width="100%"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>&nbsp;</td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Panel ID="Panel6" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td style="text-align: center">
                                        <asp:ImageButton ID="ImgAddTelefono" runat="server" Height="25px" ImageUrl="~/Botones/agregarbg.png" OnClick="ImgAddTelefono_Click" TabIndex="39" Enabled="False" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Panel ID="Panel5" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr runat="server" id="TrTelefonos">
                                    <td colspan="6">
                                        <asp:Panel ID="Panel4" runat="server" Height="180px" ScrollBars="Vertical">
                                            <asp:GridView ID="GrdvTelefonos" runat="server" AutoGenerateColumns="False"
                                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                ForeColor="#333333" PageSize="7" TabIndex="40" Width="100%" DataKeyNames="CodigoTELE">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="Prefijo" HeaderText="Prefijo"></asp:BoundField>
                                                    <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo"></asp:BoundField>
                                                    <asp:BoundField DataField="Propietario" HeaderText="Propietario"></asp:BoundField>
                                                    <asp:BoundField DataField="NomApe" HeaderText="Nombres">
                                                        <ItemStyle Wrap="True" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Eliminar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgEliTelefono" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgEliTelefono_Click" OnClientClick="return asegurar();" />
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
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Datos - GARANTES/CODEUDOR</h3>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 5%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 35%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 35%"></td>
                                    <td style="width: 5%"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Tipo:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlTipoGarante" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="41" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <h5>Cédula:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtCedulaGarante" runat="server" AutoPostBack="True" CssClass="form-control upperCase" MaxLength="20" TabIndex="42" Width="100%"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Nombres:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtGarante" runat="server" CssClass="form-control upperCase" MaxLength="80" TabIndex="43" Width="100%"></asp:TextBox>
                                    </td>
                                    <td>
                                        <h5>Apellidos:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtApellidoGarante" runat="server" CssClass="form-control upperCase" MaxLength="80" TabIndex="44" Width="100%"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Operación:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtNumOperacion" runat="server" CssClass="form-control upperCase" MaxLength="50" TabIndex="45" Width="100%"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Panel ID="Panel7" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td style="text-align: center">
                                        <asp:ImageButton ID="ImgAddGarante" runat="server" Height="25px" ImageUrl="~/Botones/agregarbg.png" OnClick="ImgAddGarante_Click" TabIndex="46" Enabled="False" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ImgEditGarante" runat="server" Height="25px" ImageUrl="~/Botones/modificarbg.png" OnClick="ImgEditGarante_Click" TabIndex="47" Enabled="False" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Panel ID="Panel9" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr runat="server" id="TrDeudor">
                                    <td colspan="6">
                                        <asp:Panel ID="Panel8" runat="server" Height="180px" ScrollBars="Vertical">
                                            <asp:GridView ID="GrdvGarante" runat="server" AutoGenerateColumns="False"
                                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                DataKeyNames="CodigoGART,Tipo"
                                                ForeColor="#333333" PageSize="7" TabIndex="48" Width="100%">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo"></asp:BoundField>
                                                    <asp:BoundField DataField="Cedula" HeaderText="Cédula" />
                                                    <asp:BoundField DataField="Garante" HeaderText="Nombres"></asp:BoundField>
                                                    <asp:BoundField DataField="Operacion" HeaderText="Operación"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Selecc">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgSeleccGarante" runat="server" Height="20px" ImageUrl="~/Botones/seleccbg.png" OnClick="ImgSeleccGarante_Click" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Eliminar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgEliGarante" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgEliGarante_Click" OnClientClick="return asegurar();" />
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
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Dirección - GARANTE/CODEUDOR</h3>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 5%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 35%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 35%"></td>
                                    <td style="width: 5%"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Tipo:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlDirGarante" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="49" Width="100%">
                                            <asp:ListItem Value="0">--Seleccione Tipo--</asp:ListItem>
                                            <asp:ListItem Value="DOMICILIO">DOMICILIO</asp:ListItem>
                                            <asp:ListItem Value="TRABAJO">TRABAJO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Dircción:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtDirGarante" runat="server" CssClass="form-control upperCase" Height="50px" MaxLength="150" onkeydown="return (event.keyCode!=13);" TabIndex="50" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                    </td>
                                    <td>
                                        <h5>Referencia:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtRefGarante" runat="server" CssClass="form-control upperCase" Height="50px" MaxLength="150" onkeydown="return (event.keyCode!=13);" TabIndex="51" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Panel ID="Panel16" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td style="text-align: center">
                                        <asp:ImageButton ID="ImgAddDirGarante" runat="server" Height="25px" ImageUrl="~/Botones/agregarbg.png" OnClick="ImgAddDirGarante_Click" TabIndex="52" Enabled="False" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ImgModDirGarante" runat="server" Height="25px" ImageUrl="~/Botones/modificarbg.png" OnClick="ImgModDirGarante_Click" TabIndex="53" Enabled="False" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Panel ID="Panel17" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr runat="server" id="Tr3">
                                    <td colspan="6">
                                        <asp:Panel ID="Panel18" runat="server" Height="180px" ScrollBars="Vertical">
                                            <asp:GridView ID="GrdvDirecGarante" runat="server" AutoGenerateColumns="False"
                                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                DataKeyNames="CodigoDIGT,Nuevo,Definicion"
                                                ForeColor="#333333" PageSize="7" TabIndex="54" Width="100%">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo"></asp:BoundField>
                                                    <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                                                    <asp:BoundField DataField="Referencia" HeaderText="Referencia"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Selecc">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgSelecDirGarante" runat="server" Height="20px" ImageUrl="~/Botones/seleccbg.png" OnClick="ImgSelecDirGarante_Click" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Eliminar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgEliDirGarante" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgEliDirGarante_Click" OnClientClick="return asegurar();" />
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
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">E-Mail - GARANTE/CODEUDOR</h3>
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 5%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 35%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 35%"></td>
                                    <td style="width: 5%"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Tipo:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlMailGarante" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="55" Width="100%">
                                            <asp:ListItem Value="0">--Seleccione Tipo--</asp:ListItem>
                                            <asp:ListItem Value="1">DOMICILIO</asp:ListItem>
                                            <asp:ListItem Value="2">TRABAJO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <h5>E-mail:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtMailGarante" runat="server" CssClass="form-control lowCase" MaxLength="100" TabIndex="56" Width="100%"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Panel ID="Panel19" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td style="text-align: center">
                                        <asp:ImageButton ID="ImgAddMailGarante" runat="server" Height="25px" ImageUrl="~/Botones/agregarbg.png" OnClick="ImgAddMailGarante_Click" TabIndex="57" Enabled="False" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ImgModMailGarante" runat="server" Height="25px" ImageUrl="~/Botones/modificarbg.png" OnClick="ImgModMailGarante_Click" TabIndex="58" Enabled="False" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Panel ID="Panel20" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr runat="server" id="Tr4">
                                    <td colspan="6">
                                        <asp:Panel ID="Panel21" runat="server" Height="180px" ScrollBars="Vertical">
                                            <asp:GridView ID="GrdvMailGarante" runat="server" AutoGenerateColumns="False"
                                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                DataKeyNames="CodigoDIGT,Nuevo"
                                                ForeColor="#333333" PageSize="7" TabIndex="59" Width="100%">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo"></asp:BoundField>
                                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                                    <asp:TemplateField HeaderText="Selecc">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgSelecMailGarante" runat="server" Height="20px" ImageUrl="~/Botones/seleccbg.png" OnClick="ImgSelecMailGarante_Click" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Eliminar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgEliMailGarante" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgEliMailGarante_Click" OnClientClick="return asegurar();" />
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
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="UpdBotones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right; width: 45%">
                                    <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="BtnGrabar_Click" TabIndex="60" Visible="False" />
                                </td>
                                <td style="width: 10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="61" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <script src="../../Scripts/chosen.jquery.js" type="text/javascript"></script>
        <script type="text/javascript"> $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); </script>
        <script>
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
            function endRequestHandler() {
                $(".chzn-select").chosen({ width: "100%" });
                $(".chzn-container").css({ "width": "100%" });
                $(".chzn-drop").css({ "width": "95%" });
            }
        </script>
    </form>
</body>
</html>
