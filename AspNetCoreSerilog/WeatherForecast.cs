using System;
using System.ComponentModel;

namespace AspNetCoreSerilog
{
    [Description("WeatherForecast Object")]
    public class WeatherForecast
    {
        [Description("This is Date Info")]
        public DateTime Date { get; set; }

        [Description("Temperature of C")]
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
