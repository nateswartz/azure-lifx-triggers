namespace lifxtriggers
{
    public class LightSettings
    {
        public string Power { get; set; }
        public string Color { get; set; }
        public double Brightness { get; set; }
        public double Duration { get; set; }
        public double Infrared { get; set; }
        public bool Fast { get; set; }
    }

    public class LightStatus
    {
        public bool Connected { get; set; }
    }
}
