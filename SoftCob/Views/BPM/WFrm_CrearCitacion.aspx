<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_CrearCitacion.aspx.cs" Inherits="SoftCob.Views.BPM.WFrm_CrearCitacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nuevos Campos Estrategia</title>
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <link href="../../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <link href="../../JS/css/alertify.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="../../Style/chosen.css" />

    <script type="text/javascript" src="../../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../../JS/alertify.min.js"></script>

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

        .ChildGrid td {
            background-color: #eee !important;
            color: black;
            font-size: 8pt;
            line-height: 200%
        }

        .ChildGrid th {
            background-color: #6C6C6C !important;
            color: White;
            font-size: 10pt;
            line-height: 200%
        }
    </style>
    <script type="text/javascript">
        $("[src*=agregar]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../../Botones/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../../Botones/agregar.png");
            $(this).closest("tr").next().remove();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div class="panel panel-primary">
            <div class="panel-heading">
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
                                    <td style="width: 10%"></td>
                                    <td style="width: 40%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 5%"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td colspan="3">
                                        <asp:Panel ID="PnlDatosDeudor" runat="server" CssClass="panel panel-primary" Height="200px"
                                            GroupingText="Datos Deudor" TabIndex="15">
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
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Panel ID="pnlDivision" runat="server" Height="10px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td colspan="3">
                                        <asp:Panel ID="PnlDatosGetion" runat="server" CssClass="panel panel-primary" Height="200px"
                                            GroupingText="Datos Operación" ScrollBars="Vertical" TabIndex="17">
                                            <asp:GridView ID="GrdvDatosObligacion" runat="server" AutoGenerateColumns="False"
                                                CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333"
                                                PageSize="5" TabIndex="2" Width="100%" DataKeyNames="Operacion,DiasMora" OnRowDataBound="GrdvDatosObligacion_RowDataBound" ShowFooter="True">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                                    <asp:BoundField DataField="Operacion" HeaderText="Operación"></asp:BoundField>
                                                    <asp:BoundField DataField="HDiasMora" HeaderText="H.Mora">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ValorDeuda" HeaderText="Valor Deuda">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Exigible" HeaderText="Exigible">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
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
                                    <td></td>
                                    <td colspan="3">
                                        <asp:Panel ID="Panel2" runat="server" Height="10px"></asp:Panel>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr runat="server" id="TrGarantes" visible="false">
                                    <td></td>
                                    <td colspan="3">
                                        <asp:Panel ID="PnlDatosGarante" runat="server" CssClass="panel panel-primary" Height="200px"
                                            GroupingText="Datos Garante" ScrollBars="Vertical" TabIndex="17">
                                            <asp:GridView ID="GrdvDatosGarante" runat="server" AutoGenerateColumns="False"
                                                CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333"
                                                PageSize="3" TabIndex="16" Width="100%" DataKeyNames="CedulaGarante,Existe,CodigoGARA,Operacion">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo"></asp:BoundField>
                                                    <asp:BoundField DataField="CedulaGarante" HeaderText="Cédula" />
                                                    <asp:BoundField DataField="Garante" HeaderText="Nombres"></asp:BoundField>
                                                    <asp:BoundField DataField="Operacion" HeaderText="Operación"></asp:BoundField>
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
                            <table style="width: 100%" runat="server" id="TblHistorial" visible="false">
                                <tr>
                                    <td style="width: 5%"></td>
                                    <td style="width: 20%"></td>
                                    <td style="width: 5%"></td>
                                    <td style="width: 65%"></td>
                                    <td style="width: 5%"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td colspan="3">
                                        <asp:Panel ID="Panel1" runat="server" CssClass="panel panel-primary" Height="290px" GroupingText="Histórico Citación" ScrollBars="Vertical" TabIndex="17">
                                            <asp:GridView ID="GrdvCitaciones" runat="server" AutoGenerateColumns="False"
                                                CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333"
                                                PageSize="5" TabIndex="7" Width="100%" DataKeyNames="Canal,CodigoCITA" Height="25px" OnRowDataBound="GrdvCitaciones_RowDataBound">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <img alt="" style="cursor: pointer" src="../../Botones/agregarbg.png" width="20" height="20" id="btnplus" />
                                                            <asp:Panel runat="server" Style="display: none">
                                                                <asp:GridView ID="GrdvCanales" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-bordered table-hover table-responsive">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="TipoCliente" HeaderText="Tipo" />
                                                                        <asp:BoundField DataField="Definicion" HeaderText="Definicion" />
                                                                        <asp:BoundField DataField="Celular" HeaderText="Celular" />
                                                                        <asp:BoundField DataField="Email" HeaderText="Email" />
                                                                        <asp:BoundField DataField="Direccion" HeaderText="Direccion" />
                                                                        <asp:BoundField DataField="Referencia" HeaderText="Referencia" />
                                                                        <asp:BoundField DataField="Sector" HeaderText="Sector" />
                                                                        <asp:BoundField DataField="FechaVisita" HeaderText="Fecha Visita" />
                                                                        <asp:BoundField DataField="RespuestaGeneral" HeaderText="Respuesta" />
                                                                        <asp:BoundField DataField="Registro" HeaderText="Observación" />
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="GVFixedHeader" Font-Bold="True" ForeColor="White" />
                                                                    <RowStyle Font-Size="X-Small" />
                                                                    <EditRowStyle BackColor="#2461BF" />
                                                                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Logo">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgLogo" runat="server" Height="20px" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Canal" HeaderText="Canal" />
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
                                    <td></td>
                                    <td colspan="3">
                                        <asp:Panel ID="Panel3" runat="server" Height="10px"></asp:Panel>
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 5%"></td>
                                    <td style="width: 15%"></td>
                                    <td style="width: 15%"></td>
                                    <td style="width: 35%"></td>
                                    <td style="width: 25%" colspan="2"></td>
                                    <td style="width: 5%"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td style="text-align: right">
                                        <h5>Valor Citación:</h5>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="TxtValor" runat="server" CssClass="form-control alinearDerecha" MaxLength="9" TabIndex="4"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:CheckBox ID="ChkWathaspp" runat="server" Text="Whatsapp" TabIndex="5" AutoPostBack="True" OnCheckedChanged="ChkWathaspp_CheckedChanged" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td colspan="2"></td>
                                    <td></td>
                                </tr>
                                <tr runat="server" id="TrWhast1" visible="false">
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <h5>Observación:</h5>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="TxtObservaWhatsapp" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="form-control upperCase" MaxLength="500" TabIndex="6" Width="100%" Height="50px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr runat="server" id="TrWhast2" visible="false">
                                    <td colspan="7">
                                        <asp:Panel ID="Panel5" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr runat="server" id="TrWhast3" visible="false">
                                    <td></td>
                                    <td colspan="5">
                                        <asp:Panel ID="Panel4" runat="server" CssClass="panel panel-primary"
                                            GroupingText="Celulares Agregados" Height="350px" ScrollBars="Vertical" TabIndex="17">
                                            <asp:GridView ID="GrdvCelulares" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ForeColor="#333333" PageSize="5" TabIndex="7" Width="100%" DataKeyNames="Codigo">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo Cliente" />
                                                    <asp:BoundField DataField="Celular" HeaderText="Celular" />
                                                    <asp:TemplateField HeaderText="Observación">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TxtObservaCelular" runat="server" CssClass="form-control upperCase" MaxLength="150" Width="100%"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Solicitar">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ChkCelular" runat="server" AutoPostBack="True" OnCheckedChanged="ChkCelular_CheckedChanged" />
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
                                    <td></td>
                                    <td>
                                        <asp:CheckBox ID="ChkEmail" runat="server" Text="Email" TabIndex="8" OnCheckedChanged="ChkEmail_CheckedChanged" AutoPostBack="True" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td colspan="2"></td>
                                    <td></td>
                                </tr>
                                <tr runat="server" id="TrEmail1" visible="false">
                                    <td></td>
                                    <td></td>
                                    <td>Tipo:</td>
                                    <td>
                                        <asp:DropDownList ID="DdlTipoMail" runat="server" CssClass="form-control" TabIndex="9" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:RadioButtonList ID="RdbEmail" runat="server" AutoPostBack="True" CellSpacing="10" RepeatDirection="Horizontal" TabIndex="10">
                                            <asp:ListItem Selected="True" Value="PER">Personal</asp:ListItem>
                                            <asp:ListItem Value="TRA">Trabajo</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr runat="server" id="TrEmail2" visible="false">
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <h5>Email:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control lowCase" MaxLength="100" TabIndex="11" Width="100%"></asp:TextBox>
                                    </td>
                                    <td style="text-align: center" colspan="2">
                                        <asp:ImageButton ID="ImgAgregarMail" runat="server" Height="25px" ImageUrl="~/Botones/agregarbg.png" OnClick="ImgAgregarMail_Click" TabIndex="12" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr runat="server" id="TrEmail3" visible="false">
                                    <td></td>
                                    <td></td>
                                    <td>Observación:</td>
                                    <td colspan="3">
                                        <asp:TextBox ID="TxtObservaEmail" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="form-control upperCase" MaxLength="500" TabIndex="13" Width="100%" Height="50px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr runat="server" id="TrEmail4" visible="false">
                                    <td colspan="7">
                                        <asp:Panel ID="Panel7" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr runat="server" id="TrEmail5" visible="false">
                                    <td></td>
                                    <td colspan="5">
                                        <asp:Panel ID="PnlEmail" runat="server" CssClass="panel panel-primary"
                                            GroupingText="Emails Agregados" Height="350px" ScrollBars="Vertical" TabIndex="17">
                                            <asp:GridView ID="GrdvEmails" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="CodigoMATD,Email,DirIncorrecta" ForeColor="#333333" PageSize="5" TabIndex="14" Width="100%" OnRowDataBound="GrdvEmails_RowDataBound">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo Cliente" />
                                                    <asp:BoundField DataField="Definicion" HeaderText="Correo" />
                                                    <asp:BoundField DataField="Email" HeaderText="Email"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Observación">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TxtObservaCorreo" runat="server" CssClass="form-control upperCase" MaxLength="150" Width="100%"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Solicitar">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ChkSolEmail" runat="server" AutoPostBack="True" OnCheckedChanged="ChkSolEmail_CheckedChanged" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Eliminar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgDelEmail" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgDelEmail_Click" OnClientClick="return asegurar();" />
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
                                    <td></td>
                                    <td>
                                        <asp:CheckBox ID="ChkTerreno" runat="server" Text="Terreno" TabIndex="15" OnCheckedChanged="ChkTerreno_CheckedChanged" AutoPostBack="True" />
                                    </td>
                                    <td></td>
                                    <td colspan="3"></td>
                                    <td></td>
                                </tr>
                                <tr runat="server" id="TrTerreno1" visible="false">
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <h5>Tipo:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlDireccion" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="16" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:RadioButtonList ID="RdbTerreno" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" CellSpacing="10" TabIndex="17">
                                            <asp:ListItem Selected="True" Value="DOM">Domicilio</asp:ListItem>
                                            <asp:ListItem Value="TRA">Trabajo</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr runat="server" id="TrTerreno2" visible="false">
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <h5>Dirección:</h5>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="TxtDireccion" runat="server" CssClass="form-control upperCase" Height="50px" MaxLength="250" onkeydown="return (event.keyCode!=13);" TabIndex="18" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr runat="server" id="TrTerreno3" visible="false">
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <h5>Referencia:</h5>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="TxtReferencia" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="form-control upperCase" MaxLength="250" TabIndex="19" Width="100%" Height="50px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr runat="server" id="TrTerreno8" visible="false">
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <h5>Provincia:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlProvincia" runat="server" AutoPostBack="True" CssClass="form-control chzn-select" OnSelectedIndexChanged="DdlProvincia_SelectedIndexChanged" TabIndex="20" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2"></td>
                                    <td></td>

                                </tr>
                                <tr runat="server" id="TrTerreno9" visible="false">
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <h5>Ciudad:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlCiudad" runat="server" CssClass="form-control chzn-select" TabIndex="21" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2"></td>
                                    <td></td>

                                </tr>
                                <tr runat="server" id="TrTerreno4" visible="false">
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <h5>Sector:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlSector" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="20" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: center">
                                        <asp:ImageButton ID="ImgAgregarTerreno" runat="server" Height="25px" ImageUrl="~/Botones/agregar.png" OnClick="ImgAgregarTerreno_Click" TabIndex="21" />
                                    </td>
                                    <td style="text-align: center">
                                        <asp:ImageButton ID="ImgModificaTerreno" runat="server" Enabled="False" Height="25px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModificaTerreno_Click" TabIndex="23" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr runat="server" id="TrTerreno5" visible="false">
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <h5>Observación:</h5>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="TxtObservaTerreno" runat="server" CssClass="form-control upperCase" Height="50px" MaxLength="500" onkeydown="return (event.keyCode!=13);" TabIndex="22" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr runat="server" id="TrTerreno6" visible="false">
                                    <td colspan="7">
                                        <asp:Panel ID="Panel6" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr runat="server" id="TrTerreno7" visible="false">
                                    <td colspan="7">
                                        <asp:Panel ID="PnlTerreno" runat="server" CssClass="panel panel-primary"
                                            GroupingText="Direcciones Agregadas" Height="350px" ScrollBars="Vertical" TabIndex="22">
                                            <asp:GridView ID="GrdvTerreno" runat="server" AutoGenerateColumns="False"
                                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                ForeColor="#333333" PageSize="5" TabIndex="21" Width="100%"
                                                DataKeyNames="CodigoMATD,CodigoTIPO,DirIncorrecta,CodigoCIUD" OnRowDataBound="GrdvTerreno_RowDataBound">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                                    <asp:BoundField DataField="Definicion" HeaderText="Definicion" />
                                                    <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                                                    <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                                    <asp:BoundField DataField="Sector" HeaderText="Sector" />
                                                    <asp:TemplateField HeaderText="Observación">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TxtObservaTerreno" runat="server" CssClass="form-control upperCase"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Solicitar">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ChkSolTerreno" runat="server" AutoPostBack="True" OnCheckedChanged="ChkSolTerreno_CheckedChanged" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Selecc">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgSelecc" runat="server" Height="20px" ImageUrl="~/Botones/seleccbg.png" OnClick="ImgSelecc_Click" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Eliminar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgDelTerreno" runat="server" Height="20px" ImageUrl="~/Botones/eliminarbg.png" OnClick="ImgDelTerreno_Click" />
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
                                    <td>
                                        <h5>Observación:</h5>
                                    </td>
                                    <td colspan="4">
                                        <asp:TextBox ID="TxtObservacion" runat="server" CssClass="form-control upperCase" Height="80px" MaxLength="500" onkeydown="return (event.keyCode!=13);" TabIndex="23" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                    </td>
                                    <td></td>
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
                                    <td style="text-align: right; width: 45%">
                                        <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="BtnGrabar_Click" TabIndex="24" />
                                    </td>
                                    <td style="width: 5%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="25" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
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
    <script type="text/javascript">
        var cronometro;

        function ValidarDecimales() {
            var numero = document.getElementById("<%=TxtValor.ClientID%>").value;
            if (!/^([0-9])*[.]?[0-9]*$/.test(numero)) {
                alert("El valor " + numero + " no es un número válido. Ejemplo 157.68");
                document.getElementById("<%=TxtValor.ClientID%>").value = "";
                return false;
            }
        }

        function asegurar() {
            rc = confirm("¿Seguro que desea Eliminar?");
            return rc;
        }

    </script>
</body>
</html>
