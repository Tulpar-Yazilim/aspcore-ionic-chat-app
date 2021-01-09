using System;

namespace ChatApp.Service.DTOs
{
    public class DeviceAddDto
    {
        public string Name { get; set; }
        public string Uuid { get; set; }
        public string DeviceInformation { get; set; }
    }

    public class DeviceUpdateDto : EntityUpdateDto<Guid>
    {
        public string Name { get; set; }
        public string Uuid { get; set; }
        public string DeviceInformation { get; set; }

    }

    public class DeviceCardDto : EntityGetDto<Guid>
    {
        public string Name { get; set; }
        public string Uuid { get; set; }
        public string DeviceInformation { get; set; }
    }

}
