﻿using System;
using System.Collections.Generic;

namespace SmartPowerElectricAPI.Models;

public partial class Material
{
    public int Id { get; set; }

    public double? Precio { get; set; }

    public double? Cantidad { get; set; }

    public int IdTipoMaterial { get; set; }

    public int IdUnidadMedida { get; set; }

    public virtual TipoMaterial IdTipoMaterialNavigation { get; set; } = null!;

    public virtual UnidadMedidum IdUnidadMedidaNavigation { get; set; } = null!;
}