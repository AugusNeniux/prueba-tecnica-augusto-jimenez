# Prueba Técnica - Desarrollador Backend .NET

## 📋 Información General

| Concepto              | Detalle                                      |
| --------------------- | -------------------------------------------- |
| **Tiempo límite**     | 2 días calendario desde la recepción         |
| **Stack tecnológico** | .NET 8, Entity Framework Core, PostgreSQL    |
| **Arquitectura base** | Clean Architecture (plantilla proporcionada) |
| **Entrega**           | Repositorio público en GitHub                |

---

## 🚀 Cómo Iniciar

### Paso 1: Crear tu repositorio

1. Haz clic en el botón verde **"Use this template"** (arriba de la lista de archivos)
2. Selecciona **"Create a new repository"**
3. Configura tu repositorio:
   - **Nombre:** `prueba-tecnica-tu-nombre` (ejemplo: `prueba-tecnica-juan-perez`)
   - **Visibilidad:** Public
4. Haz clic en **"Create repository"**

> ⚠️ **Importante:** No hagas "Fork". Usa "Use this template" para que tu historial de commits sea limpio y propio.

### Paso 2: Clonar y configurar

```bash
git clone https://github.com/TU-USUARIO/prueba-tecnica-tu-nombre.git
cd prueba-tecnica-tu-nombre
```

### Paso 3: Levantar el ambiente

```bash
# 1. Levantar la base de datos PostgreSQL
docker-compose up -d

# 2. Verificar que está corriendo
docker-compose ps

# 3. Crear la migración inicial (cuando tengas tus entidades)
make add-migration NAME=InitialCreate

# 4. Aplicar migraciones a la base de datos
dotnet ef database update --project CleanArchitecture.PracticalTest.Infrastructure --startup-project CleanArchitecture.PracticalTest.API

# 5. Ejecutar la API
dotnet run --project CleanArchitecture.PracticalTest.API
```

> **Prerequisitos:** Docker y .NET 8 SDK instalados en tu máquina.

### Paso 4: Verificar que funciona

- Swagger UI: http://localhost:5144/swagger
- Health check: http://localhost:5144/health

---

## 🎯 El Problema de Negocio

Una empresa de logística necesita un **sistema de gestión de envíos**. El sistema debe permitir:

1. **Registrar paquetes** para envío
2. **Asignar rutas** a los paquetes
3. **Actualizar el estado** del paquete a lo largo de su ciclo de vida
4. **Consultar información** del paquete incluyendo historial de estados

### Reglas de Negocio

#### 1. Validaciones del paquete

| Restricción                           | Valor         |
| ------------------------------------- | ------------- |
| Peso mínimo                           | 0.1 kg        |
| Peso máximo                           | 50 kg         |
| Dimensión máxima (cualquier lado)     | 150 cm        |
| Volumen máximo (largo × ancho × alto) | 1,000,000 cm³ |

#### 2. Estados y transiciones permitidas

```
Registrado → EnBodega → EnTránsito → EnReparto → Entregado
                 ↓           ↓            ↓
              Devuelto    Devuelto     Devuelto
```

| Regla                                                                    |
| ------------------------------------------------------------------------ |
| Un paquete solo puede avanzar al siguiente estado en la secuencia        |
| Desde EnBodega, EnTránsito o EnReparto se puede pasar a Devuelto         |
| Un paquete en estado Entregado o Devuelto **no puede cambiar** de estado |
| Cada cambio de estado debe registrar: fecha/hora y motivo (opcional)     |

#### 3. Asignación de rutas

| Regla                                                                     |
| ------------------------------------------------------------------------- |
| Solo se puede asignar ruta si el paquete está en estado **EnBodega**      |
| Una ruta tiene: origen, destino, distancia (km) y tiempo estimado (horas) |
| Al asignar ruta, el paquete cambia automáticamente a **EnTránsito**       |

#### 4. Cálculo de costo de envío

| Concepto                                 | Valor               |
| ---------------------------------------- | ------------------- |
| Costo base                               | $50.00 MXN          |
| Por kg adicional (después del primer kg) | $15.00 MXN          |
| Por km de distancia                      | $2.50 MXN           |
| Recargo por volumen > 500,000 cm³        | +20% sobre el total |

**Fórmula:**
```
costoBase = 50
costoPeso = max(0, peso - 1) * 15
costoDistancia = distanciaKm * 2.5
subtotal = costoBase + costoPeso + costoDistancia
total = volumen > 500000 ? subtotal * 1.20 : subtotal
```

---

## 📌 Requerimientos Funcionales

