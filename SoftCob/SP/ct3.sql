USE [SoftCob]
GO

/****** Object:  Table [dbo].[SoftCob_BPM_CORREO_DIRECCIONES]    Script Date: 10/6/2021 19:08:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SoftCob_BPM_CORREO_DIRECCIONES](
	[MATD_CODIGO] [int] IDENTITY(1,1) NOT NULL,
	[matd_numerodocumento] [varchar](20) NOT NULL,
	[matd_tipo] [varchar](10) NOT NULL,
	[matd_tipocliente] [varchar](5) NULL,
	[matd_definicion] [varchar](20) NOT NULL,
	[matd_email] [varchar](100) NULL,
	[matd_direccion] [varchar](250) NULL,
	[matd_referencia] [varchar](250) NULL,
	[matd_auxv1] [varchar](50) NOT NULL,
	[matd_auxv2] [varchar](50) NOT NULL,
	[matd_auxv3] [varchar](50) NOT NULL,
	[matd_auxi1] [int] NOT NULL,
	[matd_auxi2] [int] NOT NULL,
	[matd_auxi3] [int] NOT NULL,
	[matd_fechacreacion] [datetime] NULL,
	[matd_usuariocreacion] [int] NULL,
	[matd_terminalcreacion] [varchar](50) NULL,
	[matd_fum] [datetime] NULL,
	[matd_uum] [int] NULL,
	[matd_tum] [varchar](50) NULL,
 CONSTRAINT [PK_SoftCob_BPM_CORREO_DIRECCIONES] PRIMARY KEY CLUSTERED 
(
	[MATD_CODIGO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

