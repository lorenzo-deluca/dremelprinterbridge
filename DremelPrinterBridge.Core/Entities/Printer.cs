using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DremelPrinterBridge.Core.Entities
{
    public class Printer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }
        public DateTime LastUpdate { get; protected set; }
        public string SoftwareVersion { get; protected set; }
        public bool PortOpen { get; protected set; }
        public int Extruder_Temperature { get; protected set; }
        public int Extruder_TargetTemperature { get; set; }
        public int BuildPlate_TargetTemperature { get; protected set; }
        public int BuildPlate_Temperature { get; set; }
        public int Chamber_Temperature { get; set; }
        public int Printing_Elaspedtime { get; set; }
        public int Printing_TotalTime { get; set; }
        public int Printing_Progress { get; set; }
        public int Printing_Remaining { get; set; }
        public string FilamentType { get; set; }
        public string JobName { get; set; }
        public string JobStatus { get; set; }
        public int Layer { get; set; }
        public string Status { get; set; }

        public void Update(string firmware_version, int door_open, 
            int elaspedtime, int progress, int remaining, int totalTime,
            string jobname, string jobstatus,
            int temperature, int extruder_target_temperature,
            int platform_temperature, int buildPlate_target_temperature,
            int chamber_temperature, string filament_type, int layer,
            string status
            )
        {
            LastUpdate = DateTime.Now;

            SoftwareVersion = firmware_version;
            PortOpen = door_open == 1;

            Printing_Elaspedtime = elaspedtime;
            Printing_Progress = progress;
            Printing_Remaining = remaining;
            Printing_TotalTime = totalTime;
            JobName = jobname;
            JobStatus = jobstatus;

            Extruder_Temperature = temperature;
            Extruder_TargetTemperature = extruder_target_temperature;

            BuildPlate_Temperature = platform_temperature;
            BuildPlate_TargetTemperature = buildPlate_target_temperature;

            Chamber_Temperature = chamber_temperature;

            FilamentType = filament_type;
            Layer = layer;
            Status = status;
        }
    }
}