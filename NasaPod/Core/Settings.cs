namespace Nasa.Core
{
    public class AppSettings
    {
        public int BlurLevel { get; set; }
        public string ApiKey { get; set; }
        public string Endpoint { get; set; }
        public int HoursInterval { get; set; }
        public string Lang { get; set; }
        public string FillerPath { get; set; }
        public int Ratio { get; set; }
        public int ScaleThresholdHeight { get; set; }
        public int ScaleThresholdWidth { get; set; }
        public int FillerTransparency { get; set; }
    }
}
