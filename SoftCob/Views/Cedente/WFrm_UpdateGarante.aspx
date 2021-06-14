<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_UpdateGarante.aspx.cs" Inherits="SoftCob.Views.Cedente.WFrm_UpdateGarante" %>

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
        function Validar_Cedula() {
            var cedula = document.getElementById("<%=TxtNumeroDocumento.ClientID%>").value;
            var digito_region = cedula.substring(0, 2);
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
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">DATOS GARANTE/CODEUDOR</h3>
                    <asp:UpdatePanel ID="updCabecera" runat="server">
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
                                        <h5>No. Documento:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtNumeroDocumento" runat="server" AutoPostBack="True" CssClass="form-control upperCase" MaxLength="10" TabIndex="1" Width="100%"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="TxtNumeroDocumento_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="TxtNumeroDocumento">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <h5>Tipo:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlTipo" runat="server" CssClass="form-control" TabIndex="2" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Nombres:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtNombres" runat="server" CssClass="form-control upperCase" MaxLength="80" TabIndex="3" Width="100%" style="margin-bottom: 0"></asp:TextBox>
                                    </td>
                                    <td>
                                        <h5>Apellidos:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtApellidos" runat="server" CssClass="form-control upperCase" MaxLength="80" TabIndex="3" Width="100%"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Operación:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtOperacion" runat="server" CssClass="form-control upperCase" MaxLength="50" TabIndex="4" Width="100%"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="panel-body">
                <div id="acordionParametro">
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
                                        <asp:DropDownList ID="DdlDirGarante" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="5" Width="100%">
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
                                        <asp:TextBox ID="TxtDirGarante" runat="server" CssClass="form-control upperCase" Height="50px" MaxLength="150" onkeydown="return (event.keyCode!=13);" TabIndex="6" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                    </td>
                                    <td>
                                        <h5>Referencia:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtRefGarante" runat="server" CssClass="form-control upperCase" Height="50px" MaxLength="150" onkeydown="return (event.keyCode!=13);" TabIndex="7" TextMode="MultiLine" Width="100%"></asp:TextBox>
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
                                        <asp:ImageButton ID="ImgAddDirGarante" runat="server" Height="25px" ImageUrl="~/Botones/agregar.png" OnClick="ImgAddDirGarante_Click" TabIndex="8" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ImgModDirGarante" runat="server" Height="25px" ImageUrl="~/Botones/modificarnew.png" OnClick="ImgModDirGarante_Click" TabIndex="9" Enabled="False" />
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
                                                DataKeyNames="CodigoDIGT,Nuevo"
                                                ForeColor="#333333" PageSize="7" TabIndex="10" Width="100%">
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
                                        <asp:DropDownList ID="DdlMailGarante" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="11" Width="100%">
                                            <asp:ListItem Value="0">--Seleccione Tipo--</asp:ListItem>
                                            <asp:ListItem Value="1">DOMICILIO</asp:ListItem>
                                            <asp:ListItem Value="2">TRABAJO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <h5>E-mail:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtMailGarante" runat="server" CssClass="form-control lowCase" MaxLength="100" TabIndex="12" Width="100%"></asp:TextBox>
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
                                        <asp:ImageButton ID="ImgAddMailGarante" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAddMailGarante_Click" TabIndex="13" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ImgModMailGarante" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModMailGarante_Click" TabIndex="14" Enabled="False" />
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
                                                ForeColor="#333333" PageSize="7" TabIndex="15" Width="100%">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo"></asp:BoundField>
                                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                                    <asp:TemplateField HeaderText="Selecc">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgSelecMailGarante" runat="server" Height="20px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSelecMailGarante_Click" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Eliminar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgEliMailGarante" runat="server" Height="20px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgEliMailGarante_Click" OnClientClick="return asegurar();" />
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
                                    <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="BtnGrabar_Click" TabIndex="16" />
                                </td>
                                <td style="width: 10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="17" />
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
