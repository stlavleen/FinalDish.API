
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinalDish.API.DTO
{
    public class RangeRequestDTO
    {
        [Range(0, 1000)]
        [DefaultValue(0)]
        public int RangeId { get; set; } = 0;

        [Range(0, 100)]
        [DefaultValue(15)]
        public int RangeSize { get; set; } = 15;
    }
}
