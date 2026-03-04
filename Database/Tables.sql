USE SistemaNominaDB;

-----------------------------------------
-----------------------------------------
--TABLA USUARIOS
-----------------------------------------
-----------------------------------------

CREATE TABLE Usuario (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Rol VARCHAR(50) NOT NULL
);

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

