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
    }
}