using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Linq;
using System.Diagnostics;
using OfficeOpenXml;

public partial class Reportes : System.Web.UI.Page
{

    /**
    * @autor: 
    * @co autor: gonzalo zeballos. mail: maxpayne_ga@hotmail.com
    * @Sistema de toma de horas, generación de certificados y administración de cliente; dentro de los cursos Primavera
    * */
    /**
     * nombresEmp2: almacena un arreglo con todos los nombres de las empresas registradas en el sistema.
     */
    static string[] nombresEmp2;

    /**
     * Al iniciar la pagina, se cargaran las empresas en el arreglo para su próximo uso.
     * @Exception: error en la coneccion de la base de datos
     * */
    protected void Page_Load(object sender, EventArgs e)
    {
        BindAno();
        ddlReporte.Enabled = true;
        var adp = Namespace.Conexion.adapter("SELECT Crazon_social FROM cliente_empresa");
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
     * Maneja las opciones del menu de reportes dentro del programa, ademas genera algunos grid
     * donde la opcion corresponde a registros del año aprobados y pendientes.
     * @throw solo en las opciones 4 y 5 em caso que la base de datos falle.
     * */
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReporte.SelectedIndex == 0)
        {
            btnReporte.Visible = true;
            txtEmpresa.Text = string.Empty;
            imgCal1.Visible = false; ImgCal2.Visible = false; lblEmpresa.Visible = false; txtEmpresa.Visible = false;
            ddlAno.Visible = true; lblAno.Visible = true; lblCurso.Visible = false; ddlCurso.Visible = false; gvReportes.Visible = false; btnEx.Visible = false; txtFecha1.Visible = false; lblFecha1.Visible = false;
            txtFecha2.Visible = false; lblFecha2.Visible = false; Cal1.Visible = false; Cal2.Visible = false;
        }


