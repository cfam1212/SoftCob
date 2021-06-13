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
        <div class="panel panel-primary">
            <div class="panel-heading" style="background-color: #79BBB8;">
                <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
            </div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
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
                                    <asp:TextBox ID="TxtNumeroDocumento" runat="server" CssClass="form-control upperCase" MaxLength="20" TabIndex="2" Width="100%" OnTextChanged="TxtNumeroDocumento_TextChanged" AutoPostBack="True"></asp:TextBox>
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
                                    <h5>Fec. Nacimiento:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtFechaNacimiento" runat="server" Width="100%" CssClass="form-control form-control" TabIndex="5"></asp:TextBox>
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
                                    <h5>Provincia:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlProvincia" runat="server" AutoPostBack="True" CssClass="form-control chzn-select" OnSelectedIndexChanged="DdlProvincia_SelectedIndexChanged" TabIndex="8" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>Ciudad:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlCiudad" runat="server" CssClass="form-control chzn-select" TabIndex="9" Width="100%">
                                    </asp:DropDownList>
                                </td>
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
                                        <asp:DropDownList ID="DdlDirTitular" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="12" Width="100%">
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
                                        <asp:TextBox ID="TxtDirTitular" runat="server" CssClass="form-control upperCase" Height="50px" MaxLength="250" onkeydown="return (event.keyCode!=13);" TabIndex="13" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                    </td>
                                    <td>
                                        <h5>Referencia:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtRefTitular" runat="server" CssClass="form-control upperCase" Height="50px" MaxLength="250" onkeydown="return (event.keyCode!=13);" TabIndex="14" TextMode="MultiLine" Width="100%"></asp:TextBox>
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
                                        <asp:ImageButton ID="ImgAddDirTitular" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAddDirTitular_Click" TabIndex="15" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ImgModDirTitular" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModDirTitular_Click" TabIndex="16" Enabled="False" />
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
                                                ForeColor="#333333" PageSize="7" TabIndex="17" Width="100%">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo"></asp:BoundField>
                                                    <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                                                    <asp:BoundField DataField="Referencia" HeaderText="Referencia"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Selecc">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgSelecDirTitular" runat="server" Height="20px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSelecDirTitular_Click" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Eliminar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgEliDirTitular" runat="server" Height="20px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgEliDirTitular_Click" OnClientClick="return asegurar();" />
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
                                        <asp:DropDownList ID="DdlMailTitular" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="18" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <h5>E-mail:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtMailTitular" runat="server" CssClass="form-control lowCase" MaxLength="100" TabIndex="19" Width="100%"></asp:TextBox>
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
                                        <asp:ImageButton ID="ImgAddMailTitular" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAddMailTitular_Click" TabIndex="20" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ImgModMailTitular" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModMailTitular_Click" TabIndex="21" Enabled="False" />
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
                                                ForeColor="#333333" PageSize="7" TabIndex="22" Width="100%">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo"></asp:BoundField>
                                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                                    <asp:TemplateField HeaderText="Selecc">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgSelecMailTitular" runat="server" Height="20px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSelecMailTitular_Click" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Eliminar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgEliMailTitular" runat="server" Height="20px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgEliMailTitular_Click" OnClientClick="return asegurar();" />
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
                                    <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="BtnGrabar_Click" TabIndex="23" />
                                </td>
                                <td style="width: 10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="24" />
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
