USE [SoftCob]
GO

/****** Object:  Table [dbo].[SoftCob_BPM_CITA_CABECERA]    Script Date: 10/6/2021 19:07:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SoftCob_BPM_CITA_CABECERA](
	[CITA_CODIGO] [int] IDENTITY(1,1) NOT NULL,
	[CPCE_CODIGO] [int] NOT NULL,
	[cita_perscodigo] [int] NOT NULL,
	[cita_cldecodigo] [int] NOT NULL,
	[cita_numdocumento] [varchar](20) NOT NULL,
	[cita_estado] [varchar](5) NOT NULL,
	[cita_fechasolicitud] [datetime] NOT NULL,
	[cita_valorcitacion] [decimal](12, 2) NOT NULL,
	[cita_usuacitacion] [int] NOT NULL,
	[cita_totalcitas] [int] NOT NULL,
	[cita_observacioncita] [varchar](500) NULL,
	[cita_whatsapp] [int] NULL,
	[cita_observawhatsapp] [varchar](500) NULL,
	[cita_whatsappfin] [varchar](2) NULL,
	[cita_email] [int] NULL,
	[cita_observaemail] [varchar](500) NULL,
	[cita_emailfin] [varchar](2) NULL,
	[cita_terreno] [int] NULL,
	[cita_observaterreno] [varchar](500) NULL,
	[cita_terrenofin] [varchar](2) NULL,
	[cita_fecharutaterreno] [date] NOT NULL,
	[cita_patharchivo] [varchar](50) NULL,
	[cita_nomarchivo] [varchar](50) NULL,
	[cita_extarchivo] [varchar](5) NULL,
	[cita_contentarchivo] [varchar](50) NULL,
	[cita_fechacitacion] [date] NOT NULL,
	[cita_horacitacion] [int] NULL,
	[cita_usuariocitacion] [int] NULL,
	[cita_respuestacitacion] [varchar](250) NULL,
	[cita_convenio] [varchar](5) NULL,
	[cita_observacionconvenio] [varchar](500) NULL,
	[cita_fechaconvenio] [date] NOT NULL,
	[cita_totalconvenio] [decimal](12, 2) NULL,
	[cita_pathpagare] [varchar](50) NULL,
	[cita_nompagare] [varchar](50) NULL,
	[cita_extpagare] [varchar](5) NULL,
	[cita_contentpagare] [varchar](50) NULL,
	[cita_numjuicio] [varchar](20) NULL,
	[cita_estadojuicio] [varchar](5) NULL,
	[cita_fechajuicio] [datetime] NOT NULL,
	[cita_usuariojuicio] [int] NULL,
	[cita_pathjuicio] [varchar](150) NULL,
	[cita_nomjuicio] [varchar](50) NULL,
	[cita_extjuicio] [varchar](5) NULL,
	[cita_contentjuicio] [varchar](50) NULL,
	[cita_auxv1] [varchar](50) NULL,
	[cita_auxv2] [varchar](50) NULL,
	[cita_auxv3] [varchar](50) NULL,
	[cita_auxv4] [varchar](50) NULL,
	[cita_auxv5] [varchar](50) NULL,
	[cita_auxv6] [varchar](50) NULL,
	[cita_auxv7] [varchar](50) NULL,
	[cita_auxv8] [varchar](50) NULL,
	[cita_auxv9] [varchar](50) NULL,
	[cita_auxv10] [varchar](50) NULL,
	[cita_auxi1] [int] NULL,
	[cita_auxi2] [int] NULL,
	[cita_auxi3] [int] NULL,
	[cita_auxi4] [int] NULL,
	[cita_auxi5] [int] NULL,
	[cita_auxf1] [datetime] NULL,
	[cita_auxf2] [datetime] NULL,
	[cita_auxf3] [datetime] NULL,
	[cita_auxd1] [decimal](12, 2) NULL,
	[cita_auxd2] [decimal](12, 2) NULL,
	[cita_auxd3] [decimal](12, 2) NULL,
	[cita_fechacreacion] [datetime] NOT NULL,
	[cita_usuariocreacion] [int] NOT NULL,
	[cita_terminalcreacion] [varchar](50) NOT NULL,
	[cita_fum] [datetime] NOT NULL,
	[cita_uum] [int] NOT NULL,
	[cita_tum] [varchar](50) NOT NULL,
 CONSTRAINT [PK_SoftCob_BPM_CITA_CABECERA] PRIMARY KEY CLUSTERED 
(
	[CITA_CODIGO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SoftCob_BPM_CITA_CABECERA]  WITH CHECK ADD  CONSTRAINT [FK_SoftCob_BPM_CITA_CABECERA_SoftCob_CATALOGO_PRODUCTOS_CEDENTE] FOREIGN KEY([CPCE_CODIGO])
REFERENCES [dbo].[SoftCob_CATALOGO_PRODUCTOS_CEDENTE] ([CPCE_CODIGO])
GO

ALTER TABLE [dbo].[SoftCob_BPM_CITA_CABECERA] CHECK CONSTRAINT [FK_SoftCob_BPM_CITA_CABECERA_SoftCob_CATALOGO_PRODUCTOS_CEDENTE]
GO

