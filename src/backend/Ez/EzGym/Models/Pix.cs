namespace EzGym.Models
{
    public struct Pix
    {

        public PixTypeEnum Type { get; set; }
        public string Value { get; set; }
        public Pix(PixTypeEnum type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
