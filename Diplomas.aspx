<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Diplomas.aspx.cs" Inherits="Pag_Diploma_Diplomas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="styleSheet/StyleSheet.css" rel="Stylesheet" type="text/css" media="screen" />
<link rel="stylesheet" type="text/css" href="styleSheet/imprimir.css" media="print" />

    <title></title>
    </head>
<body>



       <form id="form1" runat="server">



       <div id="contenedor">
        <div id="diploma">
            <img  alt="Smiley face" src="styleSheet/Diploma_Sistema_chico.jpg" 
                style="height: 888px; margin-bottom: 0px" />
                <div class="titulo">
                    Certificamos que el          <asp:Label ID="lblSexo" runat="server"></asp:Label>
                    :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     <br />
                    <br />                   
                    <asp:Label ID="lblNombre" runat="server" CssClass="fuente" style="font-size: xx-large"></asp:Label>
        </div>
        <div class="codigoDiploma">
        <asp:Label ID="lblCodigo" runat="server" 
                style="font-size: large; font-weight: 700"></asp:Label>          
        </div>
        <div class="titulo2">
        Ha aprobado el curso de:
            <br />
            <asp:Label ID="lblCurso" runat="server" style="font-size: xx-large"></asp:Label>
        </div>
        <div class="titulo3">
            Dictado los días
        <asp:Label ID="lblFecha" runat="server"></asp:Label>
        &nbsp;</div>
        <div class="titulo4">
        Santiago, <asp:Label ID="lblmes" runat="server"></asp:Label> &nbsp;<asp:Label ID="lbldia" runat="server"></asp:Label> &nbsp;de  <asp:Label ID="lblano" runat="server"></asp:Label>
            </div>
         </div>
         </div>
        </form>
        </body>
</html>
