USE SistemaNominaDB;

-----------------------------------------
-----------------------------------------
--TABLA MAESTROS 
-----------------------------------------
-----------------------------------------

CREATE TABLE Maestros (
    IdMaestro INT IDENTITY(1,1) PRIMARY KEY,
    NombreCompleto VARCHAR(150) NOT NULL,
    DocumentoIdentidad VARCHAR(20) NOT NULL,
    Telefono VARCHAR(20) NOT NULL,
    TipoClase VARCHAR(100) NOT NULL,
    TarifaPorHora DECIMAL(10,2) NOT NULL
);

