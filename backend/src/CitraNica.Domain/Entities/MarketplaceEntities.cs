namespace CitraNica.Domain.Entities;

public enum TipoUsuario
{
    Productor,
    Consumidor,
    Admin
}

public enum EstadoPedido
{
    Pendiente,
    Aceptado,
    Rechazado,
    Preparando,
    EnCamino,
    Completado,
    Cancelado
}

public enum EstadoVerificacion
{
    Pendiente,
    Verificado,
    Rechazado,
    Vencida
}

public enum EstadoPublicacion
{
    Activa,
    Agotada,
    Pausada
}

public enum MetodoEntrega
{
    EntregaDirecta,
    RetiroFinca,
    LugarAcopio
}

public sealed class Usuario
{
    public int IdUsuario { get; set; }
    public string NombreCompleto { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public TipoUsuario TipoUsuario { get; set; }
    public string? NumeroIdentificacion { get; set; }
    public string? TelefonoWhatsapp { get; set; }
    public string? Departamento { get; set; }
    public string? Municipio { get; set; }
    public string? FrutaPreferida { get; set; }
    public int? AnosExperiencia { get; set; }
    public string EstadoVerificacion { get; set; } = "no_verificado";
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    public Finca? Finca { get; set; }
}

public sealed class Finca
{
    public int IdFinca { get; set; }
    public int IdProductor { get; set; }
    public string NombreFinca { get; set; } = null!;
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public string? HistoriaCultivo { get; set; }
    public Usuario Productor { get; set; } = null!;
}

public sealed class Direccion
{
    public int IdDireccion { get; set; }
    public int IdUsuario { get; set; }
    public string? Etiqueta { get; set; }
    public string DireccionTexto { get; set; } = null!;
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public bool EsPrincipal { get; set; }
    public Usuario Usuario { get; set; } = null!;
}

public sealed class MetodoPago
{
    public int IdMetodoPago { get; set; }
    public int IdUsuario { get; set; }
    public string Tipo { get; set; } = null!;
    public string NumeroEnmascarado { get; set; } = null!;
    public string? NombreTitular { get; set; }
    public string? FechaExpiracion { get; set; }
    public Usuario Usuario { get; set; } = null!;
}

public sealed class Institucion
{
    public int IdInstitucion { get; set; }
    public string NombreInstitucion { get; set; } = null!;
    public string? TipoOrganizacion { get; set; }
    public string NombreResponsable { get; set; } = null!;
    public string? TelefonoResponsable { get; set; }
    public string? Email { get; set; }
    public string PasswordHash { get; set; } = null!;
    public string? Departamento { get; set; }
    public string? Municipio { get; set; }
    public string? Direccion { get; set; }
    public string? DescripcionBreve { get; set; }
    public string? LogoUrl { get; set; }
    public string? DocumentoVerificacionUrl { get; set; }
    public string EstadoCuenta { get; set; } = "pendiente";
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}

public sealed class VerificacionProductor
{
    public int IdVerificacion { get; set; }
    public int IdProductor { get; set; }
    public int IdInstitucion { get; set; }
    public EstadoVerificacion Estado { get; set; } = EstadoVerificacion.Pendiente;
    public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;
    public DateTime? FechaVerificacion { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public DateTime? FechaRenovacion { get; set; }
}

public sealed class Categoria
{
    public int IdCategoria { get; set; }
    public string NombreCategoria { get; set; } = null!;
}

public sealed class ProductoCatalogo
{
    public int IdProducto { get; set; }
    public string NombreProducto { get; set; } = null!;
    public int IdCategoria { get; set; }
}

public sealed class Publicacion
{
    public int IdPublicacion { get; set; }
    public int IdProductor { get; set; }
    public int IdProducto { get; set; }
    public int? IdFinca { get; set; }
    public decimal CantidadDisponible { get; set; }
    public string UnidadMedida { get; set; } = null!;
    public decimal PrecioUnitario { get; set; }
    public DateOnly? FechaCosechaEstimada { get; set; }
    public string? EstadoMadurez { get; set; }
    public EstadoPublicacion EstadoPublicacion { get; set; } = EstadoPublicacion.Activa;
    public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow;
}

public sealed class PublicacionImagen
{
    public int IdImagen { get; set; }
    public int IdPublicacion { get; set; }
    public string UrlImagen { get; set; } = null!;
    public int Orden { get; set; }
}

public sealed class LugarAcopio
{
    public int IdLugarAcopio { get; set; }
    public string Nombre { get; set; } = null!;
    public string Departamento { get; set; } = null!;
    public string Municipio { get; set; } = null!;
    public string? Direccion { get; set; }
}

public sealed class Pedido
{
    public int IdPedido { get; set; }
    public int IdComprador { get; set; }
    public int IdProductor { get; set; }
    public int? IdDireccionEntrega { get; set; }
    public int? IdMetodoPago { get; set; }
    public int? IdLugarAcopio { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public MetodoEntrega MetodoEntrega { get; set; }
    public EstadoPedido EstadoPedido { get; set; } = EstadoPedido.Pendiente;
    public decimal Subtotal { get; set; }
    public decimal ComisionPorcentaje { get; set; } = 3m;
    public decimal MontoComision { get; set; }
    public decimal Total { get; set; }
    public string? TipoPagoAcordado { get; set; }
}

public sealed class DetalleMovimiento
{
    public int IdDetalle { get; set; }
    public int IdPedido { get; set; }
    public int IdPublicacion { get; set; }
    public decimal CantidadMovimiento { get; set; }
    public decimal PrecioAcordado { get; set; }
}

public sealed class Conversacion
{
    public int IdConversacion { get; set; }
    public int IdPedido { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaCierre { get; set; }
}

public sealed class Mensaje
{
    public int IdMensaje { get; set; }
    public int IdConversacion { get; set; }
    public int IdEmisor { get; set; }
    public int IdReceptor { get; set; }
    public string Contenido { get; set; } = null!;
    public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;
    public bool Leido { get; set; }
}

public sealed class Valoracion
{
    public int IdValoracion { get; set; }
    public int IdPedido { get; set; }
    public int IdEvaluador { get; set; }
    public int IdEvaluado { get; set; }
    public int Puntuacion { get; set; }
    public string? ComentarioTexto { get; set; }
    public DateTime FechaValoracion { get; set; } = DateTime.UtcNow;
}

public sealed class Notificacion
{
    public int IdNotificacion { get; set; }
    public int IdUsuario { get; set; }
    public string Tipo { get; set; } = null!;
    public string Contenido { get; set; } = null!;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public bool Leido { get; set; }
}

public sealed class PublicacionInstitucional
{
    public int IdPublicacionInst { get; set; }
    public int IdInstitucion { get; set; }
    public string TipoContenido { get; set; } = null!;
    public string Titulo { get; set; } = null!;
    public string? ContenidoTexto { get; set; }
    public string? ImagenUrl { get; set; }
    public string? ArchivoUrl { get; set; }
    public DateOnly? FechaEvento { get; set; }
    public TimeOnly? HoraEvento { get; set; }
    public string? Departamento { get; set; }
    public string? Municipio { get; set; }
    public string Alcance { get; set; } = "publico";
    public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow;
}

public sealed class FormularioPregunta
{
    public int IdPregunta { get; set; }
    public int IdPublicacionInst { get; set; }
    public string TextoPregunta { get; set; } = null!;
    public string TipoRespuesta { get; set; } = null!;
    public int Orden { get; set; }
}

public sealed class FormularioRespuesta
{
    public int IdRespuesta { get; set; }
    public int IdPregunta { get; set; }
    public int IdUsuario { get; set; }
    public string? RespuestaTexto { get; set; }
    public DateTime FechaRespuesta { get; set; } = DateTime.UtcNow;
}
