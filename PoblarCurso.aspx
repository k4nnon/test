<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PoblarCurso.aspx.cs" Inherits="PoblarCurso"%>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style2
        {
            width: 147px;
            text-align: right;
        }
        .style3
        {
            width: 147px;
            font-size: large;
            color: #336699;
            text-align: left;
        }
        .style4
        {
            width: 147px;
            height: 21px;
            text-align: right;
        }
        .style5
        {
            height: 21px;
            width: 166px;
        }
        .style6
        {
            width: 147px;
            text-align: right;
            color: #336699;
        }
        .style7
        {
            text-align: left;
        }
        .style8
        {
            height: 21px;
            text-align: left;
        }
        .style10
        {
            height: 21px;
            text-align: right;
        }
        
        .style12
        {
            width: 166px;
        }

        .style13
        {
            width: 147px;
            height: 82px;
            text-align: right;
        }
        .style14
        {
            height: 82px;
            text-align: left;
        }
        .style15
        {
            height: 82px;
            width: 166px;
        }

        </style>
        <script type = "text/javascript">
            window.onload = function () {
                var scrollY = parseInt('<%=Request.Form["scrollY"] %>');
                if (!isNaN(scrollY)) {
                    window.scrollTo(0, scrollY);
                }
            };
            window.onscroll = function () {
                var scrollY = document.body.scrollTop;
                if (scrollY == 0) {
                    if (window.pageYOffset) {
                        scrollY = window.pageYOffset;
                    }
                    else {
                        scrollY = (document.body.parentElement) ? document.body.parentElement.scrollTop : 0;
                    }
                }
                if (scrollY > 0) {
                    var input = document.getElementById("scrollY");
                    if (input == null) {
                        input = document.createElement("input");
                        input.setAttribute("type", "hidden");
                        input.setAttribute("id", "scrollY");
                        input.setAttribute("name", "scrollY");
                        document.forms[0].appendChild(input);
                    }
                    input.value = scrollY;
                }
            };
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table style="width:100%;">
        <tr>
            <td class="style4">
                </td>
            <td class="style10">
                &nbsp;</td>
            <td class="style5">
                </td>
        </tr>
        <tr>
            <td class="style3">
                <strong>Poblar Curso</strong></td>
            <td class="style7">
                &nbsp;</td>
            <td class="style12">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style4">
            </td>
            <td class="style8">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblError" runat="server" style="color: #FF0000"></asp:Label>
                <asp:Label ID="lblFechasResp" runat="server" Visible="False"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
            <td class="style5">
            </td>
        </tr>
        <tr>
            <td class="style6">
                Rut:</td>
            <td class="style7">
                <asp:TextBox ID="txtRut" runat="server" ontextchanged="txtRut_TextChanged" 
                    AutoPostBack="True"></asp:TextBox>
                <asp:Label ID="lblRut" runat="server" style="color: #FF0000" 
                    Text="Rut no valido" Visible="False"></asp:Label>
            </td>
            <td class="style12">
                <asp:Label ID="lblFeVa" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lblId" runat="server"></asp:Label>
                <asp:Label ID="lblIdFecha" runat="server" Visible="False"></asp:Label>
                </td>
        </tr>
        <tr>
            <td class="style6">
                Curso:</td>
            <td class="style7">
                <asp:TextBox ID="txtCurso" runat="server" AutoPostBack="True" 
                    ontextchanged="txtCurso_TextChanged" ReadOnly="True"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" Height="20px" 
                    ImageUrl="~/Imagenes/cursos_logo.png" onclick="ImageButton1_Click" 
                    Width="23px" Enabled="False" />
                <asp:Label ID="lblCursoId" runat="server" Visible="False"></asp:Label>
&nbsp;<asp:GridView ID="gvCurso" runat="server" Width="393px" BackColor="White" 
                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                onrowcommand="gvLista_RowCommand" 
                onselectedindexchanged="gvLista_SelectedIndexChanged" ShowHeader="False" 
                    Height="63px" Visible="False">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                </Columns>
                <FooterStyle BackColor="White" ForeColor="#000066" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <RowStyle ForeColor="#000066" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#00547E" />
            </asp:GridView>
            </td>
            <td class="style12">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style6">
                Cantidad de días:</td>
            <td class="style7">
                <asp:TextBox ID="txtDias" runat="server" AutoPostBack="True" 
                    ontextchanged="txtDias_TextChanged1" Enabled="False"></asp:TextBox>
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
                    Visible="False" 
                    onselectedindexchanged="DropDownList1_SelectedIndexChanged2">
                </asp:DropDownList>
                <asp:DropDownList ID="DropDownList2" runat="server" Visible="False">
                </asp:DropDownList>
            </td>
            <td class="style12">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style6">
                Fecha:</td>
            <td class="style7">
                <asp:TextBox ID="txtFecha" runat="server" ReadOnly="True" 
                    ontextchanged="txtFecha_TextChanged"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" Height="22px" 
                    ImageUrl="~/Imagenes/images.jpg" onclick="ImageButton2_Click" Width="31px" 
                    Enabled="False" />
                &nbsp;<asp:Label ID="lblCompletos" runat="server" style="color: #009900" 
                    Text="Cantidad de días completos" Visible="False"></asp:Label>
                <asp:Label ID="lblDerror" runat="server" style="color: #FF0000" 
                    Text="Día no habíl" Visible="False"></asp:Label>
                &nbsp;<asp:Label ID="lblCompletos2" runat="server" style="color: #FF0000" 
                    Text=" Error, cantidad de dias superior a la indicada,se deseleccionaron las ultimas 2 fechas!" 
                    Visible="False"></asp:Label>
           
                <obout:Calendar ID="calMulti" runat="server" MultiSelectedDates="true" 
                    AutoPostBack="True" ondatechanged="calMulti_DateChanged" Visible="False" 
                    TitleText="Selección de fechas para este registro">
                </obout:Calendar>
            </td>
            <td class="style12">
                <br />
                <br />
                <obout:Calendar   ID="calBD" runat="server" AllowDeselect="False" 
                    AllowSelectRegular="False" AutoPostBack="False" 
                    MultiSelectedDates="True" Visible="False" 
                    DisableEmbeddedScriptFileResource="False" ForcePosition="False" 
                    TitleText="Fechas disponibles para este curso">
                </obout:Calendar>
            </td>
        </tr>
        <tr>
            <td class="style13">
                &nbsp;</td>
            <td class="style14">
                <asp:Panel ID="Panel2" runat="server" Height="142px" Width="422px">
                    <asp:TextBox ID="txtIngresados" runat="server" AutoPostBack="True" 
                        Height="142px" ontextchanged="TextBox1_TextChanged" ReadOnly="True" 
                        TextMode="MultiLine" Width="414px"></asp:TextBox>
                </asp:Panel>
            </td>
            <td class="style15">
                </td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style7">
                <asp:Button ID="btnRegistro" runat="server" Text="Registrar" 
                    onclick="btnRegistro_Click" Visible="False" />
                <asp:Button ID="Button1" runat="server" Text="Limpiar" 
                    onclick="Button1_Click1" />
            </td>
            <td class="style12">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style7">
                &nbsp;</td>
            <td class="style12">
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

