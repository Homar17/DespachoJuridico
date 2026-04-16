namespace backend.Services
{
    public interface IEmailService
    {
        Task EnviarCorreoConfirmacionAsync(string destino, string nombreCliente, string fecha, string hora);
        
        Task EnviarCorreoBienvenidaAsync(string destino, string nombreUsuario);
    }
}