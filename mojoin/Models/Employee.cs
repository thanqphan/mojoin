using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? ZaloInfo { get; set; }

    public string? FacebookInfo { get; set; }

    public int? AccountId { get; set; }

    public virtual UserAccount? Account { get; set; }
}
