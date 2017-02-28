<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AgregarDatos.aspx.cs" Inherits="AgregarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style2
        {
            width: 75px;
            text-align: right;
            color: #336699;
        }
    .style3
    {
        font-size: large;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table style="width:100%;">
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                <strong><span class="style3">Evaluación</span></strong> </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td>
                <asp:DropDownList ID="ddlSeleccion" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="ddlSeleccion_SelectedIndexChanged">
                    <asp:ListItem>Agregar Evaluación</asp:ListItem>
                    <asp:ListItem>Modificar Evaluación</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                Rut:</td>
            <td>
                <asp:TextBox ID="txtRut" runat="server" AutoPostBack="True" 
                    ontextchanged="txtRut_TextChanged"></asp:TextBox>
                <asp:Label ID="lblErrorRut" runat="server" style="color: #FF0000" 
                    Text="No existe un cliente con este rut" Visible="False"></asp:Label>
                <asp:Label ID="lblSinReg" runat="server" style="color: #FF0000" 
                    Text="Rut sin registros!" Visible="False"></asp:Label>
                <asp:Label ID="lblCompletos" runat="server" style="color: #009900" 
                    Visible="False">Registros ya evaluados</asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                <asp:Label ID="lblId" runat="server" Text="Id:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtId" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                <asp:ImageButton ID="imgId" runat="server" Enabled="False" Height="22px" 
                    ImageUrl="~/Imagenes/Imagen2.jpg" onclick="imgId_Click" Width="21px" />
            <asp:GridView ID="gvId" runat="server" Width="16px" BackColor="White" 
                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                onrowcommand="gvId_RowCommand" 
                    Height="59px">
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
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                Nombre:</td>
            <td>
                <asp:TextBox ID="txtNombre" runat="server" ReadOnly="True"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                Fecha:</td>
            <td>
                <asp:TextBox ID="txtFecha" runat="server" ReadOnly="True"></asp:TextBox>
                <asp:ImageButton ID="imgFecha" runat="server" Height="21px" 
                    ImageUrl="~/Imagenes/images.jpg" onclick="imgFecha_Click" Width="29px" 
                    Enabled="False" />
                <asp:Calendar ID="Calendar1" runat="server" Height="83px" 
                    onselectionchanged="Calendar1_SelectionChanged" Visible="False" Width="16px">
                </asp:Calendar>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                Nota:</td>
            <td>
                <asp:TextBox ID="txtNota" runat="server" ReadOnly="True"></asp:TextBox>
                %</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                Id Registro:
            </td>
            <td>
                <asp:TextBox ID="txtIdReg" runat="server" ontextchanged="txtIdReg_TextChanged" 
                    ReadOnly="True"></asp:TextBox>
                <asp:ImageButton ID="imgReg" runat="server" Height="24px" 
                    ImageUrl="~/Imagenes/Imagen2.jpg" onclick="imgReg_Click" Width="22px" 
                    Enabled="False" />
            <asp:GridView ID="gvIdRegistro" runat="server" Width="16px" BackColor="White" 
                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                onrowcommand="gvIdRegistro_RowCommand" 
                onselectedindexchanged="gvIdRegistro_SelectedIndexChanged" 
                    Height="59px" Visible="False">
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
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td>
                <asp:Button ID="btnAgregar" runat="server" onclick="Button1_Click" 
                    Text="Agregar" />
                <asp:Button ID="btnModificar" runat="server" onclick="btnModificar_Click" 
                    Text="Modificar" Visible="False" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