        if (ddlReporte.SelectedIndex == 1)
        {
            ddlAno.Visible = false; lblAno.Visible = false; lblCurso.Visible = false; ddlCurso.Visible = false; gvReportes.Visible = false; btnEx.Visible = false; txtFecha1.Visible = true; lblFecha1.Visible = true;
            txtFecha2.Visible = true; lblFecha2.Visible = true; Cal1.Visible = false; imgCal1.Visible = true; ImgCal2.Visible = true; Cal2.Visible = false;
            lblEmpresa.Visible = false; txtEmpresa.Visible = false; txtEmpresa.Text = string.Empty; btnReporte.Visible = true;
        }
        if (ddlReporte.SelectedIndex == 2)
        {
            ddlAno.Visible = false; lblAno.Visible = false; lblCurso.Visible = true; ddlCurso.Visible = true; BindCursos(); gvReportes.Visible = false; btnEx.Visible = false;
            txtFecha1.Visible = false; lblFecha1.Visible = false; txtFecha2.Visible = false; lblFecha2.Visible = false; Cal1.Visible = false; imgCal1.Visible = false; ImgCal2.Visible = false; Cal2.Visible = false;
            lblEmpresa.Visible = false; txtEmpresa.Visible = false; txtEmpresa.Text = string.Empty; btnReporte.Visible = true;
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
                DateTime fecha = DateTime.Now.Date;
                string fechaHoy =  fecha.ToString("yyyy/MM/dd");
                Debug.Print("la fecha de hoy es "+fechaHoy);
               DataTable dt = new DataTable();
               string query = "SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', DATE_FORMAT(C.fechaCurso,'%d/%m/%Y') AS 'Fecha de Inicio', C.estado AS 'Estado' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND C.estado = 'Aprobado' AND DATE_FORMAT(C.fechaIngreso,'%Y') = DATE_FORMAT('" + fechaHoy + "','%Y') ";

                var adp = Namespace.Conexion.adapter(query);

               if (adp != null)
                {
                    lblError.Visible = true;
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
        if (ddlReporte.SelectedIndex == 5)
        {
            btnReporte.Visible = false;
            txtEmpresa.Text = string.Empty;
            lblEmpresa.Visible = false; txtEmpresa.Visible = false;
            imgCal1.Visible = false; ImgCal2.Visible = false;
            ddlAno.Visible = false; lblAno.Visible = false; lblCurso.Visible = false; ddlCurso.Visible = false; gvReportes.Visible = false; btnEx.Visible = false; txtFecha1.Visible = false; lblFecha1.Visible = false;
            txtFecha2.Visible = false; lblFecha2.Visible = false; Cal1.Visible = false; Cal2.Visible = false;

            try
            {
                DateTime fecha = DateTime.Now.Date;
                string fechaHoy = fecha.ToString("yyyy/MM/dd");

                string query = "SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', DATE_FORMAT(C.fechaCurso,'%d/%m/%Y') AS 'Fecha de Inicio', C.estado AS 'Estado' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND C.estado = 'Pendiente' AND DATE_FORMAT(C.fechaCurso,'%Y') = DATE_FORMAT('" + fechaHoy + "','%Y') ";
                DataTable dt = new DataTable();
                var adp = Namespace.Conexion.adapter(query);
               if(adp != null)
                {
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
        
      
    }

    //no se usa
    protected void DropDownList1_SelectedIndexChanged1(object sender, EventArgs e)
    {
    }

    /**
     * genera la lista de años que acepta el reporte anual, se dejo configurado que parta desde el 2014 el inicio de los registros
     * */
    private void BindAno()
    {
        string año = "2014";
        int i = Convert.ToInt32(año);
        for (i = Convert.ToInt32(año); i < 3001; i++)
        {
            string ano = Convert.ToString(i);
            ddlAno.Items.Add(ano);
            ddlAno.DataBind();
        }
    }

    /**
     * boton
     * genera los reportes en griview, dependiendo de la opción elegida en el menú principal 
     * @throw en caso que la conexion a la base de datos falle
     * */
    protected void Button1_Click(object sender, EventArgs e)
    {
      
        if (ddlReporte.SelectedIndex == 0)
        {
            string val = ddlAno.SelectedValue;
            val = val + "-01-01";
            Debug.Print("la fecha es : "+val);
            DataTable dt = new DataTable();
            var adp = Namespace.Conexion.adapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', DATE_FORMAT(C.fechaCurso,'%d/%m/%Y') AS 'Fecha de Inicio', C.estado AS 'Estado' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND DATE_FORMAT(C.fechaCurso,'%Y') = DATE_FORMAT('" + val + "','%Y')");
           if(adp != null)
            {
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
                var adp = Namespace.Conexion.adapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', DATE_FORMAT(C.fechaCurso,'%d/%m/%Y') AS 'Fecha de Inicio', C.estado AS 'Estado',  DATE_FORMAT(C.fechaIngreso, '%d/%m/%Y') AS 'Fecha de ingreso' FROM curso_registros C,curso_cliente A WHERE  A.rut=C.rut AND  DATE_FORMAT(C.fechaIngreso,'%Y-%m-%d') BETWEEN '" + val + "' and '" + val1 + "'");
                if (adp != null)
                {
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

        if (ddlReporte.SelectedIndex == 2)
        {
            string val = ddlCurso.SelectedValue;
            var adp = Namespace.Conexion.adapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', DATE_FORMAT(C.fechaCurso,'%d/%m/%Y') AS 'Fecha de Inicio', C.estado AS 'Estado' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND C.curso='" + val + "'");
           if(adp != null)
            {
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

        if (ddlReporte.SelectedIndex == 3)
        {
            string val = txtEmpresa.Text;
            DataTable dt = new DataTable();
            var adp = Namespace.Conexion.adapter(("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', DATE_FORMAT(C.fechaCurso,'%d/%m/%Y') AS 'Fecha de Inicio', C.estado AS 'Estado', A.nomEmpresa AS'Empresa' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND A.rut IN (SELECT rut FROM curso_cliente WHERE ingreso='Empresa' AND nomEmpresa LIKE'" + val + "%')"));
            if (adp != null)
            {
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

    }

    /**
     * boton que genera los reportes en excel para luego su descarga. dependiendo de la opcion del menu elegida
     * throw en caso que la conexion con la base de datos falle
     * nota: existe 2 versiones de generar el excel en este metodo, uno el cual genera excel sin ningun problema, pero
     * al momento de abrirlos arroja el mensaje que el archivo no es seguro y el otro que no genera este mensaje pero
     * al momento de realizar adapter con informacion menor a 5 rows el excel falla. Ahora esta la primera opcion
     * */
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        //reporte anual
        if (ddlReporte.SelectedIndex == 0)
        {
            try
            {
                string val = ddlAno.SelectedValue;
                val = val + "-01-01";
                Debug.Print("la segunda fecha es : " + val);
                DataTable dt = new DataTable();
                var adp = Namespace.Conexion.adapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', DATE_FORMAT(C.fechaCurso,'%d/%m/%Y') AS 'Fecha de Inicio', C.estado AS 'Estado' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND DATE_FORMAT(C.fechaIngreso,'%Y') = DATE_FORMAT('"+val+"','%Y')");
               if(adp != null)
                {
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

                //DataSet ds = new DataSet("New_DataSet");
                //DataTable dt2 = new DataTable("New_DataTable");

                //adp.Fill(dt2);
                var webAppPath = Context.Server.MapPath("~/") + "Reportes generados/";
                //ds.Tables.Add(dt2);
                //ExcelLibrary.DataSetHelper.CreateWorkbook(webAppPath + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Anual.xls", ds);
                //Response.ContentType = "application/octet-stream";
                //Response.AppendHeader("Content-Disposition", "attachment; filename=" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Anual.xls");
                //Response.TransmitFile(Server.MapPath("~/" + "Reportes generados/" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Anual.xls"));
                //Response.End();

                //var webAppPath = Context.Server.MapPath("~/");
                string Path = webAppPath + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString()+ "_Reporte_Anual_"+val+".xls";
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
                Debug.Print("error es : " + e);
            }
        }

        //reporte  entre fechas
        if (ddlReporte.SelectedIndex == 1)
        {
            try
            {
                string val = txtFecha1.Text;
                string val1 = txtFecha2.Text;
                DataTable dt = new DataTable();
                var adp = Namespace.Conexion.adapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', DATE_FORMAT(C.fechaCurso,'%d/%m/%Y') AS 'Fecha de Inicio', C.estado AS 'Estado',  DATE_FORMAT(C.fechaIngreso, '%d/%m/%Y') AS 'Fecha de ingreso' FROM curso_registros C,curso_cliente A WHERE  A.rut=C.rut AND  DATE_FORMAT(C.fechaIngreso,'%Y-%m-%d') BETWEEN '" + val + "' and '" + val1 + "'");
               if(adp != null)
                {
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

                //DataSet ds = new DataSet("New_DataSet");
                //DataTable dt2 = new DataTable("New_DataTable");

                //adp.Fill(dt2);
                var webAppPath = Context.Server.MapPath("~/") + "Reportes generados/";
                //ds.Tables.Add(dt2);
                //ExcelLibrary.DataSetHelper.CreateWorkbook(webAppPath + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Entre_Fechas.xls", ds);
                //Response.ContentType = "application/octet-stream";
                //Response.AppendHeader("Content-Disposition", "attachment; filename=" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Entre_Fechas.xls");
                //Response.TransmitFile(Server.MapPath("~/" + "Reportes generados/" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Entre_Fechas.xls"));
                //Response.End();

                //var webAppPath = Context.Server.MapPath("~/");
                string Path = webAppPath + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Entre_Fechas_"+val+"_Y_"+val1+".xls";
                FileInfo FI = new FileInfo(Path);
                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWriter);
                DataGrid DataGrd = new DataGrid();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Charset = "65001";
                var b = new byte[]
                {
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

        //reporte por cursos
        if (ddlReporte.SelectedIndex == 2)
        {
            try
            {
                string val = ddlCurso.SelectedValue;

                DataTable dt = new DataTable();
                var adp = Namespace.Conexion.adapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', DATE_FORMAT(C.fechaCurso,'%d/%m/%Y') AS 'Fecha de Inicio', C.estado AS 'Estado' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND C.curso='" + val + "'");
               if(adp != null)
                {
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

                //DataSet ds = new DataSet("New_DataSet");
                //DataTable dt2 = new DataTable("New_DataTable");

                //adp.Fill(dt2);
                var webAppPath = Context.Server.MapPath("~/") + "Reportes generados/";
                //ds.Tables.Add(dt2);
                //ExcelLibrary.DataSetHelper.CreateWorkbook(webAppPath + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Por_Curso.xls", ds);
                //Response.ContentType = "application/octet-stream";
                //Response.AppendHeader("Content-Disposition", "attachment; filename=" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Por_Curso.xls");
                //Response.TransmitFile(Server.MapPath("~/" + "Reportes generados/" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Por_Curso.xls"));
                //Response.End();

                //var webAppPath = Context.Server.MapPath("~/");
                string Path = webAppPath + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Por_Curso_"+val+".xls";
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


        //reporte por empresa
        if (ddlReporte.SelectedIndex == 3)
        {
            try
            {
                string val = txtEmpresa.Text;

                DataTable dt = new DataTable();
                var adp = Namespace.Conexion.adapter("SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', DATE_FORMAT(C.fechaCurso,'%d/%m/%Y') AS 'Fecha de Inicio', C.estado AS 'Estado', A.nomEmpresa AS'Empresa' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND A.rut IN (SELECT rut FROM curso_cliente WHERE ingreso='Empresa' AND nomEmpresa LIKE'" + val + "%')");
               if(adp != null)
                {
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

                //DataSet ds = new DataSet("New_DataSet");
                //DataTable dt2 = new DataTable("New_DataTable");

                //adp.Fill(dt2);
                var webAppPath = Context.Server.MapPath("~/") + "Reportes generados/";
                //ds.Tables.Add(dt2);
                //ExcelLibrary.DataSetHelper.CreateWorkbook(webAppPath + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Por_Empresa.xls", ds);
                //Response.ContentType = "application/octet-stream";
                //Response.AppendHeader("Content-Disposition", "attachment; filename=" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Por_Empresa.xls");
                //Response.TransmitFile(Server.MapPath("~/" + "Reportes generados/" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Por_Empresa.xls"));
                //Response.End();

              //  var webAppPath = Context.Server.MapPath("~/");
                string Path = webAppPath + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Por_Empresa_"+val+".xls";
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

        //reportes aprobados
        if (ddlReporte.SelectedIndex == 4)
        {
            try
            {
                
               
                DateTime fecha = DateTime.Now.Date;
                string fechaHoy = fecha.ToString("yyyy/MM/dd");
               
                string query = "SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', DATE_FORMAT(C.fechaCurso,'%d/%m/%Y') AS 'Fecha de Inicio', C.estado AS 'Estado' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND C.estado = 'Aprobado' AND DATE_FORMAT(C.fechaCurso,'%Y') = DATE_FORMAT('" + fechaHoy + "','%Y') ";
                DataTable dt = new DataTable();
                var adp = Namespace.Conexion.adapter(query);
               if(adp != null)
                {
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


                //DataSet ds = new DataSet("New_DataSet");
                //DataTable dt2 = new DataTable("New_DataTable");


                //adp.Fill(dt2);
                var webAppPath = Context.Server.MapPath("~/") + "Reportes generados/";
                //ds.Tables.Add(dt2);

                //ExcelLibrary.DataSetHelper.CreateWorkbook(webAppPath + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Aprobados_Este_Año.xls", ds);
                //Response.ContentType = "application/octet-stream";
                //Response.AppendHeader("Content-Disposition", "attachment; filename=" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Aprobados_Este_Año.xls");
                //Response.TransmitFile(Server.MapPath("~/" + "Reportes generados/" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Aprobados_Este_Año.xls"));
                //Response.End();

               // var webAppPath = Context.Server.MapPath("~/");
                string Path = webAppPath + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Aprobados_Este_Año.xls";
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
                var directory = Path.Substring(0, Path.LastIndexOf("\\", StringComparison.Ordinal));
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
                Debug.Print("el error es : " + ex);
            }
        }

        //reportes pendientes
        if (ddlReporte.SelectedIndex == 5)
        {
            try
            {
                DateTime fecha = DateTime.Now.Date;
                string fechaHoy = fecha.ToString("yyyy/MM/dd");
                string query = "SELECT CONCAT(CONCAT(A.nombres,' ',A.apellidoP),' ',A.apellidoM) AS 'Nombre', C.rut AS 'Rut', C.curso AS 'Curso', DATE_FORMAT(C.fechaCurso,'%d/%m/%Y') AS 'Fecha de Inicio', C.estado AS 'Estado' FROM curso_cliente A , curso_registros C WHERE A.rut=C.rut AND C.estado = 'Pendiente' AND DATE_FORMAT(C.fechaCurso,'%Y') = DATE_FORMAT('" + fechaHoy + "','%Y') ";
                DataTable dt = new DataTable();
                var adp = Namespace.Conexion.adapter(query);
                if (adp != null)
                {
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


                //DataSet ds = new DataSet("New_DataSet");
                //DataTable dt2 = new DataTable("New_DataTable");
              
                //    adp.Fill(dt2);
                var webAppPath = Context.Server.MapPath("~/") + "Reportes generados/";
                //    ds.Tables.Add(dt2);
                //    ExcelLibrary.DataSetHelper.CreateWorkbook(webAppPath + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Reprobados_Este_Año.xls", ds);
                //    Response.ContentType = "application/octet-stream";
                //    Response.AppendHeader("Content-Disposition", "attachment; filename=" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Reprobados_Este_Año.xls");
                //    Response.TransmitFile(Server.MapPath("~/" + "Reportes generados/" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Reprobados_Este_Año.xls"));
                //    Response.End();


               // var webAppPath = Context.Server.MapPath("~/");
                string Path = webAppPath + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_Reporte_Pendientes_Este_Año.xls";
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
    }

    /**
     * busvs ls lista de cursos existentes en la base de datos pra luego guardarlos
     * @trhow en caso que la conexion con la base de datos falle
     * */
    private void BindCursos()
    {
        try
        {
            var adp = Namespace.Conexion.adapter("SELECT curso FROM curso_creado");
           if(adp != null)
            {

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
        catch (Exception ex)
        {

        }
    }

    /**
     * funcion encargada de descargar los excel generados en el sistema
     * @throw en caso que el archivo no exista o ocurra otro tipo de error 
     * */
    public static void WriteAttachment(string FileName, string FileType, string content)
    {
        try
        {
            HttpResponse Response = System.Web.HttpContext.Current.Response;
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
            HttpContext.Current.Response.ContentType = FileType;
            HttpContext.Current.Response.Write(content);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
        }catch(Exception ex)
        {
            Debug.Print("ocurrio un error al descargar el excel : " + ex);
        }
    }

    /**
     * se encarga de obtener los dias ingresados en el calendario 1 para luego
     * copiarlo con el formato correspondiente en el text fecha
     * */
    protected void Cal1_SelectionChanged(object sender, System.EventArgs e)
    {
        DateTime vari = Convert.ToDateTime(Cal1.SelectedDate.ToShortDateString());
        txtFecha1.Text = vari.ToString("yyyy/MM/dd");
        Cal1.Visible = false;
    }

    /**
     * boton
     * muestra el calendario o lo desaparece, se utiliza al momento de ingresar un registro 
     */
    protected void ImageButton1_Click1(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        if (Cal1.Visible == true)
        {
            Cal1.Visible = false;
        }
        else
        {
            Cal1.Visible = true;
            Cal2.Visible = false;
        }
    }

    /**
     * boton
     * muestra el calendario o lo desaparece, se utiliza al momento de ingresar un registro 
     */
    protected void ImageButton1_Click2(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        if (Cal2.Visible == true)
        {
            Cal2.Visible = false;
        }
        else
        {
            Cal2.Visible = true;
            Cal1.Visible = false;
        }
    }

    /**
 * se encarga de obtener los dias ingresados en el calendario 2 para luego
 * copiarlo con el formato correspondiente en el text fecha
 * */
    protected void Cal2_SelectionChanged(object sender, System.EventArgs e)
    {
        DateTime vari = Convert.ToDateTime(Cal2.SelectedDate.ToShortDateString());
        txtFecha2.Text = vari.ToString("yyyy/MM/dd");
        Cal2.Visible = false;
        if (txtFecha1.Text != string.Empty && txtFecha2.Text != string.Empty) { btnEx.Visible = true; }
        else { lblError.Text = "Debe seleccionar las fechas"; lblError.Visible = true; }
    }

    //no se usa
    protected void txtFecha1_TextChanged(object sender, System.EventArgs e)
    {

    }

    //no se usa
    protected void ddlCurso_SelectedIndexChanged(object sender, System.EventArgs e)
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
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        return (from m in nombresEmp2 where m.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select m).Take(count).ToArray();
    }

    //no se usa
    protected void txtEmpresa_TextChanged(object sender, System.EventArgs e)
    {

    }


}