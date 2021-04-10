<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_AsignarCartera.aspx.cs" Inherits="SoftCob.Views.Cedente.WFrm_AsignarCartera" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../../Scripts/external/jquery/jquery.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <style>
        legend {
            color: darkblue;
            font-size: 14px;
            font-weight: bold;
        }
    </style>
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
            height: 35px;
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
                <div class="panel-body">
                    <asp:UpdatePanel ID="updCabecera" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table style="width: 100%" class="table table-bordered table-responsive">
                                <tr>
                                    <td style="width: 20%">
                                        <div style="display: inline-block;">
                                            <asp:TreeView ID="TrvCedentes" runat="server" ImageSet="Arrows" OnTreeNodePopulate="TrvCedentes_TreeNodePopulate" OnSelectedNodeChanged="TrvCedentes_SelectedNodeChanged" TabIndex="1">
                                                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                                <Nodes>
                                                    <asp:TreeNode PopulateOnDemand="True" SelectAction="Expand" Text="Cedentes/Productos" Value="Cedentes/Productos"></asp:TreeNode>
                                                </Nodes>
                                                <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                                <ParentNodeStyle Font-Bold="False" />
                                                <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                                            </asp:TreeView>
                                        </div>
                                    </td>
                                    <td style="width: 80%">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 15%">
                                                    <h5>Catálogo:</h5>
                                                </td>
                                                <td style="width: 35%">
                                                    <asp:DropDownList ID="DdlCatalogo" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" TabIndex="2" OnSelectedIndexChanged="DdlCatalogo_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 15%">
                                                    <h5>Operaciones:</h5>
                                                </td>
                                                <td style="width: 35%">
                                                    <h3 runat="server" id="lblTotal" style="color: red"></h3>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="pnlDiv4" runat="server" Height="30px" GroupingText="Días Mora">
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Operador:</h5>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DdlOperador1" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="3" Width="100%">
                                                        <asp:ListItem>&gt;=</asp:ListItem>
                                                        <asp:ListItem>&lt;=</asp:ListItem>
                                                        <asp:ListItem>=</asp:ListItem>
                                                        <asp:ListItem>&gt;</asp:ListItem>
                                                        <asp:ListItem>&lt;</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <h5>Días Mora:</h5>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DdlDiasMora" runat="server" CssClass="form-control" TabIndex="4" Width="100%">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:CheckBox ID="ChkLogico1" runat="server" AutoPostBack="True" OnCheckedChanged="ChkLogico1_CheckedChanged" TabIndex="5" Text="Op. Lógico" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DdlDiasLogico" runat="server" CssClass="form-control" TabIndex="6" Width="100%">
                                                        <asp:ListItem>and</asp:ListItem>
                                                        <asp:ListItem>or</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr runat="server" id="trLogico1" visible="false">
                                                <td>
                                                    <h5>Operador:</h5>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DdlOperador3" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="7" Width="100%">
                                                        <asp:ListItem>&gt;=</asp:ListItem>
                                                        <asp:ListItem>&lt;=</asp:ListItem>
                                                        <asp:ListItem>=</asp:ListItem>
                                                        <asp:ListItem>&gt;</asp:ListItem>
                                                        <asp:ListItem>&lt;</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <h5>Días Mora:</h5>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DdlDiasMora1" runat="server" CssClass="form-control" TabIndex="8" Width="100%">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="pnlDiv0" runat="server" Height="30px" GroupingText="Exigible">
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Operador:</h5>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DdlOperador2" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="9" Width="100%">
                                                        <asp:ListItem>&gt;=</asp:ListItem>
                                                        <asp:ListItem>&lt;=</asp:ListItem>
                                                        <asp:ListItem>=</asp:ListItem>
                                                        <asp:ListItem>&gt;</asp:ListItem>
                                                        <asp:ListItem>&lt;</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <h5>Exigible:</h5>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TxtExigible" runat="server" CssClass="form-control alinearDerecha" TabIndex="10">1</asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="txtExigible_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtExigible">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:CheckBox ID="ChkLogico2" runat="server" AutoPostBack="True" OnCheckedChanged="ChkLogico2_CheckedChanged" TabIndex="11" Text="Op. Logico" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DdlDiasLogico0" runat="server" CssClass="form-control" TabIndex="12" Width="100%">
                                                        <asp:ListItem>and</asp:ListItem>
                                                        <asp:ListItem>or</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr runat="server" id="trLogico2" visible="false">
                                                <td>
                                                    <h5>Operador:</h5>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DdlOperador4" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="13" Width="100%">
                                                        <asp:ListItem>&gt;=</asp:ListItem>
                                                        <asp:ListItem>&lt;=</asp:ListItem>
                                                        <asp:ListItem>=</asp:ListItem>
                                                        <asp:ListItem>&gt;</asp:ListItem>
                                                        <asp:ListItem>&lt;</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <h5>Exigible:</h5>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExigible0" runat="server" CssClass="form-control alinearDerecha" TabIndex="14">1</asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="txtExigible0_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtExigible0">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="pnlDiv2" runat="server" Height="30px">
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Resultado</h5>
                                                </td>
                                                <td>
                                                    <asp:Panel ID="pnlResultado" runat="server" Height="180px" ScrollBars="Vertical">
                                                        <asp:GridView ID="GrdvResultado" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ShowFooter="True" ShowHeaderWhenEmpty="True" TabIndex="15" Width="100%" OnRowDataBound="GrdvResultado_RowDataBound">
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:BoundField DataField="DiasMora" HeaderText="Dias_Mora" />
                                                                <asp:BoundField DataField="Operacion" HeaderText="Operaciones" />
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
                                                    <asp:Panel ID="pnlSelect" runat="server" Height="150px" ScrollBars="Vertical">
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="pnlDiv3" runat="server" Height="30px">
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style1">
                                                    <h5>Procesar-></h5>
                                                </td>
                                                <td class="auto-style1">
                                                    <asp:ImageButton ID="ImgAgregar" runat="server" Height="25px" ImageUrl="~/Botones/procesar.png" TabIndex="16" Enabled="False" OnClick="ImgAgregar_Click" />
                                                </td>
                                                <td class="auto-style1"></td>
                                                <td class="auto-style1"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="pnlAsignar" runat="server" Height="190px" ScrollBars="Vertical">
                                                        <asp:GridView ID="GrdvDatos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" Width="100%" TabIndex="17" PageSize="5">
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:BoundField DataField="DiasMora" HeaderText="Dias Mora" />
                                                                <asp:BoundField DataField="Exigible" HeaderText="Exigible" />
                                                                <asp:TemplateField HeaderText="Eliminar">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImgDel" runat="server" Height="20px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgDel_Click" />
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
                                                <td colspan="4">
                                                    <asp:Panel ID="pnlDiv1" runat="server" Height="50px">
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Asignar:</h5>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="RdbOpera" runat="server" AutoPostBack="True" Enabled="False" Font-Size="10pt" TabIndex="18" Text="por Operaciones" OnCheckedChanged="RdbOpera_CheckedChanged" />
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="RdbClientes" runat="server" AutoPostBack="True" Enabled="False" Font-Size="10pt" TabIndex="19" Text="por Clientes" OnCheckedChanged="RdbClientes_CheckedChanged" />
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                        <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">ASIGNAR POR OPERACIONES</h3>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 50%">
                                                    <asp:Panel ID="pnlOpciones" runat="server" Height="150px">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 25%"></td>
                                                                <td style="width: 50%">
                                                                    <asp:RadioButton ID="RdbTodos" runat="server" AutoPostBack="True" Font-Size="10pt" OnCheckedChanged="RdbTodos_CheckedChanged" Text="Todos" Enabled="False" TabIndex="20" />
                                                                </td>
                                                                <td style="width: 25%"></td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td colspan="2">
                                                                    <asp:RadioButton ID="RdbGestor" runat="server" AutoPostBack="True" Font-Size="10pt" Text="Por Gestor" OnCheckedChanged="RdbGestor_CheckedChanged" Enabled="False" TabIndex="21" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:DropDownList ID="DdlGestores" runat="server" CssClass="form-control" TabIndex="22" Width="100%" Enabled="False">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <h5>Operaciones:</h5>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="TxtOperaciones" runat="server" Enabled="False" CssClass="alinearDerecha" TabIndex="23"></asp:TextBox>
                                                                </td>
                                                                <td style="text-align: center">
                                                                    <asp:ImageButton ID="ImgPasar" runat="server" Height="25px" ImageUrl="~/Botones/btnpasaruno.jpg" TabIndex="24" Enabled="False" OnClick="ImgPasar_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td></td>
                                                                <td></td>
                                                            </tr>
                                                        </table>

                                                    </asp:Panel>
                                                </td>
                                                <td style="width: 50%">
                                                    <asp:Panel ID="PnlGestores" runat="server" Height="150px" ScrollBars="Auto">
                                                        <asp:GridView ID="GrdvGestores" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="GestorCodigo" ShowHeaderWhenEmpty="True" TabIndex="25" Width="100%" OnRowDataBound="GrdvGestores_RowDataBound" ShowFooter="True">
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Gestor" HeaderText="Gestor" />
                                                                <asp:BoundField DataField="Operaciones" HeaderText="Operaciones">
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Eliminar">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgDelGestor" runat="server" Height="20px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgDelGestor_Click" />
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
                                        <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">ASIGNAR POR CLIENTE</h3>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 5%">
                                                    <h5>Gestor:</h5>
                                                </td>
                                                <td style="width: 45%">
                                                    <asp:DropDownList ID="DdlGestorCli" runat="server" CssClass="form-control" TabIndex="26" Width="100%" Enabled="False">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 5%"></td>
                                                <td style="width: 45%">
                                                    <asp:RadioButton ID="RdbDeudor" runat="server" AutoPostBack="True" Font-Size="10pt" TabIndex="29" Text="Buscar Por Cliente" Checked="True" Enabled="False" OnCheckedChanged="RdbDeudor_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Buscar:</h5>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TxtBuscar" runat="server" CssClass="form-control upperCase" MaxLength="80" TabIndex="27" Width="100%" Enabled="False"></asp:TextBox>
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="ImgBuscar" runat="server" Height="25px" ImageUrl="~/Botones/Buscar.png" TabIndex="28" OnClick="ImgBuscar_Click" Enabled="False" />
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="RdbIdentificacion" runat="server" AutoPostBack="True" Font-Size="10pt" TabIndex="30" Text="Buscar por Identificación" Enabled="False" OnCheckedChanged="RdbIdentificacion_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td style="text-align: center">
                                                    <h5>Cliente Origen</h5>
                                                </td>
                                                <td></td>
                                                <td style="text-align: center">
                                                    <h5>Cliente Destino</h5>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td rowspan="2"></td>
                                                <td rowspan="2">
                                                    <asp:ListBox ID="LstOrigen" runat="server" CssClass="form-control" Height="230px" SelectionMode="Multiple" Width="100%" TabIndex="31"></asp:ListBox>
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="ImgPasar1" runat="server" Height="25px" ImageUrl="~/Botones/btnpasaruno.jpg" TabIndex="32" OnClick="ImgPasar1_Click" Enabled="False" />
                                                </td>
                                                <td rowspan="2">
                                                    <asp:ListBox ID="LstDestino" runat="server" CssClass="form-control" Height="230px" Width="100%" TabIndex="34"></asp:ListBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="ImgQuitar1" runat="server" Height="25px" ImageUrl="~/Botones/btnquitaruno.png" TabIndex="33" OnClick="ImgQuitar1_Click" Enabled="False" />
                                                </td>
                                            </tr>
                                        </table>
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
                                    <td style="text-align: center; width: 45%">
                                        <asp:Button ID="BtnGrabar" runat="server" CssClass="button" TabIndex="34" Text="Asignar" Width="120px" OnClick="BtnGrabar_Click" />
                                    </td>
                                    <td style="width: 10%"></td>
                                    <td style="text-align: center; width: 45%">
                                        <asp:Button ID="BtnSalir" runat="server" CssClass="button" TabIndex="35" Text="Salir" Width="120px" OnClick="BtnSalir_Click" />
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
