using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class ModRut : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        var adp = Namespace.Conexion.adapter("SELECT id from curso_datos");
        DataTable dt = new DataTable();
        using (adp)
        {
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DropDownList1.DataSource = null;
                DropDownList1.DataBind();
                DropDownList1.DataSource = dt;
                DropDownList1.DataTextField = "id";
                DropDownList1.DataBind();
            }
            else { lblError.Text = "No hay registros"; lblError.Visible = true; }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
      
        for (int i = 0; i < DropDownList1.Items.Count; i++)
        {
            DropDownList1.SelectedIndex = i;
            int id = Convert.ToInt32(DropDownList1.SelectedValue);
            buscarRut(id);
            lblRut.Text = DropDownList2.SelectedValue;
            string rut=null;
            rut=lblRut.Text;
            lblRut.Text = Rut(rut);
            string rut2=null;
            rut2=lblRut.Text;
            actualizarRut(rut2, id);
        }
    }
    public void buscarRut(int id)
    {
        var adp = Namespace.Conexion.adapter("SELECT rut from curso_datos where id=" + id);
        DataTable dt = new DataTable();
        using (adp)
        {
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DropDownList2.DataSource = null;
                DropDownList2.DataBind();
                DropDownList2.DataSource = dt;
                DropDownList2.DataTextField = "rut";
                DropDownList2.DataBind();
            }
            else { lblError.Text = "No hay registros"; lblError.Visible = true; }
        }
    }
   
    public void actualizarRut(string val1, int val2)
    {
        string strSQL = "UPDATE curso_datos set rut='" + val1 + "' WHERE id=" + val2 + ";";
        Namespace.Conexion.update(strSQL);
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
}