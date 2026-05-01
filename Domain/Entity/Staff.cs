using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity;
[Table("Staff")]
public class Staff
{
    [Key]
    public int StaffId { get; set; }

    [StringLength(50)]
    public string Name { get; set; }

    [StringLength(100)]
    public string Position { get; set; }

    [Column(TypeName = "money")]
    public decimal Salary { get; set; }
}
