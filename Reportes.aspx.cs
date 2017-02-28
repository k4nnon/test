using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.IO;

public partial class Reportes : System.Web.UI.Page
{
    static MySqlConnection connection = new MySqlConnection();
    static MySqlCommand Comando = new MySqlCommand();
    static MySqlDataAdapter Adaptador = new MySqlDataAdapter();
    static MySqlDataReader Reader;
    string connectionString;
    bool aux = false;
    static string[] nombresEmp2;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindAno();
        connectionString = "Server=192.168.1.7; Database=sgsmtc; userid=root; password='s0p0rte'; ";
        ddlReporte.Enabled = true;
        iniciarConexion();
        using (MySqlConnection cn = new MySqlConnection(connectionString))
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
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReporte.SelectedIndex == 6) { Response.Redirect("Default.aspx"); }
        if (ddlReporte.SelectedIndex == 4)
        {
            btnReporte.Visible = false; 
            txtEmpresa.Text = string.Empty;
            lblEmpresa.Visible = false; txtEmpresa.Visible = false;
            imgCal1.Visible = false; ImgCal2.Visible = false;
            ddlAno.Visible = false; lblAno.Visible = false; lblCurso.Visible = false; ddlCurso.Visible = false; gvReportes.Visible = false; btnEx.Visible = false; txtFecha1.Visible = false; lblFecha1.Visible = false;
            txtFecha2.Visible = false; lblFecha2.Visible = false; Cal1.Visible = false; Cal2.Visible = false;
            try
            {

                string ano;
                DateTime fechaHoy = DateTime.Now.Date;
                ano = fechaHoy.Year.ToString();
                lblError.Text = ano.ToString();

                string val = "Aprobado";
                iniciarConexion();
                DataTable dt = new DataTable();
                string mysqlConnString = connectionString.ToString();
                using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
                {
                   
                    lblError.Visible = true;

                    MySqlDataAdapter adp = new MySqlDataAdapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', C.fechaCurso AS 'Fecha_de_Inicio', C.estado AS 'Estado', A.nomEmpresa AS'Empresa' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND C.estado='" + val + "' AND C.fechaIngreso LIKE'%" + ano + "%'", cn);
                    // MySqlDataAdapter adp = new MySqlDataAdapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', C.fechaCurso AS 'Fecha_de_Inicio', C.fechaIngreso AS  'Fecha_de_ingreso', C.codigoA AS 'Codigo', C.estado AS 'Estado', C.id AS 'Id',C.id_curso AS 'ID_Curso' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND C.fechaIngreso LIKE'%" + val + "%'", cn);

                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        btnEx.Visible = true;
                        gvReportes.Visible = true;
                        lblError.Visible = false;
                        gvReportes.DataSource = null;
                        gvReportes.DataBind();
                        gvReportes.DataSource = dt;
                        gvReportes.DataBind();
                        btnEx.Visible = true;
                    }
                    else { lblError.Text = "No hay registros"; lblError.Visible = true; gvReportes.Visible = false; btnEx.Visible = false; }
                }
            }
            catch (Exception ex) { }
        }
        if (ddlReporte.SelectedIndex == 5) { btnReporte.Visible = false;
        txtEmpresa.Text = string.Empty;
        lblEmpresa.Visible = false; txtEmpresa.Visible = false;
        imgCal1.Visible = false; ImgCal2.Visible = false;
        ddlAno.Visible = false; lblAno.Visible = false; lblCurso.Visible = false; ddlCurso.Visible = false; gvReportes.Visible = false; btnEx.Visible = false; txtFecha1.Visible = false; lblFecha1.Visible = false;
        txtFecha2.Visible = false; lblFecha2.Visible = false; Cal1.Visible = false; Cal2.Visible = false;

        try
        {
            string ano;
            DateTime fechaHoy = DateTime.Now.Date;
            ano = fechaHoy.Year.ToString();
            lblError.Text = ano.ToString();

            string val = "Pendiente";
            iniciarConexion();
            DataTable dt = new DataTable();
            string mysqlConnString = connectionString.ToString();
            using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
            {
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', C.fechaCurso AS 'Fecha_de_Inicio', C.estado AS 'Estado', A.nomEmpresa AS'Empresa' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut  AND C.estado LIKE'" + val + "%' AND C.fechaIngreso LIKE'%" + ano + "%'", cn);
                // MySqlDataAdapter adp = new MySqlDataAdapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', C.fechaCurso AS 'Fecha_de_Inicio', C.fechaIngreso AS  'Fecha_de_ingreso', C.codigoA AS 'Codigo', C.estado AS 'Estado', C.id AS 'Id',C.id_curso AS 'ID_Curso' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND C.fechaIngreso LIKE'%" + val + "%'", cn);

                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    btnEx.Visible = true;
                    gvReportes.Visible = true;
                    lblError.Visible = false;
                    gvReportes.DataSource = null;
                    gvReportes.DataBind();
                    gvReportes.DataSource = dt;
                    gvReportes.DataBind();
                    btnEx.Visible = true;
                }
                else { lblError.Text = "No hay registros"; lblError.Visible = true; gvReportes.Visible = false; btnEx.Visible = false; }
            }
        }
        catch (Exception ex) { }
        
        }
        if (ddlReporte.SelectedIndex == 3)
        {
            btnReporte.Visible = true; 
            txtEmpresa.Text = string.Empty;
            lblEmpresa.Visible = true; txtEmpresa.Visible = true;
            imgCal1.Visible = false; ImgCal2.Visible = false;
            ddlAno.Visible = false; lblAno.Visible = false; lblCurso.Visible = false; ddlCurso.Visible = false; gvReportes.Visible = false; btnEx.Visible = false; txtFecha1.Visible = false; lblFecha1.Visible = false;
            txtFecha2.Visible = false; lblFecha2.Visible = false; Cal1.Visible = false; Cal2.Visible = false;
        }
        if (ddlReporte.SelectedIndex == 0)
        {
            btnReporte.Visible = true; 
            txtEmpresa.Text = string.Empty;
            imgCal1.Visible = false; ImgCal2.Visible = false; lblEmpresa.Visible = false; txtEmpresa.Visible = false;
            ddlAno.Visible = true; lblAno.Visible = true; lblCurso.Visible = false; ddlCurso.Visible = false; gvReportes.Visible = false; btnEx.Visible = false; txtFecha1.Visible = false; lblFecha1.Visible = false;
            txtFecha2.Visible = false; lblFecha2.Visible = false; Cal1.Visible = false; Cal2.Visible = false;
        }
        if (ddlReporte.SelectedIndex == 1) { ddlAno.Visible = false; lblAno.Visible = false; lblCurso.Visible = false; ddlCurso.Visible = false; gvReportes.Visible = false; btnEx.Visible = false; txtFecha1.Visible = true; lblFecha1.Visible = true;
        txtFecha2.Visible = true; lblFecha2.Visible = true; Cal1.Visible = false; imgCal1.Visible = true; ImgCal2.Visible = true; Cal2.Visible = false;
        lblEmpresa.Visible = false; txtEmpresa.Visible = false; txtEmpresa.Text = string.Empty; btnReporte.Visible = true; 
        }
        if (ddlReporte.SelectedIndex == 2) { ddlAno.Visible = false; lblAno.Visible = false; lblCurso.Visible = true; ddlCurso.Visible = true; BindCursos(); gvReportes.Visible = false; btnEx.Visible = false;
        txtFecha1.Visible = false; lblFecha1.Visible = false; txtFecha2.Visible = false; lblFecha2.Visible = false; Cal1.Visible = false; imgCal1.Visible = false; ImgCal2.Visible = false; Cal2.Visible = false;
        lblEmpresa.Visible = false; txtEmpresa.Visible = false; txtEmpresa.Text = string.Empty; btnReporte.Visible = true; 
        }
    }
    protected void DropDownList1_SelectedIndexChanged1(object sender, EventArgs e)
    {
    }

    private void BindAno() 
    {
    string año="2014";
    int i=Convert.ToInt32(año);
    for (i = Convert.ToInt32(año); i < 3001; i++) 
    {
        string ano = Convert.ToString(i);
        ddlAno.Items.Add(ano);
        ddlAno.DataBind();
        
    }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (ddlReporte.SelectedIndex == 3) 
        {
            string val = txtEmpresa.Text;
            iniciarConexion();
            DataTable dt = new DataTable();
            string mysqlConnString = connectionString.ToString();
            using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
            {
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', C.fechaCurso AS 'Fecha_de_Inicio', C.estado AS 'Estado', A.nomEmpresa AS'Empresa' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND A.rut IN (SELECT rut FROM curso_cliente WHERE ingreso='Empresa' AND nomEmpresa LIKE'" + val + "%')", cn);
              // MySqlDataAdapter adp = new MySqlDataAdapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', C.fechaCurso AS 'Fecha_de_Inicio', C.fechaIngreso AS  'Fecha_de_ingreso', C.codigoA AS 'Codigo', C.estado AS 'Estado', C.id AS 'Id',C.id_curso AS 'ID_Curso' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND C.fechaIngreso LIKE'%" + val + "%'", cn);

                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    btnEx.Visible = true;
                    gvReportes.Visible = true;
                    lblError.Visible = false;
                    gvReportes.DataSource = null;
                    gvReportes.DataBind();
                    gvReportes.DataSource = dt;
                    gvReportes.DataBind();
                    btnEx.Visible = true;
                }
                else { lblError.Text = "No hay registros"; lblError.Visible = true; gvReportes.Visible = false; btnEx.Visible = false; }
            }
        }
        if (ddlReporte.SelectedIndex == 0) 
        { 
            string val = ddlAno.SelectedValue;
            iniciarConexion();
            DataTable dt = new DataTable();
            string mysqlConnString = connectionString.ToString();
            using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
            {
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', C.fechaCurso AS 'Fecha_de_Inicio', C.estado AS 'Estado' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND C.fechaIngreso LIKE'%" + val + "%'", cn);

                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    btnEx.Visible = true;
                    gvReportes.Visible = true;
                    lblError.Visible = false;
                    gvReportes.DataSource = null;
                    gvReportes.DataBind();
                    gvReportes.DataSource = dt;
                    gvReportes.DataBind();
                    btnEx.Visible = true;
                }
                else { lblError.Text = "No hay registros"; lblError.Visible = true; gvReportes.Visible = false; btnEx.Visible = false; }
            }
        }
        if (ddlReporte.SelectedIndex == 2)
        {
            string val = ddlCurso.SelectedValue;
            iniciarConexion();

            string mysqlConnString = connectionString.ToString();
            using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
            {
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', C.fechaCurso AS 'Fecha_de_Inicio', C.estado AS 'Estado' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND C.curso='" + val + "'", cn);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    btnEx.Visible = true;
                    gvReportes.Visible = true;
                    lblError.Visible = false;
                    gvReportes.DataSource = null;
                    gvReportes.DataBind();
                    gvReportes.DataSource = dt;
                    gvReportes.DataBind();
                    btnEx.Visible = true;
                }
                else { lblError.Text = "No hay registros"; lblError.Visible = true; gvReportes.Visible = false; btnEx.Visible = false; }
            }
        }
        if (ddlReporte.SelectedIndex == 1) 
        {
            try
            {
                string val = txtFecha1.Text;
                string val1 = txtFecha2.Text;
                iniciarConexion();

                string mysqlConnString = connectionString.ToString();
                using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
                {
                    MySqlDataAdapter adp = new MySqlDataAdapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', C.fechaCurso AS 'Fecha_de_Inicio', C.estado AS 'Estado' FROM curso_registros C,curso_cliente A WHERE  A.rut=C.rut AND fechaIngreso BETWEEN '" + val + "' and '" + val1 + "'", cn);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        btnEx.Visible = true;
                        gvReportes.Visible = true;
                        lblError.Visible = false;
                        gvReportes.DataSource = null;
                        gvReportes.DataBind();
                        gvReportes.DataSource = dt;
                        gvReportes.DataBind();
                        btnEx.Visible = true;
                    }
                    else { lblError.Text = "No hay registros"; lblError.Visible = true; gvReportes.Visible = false; btnEx.Visible = false; }
                }
            }
            catch (Exception ex) { lblError.Visible = true; lblError.Text = ex.Message; }
        }
    }
    private void iniciarConexion()
    {
        try
        {
       
            connection.ConnectionString = connectionString;
            connection.Open();
        }
        catch (Exception ex) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Error al conectarse a la base de datos" + ex + "')", true); }
    }
    protected void btnPdf_Click(object sender, EventArgs e)
    {
        if (ddlReporte.SelectedIndex == 4)
        {
            try
            {
                string ano;
                DateTime fechaHoy = DateTime.Now.Date;
                ano = fechaHoy.Year.ToString();
                lblError.Text = ano.ToString();

                string val = "Pendiente";
                iniciarConexion();
                DataTable dt = new DataTable();
                string mysqlConnString = connectionString.ToString();
                using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
                {
                    MySqlDataAdapter adp = new MySqlDataAdapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', C.fechaCurso AS 'Fecha_de_Inicio', C.estado AS 'Estado', A.nomEmpresa AS'Empresa' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut  AND C.estado='" + val + "' AND C.fechaIngreso LIKE'%" + ano + "%'", cn);

                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {

                        gvReportes.Visible = true;
                        lblError.Visible = false;
                        gvReportes.DataSource = null;
                        gvReportes.DataBind();
                        gvReportes.DataSource = dt;
                        gvReportes.DataBind();
                        btnEx.Visible = true;

                    }
                    else { lblError.Text = "No hay registros"; lblError.Visible = true; gvReportes.Visible = false; btnEx.Visible = true; }
                }
                if (dt == null)
                {
                    throw new Exception("No Records to Export");
                }
                string Path = "C:\\" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + ".xls";
                FileInfo FI = new FileInfo(Path);
                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWriter);
                DataGrid DataGrd = new DataGrid();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Charset = "65001";
                var b = new byte[] {
                0xef,
                0xbb,
                0xbf
                };
                Response.BinaryWrite(b);
                DataGrd.DataSource = dt;
                DataGrd.ShowHeader = true;
                DataGrd.DataBind();

                DataGrd.RenderControl(htmlWrite);
                string directory = Path.Substring(0, Path.LastIndexOf("\\"));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                System.IO.StreamWriter vw = new System.IO.StreamWriter(Path, true, System.Text.Encoding.GetEncoding("Windows-1252"));
                stringWriter.ToString().Normalize();
                vw.Write(stringWriter.ToString());
                vw.Flush();
                vw.Close();
                WriteAttachment(FI.Name, "application/vnd.ms-excel", stringWriter.ToString());
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
            }
        }
        if (ddlReporte.SelectedIndex == 5)
        {
            try
            {
                string ano;
                DateTime fechaHoy = DateTime.Now.Date;
                ano = fechaHoy.Year.ToString();
                lblError.Text = ano.ToString();

                string val = "Pendiente";
                iniciarConexion();
                DataTable dt = new DataTable();
                string mysqlConnString = connectionString.ToString();
                using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
                {
                    MySqlDataAdapter adp = new MySqlDataAdapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', C.fechaCurso AS 'Fecha_de_Inicio', C.estado AS 'Estado', A.nomEmpresa AS'Empresa' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut  AND C.estado='" + val + "' AND C.fechaIngreso LIKE'" + ano + "%'", cn);

                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {

                        gvReportes.Visible = true;
                        lblError.Visible = false;
                        gvReportes.DataSource = null;
                        gvReportes.DataBind();
                        gvReportes.DataSource = dt;
                        gvReportes.DataBind();
                        btnEx.Visible = true;

                    }
                    else { lblError.Text = "No hay registros"; lblError.Visible = true; gvReportes.Visible = false; btnEx.Visible = true; }
                }
                if (dt == null)
                {
                    throw new Exception("No Records to Export");
                }
                string Path = "C:\\" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + ".xls";
                FileInfo FI = new FileInfo(Path);
                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWriter);
                DataGrid DataGrd = new DataGrid();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Charset = "65001";
                var b = new byte[] {
                0xef,
                0xbb,
                0xbf
                };
                Response.BinaryWrite(b);
                DataGrd.DataSource = dt;
                DataGrd.ShowHeader = true;
                DataGrd.DataBind();

                DataGrd.RenderControl(htmlWrite);
                string directory = Path.Substring(0, Path.LastIndexOf("\\"));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                System.IO.StreamWriter vw = new System.IO.StreamWriter(Path, true, System.Text.Encoding.GetEncoding("Windows-1252"));
                stringWriter.ToString().Normalize();
                vw.Write(stringWriter.ToString());
                vw.Flush();
                vw.Close();
                WriteAttachment(FI.Name, "application/vnd.ms-excel", stringWriter.ToString());
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
            }
        }
        if (ddlReporte.SelectedIndex == 3) 
        {
            try
            {
                string val = txtEmpresa.Text;
                iniciarConexion();
                DataTable dt = new DataTable();
                string mysqlConnString = connectionString.ToString();
                using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
                {
                    MySqlDataAdapter adp = new MySqlDataAdapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', C.fechaCurso AS 'Fecha_de_Inicio', C.estado AS 'Estado', A.nomEmpresa AS'Empresa' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND A.rut IN (SELECT rut FROM curso_cliente WHERE ingreso='Empresa' AND nomEmpresa LIKE'" + val + "%')", cn);

                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {

                        gvReportes.Visible = true;
                        lblError.Visible = false;
                        gvReportes.DataSource = null;
                        gvReportes.DataBind();
                        gvReportes.DataSource = dt;
                        gvReportes.DataBind();
                        btnEx.Visible = true;

                    }
                    else { lblError.Text = "No hay registros"; lblError.Visible = true; gvReportes.Visible = false; btnEx.Visible = true; }
                }
                if (dt == null)
                {
                    throw new Exception("No Records to Export");
                }
                string Path = "C:\\" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + ".xls";
                FileInfo FI = new FileInfo(Path);
                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWriter);
                DataGrid DataGrd = new DataGrid();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Charset = "65001";
                var b = new byte[] {
                0xef,
                0xbb,
                0xbf
                };
                Response.BinaryWrite(b);
                DataGrd.DataSource = dt;
                DataGrd.ShowHeader = true;
                DataGrd.DataBind();

                DataGrd.RenderControl(htmlWrite);
                string directory = Path.Substring(0, Path.LastIndexOf("\\"));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                System.IO.StreamWriter vw = new System.IO.StreamWriter(Path, true, System.Text.Encoding.GetEncoding("Windows-1252"));
                stringWriter.ToString().Normalize();
                vw.Write(stringWriter.ToString());
                vw.Flush();
                vw.Close();
                WriteAttachment(FI.Name, "application/vnd.ms-excel", stringWriter.ToString());
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
            }
        }
        if (ddlReporte.SelectedIndex == 2)
        {
            try
            {
                string val = ddlCurso.SelectedValue;
                iniciarConexion();
                DataTable dt = new DataTable();
                string mysqlConnString = connectionString.ToString();
                using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
                {
                    MySqlDataAdapter adp = new MySqlDataAdapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', C.fechaCurso AS 'Fecha_de_Inicio', C.estado AS 'Estado' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND C.curso='" + val + "'", cn);
                    
                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {

                        gvReportes.Visible = true;
                        lblError.Visible = false;
                        gvReportes.DataSource = null;
                        gvReportes.DataBind();
                        gvReportes.DataSource = dt;
                        gvReportes.DataBind();
                        btnEx.Visible = true;

                    }
                    else { lblError.Text = "No hay registros"; lblError.Visible = true; gvReportes.Visible = false; btnEx.Visible = true; }
                }
                if (dt == null)
                {
                    throw new Exception("No Records to Export");
                }
                string Path = "C:\\" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + ".xls";
                FileInfo FI = new FileInfo(Path);
                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWriter);
                DataGrid DataGrd = new DataGrid();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Charset = "65001";
                var b = new byte[] {
                0xef,
                0xbb,
                0xbf
                };
                Response.BinaryWrite(b);
                DataGrd.DataSource = dt;
                DataGrd.ShowHeader = true;
                DataGrd.DataBind();

                DataGrd.RenderControl(htmlWrite);
                string directory = Path.Substring(0, Path.LastIndexOf("\\"));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                System.IO.StreamWriter vw = new System.IO.StreamWriter(Path, true, System.Text.Encoding.GetEncoding("Windows-1252"));
                stringWriter.ToString().Normalize();
                vw.Write(stringWriter.ToString());
                vw.Flush();
                vw.Close();
                WriteAttachment(FI.Name, "application/vnd.ms-excel", stringWriter.ToString());
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
            }
        }
        if (ddlReporte.SelectedIndex == 0) 
        { 
        try
            {
                string val = ddlCurso.SelectedValue;
                iniciarConexion();
                DataTable dt = new DataTable();
                string mysqlConnString = connectionString.ToString();
                using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
                {
                    MySqlDataAdapter adp = new MySqlDataAdapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', C.fechaCurso AS 'Fecha_de_Inicio', C.estado AS 'Estado' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND C.fechaIngreso LIKE'%" + val + "%'", cn);
                
     
                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {

                        gvReportes.Visible = true;
                        lblError.Visible = false;
                        gvReportes.DataSource = null;
                        gvReportes.DataBind();
                        gvReportes.DataSource = dt;
                        gvReportes.DataBind();
                        btnEx.Visible = true;
                    }
                    else { lblError.Text = "No hay registros"; lblError.Visible = true; gvReportes.Visible = false; btnEx.Visible = true; }
                }
                if (dt == null)
                {
                    throw new Exception("No Records to Export");
                }
                string Path = "C:\\" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + ".xls";
                FileInfo FI = new FileInfo(Path);
                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWriter);
                DataGrid DataGrd = new DataGrid();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Charset = "65001";
                var b = new byte[] {
0xef,
0xbb,
0xbf
};
                Response.BinaryWrite(b);
                DataGrd.DataSource = dt;
                DataGrd.ShowHeader = true; 
                DataGrd.DataBind();

                DataGrd.RenderControl(htmlWrite);
                string directory = Path.Substring(0, Path.LastIndexOf("\\"));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                System.IO.StreamWriter vw = new System.IO.StreamWriter(Path, true);
                stringWriter.ToString().Normalize();
                vw.Write(stringWriter.ToString());
                vw.Flush();
                vw.Close();
                WriteAttachment(FI.Name, "application/vnd.ms-excel", stringWriter.ToString());
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
            }
        }

        if (ddlReporte.SelectedIndex == 1)
        {
            try
            {
                string val =txtFecha1.Text;
                string val1=txtFecha2.Text;
                iniciarConexion();
                DataTable dt = new DataTable();
                string mysqlConnString = connectionString.ToString();
                using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
                {
                    MySqlDataAdapter adp = new MySqlDataAdapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', C.fechaCurso AS 'Fecha_de_Inicio', C.estado AS 'Estado' FROM curso_registros C,curso_cliente A WHERE  A.rut=C.rut AND fechaIngreso BETWEEN '" + val + "' and '" + val1 + "'", cn);


                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {

                        gvReportes.Visible = true;
                        lblError.Visible = false;
                        gvReportes.DataSource = null;
                        gvReportes.DataBind();
                        gvReportes.DataSource = dt;
                        gvReportes.DataBind();
                        btnEx.Visible = true;
                    }
                    else { lblError.Text = "No hay registros"; lblError.Visible = true; gvReportes.Visible = false; btnEx.Visible = true; }
                }
                if (dt == null)
                {
                    throw new Exception("No Records to Export");
                }
                string Path = "C:\\" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + ".xls";
                FileInfo FI = new FileInfo(Path);
                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWriter);
                DataGrid DataGrd = new DataGrid();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Charset = "65001";
                var b = new byte[] {
0xef,
0xbb,
0xbf
};
                Response.BinaryWrite(b);
                DataGrd.DataSource = dt;
                DataGrd.ShowHeader = true;
                DataGrd.DataBind();

                DataGrd.RenderControl(htmlWrite);
                string directory = Path.Substring(0, Path.LastIndexOf("\\"));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                System.IO.StreamWriter vw = new System.IO.StreamWriter(Path, true);
                stringWriter.ToString().Normalize();
                vw.Write(stringWriter.ToString());
                vw.Flush();
                vw.Close();
                WriteAttachment(FI.Name, "application/vnd.ms-excel", stringWriter.ToString());
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
            }
        }
    }
    private void BindCursos()
    {
        string mysqlConnString = connectionString.ToString();
        using (MySqlConnection cn = new MySqlConnection(mysqlConnString))
        {
            MySqlDataAdapter adp = new MySqlDataAdapter("SELECT curso FROM curso_creado", cn);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ddlCurso.DataSource = dt;
                ddlCurso.DataTextField = "curso";
                ddlCurso.DataBind();
            }
        }
    }
    public static void WriteAttachment(string FileName, string FileType, string content)
    {
        HttpResponse Response = System.Web.HttpContext.Current.Response;
        Response.ClearHeaders();
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
        Response.ContentType = FileType;
        Response.Write(content);
        Response.End();

    }
 
    protected void Cal1_SelectionChanged(object sender, System.EventArgs e)
    {
        DateTime vari = Convert.ToDateTime(Cal1.SelectedDate.ToShortDateString());
        txtFecha1.Text = vari.ToString("yyyy/MM/dd");
        Cal1.Visible = false;
    }
    protected void ImageButton1_Click1(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        aux =false;
        Cal1.Visible = true;
        Cal2.Visible = false;
    }
    protected void ImageButton1_Click2(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        aux =true;
        Cal1.Visible = false;
        Cal2.Visible = true;
    }
    protected void Cal2_SelectionChanged(object sender, System.EventArgs e)
    {
        DateTime vari = Convert.ToDateTime(Cal2.SelectedDate.ToShortDateString());
        txtFecha2.Text = vari.ToString("yyyy/MM/dd");
        Cal2.Visible = false;
            if (txtFecha1.Text != string.Empty && txtFecha2.Text != string.Empty) { btnEx.Visible = true; }
            else { lblError.Text = "Debe seleccionar las fechas"; lblError.Visible = true; }
    }
    protected void txtFecha1_TextChanged(object sender, System.EventArgs e)
    {

    }
    protected void ddlCurso_SelectedIndexChanged(object sender, System.EventArgs e)
    {

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        return (from m in nombresEmp2 where m.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select m).Take(count).ToArray();
    }
    protected void txtEmpresa_TextChanged(object sender, System.EventArgs e)
    {

    }
}