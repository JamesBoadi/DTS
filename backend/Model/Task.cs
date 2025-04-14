
using System.ComponentModel.DataAnnotations;

public class Task   
{
    [Required(ErrorMessage = "Id is required.")]
    public string Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }
    public bool Status { get; set; }
}

public class TaskDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }
    public bool Status { get; set; }
}