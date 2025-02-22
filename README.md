# Search Service API

Este servicio maneja las búsquedas y la gestión de estudiantes, restricciones y calificaciones. Utiliza **MongoDB** como base de datos y está desplegado en **Render**.

## Índice
- [Búsquedas](#búsquedas)
  - [Obtener todos los estudiantes](#obtener-todos-los-estudiantes)
  - [Buscar estudiante por UUID o nombre](#buscar-estudiante-por-uuid-o-nombre)
  - [Buscar estudiantes por restricción](#buscar-estudiantes-por-restricción)
  - [Buscar estudiantes por rango de notas](#buscar-estudiantes-por-rango-de-notas)
- [CRUD de Estudiantes, Restricciones y Calificaciones](#crud-de-estudiantes-restricciones-y-calificaciones)
  - [Estudiantes](#estudiantes)
    - [Crear estudiante](#crear-estudiante)
    - [Actualizar estudiante](#actualizar-estudiante)
    - [Eliminar estudiante](#eliminar-estudiante)
  - [Restricciones](#restricciones)
    - [Añadir restricción a un estudiante](#añadir-restricción-a-un-estudiante)
    - [Actualizar restricción de un estudiante](#actualizar-restricción-de-un-estudiante)
    - [Eliminar restricción de un estudiante](#eliminar-restricción-de-un-estudiante)
  - [Calificaciones](#calificaciones)
    - [Añadir calificación a un estudiante](#añadir-calificación-a-un-estudiante)
    - [Actualizar calificación de un estudiante](#actualizar-calificación-de-un-estudiante)
    - [Eliminar calificación de un estudiante](#eliminar-calificación-de-un-estudiante)

---

## Búsquedas

### Obtener todos los estudiantes
- **URL**: `GET https://ads-searchservice.onrender.com/api/search/students`
- **Descripción**: Retorna todos los estudiantes registrados en el sistema.

### Buscar estudiante por UUID o nombre
- **URL**: `GET https://ads-searchservice.onrender.com/api/search/student/{query}`
- **Descripción**: Busca un estudiante por su UUID o nombre (o parte del nombre).

### Buscar estudiantes por restricción
- **URL**: `GET https://ads-searchservice.onrender.com/api/search/restriction?restrictionReason={reason}`
- **Descripción**: Retorna todos los estudiantes que tengan una restricción específica.

### Buscar estudiantes por rango de notas
- **URL**: `GET https://ads-searchservice.onrender.com/api/search/grades?min={minValue}&max={maxValue}`
- **Descripción**: Retorna todos los estudiantes cuyas notas estén dentro del rango especificado.

---

## CRUD de Estudiantes, Restricciones y Calificaciones

### Estudiantes

#### Crear estudiante
- **URL**: `POST https://ads-searchservice.onrender.com/api/manage/student`
- **Descripción**: Crea un nuevo estudiante. El UUID es obligatorio.
- **Cuerpo**:
    ```json
    {
      "id": "UUID",
      "name": "Nombre del estudiante",
      "email": "email@example.com"
    }
    ```

#### Actualizar estudiante
- **URL**: `PUT https://ads-searchservice.onrender.com/api/manage/student/{id}`
- **Descripción**: Actualiza un estudiante existente.
- **Cuerpo**:
    ```json
    {
      "name": "Nuevo nombre",
      "email": "nuevoemail@example.com"
    }
    ```

#### Eliminar estudiante
- **URL**: `DELETE https://ads-searchservice.onrender.com/api/manage/student/{id}`
- **Descripción**: Elimina un estudiante por su UUID.

---

### Restricciones

#### Añadir restricción a un estudiante
- **URL**: `POST https://ads-searchservice.onrender.com/api/manage/student/{studentId}/restriction`
- **Descripción**: Añade una restricción a un estudiante. El UUID de la restricción es obligatorio.
- **Cuerpo**:
    ```json
    {
      "restrictionId": "UUID",
      "reason": "Motivo de la restricción",
      "creationDate": "Fecha de creación"
    }
    ```

#### Actualizar restricción de un estudiante
- **URL**: `PUT https://ads-searchservice.onrender.com/api/manage/student/{studentId}/restriction/{restrictionId}`
- **Descripción**: Actualiza una restricción existente de un estudiante.
- **Cuerpo**:
    ```json
    {
      "reason": "Nuevo motivo de la restricción",
      "creationDate": "Nueva fecha"
    }
    ```

#### Eliminar restricción de un estudiante
- **URL**: `DELETE https://ads-searchservice.onrender.com/api/manage/student/{studentId}/restriction/{restrictionId}`
- **Descripción**: Elimina una restricción específica de un estudiante.

---

### Calificaciones

#### Añadir calificación a un estudiante
- **URL**: `POST https://ads-searchservice.onrender.com/api/manage/student/{studentId}/grade`
- **Descripción**: Añade una calificación a un estudiante. El UUID de la calificación es obligatorio.
- **Cuerpo**:
    ```json
    {
      "gradeId": "UUID",
      "course": "Nombre del curso",
      "gradeName": "Nombre de la calificación",
      "gradeValue": "Valor numérico decimal",
      "comment": "Comentario"
    }
    ```

#### Actualizar calificación de un estudiante
- **URL**: `PUT https://ads-searchservice.onrender.com/api/manage/student/{studentId}/grade/{gradeId}`
- **Descripción**: Actualiza una calificación existente de un estudiante.
- **Cuerpo**:
    ```json
    {
      "course": "Nuevo nombre del curso",
      "gradeName": "Nuevo nombre de la calificación",
      "gradeValue": "Valor numérico decimal",
      "comment": "Nuevo comentario"
    }
    ```

#### Eliminar calificación de un estudiante
- **URL**: `DELETE https://ads-searchservice.onrender.com/api/manage/student/{studentId}/grade/{gradeId}`
- **Descripción**: Elimina una calificación específica de un estudiante.