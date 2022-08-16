namespace EzGym.Models
{
    public struct Pix
    {

        public PixTypeEnum Type { get; private set; }
        public string Value { get; private set; }
        public Pix(PixTypeEnum type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
