<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AgregarDatos.aspx.cs" Inherits="AgregarDatos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <table style="width: 100%;">
        <link href="/Styles/AgregarDatos.css" rel="stylesheet" type="text/css" />
        <script src="/Scripts/AgregarDatos.js" language="javascript" type="text/javascript"></script>
        <tr>
            <td class="titulo">
                 <strong><span>Evaluación</span></strong> 
            </td>
        </tr>
        
        
        
        
         <tr>
            <td>&nbsp;</td>
            <td>
                <asp:DropDownList ID="ddlSeleccion" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlSeleccion_SelectedIndexChanged" CssClass="menus">
                    <asp:ListItem>Agregar Evaluación</asp:ListItem>
                    <asp:ListItem>Modificar Evaluación</asp:ListItem>
                    <asp:ListItem>Agregar Evaluación por Curso</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>

        <tr runat ="server" id="tr1">
            <td class="text">
                Rut:
            </td>
            <td>
                <asp:TextBox ID="txtRut" runat="server" AutoPostBack="True" CssClass="textbox"
                    OnTextChanged="txtRut_TextChanged"></asp:TextBox>


            </td>
            <td align="right">
                <asp:Label ID="lblErrorRut" CssClass="messageAlert" runat="server" 
                    Text="No existe un cliente con este rut" Visible="false"></asp:Label>

                <asp:Label  ID="lblSinReg" CssClass="messageAlert2" runat="server" 
                    Text="Rut sin registros!" Visible="false"></asp:Label>

                <asp:Label  ID="lblCompletos" CssCLass="messageAlert3" runat="server" 
                    Visible="false">Registros ya evaluados</asp:Label>

            </td>
        </tr>

        <tr runat ="server" id="tr3">
            <td class="text">
                Nombre:</td>
            <td>
                <asp:TextBox ID="txtNombre" runat="server" ReadOnly="True" CssClass="textbox"></asp:TextBox>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>

          <tr runat ="server" id="tr2" visible =" false">
            <td class="text">
                Prueba a editar: 
            </td>
            <td>
                <asp:TextBox ID="txtId" runat="server" Enabled="False" ReadOnly="True" CssClass="textbox"></asp:TextBox>
            </td>
            <td align="right">
                <asp:ImageButton ID="imgId" runat="server" Enabled="False" CssClass="bId"
                    ImageUrl="~/Imagenes/Imagen2.jpg" OnClick="imgId_Click" />
            </td>
        </tr>

        <tr runat ="server" id="tr5">
            <td class="text">
                Nota:
            </td>
            <td>
                <asp:TextBox ID="txtNota" runat="server" ReadOnly="True" CssClass="textbox"></asp:TextBox>
            </td>
            <td align="right" >
                <asp:Label ID="porcentaje" runat="server" CssClass="porcentaje" Text=" % (Ej: 1-100)" ></asp:Label>
               
            </td>
        </tr>
        <tr runat ="server" id="tr6">
            <td class="text">
               Curso Inscrito:
            </td>
            <td>
                <asp:TextBox ID="txtIdReg" runat="server" OnTextChanged="txtIdReg_TextChanged"
                    ReadOnly="True" CssClass="textbox"></asp:TextBox>

            </td>
            <td align="right">
                <asp:ImageButton
                    ID="imgReg"
                    runat="server"
                    ImageUrl="~/Imagenes/Imagen2.jpg"
                    OnClick="imgReg_Click"
                    Enabled="False"
                    CssClass="bId" />
            </td>
        </tr>
        <tr runat ="server" id ="tr7">
            <td class="text">
                </td>
            <td>
               </td>
            <td align="right">
                </td>
        </tr>
        <tr runat ="server" id="tr8">
            <td class="text">
                &nbsp;</td>
            <td>

                <asp:GridView ID="gvIdRegistro" runat="server" BackColor="White"
                    BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                    OnRowCommand="gvIdRegistro_RowCommand"
                    OnSelectedIndexChanged="gvIdRegistro_SelectedIndexChanged"
                    Visible="False" CssClass="idRegistro">
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
        <tr runat ="server" id="tr9">
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnAgregar" runat="server" OnClick="Button1_Click"
                    Text="Agregar" CssClass="bAgregar" />
                <asp:Button ID="btnModificar" runat="server" OnClick="btnModificar_Click"
                    Text="Modificar" Visible="False" CssClass="bAgregar" />
            </td>
            <td>&nbsp;</td>
        </tr>



         <tr runat ="server" id="tr11" visible="false">
            <td class="text">
                Curso :</td>
            <td>
                <asp:TextBox ID="txtCurso" runat="server" OnTextChanged="txtIdReg_TextChanged"
                    ReadOnly="True" CssClass="textbox"></asp:TextBox>

            </td>
            <td align ="right">
                    <asp:ImageButton
                    ID="imgCursos"
                    runat="server"
                    ImageUrl="~/Imagenes/Imagen2.jpg"
                    OnClick="imgCursos_Click"
                    CssClass="bCursos" />
            </td>
        </tr>
        <tr runat ="server" id="tr12" visible="false">
            <td class="text">
                </td>
            <td>
                  <asp:GridView ID="gvCurso" runat="server" 
                    BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                    OnRowCommand="gvCurso_RowCommand" CssClass ="cursos"
                   ShowHeader="True"
                    >
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
            <td align ="right">
            </td>
        </tr>


       
        <tr runat ="server" id="tr13" visible="false">
            <td class="text">
                Tipo de Curso :</td>
            <td>
                <asp:DropDownList 
                    ID="ddlIngreso" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlIngreso_SelectedIndexChanged" CssClass="seleccionCurso" Enabled="False">
                    <asp:ListItem>Seleccione</asp:ListItem>
                    <asp:ListItem>Particular</asp:ListItem>
                    <asp:ListItem>Empresa</asp:ListItem>
                </asp:DropDownList>


                </td>
            <td align ="right">
                &nbsp;</td>
        </tr>


       
        <tr runat ="server" id="tr14" visible="false">
            <td class="text">
                &nbsp;Nombre de la empresa :</td>
            <td>
                 <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                </asp:ToolkitScriptManager>

                <asp:TextBox ID="boxEmpresa" Visible="false" runat="server" 
                    AutoPostBack="True" CssClass="textbox"
                    OnTextChanged="txtNomEmpresa_TextChanged1"></asp:TextBox>


                </td>
            <td align ="right">
                    <asp:AutoCompleteExtender ID="txtNomEmpresa_AutoCompleteExtender"
                    runat="server" TargetControlID="boxEmpresa" UseContextKey="true" CompletionInterval="100"
                    MinimumPrefixLength="1" ServiceMethod="GetCompletionList" CompletionListCssClass="autocomplete_completionListElement">
                </asp:AutoCompleteExtender>

                <asp:RequiredFieldValidator ID="rfvEmpresa" runat="server"
                    ControlToValidate="txtNomEmpresa" ErrorMessage="Nombre de la empresa"
                    CssClass ="nombreEmpresa" Visible="False"></asp:RequiredFieldValidator>
            </td>
        </tr>


       
        <tr runat ="server" id="tr15" visible="false">
            <td class="text">
                Fecha de Inicio del Curso:</td>
            <td>
                <asp:TextBox id="txtFechas" runat="server"  ReadOnly="True" CssClass="textbox"></asp:TextBox>


            </td>
            <td align ="right">
                
                <asp:ImageButton
                    ID="ImageCalendario"
                    runat="server"
                    ImageUrl="~/Imagenes/calendario.jpg"
                    OnClick="imgCalendario_Click"
                    CssClass="bCalendario2"
                    Enable="false" Enabled="False" />
                
            </td>
        </tr>
        <tr runat ="server" id="tr16" visible="false">
            <td ></td>
            <td align="left">
                <obout:Calendar CultureName="es-CL" ID="calMulti" runat="server" MultiSelectedDates="true" CSSCalendar="calendarioNotas"
                    AutoPostBack="True" Visible="False" OnDateChanged="calMulti_DateChanged"
                    TitleText="Selección la fecha Curso" >
                    
                </obout:Calendar>  


            </td>
            <td align ="right">
                
                <asp:Label ID="lblDerror" CssClass="mensajeAlert" runat="server"  Text="Día no habíl" Visible="False"></asp:Label>
                
                </td>
        </tr>
        <tr runat ="server" id="tr17" visible="false">
            <td></td>
            <td align="left">

                <asp:GridView ID="gvListarAlumnos" runat="server" BackColor="White"
                    BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                   
                    
                    Visible="false" CssClass="notasClases">
                   
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <RowStyle ForeColor="#000066" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#00547E" />

                     <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:TextBox ID="txtNotas" runat="server" Width="40" >
                             </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                     <Columns >
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:TextBox ID="txtAsistencia" runat="server" Width="55" >
                             </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
            <td align ="right">               
            </td>
        </tr>

        <tr>
            <td>

            </td>
            <td>
                <asp:CheckBox ID="CBaprovar" visible="false" CssClass="check" runat="server" Text ="¿Desea actualizar los alumnos a estado 'Aprobado'?"  />
            </td>
            <td>

            </td>
        </tr>

        <tr runat ="server" id="tr18" visible="false">
            <td>
                &nbsp;</td>
            <td>

                <asp:Button ID="buscarAlumnos" runat="server" OnClick="buscarAlumnos_Click"
                    Text="Buscar" CssClass="bBuscar" />

                 <asp:Button Visible="false" ID="agregarNotas" runat="server" OnClick="agregarNotas_Click"
                    Text="agregar" CssClass="agregarNotas" OnClientClick="Confirm()" />


                 <asp:Button Visible="false" ID="bReinicio" runat="server" OnClick="limpiar_CLick"
                    Text="Limpiar" CssClass="bBuscar" />
            </td>
            <td align ="right">               
                
               <asp:Label id="errorVacio" runat="server" Visible ="false" CssClass="errorVacio" text="No existen registros con estos datos" ></asp:Label>
              <asp:Label ID="lblError" runat="server" CssClass="errorAgregar" Visible="False"></asp:Label>
                    
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvId" runat="server"
        OnRowCommand="gvId_RowCommand" CssClass="idPrueba" >
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
    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True"
                    Visible="false" >
                </asp:DropDownList>
    <asp:DropDownList ID="ddlEmpresas" runat="server" Visible="False">
                    </asp:DropDownList>
 </asp:Content>






