<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_EfectivoLostPayment.aspx.cs" Inherits="SoftCob.Views.BPM.WFrm_EfectivoLostPayment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../../Scripts/external/jquery/jquery.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>

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
        function asegurar() {
            rc = confirm("¿Confirmar Actualizar Pago Efectivo?");
            return rc;
        }
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
             <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdOpciones">
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
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="panel panel-default">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 5%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 15%"></td>
                                    <td style="width: 20%"></td>
                                    <td style="width: 20%"></td>
                                    <td style="width: 20%"></td>
                                    <td style="width: 10%"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Cedente:</h5>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="DdlCedente" runat="server" CssClass="form-control" Width="100%" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="DdlCedente_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <h5>Catálogo:</h5>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="DdlCatalogo" runat="server" CssClass="form-control" TabIndex="2" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <asp:Panel ID="Panel1" runat="server" Height="10px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Gestor:</h5>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="DdlGestor" runat="server" CssClass="form-control" Width="100%" TabIndex="3">
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdOpciones" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="BtnProcesar" runat="server" CssClass="button" OnClick="BtnProcesar_Click" TabIndex="6" Text="Procesar" Width="120px" />
                                                </td>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                </tr>
                            </table>
                        </div>
                        <div class="panel panel-default">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left">
                                        <asp:ImageButton ID="ImgExportar" runat="server" ImageUrl="~/Botones/excel.png" Width="40px" Height="30px" OnClick="ImgExportar_Click" Visible="false" TabIndex="8" />
                                        <asp:Label ID="lblExportar" runat="server" Text="Exportar" Visible="false"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                        <div class="panel panel-default" runat="server" id="DivPagos" visible="false">
                            <asp:Panel ID="PnlPagos" runat="server" Height="200px" ScrollBars="Vertical">
                                <asp:GridView ID="GrdvLost" runat="server" AutoGenerateColumns="False"
                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                    DataKeyNames="CodigoPROY,CodigoPERS"
                                    ForeColor="#333333" PageSize="7" TabIndex="8" Width="100%">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="Identificacion" HeaderText="Identificacion">
                                            <ItemStyle Wrap="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Cliente" HeaderText="Cliente"></asp:BoundField>
                                        <asp:BoundField DataField="FechaGestion" HeaderText="Fecha Gestion"></asp:BoundField>
                                        <asp:BoundField DataField="FechaPago" HeaderText="Fecha Pago">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Valor" HeaderText="Valor">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Efetivo">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgEfectivo" runat="server" Height="20px" ImageUrl="~/Botones/abonar.png" OnClick="ImgEfectivo_Click" OnClientClick="return asegurar();" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gestiones">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgGestiones" runat="server" Height="20px" ImageUrl="~/Botones/Buscar.png" OnClick="ImgGestiones_Click" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                    <RowStyle Font-Size="X-Small" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                                </script>
                            </asp:Panel>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel-body">
                <div class="panel panel-default">
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: right; width: 45%"></td>
                            <td style="width: 10%"></td>
                            <td style="text-align: left; width: 45%">
                                <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="7" OnClick="BtnSalir_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
