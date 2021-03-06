USE [MSLibraryDB]
GO
/****** Object:  Table [dbo].[User]    Script Date: 01/13/2013 13:56:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](20) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[BirthDate] [datetime] NOT NULL,
	[Email] [nvarchar](60) NOT NULL,
	[Phone] [nvarchar](60) NULL,
	[AddDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Użytkownik' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User'
GO
/****** Object:  Table [dbo].[DictBookGenre]    Script Date: 01/13/2013 13:56:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DictBookGenre](
	[BookGenreId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_DictBookGenre] PRIMARY KEY CLUSTERED 
(
	[BookGenreId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Słownik gatunków' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DictBookGenre'
GO
SET IDENTITY_INSERT [dbo].[DictBookGenre] ON
INSERT [dbo].[DictBookGenre] ([BookGenreId], [Name]) VALUES (1, N'Horror')
INSERT [dbo].[DictBookGenre] ([BookGenreId], [Name]) VALUES (2, N'Fantasy')
INSERT [dbo].[DictBookGenre] ([BookGenreId], [Name]) VALUES (3, N'Sci-Fi')
INSERT [dbo].[DictBookGenre] ([BookGenreId], [Name]) VALUES (4, N'Biografia')
INSERT [dbo].[DictBookGenre] ([BookGenreId], [Name]) VALUES (5, N'Romans')
INSERT [dbo].[DictBookGenre] ([BookGenreId], [Name]) VALUES (6, N'Kryminał')
SET IDENTITY_INSERT [dbo].[DictBookGenre] OFF
/****** Object:  Table [dbo].[Book]    Script Date: 01/13/2013 13:56:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Book](
	[BookId] [int] IDENTITY(1,1) NOT NULL,
	[Author] [varchar](70) NOT NULL,
	[ReleaseDate] [datetime] NULL,
	[ISBN] [nvarchar](50) NOT NULL,
	[BookGenreId] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[AddDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED 
(
	[BookId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Borrow]    Script Date: 01/13/2013 13:56:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Borrow](
	[BorrowId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[BookId] [int] NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[IsReturned] [bit] NOT NULL,
 CONSTRAINT [PK_Borrow] PRIMARY KEY CLUSTERED 
(
	[BorrowId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF_User_IsActive]    Script Date: 01/13/2013 13:56:54 ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
/****** Object:  Default [DF_Borrow_IsReturned]    Script Date: 01/13/2013 13:56:54 ******/
ALTER TABLE [dbo].[Borrow] ADD  CONSTRAINT [DF_Borrow_IsReturned]  DEFAULT ((0)) FOR [IsReturned]
GO
/****** Object:  ForeignKey [fk_Book_DictBookGenre]    Script Date: 01/13/2013 13:56:54 ******/
ALTER TABLE [dbo].[Book]  WITH CHECK ADD  CONSTRAINT [fk_Book_DictBookGenre] FOREIGN KEY([BookGenreId])
REFERENCES [dbo].[DictBookGenre] ([BookGenreId])
GO
ALTER TABLE [dbo].[Book] CHECK CONSTRAINT [fk_Book_DictBookGenre]
GO
/****** Object:  ForeignKey [FK_Borrow_Book]    Script Date: 01/13/2013 13:56:54 ******/
ALTER TABLE [dbo].[Borrow]  WITH CHECK ADD  CONSTRAINT [FK_Borrow_Book] FOREIGN KEY([BookId])
REFERENCES [dbo].[Book] ([BookId])
GO
ALTER TABLE [dbo].[Borrow] CHECK CONSTRAINT [FK_Borrow_Book]
GO
/****** Object:  ForeignKey [FK_Borrow_User]    Script Date: 01/13/2013 13:56:54 ******/
ALTER TABLE [dbo].[Borrow]  WITH CHECK ADD  CONSTRAINT [FK_Borrow_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Borrow] CHECK CONSTRAINT [FK_Borrow_User]
GO
