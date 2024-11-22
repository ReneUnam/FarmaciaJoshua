Create Database FarmaciaJoshua

Use FarmaciaJoshua

Create schema Compras;
Create schema Ventas;
Create schema Productos;

--DBO
Create table dbo.Roles(
	IdRol int primary key identity (1,1) not null,
	Nombre varchar(50) not null, 
	Descripcion varchar(250) not null
)
Create table dbo.Usuarios(
	IdUsuario int primary key identity (1,1),
	Nombres varchar(50) not null,
	Apellidos varchar(50) not null,
	NombreUsuario varchar(50) not null,
	Contraseña nvarchar(50) not null,
	IdRol int not null,
	UsuarioSalt varbinary(64) null,

	foreign key IdRol references dbo.Rol(IdRol)
)
Create table dbo.Categorias(
	IdCategoria int primary key identity (1,1) not null,
	Nombre varchar(50) not null, 
	Descripcion varchar(250) null
)

--PRODUCTOS
Create table Productos.Cat_Producto(
	IdProducto int primary key identity(1,1) not null,
	Nombre varchar(50) not null,
	IdCategoria int not null,
	Estado varchar(50) null

	foreign key IdCategoria references dbo.Categoria(IdCategoria)
)

Create table Productos.Cat_DetalleProducto(
	Detalle_Id int primary key identity (1,1) not null,
	Detalle_Descripcion varchar(200) not null,
	Detalle_IdProducto int,
	Detalle_Estado bit null,

	foreign key Detalle_IdProducto references Productos.Cat_Producto(IdProducto)
)

Create table Productos.Tbl_ProductoAlmacenado(
	Almc_Id int primary key identity (1,1) not null,
	Almc_Detalle_Id int not null,
	Almc_Proveedor_Id int not null,
	Almc_Lote varchar(10), null
	Almc_Existencia int not null,
	Almc_PrecioCompra decimal(18,0) not null,
	Almc_PrecioVenta decimal(18,0) not null,
	Almc_Estado bit null,

	foreign key Almc_Detalle_Id references Productos.Cat_DetalleProducto(Detalle_Id),
	foreign key Almc_Proveedor_Id references Compras.Proveedores(IdProveedor)
)

--COMPRAS
Create table Compras.Proveedores(
	IdProveedor int primary key identity(1,1) not null,
	Nombre varchar(50) not null,
	Descripcion varchar (250) null
)

Create table Compras.Compras(
	IdCompra int primary key identity(1,1) not null,
	IdProveedor int not null,
	IdUsuario int not null,
	FechaCompra datetime not null,
	Total decimal(10,2) not null,

	foreign key IdProveedor references Compras.Proveedores(IdProveedor),
	foreign key IdUsuario references dbo.Usuario(IdUsuario)
)

Create table Compras.DetalleCompra(
	IdDetalleCompra int primary key identity(1,1) not null,
	IdCompra int not null,
	IdProducto int not null,
	Cantidad int not null,
	PrecioUnitario decimal(10,2) not null,
	Subtotal AS (Cantidad * PrecioUnitario) not null,

	foreign key IdCompra references Compras.Compras(IdCompra),
	foreign key IdProducto references Productos.Cat_Producto(IdProducto) 
)

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
Create table Ventas.DetalleVenta(
	IdDetalleVenta int primary key identity (1,1) not null,
	IdVenta int not null,
	IdProducto int not null, 
	Cantidad int not null,
	PrecioUnitario decimal(10,2) not null,
	Subtotal AS (Cantidad * PrecioUnitario) not null,

	foreign key IdVenta references Ventas.Ventas(IdVenta),
	foreign key IdProducto references Productos.Cat_Producto(IdProducto)
)

--PROCEDIMIENTOS ALMACENADOS

--USUARIO
--GET ALL
Create procedure dbo.Sp_MostrarUsuarios
AS
BEGIN
	SELECT * 
	FROM dbo.Usuarios
END;

--GET BY ID
CREATE PROCEDURE dbo.Sp_MostrarUsuarioPorId
@id AS INT
AS 
BEGIN
	SELECT * 
	FROM dbo.Usuarios 
	WHERE IdUsuario = @id;
END;

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

