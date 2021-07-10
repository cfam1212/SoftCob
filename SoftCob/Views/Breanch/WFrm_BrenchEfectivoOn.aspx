<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_BrenchEfectivoOn.aspx.cs" Inherits="SoftCob.Views.Breanch.WFrm_BrenchEfectivoOn" %>

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
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>

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
          <%--  <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdOpciones">
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
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="panel panel-default">

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 5%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 15%"></td>
                                    <td style="width: 20%"></td>
                                    <td style="width: 20%"></td>
                                    <td style="width: 20%"></td>
                                    <td style="width: 10%"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Cedente:</h5>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="DdlCedente" runat="server" CssClass="form-control" Width="100%" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="DdlCedente_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <h5>Catálogo:</h5>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="DdlCatalogo" runat="server" CssClass="form-control" TabIndex="2" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Gestor:</h5>
                                    </td>
                                    <td colspan="2">

                                        <asp:DropDownList ID="DdlGestores" runat="server" CssClass="form-control" Width="100%" TabIndex="3">
                                        </asp:DropDownList>

                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlTipoDocumento" runat="server" CssClass="form-control" TabIndex="4" Width="100%">
                                            <asp:ListItem Value="0">--Seleccione Tipo--</asp:ListItem>
                                            <asp:ListItem Value="1">Identificación</asp:ListItem>
                                            <asp:ListItem Value="2">Operación</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="TxtDocumento" runat="server" CssClass="form-control" Width="100%" TabIndex="5"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="panel panel-default" runat="server" id="DivPagos" visible="false">
                            <asp:Panel ID="PnlPagos" runat="server" Height="200px" ScrollBars="Vertical">
                                <asp:GridView ID="GrdvPagos" runat="server" AutoGenerateColumns="False"
                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                    DataKeyNames="Codigo,CodigoGEST,Operacion"
                                    ForeColor="#333333" PageSize="7" TabIndex="8" Width="100%">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha_Registro">
                                            <ItemStyle Wrap="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Gestor" HeaderText="Gestor"></asp:BoundField>
                                        <asp:BoundField DataField="Operacion" HeaderText="Operación" />
                                        <asp:BoundField DataField="FechaPago" HeaderText="Fecha_Pago"></asp:BoundField>
                                        <asp:BoundField DataField="Valor" HeaderText="Valor">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Efetivo">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkEfectivo" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEfectivo_CheckedChanged" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gestiones">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgGestiones" runat="server" Height="20px" ImageUrl="~/Botones/buscarbg.png" OnClick="ImgGestiones_Click" />
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
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel-body">
                <div class="panel panel-default">
                    <asp:UpdatePanel ID="UpdOpciones" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 45%">
                                        <asp:Button ID="BtnProcesar" runat="server" Text="Procesar" Width="120px" CssClass="button" TabIndex="6" OnClick="BtnProcesar_Click" />
                                    </td>
                                    <td style="width: 10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="7" OnClick="BtnSalir_Click" />
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
