using System;
using System.Collections.Generic;

namespace FidelParkingManagement.Models;

public partial class Payment
{
    public int Id { get; set; }

    public decimal? Paid { get; set; }

    public DateTime? TimeStamp { get; set; }

    public virtual ICollection<VehiclesDetected> VehiclesDetecteds { get; set; } = new List<VehiclesDetected>();
}
