/****** Object:  Table [dbo].[city]    Script Date: 11/1/2023 1:06:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[city](
	[Id] [bigint] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[StateId] [bigint] NULL,
 CONSTRAINT [PK_city] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[country]    Script Date: 11/1/2023 1:06:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[country](
	[Id] [bigint] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_country] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[location_type]    Script Date: 11/1/2023 1:06:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[location_type](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_location_type] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[state]    Script Date: 11/1/2023 1:06:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[state](
	[Id] [bigint] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[CountryId] [bigint] NULL,
 CONSTRAINT [PK_state] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user_location]    Script Date: 11/1/2023 1:06:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user_location](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[ZipCode] [nvarchar](max) NULL,
	[Province] [nvarchar](max) NULL,
	[CountryId] [bigint] NULL,
	[LocationTypeId] [bigint] NULL,
	[UserAppId] [bigint] NULL,
 CONSTRAINT [PK_user_location] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[userapp]    Script Date: 11/1/2023 1:06:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[userapp](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NULL,
	[DateBirth] [datetime2](7) NULL,
	[Age] [int] NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[CountryId] [bigint] NULL,
 CONSTRAINT [PK_user_app] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_city_StateId]    Script Date: 11/1/2023 1:06:50 AM ******/
CREATE NONCLUSTERED INDEX [IX_city_StateId] ON [dbo].[city]
(
	[StateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_state_CountryId]    Script Date: 11/1/2023 1:06:50 AM ******/
CREATE NONCLUSTERED INDEX [IX_state_CountryId] ON [dbo].[state]
(
	[CountryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_user_location_CountryId]    Script Date: 11/1/2023 1:06:50 AM ******/
CREATE NONCLUSTERED INDEX [IX_user_location_CountryId] ON [dbo].[user_location]
(
	[CountryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_user_location_LocationTypeId]    Script Date: 11/1/2023 1:06:50 AM ******/
CREATE NONCLUSTERED INDEX [IX_user_location_LocationTypeId] ON [dbo].[user_location]
(
	[LocationTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_user_location_UserAppId]    Script Date: 11/1/2023 1:06:50 AM ******/
CREATE NONCLUSTERED INDEX [IX_user_location_UserAppId] ON [dbo].[user_location]
(
	[UserAppId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_user_app_CountryId]    Script Date: 11/1/2023 1:06:50 AM ******/
CREATE NONCLUSTERED INDEX [IX_user_app_CountryId] ON [dbo].[userapp]
(
	[CountryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[city]  WITH CHECK ADD  CONSTRAINT [FK_city_state_StateId] FOREIGN KEY([StateId])
REFERENCES [dbo].[state] ([Id])
GO
ALTER TABLE [dbo].[city] CHECK CONSTRAINT [FK_city_state_StateId]
GO
ALTER TABLE [dbo].[state]  WITH CHECK ADD  CONSTRAINT [FK_state_country_CountryId] FOREIGN KEY([CountryId])
REFERENCES [dbo].[country] ([Id])
GO
ALTER TABLE [dbo].[state] CHECK CONSTRAINT [FK_state_country_CountryId]
GO
ALTER TABLE [dbo].[user_location]  WITH CHECK ADD  CONSTRAINT [FK_user_location_country_CountryId] FOREIGN KEY([CountryId])
REFERENCES [dbo].[country] ([Id])
GO
ALTER TABLE [dbo].[user_location] CHECK CONSTRAINT [FK_user_location_country_CountryId]
GO
ALTER TABLE [dbo].[user_location]  WITH CHECK ADD  CONSTRAINT [FK_user_location_location_type_LocationTypeId] FOREIGN KEY([LocationTypeId])
REFERENCES [dbo].[location_type] ([Id])
GO
ALTER TABLE [dbo].[user_location] CHECK CONSTRAINT [FK_user_location_location_type_LocationTypeId]
GO
ALTER TABLE [dbo].[user_location]  WITH CHECK ADD  CONSTRAINT [FK_user_location_user_app_UserAppId] FOREIGN KEY([UserAppId])
REFERENCES [dbo].[userapp] ([Id])
GO
ALTER TABLE [dbo].[user_location] CHECK CONSTRAINT [FK_user_location_user_app_UserAppId]
GO
ALTER TABLE [dbo].[userapp]  WITH CHECK ADD  CONSTRAINT [FK_user_app_country_CountryId] FOREIGN KEY([CountryId])
REFERENCES [dbo].[country] ([Id])
GO
ALTER TABLE [dbo].[userapp] CHECK CONSTRAINT [FK_user_app_country_CountryId]
GO
