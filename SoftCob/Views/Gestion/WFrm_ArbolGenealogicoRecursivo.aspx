<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_ArbolGenealogicoRecursivo.aspx.cs" Inherits="SoftCob.Views.Gestion.WFrm_ArbolGenealogicoRecursivo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../../Scripts/external/jquery/jquery.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../Scripts/Tables/DataTables.js"></script>
    <script src="../../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>

    <script type="text/javascript">
        function pageLoad(sender, arg) {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#txtFechaInicio').datepicker(
                    {
                        inline: true,
                        dateFormat: "mm/dd/yy",
                        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                        numberOfMonths: 1,
                        showButtonPanel: true,
                        changeMonth: true,
                        changeYear: true,
                        yearRange: "-100:+5"
                    });
            });

            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#txtFechaFin').datepicker(
                    {
                        inline: true,
                        dateFormat: "mm/dd/yy",
                        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                        numberOfMonths: 1,
                        showButtonPanel: true,
                        changeMonth: true,
                        changeYear: true,
                        yearRange: "-100:+5"
                    });
            });
        }
    </script>
    <style type="text/css">
        legend {
            color: darkblue;
            font-size: 10px;
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
                <asp:Label ID="LblTitulo" runat="server"></asp:Label>
            </div>
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="LblError" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
         <%--   <div class="panel-info">
                <asp:UpdateProgress ID="UpdProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdCabecera">
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
                <asp:UpdatePanel ID="UpdCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 90%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Panel ID="PnlDatosGenerales" runat="server" Height="350px"
                                        GroupingText="Información General">
                                        <table style="border: 1px solid #0000FF; width: 100%">
                                            <tr>
                                                <td style="width: 15%"></td>
                                                <td style="width: 35%"></td>
                                                <td style="width: 15%"></td>
                                                <td style="width: 35%"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="Panel1" runat="server" Height="10px"></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td>
                                                    <asp:ImageButton ID="BtnRegresar" runat="server" ImageUrl="~/Botones/cancelarbg.png" OnClick="BtnRegresar_Click" Width="25px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Nombres:</h5>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label ID="LblNombres" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="#3366FF"></asp:Label>
                                                </td>
                                                <td style="text-align: center">
                                                    <h4 runat="server" id="LblCedula"></h4>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Fec.Nacimiento:</h5>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblFecNaci" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <h5>Edad:</h5>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblEdad" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Est.Civil:</h5>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblEsCivil" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <h5>Tipo Cedula:</h5>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblTipoCedula" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Nivel Estudios:</h5>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblNivelEstudios" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <h5>Profesión:</h5>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblProfesion" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Conyuge:</h5>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblConyuge" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <h5>Cedula:</h5>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblCedConyuge" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Padre:</h5>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblPadre" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <h5>Cédula:</h5>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblCedPadre" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Madre:</h5>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblMadre" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <h5>Cédula:</h5>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblCedMadre" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                        <div class="panel-info" runat="server" id="DivDatos" visible="false">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 10%"></td>
                                    <td style="width: 80%"></td>
                                    <td style="width: 10%"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <div runat="server" id="DivTitulo" visible="false">Datos IESS</div>
                                        <div style="overflow: scroll; width: 800px; height: 200px" runat="server" id="DivIess">
                                            <asp:GridView ID="GrdvDatosIESS" runat="server" Width="100%" AutoGenerateColumns="False"
                                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar"
                                                TabIndex="8">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField HeaderText="Teléfono" DataField="TeleAfi" />
                                                    <asp:BoundField DataField="Celular" HeaderText="Celular" />
                                                    <asp:BoundField HeaderText="Direccion" DataField="DirAfi">
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Email" DataField="Email" />
                                                    <asp:BoundField DataField="Empresa" HeaderText="Empresa">
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Ruc" HeaderText="Ruc" />
                                                    <asp:BoundField DataField="TelEmpre" HeaderText="Teléfono" />
                                                    <asp:BoundField DataField="FaxExt" HeaderText="Fax/Ext" />
                                                    <asp:BoundField DataField="DirEmpre" HeaderText="Direccion">
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FechaIng" HeaderText="Fec.Ingreso" />
                                                    <asp:BoundField DataField="FechaSal" HeaderText="Fec.Salida" />
                                                    <asp:BoundField DataField="Ocupacion" HeaderText="Ocupación">
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Salario" HeaderText="Salario" />
                                                </Columns>
                                                <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                                <RowStyle Font-Size="X-Small" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Panel ID="Panel2" runat="server" Height="10px"></asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="panel-info" runat="server" id="DivArbol" visible="false">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 5%"></td>
                                    <td style="width: 90%"></td>
                                    <td style="width: 5%"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:Panel ID="PnlArbol" runat="server" Height="200px" ScrollBars="Vertical" GroupingText="Árbol Genealógico">
                                            <asp:GridView ID="GrdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar"
                                                TabIndex="8" DataKeyNames="Cedula">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField HeaderText="Cédula" DataField="Cedula" />
                                                    <asp:BoundField DataField="Nombres" HeaderText="Nombres" />
                                                    <asp:BoundField HeaderText="Parentesco" DataField="Parentesco" />
                                                    <asp:BoundField HeaderText="FechaFallece" DataField="FechaFallece" />
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
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="updBotones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: center; width: 100%">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="9" OnClick="BtnSalir_Click" />
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
