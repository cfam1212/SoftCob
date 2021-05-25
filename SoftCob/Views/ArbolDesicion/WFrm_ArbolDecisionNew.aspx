<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_ArbolDecisionNew.aspx.cs" Inherits="SoftCob.Views.ArbolDesicion.WFrm_ArbolDecisionNew" %>

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
        <div class="panel panel-primary">
            <div class="panel-heading" style="background-color: #79BBB8;">
                <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
            </div>
            <div class="panel-body">
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
                <asp:UpdatePanel ID="updError" runat="server">
                    <ContentTemplate>
                        <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                            <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="panel-info">
                    <asp:UpdateProgress ID="UpdProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdOpciones">
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
                    <asp:UpdatePanel ID="UpdCabecera" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table style="width: 100%" class="table table-bordered table-responsive">
                                <tr>
                                    <td style="width: 40%">
                                        <asp:Panel ID="PnlArbol" runat="server" Height="550px" ScrollBars="Vertical">
                                            <asp:TreeView ID="TrvCedenteArbol" runat="server" ImageSet="Arrows" TabIndex="1" OnSelectedNodeChanged="TrvCedenteArbol_SelectedNodeChanged">
                                                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" Font-Bold="true" />
                                                <Nodes>
                                                    <asp:TreeNode SelectAction="Expand" Text="Cedentes-Árbol" Value="0"></asp:TreeNode>
                                                </Nodes>
                                                <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                                <ParentNodeStyle ForeColor="Black" />
                                                <SelectedNodeStyle Font-Underline="True" ForeColor="Blue" HorizontalPadding="0px" VerticalPadding="0px" />
                                            </asp:TreeView>
                                        </asp:Panel>
                                    </td>
                                    <td style="width: 60%">
                                        <table class="nav-justified">
                                            <tr>
                                                <td style="width: 90%"></td>
                                                <td style="width: 10%"></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h4 runat="server" id="LblDefinicion" style="color: brown;"></h4>
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="ImgDelete" runat="server" Height="25px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgDelete_Click" OnClientClick="return asegurar();" TabIndex="11" Visible="False" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Panel ID="PnlOpciones" runat="server" Height="280px"
                                                        GroupingText="Datos" Visible="False" TabIndex="2">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 15%"></td>
                                                                <td style="width: 45%"></td>
                                                                <td style="width: 20%"></td>
                                                                <td style="width: 20%"></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <h5 runat="server" id="LblArbol" visible="false">Arbol:</h5>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:TextBox ID="TxtArbol" runat="server" CssClass="form-control upperCase" MaxLength="250" TabIndex="3" Width="100%" Visible="False"></asp:TextBox>
                                                                </td>
                                                                <td style="text-align: center">
                                                                    <asp:ImageButton ID="ImgModificar" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModificar_Click" TabIndex="4" Visible="False" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <h5 runat="server" id="LblEstado" visible="false">Estado</h5>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="ChkEstado" runat="server" AutoPostBack="True" TabIndex="5" Text="Activo" Visible="False" OnCheckedChanged="ChkEstado_CheckedChanged" />
                                                                </td>
                                                                <td>&nbsp;</td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: center"></td>
                                                                <td style="text-align: center">
                                                                    <asp:CheckBox ID="ChkPago" runat="server" AutoPostBack="True" OnCheckedChanged="ChkPago_CheckedChanged" TabIndex="7" Text="Pago" Visible="False" />
                                                                    <asp:CheckBox ID="ChkLlamar" runat="server" AutoPostBack="True" OnCheckedChanged="ChkLlamar_CheckedChanged" TabIndex="8" Text="LLamar" Visible="False" />
                                                                    <asp:CheckBox ID="ChkEfectivo" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEfectivo_CheckedChanged" TabIndex="9" Text="No Efectivo" Visible="False" />
                                                                    <asp:CheckBox ID="ChkComisiona" runat="server" TabIndex="10" Text="Comisiona" Visible="False" />
                                                                </td>
                                                                <td style="text-align: center"></td>
                                                                <td style="text-align: center"></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td></td>
                                                                <td></td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <h5 runat="server" id="LblDescripcion">Descripción</h5>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:TextBox ID="TxtDescripcion" runat="server" MaxLength="250" CssClass="form-control upperCase" TabIndex="6" Width="100%"></asp:TextBox>
                                                                </td>
                                                                <td style="text-align: center">
                                                                    <asp:CheckBox ID="ChkContacto" runat="server" AutoPostBack="True" Checked="True" OnCheckedChanged="ChkContacto_CheckedChanged" TabIndex="9" Text="Directo" Visible="False" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <asp:Panel ID="PnlDiv0" runat="server" Height="20px"></asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: center"></td>
                                                                <td style="text-align: center">
                                                                    <asp:ImageButton ID="ImgAdd" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAdd_Click" TabIndex="11" />
                                                                </td>
                                                                <td style="text-align: left"></td>
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
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="panel panel-default">
                    <asp:UpdatePanel ID="UpdOpciones" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: center; width: 45%">
                                        <asp:Button ID="BtnGrabar" runat="server" CssClass="button" TabIndex="12" Text="Grabar" Width="120px" OnClick="BtnGrabar_Click" Visible="False" />
                                    </td>
                                    <td style="width: 10%"></td>
                                    <td style="text-align: center; width: 45%">
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
