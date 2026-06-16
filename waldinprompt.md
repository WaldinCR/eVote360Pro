# PROMPT — Implementación de Interfaz Visual eVote360 Pro (PARTE 2: Admin y Dirigente)
## Para: Desarrollador Frontend 2
## Proyecto: eVote360 Pro — Sistema de Votación Electrónica

---

## CONTEXTO GENERAL

Tienes que implementar la interfaz visual de una aplicación web llamada **eVote360 Pro**, desarrollada en **ASP.NET Core MVC con .NET 9**. 

Recibes un archivo **`eVote360_Pro_Wireframes.html`** que es la referencia visual **definitiva y aprobada**. Tu trabajo es trasladar todos los paneles de administración y gestión a las Razor Views (`.cshtml`) del proyecto MVC.

## ⚠️ REGLA DE ORO: SOBREESCRITURA TOTAL
**NO ASUMAS QUE NINGUNA CLASE (CSS) O ESTRUCTURA HTML EN LAS VISTAS EXISTENTES SIRVE. SI LOS ARCHIVOS `.cshtml` YA TIENEN CONTENIDO, DEBES REEMPLAZARLO TOTALMENTE.** Tu trabajo es construir las vistas desde cero utilizando exclusivamente los estilos base ya proveídos.

Tampoco modificarás ningún archivo `.cs` (Controllers, ViewModels, Models). Si necesitas datos, usa los modelos que ya lleguen a la vista.

---

## ARCHIVOS BASE (YA EXISTEN - NO LOS CREES)

El proyecto ya cuenta con el sistema de diseño integrado. **Debes utilizar obligatoriamente** las clases y variables que se encuentran en estos archivos:
- `wwwroot/css/evote-variables.css` (Colores, tipografías, componentes)
- `wwwroot/css/evote-icons.css` (Iconos SVG)
- `wwwroot/css/evote-layout.css` / `_Layout.cshtml.css`
- `wwwroot/js/evote-ui.js`
- `Views/Shared/_Layout.cshtml`

---

## TU ALCANCE: FLUJO ADMIN Y DIRIGENTE (12 Pantallas)

Tu responsabilidad son exclusivamente estas pantallas (Paneles de control y CRUDs):

### FLUJO ADMINISTRADOR
| # | Pantalla | Vista MVC a implementar (Sobreescribir) |
|---|----------|------------------------|
| 7 | Home del Administrador | `/Admin/Index` |
| 8 | Control de Elecciones CRUD | `/Admin/Elecciones/Index` |
| 9 | Mantenimiento de Puestos Electivos | `/Admin/PuestosElectivos/Index` |
| 10 | Mantenimiento de Ciudadanos | `/Admin/Ciudadanos/Index` |
| 11 | Mantenimiento de Partidos Políticos | `/Admin/Partidos/Index` |
| 12 | Mantenimiento de Usuarios del Sistema | `/Admin/Usuarios/Index` |
| 13 | Asignación de Dirigentes Políticos | `/Admin/AsignacionDirigentes/Index` |
| 14 | Resultados Electorales | `/Admin/Elecciones/Resultados` |

### FLUJO DIRIGENTE
| # | Pantalla | Vista MVC a implementar (Sobreescribir) |
|---|----------|------------------------|
| 15 | Home del Dirigente Político | `/Dirigente/Index` |
| 16 | Mantenimiento de Candidatos | `/Dirigente/Candidatos/Index` |
| 17 | Asignar Candidato a Puesto | `/Dirigente/AsignarCandidato/Index` |
| 18 | Alianzas Políticas | `/Dirigente/Alianzas/Index` |

---

## ESPECIFICACIONES CRÍTICAS POR PANTALLA

### Layout General (Admin y Dirigente)
Todas usan un layout de dos columnas: Sidebar fijo izquierdo (210px, `--b900`) y contenido principal. Debes implementar/sobreescribir `_SidebarAdmin.cshtml` y `_SidebarDirigente.cshtml` para integrarlos con `_Layout.cshtml`.

### Pantalla 7 y 15 — Homes
- **Admin:** 3 metric cards y tabla de elecciones del año.
- **Dirigente:** Hero card con gradiente azul, logo del partido, 5 KPI cards.

### Pantallas 8 a 13 — Módulos CRUD
Siguen un patrón estricto:
1. Header con botón `"Nuevo [entidad]"` (`.bnew`).
2. Barra de filtros.
3. Tabla con clases base del sistema (usa acciones en línea: `.bsm.gray` para Editar, `.bsm.red` para Inactivar, `.bsm.green` para Activar).
4. Badges de estado (`.bdg.bpend`, `.bdg.bact`, `.bdg.bdone`).

**Especial Pantalla 11 (Partidos):** El upload de logo usa zona drag-and-drop.
**Especial Pantalla 14 (Resultados):** El ganador tiene fondo `--g50`. Los empates tienen fondo `--a50`.

---
## ENTREGABLE
Un Pull Request solo con las vistas `.cshtml` de Admin, Dirigente y sus Shared Views modificadas/sobreescritas. Cero cambios en el backend.