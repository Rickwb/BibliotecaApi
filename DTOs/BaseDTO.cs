namespace BibliotecaApi.DTOs
{
    public abstract class BaseDTO
    {

        public bool IsValid { get; set; }

        public abstract void Valid();

    }
}
