namespace Core.Abstractions.Types.Exception
{
    public class NaoEncontradoException : System.Exception
    {
        public NaoEncontradoException()
        {

        }

        public NaoEncontradoException(string message)
            : base(message)
        {
        }
    }
}
