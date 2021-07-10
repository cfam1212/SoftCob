<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_GestionListaTrabajo.aspx.cs" Inherits="SoftCob.Views.Gestion.WFrm_GestionListaTrabajo" %>

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

                $('#TxtFechaCitacion').datepicker(
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
            <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                <asp:LinkButton ID="LnkSeguimiento" runat="server" OnClick="LnkSeguimiento_Click"></asp:LinkButton>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel ID="updError" runat="server">
                    <ContentTemplate>
                        <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                            <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="panel-info">
                    <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updOpciones">
                        <ProgressTemplate>
                            <div class="overlay" />
                            <div class="overlayContent">
                                <h2>Procesando..</h2>
                                <img src="../../Images/load.gif" alt="Loading" border="1" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <asp:UpdatePanel ID="updTimer" runat="server">
                    <ContentTemplate>
                        <div>
                            <asp:Timer ID="TmrTimer" runat="server" Interval="1000" OnTick="TmrTimer_Tick" Enabled="False">
                            </asp:Timer>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updCabecera" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table style="width: 100%" class="table table-bordered table-responsive">
                                <tr>
                                    <td style="width: 20%">
                                        <div style="display: inline-block; overflow: scroll; height: 480px">
                                            <asp:TreeView ID="TrvListaTrabajo" runat="server" ImageSet="Arrows" TabIndex="1" OnTreeNodePopulate="TrvListaTrabajo_TreeNodePopulate" OnSelectedNodeChanged="TrvListaTrabajo_SelectedNodeChanged">
                                                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                                <Nodes>
                                                    <asp:TreeNode PopulateOnDemand="True" SelectAction="Expand" Text="Lista de Trabajo" Value="Lista de Trabajo"></asp:TreeNode>
                                                </Nodes>
                                                <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                                <ParentNodeStyle Font-Bold="False" />
                                                <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                                            </asp:TreeView>

                                        </div>
                                    </td>
                                    <td style="width: 80%">
                                        <div class="panel panel-primary">
                                            <div class="panel-body">
                                                <asp:UpdatePanel ID="updEstadisticas" runat="server">
                                                    <ContentTemplate>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 100%">
                                                                    <asp:Panel ID="PnlHeader" runat="server">
                                                                        Estadisticas
                                                                        <asp:Image runat="server" ID="ImgCollapse" ImageUrl="~/Botones/collapseopen.png" Height="20px" />
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel ID="pnlEstadisticas" runat="server" CssClass="panel panel-primary" Height="200px"
                                                                        GroupingText="Estadisticas Gestión" ScrollBars="Horizontal">
                                                                        <asp:GridView ID="GrdvEstadisticas" runat="server"
                                                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                                            ForeColor="#333333" PageSize="3" TabIndex="1" Width="100%">
                                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                            <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                                                            <RowStyle Font-Size="X-Small" />
                                                                            <EditRowStyle BackColor="#2461BF" />
                                                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                    <asp:CollapsiblePanelExtender ID="pnlEstadisticas_CollapsiblePanelExtender"
                                                                        runat="server" Enabled="True" TargetControlID="pnlEstadisticas"
                                                                        ExpandControlID="PnlHeader" CollapsedImage="~/Botones/collapseopen.png"
                                                                        ExpandedImage="~/Botones/collapseclose.png" CollapsedText="Mostrando Historial..."
                                                                        ImageControlID="ImgCollapse" SuppressPostBack="True" Collapsed="True"
                                                                        CollapseControlID="PnlHeader" ExpandedText="Ocultar Estadisticas">
                                                                    </asp:CollapsiblePanelExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>

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
                                                            <asp:Panel ID="PnlPresupuesto" runat="server" CssClass="panel panel-primary" Height="400px">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Panel ID="PnlPresuCompromiso" runat="server" CssClass="panel panel-primary" Height="200px"
                                                                                GroupingText="Presupuesto Compromisos de Pago" ScrollBars="Horizontal">
                                                                                <asp:GridView ID="GrdvBrenchGestor" runat="server" Width="100%"
                                                                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                                                    EmptyDataText="No existen datos para mostrar" TabIndex="2"
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
                                                                            <asp:Panel ID="Panel3" runat="server" Height="10px"></asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Panel ID="PnlBrenchPagos" runat="server" Height="200px" GroupingText="Presupuesto Pagos Realizados">
                                                                                <asp:GridView ID="GrdvBrenchPago" runat="server" Width="100%"
                                                                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                                                    EmptyDataText="No existen datos para mostrar" TabIndex="3"
                                                                                    AutoGenerateColumns="False" PageSize="12" DataKeyNames="CodigoBRMC,PorCumplido"
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
                                                                                        <asp:TemplateField HeaderText="Calififación">
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

                                        <div class="panel panel-primary" runat="server" id="DivRegistrarTele" visible="false">
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
                                                        <asp:Panel ID="PnlDatosTelefonos" runat="server" CssClass="panel panel-primary"
                                                            Height="780px">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="PnlAgregarTelefono" runat="server" Height="290px"
                                                                            GroupingText="Agregar Teléfono" Visible="False">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td style="width: 15%"></td>
                                                                                    <td style="width: 30%"></td>
                                                                                    <td style="width: 15%"></td>
                                                                                    <td style="width: 40%"></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <h5>Tipo Teléfono:</h5>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="DdlTipTelefono2" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlTipTelefono2_SelectedIndexChanged" TabIndex="4" Width="100%">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td>
                                                                                        <h5>Número:</h5>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="TxtTelefono" runat="server" CssClass="form-control" MaxLength="10" TabIndex="5" Width="100%"></asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="txtTelefono_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtTelefono">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <h5>Prefijo:</h5>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="DdlPrefijo" runat="server" CssClass="form-control" TabIndex="6" Width="100%">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td>
                                                                                        <h5>Propietario:</h5>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="DdlPropietario2" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlPropietario2_SelectedIndexChanged" TabIndex="7" Width="100%">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td></td>
                                                                                    <td></td>
                                                                                    <td></td>
                                                                                    <td></td>
                                                                                    <td></td>
                                                                                    <td></td>
                                                                                </tr>
                                                                                <tr runat="server" id="TrFila1" visible="false">
                                                                                    <td>
                                                                                        <h5>No. Documento:</h5>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="TxtDocumentoRef" runat="server" CssClass="form-control upperCase" MaxLength="10" TabIndex="8" Width="100%"></asp:TextBox>
                                                                                    </td>
                                                                                    <td></td>
                                                                                    <td></td>
                                                                                    <td></td>
                                                                                    <td></td>
                                                                                </tr>
                                                                                <tr runat="server" id="TrFila2" visible="false">
                                                                                    <td>
                                                                                        <h5>Nombres:</h5>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="TxtNombres" runat="server" CssClass="form-control upperCase" MaxLength="80" TabIndex="9" Width="100%"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <h5>Apellidos:</h5>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="TxtApellidos" runat="server" CssClass="form-control upperCase" MaxLength="80" TabIndex="10" Width="100%"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="4">
                                                                                        <asp:Panel ID="pnlDiv5" runat="server" Height="20px"></asp:Panel>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td></td>
                                                                                    <td style="text-align: center">
                                                                                        <asp:ImageButton ID="ImgAddTelefono" runat="server" Height="25px" ImageUrl="~/Botones/agregar.png" OnClick="ImgAddTelefono_Click" TabIndex="11" Enabled="False" ToolTip="Agregar Nuevo Teléfono" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="ImgEditelefono" runat="server" Enabled="False" Height="25px" ImageUrl="~/Botones/modificarnew.png" OnClick="ImgEditelefono_Click" TabIndex="12" ToolTip="Modificar Teléfono" />
                                                                                    </td>
                                                                                    <td></td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="PnlDiv10" runat="server" Height="30px"></asp:Panel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="PnlTelefonosExistentes" runat="server" Height="510px"
                                                                            GroupingText="Teléfonos Existentes">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td style="width: 15%"></td>
                                                                                    <td style="width: 30%"></td>
                                                                                    <td style="width: 15%"></td>
                                                                                    <td style="width: 40%"></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td></td>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="ChkAgregar" runat="server" CssClass="form-control" AutoPostBack="True" OnCheckedChanged="ChkAgregar_CheckedChanged" Text="Agregar Teléfono" Visible="False" TabIndex="13" />
                                                                                    </td>
                                                                                    <td></td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="ImgBuscarFono0" runat="server" Height="25px" ImageUrl="~/Botones/bntbuscarfono.png" OnClick="ImgBuscarFono_Click" ToolTip="Buscar Teléfonos" Visible="False" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="4"></td>
                                                                                </tr>
                                                                                <tr runat="server" id="TrOpciones" visible="false">
                                                                                    <td>
                                                                                        <h5>Acción:</h5>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="DdlAccionDel" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlAccionDel_SelectedIndexChanged" TabIndex="9" Width="100%">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td>
                                                                                        <h5>Respuesta:</h5>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="DdlRespuestaDel" runat="server" CssClass="form-control" TabIndex="10" Width="100%">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="4">
                                                                                        <asp:Panel ID="PnlDiv0" runat="server" Height="20px"></asp:Panel>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="4">
                                                                                        <asp:Panel ID="pnlTelefonos" runat="server" Height="390px" ScrollBars="Vertical">
                                                                                            <asp:GridView ID="GrdvTelefonos" runat="server" AutoGenerateColumns="False"
                                                                                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                                                                DataKeyNames="Codigo,Telefono,Prefijo,CodTipo,CodPro,Nuevo,Score,BCast,NumDocumento"
                                                                                                ForeColor="#333333" TabIndex="14" Width="100%"
                                                                                                OnRowDataBound="GrdvTelefonos_RowDataBound">
                                                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                                                <Columns>
                                                                                                    <asp:BoundField DataField="Prefijo" HeaderText="Prefijo">
                                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                                                                                                    <asp:BoundField DataField="Propietario" HeaderText="Propietario" />
                                                                                                    <asp:BoundField DataField="NomApe" HeaderText="Nombres">
                                                                                                        <ItemStyle Wrap="True" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:TemplateField HeaderText="Selecc">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:ImageButton ID="ImgEditar" runat="server" Height="20px" ImageUrl="~/Botones/seleccbg.png" OnClick="ImgEditar_Click" />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Del">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:ImageButton ID="ImgDelete" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgDelete_Click" OnClientClick="return asegurar();" />
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
                                                                                                    <asp:TemplateField HeaderText="Bcast">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:CheckBox ID="ChkBCast" runat="server" OnCheckedChanged="ChkBCast_CheckedChanged" AutoPostBack="True" />
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
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="Panel6" runat="server" Height="20px"></asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>

                                        <div class="panel panel-primary" runat="server" id="DivDeudor" visible="false">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 100%">
                                                        <asp:Panel ID="PnlHeaderDeudor" runat="server">
                                                            Datos Deudor-Operacion
                                                                <asp:Image runat="server" ID="ImgCollapseDeudor" ImageUrl="~/Botones/collapseopen.png" Height="20px" />
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="PnlDatosDeudorOperacion" runat="server" CssClass="panel panel-primary" Height="620px">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td style="width: 100%"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlDatosDeudor" runat="server" CssClass="panel panel-primary" Height="200px"
                                                                            GroupingText="Datos Deudor" TabIndex="15">
                                                                            <asp:GridView ID="GrdvDatosDeudor" runat="server" AutoGenerateColumns="False"
                                                                                CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333"
                                                                                PageSize="3" TabIndex="16" Width="100%">
                                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="Cedula" HeaderText="Cédula" />
                                                                                    <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                                                                                    <asp:BoundField DataField="Provincia" HeaderText="Provincia" />
                                                                                    <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                                                                    <asp:BoundField DataField="Edad" HeaderText="Edad" >
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:TemplateField HeaderText="Actualizar">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="ImgUpdate" runat="server" Height="20px" ImageUrl="~/Botones/modificarbg.png" OnClick="Imgupdate_Click" />
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
                                                                        <asp:Panel ID="pnlDatosGetion" runat="server" CssClass="panel panel-primary" Height="200px" GroupingText="Datos Operación" ScrollBars="Vertical" TabIndex="17">
                                                                            <asp:GridView ID="GrdvDatosObligacion" runat="server" AutoGenerateColumns="False"
                                                                                CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333"
                                                                                PageSize="5" TabIndex="18" Width="100%" DataKeyNames="Operacion,DiasMora" OnRowDataBound="GrdvDatosObligacion_RowDataBound" ShowFooter="True">
                                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                                                                    <asp:BoundField DataField="Catalogo" HeaderText="Catalogo">
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Operacion" HeaderText="Operación">
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="HDiasMora" HeaderText="H.Mora">
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField HeaderText="Valor Deuda" DataField="ValorDeuda">
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="CVencido" HeaderText="Capital Vencido">
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Exigible" HeaderText="Exigible">
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:BoundField>
                                                                                    <asp:TemplateField HeaderText="Inf.">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="ImgInformacion" runat="server" Height="20px" ImageUrl="~/Botones/informacionbg.png" OnClick="ImgInformacion_Click" />
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
                                                                        <asp:Panel ID="PnlDatosGarante" runat="server" CssClass="panel panel-primary" Height="200px" ScrollBars="Vertical"
                                                                            GroupingText="Datos Garante-CoDeudor" TabIndex="15" Visible="False">
                                                                            <asp:GridView ID="GrdvDatosGarante" runat="server" AutoGenerateColumns="False"
                                                                                CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333"
                                                                                PageSize="3" TabIndex="16" Width="100%" DataKeyNames="CedulaGarante,CodigoGARA,Operacion">
                                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="CedulaGarante" HeaderText="Cédula" />
                                                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                                                                    <asp:BoundField DataField="Garante" HeaderText="Nombres" />
                                                                                    <asp:BoundField DataField="Operacion" HeaderText="Operación" />
                                                                                    <asp:TemplateField HeaderText="Actualizar">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="ImgEditGarante" runat="server" Height="20px" ImageUrl="~/Botones/modificarbg.png" OnClick="ImgEditGarante_Click" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Buscar">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="ImgBuscarAarbol" runat="server" Height="20px" ImageUrl="~/Botones/con_usuariobg.png" OnClick="ImgBuscarAarbol_Click" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Datos" Visible="False">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="ImgDatoEqui" runat="server" Height="20px" ImageUrl="~/Botones/identidad.png" OnClick="ImgDatoEqui_Click" />
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
                                                        <asp:CollapsiblePanelExtender ID="PnlDatosDeudorOperacion_CollapsiblePanelExtender" runat="server"
                                                            CollapseControlID="PnlHeaderDeudor" Collapsed="True" CollapsedImage="~/Botones/collapseopen.png"
                                                            CollapsedText="Mostrando Deudor-Operación" Enabled="True" ExpandControlID="PnlHeaderDeudor"
                                                            ExpandedImage="~/Botones/collapseclose.png" ExpandedText="Ocultar Datos-Deudor"
                                                            ImageControlID="ImgCollapseDeudor" SuppressPostBack="True" TargetControlID="PnlDatosDeudorOperacion">
                                                        </asp:CollapsiblePanelExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>

                                        <div class="panel panel-primary" runat="server" id="DivBotones" visible="false">
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
                                                        <asp:Panel ID="PnlBotones" runat="server" Height="140px" GroupingText="Grupo de Botones">
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
                                                                        <asp:ImageButton ID="ImgSpeechBV" runat="server" Height="30px" ImageUrl="~/Botones/speechBVbg.png" OnClick="ImgSpeechBV_Click" TabIndex="21" ToolTip="Speech de BienVenida" />
                                                                    </td>
                                                                    <td style="text-align: center">
                                                                        <asp:ImageButton ID="ImgSpeechAD" runat="server" Height="30px" ImageUrl="~/Botones/speechABbg.png" OnClick="ImgSpeechAD_Click" TabIndex="22" ToolTip="Speech por Arbol de Desición" />
                                                                    </td>
                                                                    <td style="text-align: center">
                                                                        <asp:ImageButton ID="ImgNotas" runat="server" Height="30px" ImageUrl="~/Botones/notasbg.png" OnClick="ImgNotas_Click" TabIndex="23" ToolTip="Agregar Notas" />
                                                                    </td>
                                                                    <td style="text-align: center">
                                                                        <asp:ImageButton ID="ImgArbolGen" runat="server" Height="30px" ImageUrl="~/Botones/con_usuariobg.png" OnClick="ImgArbolGen_Click" TabIndex="23" ToolTip="Mostrar Árbol Genealógico" />
                                                                    </td>
                                                                    <td style="text-align: center">
                                                                        <asp:ImageButton ID="ImgCitacion" runat="server" Height="30px" ImageUrl="~/Botones/btncitacionbg.png" OnClick="ImgCitacion_Click" TabIndex="23" ToolTip="Crear Citación" />
                                                                    </td>
                                                                    <td style="text-align: center">
                                                                        <asp:ImageButton ID="ImgComparar" runat="server" Height="35px" ImageUrl="~/Botones/comparar.png" OnClick="ImgComparar_Click" TabIndex="23" ToolTip="Compararme con Todos" Visible="False" />
                                                                    </td>
                                                                    <td style="text-align: center"></td>
                                                                    <td></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="9">
                                                                        <asp:Panel ID="Panel4" runat="server" Height="10px"></asp:Panel>
                                                                    </td>
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
                                                                        <asp:ImageButton ID="ImgArbol" runat="server" Height="35px" ImageUrl="~/Botones/arbolbg.png" OnClick="ImgArbol_Click" TabIndex="23" ToolTip="Árbol Genealógico" />
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

                                        <div class="panel panel-primary" runat="server" id="DivGestion" visible="false">
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
                                                        <asp:Panel ID="PnlDatosRegGestion" runat="server" CssClass="panel panel-primary" Height="350px" GroupingText="Opción Gestiones">
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
                                                                    <td colspan="3">
                                                                        <asp:Label ID="lblNumMarcado" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Blue"></asp:Label>
                                                                    </td>
                                                                    <td></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="5">
                                                                        <div id="divException" class="alert-danger" style="visibility: hidden">
                                                                            <asp:Literal ID="litMensajeEx" runat="server">
                                                                            </asp:Literal>
                                                                        </div>
                                                                    </td>
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
                                                                        <asp:Panel ID="Panel2" runat="server" Height="20px"></asp:Panel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h5>Acción:</h5>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="DdlAccion" runat="server" CssClass="form-control" Width="100%" TabIndex="24" AutoPostBack="True" OnSelectedIndexChanged="DdlAccion_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <h5>Efecto:</h5>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:DropDownList ID="DddlEfecto" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" TabIndex="25" OnSelectedIndexChanged="DdlEfecto_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h5>Respuesta:</h5>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="DdlRespuesta" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" TabIndex="26" OnSelectedIndexChanged="DdlRespuesta_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <h5 runat="server" id="LblContacto">Contacto:</h5>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:DropDownList ID="DdlContacto" runat="server" CssClass="form-control" Width="100%" TabIndex="27" OnSelectedIndexChanged="DdlContacto_SelectedIndexChanged" AutoPostBack="True">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <h5>Observación</h5>
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <asp:UpdatePanel ID="updObservacion" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:TextBox ID="TxtObservacion" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="form-control upperCase" Height="80px" TextMode="MultiLine" Width="100%" TabIndex="28" MaxLength="500"></asp:TextBox>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:CheckBox ID="ChkPerfilDeudor" runat="server" AutoPostBack="True" TabIndex="29" Text="Perfil Deudor" OnCheckedChanged="ChkPerfilDeudor_CheckedChanged" Enabled="False" />
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:CheckBox ID="ChkMantenerReg" runat="server" AutoPostBack="True" TabIndex="29" Text="Mantener Registro" />
                                                                    </td>
                                                                    <td></td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <asp:CollapsiblePanelExtender ID="PnlDatosRegGestion_CollapsiblePanelExtender" runat="server"
                                                            CollapseControlID="PnlHeaderRegGestion" Collapsed="True" CollapsedImage="~/Botones/collapseopen.png"
                                                            CollapsedText="Mostrando Opciones..." Enabled="True" ExpandControlID="PnlHeaderRegGestion"
                                                            ExpandedImage="~/Botones/collapseclose.png" ExpandedText="Ocultando Opciones..."
                                                            ImageControlID="ImgCollapseRegGestion" SuppressPostBack="True" TargetControlID="PnlDatosRegGestion">
                                                        </asp:CollapsiblePanelExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>

                                        <div class="panel panel-primary" runat="server" id="divPagos" visible="False">
                                            <asp:Panel ID="PnlRegistroPagos" runat="server" Height="120px" GroupingText="Registro Pagos-Abonos">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 15%">
                                                            <h5>Fecha Pago:</h5>
                                                        </td>
                                                        <td style="width: 40%">
                                                            <asp:TextBox ID="TxtFechaPago" runat="server" CssClass="form-control" Width="100%" TabIndex="30"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 15%">
                                                            <h5>Valor Pago:</h5>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <asp:TextBox ID="TxtValorAbono" runat="server" CssClass="form-control alinearDerecha" MaxLength="7" TabIndex="31"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <h5>Tipo Pago:</h5>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="DdlTipoPago" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="32" Width="100%" OnSelectedIndexChanged="DdlTipoPago_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <h5>No.Documento:</h5>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtNumDocumento" runat="server" CssClass="form-control" MaxLength="20" TabIndex="33"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </div>

                                        <div class="panel panel-primary" runat="server" id="divLLamar" visible="False">
                                            <asp:Panel ID="PnlVolveraLlamar" runat="server" Height="100px" GroupingText="Volver a LLamar">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <h5>Fecha LLamada:</h5>
                                                        </td>
                                                        <td style="width: 35%">
                                                            <asp:TextBox ID="TxtFechaLLamar" runat="server" CssClass="form-control" Width="100%" TabIndex="34"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 15%">
                                                            <h5>Hora LLamada:</h5>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <asp:TextBox ID="TxtHoraLLamar" runat="server" CssClass="form-control" MaxLength="9" TabIndex="35"></asp:TextBox>
                                                            <asp:MaskedEditExtender ID="txtHoraLLamar_MaskedEditExtender" runat="server" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtHoraLLamar">
                                                            </asp:MaskedEditExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <h5>Mismo Número:</h5>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:RadioButton ID="RdbSi" runat="server" CssClass="form-control" Text="Si" TabIndex="36" OnCheckedChanged="RdbSi_CheckedChanged" AutoPostBack="True" />
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="RdbNo" runat="server" CssClass="form-control" Text="No" TabIndex="37" OnCheckedChanged="RdbNo_CheckedChanged" AutoPostBack="True" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </div>

                                        <div class="panel panel-primary" runat="server" id="divCitacion" visible="False">
                                            <asp:Panel ID="Panel1" runat="server" Height="100px" GroupingText="Datos Citación">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <h5>Fecha Citación:</h5>
                                                        </td>
                                                        <td style="width: 35%">
                                                            <asp:TextBox ID="TxtFechaCitacion" runat="server" CssClass="form-control" Width="100%" TabIndex="34"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 15%">
                                                            <h5>Hora Citación:</h5>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <asp:TextBox ID="TxtHoraCitacion" runat="server" CssClass="form-control" MaxLength="9" TabIndex="35"></asp:TextBox>
                                                            <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtHoraLLamar">
                                                            </asp:MaskedEditExtender>
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
                                        </div>

                                        <div class="panel panel-primary" runat="server" id="divPerfiles" visible="False">
                                            <asp:Panel ID="PnlPerfilDeudor" runat="server" Height="350px" GroupingText="Perfil Deudor">
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
                                                            <asp:Panel ID="pnlDivision" runat="server" Height="20px">
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

                                        <div class="panel panel-primary" runat="server" id="DivOpciones" visible="false">
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
                                                        <asp:Panel ID="PnlResultadoGestiones" runat="server" Height="350px">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:LinkButton ID="LnkGestiones" runat="server" TabIndex="38" OnClick="LnkGestiones_Click" Enabled="False">Todas las Gestiones</asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="PnlDatosGestiones" runat="server" CssClass="panel panel-primary" Height="340px" ScrollBars="Vertical">
                                                                            <asp:GridView ID="GrdvGestiones" runat="server" AutoGenerateColumns="False"
                                                                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                                                ForeColor="#333333" PageSize="15" TabIndex="39" Width="100%" DataKeyNames="Tipo" OnRowDataBound="GrdvGestiones_RowDataBound">
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
                    <asp:UpdatePanel ID="updOpciones" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 25%"></td>
                                    <td style="width: 25%"></td>
                                    <td style="width: 25%"></td>
                                    <td style="width: 25%"></td>
                                </tr>
                                <tr>
                                    <td style="text-align: right"></td>
                                    <td style="text-align: center">
                                        <asp:Button ID="BtnGrabar" runat="server" CssClass="button" OnClick="BtnGrabar_Click" TabIndex="40" Text="Siguiente" Width="120px" OnClientClick="return grabar();" />
                                    </td>
                                    <td style="text-align: center">
                                        <asp:Button ID="BtnSalir" runat="server" CssClass="button" OnClick="BtnSalir_Click" TabIndex="42" Text="Salir" Width="120px" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Button ID="BtnPasar" runat="server" BackColor="#FF3300" CssClass="button" OnClick="BtnPasar_Click" TabIndex="41" Text="Pasar" Width="100px" Visible="False" />
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <table style="width: 100%">
            <tr>
                <td>
                    <div style="visibility: hidden; display: none">
                        <asp:Button ID="BtnRefrescar" runat="server" OnClick="BtnRefrescar_Click" Text="Refrescar" />
                    </div>

                </td>
            </tr>
        </table>
    </form>
    <script type="text/javascript">
        var cronometro;

        function CerrarVentana() {
            window.open('', '_parent', '');
            window.close();
        }

        function CloseFrame() {
            window.top.location.reload();
        }

        function ValidarDecimales() {
            var numero = document.getElementById("<%=TxtValorAbono.ClientID%>").value;
            if (!/^([0-9])*[.]?[0-9]*$/.test(numero)) {
                alert("El valor " + numero + " no es un número válido. Ejemplo 157.68");
                document.getElementById("<%=TxtValorAbono.ClientID%>").value = "";
                return false;
            }
        }

        function ShowModalPopUp() {
            $find("PexGestionCTC").show();
        }

        function Refrescar() {
            document.getElementById('BtnRefrescar').click();
        }

    </script>
    <script>
        function funMensajeException() {
            $("#divException").attr("style", "visibility:visible");
        }
    </script>
</body>
</html>
