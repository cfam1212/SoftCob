<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_ParametroNuevo.aspx.cs" Inherits="SoftCob.Views.Configuraciones.WFrm_ParametroNuevo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../JS/css/alertify.min.css" rel="stylesheet" />

    <script src="../../Scripts/external/jquery/jquery.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../JS/alertify.min.js"></script>
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <script src="../../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../../Scripts/jquery-ui.min.js"></script>
    <link href="../../css/jquery.ui.accordion.css" rel="stylesheet" />

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
        $(function () {
            $("#acordionParametro").accordion();
        });
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
                <div id="acordionParametro">
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">PARAMETROS</h3>
                    <asp:UpdatePanel ID="updCabecera" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 10%"></td>
                                        <td style="width: 20%">
                                            <h5>Parámetro:</h5>
                                        </td>
                                        <td style="width: 45%">
                                            <asp:TextBox ID="TxtParametro" runat="server" Width="100%" CssClass="form-control upperCase" MaxLength="80" TabIndex="1"></asp:TextBox>
                                        </td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 10%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Descripción:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtDescripcion" runat="server" onkeydown="return (event.keyCode!=13);" Width="100%" CssClass="form-control upperCase" MaxLength="80" Height="50px" TextMode="MultiLine" TabIndex="2"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5 runat="server" id="LblEstadoPar" visible="false">Estado:</h5>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="ChkEstadoPar" runat="server" AutoPostBack="True" Text="Activo" Checked="True" OnCheckedChanged="ChkEstadoPar_CheckedChanged" Visible="False" CssClass="form-control" TabIndex="3" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">DETALLE DE PARAMETROS</h3>
                    <asp:UpdatePanel ID="updDetalle" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 20%">
                                            <h5>Detalle:</h5>
                                        </td>
                                        <td style="width: 55%">
                                            <asp:TextBox ID="TxtDetalle" runat="server" Width="100%" CssClass="form-control upperCase" MaxLength="80" TabIndex="4"></asp:TextBox>
                                        </td>
                                        <td style="width: 15%; text-align: center;">
                                            <asp:ImageButton ID="ImgAgregar" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAgregar_Click" TabIndex="8" />
                                        </td>
                                        <td style="width: 5%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Valor Varchar:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtValorV" runat="server" CssClass="form-control" Width="100%" MaxLength="80" TabIndex="5"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ImgModificar" runat="server" Enabled="False" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModificar_Click" TabIndex="9" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Valor Entero:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtValorI" runat="server" CssClass="form-control upperCase" MaxLength="8" Width="100%" TabIndex="6"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtValorI_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtValorI">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ImgCancelar" runat="server" CausesValidation="False" Enabled="False" Height="25px" ImageUrl="~/Botones/cancelarbg.png" OnClick="ImgCancelar_Click" TabIndex="10" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5 runat="server" id="LblEstadoDet" visible="false">Estado</h5>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="ChkEstadoDet" runat="server" AutoPostBack="True" Text="Activo" Checked="True" Visible="False" CssClass="form-control" OnCheckedChanged="ChkEstadoDet_CheckedChanged" TabIndex="7" />
                                        </td>
                                        <td style="text-align: center"></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="text-align: center"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="3">
                                            <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Height="230px" GroupingText="Parámetros">
                                                <asp:GridView ID="GrdvDatos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,Orden,Nuevo" ForeColor="#333333" OnRowDataBound="GrdvDatos_RowDataBound" OnSelectedIndexChanged="GrdvDatos_SelectedIndexChanged" PageSize="5" TabIndex="11" Width="100%">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:CommandField HeaderText="Selecc" ShowSelectButton="True" />
                                                        <asp:BoundField DataField="Detalle" HeaderText="Detalle" />
                                                        <asp:BoundField DataField="ValorV" HeaderText="Valor Varchar" />
                                                        <asp:BoundField DataField="ValorI" HeaderText="Valor Entero" />
                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                        <asp:TemplateField HeaderText="Subir Nivel">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgSubirNivel" runat="server" Height="20px" ImageUrl="~/Botones/activada_up.png" OnClick="ImgSubirNivel_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Bajar Nivel">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgBajarNivel" runat="server" Height="20px" ImageUrl="~/Botones/activada_down.png" OnClick="ImgBajarNivel_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgEliminar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/eliminargris.png" OnClick="ImgEliminar_Click" />
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
                                        <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" OnClick="BtnGrabar_Click" CssClass="button" TabIndex="12" />
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
        </div>
    </form>
</body>
</html>