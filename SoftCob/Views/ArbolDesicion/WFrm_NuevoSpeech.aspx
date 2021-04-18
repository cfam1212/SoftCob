<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_NuevoSpeech.aspx.cs" Inherits="SoftCob.Views.ArbolDesicion.WFrm_NuevoSpeech" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

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
    <link href="../../css/jquery-te-1.4.0.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        legend {
            color: darkblue;
            font-size: 12px;
            font-weight: bold;
            height: 15px;
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
<%--                <div class="panel-info">
                    <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updOpciones">
                        <ProgressTemplate>
                            <div class="overlay" />
                            <div class="overlayContent">
                                <h2>Procesando..</h2>
                                <img src="../../Images/load.gif" alt="Loading" border="1" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>--%>
                <div class="panel-body">
                    <table style="width: 100%" class="table table-bordered table-responsive">
                        <tr>
                            <td style="width: 20%">
                                <div style="display: inline-block;">
                                    <asp:TreeView ID="TrvCedentes" runat="server" ImageSet="Arrows" TabIndex="1" OnSelectedNodeChanged="TrvCedentes_SelectedNodeChanged" OnTreeNodePopulate="TrvCedentes_TreeNodePopulate">
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
                                <div class="panel panel-primary">
                                    <div class="panel-heading">Speech Bienvenida</div>
                                    <div class="panel-body">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 25%">
                                                    <asp:HiddenField ID="hidText" runat="server" />
                                                </td>
                                                <td style="width: 5%"></td>
                                                <td style="width: 65%">
                                                    <h3 id="lblCatalogo" runat="server"></h3>
                                                </td>
                                                <td style="width: 5%"></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlCampos" runat="server" ScrollBars="Vertical" Height="200px" GroupingText="Campos">
                                                        <asp:ListBox ID="LstCamposA" runat="server" Width="100%" TabIndex="2" Height="150px"></asp:ListBox>
                                                    </asp:Panel>
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:Button ID="BtnPasar0" runat="server" CssClass="button" TabIndex="3" Text="&gt;" Width="50%" OnClick="BtnPasar0_Click" />
                                                </td>
                                                <td colspan="2">
                                                    <asp:Panel ID="pnlSpeechB" runat="server" ScrollBars="Vertical" Height="200px" GroupingText="Speech Bienvenida">
                                                        <cc1:Editor ID="TxtEditor1" runat="server" />
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5 runat="server" id="lblEstado" visible="false">Estado:</h5>
                                                </td>
                                                <td></td>
                                                <td>
                                                    <asp:CheckBox ID="ChkEstadoB" runat="server" Visible="False" AutoPostBack="True" Checked="True" CssClass="form-control" TabIndex="5" OnCheckedChanged="ChkEstadoB_CheckedChanged" />
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="panel panel-primary">
                                    <div class="panel-heading">Speech Árbol Desición</div>
                                    <div class="panel-body">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 10%"></td>
                                                <td style="width: 40%"></td>
                                                <td style="width: 10%"></td>
                                                <td style="width: 40%"></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Acción:</h5>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updAccion" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="DdlAccion" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlAccion_SelectedIndexChanged" TabIndex="6">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>

                                                </td>
                                                <td>
                                                    <h5>Efecto:</h5>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updEfecto" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="DdlEfecto" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlEfecto_SelectedIndexChanged" TabIndex="7">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Respuesta:</h5>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updRespuesta" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="DdlRespuesta" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlRespuesta_SelectedIndexChanged" TabIndex="8">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <h5>Contacto:</h5>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updContacto" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="DdlContacto" runat="server" CssClass="form-control" Width="100%" TabIndex="9">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                        </table>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 25%">
                                                    <asp:HiddenField ID="hidText1" runat="server" />
                                                </td>
                                                <td style="width: 5%"></td>
                                                <td style="width: 65%"></td>
                                                <td style="width: 5%"></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlCampos1" runat="server" GroupingText="Campos" Height="200px" ScrollBars="Vertical">
                                                        <asp:ListBox ID="LstCamposB" runat="server" Height="150px" TabIndex="10" Width="100%"></asp:ListBox>
                                                    </asp:Panel>
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:Button ID="BtnPasar1" runat="server" CssClass="button" TabIndex="11" Text="&gt;" Width="50%" OnClick="BtnPasar1_Click" />
                                                </td>
                                                <td colspan="2">
                                                    <asp:Panel ID="pnlSpeechA" runat="server" GroupingText="Speech Árbol" Height="200px" ScrollBars="Vertical">
                                                        <cc1:Editor ID="TxtEditor2" runat="server" />
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Observación:</h5>
                                                </td>
                                                <td></td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtObservacion" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="form-control upperCase" Width="100%" TabIndex="13" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 10%"></td>
                                                <td style="width: 40%"></td>
                                                <td style="width: 10%"></td>
                                                <td style="width: 40%"></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ImageButton ID="ImgAddSpeech" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" TabIndex="14" OnClick="ImgAddSpeech_Click" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="ImgModiSpeech" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" TabIndex="15" OnClick="ImgModiSpeech_Click" Enabled="False" />
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
                                                <td colspan="4">
                                                    <asp:Panel ID="pnlSpeech" runat="server" Height="250px" ScrollBars="Vertical">
                                                        <asp:GridView ID="GrdvSpeech" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo" ForeColor="#333333" PageSize="5" TabIndex="16" Width="100%" OnRowDataBound="GrdvSpeech_RowDataBound">
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Accion" HeaderText="Acción" />
                                                                <asp:BoundField DataField="Efecto" HeaderText="Efecto" />
                                                                <asp:BoundField DataField="Respuesta" HeaderText="Respuesta" />
                                                                <asp:BoundField DataField="Contacto" HeaderText="Contacto" />
                                                                <asp:TemplateField HeaderText="Estado">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="ChkEstadoD" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEstadoD_CheckedChanged" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Del">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImgEliminar" runat="server" Height="20px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgEliminar_Click" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Selecc">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImgSelecc" runat="server" Height="20px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSelecc_Click" />
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
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="panel panel-default">
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: right; width: 45%">
                                <asp:Button ID="BtnGrabar" runat="server" CssClass="button" TabIndex="17" Text="Grabar" Width="120px" OnClick="BtnGrabar_Click" />
                            </td>
                            <td style="width: 10%"></td>
                            <td style="text-align: left; width: 45%">
                                <asp:Button ID="BtnSalir" runat="server" CssClass="button" TabIndex="18" Text="Salir" Width="120px" OnClick="BtnSalir_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
<%--<script src="../../JS/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="../../JS/jquery-te-1.4.0.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(".textEditor").jqte({
        blur: function () {
            document.getElementById('<%=hidText.ClientID %>').value = document.getElementById('<%=txtEditor.ClientID %>').value;
        }
    });
    $(".textEditor1").jqte({
        blur: function () {
            document.getElementById('<%=hidText1.ClientID %>').value = document.getElementById('<%=txtEditor1.ClientID %>').value;
        }
    });
</script>--%>
</html>
