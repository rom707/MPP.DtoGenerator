namespace DtoGenerator.Entity
{
    class ClassInfo
    {
        public string Name { get; set; }
        public FieldInfo[] Fields { get; set; }

        public ClassInfo(string name, FieldInfo[] fields)
        {
            this.Name = name;
            this.Fields = fields;
        }
    }
}
