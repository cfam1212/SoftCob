<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_ReporteGerencialV3.aspx.cs" Inherits="SoftCob.Views.ReportesGerenciales.WFrm_ReporteGerencialV3" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

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

    <script type="text/javascript">
        function pageLoad(sender, arg) {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#TxtFechaIni').datepicker(
                    {
                        inline: true,
                        dateFormat: "mm/dd/yy",
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

            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#TxtFechaFin').datepicker(
                    {
                        inline: true,
                        dateFormat: "mm/dd/yy",
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
    </script>
    <style type="text/css">
        legend {
            color: darkblue;
            font-size: 10px;
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
            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updCabecera">
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
                <asp:UpdatePanel ID="updCabecera" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <table style="width: 100%" runat="server" id="TblOpciones">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 25%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 25%"></td>
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
                                <td>
                                    <asp:DropDownList ID="DdlCatalogo" runat="server" CssClass="form-control" TabIndex="2" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <asp:Panel runat="server" Height="10px"></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Fecha Proceso:</h5>
                                </td>
                                <td>
                                    <h5>Desde:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtFechaIni" runat="server" CssClass="form-control" Width="100%" TabIndex="3"></asp:TextBox>
                                </td>
                                <td>
                                    <h5>Hasta:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtFechaFin" runat="server" CssClass="form-control" Width="100%" TabIndex="4"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td colspan="2"></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <table style="width: 100%" runat="server" id="TblFiltros">
                            <tr>
                                <td style="width: 12%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 1%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 12%"></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Panel ID="Panel1" runat="server" Height="250px" GroupingText="Opciones de Filtro">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 10%"></td>
                                                <td style="width: 25%"></td>
                                                <td style="width: 55%"></td>
                                                <td style="width: 10%"></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td colspan="2">
                                                    <asp:RadioButtonList ID="RdbOpciones" runat="server" TabIndex="5" OnSelectedIndexChanged="RdbOpciones_SelectedIndexChanged" AutoPostBack="True">
                                                        <asp:ListItem Value="0" Selected="True">Compromiso de Pago</asp:ListItem>
                                                        <asp:ListItem Value="1">Compromiso por Arbol</asp:ListItem>
                                                        <asp:ListItem Value="2">Pagos Realizados</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="Panel3" runat="server" Height="20px"></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:CheckBox ID="ChkPorGestor" runat="server" Text="Por Gestor" TabIndex="6" OnCheckedChanged="ChkPorGestor_CheckedChanged" AutoPostBack="True" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DdlGestor" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlGestor_SelectedIndexChanged" TabIndex="7" Width="100%" Enabled="False">
                                                    </asp:DropDownList>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="Panel4" runat="server" Height="20px"></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td colspan="2">
                                                    <asp:CheckBox ID="ChkComparativas" runat="server" Text="Realizar Comporativas" TabIndex="8" OnCheckedChanged="ChkComparativas_CheckedChanged" />
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="Panel5" runat="server" Height="20px"></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:CheckBox ID="ChkPorYear" runat="server" TabIndex="9" Text="Por Año" AutoPostBack="True" Checked="True" OnCheckedChanged="ChkPorYear_CheckedChanged" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="ChkPorMes" runat="server" TabIndex="10" Text="Por Mes" AutoPostBack="True" OnCheckedChanged="ChkPorMes_CheckedChanged" />
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td></td>
                                <td colspan="3">
                                    <asp:Panel ID="Panel2" runat="server" Height="250px" GroupingText="Gestores Seleccionados">
                                        <asp:GridView ID="GrdvGestores" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="Codigo" ForeColor="#333333" PageSize="7" TabIndex="11" Width="100%">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="Gestor" HeaderText="Gestor"></asp:BoundField>
                                                <asp:TemplateField HeaderText="Borrar">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgDelete" runat="server" Height="15px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgDelete_Click" OnClientClick="return asegurar();" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle Font-Size="X-Small" />
                                            <RowStyle Font-Size="X-Small" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <asp:Panel ID="Panel6" runat="server" Height="30px"></asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <div class="panel panel-default">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 45%">
                                        <asp:Button ID="BtnProcesar" runat="server" Text="Procesar" Width="120px" CssClass="button" TabIndex="12" OnClick="BtnProcesar_Click" />
                                    </td>
                                    <td style="width: 10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="13" OnClick="BtnSalir_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>


            <div class="panel-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%" runat="server" id="TblReporte">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 90%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <rsweb:ReportViewer ID="RptViewDatos" runat="server" Width="100%">
                                    </rsweb:ReportViewer>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
