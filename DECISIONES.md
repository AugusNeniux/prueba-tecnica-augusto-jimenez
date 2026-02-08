# Decisiones de Diseño

## 1. Estructura del Dominio

El dominio fue modelado utilizando un enfoque de **Domain-Driven Design (DDD)**, donde el agregado principal es la entidad **Package**.

`Package` representa el ciclo de vida completo de un envío y es responsable de:
- Validar reglas de negocio (peso, dimensiones, volumen).
- Controlar las transiciones de estado permitidas.
- Mantener el historial de estados.
- Calcular y almacenar el costo de envío al asignar una ruta.

Las entidades **Route** y **PackageStatusHistory** forman parte del mismo agregado y no exponen comportamiento fuera del dominio.  
El historial de estados se maneja como una colección interna (`_statusHistory`) para asegurar que solo el agregado pueda modificarlo.

Este enfoque evita modelos anémicos y mantiene las reglas de negocio centralizadas.

---

## 2. Ubicación de Reglas de Negocio

Las reglas de negocio se colocaron **exclusivamente en la capa de Dominio**, principalmente dentro de la entidad `Package`.

Ejemplos:
- Validación de peso, dimensiones y volumen al crear el paquete.
- Validación de transiciones de estado permitidas.
- Bloqueo de cambios cuando el paquete está en estados finales (`Entregado` o `Devuelto`).
- Restricción de asignación de ruta únicamente cuando el paquete está en estado `EnBodega`.
- Registro obligatorio de historial en cada cambio de estado.

Cuando una regla se viola, se lanza un `DomainException`, la cual es transformada en una respuesta HTTP 422 (Unprocessable Entity) por el middleware global de errores.

Las validaciones de formato y datos de entrada (DTOs) se manejan con **FluentValidation**, retornando errores 400 (Bad Request).

---

## 3. Patrones Utilizados

- **Clean Architecture**  
  Separación clara entre capas: API, Application, Domain e Infrastructure.

- **CQRS (Command Query Responsibility Segregation)**  
  Se separaron comandos (mutaciones de estado) y queries (lectura), facilitando claridad, mantenimiento y pruebas.

- **Mediator Pattern (MediatR)**  
  Los controladores delegan la lógica a handlers, manteniendo la API delgada.

- **Repository / Unit of Work (abstracción de persistencia)**  
  La capa Application depende de interfaces (`IContextDb`) y no de EF Core directamente.

- **Rich Domain Model**  
  Las entidades contienen comportamiento y no solo datos.

- **Global Exception Handling**  
  Se utiliza un middleware y un `ProblemDetailsFactory` para mapear errores a RFC 7807 de forma consistente.

---

## 4. Trade-offs y Limitaciones

Por restricciones de tiempo:

- No se implementó paginación ni filtros avanzados en el listado de paquetes.
- No se incluyeron pruebas unitarias del dominio, aunque el diseño facilita su implementación.
- No se manejaron escenarios avanzados de concurrencia más allá del flujo esperado.

Estas mejoras podrían abordarse fácilmente en una siguiente iteración sin modificar la arquitectura existente.

---

## 5. Supuestos

- Un paquete siempre inicia en estado `Registrado`.
- Los estados `Entregado` y `Devuelto` son finales y no permiten transiciones posteriores.
- El costo de envío se calcula únicamente al asignar una ruta.
- El historial de estados es inmutable y representa la fuente de verdad del ciclo de vida del paquete.
- Las fechas se manejan en UTC para evitar inconsistencias entre zonas horarias.

---

## Conclusión

El sistema fue diseñado priorizando **claridad, mantenibilidad y consistencia del dominio**, asegurando que las reglas de negocio se mantengan protegidas y centralizadas, y que la API exponga un contrato limpio y predecible.
