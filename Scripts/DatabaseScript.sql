Use FarmaciaJoshua;
GO
CREATE SCHEMA Compras;
GO
CREATE SCHEMA Ventas;
GO
CREATE SCHEMA Productos;
GO
--ROLES
CREATE ROLE Vendedor_rol;
CREATE ROLE Gerente_rol;

--LOGIN Y USER GERENTE
CREATE LOGIN Gerente
WITH PASSWORD = '1234',
DEFAULT_DATABASE = FarmaciaJoshua;

CREATE USER GERENTE
FOR LOGIN GERENTE;

--ASIGNAR ESQUEMAS AL ROL GERENTE
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::Ventas TO Gerente_rol;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::Compras TO Gerente_rol;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::Productos TO Gerente_rol;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO Gerente_rol;

--AGREGAR AL GERENTE AL ROL
ALTER ROLE Gerente_rol ADD MEMBER Gerente;

--LOGIN Y USER VENDEDOR
CREATE LOGIN Vendedor
WITH PASSWORD = '1234',
DEFAULT_DATABASE = FarmaciaJoshua;

CREATE USER Vendedor
FOR LOGIN Vendedor;

--ASIGNAR ESQUEMAS AL ROL VENDEDOR
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::Ventas TO Vendedor_rol;
GRANT SELECT, INSERT, UPDATE ON SCHEMA::Productos TO Vendedor_rol;

ALTER ROLE Vendedor_rol ADD MEMBER Vendedor;

