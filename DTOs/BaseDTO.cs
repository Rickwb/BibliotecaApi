namespace BibliotecaApi.DTOs
{
    public abstract class BaseDTO
    {

        public bool IsValid { get; protected set; }

        public abstract void Valid();

    }
}
