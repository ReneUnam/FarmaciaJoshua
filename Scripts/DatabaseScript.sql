Create Database FarmaciaJoshua
GO

Use FarmaciaJoshua;
GO

CREATE LOGIN Gerente
WITH PASSWORD = '1234';
GO

CREATE USER GERENTE
FOR LOGIN GERENTE;
GO

--CREATE USER Vendedor FOR LOGIN Vendedor WITH DEFAULT_SCHEMA = dbo
GO

CREATE SCHEMA Compras;
GO
CREATE SCHEMA Ventas;
GO
CREATE SCHEMA Productos;
GO
--DBO
Create table dbo.Roles(
	IdRol int primary key identity (1,1) not null,
	Nombre varchar(50) not null, 
	Descripcion varchar(250) not null
)
GO
Create table dbo.Usuarios(
	IdUsuario int primary key identity (1,1),
	Nombres varchar(50) not null,
	Apellidos varchar(50) not null,
	NombreUsuario varchar(50) not null,
	UsuarioSalt varbinary(64) NULL,
	Contraseña nvarchar(50) not null,
	IdRol int NOT NULL,

	foreign key (IdRol) references dbo.Roles(IdRol)
);
GO
Create table dbo.Categorias(
	IdCategoria int primary key identity (1,1) not null,
	Nombre varchar(50) not null, 
	Descripcion varchar(250) null
)
GO
--COMPRAS
Create table Compras.Proveedores(
	IdProveedor int primary key identity(1,1) not null,
	Nombre varchar(50) not null,
	Telefono varchar (250) null
)
GO
Create table Compras.Compras(
	IdCompra int primary key identity(1,1) not null,
	IdProveedor int not null,
	IdUsuario int not null,
	FechaCompra datetime not null,
	Total decimal(10,2) not null,

	foreign key (IdProveedor) references Compras.Proveedores(IdProveedor),
	foreign key (IdUsuario) references dbo.Usuarios(IdUsuario)
)
GO
--PRODUCTOS
Create table Productos.Cat_Producto(
	IdProducto int primary key identity(1,1) not null,
	Nombre varchar(50) not null,
	IdCategoria int not null,
	Estado varchar(50) null

	foreign key (IdCategoria) references dbo.Categorias(IdCategoria)
)
GO
--COMPRAS
Create table Compras.DetalleCompra(
	IdDetalleCompra int primary key identity(1,1) not null,
	IdCompra int not null,
	IdProducto int not null,
	Cantidad int not null,
	PrecioUnitario decimal(10,2) not null,
	Subtotal AS (Cantidad * PrecioUnitario),

	foreign key (IdCompra) references Compras.Compras(IdCompra),
	foreign key (IdProducto) references Productos.Cat_Producto(IdProducto) 
)
GO
--PRODUCTOS
Create table Productos.Cat_DetalleProducto(
	Detalle_Id int primary key identity (1,1) not null,
	Detalle_Descripcion varchar(200) not null,
	Detalle_IdProducto int,
	Detalle_FechaVencimiento DATETIME,
	Detalle_Estado bit null,

	foreign key (Detalle_IdProducto) references Productos.Cat_Producto(IdProducto)
)
GO
Create table Productos.Tbl_ProductoAlmacenado(
	Almc_Id int primary key identity (1,1) not null,
	Almc_Detalle_Id int not null,
	Almc_Proveedor_Id int not null,
	Almc_Lote varchar(10) null,
	Almc_Existencia int not null,
	Almc_PrecioCompra decimal(18,0) not null,
	Almc_PrecioVenta decimal(18,0) not null,
	Almc_Estado bit null,

	foreign key (Almc_Detalle_Id) references Productos.Cat_DetalleProducto(Detalle_Id),
	foreign key (Almc_Proveedor_Id) references Compras.Proveedores(IdProveedor)
)
GO
--VENTAS
Create table Ventas.Clientes(
	IdCliente int primary key identity(1,1) not null,
	Nombre varchar(100) not null, 
	Apellido varchar(100) not null
)
Create table Ventas.Ventas(
	IdVenta int primary key identity(1,1) not null,
	IdCliente int not null,
	IdUsuario int not null,
	FechaVenta datetime not null,
	Total decimal(10,2) not null
)
GO
Create table Ventas.DetalleVenta(
	IdDetalleVenta int primary key identity (1,1) not null,
	IdVenta int not null,
	IdProducto int not null, 
	Cantidad int not null,
	PrecioUnitario decimal(10,2) not null,
	Subtotal AS (Cantidad * PrecioUnitario),

	foreign key (IdVenta) references Ventas.Ventas(IdVenta),
	foreign key (IdProducto) references Productos.Cat_Producto(IdProducto)
)
GO
--PROCEDIMIENTOS ALMACENADOS

