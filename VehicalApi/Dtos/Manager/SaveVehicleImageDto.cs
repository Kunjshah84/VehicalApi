public class SaveVehicleImageDto
{
    public int ImageId { get; set; }       
    public string ImageLocation { get; set; } = null!;
    public int SortOrder { get; set; }
}
