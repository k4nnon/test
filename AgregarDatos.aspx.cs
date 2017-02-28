using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Collections;
using System.Linq;

public partial class AgregarDatos : System.Web.UI.Page
{
    /**
   * @autor: 
   * @co autor: gonzalo zeballos. mail: maxpayne_ga@hotmail.com
   * @Sistema de toma de horas, generación de certificados y administración de cliente; dentro de los cursos Primavera
   * */

    /**
     * nombresEmp2: almacena el nombre de la empresa antes  de ser almacenada
     * nombresEmp: almacena un arreglo con todos los nombres de las empresas registradas en el sistema.
     * */
    string[] nombresEmp;
    static string[] nombresEmp2;


    /**
     * Al iniciar la pagina, se cargaran las empresas en el arreglo para su próximo uso.
     * @Exception: error en la coneccion de la base de datos, 
     * */
    protected void Page_Load(object sender, EventArgs e)
    {
        lblSinReg.Visible = false;
        lblCompletos.Visible = false;
      
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
        catch (Exception ex) { }
    }

    /**
     * DropDownList
     * Menu principal de la pagina, controla la vistas de todos los label, text, dropd del formulario
     */
    protected void ddlSeleccion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSeleccion.SelectedIndex == 0) { btnAgregar.Visible = true; btnModificar.Visible = false; imgId.Enabled = false; txtId.Enabled = false; LimpiarControles(); txtNota.ReadOnly = true;
                                                tr1.Visible = true; tr2.Visible = false; tr3.Visible = true; tr5.Visible = true; tr6.Visible = true; tr7.Visible = true;
                                                tr8.Visible = true; tr9.Visible = true; tr11.Visible = false; tr12.Visible = false; tr13.Visible = false; tr14.Visible = false;
                                                tr15.Visible = false; tr16.Visible = false; tr17.Visible = false; LimpiarControles();
        }
        if (ddlSeleccion.SelectedIndex == 1) { btnAgregar.Visible = false; btnModificar.Visible = true; imgId.Enabled = false; txtId.Enabled = false; LimpiarControles(); txtNota.ReadOnly = true;
                                                tr1.Visible = true; tr2.Visible = true; tr3.Visible = true;  tr5.Visible = true; tr6.Visible = false; tr7.Visible = true;
                                                tr8.Visible = true; tr9.Visible = true; tr11.Visible = false; tr12.Visible = false; tr13.Visible = false; tr14.Visible = false;
                                                tr15.Visible = false; tr16.Visible = false; tr17.Visible = false; LimpiarControles();

        }
        if (ddlSeleccion.SelectedIndex == 2) { tr1.Visible = false; tr2.Visible = false; tr3.Visible = false; tr5.Visible = false; tr6.Visible = false; tr7.Visible = false;
                                               tr8.Visible = false; tr9.Visible = false; tr11.Visible = true; tr12.Visible = true; tr13.Visible = true; tr14.Visible = false;
                                             tr15.Visible = true; tr16.Visible = true; tr17.Visible = true; LimpiarControles();
        } 
    }

    /**
     *Label OnTextChanged
     * valida y reconstruche el rut al momento de ser ingresado en la casilla rut
     */
    protected void txtRut_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrorRut.Visible = false;
            string rut = txtRut.Text;
            txtRut.Text = Rut(rut);
            string rut2 = txtRut.Text;
            txtNombre.Text = string.Empty;
            txtIdReg.Text = string.Empty;
            gvIdRegistro.DataSource = null;
            gvIdRegistro.DataBind();
            buscarCliente(rut2);
            if (ddlSeleccion.SelectedIndex == 1)
            {
                imgId.Visible = true;
                txtId.Enabled = true;
            }
        }
        catch (Exception ex) { }
    }

    /**
     * boton
	 * inserta la evaluacion en el sistema, con los datos ingresados en los txtCurso
	 * @throw en caso que la base de datos no funcione
     * */
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtRut.Text != string.Empty && txtNombre.Text != string.Empty &&  txtNota.Text != string.Empty && txtIdReg.Text != string.Empty)
            {
                int aux = -1;
                if (!int.TryParse(txtNota.Text,out aux))
                {
                    string message2 = "alert('La nota debe ser un numero')";
                    ScriptManager.RegisterClientScriptBlock((sender as System.Web.UI.Control), this.GetType(), "alert", message2, true);
                }
                if (aux <= 0 || aux > 100)
                {
                    string message2 = "alert('La nota debe estar en un rango entre 1 y 100')";
                    ScriptManager.RegisterClientScriptBlock((sender as System.Web.UI.Control), this.GetType(), "alert", message2, true);
                    return;
                }
                string nombre = txtNombre.Text;
                string fecha = DateTime.Today.ToString("yyyy-MM-dd") ;
                string rut = txtRut.Text;
                string nota = txtNota.Text + "%";
                int id_reg = Convert.ToInt32(txtIdReg.Text);
                insertarEval(nombre, fecha, rut, nota, id_reg);
                string message = "alert('Evaluación agregada')";
                ScriptManager.RegisterClientScriptBlock((sender as System.Web.UI.Control), this.GetType(), "alert", message, true);
                LimpiarControles();
            }
            else
            {
                string message = "alert('Debe rellenar todos los campos ')";
                ScriptManager.RegisterClientScriptBlock((sender as System.Web.UI.Control), this.GetType(), "alert", message, true);
            }
        }
        catch (Exception ex)
        {
            Debug.Print("error al ingresar la nota : " + ex);
        }
    }

    /**
     * limpia todos los label, reinicia los calendario, dropDownList,etc
     * */
    public void LimpiarControles()
    {
        bReinicio.Visible = false;
        errorVacio.Visible = false;
        lblDerror.Visible = false;
        txtRut.Text = string.Empty;
        txtNombre.Text = string.Empty;
        txtId.Text = string.Empty;
        gvId.DataSource = null;
        gvId.DataBind();
        errorVacio.Visible = false;
        txtNota.Text = string.Empty;
        txtIdReg.Text = string.Empty;
        gvIdRegistro.DataSource = null;
        gvIdRegistro.DataBind();
        imgId.Enabled = false;
        txtId.Enabled = false;
        imgReg.Enabled = false;
        txtNota.ReadOnly = true;
        calMulti.SelectedDates = null;
        calMulti.DatePickerImagePath = null;
        this.calMulti.SelectedDate = new DateTime();
        calMulti.SelectedDates.Clear();
        buscarAlumnos.Visible = false;
        CBaprovar.Visible = false;
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
        catch (Exception ex) { }
    }

    /**
     * buscar un cliente con el rut ingresado, retornando los datos de estes a los label correspondientes
     * @param: rut: rut del cliente a buscar
     * @return: los datos del usuario a buscar, en los label de informacion.
     * */
    public void buscarCliente(string rut)
    {
        if (ddlSeleccion.SelectedIndex == 0)
        {
            try
            {

                string run = txtRut.Text;
                var adp = Namespace.Conexion.adapter("SELECT id FROM curso_registros WHERE rut='" + rut + "' && not exists (select 1 from curso_datos where curso_datos.id_reg = curso_registros.id)");
                System.Data.DataTable dt = new System.Data.DataTable();
                adp.Fill(dt);
                var adp2 = Namespace.Conexion.adapter("SELECT id FROM curso_registros WHERE rut='" + run + "'");
                System.Data.DataTable dt2 = new System.Data.DataTable();
                adp2.Fill(dt2);
                txtNombre.Text = string.Empty;
                txtId.Text = string.Empty;
                gvId.DataSource = null;
                gvId.DataBind();
                txtNota.Text = string.Empty;
                txtIdReg.Text = string.Empty;
                gvIdRegistro.DataSource = null;
                gvIdRegistro.DataBind();
                imgId.Enabled = false;
                txtId.Enabled = false;
                txtNota.ReadOnly = false;
                if (dt2.Rows.Count > 0)
                {
                    if (dt.Rows.Count > 0)
                    { 
                       
                       imgReg.Enabled = true; 
                        lblSinReg.Visible = false; lblCompletos.Visible = false;
                        lblCompletos.Visible = false;
                        var Reader = Namespace.Conexion.reader("SELECT * FROM curso_cliente WHERE rut='" + rut + "'");
                        using (Reader)
                        {
                            if (Reader.Read())
                            {
                                txtRut.Text = Reader["rut"].ToString();
                                txtNombre.Text = Reader["nombres"].ToString() + " " + Reader["apellidoP"].ToString() + " " + Reader["apellidoM"].ToString();
                               
                                if (ddlSeleccion.SelectedIndex == 1)
                                {
                                    
                                    imgId.Enabled = true; txtId.Enabled = true;imgReg.Enabled = true;
                                 
                                }

                            }
                            else
                            {
                                lblErrorRut.Visible = true;
                              
                            }
                        }
                        Reader.Close();
                        Namespace.Conexion.Desconectar();
                    }
                    else {  lblSinReg.Visible = false; lblCompletos.Visible = true; imgReg.Enabled = false; }
                }
                else { lblSinReg.Visible = true; lblCompletos.Visible = false; imgReg.Enabled = false; }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Rut formato erroneo" + ex + "')", true);
            }
        }
        if (ddlSeleccion.SelectedIndex == 1)
        {
            try
            {
                var Reader = Namespace.Conexion.reader("SELECT * FROM curso_cliente WHERE rut='" + rut + "'");
                using (Reader)
                {
                    if (Reader.Read())
                    {
                        txtRut.Text = Reader["rut"].ToString();
                        txtNombre.Text = Reader["nombres"].ToString() + " " + Reader["apellidoP"].ToString() + " " + Reader["apellidoM"].ToString();
                        if (ddlSeleccion.SelectedIndex == 1) { imgId.Enabled = true; txtId.Enabled = true;  }
                    }
                    else { lblErrorRut.Visible = true; }
                }
                Reader.Close();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Rut formato erroneo" + ex + "')", true);
            }
        }
    }

    /**
     * Valida el rut ingresado al label
     * @param: rut: rut ingresado es vereficado si es real o falso
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

	//no se usa
    protected void txtIdReg_TextChanged(object sender, EventArgs e)
    {

    }

	/**
	* boton
	* muestra en un gridview todas las notas que posee el alumno hasta la fecha
	* @throw en caso que la conexion con la base de datos falle
	*/
    protected void imgId_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (gvId.Visible == false)
            {
                string rut = txtRut.Text;
                var adp = Namespace.Conexion.adapter("SELECT C.id AS 'Id de la evaluacion', C.fecha AS 'Fecha de la evaluacion',C.nota AS 'Nota', C.id_reg AS 'Id del registro', A.curso AS 'Curso', DATE_FORMAT(A.fechaCurso,'%d/%m/%Y') AS 'Inicio del curso' FROM curso_datos C, curso_registros A WHERE C.rut='" + rut + "' AND C.id_reg=A.id");
                System.Data.DataTable dt = new System.Data.DataTable();
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (gvId.Visible == false) { gvId.Visible = true; }
                    gvId.DataSource = null;
                    gvId.DataBind();
                    gvId.DataSource = dt;
                    gvId.DataBind();
                }

                gvId.Visible = true;
            }
            else { gvId.Visible = false; }
        }
        catch (Exception ex) { }
    }

    /**
     * boton
     * al apretar el boton, se mostrara un gridview con todas los cursos que posee el alumno, con sus datos
     * respectivos, para luego seleccionarlos y trabajar con ellos
     * @throw en caso que la conexion con la base de datos falle
     * */
    protected void imgReg_Click(object sender, ImageClickEventArgs e)
    {
        
        try
        {
            if (gvIdRegistro.Visible == false)
            {
                string rut = txtRut.Text;
                var adp = Namespace.Conexion.adapter("SELECT id AS 'Id Registro',curso AS 'Curso',fechaCurso AS 'Fecha de inicio del curso'  FROM curso_registros WHERE rut='" + rut + "' && not exists (select 1 from curso_datos where curso_datos.id_reg = curso_registros.id)");
                using (adp)
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (gvIdRegistro.Visible == false) { gvIdRegistro.Visible = true; }
                        gvIdRegistro.DataSource = null;
                        gvIdRegistro.DataBind();
                        gvIdRegistro.DataSource = dt;
                        gvIdRegistro.DataBind();

                    }
                    else { lblCompletos.Visible = true; }
                }
                gvIdRegistro.Visible = true;
            }
            else { gvIdRegistro.Visible = false; }
        }
        catch (Exception ex) { }
    }

   /**
   *
   */	
    protected void gvId_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select" && ddlSeleccion.SelectedIndex == 1)
        {

            try
            {
                Int16 num = Convert.ToInt16(e.CommandArgument);
                string t1, t2, t3, t4;
                t1 = gvId.Rows[num].Cells[1].Text;
                txtId.Text = t1;
                t2 = gvId.Rows[num].Cells[2].Text;
                t3 = gvId.Rows[num].Cells[3].Text;
                string[] array = t3.Split('%');
                txtNota.Text = array[0];
                t4 = gvId.Rows[num].Cells[4].Text;
                txtIdReg.Text = t4;
                gvId.Visible = false;
                imgReg.Enabled = true;
            }
            catch (Exception ex) { }
        }
    }

    /**
     * maneja la seleccion del gridview registro, el cual contiene todas las notas ingresadas al rut 
     * selecionado anteriormente, para asi cargar sus datos y modificarlos
     * @throw en caso que ocurra un null exception
     * */
    protected void gvIdRegistro_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            try
            {
                Int16 num = Convert.ToInt16(e.CommandArgument);
                string t1;
                t1 = gvIdRegistro.Rows[num].Cells[1].Text;
                txtIdReg.Text = t1;
                gvIdRegistro.Visible = false;
            }
            catch (Exception ex) { }
        }
    }

	//no se usa
    protected void gvId_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
  
	//no se usa
    protected void gvIdRegistro_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    /**
     * boton
     * modifica la nota del alumno selecionado con aterioridad, para ello se obtienen lso datos desde los txt
     * y se llama a actualizarEva para almacenar el nuevo registro
     * @throw en caso que la conexion con la base de datos falle
     * */
    protected void btnModificar_Click(object sender, EventArgs e)
    {
        try
        {
            int aux = -1;
            if (!int.TryParse(txtNota.Text, out aux))
            {
                string message2 = "alert('La nota debe ser un numero')";
                ScriptManager.RegisterClientScriptBlock((sender as System.Web.UI.Control), this.GetType(), "alert", message2, true);
            }
            if (aux <= 0 || aux > 100)
            {
                string message2 = "alert('La nota debe estar en un rango entre 1 y 100')";
                ScriptManager.RegisterClientScriptBlock((sender as System.Web.UI.Control), this.GetType(), "alert", message2, true);
                return;
            }
            string nota = txtNota.Text + "%";
            int id_reg = Convert.ToInt32(txtIdReg.Text);
            int id = Convert.ToInt32(txtId.Text);
            string fecha = DateTime.Today.ToString("yyyy-MM-dd");
            actualizarEva(fecha, nota, id_reg, id);
            string message = "alert('Evaluación Modificada')";
            ScriptManager.RegisterClientScriptBlock((sender as System.Web.UI.Control), this.GetType(), "alert", message, true);
            LimpiarControles();
        }
        catch (Exception ex) { }
    }

    /**
     * genera el script para la actualisacion de la nota en caso de modificacion
     * @param fecha: fecha del dia en el que se realizo la modificacion
     * @param nota: la nueva nota ingresada para el alumno
     * @param id_reg: id del registro del curso del alumno
     * @id: id: id de la nota dentro de la tabla datos
     * @throw en caso que la conexion con la base de datos falle
     * */
    public void actualizarEva(string fecha, string nota, int id_reg, int id)
    {
        try
        {
            string strSQL = "UPDATE curso_datos set fecha='" + fecha + "',nota='" + nota + "',id_reg='" + id_reg + "' WHERE id=" + id + ";";
            Namespace.Conexion.update(strSQL);
            Namespace.Conexion.Desconectar();
        }
        catch (Exception ex)
        {
            Debug.Print("no se pudo actualizar la nota : " + ex);
        }
    }

    /**
     * maneja la seleccion de los cursos mostrados en el griview curso
     * al selecionar un curso llama al llenado del calendario como tambien los botones para realizar la busqueda
     * */
    protected void gvCurso_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            Int16 num = Convert.ToInt16(e.CommandArgument);
            txtCurso.Text = HttpUtility.HtmlDecode(gvCurso.Rows[num].Cells[1].Text);
            int id =  Convert.ToInt32(gvCurso.Rows[num].Cells[3].Text);
            llenarCal(id);
            gvCurso.Visible = false;
            ddlIngreso.Enabled = true;
        }
    }

    /**
     * almacena las notas de todos los alumnos y sus datos en arraylist, para luego recorrerlos
     * e ir almacenando cada uno en la bsae de datos
     * @throw en caso que falle la conexion con la base de datos
     * nota: utiliza un sistema de confirm en javascript para manejar la confirmacion del ingreso
     * */
    protected void agregarNotas_Click(object sender, EventArgs e)
    {
        
        string confirmValue = Request.Form["confirm_value"];
        if (confirmValue == "Si")
        {
            string curso = txtCurso.Text;
            string fecha = calMulti.SelectedDate.ToString();
            DateTime fechaAux = Convert.ToDateTime(fecha);
            fecha = fechaAux.ToString("yyyy/MM/dd");
            ArrayList notas = new ArrayList();
            ArrayList nombres = new ArrayList();
            ArrayList apellidoP = new ArrayList();
            ArrayList apellidoM = new ArrayList();
            ArrayList rut = new ArrayList();
            ArrayList id = new ArrayList();
            ArrayList fail = new ArrayList();
            fail.Add("1");
            string nota;
            
            for (int i = 0; i < gvListarAlumnos.Rows.Count; i++)
            {
                TextBox dato = (TextBox)(gvListarAlumnos.Rows[i].Cells[0].Controls[1]);
                nota = dato.Text;
                notas.Add(nota);

                nombres.Add(gvListarAlumnos.Rows[i].Cells[2].Text);

                apellidoP.Add(gvListarAlumnos.Rows[i].Cells[3].Text);

                apellidoM.Add(gvListarAlumnos.Rows[i].Cells[4].Text);

                rut.Add(gvListarAlumnos.Rows[i].Cells[5].Text);
                
            }
            try
            {
                for (int i = 0; i < rut.Count; i++)
                {
                    var Reader = Namespace.Conexion.reader("SELECT id from curso_registros where rut = '" + rut[i] + "' AND curso = '" + curso + "' AND fechaCurso ='" + fecha + "'  ;");
                    using (Reader)
                    {
                        if (Reader.Read())
                        {
                            id.Add(Reader["id"].ToString());
                        }
                    }
                    Reader.Close();
                    Namespace.Conexion.Desconectar();
                }
            }
            catch(Exception ex)
            {
                Debug.Print("error al obtener las id" + ex);
            }
            for (int i = 0; i < gvListarAlumnos.Rows.Count; i++)
            {
                int aux = -1;
                if (int.TryParse(notas[i].ToString(), out aux) )
                {
                    if (aux > 0 || aux <= 100)
                    {
                        string nuevoNombre = nombres[i] + " " + apellidoP [i] + " " + apellidoM[i];
                        insertarEval(nuevoNombre, fecha, rut[i].ToString(), notas[i].ToString()+"%", Convert.ToInt32(id[i]));
                    }
                    else
                    {
                        fail.Add(rut[i]);
                    }
                }
                else
                {
                    fail.Add(rut[i]);
                }
               
            }
            string confirmValue1 = Request.Form["confirm_value1"];
            int count = 0;
            if ( CBaprovar.Checked)
            {
                ArrayList asistencia = new ArrayList();
                string asistenAUX;
                for (int i = 0; i<gvListarAlumnos.Rows.Count; i++)
                {
                    TextBox dato2 = (TextBox)(gvListarAlumnos.Rows[i].Cells[1].Controls[1]);
                    asistenAUX = dato2.Text;
                    asistencia.Add(asistenAUX);
                }
               
                for (int i = 0; i < gvListarAlumnos.Rows.Count; i++)
                {
                    if (rut[i].ToString() != fail[count].ToString())
                    {
                       
                        int aux = -1;
                        if (int.TryParse(asistencia[i].ToString(), out aux))
                        {
                            if (aux > 0 || aux <= 100)
                            {
                                string sig = null;
                                if (curso == "Primavera P6 Básico")
                                {
                                    sig = "P6B";
                                }
                                if (curso == "Primavera Risk")
                                {
                                    sig = "PRA";
                                }
                                if (curso == "Primavera P6 Avanzado")
                                {
                                    sig = "P6A";
                                }
                                if (sig != null)
                                {
                                    DateTime fechaHoy = DateTime.Now.Date;
                                    fecha = fechaHoy.ToString("yyyy/MM/dd");
                                    string fecha2 = fechaHoy.ToString("dd/MM/yyyy");
                                    string año = fechaHoy.ToString("yy");
                                    string codigo = null;
                                    for (int j = 0; j < 1;)
                                    {
                                        codigo = "MTC" + año + sig + CreateRandomCode();
                                        bool value = codigoExistente(codigo);
                                        if (value == false)
                                        {
                                            j++;
                                            insertarCodigo(codigo, rut[i].ToString());
                                        }
                                    }
                                
                                    actualizarRegistro(rut[i].ToString(), Convert.ToInt32(id[i]), codigo, Convert.ToInt32(asistencia[i]));
                                }
                                else
                                {
                                    fail.Add(rut[i]);
                                    count++;
                                }
                            }
                            else
                            {
                                fail.Add(rut[i]);
                                count++;
                            }
                        }
                    }
                }
            }

            if(fail != null)
            {
                string mensaje = null;
                for(int i=0; i<fail.Count; i++)
                {
                    if (i != 0)
                    {
                        mensaje = "\\n" + mensaje + fail[i] + "\\n";
                        Debug.Print("los rut son : " + fail[i]);
                    }
                }
                if (fail.Count > 1)
                {
                    Debug.Print("no entre");
                    string message2 = "alert('Los siguientes Rut no se ingresaron" + mensaje + "')";
                    ScriptManager.RegisterClientScriptBlock((sender as System.Web.UI.Control), this.GetType(), "alert", message2, true);
                }

            }
        }
        LimpiarControles();
        txtCurso.Text = string.Empty;
        ddlIngreso.SelectedIndex = 0;
        txtFechas.Text = string.Empty;
        boxEmpresa.Text = string.Empty;
        bReinicio.Visible = false;
        ddlEmpresas.Enabled = false;
        ImageCalendario.Enabled = false;
        tr14.Visible = false;
        buscarAlumnos.Visible = true;
        errorVacio.Visible = false;
        calMulti.Visible = false;
        agregarNotas.Visible = false;
        buscarAlumnos.Visible = false;
        gvListarAlumnos.Visible = false;
    }

    /**
     * busca todos los alumnos que cumplan con los datos ingresados en los txt
     * que esten sin nota y en un estado de pendiente, para luego su ingreso de evaluacion. En caso que 
     * el alumno sea de una empresa se crea un segundo filtro en caso de error
     * @throw en caso que falle la conexion con la base de datos
     * */
    protected void buscarAlumnos_Click(object sender, EventArgs e)
    {

        string curso = txtCurso.Text;
        string fecha = calMulti.SelectedDate.ToString();
        DateTime fechaAux = Convert.ToDateTime(fecha);
        fecha = fechaAux.ToString("yyyy/MM/dd");
        if (ddlIngreso.SelectedIndex == 1)
        {
            try
            {
                if (gvListarAlumnos.Visible == false)
                {
                    var adp = Namespace.Conexion.adapter("SELECT A.nombres AS 'Nombre', A.apellidoP AS 'Apellido Paterno', A.apellidoM AS 'Apellido Materno', A.rut AS 'Rut', B.estado AS 'Estado', A.telefono AS 'Telefono', DATE_FORMAT(B.fechaCurso,'%d/%m/%Y') AS 'Fecha de Inicio' FROM curso_cliente A, curso_registros B where A.rut = B.rut AND B.curso = '" + curso + "' AND B.fechaCurso ='" + fecha + "' AND B.estado = 'Pendiente' &&  not exists (select nota from sgsmtc2.curso_datos where curso_datos.id_reg = B.id)  ;");
                    using (adp)
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        adp.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            gvListarAlumnos.Visible = true;
                            gvListarAlumnos.DataSource = null;
                            gvListarAlumnos.DataBind();
                            gvListarAlumnos.DataSource = dt;
                            gvListarAlumnos.DataBind();
                        }
                        else
                        {
                            bReinicio.Visible = true;
                            buscarAlumnos.Visible = false;
                            CBaprovar.Visible = false;
                            errorVacio.Visible = true;
                            return;
                        }
                    }

                    gvListarAlumnos.Visible = true;
                    buscarAlumnos.Visible = false;
                    agregarNotas.Visible = true;
                    CBaprovar.Visible = false;
                    gvListarAlumnos.HeaderRow.Cells[0].Text = "Nota";
                    gvListarAlumnos.HeaderRow.Cells[1].Text = "Asistencia";
                    if (!CBaprovar.Checked)
                    {
                        gvListarAlumnos.Columns[1].Visible = false;
                    }
                }
                else { gvListarAlumnos.Visible = false; }
            }
            catch (Exception ex)
            {
                Debug.Print("Error en la busqueda de alumnos sin nota: " + ex);
            }
        }
        if (ddlIngreso.SelectedIndex == 2)
        {
            string empresa = boxEmpresa.Text;
            try
            {
                if (gvListarAlumnos.Visible == false)
                {
                    var adp = Namespace.Conexion.adapter("SELECT A.nombres AS 'Nombre', A.apellidoP AS 'Apellido Paterno', A.apellidoM AS 'Apellido Materno', A.rut AS 'Rut', B.estado AS 'Estado', A.telefono AS 'Telefono', DATE_FORMAT(B.fechaCurso,'%d/%m/%Y') AS 'Fecha de Inicio' FROM curso_cliente A, curso_registros B where A.rut = B.rut AND B.curso = '" + curso + "' AND B.fechaCurso ='" + fecha + "' AND B.estado = 'Pendiente' AND nomEmpresa = '"+empresa+ "' &&  not exists (select nota from sgsmtc2.curso_datos where curso_datos.id_reg = B.id) ;");
                    using (adp)
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        adp.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            gvListarAlumnos.Visible = true;
                            gvListarAlumnos.DataSource = null;
                            gvListarAlumnos.DataBind();
                            gvListarAlumnos.DataSource = dt;
                            gvListarAlumnos.DataBind();
                        }
                        else
                        {
                            bReinicio.Visible = true;
                            errorVacio.Visible = true;
                            buscarAlumnos.Visible = false;
                            CBaprovar.Visible = false;
                            return;
                        }
                    }
                    gvListarAlumnos.Visible = true;
                    buscarAlumnos.Visible = false;
                    agregarNotas.Visible = true;
                    CBaprovar.Visible = false;
                    gvListarAlumnos.HeaderRow.Cells[0].Text = "Nota";
                    gvListarAlumnos.HeaderRow.Cells[1].Text = "Asistencia";
                    if (!CBaprovar.Checked)
                    {
                        gvListarAlumnos.Columns[1].Visible = false;
                    }
                }
                else { gvListarAlumnos.Visible = false; }
            }
            catch (Exception ex)
            {
                Debug.Print("Error en la busqueda de alumnos sin nota: " + ex);
            }
        }

    }

    /**
     * boton
     * muestra en un gridview todos los cursos inscritos en la base de datos
     * para luego seleccionar uno de ellos
     * @throw en caso que falle la conexion con la base de datos
     * */
    protected void imgCursos_Click(object sender, ImageClickEventArgs e)
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
                        gvCurso.DataSource = null;
                        gvCurso.DataSource = dt;
                        gvCurso.DataBind();
                        gvCurso.Visible = true;
                    }
                }
               
            }
            else { gvCurso.Visible = false; }
        }
        catch (Exception ex) { }
    }

    /**
     *DropDownList
     * Selecciona si el usuario es particular o bajo una empresa
     * */
    protected void ddlIngreso_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlIngreso.SelectedIndex != 2 && tr14.Visible == true)
        {
            tr14.Visible = false;
        }
        if (ddlIngreso.SelectedIndex != 0)
        {
            ImageCalendario.Enabled = true;
            
        }
        if ( ddlIngreso.SelectedIndex == 2)
        {
            tr14.Visible = true;
            boxEmpresa.Visible = true;
           
        }
       
    }

    /**
     * boton
     * muestra el calendario o lo desaparece, se utiliza al momento de ingresar un registro 
     */
    protected void imgCalendario_Click(object sender, ImageClickEventArgs e)
    {
        if (calMulti.Visible == true)
        {
            calMulti.Visible = false;
            lblDerror.Visible = false;
        }
        else
        {
            calMulti.Visible = true;
            lblDerror.Visible = false;
        }
    }

    /**
     * funcion que se encarga ver las fechas seleccionadas en el calendario, como tambien ver si cumple que los requisitos
     * que las fechas sean del curso asignado, y la cantidad de dias asignados.
     * throw en caso que ocurra un error al obtener las fechas del calendario, el sistema se reinicia.
     * */
    protected void calMulti_DateChanged(object sender, EventArgs e)
    {
        
        int cont = calMulti.SelectedDates.Count;
        if(cont == 1)
        {
            int id=0;
            string fecha = calMulti.SelectedDate.ToString();
            DateTime fechaAux = Convert.ToDateTime(fecha);
            txtFechas.Text = fechaAux.ToString("dd/MM/yyyy");
            ImageCalendario.Enabled = false;
            if (txtCurso.Text == "Primavera P6 Básico")
            {
               id = 1;
            }
            if (txtCurso.Text == "Primavera Risk")
            {
                id = 10;
            }
            if (txtCurso.Text == "Primavera P6 Avanzado")
            {
              id = 12;
            }
            if (validarFecha(fecha, id))
            {
                calMulti.AllowSelectRegular = false;
                calMulti.Visible = false;
                tr18.Visible = true;
                CBaprovar.Visible = true;
                buscarAlumnos.Visible = true;
            }
        }
        else
        {
            string fecha = calMulti.SelectedDate.ToString();
            DateTime fechaAux = Convert.ToDateTime(fecha);
            txtFechas.Text = fechaAux.ToString("dd/MM/yyyy");
            calMulti.SelectedDates.Clear();
            int id = 0;
            if (txtCurso.Text == "Primavera P6 Básico")
            {
                id = 1;
            }
            if (txtCurso.Text == "Primavera Risk")
            {
                id = 10;
            }
            if (txtCurso.Text == "Primavera P6 Avanzado")
            {
                id = 12;
            }
            if (validarFecha(fecha, id))
            {
                calMulti.AllowSelectRegular = false;
                calMulti.Visible = false;
                tr18.Visible = true;
                CBaprovar.Visible = true;
            }
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
            return false;
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
                var adp = Namespace.Conexion.adapter("SELECT fechaCurso AS fecha FROM curso_registros WHERE id_curso=1");
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
                var adp2 = Namespace.Conexion.adapter("SELECT fechaCurso AS fecha FROM curso_registros WHERE id_curso= 10");
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
                var adp3 = Namespace.Conexion.adapter("SELECT fechaCurso AS fecha FROM curso_registros WHERE id_curso= 12");
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
        }
        catch (Exception ex) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('" + ex + "')", true); }
        return validador;
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
        }
        catch(Exception ex)
        {
            Debug.Print("no se pudo insertar el codigo : " + ex);
        }
    }

    /**
     * crea el script con el nuevo registro en la base de datos, agregando el codigo y la asistencia al usuario.
     * este solo se utiliza cuando se activa la opcion de ingreso de notas rapido
     * @param rut: el rut del cliente
     * @param id: id del curso registrado
     * @para codigo: codigo generado, el cual se utiliza mas adelante para obtener los certificados y diplomas
     * @param asistencia: la asistencia del alumno dentro del curso
     * @throw en caso que la base de datos este desconectada
     * */
    public void actualizarRegistro( string rut, int id, string codigo, int asistencia )
    {
        string strSQL = "UPDATE curso_registros set codigoA  ='"+codigo+ "',  asistencia  = " + asistencia+", estado  = 'Aprobado'  WHERE id="+id+" AND rut = '"+rut+"' ";
        try
        {
            Namespace.Conexion.update(strSQL);
            Namespace.Conexion.Desconectar();
        }
        catch (Exception ex)
        {
            Debug.Print("el error acutalizar el registro es : " + ex);
        }
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
            string nomEmp = "";
            nomEmp = boxEmpresa.Text;
            var Reader = Namespace.Conexion.reader("SELECT Crut FROM cliente_empresa WHERE Crazon_social='" + nomEmp + "'");
            string val = "";
            using (Reader)
            {
                if (Reader.Read())
                {
                    val = Reader["Crut"].ToString();
                   
                }
                else
                {
                    //lblRutEmpresa.Text = "Esta empresa no esta registrada";
                }
            }
            Reader.Close();
            Namespace.Conexion.Desconectar();
        }
        catch (Exception ex)
        {
            Debug.Print("se callo el reader: " + ex);
        }
    }

    /**
     * se utiliza para generar una lista de las empresas registradas en la bsae de datos y estas se puedan ir filtrando en tiempo real
     * cuando se van ingresando a un cliente.
     * @param frefitext: texto que se esta escribiendo en la casilla y se esta revisando constantemente para luego reenviar los posibles resultados a lo que esta escrito
     * @param count; cantidad de empresas inscritas en el arraylist existente
     * @return: retorna un array string con los posibles resultados de la busqueda
     * */
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        try
        {
            return (from m in nombresEmp2 where m.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select m).Take(count).ToArray();
        }
        catch (Exception ex)
        {
            Debug.Print("fallo buscar el curso : " + ex);
            return null;
        }
    }

    /**
     * boton
     * limpia los controles (txt,label, grid,etc) al momento de seleccionarlo
     * */
    protected void limpiar_CLick(object sender, EventArgs e)
    {
        LimpiarControles();
        txtCurso.Text = string.Empty;
        ddlIngreso.SelectedIndex = 0;
        txtFechas.Text = string.Empty;
        boxEmpresa.Text = string.Empty;
        bReinicio.Visible = false;
        ddlEmpresas.Enabled = false;
        ImageCalendario.Enabled = false;
        tr14.Visible = false;
        buscarAlumnos.Visible = true;
        errorVacio.Visible = false;
        calMulti.Visible = false;
        
    }

}
