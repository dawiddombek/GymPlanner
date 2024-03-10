using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Excercise
{
    [Key]
    public int id { get; set; }

    [ForeignKey("Training")]
    public int TrainingId { get; set; }

    public Training? Training { get; set; }

    [Required]
    public string name { get; set; }

    [Required]
    public string muscleTarget { get; set; }

    public List<Serie>? Series { get; set; }
}