<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_RegLlamadaEntrante.aspx.cs" Inherits="SoftCob.Views.Gestion.WFrm_RegLlamadaEntrante" %>

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
                $('#TxtFechaPago').datepicker(
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
                $('#TxtFechaLLamar').datepicker(
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

        #divHeaderPerfil {
            position: static;
            top: auto;
            height: 20px;
            width: 220px;
            color: #006dcc;
            font-weight: bold;
            background-color: lightgray;
            text-align: center;
        }

        #divContentPerfil {
            height: 120px;
            overflow: auto;
            width: 220px;
            border-left: double;
            border-bottom: double;
            border-top: double;
            border-right: double;
            font-size: 10px;
            font-weight: bold;
        }

        #divHeaderEsti {
            position: static;
            top: auto;
            height: 20px;
            width: 220px;
            color: #006dcc;
            font-weight: bold;
            background-color: lightgray;
            text-align: center;
        }

        #divContentEsti {
            height: 120px;
            overflow: auto;
            width: 220px;
            border-left: double;
            border-bottom: double;
            border-top: double;
            border-right: double;
            font-size: 10px;
            font-weight: bold;
        }

        #divHeaderEstados {
            position: static;
            top: auto;
            height: 20px;
            width: 220px;
            color: #006dcc;
            font-weight: bold;
            background-color: lightgray;
            text-align: center;
        }

        #divContentEstados {
            height: 120px;
            overflow: auto;
            width: 220px;
            border-left: double;
            border-bottom: double;
            border-top: double;
            border-right: double;
            font-size: 10px;
            font-weight: bold;
        }

        #divHeaderMeta {
            /*position: absolute;*/
            position: static;
            top: auto;
            height: 20px;
            width: 220px;
            color: #006dcc;
            font-weight: bold;
            background-color: lightgray;
            text-align: center;
        }

        #divContentMeta {
            height: 120px;
            overflow: auto;
            width: 220px;
            border-left: double;
            border-bottom: double;
            border-top: double;
            border-right: double;
            font-size: 10px;
            font-weight: bold;
        }

        #divHeaderModali {
            position: static;
            top: auto;
            height: 20px;
            width: 220px;
            color: #006dcc;
            font-weight: bold;
            background-color: lightgray;
            text-align: center;
        }

        #divContentModali {
            height: 120px;
            overflow: auto;
            width: 220px;
            border-left: double;
            border-bottom: double;
            border-top: double;
            border-right: double;
            font-size: 10px;
            font-weight: bold;
        }

        #divHeaderImpul {
            position: static;
            top: auto;
            height: 20px;
            width: 220px;
            color: #006dcc;
            font-weight: bold;
            background-color: lightgray;
            text-align: center;
        }

        #divContentImpul {
            height: 120px;
            overflow: auto;
            width: 220px;
            border-left: double;
            border-bottom: double;
            border-top: double;
            border-right: double;
            font-size: 10px;
            font-weight: bold;
        }
    </style>
    <script>
        function asegurar() {
            rc = confirm("¿Seguro que desea Eliminar?");
            return rc;
        }
    </script>
    <script type="text/javascript">
        function ValidarDecimales() {
            var numero = document.getElementById("<%=TxtValorAbono.ClientID%>").value;
            if (!/^([0-9])*[.]?[0-9]*$/.test(numero)) {
                alert("El valor " + numero + " no es un número válido. Ejemplo 157.68");
                document.getElementById("<%=TxtValorAbono.ClientID%>").value = "";
                return false;
            }
        }

        function grabar() {
            var valor = document.getElementById("<%=TxtValorAbono.ClientID%>").value;
            if (valor > 1000) {
                resconf = confirm("Está Seguro que el Valor acordado es: " + valor);
                return resconf;
            }
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
                                <td style="width: 15%"></td>
                                <td style="width: 35%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <div class="panel panel-primary" runat="server" id="DivPresupuesto" visible="false">
                                        <div class="panel-body">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 100%">
                                                        <asp:Panel ID="PnlHeaderPresu" runat="server">
                                                            Presupuesto
                                                                        <asp:Image runat="server" ID="ImgPresu" ImageUrl="~/Botones/collapseopen.png" Height="20px" />
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="PnlPresupuesto" runat="server" CssClass="panel panel-primary" Height="450px">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="PnlPresuCompromiso" runat="server"
                                                                            CssClass="panel panel-primary" Height="220px"
                                                                            GroupingText="Presupuesto Compromisos de Pago"
                                                                            ScrollBars="Horizontal">
                                                                            <asp:GridView ID="GrdvBrenchGestor" runat="server" Width="100%"
                                                                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                                                EmptyDataText="No existen datos para mostrar" TabIndex="7"
                                                                                AutoGenerateColumns="False" PageSize="12" DataKeyNames="CodigoBRMC,PorCumplido"
                                                                                OnRowDataBound="GrdvBrenchGestor_RowDataBound">
                                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="Operaciones" HeaderText="Operaciones">
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Exigible" HeaderText="Exigible">
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Porcentaje" HeaderText="%" Visible="False">
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Presupuesto" HeaderText="Presupuesto">
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="ValCumplido" HeaderText="Valor Compromiso">
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="PorCumplido" HeaderText="%">
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:TemplateField HeaderText="Calificación">
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
                                                                    <td>
                                                                        <asp:Panel ID="Panel5" runat="server" Height="10px"></asp:Panel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="PnlBrenchPagos" runat="server" Height="220px" GroupingText="Presupuesto Pagos Realizados">
                                                                            <asp:GridView ID="GrdvBrenchPago" runat="server" Width="100%"
                                                                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                                                EmptyDataText="No existen datos para mostrar" TabIndex="7"
                                                                                AutoGenerateColumns="False" PageSize="12" DataKeyNames="CodigoGEST,CodigoBRMC,PorCumplido"
                                                                                OnRowDataBound="GrdvBrenchPago_RowDataBound">
                                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="Operaciones" HeaderText="Operaciones">
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Exigible" HeaderText="Exigible">
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Porcentaje" HeaderText="%" Visible="False">
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Presupuesto" HeaderText="Presupuesto">
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="ValCumplido" HeaderText="Pagos Registrados">
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="PorCumplido" HeaderText="%">
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:TemplateField HeaderText="Calificación">
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
                                                            </table>
                                                        </asp:Panel>
                                                        <asp:CollapsiblePanelExtender ID="PnlPresupuesto_CollapsiblePanelExtender"
                                                            runat="server" Enabled="True" TargetControlID="PnlPresupuesto"
                                                            ExpandControlID="PnlHeaderPresu" CollapsedImage="~/Botones/collapseopen.png"
                                                            ExpandedImage="~/Botones/collapseclose.png" CollapsedText="Mostrando Presupuesto..."
                                                            ImageControlID="ImgPresu" SuppressPostBack="True" Collapsed="True"
                                                            CollapseControlID="PnlHeaderPresu" ExpandedText="Ocultar Presupuesto">
                                                        </asp:CollapsiblePanelExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>

                                    <div class="panel panel-primary" runat="server" id="DivDatosDeudor">
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="PnlHeaderOperacion" runat="server">
                                                        Datos Deudor-Operacion
                                                            <asp:Image runat="server" ID="ImgCollapseDeudor" ImageUrl="~/Botones/collapseopen.png" Height="20px" />
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="PnlDatosOperacion" runat="server" CssClass="panel panel-primary"
                                                        Height="710px">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 100%"></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel ID="pnlDatosDeudor" runat="server" CssClass="panel panel-primary"
                                                                        Height="220px" GroupingText="Datos Deudor" TabIndex="1">
                                                                        <asp:GridView ID="GrdvDatosDeudor" runat="server" AutoGenerateColumns="False"
                                                                            CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333"
                                                                            PageSize="3" TabIndex="2" Width="100%" DataKeyNames="Existe" OnRowDataBound="GrdvDatosDeudor_RowDataBound">
                                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="Cedula" HeaderText="Cédula" />
                                                                                <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                                                                                <asp:BoundField DataField="Provincia" HeaderText="Provincia" />
                                                                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                                                                <asp:BoundField DataField="Edad" HeaderText="Edad" />
                                                                                <asp:TemplateField HeaderText="Actualizar">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="Imgupdate" runat="server" Height="20px" ImageUrl="~/Botones/modificar.png" OnClick="Imgupdate_Click" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Teléfono">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="ImgPhoneD" runat="server" Height="20px" ImageUrl="~/Botones/Buscargris.png" OnClick="ImgPhoneD_Click" />
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
                                                                <td>
                                                                    <asp:Panel ID="pnlDatosObligacion" runat="server" Height="220px"
                                                                        CssClass="panel panel-primary" ScrollBars="Vertical"
                                                                        GroupingText="Datos Obligación" TabIndex="3">
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:GridView ID="GrdvDatos" runat="server" AutoGenerateColumns="False"
                                                                                        CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333" PageSize="5" TabIndex="4"
                                                                                        Width="100%" EmptyDataText="No Existen Registros" OnRowDataBound="GrdvDatos_RowDataBound" DataKeyNames="DiasMora,Operacion" ShowFooter="True">
                                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                                                                            <asp:BoundField DataField="Operacion" HeaderText="Operación">
                                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField HeaderText="Código" DataField="Codigo">
                                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField HeaderText="Grupo" DataField="Grupo">
                                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="Campa" HeaderText="Camp.">
                                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="HDiasMora" HeaderText="H.Mora">
                                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="MontoOriginal" HeaderText="Cupo">
                                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField HeaderText="ValorIni" DataField="MontoGSPBO">
                                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="CVencido" HeaderText="C.Vencido">
                                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField HeaderText="Exigible" DataField="Exigible">
                                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                            </asp:BoundField>
                                                                                            <asp:TemplateField HeaderText="Inf.">
                                                                                                <ItemTemplate>
                                                                                                    <asp:ImageButton ID="ImgInformacion" runat="server" Height="20px" ImageUrl="~/Botones/informacion.png" OnClick="ImgInformacion_Click" />
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
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel ID="PnlDatosGarante" runat="server" CssClass="panel panel-primary" Height="240px" ScrollBars="Vertical"
                                                                        GroupingText="Datos Garante-CoDeudor" TabIndex="15" Visible="False">
                                                                        <asp:GridView ID="GrdvDatosGarante" runat="server" AutoGenerateColumns="False"
                                                                            CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333"
                                                                            PageSize="3" TabIndex="16" Width="100%" DataKeyNames="CedulaGarante,Existe,CodigoGARA,Operacion" OnRowDataBound="GrdvDatosGarante_RowDataBound">
                                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="CedulaGarante" HeaderText="Cédula" />
                                                                                <asp:BoundField DataField="Garante" HeaderText="Nombres" />
                                                                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                                                                <asp:BoundField DataField="Operacion" HeaderText="Operación" />
                                                                                <asp:TemplateField HeaderText="Actualizar">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="ImgEditGarante" runat="server" Height="20px" ImageUrl="~/Botones/modificar.png" OnClick="ImgEditGarante_Click" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Buscar">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="ImgBuscarAarbol" runat="server" Height="20px" ImageUrl="~/Botones/con_usuario.jpg" OnClick="ImgBuscarAarbol_Click" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Teléfono">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="ImgPhoneG" runat="server" Height="15px" ImageUrl="~/Botones/Buscargris.png" OnClick="ImgPhoneG_Click" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Datos">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="ImgEquifax" runat="server" Height="15px" ImageUrl="~/Botones/identidad.png" OnClick="ImgEquifax_Click" />
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
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:CollapsiblePanelExtender ID="PnlDatosOperacion_CollapsiblePanelExtender" runat="server"
                                                        CollapseControlID="PnlHeaderOperacion" Collapsed="True" CollapsedImage="~/Botones/collapseopen.png"
                                                        CollapsedText="Mostrando Deudor-Operación" Enabled="True" ExpandControlID="PnlHeaderOperacion"
                                                        ExpandedImage="~/Botones/collapseclose.png" ExpandedText="Ocultar Datos-Deudor"
                                                        ImageControlID="ImgCollapseDeudor" SuppressPostBack="True" TargetControlID="PnlDatosOperacion">
                                                    </asp:CollapsiblePanelExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlDivision2" runat="server" Height="20px">
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                    <div class="panel panel-primary" runat="server" id="DivRegistrarTele">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 100%">
                                                    <asp:Panel ID="PnlHeaderTelefonos" runat="server">
                                                        Datos Teléfonos
                                                            <asp:Image runat="server" ID="ImgCollapsePhone" ImageUrl="~/Botones/collapseopen.png" Height="20px" />
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="PnlDatosTelefonos" runat="server" CssClass="panel panel-primary" Height="490px">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel ID="PnlOpcionTelefonos" runat="server" Height="250px" GroupingText="Datos Teléfonos" TabIndex="6">
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 15%"></td>
                                                                                <td style="width: 30%"></td>
                                                                                <td style="width: 15%"></td>
                                                                                <td style="width: 40%"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <h5>Número:</h5>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="TxtTelefono" runat="server" CssClass="form-control" MaxLength="10" TabIndex="7" Width="100%"></asp:TextBox>
                                                                                    <asp:FilteredTextBoxExtender ID="txtTelefono_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtTelefono">
                                                                                    </asp:FilteredTextBoxExtender>
                                                                                </td>
                                                                                <td>
                                                                                    <h5>Tipo Teléfono:</h5>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="DdlTipTelefono" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="8" Width="100%" OnSelectedIndexChanged="DdlTipTelefono_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <h5>Prefijo:</h5>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="DdlPrefijo" runat="server" CssClass="form-control" TabIndex="9" Width="100%">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td>
                                                                                    <h5>Propietario:</h5>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="DdlPropietario2" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlPropietario2_SelectedIndexChanged" TabIndex="10" Width="100%">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <h5>Nombres:</h5>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="TxtNombres" runat="server" CssClass="form-control upperCase" MaxLength="80" TabIndex="11" Width="100%" Enabled="False"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <h5>Apellidos:</h5>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="TxtApellidos" runat="server" CssClass="form-control upperCase" MaxLength="80" TabIndex="12" Width="100%" Enabled="False"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <h5>Acción:</h5>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="DdlAccionDel" runat="server" CssClass="form-control" OnSelectedIndexChanged="DdlAccionDel_SelectedIndexChanged" TabIndex="13" Width="100%">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td>
                                                                                    <h5>Respuesta:</h5>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="DdlRespuestaDel" runat="server" CssClass="form-control" TabIndex="14" Width="100%">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="4">
                                                                                    <asp:Panel ID="Panel3" runat="server" Height="20px"></asp:Panel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td style="text-align: center">
                                                                                    <asp:ImageButton ID="ImgAddTelefono" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAddTelefono_Click" TabIndex="15" ToolTip="Agregar Teléfono" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="ImgEditelefono" runat="server" Enabled="False" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgEditelefono_Click" TabIndex="16" ToolTip="Editar Teléfono" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="ImgBuscarFono" runat="server" Height="25px" ImageUrl="~/Botones/bntbuscarfono.png" OnClick="ImgBuscarFono_Click" ToolTip="Buscar Teléfonos" Visible="False" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel ID="pnlTelefonos" runat="server" Height="200px" ScrollBars="Vertical" GroupingText="Teléfonos" TabIndex="17">
                                                                        <asp:GridView ID="GrdvTelefonos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,Telefono,Prefijo,CodTipo,CodPro,Nuevo,Score,Origen,Modif" ForeColor="#333333" PageSize="5" TabIndex="18" Width="100%" OnRowDataBound="GrdvTelefonos_RowDataBound">
                                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="Prefijo" HeaderText="Prefijo">
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                                                                                <asp:BoundField DataField="Propietario" HeaderText="Propietario" />
                                                                                <asp:BoundField DataField="NomApe" HeaderText="Nombres" />
                                                                                <asp:BoundField DataField="Origen" HeaderText="Origen" />
                                                                                <asp:BoundField DataField="Modif" HeaderText="Modificado">
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>
                                                                                <asp:TemplateField HeaderText="Selecc">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="ImgSelectT" runat="server" Height="20px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSelectT_Click" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Del">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="ImgEliminar" runat="server" Height="20px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgEliminar_Click" OnClientClick="return asegurar();" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Call">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="ImgCall" runat="server" ImageUrl="~/Botones/call_small.png" OnClick="ImgCall_Click" Height="20px" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Efec">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="ChkTelEfec" runat="server" AutoPostBack="True" OnCheckedChanged="ChkTelEfec_CheckedChanged" />
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
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:CollapsiblePanelExtender ID="PnlDatosTelefonos_CollapsiblePanelExtender" runat="server"
                                                        CollapseControlID="PnlHeaderTelefonos" Collapsed="True" CollapsedImage="~/Botones/collapseopen.png"
                                                        CollapsedText="Teléfono Registrados.." Enabled="True" ExpandControlID="PnlHeaderTelefonos"
                                                        ExpandedImage="~/Botones/collapseclose.png" ImageControlID="ImgCollapsePhone" SuppressPostBack="True"
                                                        TargetControlID="PnlDatosTelefonos">
                                                    </asp:CollapsiblePanelExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                    <div class="panel panel-primary" runat="server" id="DivBotones">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 15%"></td>
                                                <td style="width: 35%"></td>
                                                <td style="width: 15%"></td>
                                                <td style="width: 17%"></td>
                                                <td style="width: 18%"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Panel ID="PnlHeaderBotones" runat="server">
                                                        Opción Botones
                                                            <asp:Image runat="server" ID="ImgCollapseBotones" ImageUrl="~/Botones/collapseopen.png" Height="20px" />
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Panel ID="PnlBotones" runat="server" Height="140px" GroupingText="Opción Botones">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 15%"></td>
                                                                <td style="width: 10%"></td>
                                                                <td style="width: 10%"></td>
                                                                <td style="width: 10%"></td>
                                                                <td style="width: 10%"></td>
                                                                <td style="width: 10%"></td>
                                                                <td style="width: 10%"></td>
                                                                <td style="width: 10%"></td>
                                                                <td style="width: 15%"></td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td style="text-align: center">
                                                                    <asp:ImageButton ID="ImgArbolGen" runat="server" Height="30px" ImageUrl="~/Botones/con_usuario.jpg" OnClick="ImgArbolGen_Click" TabIndex="23" ToolTip="Árbol Genealógico" />
                                                                </td>
                                                                <td style="text-align: center">
                                                                    <asp:ImageButton ID="ImgSpeechBV" runat="server" Height="30px" ImageUrl="~/Botones/speechBV.png" OnClick="ImgSpeechBV_Click" TabIndex="20" ToolTip="Speech de BienVenida" />
                                                                </td>
                                                                <td style="text-align: center">
                                                                    <asp:ImageButton ID="ImgSpeechAD" runat="server" Height="30px" ImageUrl="~/Botones/speechAB.png" OnClick="ImgSpeechAD_Click" TabIndex="21" ToolTip="Speech Árbol de Decisión" />
                                                                </td>
                                                                <td style="text-align: center">
                                                                    <asp:ImageButton ID="ImgNotas" runat="server" Height="30px" ImageUrl="~/Botones/notas.png" OnClick="ImgNotas_Click" TabIndex="22" ToolTip="Registro de Notas" />
                                                                </td>
                                                                <td style="text-align: center">
                                                                    <asp:ImageButton ID="ImgCitacion" runat="server" Height="30px" ImageUrl="~/Botones/btncitacion.png" OnClick="ImgCitacion_Click" TabIndex="23" Visible="False" ToolTip="Realizar Citación" />
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ID="ImgComparar" runat="server" Height="35px" ImageUrl="~/Botones/comparar.png" OnClick="ImgComparar_Click" TabIndex="23" ToolTip="Compararme con Todos" Visible="False" />
                                                                </td>
                                                                <td></td>
                                                                <td></td>
                                                            </tr>
                                                            <tr runat="server" id="TrArbol" visible="false">
                                                                <td></td>
                                                                <td></td>
                                                                <td>
                                                                    <h5>Cédula:</h5>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:TextBox ID="TxtCedula" runat="server" CssClass="form-control" MaxLength="20" TabIndex="33"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ID="ImgArbol" runat="server" Height="35px" ImageUrl="~/Botones/arbol.png" OnClick="ImgArbol_Click" TabIndex="23" ToolTip="Árbol Genealógico" />
                                                                </td>
                                                                <td></td>
                                                                <td></td>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:CollapsiblePanelExtender ID="PnlBotones_CollapsiblePanelExtender" runat="server" CollapseControlID="PnlHeaderBotones"
                                                        Collapsed="True" CollapsedImage="~/Botones/collapseopen.png" CollapsedText="Mostrando Botones..."
                                                        Enabled="True" ExpandControlID="PnlHeaderBotones" ExpandedImage="~/Botones/collapseclose.png"
                                                        ExpandedText="Ocultando Botones..." ImageControlID="ImgCollapseBotones" SuppressPostBack="True"
                                                        TargetControlID="PnlBotones">
                                                    </asp:CollapsiblePanelExtender>
                                                </td>
                                            </tr>
                                        </table>

                                    </div>

                                    <div class="panel panel-primary" runat="server" id="DivRegGestion">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 100%">
                                                    <asp:Panel ID="PnlHeaderRegGestion" runat="server">
                                                        Opción Registros-Gestión
                                                            <asp:Image runat="server" ID="ImgCollapseRegGestion" ImageUrl="~/Botones/collapseopen.png" Height="20px" />
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="PnlOpcionRegistros" runat="server" Height="310px" GroupingText="Opciones de Registro" TabIndex="19">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 15%"></td>
                                                                <td style="width: 35%"></td>
                                                                <td style="width: 15%"></td>
                                                                <td style="width: 17%"></td>
                                                                <td style="width: 18%"></td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chkcitacion" runat="server" CssClass="form-control" Text="Cancelar Citación" Visible="False" />
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:DropDownList ID="DdlCitacion" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="25" Width="100%" Visible="False">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="5">
                                                                    <asp:Panel ID="Panel4" runat="server" Height="20px"></asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <h5>Acción:</h5>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="DdlAccion" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlAccion_SelectedIndexChanged" TabIndex="23" Width="100%">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <h5>Efecto:</h5>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:DropDownList ID="DdlEfecto" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlEfecto_SelectedIndexChanged" TabIndex="24" Width="100%">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <h5>Respuesta:</h5>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="DdlRespuesta" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlRespuesta_SelectedIndexChanged" TabIndex="25" Width="100%">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <h5 runat="server" id="LblContacto">Contacto:</h5>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:DropDownList ID="DdlContacto" runat="server" CssClass="form-control" TabIndex="26" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlContacto_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <h5>Observación:</h5>
                                                                </td>
                                                                <td colspan="4">
                                                                    <asp:TextBox ID="TxtObservacion" runat="server" CssClass="form-control upperCase" Height="80px" MaxLength="500" onkeydown="return (event.keyCode!=13);" TabIndex="27" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel ID="Panel2" runat="server" Height="20px"></asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="ChkPerfilDeudor" runat="server" AutoPostBack="True" CssClass="form-control" Enabled="False" OnCheckedChanged="ChkPerfilDeudor_CheckedChanged" TabIndex="5" Text="Perfil Deudor" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:CollapsiblePanelExtender ID="PnlOpcionRegistros_CollapsiblePanelExtender" runat="server"
                                                        CollapseControlID="PnlHeaderRegGestion" Collapsed="True" CollapsedImage="~/Botones/collapseopen.png"
                                                        CollapsedText="Mostrando Opciones..." Enabled="True" ExpandControlID="PnlHeaderRegGestion"
                                                        ExpandedImage="~/Botones/collapseclose.png" ExpandedText="Ocultando Opciones..."
                                                        ImageControlID="ImgCollapseRegGestion" SuppressPostBack="True" TargetControlID="PnlOpcionRegistros">
                                                    </asp:CollapsiblePanelExtender>
                                                </td>
                                            </tr>

                                        </table>
                                    </div>

                                    <div class="panel panel-primary" runat="server" id="DivPerfiles" visible="false">
                                        <asp:Panel ID="pnlPerfilCalifica" runat="server">
                                            <table style="width: 100%">
                                                <tr>

                                                    <td style="width: 33%">
                                                        <div runat="server" id="divHeaderPerfil" visible="false">Perfil Actitudinal</div>
                                                        <div runat="server" id="divContentPerfil" visible="false">
                                                            <asp:PlaceHolder ID="pchPerfilActitudinal" runat="server"></asp:PlaceHolder>
                                                        </div>
                                                    </td>
                                                    <td style="width: 33%">
                                                        <div runat="server" id="divHeaderEsti" visible="false">Estilos de Negociación</div>
                                                        <div runat="server" id="divContentEsti" visible="false">
                                                            <asp:PlaceHolder ID="pchEstilosNegociacion" runat="server"></asp:PlaceHolder>
                                                        </div>
                                                    </td>
                                                    <td style="width: 33%">
                                                        <div runat="server" id="divHeaderEstados" visible="false">Estados del YO</div>
                                                        <div runat="server" id="divContentEstados" visible="false">
                                                            <asp:PlaceHolder ID="pchEstadosYo" runat="server"></asp:PlaceHolder>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Panel ID="Panel1" runat="server" Height="20px">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div runat="server" id="divHeaderMeta" visible="false">Metaprogramas</div>
                                                        <div runat="server" id="divContentMeta" visible="false">
                                                            <asp:PlaceHolder ID="pchMetaprogramas" runat="server"></asp:PlaceHolder>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divHeaderModali" visible="false">Modalidades</div>
                                                        <div runat="server" id="divContentModali" visible="false">
                                                            <asp:PlaceHolder ID="pchModalidades" runat="server"></asp:PlaceHolder>
                                                        </div>

                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divHeaderImpul" visible="false">Impulsores</div>
                                                        <div runat="server" id="divContentImpul" visible="false">
                                                            <asp:PlaceHolder ID="pchImpulsores" runat="server"></asp:PlaceHolder>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </div>

                                    <div class="panel panel-primary" runat="server" id="DivPagos" visible="False">
                                        <asp:Panel ID="PnlOpcionPago" runat="server" Height="150px" GroupingText="Registro Pagos/Abonos" TabIndex="28">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 15%"></td>
                                                    <td style="width: 30%"></td>
                                                    <td style="width: 15%"></td>
                                                    <td style="width: 40%"></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <h5>Fecha Pago:</h5>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtFechaPago" runat="server" CssClass="form-control" TabIndex="29" Width="100%"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <h5>Valor Pago:</h5>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtValorAbono" runat="server" CssClass="form-control alinearDerecha" MaxLength="7" TabIndex="30"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <h5>Tipo Pago:</h5>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DdlTipoPago" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlTipoPago_SelectedIndexChanged" TabIndex="31" Width="100%">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <h5>No.Documento:</h5>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtNumDocumento" runat="server" CssClass="form-control" MaxLength="20" TabIndex="32"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </div>

                                    <div class="panel panel-primary" runat="server" id="DivLLamar" visible="False">
                                        <asp:Panel ID="PnlOpcionLlamar" runat="server" Height="100px" GroupingText="Registro Volver a Llamar" TabIndex="33">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 15%"></td>
                                                    <td style="width: 30%"></td>
                                                    <td style="width: 15%"></td>
                                                    <td style="width: 40%"></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <h5>Fecha Llamar:</h5>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtFechaLLamar" runat="server" CssClass="form-control" TabIndex="34" Width="100%"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <h5>Hora Llamar:</h5>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtHoraLLamar" runat="server" CssClass="form-control" MaxLength="9" TabIndex="35"></asp:TextBox>
                                                        <asp:MaskedEditExtender ID="txtHoraLLamar_MaskedEditExtender" runat="server" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtHoraLLamar">
                                                        </asp:MaskedEditExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </div>

                                    <div class="panel panel-primary" runat="server" id="DivOpciones">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 100%">
                                                    <asp:Panel ID="PnlHeaderGestiones" runat="server">
                                                        Gestiones Realizadas
                                                                <asp:Image runat="server" ID="ImgCollapseGestiones" ImageUrl="~/Botones/collapseopen.png" Height="20px" />
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="PnlResultadoGestiones" runat="server" Height="200px">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:LinkButton ID="LnkGestiones" runat="server" OnClick="LnkGestiones_Click" TabIndex="36">Todas las Gestiones</asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel ID="PnlGestiones" runat="server" Height="280px" ScrollBars="Vertical" GroupingText="Gestiones" TabIndex="37">
                                                                        <asp:GridView ID="GrdvGestiones" runat="server" AutoGenerateColumns="False"
                                                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                                            ForeColor="#333333" PageSize="5" TabIndex="38" Width="100%" DataKeyNames="Tipo" OnRowDataBound="GrdvGestiones_RowDataBound">
                                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha Registro" />
                                                                                <asp:BoundField DataField="Operacion" HeaderText="Operación" />
                                                                                <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
                                                                                <asp:BoundField DataField="Gestor" HeaderText="Gestor" />
                                                                                <asp:BoundField DataField="Respuesta" HeaderText="Respuesta" />
                                                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                                                                            </Columns>
                                                                            <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                                                            <RowStyle Font-Size="X-Small" />
                                                                            <EditRowStyle BackColor="#2461BF" />
                                                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                                                        </asp:GridView>
                                                                    </asp:Panel>

                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:CollapsiblePanelExtender ID="PnlResultadoGestiones_CollapsiblePanelExtender" runat="server"
                                                        CollapseControlID="PnlHeaderGestiones" Collapsed="True" CollapsedImage="~/Botones/collapseopen.png"
                                                        CollapsedText="Mostrando Gestiones..." Enabled="True" ExpandControlID="PnlHeaderGestiones"
                                                        ExpandedImage="~/Botones/collapseclose.png" ExpandedText="Ocultar Gestiones..."
                                                        ImageControlID="ImgCollapseGestiones" SuppressPostBack="True" TargetControlID="PnlResultadoGestiones">
                                                    </asp:CollapsiblePanelExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

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
                                    <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" TabIndex="39" OnClick="BtnGrabar_Click" OnClientClick="return grabar();" />
                                </td>
                                <td style="width: 10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="40" OnClick="BtnSalir_Click" />
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
