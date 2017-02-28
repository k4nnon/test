using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Globalization;
using System.Configuration;
using iTextSharp.text.pdf;
using System.Diagnostics;


public partial class _Default : System.Web.UI.Page
{
    /**
     * @autor: 
     * @co autor: gonzalo zeballos. mail: maxpayne_ga@hotmail.com
     * @Sistema de toma de horas, generación de certificados y administración de cliente; dentro de los cursos Primavera
     * */

    /**
     * id: id: id del registro de cursos realizados por los clientes, obtenido desde la base de datos
     * fecha: fecha actual en el sistema
     * año: año actual
     * rut: rut del usuario buscado en la base de datos
     * encontrado = booleano que entrega resultado de si la fecha seleccionada cumple con los requisitos del registro.
     * nombres, apellidoP, apellidoM, sexo, ingreso, nomEmpresa = datos del cliente.
     * p 
     * nombresEmp2: almacena el nombre de la empresa antes  de ser almacenada
     * nombresEmp: almacena un arreglo con todos los nombres de las empresas registradas en el sistema.
     * */

    int id;
    string codigo = null;
    string fecha;
    string año;
    string rut;
    string codigoAux;
    bool encontrado = false;
    string nombres, apellidoP, apellidoM, sexo, ingreso, nomEmpresa;
    int P = 0;
    string[] nombresEmp;
    static string[] nombresEmp2;

