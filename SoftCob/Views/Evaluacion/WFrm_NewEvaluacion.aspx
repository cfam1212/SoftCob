<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_NewEvaluacion.aspx.cs" Inherits="SoftCob.Views.Evaluacion.WFrm_NewEvaluacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nueva Lista de Trabajo</title>
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
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div class="panel panel-primary">
            <div class="panel-heading" style="background-color: #79BBB8;">
                <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
            </div>            
            <asp:UpdatePanel ID="UpdError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdBotones">
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
                <asp:UpdatePanel ID="UpdCabecera" runat="server">
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
                                <td colspan="4">
                                    <asp:Panel ID="pnlDivision1" runat="server" Height="20px">
                                    </asp:Panel>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="4">
                                    <asp:Panel ID="PnlDatosEvaluacion" runat="server" Height="450px" GroupingText="Datos Evaluación" TabIndex="3">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 15%"></td>
                                                <td style="width: 35%"></td>
                                                <td style="width: 15%"></td>
                                                <td style="width: 35%"></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Evaluación:</h5>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TxtEvaluacion" runat="server" CssClass="upperCase" MaxLength="150" TabIndex="1" Width="100%"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <h5>Descripción:</h5>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TxtDescripcion" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="upperCase" Height="50px" MaxLength="250" TextMode="MultiLine" Width="100%" TabIndex="2"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Fecha Inicio:</h5>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TxtFechaInicio" runat="server" Width="100%" TabIndex="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <h5>Fecha Fin:</h5>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TxtFechaFin" runat="server" Width="100%" TabIndex="4"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5 runat="server" id="LblEstado" visible="false">Estado:</h5>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="ChkEstado" runat="server" AutoPostBack="True" Checked="True" OnCheckedChanged="ChkEstado_CheckedChanged" TabIndex="5" Visible="False" Text="Activo" />
                                                </td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <h5>Evaluación Departamentos</h5>
                                                </td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:GridView ID="GrdvDepartamentos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,Estado" ForeColor="#333333" PageSize="5" TabIndex="6" Width="100%" OnRowDataBound="GrdvDepartamentos_RowDataBound">
                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                        <Columns>
                                                            <asp:BoundField DataField="Descripcion" HeaderText="Departamento" />
                                                            <asp:TemplateField HeaderText="Evaluar">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="ChkEvaluar" runat="server" OnCheckedChanged="ChkEvaluar_CheckedChanged" AutoPostBack="True" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle Font-Size="Small" />
                                                        <RowStyle Font-Size="X-Small" />
                                                    </asp:GridView>
                                                </td>
                                                <td></td>
                                                <td></td>
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
                                <td colspan="6">
                                    <asp:Panel ID="pnlDivision" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="UpdBotones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right; width: 45%">
                                    <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" TabIndex="7" OnClick="BtnGrabar_Click" />
                                </td>
                                <td style="width: 10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="8" OnClick="BtnSalir_Click" />
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