<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_NuevaBitacora.aspx.cs" Inherits="SoftCob.Views.Bitacora.WFrm_NuevaBitacora" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
                $('#TxtFechaPE').datepicker(
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

        .ajax__tab_xp .ajax__tab_header .ajax__tab_tab {
            height: 30px;
            /*background-color:white;*/
            font-weight: 200;
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
                <asp:UpdateProgress ID="UpdProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdBotones">
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
                <div>
                    <table style="border: 2px solid #008080; width: 100%">
                        <tr>
                            <td style="width: 5%"></td>
                            <td style="width: 50%"></td>
                            <td style="width: 5%"></td>
                            <td style="width: 35%"></td>
                            <td style="width: 5%"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <h3 runat="server" id="LblFecha"></h3>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Panel ID="PnlDatosSupervisor" runat="server" Height="180px" ScrollBars="Vertical">
                                    <asp:GridView ID="GrdvSupervisores" runat="server" Width="100%" ForeColor="#333333"
                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                        AutoGenerateColumns="False" PageSize="5" TabIndex="1" OnRowDataBound="GrdvSupervisores_RowDataBound" DataKeyNames="Codigo">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="Supervisor" HeaderText="Supervisor">
                                                <HeaderStyle CssClass="GVFixedHeader" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="HoraLogueo" HeaderText="Hora Logueo">
                                                <HeaderStyle CssClass="GVFixedHeader" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UltimoIngreso" HeaderText="Ult. Ingreso">
                                                <HeaderStyle CssClass="GVFixedHeader" />
                                            </asp:BoundField>
                                        </Columns>
                                        <HeaderStyle Font-Size="X-Small" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <RowStyle Font-Size="X-Small" />
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Panel ID="PnlDiv1" runat="server" Height="20px"></asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:Panel ID="Pnldiv6" runat="server" Height="20px"></asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:UpdatePanel ID="UpdCabecera" runat="server">
                    <ContentTemplate>
                        <div>
                            <table style="border: 2px solid #800000; width: 100%">
                                <tr>
                                    <td style="width: 5%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 25%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 15%"></td>
                                    <td style="width: 30%"></td>
                                    <td style="width: 5%"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Colaborador:</h5>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="DdlGestor" runat="server" class="chzn-select" TabIndex="2" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 25%"></td>
                                    <td style="width: 25%"></td>
                                    <td style="width: 25%"></td>
                                    <td style="width: 25%"></td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Panel ID="PnlDiv2" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:TabContainer ID="TabDatosBitacora" runat="server" Width="100%" Height="400px" ActiveTabIndex="8">
                                            <asp:TabPanel runat="server" HeaderText="Atrasos" ID="TabAtrasos">
                                                <HeaderTemplate>Atrasos</HeaderTemplate>
                                                <ContentTemplate>
                                                    <table style="border: 2px solid #0000FF; width: 100%">
                                                        <tr>
                                                            <td style="width: 5%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 25%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 15%"></td>
                                                            <td style="width: 30%"></td>
                                                            <td style="width: 5%"></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="PnlDiv4" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Descripción:</h5>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:TextBox ID="TxtDescripAT" runat="server" CssClass="upperCase" MaxLength="150" TabIndex="3" Width="100%"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Hora:</h5>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="TxtHoraAT" runat="server" MaxLength="9" TabIndex="4"></asp:TextBox><asp:MaskedEditExtender ID="TxtHoraAT_MaskedEditExtender" runat="server" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="TxtHoraAT"></asp:MaskedEditExtender>
                                                            </td>
                                                            <td style="text-align: right">
                                                                <asp:ImageButton ID="ImgAddAT" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAddAT_Click" TabIndex="5" />
                                                            </td>
                                                            <td style="text-align: center">
                                                                <asp:ImageButton ID="ImgModAT" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModAT_Click" TabIndex="6" Visible="False" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgDelAT" runat="server" Height="25px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgDelAT_Click" OnClientClick="return asegurar();" TabIndex="7" Visible="False" />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Observación:</h5>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:TextBox ID="TxtObservaAT" runat="server" CssClass="upperCase" MaxLength="250" TabIndex="8" Width="100%" Height="50px" onkeydown="return (event.keyCode!=13);" TextMode="MultiLine"></asp:TextBox></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="PnlDiv0" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:Panel ID="Pnl3" runat="server" Height="180px" ScrollBars="Vertical" Visible="False">
                                                                    <asp:GridView ID="GrdvAtrasos" runat="server" AutoGenerateColumns="False"
                                                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                                        DataKeyNames="Codigo,CodigoGestorAT,FirmaCodigo" ForeColor="#333333" PageSize="5" TabIndex="9" Width="100%" OnRowDataBound="GrdvAtrasos_RowDataBound">
                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Gestor" HeaderText="Colaborador">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                                <ItemStyle Wrap="True" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Hora" HeaderText="Hora">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                                <ItemStyle Wrap="True" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Observacion" HeaderText="Observación">
                                                                            <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Firma" HeaderText="Firma">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                                <ItemStyle Wrap="True" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Seleccionar">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImgSeleccAT" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSeleccAT_Click" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle Font-Size="X-Small" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        <RowStyle Font-Size="X-Small" />
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="TabFaltasF" runat="server" HeaderText="Faltas Justificadas">
                                                <HeaderTemplate>Faltas Justificadas</HeaderTemplate>
                                                <ContentTemplate>
                                                    <table style="border: 2px solid #0000FF; width: 100%">
                                                        <tr>
                                                            <td style="width: 5%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 25%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 15%"></td>
                                                            <td style="width: 30%"></td>
                                                            <td style="width: 5%"></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="PnlDiv5" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td><h5>Descripción:</h5></td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtDescripFJ" runat="server" CssClass="upperCase" MaxLength="150" TabIndex="10" Width="100%"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Observación:</h5>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtObservaFJ" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="upperCase" MaxLength="250" TabIndex="11" Width="100%" TextMode="MultiLine" Height="50px"></asp:TextBox></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgAddFJ" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" TabIndex="12" OnClick="ImgAddFJ_Click" /></td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgModFJ" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" Visible="False" TabIndex="13" OnClick="ImgModFJ_Click" /></td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgDelFJ" runat="server" Height="25px" ImageUrl="~/Botones/eliminar.png" Visible="False" TabIndex="14" OnClick="ImgDelFJ_Click" OnClientClick="return asegurar();" /></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel1" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:Panel ID="Pnl4" runat="server" Height="180px" ScrollBars="Vertical" Visible="False">
                                                                    <asp:GridView ID="GrdvFaltasJ" runat="server" AutoGenerateColumns="False"
                                                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                                        DataKeyNames="Codigo,CodigoGestorFJ,FirmaCodigo" ForeColor="#333333" PageSize="5"
                                                                        TabIndex="15" Width="100%" OnRowDataBound="GrdvFaltasJ_RowDataBound">
                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Gestor" HeaderText="Colaborador">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Observacion" HeaderText="Observación">
                                                                            <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Firma" HeaderText="Firma">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Seleccionar">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImgSeleccFJ" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSeleccFJ_Click" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" Font-Size="X-Small" ForeColor="White" />
                                                                        <RowStyle Font-Size="X-Small" />
                                                                    </asp:GridView>
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
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="TabFaltasI" runat="server" HeaderText="Faltas Injustificadas">
                                                <HeaderTemplate>Faltas Injustificadas</HeaderTemplate>
                                                <ContentTemplate>
                                                    <table style="border: 2px solid #0000FF; width: 100%">
                                                        <tr>
                                                            <td style="width: 5%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 25%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 15%"></td>
                                                            <td style="width: 30%"></td>
                                                            <td style="width: 5%"></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel2" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td><h5>Descripción:</h5></td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtDescripFI" runat="server" CssClass="upperCase" MaxLength="150" TabIndex="16" Width="100%"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Observación:</h5>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtObservaFI" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="upperCase" MaxLength="250" TabIndex="17" Width="100%" TextMode="MultiLine" Height="50px"></asp:TextBox></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgAddFI" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" TabIndex="18" OnClick="ImgAddFI_Click" /></td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgModFI" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" Visible="False" TabIndex="19" OnClick="ImgModFI_Click" /></td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgDelFI" runat="server" Height="25px" ImageUrl="~/Botones/eliminar.png" Visible="False" TabIndex="20" OnClick="ImgDelFI_Click" OnClientClick="return asegurar();" /></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel3" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:Panel ID="Pnl5" runat="server" Height="180px" ScrollBars="Vertical" Visible="False">
                                                                    <asp:GridView ID="GrdvFaltasI" runat="server" AutoGenerateColumns="False"
                                                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                                        DataKeyNames="Codigo,CodigoGestorFI,FirmaCodigo" ForeColor="#333333" PageSize="5"
                                                                        TabIndex="21" Width="100%" OnRowDataBound="GrdvFaltasI_RowDataBound">
                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Gestor" HeaderText="Colaborador">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Observacion" HeaderText="Observación">
                                                                            <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Firma" HeaderText="Firma">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Seleccionar">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImgSeleccFI" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSeleccFI_Click" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" Font-Size="X-Small" ForeColor="White" />
                                                                        <RowStyle Font-Size="X-Small" />
                                                                    </asp:GridView>
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
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="TabPermiso" runat="server" HeaderText="Permisos">
                                                <HeaderTemplate>Permisos</HeaderTemplate>
                                                <ContentTemplate>
                                                    <table style="border: 2px solid #0000FF; width: 100%">
                                                        <tr>
                                                            <td style="width: 5%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 25%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 15%"></td>
                                                            <td style="width: 30%"></td>
                                                            <td style="width: 5%"></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel22" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Descripción:</h5>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtDescripPE" runat="server" CssClass="upperCase" MaxLength="150" TabIndex="22" Width="100%"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Fecha:</h5>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="TxtFechaPE" runat="server" TabIndex="23" Width="100%"></asp:TextBox>
                                                                <asp:CalendarExtender ID="TxtFechaPE_CalendarExtender" runat="server" Enabled="True" Format="MM/dd/yyyy" TargetControlID="TxtFechaPE">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td style="text-align: center">
                                                                <asp:ImageButton ID="ImgAddPE" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAddPE_Click" TabIndex="24" />
                                                            </td>
                                                            <td style="text-align: center">
                                                                <asp:ImageButton ID="ImgModPE" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModPE_Click" TabIndex="25" Visible="False" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgDelPE" runat="server" Height="25px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgDelPE_Click" OnClientClick="return asegurar();" TabIndex="26" Visible="False" />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Observación:</h5>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtObservaPE" runat="server" CssClass="upperCase" MaxLength="250" TabIndex="27" Width="100%" Height="50px" onkeydown="return (event.keyCode!=13);" TextMode="MultiLine"></asp:TextBox></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel7" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:Panel ID="Pnl6" runat="server" Height="180px" ScrollBars="Vertical" Visible="False">
                                                                    <asp:GridView ID="GrdvPermisos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,CodigoGestorPE,FirmaCodigo" ForeColor="#333333" PageSize="5" TabIndex="28" Width="100%" OnRowDataBound="GrdvPermisos_RowDataBound">
                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Gestor" HeaderText="Colaborador">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="FechaPermiso" HeaderText="FechaPermiso">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Observacion" HeaderText="Observación">
                                                                            <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Firma" HeaderText="Firma">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Seleccionar">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImgSeleccPE" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSeleccPE_Click" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" Font-Size="X-Small" ForeColor="White" />
                                                                        <RowStyle Font-Size="X-Small" />
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="TabCambioTurno" runat="server" HeaderText="Cambio de Turno">
                                                <HeaderTemplate>Cambio de Turno</HeaderTemplate>
                                                <ContentTemplate>
                                                    <table style="border: 2px solid #0000FF; width: 100%">
                                                        <tr>
                                                            <td style="width: 5%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 25%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 15%"></td>
                                                            <td style="width: 30%"></td>
                                                            <td style="width: 5%"></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel6" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Descripción:</h5>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtDescripCT" runat="server" CssClass="upperCase" MaxLength="150" TabIndex="29" Width="100%"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Turno Actual:</h5>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="DdlTurnoA" runat="server" CssClass="form-control" TabIndex="30" Width="100%"></asp:DropDownList></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Cambiar A:</h5>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="DdlTurnoN" runat="server" CssClass="form-control" TabIndex="31" Width="100%"></asp:DropDownList></td>
                                                            <td style="text-align: right">
                                                                <asp:ImageButton ID="ImgAddCT" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" TabIndex="32" OnClick="ImgAddCT_Click" /></td>
                                                            <td style="text-align: right">
                                                                <asp:ImageButton ID="ImgModCT" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" Visible="False" TabIndex="33" OnClick="ImgModCT_Click" /></td>
                                                            <td style="text-align: center">
                                                                <asp:ImageButton ID="ImgDelCT" runat="server" Height="25px" ImageUrl="~/Botones/eliminar.png" Visible="False" TabIndex="34" OnClick="ImgDelCT_Click" OnClientClick="return asegurar();" /></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Observación:</h5>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtObservaCT" runat="server" CssClass="upperCase" TabIndex="35" Width="100%" MaxLength="250" Height="50px" onkeydown="return (event.keyCode!=13);" TextMode="MultiLine"></asp:TextBox></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel5" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:Panel ID="Pnl7" runat="server" Height="180px" ScrollBars="Vertical" Visible="False">
                                                                    <asp:GridView ID="GrdvCambioTurno" runat="server"
                                                                        AutoGenerateColumns="False"
                                                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                                        DataKeyNames="Codigo,CodigoGestorCT,FirmaCodigo" ForeColor="#333333" PageSize="5"
                                                                        TabIndex="36" Width="100%" OnRowDataBound="GrdvCambioTurno_RowDataBound">
                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Gestor" HeaderText="Colaborador">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="TurnoActual" HeaderText="TurnoActual">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="TurnoNuevo" HeaderText="TurnoNuevo">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Observacion" HeaderText="Observación">
                                                                            <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Firma" HeaderText="Firma">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Seleccionar">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImgSeleccCT" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSeleccCT_Click" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" Font-Size="X-Small" ForeColor="White" />
                                                                        <RowStyle Font-Size="X-Small" />
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="TabVarios" runat="server" HeaderText="Varios">
                                                <HeaderTemplate>Varios</HeaderTemplate>
                                                <ContentTemplate>
                                                    <table style="border: 2px solid #0000FF; width: 100%">
                                                        <tr>
                                                            <td style="width: 5%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 25%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 15%"></td>
                                                            <td style="width: 30%"></td>
                                                            <td style="width: 5%"></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel8" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td><h5>Descripción:</h5></td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtDescripVA" runat="server" CssClass="upperCase" MaxLength="150" TabIndex="37" Width="100%"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Observación:</h5>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtObservaVA" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="upperCase" MaxLength="250" TabIndex="38" Width="100%" TextMode="MultiLine" Height="50px"></asp:TextBox></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgAddVA" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAddVA_Click" TabIndex="39" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgModVA" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModVA_Click" TabIndex="40" Visible="False" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgDelVA" runat="server" Height="25px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgDelVA_Click" TabIndex="41" Visible="False" />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel4" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:Panel ID="Pnl8" runat="server" Height="180px" ScrollBars="Vertical" Visible="False">
                                                                    <asp:GridView ID="GrdvVarios" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,CodigoGestorVA,FirmaCodigo" ForeColor="#333333" PageSize="5" TabIndex="42" Width="100%" OnRowDataBound="GrdvVarios_RowDataBound">
                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Gestor" HeaderText="Colaborador">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Observacion" HeaderText="Observación">
                                                                            <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Firma" HeaderText="Firma">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Seleccionar">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImgSeleccVA" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSeleccVA_Click" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" Font-Size="X-Small" ForeColor="White" />
                                                                        <RowStyle Font-Size="X-Small" />
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="TabNovedades" runat="server" HeaderText="Novedades">
                                                <HeaderTemplate>Novedades</HeaderTemplate>
                                                <ContentTemplate>
                                                    <table style="border: 2px solid #0000FF; width: 100%">
                                                        <tr>
                                                            <td style="width: 5%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 25%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 15%"></td>
                                                            <td style="width: 30%"></td>
                                                            <td style="width: 5%"></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel9" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td><h5>Descripción:</h5></td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtDescripNV" runat="server" CssClass="upperCase" MaxLength="150" TabIndex="43" Width="100%"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Observación:</h5>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtObservaNV" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="upperCase" MaxLength="250" TabIndex="44" Width="100%" TextMode="MultiLine" Height="50px"></asp:TextBox></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgAddNV" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" TabIndex="45" OnClick="ImgAddNV_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgModNV" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" TabIndex="46" Visible="False" OnClick="ImgModNV_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgDelNV" runat="server" Height="25px" ImageUrl="~/Botones/eliminar.png" TabIndex="47" Visible="False" OnClick="ImgDelNV_Click" OnClientClick="return asegurar();" />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel10" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:Panel ID="Pnl9" runat="server" Height="180px" ScrollBars="Vertical" Visible="False">
                                                                    <asp:GridView ID="GrdvNovedad" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,FirmaCodigo" ForeColor="#333333" PageSize="5" TabIndex="48" Width="100%" OnRowDataBound="GrdvNovedad_RowDataBound">
                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Observacion" HeaderText="Observación">
                                                                            <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Firma" HeaderText="Firma">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Seleccionar">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImgSeleccNV" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSeleccNV_Click" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" Font-Size="X-Small" ForeColor="White" />
                                                                        <RowStyle Font-Size="X-Small" />
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="TabRefuerzos" runat="server" HeaderText="Refuerzos">
                                                <HeaderTemplate>Refuerzos</HeaderTemplate>
                                                <ContentTemplate>
                                                    <table style="border: 2px solid #0000FF; width: 100%">
                                                        <tr>
                                                            <td style="width: 5%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 25%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 15%"></td>
                                                            <td style="width: 30%"></td>
                                                            <td style="width: 5%"></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel11" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td><h5>Descripción:</h5></td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtDescripRE" runat="server" CssClass="upperCase" MaxLength="150" TabIndex="49" Width="100%"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Observación:</h5>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtObservaRE" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="upperCase" MaxLength="250" TabIndex="50" Width="100%" TextMode="MultiLine" Height="50px"></asp:TextBox></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgAddRE" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAddRE_Click" TabIndex="51" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgModRE" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModRE_Click" TabIndex="52" Visible="False" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgDelRE" runat="server" Height="25px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgDelRE_Click" TabIndex="53" Visible="False" OnClientClick="return asegurar();" />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel12" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:Panel ID="Pnl10" runat="server" Height="180px" ScrollBars="Vertical" Visible="False">
                                                                    <asp:GridView ID="GrdvRefuerzo" runat="server" AutoGenerateColumns="False"
                                                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                                        DataKeyNames="Codigo,CodigoGestorRE,FirmaCodigo" ForeColor="#333333" PageSize="5" TabIndex="54" Width="100%" OnRowDataBound="GrdvRefuerzo_RowDataBound">
                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Gestor" HeaderText="Colaborador">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Observacion" HeaderText="Observación">
                                                                            <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Firma" HeaderText="Firma">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Seleccionar">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImgSeleccRE" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSeleccRE_Click" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" Font-Size="X-Small" ForeColor="White" />
                                                                        <RowStyle Font-Size="X-Small" />
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="TabGestion" runat="server" HeaderText="Gestion Terreno">
                                                <HeaderTemplate>Gestión Terreno</HeaderTemplate>
                                                <ContentTemplate>
                                                    <table style="border: 2px solid #0000FF; width: 100%">
                                                        <tr>
                                                            <td style="width: 5%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 25%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 15%"></td>
                                                            <td style="width: 30%"></td>
                                                            <td style="width: 5%"></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel13" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td><h5>Descripción:</h5></td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtDescripGT" runat="server" CssClass="upperCase" MaxLength="150" TabIndex="55" Width="100%"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Observación:</h5>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtObservaGT" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="upperCase" MaxLength="250" TabIndex="56" Width="100%" TextMode="MultiLine" Height="50px"></asp:TextBox></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgAddGT" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAddGT_Click" TabIndex="57" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgModGT" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModGT_Click" TabIndex="58" Visible="False" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgDelGT" runat="server" Height="25px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgDelGT_Click" TabIndex="59" Visible="False" OnClientClick="return asegurar();" />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel14" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:Panel ID="Pnl11" runat="server" Height="180px" ScrollBars="Vertical" Visible="False">
                                                                    <asp:GridView ID="GrdvTerreno" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,FirmaCodigo" ForeColor="#333333" PageSize="5" TabIndex="60" Width="100%" OnRowDataBound="GrdvTerreno_RowDataBound">
                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Observacion" HeaderText="Observación">
                                                                            <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Firma" HeaderText="Firma">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Seleccionar">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImgSeleccGT" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSeleccGT_Click" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" Font-Size="X-Small" ForeColor="White" />
                                                                        <RowStyle Font-Size="X-Small" />
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="TabSistemas" runat="server" HeaderText="Novedad Sistemas">
                                                <HeaderTemplate>Novedad Sistemas</HeaderTemplate>
                                                <ContentTemplate>
                                                    <table style="border: 2px solid #0000FF; width: 100%">
                                                        <tr>
                                                            <td style="width: 5%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 25%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 15%"></td>
                                                            <td style="width: 30%"></td>
                                                            <td style="width: 5%"></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel15" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td><h5>Descripción:</h5></td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtDescripNS" runat="server" CssClass="upperCase" MaxLength="150" TabIndex="61" Width="100%"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Observación:</h5>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtObservaNS" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="upperCase" MaxLength="250" TabIndex="62" Width="100%" TextMode="MultiLine" Height="50px"></asp:TextBox></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgAddNS" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAddNS_Click" TabIndex="63" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgModNS" runat="server" Height="20px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModNS_Click" TabIndex="64" Visible="False" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgDelNS" runat="server" Height="15px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgDelNS_Click" TabIndex="65" Visible="False" OnClientClick="return asegurar();" />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel16" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:Panel ID="Pnl12" runat="server" Height="180px" ScrollBars="Vertical" Visible="False">
                                                                    <asp:GridView ID="GrdvSistemas" runat="server" AutoGenerateColumns="False"
                                                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                                        DataKeyNames="Codigo,FirmaCodigo" ForeColor="#333333" PageSize="5" TabIndex="66" Width="100%" OnRowDataBound="GrdvSistemas_RowDataBound">
                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Observacion" HeaderText="Observación">
                                                                            <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Firma" HeaderText="Firma">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Seleccionar">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImgSeleccNS" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSeleccNS_Click" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" Font-Size="X-Small" ForeColor="White" />
                                                                        <RowStyle Font-Size="X-Small" />
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="TabClientes" runat="server" HeaderText="Clientes-Pagos">
                                                <HeaderTemplate>Clientes-Pagos</HeaderTemplate>
                                                <ContentTemplate>
                                                    <table style="border: 2px solid #0000FF; width: 100%">
                                                        <tr>
                                                            <td style="width: 5%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 25%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 15%"></td>
                                                            <td style="width: 30%"></td>
                                                            <td style="width: 5%"></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel17" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td><h5>Descripción:</h5></td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtDescripCP" runat="server" CssClass="upperCase" MaxLength="150" TabIndex="67" Width="100%"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Observación:</h5>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtObservaCP" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="upperCase" MaxLength="250" TabIndex="68" Width="100%" TextMode="MultiLine" Height="50px"></asp:TextBox></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgAddCP" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAddCP_Click" TabIndex="69" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgModCP" runat="server" Height="20px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModCP_Click" TabIndex="70" Visible="False" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgDelCP" runat="server" Height="15px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgDelCP_Click" TabIndex="71" Visible="False" OnClientClick="return asegurar();" />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel18" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:Panel ID="Pnl13" runat="server" Height="180px" ScrollBars="Vertical" Visible="False">
                                                                    <asp:GridView ID="GrdvPagos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,FirmaCodigo" ForeColor="#333333" PageSize="5" TabIndex="72" Width="100%" OnRowDataBound="GrdvPagos_RowDataBound">
                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Observacion" HeaderText="Observación">
                                                                            <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Firma" HeaderText="Firma">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Seleccionar">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImgSeleccCP" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSeleccCP_Click" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" Font-Size="X-Small" ForeColor="White" />
                                                                        <RowStyle Font-Size="X-Small" />
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="TabAdicionales" runat="server" HeaderText="Adicionales">
                                                <HeaderTemplate>Adicionales</HeaderTemplate>
                                                <ContentTemplate>
                                                    <table style="border: 2px solid #0000FF; width: 100%">
                                                        <tr>
                                                            <td style="width: 5%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 25%"></td>
                                                            <td style="width: 10%"></td>
                                                            <td style="width: 15%"></td>
                                                            <td style="width: 30%"></td>
                                                            <td style="width: 5%"></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel19" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td><h5>Descripción:</h5></td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtDescripAD" runat="server" CssClass="upperCase" MaxLength="150" TabIndex="73" Width="100%"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <h5>Observación:</h5>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="TxtObservaAD" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="upperCase" MaxLength="250" TabIndex="74" Width="100%" TextMode="MultiLine" Height="50px"></asp:TextBox></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgAddAD" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAddAD_Click" TabIndex="75" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgModAD" runat="server" Height="20px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModAD_Click" TabIndex="76" Visible="False" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgDelAD" runat="server" Height="15px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgDelAD_Click" TabIndex="77" Visible="False" OnClientClick="return asegurar();" />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel20" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:Panel ID="Pnl14" runat="server" Height="180px" ScrollBars="Vertical" Visible="False">
                                                                    <asp:GridView ID="GrdvAdicionales" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,FirmaCodigo" ForeColor="#333333" PageSize="5" TabIndex="78" Width="100%" OnRowDataBound="GrdvAdicionales_RowDataBound">
                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion">
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Observacion" HeaderText="Observación">
                                                                            <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Firma" HeaderText="Firma">
                                                                            <HeaderStyle CssClass="GVFixedHeader" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Seleccionar">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImgSeleccAD" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSeleccAD_Click" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle CssClass="GVFixedHeader" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" Font-Size="X-Small" ForeColor="White" />
                                                                        <RowStyle Font-Size="X-Small" />
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                        </asp:TabContainer>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div>
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:Panel ID="PnlDiv3" Height="30px" runat="server"></asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="panel panel-default">
                    <asp:UpdatePanel ID="UpdBotones" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 45%">
                                        <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" TabIndex="79" OnClick="BtnGrabar_Click" />
                                    </td>
                                    <td style="width: 10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="80" OnClick="BtnSalir_Click" />
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
