-- MySQL dump 10.13  Distrib 8.0.45, for Win64 (x86_64)
--
-- Host: localhost    Database: citranica
-- ------------------------------------------------------
-- Server version	8.0.45

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `migration_id` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `product_version` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`migration_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20260815232420_InitialCreate','9.0.0');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `catalogo_productos`
--

DROP TABLE IF EXISTS `catalogo_productos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `catalogo_productos` (
  `id_producto` int NOT NULL AUTO_INCREMENT,
  `nombre_producto` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `id_categoria` int NOT NULL,
  PRIMARY KEY (`id_producto`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `catalogo_productos`
--

LOCK TABLES `catalogo_productos` WRITE;
/*!40000 ALTER TABLE `catalogo_productos` DISABLE KEYS */;
/*!40000 ALTER TABLE `catalogo_productos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categorias`
--

DROP TABLE IF EXISTS `categorias`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categorias` (
  `id_categoria` int NOT NULL AUTO_INCREMENT,
  `nombre_categoria` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`id_categoria`),
  UNIQUE KEY `ix_categorias_nombre_categoria` (`nombre_categoria`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categorias`
--

LOCK TABLES `categorias` WRITE;
/*!40000 ALTER TABLE `categorias` DISABLE KEYS */;
/*!40000 ALTER TABLE `categorias` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `conversaciones`
--

DROP TABLE IF EXISTS `conversaciones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `conversaciones` (
  `id_conversacion` int NOT NULL AUTO_INCREMENT,
  `id_pedido` int NOT NULL,
  `fecha_creacion` datetime(6) NOT NULL,
  `fecha_cierre` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`id_conversacion`),
  UNIQUE KEY `ix_conversaciones_id_pedido` (`id_pedido`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `conversaciones`
--

LOCK TABLES `conversaciones` WRITE;
/*!40000 ALTER TABLE `conversaciones` DISABLE KEYS */;
/*!40000 ALTER TABLE `conversaciones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `detalle_movimiento`
--

DROP TABLE IF EXISTS `detalle_movimiento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `detalle_movimiento` (
  `id_detalle` int NOT NULL AUTO_INCREMENT,
  `id_pedido` int NOT NULL,
  `id_publicacion` int NOT NULL,
  `cantidad_movimiento` decimal(10,2) NOT NULL,
  `precio_acordado` decimal(10,2) NOT NULL,
  PRIMARY KEY (`id_detalle`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `detalle_movimiento`
--

LOCK TABLES `detalle_movimiento` WRITE;
/*!40000 ALTER TABLE `detalle_movimiento` DISABLE KEYS */;
/*!40000 ALTER TABLE `detalle_movimiento` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `direcciones`
--

DROP TABLE IF EXISTS `direcciones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `direcciones` (
  `id_direccion` int NOT NULL AUTO_INCREMENT,
  `id_usuario` int NOT NULL,
  `etiqueta` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `direccion_texto` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `latitud` decimal(65,30) DEFAULT NULL,
  `longitud` decimal(65,30) DEFAULT NULL,
  `es_principal` tinyint(1) NOT NULL,
  `usuario_id_usuario` int NOT NULL,
  PRIMARY KEY (`id_direccion`),
  KEY `ix_direcciones_usuario_id_usuario` (`usuario_id_usuario`),
  CONSTRAINT `fk_direcciones_usuarios_usuario_id_usuario` FOREIGN KEY (`usuario_id_usuario`) REFERENCES `usuarios` (`id_usuario`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `direcciones`
--

LOCK TABLES `direcciones` WRITE;
/*!40000 ALTER TABLE `direcciones` DISABLE KEYS */;
/*!40000 ALTER TABLE `direcciones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fincas`
--

DROP TABLE IF EXISTS `fincas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fincas` (
  `id_finca` int NOT NULL AUTO_INCREMENT,
  `id_productor` int NOT NULL,
  `nombre_finca` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `latitud` decimal(65,30) DEFAULT NULL,
  `longitud` decimal(65,30) DEFAULT NULL,
  `historia_cultivo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`id_finca`),
  UNIQUE KEY `ix_fincas_id_productor` (`id_productor`),
  CONSTRAINT `fk_fincas_usuarios_id_productor` FOREIGN KEY (`id_productor`) REFERENCES `usuarios` (`id_usuario`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fincas`
--

LOCK TABLES `fincas` WRITE;
/*!40000 ALTER TABLE `fincas` DISABLE KEYS */;
/*!40000 ALTER TABLE `fincas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `formulario_preguntas`
--

DROP TABLE IF EXISTS `formulario_preguntas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `formulario_preguntas` (
  `id_pregunta` int NOT NULL AUTO_INCREMENT,
  `id_publicacion_inst` int NOT NULL,
  `texto_pregunta` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `tipo_respuesta` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `orden` int NOT NULL,
  PRIMARY KEY (`id_pregunta`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `formulario_preguntas`
--

LOCK TABLES `formulario_preguntas` WRITE;
/*!40000 ALTER TABLE `formulario_preguntas` DISABLE KEYS */;
/*!40000 ALTER TABLE `formulario_preguntas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `formulario_respuestas`
--

DROP TABLE IF EXISTS `formulario_respuestas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `formulario_respuestas` (
  `id_respuesta` int NOT NULL AUTO_INCREMENT,
  `id_pregunta` int NOT NULL,
  `id_usuario` int NOT NULL,
  `respuesta_texto` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `fecha_respuesta` datetime(6) NOT NULL,
  PRIMARY KEY (`id_respuesta`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `formulario_respuestas`
--

LOCK TABLES `formulario_respuestas` WRITE;
/*!40000 ALTER TABLE `formulario_respuestas` DISABLE KEYS */;
/*!40000 ALTER TABLE `formulario_respuestas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `instituciones`
--

DROP TABLE IF EXISTS `instituciones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `instituciones` (
  `id_institucion` int NOT NULL AUTO_INCREMENT,
  `nombre_institucion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `tipo_organizacion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `nombre_responsable` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `telefono_responsable` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `password_hash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `departamento` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `municipio` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `direccion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `descripcion_breve` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `logo_url` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `documento_verificacion_url` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `estado_cuenta` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `fecha_registro` datetime(6) NOT NULL,
  PRIMARY KEY (`id_institucion`),
  UNIQUE KEY `ix_instituciones_email` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `instituciones`
--

LOCK TABLES `instituciones` WRITE;
/*!40000 ALTER TABLE `instituciones` DISABLE KEYS */;
/*!40000 ALTER TABLE `instituciones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `lugares_acopio`
--

DROP TABLE IF EXISTS `lugares_acopio`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `lugares_acopio` (
  `id_lugar_acopio` int NOT NULL AUTO_INCREMENT,
  `nombre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `departamento` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `municipio` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `direccion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`id_lugar_acopio`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `lugares_acopio`
--

LOCK TABLES `lugares_acopio` WRITE;
/*!40000 ALTER TABLE `lugares_acopio` DISABLE KEYS */;
/*!40000 ALTER TABLE `lugares_acopio` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mensajes`
--

DROP TABLE IF EXISTS `mensajes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mensajes` (
  `id_mensaje` int NOT NULL AUTO_INCREMENT,
  `id_conversacion` int NOT NULL,
  `id_emisor` int NOT NULL,
  `id_receptor` int NOT NULL,
  `contenido` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `fecha_envio` datetime(6) NOT NULL,
  `leido` tinyint(1) NOT NULL,
  PRIMARY KEY (`id_mensaje`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mensajes`
--

LOCK TABLES `mensajes` WRITE;
/*!40000 ALTER TABLE `mensajes` DISABLE KEYS */;
/*!40000 ALTER TABLE `mensajes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `metodos_pago`
--

DROP TABLE IF EXISTS `metodos_pago`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `metodos_pago` (
  `id_metodo_pago` int NOT NULL AUTO_INCREMENT,
  `id_usuario` int NOT NULL,
  `tipo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `numero_enmascarado` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `nombre_titular` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `fecha_expiracion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `usuario_id_usuario` int NOT NULL,
  PRIMARY KEY (`id_metodo_pago`),
  KEY `ix_metodos_pago_usuario_id_usuario` (`usuario_id_usuario`),
  CONSTRAINT `fk_metodos_pago_usuarios_usuario_id_usuario` FOREIGN KEY (`usuario_id_usuario`) REFERENCES `usuarios` (`id_usuario`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `metodos_pago`
--

LOCK TABLES `metodos_pago` WRITE;
/*!40000 ALTER TABLE `metodos_pago` DISABLE KEYS */;
/*!40000 ALTER TABLE `metodos_pago` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `notificaciones`
--

DROP TABLE IF EXISTS `notificaciones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notificaciones` (
  `id_notificacion` int NOT NULL AUTO_INCREMENT,
  `id_usuario` int NOT NULL,
  `tipo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `contenido` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `fecha` datetime(6) NOT NULL,
  `leido` tinyint(1) NOT NULL,
  PRIMARY KEY (`id_notificacion`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `notificaciones`
--

LOCK TABLES `notificaciones` WRITE;
/*!40000 ALTER TABLE `notificaciones` DISABLE KEYS */;
/*!40000 ALTER TABLE `notificaciones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pedidos`
--

DROP TABLE IF EXISTS `pedidos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pedidos` (
  `id_pedido` int NOT NULL AUTO_INCREMENT,
  `id_comprador` int NOT NULL,
  `id_productor` int NOT NULL,
  `id_direccion_entrega` int DEFAULT NULL,
  `id_metodo_pago` int DEFAULT NULL,
  `id_lugar_acopio` int DEFAULT NULL,
  `fecha_creacion` datetime(6) NOT NULL,
  `metodo_entrega` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `estado_pedido` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `subtotal` decimal(10,2) NOT NULL,
  `comision_porcentaje` decimal(5,2) NOT NULL,
  `monto_comision` decimal(10,2) NOT NULL,
  `total` decimal(10,2) NOT NULL,
  `tipo_pago_acordado` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`id_pedido`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pedidos`
--

LOCK TABLES `pedidos` WRITE;
/*!40000 ALTER TABLE `pedidos` DISABLE KEYS */;
/*!40000 ALTER TABLE `pedidos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `publicacion_imagenes`
--

DROP TABLE IF EXISTS `publicacion_imagenes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `publicacion_imagenes` (
  `id_imagen` int NOT NULL AUTO_INCREMENT,
  `id_publicacion` int NOT NULL,
  `url_imagen` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `orden` int NOT NULL,
  PRIMARY KEY (`id_imagen`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `publicacion_imagenes`
--

LOCK TABLES `publicacion_imagenes` WRITE;
/*!40000 ALTER TABLE `publicacion_imagenes` DISABLE KEYS */;
/*!40000 ALTER TABLE `publicacion_imagenes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `publicaciones`
--

DROP TABLE IF EXISTS `publicaciones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `publicaciones` (
  `id_publicacion` int NOT NULL AUTO_INCREMENT,
  `id_productor` int NOT NULL,
  `id_producto` int NOT NULL,
  `id_finca` int DEFAULT NULL,
  `cantidad_disponible` decimal(10,2) NOT NULL,
  `unidad_medida` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `precio_unitario` decimal(10,2) NOT NULL,
  `fecha_cosecha_estimada` date DEFAULT NULL,
  `estado_madurez` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `estado_publicacion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `fecha_publicacion` datetime(6) NOT NULL,
  PRIMARY KEY (`id_publicacion`),
  UNIQUE KEY `ix_publicaciones_id_productor_id_producto` (`id_productor`,`id_producto`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `publicaciones`
--

LOCK TABLES `publicaciones` WRITE;
/*!40000 ALTER TABLE `publicaciones` DISABLE KEYS */;
/*!40000 ALTER TABLE `publicaciones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `publicaciones_institucionales`
--

DROP TABLE IF EXISTS `publicaciones_institucionales`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `publicaciones_institucionales` (
  `id_publicacion_inst` int NOT NULL AUTO_INCREMENT,
  `id_institucion` int NOT NULL,
  `tipo_contenido` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `titulo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `contenido_texto` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `imagen_url` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `archivo_url` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `fecha_evento` date DEFAULT NULL,
  `hora_evento` time(6) DEFAULT NULL,
  `departamento` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `municipio` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `alcance` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `fecha_publicacion` datetime(6) NOT NULL,
  PRIMARY KEY (`id_publicacion_inst`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `publicaciones_institucionales`
--

LOCK TABLES `publicaciones_institucionales` WRITE;
/*!40000 ALTER TABLE `publicaciones_institucionales` DISABLE KEYS */;
/*!40000 ALTER TABLE `publicaciones_institucionales` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuarios` (
  `id_usuario` int NOT NULL AUTO_INCREMENT,
  `nombre_completo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `password_hash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `tipo_usuario` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `numero_identificacion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `telefono_whatsapp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `departamento` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `municipio` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `fruta_preferida` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `anos_experiencia` int DEFAULT NULL,
  `estado_verificacion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `fecha_registro` datetime(6) NOT NULL,
  PRIMARY KEY (`id_usuario`),
  UNIQUE KEY `ix_usuarios_email` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--

LOCK TABLES `usuarios` WRITE;
/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `valoraciones`
--

DROP TABLE IF EXISTS `valoraciones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `valoraciones` (
  `id_valoracion` int NOT NULL AUTO_INCREMENT,
  `id_pedido` int NOT NULL,
  `id_evaluador` int NOT NULL,
  `id_evaluado` int NOT NULL,
  `puntuacion` int NOT NULL,
  `comentario_texto` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `fecha_valoracion` datetime(6) NOT NULL,
  PRIMARY KEY (`id_valoracion`),
  UNIQUE KEY `ix_valoraciones_id_pedido` (`id_pedido`),
  CONSTRAINT `ck_valoraciones_puntuacion` CHECK ((`puntuacion` between 1 and 5))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `valoraciones`
--

LOCK TABLES `valoraciones` WRITE;
/*!40000 ALTER TABLE `valoraciones` DISABLE KEYS */;
/*!40000 ALTER TABLE `valoraciones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `verificaciones_productor`
--

DROP TABLE IF EXISTS `verificaciones_productor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `verificaciones_productor` (
  `id_verificacion` int NOT NULL AUTO_INCREMENT,
  `id_productor` int NOT NULL,
  `id_institucion` int NOT NULL,
  `estado` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `fecha_solicitud` datetime(6) NOT NULL,
  `fecha_verificacion` datetime(6) DEFAULT NULL,
  `fecha_vencimiento` datetime(6) DEFAULT NULL,
  `fecha_renovacion` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`id_verificacion`),
  UNIQUE KEY `ix_verificaciones_productor_id_productor_id_institucion` (`id_productor`,`id_institucion`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `verificaciones_productor`
--

LOCK TABLES `verificaciones_productor` WRITE;
/*!40000 ALTER TABLE `verificaciones_productor` DISABLE KEYS */;
/*!40000 ALTER TABLE `verificaciones_productor` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-08-21 19:07:25