--CLIENTE
--GET ALL
CREATE PROCEDURE Ventas.Sp_MostrarCliente
AS
BEGIN
	SELECT *
	FROM Ventas.Clientes
END;
GO
--ADD
CREATE PROCEDURE Ventas.Sp_AgregarClientes
@nombre AS VARCHAR(100),
@apellido AS VARCHAR(100)
AS
BEGIN
	INSERT INTO ventas.Clientes(Nombre,Apellido) 
	VALUES (@nombre,@apellido)
END;
GO
--GET BY ID
CREATE PROCEDURE Ventas.Sp_MostrarClientePorId
@Id AS INT
AS
BEGIN
	SELECT * FROM Ventas.Clientes WHERE IdCliente = @Id
END;
GO
--UPDATE
CREATE PROCEDURE Ventas.Sp_EditarCliente
@id AS INT,
@nombre AS VARCHAR(100) = null,
@apellido AS VARCHAR(255) = null
AS
BEGIN
	UPDATE Ventas.Clientes 
	SET
		Nombre = ISNULL(@nombre, Nombre),
		Apellido = ISNULL(@apellido, Apellido)
	WHERE IdCliente =@id
END;
GO
--DELETE
CREATE PROCEDURE Ventas.Sp_ElimnarClientes
@id AS INT 
AS
BEGIN
	DELETE 
	FROM Ventas.Clientes 
	WHERE IdCliente = @id;
END;
--USUARIO
--GET ALL
GO
CREATE PROCEDURE dbo.Sp_MostrarUsuarios
AS
BEGIN
	SELECT * 
	FROM dbo.Usuarios
END;
GO
--GET BY ID
CREATE PROCEDURE dbo.Sp_MostrarUsuarioPorId
@id AS INT
AS 
BEGIN
	SELECT * 
	FROM dbo.Usuarios 
	WHERE IdUsuario = @id;
END;
GO
--ADD
--UPDATE
CREATE PROCEDURE dbo.Sp_EditarUsuario
@id AS INT,
@nombres AS VARCHAR(50) = null,
@apellidos AS VARCHAR(50) = null,
@nombreDeUsuario AS VARCHAR(50) = null,
@pwd AS VARCHAR (50) = null,
@idrol AS VARCHAR (50) =null 
AS
BEGIN
	UPDATE dbo.Usuarios 
	SET
		Nombres = isnull(@nombres,Nombres),
		Apellidos = isnull(@apellidos,Apellidos),
		NombreUsuario = isnull(@nombreDeUsuario,NombreUsuario),
		Contraseña = isnull(@pwd,Contraseña),
		IdRol = ISNULL(@idrol,IdRol)
	WHERE IdUsuario = @id
END;
GO
--DELETE
CREATE PROCEDURE dbo.Sp_EliminarUsuario
@id AS INT
AS
BEGIN
	DELETE 
	FROM dbo.Usuarios 
	WHERE IdUsuario = @id;
END;
GO
--ROL
--GET ALL
CREATE PROCEDURE dbo.Sp_MostrarRoles
AS
BEGIN
	SELECT * 
	FROM dbo.Roles;
END;
GO
--GET BY ID
CREATE PROCEDURE dbo.Sp_MostrarRolPorId
@id INT
AS
BEGIN
	SELECT *
	FROM dbo.Roles 
	WHERE IdRol = @id;
END;
GO
--ADD
CREATE PROCEDURE dbo.Sp_AgregarRol
@nombre AS VARCHAR(50),
@descripcion AS VARCHAR(50)
AS
BEGIN
	INSERT INTO dbo.Roles(Nombre, Descripcion) 
	VALUES(@nombre, @descripcion)
END;
GO
--UPDATE
CREATE PROCEDURE dbo.Sp_EditarRol
@id AS INT,
@nombre AS VARCHAR(50) = null,
@descripcion AS VARCHAR(50) = null
AS
BEGIN
	UPDATE dbo.Roles 
	SET
		Nombre = isnull(@nombre,Nombre),
		Descripcion = isnull(@descripcion,Descripcion)
	WHERE IdRol = @id;