### Obligatorios (Mínimo para aprobar)

| Método | Endpoint                             | Descripción                                                                   |
| ------ | ------------------------------------ | ----------------------------------------------------------------------------- |
| POST   | `/api/v1/packages`                   | Registrar nuevo paquete. Validar peso/dimensiones. Estado inicial: Registrado |
| PATCH  | `/api/v1/packages/{id}/status`       | Actualizar estado. Validar transiciones. Registrar historial                  |
| GET    | `/api/v1/packages/{id}`              | Obtener paquete con estado actual e historial                                 |
| POST   | `/api/v1/packages/{id}/assign-route` | Asignar ruta. Validar estado EnBodega. Calcular costo. Cambiar a EnTránsito   |

### Opcionales (Valor agregado)

- GET `/api/v1/packages` - Listar con filtros (estado, rango de fechas)
- GET `/api/v1/packages/{id}/shipping-cost` - Simular costo sin asignar ruta
- Paginación en listados
- Pruebas unitarias del dominio

---

## 🛠️ La Plantilla

Se proporciona una estructura base con:

| Incluido           | Descripción                                     |
| ------------------ | ----------------------------------------------- |
| ✅ Proyectos        | Domain, Application, Infrastructure, API, Tests |
| ✅ EF Core          | Configurado con PostgreSQL y migraciones        |
| ✅ Repository + UoW | Patrón implementado y listo para usar           |
| ✅ MediatR          | Pipeline con validación y manejo de excepciones |
| ✅ Excepciones      | Middleware global de manejo de errores          |
| ✅ Swagger          | Documentación de API configurada                |
| ✅ Localización     | Sistema de mensajes listo                       |
| ✅ Docker           | Compose básico para PostgreSQL                  |

> **Nota:** La plantilla está intencionalmente vacía de lógica de negocio. Debes crear las entidades, servicios, handlers y controladores necesarios.

## 📐 Convenciones de API (Obligatorias)

La plantilla incluye convenciones de respuesta que **debes seguir**.

### Respuestas Exitosas

Usa la estructura `APIResponse<T>` para todas las respuestas exitosas:

```json
{
  "message": "Mensaje descriptivo de la operación",
  "result": {
    "data": { /* Tu objeto o valor de respuesta */ },
    "warnings": ["Advertencia opcional"],
    "metadata": { "key": "value" }
  }
}
```

**En el Controller:**
```csharp
[HttpPost]
public async Task<ActionResult<APIResponse<Guid>>> Create([FromBody] CreateCommand command)
{
    var result = await _mediator.Send(command);
    var message = _localizer.GetResponseMessage("Entity.Created");
    return Ok(APIResponse.From(result, message));
}
```

**En el Handler:**
```csharp
public async Task<OperationResult<Guid>> Handle(CreateCommand request, CancellationToken ct)
{
    // ... lógica ...
    return OperationResult.With(entity.Id);
}
```

### Respuestas de Error

Los errores usan el estándar **RFC 7807 (Problem Details)** automáticamente:

| Tipo de Error         | HTTP Status               | Cuándo usar                |
| --------------------- | ------------------------- | -------------------------- |
| Validación de formato | 400 Bad Request           | FluentValidation falla     |
| Regla de negocio      | 422 Unprocessable Entity  | Lanzas `DomainException`   |
| No encontrado         | 404 Not Found             | Lanzas `NotFoundException` |
| Error interno         | 500 Internal Server Error | Excepción no controlada    |

**Ejemplo de error de regla de negocio:**
```json
{
  "title": "Business Rule Violation",
  "status": 422,
  "detail": "No se puede cambiar el estado de un paquete entregado",
  "errorCode": "Package.InvalidStatusTransition"
}
```

### Clases Disponibles

| Clase                | Uso                                         |
| -------------------- | ------------------------------------------- |
| `APIResponse<T>`     | Envolver respuestas exitosas en Controllers |
| `OperationResult<T>` | Retornar resultados desde Handlers          |
| `DomainException`    | Lanzar errores de reglas de negocio         |
| `NotFoundException`  | Lanzar cuando no existe una entidad         |

### ⚠️ Reglas

- ✅ **USA** `APIResponse.From()` para todas las respuestas exitosas
- ✅ **USA** `DomainException` para violaciones de reglas de negocio
- ✅ **USA** FluentValidation para validaciones de formato
- ❌ **NO** retornes objetos sin envolver en `APIResponse`
- ❌ **NO** crees estructuras de respuesta propias

---

## 🐳 Docker

### Configuración incluida

