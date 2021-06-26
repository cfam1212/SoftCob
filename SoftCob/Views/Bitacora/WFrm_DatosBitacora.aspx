<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_DatosBitacora.aspx.cs" Inherits="SoftCob.Views.Bitacora.WFrm_DatosBitacora" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
                                        AutoGenerateColumns="False" PageSize="5" TabIndex="1" DataKeyNames="Codigo">
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
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 25%"></td>
                                    <td style="width: 25%"></td>
                                    <td style="width: 25%"></td>
                                    <td style="width: 25%"></td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:TabContainer ID="TabDatosBitacora" runat="server" Width="100%" Height="400px" ActiveTabIndex="0">
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
                                                                        DataKeyNames="Codigo,CodigoGestorAT,FirmaCodigo" ForeColor="#333333" PageSize="5" TabIndex="9" Width="100%">
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
                                                                        TabIndex="15" Width="100%">
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
                                                                        TabIndex="21" Width="100%">
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
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel7" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:Panel ID="Pnl6" runat="server" Height="180px" ScrollBars="Vertical" Visible="False">
                                                                    <asp:GridView ID="GrdvPermisos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,CodigoGestorPE,FirmaCodigo" ForeColor="#333333" PageSize="5" TabIndex="28" Width="100%">
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
                                                                        TabIndex="36" Width="100%">
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
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel4" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:Panel ID="Pnl8" runat="server" Height="180px" ScrollBars="Vertical" Visible="False">
                                                                    <asp:GridView ID="GrdvVarios" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,CodigoGestorVA,FirmaCodigo" ForeColor="#333333" PageSize="5" TabIndex="42" Width="100%">
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
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel10" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:Panel ID="Pnl9" runat="server" Height="180px" ScrollBars="Vertical" Visible="False">
                                                                    <asp:GridView ID="GrdvNovedad" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,FirmaCodigo" ForeColor="#333333" PageSize="5" TabIndex="48" Width="100%">
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
                                                                        DataKeyNames="Codigo,CodigoGestorRE,FirmaCodigo" ForeColor="#333333" PageSize="5" TabIndex="54" Width="100%">
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
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel14" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:Panel ID="Pnl11" runat="server" Height="180px" ScrollBars="Vertical" Visible="False">
                                                                    <asp:GridView ID="GrdvTerreno" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,FirmaCodigo" ForeColor="#333333" PageSize="5" TabIndex="60" Width="100%">
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
                                                                        DataKeyNames="Codigo,FirmaCodigo" ForeColor="#333333" PageSize="5" TabIndex="66" Width="100%">
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
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel18" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:Panel ID="Pnl13" runat="server" Height="180px" ScrollBars="Vertical" Visible="False">
                                                                    <asp:GridView ID="GrdvPagos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,FirmaCodigo" ForeColor="#333333" PageSize="5" TabIndex="72" Width="100%">
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
                                                            <td colspan="7">
                                                                <asp:Panel ID="Panel20" runat="server" Height="20px"></asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:Panel ID="Pnl14" runat="server" Height="180px" ScrollBars="Vertical" Visible="False">
                                                                    <asp:GridView ID="GrdvAdicionales" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,FirmaCodigo" ForeColor="#333333" PageSize="5" TabIndex="78" Width="100%">
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
                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:Panel ID="PnlDiv3" Height="30px" runat="server"></asp:Panel>
                        </td>
                    </tr>
                </table>
                <div class="panel panel-default">
                    <asp:UpdatePanel ID="UpdBotones" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 45%">&nbsp;</td>
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
