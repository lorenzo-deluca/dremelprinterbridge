namespace DremelPrinterBridge.Core.HostedService
{
    public class DremelPrinterStatusModel
    {
        public int buildPlate_target_temperature { get; set; }
        public int chamber_temperature { get; set; }
        public int door_open { get; set; }
        public int elaspedtime { get; set; }
        public int error_code { get; set; }
        public int extruder_target_temperature { get; set; }
        public int fanSpeed { get; set; }
        public string filament_type { get; set; }
        public string firmware_version { get; set; }
        public string jobname { get; set; }
        public string jobstatus { get; set; }
        public int layer { get; set; }
        public string message { get; set; }
        public int networkBuild { get; set; }
        public int platform_temperature { get; set; }
        public int progress { get; set; }
        public int remaining { get; set; }
        public string status { get; set; }
        public int temperature { get; set; }
        public int totalTime { get; set; }
    }
}
