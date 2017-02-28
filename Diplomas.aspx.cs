using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.html.simpleparser;   
using System.Text;
using MySql.Data.MySqlClient;
using Microsoft.Office.Interop.Excel;
public partial class Pag_Diploma_Diplomas : System.Web.UI.Page
{

    static MySqlConnection connection = new MySqlConnection();
    static MySqlCommand Comando = new MySqlCommand();
    static MySqlDataAdapter Adaptador = new MySqlDataAdapter();
    static MySqlDataReader Reader;
    string connectionString;
    string codigo;
    string fecha;
    string codigoAux;
    bool encontrado = false;
    string nombres, apellidoP, apellidoM, sexo, ingreso, nomEmpresa;

    
    protected void Page_Load(object sender, EventArgs e)
    {
        connectionString = "Server=192.168.1.7; Database=sgsmtc; userid=root; password='s0p0rte'; ";
        try
        {
        iniciarConexion();
        DateTime now = DateTime.Now;
        int mes = Convert.ToInt32(now.Month);
        string ano = now.ToString("yyyy");
        string dia =Convert.ToString(now.Day);
        string rut;
      
        string code;
        string me = obtenerNombreMesNumero(mes);
        me=ConvertirPrimeraLetraEnMayuscula(me);
        lblmes.Text = me;
        lblano.Text = ano;
        lbldia.Text = dia;

        rut = Request.Params["parametro"];
        code = Request.Params["parametro2"];
        buscarCliente(rut);
        buscarRegistro(code);
      
        }
        catch (Exception ex)
        {
        }
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
    private static string ConvertirPrimeraLetraEnMayuscula(string texto){
    string str = "";
    str = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(texto);
    return str;
    }

    private void iniciarConexion() 
    {
        try
        {
            connection.ConnectionString = connectionString;
            connection.Open();
        }
        catch (Exception ex) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Error al conectarse a la base de datos"+ex+"')", true); }
    }
    public void buscarCliente(string rut)
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
                    encontrado = true;
                    nombres = Reader["nombres"].ToString();
                    apellidoP = Reader["apellidoP"].ToString();
                    apellidoM = Reader["apellidoM"].ToString();
                    sexo = Reader["sexo"].ToString();

                    lblNombre.Text = nombres+" "+apellidoP+" "+apellidoM;
                    if (sexo == "M") { lblSexo.Text = "señor"; }
                    if (sexo == "F") { lblSexo.Text = "señorita"; }

                    
                }
        }
        catch (Exception ex) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Rut formato erroneo" + ex + "')", true); }
    }
    public void buscarRegistro(string codigo)
    {
        int o = 0;
        iniciarConexion();
        Comando.CommandText = "SELECT * FROM curso_registros WHERE codigoA='" + codigo + "'";
        Comando.Connection = connection;
        Reader = Comando.ExecuteReader();
        using (Reader)
            if (Reader.Read())
            {
                lblCurso.Text = Reader["curso"].ToString();
                lblCodigo.Text = Reader["codigoA"].ToString();
                DateTime val = Convert.ToDateTime(Reader["fechaCurso"].ToString());
                string fechas = Reader["fechas"].ToString();
                DateTime fe;
                string fei = fechas;
                string palabra = fechas;
                string[] array = palabra.Split(';');
                DateTime fe2;
                DateTime fe3;
                string[] array2;
                for (int i = 0; i < array.Length; i++)
                { 
                    if(i<=array.Length-3){
                    fe2=Convert.ToDateTime(array[i+1].ToString());}
                    else{
                    fe2=Convert.ToDateTime(array[i].ToString());}
                    fe3=Convert.ToDateTime(array[i].ToString());
                    if (fe2.ToString("MM") == fe3.ToString("MM"))
                    {
                        if (i == array.Length - 3 && o==0)
                        {
                            fe = Convert.ToDateTime(array[i].ToString());
                            lblFecha.Text += fe.ToString("dd") + " y ";
                        }
                        if (i == array.Length - 3 && o == 1) {
                            fe = Convert.ToDateTime(array[i].ToString());
                            lblFecha.Text += fe.ToString("dd")+",";
                        }
                        if (i != array.Length - 2 && i != array.Length - 3)
                        {
                            fe = Convert.ToDateTime(array[i].ToString());
                            lblFecha.Text += fe.ToString("dd") + ",";
                        }
                        if (i == array.Length - 2)
                        {
                            fe = Convert.ToDateTime(array[i].ToString());
                            lblFecha.Text += fe.ToString("dd") + " de " + ConvertirPrimeraLetraEnMayuscula(obtenerNombreMesNumero(Convert.ToInt32(fe.ToString("MM")))) + " de " + fe.ToString("yyyy");
                        }
                    }
                    if(fe2.ToString("MM")!=fe3.ToString("MM"))
                    {
                        o = 1;
                        lblFecha.Text += fe3.ToString("dd") + " de " + ConvertirPrimeraLetraEnMayuscula(obtenerNombreMesNumero(Convert.ToInt32(fe3.ToString("MM")))) + " y ";
                    }
                }
            }

    }
}
