using GymPlanner.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Training : IComparable<Training>
{
    [Key]
    public int id { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; }

    public User? User { get; set; }
    
    [DataType(DataType.Date)]
    public DateOnly date { get; set; }
    
    public int durationTime { get; set; }
    
    public List<Excercise>? Excercises { get; set; }

    public int CompareTo(Training? other)
    {
        if (other == null)
        {
            return 1;
        }

        return -date.CompareTo(other.date);
    }
}