<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_SectorVisitaCitacion.aspx.cs" Inherits="SoftCob.Views.BPM.WFrm_SectorVisitaCitacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nuevos Campos Estrategia</title>
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
                $('#TxtFecha').datepicker(
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
            <div class="panel-heading">
                <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
            </div>
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
<%--            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updBotones">
                    <ProgressTemplate>
                        <div class="overlay" />
                        <div class="overlayContent">
                            <h2>Procesando..</h2>
                            <img src="../../Images/load.gif" alt="Loading" border="1" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>--%>
            <div class="panel-body">
                <asp:UpdatePanel ID="updCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 40%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="3">
                                    <asp:Panel ID="PnlDatosDeudor" runat="server" CssClass="panel panel-primary" Height="200px"
                                        GroupingText="Datos Deudor" TabIndex="15">
                                        <asp:GridView ID="GrdvDatosDeudor" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333"
                                            PageSize="3" TabIndex="1" Width="100%">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="Cedula" HeaderText="Cédula" />
                                                <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                                                <asp:BoundField DataField="Provincia" HeaderText="Provincia" />
                                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                                <asp:BoundField DataField="Edad" HeaderText="Edad" />
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
                                <td colspan="5">
                                    <asp:Panel ID="pnlDivision" runat="server" Height="10px"></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="3">
                                    <asp:Panel ID="PnlDatosGetion" runat="server" CssClass="panel panel-primary" Height="250px"
                                        GroupingText="Datos Operación" ScrollBars="Vertical" TabIndex="17">
                                        <asp:GridView ID="GrdvDatosObligacion" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333"
                                            PageSize="5" TabIndex="2" Width="100%" DataKeyNames="Operacion,DiasMora" OnRowDataBound="GrdvDatosObligacion_RowDataBound" ShowFooter="True">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                                <asp:BoundField DataField="Operacion" HeaderText="Operación"></asp:BoundField>
                                                <asp:BoundField DataField="HDiasMora" HeaderText="H.Mora">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ValorDeuda" HeaderText="Valor Deuda">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Capital Vencido" DataField="CVencido">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Exigible" HeaderText="Exigible">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
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
                                <td colspan="3">
                                    <asp:Panel ID="Panel2" runat="server" Height="10px"></asp:Panel>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 5%"></td>
                                <td style="width: 65%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>&nbsp;</td>
                                <td></td>
                                <td>&nbsp;</td>
                                <td></td>
                            </tr>
                        </table>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 25%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 25%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="4">
                                    <asp:Panel ID="Panel7" runat="server" CssClass="panel panel-primary"
                                        GroupingText="Direcciones Terreno Citación" Height="350px" ScrollBars="Vertical" TabIndex="17">
                                        <asp:GridView ID="GrdvTerreno" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="CodigoTERR,Sector,CodigoMATD,CodigoSECT" ForeColor="#333333" Height="25px" PageSize="5"
                                            TabIndex="7" Width="100%" OnRowDataBound="GrdvTerreno_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="TipoCliente" HeaderText="Tipo" />
                                                <asp:BoundField DataField="Definicion" HeaderText="Definición"></asp:BoundField>
                                                <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                                                <asp:BoundField DataField="Referencia" HeaderText="Referencia" />
                                                <asp:TemplateField HeaderText="Sector">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="DdlSector" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Visitar">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkVisitar" runat="server" AutoPostBack="True" OnCheckedChanged="ChkVisitar_CheckedChanged" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fecha Visita">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtFechaVisita" runat="server" CssClass="form-control alinearDerecha" MaxLength="10"
                                                            Enabled="False"></asp:TextBox>
                                                        <asp:CalendarExtender ID="TxtFechaVisita_CalendarExtender" runat="server" Enabled="True" Format="yyyy-MM-dd" TargetControlID="TxtFechaVisita">
                                                        </asp:CalendarExtender>
                                                    </ItemTemplate>
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
                                <td colspan="6">
                                    <asp:Panel ID="Panel5" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="4"></td>
                                <td></td>
                            </tr>
                            <tr runat="server" id="TrEmail4" visible="false">
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr runat="server" id="TrTerreno5" visible="false">
                                <td colspan="6">
                                    <asp:Panel ID="Panel6" runat="server" Height="20px"></asp:Panel>
                                </td>
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
                                    <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" TabIndex="8" OnClick="BtnGrabar_Click" />
                                </td>
                                <td style="width: 5%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="9" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        function asegurar() {
            rc = confirm("¿Seguro que desea Eliminar?");
            return rc;
        }
    </script>
</body>
</html>
