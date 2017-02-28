using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using System.Web.Script.Serialization;

public partial class PoblarCurso : System.Web.UI.Page
{
    /**
    * @autor: 
    * @co autor: gonzalo zeballos. mail: maxpayne_ga@hotmail.com
    * @Sistema de toma de horas, generación de certificados y administración de cliente; dentro de los cursos Primavera
    * */

    /**
      * id = almacena el id de los cursos ingresados en la base de datos
      * encontrado = booleano que entrega resultado de si la fecha seleccionada cumple con los requisitos del registro.
      * values = corresponde a todos los rut almacenados para ser ingresados al curso
     * */

    bool encontrado;
    string[,] id;
    protected string Values;
    /**
     *  Al iniciar la pagina, se cargaran cursos de la base de datos.
     * */
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) { BindCursos(); }
    }

    //no se usa
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    /**
     * valida el rut y si este cumple con requisitos, se reescribe con el formato que la base de datos acepta
     * @param rut: el rut el cual sera modificado y validado
     * @return string con el rut modificado
     * */
    protected string rutChange(string rut)
    {
        rut = Rut(rut);
        bool val = rutExistente(rut);
        if (val == true)
        {
            return rut;
        }
        else
            return null;
    }


    //public class AutoCompleClass
    //{

    //    public System.Data.DataTable Datos()
    //    {
    //        var adp = Namespace.Conexion.adapter("SELECT rut FROM curso_cliente");
    //        using (adp)
    //        {
    //            System.Data.DataTable dt = new System.Data.DataTable();
    //            adp.Fill(dt);

    //            return dt;
    //        } 

    //    }

    //    public AutoCompleteStringCollection Autocomplete()
    //    {
    //        System.Data.DataTable dt = Datos();

    //        AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
    //        foreach (DataRow row in dt.Rows)
    //        {
    //            coleccion.Add(Convert.ToString(row["rut"]));
    //        }

    //        return coleccion;
    //    }
    //}


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

    /**
     * busca los cursos en la base de datos y almacena sus id
     * @throw en caso que la conexion con la base de datos falle
     * */
    private void BindCursos()
    {
        try
        {
            var adp = Namespace.Conexion.adapter("SELECT curso , direccion , id_curso FROM curso_creado");
            using (adp)
            {

                System.Data.DataTable dt = new System.Data.DataTable();
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {

                    /*ddlCurso.DataSource = dt;
                     ddlCurso.DataTextField = "curso";
                     ddlCurso.DataBind();*/
                    int i = 0;

                    id = new string[i, 2];
                }
            }
        }
        catch (Exception ex) { lblError.Text = ex.Message; lblError.Visible = true; }
    }

    /**
     * obtiene el del curso
     * @param i: numero de la ubicacion del id a buscar
     * */
    public void obtenerID(int i)
    {
        int x = 0;
        int[,] id = new int[i, x];

    }

    //no se usa
    protected void gvLista_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    /**
     * boton
     * se obtiene la lista de cursos en la base de datos y se guarda en un gridview
     * @throw en caso que la coneccion con la base de datos falle
     * */
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblCompletos2.Visible = false;
            if (gvCurso.Visible == false)
            {
                var adp = Namespace.Conexion.adapter("SELECT curso, direccion ,id_curso FROM curso_creado");
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
        catch (Exception ex) { }
    }

    /**
     * maneja la seleccion del gridview lista el cual muestra los cursos y al momento de seleccionar uno
     * se obtiene los datos de este
     * @throw en caso que la coneccion de la base de datos falle
     */
    protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            try
            {
                lblCompletos2.Visible = false;
                lblCompletos.Visible = false;
                calMulti.SelectedDates = null;
                calMulti.DatePickerImagePath = null;
                this.calMulti.SelectedDate = new DateTime();
                calMulti.SelectedDates.Clear();
                txtFecha.Text = string.Empty;
                calMulti.Visible = false;
                txtDias.Enabled = true;
                BindCursos();
                Int16 num = Convert.ToInt16(e.CommandArgument);
                txtCurso.Text = HttpUtility.HtmlDecode(gvCurso.Rows[num].Cells[1].Text);
                lblCursoId.Text = gvCurso.Rows[num].Cells[3].Text;
                int id = Convert.ToInt32(lblCursoId.Text);
                llenarCal(id);
            }
            catch (Exception ex) { }
            gvCurso.Visible = false;
        }
    }

    /**
     * inserta  el o los alumno  con los rut correspondiente y luego retorna un string con con un mensaje el cual se utilizara mas tarde
     * @param rut: rut del alumno que sera ingresado al curso
     * @return retorna un string que posee el mensaje de si el alumno pudo o no ser ingresado
     * @throw en caso que la base de datos falle
     * */
    protected string insertarRut(string rut)
    {
        if (rut != null)
        {
            try
            {

                string año = string.Empty;
                string fecha = string.Empty;
                DateTime fechaHoy = DateTime.Now.Date;
                fecha = fechaHoy.ToString("yyyy/MM/dd");
                string fecha2 = fechaHoy.ToString("dd/MM/yyyy");
                string fechas = txtFecha.Text;
                string ids = lblIdFecha.Text;
                int idCurso = Convert.ToInt32(lblCursoId.Text);

                string curso = txtCurso.Text;
                string estado = "Pendiente";
                string fechaCurso = fechas3();
                int asistencia = 0;

                System.Data.DataTable dt = new System.Data.DataTable();
                var adp = Namespace.Conexion.adapter("SELECT * FROM curso_registros WHERE rut='" + rut + "' AND id_curso=" + idCurso + " AND id_fechas='" + ids + "'");
                using (adp)
                {
                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        string message = "Este rut " + rut + " contiene un registro en ese curso y en las mismas fechas.";
                        return message;
                    }
                    else
                    {
                        insertarRegistro(rut, curso, fechaCurso, fecha, null, estado, fechas, ids, idCurso, asistencia);
                        txtIngresados.Text += "\n \t" + "Rut:" + rut + "\n \t" + "Curso:" + curso + "\n \t" + "Fecha curso:" + fechaCurso + "\n \t";
                    }
                }
            }
            catch (Exception ex)
            {
                string myStringVariable = "Error en Ingreso ";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + ex + "');", true);
            }
        }
        return null;
    }

    /**
     * se crea el script con los datos del usuario que sera ingresado al curso
     * @param rut: rut del cliente
     * @param curso: curso al cual esta siendo ingresado
     * @param fechaCurso: fecha de inicio del curso
     * @param fecha : fecha de ingreso del registro
     * @param codigo: codigo es null, el cual es el codigo de validacion de los certificados y diplomas
     * @param estado: estado del usuario en el curso, en este caso es pendiente
     * @param fechas: es el conjunto de fechas del curso en un string y separados por ;
     * @param ids: son los id de las fechas del curso
     * @param idCurso: es el id del curso al cual sera registrado
     * @param asistencia: es la asistencia dentro del curso, que en este caso coressponde a 0
     * @throw en caso que la base de datos este desconectada
     * */
    public void insertarRegistro(string rut, string curso, string fechaCurso, string fecha, string codigo, string estado, string fechas, string ids, int idCurso, int asistencia)
    {
        try
        {
            string strSQL = "INSERT INTO curso_registros VALUES('" + rut + "','" + curso + "','" + fechaCurso + "','" + fecha + "','" + codigo + "','" + estado + "'," + 0 + ",'" + fechas + "','" + ids + "'," + idCurso + "," + asistencia + ");";
            Namespace.Conexion.insert(strSQL);
            Namespace.Conexion.Desconectar();
        }
        catch (Exception ex)
        {
            Debug.Print("ocurrio un error al ingresar el alumno :" + rut + "el error es ; " + ex);
        }
    }

    /**
     * prepara las fechas para ser ingresadas a la base de datos, como tambien obtiene la id de estas
     * ademas dependiendo de la opcion ingresada las fechas se ordenan de diferente forma
     * @return string con las fechas adaptadas para su almacenado o uso
     * @throw en caso que la conversion o modificacion genera un campo vacio o similar
     * */
    public string fechas()
    {
        int ve = Convert.ToInt32(lblCursoId.Text);
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
        for (int i = 0; i < selectedDates.Count; i++)
        {
            fe2 = list2[i].ToString("yyyy/MM/dd");
            obtenerID(fe2, ve);
            selectedDatesString += fe2 + ";";
        }
        return selectedDatesString.ToString();
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
     * Rellena el calendario con las fechas ingresadas en la base de datos
     * @param palabra: es la lista de fechas ingresadas, las cuales se utilizaran para rellanar el calendario
     * throw, en caso que no se pueda acceder al calendario.
     * */
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
            for (int i = 0; i < array.Length; i++)
            {
                fe = Convert.ToDateTime(array[i].ToString());
                calMulti.SelectedDates.Add(fe);
            }
        }
        catch (Exception ex) { }
    }

    /**
  * prepara las fechas para ser ingresadas a la base de datos, como tambien obtiene la id de estas
  * ademas dependiendo de la opcion ingresada las fechas se ordenan de diferente forma
  * @return string con las fechas adaptadas para su almacenado o uso
  * @throw en caso que la conversion o modificacion genera un campo vacio o similar
     */
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
            for (int i = 0; i < array.Length - 3; i++)
            {
                fe = Convert.ToDateTime(array[i].ToString());
                calMulti.SelectedDates.Add(fe);
            }
        }
        catch (Exception ex) { }
    }

    /**
     * funcion que se encarga ver las fechas seleccionadas en el calendario, como tambien ver si cumple que los requisitos
     * que las fechas sean del curso asignado, y la cantidad de dias asignados.
     * throw en caso que ocurra un error al obtener las fechas del calendario, el sistema se reinicia.
     * */
    protected void calMulti_DateChanged(object sender, System.EventArgs e)
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
                if (con == con2)
                {
                    lblCompletos2.Visible = false;
                }
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
                    //btnRegistro.Visible = true;
                    calMulti.AllowSelectRegular = false;
                    txtFecha.Text = fechas();
                    lblCompletos.Visible = true;
                    calMulti.Visible = false;

                }
                else
                {
                    //btnRegistro.Visible = false;
                    txtFecha.Text = string.Empty;
                    calMulti.AllowSelectRegular = true;
                    lblCompletos.Visible = false;
                }
                if (con2 > con)
                {
                    //btnRegistro.Visible = false;
                    txtFecha.Text = string.Empty;
                    lblCompletos2.Visible = true;
                    lblFechasResp.Text = fechas2();
                    fechasInversas4();
                    con2 = con2 - 2;
                }
            }
            catch (Exception ex)
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
        }

    }

    /**
     * boton
     * muestra el calendario o lo desaparece, se utiliza al momento de ingresar un registro 
     */
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        lblCompletos2.Visible = false;
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
     * actualisa la cantidad de dias maximos que se pueden ingresar en el calendario
     * @param mediante el txtdias recive la cantidad maxima de dias a aceptar
     * @throw en caso que exista algun problema al convertir los dias a int
     * */
    protected void txtDias_TextChanged1(object sender, System.EventArgs e)
    {
        try
        {
            lblCompletos2.Visible = false;
            calMulti.Visible = false;

            calMulti.AllowSelectRegular = true;
            calMulti.SelectedDates = null;
            calMulti.DatePickerImagePath = null;
            this.calMulti.SelectedDate = new DateTime();
            calMulti.SelectedDates.Clear();
            lblCompletos.Visible = false;
            txtFecha.Text = string.Empty;
            //btnRegistro.Visible = false;
            int va = Convert.ToInt32(txtDias.Text);
            if (va > 0)
            {
                ImageButton2.Enabled = true;
            }
            else { ImageButton2.Enabled = false; calMulti.Visible = false; }
        }
        catch (Exception ex) { }
    }

    //no se usa
    protected void txtFecha_TextChanged(object sender, System.EventArgs e)
    {

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
                        Debug.Print("error en el calendario es : " + e);
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
                Namespace.Conexion.Desconectar();
                calMulti.RemoveAllSpecialDates();
                var adp2 = Namespace.Conexion.adapter("SELECT fecha FROM curso_fechas WHERE id_curso= 10");
                System.Data.DataTable dt2 = new System.Data.DataTable();
                using (adp2)
                {
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
            }
            if (id == 12)
            {
                Namespace.Conexion.Desconectar();
                calMulti.RemoveAllSpecialDates();
                var adp3 = Namespace.Conexion.adapter("SELECT fecha FROM curso_fechas WHERE id_curso= 12");
                System.Data.DataTable dt3 = new System.Data.DataTable();
                using (adp3)
                {
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

        }
        catch (Exception ex) { }
    }

    //no se usa
    protected void txtCurso_TextChanged(object sender, System.EventArgs e)
    {

    }

    //no se usa
    protected void DropDownList1_SelectedIndexChanged1(object sender, EventArgs e)
    {

    }

    //no se usa
    protected void gvCurso_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    //no se usa
    protected void DropDownList1_SelectedIndexChanged2(object sender, EventArgs e)
    {

    }

    /**
  * limpia todos los label, reinicia los calendario, dropDownList,etc
  * */
    protected void Button1_Click1(object sender, EventArgs e)
    {
        txtIngresados.Text = string.Empty;
        txtDias.Enabled = false;
        calMulti.AllowSelectRegular = true;
        //txtRut.Text = string.Empty;
        txtCurso.Text = string.Empty;
        gvCurso.Visible = false;
        txtDias.Text = string.Empty;
        txtFecha.Text = string.Empty;
        ImageButton2.Enabled = false;
        txtFecha.Text = string.Empty;
        lblCompletos.Visible = false;
        calMulti.Visible = false;
        calMulti.SelectedDates = null;
        calMulti.DatePickerImagePath = null;
        this.calMulti.SelectedDate = new DateTime();
        calMulti.SelectedDates.Clear();
        //btnRegistro.Visible = false;
    }

    // no se usa
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }

    /**
     * ingrea todos los alumnos selecionados y se realiza un post registrandolos a todos en la base de datos.
     * @throw en caso que la base de datos tenga algun tipo de falla
     * */
    protected void Post(object sender, EventArgs e)
    {
        try
        {
            string[] textboxValues = Request.Form.GetValues("DynamicTextBox");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            this.Values = serializer.Serialize(textboxValues);
            string message = "";
            string nuevoRut;
            string alert = "";
            string aux = null;
            string aux2;
            Boolean flag = false;

            foreach (string textboxValue in textboxValues)
            {
                //message += textboxValue + "\\n";
                nuevoRut = rutChange(textboxValue);
                if (nuevoRut == null)
                {
                    aux2 = "\\nEl rut :" + Rut(textboxValue) + " no esta registrado ";
                    alert = alert + aux2;
                    flag = true;
                }
                aux = insertarRut(nuevoRut);

                if (aux != null)
                {
                    alert = alert + "\\n" + aux + " ";
                    flag = true;
                }
            }
            alert = "alert('" + alert + "')";

            if (flag)
            {
                ScriptManager.RegisterClientScriptBlock((sender as System.Web.UI.Control), this.GetType(), "alert", alert, true);
            }
            // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + message + "');", true);
            //string message = "alert('Registro Agregado')";
        }
        catch
        {
            string messages = "alert('Ingrese un RUT')";
            ScriptManager.RegisterClientScriptBlock((sender as System.Web.UI.Control), this.GetType(), "alert", messages, true);
        }
    }

}