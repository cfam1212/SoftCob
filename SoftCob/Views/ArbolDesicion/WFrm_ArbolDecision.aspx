<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_ArbolDecision.aspx.cs" Inherits="SoftCob.Views.ArbolDesicion.WFrm_ArbolDecision" %>

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
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <script src="../../JS/alertify.min.js"></script>

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
                                                <td style="width: 15%"></td>
                                                <td style="width: 45%">
                                                    <h3 runat="server" id="lblCatalogo"></h3>
                                                </td>
                                                <td style="width: 25%" colspan="2"></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5 style="color: darkblue">Accion:</h5>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updEtiquetaAccion" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="TxtAccion" runat="server" CssClass="form-control upperCase" MaxLength="250" Width="100%" TabIndex="2"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="ImgAddAccion" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" TabIndex="3" OnClick="ImgAddAccion_Click" />
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="ImgModiAccion" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModiAccion_Click" TabIndex="4" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel runat="server" Height="20px"></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:UpdatePanel ID="updAccion" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="pnlAccion" runat="server" Height="320px" ScrollBars="Vertical" Visible="false">
                                                                <asp:GridView ID="GrdvAccion" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,Estado,Contacto" ForeColor="#333333" PageSize="5" TabIndex="5" Width="100%" OnRowDataBound="GrdvAccion_RowDataBound" OnSelectedIndexChanged="GrdvAccion_SelectedIndexChanged">
                                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                    <Columns>
                                                                        <asp:CommandField HeaderText="Seleccionar" ShowSelectButton="True" />
                                                                        <asp:BoundField DataField="Descripcion" HeaderText="Acción" />
                                                                        <asp:TemplateField HeaderText="Activo">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="ChkEstAccion" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEstAccion_CheckedChanged" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Contac.Directo">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="ChkContacto" runat="server" AutoPostBack="True" OnCheckedChanged="ChkContacto_CheckedChanged" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Eliminar">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImgDelAccion" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgDelAccion_Click" />
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
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="pnlSepara1" runat="server" Height="30px"></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5 style="color: darkblue">Efecto:</h5>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updEtiquetaEfecto" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="TxtEfecto" runat="server" CssClass="form-control upperCase" MaxLength="250" Width="100%" TabIndex="6"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="ImgAddEfecto" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" TabIndex="7" OnClick="ImgAddEfecto_Click" />
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="ImgModiEfecto" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModiEfecto_Click" TabIndex="8" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel runat="server" Height="20px"></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:UpdatePanel ID="updEfecto" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="pnlEfecto" runat="server" Height="380px" ScrollBars="Vertical" Visible="false">
                                                                <asp:GridView ID="GrdvEfecto" runat="server" AutoGenerateColumns="False"
                                                                    CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,CodigoAccion" ForeColor="#333333" PageSize="5" TabIndex="10" Width="100%" OnRowDataBound="GrdvEfecto_RowDataBound" OnSelectedIndexChanged="GrdvEfecto_SelectedIndexChanged">
                                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                    <Columns>
                                                                        <asp:CommandField HeaderText="Seleccionar" ShowSelectButton="True" />
                                                                        <asp:BoundField DataField="Descripcion" HeaderText="Efecto" />
                                                                        <asp:TemplateField HeaderText="Activo">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="ChkEstEfecto" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEstEfecto_CheckedChanged" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Eliminar">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImgDelEfecto" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgDelEfecto_Click" />
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
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="pnlSepara2" runat="server" Height="30px"></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5 style="color: darkblue">Respuesta:</h5>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updEtiquetaRespuesta" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="TxtRespuesta" runat="server" CssClass="form-control upperCase" MaxLength="250" Width="100%" TabIndex="10"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="ImgAddRespuesta" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" TabIndex="12" OnClick="ImgAddRespuesta_Click" />
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="ImgModiRespuesta" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModiRespuesta_Click" TabIndex="13" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel runat="server" Height="20px"></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:UpdatePanel ID="updRespuesta" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="pnlRespuesta" runat="server" Height="380px" ScrollBars="Vertical" Visible="false">
                                                                <asp:GridView ID="GrdvRespuesta" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,CodigoEfecto" ForeColor="#333333" PageSize="5" TabIndex="14" Width="100%" OnRowDataBound="GrdvRespuesta_RowDataBound" OnSelectedIndexChanged="GrdvRespuesta_SelectedIndexChanged">
                                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                    <Columns>
                                                                        <asp:CommandField HeaderText="Seleccionar" ShowSelectButton="True" />
                                                                        <asp:BoundField DataField="Descripcion" HeaderText="Respuesta" />
                                                                        <asp:TemplateField HeaderText="Activo">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="ChkEstRespuesta" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEstRespuesta_CheckedChanged" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Pago">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="ChkPago" runat="server" AutoPostBack="True" OnCheckedChanged="ChkPago_CheckedChanged" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Llamar">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="ChkLlamar" runat="server" AutoPostBack="True" OnCheckedChanged="ChkLlamar_CheckedChanged" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Efectivo">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="ChkEfectivo" runat="server" OnCheckedChanged="ChkEfectivo_CheckedChanged" AutoPostBack="True" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Comisiona">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="ChkComisiona" runat="server" OnCheckedChanged="ChkComisiona_CheckedChanged1" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Eliminar">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImgDelRespuesta" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgDelRespuesta_Click" />
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
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="pnlSeparador3" runat="server" Height="30px"></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5 runat="server" id="LblContacto" style="color: darkblue">Contacto:</h5>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updEtiquetaContacto" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="TxtContacto" runat="server" CssClass="form-control upperCase" MaxLength="250" Width="100%" TabIndex="15"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="ImgAddContacto" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" TabIndex="16" OnClick="ImgAddContacto_Click" />
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="ImgModiContacto" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModiContacto_Click" TabIndex="17" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel runat="server" Height="20px"></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:UpdatePanel ID="updContacto" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="pnlContacto" runat="server" Height="380px" ScrollBars="Vertical" Visible="false">
                                                                <asp:GridView ID="GrdvContacto" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,CodigoRespuesta" ForeColor="#333333" PageSize="5" TabIndex="18" Width="100%" OnRowDataBound="GrdvContacto_RowDataBound" OnSelectedIndexChanged="GrdvContacto_SelectedIndexChanged">
                                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                    <Columns>
                                                                        <asp:CommandField HeaderText="Seleccionar" ShowSelectButton="True" />
                                                                        <asp:BoundField DataField="Descripcion" HeaderText="Contacto" />
                                                                        <asp:TemplateField HeaderText="Activo">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="ChkEstContacto" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEstContacto_CheckedChanged" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Pago">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="ChkPagoContac" runat="server" OnCheckedChanged="ChkPagoContac_CheckedChanged" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Eliminar">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImgDelContacto" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgDelContacto_Click" />
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
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
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
                                    <td style="text-align: right; width: 45%">
                                        <asp:Button ID="BtnAsignar" runat="server" CssClass="button" TabIndex="19" Text="Grabar" Width="120px" OnClick="BtnAsignar_Click" />
                                    </td>
                                    <td style="width: 10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="BtnSalir" runat="server" CssClass="button" TabIndex="20" Text="Salir" Width="120px" OnClick="BtnSalir_Click" />
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
