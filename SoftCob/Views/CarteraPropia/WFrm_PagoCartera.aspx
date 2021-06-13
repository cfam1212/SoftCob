<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_PagoCartera.aspx.cs" Inherits="SoftCob.Views.CarteraPropia.WFrm_PagoCartera" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Pagos Cartera</title>
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../../Scripts/external/jquery/jquery.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>

    <script type="text/javascript">
        function pageLoad(sender, arg) {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#TxtFechaPago').datepicker(
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
        function ValidarDecimales() {
            var numero = document.getElementById("<%=TxtValor.ClientID%>").value;
            if (!/^([0-9])*[.]?[0-9]*$/.test(numero)) {
                alert("El valor " + numero + " no es un número válido. Ejemplo 157.68");
                document.getElementById("<%=TxtValor.ClientID%>").value = "";
                return false;
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
            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updPagos">
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
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Buscar Datos</h3>
                <asp:UpdatePanel ID="updCabecera" runat="server">
                    <ContentTemplate>
                        <div class="table-responsive">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 15%"></td>
                                    <td style="width: 35%"></td>
                                    <td style="width: 15%"></td>
                                    <td style="width: 35%"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <h5>Cedente:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlCedente" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlCedente_SelectedIndexChanged" Width="100%" TabIndex="1">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <h5>Catágolo/Producto:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlCatalogo" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="2" Width="100%" OnSelectedIndexChanged="DdlCatalogo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <h5>Buscar Por:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlBuscarPor" runat="server" CssClass="form-control" Width="100%" TabIndex="4">
                                            <asp:ListItem Value="I">Identificación</asp:ListItem>
                                            <asp:ListItem Value="O">Operación</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <h5>No. Documento:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtBuscarPor" runat="server" CssClass="form-control" MaxLength="20" TabIndex="5" Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="text-align: center">
                                        <asp:Button ID="BtnBuscar" runat="server" CausesValidation="False" CssClass="button" Text="Buscar" Width="120px" OnClick="BtnBuscar_Click" TabIndex="6" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Datos Cliente</h3>
                <asp:UpdatePanel ID="updDatos" runat="server">
                    <ContentTemplate>
                        <div class="table-responsive">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 15%">
                                        <h5>Cliente:</h5>
                                    </td>
                                    <td style="width: 35%">
                                        <h5 runat="server" id="lblCliente" style="font-size: 14px; font-weight: bold"></h5>
                                    </td>
                                    <td style="width: 15%">
                                        <h5>Identificación:</h5>
                                    </td>
                                    <td style="width: 35%">
                                        <h5 runat="server" id="lblIdentificacion" style="font-size: 14px; font-weight: bold"></h5>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <h5>Estado Cliente:</h5>
                                    </td>
                                    <td>
                                        <h5 runat="server" id="lblEstado" style="font-size: 14px; font-weight: bold"></h5>
                                    </td>
                                    <td>
                                        <h5>Estado Operación:</h5>
                                    </td>
                                    <td>
                                        <h5 runat="server" id="lblEstadoOperacion" style="font-size: 14px; font-weight: bold"></h5>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Panel ID="Panel1" runat="server" Height="180px" ScrollBars="Vertical">
                                            <asp:GridView ID="GrdvDeudas" runat="server" Width="100%" AutoGenerateColumns="False"
                                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" DataKeyNames="Cedula,CodigoCPCE,SumSaldo,CodigoGEST,DiasMora" PageSize="5" TabIndex="7">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField HeaderText="Operacion" DataField="Operacion" />
                                                    <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                                    <asp:BoundField HeaderText="Deuda" DataField="Deuda">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Saldo" DataField="Saldo">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Gestor" HeaderText="Gestor" />
                                                    <asp:TemplateField HeaderText="Pagar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgSelecc" runat="server" Height="20px" ImageUrl="~/Botones/abonarbg.png" Width="22px" OnClick="ImgSelecc_Click" />
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
                                <tr>
                                    <td colspan="4">
                                        <asp:Panel ID="pnlDiv0" runat="server" Height="40px"></asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 21%"></td>
                                    <td style="width: 19%">
                                        <h5><strong>Total:</strong></h5>
                                    </td>
                                    <td style="text-align: center; width: 16%">
                                        <h5 runat="server" id="lblTotalDeuda"></h5>
                                    </td>
                                    <td style="width: 16%; text-align: center">
                                        <h5 runat="server" id="lblTotalSaldo"></h5>
                                    </td>
                                    <td style="width: 16%"></td>
                                    <td style="width: 12%"></td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Gestión Pagos</h3>
                <asp:UpdatePanel ID="updPagos" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlPagos" runat="server" Visible="false">
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 15%">
                                            <h5>Fecha Pago:</h5>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:TextBox ID="TxtFechaPago" CssClass="form-control" runat="server" Width="100%" TabIndex="8"></asp:TextBox>
                                        </td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 5%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Valor:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtValor" runat="server" CssClass="form-control alinearDerecha" MaxLength="10" Width="100%" TabIndex="9"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center">
                                            <h5>No. Documento:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtDocumento" runat="server" CssClass="form-control" MaxLength="50" Width="100%" TabIndex="10"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Tipo:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlTipoPago" runat="server" CssClass="form-control" Width="100%" TabIndex="11">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ImgAgregar" runat="server" Enabled="False" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAgregar_Click" TabIndex="12" Height="25px" />
                                        </td>
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
                                    <tr>
                                        <td></td>
                                        <td colspan="4">
                                            <asp:Panel ID="pnlDatos" runat="server" Height="180px" ScrollBars="Vertical">
                                                <asp:GridView ID="GrdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                    ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" DataKeyNames="Operacion,Codigo,SumPago" TabIndex="13">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Fecha Registro" DataField="FechaRegistro" />
                                                        <asp:BoundField HeaderText="Fecha Pago" DataField="FechaPago" />
                                                        <asp:BoundField HeaderText="$ Pago" DataField="Pago">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Documento" HeaderText="No. Documento" />
                                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                                        <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                                                        <asp:TemplateField HeaderText="Reversar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgReversar" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgReversar_Click" />
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
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 18%"></td>
                                        <td style="width: 14%">
                                            <h5><strong>Total:</strong></h5>
                                        </td>
                                        <td style="width: 10%; text-align: center;">
                                            <h5 runat="server" id="lblTotalPago"></h5>
                                        </td>
                                        <td style="width: 48%"></td>
                                        <td style="width: 5%"></td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="updBotones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: center; width: 100%">
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
