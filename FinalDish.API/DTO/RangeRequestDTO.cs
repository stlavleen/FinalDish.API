
using System.ComponentModel;

namespace FinalDish.API.DTO
{
    public class RangeRequestDTO
    {
        [DefaultValue(0)]
        public int RangeId { get; set; } = 0;

        [DefaultValue(100)]
        public int RangeSize { get; set; } = 100;
    }
}