    /**
     * Al iniciar la pagina, se cargaran las empresas en el arreglo para su próximo uso, la fecha y año actuales.
     * @Exception: error en la coneccion de la base de datos, 
     * @Exception: error obtener la fecha y hora del dia
     * */
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack) { BindCursos(); }


            año = string.Empty;
            fecha = string.Empty;
            DateTime fechaHoy = DateTime.Now.Date;
            fecha = fechaHoy.ToString("yyyy/MM/dd");
            string fecha2 = fechaHoy.ToString("dd/MM/yyyy");
            año = fechaHoy.ToString("yy");
            lblFecha.Text = fecha2;

            var adp = Namespace.Conexion.adapter("SELECT Crazon_social FROM cliente_empresa");
            using (adp)
            {
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
            catch (Exception ex)
            {
                Debug.Print("ocurrio error al guardar loss empresas en nombreEmpe : " + ex);
            }
        }
        catch (Exception ex)
        {
            Debug.Print("ocurrio un error al cargar la pagina : " + ex);
        }

    }

    /**
     *DropDownList
     * selecionado el estado del estudiante, entre aprobado y pendiente
     *  */
    protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlIngreso.SelectedIndex == 1) { txtNomEmpresa.Enabled = true; }
        if (ddlEstado.SelectedIndex == 1)
        {
            lblSigla.Visible = true; ddlSigla.Visible = true; lblErrorSigla.Visible = true; lblAsistencia.Visible = true;
            txtAsistencia.Visible = true; RangeValidator1.Visible = true; txtAsistencia.ReadOnly = false; ddlSigla.Enabled = true;
        }
        else
        {
            lblSigla.Visible = true; ddlSigla.Visible = true; ddlSigla.SelectedIndex = 0; lblErrorSigla.Visible = false; lblAsistencia.Visible = true;
            txtAsistencia.Visible = true; txtAsistencia.Text = string.Empty; RangeValidator1.Visible = false; lblNombreCurso.Visible = false; txtAsistencia.ReadOnly = true; ddlSigla.Enabled = false;
        }
    }
    /**
     * Boton
     * Actualiza el registro del cliente ingresado
     * @return mensajes pop alertando el estado de la actualización del registro
     * @exception error al momento de modificar los datos dentro de la base de datos
     *   */
    protected void btnModificar_Click(object sender, EventArgs e)
    {
        try
        {
            string curso = txtCurso.Text;


            if (curso == "Primavera P6 Básico")
            {
                ddlSigla.SelectedIndex = 2;
            }
            if (curso == "Primavera Risk")
            {
                ddlSigla.SelectedIndex = 3;
            }
            if (curso == "Primavera P6 Avanzado")
            {
                ddlSigla.SelectedIndex = 1;
            }
        }
        catch
        {
            Debug.Print("Error al leer la sigla");
        }

        string confirmValue = Request.Form["confirm_value"];
        //el nuevo registro es aprovado
        if (confirmValue == "Si")
        {
            txtRut.ReadOnly = true;
            gvLista.Visible = false;
            gvLista.DataSource = null;
            gvLista.DataBind();
            btnModificar.Visible = false;
            try
            {

                id = Convert.ToInt32(lblID.Text);
                int idO = id;
                Debug.Print("el id es : "+ idO);
                var Reader = Namespace.Conexion.reader("SELECT estado FROM curso_registros WHERE id=" + idO);
                string estadx = "";
                using (Reader)
                {
                    if (Reader.Read())
                    {
                        estadx = Reader["estado"].ToString();
                    }
                }
                Reader.Close();
                Namespace.Conexion.Desconectar();
                //el usuario anteriormente estaba aprovado
                //se guarda los registros de cursos antes de modificar
                if (estadx == "Aprobado")
                {

                    Reader = Namespace.Conexion.reader("SELECT rut,codigoA,id,estado FROM curso_registros WHERE id=" + idO);

                    string rutAux, codigoAux, idAux, mensaje;
                    rutAux = string.Empty;
                    codigoAux = string.Empty;
                    idAux = string.Empty;
                    mensaje = string.Empty;
                    using (Reader)
                    {
                        if (Reader.Read())
                        {
                            rutAux = Reader["rut"].ToString();
                            codigoAux = Reader["codigoA"].ToString();
                            idAux = Reader["id"].ToString();
                        }
                    }
                    Reader.Close();
                    Namespace.Conexion.Desconectar();
                    DateTime fechaH = DateTime.Now.Date;
                    mensaje = fechaH.ToString("yyyy/MM/dd");
                    lblError.Visible = true;
                    lblError.Text = rutAux + codigoAux + idAux;
                    string strSQL = "INSERT INTO curso_registros_old VALUES('" + rutAux + "','" + codigoAux + "'," + idAux + "," + 0 + ",'" + mensaje + "');";
                    Namespace.Conexion.insert(strSQL);

                }

                //se obtienen los datos del formulario
                rut = txtRut.Text;
                string curso = txtCurso.Text;
                string estado = ddlEstado.SelectedItem.ToString();
                string fechaCurso = fechas3();
                string sig = ddlSigla.SelectedValue;
                string ids = lblIdFecha.Text;
                int idCurso = Convert.ToInt32(lblCursoId.Text);
                string fechas = txtFecha.Text;
                int asistencia = 0;
                
                var adp = Namespace.Conexion.adapter("SELECT * FROM curso_registros WHERE rut='" + rut + "' AND id_curso=" + idCurso + " AND id_fechas='" + ids + "' AND id NOT IN('" + id + "') ");
                System.Data.DataTable dt = new System.Data.DataTable();
                using (adp)
                {

                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        string message = "alert('Este rut contiene un registro en ese curso y en las mismas fechas.')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                    }
                    
                    //un estado a sido seleccionado
                    if (ddlEstado.SelectedIndex != 0 && dt.Rows.Count <= 0)
                    {
                        // el usuario se selecciono como aprovado
                        if (ddlEstado.SelectedIndex == 1 && ddlSigla.SelectedIndex != 0)
                        {
                            for (int i = 0; i < 1;)
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
                            //valida si el usuario posee notas dentro de su registro, caso contrario habilita la opcion de ingresar notas
                            var validar = Namespace.Conexion.reader("SELECT * FROM curso_datos WHERE id_reg=" + idO);
                            if (validar.Read())
                            {
                                if (validar["nota"].ToString() != null)
                                {
                                    validar.Close();
                                    actualizarRegistro(rut, curso, fechaCurso, codigo, estado, id, fechas, ids, idCurso, asistencia);
                                    LimpiarControles();
                                    txtRut.ReadOnly = true; txtRut.Enabled = true;
                                    lblIdFecha.Text = string.Empty;
                                    string message = "alert('Registro Modificado')";
                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                    txtRut.Visible = true;
                                    btnListar.Visible = true;
                                    return;

                                }
                                else
                                {
                                    btnAgregar.Visible = false;
                                    lblErrorNota.Visible = true;
                                    lblErrorNota.Text = "El usuario no posee calificaciones,\n para continuar debe agregar alguna\n calificacion";
                                    lblNota.Visible = true;
                                    nota.Visible = true;
                                    lblporcentaje.Visible = true;
                                    validar.Close();
                                    return;
                                }
                               

                            }
                            else
                            {
                                btnAgregar.Visible = false;
                                lblErrorNota.Visible = true;
                                lblErrorNota.Text = "El usuario no posee calificaciones,\n para continuar debe agregar alguna\n calificacion";
                                lblNota.Visible = true;
                                nota.Visible = true;
                                lblporcentaje.Visible = true;
                                validar.Close();
                                return;
                            }
                        }
                        //el usuario se selecciono como pendiente
                        if (ddlEstado.SelectedIndex == 2 && dt.Rows.Count <= 0)
                        {

                            string code = lblCodigo.Text;
                            if (txtAsistencia.Text != string.Empty)
                            {
                                asistencia = Convert.ToInt32(txtAsistencia.Text);
                            }
                            eliminarCodigo(code);
                            actualizarRegistro(rut, curso, fechaCurso, null, estado, id, fechas, ids, idCurso, asistencia);
                            string message = "alert('Registro Modificado')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        }
                    }
                    else
                    {
                        string message = "alert('Debe seleccionar un estado')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        txtRut.Visible = true;
                        btnListar.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                string message = "alert('Error en modificación" + ex.Message + "')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                LimpiarControles();
                txtRut.ReadOnly = true;
                gvLista.Visible = false;
                gvLista.DataSource = null;
                gvLista.DataBind();
                btnModificar.Visible = false;
                txtRut.Visible = true;
                btnListar.Visible = true;
            }
        }
        else
        {
            string message = "alert('Se ha cancelado la modificación')";
            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

            LimpiarControles();
            txtRut.ReadOnly = true;
            gvLista.Visible = false;
            gvLista.DataSource = null;
            gvLista.DataBind();
            btnModificar.Visible = false;
            txtRut.Visible = true;
            btnListar.Visible = true;
        }
        LimpiarControles();
        txtRut.ReadOnly = true;
        gvLista.Visible = false;
        gvLista.DataSource = null;
        gvLista.DataBind();
        btnModificar.Visible = false;
        txtRut.Visible = true;
        btnListar.Visible = true;
    }

    /**
     *boton
     *Agrega el cliente a la base de datos
     * @return mensajes pop alertando el estado de la actualización del ingreso
     * @exception error en la base de datos o su coneccion
     * */
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
                vali = rutExistente(rut);
                string telefono, email;
                telefono = txtTelefono.Text;
                email = txtEmail.Text;
                if (vali != true)
                {
					if (validarRut(rut))
                    {
						insertarCliente(rut, nombres, apellidoP, apellidoM, sexo, ingreso, nomEmpresa, telefono, email);
						LimpiarControles();
						Namespace.Conexion.Desconectar();
						lblError.Visible = false;
						string message = "alert('Cliente Agregado')";
						ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
					}
					else
					{
						 string message = "alert('Error en el ingreso del cliente')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    lblError.Visible = true; lblError.Text = "Rut no valido";
					}
                }

                else
                {
                    string message = "alert('Error en el ingreso del cliente')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    lblError.Visible = true; lblError.Text = "Rut ya existe en la BD";
                }


            }
        }
        catch (Exception ex)
        {
            Debug.Print("error al ingresar el cliente : " + ex);
        }
    }

    /**
     * limpia todos los label, reinicia los calendario, dropDownList,etc
     * */
    public void LimpiarControles()
    {
        btnModificarConNota.Visible = false;
        btnAgregarConNota.Visible = false;
        nota.Visible = false;
        lblNota.Visible = false;
        lblporcentaje.Visible = false;
        lblErrorNota.Visible = false;
        Namespace.Conexion.Desconectar();
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
        gvLista.DataSource = null;
        gvLista.DataBind();
        lblCompletos.Visible = false;
        lblDerror.Visible = false;
        lblErrorSigla.Visible = false;
     
        ddlSigla.Enabled = false;
        ddlSigla.SelectedIndex = 0;
        lblNombreCurso.Visible = false;
        lblIdFecha.Text = string.Empty;
        lblAsistencia.Visible = true;
        txtAsistencia.Visible = true;
        txtAsistencia.Text = string.Empty;
        txtAsistencia.ReadOnly = true;
        ImageButton1.Enabled = false;
        ImageButton2.Enabled = false;
        lblRutEmpresa.Visible = false;
        lblRutEmpresa.Text = string.Empty;
    }

    /**
     * boton
     * Modifica los datos del cliente ingresado
     * @return mensajes pop alertando el estado de la actualización del cliente
     * @exception error en la base de datos o su coneccion
     * */
    protected void btnModificarCliente_Click(object sender, EventArgs e)
    {

    }

    /**
     *Crea un código aleatorio identificador, el cual se utiliza para identificar el registro interno del usuario y curso realizado
     * @return el codigo creado en string
     */
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
                return "" + CreateRandomCode();
            }
            temp = t;
            randomCode += allCharArray[t];
        }
        return randomCode;
    }

    /**
     *Busca la sigla del curso perteneciente al registro, para luego utilizarlo al momento de crear el código identificador
     * @param id del curso ingresado
     * @return la sigla en string
     * nota en caso que existan mas curso se deberan ingresar, en caso contrario al momento de crear los certificados el programa fallara
     */
    public string sigla(int index)
    {
        string value = null;
        if (index == 0)
        {
            value = "P6A";
        }
        if (index == 1)
        {
            value = "P6B";
        }
        if (index == 2)
        {
            value = "PRA";
        }
        return value;
    }

    /**
     *Ajusta las fechas para luego se utilizadas al momento de crear el diploma
     *  */

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

    /**
     *  boton
     *  Crea el diploma del curso y alumno seleccionado
     *  @return entrega un diploma en pdf para su descarga
     */
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
        fecom = me2.ToString() + " " + dia2.ToString() + " de " + ano2.ToString();

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

    /**
     * Recive los datos para ingresar el cliente
     * @param rut: rut del cliente ingresado
     * @param nombres: nombre del cliente
     * @param apellidoP: apellido paterno del usuario
     * @param apellidoM: apellido materno del usuario
     * @param sexo: sexo del usuario
     * @param ingreso; si el usuario esta pendiente o aprovado (por defeco esta pendiente)
     * @param nomEmpresa; nombre de la empresa a la que esta asociado el alumno
     * @param telefono: numero telefono del usuaruio
     * @param email: email del usuario; 
     */
    public void insertarCliente(string rut, string nombres, string apellidoP, string apellidoM, string sexo, string ingreso, string nomEmpresa, string telefono, string email)
    {
        string strSQL = "INSERT INTO curso_cliente VALUES('" + rut + "','" + nombres + "','" + apellidoP + "','" + apellidoM + "','" + sexo + "','" + ingreso + "','" + nomEmpresa + "','" + telefono + "','" + email + "');";
        Namespace.Conexion.insert(strSQL);
    }

    /**
     * Revisa si el rut existe en la base de datos
     * @param rut: rut del usuario, se utiliza para verificar su existencia
     * @return: retorna un boleano true si el rut se encontro en la base de datos
     * @Throws: si el rut no esta con el formato aceptado por la base de datos o la coneccion a esta offline
     */
    public bool rutExistente(string rut)
    {
        bool validador = false;
        try
        {
            var Reader = Namespace.Conexion.reader("SELECT * FROM curso_cliente WHERE rut='" + rut + "'");
            using (Reader)
            {
                if (Reader.Read())
                {
                    encontrado = true;

                }
                else { encontrado = false; }
            }
            Reader.Close();
            Namespace.Conexion.Desconectar();
            return encontrado;
        }
        catch (Exception ex) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Rut formato erroneo" + ex + "')", true); }
        return validador;
    }

    /**
     *  Revisa si el codigo verificador del registro se encuentra ocupado o existe, en caso de existir retorna true
     * @param codigo: recive el codigo a buscar en la base de datos
     * @return: retorna un boleano con el resultado de la busqueda
     * @Throws: excepsion puede ocurrir por formato no adecuado del codigo o la base de datos esta offline
     */
    public bool codigoExistente(string codigoA)
    {
        bool validador = false;
        try
        {
            var Reader = Namespace.Conexion.reader("SELECT * FROM curso_codigo WHERE codigoA='" + codigoA + "'");
            using (Reader)
            {
                if (Reader.Read())
                {
                    validador = true;
                }
                else { validador = false; }
            }
            Reader.Close();
            Namespace.Conexion.Desconectar();
            return encontrado;
        }
        catch (Exception ex) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('" + ex + "')", true); }
        return validador;
    }

    /**
     *DropDownList
     * Selecciona si el usuario es particular o bajo una empresa
     * */
    protected void ddlIngreso_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlIngreso.SelectedIndex == 1) { txtNomEmpresa.Visible = true; lblNomEmpresa.Visible = true; rfvEmpresa.Visible = true; }
        else { txtNomEmpresa.Visible = false; lblNomEmpresa.Visible = false; rfvEmpresa.Visible = false; }
        if (ddlIngreso.SelectedIndex == 1 && ddlSeleccion.SelectedIndex == 0) { txtNomEmpresa.Visible = true; lblNomEmpresa.Visible = true; rfvEmpresa.Visible = true; txtNomEmpresa.ReadOnly = false; }
        else { txtNomEmpresa.ReadOnly = true; }
    }

    /**
     * DropDownList
     * Menu principal de la pagina principa, controla la vistas de todos los label, text, dropd del formulario
     */
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ingreso de clientes
        if (ddlSeleccion.SelectedIndex == 0)
        {

            LimpiarControles();

            //vistas
            bModificar.Visible = false;
            aRegistro.Visible = false;
            btnCliente.Visible = true;
            btnListar.Visible = false;
            lblNota.Visible = false;
            nota.Visible = false;
            lblporcentaje.Visible = false;
            gvLista.Visible = false;
            rvRut.Visible = true;
            ImageButton1.Visible = true;
            txtListar.Visible = false;
            txtNomEmpresa.Visible = false;
            btnAgregar.Visible = false;
            lblError.Visible = false;
            btnModificarCli.Visible = false;
            btnGenerar.Visible = false;
            btnCertificado.Visible = false;
            ddlSigla.Visible = true;
            gvCurso.Visible = false;
            gvLista.Visible = false;
            calMulti.Visible = false;
            calMulti.Visible = false;
            lblCompletos.Visible = false;
            lblDerror.Visible = false;
            lblSigla.Visible = true;
            txtAsistencia.Visible = true;
            lblErrorSigla.Visible = false;
            lblNombreCurso.Visible = false;
            lblVeri1.Visible = false;
            lblAsistencia.Visible = true;
            lblRutEmpresa.Visible = false;
            lblTitulo.Visible = false;

            //segundo formulario
            formulario1.Visible = false;
            formulario2.Visible = false;
            formulario3.Visible = false;
            formulario4.Visible = false;
            formulario5.Visible = false;
            formulario6.Visible = false;
            formulario7.Visible = false;
            formulario8.Visible = false;
            formulario9.Visible = false;


            //text
            txtNombres.ReadOnly = false;
            txtApellidoM.ReadOnly = false;
            txtApellidoP.ReadOnly = false;
            txtRut.ReadOnly = false;
            txtTelefono.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtDias.ReadOnly = true;
            txtCurso.ReadOnly = true;
            txtFecha.ReadOnly = true;
            txtAsistencia.ReadOnly = true;


            //enabled
            ddlSexo.Enabled = true;
            ddlIngreso.Enabled = true;
            txtCurso.Enabled = true;
            ddlEstado.Enabled = false;
            txtFecha.Enabled = true;
            ImageButton2.Enabled = false;
            txtDias.Enabled = true;
            ImageButton1.Enabled = false;
            ddlSigla.Enabled = false;
            ddlSigla.Enabled = false;


            txtDias.Text = string.Empty;

        }

        //buscar cliente
        //desactivado
        if (ddlSeleccion.SelectedIndex == -1)
        {
            LimpiarControles();

            //visible
            bModificar.Visible = false;
            aRegistro.Visible = false;
            btnCliente.Visible = false;
            btnListar.Visible = false;
            lblNota.Visible = false;
            nota.Visible = false;
            lblporcentaje.Visible = false;
            gvLista.Visible = false;
            rvRut.Visible = false;
            ImageButton1.Visible = true;
            txtListar.Visible = false;
            rfvEmpresa.Visible = false;
            ddlSigla.Visible = true;
            gvCurso.Visible = false;


            calMulti.Visible = false;
            ddlSigla.Enabled = false;
            lblRutEmpresa.Visible = false;
            txtNomEmpresa.Visible = false;
            btnAgregar.Visible = false;
            lblError.Visible = false;
            btnModificarCli.Visible = false;
            btnGenerar.Visible = false;
            btnModificar.Visible = false;
            btnCertificado.Visible = false;
            txtAsistencia.Visible = true;

            calMulti.Visible = false;

            lblCompletos.Visible = false;
            lblDerror.Visible = false;
            lblSigla.Visible = true;
            ddlSigla.Visible = true;
            lblErrorSigla.Visible = false;
            lblNombreCurso.Visible = false;
            lblVeri1.Visible = true;
            lblAsistencia.Visible = true;

            //textos
            txtNombres.ReadOnly = true;
            txtApellidoM.ReadOnly = true;
            txtApellidoP.ReadOnly = true;
            txtTelefono.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtDias.ReadOnly = true;
            txtCurso.ReadOnly = true;
            txtFecha.ReadOnly = true;
            txtRut.ReadOnly = true;
            txtAsistencia.ReadOnly = true;

            //enabled
            ddlSexo.Enabled = false;
            ddlIngreso.Enabled = false;
            txtCurso.Enabled = true;
            ddlEstado.Enabled = false;
            txtFecha.Enabled = true;
            ImageButton2.Enabled = false;
            txtDias.Enabled = true;
            ImageButton1.Enabled = false;


            txtDias.Text = string.Empty;
        }
        //listar por curso (ahora buscar cliente)
        if (ddlSeleccion.SelectedIndex == 1)
        {

            LimpiarControles();

            //visible

            btnCliente.Visible = false;
            btnListar.Visible = true;
            lblNota.Visible = false;
            nota.Visible = false;
            lblporcentaje.Visible = false;
            gvLista.Visible = false;
            rvRut.Visible = false;

            txtListar.Visible = true;
            rfvEmpresa.Visible = false;
            ddlSigla.Visible = true;
            gvCurso.Visible = false;
            
            calMulti.Visible = false;
            btnAgregar.Visible = false;
            txtNomEmpresa.Visible = false;
            lblError.Visible = false;
            btnModificarCli.Visible = false;
            btnGenerar.Visible = false;
            btnModificar.Visible = false;
            btnCertificado.Visible = false;
            calMulti.Visible = false;

            lblCompletos.Visible = false;
            lblDerror.Visible = false;
            lblSigla.Visible = true;
            ddlSigla.Visible = true;
            lblErrorSigla.Visible = false;
            lblNombreCurso.Visible = false;
            lblVeri1.Visible = true;
            lblAsistencia.Visible = true;
            txtAsistencia.Visible = true;

            lblRutEmpresa.Visible = false;
            txtCurso.Visible = true;



            //segundo formulario
            formulario1.Visible = false;
            formulario2.Visible = false;
            formulario3.Visible = true;
            formulario4.Visible = true;
            formulario5.Visible = false;
            formulario6.Visible = false;
            formulario7.Visible = false;
            formulario8.Visible = false;
            formulario9.Visible = false;


            ImageButton1.Visible = false;

            //text
            txtNombres.ReadOnly = true;
            txtApellidoM.ReadOnly = true;
            txtApellidoP.ReadOnly = true;
            txtRut.ReadOnly = true;
            txtTelefono.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtDias.ReadOnly = true;
            txtCurso.ReadOnly = true;
            txtFecha.ReadOnly = true;
            txtAsistencia.ReadOnly = true;

            //enabled
            ddlSexo.Enabled = false;
            ddlIngreso.Enabled = false;
            txtCurso.Enabled = true;
            ddlEstado.Enabled = false;
            txtFecha.Enabled = true;
            ImageButton2.Enabled = false;
            txtDias.Enabled = true;
            ImageButton1.Enabled = false;
            ddlSigla.Enabled = false;


            txtDias.Text = string.Empty;
        }

        //agregar registro
        if (ddlSeleccion.SelectedIndex == 2)
        {
            LimpiarControles();



            //visibles
            bModificar.Visible = false;
            aRegistro.Visible = false;
            txtNomEmpresa.Visible = false;
            lblNomEmpresa.Visible = false;
            btnCliente.Visible = false;
            btnListar.Visible = true;
            lblNota.Visible = false;
            nota.Visible = false;
            lblporcentaje.Visible = false;
            gvLista.Visible = false;
            rvRut.Visible = false;
            ImageButton1.Visible = true;

            txtListar.Visible = true;
            lblError.Visible = false;
            ddlSigla.Visible = true;
            gvCurso.Visible = false;
            calMulti.Visible = false;
            txtAsistencia.Visible = true;
            lblSigla.Visible = true;
            ddlSigla.Visible = true;
            btnAgregar.Visible = false;

            rfvEmpresa.Visible = false;
            btnModificarCli.Visible = false;
            lblErrorSigla.Visible = false;
            lblNombreCurso.Visible = false;
            lblVeri1.Visible = false;
            lblAsistencia.Visible = true;
            calMulti.Visible = false;

            lblCompletos.Visible = false;
            lblDerror.Visible = false;
            btnModificarCli.Visible = false;
            lblRutEmpresa.Visible = false;
            btnGenerar.Visible = false;
            btnModificar.Visible = false;
            btnCertificado.Visible = false;

            //segundo formulario
            formulario1.Visible = true;
            formulario2.Visible = true;
            formulario3.Visible = true;
            formulario4.Visible = true;
            formulario5.Visible = false;
            formulario6.Visible = true;
            formulario7.Visible = true;
            formulario8.Visible = true;
            formulario9.Visible = true;


            //text
            txtNombres.ReadOnly = true;
            txtApellidoM.ReadOnly = true;
            txtApellidoP.ReadOnly = true;
            txtNomEmpresa.ReadOnly = true;

            //enabled
            ddlSexo.Enabled = false;
            ddlIngreso.Enabled = false;
            txtCurso.Enabled = true;
            ddlEstado.Enabled = false;
            txtFecha.Enabled = true;
            txtDias.Enabled = true;
            ImageButton1.Enabled = false;
            ImageButton2.Enabled = false;
            ddlSigla.Enabled = false;

            //randoly
            txtRut.ReadOnly = true;
            txtDias.ReadOnly = true;
            txtCurso.ReadOnly = true;
            txtFecha.ReadOnly = true;
            txtTelefono.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtAsistencia.ReadOnly = true;

            txtDias.Text = string.Empty;

            calMulti.AllowDeselect = true;
            calMulti.AllowSelectRegular = true;
        }
        //modificar un cliente
        if (ddlSeleccion.SelectedIndex == 3)
        {
            try
            {
                LimpiarControles();

                //visibles
                bModificar.Visible = false;
                aRegistro.Visible = false;
                btnCliente.Visible = false;
                btnListar.Visible = true;
                lblNota.Visible = false;
                nota.Visible = false;
                lblporcentaje.Visible = false;
                rvRut.Visible = true;
                ImageButton1.Visible = true;
                txtListar.Visible = true;
                rfvEmpresa.Visible = false;
                txtNomEmpresa.Visible = false;
                btnAgregar.Visible = false;
                lblError.Visible = false;
                btnModificarCli.Visible = true;
                btnGenerar.Visible = false;
                btnModificar.Visible = false;
                btnCertificado.Visible = false;
                ddlSigla.Visible = true;
                gvCurso.Visible = false;
                gvLista.Visible = false;
                calMulti.Visible = false;
                lblSigla.Visible = true;
                ddlSigla.Visible = true;
                lblErrorSigla.Visible = false;
                lblNombreCurso.Visible = false;
                lblVeri1.Visible = false;
                lblAsistencia.Visible = true;
                txtAsistencia.Visible = true;
                calMulti.Visible = false;
                lblCompletos.Visible = false;
                lblDerror.Visible = false;
                lblRutEmpresa.Visible = false;


                //randoly
                txtRut.ReadOnly = false;
                txtNombres.ReadOnly = true;
                txtApellidoM.ReadOnly = true;
                txtApellidoP.ReadOnly = true;
                txtNomEmpresa.ReadOnly = false;
                txtTelefono.ReadOnly = true;
                txtEmail.ReadOnly = true;
                txtDias.ReadOnly = true;
                txtCurso.ReadOnly = true;
                txtFecha.ReadOnly = true;
                txtAsistencia.ReadOnly = true;


                //enabled
                ddlSexo.Enabled = false;
                ddlIngreso.Enabled = false;
                txtCurso.Enabled = true;
                ddlEstado.Enabled = false;
                txtFecha.Enabled = true;
                ImageButton2.Enabled = false;
                txtDias.Enabled = true;
                ImageButton1.Enabled = false;
                ddlSigla.Enabled = false;


                txtDias.Text = string.Empty;


                //segundo formulario
                formulario1.Visible = false;
                formulario2.Visible = false;
                formulario3.Visible = false;
                formulario4.Visible = false;
                formulario5.Visible = false;
                formulario6.Visible = false;
                formulario7.Visible = false;
                formulario8.Visible = false;
                formulario9.Visible = false;

            }
            catch (Exception ex) { }
        }
        //modificar registro
        if (ddlSeleccion.SelectedIndex == 4)
        {

            LimpiarControles();

            //visible
            bModificar.Visible = false;
            aRegistro.Visible = false;
            btnCliente.Visible = false;
            btnListar.Visible = true;
            lblNota.Visible = false;
            nota.Visible = false;
            lblporcentaje.Visible = false;
            gvLista.Visible = false;
            rvRut.Visible = false;
            ImageButton1.Visible = true;
            txtListar.Visible = true;
            rfvEmpresa.Visible = false;
            txtNomEmpresa.Visible = false;
            btnAgregar.Visible = false;
            lblError.Visible = false;
            btnModificarCli.Visible = false;
            btnGenerar.Visible = false;
            btnModificar.Visible = false;
            btnCertificado.Visible = false;
            ddlSigla.Visible = true;
            gvCurso.Visible = false;
            calMulti.Visible = false;
            lblCompletos.Visible = false;
            lblDerror.Visible = false;
            lblSigla.Visible = true;
            ddlSigla.Visible = true;
            lblErrorSigla.Visible = false;
            lblNombreCurso.Visible = false;
            lblVeri1.Visible = true;
            lblAsistencia.Visible = true;
            txtAsistencia.Visible = true;
            calMulti.Visible = false;
            lblRutEmpresa.Visible = false;

            //randoly
            txtNombres.ReadOnly = true;
            txtApellidoM.ReadOnly = true;
            txtApellidoP.ReadOnly = true;
            txtRut.ReadOnly = true;
            txtTelefono.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtDias.ReadOnly = true;
            txtCurso.ReadOnly = true;
            txtFecha.ReadOnly = true;
            txtAsistencia.ReadOnly = true;

            //enabled
            ddlSexo.Enabled = false;
            ddlIngreso.Enabled = false;
            txtCurso.Enabled = true;
            ddlEstado.Enabled = false;
            txtFecha.Enabled = true;
            ImageButton2.Enabled = false;
            txtDias.Enabled = true;
            ImageButton1.Enabled = false;
            ddlSigla.Enabled = false;

            //segundo formulario
            formulario1.Visible = true;
            formulario2.Visible = true;
            formulario3.Visible = true;
            formulario4.Visible = true;
            formulario5.Visible = false;
            formulario6.Visible = true;
            formulario7.Visible = true;
            formulario8.Visible = true;
            formulario9.Visible = true;


            txtDias.Text = string.Empty;

        }
        //if (ddlSeleccion.SelectedIndex == 5) { Response.Redirect("Reportes.aspx"); lblVeri1.Visible = false; }
    }

    /**
     *Label OnTextChanged
     * valida y reconstruche el rut al momento de ser ingresado en la casilla rut
     */
    protected void txtRut_TextChanged(object sender, EventArgs e)
    {

        lblError.Visible = false;
        string texto = txtRut.Text;
        txtRut.Text = Rut(texto);
        if (ddlSeleccion.SelectedIndex == 2 && txtRut.Text != string.Empty)
        {

            string var = txtRut.Text;
            lblError.Visible = false;
            //  buscarCliente(var);

        }
        if (ddlSeleccion.SelectedIndex == 3 && txtRut.Text != string.Empty)
        {

            string var = txtRut.Text;
            lblError.Visible = false;
            //  buscarCliente(var);
            txtNomEmpresa.ReadOnly = false;

        }


    }

    /**
     * Valida el rut ingresado al label
     * @param: texto: Es el rut ingresado para vereficarlo si es real o falso
     * @return retorna el rut con el formato a utilizar en la pagina, como tambien si el rut no es valido retorna el rut original y un mensaje de alerta
     * @throws en caso que la conversion este fura del rango de trabajo, retorna el rut original.
     * */
    public string Rut(string texto)
    {
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
                if (!validarRut(texto))
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

    /**
     * AutoPostBack
     * @Param rut: recive el rut ingresado y le cambio al formato aceptado por el servidor, en caso contrario lanza los text error
     * @return: mensaje de error "rut no valido"
     * */
    protected void txtListar_TextChanged(object sender, EventArgs e)
    {
        string texto = txtListar.Text;
        txtListar.Text = Rut(texto);
    }

    /**
     * al momento de realzar alguna modificacion en el registro del usuario, o en el registro de sus cursos
     * guarda una copia provisoria de los datos antiguos antes de modificar
     *@param: recive el id del registro a guardar
     * @Throws: en caso que la base de datos este offline
     * */
    public void regOld(int idO)
    {
        try
        {
            var Reader = Namespace.Conexion.reader("SELECT rut,codigoA,id FROM curso_registros WHERE id=" + idO);
            using (Reader)
            {
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
            Reader.Close();
            Namespace.Conexion.Desconectar();
        }
        catch (Exception ex) { }
    }

    /**
     *boton
     * obtiene los datos del rut ingresado y los introduce en los label selecionados
     * param: rut: rut ingresado en el box busqueda  
     */
    protected void btnListar_Click(object sender, EventArgs e)
    {
        try
        {
            LimpiarControles();
            if (ddlSeleccion.SelectedIndex == 1)
            {
                aRegistro.Visible = true;
                bModificar.Visible = true;
				ddlEstado.Enabled = false;
            }
            string vari = txtListar.Text;
            buscarCliente(vari);
            var adp = Namespace.Conexion.adapter("SELECT curso AS 'Curso',DATE_FORMAT(fechaCurso,'%d/%m/%Y') AS 'Fecha de Inicio',codigoA AS 'Codigo',estado AS 'Estado',id AS'N Registro',fechas AS 'Fechas',id_curso AS 'ID Curso',asistencia AS 'Asistencia' FROM curso_registros WHERE rut='" + vari + "'");
            using (adp)
            {

                System.Data.DataTable dt = new System.Data.DataTable();
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (ddlSeleccion.SelectedIndex == 1 || ddlSeleccion.SelectedIndex == 4)
                    {
                        gvLista.Visible = true;
                        lblError.Visible = false;
                        gvLista.DataSource = null;
                        gvLista.DataBind();
                        gvLista.DataSource = dt;
                        gvLista.DataBind();
                    }
                    ddlSigla.Enabled = false;
                }
                else { lblError.Text = "No hay registros con este rut"; lblError.Visible = true; gvLista.Visible = false; }
            }
        }
        catch (Exception ex)
        {
            Debug.Print("error al buscar un cliente " + ex);
        }
    }

    /**
     * obtiene los cursos del rut ingresado
     * @param: rut: se utiliza para buscar los cursos almacenados con dicho rut
     * @return: retorna un grid con los cursos registrados por el rut
     * */
    public void listarCursos(string rut)
    {
        try
        {

            var adp = Namespace.Conexion.adapter("SELECT curso AS 'Curso',fechaCurso AS 'Fecha de Inicio',codigoA AS 'Codigo',estado AS 'Estado',id AS'N Registro',fechas AS 'Fechas',id_curso AS 'ID Curso',asistencia AS 'Asistencia' FROM curso_registros WHERE rut='" + rut + "'");
            using (adp)
            {

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
                    ddlSigla.Enabled = false;
                }
                else { lblError.Text = "No hay registros con este rut"; lblError.Visible = true; gvLista.Visible = false; }
            }
        }
        catch (Exception ex) { }
    }

    /**
     * buscar un cliente con el rut ingresado, retornando los datos de estes a los label correspondientes
     * @param: rut: rut del cliente a buscar
     * @return: los datos del usuario a buscar, en los label de informacion.
     * */
    public void buscarCliente(string rut)
    {
        try
        {

            var Reader = Namespace.Conexion.reader("SELECT * FROM curso_cliente WHERE rut='" + rut + "'");
            using (Reader)
            {
                if (Reader.Read())
                {
                    if (ddlSeleccion.SelectedIndex == 2)
                    {
                        txtDias.ReadOnly = false; txtDias.Enabled = true; txtDias.Text = string.Empty; txtDias.Enabled = true;
                        ImageButton1.Enabled = true; txtCurso.Enabled = true; ddlEstado.Enabled = true; txtFecha.Enabled = true;
                        txtRut.ReadOnly = true;
                        ImageButton2.Enabled = true;
                    }

                    if (ddlSeleccion.SelectedIndex == 3)
                    {
                        txtDias.Enabled = false; txtDias.ReadOnly = true; txtDias.Text = string.Empty; txtDias.Enabled = false;
                        ImageButton1.Enabled = false; txtRut.ReadOnly = true; txtNombres.ReadOnly = false; txtApellidoP.ReadOnly = false;
                        txtApellidoM.ReadOnly = false; ddlSexo.Enabled = true; ddlIngreso.Enabled = true; txtTelefono.ReadOnly = false;
                        txtEmail.ReadOnly = false; txtNomEmpresa.ReadOnly = false;
                    }
                    encontrado = true;
                    txtRut.Text = Reader["rut"].ToString();
                    txtNombres.Text = Reader["nombres"].ToString();
                    txtApellidoP.Text = Reader["apellidoP"].ToString();
                    txtApellidoM.Text = Reader["apellidoM"].ToString();
                    ddlSexo.SelectedValue = Reader["sexo"].ToString();
                    ddlIngreso.SelectedValue = Reader["ingreso"].ToString();
                    txtTelefono.Text = Reader["telefono"].ToString();
                    txtEmail.Text = Reader["mail"].ToString();
                    if (ddlSeleccion.SelectedIndex == 2)
                    {
                        ddlSigla.Enabled = true; txtAsistencia.ReadOnly = false;
                    }
                    if (ddlIngreso.SelectedIndex == 1)
                    {
                        lblNomEmpresa.Visible = true;
                        txtNomEmpresa.Visible = true;
                        txtNomEmpresa.ReadOnly = true;
                        txtNomEmpresa.Text = Reader["nomEmpresa"].ToString();
                    }
                    else
                    {
                        lblNomEmpresa.Visible = false;
                        txtNomEmpresa.Visible = false;
                        txtNomEmpresa.Text = Reader["nomEmpresa"].ToString();
                    }
                }
                else { lblError.Visible = true; lblError.Text = "Rut no existe en la BD"; LimpiarControles(); }
            }
            Reader.Close();
            Namespace.Conexion.Desconectar();
        }
        catch (Exception ex) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Rut formato erroneo" + ex + "')", true); }
    }

   //No se utiliza
    protected void ddlCurso_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    /**
     *buscar los cursos creados en la base de datos
     * @return retorna los cursos en un grid para luego seleccionar el curso a registrar
     * @throws en caso que la base de datos este offline
     *  */
    private void BindCursos()
    {
        try
        {
            var adp = Namespace.Conexion.adapter("SELECT curso FROM curso_creado");
            using (adp)
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                adp.Fill(dt);
            }
        }
        catch (Exception ex) { lblError.Text = ex.Message; lblError.Visible = true; }
    }

    /**
     * inserta el registro de un curso realizado o por realizar por el estudiante
     * @param strSQL: recive la query con los datos a almacenar en la base de datos
     * @throws en caso que la base de datos este offline
     * */
    public void insertarRegistro(string strSQL)
    {
        try
        {
            Namespace.Conexion.insert(strSQL);
            Namespace.Conexion.Desconectar();
        }catch(Exception ex)
        {
            Debug.Print("error al ingresar el registro : " + ex);
        }
    }

    /**
     * Inserta el codigo de registro al rut ingresado
     * @param: codigo: codigo de registro generado, el cual despues se utiliza para la generacion de certificados
     * @param: rut: rut al cual se le ingresara el codigo de registro
     * @throws: en caso que la base de datos este offline
     * */
    public void insertarCodigo(string codigo, string rut)
    {
        try
        {
            string strSQL = "INSERT INTO curso_codigo VALUES('" + codigo + "','" + rut + "'," + 0 + ");";
            Namespace.Conexion.insert(strSQL);
            Namespace.Conexion.Desconectar();
        }catch(Exception ex)
        {
            Debug.Print("error al ingresar codigo de registro ; " + ex);
        }
    }

    //no se usa
    protected void txtFecha_TextChanged(object sender, EventArgs e)
    {

    }
    
    /**
     *button  
     * devuelve un grid con los cursos inscritos en la base de datos, se utiliza para seleccionar el curso a inscribir
     * @return grid con los cursos
     * @throws en caso que la base de datos este offline
     */
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (gvCurso.Visible == false)
            {
                var adp = Namespace.Conexion.adapter("SELECT curso AS 'Curso', direccion AS 'Direccíon' ,id_curso AS 'ID Curso' FROM curso_creado");
                using (adp)
                {

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
        catch (Exception ex)
        {
            Debug.Print("mo se pudieron obtene los cursos : "+ex);
        }
    }
  
    //no se usa
    protected void txtFecha_TextChanged1(object sender, EventArgs e)
    {
    }

    /**
     * agrega un registro de un alumno a un curso especificado y fecha. En caso que alumno figure como aprobado, inmediatamente el sistema obligará 
     * al usuario a ingresar una nota y asistencia para finalizar la aprobación del alumno
     * @return retorna mensajes pop, con el estado del registro
     * @throw en caso que ocurra algun error al momento de ingresar los datos o en caso que la base de datos este desconectada
     * */
    protected void btnAgrega_Click(object sender, EventArgs e)
    {
        try
        {
            Debug.Print("entre");
            rut = txtRut.Text;
            string curso = txtCurso.Text;
            string estado = ddlEstado.SelectedItem.ToString();
            string fechaCurso = fechas3();
            string sig = ddlSigla.SelectedValue;
            string ids = lblIdFecha.Text;
            int idCurso = Convert.ToInt32(lblCursoId.Text);
            string fechas = txtFecha.Text;
            int asistencia = 0;

            if(validarFechaIndividual(rut, idCurso))
            {
                string message = "alert('Este rut contiene un registro en ese curso y en las mismas fechas.')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }

            var adp = Namespace.Conexion.adapter("SELECT * FROM curso_registros WHERE rut='" + rut + "' AND id_curso=" + idCurso + " AND id_fechas='" + ids + "'");
            System.Data.DataTable dt = new System.Data.DataTable();
            using (adp)
            {
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string message = "alert('Este rut contiene un registro en ese curso y en las mismas fechas.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                }
            }
            Namespace.Conexion.Desconectar();
            
            if (ddlEstado.SelectedIndex != 0 && dt.Rows.Count <= 0)
            {
                //sigla aprovado
                if (ddlEstado.SelectedIndex == 1 && ddlSigla.SelectedIndex != 0 && dt.Rows.Count <= 0)
                {
                    for (int i = 0; i < 1;)
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

                    string strSQL = "INSERT INTO curso_registros VALUES('" + rut + "','" + curso + "','" + fechaCurso + "','" + fecha + "','" + codigo + "','" + estado + "'," + 0 + ",'" + fechas + "','" + ids + "'," + idCurso + "," + asistencia + ");";
                    btnAgregar.Visible = false;
                    lblErrorNota.Visible = true;
                    lblErrorNota.Text = "El usuario no posee calificaciones,\n para continuar debe agregar alguna\n calificacion";
                    lblNota.Visible = true;
                    nota.Visible = true;
                    lblporcentaje.Visible = true;
                    return;
                }
                //registro pendiente
                if (ddlEstado.SelectedIndex == 2 && dt.Rows.Count <= 0)
                {
                    asistencia = 0;
                    string strSQL = "INSERT INTO curso_registros VALUES('" + rut + "','" + curso + "','" + fechaCurso + "','" + fecha + "','" + null + "','" + estado + "'," + 0 + ",'" + fechas + "','" + ids + "'," + idCurso + "," + asistencia + ");";
                    insertarRegistro(strSQL);
                    string message = "alert('Registro Agregado')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                }
                //registro rapido
                if (ddlSeleccion.SelectedIndex == 1)
                {
                    buscarCliente(rut);
                    formulario1.Visible = false;
                    formulario2.Visible = false;
                    formulario3.Visible = true;
                    formulario4.Visible = true;
                    formulario5.Visible = false;
                    formulario6.Visible = false;
                    formulario7.Visible = false;
                    formulario8.Visible = false;
                    formulario9.Visible = false;
                    calMulti.Visible = false;
                    ddlEstado.Enabled = false;
                    ddlEstado.SelectedIndex = 0;
                    txtCurso.Text = string.Empty;
                    ImageButton1.Visible = false;
                    lblCompletos.Visible = false;
                    listarCursos(rut);
                }
                //registro normal
                else
                {
                    LimpiarControles();
                    txtRut.ReadOnly = true;
                    txtRut.Enabled = true;
                    lblIdFecha.Text = string.Empty;
                    gvLista.Visible = false;
                }

            }
            else
            {
                string message = "alert('Debe seleccionar un estado')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                gvLista.Visible = false;
            }


        }
        catch (Exception ex)
        {
            string message = "alert('Error en ingreso de registro" + ex.Message + "')";
            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            gvLista.Visible = false;
        }

    }

    //no se usa
    protected void txtNomEmpresa_TextChanged(object sender, EventArgs e)
    {

    }

    //no se usa
    protected void gvLista_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


    /**
     * maneja las acciones del grid lista, el cual retorna los cursos registrados por el alumno. Al seleccionar un curso y este esta aprovado 
     * activa los botones para generar un certificado y diploma
     * Nota: si el alumno no posee notas por algun error o registro antiguo el sistema no permitira generar un certificado
     * throw: en  caso de un error al buscar los id del curso del alumno
     */
    protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select" && ddlSeleccion.SelectedIndex == 4 || e.CommandName == "Select" && ddlSeleccion.SelectedIndex == 1)
        {
			
            if (ddlSeleccion.SelectedIndex == 4 || ddlSeleccion.SelectedIndex == 1)
            {
                try
                {
                    txtAsistencia.ReadOnly = false;
                    lblAsistencia.Visible = true;
                    txtAsistencia.Visible = true;
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
                    string palabra = txtFecha.Text;
                    fechasInversas(palabra);
                    string va = gvLista.Rows[num].Cells[4].Text;
                    ddlEstado.SelectedValue = gvLista.Rows[num].Cells[4].Text;
                    lblSigla.Visible = true; ddlSigla.Visible = true; lblErrorSigla.Visible = false;

                    if (ddlSeleccion.SelectedIndex == 4)
                    {
                        lblID.Text = gvLista.Rows[num].Cells[5].Text; btnModificar.Visible = true;
                        lblCodigo.Text = gvLista.Rows[num].Cells[3].Text;
                    }
                    if (va == "Aprobado")
                    {
                        lblAsistencia.Visible = true; txtAsistencia.Visible = true; RangeValidator1.Visible = true;
                        string cof = lblCodigo.Text; txtAsistencia.ReadOnly = false; ddlSigla.Enabled = true;
                        string cof2 = cof.Substring(5, 3);
                        if (cof2 == "P6A") { ddlSigla.SelectedIndex = 1; }
                        if (cof2 == "P6B") { ddlSigla.SelectedIndex = 2; }
                        if (cof2 == "PRA") { ddlSigla.SelectedIndex = 3; }
                        if (ddlSigla.SelectedIndex == 1) { lblNombreCurso.Text = "Primavera P6 Avanzado"; lblNombreCurso.Visible = true; }
                        if (ddlSigla.SelectedIndex == 2) { lblNombreCurso.Text = "Primavera P6 Básico"; lblNombreCurso.Visible = true; }
                        if (ddlSigla.SelectedIndex == 3) { lblNombreCurso.Text = "Primavera Risk Análisis"; lblNombreCurso.Visible = true; }

                    }
                    else
                    {
                        txtAsistencia.ReadOnly = true; ddlSigla.Enabled = false; lblNombreCurso.Visible = false; lblAsistencia.Visible = true; txtAsistencia.Visible = true;
                        RangeValidator1.Visible = false; lblErrorSigla.Visible = false;
                    }
                    calMulti.AllowDeselect = true;
                    calMulti.AllowSelectRegular = false;
                    txtCurso.Enabled = true;
                    ddlEstado.Enabled = true;
                    txtFecha.Enabled = true;
                    txtCurso.Text = HttpUtility.HtmlDecode(t2);
                    gvLista.Visible = true;
                    gvLista.DataSource = null;
                    txtDias.Enabled = true;
                    ImageButton1.Enabled = true;
                    txtDias.ReadOnly = false;
                    gvLista.DataBind();
                    ImageButton2.Enabled = true;
                    lblIdFecha.Text = "";
                    int numId = Convert.ToInt32(lblID.Text);
                    var Reader = Namespace.Conexion.reader("SELECT id_fechas FROM curso_registros WHERE id=" + numId);
                    using (Reader)
                    {
                        if (Reader.Read())
                        {
                            lblIdFecha.Text = Reader["id_fechas"].ToString();
                        }
                    }
                    Reader.Close();
                    Namespace.Conexion.Desconectar();
                }


                catch (Exception ex) { }
            }
            if (ddlSeleccion.SelectedIndex == 1)
            {
                try
                {
                    txtAsistencia.ReadOnly = true;
                    BindCursos();
                    Int16 num = Convert.ToInt16(e.CommandArgument);
                    string t2, t6;
                    lblAsistencia.Visible = true;
                    txtAsistencia.Visible = true;
                    t2 = gvLista.Rows[num].Cells[1].Text;
                    txtFecha.Text = gvLista.Rows[num].Cells[6].Text;
                    ddlEstado.SelectedValue = gvLista.Rows[num].Cells[4].Text;
                    lblCodigo.Text = gvLista.Rows[num].Cells[3].Text;
                    lblID.Text = gvLista.Rows[num].Cells[5].Text;
                    string vari = gvLista.Rows[num].Cells[6].Text;
                    string va = gvLista.Rows[num].Cells[4].Text;
                    if (va == "Aprobado") { lblAsistencia.Visible = true; txtAsistencia.Visible = true; txtAsistencia.Text = gvLista.Rows[num].Cells[8].Text; }
                    else { lblAsistencia.Visible = true; txtAsistencia.Visible = true; }
                    calMulti.SelectedDates = null;
                    calMulti.DatePickerImagePath = null;
                    this.calMulti.SelectedDate = new DateTime();
                    calMulti.SelectedDates.Clear();
                    calMulti.Visible = false;
                    fechasInversas3(vari);
                    string palabra = txtFecha.Text;
                    fechasInversas(palabra);
                    if (ddlEstado.SelectedIndex == 1) { btnGenerar.Visible = true; btnCertificado.Visible = true; }
                    else { btnGenerar.Visible = false; btnCertificado.Visible = false; }
                    ImageButton2.Enabled = true;
                    calMulti.AllowDeselect = false;
                    calMulti.AllowSelectRegular = false;
                    txtCurso.Enabled = false;
                    ddlEstado.Enabled = false;
                    txtFecha.Enabled = true;
                    txtFecha.ReadOnly = true;
                    txtCurso.Text = HttpUtility.HtmlDecode(t2);
                    gvLista.Visible = true;
                    gvLista.DataSource = null;
                    gvLista.DataBind();
                    txtDias.Enabled = true;
                    ImageButton1.Enabled = false;
                    txtDias.ReadOnly = true;
                    ddlSigla.Enabled = false;
                }
                catch (Exception ex)
                {
                    Debug.Print("ocurrio un error al seleccionar el curso : " + ex);
                }
            }
        }
		if (ddlSeleccion.SelectedIndex == 1)
		{
			ddlEstado.Enabled = false;
		}
    }

    //no se usa, su proposito era validad los numeros de telefono
    protected void txtTelefono_TextChanged(object sender, EventArgs e)
    {

    }

    /**
     * actualiza los datos del cliente
     * @return retorna mensajes pop con el estado de la actualizacion
     * @throw en caso que falle la coneccion de la base de datos
     * */
    protected void btnModificarCli_Click(object sender, EventArgs e)
    {
        try
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
            string telefono, email;
            telefono = txtTelefono.Text;
            email = txtEmail.Text;
            actualizarCliente(rut, nombres, apellidoP, apellidoM, sexo, ingreso, nomEmpresa, telefono, email);
            string message = "alert('Cliente Modificado')";
            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            LimpiarControles();
            Namespace.Conexion.Desconectar();
            lblError.Visible = false;
            try
            {
                btnCliente.Visible = false; btnListar.Visible = true;
                gvLista.Visible = false; rvRut.Visible = true;
                txtListar.Visible = true; LimpiarControles(); rfvEmpresa.Visible = false; txtRut.ReadOnly = true;
                txtNombres.ReadOnly = true; txtApellidoM.ReadOnly = true; txtApellidoP.ReadOnly = true; ddlSexo.Enabled = false;
                ddlIngreso.Enabled = false; txtNomEmpresa.Visible = false; txtCurso.Enabled = false; ddlEstado.Enabled = false; txtFecha.Enabled = false; btnAgregar.Visible = false;
                lblError.Visible = false;
                txtNomEmpresa.ReadOnly = false;
                ImageButton2.Enabled = false;
                btnModificarCli.Visible = true;
                if (ddlSeleccion.SelectedIndex == 3 && txtRut.Text == string.Empty)
                {

                }
                txtTelefono.ReadOnly = true;
                txtEmail.ReadOnly = true;

            }
            catch (Exception ex)
            {
                string message2 = "alert('Error en modificación" + ex.Message + "')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message2, true);
            }

        }
        catch (Exception ex) { }
    }

    /**
     * genera el script y lo envia para actualizar los datos del usuario
     * @param rut: el rut de usuario se utiliza para buscarlo en la base de datos
     * @param nombres: es el nombre del usuario a modificar
     * @param apellidoP: es el apellido paterno del usuario a modificar
     * @param apellidoM: es el apellido materno del usuario a modificar
     * @param seox: es el sexo a modificar
     * @param: ingreso: modificar si el usuario es particualr o de una empresa
     * @param nomEmpresa: en caso que pertenesca a una empresa recive el nombre de la empresa, en caso contrario envia null
     * @param telefono: el telefono del usuario a modificar
     * @param email: el mail del usuario a modificar
     * @throw en caso que la base de datos este desconectada
     * */
    public void actualizarCliente(string rut, string nombres, string apellidoP, string apellidoM, string sexo, string ingreso, string nomEmpresa, string telefono, string email)
    {
        try
        {
            string strSQL = "UPDATE curso_cliente set nombres='" + nombres + "',apellidoP='" + apellidoP + "',apellidoM='" + apellidoM + "',sexo='" + sexo + "',ingreso='" + ingreso + "',nomEmpresa='" + nomEmpresa + "',telefono='" + telefono + "',mail='" + email + "' WHERE rut='" + rut + "';";
            Namespace.Conexion.update(strSQL);
            Namespace.Conexion.Desconectar();
        }
        catch(Exception ex)
        {
            Debug.Print("error al actualizar el usuario : " + ex);
        }
    }

    /**
     * genera el script y lo envia para aactualizar el registro del usuario
     * @param rut: el rut del usuario a modificar, se utiliza para verificar si el registro corresponde al usuario
     * @param curso: ingresa si el curso sera modificado, en caso que sea modificado, el metodo que llame a esta funcion obligara al usuario a cambiar las fechas
     * @param fechaCurso: es el primer dia ingresado en el calendario, corresponde al primer dia y inicio del curso.
     * @param codigo: corresponde al codigo verificador generado por el sistema, este se utiliza cuando el usuario esta aprovado y se necesite generar un certificado.
     * En caso que el alumno sea traspasado a pendiente, se le eliminara el codigo o al revez en caso que el alumno sea aprobado se le generara el codigo
     * @param: estado: si el alumno esta en estado pendiente o aprobado, se utiliza para verificiar si alumno termino con exito o no su curso
     * @param id: id del registro generado, se utiliza para buscar el registro a modificar
     * @param fechas: es un string con todas las fechas ingresadas del curso, separadas con ;
     * @param id_fechas: son las id de las fechas registradas para el curso asignado, estas las genera un programa aparte de este sistema
     * @param id_curso: es el id del curso ingresado al registro
     * @param asistencia: corresponde a la asistencia del alumno dentro del curso. Este campo solo se puede rellenar si el alumno esta aprobado.
     * @throws en caso que la base de datos este desconectada
     * nota: mantener ojo al momento de manipular este codigo ya que debe mantener siempre el nombre del curso y su id correspondiente.
     * 
     * */
    public void actualizarRegistro(string rut, string curso, string fechaCurso, string codigo, string estado, int id, string fechas, string id_fechas, int id_curso, int asistencia)
    {

        string strSQL = "UPDATE curso_registros set curso='" + curso + "',fechaCurso='" + fechaCurso + "',codigoA='" + codigo + "',estado='" + estado + "',fechas='" + fechas + "', id_fechas='" + id_fechas + "',id_curso=" + id_curso + ",asistencia=" + asistencia + " WHERE id=" + id + " AND rut = '"+rut+"';";
        try
        {

            Namespace.Conexion.update(strSQL);
            Namespace.Conexion.Desconectar();
        }
        catch (Exception ex)
        {
            Debug.Print("error al actualizar el registro : " + ex);
        }
    }

    /**
     * elimina el codigo de registro del usuario, el cual se utiliza para generar los certificados y diplomas
     * @param codigo a eliminar
     * @throw en caso que la coneccion falle
     * nota: este se elimina cuando el usuario pasa de estar aprobado a pendiente.
     * */
    public void eliminarCodigo(string codigo)
    {
        try
        {
            string strSQL = "DELETE FROM curso_codigo WHERE codigoA='" + codigo + "'";
            Namespace.Conexion.update(strSQL);
            Namespace.Conexion.Desconectar();
        }catch(Exception ex)
        {
            Debug.Print("ocurrio un error al eliminar el codigo : " + ex);
        }

    }
    
    /**
     * boton
     * genera el certificado con los datos del cliente almacenados en la base de datos, este se crea en pdf y se descarga
     * @param adapter del cliente y su registro en clase
     * @return un archivo pdf con el certificado
     * @throw en caso que la coneccion a la base de datos falle
     * nota: en caso de haber algun problema en la consulta en la base de datos o otros, el certificado no se genera
     * */
    protected void btnCertificado_Click(object sender, EventArgs e)
    {
       
        try
        {
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
            string cof2 = cof.Substring(5, 3);
            string hor = "0";
            if (cof2 == "P6A") { hor = "16"; }
            if (cof2 == "P6B") { hor = "24"; }
            if (cof2 == "PRA") { hor = "16"; }
            int id = Convert.ToInt32(lblID.Text);
            not = evaBuscar(id);
            string nomEmp = "";
            if (ddlIngreso.SelectedIndex == 1) { nomEmp = txtNomEmpresa.Text; }

            var webAppPath = Context.Server.MapPath("~/PdfTemplate/Plantilla.PDF");
            //si el sistema de certificado falla, descomentar la linea de abajo e inscrustar la rutaPdf en
            //RandomAccessFileOrArray form = new RandomAccessFileOrArray(webAppPath + rutaPdf );
            // var rutaPdf = ConfigurationManager.AppSettings["pdfPath"];
      
            MemoryStream ms = new MemoryStream();
          
            RandomAccessFileOrArray form = new RandomAccessFileOrArray(webAppPath);

            PdfReader reader = new PdfReader(form, null);
            PdfStamper stamper = new PdfStamper(reader, ms);

            string nombre = txtNombres.Text + " " + txtApellidoP.Text + " " + txtApellidoM.Text;
            if (ddlIngreso.SelectedIndex == 0) { nomEmp = nombre; }
            string fechaC = me.ToString() + " " + dia.ToString() + " de " + ano.ToString();
            stamper.AcroFields.SetField("lblCurso", txtCurso.Text + ",");
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
            Response.AddHeader("Content-Disposition", "attachment; filename=" + lblCodigo.Text + ".pdf; size=" + buffer.Length.ToString());
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
        catch (Exception ex)
        {
            //lblError.Text = ex.Message; lblError.Visible = true; 
            Debug.Print("el certificado no se imprimio por " + ex);
        }
    }

    /**
     * busca si el usuario posee alguna evaluacion dentro del programa
     * en caso de tener una evaluacion retorna la nota o sin evaluacion
     * @param: id: recibe el id del registro del alumno y retorna si este tiene alguna evaluacion adjunta
     * @return: string: retorna la nota o sin evaluacion en caso que no encuentre una
     * @throw en caso que no se pueda conectar a la base de datos
     * */
    public string evaBuscar(int id)
    {
        string vari = null;
        try
        {
            var Reader = Namespace.Conexion.reader("SELECT nota FROM curso_datos WHERE id_reg=" + id);
            using (Reader)
            {
                if (Reader.Read())
                {
                    vari = Reader["nota"].ToString();
                }
                else { vari = "Sin evaluación"; }
            }
            Reader.Close();
            Namespace.Conexion.Desconectar();
        }
        catch (Exception ex) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Error" + ex.Message + "')", true); }
        return vari;
    }

    /**
     * obtiene el nombre de mes
     * @param numeroMes: el numero del mes
     * @return el nombre del mes en string
     * @throw en caso que falle la obtencion de la fecha
     * */
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

    /**
     * convierte la primera letra de cualquier texto en mayuscula, se utiliza para la generacion de certificados y diplomas
     * para mantener un orden correcto de los nombres
     * @param texto: recive un string el cual transformara la primera palabara en mayuscula
     * @return string con el texto corregido y con la primera letra en mayuscula
     * */
    private static string ConvertirPrimeraLetraEnMayuscula(string texto)
    {
        string str = "";
        str = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(texto);
        return str;
    }

   //no se usa
    protected void txtCurso_TextChanged(object sender, EventArgs e)
    {

    }

    /**
     * maneja la seleccion del curso a registrar, a la ves asigna la sigla al generador de codigo
     * throw en caso que falle algun campo al momento de seleccionar el curso
     * nota: en caso de modificar o agregar mas curso se debera ingresar las nuevas siglas aca.
     * */
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

                calMulti.Visible = false;
                llenarCal(id);

                if (ddlSeleccion.SelectedIndex == 4 && txtFecha.Text != string.Empty)
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

            try
            {
                string curso = txtCurso.Text;


                if (curso == "Primavera P6 Básico")
                {
                    ddlSigla.SelectedIndex = 2;
                }
                if (curso == "Primavera Risk")
                {
                    ddlSigla.SelectedIndex = 3;
                }
                if (curso == "Primavera P6 Avanzado")
                {
                    ddlSigla.SelectedIndex = 1;
                }
            }
            catch { }
        }
    }

    /**
     * rellena el calendario con los dias del curso seleccionado
     * @param id: recive el id del curso, para luego buscar los dias en que tiene fecha disponible y llenar el calendario con estas
     * @throw en caso que falle la coneccion con la base de datos
     * nota: los dias se optienen de una bsae de datos aparte 
     */
    public void llenarCal(int id)
    {

        try
        {
            if (id == 1)
            {
                var adp = Namespace.Conexion.adapter("SELECT fecha FROM curso_fechas WHERE id_curso=1");
                calMulti.RemoveAllSpecialDates();
                using (adp)
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    try
                    {
                        adp.Fill(dt);
                    }
                    catch (Exception e)
                    {
                        Debug.Print("el error es : " + e);
                    }

                    if (dt.Rows.Count > 0)
                    {

                        DropDownList1.DataSource = dt;
                        DropDownList1.DataTextField = "fecha";
                        DropDownList1.DataBind();
                        DateTime fe;
                        calMulti.SelectedDates = null;
                        calMulti.DatePickerImagePath = null;
                        this.calMulti.SelectedDate = new DateTime();
                        calMulti.SelectedDates.Clear();
                        for (int i = 0; i < DropDownList1.Items.Count; i++)
                        {
                            DropDownList1.SelectedIndex = i;
                            fe = Convert.ToDateTime(DropDownList1.SelectedValue);
                            //calMulti.AddSpecialDate(fe);

                            calMulti.AddSpecialDate(fe, "primavera Basico", "primaveraBasico");

                        }
                    }
                }
            }
            if (id == 10)
            {
                calMulti.RemoveAllSpecialDates();
                Namespace.Conexion.Desconectar();
                var adp2 = Namespace.Conexion.adapter("SELECT fecha FROM curso_fechas WHERE id_curso= 10");
                System.Data.DataTable dt2 = new System.Data.DataTable();
                try
                {
                    adp2.Fill(dt2);
                }
                catch (Exception e)
                {
                    Debug.Print("el error es : " + e);
                }
                if (dt2.Rows.Count > 0)
                {
                    DropDownList1.DataSource = dt2;
                    DropDownList1.Items.Clear();
                    DropDownList1.ClearSelection();
                    DropDownList1.SelectedValue = null;
                    DropDownList1.DataTextField = "fecha";

                    try
                    {
                        DropDownList1.DataBind();
                    }
                    catch (Exception e)
                    {
                        Debug.Print("el error es : " + e);
                    }
                    DateTime fe2;
                    for (int i = 0; i < DropDownList1.Items.Count; i++)
                    {
                        DropDownList1.SelectedIndex = i;
                        fe2 = Convert.ToDateTime(DropDownList1.SelectedValue);
                        //calMulti.AddSpecialDate(fe);
                        calMulti.AddSpecialDate(fe2, "Primavera Risk", "primaveraRisk");
                    }
                }
            }
            if (id == 12)
            {
                calMulti.RemoveAllSpecialDates();
                Namespace.Conexion.Desconectar();
                var adp3 = Namespace.Conexion.adapter("SELECT fecha FROM curso_fechas WHERE id_curso= 12");
                System.Data.DataTable dt3 = new System.Data.DataTable();
                try
                {
                    adp3.Fill(dt3);
                }
                catch (Exception e)
                {
                    Debug.Print("el error es : " + e);
                }
                if (dt3.Rows.Count > 0)
                {
                    DropDownList1.DataSource = dt3;
                    DropDownList1.Items.Clear();
                    DropDownList1.ClearSelection();
                    DropDownList1.SelectedValue = null;
                    DropDownList1.DataTextField = "fecha";

                    try
                    {
                        DropDownList1.DataBind();
                    }
                    catch (Exception e)
                    {
                        Debug.Print("el error es : " + e);
                    }
                    DateTime fe3;
                    for (int i = 0; i < DropDownList1.Items.Count; i++)
                    {
                        DropDownList1.SelectedIndex = i;
                        fe3 = Convert.ToDateTime(DropDownList1.SelectedValue);
                        //calMulti.AddSpecialDate(fe);
                        calMulti.AddSpecialDate(fe3, "Primavera Avanzado", "PrimaveraAvanzado");
                    }
                }
            }


        }
        catch (Exception ex) { }
    }

    /**
     * actualisa la cantidad de dias maximos que se pueden ingresar en el calendario
     * @param mediante el txtdias recive la cantidad maxima de dias a aceptar
     * @throw en caso que exista algun problema al convertir los dias a int
     * */
    protected void txtDias_TextChanged1(object sender, EventArgs e)
    {
        try
        {
            int va = Convert.ToInt32(txtDias.Text);
            if (va > 0)
            {
                ImageButton2.Enabled = true; calMulti.Visible = false;
                calMulti.SelectedDates = null;
                calMulti.DatePickerImagePath = null;
                this.calMulti.SelectedDate = new DateTime();
                calMulti.SelectedDates.Clear();
                lblCompletos.Visible = false;

                calMulti.AllowSelectRegular = true;
                txtFecha.Text = string.Empty;
                btnAgregar.Visible = false;
                lblIdFecha.Text = string.Empty;
            }
            else { ImageButton2.Enabled = false; calMulti.Visible = false; }
        }
        catch (Exception ex) { }
    }

    /**
     * boton
     * muestra el calendario o lo desaparece, se utiliza al momento de ingresar un registro 
     */
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        if (calMulti.Visible == true)
        {
            calMulti.Visible = false;

        }
        else
        {
            calMulti.Visible = true;

        }

    }

    /**
     * obtiene el id de las fechas del curso.
     * @param fecha:  fecha de la id que se estan buscando
     * @param n: id del curso a buscar
     * throw en caso que la conecccion a la base de datos falle
     * */
    public void obtenerID(string fecha, int n)
    {
        try
        {
            DateTime fr = Convert.ToDateTime(fecha);
            string fecha2 = fr.ToString("yyyy/MM/dd");
            var adp = Namespace.Conexion.adapter("SELECT id_cfecha FROM curso_fechas WHERE id_curso=" + n + " AND fecha='" + fecha2 + "'");
            using (adp)
            {
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
            Debug.Print("ocurrio un error al obtener la id de las fechas : " + ex);
        }

    }

    /**
     * valida que la o las fechas ingreadas al calendario correspondan a las fechas existentes para el curso ingresado
     * @param fecha: fecha a verficar si es valida o no
     * @param n: id del curso a buscar
     * @return: retorna un booleano de si la fecha ingresada es valida y es un dia registrado para ese curso
     * @throw en caso que falle la coneccion con la base de datos.
     * */
    public bool validarFecha(string fecha, int n)
    {
        try
        {
            DateTime fr = Convert.ToDateTime(fecha);
            string fecha2 = fr.ToString("yyyy/MM/dd");
            bool validador;
            var adp = Namespace.Conexion.adapter("SELECT * FROM curso_fechas WHERE id_curso=" + n + " AND fecha='" + fecha2 + "'");
            using (adp)
            {
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

    /**
     * ordena los dias del curso ingresado para luego ser ingresados en la base de datos
     * @param recive una lista con las fechas a ordenar
     * @return retorna las fechas ordenadas en una lista
     * */
    static List<DateTime> SortAscending(List<DateTime> list)
    {
        list.Sort((a, b) => a.CompareTo(b));
        return list;
    }

    /**
     * prepara las fechas para ser ingresadas a la base de datos, como tambien obtiene la id de estas
     * ademas dependiendo de la opcion ingresada las fechas se ordenan de diferente forma
     * @return string con las fechas adaptadas para su almacenado o uso
     * @throw en caso que la conversion o modificacion genera un campo vacio o similar
     * */
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
            string fe2;
            int j = 1;
            int num = Convert.ToInt32(txtDias.Text);
            for (int i = 0; i < selectedDates.Count; i++)
            {
                fe = Convert.ToDateTime(selectedDates[i]);
                list.Add(fe);
                list2 = SortAscending(list);
            }
            if (ddlSeleccion.SelectedIndex == 4 && num > 1)
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
            if (ddlSeleccion.SelectedIndex == 2 || ddlSeleccion.SelectedIndex == 1)
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

    /**
     * Rellena el calendario con las fechas ingresadas en la base de datos
     * @param palabra: es la lista de fechas ingresadas, las cuales se utilizaran para rellanar el calendario
     * throw, en caso que no se pueda acceder al calendario.
     * */
    public void fechasInversas(string palabra)
    {
        try
        {
            DateTime fe;
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

    /**
     * similar a la funcion anterior, solo que el for es mas corto
     * */
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
            for (int i = 0; i < array.Length - 3; i++)
            {
                fe = Convert.ToDateTime(array[i].ToString());
                calMulti.SelectedDates.Add(fe);
            }
        }
        catch (Exception ex) { }
    }

    /**
     * recive las fechas en un array le elimine el caracter ; para luego insertarlas en txtdias
     * @param pal: string de fechas ingresdas al sistema
     * @throw en caso que exista algun error almomento de modificar las fechas.
     */
    public void fechasInversas3(string pal)
    {
        try
        {
            DateTime fe;
            string fei = txtFecha.Text;
            string[] array = pal.Split(';');
            txtDias.Text = Convert.ToString(array.Length - 1);
        }
        catch (Exception ex) { }
    }

    /**
    * prepara las fechas para ser ingresadas a la base de datos, como tambien obtiene la id de estas
     * ademas dependiendo de la opcion ingresada las fechas se ordenan de diferente forma
     * @return string con las fechas adaptadas para su almacenado o uso
     * @throw en caso que la conversion o modificacion genera un campo vacio o similar
     * */
    public string fechas2()
    {
        lblDerror.Visible = true;
        int ve = Convert.ToInt32(lblCursoId.Text);
        bool val = false;
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
            val = validarFecha(fe2, ve);

            if (val == true)
            {
                lblDerror.Visible = false;
                selectedDatesString += fe2 + ";";
            }
            else { lblDerror.Visible = true; }
        }
        return selectedDatesString.ToString();
    }
    /**
     * prepara las fechas para ser ingresadas a la base de datos, como tambien obtiene la id de estas
     * ademas dependiendo de la opcion ingresada las fechas se ordenan de diferente forma
     * @return string con las fechas adaptadas para su almacenado o uso
     * @throw en caso que la conversion o modificacion genera un campo vacio o similar
        */
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

        selectedDatesString = fe2;
        return selectedDatesString.ToString();

    }

    /**
  * prepara las fechas para ser ingresadas a la base de datos, como tambien obtiene la id de estas
  * ademas dependiendo de la opcion ingresada las fechas se ordenan de diferente forma
  * @return string con las fechas adaptadas para su almacenado o uso
  * @throw en caso que la conversion o modificacion genera un campo vacio o similar
     */
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
        fe2 = list2[selectedDates.Count - 1].ToString("yyyy/MM/dd");
        selectedDatesString = fe2;
        return selectedDatesString.ToString();
    }
    
    /* las funciones de fechas varian dependiendo el tipo de ingreso o obtencion de los datos en tiempo de ejecucion, lamentablemente por tiempo se me fue imposible llegar enteder cada una
     * espero que almenos te sirva para entender su funcionamiento general*/

    /**
     * funcion que se encarga ver las fechas seleccionadas en el calendario, como tambien ver si cumple que los requisitos
     * que las fechas sean del curso asignado, y la cantidad de dias asignados.
     * throw en caso que ocurra un error al obtener las fechas del calendario, el sistema se reinicia.
     * */
    protected void calMulti_DateChanged(object sender, EventArgs e)
    {

        if (txtDias.Text != string.Empty)
        {
            try
            {

                calMulti.AllowSelectRegular = true;
                string fe2 = calMulti.SelectedDate.ToString();
                int con = Convert.ToInt32(txtDias.Text);
                int con2 = calMulti.SelectedDates.Count;

                string fer = calMulti.SelectedDate.ToString();
                int n = Convert.ToInt32(lblCursoId.Text);
                bool val = validarFecha(fer, n);
                if (val == false)
                {
                    if (fe2 == "01/01/0001 0:00:00") { lblDerror.Visible = false; con2--; }
                    else
                    {
                        lblFechasResp.Text = fechas2();
                        string palabra = lblFechasResp.Text;
                        fechasInversas(palabra);
                        con2--;
                    }
                }
                if (con2 == con)
                {
                    lblIdFecha.Text = string.Empty;

                    calMulti.AllowSelectRegular = false;
                    txtFecha.Text = fechas();
                    lblCompletos.Visible = true;
                    if (ddlSeleccion.SelectedIndex == 2 || ddlSeleccion.SelectedIndex == 1)
                    {
                        btnAgregar.Visible = true;
                        calMulti.Visible = false;
                    }
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

                    lblFechasResp.Text = fechas2();
                    fechasInversas2();
                    con2 = con2 - 2;
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

            ImageButton2.Enabled = false;
            txtFecha.Text = string.Empty;
            lblCompletos.Visible = false;
            lblDerror.Visible = false;
            calMulti.AllowSelectRegular = true;
        }
    }
  
    /**
     * sistema antiguo de sigla, que se utilizaba para asignar la sigla que correspondia a cada curso. Anteriormente la sigla del curso se asignaba manualmente,
     * pero ahora se asigna automaticamente, pero se usa la misma interface ya creada
     * */
    protected void ddlSigla_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSigla.SelectedIndex == 0) { lblErrorSigla.Visible = true; lblNombreCurso.Visible = false; }
        else { lblErrorSigla.Visible = false; }
        if (ddlSigla.SelectedIndex == 1) { lblNombreCurso.Text = "Primavera P6 Avanzado"; lblNombreCurso.Visible = true; }
        if (ddlSigla.SelectedIndex == 2) { lblNombreCurso.Text = "Primavera P6 Básico"; lblNombreCurso.Visible = true; }
        if (ddlSigla.SelectedIndex == 3) { lblNombreCurso.Text = "Primavera Risk Análisis"; lblNombreCurso.Visible = true; }
    }

    /**
     * obtiene los rut de las empresas ingreadas en la base de datos, al momento de ingresar un cliente
     * que este trabajando con una empresa. se busca en la base de datos y se retorna el rut de la empresa
     * en caso de no existir arroja un mensaje advirtiendo
     * @param nomEmp: se obtiene el nombre empresa del txtNomEmpresa para luego realizar la busqueda en la base de datos
     * @throw en caso que ocurra un error con la conexion de la base de datos
     * */
    protected void txtNomEmpresa_TextChanged1(object sender, EventArgs e)
    {

        try
        {

            lblRutEmpresa.Visible = true;
            string nomEmp = "";
            nomEmp = txtNomEmpresa.Text;
            var Reader = Namespace.Conexion.reader("SELECT Crut FROM cliente_empresa WHERE Crazon_social='" + nomEmp + "'");
            string val = "";
            using (Reader)
            {
                if (Reader.Read())
                {
                    val = Reader["Crut"].ToString();
                    lblRutEmpresa.Text = val;
                }
                else
                {
                    lblRutEmpresa.Text = "Esta empresa no esta registrada";
                }
            }
            Reader.Close();
            Namespace.Conexion.Desconectar();
        }
        catch (Exception ex)
        {
            
        }
    }

    //no se usa
    protected void DropDownList1_SelectedIndexChanged1(object sender, EventArgs e)
    {

    }

    /**
     * se utiliza para generar una lista de las empresas registradas en la bsae de datos y estas se puedan ir filtrando en tiempo real
     * cuando se van ingresando a un cliente.
     * @param frefitext: texto que se esta escribiendo en la casilla y se esta revisando constantemente para luego reenviar los posibles resultados a lo que esta escrito
     * @param count; cantidad de empresas inscritas en el arraylist existente
     * @return: retorna un array string con los posibles resultados de la busqueda
     * */
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count)
    {
        return (from m in nombresEmp2 where m.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select m).Take(count).ToArray();
    }

    //no se usa
    protected void btnModificar_PopupControlExtender_DataBinding(object sender, EventArgs e)
    {

    }

    /**
     * verificador de la asistencia que esta siendo ingresada en la casilla txtAsistencia
     * @throw en caso que el valor sea vacio o negativo
     * */
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

    /**
     * boton
     * muestra el registro del usuario despues de buscarlo en la base de datos, se utiliza en la opcion del menu 0, buscar cliente
     * @throw en caso que al momento de limpiar los controles ocurra un error. 
     * */
    protected void mostrarRegistro(object sender, EventArgs e)
    {
        txtNomEmpresa.Visible = false;
        lblNomEmpresa.Visible = false;
        btnCliente.Visible = false;
        btnListar.Visible = true;
		 ddlEstado.Enabled = true;
        gvLista.Visible = false;
        rvRut.Visible = false;
        ImageButton1.Visible = true;

        txtListar.Visible = true;
        lblError.Visible = false;
        ddlSigla.Visible = true;
        gvCurso.Visible = false;
        calMulti.Visible = false;
        txtAsistencia.Visible = true;
        lblSigla.Visible = true;
        ddlSigla.Visible = true;
        btnAgregar.Visible = false;

        rfvEmpresa.Visible = false;
        btnModificarCli.Visible = false;
        lblErrorSigla.Visible = false;
        lblNombreCurso.Visible = false;
        lblVeri1.Visible = false;
        lblAsistencia.Visible = true;
        calMulti.Visible = false;

        lblCompletos.Visible = false;
        lblDerror.Visible = false;
        btnModificarCli.Visible = false;
        lblRutEmpresa.Visible = false;
        btnGenerar.Visible = false;
        btnModificar.Visible = false;
        btnCertificado.Visible = false;

        //segundo formulario
        formulario1.Visible = true;
        formulario2.Visible = true;
        formulario3.Visible = true;
        formulario4.Visible = true;
        formulario5.Visible = false;
        formulario6.Visible = true;
        formulario7.Visible = true;
        formulario8.Visible = true;
        formulario9.Visible = true;


        //text
        txtNombres.ReadOnly = true;
        txtApellidoM.ReadOnly = true;
        txtApellidoP.ReadOnly = true;
        txtNomEmpresa.ReadOnly = true;

        //enabled
        ddlSexo.Enabled = false;
        ddlIngreso.Enabled = false;
        txtCurso.Enabled = true;
        ddlEstado.Enabled = true;
        txtFecha.Enabled = true;
        txtDias.Enabled = true;
        ImageButton1.Enabled = true;
        ImageButton2.Enabled = true;
        ddlSigla.Enabled = false;
		btnListar.Enabled = false;
        //randoly
        txtRut.ReadOnly = true;
        txtDias.ReadOnly = false;
        txtCurso.ReadOnly = true;
        txtFecha.ReadOnly = false;
        txtTelefono.ReadOnly = true;
        txtEmail.ReadOnly = true;
        txtAsistencia.ReadOnly = true;

        txtDias.Text = string.Empty;

        calMulti.AllowDeselect = true;
        calMulti.AllowSelectRegular = true;
    }

    /**
     * envia toda la informacion del cliente buscando en la opcion del menu 0 "buscar cliente" a la opcion "modificar cliente" para
     * facilitar la interfaz de la pagina.
     * @throw en caso que al momento de limpiar los controles ocurra un error. 
     */
    protected void modificarCliente(object sender, EventArgs e)
    {
        string rut = txtRut.Text;
        ddlSeleccion.SelectedIndex = 3;
        

        try
        {
            LimpiarControles();

            //visibles
			btnCliente.Visible = false;
            btnListar.Visible = true;
            btnListar.Visible = true;
			bModificar.Visible = false;
			aRegistro.Visible = false;
            rvRut.Visible = true;
            ImageButton1.Visible = true;
            txtListar.Visible = false;
            rfvEmpresa.Visible = false;
            txtNomEmpresa.Visible = false;
            btnAgregar.Visible = false;
            lblError.Visible = false;
            btnModificarCli.Visible = true;
            btnGenerar.Visible = false;
            btnModificar.Visible = false;
            btnCertificado.Visible = false;
            ddlSigla.Visible = true;
            gvCurso.Visible = false;
            gvLista.Visible = false;
            calMulti.Visible = false;
            lblSigla.Visible = true;
            ddlSigla.Visible = true;
            lblErrorSigla.Visible = false;
            lblNombreCurso.Visible = false;
            lblVeri1.Visible = false;
            lblAsistencia.Visible = true;
            txtAsistencia.Visible = true;
            calMulti.Visible = false;
            lblCompletos.Visible = false;
            lblDerror.Visible = false;
            lblRutEmpresa.Visible = false;


            //randoly
            txtRut.ReadOnly = false;
            txtNombres.ReadOnly = true;
            txtApellidoM.ReadOnly = true;
            txtApellidoP.ReadOnly = true;
            txtNomEmpresa.ReadOnly = false;
            txtTelefono.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtDias.ReadOnly = true;
            txtCurso.ReadOnly = true;
            txtFecha.ReadOnly = true;
            txtAsistencia.ReadOnly = true;


            //enabled
            ddlSexo.Enabled = false;
            ddlIngreso.Enabled = false;
            txtCurso.Enabled = true;
            ddlEstado.Enabled = false;
            txtFecha.Enabled = true;
            ImageButton2.Enabled = false;
            txtDias.Enabled = true;
            ImageButton1.Enabled = false;
            ddlSigla.Enabled = false;
			btnListar.Enabled = true;

            txtDias.Text = string.Empty;


            //segundo formulario
            formulario1.Visible = false;
            formulario2.Visible = false;
            formulario3.Visible = false;
            formulario4.Visible = false;
            formulario5.Visible = false;
            formulario6.Visible = false;
            formulario7.Visible = false;
            formulario8.Visible = false;
            formulario9.Visible = false;

            //buscar cliente
            buscarCliente(rut);

        }
        catch (Exception ex) { }

    }

       /**
     * en caso que se inserte alguna valor numero mayor a 100 o menor a 0 en txt nota se habilita el boton de 
     * agregar o modificar con nota
     * @throw en caso que exista un valor null
     * */
    protected void txtnota_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(nota.Text) > 0 || Convert.ToInt32(nota.Text) <= 100)
            {
                if (ddlSeleccion.SelectedIndex == 2)
                {
                    btnAgregarConNota.Visible = true;
                }
                if (ddlSeleccion.SelectedIndex == 4)
                {
                    btnModificarConNota.Visible = true;
                }
            }
        }catch(Exception ex)
        {
            Debug.Print("error : "+ex);
        }
    }


    /**
     * agregar un usuario con nota incluida, en caso que se elija ingresar un alumno en estado aprobado
     * @throw en caso que la coneccion a la base de datos falle
     * */
    protected void btnAgregaConNota_Click(object sender, EventArgs e)
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
            var adp = Namespace.Conexion.adapter("SELECT * FROM curso_registros WHERE rut='" + rut + "' AND id_curso=" + idCurso + " AND id_fechas='" + ids + "'");
            System.Data.DataTable dt = new System.Data.DataTable();
            using (adp)
            {
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
                        for (int i = 0; i < 1;)
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
                        string strSQL = "INSERT INTO curso_registros VALUES('" + rut + "','" + curso + "','" + fechaCurso + "','" + fecha + "','" + codigo + "','" + estado + "'," + 0 + ",'" + fechas + "','" + ids + "'," + idCurso + "," + asistencia + ");";
                        insertarRegistro(strSQL);
                        agregarNota(rut, nota.Text,fechaCurso);
                        string message = "alert('Registro Agregado')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        LimpiarControles();
                    }
                }
            }
        }
        catch { }
    }

    /**
     * recive el rut, la nota y fecha del curso, donde se genera el script y se obtiene el nombre completo del usuario, para luego ingresarlo en la base de datos
     * @throw en caso que la base de datos falle
     * */
    public void agregarNota(string rut, string nota, string fechaCurso)
    {
       try
        {
            string nombre2;
            int id_reg2;
            var Reader = Namespace.Conexion.reader("SELECT A.nombres, A.apellidoP, A.apellidoM, MAX( B.id )FROM sgsmtc2.curso_cliente A, sgsmtc2.curso_registros B WHERE A.rut ='"+ rut + "'AND B.rut = '"+ rut + "';") ;
            using (Reader)
            {
                if (Reader.Read())
                {
                    nombre2 = Reader["nombres"].ToString() + " " + Reader["apellidoP"].ToString() + " " + Reader["apellidoM"].ToString();
                   var aux =  Reader["MAX( B.id )"];
                    id_reg2 = Int32.Parse(aux.ToString());
                    nota = nota + "%";
                    Reader.Close();
                    Namespace.Conexion.Desconectar();
                    insertarEval(nombre2, fechaCurso, rut, nota, id_reg2);
                }
            }
           
        }catch(Exception ex)
        {
            Debug.Print("error ingresar nota es : "+ex);
        }
    }

    /**
     * inserta la evaluacion al usuario recien ingresado
     *  @param nombre: nombre completo del usuario
     *  @param fecha: fecha del curso realizado
     *  @param rut: rut: rut del usuario al cual se evaluo
     *  @param nota: nota ingresada del curso
     *  @oaram id_reg: id del registro al cual se le insertara la evaluacion
     *  @throw en caso que la base de datos falle
     * */
    public void insertarEval(string nombre, string fecha, string rut, string nota, int id_reg)
    {
        try
        {
            string strSQL = "INSERT INTO curso_datos VALUES('" + nombre + "','" + fecha + "','" + rut + "','" + nota + "'," + 0 + "," + id_reg + ");";
            Namespace.Conexion.insert(strSQL);
            Namespace.Conexion.Desconectar();
        }
        catch (Exception ex)
        {
            Debug.Print("error al insertar la nota : " + ex);
        }
    }

    /**
     * modificar el registro y le ingresa la nota nueva al usuario, todo esto se hace al momento de modificar usuario y se toma la desicion de
     * pasarlo a aprobado, pero este aun no tiene una nota ingreada en la base de datos
     * @throw en caso que falle la coneccion con la base de datos
     * */
    protected void btnModificaConNota_Click(object sender, EventArgs e)
    {
        try
        {
            string curso = txtCurso.Text;


            if (curso == "Primavera P6 Básico")
            {
                ddlSigla.SelectedIndex = 2;
            }
            if (curso == "Primavera Risk")
            {
                ddlSigla.SelectedIndex =  3;
            }
            if (curso == "Primavera P6 Avanzado")
            {
                ddlSigla.SelectedIndex = 1;
            }
        }
        catch
        {
            Debug.Print("Error al leer la sigla");
        }

        string confirmValue = Request.Form["confirm_value"];
        //el nuevo registro es aprovado
        if (confirmValue == "Si")
        {
            txtRut.ReadOnly = true;
            gvLista.Visible = false;
            gvLista.DataSource = null;
            gvLista.DataBind();
            btnModificar.Visible = false;
            try
            {

                id = Convert.ToInt32(lblID.Text);
                int idO = id;
                Debug.Print("el id es : " + idO);
                var Reader = Namespace.Conexion.reader("SELECT estado FROM curso_registros WHERE id=" + idO);
                string estadx = "";
                using (Reader)
                {
                    if (Reader.Read())
                    {
                        estadx = Reader["estado"].ToString();
                    }
                }
                Reader.Close();
                Namespace.Conexion.Desconectar();
                //el usuario anteriormente estaba aprovado
                //se guarda los registros de cursos antes de modificar
                if (estadx == "Aprobado")
                {

                    Reader = Namespace.Conexion.reader("SELECT rut,codigoA,id,estado FROM curso_registros WHERE id=" + idO);

                    string rutAux, codigoAux, idAux, mensaje;
                    rutAux = string.Empty;
                    codigoAux = string.Empty;
                    idAux = string.Empty;
                    mensaje = string.Empty;
                    using (Reader)
                    {
                        if (Reader.Read())
                        {
                            rutAux = Reader["rut"].ToString();
                            codigoAux = Reader["codigoA"].ToString();
                            idAux = Reader["id"].ToString();
                        }
                    }
                    Reader.Close();
                    Namespace.Conexion.Desconectar();
                    DateTime fechaH = DateTime.Now.Date;
                    mensaje = fechaH.ToString("yyyy/MM/dd");
                    lblError.Visible = true;
                    lblError.Text = rutAux + codigoAux + idAux;
                    string strSQL = "INSERT INTO curso_registros_old VALUES('" + rutAux + "','" + codigoAux + "'," + idAux + "," + 0 + ",'" + mensaje + "');";
                    Namespace.Conexion.insert(strSQL);

                }

                //se obtienen los datos del formulario
                rut = txtRut.Text;
                string curso = txtCurso.Text;
                string estado = ddlEstado.SelectedItem.ToString();
                string fechaCurso = fechas3();
                string sig = ddlSigla.SelectedValue;
                string ids = lblIdFecha.Text;
                int idCurso = Convert.ToInt32(lblCursoId.Text);
                string fechas = txtFecha.Text;
                int asistencia = 0;

                var adp = Namespace.Conexion.adapter("SELECT * FROM curso_registros WHERE rut='" + rut + "' AND id_curso=" + idCurso + " AND id_fechas='" + ids + "' AND id NOT IN('" + id + "') ");
                System.Data.DataTable dt = new System.Data.DataTable();
                using (adp)
                {

                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        string message = "alert('Este rut contiene un registro en ese curso y en las mismas fechas.')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                    }

                    //un estado a sido seleccionado
                    if (ddlEstado.SelectedIndex != 0 && dt.Rows.Count <= 0)
                    {
                        // el usuario se selecciono como aprovado
                        if (ddlEstado.SelectedIndex == 1 && ddlSigla.SelectedIndex != 0)
                        {

                            for (int i = 0; i < 1;)
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
                            string nombre2 = txtNombres.Text + " " + txtApellidoP.Text + " " + txtApellidoM.Text;
                            actualizarRegistro(rut, curso, fechaCurso, codigo, estado, id, fechas, ids, idCurso, asistencia);
                            insertarEval(nombre2, fechaCurso, rut, nota.Text, idO);
                            LimpiarControles();
                            txtRut.ReadOnly = true; txtRut.Enabled = true;
                            lblIdFecha.Text = string.Empty;
                            string message = "alert('Registro Modificado')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                            txtRut.Visible = true;
                            btnListar.Visible = true;
                        }
                    }
                    else
                    {
                        string message = "alert('Ocurrió un error durante la modificación del registro.')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print("ocurrio un error en modificar el registro : " + ex);
            }

        }
    }
    
    /**
     *valida si las fechas ingreadas en el registro no estan repetidas en otro curso del alumno, para evitar registros dobles
     * @param rut: el rut del usuario que se utiliza para buscarlo dentro de la base de datos para ver si tiene otro curso con las mismas fechas
     * @param idCurso: id del curso seleccionado para el registro para asi evitar un doble registro
     * @return: boleano true si el ususario aparece en otro curso con la misma fechas selecionadas anteriormente
     * @throw en caso que la base de datos falle
     * nota: solo valida que el usuario no mantenga registros repetidos con el mismo id del curso, pero si acepta que el alumno pueda tener cursos diferentes en la misma fecha
     * por si los cursos son a diferente hora.
     * */
    public Boolean validarFechaIndividual(string rut, int idCurso)
    {
        try
        {
            char[] delimiterChars = { ';' };
            var aux = Namespace.Conexion.adapter("SELECT fechas FROM curso_registros WHERE rut='" + rut + "' AND id_curso=" + idCurso);
            System.Data.DataTable dtaux = new System.Data.DataTable();
            using (aux)
            {
                Boolean flag = false;
                string info = null;
                aux.Fill(dtaux);

                if (dtaux.Rows.Count > 0)
                {
                    for (int i = 0; i < dtaux.Rows.Count; i++)
                    {
                        info = dtaux.Rows[i][0].ToString();
                        string[] words = info.Split(delimiterChars);
                        string[] words2 = txtFecha.Text.Split(delimiterChars);
                        for (int j = 0; j < words2.LongLength; j++)
                        {
                            for (int z = 0; z < words.LongLength; z++)
                            {
                                if (words2[j].Equals(words[z]))
                                {

                                    if (words[z] != "" && words2[j] != "")
                                    {
                                        flag = true;

                                    }
                                }
                            }
                        }
                    }
                }
                if (flag)
                {
                    Namespace.Conexion.Desconectar();
                    return true;
                }
            }
            Namespace.Conexion.Desconectar();
            return false;
        }
        catch (Exception ex)
        {
            Debug.Print("error al validar la fechas del usuario : " + ex);
            return true;
        }
    }
	
	public bool validarRut(string rut ) {
            
     bool validacion = false;
     try {
        rut =  rut.ToUpper();
        rut = rut.Replace(".", "");
        rut = rut.Replace("-", "");
        int rutAux = int.Parse(rut.Substring(0, rut.Length - 1));
 
        char dv = char.Parse(rut.Substring(rut.Length - 1, 1));
 
        int m = 0, s = 1;
        for (; rutAux != 0; rutAux /= 10) {
           s = (s + rutAux % 10 * (9 - m++ % 6)) % 11;
        }
        if (dv == (char) (s != 0 ? s + 47 : 75)) {
           validacion = true;
        }
     } catch (Exception ex) 
	 {
		Debug.Print("ocurrio un error al validar el rut : "+ex);
	 }
     return validacion;
  }

}