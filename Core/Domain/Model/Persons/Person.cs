namespace Core.Model.Persons
{
    public abstract class Person
    {
        public Person(string name, string contact)
        {
            Name = name;
            Contact = contact;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Person()
        {
        }

        public uint? Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
    }
}