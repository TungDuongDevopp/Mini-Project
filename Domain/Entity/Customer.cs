using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity;


[Table("Customer")]
public class Customer
{

    [Key]
    public int CustomerId { get; set; }

    [StringLength(50)]
    public string Name { get; set; }

    [StringLength(200)]
    public string Email { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    public List<Order> Orders { get; set; } = new();    

}
