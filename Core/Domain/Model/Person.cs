namespace Core.Domain.Model
{
    public abstract class Person
    {
        protected Person(string name, string contact)
        {
            Name = name;
            Contact = contact;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        protected Person()
        {
        }

        public uint? Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}