--DBO
Create table dbo.Roles(
	IdRol int primary key identity (1,1) not null,
	Nombre varchar(50) not null, 
	Descripcion varchar(250) not null
)
GO
CREATE TABLE dbo.Usuarios(
	IdUsuario int primary key identity (1,1),
	Nombres varchar(50) not null,
	Apellidos varchar(50) not null,
	NombreUsuario varchar(50) not null,
	UsuarioSalt varbinary(64) NULL,
	Contraseña nvarchar(50) not null,
	IdRol int NOT NULL,

	FOREIGN KEY (IdRol) REFERENCES dbo.Roles(IdRol)
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
CREATE TYPE Ventas.TDetalleVenta AS TABLE(
idproducto INT,
cantidad INT,
precio DECIMAL(10,2));
GO
CREATE PROCEDURE Ventas.Sp_AgregarVenta
@idcliente INT,
@idusuario INT,
@fecha DATETIME,
@detalles Ventas.TDetalleVenta READONLY
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

		EXEC Ventas.sp_ProcesarVentaDetalle
        @IDVenta = @idventa,
        @Detalle = @detalles;

		COMMIT TRANSACTION;
	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END;
GO
CREATE PROCEDURE Ventas.Sp_ProcesarVentaDetalle
    @IDVenta INT,   
    @Detalle AS Ventas.TDetalleVenta READONLY
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @IDProducto INT, @CantidadSolicitada INT;

        -- Tabla temporal para gestionar datos del Detalle Type
        CREATE TABLE #DetallePendiente (
            IDProducto INT PRIMARY KEY,
            Cantidad INT
        );

        -- Tabla temporal para depuración (no requerida)
        CREATE TABLE #LogDebug (
            Step NVARCHAR(50),
            IDProducto INT,
            CantidadSolicitada INT,
            CantidadTomar INT,
            Fecha DATETIME
        );

        -- Transferencia de registros del Type a la tabla temporal
        INSERT INTO #DetallePendiente (IDProducto, Cantidad)
        SELECT IDProducto, Cantidad FROM @Detalle;

        -- Ciclo para recorrer cada elemento en la tabla temporal
        WHILE EXISTS (SELECT 1 FROM #DetallePendiente WHERE Cantidad > 0)
        BEGIN
            -- Seleccionar el primer elemento cuya cantidad sea mayor a 0
            SELECT TOP 1 
                @IDProducto = IDProducto,
                @CantidadSolicitada = Cantidad
            FROM #DetallePendiente
            WHERE Cantidad > 0;

            -- Monitoreo de proceso en el SP
            INSERT INTO #LogDebug (Step, IDProducto, CantidadSolicitada, CantidadTomar, Fecha)
            VALUES ('Inicio Producto', @IDProducto, @CantidadSolicitada, NULL, GETDATE());

            -- Ciclo enfocado a la gestión de existencias
            WHILE @CantidadSolicitada > 0
            BEGIN
                DECLARE @IDProductoAlmacenado INT, @CantidadActual INT, @CantidadTomar INT;

                -- Seleccionar el inventario disponible del producto
                SELECT TOP 1 
					@IDProductoAlmacenado = p.Almc_Id, -- Ajustar al nombre de columna correcto
					@CantidadActual = p.Almc_Existencia -- Ajustar al nombre de columna correcto
				FROM Productos.Tbl_ProductoAlmacenado p
				WHERE p.Almc_Detalle_Id = @IDProducto -- Cambiar idProducto al nombre correcto (Almc_Detalle_Id)
  				AND p.Almc_Existencia > 0 -- Asegurar consistencia con el nombre correcto
				ORDER BY p.Almc_Lote ASC; -- Ajustar a Almc_Lote o cualquier columna relevante
                IF @IDProductoAlmacenado IS NULL
                BEGIN
                    THROW 50001, 'Inventario insuficiente para completar la solicitud.', 1;
                END;

                -- Determinar la cantidad a tomar
                SET @CantidadTomar = CASE 
                                        WHEN @CantidadSolicitada <= @CantidadActual THEN @CantidadSolicitada
                                        ELSE @CantidadActual
                                     END;

                -- Actualización de inventario
                UPDATE Productos.Tbl_ProductoAlmacenado
                SET Almc_Existencia = Almc_Existencia - @CantidadTomar
                WHERE Almc_Id = @IDProductoAlmacenado;

                -- Insertar en el detalle de la venta
                INSERT INTO Ventas.DetalleVenta (IDVenta, IDProducto, Cantidad, PrecioUnitario)
                VALUES (@IDVenta, @IDProducto, @CantidadTomar, 0);

                -- Monitoreo del segundo proceso
                INSERT INTO #LogDebug (Step, IDProducto, CantidadSolicitada, CantidadTomar, Fecha)
                VALUES ('Procesar Inventario', @IDProducto, @CantidadSolicitada, @CantidadTomar, GETDATE());

                -- Actualizar la cantidad solicitada tras la operación realizada
                SET @CantidadSolicitada = @CantidadSolicitada - @CantidadTomar;

                -- Actualizar tabla temporal
                UPDATE #DetallePendiente
                SET Cantidad = @CantidadSolicitada
                WHERE IDProducto = @IDProducto;

                -- Eliminar registros sin pendientes
                DELETE FROM #DetallePendiente
                WHERE IDProducto = @IDProducto AND Cantidad <= 0;
            END;
        END;

        -- Monitoreo de fin de ciclo
        INSERT INTO #LogDebug (Step, IDProducto, CantidadSolicitada, CantidadTomar, Fecha)
        VALUES ('Fin Proceso', NULL, NULL, NULL, GETDATE());

        COMMIT TRANSACTION;

        -- Mostrar el historial de monitoreo
        SELECT * FROM #LogDebug;
    END TRY
    BEGIN CATCH

        ROLLBACK TRANSACTION;

        -- Mostrar el historial de monitoreo
        SELECT * FROM #LogDebug;

        THROW;
    END CATCH;
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
--COMPRAS
--ADD
CREATE TYPE Compras.TDetalleCompra AS TABLE(
	IdProducto int,
	Cantidad int,
	PrecioUnitario decimal(10,2)
);
CREATE PROCEDURE Compras.Sp_AgregarCompra
@idproveedor INT,
@idusuario INT,
@fecha DATETIME,
@detalles TDetalleCompra READONLY 
AS 
BEGIN 
	SET NOCOUNT ON;
		DECLARE @idcompra INT;
		DECLARE @total DECIMAL(10,2);
	BEGIN TRY 
		BEGIN TRANSACTION;

		INSERT INTO Compras.Compras (IdProveedor, IdUsuario, FechaCompra, Total) 
		VALUES (@idproveedor, @idusuario, @fecha, 0);

		SET @idcompra = SCOPE_IDENTITY();
		SET @total = 0;

		INSERT INTO Compras.DetalleCompra(IdCompra, IdProducto, Cantidad, PrecioUnitario)
		SELECT @idcompra, idproducto, cantidad, precio
		FROM @detalles;

		SET @total = (SELECT sum(subtotal) 
		FROM Compras.DetalleCompra
		WHERE IdCompra = @idcompra);

		UPDATE Compras.Compras
		SET total = @total
		WHERE IdCompra = @idcompra;

		COMMIT TRANSACTION;
	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END;
GO
--GET ALL
CREATE PROCEDURE Compras.Sp_MostrarCompras
AS
BEGIN
	SELECT * 
	FROM Compras.Compras
END;
GO
--GET BY ID
CREATE PROCEDURE Compras.Sp_MostrarCompraPorId
@id INT
AS
BEGIN
	SELECT *
	FROM Compras.Compras
	WHERE IdCompra = @id;
END;
GO
--UPDATE
CREATE PROCEDURE Compras.Sp_EditarCompra
@id INT,
@idproveedor INT = NULL,
@idusuario INT = NULL,
@fecha DATETIME = NULL 
AS
BEGIN
	UPDATE Compras.Compras
	SET 
		IdProveedor = ISNULL(@idproveedor, IdProveedor),
		IdUsuario = ISNULL(@idusuario, IdUsuario),
		FechaCompra = ISNULL(@fecha, FechaCompra)
	WHERE IdCompra = @id;
END;
GO
--DELETE
CREATE PROCEDURE Compras.Sp_EliminarCompra
@id INT
AS
BEGIN
	DELETE FROM Compras.Compras
	WHERE IdCompra = @id;
END;
GO
--DETALLE COMPRA
--GET ALL
CREATE PROCEDURE Compras.Sp_MostrarDetalleCompra
AS
BEGIN 
	SELECT * 
	FROM Compras.DetalleCompra;
END;
GO
--UPDATE
CREATE PROCEDURE Compras.Sp_EditarDetalleCompra
@idcompra INT,
@iddetalle INT,
@idproducto INT = NULL,
@cantidad INT = NULL,
@precio DECIMAL(10,2) = NULL
AS
BEGIN
	UPDATE Compras.DetalleCompra
	SET	
		IdProducto = ISNULL(@idproducto, IdProducto),
		Cantidad = ISNULL(@cantidad, Cantidad),
		PrecioUnitario = ISNULL(@precio, PrecioUnitario)
	WHERE IdDetalleCompra = @iddetalle;

	DECLARE @nuevototal DECIMAL(10,2);
	SELECT @nuevototal = SUM(Cantidad * PrecioUnitario) 
	FROM Compras.DetalleCompra
	WHERE IdCompra = @idcompra;

	UPDATE Compras.Compras
	SET 
		Total = @nuevototal
	WHERE IdCompra = @idcompra;
END;
GO
--DELETE
CREATE PROCEDURE Compras.Sp_EliminarDetalleCompra
@id INT
AS
BEGIN
	DELETE 
	FROM Compras.DetalleCompra
	WHERE IdCompra = @id;
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
--ADD
CREATE PROCEDURE Productos.Sp_AgregarProducto
@nombre VARCHAR(100),
@estado VARCHAR(50),
@IdCategoria INT
AS
BEGIN
	INSERT INTO Productos.Cat_Producto(Nombre,Estado,IdCategoria)
	VALUES (@nombre,@estado, @IdCategoria)
END;
GO
--GET ALL
CREATE PROCEDURE Productos.Sp_MostrarProductos
AS
BEGIN
	SELECT * 
	FROM Productos.Cat_Producto
END;
GO
--DELETE
CREATE PROCEDURE Productos.Sp_EliminarProductos
@id AS INT
AS
BEGIN
	DELETE
	FROM  Productos.Cat_Producto
	WHERE IdProducto = @id
END;
GO
--GET BY ID
CREATE PROCEDURE Productos.Sp_MostrarProductosPorId
@Id AS INT
AS
BEGIN
	SELECT * 
	FROM Productos.Cat_Producto 
	WHERE IdProducto = @Id
END;
GO
--UPDATE
CREATE PROCEDURE Productos.Sp_EditarProductos
@id INT,
@nombre VARCHAR(100) = null,
@estado VARCHAR(50) = null,
@IdCategoria INT = null
AS
BEGIN
	UPDATE Productos.Cat_Producto 
	SET
		Nombre = ISNULL(@nombre, Nombre),
		Estado = ISNULL(@estado, Estado),
		IdCategoria = ISNULL(@IdCategoria, IdCategoria)
	WHERE IdProducto = @id
END;
GO
--CAT_DETALLEPRODUCTO
--ADD
CREATE PROCEDURE Productos.Sp_AgregarDetalleProducto
@descripcion varchar(250),
@idproducto INT,
@detalle_vencimiento DATETIME,
@estado BIT
AS
BEGIN
	INSERT INTO Productos.Cat_DetalleProducto(Detalle_Descripcion,Detalle_IdProducto,Detalle_FechaVencimiento,Detalle_Estado)
	VALUES (@descripcion,@idproducto,@detalle_vencimiento,@estado)
END;
GO
--DELETE
CREATE PROCEDURE Productos.Sp_EliminarDetalleProductos
@id INT
AS
BEGIN
	DELETE FROM Productos.Cat_DetalleProducto 
	where Detalle_Id = @id
END;
GO
--GET ALL
CREATE PROCEDURE Productos.Sp_MostrarDetalleProductos
AS
BEGIN
	SELECT * 
	FROM Productos.Cat_DetalleProducto
END;
GO
--GET BY ID
CREATE PROCEDURE Productos.Sp_MostrarDetalleProductosPorId
@Id INT
AS
BEGIN
	SELECT * 
	FROM Productos.Cat_DetalleProducto 
	WHERE Detalle_Id = @Id
END;
GO
--UPDATE
CREATE PROCEDURE Productos.Sp_EditarDetalleProductos
@descripcion varchar(250) = null,
@detalle_id INT,
@idproducto INT,
@detalle_vencimiento DATETIME = null,
@estado VARCHAR(50) = null
AS
BEGIN
	UPDATE Productos.Cat_DetalleProducto 
	SET
		Detalle_Descripcion = ISNULL(@descripcion, Detalle_Descripcion),
		Detalle_IdProducto = ISNULL(@idproducto, Detalle_IdProducto),
		Detalle_FechaVencimiento = ISNULL(@detalle_vencimiento, Detalle_FechaVencimiento),
		Detalle_Estado = ISNULL(@estado, Detalle_Estado)
	WHERE Detalle_Id = @detalle_id;
END;
GO
--TBL_PRODUCTOALMACENADO
--GET ALL
CREATE PROCEDURE Productos.Sp_MostrarProductosAlmacenados
AS
BEGIN
	SELECT * 
	FROM Productos.Tbl_ProductoAlmacenado
END;
GO
--GET BY ID
CREATE PROCEDURE Productos.Sp_MostrarProductosAlmacenadoPorId
@Id AS INT
AS
BEGIN
	SELECT * 
	FROM Productos.Tbl_ProductoAlmacenado 
	WHERE Almc_Id = @Id
END;
GO
--ADD
CREATE PROCEDURE Productos.Sp_AgregarProductoAlmacenado
@detalleid AS INT,
@proveedorid AS INT,
@lote VARCHAR(50),
@Existencia AS INT,
@preciocompra AS DECIMAL(10,2),
@precioventa AS DECIMAL(10,2),
@estado AS BIT
AS
BEGIN
	INSERT INTO Productos.Tbl_ProductoAlmacenado(Almc_Detalle_Id,Almc_Proveedor_Id,Almc_Lote,Almc_Existencia,Almc_PrecioCompra,Almc_PrecioVenta,Almc_Estado)
	VALUES (@detalleid,@proveedorid,@lote,@Existencia,@preciocompra,@precioventa,@estado)
END;
GO
--UPDATE
CREATE PROCEDURE Productos.Sp_EditarProductosAlmacenado
@idproductoalmacenado AS INT,
@detalleid AS INT,
@proveedorid AS INT,
@lote AS VARCHAR,
@Existencia AS INT,
@preciocompra AS DECIMAL(10,2),
@precioventa AS DECIMAL(10,2),
@estado AS BIT
AS
BEGIN
	UPDATE Productos.Tbl_ProductoAlmacenado 
	SET
		Almc_Detalle_Id = ISNULL(@detalleid, Almc_Detalle_Id),
		Almc_Proveedor_Id = ISNULL(@proveedorid, Almc_Proveedor_Id),
		Almc_Lote = ISNULL(@lote, Almc_Lote),
		Almc_Existencia = ISNULL(@Existencia, Almc_Existencia),
		Almc_PrecioCompra = ISNULL(@preciocompra, Almc_PrecioCompra),
		Almc_PrecioVenta = ISNULL(@precioventa, Almc_PrecioVenta),
		Almc_Estado = ISNULL(@estado, Almc_Estado)
	WHERE Almc_Id = @idproductoalmacenado;
END;
GO

--DELETE
CREATE PROCEDURE Productos.Sp_EliminarProductosAlmacenado
@id AS INT
AS
BEGIN
	DELETE FROM Productos.Tbl_ProductoAlmacenado 
	WHERE Almc_Id = @id
END;
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
(3, 2, 'Lote9876', 10, 2.00, 4.00, 1);

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
CREATE TABLE dbo.auditoria(
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
        INSERT INTO dbo.auditoria (accion, id_producto, fecha, usuario, terminal)
        SELECT 'DELETE', IdProducto, GETDATE(), SYSTEM_USER, @@servername
        FROM deleted;
    END

    -- Manejar actualizaciones
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        INSERT INTO dbo.auditoria (accion, id_producto, fecha, usuario, terminal)
        SELECT 'UPDATE', IdProducto, GETDATE(), SYSTEM_USER, @@servername
        FROM inserted;
    END
END;
GO