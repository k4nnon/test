<%@ Page MaintainScrollPositionOnPostback="true"Title="Página principal" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register assembly="obout_Calendar2_Net" namespace="OboutInc.Calendar2" tagprefix="obout" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
    .style1
    {
        width: 263px;
    }
    .style2
    {
        width: 464px;
        color: #336699;
        font-family: Calibri;
        font-size: x-large;
    }
    .style3
    {
        width: 153px;
    }
        .style11
    {
        color: #546E96;
    }
    .style12
    {
        color: #336699;
    }
        .style13
        {
            width: 153px;
            font-family: Calibri;
            font-size: large;
            color: #336699;
            height: 57px;
        }
        .style14
        {
            width: 464px;
            font-weight: bold;
        }
        .style15
        {
        width: 153px;
        color: #336699;
        font-weight: bold;
    }
        .style16
        {
            font-size: medium;
        }
    .style17
    {
        color: #FF0000;
    }
    .style23
    {
        height: 57px;
    }
    .style24
    {
            width: 464px;
            height: 57px;
        }
        .style34
        {
            width: 153px;
            color: #336699;
            text-align: right;
        }
        .style35
        {
            width: 464px;
        }
        .style36
        {
            color: #336699;
        }
        .style42
        {
            width: 153px;
            height: 25px;
        }
        .style43
        {
            width: 464px;
            height: 25px;
        }
        .style44
        {
            height: 25px;
        }
        .style45
        {
            width: 153px;
            color: #336699;
            height: 47px;
        }
        .style46
        {
            width: 464px;
            height: 47px;
        }
        .style47
        {
            height: 47px;
        }
        .style48
        {
            width: 153px;
            color: #336699;
            text-align: right;
            height: 29px;
        }
        .style49
        {
            width: 464px;
            height: 29px;
        }
        .style50
        {
            height: 29px;
        }
         .autocomplete_completionListElement
        {
            margin: 0px !important;
            padding:0 0 0 1px;
            background-color:White;
            color: windowtext;
            border: buttonshadow;
            border-width: 1px;
            border-style: solid;
            cursor: 'default';
            height: 60px;
            width:auto;
            overflow:auto;
            text-align: left;
            list-style: none;
        }
        .style51
        {
            width: 153px;
            color: #336699;
            text-align: right;
            font-size: large;
        }
    </style>
     <script type = "text/javascript">
         function Confirm() {
             var confirm_value = document.createElement("INPUT");
             confirm_value.type = "hidden";
             confirm_value.name = "confirm_value";
             if (confirm("¿Esta acción puede modificar el codigo del registro, desea continuar?")) {
                 confirm_value.value = "Si";
             } else {
                 confirm_value.value = "No";
             }
             if (confirm_value.value == "Si") {
                 confirm_value=document.createElement("INPUT");
                 confirm_value.type = "hidden";
                 confirm_value.name = "confirm_value";
                 if (confirm("Esta seguro de continuar?")) {
                     confirm_value.value = "Si";
                 } else {
                     confirm_value.value = "No";
                 }
             }
             document.forms[0].appendChild(confirm_value);
         }
    </script>
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
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table style="width:100%;">
    <tr>
        <td class="style3">
            &nbsp;</td>
        <td class="style2" style="text-align: center">
            <strong>Ingreso de Capacitación</strong></td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>

        <td class="style13">
            &nbsp;</td>
        <td class="style24">
            <asp:Label ID="lblSeleccion" runat="server" style="color: #336699" 
                Text="Seleccion:"></asp:Label>
            <asp:DropDownList ID="ddlSeleccion" runat="server" AutoPostBack="True" 
                onselectedindexchanged="DropDownList1_SelectedIndexChanged">
                <asp:ListItem>Ingreso de clientes</asp:ListItem>
                <asp:ListItem>Busqueda de clientes</asp:ListItem>
                <asp:ListItem>Listar cursos asociados a un rut</asp:ListItem>
                <asp:ListItem>Agregar Registro</asp:ListItem>
                <asp:ListItem>Modificar un cliente</asp:ListItem>
                <asp:ListItem>Modificar un registro</asp:ListItem>
                <asp:ListItem>Reportes</asp:ListItem>
            </asp:DropDownList>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblFecha" runat="server" 
                style="color: #336699"></asp:Label>
            </td>
        <td class="style23">
            <asp:DropDownList ID="ddlEmpresas" runat="server" Visible="False">
            </asp:DropDownList>
        </td>
    </tr>
        <tr>
        <td class="style34">
            <asp:Label ID="lblID" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="lblCodigo" runat="server" Visible="False"></asp:Label>
        </td>
        <td class="style35">
            <asp:Button ID="btnListar" runat="server" onclick="btnListar_Click" 
                Text="Listar" Visible="False" />
            <asp:TextBox ID="txtListar" runat="server" AutoPostBack="True" 
                ontextchanged="txtListar_TextChanged" Visible="False"></asp:TextBox>
            <asp:Button ID="btnBusqueda" runat="server" onclick="btnBusqueda_Click" 
                Text="Busqueda" Visible="False" />
            <asp:TextBox ID="txtBuscarRut" runat="server" AutoPostBack="True" 
                ontextchanged="txtBuscarRut_TextChanged" Visible="False"></asp:TextBox>
            <asp:Label ID="lblVeri1" runat="server" style="color: #009900" 
                Text="Ingrese un rut" Visible="False"></asp:Label>
        </td>
        <td>
            &nbsp;</td>
        </tr>
        <tr>
        <td class="style51">
            <strong>Datos Cliente</strong></td>
        <td class="style35">
            &nbsp;</td>
        <td>
            &nbsp;</td>
        </tr>
    <tr>
        <td class="style34">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
            <span class="style36">Rut:</span></td>
        <td class="style35">
            <asp:TextBox ID="txtRut" runat="server" AutoPostBack="True" MaxLength="12" 
                ontextchanged="txtRut_TextChanged"></asp:TextBox>
            <asp:Label ID="lblVeri" runat="server" style="color: #009900" 
                Text="Ingrese un rut existente" Visible="False"></asp:Label>
            <asp:RequiredFieldValidator ID="rvRut" runat="server" 
                ControlToValidate="txtRut" ErrorMessage="Rut requerido" style="color: #FF0000"></asp:RequiredFieldValidator>
            <asp:Label ID="lblError" runat="server" style="color: #FF0000" Visible="False"></asp:Label>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style34">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Nombres:</td>
        <td class="style35">
            <asp:TextBox ID="txtNombres" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvNombres" runat="server" CssClass="style17" 
                ErrorMessage="Nombres requeridos" ControlToValidate="txtNombres" 
                Visible="False"></asp:RequiredFieldValidator>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style34">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Primer Apellido:</td>
        <td class="style35">
            <asp:TextBox ID="txtApellidoP" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvA1" runat="server" 
                CssClass="style17" ErrorMessage="Apellido requerido" 
                ControlToValidate="txtApellidoP" Visible="False"></asp:RequiredFieldValidator>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style34">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Segundo Apellido:</td>
        <td class="style35">
            <asp:TextBox ID="txtApellidoM" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvA2" runat="server" 
                CssClass="style17" ErrorMessage="Apellido Requerido" 
                ControlToValidate="txtApellidoM" Visible="False"></asp:RequiredFieldValidator>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style34">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
            Sexo:&nbsp;</td>
        <td class="style35">
            <asp:DropDownList ID="ddlSexo" runat="server">
                <asp:ListItem>F</asp:ListItem>
                <asp:ListItem>M</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style34">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Telefono:</td>
        <td class="style35">
            <asp:TextBox ID="txtTelefono" runat="server"></asp:TextBox>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style34">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
            Email:&nbsp;</td>
        <td class="style35">
            <asp:TextBox ID="txtEmail" runat="server" AutoCompleteType="Gender" 
                style="color: #0000FF"></asp:TextBox>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style34">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
            Ingreso:&nbsp;</td>
        <td class="style35">
            <asp:DropDownList ID="ddlIngreso" runat="server" AutoPostBack="True" 
                onselectedindexchanged="ddlIngreso_SelectedIndexChanged">
                <asp:ListItem>Particular</asp:ListItem>
                <asp:ListItem>Empresa</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style34">
            &nbsp;
            <asp:Label ID="lblNomEmpresa" runat="server" Text="Nombre de la empresa:" 
                Visible="False"></asp:Label>
            &nbsp;</td>
        <td class="style35">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <asp:TextBox ID="txtNomEmpresa" runat="server" Visible="False" 
                ontextchanged="txtNomEmpresa_TextChanged1" AutoPostBack="True"></asp:TextBox>
     <asp:AutoCompleteExtender ID="txtNomEmpresa_AutoCompleteExtender" 
                runat="server" TargetControlID="txtNomEmpresa" UseContextKey="true" CompletionInterval="100"
                MinimumPrefixLength="1" ServiceMethod="GetCompletionList"  CompletionListCssClass="autocomplete_completionListElement" >
            </asp:AutoCompleteExtender>
            <asp:Label ID="lblRutEmpresa" runat="server" style="color: #009900" 
                Visible="False"></asp:Label>
            <asp:RequiredFieldValidator ID="rfvEmpresa" runat="server" 
                ControlToValidate="txtNomEmpresa" ErrorMessage="Nombre de la empresa" 
                style="color: #FF0000" Visible="False"></asp:RequiredFieldValidator>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style34">
            &nbsp;</td>
        <td class="style35">
            <asp:Button ID="btnCliente" runat="server" onclick="btnCliente_Click" 
                Text="Ingresar" />
            <asp:Button ID="btnModificarCli" runat="server" onclick="btnModificarCli_Click" 
                Text="Modificar" Visible="False" />
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style34">
            &nbsp;</td>
        <td class="style35">
            <asp:GridView ID="gvLista" runat="server" Width="383px" BackColor="White" 
                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                onrowcommand="gvLista_RowCommand" 
                onselectedindexchanged="gvLista_SelectedIndexChanged">
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
    <tr class="style16">
        <td class="style15">
            -----------------------</td>
        <td class="style14">
            <span class="style12">----------------------------------</td>
        <td>
            </span></td>
    </tr>
    <tr class="style16">
        <td class="style15">
            <asp:Label ID="lblTitulo" runat="server" Text="Agregar o Modificar un Registro"></asp:Label>
        </td>
        <td class="style14">
            <asp:Label ID="lblFecha2" runat="server" Visible="False"></asp:Label>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style34">
