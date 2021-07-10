<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_NuevoCedente.aspx.cs" Inherits="SoftCob.Views.Cedente.WFrm_NuevoCedente" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/jquery.ui.accordion.css" rel="stylesheet" />
    <link href="../../JS/css/alertify.min.css" rel="stylesheet" />

    <script src="../../Scripts/external/jquery/jquery.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../JS/alertify.min.js"></script>
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <script src="../../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>
    <link rel="stylesheet" href="../../Style/chosen.css" />

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
        $(function () {
            $("#acordionParametro").accordion();
        });
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
            <div class="table-responsive">
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">DATOS CEDENTE</h3>
                <asp:UpdatePanel ID="updDatosCedente" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 15%">
                                    <h5>Provincia:</h5>
                                </td>
                                <td style="width: 30%">
                                    <asp:DropDownList ID="DdlProvincia" runat="server" CssClass="form-control chzn-select" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlProvincia_SelectedIndexChanged" TabIndex="1">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 15%">
                                    <h5>Ciudad:</h5>
                                </td>
                                <td style="width: 30%">
                                    <asp:DropDownList ID="DdlCiudad" runat="server" CssClass="form-control chzn-select" Width="100%" TabIndex="2">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Cedente:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtCedente" runat="server" Width="100%" MaxLength="150" CssClass="form-control upperCase" TabIndex="3"></asp:TextBox>
                                </td>
                                <td>
                                    <h5>RUC:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtRuc" runat="server" CssClass="form-control" Width="100%" MaxLength="13" TabIndex="4"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Dirección:</h5>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="TxtDireccion" runat="server" onkeydown="return (event.keyCode!=13);" Width="100%" MaxLength="80" CssClass="form-control upperCase" TextMode="MultiLine" Height="40px" TabIndex="5"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Telefono_1:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtTelefono1" runat="server" CssClass="form-control" Width="100%" MaxLength="10" TabIndex="6"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtTelefono1_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtTelefono1">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <h5>Teléfono_2:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtTelefono2" runat="server" CssClass="form-control" Width="100%" MaxLength="10" TabIndex="7"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtTelefono2_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtTelefono2">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Fax:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtFax" runat="server" CssClass="form-control" Width="100%" MaxLength="10" TabIndex="8"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtFax_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtFax">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <h5>URL:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtUrl" runat="server" CssClass="form-control" Width="100%" MaxLength="80" TabIndex="9"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtUrl_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtUrl">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Nivel Árbol:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlNivelArbol" runat="server" CssClass="form-control" TabIndex="2" Width="100%">
                                        <asp:ListItem Value="0">--Seleccione Nivel Árbol--</asp:ListItem>
                                        <asp:ListItem Value="3">Nivel 3</asp:ListItem>
                                        <asp:ListItem Value="4">Nivel 4</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5 id="lblEstado" runat="server" visible="false">Estado:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkEstado" runat="server" AutoPostBack="True" Checked="True" CssClass="form-control" OnCheckedChanged="ChkEstado_CheckedChanged" TabIndex="10" Text="Activo" Visible="False" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel-body">
                <div id="acordionParametro">
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Contactos</h3>
                    <asp:UpdatePanel ID="updContactos" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 1%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 9%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Contacto:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtContacto" runat="server" CssClass="form-control upperCase" MaxLength="150" Width="100%" TabIndex="11"></asp:TextBox>
                                        </td>
                                        <td>
                                            <h5>Cargo:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlCargo" runat="server" CssClass="form-control form-control" Width="100%" TabIndex="12">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Ext:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtExt" runat="server" CssClass="form-control" MaxLength="10" Width="100%" TabIndex="13"></asp:TextBox>
                                        </td>
                                        <td>
                                            <h5>Celular:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtCelular" runat="server" CssClass="form-control" MaxLength="10" Width="100%" TabIndex="14"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtCelular_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtCelular">
                                            </asp:FilteredTextBoxExtender>

                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ImgNewContacto" runat="server" Height="25px" ImageUrl="~/Botones/agregarbg.png" OnClick="ImgNewContacto_Click" TabIndex="17" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Email_1:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtEmail1" runat="server" CssClass="form-control lowCase" MaxLength="80" Width="100%" TabIndex="15"></asp:TextBox>
                                        </td>
                                        <td>
                                            <h5>Email_2:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtEmail2" runat="server" CssClass="form-control lowCase" MaxLength="80" Width="100%" TabIndex="16"></asp:TextBox>

                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ImgEditarContacto" runat="server" Height="25px" ImageUrl="~/Botones/modificarbg.png" TabIndex="18" Enabled="False" OnClick="ImgEditarContacto_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td style="text-align: center"></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="5">
                                            <asp:Panel ID="PnlContactos" runat="server" ScrollBars="Vertical" Height="230px">
                                                <asp:GridView ID="GrdvContactos" runat="server" AutoGenerateColumns="False"
                                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                    DataKeyNames="CodigoContacto" ForeColor="#333333" PageSize="5" Width="100%" TabIndex="19">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Contacto" HeaderText="Contacto" />
                                                        <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                                                        <asp:BoundField DataField="Celular" HeaderText="Celular" />
                                                        <asp:BoundField DataField="Ext" HeaderText="Ext." />
                                                        <asp:BoundField DataField="Email1" HeaderText="Email" />
                                                        <asp:TemplateField HeaderText="Editar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgEdiContacto" runat="server" Height="20px" ImageUrl="~/Botones/seleccbg.png" OnClick="ImgEdiContacto_Click" />
                                                            </ItemTemplate>
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
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Productos</h3>
                    <asp:UpdatePanel ID="updProductos" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 1%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 9%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Producto:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="TxtProducto" runat="server" CssClass="form-control upperCase" MaxLength="150" Width="100%" TabIndex="20"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ImgNewProducto" runat="server" Height="25px" ImageUrl="~/Botones/agregarbg.png" TabIndex="22" OnClick="ImgNewProducto_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Descripción:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="TxtDescripcion" runat="server" CssClass="form-control upperCase" MaxLength="250" Width="100%" Height="50px" TextMode="MultiLine" TabIndex="21"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ImgEditarProducto" runat="server" Height="25px" ImageUrl="~/Botones/modificarbg.png" TabIndex="23" OnClick="ImgEditarProducto_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <asp:Panel ID="Panel3" runat="server" Height="30px">
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="5">
                                            <asp:Panel ID="pnlProductos" runat="server" ScrollBars="Vertical" Height="230px">
                                                <asp:GridView ID="GrdvProductos" runat="server" AutoGenerateColumns="False"
                                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                    DataKeyNames="CodigoProducto" ForeColor="#333333" PageSize="5" Width="100%" TabIndex="24" OnRowDataBound="GrdvProductos_RowDataBound">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                                        <asp:TemplateField HeaderText="Activo">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkEstProducto" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEstProducto_CheckedChanged" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Editar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgEdiProducto" runat="server" Height="20px" ImageUrl="~/Botones/seleccbg.png" OnClick="ImgEdiProducto_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgDelProducto" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgDelProducto_Click" />
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
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Catalogo - Productos</h3>
                    <asp:UpdatePanel ID="updCatalogoProductos" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 1%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 9%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Producto:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DdlProducto" runat="server" CssClass="form-control form-control" Width="100%" TabIndex="25">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Código Catálogo:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtCodigoProd" runat="server" CssClass="form-control upperCase" MaxLength="10" Width="100%" TabIndex="26"></asp:TextBox>
                                        </td>
                                        <td>
                                            <h5>Catálogo Producto:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtCatalgoProducto" runat="server" CssClass="form-control upperCase" MaxLength="250" Width="100%" TabIndex="27"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ImgNewCatalogo" runat="server" Height="25px" ImageUrl="~/Botones/agregarbg.png" TabIndex="30" OnClick="ImgNewCatalogo_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Código Familia:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtCodigoFamilia" runat="server" CssClass="form-control upperCase" MaxLength="10" Width="100%" TabIndex="28"></asp:TextBox>
                                        </td>
                                        <td>
                                            <h5>Familia:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtFamilia" runat="server" CssClass="form-control upperCase" MaxLength="250" Width="100%" TabIndex="29"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ImgEditarCatalogo" runat="server" Height="25px" ImageUrl="~/Botones/modificarbg.png" TabIndex="31" OnClick="ImgEditarCatalogo_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:Panel ID="Panel4" runat="server" Height="30px">
                                            </asp:Panel>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="5">
                                            <asp:Panel ID="pnlCatalogoProd" runat="server" ScrollBars="Vertical" Height="230px">
                                                <asp:GridView ID="GrdvCatalogoProd" runat="server" AutoGenerateColumns="False"
                                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                    DataKeyNames="CodigoCatalogo,Producto,CodProducto" ForeColor="#333333" PageSize="5" Width="100%" TabIndex="32" OnRowDataBound="GrdvCatalogoProd_RowDataBound">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                                        <asp:BoundField DataField="CodigoProducto" HeaderText="Cod.Catalogo" />
                                                        <asp:BoundField DataField="CatalogoProducto" HeaderText="Catalogo" />
                                                        <asp:BoundField DataField="Familia" HeaderText="Familia" />
                                                        <asp:TemplateField HeaderText="Activo">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkEstCatalogo" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEstCatalogo_CheckedChanged" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Editar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgEdiCatalogo" runat="server" Height="20px" ImageUrl="~/Botones/seleccbg.png" OnClick="ImgEdiCatalogo_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgDelCatalogo" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgDelCatalogo_Click" />
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
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Agencias</h3>
                    <asp:UpdatePanel ID="updAgencias" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 1%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 9%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Código:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtCodigoAgencia" runat="server" CssClass="form-control upperCase" MaxLength="10" Width="100%" TabIndex="33"></asp:TextBox>
                                        </td>
                                        <td>
                                            <h5>Agencia:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtAgencia" runat="server" CssClass="form-control upperCase" MaxLength="250" Width="100%" TabIndex="34"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ImgNewAgencia" runat="server" Height="25px" ImageUrl="~/Botones/agregarbg.png" TabIndex="37" OnClick="ImgNewAgencia_Click" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Sucursal:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlSucursal" runat="server" CssClass="form-control" Width="100%" TabIndex="35">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <h5>Zona:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlZona" runat="server" CssClass="form-control" Width="100%" TabIndex="36">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ImgEditarAgencia" runat="server" Height="25px" ImageUrl="~/Botones/modificarbg.png" TabIndex="38" OnClick="ImgEditarAgencia_Click" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:Panel ID="Panel5" runat="server" Height="30px">
                                            </asp:Panel>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="5">
                                            <asp:Panel ID="pnlAgencias" runat="server" ScrollBars="Vertical" Height="230px">
                                                <asp:GridView ID="GrdvAgencias" runat="server" AutoGenerateColumns="False"
                                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                    DataKeyNames="AgenCodigo" ForeColor="#333333" PageSize="5" Width="100%" TabIndex="39" OnRowDataBound="GrdvAgencias_RowDataBound">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Agencia" HeaderText="Agencia" />
                                                        <asp:BoundField DataField="CodigoAgencia" HeaderText="Codigo" />
                                                        <asp:BoundField DataField="Sucursal" HeaderText="Sucursal" />
                                                        <asp:BoundField DataField="Zona" HeaderText="Zona" />
                                                        <asp:TemplateField HeaderText="Activo">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkEstAgencias" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEstAgencias_CheckedChanged" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Editar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgEdiAgencias" runat="server" Height="20px" ImageUrl="~/Botones/seleccbg.png" OnClick="ImgEdiAgencias_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgDelAgencias" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgDelAgencias_Click" />
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
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="updOpciones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 45%; text-align: right;">
                                    <asp:Button ID="BtnGrabar" runat="server" CssClass="button" Text="Grabar" Width="120px" TabIndex="40" OnClick="BtnGrabar_Click" />
                                </td>
                                <td style="width: 10%"></td>
                                <td style="width: 45%; text-align: left;">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="41" OnClick="BtnSalir_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <script src="../../Scripts/chosen.jquery.js" type="text/javascript"></script>
        <script type="text/javascript"> $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); </script>
        <script>
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
            function endRequestHandler() {
                $(".chzn-select").chosen({ width: "100%" });
                $(".chzn-container").css({ "width": "100%" });
                $(".chzn-drop").css({ "width": "95%" });
            }
        </script>
    </form>
</body>
</html>
