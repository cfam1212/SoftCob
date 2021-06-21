<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_FormularioEvaluacion.aspx.cs" Inherits="SoftCob.Views.Evaluacion.WFrm_FormularioEvaluacion" %>

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
                                <td style="width: 30%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 25%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Evaluación:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DddlEvaluacion" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="1" Width="100%" OnSelectedIndexChanged="DddlEvaluacion_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Panel ID="PnlDiv1" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <table style="border: 2px solid #008080; width: 100%" runat="server" id="TblComunicacion" visible="false">
                            <tr>
                                <td style="width: 60%"></td>
                                <td style="width: 2%"></td>
                                <td style="width: 38%"></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="PnlComunicacion" runat="server" Height="280px" GroupingText="Comunicación" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvEvaluacionCO" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="CodigoProtocolo,CodigoPadre,Calificacion" ForeColor="#333333" PageSize="5" TabIndex="2" Width="100%">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgSelecCO" runat="server" Height="20px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSelecCO_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Calificacion" HeaderText="Clf." />
                                                <asp:BoundField DataField="Evaluar" HeaderText="Evaluación" />
                                            </Columns>
                                            <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                            <RowStyle Font-Size="X-Small" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                                <td></td>
                                <td>
                                    <asp:Panel ID="PnlEvaluacionCO" runat="server" Height="280px" GroupingText="Departamentos" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvDepartamentoCO" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="CodigoDepartamento,Selecc" ForeColor="#333333" PageSize="5" TabIndex="2" Width="100%" OnRowDataBound="GrdvDepartamentoCO_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkDepCO" runat="server" OnCheckedChanged="ChkDepCO_CheckedChanged" AutoPostBack="True" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Departamento" HeaderText="Departamento" />
                                                <asp:BoundField DataField="Calificacion" HeaderText="Clf." />
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
                                <td colspan="3">
                                    <asp:Panel ID="PnlDiv0" runat="server" Height="50px"></asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <div>
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="PnlDiv20" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table style="border: 2px solid #0000FF; width: 100%" runat="server" id="TblLiderazgo" visible="false">
                            <tr>
                                <td style="width: 60%"></td>
                                <td style="width: 2%"></td>
                                <td style="width: 38%"></td>
                            </tr>
                            <tr>
                                <td colspan="3"></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="PnlLiderarzo" runat="server" Height="280px" GroupingText="Liderazgo" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvEvaluacionLI" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="CodigoProtocolo,CodigoPadre,Calificacion" ForeColor="#333333" PageSize="5" TabIndex="2" Width="100%">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgSelecLI" runat="server" Height="20px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSelecLI_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Calificacion" HeaderText="Clf." />
                                                <asp:BoundField DataField="Evaluar" HeaderText="Evaluación" />
                                            </Columns>
                                            <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                            <RowStyle Font-Size="X-Small" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                                <td></td>
                                <td>
                                    <asp:Panel ID="PnlEvaluacionLI" runat="server" Height="280px" GroupingText="Departamentos" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvDepartamentoLI" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="CodigoDepartamento,Selecc" ForeColor="#333333" PageSize="5" TabIndex="2" Width="100%" OnRowDataBound="GrdvDepartamentoLI_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkDepLI" runat="server" OnCheckedChanged="ChkDepLI_CheckedChanged" AutoPostBack="True" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Departamento" HeaderText="Departamento" />
                                                <asp:BoundField DataField="Calificacion" HeaderText="Clf." />
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
                                <td colspan="3">
                                    <asp:Panel ID="PnlDiv2" runat="server" Height="50px"></asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <div>
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="PnlDiv21" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table style="border: 2px solid #800000; width: 100%" runat="server" id="TblMotivacion" visible="false">
                            <tr>
                                <td style="width: 60%"></td>
                                <td style="width: 2%"></td>
                                <td style="width: 38%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="PnlMotivacion" runat="server" Height="280px" GroupingText="Motivación" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvEvaluacionMO" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="CodigoProtocolo,CodigoPadre,Calificacion" ForeColor="#333333" PageSize="5" TabIndex="2" Width="100%">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgSelecMO" runat="server" Height="20px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSelecMO_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Calificacion" HeaderText="Clf." />
                                                <asp:BoundField DataField="Evaluar" HeaderText="Evaluación" />
                                            </Columns>
                                            <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                            <RowStyle Font-Size="X-Small" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                                <td></td>
                                <td>
                                    <asp:Panel ID="PnlEvaluacionMO" runat="server" Height="280px" GroupingText="Departamentos" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvDepartamentoMO" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="CodigoDepartamento,Selecc" ForeColor="#333333" PageSize="5" TabIndex="2" Width="100%" OnRowDataBound="GrdvDepartamentoMO_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkDepMO" runat="server" OnCheckedChanged="ChkDepMO_CheckedChanged" AutoPostBack="True" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Departamento" HeaderText="Departamento" />
                                                <asp:BoundField DataField="Calificacion" HeaderText="Clf." />
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
                                <td colspan="3">
                                    <asp:Panel ID="PnlDiv3" runat="server" Height="50px"></asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <div>
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="PnlDiv22" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table style="border: 2px solid #808000; width: 100%" runat="server" id="TblActitud" visible="false">
                            <tr>
                                <td style="width: 60%"></td>
                                <td style="width: 2%"></td>
                                <td style="width: 38%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="PnlActitud" runat="server" Height="280px" GroupingText="Actitud y Colaboración" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvEvaluacionAC" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="CodigoProtocolo,CodigoPadre,Calificacion" ForeColor="#333333" PageSize="5" TabIndex="2" Width="100%">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgSelecAC" runat="server" Height="20px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSelecAC_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Calificacion" HeaderText="Clf." />
                                                <asp:BoundField DataField="Evaluar" HeaderText="Evaluación" />
                                            </Columns>
                                            <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                            <RowStyle Font-Size="X-Small" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                                <td></td>
                                <td>
                                    <asp:Panel ID="PnlEvaluacionAC" runat="server" Height="280px" GroupingText="Departamentos" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvDepartamentoAC" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="CodigoDepartamento,Selecc" ForeColor="#333333" PageSize="5" TabIndex="2" Width="100%" OnRowDataBound="GrdvDepartamentoAC_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkDepAC" runat="server" OnCheckedChanged="ChkDepAC_CheckedChanged" AutoPostBack="True" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Departamento" HeaderText="Departamento" />
                                                <asp:BoundField DataField="Calificacion" HeaderText="Clf." />
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
                                <td colspan="3">
                                    <asp:Panel ID="PnlDiv4" runat="server" Height="50px"></asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <div>
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="PnlDiv26" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table style="border: 2px solid #996600; width: 100%" runat="server" id="TblSolucion" visible="false">
                            <tr>
                                <td style="width: 60%"></td>
                                <td style="width: 2%"></td>
                                <td style="width: 38%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="PnlSolucionP" runat="server" Height="280px" GroupingText="Solución Problemas" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvEvaluacionSP" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="CodigoProtocolo,CodigoPadre,Calificacion" ForeColor="#333333" PageSize="5" TabIndex="2" Width="100%">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgSelecSP" runat="server" Height="20px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSelecSP_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Calificacion" HeaderText="Clf." />
                                                <asp:BoundField DataField="Evaluar" HeaderText="Evaluación" />
                                            </Columns>
                                            <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                            <RowStyle Font-Size="X-Small" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                                <td></td>
                                <td>
                                    <asp:Panel ID="PnlEvaluacionSP" runat="server" Height="280px" GroupingText="Departamentos" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvDepartamentoSP" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="CodigoDepartamento,Selecc" ForeColor="#333333" PageSize="5" TabIndex="2" Width="100%" OnRowDataBound="GrdvDepartamentoSP_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkDepSP" runat="server" OnCheckedChanged="ChkDepSP_CheckedChanged" AutoPostBack="True" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Departamento" HeaderText="Departamento" />
                                                <asp:BoundField DataField="Calificacion" HeaderText="Clf." />
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
                                <td colspan="3">
                                    <asp:Panel ID="PnlDiv5" runat="server" Height="50px"></asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <div>
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="PnlDiv23" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table style="border: 2px solid #666699; width: 100%" runat="server" id="TblAmbiente" visible="false">
                            <tr>
                                <td style="width: 60%"></td>
                                <td style="width: 2%"></td>
                                <td style="width: 38%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="PnlAmbienteT" runat="server" Height="280px" GroupingText="Abiente de Trabajo" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvEvaluacionAT" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="CodigoProtocolo,CodigoPadre,Calificacion" ForeColor="#333333" PageSize="5" TabIndex="2" Width="100%">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgSelecAT" runat="server" Height="20px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSelecAT_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Calificacion" HeaderText="Clf." />
                                                <asp:BoundField DataField="Evaluar" HeaderText="Evaluación" />
                                            </Columns>
                                            <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                            <RowStyle Font-Size="X-Small" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                                <td></td>
                                <td>
                                    <asp:Panel ID="PnlEvaluacionAT" runat="server" Height="280px" GroupingText="Departamentos" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvDepartamentoAT" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="CodigoDepartamento,Selecc" ForeColor="#333333" PageSize="5" TabIndex="2" Width="100%" OnRowDataBound="GrdvDepartamentoAT_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkDepAT" runat="server" OnCheckedChanged="ChkDepAT_CheckedChanged" AutoPostBack="True" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Departamento" HeaderText="Departamento" />
                                                <asp:BoundField DataField="Calificacion" HeaderText="Clf." />
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
                                <td colspan="3">
                                    <asp:Panel ID="PnlDiv6" runat="server" Height="50px"></asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <div>
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="PnlDiv24" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table style="border: 2px solid #669999; width: 100%" runat="server" id="TblCapacidad" visible="false">
                            <tr>
                                <td style="width: 60%"></td>
                                <td style="width: 2%"></td>
                                <td style="width: 38%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="PnlCapacidadT" runat="server" Height="280px" GroupingText="Capacidad Personal" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvCapacidadP" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="CodigoProtocolo,CodigoPadre,Calificacion" ForeColor="#333333" PageSize="5" TabIndex="2" Width="100%">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgSelecCP" runat="server" Height="20px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSelecCP_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Calificacion" HeaderText="Clf." />
                                                <asp:BoundField DataField="Evaluar" HeaderText="Evaluación" />
                                            </Columns>
                                            <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                            <RowStyle Font-Size="X-Small" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                                <td></td>
                                <td>
                                    <asp:Panel ID="PnlEvaluacionCP" runat="server" Height="280px" GroupingText="Departamentos" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvDepartamentoCP" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="CodigoDepartamento,Selecc" ForeColor="#333333" PageSize="5" TabIndex="2" Width="100%" OnRowDataBound="GrdvDepartamentoCP_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkDepCP" runat="server" OnCheckedChanged="ChkDepCP_CheckedChanged" AutoPostBack="True" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Departamento" HeaderText="Departamento" />
                                                <asp:BoundField DataField="Calificacion" HeaderText="Clf." />
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
                                <td colspan="3">
                                    <asp:Panel ID="PnlDiv7" runat="server" Height="50px"></asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <div>
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="PnlDiv25" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table style="border: 2px solid #663300; width: 100%" runat="server" id="TblCostos" visible="false">
                            <tr>
                                <td style="width: 60%"></td>
                                <td style="width: 2%"></td>
                                <td style="width: 38%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="PnlCostosP" runat="server" Height="280px" GroupingText="Costos y Productividad" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvCostoP" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="CodigoProtocolo,CodigoPadre,Calificacion" ForeColor="#333333" PageSize="5" TabIndex="2" Width="100%">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgSelecTP" runat="server" Height="20px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSelecTP_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Calificacion" HeaderText="Clf." />
                                                <asp:BoundField DataField="Evaluar" HeaderText="Evaluación" />
                                            </Columns>
                                            <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                            <RowStyle Font-Size="X-Small" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                                <td></td>
                                <td>
                                    <asp:Panel ID="PnlEvaluacionTP" runat="server" Height="280px" GroupingText="Departamentos" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvDepartamentoTP" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            DataKeyNames="CodigoDepartamento,Selecc" ForeColor="#333333" PageSize="5" TabIndex="2" Width="100%" OnRowDataBound="GrdvDepartamentoTP_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkDepTP" runat="server" OnCheckedChanged="ChkDepTP_CheckedChanged" AutoPostBack="True" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Departamento" HeaderText="Departamento" />
                                                <asp:BoundField DataField="Calificacion" HeaderText="Clf." />
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
                                <td colspan="3">
                                    <asp:Panel ID="PnlDiv8" runat="server" Height="30px"></asp:Panel>
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
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="4"></td>
                                <td></td>
                            </tr>
                        </table>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right; width: 45%">
                                    <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" TabIndex="18" OnClick="BtnGrabar_Click" />
                                </td>
                                <td style="width: 10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="19" OnClick="BtnSalir_Click" />
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
