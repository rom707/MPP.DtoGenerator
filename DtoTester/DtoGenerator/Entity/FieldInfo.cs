namespace DtoGenerator.Entity
{
    class FieldInfo
    {
        public string Name { get; set; }
        public string Format { get; set; }
        public string Type { get; set; }

        public FieldInfo(string name, string format, string type)
        {
            this.Name = name;
            this.Format = format;
            this.Type = type;
        }
    }
}