--DELETE
CREATE PROCEDURE dbo.Sp_EliminarUsuario
@id AS INT
AS
BEGIN
	DELETE 
	FROM dbo.Usuarios 
	WHERE IdUsuario = @id;
END;

--ROL
--GET ALL
CREATE PROCEDURE dbo.Sp_MostrarRoles
AS
BEGIN
	SELECT * 
	FROM dbo.Roles;
END;

--GET BY ID
CREATE PROCEDURE dbo.Sp_MostrarRolPorId
@id INT;
AS
BEGIN
	SELECT *
	FROM dbo.Roles 
	WHERE IdRol = @id;
END;

--ADD
CREATE PROCEDURE dbo.Sp_AgregarRol
@nombre AS VARCHAR(50),
@descripcion AS VARCHAR(50)
AS
BEGIN
	INSERT INTO dbo.Roles(Nombre, Descripcion) 
	VALUES(@nombre, @descripcion)
END;

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

--DELETE
CREATE PROCEDURE dbo.Sp_EliminarRol
@id INT
AS
BEGIN
	DELETE 
	FROM dbo.Roles
	WHERE IdRol = @id;
END;

--CATEGORIA
--GET ALL
CREATE PROCEDURE dbo.Sp_MostrarCategorias
AS
BEGIN
	SELECT *
	FROM dbo.Categorias;
END;

--GET BY ID
CREATE PROCEDURE dbo.Sp_MostrarCategoriaPorId
@id
AS
BEGIN
	SELECT *
	FROM dbo.Categorias
	WHERE IdCategoria = @id;
END;

--ADD
CREATE PROCEDURE dbo.Sp_AgregarCategoria
@nombre VARCHAR(100),
@descripcion VARCHAR(255)
AS
BEGIN
	INSERT INTO dbo.Categorias (Nombre,Descripcion) 
	VALUES (@nombre,@descripcion);
END;

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

--DELETE
CREATE PROCEDURE dbo.Sp_EliminarCategoria
@id AS INT
AS
BEGIN
	DELETE 
	FROM dbo.Categorias
	WHERE IdCategoria = @id;
END;

--VENTA
--GET ALL
CREATE PROCEDURE Ventas.Sp_MostrarVentas
AS
BEGIN
	SELECT * 
	FROM Ventas.Ventas
END;

--GET BY ID
CREATE PROCEDURE Ventas.Sp_MostrarVentaPorId
@id INT
AS
BEGIN
	SELECT *
	FROM Ventas.Ventas
	WHERE IdVenta = @id;
END;

--ADD
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

--DELETE
CREATE PROCEDURE Ventas.Sp_EliminarVenta
@id INT
AS
BEGIN
	DELETE FROM Ventas.Ventas
	WHERE Idventa = @id;
END;

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

--DETALLEVENTA
--GET ALL
CREATE PROCEDURE Ventas.Sp_MostrarDetalleVenta
AS
BEGIN 
	SELECT * 
	FROM Ventas.DetalleVenta;
END;

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

--DELETE
CREATE PROCEDURE Ventas.Sp_EliminarDetalleVenta
@id INT
AS
BEGIN
	DELETE 
	FROM Ventas.DetalleVenta 
	WHERE IdVenta = @id;
END;

--INSERSIONES
--USUARIOS
--PROVEEDORES
--PRODUCTOS
INSERT INTO productos.Cat_Producto (Nombre, IdCategoria, Estado)
VALUES 
('Aspirina', 1, 1),
('Paracetamol', 2, 1),
('Ibuprofeno', 1, 1);

INSERT INTO productos.Cat_DetalleProducto (Detalle_Descripcion, Detalle_IdProducto, Detalle_Estado)
VALUES 
('500 mg - Caja con 20 tabletas', 1, 1),
('500 mg - Caja con 10 tabletas', 1, 1),
('500 mg - Caja con 30 tabletas', 2, 1);

INSERT INTO Productos.Tbl_ProductoAlmacenado (Almc_Detalle_Id, Almc_Proveedor_Id, Almc_Lote, Almc_Existencia, Almc_PrecioCompra, Almc_PrecioVenta, Almc_Estado)
VALUES 
(1, 1, 'Lote1234', 50, 1.00, 2.00, 1),
(2, 1, 'Lote5678', 30, 1.50, 3.00, 1),
(3, 2, 'Lote9876', 20, 2.00, 4.00, 1);
