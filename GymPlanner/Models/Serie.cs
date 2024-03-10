using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Serie
{
    [Key]
    public int id { get; set; }

    [ForeignKey("Excercise")]
    public int ExcerciseId { get; set; }

    public Excercise? Excercise { get; set; }

    [Required]
    public int numberOfRepetitions { get; set; }

    [Required]
    public double weight { get; set; }
}