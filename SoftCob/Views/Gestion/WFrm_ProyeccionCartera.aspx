<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="WFrm_ProyeccionCartera.aspx.cs" Inherits="SoftCob.Views.Gestion.WFrm_ProyeccionCartera" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../JS/css/alertify.min.css" rel="stylesheet" />
    <script src="../../Scripts/external/jquery/jquery.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <script src="../../JS/alertify.min.js"></script>
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>

    <script type="text/javascript">
        function pageLoad(sender, arg) {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#TxtFecha').datepicker(
                    {
                        inline: true,
                        dateFormat: "yy-mm-dd",
                        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                        numberOfMonths: 2,
                        showButtonPanel: true,
                        changeMonth: true,
                        changeYear: true,
                        yearRange: "-100:+5"
                    });
            });
        }

        function ValidarDecimales() {
            var numero = document.getElementById("<%=TxtValor.ClientID%>").value;
            if (!/^([0-9])*[.]?[0-9]*$/.test(numero)) {
                alert("El valor " + numero + " no es un número válido. Ejemplo 157.68");
                document.getElementById("<%=TxtValor.ClientID%>").value = "";
                return false;
            }
        }

        function asegurar() {
            rc = confirm("¿Seguro que desea Eliminar?");
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
<%--            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updCabecera">
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
                <asp:UpdatePanel ID="updCabecera" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 35%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="4">
                                    <asp:GridView ID="GrdvProyeccion" runat="server" AutoGenerateColumns="False"
                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                        EmptyDataText="No Existen Registros" ForeColor="#333333" PageSize="5" TabIndex="4" Width="100%">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="VPresupuesto" HeaderText="Presupuesto">
                                                <HeaderStyle BackColor="#33CCFF" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="VProyeccion" HeaderText="Proyección">
                                                <HeaderStyle BackColor="#66CCFF" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="VPorcenProyecc" HeaderText="% Proyecc.">
                                                <HeaderStyle BackColor="#66CCFF" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="VPagos" HeaderText="Pagos_Efectivos">
                                                <HeaderStyle BackColor="#66CCFF" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="VPorcenCumplimiento" HeaderText="% Cumplimiento">
                                                <HeaderStyle BackColor="#66CCFF" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NoEfectivos" HeaderText="No Efectivos">
                                                <HeaderStyle BackColor="#993333" ForeColor="White" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Lost" HeaderText="Lost Payment">
                                                <HeaderStyle BackColor="#FF9933" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                        </Columns>
                                        <HeaderStyle Font-Size="Small" />
                                        <RowStyle Font-Size="Small" />
                                    </asp:GridView>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="6"></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <div class="panel-body">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 100%">
                                                    <asp:Panel ID="PnlHeaderProy" runat="server">
                                                        Proyeccion
                                                            <asp:Image runat="server" ID="ImgPresu" ImageUrl="~/Botones/collapseopen.png" Height="20px" />
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="PnlProyeccion" runat="server"
                                                        CssClass="panel panel-primary" Height="330"
                                                        GroupingText="Datos de Proyección">
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
                                                                    <h5>Buscar Cliente:</h5>
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ID="ImgBuscar" runat="server" ImageUrl="~/Botones/user_new.png" OnClick="ImgBuscar_Click" Height="25px" />
                                                                </td>
                                                                <td></td>
                                                                <td></td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <h5>Identificación:</h5>
                                                                </td>
                                                                <td>
                                                                    <h5 runat="server" id="LblIdentificacion"></h5>
                                                                </td>
                                                                <td>
                                                                    <h5>Cliente:</h5>
                                                                </td>
                                                                <td>
                                                                    <h5 runat="server" id="LblCliente"></h5>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <h5>Fecha:</h5>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="TxtFecha" runat="server" CssClass="form-control" TabIndex="1" Width="100%" Enabled="False"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <h5>Valor:</h5>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="TxtValor" runat="server" CssClass="form-control alinearDerecha" TabIndex="2" Width="100%" Enabled="False">0.00</asp:TextBox>
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
                                                                <td>
                                                                    <h5>Observación:</h5>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="TxtObservacion" runat="server" CssClass="form-control" Enabled="False" Height="50px" MaxLength="100" onkeydown="return (event.keyCode!=13);" TabIndex="4" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    
                                                                </td>
                                                                <td></td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td></td>
                                                                <td>
                                                                    <asp:TextBox ID="TxtDocumento" runat="server" CssClass="form-control alinearDerecha" Enabled="False" TabIndex="3" Width="100%" Visible="False"></asp:TextBox>
                                                                </td>
                                                                <td></td>
                                                                <td></td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td></td>
                                                                <td style="text-align: center">
                                                                    <asp:ImageButton ID="ImgActualizar" runat="server" Enabled="False" Height="25px" ImageUrl="~/Botones/modificarnew.png" OnClick="ImgActualizar_Click" TabIndex="5" />
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ID="ImgAgregar" runat="server" Enabled="False" Height="25px" ImageUrl="~/Botones/agregar.png" OnClick="ImgAgregar_Click" TabIndex="6" />
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ID="ImgEliminar" runat="server" Enabled="False" Height="25px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgEliminar_Click" OnClientClick="return asegurar();" TabIndex="7" />
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:CollapsiblePanelExtender ID="PnlProyeccion_CollapsiblePanelExtender"
                                                        runat="server" Enabled="True" TargetControlID="PnlProyeccion"
                                                        ExpandControlID="PnlHeaderProy" CollapsedImage="~/Botones/collapseopen.png"
                                                        ExpandedImage="~/Botones/collapseclose.png" CollapsedText="Mostrando Proyección..."
                                                        ImageControlID="ImgPresu" SuppressPostBack="True" Collapsed="True"
                                                        CollapseControlID="PnlHeaderProy" ExpandedText="Ocultar Proyección">
                                                    </asp:CollapsiblePanelExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Panel ID="Panel1" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <div class="panel panel-default">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left">
                                        <asp:ImageButton ID="ImgExportar" runat="server" ImageUrl="~/Botones/editargris.png" Width="40px" Height="30px" OnClick="ImgExportar_Click" Visible="false" TabIndex="8" />
                                        <asp:Label ID="lblExportar" runat="server" Text="Exportar" Visible="false"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                        <div class="panel-info">
                            <asp:GridView ID="GrdvDatos" runat="server" Width="100%"
                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                EmptyDataText="No Existen Registros" TabIndex="9" AutoGenerateColumns="False"
                                ForeColor="#333333" OnRowDataBound="GrdvDatos_RowDataBound" PageSize="5" DataKeyNames="CodigoRESP,EstadoPago,CodigoPRCB,Respuesta,Cedula,CodigoESTA">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="EstadoPago" HeaderText="Estado_Pago"></asp:BoundField>
                                    <asp:BoundField DataField="Cedula" HeaderText="CI" />
                                    <asp:BoundField DataField="Cliente" HeaderText="Cliente"></asp:BoundField>
                                    <asp:BoundField DataField="FechaGestion" HeaderText="Fecha_Gestión">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FechaPago" HeaderText="Fecha_Pago">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Valor" HeaderText="Valor">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Observacion" HeaderText="Observación"></asp:BoundField>
                                    <asp:BoundField DataField="Respuesta" HeaderText="Respuesta" />
                                    <asp:TemplateField HeaderText="Selecc">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgSeleccionar" runat="server" Height="20px" ImageUrl="~/Botones/seleccbg.png" OnClick="ImgSeleccionar_Click" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Verificar">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgVerificar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/verificargris.png" OnClick="ImgVerificar_Click" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                <RowStyle Font-Size="X-Small" />
                                <EditRowStyle BackColor="#2461BF" />
                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                            </asp:GridView>
                            <script>
                                var prm = Sys.WebForms.PageRequestManager.getInstance();

                                prm.add_endRequest(function () {
                                    createDataTable();
                                });

                                createDataTable();

                                function createDataTable() {
                                    $('#<%= GrdvDatos.ClientID %>').DataTable();
                                }
                            </script>
                        </div>
                        <div class="panel panel-default">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 45%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="10" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
