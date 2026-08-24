using System.ComponentModel.DataAnnotations;

namespace CitraNica.Application.Chat;

public sealed class EnviarMensajeRequest
{
    [Required, MaxLength(1000)]
    public string Contenido { get; init; } = null!;
}

public sealed record MensajeChatResponse(
    int IdMensaje,
    int IdEmisor,
    string NombreEmisor,
    int IdReceptor,
    string Contenido,
    DateTime FechaEnvio,
    bool Leido);

public sealed record ChatPedidoResponse(
    int IdPedido,
    int? IdConversacion,
    bool Activo,
    IReadOnlyCollection<MensajeChatResponse> Mensajes);

public sealed record MensajesLeidosResponse(int CantidadActualizada);
