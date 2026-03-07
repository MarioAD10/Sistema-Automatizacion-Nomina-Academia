USE SistemaNominaDB;

GO

IF OBJECT_ID('sp_LoginUsuario', 'P') IS NOT NULL
    DROP PROCEDURE sp_LoginUsuario;
GO

CREATE PROCEDURE sp_LoginUsuario
    @Nombre VARCHAR(100),
    @Password VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Nombre,
        Rol
    FROM Usuario
    WHERE Nombre = @Nombre
    AND Password = @Password;
END
GO
