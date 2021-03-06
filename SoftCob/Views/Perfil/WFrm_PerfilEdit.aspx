<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_PerfilEdit.aspx.cs" Inherits="SoftCob.Views.Perfil.WFrm_PerfilEdit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../JS/css/alertify.min.css" rel="stylesheet" />

    <script src="../../Scripts/external/jquery/jquery.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../JS/alertify.min.js"></script>
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>

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
                                <td style="width: 30%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Perfil:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtPerfil" runat="server" Width="100%" MaxLength="80" CssClass="form-control upperCase" TabIndex="1"></asp:TextBox>
                                </td>
                                <td>
                                    <h5>Descripción:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtDescripcion" runat="server" onkeydown="return (event.keyCode!=13);" Width="100%" Height="60px" CssClass="form-control upperCase" MaxLength="80" TextMode="MultiLine" TabIndex="2"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Panel ID="Panel1" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Crear Parámetros:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkCrear" runat="server" AutoPostBack="True" Text="No" CssClass="form-control" OnCheckedChanged="ChkCrear_CheckedChanged" Width="50%" TabIndex="3" />
                                </td>
                                <td>
                                    <h5>Modificar Parámetros:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkModificar" runat="server" Text="No" AutoPostBack="True" CssClass="form-control" OnCheckedChanged="ChkModificar_CheckedChanged" Width="50%" TabIndex="4" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Eliminar Parámetros:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkEliminar" runat="server" Text="No" AutoPostBack="True" CssClass="form-control" OnCheckedChanged="ChkEliminar_CheckedChanged" Width="50%" TabIndex="5" />
                                </td>
                                <td>
                                    <h5>Califica Perfil:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkPerfil" runat="server" Text="No" AutoPostBack="True" CssClass="form-control" OnCheckedChanged="ChkPerfil_CheckedChanged" Width="50%" TabIndex="6" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Califica Estilos:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkEstilos" runat="server" Text="No" AutoPostBack="True" CssClass="form-control" OnCheckedChanged="ChkEstilos_CheckedChanged" Width="50%" TabIndex="7" />
                                </td>
                                <td>
                                    <h5>Califica Metaprogramas:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkMetaprogramas" runat="server" Text="No" AutoPostBack="True" CssClass="form-control" OnCheckedChanged="ChkMetaprogramas_CheckedChanged" Width="50%" TabIndex="8" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Califica Modalidad:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkModalidad" runat="server" Text="No" AutoPostBack="True" CssClass="form-control" OnCheckedChanged="ChkModalidad_CheckedChanged" Width="50%" TabIndex="9" />
                                </td>
                                <td>
                                    <h5>Califica EstadosdelYO:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkEstadosdelYo" runat="server" Text="No" AutoPostBack="True" CssClass="form-control" OnCheckedChanged="ChkEstadosdelYo_CheckedChanged" Width="50%" TabIndex="10" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Califica Impulsores:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkImpulsores" runat="server" Text="No" AutoPostBack="True" CssClass="form-control" OnCheckedChanged="ChkImpulsores_CheckedChanged" Width="50%" TabIndex="11" />
                                </td>
                                <td>
                                    <h5 runat="server" visible="false" id="Lblestado">Estado:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkEstado" runat="server" AutoPostBack="True" Checked="True" CssClass="form-control" OnCheckedChanged="ChkEstado_CheckedChanged" TabIndex="12" Text="Activo" Visible="false" Width="50%" />
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <table style="height: 100%; width: 100%">
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 90%"></td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <%--<asp:Panel ID="Panel2" runat="server" CssClass="panel panel-primary" Height="420px" ScrollBars="Vertical">--%>
                                <asp:GridView ID="GrdvDatos" runat="server" AutoGenerateColumns="False"
                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                    DataKeyNames="Codigo,Estado,CodigoTARE" EmptyDataText="No existe ningún menú agregado"
                                    OnRowDataBound="GrdvDatos_RowDataBound" TabIndex="7" Width="100%">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="Codigo" HeaderText="Código" Visible="False" />
                                        <asp:BoundField DataField="Menu" HeaderText="Menú" />
                                        <asp:BoundField DataField="SubMenu" HeaderText="SubMenu" />
                                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                        <asp:TemplateField HeaderText="Agregar">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkAgregar" runat="server" AutoPostBack="True" OnCheckedChanged="ChkAgregar_CheckedChanged" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                    <RowStyle Font-Size="X-Small" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                                <script>
                                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                                    prm.add_endRequest(function () {
                                        createDataTable();
                                    });
                                    createDataTable();
                                    function createDataTable() {
                                        $('#<%= GrdvDatos.ClientID %>').DataTable({
                                            "order": [],
                                            "ordering": false
                                        });
                                    }
                                </script>
                                <%--</asp:Panel>--%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td></td>
                </tr>
            </table>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="updBotones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right; width: 45%">
                                    <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="BtnGrabar_Click" TabIndex="14" />
                                </td>
                                <td style="width: 10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="15" />
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
