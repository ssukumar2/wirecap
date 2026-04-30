using System;
using System.Collections.Generic;

namespace WireCap
{
    public class RegisterDefinition
    {
        public ushort Address { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public double ScaleFactor { get; set; } = 1.0;
        public int RegisterCount { get; set; } = 1;
    }

    public static class RegisterMaps
    {
        public static List<RegisterDefinition> SolarInverter => new()
        {
            new() { Address = 0, Name = "grid_voltage", Unit = "V", ScaleFactor = 0.1 },
            new() { Address = 1, Name = "grid_current", Unit = "A", ScaleFactor = 0.01 },
            new() { Address = 2, Name = "grid_power", Unit = "W", ScaleFactor = 1.0 },
            new() { Address = 3, Name = "grid_frequency", Unit = "Hz", ScaleFactor = 0.01 },
            new() { Address = 4, Name = "pv_voltage", Unit = "V", ScaleFactor = 0.1 },
            new() { Address = 5, Name = "pv_current", Unit = "A", ScaleFactor = 0.01 },
            new() { Address = 6, Name = "pv_power", Unit = "W", ScaleFactor = 1.0 },
            new() { Address = 7, Name = "temperature", Unit = "°C", ScaleFactor = 0.1 },
            new() { Address = 8, Name = "daily_energy", Unit = "kWh", ScaleFactor = 0.1 },
            new() { Address = 9, Name = "total_energy", Unit = "kWh", ScaleFactor = 0.1, RegisterCount = 2 },
        };

        public static List<RegisterDefinition> EnergyMeter => new()
        {
            new() { Address = 0, Name = "voltage_l1", Unit = "V", ScaleFactor = 0.1 },
            new() { Address = 1, Name = "voltage_l2", Unit = "V", ScaleFactor = 0.1 },
            new() { Address = 2, Name = "voltage_l3", Unit = "V", ScaleFactor = 0.1 },
            new() { Address = 3, Name = "current_l1", Unit = "A", ScaleFactor = 0.001 },
            new() { Address = 4, Name = "current_l2", Unit = "A", ScaleFactor = 0.001 },
            new() { Address = 5, Name = "current_l3", Unit = "A", ScaleFactor = 0.001 },
            new() { Address = 6, Name = "total_power", Unit = "W", ScaleFactor = 1.0 },
            new() { Address = 7, Name = "power_factor", Unit = "", ScaleFactor = 0.001 },
        };

        public static List<RegisterDefinition> BatteryBMS => new()
        {
            new() { Address = 0, Name = "pack_voltage", Unit = "V", ScaleFactor = 0.01 },
            new() { Address = 1, Name = "pack_current", Unit = "A", ScaleFactor = 0.01 },
            new() { Address = 2, Name = "soc", Unit = "%", ScaleFactor = 0.1 },
            new() { Address = 3, Name = "soh", Unit = "%", ScaleFactor = 0.1 },
            new() { Address = 4, Name = "cell_temp_max", Unit = "°C", ScaleFactor = 0.1 },
            new() { Address = 5, Name = "cell_temp_min", Unit = "°C", ScaleFactor = 0.1 },
            new() { Address = 6, Name = "cell_voltage_max", Unit = "mV", ScaleFactor = 1.0 },
            new() { Address = 7, Name = "cell_voltage_min", Unit = "mV", ScaleFactor = 1.0 },
            new() { Address = 8, Name = "charge_cycles", Unit = "", ScaleFactor = 1.0 },
        };
    }
}
