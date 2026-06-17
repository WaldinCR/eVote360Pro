# eVote360 Pro

> Sistema de votación electrónica seguro, moderno y verificable. Permite gestionar elecciones, partidos, candidatos y ciudadanos desde un panel administrativo completo, con un flujo de votación que incluye verificación de identidad por OCR y código de verificación por correo electrónico.

---

## Equipo de Desarrollo

| Nombre | Matrícula |
|---|---|
| Aliandy Jiménez | 2025-1018 |
| Waldin Ceballos | 2025-1112 |
| Yailyn Santana | 2025-1104 |

---

## Descripción General

**eVote360 Pro** es una aplicación web de gestión electoral desarrollada como proyecto académico. El sistema contempla tres tipos de usuarios: **Administrador**, **Dirigente Político** y **Elector**, cada uno con acceso restringido a sus propios módulos y funcionalidades.

El flujo de votación garantiza la confidencialidad del voto mediante la separación entre el registro de participación y el voto en sí, validando la identidad del elector a través de OCR sobre su cédula y un código OTP enviado a su correo.

---

## Tecnologías Utilizadas

| Tecnología | Uso |
|---|---|
| .NET 9 / ASP.NET Core MVC | Framework principal |
| Entity Framework Core | ORM Code First |
| SQL Server | Base de datos |
| Bootstrap 5 | Interfaz de usuario |
| MailKit / MimeKit | Envío de correos SMTP |
| Tesseract OCR | Validación de cédula |

---

## Arquitectura

El proyecto implementa **Onion Architecture** separada en 4 proyectos:

```
Evote360-Pro.Core.Domain
    Entidades del dominio, enums e interfaces de repositorios.
    Sin dependencias externas.

Evote360-Pro.Core.Application
    Servicios de negocio, DTOs, ViewModels e interfaces de servicios.
    Referencia: Core.Domain

Evote360-Pro.Infrastructure.Persistence
    DbContext, configuraciones de entidades, repositorios y migraciones.
    Referencia: Core.Domain

Evote360-Pro.App
    Capa de presentación MVC con Areas por rol (Admin, Dirigente, Elector).
    Referencia: Core.Application + Infrastructure.Persistence
```

---

## Módulos del Sistema

### Administrador
- Gestión de usuarios, ciudadanos, puestos electivos y partidos políticos
- Asignación de dirigentes a partidos
- Gestión completa de elecciones (estados, activación, resultados)
- Dashboard con resumen electoral por año

### Dirigente Político
- Gestión de candidatos del partido
- Asignación de candidatos a puestos electivos
- Solicitud y gestión de alianzas políticas
- Dashboard con indicadores del partido

### Elector
- Autenticación por número de documento
- Verificación de identidad mediante OCR sobre cédula
- Código de verificación OTP enviado por correo
- Votación con opción "Ninguno" por cada puesto
- Correo de confirmación con resumen del voto

---

## Configuración del Proyecto

### Requisitos previos
- Visual Studio 2022+
- .NET 9 SDK
- SQL Server

## Correo que se maneja en el proyecto
Correo: evote360proayw@gmail.com
Contraseña: miniproyectoyts

