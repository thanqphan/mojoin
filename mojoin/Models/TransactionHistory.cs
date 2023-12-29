using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class TransactionHistory
{
    public int TransactionId { get; set; }

    public int? UserId { get; set; }

    public string? TransactionType { get; set; }

    public double? Amount { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string? PaymentMethod { get; set; }

    public string? TransactionReference { get; set; }

    public double? PromotionAmount { get; set; }

    public double? ReceivedAmount { get; set; }

    public string? Note { get; set; }

    public int? Status { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<UserPackage> UserPackages { get; set; } = new List<UserPackage>();
}
