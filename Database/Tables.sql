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

-- Inserts
INSERT INTO Usuario (Nombre, Password, Rol)
VALUES
('Mario', '1234', 'Admin'),
('Tony', '1111', 'Admin');


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
