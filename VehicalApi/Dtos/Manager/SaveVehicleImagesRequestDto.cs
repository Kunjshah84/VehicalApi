using System.ComponentModel.DataAnnotations;

public class SaveVehicleImagesRequestDto
{
    [Required]
    public List<SaveVehicleImageDto> Images { get; set; } = new();
}
