using System;
using System.Collections.Generic;

namespace Servis.Models;

public partial class Personeller
{
    public int Id { get; set; }

    public string Adi { get; set; } = null!;

    public string Soyadi { get; set; } = null!;
}
