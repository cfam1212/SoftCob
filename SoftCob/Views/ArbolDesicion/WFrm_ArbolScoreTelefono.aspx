<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_ArbolScoreTelefono.aspx.cs" Inherits="SoftCob.Views.ArbolDesicion.WFrm_ArbolScoreTelefono" %>

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

        .auto-style1 {
            height: 20px;
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
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 10%"></td>
                                        <td style="width: 40%"></td>
                                        <td style="width: 10%"></td>
                                        <td style="width: 40%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="2">
                                            <h3 id="lblCatalogo" runat="server"></h3>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <h5>Acción:</h5>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="updAccion" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="DdlAccion" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlAccion_SelectedIndexChanged" TabIndex="2">
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
                                                    <asp:DropDownList ID="DdlEfecto" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlEfecto_SelectedIndexChanged" TabIndex="3">
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
                                                    <asp:DropDownList ID="DdlRespuesta" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlRespuesta_SelectedIndexChanged" TabIndex="4">
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
                                                    <asp:DropDownList ID="DdlContacto" runat="server" CssClass="form-control" Width="100%" TabIndex="5">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <h5>Califica:</h5>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdCalifica" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="DdlCalifica" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlCalifica_SelectedIndexChanged" TabIndex="6">
                                                        <asp:ListItem Value="N">--Seleccione Califica--</asp:ListItem>
                                                        <asp:ListItem Value="Score">Score</asp:ListItem>
                                                        <asp:ListItem Value="Estado">Estado</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <h5>Cantidad:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtCantidad" runat="server" CssClass="form-control alinearDerecha" Width="100%" TabIndex="7" MaxLength="2">1</asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="TxtCantidad_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="TxtCantidad"></asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <h5>Valor:</h5>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="updValores" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="DdlValor" runat="server" CssClass="form-control" Width="100%" TabIndex="8">
                                                        <asp:ListItem Value="-1">--Seleccione Valor--</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ImgAdd" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" TabIndex="9" OnClick="ImgAdd_Click" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ImgModi" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" TabIndex="10" Enabled="False" OnClick="ImgModi_Click" />
                                        </td>
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
                                            <asp:Panel ID="pnlSpeech" runat="server" Height="250px" ScrollBars="Auto">
                                                <asp:GridView ID="GrdvArbolScore" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo" ForeColor="#333333" PageSize="5" TabIndex="11" Width="100%" OnRowDataBound="GrdvArbolScore_RowDataBound">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Accion" HeaderText="Acción" />
                                                        <asp:BoundField DataField="Efecto" HeaderText="Efecto" />
                                                        <asp:BoundField DataField="Respuesta" HeaderText="Respuesta" />
                                                        <asp:BoundField DataField="Contacto" HeaderText="Contacto" />
                                                        <asp:BoundField DataField="Califica" HeaderText="Califica" />
                                                        <asp:BoundField DataField="Valor" HeaderText="Valor" />
                                                        <asp:TemplateField HeaderText="Activo">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkEstado" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEstado_CheckedChanged" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Selecc">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgSelecc" runat="server" Height="20px" ImageUrl="~/Botones/seleccbg.png" OnClick="ImgSelecc_Click" />
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
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="panel panel-default">
                    <asp:UpdatePanel ID="updOpciones" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 45%">
                                        <asp:Button ID="BtnGrabar" runat="server" CssClass="button" TabIndex="12" Text="Grabar" Width="120px" OnClick="BtnGrabar_Click" />
                                    </td>
                                    <td style="width: 10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="BtnSalir" runat="server" CssClass="button" TabIndex="13" Text="Salir" Width="120px" OnClick="BtnSalir_Click" />
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
