namespace MisteryBlazor.Data
{
    public class Hcaptcha
    {
        public bool success { get; set; }
        public DateTime challenge_ts { get; set; }
        public string hostname { get; set; }
    }
}
