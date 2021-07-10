<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_MenuEdit.aspx.cs" Inherits="SoftCob.Views.Menu.WFrm_MenuEdit" %>

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
                                <td style="width: 10%"></td>
                                <td style="width: 20%">
                                    <h5>Nombre del Menú:</h5>
                                </td>
                                <td style="width: 60%">
                                    <asp:TextBox ID="TxtNombreMenu" runat="server" Width="100%" MaxLength="80" TabIndex="1"></asp:TextBox>
                                </td>
                                <td style="width: 10%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Estado" CssClass="Label"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkEstado" runat="server" Checked="True" Text="Activo" OnCheckedChanged="ChkEstado_CheckedChanged" AutoPostBack="True" CssClass="form-control" Width="50%" TabIndex="2" />
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
            <hr />
            <table style="height: 100%; width: 100%">
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 90%"></td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="GrdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                    CssClass="table table-condensed table-bordered table-hover table-responsive" EmptyDataText="No existe ningún menú agregado" DataKeyNames="Codigo,Selecc" PageSize="100" TabIndex="3">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="Descripcion" HeaderText="Tarea"></asp:BoundField>
                                        <asp:BoundField DataField="RutaPagina" HeaderText="Ruta/Página aspx"></asp:BoundField>
                                        <asp:BoundField DataField="Estado" HeaderText="Estado"></asp:BoundField>
                                        <asp:TemplateField HeaderText="Subir Nivel">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgSubirNivel" runat="server" Height="20px" ImageUrl="~/Botones/activada_upbg.png" OnClick="ImgSubirNivel_Click" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bajar Nivel">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgBajarNivel" runat="server" Height="20px" ImageUrl="~/Botones/activada_downbg.png" OnClick="ImgBajarNivel_Click" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
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
                                    <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" OnClick="BtnGrabar_Click" CssClass="button" TabIndex="6" />
                                </td>
                                <td style="width: 10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" OnClick="BtnSalir_Click" CssClass="button" TabIndex="7" />
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
