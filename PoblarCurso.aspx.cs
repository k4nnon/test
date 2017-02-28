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
using System.Windows.Forms;
using System.Configuration;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
public partial class PoblarCurso : System.Web.UI.Page
{
    static MySqlConnection connection = new MySqlConnection();
    static MySqlCommand Comando = new MySqlCommand();
    static MySqlDataAdapter Adaptador = new MySqlDataAdapter();
    static MySqlDataReader Reader;
    static string mysqlConnString = "Server=192.168.1.7; Database=sgsmtc; userid=root; password='s0p0rte'; ";
    int contador=0;
    bool encontrado;
    string[,] id ;
    int P = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        iniciarConexion();
        if (!IsPostBack) { BindCursos(); }
        
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }
    protected void txtRut_TextChanged(object sender, EventArgs e)
    {
        lblCompletos2.Visible = false;
        string rut = txtRut.Text;
        txtRut.Text = Rut(rut);
        string rut2 = txtRut.Text;
        bool val=rutExistente(rut2);
        if(val==true){
        lblRut.Visible = false;
        ImageButton1.Enabled = true;
        }
        else 
        { 
            lblRut.Visible = true;
            ImageButton1.Enabled = false;
            gvCurso.Visible = false;
        }
    }
    public class AutoCompleClass
    {
        
        public System.Data.DataTable Datos()
        {
            using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
            {
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT rut FROM curso_cliente", cn);
                System.Data.DataTable dt = new System.Data.DataTable();
                adp.Fill(dt);

                return dt;
            }
        }

        public AutoCompleteStringCollection Autocomplete()
        {
            System.Data.DataTable dt = Datos();

            AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
            foreach (DataRow row in dt.Rows)
            {
                coleccion.Add(Convert.ToString(row["rut"]));
            }

            return coleccion;
        }
    }
    public bool rutExistente(string rut)
    {
        bool validador = false;
        try
        {
            iniciarConexion();
            Comando.CommandText = "SELECT * FROM curso_cliente WHERE rut='" + rut + "'";
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
    private void BindCursos()
    {
        try
        {
            using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
            {
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT curso , direccion , id_curso FROM curso_creado", cn);
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
    public void obtenerID(int i) 
    {
    int x = 0;
    int [,] id=new int[i,x];
    
    }
    protected void gvLista_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblCompletos2.Visible = false;
            if(gvCurso.Visible==false){
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
                calBD.Visible = false;
                btnRegistro.Visible = false;
                txtDias.Enabled = true;
                BindCursos();
                Int16 num = Convert.ToInt16(e.CommandArgument);
                string t2, t6;

                txtCurso.Text =HttpUtility.HtmlDecode(gvCurso.Rows[num].Cells[1].Text);
                lblCursoId.Text = gvCurso.Rows[num].Cells[3].Text;
                int id = Convert.ToInt32(lblCursoId.Text);
                llenarCal(id);
            }
            catch (Exception ex) { }
            gvCurso.Visible = false;
        }
    }
    protected void btnRegistro_Click(object sender, EventArgs e)
    {
        if (txtRut.Text != string.Empty)
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
                string rut = txtRut.Text;
                string curso = txtCurso.Text;
                string estado = "Pendiente";
                string fechaCurso = fechas3();
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
                        ScriptManager.RegisterClientScriptBlock((sender as System.Web.UI.Control), this.GetType(), "alert", message, true);

                    }
                    else
                    {
                        string message = "alert('Registro Agregado')";
                        ScriptManager.RegisterClientScriptBlock((sender as System.Web.UI.Control), this.GetType(), "alert", message, true);
                        insertarRegistro(rut, curso, fechaCurso, fecha, null, estado, fechas, ids, idCurso, asistencia);
                        txtIngresados.Text += "\n \t" + "Rut:" + rut + "\n \t" + "Curso:" + curso + "\n \t" + "Fecha curso:" + fechaCurso + "\n \t";
                        txtRut.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {

                string myStringVariable = "Error en Ingreso ";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + ex + "');", true);
            }
        }
        else
        {
            string message = "alert('Es necesario un rut para el ingreso')";
            ScriptManager.RegisterClientScriptBlock((sender as System.Web.UI.Control), this.GetType(), "alert", message, true);


        }
    }
    public void insertarRegistro(string val1, string val2, string val3, string val4, string val5, string val6,string val7,string val8,int val9,int val10)
    {
        iniciarConexion();
    string strSQL = "INSERT INTO curso_registros VALUES('" + val1 + "','" + val2 + "','" + val3 + "','" + val4 + "','" + val5 + "','" + val6 + "'," + 0 + ",'"+val7+"','"+val8+"',"+val9+","+val10+");";
        Comando.CommandText = strSQL;
        Comando.Connection = connection;
        Comando.ExecuteNonQuery();
    }
    public string fechas()
    {
        int ve=Convert.ToInt32(lblCursoId.Text);
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

    protected void Button1_Click(object sender, EventArgs e)
    {
        calMulti.SelectedDates.Clear();
        calMulti.SelectedDates = null;
        calMulti.DatePickerImagePath = null;
        this.calMulti.SelectedDate = new DateTime();
        fechasInversas();
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
            for (int i = 0; i < array.Length - 3; i++)
            {
                fe = Convert.ToDateTime(array[i].ToString());
                calMulti.SelectedDates.Add(fe);
            }
        }
        catch (Exception ex) { }
    }
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
                if(con==con2 && P==1)
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
                    btnRegistro.Visible = true;
                    calMulti.AllowSelectRegular = false;
                    txtFecha.Text = fechas();
                    lblCompletos.Visible = true;
                    calMulti.Visible = false;
                    calBD.Visible = false;
                }
                else
                {
                    btnRegistro.Visible = false;
                    txtFecha.Text = string.Empty;
                    calMulti.AllowSelectRegular = true;
                    lblCompletos.Visible = false;
                }
                if (con2 > con)
                {
                    btnRegistro.Visible = false;
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
    public bool validarFecha(string fecha, int n)
    {
        try
        {
            DateTime fr= Convert.ToDateTime(fecha);
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
                    validador=true;
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
                    lblIdFecha.Text += Convert.ToString(dt.Rows[0][0].ToString())+";";
                    lblDerror.Visible = false;
                }

            }

        }
        catch (Exception ex)
        {
        }

    }
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        lblCompletos2.Visible = false;
        if (calMulti.Visible == true)
        {
            calMulti.Visible = false;
            calBD.Visible = false;
        }
        else
        {
            calMulti.Visible = true;
            calBD.Visible = true;
        }
    }
    protected void txtDias_TextChanged1(object sender, System.EventArgs e)
    {
        try
        {
            lblCompletos2.Visible = false;
            calMulti.Visible = false;
            calBD.Visible = false;
            calMulti.AllowSelectRegular = true;
            calMulti.SelectedDates = null;
            calMulti.DatePickerImagePath = null;
            this.calMulti.SelectedDate = new DateTime();
            calMulti.SelectedDates.Clear();
            lblCompletos.Visible = false;
            txtFecha.Text = string.Empty;
            btnRegistro.Visible = false;
            int va = Convert.ToInt32(txtDias.Text);
            if (va > 0)
            {
                ImageButton2.Enabled = true;
            }
            else { ImageButton2.Enabled = false; calMulti.Visible = false; }
        }
        catch (Exception ex) { }
    }
    protected void txtFecha_TextChanged(object sender, System.EventArgs e)
    {

    }
    public void llenarCal(int id)
    {
        try      
        {
            iniciarConexion();
            using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
            {
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT fecha FROM curso_fechas WHERE id_curso="+id, cn);
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
    protected void txtCurso_TextChanged(object sender, System.EventArgs e)
    {
       
    }
    protected void DropDownList1_SelectedIndexChanged1(object sender, EventArgs e)
    {

    }
    protected void gvCurso_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        txtDias.Enabled = false;
        calMulti.AllowSelectRegular = true;
        txtRut.Text = string.Empty;
        txtCurso.Text = string.Empty;
        gvCurso.Visible = false;
        txtDias.Text = string.Empty;
        txtFecha.Text = string.Empty;
        ImageButton2.Enabled = false;
        txtFecha.Text = string.Empty;
        lblCompletos.Visible = false;
        calBD.Visible = false;
        calMulti.Visible = false;
        calBD.SelectedDates = null;
        calBD.DatePickerImagePath = null;
        this.calBD.SelectedDate = new DateTime();
        calBD.SelectedDates.Clear();
        calMulti.SelectedDates = null;
        calMulti.DatePickerImagePath = null;
        this.calMulti.SelectedDate = new DateTime();
        calMulti.SelectedDates.Clear();
     
    }
    protected void DropDownList1_SelectedIndexChanged2(object sender, EventArgs e)
    {

    }

    protected void Button1_Click1(object sender, EventArgs e)
    {
        txtIngresados.Text = string.Empty;
        txtDias.Enabled = false;
        calMulti.AllowSelectRegular = true;
        txtRut.Text = string.Empty;
        txtCurso.Text = string.Empty;
        gvCurso.Visible = false;
        txtDias.Text = string.Empty;
        txtFecha.Text = string.Empty;
        ImageButton2.Enabled = false;
        txtFecha.Text = string.Empty;
        lblCompletos.Visible = false;
        calBD.Visible = false;
        calMulti.Visible = false;
        calBD.SelectedDates = null;
        calBD.DatePickerImagePath = null;
        this.calBD.SelectedDate = new DateTime();
        calBD.SelectedDates.Clear();
        calMulti.SelectedDates = null;
        calMulti.DatePickerImagePath = null;
        this.calMulti.SelectedDate = new DateTime();
        calMulti.SelectedDates.Clear();
        btnRegistro.Visible = false;
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
}