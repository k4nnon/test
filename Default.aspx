<%@ Page MaintainScrollPositionOnPostback="true" Title="Página principal" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <table style="width: 100%;">
        <link href="/Styles/Default.css" rel="stylesheet" type="text/css" />
        <script src="/Scripts/Default.js" language="javascript" type="text/javascript"></script>
        <tr>
            <td class="centro">&nbsp;</td>
            <td class="titulo">
                <strong>Ingreso de Capacitación</strong></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblSeleccion" runat="server" CssClass="labelMenu"
                    Text="Seleccion:"></asp:Label>

            </td>
            <td>
                
                <asp:DropDownList CssClass="Menu" ID="ddlSeleccion" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" Font-Size="Small">
                    <asp:ListItem>Ingreso de clientes</asp:ListItem>
                    <%--  <asp:ListItem>Busqueda de clientes</asp:ListItem>--%>
                    <asp:ListItem>Busqueda de Clientes</asp:ListItem>
                    <asp:ListItem>Agregar Registro</asp:ListItem>
                    <asp:ListItem>Modificar un cliente</asp:ListItem>
                    <asp:ListItem>Modificar un registro</asp:ListItem>
                    <%--<asp:ListItem>Reportes</asp:ListItem>--%>
                </asp:DropDownList>
            </td>
            <td align="right">

                <asp:Label ID="lblFecha" runat="server"
                    CssClass="fecha"></asp:Label>

            </td>
        </tr>
        <tr>
            <td>
                 <a onclick="GetSelectedDates()" style="cursor:hand; cursor:pointer;"
                    Get the selected dates
                </a>
                <asp:Label ID="lblFeVa" runat="server" Visible="False"></asp:Label>      
            </td>
            <td>
                <asp:Button ID="btnListar" runat="server" OnClick="btnListar_Click"
                    Text="Buscar" Visible="False" CssClass="botonBusqueda" CausesValidation="false" />
                <asp:TextBox ID="txtListar" CssClass="boxRutBusqueda" runat="server" AutoPostBack="True"
                    OnTextChanged="txtListar_TextChanged" Visible="False"></asp:TextBox>

                <asp:Label ID="lblVeri1" CssClass="mensajeRut" runat="server" Style="color: #009900"
                    Text="Ingrese un rut" Visible="False"></asp:Label>
            </td>
            <td>
            
            </td>
            <td>
                &nbsp;</td>
        </tr>



        <%-- Datos del Cliente para ser ingresados --%>

        <tr>
            <td class="tituloSeccion">
                Datos Cliente
            </td>
            <td>

            </td>
            <td>
                <asp:Label ID="lblError" runat="server" CssClass="alertMessage" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="text">Rut:
            </td>
            <td align="left">
                <asp:TextBox ID="txtRut" runat="server" AutoPostBack="True" MaxLength="12"
                    OnTextChanged="txtRut_TextChanged" CssClass="box"></asp:TextBox>

            </td>
            <td>

                <asp:RequiredFieldValidator ID="rvRut" runat="server" CssClass="alertMessage"
                    ControlToValidate="txtRut" ErrorMessage="Rut requerido"></asp:RequiredFieldValidator>

            </td>
        </tr>
        <tr>
            <td class="text">Nombres:
            </td>
            <td align="left">
                <asp:TextBox CssClass="box" ID="txtNombres" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvNombres" runat="server" CssClass="alertMessage"
                    ErrorMessage="Nombres requeridos" ControlToValidate="txtNombres"
                    Visible="False"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="text">Primer Apellido:</td>
            <td align="left">
                <asp:TextBox CssClass="box" ID="txtApellidoP" runat="server"></asp:TextBox>

            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvA1" runat="server"
                    CssClass="alertMessage" ErrorMessage="Apellido requerido"
                    ControlToValidate="txtApellidoP" Visible="False"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="text">Segundo Apellido:</td>
            <td align="left">
                <asp:TextBox CssClass="box" ID="txtApellidoM" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvA2" runat="server"
                    CssClass="alertMessage" ErrorMessage="Apellido Requerido"
                    ControlToValidate="txtApellidoM" Visible="False"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="text">Sexo:</td>
            <td align="left">
                <asp:DropDownList ID="ddlSexo" CssClass="sexBox " runat="server">
                    <asp:ListItem>F</asp:ListItem>
                    <asp:ListItem>M</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td class="text">Telefono:</td>
            <td align="left">
                <asp:TextBox CssClass="box" ID="txtTelefono" runat="server"></asp:TextBox>
            </td>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td class="text">Email:</td>
            <td align="left">
                <asp:TextBox ClassCss="box" ID="txtEmail" runat="server" AutoCompleteType="Gender"
                    Style="color: #0000FF" CssClass="box"></asp:TextBox>
            </td>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td class="text">Ingreso:</td>
            <td align="left">
                <asp:DropDownList CssClass="IngresoBox" ID="ddlIngreso" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlIngreso_SelectedIndexChanged">
                    <asp:ListItem>Particular</asp:ListItem>
                    <asp:ListItem>Empresa</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label CssClass="text" ID="lblNomEmpresa" runat="server" Text="Nombre de la empresa:"
                    Visible="False"></asp:Label>
            </td>
            <td>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                </asp:ToolkitScriptManager>

                <asp:TextBox CssClass="box" ID="txtNomEmpresa" runat="server" Visible="False"
                    OnTextChanged="txtNomEmpresa_TextChanged1" AutoPostBack="True"></asp:TextBox>

                <asp:Label CssClass="rutEmpresa" ID="lblRutEmpresa" runat="server" 
                    Visible="False"></asp:Label>


            </td>
            <td align="right">
                <asp:AutoCompleteExtender ID="txtNomEmpresa_AutoCompleteExtender"
                    runat="server" TargetControlID="txtNomEmpresa" UseContextKey="true" CompletionInterval="100"
                    MinimumPrefixLength="1" ServiceMethod="GetCompletionList" CompletionListCssClass="autocomplete_completionListElement">
                </asp:AutoCompleteExtender>

                <asp:RequiredFieldValidator ID="rfvEmpresa" runat="server"
                    ControlToValidate="txtNomEmpresa" ErrorMessage="Nombre de la empresa"
                    CssClass ="nombreEmpresa" Visible="False"></asp:RequiredFieldValidator>
            </td>
        </tr>


        <%-- seleccion de botones de ingresar o modificar --%>

        <tr>
            <td align="left">

                <asp:Button ID="aRegistro" runat="server" Visible ="false" Text="Agregar Registro" OnClick="mostrarRegistro" CssClass="bagregarRegistro" />
                 <asp:Button ID="bModificar" runat="server" Visible ="false" Text="Modificar Cliente" OnClick="modificarCliente" CssClass="bModificarCliente" />

            
            </td>
            <td>
                <asp:Button ID="btnCliente" CssClass="boton" runat="server" OnClick="btnCliente_Click"
                    Text="Ingresar" />
                <asp:Button ID="btnModificarCli" runat="server" OnClick="btnModificarCli_Click"
                    Text="Modificar" Visible="False" CssClass="bModificarEmpleado" />
            </td>
            <td>

                <asp:Label ID="lblID" runat="server" Visible="False"></asp:Label>

                </td>
        </tr>

        <tr>
            <td>  

            </td>
            <td align="right" >
                <asp:GridView ID="gvLista" runat="server" BackColor="White"
                    BorderColor="#999999" BorderStyle="None" visible="false" CellPadding="3"
                    OnRowCommand="gvLista_RowCommand"
                    OnSelectedIndexChanged="gvLista_SelectedIndexChanged" CssClass="listaCursos" GridLines="Vertical">

                    <AlternatingRowStyle BackColor="#DCDCDC" />

                    <Columns>
                        <asp:CommandField ShowSelectButton="True" />
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <RowStyle ForeColor="Black" BackColor="#EEEEEE" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#0000A9" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#000065" />
                </asp:GridView>
            </td>
            <td>
                
            </td>
        </tr>
        <tr visible="false" runat="server" id="formulario1">
            <td >
               
            </td>
            <td class="barras">--------------------------------------------------------------</td>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                 &nbsp;</td>
        </tr>
        <tr visible="false" runat="server" id="formulario2">
            <td>
                <asp:Label ID="lblTitulo" runat="server" Visible="False" Text="Agregar o Modificar un Registro" CssClass="tituloSeccion"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblFecha2" runat="server" Visible="False"></asp:Label>
            </td>
            <td>&nbsp;
            
                <asp:Label ID="lblCursoId" runat="server" Visible="False"></asp:Label>

            </td>
        </tr>
        <tr visible="false" runat="server" id="formulario3">
            <td align="right">
                <asp:Label ID="lblCurso" runat="server" Text="Curso:" CssClass="text"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtCurso" runat="server" AutoPostBack="True"
                    OnTextChanged="txtCurso_TextChanged" ReadOnly="True" CssClass="box"></asp:TextBox>

                
            </td>
            <td align="right">

                <asp:ImageButton ID="ImageButton1" runat="server" 
                    ImageUrl="~/Imagenes/cursos_logo.png" OnClick="ImageButton1_Click"
                     Enabled="False" CssClass="cursoBoton" />

            </td>
        </tr>
        <tr>
            <td>

            </td>
            <td align="center">
                <asp:GridView ID="gvCurso" runat="server" 
                    BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                    OnRowCommand="gvCurso_RowCommand"
                    OnSelectedIndexChanged="gvLista_SelectedIndexChanged" ShowHeader="True"
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
            <td class="espaciado2">

                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True"
                    Visible="false" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged1">
                </asp:DropDownList>

                <asp:DropDownList ID="DropDownList2" runat="server" Visible="False">
                </asp:DropDownList>

            </td>
        </tr>
        <tr runat="server" visible="false" id="formulario4">
            <td align="right" >
                <asp:Label ID="lblEstado" runat="server" Text="Estado del Alumno:" CssClass="text"></asp:Label>
            </td>
            <td> 
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="eCurso"
                    OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" Enabled="False"
                    AutoPostBack="True">
                    <asp:ListItem>--Seleccione--</asp:ListItem>
                    <asp:ListItem>Aprobado</asp:ListItem>
                    <asp:ListItem>Pendiente</asp:ListItem>
                </asp:DropDownList>

            </td>

            <td>

                <asp:Button ID="btnCertificado" runat="server" OnClick="btnCertificado_Click"
                    Text="Generar Certificado" Visible="False"  CssClass="botoCertificado" />
                <asp:Button ID="btnGenerar" runat="server" OnClick="btnGenerar_Click"
                    Text="Generar Diploma" Visible="False"  CssClass="botonDiploma" />
            </td>
        </tr>

        <tr visible="false" runat="server" id="formulario5">
            <td align="right">
                <asp:Label ID="lblSigla" runat="server" Text="Sigla:" CssClass="text"></asp:Label>
            </td>
            <td >
                <asp:DropDownList ID="ddlSigla" runat="server" ClassCss="boxSiglas" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlSigla_SelectedIndexChanged" Enabled="False" CssClass="sigla">
                    <asp:ListItem>--Seleccione--</asp:ListItem>
                    <asp:ListItem>P6A</asp:ListItem>
                    <asp:ListItem>P6B</asp:ListItem>
                    <asp:ListItem>PRA</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>

                <asp:Label ID="lblNombreCurso" runat="server" Style="color: #336699"
                    Visible="False"></asp:Label>

                <asp:Label ID="lblErrorSigla" runat="server" Style="color: #FF0000"
                    Text="Debe seleccionar una sigla!" Visible="False"></asp:Label>

            </td>
        </tr>

        <tr visible="false" runat="server" id="formulario6">
            <td align="right">
                <asp:Label ID="lblAsistencia" runat="server" Text="Asistencia:" CssClass="text"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtAsistencia" runat="server" ReadOnly="True"
                    AutoPostBack="True" OnTextChanged="txtAsistencia_TextChanged" CssClass="box"></asp:TextBox>
            </td>
            <td align="right">
             
                <asp:label runat="server" Text="% (1-100)" CssClass="mAsistencia"></asp:label>

                &nbsp;
            </td>
        </tr>
        <tr visible="false" runat="server" id="formulario7">
            <td align="right">
                <asp:Label ID="lblCantidad" runat="server" Text="Cantidad de días:" CssClass="text"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtDias" runat="server" AutoPostBack="True"
                    OnTextChanged="txtDias_TextChanged1" CssClass="box"></asp:TextBox>

            </td>
            <td>

                <asp:RangeValidator ID="RangeValidator1" runat="server"
                    ControlToValidate="txtAsistencia" ErrorMessage="RangeValidator"
                    MaximumValue="100" MinimumValue="0" Style="color: #FF0000" Type="Integer"
                    Visible="False"></asp:RangeValidator>
                </td>
        </tr>
        <tr visible="false" runat="server" id="formulario8">
            <td align="right">
                <asp:Label ID="lblFechaC" runat="server" Text="Fecha del curso:" CssClass="text"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtFecha" runat="server" ReadOnly="True"
                    OnTextChanged="txtFecha_TextChanged" CssClass="box"></asp:TextBox>

            </td>
            <td align="right">

                <asp:ImageButton ID="ImageButton2" runat="server" 
                    ImageUrl="~/Imagenes/calendario.jpg"
                    Enabled="False" OnClick="ImageButton2_Click" CssClass="bCalendario" />

            </td>
        </tr>

        <tr>
            <td>
                &nbsp;</td>
            <td align="center">

                <obout:Calendar ID="calMulti" CultureName="es-CL" runat="server" MultiSelectedDates="true" 
                    AutoPostBack="True" Visible="False" OnDateChanged="calMulti_DateChanged"
                    TitleText="Selección de fechas para este registro" >
                    
                </obout:Calendar>
            </td>

        <td align="right"> 
            <a onclick="GetSelectedDates()" style="cursor:hand; cursor:pointer;"
            Get the selected dates
            </a>
        <asp:Label ID="lblDerror" runat="server" Style="color: #FF0000"
            Text="Día no habíl" Visible="False" CssClass="errorCalendario"></asp:Label>

        <asp:Label ID="lblCompletos" runat="server" Style="color: #009900"
            Text="Cantidad de días completos" Visible="False" CssClass="errorCalendario2"></asp:Label>
            

       </td>
    </tr>

    <tr>
        <td align ="right">
             <asp:Label ID="lblNota" Visible="false" runat="server" Text="Nota:" CssClass="text"></asp:Label>
        </td>
        <td>
                <asp:TextBox ID="nota" runat="server" Visible ="false"  AutoPostBack="True"
                    OnTextChanged="txtnota_TextChanged" CssClass="box"></asp:TextBox>
        </td>

        <td align ="right"> 
           
             <asp:Label ID="lblporcentaje" Visible="false" runat="server" Text=" % (Ej: 1-100)" CssClass="porcentaje"></asp:Label>
        </td>
    </tr>

        <tr visible="false" runat="server" id="formulario9">
            <td>

                <asp:Label ID="lblIdFecha" runat="server" Visible="False"></asp:Label>
                <a onclick="GetSelectedDates()" style="cursor:hand; cursor:pointer;"
                Get the selected dates
                </a>
            </td>
            <td>
                <asp:Button ID="btnAgregar" runat="server" OnClick="btnAgrega_Click"
                    Text="Agregar" Visible="False" CssClass="bAgregar"/>
                 <asp:Button ID="btnAgregarConNota" runat="server" OnClick="btnAgregaConNota_Click"
                    Text="Agregar" Visible="False" CssClass="bAgregar"/>
                <asp:Button ID="btnModificar" runat="server" Text="Modificar" Visible="False"
                    OnClick="btnModificar_Click" OnClientClick="Confirm()" CssClass="bModificar" />
                 <asp:Button ID="btnModificarConNota" runat="server" Text="Modificar" Visible="False"
                    OnClick="btnModificaConNota_Click" OnClientClick="Confirm()" CssClass="bAgregar" />
            </td>
            <td>
                &nbsp;
            </td>

        </tr>
        <tr>
            <td>
                <asp:Label ID="lblErrorNota" runat="server" CssClass="notaAlert" Visible="False"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlEmpresas" runat="server" Visible="False">
                    </asp:DropDownList>

                <asp:Label ID="lblFechasResp" runat="server" Visible="False"></asp:Label>

            </td>
            <td>
                <asp:Label ID="lblCodigo" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>

