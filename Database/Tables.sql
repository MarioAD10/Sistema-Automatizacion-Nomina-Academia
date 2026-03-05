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
    Ocupacion VARCHAR(100) NOT NULL
);