&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
            <asp:Label ID="lblCurso" runat="server" Text="Curso:"></asp:Label>
            <br />
        </td>
        <td class="style35">
                <asp:TextBox ID="txtCurso" runat="server" AutoPostBack="True" 
                    ontextchanged="txtCurso_TextChanged" ReadOnly="True"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" Height="20px" 
                    ImageUrl="~/Imagenes/cursos_logo.png" onclick="ImageButton1_Click" 
                    Width="23px" Enabled="False" />
                <asp:Label ID="lblCursoId" runat="server" Visible="False"></asp:Label>
            <asp:GridView ID="gvCurso" runat="server" Width="393px" BackColor="White" 
                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                onrowcommand="gvCurso_RowCommand" 
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
                <br />
        </td>
        <td>
            </td>
    </tr>
    <tr>
        <td class="style34">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label 
                ID="lblEstado" runat="server" Text="Estado del curso:"></asp:Label>
&nbsp;</td>
        <td class="style35">
            <asp:DropDownList ID="ddlEstado" runat="server" Height="16px" Width="162px" 
                onselectedindexchanged="ddlEstado_SelectedIndexChanged" Enabled="False" 
                AutoPostBack="True">
                <asp:ListItem>--Seleccione--</asp:ListItem>
                <asp:ListItem>Aprobado</asp:ListItem>
                <asp:ListItem>Pendiente</asp:ListItem>
            </asp:DropDownList>
        &nbsp;
            <asp:Button ID="btnCertificado" runat="server" onclick="btnCertificado_Click" 
                Text="Certificado" Visible="False" style="height: 26px" />
            <asp:Button ID="btnGenerar" runat="server" onclick="btnGenerar_Click" 
                Text="Generar Diploma" Visible="False" style="height: 26px" />
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="style48">
            <asp:Label ID="lblSigla" runat="server" Text="Sigla:"></asp:Label>
        </td>
        <td class="style49">
            <asp:DropDownList ID="ddlSigla" runat="server" AutoPostBack="True" 
                onselectedindexchanged="ddlSigla_SelectedIndexChanged" Enabled="False">
                <asp:ListItem>--Seleccione--</asp:ListItem>
                <asp:ListItem>P6A</asp:ListItem>
                <asp:ListItem>P6B</asp:ListItem>
                <asp:ListItem>PRA</asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="lblErrorSigla" runat="server" style="color: #FF0000" 
                Text="Debe seleccionar una sigla!" Visible="False"></asp:Label>
            <asp:Label ID="lblNombreCurso" runat="server" style="color: #336699" 
                Visible="False"></asp:Label>
        </td>
        <td class="style50">
            </td>
    </tr>
    <tr>
        <td class="style34">
            <asp:Label ID="lblAsistencia" runat="server" Text="Asistencia:"></asp:Label>
        </td>
        <td class="style35">
            <asp:TextBox ID="txtAsistencia" runat="server" ReadOnly="True" 
                AutoPostBack="True" ontextchanged="txtAsistencia_TextChanged"></asp:TextBox>
            <asp:Label ID="lblPorcentaje" runat="server" style="color: #336699" Text="%" 
                Visible="False"></asp:Label>
            <asp:RangeValidator ID="RangeValidator1" runat="server" 
                ControlToValidate="txtAsistencia" ErrorMessage="RangeValidator" 
                MaximumValue="100" MinimumValue="0" style="color: #FF0000" Type="Integer" 
                Visible="False">Ingrese numeros del 1 al 100</asp:RangeValidator>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style34">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblCantidad" runat="server" Text="Cantidad de días:"></asp:Label>
        </td>
        <td class="style35">
                <asp:TextBox ID="txtDias" runat="server" AutoPostBack="True" 
                    ontextchanged="txtDias_TextChanged1" ReadOnly="True"></asp:TextBox>
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
                    Visible="False" onselectedindexchanged="DropDownList1_SelectedIndexChanged1" 
                    >
                </asp:DropDownList>
                <asp:DropDownList ID="DropDownList2" runat="server" Visible="False">
                </asp:DropDownList>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style34">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblFechaC" runat="server" Text="Fecha del curso:"></asp:Label>
        </td>
        <td class="style35">
                <asp:TextBox ID="txtFecha" runat="server" ReadOnly="True" 
                    ontextchanged="txtFecha_TextChanged"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" Height="22px" 
                    ImageUrl="~/Imagenes/images.jpg"  Width="31px" 
                    Enabled="False" onclick="ImageButton2_Click" />
                <asp:Label ID="lblCompletos" runat="server" style="color: #009900" 
                    Text="Cantidad de días completos" Visible="False"></asp:Label>
                <asp:Label ID="lblDerror" runat="server" style="color: #FF0000" 
                    Text="Día no habíl" Visible="False"></asp:Label>
           
                <asp:Label ID="lblCompletos2" runat="server" style="color: #FF0000" 
                    Text=" Error, cantidad de dias superior a la indicada,se deseleccionaron las ultimas 2 fechas!" 
                    Visible="False"></asp:Label>
           
        </td>
        <td>
                <asp:Label ID="lblIdFecha" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lblId0" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lblFeVa" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lblFechasResp" runat="server" Visible="False"></asp:Label>
            </td>
    </tr>
    <tr>
        <td class="style45">
            &nbsp;</td>
        <td class="style46">
           
                <obout:Calendar ID="calMulti" runat="server" MultiSelectedDates="true" 
                    AutoPostBack="True" Visible="False" ondatechanged="calMulti_DateChanged" 
                    TitleText="Selección de fechas para este registro">
                </obout:Calendar>
        </td>
        <a onclick="GetSelectedDates()" style="cursor:hand; cursor:pointer;"
    Get the selected dates
