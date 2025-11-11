CREATE DATABASE DoubleVPartners_SlnTickets;
GO

USE DoubleVPartners_SlnTickets;
GO

-- Crear usuario para la aplicación
CREATE LOGIN appuser WITH PASSWORD = 'SQL#123456789';
CREATE USER appuser FOR LOGIN appuser;

-- Asignar permisos básicos
ALTER ROLE db_datareader ADD MEMBER appuser;
ALTER ROLE db_datawriter ADD MEMBER appuser;
ALTER ROLE db_ddladmin ADD MEMBER appuser;

-- Verificar
SELECT name, type_desc FROM sys.database_principals WHERE name = 'appuser';

-- 6. Creamos La Tabla Tickets
CREATE TABLE [dbo].[Ticket]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Usuario] NVARCHAR(100) NOT NULL,
    [FechaCreacion] DATETIME NOT NULL DEFAULT SYSUTCDATETIME(),
    [FechaActualizacion] DATETIME NOT NULL DEFAULT SYSUTCDATETIME(),
    [Estatus] INT NOT NULL,
    [Eliminado] BIT NOT NULL
);

-- (OPCIONAL) 7. Insertar Datos Default
INSERT INTO [dbo].[Ticket] (Usuario, FechaCreacion, FechaActualizacion, Estatus,Eliminado)
VALUES ('crzapata', SYSUTCDATETIME(), SYSUTCDATETIME(), 1,0);

INSERT INTO [dbo].[Ticket] (Usuario, FechaCreacion, FechaActualizacion, Estatus,Eliminado)
VALUES ('evalencia', SYSUTCDATETIME(), SYSUTCDATETIME(), 1,0);