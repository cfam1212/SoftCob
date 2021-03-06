<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_NuevaListaTrabajoAPO.aspx.cs" Inherits="SoftCob.Views.ListaTrabajo.WFrm_NuevaListaTrabajoAPO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nueva Lista de Trabajo</title>
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <link href="../../JS/css/alertify.min.css" rel="stylesheet" />

    <script type="text/javascript" src="../../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../JS/alertify.min.js"></script>
    <script type="text/javascript">
        function pageLoad(sender, arg) {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#TxtFechaInicio').datepicker(
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

                $('#TxtFechaDesde').datepicker(
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

            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#TxtFechaFin').datepicker(
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
                $('#TxtFechaHasta').datepicker(
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
        .auto-style1 {
            height: 40px;
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
            <asp:UpdatePanel ID="updCabecera" runat="server">
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
                                <h5>Estrategia:</h5>
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlEstrategia" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="1" Width="100%" OnSelectedIndexChanged="DdlEstrategia_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td colspan="2">
                                <asp:GridView ID="GrdvEstrategia" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo" ForeColor="#333333" PageSize="5" TabIndex="2" Width="100%">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="Campo" HeaderText="Campo" />
                                        <asp:BoundField DataField="Operacion" HeaderText="Operacion" />
                                        <asp:BoundField DataField="Valor" HeaderText="Valor" />
                                        <asp:BoundField DataField="Ordenar" HeaderText="Ordenar" />
                                    </Columns>
                                    <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                    <RowStyle Font-Size="X-Small" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="4">
                                <asp:Panel ID="pnlDivision1" runat="server" Height="20px">
                                </asp:Panel>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="4"></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="4">
                                <asp:Panel ID="PnlDatosListas" runat="server" Height="250px" GroupingText="Datos Lista" TabIndex="3">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 15%"></td>
                                            <td style="width: 35%"></td>
                                            <td style="width: 15%"></td>
                                            <td style="width: 35%"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <h5>Lista Trabajo:</h5>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtLista" runat="server" CssClass="form-control upperCase" MaxLength="150" TabIndex="4" Width="100%">LST_APY.</asp:TextBox>
                                            </td>
                                            <td>
                                                <h5>Descripción:</h5>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtDescripcion" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="form-control upperCase" Height="50px" MaxLength="250" TextMode="MultiLine" Width="100%" TabIndex="5"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <h5>Fecha Inicio:</h5>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtFechaInicio" runat="server" CssClass="form-control" Width="100%" TabIndex="6"></asp:TextBox>
                                            </td>
                                            <td>
                                                <h5>Fecha Fin:</h5>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtFechaFin" runat="server" CssClass="form-control" Width="100%" TabIndex="7"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <h5>Cedente:</h5>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DdlCedente" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="8" Width="100%" OnSelectedIndexChanged="DdlCedente_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <h5>Catálogo/Producto:</h5>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DdlCatalogo" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="9" Width="100%" OnSelectedIndexChanged="DdlCatalogo_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <h5>Marcado:</h5>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DdlMarcado" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="10" Width="100%">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <h5 runat="server" id="lblEstado" visible="false">Estado:</h5>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkEstado" runat="server" CssClass="form-control" AutoPostBack="True" Checked="True" TabIndex="11" Visible="False" OnCheckedChanged="ChkEstado_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Panel ID="pnlDiv0" runat="server" Height="30px"></asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="4">
                                <asp:Panel ID="PnlConfiguracion" runat="server" GroupingText="Configurar Opciones" TabIndex="12" Height="100px">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 15%"></td>
                                            <td style="width: 35%"></td>
                                            <td style="width: 15%"></td>
                                            <td style="width: 35%"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <h5>Campaña:</h5>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DdlCampania" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="13" Width="100%" OnSelectedIndexChanged="DdlCampania_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <h5>Asignación:</h5>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DdlAsignacion" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="14" Width="100%" OnSelectedIndexChanged="DdlAsignacion_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="6"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="4">
                                <asp:Panel ID="PnlGestores" runat="server" GroupingText="Configurar Gestor Apoyo" TabIndex="15" Height="320px">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 15%"></td>
                                            <td style="width: 35%"></td>
                                            <td style="width: 5%"></td>
                                            <td style="width: 45%"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <h5>Gestor:</h5>
                                            </td>
                                            <td>
                                                <asp:Panel ID="Panel1" runat="server" Height="230px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 35%"></td>
                                                            <td style="width: 5%"></td>
                                                            <td style="width: 50%"></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:Panel ID="Panel2" runat="server" Height="80px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:DropDownList ID="DdlGestor" runat="server" CssClass="form-control" TabIndex="16" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlGestor_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" class="auto-style1">
                                                                <asp:RadioButtonList ID="RdbOpcionesApoyo" runat="server" Font-Size="8pt" AutoPostBack="True" OnSelectedIndexChanged="RdbOpcionesApoyo_SelectedIndexChanged" TabIndex="17">
                                                                    <asp:ListItem Value="1" Selected="True">De Gestor a uno o varios Gestores Apoyo</asp:ListItem>
                                                                    <asp:ListItem Value="2">De uno o varios Gestores Apoyo a Gestor</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </asp:Panel>
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:Panel ID="PnlGestorApoyo" runat="server" Height="230px" GroupingText="Gestores Apoyo" ScrollBars="Vertical">
                                                    <asp:GridView ID="GrdvOrigen" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo" ShowFooter="True" ShowHeaderWhenEmpty="True" TabIndex="18" Width="100%" OnRowDataBound="GrdvOrigen_RowDataBound">
                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                        <Columns>
                                                            <asp:BoundField DataField="Descripcion" HeaderText="Gestor" />
                                                            <asp:TemplateField HeaderText="Selecc">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="ChkSelecc" runat="server" OnCheckedChanged="ChkSelecc_CheckedChanged" />
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
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="6"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:CheckBox ID="ChkOpciones" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="20" Text="Opciones" OnCheckedChanged="ChkOpciones_CheckedChanged" />
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="4">
                                <asp:Panel ID="PnlOpcionGestion" runat="server" GroupingText="Opción Gestiones" TabIndex="19" Visible="false">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 15%"></td>
                                            <td style="width: 35%"></td>
                                            <td style="width: 15%"></td>
                                            <td style="width: 35%"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="ChkGestion" runat="server" CssClass="form-control" AutoPostBack="True" OnCheckedChanged="ChkGestion_CheckedChanged" TabIndex="20" Text="Gestión" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DdlTipoGestion" runat="server" AutoPostBack="True" CssClass="form-control" Enabled="False" OnSelectedIndexChanged="DdlTipoGestion_SelectedIndexChanged" TabIndex="21" Width="100%">
                                                    <asp:ListItem Value="-1">--Seleccione Tipo Gestión--</asp:ListItem>
                                                    <asp:ListItem Value="1">Efectiva</asp:ListItem>
                                                    <asp:ListItem Value="0">No Efectiva</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkArbol" runat="server" CssClass="form-control" AutoPostBack="True" OnCheckedChanged="ChkArbol_CheckedChanged" TabIndex="22" Text="Por Árbol" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DdlAccion" runat="server" AutoPostBack="True" CssClass="form-control" Enabled="False" OnSelectedIndexChanged="DdlAccion_SelectedIndexChanged" TabIndex="23" Width="100%">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="ChkFecha" runat="server" CssClass="form-control" AutoPostBack="True" OnCheckedChanged="ChkFecha_CheckedChanged" TabIndex="24" Text="Por Fecha" Checked="True" Enabled="False" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtFechaDesde" runat="server" CssClass="form-control" TabIndex="25" Width="100%"></asp:TextBox>
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:TextBox ID="TxtFechaHasta" runat="server" CssClass="form-control" TabIndex="26" Width="100%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
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
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="panel panel-default">
            <asp:UpdatePanel ID="updBotones" runat="server">
                <ContentTemplate>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 5%"></td>
                            <td style="width: 15%"></td>
                            <td style="width: 30%"></td>
                            <td style="width: 10%;"></td>
                            <td style="width: 35%"></td>
                            <td style="width: 5%"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td style="text-align: center;">
                                <asp:ImageButton ID="ImgPreview" runat="server" Height="25px" ImageUrl="~/Botones/buscarbg.png" OnClick="ImgPreview_Click" TabIndex="27" />
                            </td>
                            <td>
                                <h5 runat="server" id="LblPreview">Preview</h5>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <h5><span style="color: red">Total Registros:</span></h5>
                            </td>
                            <td style="text-align: left">
                                <h4 runat="server" id="LblTotal"><span style="color: red">0</span></h4>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                    <table style="width: 100%" runat="server" id="TblLista" visible="false">
                        <tr>
                            <td style="width: 5%"></td>
                            <td style="width: 15%"></td>
                            <td style="width: 30%"></td>
                            <td style="width: 10%;"></td>
                            <td style="width: 35%"></td>
                            <td style="width: 5%"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:ImageButton ID="ImgExportar" runat="server" Height="30px" ImageUrl="~/Botones/excelbg.png" OnClick="ImgExportar_Click" TabIndex="28" Width="40px" />
                                <asp:Label ID="LblExportar" runat="server" Text="Exportar"></asp:Label>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                                <td colspan="5">
                                    <asp:Panel runat="server" Height="20px"></asp:Panel>
                                </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="4">
                                <asp:Panel ID="pnlPreview" runat="server" Height="350px" ScrollBars="Vertical">
                                    <asp:GridView ID="GrdvPreview" runat="server" AllowPaging="True" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333" OnPageIndexChanging="GrdvPreview_PageIndexChanging" TabIndex="26" Width="100%">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="Identificacion" HeaderText="Identificación" />
                                            <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                                            <asp:BoundField DataField="Provincia" HeaderText="Provincia" />
                                            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                            <asp:BoundField DataField="FechaGestion" HeaderText="Fecha Ult. Gestión">
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
                    </table>
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: right; width: 45%">
                                <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" TabIndex="30" OnClick="BtnGrabar_Click" />
                            </td>
                            <td style="width: 10%"></td>
                            <td style="text-align: left; width: 45%">
                                <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="31" OnClick="BtnSalir_Click" />
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
