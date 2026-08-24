# CitraNica 🍊
> Del campo a tu mesa

## 🚨 Problema
Los pequeños agroproductores pierden un cierto porcentaje de sus cosechas al no contar con un canal directo de venta, mientras que los consumidores no encuentran productos frescos fácilmente. Por ello, se requiere una aplicación que funcione como un mercado digital para impulsar y gestionar pedidos.

## 💡 Propuesta de Citra-Nica
Contar con una aplicación que le permita a los pequeños agroproductores vender directamente, logrando obtener un precio más justo por sus productos. Asimismo, se espera disminuir el desperdicio de sus cosechas.

## 👥 Usuarios Objetivo
* Agricultores independientes.
* Supermercados.
* Empresas o personas interesadas en la venta y compra de productos frutales y cítricos de forma directa.

## 🛠 Tecnologías Usadas
* Frontend: React Native & Expo, TypeScript, Expo Router (Enrutamiento basado en archivos).``
* Backend: .NET SDK 10, MySQL 8.0.

## 📂 Estructura del Proyecto
El ecosistema de CitraNica mantiene una separación clara de responsabilidades entre la interfaz visual y el servidor.

### 📱 Frontend (React Native & Expo)
Esta sección maneja toda la interfaz visual, la navegación y la interacción del usuario.

* /app: Motor principal de navegación gestionado por Expo Router.
  * /(auth): Pantallas de acceso (inicio de sesión y registro).
  * /(main): Flujos de trabajo para los perfiles de Productor e Institución.
  * _layout.tsx: Plantilla base que envuelve la aplicación.
  * index.tsx: Puerta de entrada principal.
  * +not-found.tsx: Pantalla de error (404).
* /assets: Recursos estáticos (imágenes, logotipos, tipografías).
* /hooks: Lógica reutilizable de React.
* Archivos de Configuración: app.json, package.json, tsconfig.json, .gitignore.

### ⚙️ Backend (API en C# .NET)
Organizado bajo el patrón de Arquitectura Limpia (Clean Architecture).

* CitraNica.Api: Capa frontal. Controladores HTTP, autenticación, Swagger.
* CitraNica.Application: Lógica de casos de uso, contratos y DTOs.
* CitraNica.Domain: Núcleo independiente con entidades y reglas de negocio.
* CitraNica.Infrastructure: Acceso a datos, configuración de MySQL y Entity Framework.

## 🚀 Instalación y Ejecución

### 📱 Frontend

Requisitos: Node.js instalado. (Opcional: Expo Go en tu móvil).

1. Navega a la carpeta del frontend:
    cd frontend

2. Instala las dependencias:
    npm install

3. Inicia el servidor:
    npx expo start

### ⚙️ Backend

Requisitos: SDK de .NET 10 y servidor MySQL local.

1. Configura la conexión en CitraNica.Api/appsettings.json:
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=CitraNicaDB;User=root;Password=tu_contraseña;"
    }

2. Aplica las migraciones:
    dotnet ef database update

3. Levanta el servidor:
    dotnet run --project CitraNica.Api

## 🎯 Producto Mínimo Viable (MVP)
El objetivo de esta versión es construir el núcleo del sistema, garantizando la trazabilidad y facilitando la compra. Requerimientos de prioridad Alta:

* Gestión de Perfiles: Registro seguro según rol (Productor o Institución/Consumidor).
* Catálogo y Publicación: Vitrina digital de frutas y panel de publicación para el productor.
* Trazabilidad: Detalle del origen del producto.
* Flujo de Pedidos: Estados logísticos (Pendiente, En camino, Completado).
* Comunicación: Chat directo entre consumidor y productor.