```
├── docker-compose.yml    # PostgreSQL para desarrollo local
├── Dockerfile            # Build básico de la API
```

### Inicio rápido

```bash
# 1. Levantar la base de datos
docker-compose up -d

# 2. Verificar que está corriendo
docker-compose ps

# 3. Ejecutar la API localmente (en otra terminal)
dotnet run --project CleanArchitecture.PracticalTest.API
```

El `appsettings.Development.json` ya está configurado para conectar al contenedor de PostgreSQL en `localhost:5432`.

### 🏆 Reto Opcional: Mejoras de Docker

> **Este reto es completamente opcional.** No afecta la evaluación de los requerimientos principales.

El `Dockerfile` y `docker-compose.yml` son funcionales pero básicos. Si tienes experiencia con Docker y te sobra tiempo, puedes mejorarlos.

**Áreas de mejora sugeridas:**

| Área                   | Pregunta guía                                   |
| ---------------------- | ----------------------------------------------- |
| Optimización de imagen | ¿Cómo reducirías el tamaño de la imagen final?  |
| Seguridad              | ¿Qué usuario ejecuta el proceso? ¿Es root?      |
| Cache de capas         | ¿El orden de COPY aprovecha el cache de Docker? |
| Health checks          | ¿Cómo sabría Docker si la API está saludable?   |
| Ambiente completo      | ¿Cómo levantarías API + BD juntos?              |

Si realizas mejoras, documéntalas en `DECISIONES.md` sección "Mejoras de Docker (Opcional)".

---

## 📦 Entregables

### 1. Repositorio Git

- ✅ Repositorio público en GitHub
- ✅ Commits atómicos con mensajes descriptivos
- ✅ Código que compila y ejecuta

### 2. Archivo `DECISIONES.md`

Crea este archivo en la raíz del proyecto explicando:

```markdown
# Decisiones de Diseño

## 1. Estructura del Dominio
[¿Cómo modelaste las entidades? ¿Por qué?]

## 2. Ubicación de Reglas de Negocio  
[¿Dónde pusiste las validaciones y por qué?]

## 3. Patrones Utilizados
[¿Qué patrones aplicaste? ¿Cuál fue tu razonamiento?]

## 4. Trade-offs y Limitaciones
[¿Qué dejaste fuera por tiempo? ¿Cómo lo resolverías?]

## 5. Supuestos
[¿Qué asumiste que no estaba explícito en los requerimientos?]
```

### 3. Colección de pruebas

Incluye **uno** de estos para probar los endpoints:
- Archivo `.http` (compatible con VS Code REST Client)
- Colección de Postman (exportada como JSON)
- Archivo `requests.md` con ejemplos de curl

---

## 📤 Cómo Entregar

1. Verifica que tu repositorio sea **público**
2. Verifica que el archivo `DECISIONES.md` esté completo
3. Responde al correo de invitación con:
   - Link a tu repositorio: `https://github.com/tu-usuario/prueba-tecnica-tu-nombre`
   - Confirma que está listo para revisión

---

## ✅ Criterios de Evaluación

| Criterio                | Peso | Qué evaluamos                                                          |
| ----------------------- | ---- | ---------------------------------------------------------------------- |
| **Arquitectura**        | 30%  | Separación correcta de responsabilidades entre capas                   |
| **Modelado de dominio** | 25%  | Entidades con comportamiento, encapsulación, reglas donde corresponden |
| **Calidad de código**   | 20%  | Legibilidad, consistencia, manejo de errores                           |
| **Funcionalidad**       | 15%  | Endpoints funcionan según especificación                               |
| **Documentación**       | 10%  | Claridad en DECISIONES.md, justificación de decisiones                 |



## ❓ Preguntas Frecuentes

**¿Puedo usar librerías adicionales?**  
Sí, pero justifica su uso en DECISIONES.md.

**¿Puedo modificar la estructura de la plantilla?**  
Sí, pero explica por qué en DECISIONES.md.

**¿Qué pasa si no termino todo?**  
Preferimos código incompleto pero bien estructurado que código completo pero mal diseñado. Documenta qué falta y cómo lo harías.

**¿Puedo usar herramientas de IA (ChatGPT, Copilot, etc.)?**  
Sí, pero el código debe reflejar tu comprensión. Deberás explicar cada decisión en una entrevista técnica posterior.

**¿Dudas sobre la plantilla o el ambiente?**  
Contacta a: agarcia@hircasa.com.mx

**¿Dudas sobre el problema de negocio?**  
Las ambigüedades son intencionales. Documenta tus supuestos en DECISIONES.md.

---

**¡Éxito!** 🚀