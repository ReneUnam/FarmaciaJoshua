Create DataBase DbExample

Use DbExample

create table CatMateria(
	Materia_Id int Primary Key Identity(1,1),
	Materia_Nombre varchar(50) Not Null,
	Materia_State bit Default 1
)

CREATE TABLE TblFactura (
    Factura_Id INT PRIMARY KEY IDENTITY(1,1),
    Factura_Cliente NVARCHAR(100),
    Factura_Fecha DATETIME
);

CREATE TABLE TblDetalleFactura (
    Detalle_Id INT PRIMARY KEY IDENTITY(1,1),
    Factura_Id INT FOREIGN KEY REFERENCES TblFactura(Factura_Id),
    Detalle_Nombre NVARCHAR(100),
    Detalle_Cantidad INT,
    Detalle_Precio DECIMAL(18, 2)
);

Select * From CatMateria
Select * From TblFactura
Select * From TblDetalleFactura