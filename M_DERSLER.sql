USE [AdventureWorks]
GO

/****** Object:  Table [dbo].[M_DERSLER]    Script Date: 10.06.2022 11:35:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[M_DERSLER](
	[DERSID] [INT] IDENTITY(1,1) NOT NULL,
	[DERSAD] [VARCHAR](50) NULL,
 CONSTRAINT [PK_M_DERSLER] PRIMARY KEY CLUSTERED 
(
	[DERSID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


