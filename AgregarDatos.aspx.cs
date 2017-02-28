using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class AgregarDatos : System.Web.UI.Page
{
    static MySqlConnection connection = new MySqlConnection();
    static MySqlCommand Comando = new MySqlCommand();
    static MySqlDataAdapter Adaptador = new MySqlDataAdapter();
    static MySqlDataReader Reader;
    static string mysqlConnString = "Server=192.168.1.7; Database=sgsmtc; userid=root; password='s0p0rte'; ";
    protected void Page_Load(object sender, EventArgs e)
    {
        lblSinReg.Visible = false;
        lblCompletos.Visible = false;

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
    protected void ddlSeleccion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSeleccion.SelectedIndex == 0) { btnAgregar.Visible = true; btnModificar.Visible = false; imgId.Enabled = false; txtId.Enabled = false; LimpiarControles(); txtNota.ReadOnly = true; }
        if (ddlSeleccion.SelectedIndex == 1) { btnAgregar.Visible = false; btnModificar.Visible = true; imgId.Enabled = false; txtId.Enabled = false; LimpiarControles(); txtNota.ReadOnly = true; }
    }
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
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtRut.Text != string.Empty && txtNombre.Text != string.Empty && txtFecha.Text != string.Empty && txtNota.Text != string.Empty && txtIdReg.Text != string.Empty)
            {
                string nombre = txtNombre.Text;
                string fecha = txtFecha.Text;
                string rut = txtRut.Text;
                string nota = txtNota.Text + "%";
                int id_reg = Convert.ToInt32(txtIdReg.Text);
                insertarEval(nombre, fecha, rut, nota, id_reg);
                string message = "alert('Evaluación agregada')";
                ScriptManager.RegisterClientScriptBlock((sender as System.Web.UI.Control), this.GetType(), "alert", message, true);
                LimpiarControles();
            }
            else { }
        }
        catch (Exception ex) { }
    }
    public void LimpiarControles()
    {
        txtRut.Text = string.Empty;
        txtNombre.Text = string.Empty;
        txtId.Text = string.Empty;
        gvId.DataSource = null;
        gvId.DataBind();
        txtFecha.Text = string.Empty;
        txtNota.Text = string.Empty;
        txtIdReg.Text = string.Empty;
        gvIdRegistro.DataSource = null;
        gvIdRegistro.DataBind();
        imgId.Enabled = false;
        txtId.Enabled = false;
        Calendar1.Visible = false;
        imgFecha.Enabled = false; imgReg.Enabled = false; txtNota.ReadOnly = true;
    }
    public void insertarEval(string nombre, string fecha,string rut,string nota,int id_reg)
    {
        try
        {
            iniciarConexion();
            string strSQL = "INSERT INTO curso_datos VALUES('" + nombre + "','" + fecha + "','" + rut + "','" + nota + "'," + 0 + "," + id_reg + ");";
            Comando.CommandText = strSQL;
            Comando.Connection = connection;
            Comando.ExecuteNonQuery();
        }
        catch(Exception ex){}
    }
    public void buscarCliente(string rut)
    {
        if (ddlSeleccion.SelectedIndex == 0)
        {
        try
        {
            
            string run = txtRut.Text;
            iniciarConexion();
            using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
            {
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT id FROM curso_registros WHERE rut='" + rut + "' && not exists (select 1 from curso_datos where curso_datos.id_reg = curso_registros.id)", cn);
                System.Data.DataTable dt = new System.Data.DataTable();
                adp.Fill(dt);
                MySqlDataAdapter adp2 = new MySqlDataAdapter("SELECT id FROM curso_registros WHERE rut='" + run + "'", cn);
                System.Data.DataTable dt2 = new System.Data.DataTable();
                adp2.Fill(dt2);
                txtNombre.Text = string.Empty;
                txtId.Text = string.Empty;
                gvId.DataSource = null;
                gvId.DataBind();
                txtFecha.Text = string.Empty;
                txtNota.Text = string.Empty;
                txtIdReg.Text = string.Empty;
                gvIdRegistro.DataSource = null;
                gvIdRegistro.DataBind();
                imgId.Enabled = false;
                txtId.Enabled = false;
                if (dt2.Rows.Count > 0)
                {
                    if (dt.Rows.Count > 0)
                    {
                        imgFecha.Enabled = true; imgReg.Enabled = true; txtNota.ReadOnly = false;
                        lblSinReg.Visible = false; lblCompletos.Visible = false;
                        lblCompletos.Visible = false;
                        iniciarConexion();
                        Comando.CommandText = "SELECT * FROM curso_cliente WHERE rut='" + rut + "'";
                        Comando.Connection = connection;
                        Reader = Comando.ExecuteReader();
                        using (Reader)
                            if (Reader.Read())
                            {
                                txtRut.Text = Reader["rut"].ToString();
                                txtNombre.Text = Reader["nombres"].ToString() + " " + Reader["apellidoP"].ToString() + " " + Reader["apellidoM"].ToString();
                                if (ddlSeleccion.SelectedIndex == 1) { imgId.Enabled = true; txtId.Enabled = true; imgFecha.Enabled = true; imgReg.Enabled = true; txtNota.ReadOnly = true; }
                            }
                            else { lblErrorRut.Visible = true; }
                    }
                    else { lblSinReg.Visible = false; lblCompletos.Visible = true; imgFecha.Enabled = false; imgReg.Enabled = false; txtNota.ReadOnly = true; }
                }
                else { lblSinReg.Visible = true; lblCompletos.Visible = false; imgFecha.Enabled = false; imgReg.Enabled = false; txtNota.ReadOnly = true; }
            }
        }
        catch (Exception ex) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Rut formato erroneo" + ex + "')", true); }
        }
        if(ddlSeleccion.SelectedIndex==1)
        {
            try
            {
                iniciarConexion();
                Comando.CommandText = "SELECT * FROM curso_cliente WHERE rut='" + rut + "'";
                Comando.Connection = connection;
                Reader = Comando.ExecuteReader();
                using (Reader)
                    if (Reader.Read())
                    {
                        txtRut.Text = Reader["rut"].ToString();
                        txtNombre.Text = Reader["nombres"].ToString() + " " + Reader["apellidoP"].ToString() + " " + Reader["apellidoM"].ToString();
                        if (ddlSeleccion.SelectedIndex == 1) { imgId.Enabled = true; txtId.Enabled = true; txtNota.ReadOnly = true; }
                    }
                    else { lblErrorRut.Visible = true; }
            }
            catch (Exception ex) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Rut formato erroneo" + ex + "')", true); }
        }
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
    protected void txtIdReg_TextChanged(object sender, EventArgs e)
    {

    }
    protected void imgId_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (gvId.Visible == false)
            {
                string rut=txtRut.Text;
                iniciarConexion();
                using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
                {
                    MySqlDataAdapter adp = new MySqlDataAdapter("SELECT C.id AS 'Id_de_la_evaluacion', C.fecha AS 'Fecha_de_la_evaluacion',C.nota AS 'Nota', C.id_reg AS 'Id_del_registro', A.curso AS 'Curso', A.fechaCurso AS 'Inicio_del_curso' FROM curso_datos C, curso_registros A WHERE C.rut='"+rut+"' AND C.id_reg=A.id" , cn);
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
                }
                gvId.Visible = true;
            }
            else { gvId.Visible = false; }
        }
        catch (Exception ex) { }
    }
    protected void imgReg_Click(object sender, ImageClickEventArgs e)
    {

        try
        {
            if (gvIdRegistro.Visible == false)
            {
                string rut = txtRut.Text;
                iniciarConexion();
                using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
                {
                    MySqlDataAdapter adp = new MySqlDataAdapter("SELECT id AS 'Id Registro',curso AS 'Curso',fechaCurso AS 'Fecha de inicio del curso'  FROM curso_registros WHERE rut='" + rut + "' && not exists (select 1 from curso_datos where curso_datos.id_reg = curso_registros.id)", cn);
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
    protected void imgFecha_Click(object sender, ImageClickEventArgs e)
    {

        if (Calendar1.Visible == false)
        { Calendar1.Visible = true; }
        else { Calendar1.Visible = false; }
     
    }
    protected void gvId_RowCommand(object sender, GridViewCommandEventArgs e)
    {
          if (e.CommandName == "Select" && ddlSeleccion.SelectedIndex ==1)
     {

             try
             {
                 Int16 num = Convert.ToInt16(e.CommandArgument);
                 string t1,t2,t3,t4;
                 t1 = gvId.Rows[num].Cells[1].Text;
                 txtId.Text = t1;
                 t2=gvId.Rows[num].Cells[2].Text;
                 txtFecha.Text = t2;
                 t3 = gvId.Rows[num].Cells[3].Text;
                 string[] array = t3.Split('%');
                 txtNota.Text = array[0];
                 t4 = gvId.Rows[num].Cells[4].Text;
                 txtIdReg.Text = t4;
                 gvId.Visible = false;
                 txtNota.ReadOnly = false;
                 imgFecha.Enabled = true;
                 imgReg.Enabled = true;
                 }
             catch (Exception ex) { }
         }
    }
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
    protected void gvId_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
            string fe = Calendar1.SelectedDate.ToShortDateString();
            DateTime fe2 = new DateTime();
            fe2 = Convert.ToDateTime(fe);
            string fe3 = fe2.ToString("yyyy-MM-dd");
            txtFecha.Text = fe3;
            Calendar1.Visible = false;
        }
        catch (Exception ex) { }
    }
    protected void gvIdRegistro_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnModificar_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime fecha = Convert.ToDateTime(txtFecha.Text);
            string nota = txtNota.Text + "%";
            int id_reg = Convert.ToInt32(txtIdReg.Text);
            int id = Convert.ToInt32(txtId.Text);
            string fe = fecha.ToShortDateString();
            DateTime fe2 = new DateTime();
            fe2 = Convert.ToDateTime(fe);
            string fe3 = fe2.ToString("yyyy-MM-dd");
            txtFecha.Text = fe3;
            actualizarEva(fe3, nota, id_reg, id);
            string message = "alert('Evaluación Modificada')";
            ScriptManager.RegisterClientScriptBlock((sender as System.Web.UI.Control), this.GetType(), "alert", message, true);
            LimpiarControles();
        }
        catch (Exception ex) { }
    }
    public void actualizarEva(string fecha, string nota, int id_reg,int id)
    {
        try
        {
            iniciarConexion();
            string strSQL = "UPDATE curso_datos set fecha='" + fecha + "',nota='" + nota + "',id_reg='" + id_reg + "' WHERE id=" + id + ";";
            Comando.CommandText = strSQL;
            Comando.Connection = connection;
            Comando.ExecuteNonQuery();
        }
        catch (Exception ex) { }
    }
}