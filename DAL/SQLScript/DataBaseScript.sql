----------------------------------------------------
-- Use this script if you had problems with old 
-- RwaMovies script. I.e. if your database version
-- was old, or if you could not fix structure for
-- Images yourself.
----------------------------------------------------

CREATE TABLE [dbo].[Country](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [char](2) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO

CREATE TABLE [dbo].[Genre](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](1024) NULL,
	CONSTRAINT [PK_Genre] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO

CREATE TABLE [dbo].[Image](
	[Id] [int] NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO

CREATE TABLE [dbo].[Notification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ReceiverEmail] [nvarchar](256) NOT NULL,
	[Subject] [nvarchar](256) NULL,
	[Body] [nvarchar](1024) NOT NULL,
	[SentAt] [datetime2](7) NULL,
	CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO

CREATE TABLE [dbo].[Tag](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO

CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[DeletedAt] [datetime2](7) NULL,
	[Username] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](256) NOT NULL,
	[LastName] [nvarchar](256) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[PwdHash] [nvarchar](256) NOT NULL,
	[PwdSalt] [nvarchar](256) NOT NULL,
	[Phone] [nvarchar](256) NULL,
	[IsConfirmed] [bit] NOT NULL,
	[SecurityToken] [nvarchar](256) NULL,
	[CountryOfResidenceId] [int] NOT NULL,
	CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO

CREATE TABLE [dbo].[Video](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](1024) NULL,
	[GenreId] [int] NOT NULL,
	[TotalSeconds] [int] NOT NULL,
	[StreamingUrl] [nvarchar](256) NULL,
	[ImageId] [int] NULL,
	CONSTRAINT [PK_Video] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO

CREATE TABLE [dbo].[VideoTag](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VideoId] [int] NOT NULL,
	[TagId] [int] NOT NULL,
	CONSTRAINT [PK_VideoTag] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO

ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsConfirmed]  DEFAULT ((0)) FOR [IsConfirmed]
GO
ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_CreatedAt]  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_TotalSeconds]  DEFAULT ((0)) FOR [TotalSeconds]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Country] FOREIGN KEY([CountryOfResidenceId])
REFERENCES [dbo].[Country] ([Id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Country]
GO
ALTER TABLE [dbo].[Video]  WITH CHECK ADD  CONSTRAINT [FK_Video_Genre] FOREIGN KEY([GenreId])
REFERENCES [dbo].[Genre] ([Id])
GO
ALTER TABLE [dbo].[Video] CHECK CONSTRAINT [FK_Video_Genre]
GO
ALTER TABLE [dbo].[Video]  WITH CHECK ADD  CONSTRAINT [FK_Video_Images] FOREIGN KEY([ImageId])
REFERENCES [dbo].[Image] ([Id])
GO
ALTER TABLE [dbo].[Video] CHECK CONSTRAINT [FK_Video_Images]
GO
ALTER TABLE [dbo].[VideoTag]  WITH CHECK ADD  CONSTRAINT [FK_VideoTag_Tag] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tag] ([Id])
GO
ALTER TABLE [dbo].[VideoTag] CHECK CONSTRAINT [FK_VideoTag_Tag]
GO
ALTER TABLE [dbo].[VideoTag]  WITH CHECK ADD  CONSTRAINT [FK_VideoTag_Video] FOREIGN KEY([VideoId])
REFERENCES [dbo].[Video] ([Id])
GO
ALTER TABLE [dbo].[VideoTag] CHECK CONSTRAINT [FK_VideoTag_Video]
GO
INSERT INTO [dbo].[Country](Code,Name) values ('HR','Croatia')
GO
INSERT INTO [dbo].[Genre](Name, Description) values ('Triler','Scary')
GO
INSERT INTO [dbo].[Image](Id,Content) values (1,'https://s3.amazonaws.com/static.rogerebert.com/uploads/movie/movie_poster/thriller-2019/large_Thriller_Poster.jpg');
GO
INSERT INTO [dbo].[Image](Id,Content) values (2,'https://m.media-amazon.com/images/M/MV5BMGEzZjdjMGQtZmYzZC00N2I4LThiY2QtNWY5ZmQ3M2ExZmM4XkEyXkFqcGdeQXVyMTQxNzMzNDI@._V1_.jpg');
GO
INSERT INTO [dbo].[Image](Id,Content) values (3,'https://filmfare.wwmindia.com/content/2021/aug/best-comedy-movies-hollywood-were-the-millers.jpg');
GO
INSERT INTO [dbo].[Image](Id,Content) values (4,'https://hips.hearstapps.com/hmg-prod/images/the-incredibles-1529421699.jpg?crop=0.988xw:1.00xh;0.00518xw,0&resize=980');
GO
INSERT INTO [dbo].[Image](Id,Content) values (5,'https://ew.com/thmb/8qdSdjnIgWHj-b5c7xkQAT8Arlc=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/Netflix-drama-movies-032724-506959c285ca41a3be21595593bff2ae.jpg');
