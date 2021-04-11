<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_NuevoBrenchMes.aspx.cs" Inherits="SoftCob.Views.Breanch.WFrm_NuevoBrenchMes" %>

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
    <script type="text/javascript">
        function ValidarDecimales() {
            var numero = document.getElementById("<%=TxtPresupuesto.ClientID%>").value;
            if (!/^([0-9])*[.]?[0-9]*$/.test(numero)) {
                alert("El valor " + numero + " no es un número válido. Ejemplo 157.68");
                document.getElementById("<%=TxtPresupuesto.ClientID%>").value = "";
                return false;
            }
        }
    </script>
    <script>
        function asegurar() {
            rc = confirm("¿Seguro que desea Grabar?");
            return rc;
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
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updBotones">
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
            <asp:UpdatePanel ID="updCabecera" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 5%"></td>
                            <td style="width: 10%"></td>
                            <td style="width: 20%"></td>
                            <td style="width: 20%"></td>
                            <td style="width: 10%"></td>
                            <td style="width: 30%"></td>
                            <td style="width: 5%"></td>
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
                                <asp:DropDownList ID="DdlCatalogo" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="2" Width="100%" OnSelectedIndexChanged="DdlCatalogo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <h5>Gestor:</h5>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="DdlGestores" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlGestores_SelectedIndexChanged" TabIndex="3" Width="100%">
                                </asp:DropDownList>
                            </td>
                            <td colspan="3">
                                <asp:Panel ID="pnlRangoDias0" runat="server" GroupingText="Brench" Height="180px" ScrollBars="Vertical">
                                    <asp:GridView ID="GrdvBrenchCab" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" EmptyDataText="No existen datos para mostrar" PageSize="12" TabIndex="4" Width="100%">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="RangoIni" HeaderText="R.Inicial">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RangoFin" HeaderText="R.Final">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Etiqueta" HeaderText="Brench" />
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
                            <td colspan="7">
                                <asp:Panel ID="pnlDiv1" runat="server" Height="20px"></asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <h5>Presupuesto:</h5>
                            </td>
                            <td>
                                <asp:TextBox ID="TxtPresupuesto" runat="server" CssClass="form-control alinearDerecha" MaxLength="10" TabIndex="6" Width="100%" Enabled="False"></asp:TextBox>
                            </td>
                            <td style="text-align: center">
                                <asp:ImageButton ID="ImgModificar" runat="server" Enabled="False" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModificar_Click" TabIndex="6" />
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="5">
                                <asp:Panel ID="pnlBrenchMes" runat="server" ScrollBars="Vertical" Height="200px" GroupingText="Brench Mensual">
                                    <asp:GridView ID="GrdvBrenchDet" runat="server" Width="100%"
                                        CssClass="table table-condensed table-bordered table-hover table-responsive" EmptyDataText="No existen datos para mostrar" TabIndex="7" AutoGenerateColumns="False" PageSize="12" DataKeyNames="CodigoAlter,Codigo,ExigibleValor,MontoValor,PorcentajeValor,PresupuestoValor,Operaciones" ShowFooter="True" OnRowDataBound="GrdvBrenchDet_RowDataBound">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="Rango" HeaderText="Rangos"></asp:BoundField>
                                            <asp:BoundField DataField="Operaciones" HeaderText="Operaciones">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Monto" HeaderText="Monto">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Exigible" HeaderText="Exigible">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Porcentaje" HeaderText="% Presupuesto">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Presupuesto" HeaderText="Presupuesto">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Selecc">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgSelecc" runat="server" Height="20px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSelecc_Click" />
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
                    <div class="panel-info">
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="panel panel-default">
            <asp:UpdatePanel ID="updBotones" runat="server">
                <ContentTemplate>
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: right; width: 45%">
                                <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CausesValidation="False" CssClass="button" TabIndex="8" OnClick="BtnGrabar_Click" OnClientClick="return asegurar();" />
                            </td>
                            <td style="width: 10%"></td>
                            <td style="text-align: left; width: 45%">
                                <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="9" OnClick="BtnSalir_Click" />
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
