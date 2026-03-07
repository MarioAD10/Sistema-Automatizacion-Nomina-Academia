USE SistemaNominaDB;

-----------------------------------------
-----------------------------------------
--SP - LOGIN
-----------------------------------------
-----------------------------------------
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
    SELECT Nombre, Rol
    FROM Usuario
    WHERE Nombre = @Nombre
    AND Password = @Password;
END
GO

-----------------------------------------
-----------------------------------------
--SP - MAESTROS 
-----------------------------------------
-----------------------------------------
-- Insertar
CREATE PROCEDURE SP_InsertarMaestro
    @NombreCompleto VARCHAR(150),
    @DocumentoIdentidad VARCHAR(20),
    @Telefono VARCHAR(20),
    @Ocupacion VARCHAR(100)
AS
BEGIN
    INSERT INTO Maestros (NombreCompleto, DocumentoIdentidad, Telefono, Ocupacion)
    VALUES (@NombreCompleto, @DocumentoIdentidad, @Telefono, @Ocupacion);
END;

-- Actualizar
CREATE PROCEDURE SP_ActualizarMaestro
    @IdMaestro INT,
    @NombreCompleto VARCHAR(150),
    @DocumentoIdentidad VARCHAR(20),
    @Telefono VARCHAR(20),
    @Ocupacion VARCHAR(100)
AS
BEGIN
    UPDATE Maestros
    SET NombreCompleto = @NombreCompleto,
        DocumentoIdentidad = @DocumentoIdentidad,
        Telefono = @Telefono,
        Ocupacion = @Ocupacion
    WHERE IdMaestro = @IdMaestro;
END;

-- Eliminar
CREATE PROCEDURE SP_EliminarMaestro
    @IdMaestro INT
AS
BEGIN
    DELETE FROM Maestros WHERE IdMaestro = @IdMaestro;
END;

-- Buscar
CREATE PROCEDURE SP_BuscarMaestro
    @Busqueda VARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdMaestro, NombreCompleto, DocumentoIdentidad, Telefono, Ocupacion
    FROM Maestros
    WHERE LOWER(LTRIM(RTRIM(NombreCompleto))) LIKE '%' + LOWER(LTRIM(RTRIM(@Busqueda))) + '%'
    OR LOWER(LTRIM(RTRIM(DocumentoIdentidad))) LIKE '%' + LOWER(LTRIM(RTRIM(@Busqueda))) + '%';
END;
