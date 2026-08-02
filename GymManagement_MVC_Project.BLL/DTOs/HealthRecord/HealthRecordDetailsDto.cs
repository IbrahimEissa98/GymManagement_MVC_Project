using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement_MVC_Project.BLL.DTOs.HealthRecord
{
    public class HealthRecordDetailsDto
    {
        public string? PhotoUrl { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int Height { get; set; }
        public int Weight { get; set; }
        public string BloodType { get; set; } = default!;
        public string? Note { get; set; } = default!;
    }
}