END;
GO
--DELETE
CREATE PROCEDURE dbo.Sp_EliminarRol
@id INT
AS
BEGIN
	DELETE 
	FROM dbo.Roles
	WHERE IdRol = @id;
END;
GO
--CATEGORIA
--GET ALL
CREATE PROCEDURE dbo.Sp_MostrarCategorias
AS
BEGIN
	SELECT *
	FROM dbo.Categorias;
END;
GO
--GET BY ID
CREATE PROCEDURE dbo.Sp_MostrarCategoriaPorId
@id INT
AS
BEGIN
	SELECT *
	FROM dbo.Categorias
	WHERE IdCategoria = @id;
END;
GO
--ADD
CREATE PROCEDURE dbo.Sp_AgregarCategoria
@nombre VARCHAR(100),
@descripcion VARCHAR(255)
AS
BEGIN
	INSERT INTO dbo.Categorias (Nombre,Descripcion) 
	VALUES (@nombre,@descripcion);
END;
GO
--UPDATE
CREATE PROCEDURE dbo.Sp_EditarCategoria
@id AS INT,
@nombre AS VARCHAR(100) = null,
@descripcion AS VARCHAR(255) = null
AS
BEGIN
	UPDATE dbo.Categorias 
	SET
		Nombre = ISNULL(@nombre, Nombre),
		Descripcion = ISNULL(@descripcion, Descripcion)
	WHERE IdCategoria =@id;
END;
GO
--DELETE
CREATE PROCEDURE dbo.Sp_EliminarCategoria
@id AS INT
AS
BEGIN
	DELETE 
	FROM dbo.Categorias
	WHERE IdCategoria = @id;
END;
GO
--VENTA
--GET ALL
CREATE PROCEDURE Ventas.Sp_MostrarVentas
AS
BEGIN
	SELECT * 
	FROM Ventas.Ventas
END;
GO
--GET BY ID
CREATE PROCEDURE Ventas.Sp_MostrarVentaPorId
@id INT
AS
BEGIN
	SELECT *
	FROM Ventas.Ventas
	WHERE IdVenta = @id;
END;
GO
--ADD
CREATE TYPE TDetalleVenta AS TABLE(
idproducto INT,
cantidad INT,
precio DECIMAL(10,2));
GO
CREATE PROCEDURE Ventas.Sp_AgregarVenta
@idcliente INT,
@idusuario INT,
@fecha DATETIME,
@detalles TDetalleVenta READONLY
AS 
BEGIN 
	SET NOCOUNT ON;
		DECLARE @idventa INT;
		DECLARE @total DECIMAL(10,2);
	BEGIN TRY 
		BEGIN TRANSACTION;

		INSERT INTO Ventas.Ventas (idcliente, idusuario, fechaventa, total) 
		VALUES (@idcliente, @idusuario, @fecha, 0);

		SET @idventa = SCOPE_IDENTITY();
		SET @total = 0;

		INSERT INTO Ventas.DetalleVenta (idventa, idproducto, cantidad, preciounitario)
		SELECT @idventa, idproducto, cantidad, precio
		FROM @detalles;

		SET @total = (SELECT sum(subtotal) 
		FROM Ventas.DetalleVenta 
		WHERE idventa = @idventa);

		UPDATE Ventas.ventas
		SET total = @total
		WHERE idventa = @idventa;

		COMMIT TRANSACTION;
	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END;
GO
--DELETE
CREATE PROCEDURE Ventas.Sp_EliminarVenta
@id INT
AS
BEGIN
	DELETE FROM Ventas.Ventas
	WHERE Idventa = @id;
END;
GO
--UPDATE
CREATE PROCEDURE Ventas.Sp_EditarVenta
@id INT,
@idcliente INT = NULL,
@idusuario INT = NULL,
@fecha DATETIME = NULL 
AS
BEGIN
	UPDATE Ventas.Ventas
	SET 
		IdCliente = ISNULL(@idcliente, IdCliente),
		IdUsuario = ISNULL(@idusuario, IdUsuario),
		FechaVenta = ISNULL(@fecha, FechaVenta)
	WHERE IdVenta = @id;
END
GO
--DETALLEVENTA
--GET ALL
CREATE PROCEDURE Ventas.Sp_MostrarDetalleVenta
AS
BEGIN 
	SELECT * 
	FROM Ventas.DetalleVenta;
