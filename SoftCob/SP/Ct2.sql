USE [SoftCob]
GO

/****** Object:  Table [dbo].[SoftCob_BPM_CITA_DETALLE]    Script Date: 10/6/2021 19:08:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SoftCob_BPM_CITA_DETALLE](
	[HCIT_CODIGO] [int] IDENTITY(1,1) NOT NULL,
	[CITA_CODIGO] [int] NOT NULL,
	[hcit_canal] [varchar](20) NULL,
	[hcit_celular] [varchar](10) NULL,
	[hcit_correo] [varchar](80) NULL,
	[hcit_direccion] [varchar](250) NULL,
	[hcit_referencia] [varchar](250) NULL,
	[hcit_tipocliente] [varchar](5) NOT NULL,
	[hcit_definicion] [varchar](5) NOT NULL,
	[hcit_observacion] [varchar](250) NULL,
	[hcit_usuarioregistro] [int] NOT NULL,
	[hcit_fechacita] [date] NOT NULL,
	[hcit_horacita] [int] NULL,
	[hcit_respuesta] [varchar](5) NOT NULL,
	[hcit_registro] [varchar](150) NULL,
	[hcit_estado] [varchar](5) NOT NULL,
	[hcit_fechavisita] [datetime] NOT NULL,
	[cita_pathterreno] [varchar](50) NULL,
	[cita_nomterreno] [varchar](50) NULL,
	[cita_extterreno] [varchar](5) NULL,
	[cita_contentterreno] [varchar](50) NULL,
	[hcit_auxv1] [varchar](50) NULL,
	[hcit_auxv2] [varchar](50) NULL,
	[hcit_auxv3] [varchar](50) NULL,
	[hcit_auxv4] [varchar](50) NULL,
	[hcit_auxv5] [varchar](50) NULL,
	[hcit_auxv6] [varchar](50) NULL,
	[hcit_auxv7] [varchar](50) NULL,
	[hcit_auxv8] [varchar](50) NULL,
	[hcit_auxv9] [varchar](50) NULL,
	[hcit_auxv10] [varchar](50) NULL,
	[hcit_auxi1] [int] NULL,
	[hcit_auxi2] [int] NULL,
	[hcit_auxi3] [int] NULL,
	[hcit_auxi4] [int] NULL,
	[hcit_auxi5] [int] NULL,
	[hcit_fechacreacion] [datetime] NOT NULL,
	[hcit_usuariocreacion] [int] NOT NULL,
	[hcit_terminalcreacion] [varchar](50) NOT NULL,
 CONSTRAINT [PK_BPM_CITA_DETALLE] PRIMARY KEY CLUSTERED 
(
	[HCIT_CODIGO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SoftCob_BPM_CITA_DETALLE]  WITH CHECK ADD  CONSTRAINT [FK_SoftCob_BPM_CITA_DETALLE_SoftCob_BPM_CITA_CABECERA] FOREIGN KEY([CITA_CODIGO])
REFERENCES [dbo].[SoftCob_BPM_CITA_CABECERA] ([CITA_CODIGO])
GO

ALTER TABLE [dbo].[SoftCob_BPM_CITA_DETALLE] CHECK CONSTRAINT [FK_SoftCob_BPM_CITA_DETALLE_SoftCob_BPM_CITA_CABECERA]
GO
