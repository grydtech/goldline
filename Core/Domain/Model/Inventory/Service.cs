using Core.Domain.Enums;

namespace Core.Domain.Model.Inventory
{
    public sealed class Service : Product
    {
        public Service(string name)
        {
            Name = name;
        }

        public Service()
        {
        }

        public override string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}