USE SistemaNominaDB;

CREATE TABLE Usuario (
    IdUsuario INT PRIMARY KEY,
    Nombre VARCHAR(100),
    Password VARCHAR(100),
    Rol VARCHAR(50)
);

INSERT INTO Usuario (IdUsuario, Nombre, Password, Rol)
VALUES (1, 'admin', 'admin123', 'Administrador');

INSERT INTO Usuario (IdUsuario, Nombre, Password, Rol)
VALUES (2, 'juan', '123456', 'Usuario');

select * from Usuario