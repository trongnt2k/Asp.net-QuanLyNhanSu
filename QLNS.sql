USE [QLNS]
GO
/****** Object:  Table [dbo].[TrinhDoHocVan]    Script Date: 01/11/2022 16:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrinhDoHocVan](
	[MATDHV] [nvarchar](10) NOT NULL,
	[TENTRINHDO] [nvarchar](50) NULL,
 CONSTRAINT [PK_TrinhDoHocVan] PRIMARY KEY CLUSTERED 
(
	[MATDHV] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChucVu]    Script Date: 01/11/2022 16:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChucVu](
	[MACV] [nvarchar](10) NOT NULL,
	[TENCV] [nvarchar](50) NULL,
 CONSTRAINT [PK_ChucVu] PRIMARY KEY CLUSTERED 
(
	[MACV] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChuyenPhongBan]    Script Date: 01/11/2022 16:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChuyenPhongBan](
	[MACPB] [int] IDENTITY(1,1) NOT NULL,
	[MANV] [nvarchar](10) NOT NULL,
	[MAPB] [nvarchar](10) NOT NULL,
	[MAPBCHUYENDEN] [nvarchar](10) NOT NULL,
	[LYDO] [nvarchar](max) NULL,
	[NGAYCHUYEN] [date] NULL,
 CONSTRAINT [PK_ChuyenPhongBan] PRIMARY KEY CLUSTERED 
(
	[MACPB] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhongBan]    Script Date: 01/11/2022 16:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhongBan](
	[MAPB] [nvarchar](10) NOT NULL,
	[TENPB] [nvarchar](50) NULL,
	[DIACHI] [nvarchar](50) NULL,
	[SODIENTHOAI] [nvarchar](50) NULL,
 CONSTRAINT [PK_PhongBan] PRIMARY KEY CLUSTERED 
(
	[MAPB] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhanVien]    Script Date: 01/11/2022 16:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhanVien](
	[MANV] [nvarchar](10) NOT NULL,
	[HOTEN] [nvarchar](50) NULL,
	[ANHDAIDIEN] [nvarchar](max) NULL,
	[DANTOC] [nvarchar](50) NULL,
	[GIOITINH] [nvarchar](50) NULL,
	[SODIENTHOAI] [nvarchar](50) NULL,
	[QUEQUAN] [nvarchar](max) NULL,
	[NGAYSINH] [date] NULL,
	[NGAYBATDAU] [date] NULL,
	[MACV] [nvarchar](10) NULL,
	[MATDHV] [nvarchar](10) NULL,
	[MAPB] [nvarchar](10) NULL,
	[CHUYENNGANH] [nvarchar](50) NULL,
 CONSTRAINT [PK_NhanVien] PRIMARY KEY CLUSTERED 
(
	[MANV] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuanLyLuong]    Script Date: 01/11/2022 16:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuanLyLuong](
	[MAQLL] [int] IDENTITY(1,1) NOT NULL,
	[MANV] [nvarchar](10) NULL,
	[NGAYTINHLUONG] [date] NULL,
	[THUCLANH] [real] NULL,
	[LUONGCOBAN] [real] NULL,
 CONSTRAINT [PK_QuanLyLuong] PRIMARY KEY CLUSTERED 
(
	[MAQLL] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Luong]    Script Date: 01/11/2022 16:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Luong](
	[MANV] [nvarchar](10) NOT NULL,
	[LUONGCOBAN] [real] NULL,
	[HESOLUONG] [real] NULL,
	[PHUCAP] [real] NULL,
 CONSTRAINT [PK_Luong] PRIMARY KEY CLUSTERED 
(
	[MANV] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChuyenCan]    Script Date: 01/11/2022 16:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChuyenCan](
	[MACC] [int] IDENTITY(1,1) NOT NULL,
	[MANV] [nvarchar](10) NOT NULL,
	[LYDO] [nvarchar](max) NULL,
	[NGAYVANG] [date] NOT NULL,
 CONSTRAINT [PK_ChuyenCan] PRIMARY KEY CLUSTERED 
(
	[MACC] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_NhanVien_ChucVu]    Script Date: 01/11/2022 16:42:14 ******/
ALTER TABLE [dbo].[NhanVien]  WITH CHECK ADD  CONSTRAINT [FK_NhanVien_ChucVu] FOREIGN KEY([MACV])
REFERENCES [dbo].[ChucVu] ([MACV])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[NhanVien] CHECK CONSTRAINT [FK_NhanVien_ChucVu]
GO
/****** Object:  ForeignKey [FK_NhanVien_TrinhDoHocVan]    Script Date: 01/11/2022 16:42:14 ******/
ALTER TABLE [dbo].[NhanVien]  WITH CHECK ADD  CONSTRAINT [FK_NhanVien_TrinhDoHocVan] FOREIGN KEY([MATDHV])
REFERENCES [dbo].[TrinhDoHocVan] ([MATDHV])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[NhanVien] CHECK CONSTRAINT [FK_NhanVien_TrinhDoHocVan]
GO
/****** Object:  ForeignKey [FK_QuanLyLuong_NhanVien]    Script Date: 01/11/2022 16:42:14 ******/
ALTER TABLE [dbo].[QuanLyLuong]  WITH CHECK ADD  CONSTRAINT [FK_QuanLyLuong_NhanVien] FOREIGN KEY([MANV])
REFERENCES [dbo].[NhanVien] ([MANV])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[QuanLyLuong] CHECK CONSTRAINT [FK_QuanLyLuong_NhanVien]
GO
/****** Object:  ForeignKey [FK_Luong_NhanVien1]    Script Date: 01/11/2022 16:42:14 ******/
ALTER TABLE [dbo].[Luong]  WITH CHECK ADD  CONSTRAINT [FK_Luong_NhanVien1] FOREIGN KEY([MANV])
REFERENCES [dbo].[NhanVien] ([MANV])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Luong] CHECK CONSTRAINT [FK_Luong_NhanVien1]
GO
/****** Object:  ForeignKey [FK_ChuyenCan_NhanVien]    Script Date: 01/11/2022 16:42:14 ******/
ALTER TABLE [dbo].[ChuyenCan]  WITH CHECK ADD  CONSTRAINT [FK_ChuyenCan_NhanVien] FOREIGN KEY([MANV])
REFERENCES [dbo].[NhanVien] ([MANV])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChuyenCan] CHECK CONSTRAINT [FK_ChuyenCan_NhanVien]
GO
