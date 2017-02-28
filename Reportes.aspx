<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Reportes.aspx.cs" Inherits="Reportes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="/Styles/Reportes.css" rel="stylesheet" type="text/css" />

    <table style="width: 100%;">
        <tr>
            <td>&nbsp;</td>
            <td class="ancho">&nbsp;</td>
        </tr>
        <tr>
            <td>
                <span class="titulo">
                    <strong>Reportes</strong>
                </span></td>
            <td class="ancho">&nbsp;</td>
        </tr>
        <tr>
            <td>
                <br />
                <br />
            </td>
            <td class="ancho">
                <asp:DropDownList ID="ddlReporte" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
                    Style="margin-left: 0px">
                    <asp:ListItem>Reporte anual</asp:ListItem>
                    <asp:ListItem>Reporte entre fechas</asp:ListItem>
                    <asp:ListItem>Reporte por cursos</asp:ListItem>
                    <asp:ListItem>Reporte por empresa</asp:ListItem>
                    <asp:ListItem>Aprobados de este año</asp:ListItem>
                    <asp:ListItem>Pendientes de este año</asp:ListItem>
                   
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td class="ancho">
                <asp:Label ID="lblAno" runat="server" Style="color: #336699" Text="Año:"></asp:Label>
                <asp:DropDownList ID="ddlAno" runat="server"
                    OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged1">
                </asp:DropDownList>
                <asp:Label ID="lblCurso" runat="server" Style="color: #336699" Text="Curso:"
                    Visible="False"></asp:Label>
                <asp:DropDownList ID="ddlCurso" runat="server" Visible="False"
                    OnSelectedIndexChanged="ddlCurso_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Label ID="lblFecha1" runat="server" Text="Fecha 1:" Style="color: #336699"
                    Visible="False"></asp:Label>
                <asp:TextBox ID="txtFecha1" runat="server" Visible="False"
                    OnTextChanged="txtFecha1_TextChanged"></asp:TextBox>

                <asp:ImageButton ID="imgCal1" runat="server"  CssClass="calendario"
                    ImageUrl="~/Imagenes/calendario.jpg" OnClick="ImageButton1_Click1"
                    Visible="False" />

                <asp:Label ID="lblFecha2" runat="server" Text="Fecha 2:" Style="color: #336699"
                    Visible="False"></asp:Label>
                <asp:TextBox ID="txtFecha2" runat="server" Visible="False"></asp:TextBox>

                <asp:ImageButton ID="ImgCal2" runat="server"  CssClass="calendario"
                    ImageUrl="~/Imagenes/calendario.jpg" OnClick="ImageButton1_Click2" 
                    EnableTheming="True" Visible="False" />

                <asp:Label ID="lblEmpresa" runat="server" Style="color: #336699"
                    Text="Empresa:" Visible="False"></asp:Label>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                </asp:ToolkitScriptManager>
                <asp:TextBox ID="txtEmpresa" runat="server"
                    OnTextChanged="txtEmpresa_TextChanged" Visible="False"></asp:TextBox>
                <asp:AutoCompleteExtender ID="txtEmpresa_AutoCompleteExtender"
                    runat="server" TargetControlID="txtEmpresa" UseContextKey="True" CompletionInterval="100"
                    MinimumPrefixLength="1" ServiceMethod="GetCompletionList"
                    CompletionListCssClass="autocomplete_completionListElement">
                </asp:AutoCompleteExtender>

                <asp:Button ID="btnReporte" runat="server" OnClick="Button1_Click"
                    Text="Reporte" />
                <asp:Button ID="btnEx" runat="server" OnClick="btnExcel_Click"
                    Text="Generar Excel" Visible="False" />
                &nbsp;
                <asp:Label ID="lblError" runat="server" Style="color: #FF0000" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td class="ancho">
                <asp:GridView ID="gvReportes" runat="server" BackColor="White"
                    BorderColor="#336699" BorderStyle="None" BorderWidth="1px" CellPadding="4"
                    Style="text-align: center; color: #336699" Width="800px">
                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                    <HeaderStyle BackColor="#336699" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                    <RowStyle BackColor="White" ForeColor="#336699" />
                    <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                    <SortedAscendingCellStyle BackColor="#EDF6F6" />
                    <SortedAscendingHeaderStyle BackColor="#336699" />
                    <SortedDescendingCellStyle BackColor="#D6DFDF" />
                    <SortedDescendingHeaderStyle BackColor="#336699" />
                </asp:GridView>
                <asp:Calendar ID="Cal1" runat="server" BackColor="White"
                    BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest"
                    Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px"
                    Width="200px" OnSelectionChanged="Cal1_SelectionChanged" Visible="False">
                    <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                    <NextPrevStyle VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#808080" />
                    <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                    <SelectorStyle BackColor="#CCCCCC" />
                    <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                    <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <WeekendDayStyle BackColor="#FFFFCC" />
                </asp:Calendar>
                <asp:Calendar ID="Cal2" runat="server" BackColor="White" BorderColor="#999999"
                    CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt"
                    ForeColor="Black" Height="180px" OnSelectionChanged="Cal2_SelectionChanged"
                    Visible="False" Width="200px">
                    <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                    <NextPrevStyle VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#808080" />
                    <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                    <SelectorStyle BackColor="#CCCCCC" />
                    <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                    <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <WeekendDayStyle BackColor="#FFFFCC" />
                </asp:Calendar>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td class="ancho">
                <asp:DropDownList ID="ddlEmpresas" runat="server" Visible="False">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td class="ancho">&nbsp;</td>
        </tr>
    </table>
</asp:Content>

