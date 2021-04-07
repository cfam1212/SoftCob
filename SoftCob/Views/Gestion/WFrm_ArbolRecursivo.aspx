<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_ArbolRecursivo.aspx.cs" Inherits="SoftCob.Views.Gestion.WFrm_ArbolRecursivo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
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
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-info">
                <asp:UpdateProgress ID="UpdProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdCabecera">
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
                <asp:UpdatePanel ID="UpdCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 20%"></td>
                                <td style="width: 60%"></td>
                                <td style="width: 20%"></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ImageButton ID="BtnRegresar" runat="server" ImageUrl="~/Botones/cancelar.jpg" OnClick="BtnRegresar_Click" Height="25px" />
                                </td>
                                <td>
                                    <asp:Label ID="LblNombres" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="#3366FF"></asp:Label>
                                </td>
                                <td>
                                    <h4 runat="server" id="LblCedula"></h4>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="2">
                                    <h4 runat="server" id="LblDireccion"></h4>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Panel ID="Panel5" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Panel ID="Pnldatosdeudor" runat="server" CssClass="panel panel-primary" Height="420px"
                                        GroupingText="Datos Deudor - REGISTRO CIVIL" TabIndex="15">
                                        <asp:GridView ID="Grdvdatosper1" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333"
                                            PageSize="3" TabIndex="1" Width="100%">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="FechaNac" HeaderText="Fec.Nacimiento">
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Edad" HeaderText="Edad" />
                                                <asp:BoundField DataField="EstCivil" HeaderText="Est.Civil" />
                                                <asp:BoundField DataField="TipoCedula" HeaderText="Tipo_Cedula" />
                                                <asp:BoundField DataField="Nivel" HeaderText="Nivel_Estudios" />
                                                <asp:BoundField DataField="Profesion" HeaderText="Profesion" />
                                            </Columns>
                                            <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                            <RowStyle Font-Size="X-Small" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                        <asp:GridView ID="Grdvdatosper2" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333"
                                            PageSize="3" TabIndex="1" Width="100%">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="Conyuge" HeaderText="Conyuge">
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Cedulacy" HeaderText="Cédula" />
                                                <asp:BoundField DataField="Padre" HeaderText="Padre" />
                                                <asp:BoundField DataField="Cedulapa" HeaderText="Cédula" />
                                                <asp:BoundField DataField="Madre" HeaderText="Madre" />
                                                <asp:BoundField DataField="Cedulama" HeaderText="Cédula" />
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
                                <td colspan="3">
                                    <asp:Panel ID="Panel1" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%" runat="server" id="Tbldatosiess" visible="false">
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel2" runat="server" CssClass="panel panel-primary" Height="420px"
                                        GroupingText="Datos IESS" TabIndex="15">
                                        <asp:GridView ID="Grdvdatosiess1" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333"
                                            PageSize="3" TabIndex="1" Width="100%">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="TeleAfi" HeaderText="Teléfono">
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Celular" HeaderText="Celular" />
                                                <asp:BoundField DataField="DirAfi" HeaderText="Dirección" />
                                                <asp:BoundField DataField="Email" HeaderText="Email" />
                                                <asp:BoundField DataField="Ocupacion" HeaderText="Ocupación" />
                                                <asp:BoundField DataField="Salario" HeaderText="Salario" />
                                            </Columns>
                                            <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                            <RowStyle Font-Size="X-Small" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                        <asp:GridView ID="Grdvdatosiess2" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333"
                                            PageSize="3" TabIndex="1" Width="100%">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="Ruc" HeaderText="Ruc">
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Empresa" HeaderText="Empresa" />
                                                <asp:BoundField DataField="TelEmpre" HeaderText="Teléfono" />
                                                <asp:BoundField DataField="FaxExt" HeaderText="Fax" />
                                                <asp:BoundField DataField="DirEmpre" HeaderText="Dirección" />
                                                <asp:BoundField DataField="FechaIng" HeaderText="Fec.Ingreso" />
                                                <asp:BoundField DataField="FechaSal" HeaderText="Fec.Salida" />
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
                                <td>
                                    <asp:Panel ID="Panel3" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%" runat="server" id="TblFonos" visible="false">
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel6" runat="server" CssClass="panel panel-primary" Height="220px"
                                        GroupingText="Teléfonos Encontrados" TabIndex="15" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvTelefonos" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar"
                                            TabIndex="8" DataKeyNames="Cedula,Consultado">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField HeaderText="Tipo" DataField="Tipo" />
                                                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                                                <asp:TemplateField HeaderText="Marcar">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgPhone" runat="server" Height="20px" ImageUrl="~/Botones/call_small.png" OnClick="ImgPhone_Click" />
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
                                <td>
                                    <asp:Panel ID="Panel7" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                        </table>

                        <table style="width: 100%" runat="server" id="Tblarbol" visible="false">
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel4" runat="server" CssClass="panel panel-primary" Height="320px"
                                        GroupingText="Arbol Genealógico" TabIndex="15" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar"
                                            TabIndex="8" DataKeyNames="Cedula,Consultado" OnRowDataBound="GrdvDatos_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField HeaderText="Cédula" DataField="Cedula" />
                                                <asp:BoundField DataField="Nombres" HeaderText="Nombres" />
                                                <asp:BoundField HeaderText="Parentesco" DataField="Parentesco" />
                                                <asp:BoundField HeaderText="Fecha_Fallece" DataField="FechaFallece" />
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="updBotones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: center; width: 100%"></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
