<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_NuevaEstrategia.aspx.cs" Inherits="SoftCob.Views.Configuraciones.WFrm_NuevaEstrategia" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nueva Estrategia</title>
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
                $('#txtFechaInicio').datepicker(
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
                $('#txtFechaFin').datepicker(
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
    <script>
        function asegurar() {
            rc = confirm("¿Seguro que desea Eliminar?");
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
            <asp:UpdatePanel ID="updCabecera" runat="server">
                <ContentTemplate>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 5%"></td>
                            <td style="width: 10%">
                                <h5>Estrategia:</h5>
                            </td>
                            <td style="width: 35%">
                                <asp:TextBox ID="TxtEstrategia" runat="server" CssClass="form-control upperCase" MaxLength="150" TabIndex="1" Width="100%">Est.</asp:TextBox>
                            </td>
                            <td style="width: 10%">
                                <h5>Descripción:</h5>
                            </td>
                            <td style="width: 35%">
                                <asp:TextBox ID="TxtDescripcion" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="form-control upperCase" Height="50px" MaxLength="250" TextMode="MultiLine" Width="100%" TabIndex="2"></asp:TextBox>
                            </td>
                            <td style="width: 5%"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <h5 runat="server" id="lblEstado" visible="false">Estado:</h5>
                            </td>
                            <td>
                                <asp:CheckBox ID="ChkEstadoCab" runat="server" CssClass="form-control" AutoPostBack="True" Checked="True" OnCheckedChanged="ChkEstadoCab_CheckedChanged" TabIndex="3" Visible="False" />
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Panel ID="pnlDivision" runat="server" Height="30px"></asp:Panel>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 5%"></td>
                            <td style="width: 35%; text-align: center;">
                                <h5>Campo</h5>
                            </td>
                            <td style="width: 25%; text-align: center;">
                                <h5>Operación</h5>
                            </td>
                            <td style="width: 15%; text-align: center;">
                                <h5>Valor</h5>
                            </td>
                            <td style="width: 15%; text-align: center;">
                                <asp:CheckBox ID="ChkOrdenar" runat="server" CssClass="form-control" Text="Ordenar" OnCheckedChanged="ChkOrdenar_CheckedChanged" AutoPostBack="True" TabIndex="7" />
                            </td>
                            <td style="width: 5%"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:DropDownList ID="DdlCampos" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" OnSelectedIndexChanged="DdlCampos_SelectedIndexChanged" TabIndex="4">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlOperacion" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" OnSelectedIndexChanged="DdlOperacion_SelectedIndexChanged" TabIndex="5">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtValor" runat="server" CssClass="form-control" TabIndex="6" Width="100%"></asp:TextBox>
                                <asp:PlaceHolder ID="PlaceTxt" runat="server"></asp:PlaceHolder>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="RdbOrdenar" runat="server" CssClass="form-control" Font-Size="10pt" Height="40px" Enabled="False" TabIndex="8">
                                    <asp:ListItem Value="asc" Selected="True">Ascendente</asp:ListItem>
                                    <asp:ListItem Value="desc">Descendente</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Panel ID="pnlDivision1" runat="server" Height="30px"></asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:ImageButton ID="ImgAddCampo" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAddCampo_Click" TabIndex="9" />
                            </td>
                            <td>
                                <asp:ImageButton ID="ImgModCampo" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModCampo_Click" TabIndex="10" />
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Panel ID="Panel1" runat="server" Height="20px"></asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:GridView ID="GrdvCampos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,CodigoCampo,Prioridad,Estado" ForeColor="#333333" PageSize="5" TabIndex="11" Width="100%" OnRowDataBound="GrdvCampos_RowDataBound">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="Campo" HeaderText="Campo" />
                                        <asp:BoundField DataField="Operacion" HeaderText="Operacion" />
                                        <asp:BoundField DataField="Valor" HeaderText="Valor" />
                                        <asp:BoundField DataField="Orden" HeaderText="Ordenar" />
                                        <asp:TemplateField HeaderText="Prioridad">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgSubir" runat="server" Height="20px" ImageUrl="~/Botones/activada_up.png" OnClick="ImgSubir_Click" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Activo">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkEstado" runat="server" AutoPostBack="True" Checked="True" OnCheckedChanged="ChkEstado_CheckedChanged" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Selecc">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgSelecc" runat="server" Height="20px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSelecc_Click" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Eliminar">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgDelCampo" runat="server" Height="20px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgDelCampo_Click" OnClientClick="return asegurar();" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                    <RowStyle Font-Size="X-Small" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
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
                                <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="BtnGrabar_Click" TabIndex="12" />
                            </td>
                            <td style="width: 10%"></td>
                            <td style="text-align: left; width: 45%">
                                <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="13" />
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
