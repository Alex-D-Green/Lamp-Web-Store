
namespace LampWebStore.Models
{
    /// <summary>
    /// The lamp entity.
    /// </summary>
    public class Lamp
    {
        public int Id { get; set; }

        public LampType LampType { get; set; }

        public string Manufacturer { get; set; }

        public double Cost { get; set; }

        public string ImageRef { get; set; }
    }
}
