namespace Catalog.API.Exceptions
{
    public class ProductNotFoundException:Exception
    {
        public ProductNotFoundException():base("Product not found!")
        {
            
        }

        public ProductNotFoundException(Guid id) : base($"Product with ID {id} was not found!")
        {
        }
    }
}
