namespace VehicalApi.Dtos.VehicleImage
{
    public class VehicleImageResponseDto
    {
        public int ImageId { get; set; }
        public int VehicleId { get; set; }
        public string ImageLocation { get; set; } = null!;
        public int SortOrder { get; set; }
    }
}
