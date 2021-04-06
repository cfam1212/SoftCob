<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_AccionConsulta.aspx.cs" Inherits="SoftCob.Views.ConsultasManager.WFrm_AccionConsulta" %>

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
    <script src="../../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../../Scripts/jquery-ui.min.js"></script>
    <link href="../../css/jquery.ui.accordion.css" rel="stylesheet" />

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
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">DATOS CLIENTE</h3>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 5%"></td>
                        <td style="width: 25%"></td>
                        <td style="width: 20%"></td>
                        <td style="width: 20%"></td>
                        <td style="width: 25%"></td>
                        <td style="width: 5%"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="4">
                            <asp:UpdatePanel ID="updDatosDeudor" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlDatosDeudor" runat="server" CssClass="panel panel-primary" Height="200px" GroupingText="Datos Deudor">
                                        <asp:GridView ID="GrdvDatosDeudor" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333"
                                            PageSize="3" TabIndex="1" Width="100%">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="Cedula" HeaderText="Cédula" />
                                                <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                                                <asp:BoundField DataField="Provincia" HeaderText="Provincia" />
                                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                                <asp:BoundField DataField="Edad" HeaderText="Edad" />
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
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="4">
                            <asp:UpdatePanel ID="updDatosObligacion" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlDatosObligacion" runat="server" Height="200px" ScrollBars="Vertical" GroupingText="Datos Obligación" TabIndex="5">
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="GrdvDatosObligacion" runat="server" AutoGenerateColumns="False"
                                                        CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333" PageSize="5" TabIndex="2"
                                                        Width="100%" EmptyDataText="No Existen Registros" DataKeyNames="Operacion,GestorAsignado">
                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                        <Columns>
                                                            <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                                            <asp:BoundField DataField="Operacion" HeaderText="Operación" />
                                                            <asp:BoundField DataField="DiasMora" HeaderText="Días Mora">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Definicion" HeaderText="Definición" />
                                                            <asp:BoundField DataField="MontoOriginal" HeaderText="Cupo Original">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Monto GS" DataField="MontoGSPBO">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Exigible" DataField="Exigible">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="HDiasMora" HeaderText="H.Dias Mora">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Gestor" HeaderText="Gestor" />
                                                            <asp:TemplateField HeaderText="Selecc">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgSelecc" runat="server" Height="20px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSelecc_Click" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                                        <RowStyle Font-Size="X-Small" />
                                                        <EditRowStyle BackColor="#2461BF" />
                                                        <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
            <div class="panel-body">
                <div id="acordionParametro">
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Cambiar Operación</h3>
                    <asp:UpdatePanel ID="updCambiarOperacion" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 20%">
                                            <h5>Asignar a:</h5>
                                        </td>
                                        <td style="width: 45%">
                                            <asp:DropDownList ID="DdlAsignar" runat="server" CssClass="form-control" Width="100%" TabIndex="3">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 25%"></td>
                                        <td style="width: 5%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Motivo:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlMotivo1" runat="server" CssClass="form-control" Width="100%" TabIndex="4">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Observación:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtObservacion1" runat="server" CssClass="form-control upperCase" Height="50px" MaxLength="255" TextMode="MultiLine" Width="100%" TabIndex="5"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:Button ID="BtnCambiar" runat="server" CausesValidation="False" CssClass="button" Text="Cambiar" Width="120px" OnClick="BtnCambiar_Click" TabIndex="6" />
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Eliminar Teléfono</h3>
                    <asp:UpdatePanel ID="updEliminarTelefono" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 20%"></td>
                                        <td style="width: 45%"></td>
                                        <td style="width: 10%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 5%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="4">
                                            <asp:Panel ID="pnlTelefonos" runat="server" Height="220px" ScrollBars="Vertical" GroupingText="Teléfonos">
                                                <asp:GridView ID="GrdvTelefonos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,Telefono,Estado" ForeColor="#333333" PageSize="5" TabIndex="7" Width="100%" OnRowDataBound="GrdvTelefonos_RowDataBound">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                                                        <asp:BoundField DataField="Prefijo" HeaderText="Prefijo" />
                                                        <asp:BoundField DataField="Propietario" HeaderText="Propietario" />
                                                        <asp:BoundField DataField="NomApe" HeaderText="Nombres" />
                                                        <asp:TemplateField HeaderText="Estado">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkEstado" runat="server" OnCheckedChanged="ChkEstado_CheckedChanged" AutoPostBack="True" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgEliminaTele" runat="server" Height="15px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgEliminaTele_Click" OnClientClick="return asegurar();" />
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
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <asp:Panel ID="Panel2" runat="server" Height="20px"></asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Motivo:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlMotivo2" runat="server" CssClass="form-control" Width="100%" TabIndex="8">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Observación</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtObservacion2" runat="server" CssClass="upperCase" Height="50px" MaxLength="255" TextMode="MultiLine" Width="100%" TabIndex="9"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center"></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Eliminar Gestión</h3>
                    <asp:UpdatePanel ID="updEliminarGestion" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 20%"></td>
                                        <td style="width: 45%"></td>
                                        <td style="width: 10%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 5%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="4">
                                            <asp:Panel ID="pnlGestiones" runat="server" Height="200px" ScrollBars="Vertical" GroupingText="Gestiones">
                                                <asp:GridView ID="GrdvGestiones" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333" PageSize="5" TabIndex="10" Width="100%" DataKeyNames="codigoGETE,Identificacion,Operacion,codigoGESTOR,Descripcion">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha Registro" />
                                                        <asp:BoundField DataField="Operacion" HeaderText="Operación" />
                                                        <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
                                                        <asp:BoundField DataField="Gestor" HeaderText="Gestor" />
                                                        <asp:BoundField DataField="Respuesta" HeaderText="Respuesta" />
                                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                                                        <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgEliminaGes" runat="server" Height="15px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgEliminaGes_Click" OnClientClick="return asegurar();" />
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
                                            <h5>Motivo:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlMotivo3" runat="server" CssClass="form-control" Width="100%" TabIndex="11">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Observación</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtObservacion3" runat="server" CssClass="upperCase" Height="50px" MaxLength="255" TextMode="MultiLine" Width="100%" TabIndex="12"></asp:TextBox>
                                        </td>
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
                                <td style="text-align: center">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="13" />
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
