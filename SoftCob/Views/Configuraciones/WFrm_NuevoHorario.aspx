<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_NuevoHorario.aspx.cs" Inherits="SoftCob.Views.Configuraciones.WFrm_NuevoHorario" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nuevo Horario</title>
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <style type="text/css">
        legend {
            color: darkblue;
            font-size: 12px;
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
            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanel1">
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
                            <td style="width: 10%"></td>
                            <td style="width: 15%">
                                <h5>Nombre Horario:</h5>
                            </td>
                            <td style="width: 60%" colspan="2">
                                <asp:TextBox ID="TxtHorario" runat="server" CssClass="form-control upperCase" MaxLength="50" Width="100%" TabIndex="1"></asp:TextBox>
                            </td>
                            <td style="width: 15%"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <h5>Descripción:</span></h5>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="TxtDescripcion" runat="server" onkeydown="return (event.keyCode!=13);" Width="100%" CssClass="form-control upperCase" MaxLength="150" Height="50px" TextMode="MultiLine" TabIndex="2"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <h5>Intervalo:</span></h5>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="DdlIntervalo" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" TabIndex="3" OnSelectedIndexChanged="DdlIntervalo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <h5>Hora Incio:</span></h5>
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlHoraIni" runat="server" CssClass="form-control" Width="100%" TabIndex="4">
                                </asp:DropDownList>

                            </td>
                            <td>
                                <asp:DropDownList ID="DdlMinutoIni" runat="server" CssClass="form-control" Width="100%" TabIndex="5">
                                </asp:DropDownList>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <h5>Hora Fin:</span></h5>
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlHoraFin" runat="server" CssClass="form-control" Width="100%" TabIndex="6">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlMinutoFin" runat="server" CssClass="form-control" Width="100%" TabIndex="7">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:ImageButton ID="ImgProcesar" runat="server" ImageUrl="~/Botones/procesarbg.png" OnClick="ImgProcesar_Click" Height="25px" TabIndex="8" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <h5 runat="server" id="Label7" visible="false">Estado:</h5>
                            </td>
                            <td colspan="2">
                                <asp:CheckBox ID="ChkEstado" runat="server" CssClass="form-control" AutoPostBack="True" Checked="True" OnCheckedChanged="ChkEstado_CheckedChanged" Text="Activo" Visible="False" TabIndex="9" />
                            </td>
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
                            <td></td>
                            <td colspan="3">
                                <asp:Panel ID="Panel1" runat="server" GroupingText="Horarios Generados" Height="250px"
                                    ScrollBars="Vertical" Visible="false">
                                    <asp:GridView ID="GrdvDatos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" Width="100%" TabIndex="10">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="HoraInicio" HeaderText="Hora_Inicio">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Hora_Fin" DataField="HoraFin">
                                                <ItemStyle HorizontalAlign="Center" />
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="panel panel-default">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: right; width: 45%">
                                <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="BtnGrabar_Click" TabIndex="11" />
                            </td>
                            <td style="width: 10%"></td>
                            <td style="text-align: left; width: 45%">
                                <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="12" />
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
