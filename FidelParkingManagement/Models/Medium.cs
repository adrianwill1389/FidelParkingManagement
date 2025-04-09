using System;
using System.Collections.Generic;

namespace FidelParkingManagement.Models;

public partial class Medium
{
    public int Id { get; set; }

    public string Url { get; set; } = null!;

    public virtual ICollection<VehiclesDetected> VehiclesDetecteds { get; set; } = new List<VehiclesDetected>();
}
