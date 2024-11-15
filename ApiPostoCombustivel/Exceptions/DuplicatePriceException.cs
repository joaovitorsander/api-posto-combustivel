namespace ApiPostoCombustivel.Exceptions
{
    public class DuplicatePriceException : Exception
    {
        public DuplicatePriceException(string message) : base(message) 
        { 
        }
    }
}
