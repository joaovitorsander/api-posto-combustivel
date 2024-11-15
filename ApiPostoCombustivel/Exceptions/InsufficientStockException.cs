namespace ApiPostoCombustivel.Exceptions
{
    public class InsufficientStockException : Exception
    {
        public InsufficientStockException(string message) : base(message)
        {
        }
    }
}
