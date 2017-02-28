using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Cliente
/// </summary>
public class Cliente
{
    public string rut { get; set; }
    public string nombres { get; set; }
    public string apellidoP { get; set; }
    public string apellidoM { get; set; }
    public string sexo { get; set; }
    public string ingreso { get; set; }
    public string nomEmpresa { get; set; }
	public Cliente()
	{
        rut = string.Empty;
        nombres = string.Empty;
        apellidoP = string.Empty;
        apellidoM = string.Empty;
        sexo = string.Empty;
        ingreso = string.Empty;
        nomEmpresa = string.Empty;
	
	}
}