<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_TablaPagosConvenio.aspx.cs" Inherits="SoftCob.Views.BPM.WFrm_TablaPagosConvenio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>

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
        .auto-style1 {
            height: 35px;
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

            <div class="panel-body">
                <asp:UpdatePanel ID="updCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%" runat="server" id="TblHistorial">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 5%"></td>
                                <td style="width: 65%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="3"></td>
                                <td></td>
                            </tr>
                        </table>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 10%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 10%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    
                                </td>
                                <td><h5>Fecha Acuerdo:</h5></td>
                                <td style="text-align: right">
                                    
                                    <asp:Label ID="LblFechaAcuerdo" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Blue"></asp:Label>
                                    
                                </td>
                                <td style="text-align: right"></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>
                                    <h5>No. Documento:</h5>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="LblNumDocu" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="#CC6600"></asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>
                                    <h5>Nombres:</h5>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="LblNombres" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="#CC6600"></asp:Label>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="auto-style1"></td>
                                <td class="auto-style1"></td>
                                <td class="auto-style1"><h5>Valor Citación:</h5></td>
                                <td style="text-align: right" class="auto-style1">
                                    <asp:Label ID="LblValorCita" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="#CC3300" Text="0.00"></asp:Label>
                                </td>
                                <td style="text-align: right" class="auto-style1"></td>
                                <td style="text-align: center" class="auto-style1"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>
                                    <h5>Descuento:</h5>
                                </td>
                                <td style="text-align: right">
                                    
                                    <asp:Label ID="LblDescuento" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Blue" Text="0.00"></asp:Label>
                                    
                                </td>
                                <td style="text-align: right"></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>
                                    <h5>Valor Pago:</h5>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="LblPago" runat="server" Font-Bold="True" Font-Size="13pt" ForeColor="Red" Text="0.00"></asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>
                                    <h5>Tipo Pago:</h5>
                                </td>
                                <td>
                                    <asp:Label ID="LblTipoPago" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Blue"></asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Panel runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                            <tr runat="server">
                                <td colspan="6">
                                    <asp:Panel ID="Panel1" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Panel ID="Panel4" runat="server" CssClass="panel panel-primary"
                                        GroupingText="Pagos Diferidos/Totales" Height="380px" ScrollBars="Vertical" 
                                        TabIndex="17">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 10%"></td>
                                                <td style="width: 20%"></td>
                                                <td style="width: 40%"></td>
                                                <td style="width: 20%"></td>
                                                <td style="width: 10%"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Panel runat="server" Height="20px"></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="TrFila1" visible="false">
                                                <td></td>
                                                <td>
                                                    <asp:ImageButton ID="ImgExportar" runat="server" Height="30px" ImageUrl="~/Botones/excel.png" OnClick="ImgExportar_Click" TabIndex="9" Width="40px" />
                                                </td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Panel runat="server" Height="10px"></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td colspan="3">
                                                    <asp:Panel runat="server" Height="250" GroupingText="Tabla de Pagos">
                                                        <asp:GridView ID="GrdvPagos" runat="server" AutoGenerateColumns="False" 
                                                            CssClass="table table-condensed table-bordered table-hover table-responsive" 
                                                            ForeColor="#333333" PageSize="5" TabIndex="10" Width="100%" 
                                                            OnRowDataBound="GrdvPagos_RowDataBound" ShowFooter="True">
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:BoundField DataField="FechaCuota" HeaderText="Fecha Pago" />
                                                                <asp:BoundField DataField="TipoPago" HeaderText="Tipo Pago" />
                                                                <asp:BoundField DataField="ValorPago" HeaderText="Valor">
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Pagado" HeaderText="Pagado" />
                                                            </Columns>
                                                            <HeaderStyle Font-Size="X-Small" />
                                                            <RowStyle Font-Bold="true" Font-Size="X-Small" />
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td colspan="2"></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Panel ID="Panel7" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                            <tr runat="server" id="TrTerreno6" visible="false">
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
                                <td style="text-align: right; width: 45%"></td>
                                <td style="width: 5%"></td>
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
