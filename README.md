# ProductoImagenes.Tests
 Pruebas unitarias para ProductoImagenes-Azure

Este proyecto contiene pruebas unitarias para el servicio BlobService de la aplicación ProductoImagenes.

## Descripción

ProductoImagenes.Tests es un proyecto de pruebas unitarias diseñado para verificar la funcionalidad del servicio BlobService, que maneja operaciones de almacenamiento en Azure Blob Storage. Las pruebas cubren las principales operaciones CRUD (Crear, Leer, Actualizar, Eliminar) relacionadas con el manejo de archivos en el almacenamiento en la nube.

## Estructura del Proyecto

El proyecto de pruebas incluye:

- `BlobServiceTests.cs`: Contiene todas las pruebas unitarias para el BlobService.

## Pruebas Implementadas

1. `UploadFileAsync_ShouldReturnBlobUrl`: Verifica la funcionalidad de carga de archivos.
2. `DeleteFileAsync_ShouldReturnTrue_WhenFileExists`: Prueba la eliminación de archivos.
3. `GetFileAsync_ShouldReturnStream_WhenFileExists`: Comprueba la descarga de archivos.
4. `FileExistsAsync_ShouldReturnTrue_WhenFileExists`: Verifica la comprobación de existencia de archivos.
5. `GetBlobContainerClient_ShouldReturnBlobContainerClient`: Prueba la obtención del cliente del contenedor de blobs.

## Tecnologías Utilizadas

- xUnit: Framework de pruebas
- Moq: Librería de mocking para .NET
- Azure.Storage.Blobs: SDK de Azure para operaciones de Blob Storage

## Cómo Ejecutar las Pruebas

Para ejecutar las pruebas, use el siguiente comando en la raíz del proyecto: dotnet test

## Contribuciones

Las contribuciones son bienvenidas. Por favor, asegúrese de actualizar las pruebas según sea apropiado.
