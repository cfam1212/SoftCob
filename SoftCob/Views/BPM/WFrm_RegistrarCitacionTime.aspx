<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_RegistrarCitacionTime.aspx.cs" Inherits="SoftCob.Views.BPM.WFrm_RegistrarCitacionTime" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nuevos Campos Estrategia</title>

    <%--    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>--%>

    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />

    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <script src="../../Scripts/jquery.min.js"></script>
    <script src="../../JS/DatePicker/jquery-ui.js"></script>

    <script type="text/javascript">
        function pageLoad(sender, arg) {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#TxtFechaCitacion').datepicker(
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
                                    <asp:Panel ID="PnlDatosGetion" runat="server" CssClass="panel panel-primary" Height="200px"
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
                                                        <img alt="" style="cursor: pointer" src="../../Botones/agregar.png" width="15" height="15" id="btnplus" />
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
                                                                <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                                                <RowStyle Font-Size="X-Small" />
                                                                <EditRowStyle BackColor="#2461BF" />
                                                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Logo">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgLogo" runat="server" Height="15px" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Canal" HeaderText="Canal" />
                                            </Columns>
                                            <HeaderStyle Font-Size="X-Small" />
                                            <RowStyle Font-Bold="true" Font-Size="XX-Small" />
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
                                    <asp:Calendar ID="CalenCitaciones" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" Width="200px" OnSelectionChanged="CalenCitaciones_SelectionChanged" TabIndex="1">
                                        <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                        <NextPrevStyle VerticalAlign="Bottom" />
                                        <OtherMonthDayStyle ForeColor="#808080" />
                                        <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                        <SelectorStyle BackColor="#CCCCCC" />
                                        <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                                        <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                        <WeekendDayStyle BackColor="#FFFFCC" />
                                    </asp:Calendar>
                                </td>
                                <td></td>
                                <td>
                                    <asp:Panel ID="Panel3" runat="server" CssClass="panel panel-primary" Height="200px" GroupingText="Horarios Citación" ScrollBars="Vertical" TabIndex="17">
                                        <asp:GridView ID="GrdvHorarios" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333"
                                            PageSize="5" TabIndex="2" Width="100%" DataKeyNames="Codigo,EstadoCodigo,HoraInicio,HoraFin,CodigoUSUA,CodigoPERS" OnRowDataBound="GrdvHorarios_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Citación">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Imgcitacion" runat="server" Height="20px" ImageUrl="~/Botones/citamedica.png" OnClick="Imgcitacion_Click" Enabled="False" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Hora" HeaderText="Hora">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                <asp:BoundField DataField="Usuario" HeaderText="Usuario Agenda" />
                                                <asp:BoundField DataField="Gestor" HeaderText="Gestor" />
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
                                <td></td>
                                <td>
                                    <asp:Panel ID="Panel1" runat="server" Height="250px" GroupingText="Cargar Archivo Citación">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 35%"></td>
                                                <td style="width: 65%"></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Valor Citación:</h5>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:Label ID="LblValor" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Documento Citación:</h5>
                                                </td>
                                                <td>
                                                    <asp:FileUpload ID="FileUpload1" runat="server" TabIndex="14" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Panel runat="server" Height="20px"></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="LblObservacion" runat="server" Font-Bold="True" ForeColor="#0000CC"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Panel ID="Panel5" runat="server" Height="20px"></asp:Panel>
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
                    <Triggers>
                        <asp:PostBackTrigger ControlID="BtnGrabar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