</a>

<script language="javascript">
    function GetSelectedDates() {
        var selectedDates = calMulti.selectedDates;
        var selectedDatesString = ""
        for (i = 0; i < selectedDates.length; i++) {
            selectedDatesString += "\n" + selectedDates[i];
        }

        alert("The selected dates are:\n" + selectedDatesString);
    }	
</script>
        <td class="style47">
                <obout:Calendar   ID="calBD" runat="server" AllowDeselect="False" 
                    AllowSelectRegular="False" AutoPostBack="False" 
                    MultiSelectedDates="True" Visible="False" 
                    DisableEmbeddedScriptFileResource="False" ForcePosition="False" 
                    TitleText="Fechas disponibles para este curso">
                </obout:Calendar>
            </td>
    </tr>
    <tr>
        <td class="style42">
            </td>
        <td class="style43">
            <asp:Button ID="btnAgregar" runat="server" onclick="btnAgrega_Click" 
                Text="Agregar" Visible="False" style="height: 26px" />
            <asp:Button ID="btnModificar" runat="server" Text="Modificar" Visible="False" 
                onclick="btnModificar_Click" onclientclick="Confirm()"/>
        </td>
        <td class="style44">
            </td>
    </tr>
    <tr>
        <td class="style3">
            &nbsp;</td>
        <td class="style24">
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style3">
            &nbsp;</td>
        <td class="style24">
            &nbsp;</td>
        <td class="style11">
            &nbsp;</td>
    </tr>
</table>
</asp:Content>
