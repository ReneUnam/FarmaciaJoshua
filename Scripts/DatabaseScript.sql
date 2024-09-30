USE [FarmaciaJoshua]
GO
/****** Object:  Table [dbo].[Cat_DetalleProducto]    Script Date: 28/9/2024 18:39:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cat_DetalleProducto](
	[Detalle_Id] [int] IDENTITY(1,1) NOT NULL,
	[Detalle_Descripcion] [varchar](40) NOT NULL,
	[Detalle_IdProducto] [int] NULL,
	[Detalle_Estado] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Detalle_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cat_Producto]    Script Date: 28/9/2024 18:39:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cat_Producto](
	[IdProducto] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[IdCategoria] [int] NOT NULL,
	[Estado] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdProducto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categorias]    Script Date: 28/9/2024 18:39:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categorias](
	[IdCategoria] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCategoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clientes]    Script Date: 28/9/2024 18:39:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clientes](
	[IdCliente] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Apellido] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Compras]    Script Date: 28/9/2024 18:39:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Compras](
	[IdCompra] [int] IDENTITY(1,1) NOT NULL,
	[IdProveedor] [int] NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[FechaCompra] [datetime] NOT NULL,
	[Total] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCompra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetalleCompra]    Script Date: 28/9/2024 18:39:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetalleCompra](
	[IdDetalleCompra] [int] IDENTITY(1,1) NOT NULL,
	[IdCompra] [int] NOT NULL,
	[IdProducto] [int] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[PrecioUnitario] [decimal](10, 2) NOT NULL,
	[SubTotal]  AS ([Cantidad]*[PrecioUnitario]),
PRIMARY KEY CLUSTERED 
(
	[IdDetalleCompra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetalleVenta]    Script Date: 28/9/2024 18:39:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetalleVenta](
	[IdDetalleVenta] [int] IDENTITY(1,1) NOT NULL,
	[IdVenta] [int] NOT NULL,
	[IdProducto] [int] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[PrecioUnitario] [decimal](10, 2) NOT NULL,
	[SubTotal]  AS ([Cantidad]*[PrecioUnitario]),
PRIMARY KEY CLUSTERED 
(
	[IdDetalleVenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proveedores]    Script Date: 28/9/2024 18:39:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proveedores](
	[IdProveedor] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Telefono] [nvarchar](15) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdProveedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 28/9/2024 18:39:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[IdRol] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_ProductoAlmacenado]    Script Date: 28/9/2024 18:39:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_ProductoAlmacenado](
	[Almc_Id] [int] IDENTITY(1,1) NOT NULL,
	[Almc_Detalle_Id] [int] NULL,
	[Almc_Proveedor_Id] [int] NULL,
	[Almc_Lote] [varchar](10) NULL,
	[Almc_Existencia] [int] NULL,
	[Almc_PrecioCompra] [decimal](18, 0) NULL,
	[Almc_PrecioVenta] [decimal](18, 0) NULL,
	[Almc_Estado] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Almc_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 28/9/2024 18:39:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Nombres] [nvarchar](50) NOT NULL,
	[Apellidos] [nvarchar](50) NOT NULL,
	[NombreUsuario] [nvarchar](50) NOT NULL,
	[Contraseña] [nvarchar](50) NOT NULL,
	[IdRol] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ventas]    Script Date: 28/9/2024 18:39:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ventas](
	[IdVenta] [int] IDENTITY(1,1) NOT NULL,
	[IdCliente] [int] NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[FechaVenta] [datetime] NOT NULL,
	[Total] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdVenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Categorias] ON 

INSERT [dbo].[Categorias] ([IdCategoria], [Nombre], [Descripcion]) VALUES (1, N'Medicamentos', N'Productos farmacéuticos para la salud')
INSERT [dbo].[Categorias] ([IdCategoria], [Nombre], [Descripcion]) VALUES (2, N'Cosméticos', N'Productos de cuidado personal')
INSERT [dbo].[Categorias] ([IdCategoria], [Nombre], [Descripcion]) VALUES (3, N'Suplementos', N'Vitaminas y minerales para el bienestar')
SET IDENTITY_INSERT [dbo].[Categorias] OFF
GO
SET IDENTITY_INSERT [dbo].[Clientes] ON 

INSERT [dbo].[Clientes] ([IdCliente], [Nombre], [Apellido]) VALUES (1, N'Juan', N'Pérez')
INSERT [dbo].[Clientes] ([IdCliente], [Nombre], [Apellido]) VALUES (2, N'Ana', N'López')
INSERT [dbo].[Clientes] ([IdCliente], [Nombre], [Apellido]) VALUES (3, N'Carlos', N'Martínez')
SET IDENTITY_INSERT [dbo].[Clientes] OFF
GO
SET IDENTITY_INSERT [dbo].[Proveedores] ON 

INSERT [dbo].[Proveedores] ([IdProveedor], [Nombre], [Telefono]) VALUES (1, N'Laboratorio Farma', N'2225-8900')
INSERT [dbo].[Proveedores] ([IdProveedor], [Nombre], [Telefono]) VALUES (2, N'Cosméticos Bellos', N'2524-5678')
SET IDENTITY_INSERT [dbo].[Proveedores] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([IdRol], [Nombre], [Descripcion]) VALUES (1, N'Administrador', N'Gestiona la farmacia')
INSERT [dbo].[Roles] ([IdRol], [Nombre], [Descripcion]) VALUES (2, N'Vendedor', N'Atiende a los clientes y realiza ventas')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuarios] ON 

INSERT [dbo].[Usuarios] ([IdUsuario], [Nombres], [Apellidos], [NombreUsuario], [Contraseña], [IdRol]) VALUES (1, N'Gary Julian', N'Navarro Matus', N'gary', N'4321', 1)
INSERT [dbo].[Usuarios] ([IdUsuario], [Nombres], [Apellidos], [NombreUsuario], [Contraseña], [IdRol]) VALUES (2, N'Luis Carlos', N'Lopez Vado', N'Luis', N'1234', 2)
INSERT [dbo].[Usuarios] ([IdUsuario], [Nombres], [Apellidos], [NombreUsuario], [Contraseña], [IdRol]) VALUES (5, N'Sammuel Isaac', N'Zeledon Molina', N'sammuel', N'1111', 1)
SET IDENTITY_INSERT [dbo].[Usuarios] OFF
GO
ALTER TABLE [dbo].[Cat_DetalleProducto] ADD  DEFAULT ((1)) FOR [Detalle_Estado]
GO
ALTER TABLE [dbo].[Tbl_ProductoAlmacenado] ADD  DEFAULT ((1)) FOR [Almc_Estado]
GO
ALTER TABLE [dbo].[Cat_DetalleProducto]  WITH CHECK ADD FOREIGN KEY([Detalle_IdProducto])
REFERENCES [dbo].[Cat_Producto] ([IdProducto])
GO
ALTER TABLE [dbo].[Cat_Producto]  WITH CHECK ADD FOREIGN KEY([IdCategoria])
REFERENCES [dbo].[Categorias] ([IdCategoria])
GO
ALTER TABLE [dbo].[Compras]  WITH CHECK ADD FOREIGN KEY([IdProveedor])
REFERENCES [dbo].[Proveedores] ([IdProveedor])
GO
ALTER TABLE [dbo].[Compras]  WITH CHECK ADD FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[DetalleCompra]  WITH CHECK ADD FOREIGN KEY([IdCompra])
REFERENCES [dbo].[Compras] ([IdCompra])
GO
ALTER TABLE [dbo].[DetalleCompra]  WITH CHECK ADD  CONSTRAINT [FK_DetalleCompra_Cat_DetalleProducto] FOREIGN KEY([IdProducto])
REFERENCES [dbo].[Cat_DetalleProducto] ([Detalle_Id])
GO
ALTER TABLE [dbo].[DetalleCompra] CHECK CONSTRAINT [FK_DetalleCompra_Cat_DetalleProducto]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD FOREIGN KEY([IdVenta])
REFERENCES [dbo].[Ventas] ([IdVenta])
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [FK_DetalleVenta_Cat_DetalleProducto] FOREIGN KEY([IdProducto])
REFERENCES [dbo].[Cat_DetalleProducto] ([Detalle_Id])
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [FK_DetalleVenta_Cat_DetalleProducto]
GO
ALTER TABLE [dbo].[Tbl_ProductoAlmacenado]  WITH CHECK ADD FOREIGN KEY([Almc_Detalle_Id])
REFERENCES [dbo].[Cat_DetalleProducto] ([Detalle_Id])
GO
ALTER TABLE [dbo].[Tbl_ProductoAlmacenado]  WITH CHECK ADD FOREIGN KEY([Almc_Proveedor_Id])
REFERENCES [dbo].[Proveedores] ([IdProveedor])
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD FOREIGN KEY([IdRol])
REFERENCES [dbo].[Roles] ([IdRol])
GO
ALTER TABLE [dbo].[Ventas]  WITH CHECK ADD FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
GO
ALTER TABLE [dbo].[Ventas]  WITH CHECK ADD FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
/****** Object:  StoredProcedure [dbo].[Sp_AgregarUsuario]    Script Date: 28/9/2024 18:39:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Sp_AgregarUsuario]
@nombres as nvarchar(50),
@apellidos as nvarchar(50),
@nombreDeUsuario as nvarchar(50),
@pwd as nvarchar(50),
@idRol int
as
insert into Usuarios(nombres, apellidos, nombreusuario, contraseña, idRol) values(@nombres, 
@apellidos, @nombreDeUsuario, @pwd, @idRol)
GO
/****** Object:  StoredProcedure [dbo].[Sp_Editar]    Script Date: 28/9/2024 18:39:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[Sp_Editar] 
@id as int,
@nombres as nvarchar(50) = null,
@apellidos as nvarchar(50) = null,
@nombreDeUsuario as nvarchar(50) = null,
@pwd as nvarchar (50) = null,
@idrol as nvarchar (50) =null as

update Usuarios set
	Nombres = isnull(@nombres,Nombres),
	Apellidos = isnull(@apellidos,Apellidos),
	NombreUsuario = isnull(@nombreDeUsuario,NombreUsuario),
	Contraseña = isnull(@pwd,Contraseña),
	IdRol = ISNULL(@idrol,IdRol)
	where IdUsuario = @id
GO
/****** Object:  StoredProcedure [dbo].[Sp_Eliminar]    Script Date: 28/9/2024 18:39:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Sp_Eliminar]
@id as int as
delete from usuarios where idusuario = @id
GO
/****** Object:  StoredProcedure [dbo].[Sp_Mostrar]    Script Date: 28/9/2024 18:39:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create proc [dbo].[Sp_Mostrar]
as 
select * from Usuarios
GO
/****** Object:  StoredProcedure [dbo].[Sp_MostrarPorId]    Script Date: 28/9/2024 18:39:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Sp_MostrarPorId]
@id as int
as 
select * from usuarios where IdUsuario = @id
GO
