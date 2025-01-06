using System;
using System.Collections.Generic;

namespace SmartPowerElectricAPI.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Direccion { get; set; }

    public string Email { get; set; } = null!;

    public int? Telefono { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }
}
