using CitraNica.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CitraNica.Infrastructure.Persistence;

public sealed class CitraNicaDbContext(
    DbContextOptions<CitraNicaDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Finca> Fincas => Set<Finca>();
    public DbSet<Direccion> Direcciones => Set<Direccion>();
    public DbSet<MetodoPago> MetodosPago => Set<MetodoPago>();
    public DbSet<Institucion> Instituciones => Set<Institucion>();
    public DbSet<VerificacionProductor> VerificacionesProductor =>
        Set<VerificacionProductor>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<ProductoCatalogo> CatalogoProductos => Set<ProductoCatalogo>();
    public DbSet<Publicacion> Publicaciones => Set<Publicacion>();
    public DbSet<PublicacionImagen> PublicacionImagenes => Set<PublicacionImagen>();
    public DbSet<LugarAcopio> LugaresAcopio => Set<LugarAcopio>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<DetalleMovimiento> DetalleMovimientos => Set<DetalleMovimiento>();
    public DbSet<Conversacion> Conversaciones => Set<Conversacion>();
    public DbSet<Mensaje> Mensajes => Set<Mensaje>();
    public DbSet<Valoracion> Valoraciones => Set<Valoracion>();
    public DbSet<Notificacion> Notificaciones => Set<Notificacion>();
    public DbSet<PublicacionInstitucional> PublicacionesInstitucionales =>
        Set<PublicacionInstitucional>();
    public DbSet<FormularioPregunta> FormularioPreguntas => Set<FormularioPregunta>();
    public DbSet<FormularioRespuesta> FormularioRespuestas => Set<FormularioRespuesta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureUsuario(modelBuilder);
        ConfigureFinca(modelBuilder);
        ConfigureDireccion(modelBuilder);
        ConfigureMetodoPago(modelBuilder);
        ConfigureInstitucion(modelBuilder);
        ConfigureVerificacionProductor(modelBuilder);
        ConfigureCategoria(modelBuilder);
        ConfigureProductoCatalogo(modelBuilder);
        ConfigurePublicacion(modelBuilder);
        ConfigurePublicacionImagen(modelBuilder);
        ConfigureLugarAcopio(modelBuilder);
        ConfigurePedido(modelBuilder);
        ConfigureDetalleMovimiento(modelBuilder);
        ConfigureConversacion(modelBuilder);
        ConfigureMensaje(modelBuilder);
        ConfigureValoracion(modelBuilder);
        ConfigureNotificacion(modelBuilder);
        ConfigurePublicacionInstitucional(modelBuilder);
        ConfigureFormularioPregunta(modelBuilder);
        ConfigureFormularioRespuesta(modelBuilder);
    }

    private static void ConfigureUsuario(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("usuarios");
            entity.HasKey(usuario => usuario.IdUsuario);
            entity.HasIndex(usuario => usuario.Email).IsUnique();
            entity.Property(usuario => usuario.TipoUsuario).HasConversion(
                tipo => tipo == TipoUsuario.Productor
                    ? "productor"
                    : tipo == TipoUsuario.Consumidor
                        ? "consumidor"
                        : "admin",
                valor => valor == "productor"
                    ? TipoUsuario.Productor
                    : valor == "consumidor"
                        ? TipoUsuario.Consumidor
                        : TipoUsuario.Admin);
        });
    }

    private static void ConfigureFinca(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Finca>(entity =>
        {
            entity.ToTable("fincas");
            entity.HasKey(finca => finca.IdFinca);
            entity.HasIndex(finca => finca.IdProductor).IsUnique();

            entity.HasOne(finca => finca.Productor)
                .WithOne(productor => productor.Finca)
                .HasForeignKey<Finca>(finca => finca.IdProductor);
        });
    }

    private static void ConfigureDireccion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Direccion>(entity =>
        {
            entity.ToTable("direcciones");
            entity.HasKey(direccion => direccion.IdDireccion);
        });
    }

    private static void ConfigureMetodoPago(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.ToTable("metodos_pago");
            entity.HasKey(metodo => metodo.IdMetodoPago);
        });
    }

    private static void ConfigureInstitucion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Institucion>(entity =>
        {
            entity.ToTable("instituciones");
            entity.HasKey(institucion => institucion.IdInstitucion);
            entity.HasIndex(institucion => institucion.Email).IsUnique();
        });
    }

    private static void ConfigureVerificacionProductor(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VerificacionProductor>(entity =>
        {
            entity.ToTable("verificaciones_productor");
            entity.HasKey(verificacion => verificacion.IdVerificacion);
            entity.HasIndex(verificacion => new
            {
                verificacion.IdProductor,
                verificacion.IdInstitucion
            }).IsUnique();
            entity.Property(verificacion => verificacion.Estado).HasConversion(
                estado => estado == EstadoVerificacion.Pendiente
                    ? "pendiente"
                    : estado == EstadoVerificacion.Verificado
                        ? "verificado"
                        : estado == EstadoVerificacion.Rechazado
                            ? "rechazado"
                            : "vencida",
                valor => valor == "pendiente"
                    ? EstadoVerificacion.Pendiente
                    : valor == "verificado"
                        ? EstadoVerificacion.Verificado
                        : valor == "rechazado"
                            ? EstadoVerificacion.Rechazado
                            : EstadoVerificacion.Vencida);
        });
    }

    private static void ConfigureCategoria(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("categorias");
            entity.HasKey(categoria => categoria.IdCategoria);
            entity.HasIndex(categoria => categoria.NombreCategoria).IsUnique();
        });
    }

    private static void ConfigureProductoCatalogo(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductoCatalogo>(entity =>
        {
            entity.ToTable("catalogo_productos");
            entity.HasKey(producto => producto.IdProducto);
            entity.HasIndex(producto => new
            {
                producto.IdCategoria,
                producto.NombreProducto
            }).IsUnique();
        });
    }

    private static void ConfigurePublicacion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Publicacion>(entity =>
        {
            entity.ToTable("publicaciones");
            entity.HasKey(publicacion => publicacion.IdPublicacion);
            entity.Property(publicacion => publicacion.CantidadDisponible)
                .HasPrecision(10, 2);
            entity.Property(publicacion => publicacion.PrecioUnitario)
                .HasPrecision(10, 2);
            entity.Property(publicacion => publicacion.EstadoPublicacion)
                .HasConversion(
                    estado => estado == EstadoPublicacion.Activa
                        ? "activa"
                        : estado == EstadoPublicacion.Agotada
                            ? "agotada"
                            : "pausada",
                    valor => valor == "activa"
                        ? EstadoPublicacion.Activa
                        : valor == "agotada"
                            ? EstadoPublicacion.Agotada
                            : EstadoPublicacion.Pausada);
            entity.HasIndex(publicacion => new
            {
                publicacion.IdProductor,
                publicacion.IdProducto
            }).IsUnique();
        });
    }

    private static void ConfigurePublicacionImagen(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PublicacionImagen>(entity =>
        {
            entity.ToTable("publicacion_imagenes");
            entity.HasKey(imagen => imagen.IdImagen);
        });
    }

    private static void ConfigureLugarAcopio(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LugarAcopio>(entity =>
        {
            entity.ToTable("lugares_acopio");
            entity.HasKey(lugar => lugar.IdLugarAcopio);
        });
    }

    private static void ConfigurePedido(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.ToTable("pedidos");
            entity.HasKey(pedido => pedido.IdPedido);
            entity.Property(pedido => pedido.MetodoEntrega).HasConversion(
                metodo => metodo == MetodoEntrega.EntregaDirecta
                    ? "entrega_directa"
                    : metodo == MetodoEntrega.RetiroFinca
                        ? "retiro_finca"
                        : "lugar_acopio",
                valor => valor == "entrega_directa"
                    ? MetodoEntrega.EntregaDirecta
                    : valor == "retiro_finca"
                        ? MetodoEntrega.RetiroFinca
                        : MetodoEntrega.LugarAcopio);
            entity.Property(pedido => pedido.EstadoPedido).HasConversion(
                estado => estado == EstadoPedido.Pendiente
                    ? "pendiente"
                    : estado == EstadoPedido.Aceptado
                        ? "aceptado"
                        : estado == EstadoPedido.Rechazado
                            ? "rechazado"
                            : estado == EstadoPedido.Preparando
                                ? "preparando"
                                : estado == EstadoPedido.EnCamino
                                    ? "en_camino"
                                    : estado == EstadoPedido.Completado
                                        ? "completado"
                                        : "cancelado",
                valor => valor == "pendiente"
                    ? EstadoPedido.Pendiente
                    : valor == "aceptado"
                        ? EstadoPedido.Aceptado
                        : valor == "rechazado"
                            ? EstadoPedido.Rechazado
                            : valor == "preparando"
                                ? EstadoPedido.Preparando
                                : valor == "en_camino"
                                    ? EstadoPedido.EnCamino
                                    : valor == "completado"
                                        ? EstadoPedido.Completado
                                        : EstadoPedido.Cancelado);
            entity.Property(pedido => pedido.Subtotal).HasPrecision(10, 2);
            entity.Property(pedido => pedido.ComisionPorcentaje)
                .HasPrecision(5, 2);
            entity.Property(pedido => pedido.MontoComision)
                .HasPrecision(10, 2);
            entity.Property(pedido => pedido.Total).HasPrecision(10, 2);
        });
    }

    private static void ConfigureDetalleMovimiento(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DetalleMovimiento>(entity =>
        {
            entity.ToTable("detalle_movimiento");
            entity.HasKey(detalle => detalle.IdDetalle);
            entity.Property(detalle => detalle.CantidadMovimiento)
                .HasPrecision(10, 2);
            entity.Property(detalle => detalle.PrecioAcordado)
                .HasPrecision(10, 2);
        });
    }

    private static void ConfigureConversacion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Conversacion>(entity =>
        {
            entity.ToTable("conversaciones");
            entity.HasKey(conversacion => conversacion.IdConversacion);
            entity.HasIndex(conversacion => conversacion.IdPedido).IsUnique();
        });
    }

    private static void ConfigureMensaje(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mensaje>(entity =>
        {
            entity.ToTable("mensajes");
            entity.HasKey(mensaje => mensaje.IdMensaje);
        });
    }

    private static void ConfigureValoracion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Valoracion>(entity =>
        {
            entity.ToTable(
                "valoraciones",
                table => table.HasCheckConstraint(
                    "ck_valoraciones_puntuacion",
                    "puntuacion BETWEEN 1 AND 5"));
            entity.HasKey(valoracion => valoracion.IdValoracion);
            entity.HasIndex(valoracion => valoracion.IdPedido).IsUnique();
        });
    }

    private static void ConfigureNotificacion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.ToTable("notificaciones");
            entity.HasKey(notificacion => notificacion.IdNotificacion);
        });
    }

    private static void ConfigurePublicacionInstitucional(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PublicacionInstitucional>(entity =>
        {
            entity.ToTable("publicaciones_institucionales");
            entity.HasKey(publicacion => publicacion.IdPublicacionInst);
        });
    }

    private static void ConfigureFormularioPregunta(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FormularioPregunta>(entity =>
        {
            entity.ToTable("formulario_preguntas");
            entity.HasKey(pregunta => pregunta.IdPregunta);
        });
    }

    private static void ConfigureFormularioRespuesta(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FormularioRespuesta>(entity =>
        {
            entity.ToTable("formulario_respuestas");
            entity.HasKey(respuesta => respuesta.IdRespuesta);
        });
    }
}
