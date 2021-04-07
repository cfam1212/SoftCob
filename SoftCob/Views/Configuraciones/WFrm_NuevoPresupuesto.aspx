<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_NuevoPresupuesto.aspx.cs" Inherits="SoftCob.Views.Configuraciones.WFrm_NuevoPresupuesto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/ColorPicker.css" rel="stylesheet" />


    <script src="../../Scripts/external/jquery/jquery.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <script src="../../JS/ColorPickerBehavior.js"></script>

    <style type="text/css">
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
                <asp:UpdatePanel ID="updCabecera" runat="server">
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
                                            <td style="width: 35%"></td>
                                            <td style="width: 15%"></td>
                                            <td style="width: 35%"></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td colspan="2">
                                                <h3 runat="server" id="lblCatalogo"></h3>
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <h5>Valor Inicial:</h5>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtValorInicial" runat="server" CssClass="alinearDerecha" MaxLength="3" TabIndex="2" Width="100%"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtValorInicial_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtValorInicial">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <h5>Valor Final:</h5>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtValorFinal" runat="server" CssClass="alinearDerecha" MaxLength="3" TabIndex="3" Width="100%"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtValorFinal_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtValorFinal">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <h5>Etiqueta:</h5>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="TxtEtiqueta" runat="server" CssClass="form-control upperCase" MaxLength="150" TabIndex="4" Width="100%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <h5>Color:</h5>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtColor" runat="server" MaxLength="150" TabIndex="4" Width="100%" ReadOnly="True"></asp:TextBox>
                                                <asp:ColorPickerExtender ID="TxtColor_ColorPickerExtender" runat="server" Enabled="True"
                                                    TargetControlID="TxtColor" SampleControlID="preview" PopupButtonID="Right"
                                                    OnClientColorSelectionChanged="Color_Changed">
                                                </asp:ColorPickerExtender>
                                            </td>
                                            <td>
                                                <div id="preview" style="width: 40px; height: 40px; border: 1px solid #000; margin: 0 3px; float: left">
                                                </div>
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Panel ID="Panel1" runat="server" Height="20px"></asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:ImageButton ID="ImgAdd" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" TabIndex="5" OnClick="ImgAdd_Click" />
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="ImgModi" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" TabIndex="6" OnClick="ImgModi_Click" Enabled="False" />
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Panel ID="Panel2" runat="server" Height="20px"></asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:UpdatePanel ID="updAccion" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlAccion" runat="server" Height="180px" ScrollBars="Vertical">
                                                            <asp:GridView ID="GrdvSegmento" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,auxv1" ForeColor="#333333" PageSize="5" TabIndex="7" Width="100%" OnRowDataBound="GrdvSegmento_RowDataBound">
                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="ValorI" HeaderText="Valor Inicial">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ValorF" HeaderText="Valor Final">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Descripcion" HeaderText="Etiqueta" />
                                                                    <asp:BoundField DataField="auxv1" HeaderText="Color" />
                                                                    <asp:TemplateField HeaderText="Selecc">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="ImgSelecc" runat="server" Height="20px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSelecc_Click" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Eliminar">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="ImgEliminar" runat="server" Height="20px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgEliminar_Click" OnClientClick="return asegurar();" />
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
                                    <asp:Button ID="BtnGrabar" runat="server" CssClass="button" TabIndex="8" Text="Grabar" Width="120px" OnClick="BtnGrabar_Click" />
                                </td>
                                <td style="width: 10%"></td>
                                <td style="text-align: center; width: 45%">
                                    <asp:Button ID="BtnSalir" runat="server" CssClass="button" TabIndex="9" Text="Salir" Width="120px" OnClick="BtnSalir_Click" />
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

<script type="text/javascript">

    function Color_Changed(sender) {

        sender.get_element().value = "#" + sender.get_selectedColor();

    }

</script>
