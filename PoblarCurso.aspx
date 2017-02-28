<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PoblarCurso.aspx.cs" Inherits="PoblarCurso" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="/Styles/PoblarCurso.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/PoblarCurso.js" language="javascript" type="text/javascript"></script>
    <table style="width: 100%;">

        <tr>
            <td>
                <strong class="titulo">Poblar Curso</strong>
            </td>

        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Label ID="lblError" runat="server" Style="color: #FF0000"></asp:Label>
                <asp:Label ID="lblFechasResp" runat="server" Visible="False"></asp:Label>
            </td>
            <td>

            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>
                <asp:Label ID="lblFeVa" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lblId" runat="server"></asp:Label>
                <asp:Label ID="lblIdFecha" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>

        <tr>
            <td class="text">
                Curso:
            </td>
            <td>
                <asp:TextBox ID="txtCurso" runat="server" AutoPostBack="True"
                    OnTextChanged="txtCurso_TextChanged" ReadOnly="True"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" 
                    ImageUrl="~/Imagenes/cursos_logo.png" OnClick="ImageButton1_Click"
                     Enabled="true" CssClass="bCurso" />
                <asp:Label ID="lblCursoId" runat="server" Visible="False"></asp:Label>
                <asp:GridView ID="gvCurso" runat="server" Width="393px" BackColor="White"
                    BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                    OnRowCommand="gvLista_RowCommand"
                    OnSelectedIndexChanged="gvLista_SelectedIndexChanged" ShowHeader="False"
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
            <td></td>
        </tr>
        <tr>
            <td class="text">
                Cantidad de días:
            </td>
            <td>
                <asp:TextBox ID="txtDias" runat="server" AutoPostBack="True"
                    OnTextChanged="txtDias_TextChanged1" Enabled="False"></asp:TextBox>
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True"
                    Visible="False"
                    OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged2">
                </asp:DropDownList>
                <asp:DropDownList ID="DropDownList2" runat="server" Visible="False">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="text">
                Fecha:
            </td>
            <td>
                <asp:TextBox ID="txtFecha" runat="server" ReadOnly="True"
                    OnTextChanged="txtFecha_TextChanged"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" CssClass="bCalendario" runat="server" 
                    ImageUrl="~/Imagenes/calendario.jpg" OnClick="ImageButton2_Click"
                    Enabled="False" />
                &nbsp;<asp:Label ID="lblCompletos" runat="server" Style="color: #009900"
                    Text="Cantidad de días completos" Visible="False"></asp:Label>
                <asp:Label ID="lblDerror" runat="server" Style="color: #FF0000"
                    Text="Día no habíl" Visible="False"></asp:Label>
                &nbsp;<asp:Label ID="lblCompletos2" runat="server" Style="color: #FF0000"
                    Text=" Error, cantidad de dias superior a la indicada,se deseleccionaron las ultimas 2 fechas!"
                    Visible="False"></asp:Label>

                <obout:Calendar CultureName="es-CL" ID="calMulti" runat="server" MultiSelectedDates="true"
                    AutoPostBack="True" OnDateChanged="calMulti_DateChanged" Visible="False"
                    TitleText="Selección de fechas para este registro">
                </obout:Calendar>
            </td>
            <td></td>
        </tr>
        <tr>
            <td class="rut">
                Rut(s):
            </td>
            <td>

                <input id="btnAdd" class="bAgregarNuevo" type="button" value="Agregar Nuevo Alumno" onclick="AddTextBox()" />
                <br />
               
                <div id="TextBoxContainer">
                    <!--Textboxes will be added here -->
                </div>
                <br />
                <asp:Button ID="btnPost" runat="server"  Text="Agregar Alumnos al curso" OnClick="Post" />

            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="text">
                Registro:
            </td>
            <td>
                <asp:Panel ID="Panel2" runat="server" Height="142px" Width="422px">
                    <asp:TextBox ID="txtIngresados" runat="server" AutoPostBack="True"
                        Height="142px" OnTextChanged="TextBox1_TextChanged" ReadOnly="True"
                        TextMode="MultiLine" Width="414px"></asp:TextBox>
                </asp:Panel>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <%--<asp:Button ID="btnRegistro" runat="server" Text="Registrar" 
                    onclick="btnRegistro_Click" Visible="False" />--%>
                <asp:Button ID="Button1" runat="server" Text="Limpiar"
                    OnClick="Button1_Click1" />

            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>

