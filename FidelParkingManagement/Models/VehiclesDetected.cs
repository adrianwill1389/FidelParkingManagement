using System;
using System.Collections.Generic;

namespace FidelParkingManagement.Models;

public partial class VehiclesDetected
{
    public int TicketNumber { get; set; }

    public string Operation { get; set; } = null!;

    public string LicensePlateNumber { get; set; } = null!;

    public string Make { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string Color { get; set; } = null!;

    public DateOnly EntryDate { get; set; }

    public TimeOnly EntryTime { get; set; }

    public DateOnly? ExitDate { get; set; }

    public TimeOnly? ExitTime { get; set; }

    public int? UserId { get; set; }

    public int? PaymentId { get; set; }

    public int? MediaId { get; set; }

    public virtual Medium? Media { get; set; }

    public virtual Payment? Payment { get; set; }
}
