using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class EditProjectViewModel
{
    public string Id { get; set; } = null!;

    public string? Image { get; set; }

    [Display(Name = "Project Name", Prompt = "Project Name")]
    [Required(ErrorMessage = "Required")]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Client Name", Prompt = "Client Name")]
    [Required(ErrorMessage = "Required")]
    public string ClientName { get; set; } = null!;

    [Display(Name = "Description", Prompt = "Type something")]
    public string? Description { get; set; }

    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Required")]
    public DateTime? StartDate { get; set; }

    [Display(Name = "End Date")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Members")]
    public string? Member { get; set; }

    [Display(Name = "Budget", Prompt = "0")]
    [DataType(DataType.Currency)]
    public decimal? Budget { get; set; }

    [Display(Name = "Status")]
    [Required(ErrorMessage = "Required")]
    public string Status { get; set; } = null!;
}