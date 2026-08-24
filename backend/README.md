# CitraNica API

API REST del marketplace agrícola CitraNica, desarrollada con ASP.NET Core 10,
Entity Framework Core y MySQL 8.0.

## Arquitectura

- `CitraNica.Api`: controladores HTTP, autenticación, Swagger y arranque.
- `CitraNica.Application`: contratos, solicitudes y respuestas de los casos de uso.
- `CitraNica.Domain`: entidades y reglas centrales del negocio.
- `CitraNica.Infrastructure`: servicios, acceso a datos y configuración de MySQL.

## Funcionalidades implementadas

- Registro e inicio de sesión de consumidores y productores mediante JWT.
- Consulta y administración del catálogo de categorías y productos.
- Consulta de publicaciones con detalle y trazabilidad.
- Creación y actualización de publicaciones por el productor.
- Creación, consulta y gestión de pedidos.
- Control de inventario al comprar y restauración al rechazar un pedido.
- Flujo de estados del pedido y validación de transiciones.
- Chat temporal entre el consumidor y el productor de un pedido.
- Marcado de mensajes como leídos y eliminación del chat al terminar el pedido.
- Endpoints para comprobar el estado de la API y la conexión con MySQL.

## Reglas de negocio incorporadas

- Productor y consumidor son cuentas separadas.
- Se conserva el tipo de usuario `admin` para una posible necesidad futura.
- Cada productor puede tener una sola finca.
- Cada pedido pertenece a un solo productor.
- El productor acepta o rechaza los pedidos pendientes.
- La comisión del 3 % se agrega al total pagado por el consumidor.
- El inventario se valida y actualiza de manera atómica desde la API.
- El chat solamente puede ser utilizado por los participantes del pedido.
- El chat deja de existir cuando el pedido se completa, rechaza o cancela.
- Los pedidos pueden utilizar una dirección de entrega o un lugar de acopio.

## Funcionalidades modeladas pendientes de API

La base y las entidades contemplan valoraciones, verificaciones renovables,
instituciones y lugares de acopio. Todavía faltan los controladores y servicios
necesarios para administrar completamente esos módulos.

La valoración deberá permitir que el consumidor valore únicamente al productor,
una sola vez y después de completar el pedido.

El estado `Cancelado` está reservado en el modelo, pero todavía no existe una
operación para cancelar pedidos desde la API.

## Requisitos

- .NET SDK 10.
- MySQL 8.0.
- Esquema `citranica` importado desde el archivo SQL oficial del proyecto.

## Configuración local

Desde esta carpeta, configura los datos privados con User Secrets. No escribas
contraseñas reales ni claves JWT en los archivos `appsettings`.

```powershell
dotnet user-secrets set --project src/CitraNica.Api "ConnectionStrings:CitraNica" "Server=127.0.0.1;Port=3306;Database=citranica;User=TU_USUARIO;Password=TU_PASSWORD;AllowPublicKeyRetrieval=True;SslMode=None"
dotnet user-secrets set --project src/CitraNica.Api "Jwt:Key" "COLOCA_UNA_CLAVE_PRIVADA_DE_32_CARACTERES_O_MAS"
```

## Compilación y ejecución

```powershell
dotnet restore CitraNica.slnx
dotnet build CitraNica.slnx -c Release --no-restore
dotnet run --project src/CitraNica.Api
```

La dirección de Swagger aparece en la terminal cuando inicia la API.

También puedes comprobar la aplicación mediante:

- `GET /health`
- `GET /health/database`
