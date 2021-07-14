<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_DocumentosView.aspx.cs" Inherits="SoftCob.Views.BPM.WFrm_DocumentosView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
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
                <table style="width:100%">
                    <tr>
                        <td style="width:10%"></td>
                        <td style="width:80%"></td>
                        <td style="width:10%"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Panel ID="Panel1" runat="server" Height="450px" ScrollBars="Vertical">
                                <asp:Image ID="Imgdocumento" runat="server" Height="100%" Width="100%" />
                            </asp:Panel>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
