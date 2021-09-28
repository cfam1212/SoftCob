<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_SeguimientoCitacionTime.aspx.cs" Inherits="SoftCob.Views.BPM.WFrm_SeguimientoCitacionTime" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />

    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <script src="../../Scripts/jquery.min.js"></script>
    <script src="../../JS/DatePicker/jquery-ui.js"></script>


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

        .ChildGrid td {
            background-color: #eee !important;
            color: black;
            font-size: 8pt;
            line-height: 200%
        }

        .ChildGrid th {
            background-color: #6C6C6C !important;
            color: White;
            font-size: 10pt;
            line-height: 200%
        }
    </style>

    <script type="text/javascript">
        $("[src*=agregar]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../../Botones/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../../Botones/agregar.png");
            $(this).closest("tr").next().remove();
        });
    </script>

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
                                                <asp:BoundField HeaderText="Valor Deuda" DataField="ValorDeuda">
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
                            <tr runat="server" id="TrGarantes" visible="false">
                                <td></td>
                                <td colspan="3">
                                    <asp:Panel ID="PnlDatosGarante" runat="server" CssClass="panel panel-primary" Height="200px"
                                        GroupingText="Datos Garante" ScrollBars="Vertical" TabIndex="17">
                                        <asp:GridView ID="GrdvDatosGarante" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333"
                                            PageSize="3" TabIndex="16" Width="100%" DataKeyNames="CedulaGarante,Existe,CodigoGARA,Operacion">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="Tipo" HeaderText="Tipo"></asp:BoundField>
                                                <asp:BoundField DataField="CedulaGarante" HeaderText="Cédula" />
                                                <asp:BoundField DataField="Garante" HeaderText="Nombres"></asp:BoundField>
                                                <asp:BoundField DataField="Operacion" HeaderText="Operación"></asp:BoundField>
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
                                <td style="width: 20%"></td>
                                <td style="width: 5%"></td>
                                <td style="width: 65%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="3">
                                    <asp:Panel ID="PnlCitaciones" runat="server" CssClass="panel panel-primary" GroupingText="Canales para Citaciones"
                                        Height="250px" ScrollBars="Vertical" TabIndex="17">
                                        <asp:GridView ID="GrdvCitaciones" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="Canal,CodigoCITA" ForeColor="#333333" Height="25px"
                                            OnRowDataBound="GrdvCitaciones_RowDataBound" PageSize="5" TabIndex="7" Width="100%">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <img alt="" style="cursor: pointer" src="../../Botones/agregar.png" width="20" height="20" id="btnplus" />
                                                        <asp:Panel runat="server" Style="display: none">
                                                            <asp:GridView ID="GrdvCanales" runat="server" AutoGenerateColumns="false"
                                                                CssClass="table table-condensed table-bordered table-hover table-responsive">
                                                                <Columns>
                                                                    <asp:BoundField DataField="TipoCliente" HeaderText="Tipo" />
                                                                    <asp:BoundField DataField="Definicion" HeaderText="Definicion" />
                                                                    <asp:BoundField DataField="Celular" HeaderText="Celular" />
                                                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                                                    <asp:BoundField DataField="Direccion" HeaderText="Direccion" />
                                                                    <asp:BoundField DataField="Referencia" HeaderText="Referencia" />
                                                                    <asp:BoundField DataField="Sector" HeaderText="Sector" />
                                                                    <asp:BoundField DataField="FechaVisita" HeaderText="Fecha Visita" />
                                                                </Columns>
                                                                <HeaderStyle Font-Size="X-Small" />
                                                                <RowStyle Font-Bold="true" Font-Size="XX-Small" />
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Logo">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgLogo" runat="server" Height="20px" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Canal" HeaderText="Canal" />
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
                                <td style="width: 20%"></td>
                                <td style="width: 5%"></td>
                                <td style="width: 65%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:LinkButton ID="LnkHistorial" runat="server" OnClick="LnkHistorial_Click">Todo el Historial</asp:LinkButton>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="3">
                                    <asp:Panel ID="Panel3" runat="server" CssClass="panel panel-primary" Height="350px" GroupingText="Historial Citaciones" ScrollBars="Vertical" TabIndex="17">
                                        <asp:GridView ID="GrdvDetalle" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333"
                                            PageSize="3" TabIndex="1" Width="100%" DataKeyNames="PathArchivo,Content,CodigoESTA" OnRowDataBound="GrdvDetalle_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Estado" HeaderText="Estado"></asp:BoundField>
                                                <asp:BoundField DataField="Observacion" HeaderText="Observación" />
                                                <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                                                <asp:TemplateField HeaderText="Archivo">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgArchivo" runat="server" Height="20px" ImageUrl="~/Botones/Buscargris.png" Enabled="False" OnClick="ImgArchivo_Click" />
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
                                <td style="width: 20%"></td>
                                <td style="width: 60%"></td>
                                <td style="width: 20%"></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Panel ID="Panel5" runat="server" Height="20px">
                                    </asp:Panel>
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
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="9" />
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
