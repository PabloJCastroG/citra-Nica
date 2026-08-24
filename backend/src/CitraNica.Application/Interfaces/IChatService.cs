using CitraNica.Application.Chat;

namespace CitraNica.Application.Interfaces;

public interface IChatService
{
    Task<ChatPedidoResponse> ObtenerAsync(
        int idPedido,
        int idUsuario,
        CancellationToken cancellationToken = default);

    Task<MensajeChatResponse> EnviarAsync(
        int idPedido,
        int idUsuario,
        EnviarMensajeRequest request,
        CancellationToken cancellationToken = default);

    Task<MensajesLeidosResponse> MarcarLeidosAsync(
        int idPedido,
        int idUsuario,
        CancellationToken cancellationToken = default);
}