END;
GO
--UPDATE
CREATE PROCEDURE Ventas.Sp_EditarDetalleVenta
@idventa INT,
@iddetalle INT,
@idproducto INT = NULL,
@cantidad INT = NULL,
@precio DECIMAL(10,2) = NULL
AS
BEGIN
	UPDATE Ventas.DetalleVenta
	SET	
		IdProducto = ISNULL(@idproducto, IdProducto),
		Cantidad = ISNULL(@cantidad, Cantidad),
		PrecioUnitario = ISNULL(@precio, PrecioUnitario)
	WHERE IdDetalleVenta = @iddetalle;

	DECLARE @nuevototal DECIMAL(10,2);
	SELECT @nuevototal = SUM(Cantidad * PrecioUnitario) 
	FROM Ventas.DetalleVenta
	WHERE IdVenta = @idventa;

	UPDATE Ventas.Ventas
	SET 
		Total = @nuevototal
	WHERE IdVenta = @idventa;
END;
GO
--DELETE
CREATE PROCEDURE Ventas.Sp_EliminarDetalleVenta
@id INT
AS
BEGIN
	DELETE 
	FROM Ventas.DetalleVenta 
	WHERE IdVenta = @id;
END;
GO
--PROVEEDOR
--GET ALL
CREATE PROCEDURE Compras.Sp_MostrarProveedores
AS
BEGIN
	SELECT * 
	FROM Compras.Proveedores
END;
GO
--ADD
CREATE PROCEDURE Compras.Sp_AgregarProveedor
@nombre AS VARCHAR(100),
@telefono AS VARCHAR(50)
AS
BEGIN
	INSERT INTO Compras.Proveedores(Nombre,Telefono) 
	VALUES (@nombre,@telefono)
END;
GO

--DELETE
CREATE PROCEDURE Compras.Sp_ElimnarProveedores
@id AS INT
AS
BEGIN
	DELETE 
	FROM Compras.Proveedores 
	WHERE IdProveedor = @id
END;
GO
--GET BY ID
CREATE PROCEDURE Compras.Sp_MostrarProveedorPorId
@Id AS INT
AS
BEGIN
	SELECT *
	FROM Compras.Proveedores 
	WHERE IdProveedor = @Id
END;
GO
--UPDATE
CREATE PROCEDURE Compras.Sp_EditarProveedor
@id AS INT,
@nombre AS VARCHAR(100) = NULL,
@telefono as Nvarchar(50) = NULL
AS
BEGIN
	UPDATE Compras.Proveedores 
	SET
		Nombre = ISNULL(@nombre, Nombre),
		Telefono = ISNULL(@telefono, Telefono)
	WHERE IdProveedor =@id
END;
GO
--CAT_PRODUCTO
--Agregar
create procedure Productos.Sp_AgregarProducto
@nombre as nvarchar(100),
@estado as nvarchar(50),
@fecha as datetime
as
insert into Productos.Cat_Producto(Nombre,Estado)
values (@nombre,@estado,@fecha)
GO
--EliminarProductos
create procedure Productos.Sp_ElimnarProductos
@id as int as
delete from Productos.Cat_Producto where IdProducto = @id
GO
--Mostrar
Create proc Productos.Sp_MostrarProductos
as
select * from Productos.Cat_Producto
GO
--MostrarPorId
create procedure Productos.Sp_MostrarProductosPorId
@Id as int
as
select * from Productos.Cat_Producto where IdProducto = @Id
GO
--EditarProductos
Create procedure Productos.Sp_EditarProductos
@id as int,
@nombre as nvarchar(100) = null,
@estado as nvarchar(50) = null,
@fecha DateTime
as
update Productos.Cat_Producto set
Nombre = ISNULL(@nombre, Nombre),
Estado = ISNULL(@estado, Estado)
where IdProducto =@id
GO
--CAT_DETALLEPRODUCTO
--Agregar
create procedure Productos.Sp_AgregarDetalleProducto
@descripcion as nvarchar(100),
@idproducto as Int,
@detalle_vencimiento DATETIME,
@estado as nvarchar
as
insert into Productos.Cat_DetalleProducto(Detalle_Descripcion,Detalle_IdProducto,Detalle_FechaVencimiento,)
values (@descripcion,@idproducto,@detalle_vencimiento,@estado)
GO
--EliminarProductos
create procedure Productos.Sp_EliminarDetalleProductos
@id as int as
delete from Productos.Cat_DetalleProducto where Detalle_Id = @id
GO
--Mostrar
Create proc Productos.Sp_MostrarDetalleProductos
as
select * from Productos.Cat_DetalleProducto
GO
--MostrarPorId
create procedure Productos.Sp_MostrarDetalleProductosPorId
@Id as int
as
select * from Productos.Cat_DetalleProducto where Detalle_Id = @Id
GO
--EditarProductos
Create procedure Productos.Sp_EditarDetalleProductos
@descripcion as nvarchar(100) = null,
@idproducto as Int,
@detalle_vencimiento DATETIME = null,
@estado as nvarchar(50) = null
as
update Productos.Cat_DetalleProducto set
Detalle_Descripcion = ISNULL(@descripcion, Detalle_Descripcion),
Detalle_IdProducto = ISNULL(@idproducto, Detalle_IdProducto),
Detalle_FechaVencimiento = ISNULL(@detalle_vencimiento, Detalle_FechaVencimiento),
Detalle_Estado = ISNULL(@estado, Detalle_Estado)
GO
--INSERSIONES
--ROLES
INSERT INTO dbo.Roles(Nombre, Descripcion)
VALUES 
('Administrador','Gestiona la farmacia'),
('Vendedor','Atiende a los clientes')

