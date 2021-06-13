<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_EvaluacionAdmin.aspx.cs" Inherits="SoftCob.Views.Evaluacion.WFrm_EvaluacionAdmin" %>

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
            <div class="panel-body">
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
                                    <td style="width: 60%">
                                        <asp:Panel ID="PnlArbolProtocolo" runat="server" Height="550px" ScrollBars="Vertical">
                                            <asp:TreeView ID="TrvProtocolos" runat="server" ImageSet="Arrows" OnSelectedNodeChanged="TrvProtocolos_SelectedNodeChanged" TabIndex="1">
                                                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" Font-Bold="true" />
                                                <Nodes>
                                                    <asp:TreeNode SelectAction="Expand" Text="Protocolo-Evaluación" Value="0"></asp:TreeNode>
                                                </Nodes>
                                                <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                                <ParentNodeStyle ForeColor="Black" />
                                                <SelectedNodeStyle Font-Underline="True" ForeColor="Blue" HorizontalPadding="0px" VerticalPadding="0px" />
                                            </asp:TreeView>
                                        </asp:Panel>
                                    </td>
                                    <td style="width: 40%">
                                        <table style="width: 100%">
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h4 runat="server" id="LblProtocolo" style="color: brown;"></h4>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="PnlProtocolos" runat="server" Height="230px"
                                                        GroupingText="Datos">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 34%"></td>
                                                                <td style="width: 33%"></td>
                                                                <td style="width: 33%"></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <h5>Descripción</h5>
                                                                </td>
                                                                <td></td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="TxtDescripcion" runat="server" CssClass="form-control upperCase" onkeydown="return (event.keyCode!=13);" TabIndex="1" Width="100%" TextMode="MultiLine" MaxLength="255" Height="80px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <h5>Calificación</h5>
                                                                </td>
                                                                <td></td>
                                                                <td>
                                                                    <h5 runat="server" id="LblEstado" visible="false">Estado</h5>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="TxtCalificacion" runat="server" CssClass="form-control alinearDerecha" Width="100%" TabIndex="2" MaxLength="2">0</asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="TxtCalificacion_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="TxtCalificacion">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </td>
                                                                <td></td>
                                                                <td>
                                                                    <asp:CheckBox ID="ChkEtado" runat="server" CssClass="form-control" AutoPostBack="True" TabIndex="3" Text="Activo" Visible="False" OnCheckedChanged="ChkEtado_CheckedChanged" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Panel ID="PnlDiv0" runat="server" Height="20px"></asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: center">
                                                                    <asp:ImageButton ID="ImgAdd" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAdd_Click" TabIndex="4" Visible="False" />
                                                                </td>
                                                                <td style="text-align: center">
                                                                    <asp:ImageButton ID="ImgMod" runat="server" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgMod_Click" TabIndex="5" Visible="False" />
                                                                </td>
                                                                <td style="text-align: center">
                                                                    <asp:ImageButton ID="ImgDel" runat="server" Height="25px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgDel_Click" TabIndex="6" Visible="False" OnClientClick="return asegurar();" />
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
                                        <asp:Button ID="BtnGragar" runat="server" CssClass="button" TabIndex="7" Text="Grabar" Width="120px" OnClick="BtnGragar_Click" />
                                    </td>
                                    <td style="width: 10%"></td>
                                    <td style="text-align: center; width: 45%">
                                        <asp:Button ID="BtnSalir" runat="server" CssClass="button" TabIndex="8" Text="Salir" Width="120px" OnClick="BtnSalir_Click" />
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

