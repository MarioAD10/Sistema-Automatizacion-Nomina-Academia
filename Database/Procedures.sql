USE SistemaNominaDB;


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
    @TipoClase VARCHAR(100),
    @TarifaPorHora DECIMAL(10,2)
AS
BEGIN
    INSERT INTO Maestros (
        NombreCompleto,
        DocumentoIdentidad,
        Telefono,
        TipoClase,
        TarifaPorHora
    )
    VALUES (
        @NombreCompleto,
        @DocumentoIdentidad,
        @Telefono,
        @TipoClase,
        @TarifaPorHora
    );
END;


-- Actualizar

CREATE PROCEDURE SP_ActualizarMaestro
    @IdMaestro INT,
    @NombreCompleto VARCHAR(150),
    @DocumentoIdentidad VARCHAR(20),
    @Telefono VARCHAR(20),
    @TipoClase VARCHAR(100),
    @TarifaPorHora DECIMAL(10,2)
AS
BEGIN
    UPDATE Maestros
    SET 
        NombreCompleto = @NombreCompleto,
        DocumentoIdentidad = @DocumentoIdentidad,
        Telefono = @Telefono,
        TipoClase = @TipoClase,
        TarifaPorHora = @TarifaPorHora
    WHERE IdMaestro = @IdMaestro;
END;


-- Eliminar

CREATE PROCEDURE SP_EliminarMaestro
    @IdMaestro INT
AS
BEGIN
    DELETE FROM Maestros
    WHERE IdMaestro = @IdMaestro;
END;


-- Buscar

CREATE PROCEDURE SP_BuscarMaestro
    @Busqueda VARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        IdMaestro,
        NombreCompleto,
        DocumentoIdentidad,
        Telefono,
        TipoClase,
        TarifaPorHora
    FROM Maestros
    WHERE 
        LOWER(LTRIM(RTRIM(NombreCompleto))) 
            LIKE '%' + LOWER(LTRIM(RTRIM(@Busqueda))) + '%'
        OR 
        LOWER(LTRIM(RTRIM(DocumentoIdentidad))) 
            LIKE '%' + LOWER(LTRIM(RTRIM(@Busqueda))) + '%';
END;


-- CHEQUEOS 

SELECT name 
FROM sys.procedures;