--CATEGORIAS
INSERT INTO dbo.Categorias(Nombre, Descripcion)
VALUES
('Medicamentos','Productos farmaceuticos para la salud'),
('Cosmeticos','Productos de cuidado personal'),
('Suplementos','Vitaminas y minerales para el bienestar')

--CLIENTES
INSERT INTO Ventas.Clientes(Nombre, Apellido)
VALUES
('ROSENDO','GAVILAN'),
('BETY','LA FEA')

--PROVEEDORES
INSERT INTO Compras.Proveedores(Nombre, Telefono)
VALUES
('Laboratorio Farma','2225-8900'),
('Disegsa','2522-7889')

--PRODUCTOS
INSERT INTO productos.Cat_Producto (Nombre, IdCategoria, Estado)
VALUES 
('Aspirina', 1, 1),
('Paracetamol', 2, 1),
('Ibuprofeno', 1, 1);

INSERT INTO productos.Cat_DetalleProducto (Detalle_Descripcion, Detalle_IdProducto, Detalle_FechaVencimiento, Detalle_Estado)
VALUES 
('500 mg - Caja con 20 tabletas', 1, '2026-05-16T18:30:00', 1),
('500 mg - Caja con 10 tabletas', 1, '2025-07-13T16:30:00', 1),
('500 mg - Caja con 30 tabletas', 2, '2024-12-25T09:30:00', 1);

INSERT INTO Productos.Tbl_ProductoAlmacenado (Almc_Detalle_Id, Almc_Proveedor_Id, Almc_Lote, Almc_Existencia, Almc_PrecioCompra, Almc_PrecioVenta, Almc_Estado)
VALUES 
(1, 1, 'Lote1234', 50, 1.00, 2.00, 1),
(2, 1, 'Lote5678', 30, 1.50, 3.00, 1),
(3, 2, 'Lote9876', 20, 2.00, 4.00, 1);

--VENTA
INSERT INTO ventas.Ventas(idcliente, idusuario, fechaventa, total) 
VALUES 
(1, 1, '2024-11-15', 500)

--DETALLE VENTA
INSERT INTO ventas.DetalleVenta(IdVenta, IdProducto, Cantidad, PrecioUnitario) 
VALUES
(1, 1, 1, 2.00)

GO
--BITACORA
CREATE TABLE dbo.auditoria_productos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    accion VARCHAR(10),
    id_producto INT,
    fecha DATETIME,
    usuario VARCHAR(255),
    terminal VARCHAR(255)
);
GO
--TRIGGERS
CREATE TRIGGER auditoria_producto_changes
ON Productos.Cat_Producto
AFTER UPDATE, DELETE 
AS
BEGIN
    SET NOCOUNT ON;

    -- Manejar eliminaciones
    IF EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO auditoria_productos (accion, id_producto, fecha, usuario, terminal)
        SELECT 'DELETE', IdProducto, GETDATE(), SYSTEM_USER, @@servername
        FROM deleted;
    END

    -- Manejar actualizaciones
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        INSERT INTO auditoria_productos (accion, id_producto, fecha, usuario, terminal)
        SELECT 'UPDATE', IdProducto, GETDATE(), SYSTEM_USER, @@servername
        FROM inserted;
    END
END;
GO