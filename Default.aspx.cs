using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Data;
using System.Data.Query;
using System.Web.Query;
using System.IO;
using Microsoft.Office.Interop;
using Microsoft.Office.Interop.Word;
using System.Globalization;
using System.Configuration;
using iTextSharp.text.pdf;
public partial class _Default : System.Web.UI.Page
{
    static MySqlConnection connection = new MySqlConnection();
    static MySqlCommand Comando = new MySqlCommand();
    static MySqlDataAdapter Adaptador = new MySqlDataAdapter();
    static MySqlDataReader Reader;
    int id;
    string mysqlConnString = "Server=192.168.1.7; Database=sgsmtc; userid=root; password='s0p0rte'; ";
    string codigo = null;
    string fecha;
    string año;
    string rut;
    string codigoAux;
    bool encontrado=false;
    string nombres, apellidoP, apellidoM, sexo, ingreso, nomEmpresa;
    IngresoModel.IngresoEntitie ingresoEntity;
    int P = 0;
    string[] nombresEmp;
    static string[] nombresEmp2;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack) { BindCursos(); }

            iniciarConexion();
            año = string.Empty;
            fecha = string.Empty;
            ingresoEntity = new IngresoModel.IngresoEntitie();
            DateTime fechaHoy = DateTime.Now.Date;
            fecha = fechaHoy.ToString("yyyy/MM/dd");
            string fecha2 = fechaHoy.ToString("dd/MM/yyyy");
            año = fechaHoy.ToString("yy");
            lblFecha.Text = fecha2;

            iniciarConexion();
            using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
            {
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT Crazon_social FROM cliente_empresa", cn);
                System.Data.DataTable dt = new System.Data.DataTable();
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    ddlEmpresas.DataSource = null;
                    ddlEmpresas.DataBind();
                    ddlEmpresas.DataSource = dt;
                    ddlEmpresas.DataTextField = "Crazon_social";
                    ddlEmpresas.DataBind();
                }
            }
            try
            {
                string[] nombresEmp = new string[ddlEmpresas.Items.Count];
                for (int i = 0; i < ddlEmpresas.Items.Count; i++)
                {
                    ddlEmpresas.SelectedIndex = i;
                    nombresEmp[i] = ddlEmpresas.SelectedItem.ToString();
                }
                nombresEmp2 = nombresEmp;
            }
            catch (Exception ex) { }
        }
        catch (Exception ex) { }
     
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            int ru = Convert.ToInt32(txtRut.Text);
            IngresoModel.Registros reg = ingresoEntity.Registros.FirstOrDefault(a => a.rut == ru);
            ingresoEntity.DeleteObject(reg);
            IngresoModel.Cliente clie = ingresoEntity.Cliente.FirstOrDefault(a => a.rut == ru);
            ingresoEntity.DeleteObject(clie);
            IngresoModel.Codigo cod = ingresoEntity.Codigo.FirstOrDefault(a => a.rut == ru);
            ingresoEntity.DeleteObject(cod);
            ingresoEntity.SaveChanges();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Datos eliminados')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Error en la eliminacion de datos')", true);
        }
    }
    protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlIngreso.SelectedIndex == 1) { txtNomEmpresa.Enabled = true; }
        if (ddlEstado.SelectedIndex == 1) { lblSigla.Visible = true; ddlSigla.Visible = true; lblErrorSigla.Visible = true; lblAsistencia.Visible = true;
        txtAsistencia.Visible = true; lblPorcentaje.Visible = true; RangeValidator1.Visible = true; txtAsistencia.ReadOnly = false; ddlSigla.Enabled = true;
        }
        else
        {
            lblSigla.Visible = true; ddlSigla.Visible = true; ddlSigla.SelectedIndex = 0; lblErrorSigla.Visible = false; lblAsistencia.Visible = true;
            txtAsistencia.Visible = true; lblPorcentaje.Visible = true; txtAsistencia.Text = string.Empty; RangeValidator1.Visible = false; lblNombreCurso.Visible = false; txtAsistencia.ReadOnly = true; ddlSigla.Enabled = false;
        }   
    }
    protected void btnModificar_Click(object sender, EventArgs e)
    {

    string confirmValue = Request.Form["confirm_value"];
    if (confirmValue == "Si")
    {

        try
        {
            id = Convert.ToInt32(lblID.Text);
            int idO = id;
            iniciarConexion();
            Comando.CommandText = "SELECT estado FROM curso_registros WHERE id=" + idO;
            Comando.Connection = connection;
            Reader = Comando.ExecuteReader();
            string estadx="";
            using (Reader)
                if (Reader.Read())
                {
                    estadx=Reader["estado"].ToString();
                }

            if (estadx == "Aprobado")
            {
                Comando.CommandText = "SELECT rut,codigoA,id,estado FROM curso_registros WHERE id=" + idO;
                Comando.Connection = connection;
                Reader = Comando.ExecuteReader();
                string r, c, ix, fex;
                r = string.Empty;
                c = string.Empty;
                ix = string.Empty;
                fex = string.Empty;
                using (Reader)
                    if (Reader.Read())
                    {
                        r = Reader["rut"].ToString();
                        c = Reader["codigoA"].ToString();
                        ix = Reader["id"].ToString();
                    }
                DateTime fechaH = DateTime.Now.Date;
                fex = fechaH.ToString("yyyy/MM/dd");
                lblError.Visible = true;
                lblError.Text = r + c + ix;
                string strSQL = "INSERT INTO curso_registros_old VALUES('" + r + "','" + c + "'," + ix + "," + 0 + ",'" + fex + "');";
                Comando.CommandText = strSQL;
                Comando.Connection = connection;
                Comando.ExecuteNonQuery();
            }
            rut = txtRut.Text;
            string curso = txtCurso.Text;
            string estado = ddlEstado.SelectedItem.ToString();
            string fechaCurso = fechas3();
            string sig = ddlSigla.SelectedValue;
            string ids = lblIdFecha.Text;
            int idCurso = Convert.ToInt32(lblCursoId.Text);
            string fechas = txtFecha.Text;
           
            int asistencia = 0;
            iniciarConexion();
            System.Data.DataTable dt = new System.Data.DataTable();
            using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
            {
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM curso_registros WHERE rut='" + rut + "' AND id_curso=" + idCurso + " AND id_fechas='" + ids + "' AND id NOT IN('" + id + "') ", cn);
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string message = "alert('Este rut contiene un registro en ese curso y en las mismas fechas.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                }

                if (ddlEstado.SelectedIndex != 0 && dt.Rows.Count <= 0)
                {

                    if (ddlEstado.SelectedIndex == 1 && ddlSigla.SelectedIndex != 0 && dt.Rows.Count <= 0)
                    {
                        for (int i = 0; i < 1; )
                        {
                            codigo = "MTC" + año + sig + CreateRandomCode();
                            bool value = codigoExistente(codigo);
                            if (value == false)
                            {
                                i++;
                                insertarCodigo(codigo, rut);
                            }
                        }
                        if (txtAsistencia.Text != string.Empty)
                        {
                            asistencia = Convert.ToInt32(txtAsistencia.Text);
                        }
                        actualizarRegistro(rut, curso, fechaCurso, fecha, codigo, estado, id, fechas, ids, idCurso, asistencia);
                        LimpiarControles();
                        lblVeri.Visible = true; txtRut.ReadOnly = false; txtRut.Enabled = true;
                        btnModificar.Visible = false;
                        lblIdFecha.Text = string.Empty;
                        string message = "alert('Registro Modificado')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                    }
                    if (ddlEstado.SelectedIndex == 2 && dt.Rows.Count <= 0)
                    {
                        string code = lblCodigo.Text;
                        if (txtAsistencia.Text != string.Empty)
                        {
                            asistencia = Convert.ToInt32(txtAsistencia.Text);
                        }
                        eliminarCodigo(code);
                        actualizarRegistro(rut, curso, fechaCurso, fecha, null, estado, id, fechas, ids, idCurso, asistencia);
                        LimpiarControles();
                        lblVeri.Visible = true; txtRut.ReadOnly = false; txtRut.Enabled = true;
                        lblIdFecha.Text = string.Empty;
                        string message = "alert('Registro Modificado')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    }
                    txtRut.ReadOnly = true;
                    lblVeri.Visible = false;
                    gvLista.Visible = false;
                    gvLista.DataSource = null;
                    gvLista.DataBind();
                    btnModificar.Visible = false;
                }
                else
                {
                    string message = "alert('Debe seleccionar un estado')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
            }
        }
        catch (Exception ex)
        {
            string message = "alert('Error en modificación" + ex.Message + "')";
            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
        }
    }
    else
    {
        string message = "alert('Se ha cancelado la modificación')";
        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
    }


    }
   
    protected void btnCliente_Click(object sender, EventArgs e)
    {
        try
        {
            bool vali = false;
            string val = txtRut.Text;
            if (val.Length == 11 || val.Length == 12)
            {
                rut = txtRut.Text;
                nombres = txtNombres.Text;
                apellidoP = txtApellidoP.Text;
                apellidoM = txtApellidoM.Text;
                sexo = ddlSexo.SelectedValue;
                ingreso = ddlIngreso.SelectedValue;
                if (ingreso == "Empresa")
                {
                    nomEmpresa = txtNomEmpresa.Text;
                }
                else { nomEmpresa = null; }
                vali=rutExistente(rut);
                string telefono, email;
                telefono = txtTelefono.Text;
                email = txtEmail.Text;
                if (vali != true)
                {
                    insertarCliente(rut, nombres, apellidoP, apellidoM, sexo, ingreso, nomEmpresa,telefono,email);
                    LimpiarControles();
                    Desconectar();
                    lblError.Visible = false;
                    string message = "alert('Cliente Agregado')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }

                else
                {
                    string message = "alert('Error en el ingreso del cliente')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true); 
                    lblError.Visible = true; lblError.Text = "Rut ya existe en la BD"; }

                
            }
        }
        catch (Exception ex) { }
    }
    public void LimpiarControles()
    {
        txtCurso.Text = string.Empty;
        ddlEstado.SelectedIndex = 0;
        txtApellidoP.Text = string.Empty;
        txtApellidoM.Text = string.Empty;
        txtFecha.Text = string.Empty;
        txtNombres.Text = string.Empty;
        txtRut.Text = string.Empty;
        txtNomEmpresa.Text = string.Empty;
        ddlSexo.SelectedIndex = 0;
        ddlIngreso.SelectedIndex = 0;
        lblNomEmpresa.Visible = false;
        txtNomEmpresa.Visible = false;
        btnAgregar.Visible = false;
        txtFecha.Enabled = false;
        ddlEstado.Enabled = false;
        txtCurso.Enabled = false;
        txtEmail.Text = string.Empty;
        txtTelefono.Text = string.Empty;
        txtDias.Text = string.Empty;
        calMulti.SelectedDates = null;
        calMulti.DatePickerImagePath = null;
        this.calMulti.SelectedDate = new DateTime();
        calMulti.SelectedDates.Clear();
        calMulti.Visible = false;
        calBD.SelectedDates = null;
        calBD.DatePickerImagePath = null;
        this.calBD.SelectedDate = new DateTime();
        calBD.SelectedDates.Clear();
        calBD.Visible = false;
        lblCompletos.Visible = false;
        lblDerror.Visible = false;
        lblErrorSigla.Visible = false;
        ddlSigla.Visible = true;
        lblSigla.Visible = true;
        ddlSigla.Enabled = false;
        ddlSigla.SelectedIndex = 0;
        lblNombreCurso.Visible = false;
        lblIdFecha.Text = string.Empty;
        lblAsistencia.Visible = true;
        txtAsistencia.Visible = true; lblPorcentaje.Visible = true;
        txtAsistencia.Text = string.Empty;
        txtAsistencia.ReadOnly = true;
        ImageButton1.Enabled = false;
        ImageButton2.Enabled = false;
        lblRutEmpresa.Visible = false;
        lblRutEmpresa.Text = string.Empty;
    }
    protected void btnModificarCliente_Click(object sender, EventArgs e)
    {

    }
    public string CreateRandomCode()
    {
            Random rand = new Random();
            int codeCount = 7;
            string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;

            
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(36);
                if (temp != -1 && temp == t)
                {
                    return ""+CreateRandomCode();
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
    }
    public string sigla(int index)
    {
        string value = null;
        if(index==0)
        {
            value = "P6A";
        }
        if (index == 1) 
        {
            value = "P6B";
        }
        if(index==2)
        {
            value="PRA";
        }
    return value; 
    }
    public void fechasDip()
    {
        int o = 0;
        try
        {
            string fechas = txtFecha.Text;
            DateTime fe;
            string fei = fechas;
            string palabra = fechas;
            string[] array = palabra.Split(';');
            DateTime fe2;
            DateTime fe3;
            string[] array2;
            for (int i = 0; i < array.Length; i++)
            {
                if (i <= array.Length - 3)
                {
                    fe2 = Convert.ToDateTime(array[i + 1].ToString());
                }
                else
                {
                    fe2 = Convert.ToDateTime(array[i].ToString());
                }
                fe3 = Convert.ToDateTime(array[i].ToString());
                if (fe2.ToString("MM") == fe3.ToString("MM"))
                {
                    if (i == array.Length - 3 && o == 0)
                    {
                        fe = Convert.ToDateTime(array[i].ToString());
                        lblFecha2.Text += fe.ToString("dd") + " y ";
                    }
                    if (i == array.Length - 3 && o == 1)
                    {
                        fe = Convert.ToDateTime(array[i].ToString());
                        lblFecha2.Text += fe.ToString("dd") + ",";
                    }
                    if (i != array.Length - 2 && i != array.Length - 3)
                    {
                        fe = Convert.ToDateTime(array[i].ToString());
                        lblFecha2.Text += fe.ToString("dd") + ",";
                    }
                    if (i == array.Length - 2)
                    {
                        fe = Convert.ToDateTime(array[i].ToString());
                        lblFecha2.Text += fe.ToString("dd") + " de " + ConvertirPrimeraLetraEnMayuscula(obtenerNombreMesNumero(Convert.ToInt32(fe.ToString("MM")))) + " de " + fe.ToString("yyyy");
                    }
                }
                if (fe2.ToString("MM") != fe3.ToString("MM"))
                {
                    o = 1;
                    lblFecha2.Text += fe3.ToString("dd") + " de " + ConvertirPrimeraLetraEnMayuscula(obtenerNombreMesNumero(Convert.ToInt32(fe3.ToString("MM")))) + " y ";
                }
            }
        }
        catch (Exception ex) { }
    }
    protected void btnGenerar_Click(object sender, EventArgs e)
    {
        //Response.Redirect("Diplomas.aspx?parametro="+txtRut.Text.ToString()+"&parametro2="+lblCodigo.Text.ToString());  

         DateTime now = DateTime.Now;
         int mes2 = Convert.ToInt32(now.Month);
         string ano2 = now.ToString("yyyy");
         string dia2 = Convert.ToString(now.Day);
         string me2 = obtenerNombreMesNumero(mes2);
         me2 = ConvertirPrimeraLetraEnMayuscula(me2);

         string fecom = "";
         fecom=me2.ToString()+" "+dia2.ToString()+" de "+ano2.ToString();
      
         int o = 0;
         var webAppPath = Context.Server.MapPath("~/");
         var rutaPdf = ConfigurationManager.AppSettings["pdfPath2"];
         try
         {
             MemoryStream ms = new MemoryStream();

             RandomAccessFileOrArray form = new RandomAccessFileOrArray(webAppPath + rutaPdf);

             PdfReader reader = new PdfReader(form, null);
             PdfStamper stamper = new PdfStamper(reader, ms);
             fechasDip();

             string nombre = txtNombres.Text + " " + txtApellidoP.Text + " " + txtApellidoM.Text;
             string sex = " ";
             if (ddlSexo.SelectedIndex == 0) { sex = "la señorita:"; }
             if (ddlSexo.SelectedIndex == 1) { sex = "el señor:"; }

             stamper.AcroFields.SetField("lblSexo", sex.ToString());
             stamper.AcroFields.SetField("lblNombre", nombre.ToString());
             stamper.AcroFields.SetField("lblCurso", txtCurso.Text);
             stamper.AcroFields.SetField("lblFecha", lblFecha2.Text);
             stamper.AcroFields.SetField("lblFechaActual", fecom.ToString());
             stamper.AcroFields.SetField("lblCodigo", lblCodigo.Text);

             stamper.FormFlattening = true;
             stamper.Close();

             var buffer = ms.GetBuffer();

             Response.Clear();
             Response.AddHeader("Content-Type", "application/pdf");
             Response.AddHeader("Content-Disposition", "attachment; filename=" + lblCodigo.Text + ".pdf; size=" + buffer.Length.ToString());
             Response.BinaryWrite(buffer);
             Context.ApplicationInstance.CompleteRequest();
         }
         catch (Exception ex)
         { lblError.Text = ex.Message; lblError.Visible = true; }
         }
         
    private void iniciarConexion()
    {
        try
        {
            connection.ConnectionString = mysqlConnString;
            connection.Open();
        }
        catch (Exception ex) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Error al conectarse a la base de datos" + ex + "')", true); }
    }

    public void Desconectar()
    {
        connection.Close();
    }
    public void insertarCliente(string rut, string nombres, string apellidoP, string apellidoM, string sexo, string ingreso, string nomEmpresa,string telefono,string email)
    {
        string strSQL = "INSERT INTO curso_cliente VALUES('"+rut+"','"+nombres+"','"+apellidoP+"','"+apellidoM+"','"+sexo+"','"+ingreso+"','"+nomEmpresa+"','"+telefono+"','"+email+"');";
       
 Comando.CommandText = strSQL;
        Comando.Connection = connection;
        Comando.ExecuteNonQuery();
    }
    public bool rutExistente(string rut)
    {
        bool validador = false;
            try
            {
                iniciarConexion();
         Comando.CommandText = "SELECT * FROM curso_cliente WHERE rut='" + rut + "'";
         Comando.Connection = connection;
         Reader=Comando.ExecuteReader();
         using(Reader)
             if (Reader.Read())
             {
                 encontrado = true;
                 
             }
             else { encontrado=false; }
                return encontrado;
            }
            catch (Exception ex) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Rut formato erroneo" + ex + "')", true); }
            return validador;
    }
    public bool rutCodigo(string codigo)
    {
        bool validador = false;
        try
        {
            iniciarConexion();
            Comando.CommandText = "SELECT * FROM curso_registros WHERE codigoA='" + codigo + "'";
            Comando.Connection = connection;
            Reader = Comando.ExecuteReader();
            using (Reader)
                if (Reader.Read())
                {
                    encontrado = true;

                }
                else { encontrado = false; }
            return encontrado;
        }
        catch (Exception ex) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Rut formato erroneo" + ex + "')", true); }
        return validador;
    }
    public bool codigoExistente(string codigoA)
    {
        bool validador = false;
        try
        {
            iniciarConexion();
            Comando.CommandText = "SELECT * FROM curso_codigo WHERE codigoA='" + codigoA + "'";
            Comando.Connection = connection;
            Reader = Comando.ExecuteReader();
            using (Reader)
                if (Reader.Read())
                {
                    validador = true;
                }
                else { validador = false; }
            return encontrado;
        }
        catch (Exception ex) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('" + ex + "')", true); }
        return validador;
    }
protected void ddlIngreso_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlIngreso.SelectedIndex == 1) { txtNomEmpresa.Visible = true; lblNomEmpresa.Visible = true; rfvEmpresa.Visible = true; }
        else { txtNomEmpresa.Visible = false; lblNomEmpresa.Visible = false; rfvEmpresa.Visible = false; }
        if (ddlIngreso.SelectedIndex == 1 && ddlSeleccion.SelectedIndex==0) { txtNomEmpresa.Visible = true; lblNomEmpresa.Visible = true; rfvEmpresa.Visible = true; txtNomEmpresa.ReadOnly = false; }
        else { txtNomEmpresa.ReadOnly = true; }
    }
 protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSeleccion.SelectedIndex == 0) { btnCliente.Visible = true; btnListar.Visible = false; btnBusqueda.Visible = false;
        txtBuscarRut.Visible = false; gvLista.Visible = false; rvRut.Visible = true; rfvNombres.Visible = true; rfvA1.Visible = true;
        rfvA2.Visible = true; txtListar.Visible = false; LimpiarControles(); txtNombres.ReadOnly = false; txtApellidoM.ReadOnly = false; txtApellidoP.ReadOnly = false; ddlSexo.Enabled = true; ddlIngreso.Enabled = true;
        txtNomEmpresa.Visible = false; txtRut.ReadOnly = false; txtCurso.Enabled = true; ddlEstado.Enabled = false; txtFecha.Enabled = true; btnAgregar.Visible = false; lblError.Visible = false;
        ImageButton2.Enabled = false; txtTelefono.ReadOnly = false; txtEmail.ReadOnly = false; btnModificarCli.Visible = false; btnGenerar.Visible = false; btnCertificado.Visible = false; txtDias.Text = string.Empty;
        txtDias.Enabled = true;
        ImageButton1.Enabled = false;
        calMulti.Visible = false;
        calBD.Visible = false;
        lblCompletos.Visible = false;
        lblDerror.Visible = false;
        lblSigla.Visible = true; ddlSigla.Enabled=false; lblErrorSigla.Visible = false;
        lblNombreCurso.Visible = false;
        lblVeri1.Visible = false;
        lblAsistencia.Visible = true;
        txtAsistencia.ReadOnly = true;
        txtAsistencia.Visible = true; lblPorcentaje.Visible = true;
        txtDias.ReadOnly = true;
        txtCurso.ReadOnly = true;
        txtFecha.ReadOnly = true;
        ddlSigla.Visible = true;
        gvCurso.Visible = false;
        gvLista.Visible = false;
        calBD.Visible = false;
        calMulti.Visible = false;
        ddlSigla.Enabled = false;
        lblRutEmpresa.Visible = false;
        }
        if (ddlSeleccion.SelectedIndex == 1)
        {
            lblVeri.Visible = false;btnCliente.Visible = false; btnListar.Visible = false;btnBusqueda.Visible = true;
            txtBuscarRut.Visible = true; gvLista.Visible = false; rvRut.Visible = false;rfvNombres.Visible = false; rfvA1.Visible = false;
            rfvA2.Visible = false;  txtListar.Visible = false; LimpiarControles();  rfvEmpresa.Visible = false; 
            txtNombres.ReadOnly = true; txtApellidoM.ReadOnly = true; txtApellidoP.ReadOnly = true; ddlSexo.Enabled = false;
            ddlIngreso.Enabled = false; txtNomEmpresa.Visible = false; txtRut.ReadOnly = true; txtCurso.Enabled = true; ddlEstado.Enabled = false; txtFecha.Enabled = true; btnAgregar.Visible = false;
            lblError.Visible = false;
            ImageButton2.Enabled = false;
            txtTelefono.ReadOnly = true;
            txtEmail.ReadOnly = true;
            btnModificarCli.Visible = false;
            btnGenerar.Visible = false;
            btnModificar.Visible = false;
            btnCertificado.Visible = false;
            txtDias.Text = string.Empty;
            txtDias.Enabled = true;
            ImageButton1.Enabled = false;
            calMulti.Visible = false;
            calBD.Visible = false;
            lblCompletos.Visible = false;
            lblDerror.Visible = false;
            lblSigla.Visible = true; ddlSigla.Visible = true; lblErrorSigla.Visible = false;
            lblNombreCurso.Visible = false;
            lblVeri1.Visible = true;
            lblAsistencia.Visible = true;
            txtAsistencia.ReadOnly = true;
            txtAsistencia.Visible = true; lblPorcentaje.Visible = true;
            txtDias.ReadOnly = true;
            txtCurso.ReadOnly = true;
            txtFecha.ReadOnly = true;
            ddlSigla.Visible = true;
            gvCurso.Visible = false;
            gvLista.Visible = false;
            calBD.Visible = false;
            calMulti.Visible = false;
            ddlSigla.Enabled = false;
            lblRutEmpresa.Visible = false;
        }
        if (ddlSeleccion.SelectedIndex == 2)
        {
            lblVeri.Visible = false;
            btnCliente.Visible = false; btnListar.Visible = true; btnBusqueda.Visible = false;
            txtBuscarRut.Visible = false; gvLista.Visible = false; rvRut.Visible = false; rfvNombres.Visible = false; rfvA1.Visible = false;
            rfvA2.Visible = false; txtListar.Visible = true; LimpiarControles(); rfvEmpresa.Visible = false; txtNombres.ReadOnly = true; txtApellidoM.ReadOnly = true; txtApellidoP.ReadOnly = true; ddlSexo.Enabled = false; ddlIngreso.Enabled = false;
            txtCurso.Enabled = true; ddlEstado.Enabled = false; txtFecha.Enabled = true; btnAgregar.Visible = false;
            txtNomEmpresa.Visible = false;
            lblError.Visible = false;
            txtRut.ReadOnly = true;
            ImageButton2.Enabled = false;
            txtTelefono.ReadOnly = true;
            txtEmail.ReadOnly = true;
            btnModificarCli.Visible = false;
            btnGenerar.Visible = false;
            btnModificar.Visible = false;
            btnCertificado.Visible = false;
            txtDias.Text = string.Empty;
            txtDias.Enabled = true;
            ImageButton1.Enabled = false;
            calMulti.Visible = false;
            calBD.Visible = false;
            txtAsistencia.ReadOnly = true;
            lblCompletos.Visible = false;
            lblDerror.Visible = false;
            lblSigla.Visible = true; ddlSigla.Visible = true; lblErrorSigla.Visible = false;
            lblNombreCurso.Visible = false;
            lblVeri1.Visible = true;
            lblAsistencia.Visible = true;
            txtAsistencia.Visible = true; lblPorcentaje.Visible = true;
            txtDias.ReadOnly = true;
            txtCurso.ReadOnly = true;
            txtFecha.ReadOnly = true;
            ddlSigla.Visible = true;
            gvCurso.Visible = false;
            gvLista.Visible = false;
            calBD.Visible = false;
            calMulti.Visible = false;
            ddlSigla.Enabled = false;
            lblRutEmpresa.Visible = false;
        }
        if (ddlSeleccion.SelectedIndex == 3) { LimpiarControles(); txtNombres.ReadOnly = true; txtApellidoM.ReadOnly = true; txtApellidoP.ReadOnly = true; ddlSexo.Enabled = false; ddlIngreso.Enabled = false;
        txtNomEmpresa.Visible = false; lblNomEmpresa.Visible = false; txtNomEmpresa.ReadOnly = true; btnCliente.Visible = false; btnListar.Visible = false; btnBusqueda.Visible = false;
        txtBuscarRut.Visible = false; gvLista.Visible = false; rvRut.Visible = false; rfvNombres.Visible = false; rfvA1.Visible = false; txtListar.Visible = false; txtRut.ReadOnly = false;
        rfvA2.Visible = false; rfvEmpresa.Visible = false; btnModificarCli.Visible = false;
        if (ddlSeleccion.SelectedIndex == 3 && txtRut.Text == string.Empty)      
        {
            lblVeri.Visible = true;
        }
        lblError.Visible = false;
        txtRut.ReadOnly = false; txtCurso.Enabled = true; ddlEstado.Enabled = false; txtFecha.Enabled = true; btnAgregar.Visible = false;
        ImageButton2.Enabled = false;
        txtTelefono.ReadOnly = true;
        txtEmail.ReadOnly = true;
        btnModificarCli.Visible = false;
        btnGenerar.Visible = false;
        btnModificar.Visible = false;
        btnCertificado.Visible = false;
        txtDias.Text = string.Empty;
        txtDias.Enabled = true;
        ImageButton1.Enabled = false;
        calMulti.Visible = false;
        calBD.Visible = false;
        lblCompletos.Visible = false;
        lblDerror.Visible = false;
        calMulti.AllowDeselect = true;
        calMulti.AllowSelectRegular = true;
        lblSigla.Visible = true; ddlSigla.Visible = true; lblErrorSigla.Visible = false;
        lblNombreCurso.Visible = false;
        lblVeri1.Visible = false;
        lblAsistencia.Visible = true;
        txtAsistencia.ReadOnly = true;
        txtAsistencia.Visible = true; lblPorcentaje.Visible = true;
        txtDias.ReadOnly = true;
        txtCurso.ReadOnly = true;
        txtFecha.ReadOnly = true;
        ddlSigla.Visible = true;
        gvCurso.Visible = false;
        gvLista.Visible = false;
        calBD.Visible = false;
        calMulti.Visible = false;
        ddlSigla.Enabled = false;
        lblRutEmpresa.Visible = false;
        }
        if (ddlSeleccion.SelectedIndex == 4) 
        {
            try
            {
                lblVeri.Visible = false; btnCliente.Visible = false; btnListar.Visible = false; btnBusqueda.Visible =false;
                txtBuscarRut.Visible = false; gvLista.Visible = false; rvRut.Visible = true; rfvNombres.Visible = true; rfvA1.Visible = true;
                rfvA2.Visible = true; txtListar.Visible = false; LimpiarControles(); rfvEmpresa.Visible = false; txtRut.ReadOnly = false;
                txtNombres.ReadOnly = true; txtApellidoM.ReadOnly = true; txtApellidoP.ReadOnly = true; ddlSexo.Enabled = false;
                ddlIngreso.Enabled = false; txtNomEmpresa.Visible = false; txtCurso.Enabled = true; ddlEstado.Enabled = false; txtFecha.Enabled = true; btnAgregar.Visible = false;
                lblError.Visible = false;
                txtNomEmpresa.ReadOnly = false;
                ImageButton2.Enabled = false;
                if (ddlSeleccion.SelectedIndex == 4 && txtRut.Text == string.Empty)
                {
                    lblVeri.Visible = true;
                }
                txtTelefono.ReadOnly = true;
                txtEmail.ReadOnly = true;
                btnModificarCli.Visible = true;
                btnGenerar.Visible = false;
                btnModificar.Visible = false;
                btnCertificado.Visible = false;
                txtDias.Text = string.Empty;
                txtDias.Enabled = true;
                ImageButton1.Enabled = false;
                calMulti.Visible = false;
                calBD.Visible = false;
                lblCompletos.Visible = false;
                lblDerror.Visible = false;
                txtAsistencia.ReadOnly = true;
                lblSigla.Visible = true; ddlSigla.Visible = true; lblErrorSigla.Visible = false;
                lblNombreCurso.Visible = false;
                lblVeri1.Visible = false;
                lblAsistencia.Visible = true;
                txtAsistencia.Visible = true; lblPorcentaje.Visible = true;
                txtDias.ReadOnly = true;
                txtCurso.ReadOnly = true;
                txtFecha.ReadOnly = true;
                ddlSigla.Visible = true;
                gvCurso.Visible = false;
                gvLista.Visible = false;
                calBD.Visible = false;
                calMulti.Visible = false;
                ddlSigla.Enabled = false;
                lblRutEmpresa.Visible = false;
                
            }
            catch (Exception ex) { }
        }
        if (ddlSeleccion.SelectedIndex == 5) 
        {
            lblVeri.Visible = false; btnCliente.Visible = false; btnListar.Visible = true; btnBusqueda.Visible = false;
            txtBuscarRut.Visible = false; gvLista.Visible = false; rvRut.Visible = false; rfvNombres.Visible = false; rfvA1.Visible = false;
            rfvA2.Visible = false; txtListar.Visible = true; LimpiarControles(); rfvEmpresa.Visible = false;
            txtNombres.ReadOnly = true; txtApellidoM.ReadOnly = true; txtApellidoP.ReadOnly = true; ddlSexo.Enabled = false;
            ddlIngreso.Enabled = false; txtNomEmpresa.Visible = false; txtRut.ReadOnly = true; txtCurso.Enabled = true; ddlEstado.Enabled = false; txtFecha.Enabled = true; btnAgregar.Visible = false;
            lblError.Visible = false;
            ImageButton2.Enabled = false;
            txtTelefono.ReadOnly = true;
            txtEmail.ReadOnly = true;
            btnModificarCli.Visible = false;
            btnGenerar.Visible = false; btnModificar.Visible = false;
            btnCertificado.Visible = false;
            txtDias.Text = string.Empty;
            txtDias.Enabled = true;
            ImageButton1.Enabled = false;
            calMulti.Visible = false;
            calBD.Visible = false;
            lblCompletos.Visible = false;
            lblDerror.Visible = false;
            lblSigla.Visible = true; ddlSigla.Visible = true; lblErrorSigla.Visible = false;
            lblNombreCurso.Visible = false;
            lblVeri1.Visible = true;
            lblAsistencia.Visible = true;
            txtAsistencia.ReadOnly = true;
            txtAsistencia.Visible = true; lblPorcentaje.Visible = true;
            txtDias.ReadOnly = true;
            txtCurso.ReadOnly = true;
            txtFecha.ReadOnly = true;
            ddlSigla.Visible = true;
            gvCurso.Visible = false;
            gvLista.Visible = false;
            calBD.Visible = false;
            calMulti.Visible = false;
            ddlSigla.Enabled = false;
            lblRutEmpresa.Visible = false;

        }
        if (ddlSeleccion.SelectedIndex == 6) { Response.Redirect("Reportes.aspx"); lblVeri1.Visible = false; }
    }
 protected void txtRut_TextChanged(object sender, EventArgs e)
    {
      
     lblError.Visible = false;
      string texto=txtRut.Text;
      txtRut.Text=Rut(texto);
    if (ddlSeleccion.SelectedIndex == 3 && txtRut.Text != string.Empty)
    {
        lblVeri.Visible = false;
        string var = txtRut.Text;
        lblError.Visible = false;
        buscarCliente(var);

    }
    if (ddlSeleccion.SelectedIndex == 4 && txtRut.Text != string.Empty)
    {
        lblVeri.Visible = false;
        string var = txtRut.Text;
        lblError.Visible = false;
        buscarCliente(var);
        txtNomEmpresa.ReadOnly = false;

    }
   

    }
 public string Rut(string texto){
       try
       {
           var tmpstr = "";
           int largo;
           int cnt;
           for (int i = 0; i < texto.Length; i++)
           {
               if (texto[i] != ' ' && texto[i] != '.' && texto[i] != '-')
               {
                   tmpstr = tmpstr + texto[i];
               }
           }
           texto = tmpstr;
           largo = texto.Length;
           var invertido = "";
           for (int i = (largo - 1), j = 0; i >= 0; i--, j++)
               invertido = invertido + texto[i];
           var dtexto = "";
           dtexto = dtexto + invertido[0];
           dtexto = dtexto + '-';
           for (int i = 0; i < largo; i++)
           {
               if (texto[i] != '0' && texto[i] != '1' && texto[i] != '2' && texto[i] != '3' && texto[i] != '4' && texto[i] != '5' && texto[i] != '6' && texto[i] != '7' && texto[i] != '8' && texto[i] != '9' && texto[i] != 'k' && texto[i] != 'K')
               {
                   ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Rut invalido')", true);
               }
           }
           cnt = 0;

           for (int i = 1, j = 2; i < largo; i++, j++)
           {
               if (cnt == 3)
               {
                   dtexto = dtexto + '.';
                   j++;
                   dtexto = dtexto + invertido[i];
                   cnt = 1;
               }
               else
               {
                   dtexto = dtexto + invertido[i];
                   cnt++;
               }
           }

           invertido = "";
           for (int i = (dtexto.Length - 1), j = 0; i >= 0; i--, j++)
           {
               invertido = invertido + dtexto[i];
           }


           return invertido;
       }
       catch (Exception ex) { return texto; }
   }
 protected void txtListar_TextChanged(object sender, EventArgs e)
 {
     string texto = txtListar.Text;
     txtListar.Text = Rut(texto);
     
 }
 protected void txtBuscarRut_TextChanged(object sender, EventArgs e)
 {
     string texto = txtBuscarRut.Text;
     txtBuscarRut.Text = Rut(texto);
 }
 protected void btnBusqueda_Click(object sender, EventArgs e)
 {
     try
     {
         lblError.Visible = false;
         string bu = txtBuscarRut.Text;
         buscarCliente(bu);
     }
     catch(Exception ex){}
 }
 public void regOld(int idO)
 {
     try
     {
         iniciarConexion();
         Comando.CommandText = "SELECT rut,codigoA,id FROM curso_registros WHERE id=" + idO;
         Comando.Connection = connection;
         Reader = Comando.ExecuteReader();
         using (Reader)
             if (Reader.Read())
             {
                 string r, c, i;
                 r = string.Empty;
                 c = string.Empty;
                 i = string.Empty;
                 r = Reader["rut"].ToString();
                 c = Reader["codigoA"].ToString();
                 i = Reader["id"].ToString();
             }
     }
     catch (Exception ex) { }
 }
 protected void btnListar_Click(object sender, EventArgs e)
 {
     try
     {
         iniciarConexion();
         string vari = txtListar.Text;
         buscarCliente(vari);
         using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
         {
             MySqlDataAdapter adp = new MySqlDataAdapter("SELECT curso AS 'Curso',fechaCurso AS 'Fecha_de_Inicio',codigoA AS 'Codigo',estado AS 'Estado',id AS'N_Registro',fechas AS 'Fechas',id_curso AS 'ID_Curso',asistencia AS 'Asistencia' FROM curso_registros WHERE rut='" + vari + "'", cn);
             System.Data.DataTable dt = new System.Data.DataTable();
             adp.Fill(dt);
             if (dt.Rows.Count > 0)
             {
                 gvLista.Visible = true;
                 lblError.Visible = false;
                 gvLista.DataSource = null;
                 gvLista.DataBind();
                 gvLista.DataSource = dt;
                 gvLista.DataBind();
                 ddlSigla.Enabled = false ;
             }
             else { lblError.Text = "No hay registros con este rut"; lblError.Visible = true; gvLista.Visible = false; }
         }
     }
     catch (Exception ex) { }
 }
 public void buscarCliente(string rut)
 {
     try
     {
         iniciarConexion();
         Comando.CommandText = "SELECT * FROM curso_cliente WHERE rut='" + rut + "'";
         Comando.Connection = connection;
         Reader=Comando.ExecuteReader();
         using(Reader)
             if (Reader.Read())
             {
                 if (ddlSeleccion.SelectedIndex == 3)
                 {
                     txtDias.ReadOnly = false; txtDias.Enabled = true; txtDias.Text = string.Empty; txtDias.Enabled = true; ImageButton1.Enabled = true; txtCurso.Enabled = true; ddlEstado.Enabled = true; txtFecha.Enabled = true; txtRut.ReadOnly = true; ImageButton2.Enabled = true;
                 }
                 if (ddlSeleccion.SelectedIndex == 4) { txtDias.Enabled = false; txtDias.ReadOnly = true; txtDias.Text = string.Empty; txtDias.Enabled = false; ImageButton1.Enabled = false; txtRut.ReadOnly = true; txtNombres.ReadOnly = false; txtApellidoP.ReadOnly = false; txtApellidoM.ReadOnly = false; ddlSexo.Enabled = true; ddlIngreso.Enabled = true; txtTelefono.ReadOnly = false; txtEmail.ReadOnly = false; txtNomEmpresa.ReadOnly = false; }
                 encontrado = true;
                 txtRut.Text = Reader["rut"].ToString();
                 txtNombres.Text = Reader["nombres"].ToString();
                 txtApellidoP.Text = Reader["apellidoP"].ToString();
                 txtApellidoM.Text = Reader["apellidoM"].ToString();
                 ddlSexo.SelectedValue=Reader["sexo"].ToString();
                 ddlIngreso.SelectedValue =Reader["ingreso"].ToString();
                 txtTelefono.Text = Reader["telefono"].ToString();
                 txtEmail.Text = Reader["mail"].ToString();
                 if (ddlSeleccion.SelectedIndex == 3) { ddlSigla.Enabled = true; txtAsistencia.ReadOnly = false; }
                 if(ddlIngreso.SelectedIndex==1){
                     lblNomEmpresa.Visible=true;
                     txtNomEmpresa.Visible=true;
                     txtNomEmpresa.ReadOnly = true;
                     txtNomEmpresa.Text = Reader["nomEmpresa"].ToString();

                 }
                    
                 else
                 {
                     lblNomEmpresa.Visible=false; 
                     txtNomEmpresa.Visible=false;
                     txtNomEmpresa.Text = Reader["nomEmpresa"].ToString();
                 }
                 }
         else { lblError.Visible = true; lblError.Text = "Rut no existe en la BD"; LimpiarControles(); }
  
     }
     catch (Exception ex) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Rut formato erroneo" + ex + "')", true); }
 }

 protected void ddlCurso_SelectedIndexChanged(object sender, EventArgs e)
 {
   
 }
 private void BindCursos()
 {
     try
     {
         using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
         {
             MySqlDataAdapter adp = new MySqlDataAdapter("SELECT curso FROM curso_creado", cn);
             System.Data.DataTable dt = new System.Data.DataTable();
             adp.Fill(dt);
         }
     }
     catch (Exception ex) { lblError.Text = ex.Message; lblError.Visible = true; }
 }
 public void insertarRegistro(string val1, string val2, string val3, string val4, string val5, string val6, string val7, string val8, int val9,int val10)
 {
     iniciarConexion();
     string strSQL = "INSERT INTO curso_registros VALUES('" + val1 + "','" + val2 + "','" + val3 + "','" + val4 + "','" + val5 + "','" + val6 + "'," + 0 + ",'" + val7 + "','" + val8 + "'," + val9 + ","+val10+");";
     Comando.CommandText = strSQL;
     Comando.Connection = connection;
     Comando.ExecuteNonQuery();
 }

 public void insertarCodigo(string codigo, string rut)
 {
     iniciarConexion();
     string strSQL = "INSERT INTO curso_codigo VALUES('"+ codigo + "','" +rut+ "',"+0+");";
     Comando.CommandText = strSQL;
     Comando.Connection = connection;
     Comando.ExecuteNonQuery();
 }

 protected void txtFecha_TextChanged(object sender, EventArgs e)
 {

 }
 protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
 {
     try
     {
         if (gvCurso.Visible == false)
         {
             iniciarConexion();
             using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
             {
                 MySqlDataAdapter adp = new MySqlDataAdapter("SELECT curso, direccion ,id_curso FROM curso_creado", cn);
                 System.Data.DataTable dt = new System.Data.DataTable();
                 adp.Fill(dt);
                 if (dt.Rows.Count > 0)
                 {
                     gvCurso.Visible = true;
                     lblError.Visible = false;
                     gvCurso.DataSource = null;
                     gvCurso.DataBind();
                     gvCurso.DataSource = dt;
                     gvCurso.DataBind();
                 }
             }
             gvCurso.Visible = true;
         }
         else { gvCurso.Visible = false; }
     }
     catch (Exception ex) { }
 }
 /*protected void Cal1_SelectionChanged(object sender, EventArgs e)
 {
     DateTime vari = Convert.ToDateTime(Cal1.SelectedDate.ToShortDateString());
     txtFecha.Text = vari.ToString("yyyy/MM/dd");
     Cal1.Visible = false;
     if (txtFecha.Text != string.Empty) { rfvFecha.Visible = false; }
     else{rfvFecha.Visible=true;}
 }*/
 protected void txtFecha_TextChanged1(object sender, EventArgs e)
 {
 }
 protected void btnAgrega_Click(object sender, EventArgs e)
 {
     try
     {
         rut = txtRut.Text;
         string curso = txtCurso.Text;
         string estado = ddlEstado.SelectedItem.ToString();
         string fechaCurso = fechas3();
         string sig = ddlSigla.SelectedValue;
         string ids = lblIdFecha.Text;
         int idCurso = Convert.ToInt32(lblCursoId.Text);
         string fechas = txtFecha.Text;
         int asistencia = 0;
         iniciarConexion();
         System.Data.DataTable dt = new System.Data.DataTable();
         using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
         {
             MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM curso_registros WHERE rut='" + rut + "' AND id_curso=" + idCurso + " AND id_fechas='" + ids + "'", cn);
             adp.Fill(dt);
             if (dt.Rows.Count > 0)
             {
                 string message = "alert('Este rut contiene un registro en ese curso y en las mismas fechas.')";
                 ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

             }

             if (ddlEstado.SelectedIndex != 0 && dt.Rows.Count<=0)
             {
                 if (ddlEstado.SelectedIndex == 1 && ddlSigla.SelectedIndex != 0 && dt.Rows.Count<=0)
                 {
                     for (int i = 0; i < 1; )
                     {
                         codigo = "MTC" + año + sig + CreateRandomCode();
                         bool value = codigoExistente(codigo);
                         if (value == false)
                         {
                             i++;
                             insertarCodigo(codigo, rut);
                         }
                     }
                     if (txtAsistencia.Text != string.Empty)
                     {
                         asistencia = Convert.ToInt32(txtAsistencia.Text);
                     }
                     insertarRegistro(rut, curso, fechaCurso, fecha, codigo, estado, fechas, ids, idCurso, asistencia);
                     string message = "alert('Registro Agregado')";
                     ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                     LimpiarControles();
                     lblVeri.Visible = true; txtRut.ReadOnly = false; txtRut.Enabled = true;
                     lblIdFecha.Text = string.Empty;
                 }
                 if (ddlEstado.SelectedIndex == 2 && dt.Rows.Count <= 0)
                 {
                     asistencia = 0;
                     insertarRegistro(rut, curso, fechaCurso, fecha, null, estado, fechas, ids, idCurso, asistencia);
                     string message = "alert('Registro Agregado')";
                     ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                     LimpiarControles();
                     lblVeri.Visible = true; txtRut.ReadOnly = false; txtRut.Enabled = true;
                     lblIdFecha.Text = string.Empty;
                 }
             }
             else
             {
                 string message = "alert('Debe seleccionar un estado')";
                 ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
             }
         }
         if (ddlSigla.SelectedIndex == 0)
         {
             string message = "alert('Debe seleccionar una sigla!')";
             ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
         }
     }
     catch (Exception ex)
     {
         string message = "alert('Error en ingreso de registro"+ex.Message+"')";
         ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
     }
 }
 protected void txtNomEmpresa_TextChanged(object sender, EventArgs e)
 {

 }
 protected void gvLista_SelectedIndexChanged(object sender, EventArgs e)
 {
     
 }
 public void fechasInversas3(string pal)
 {
     try
     {
         DateTime fe;
         string fei = txtFecha.Text;
         string[] array = pal.Split(';');
         txtDias.Text = Convert.ToString(array.Length-1);
     }
     catch (Exception ex) { }
 }
 protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
 {
     if (e.CommandName == "Select" && ddlSeleccion.SelectedIndex == 5 || e.CommandName == "Select" && ddlSeleccion.SelectedIndex ==2)
     {

         if (ddlSeleccion.SelectedIndex == 5)
         {
             try
             {
                 txtAsistencia.ReadOnly = false;
                 lblAsistencia.Visible = true;
                 txtAsistencia.Visible = true; lblPorcentaje.Visible = true;
                 BindCursos();
                 Int16 num = Convert.ToInt16(e.CommandArgument);
                 string t2, t6;
                 t2 = gvLista.Rows[num].Cells[1].Text;
                 txtFecha.Text = gvLista.Rows[num].Cells[6].Text;
                 string vari = gvLista.Rows[num].Cells[6].Text;
                 lblCursoId.Text = gvLista.Rows[num].Cells[7].Text;
                 txtAsistencia.Text = gvLista.Rows[num].Cells[8].Text;
                 lblID.Text = gvLista.Rows[num].Cells[5].Text;
                 int id = Convert.ToInt32(lblCursoId.Text);
                 calMulti.SelectedDates = null;
                 calMulti.DatePickerImagePath = null;
                 this.calMulti.SelectedDate = new DateTime();
                 calMulti.SelectedDates.Clear();
                 llenarCal(id);
                 fechasInversas3(vari);
                 fechasInversas();
                 string va=gvLista.Rows[num].Cells[4].Text;
                 ddlEstado.SelectedValue = gvLista.Rows[num].Cells[4].Text;
                 lblSigla.Visible = true; ddlSigla.Visible = true; lblErrorSigla.Visible = false;
                 
                 if (ddlSeleccion.SelectedIndex == 5)
                 {
                     lblID.Text = gvLista.Rows[num].Cells[5].Text; btnModificar.Visible = true;
                     lblCodigo.Text = gvLista.Rows[num].Cells[3].Text;
                 }
                 if (va == "Aprobado")
                 {
                     lblAsistencia.Visible = true; txtAsistencia.Visible = true; lblPorcentaje.Visible = true; RangeValidator1.Visible = true;
                     string cof = lblCodigo.Text; txtAsistencia.ReadOnly = false; ddlSigla.Enabled = true;
                     string cof2 = cof.Substring(5, 3);
                     if (cof2 == "P6A") { ddlSigla.SelectedIndex = 1; }
                     if (cof2 == "P6B") { ddlSigla.SelectedIndex = 2; }
                     if (cof2 == "PRA") { ddlSigla.SelectedIndex = 3; }
                     if (ddlSigla.SelectedIndex == 1) { lblNombreCurso.Text = "Primavera P6 Avanzado"; lblNombreCurso.Visible = true; }
                     if (ddlSigla.SelectedIndex == 2) { lblNombreCurso.Text = "Primavera P6 Básico"; lblNombreCurso.Visible = true; }
                     if (ddlSigla.SelectedIndex == 3) { lblNombreCurso.Text = "Primavera Risk Análisis"; lblNombreCurso.Visible = true; }

                 }
                 else { txtAsistencia.ReadOnly = true; ddlSigla.Enabled = false; lblNombreCurso.Visible = false; lblAsistencia.Visible = true; txtAsistencia.Visible = true; lblPorcentaje.Visible = true; RangeValidator1.Visible = false; lblErrorSigla.Visible = false; }
                 calMulti.AllowDeselect = true;
                 calMulti.AllowSelectRegular = false;
                 txtCurso.Enabled = true;
                 ddlEstado.Enabled = true;
                 txtFecha.Enabled = true;
                 txtCurso.Text =HttpUtility.HtmlDecode(t2);
                 gvLista.Visible = false; 
                 gvLista.DataSource = null;
                 txtDias.Enabled = true;
                 ImageButton1.Enabled = true;
                 txtDias.ReadOnly = false;
                 gvLista.DataBind();
                 ImageButton2.Enabled = true;
                 lblIdFecha.Text = "";
                 iniciarConexion();
                 int numId = Convert.ToInt32(lblID.Text);
                 Comando.CommandText = "SELECT id_fechas FROM curso_registros WHERE id=" + numId;
                 Comando.Connection = connection;
                 Reader=Comando.ExecuteReader();
                 using(Reader)
                     if (Reader.Read())
                     {
                          lblIdFecha.Text= Reader["id_fechas"].ToString();
                     }
                 }

             
             catch (Exception ex) { }
         }
         if (ddlSeleccion.SelectedIndex == 2)
         {
             try
             {
                 txtAsistencia.ReadOnly = true;
                 BindCursos();
                 Int16 num = Convert.ToInt16(e.CommandArgument);
                 string t2, t6;
                 lblAsistencia.Visible = true;
                 txtAsistencia.Visible = true; lblPorcentaje.Visible = true;
                 t2 = gvLista.Rows[num].Cells[1].Text;
                 txtFecha.Text = gvLista.Rows[num].Cells[6].Text;
                 ddlEstado.SelectedValue = gvLista.Rows[num].Cells[4].Text;
                 lblCodigo.Text = gvLista.Rows[num].Cells[3].Text;
                 lblID.Text = gvLista.Rows[num].Cells[5].Text;
                 string vari = gvLista.Rows[num].Cells[6].Text;
                 string va = gvLista.Rows[num].Cells[4].Text;
                 if (va == "Aprobado") { lblAsistencia.Visible = true; txtAsistencia.Visible = true; lblPorcentaje.Visible = true; txtAsistencia.Text = gvLista.Rows[num].Cells[8].Text; }
                 else { lblAsistencia.Visible = true; txtAsistencia.Visible = true; lblPorcentaje.Visible = true; }
                 calMulti.SelectedDates = null;
                 calMulti.DatePickerImagePath = null;
                 this.calMulti.SelectedDate = new DateTime();
                 calMulti.SelectedDates.Clear();
                 calMulti.Visible = false;
                 fechasInversas3(vari);
                 fechasInversas();
                 if (ddlEstado.SelectedIndex == 1) { btnGenerar.Visible = true; btnCertificado.Visible = true; }
                 else { btnGenerar.Visible = false; btnCertificado.Visible = false; }
                 ImageButton2.Enabled = true;
                 calMulti.AllowDeselect = false;
                 calMulti.AllowSelectRegular = false;
                 txtCurso.Enabled = false;
                 ddlEstado.Enabled = false;
                 txtFecha.Enabled = true;
                 txtFecha.ReadOnly = true;
                 txtCurso.Text =HttpUtility.HtmlDecode(t2);
                 gvLista.Visible = false;
                 gvLista.DataSource = null;
                 gvLista.DataBind();
                 txtDias.Enabled = true;
                 ImageButton1.Enabled = false;
                 txtDias.ReadOnly = true;
                 ddlSigla.Enabled = false;
             }
             catch (Exception ex) { }
         }
     }
 }

 protected void txtTelefono_TextChanged(object sender, EventArgs e)
 {

 }
 protected void TextBox1_TextChanged(object sender, EventArgs e)
 {
     lblError.Visible = true;
     //lblError
 }
 protected void btnModificarCli_Click(object sender, EventArgs e)
 {
     try
     {
             bool vali = false;
             rut = txtRut.Text;
             nombres = txtNombres.Text;
             apellidoP = txtApellidoP.Text;
             apellidoM = txtApellidoM.Text;
             sexo = ddlSexo.SelectedValue;
             ingreso = ddlIngreso.SelectedValue;
             if (ingreso == "Empresa")
             {
                 nomEmpresa = txtNomEmpresa.Text;
             }
             else { nomEmpresa = null; }
             string telefono, email;
             telefono = txtTelefono.Text;
             email = txtEmail.Text;
             actualizarCliente(rut, nombres, apellidoP, apellidoM, sexo, ingreso, nomEmpresa, telefono, email);
             string message = "alert('Cliente Modificado')";
             ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
             LimpiarControles();
             Desconectar();
             lblError.Visible = false;
             try
             {
                 lblVeri.Visible = false; btnCliente.Visible = false; btnListar.Visible = false; btnBusqueda.Visible = false;
                 txtBuscarRut.Visible = false; gvLista.Visible = false; rvRut.Visible = true; rfvNombres.Visible = true; rfvA1.Visible = true;
                 rfvA2.Visible = true; txtListar.Visible = false; LimpiarControles(); rfvEmpresa.Visible = false; txtRut.ReadOnly = false;
                 txtNombres.ReadOnly = true; txtApellidoM.ReadOnly = true; txtApellidoP.ReadOnly = true; ddlSexo.Enabled = false;
                 ddlIngreso.Enabled = false; txtNomEmpresa.Visible = false; txtCurso.Enabled = false; ddlEstado.Enabled = false; txtFecha.Enabled = false; btnAgregar.Visible = false;
                 lblError.Visible = false;
                 txtNomEmpresa.ReadOnly = false;
                 ImageButton2.Enabled = false;
                 btnModificarCli.Visible = true;
                 if (ddlSeleccion.SelectedIndex == 4 && txtRut.Text == string.Empty)
                 {
                     lblVeri.Visible = true;
                 }
                 txtTelefono.ReadOnly = true;
                 txtEmail.ReadOnly = true;

             }
             catch (Exception ex)
             {
                 string message2 = "alert('Error en modificación"+ex.Message+"')";
                 ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message2, true);
             }
             
     }
     catch (Exception ex) { }
 }
 public void actualizarCliente(string rut, string nombres, string apellidoP, string apellidoM, string sexo, string ingreso, string nomEmpresa, string telefono, string email)
 {
     string strSQL = "UPDATE curso_cliente set nombres='" + nombres + "',apellidoP='" + apellidoP + "',apellidoM='" + apellidoM + "',sexo='" + sexo + "',ingreso='" + ingreso + "',nomEmpresa='" + nomEmpresa + "',telefono='" + telefono + "',mail='" + email + "' WHERE rut='"+rut+"';";
     Comando.CommandText = strSQL;
     Comando.Connection = connection;
     Comando.ExecuteNonQuery();
 }
 public void actualizarRegistro(string val1, string val2, string val3, string val4, string val5, string val6, int val7, string val8, string val9,int val10,int val11)
 {
     iniciarConexion();
     string strSQL = "UPDATE curso_registros set curso='" + val2 + "',fechaCurso='" + val3 + "',codigoA='" + val5 + "',estado='" + val6 + "',fechas='"+val8+"', id_fechas='"+val9+"',id_curso="+val10+",asistencia="+val11+" WHERE id="+val7+";";
     Comando.CommandText = strSQL;
     Comando.Connection = connection;
     Comando.ExecuteNonQuery();
 }
 public void eliminarCodigo(string codigo)
 {
     iniciarConexion();
     string strSQL = "DELETE FROM curso_codigo WHERE codigoA='" + codigo + "'";
     Comando.CommandText = strSQL;
     Comando.Connection = connection;
     Comando.ExecuteNonQuery();
 }
   /* public void FillPDF(string templateFile, Stream stream){
   

      PdfReader reader = new PdfReader(template);
      PdfStamper stamp = new PdfStamper(reader, stream);
      string nombre = txtNombres.Text + " " + txtApellidoP.Text + " " + txtApellidoM.Text;

       // Introducimos el valor en los campos del formulario...
      stamp.AcroFields.SetField("lblNombreAlumno", "OLI"+nombre);

    
       // Fijamos los valores y enviamos el resultado al stream...
       stamp.FormFlattening = true;
       stamp.Close(); 
  }*/
 protected void btnCertificado_Click(object sender, EventArgs e)
 {
     /*try{
         Response.Clear();
         Response.ContentType = "application/pdf";
         Response.AddHeader("content-disposition", "attachment;filename=Formulario.pdf");
         FillPDF(Server.MapPath("Plantilla.pdf"), Response.OutputStream);
     */

         DateTime now = DateTime.Now;
         int mes = Convert.ToInt32(now.Month);
         string ano = now.ToString("yyyy");
         string dia = Convert.ToString(now.Day);
         string me = obtenerNombreMesNumero(mes);
         me = ConvertirPrimeraLetraEnMayuscula(me);
         //DateTime fe = Convert.ToDateTime(txtFecha.Text);
         lblError.Visible = false;
         string fechain = fechas3();
         string fechafin = fechas4();
         DateTime fe1 = new DateTime();
         DateTime fe2 = new DateTime();
         fe1 = Convert.ToDateTime(fechain);
         fe2 = Convert.ToDateTime(fechafin);
         fechain = fe1.ToString("dd/MM/yyyy");
         fechafin = fe2.ToString("dd/MM/yyyy");
         string not;
         string cof = lblCodigo.Text;
         string cof2=cof.Substring(5, 3);
         string hor="0";
         if (cof2 == "P6A") { hor = "16"; }
         if (cof2 == "P6B") { hor="24";}
         if (cof2 == "PRA") { hor="16";}
         int id=Convert.ToInt32(lblID.Text);
         not=evaBuscar(id);
         string nomEmp="";
         if (ddlIngreso.SelectedIndex == 1) { nomEmp = txtNomEmpresa.Text; }
         
         var webAppPath = Context.Server.MapPath("~/");
         var rutaPdf = ConfigurationManager.AppSettings["pdfPath"];
         try
         {
         MemoryStream ms = new MemoryStream();

         RandomAccessFileOrArray form = new RandomAccessFileOrArray(webAppPath + rutaPdf);
         
         PdfReader reader = new PdfReader(form, null);
         PdfStamper stamper = new PdfStamper(reader, ms);

         string nombre = txtNombres.Text + " " + txtApellidoP.Text + " " + txtApellidoM.Text;
         if (ddlIngreso.SelectedIndex == 0) { nomEmp = nombre; }
         string fechaC = me.ToString() + " " + dia.ToString() + " de " + ano.ToString();
         stamper.AcroFields.SetField("lblCurso", txtCurso.Text+",");
         stamper.AcroFields.SetField("lblEmpresa", nomEmp);
         stamper.AcroFields.SetField("lblHoras", hor);
         stamper.AcroFields.SetField("lblFechaInicio", fechain.ToString());
         stamper.AcroFields.SetField("lblFechaTermino", fechafin.ToString());
         stamper.AcroFields.SetField("lblNombreAlumno", nombre.ToString());
         stamper.AcroFields.SetField("lblRut", txtRut.Text);
         stamper.AcroFields.SetField("lblFechaC", fechaC.ToString());
         stamper.AcroFields.SetField("lblAsistencia", txtAsistencia.Text);
         stamper.AcroFields.SetField("lblEvaluacion", not.ToString());
         stamper.AcroFields.SetField("lblCodigo", lblCodigo.Text);
           
     

         stamper.FormFlattening = true;
         stamper.Close();

         var buffer = ms.GetBuffer();

         Response.Clear();
         Response.AddHeader("Content-Type", "application/pdf");
         Response.AddHeader("Content-Disposition", "attachment; filename="+lblCodigo.Text+".pdf; size=" + buffer.Length.ToString());
         Response.BinaryWrite(buffer);
         Context.ApplicationInstance.CompleteRequest();
         /*Microsoft.Office.Interop.Word.Document Documento;
         Microsoft.Office.Interop.Word.Application MSWord = new Microsoft.Office.Interop.Word.Application();
         File.Copy("C:\\inetpub\\wwwroot\\Ingreso\\Plantilla.doc", "C:\\Certificado" + lblCodigo.Text + ".doc");
         Documento = MSWord.Documents.Open("C:\\Certificado" + lblCodigo.Text + ".doc");
         Documento.Bookmarks["rut"].Range.Text = txtRut.Text;
         Documento.Bookmarks["nombreAlumno"].Range.Text = txtNombres.Text + " " + txtApellidoP.Text;
         Documento.Bookmarks["curso"].Range.Text = txtCurso.Text;
         Documento.Bookmarks["dia"].Range.Text = dia.ToString();
         Documento.Bookmarks["mes"].Range.Text = me.ToString();
         Documento.Bookmarks["año"].Range.Text = ano.ToString();*/
        /*Documento.Bookmarks["fechaInicio"].Range.Text =fe.ToString("dd/MM/yyyy") ;
        /* if ( == 1) { Documento.Bookmarks["horasCurso"].Range.Text = "24"; }
         else { Documento.Bookmarks["horasCurso"].Range.Text = "16"; }*/
       /*Documento.Bookmarks["codigo"].Range.Text = lblCodigo.Text;
         Documento.Save();
         MSWord.Visible = true;*/
     }
         catch (Exception ex) { lblError.Text = ex.Message; lblError.Visible = true; }
 }
 public string evaBuscar(int id)
 {
     string vari = null;
     try{
    iniciarConexion();
            Comando.CommandText = "SELECT nota FROM curso_datos WHERE id_reg=" + id;
            Comando.Connection = connection;
            Reader = Comando.ExecuteReader();
            
            using (Reader)
                if (Reader.Read())
                {
                    
                    vari = Reader["nota"].ToString();
   
                }
                else { vari="Sin evaluación";}
        }
        catch (Exception ex) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Error" + ex.Message + "')", true); }
     return vari;
 }
 private string obtenerNombreMesNumero(int numeroMes)
 {
     try
     {
         DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
         string nombreMes = formatoFecha.GetMonthName(numeroMes);
         return nombreMes;
     }
     catch
     {
         return "Desconocido";
     }
 }
 private static string ConvertirPrimeraLetraEnMayuscula(string texto)
 {
     string str = "";
     str = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(texto);
     return str;
 }
 protected void txtCurso_TextChanged(object sender, EventArgs e)
 {

 }
 protected void gvCurso_RowCommand(object sender, GridViewCommandEventArgs e)
 {
     if (e.CommandName == "Select")
     {
         try
         {
             txtDias.Enabled = true;
             BindCursos();
             Int16 num = Convert.ToInt16(e.CommandArgument);
             string t2, t6;

             txtCurso.Text = HttpUtility.HtmlDecode(gvCurso.Rows[num].Cells[1].Text);
             lblCursoId.Text = gvCurso.Rows[num].Cells[3].Text;
             int id = Convert.ToInt32(lblCursoId.Text);
             calMulti.SelectedDates = null;
             calMulti.DatePickerImagePath = null;
             this.calMulti.SelectedDate = new DateTime();
             calMulti.SelectedDates.Clear();
             calBD.Visible = false;
             calMulti.Visible = false;
             llenarCal(id);
             
             if (ddlSeleccion.SelectedIndex == 5 && txtFecha.Text != string.Empty) 
             {
                 txtFecha.Text = string.Empty;
                 calMulti.AllowSelectRegular = true;
                 lblCompletos.Visible = false;
                 txtDias.Text = string.Empty;
                 ImageButton2.Enabled = false;
             }
        
             txtDias.Text = string.Empty;
             calMulti.SelectedDates = null;
             calMulti.DatePickerImagePath = null;
             this.calMulti.SelectedDate = new DateTime();
             calMulti.SelectedDates.Clear();
             lblCompletos.Visible = false;
             lblDerror.Visible = false;
             ddlSigla.SelectedIndex = 0;
             lblNombreCurso.Visible = false;
             lblIdFecha.Text = string.Empty;
             txtFecha.Text = string.Empty;
             btnAgregar.Visible = false;
             btnModificar.Visible = false;
         }
         catch (Exception ex) { }
         gvCurso.Visible = false;
     }
 }
 public void llenarCal(int id)
 {
     try
     {
         iniciarConexion();
         using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
         {
             MySqlDataAdapter adp = new MySqlDataAdapter("SELECT fecha FROM curso_fechas WHERE id_curso=" + id, cn);
             System.Data.DataTable dt = new System.Data.DataTable();
             adp.Fill(dt);
             if (dt.Rows.Count > 0)
             {
                 DropDownList1.DataSource = dt;
                 DropDownList1.DataTextField = "fecha";
                 DropDownList1.DataBind();
                 DateTime fe;
                 calBD.SelectedDates = null;
                 calBD.DatePickerImagePath = null;
                 this.calBD.SelectedDate = new DateTime();
                 calBD.SelectedDates.Clear();

                 for (int i = 0; i < DropDownList1.Items.Count; i++)
                 {
                     DropDownList1.SelectedIndex = i;
                     fe = Convert.ToDateTime(DropDownList1.SelectedValue);
                     calBD.SelectedDates.Add(fe);
                 }
             }
         }
     }
     catch (Exception ex) { }
 }
 protected void txtDias_TextChanged1(object sender, EventArgs e)
 {
     try
     {
         int va = Convert.ToInt32(txtDias.Text);
         if (va > 0)
         {
             ImageButton2.Enabled = true; calMulti.Visible = false; calBD.Visible = false;
             calMulti.SelectedDates = null;
             calMulti.DatePickerImagePath = null;
             this.calMulti.SelectedDate = new DateTime();
             calMulti.SelectedDates.Clear();
             lblCompletos.Visible = false;
             lblCompletos2.Visible = false;
             calMulti.AllowSelectRegular = true;
             txtFecha.Text = string.Empty;
             btnAgregar.Visible = false;
             lblIdFecha.Text = string.Empty;
         }
         else { ImageButton2.Enabled = false; calMulti.Visible = false; }
     }
     catch (Exception ex) { }
 }
 protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
 {
     if (calMulti.Visible == true)
     {
         calMulti.Visible = false;
         calBD.Visible = false;
     }
     else
     {
         calMulti.Visible = true;
         if (ddlSeleccion.SelectedIndex == 2) { calBD.Visible = false; }
         else{
         calBD.Visible = true;}
     }
     
 }
 public void obtenerID(string fecha, int n)
 {
     try
     {
         DateTime fr = Convert.ToDateTime(fecha);
         string fecha2 = fr.ToString("yyyy/MM/dd");
         iniciarConexion();
         using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
         {
             MySqlDataAdapter adp = new MySqlDataAdapter("SELECT id_cfecha FROM curso_fechas WHERE id_curso=" + n + " AND fecha='" + fecha2 + "'", cn);
             System.Data.DataTable dt = new System.Data.DataTable();
             adp.Fill(dt);
             if (dt.Rows.Count > 0)
             {
                 lblIdFecha.Text += Convert.ToString(dt.Rows[0][0].ToString()) + ";";
                 lblDerror.Visible = false;
             }

         }

     }
     catch (Exception ex)
     {
     }

 }
    public string fechas()
    {
        try
        {
            int ve = Convert.ToInt32(lblCursoId.Text);
            var list = new List<DateTime>();
            var list2 = new List<DateTime>();
            var selectedDates = calMulti.SelectedDates;
            var selectedDatesString = "";
            DateTime fe;
            string fe2, fe3;
            int j = 1;
            int num = Convert.ToInt32(txtDias.Text);
            for (int i = 0; i < selectedDates.Count; i++)
            {
                fe = Convert.ToDateTime(selectedDates[i]);
                list.Add(fe);
                list2 = SortAscending(list);
            }
            if (ddlSeleccion.SelectedIndex == 5 && num>1)
            {
                for (int i = 0; i < selectedDates.Count; i++)
                {
                    fe2 = list2[i].ToString("yyyy/MM/dd");
                   if (list2[j].ToString() != fe2)
                    {
                        obtenerID(fe2, ve);
                        if (j + 1 < selectedDates.Count) { j++; }
                    }
                    if (j == selectedDates.Count - 1) { obtenerID(fe2, ve); }
                    selectedDatesString += fe2 + ";";
                }
            }
            if (ddlSeleccion.SelectedIndex == 3)
            {
                for (int i = 0; i < selectedDates.Count; i++)
                {
                    fe2 = list2[i].ToString("yyyy/MM/dd");
                    obtenerID(fe2, ve);
                    selectedDatesString += fe2 + ";";
                }

            }
            if (ddlSeleccion.SelectedIndex == 5 && num == 1) 
            {
                for (int i = 0; i < 1; i++)
                {
                    fe2 = list2[i].ToString("yyyy/MM/dd");                 
                    obtenerID(fe2, ve);
                    selectedDatesString += fe2 + ";";
                }
            }
            return selectedDatesString.ToString();
        }
        catch (Exception ex) { return ex.Message; }
    }
    public bool validarFecha(string fecha, int n)
    {
        try
        {
            DateTime fr = Convert.ToDateTime(fecha);
            string fecha2 = fr.ToString("yyyy/MM/dd");
            bool validador;
            iniciarConexion();
            using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
            {
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM curso_fechas WHERE id_curso=" + n + " AND fecha='" + fecha2 + "'", cn);
                System.Data.DataTable dt = new System.Data.DataTable();
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    lblDerror.Visible = false;
                    validador = true;
                }
                else { validador = false; lblDerror.Visible = true; }
            }
            return validador;
        }

        catch (Exception ex)
        {
            bool val2 = false;
            return val2;
        }

    }
    static List<DateTime> SortAscending(List<DateTime> list)
    {
        list.Sort((a, b) => a.CompareTo(b));
        return list;
    }
    public void fechasInversas() 
    {
        try
        {
            DateTime fe;
            string fei = txtFecha.Text;
            string palabra = txtFecha.Text;
            string[] array = palabra.Split(';');
            calMulti.SelectedDates = null;
            calMulti.DatePickerImagePath = null;
            this.calMulti.SelectedDate = new DateTime();
            calMulti.SelectedDates.Clear();
            for (int i = 0; i < array.Length; i++)
            {
                fe = Convert.ToDateTime(array[i].ToString());
                calMulti.SelectedDates.Add(fe);
            }
        }
        catch(Exception ex){}
    }
    public string fechas2()
    {
        lblDerror.Visible = true;
        int ve=Convert.ToInt32(lblCursoId.Text);
        bool val=false;
        var list = new List<DateTime>();
        var selectedDates = calMulti.SelectedDates;
        var selectedDatesString = "";
        DateTime fe;
        string fe2;
        for (int i = 0; i < selectedDates.Count; i++)
        {
            fe = Convert.ToDateTime(selectedDates[i]);
            list.Add(fe);
        }
        for (int i = 0; i < selectedDates.Count; i++)
        {
            fe2 = list[i].ToString("yyyy/MM/dd");
            val=validarFecha(fe2,ve);
            
            if (val == true)
            {
                lblDerror.Visible = false;
                selectedDatesString += fe2 + ";";
            }
            else { lblDerror.Visible = true; }
           
   
           

        }
        return selectedDatesString.ToString();
    }
    public string fechas3()
    {
        var list = new List<DateTime>();
        var list2 = new List<DateTime>();
        var selectedDates = calMulti.SelectedDates;
        var selectedDatesString = "";
        DateTime fe;
        string fe2;
        for (int i = 0; i < selectedDates.Count; i++)
        {
            fe = Convert.ToDateTime(selectedDates[i]);
            list.Add(fe);
            list2 = SortAscending(list);
        }
            fe2 = list2[0].ToString("yyyy/MM/dd");
            selectedDatesString = fe2 ;
        return selectedDatesString.ToString();
    }
    public string fechas4()
    {
        var list = new List<DateTime>();
        var list2 = new List<DateTime>();
        var selectedDates = calMulti.SelectedDates;
        var selectedDatesString = "";
        DateTime fe;
        string fe2;
        for (int i = 0; i < selectedDates.Count; i++)
        {
            fe = Convert.ToDateTime(selectedDates[i]);
            list.Add(fe);
            list2 = SortAscending(list);
        }
        fe2 = list2[selectedDates.Count-1].ToString("yyyy/MM/dd");
        selectedDatesString = fe2;
        return selectedDatesString.ToString();
    }
    public void fechasInversas2()
    {
        try
        {
            DateTime fe;
            string fei = lblFechasResp.Text;
            string palabra = lblFechasResp.Text;
            string[] array = palabra.Split(';');
            calMulti.SelectedDates = null;
            calMulti.DatePickerImagePath = null;
            this.calMulti.SelectedDate = new DateTime();
            calMulti.SelectedDates.Clear();
            for (int i = 0; i < array.Length; i++)
            {
                fe = Convert.ToDateTime(array[i].ToString());
                calMulti.SelectedDates.Add(fe);
            }
        }
        catch (Exception ex) { }
    }
    public void fechasInversas4()
    {
        try
        {
            DateTime fe;
            string fei = lblFechasResp.Text;
            string palabra = lblFechasResp.Text;
            string[] array = palabra.Split(';');
            calMulti.SelectedDates = null;
            calMulti.DatePickerImagePath = null;
            this.calMulti.SelectedDate = new DateTime();
            calMulti.SelectedDates.Clear();
            for (int i = 0; i < array.Length-3; i++)
            {
                fe = Convert.ToDateTime(array[i].ToString());
                calMulti.SelectedDates.Add(fe);
            }
        }
        catch (Exception ex) { }
    }

 protected void calMulti_DateChanged(object sender, EventArgs e)
    {
        
     if (txtDias.Text != string.Empty)
     {
         try
         {
             lblCompletos2.Visible = false;
             calMulti.AllowSelectRegular = true;
             string fe2 = calMulti.SelectedDate.ToString();
             int con = Convert.ToInt32(txtDias.Text);
             int con2 = calMulti.SelectedDates.Count;
             if (con == con2 && P == 1) { lblCompletos2.Visible = false; }
             string fer = calMulti.SelectedDate.ToString();
             int n = Convert.ToInt32(lblCursoId.Text);
             bool val = validarFecha(fer, n);
             if (val == false)
             {
                 if (fe2 == "01/01/0001 0:00:00") { lblDerror.Visible = false; con2--; }
                 else
                 {
                     lblFechasResp.Text = fechas2();
                     fechasInversas2();
                     con2--;
                 }
             }
             if (con2 == con)
             {
                 lblIdFecha.Text = string.Empty;
                 lblCompletos2.Visible = false;
                 calMulti.AllowSelectRegular = false;
                 txtFecha.Text = fechas();
                 lblCompletos.Visible = true;
                 if (ddlSeleccion.SelectedIndex == 3) { btnAgregar.Visible = true; }
             }
             else
             {
                 btnAgregar.Visible = false;
                 txtFecha.Text = string.Empty;
                 calMulti.AllowSelectRegular = true;
                 lblCompletos.Visible = false;
             }
             if (con2 > con)
             {
                 btnAgregar.Visible = false;
                 txtFecha.Text = string.Empty;
                 lblCompletos2.Visible = true;
                 lblFechasResp.Text = fechas2();
                 fechasInversas4();
                 con2=con2-2;
                 }
             
         }
         catch (Exception ex)
         {
             lblError.Visible = true;
             lblError.Text = ex.Message;
             calMulti.SelectedDates = null;
             calMulti.DatePickerImagePath = null;
             this.calMulti.SelectedDate = new DateTime();
             calMulti.SelectedDates.Clear();
             calMulti.Visible = false;
             calBD.Visible = false;
             ImageButton2.Enabled = false;
             txtFecha.Text = string.Empty;
             lblCompletos.Visible = false;
             lblDerror.Visible = false;
             calMulti.AllowSelectRegular = true;
         }
     }
     else
     {
         calMulti.SelectedDates = null;
         calMulti.DatePickerImagePath = null;
         this.calMulti.SelectedDate = new DateTime();
         calMulti.SelectedDates.Clear();
         calMulti.Visible = false;
         calBD.Visible = false;
         ImageButton2.Enabled = false;
         txtFecha.Text = string.Empty;
         lblCompletos.Visible = false;
         lblDerror.Visible = false;
         calMulti.AllowSelectRegular = true;
     }
 }
 protected void ddlSigla_SelectedIndexChanged(object sender, EventArgs e)
 {
     if (ddlSigla.SelectedIndex == 0) { lblErrorSigla.Visible = true; lblNombreCurso.Visible = false; }
     else { lblErrorSigla.Visible = false; }
     if (ddlSigla.SelectedIndex == 1) { lblNombreCurso.Text = "Primavera P6 Avanzado"; lblNombreCurso.Visible = true; }
     if (ddlSigla.SelectedIndex == 2) { lblNombreCurso.Text = "Primavera P6 Básico"; lblNombreCurso.Visible = true; }
     if (ddlSigla.SelectedIndex == 3) { lblNombreCurso.Text = "Primavera Risk Análisis"; lblNombreCurso.Visible = true; }
 }
 protected void txtNomEmpresa_TextChanged1(object sender, EventArgs e)
 {

     try
     {
         
         lblRutEmpresa.Visible = true;
         string nomEmp="";
         nomEmp=txtNomEmpresa.Text;
         iniciarConexion();
         Comando.CommandText = "SELECT Crut FROM cliente_empresa WHERE Crazon_social='"+nomEmp+"'";
         Comando.Connection = connection;
         Reader = Comando.ExecuteReader();
         string val = "";
         using (Reader)
         if (Reader.Read())
          {
             val = Reader["Crut"].ToString();
             lblRutEmpresa.Text=val;
          }
         else
         {
             lblRutEmpresa.Text="Esta empresa no esta registrada";
         }
     }
     catch(Exception ex){}
 }
 protected void DropDownList1_SelectedIndexChanged1(object sender, EventArgs e)
 {

 }

 [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
 public static string[] GetCompletionList(string prefixText, int count, string contextKey)
 {
     return (from m in nombresEmp2 where m.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select m).Take(count).ToArray();
 }
 protected void btnModificar_PopupControlExtender_DataBinding(object sender, EventArgs e)
 {

 }
 protected void txtAsistencia_TextChanged(object sender, EventArgs e)
 {
     try
     {
         string asistencia;
         asistencia = txtAsistencia.Text;
         int asis;
         asis = Convert.ToInt32(asistencia);
         if (asis < 57)
         {
             string message = "alert('El alumno esta siendo aprobado con una asistencia menor al 57%!!!')";
             ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
         }
     }
     catch (Exception ex) { }
 }
}
    
    
