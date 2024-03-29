USE [master]
GO
/****** Object:  Database [FileAudit]    Script Date: 17/01/2020 19:39:41 ******/
CREATE DATABASE [FileAudit]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FileAudit', FILENAME = N'D:\SQLDATA\MSSQL14.MSSQLSERVER\MSSQL\DATA\FileAudit.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FileAudit_log', FILENAME = N'D:\SQLDATA\MSSQL14.MSSQLSERVER\MSSQL\DATA\FileAudit_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [FileAudit] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FileAudit].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FileAudit] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FileAudit] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FileAudit] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FileAudit] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FileAudit] SET ARITHABORT OFF 
GO
ALTER DATABASE [FileAudit] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FileAudit] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FileAudit] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FileAudit] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FileAudit] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FileAudit] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FileAudit] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FileAudit] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FileAudit] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FileAudit] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FileAudit] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FileAudit] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FileAudit] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FileAudit] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FileAudit] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FileAudit] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FileAudit] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FileAudit] SET RECOVERY FULL 
GO
ALTER DATABASE [FileAudit] SET  MULTI_USER 
GO
ALTER DATABASE [FileAudit] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FileAudit] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FileAudit] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FileAudit] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FileAudit] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'FileAudit', N'ON'
GO
ALTER DATABASE [FileAudit] SET QUERY_STORE = OFF
GO
USE [FileAudit]
GO
/****** Object:  Table [dbo].[eWatcher]    Script Date: 17/01/2020 19:39:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[eWatcher](
	[EventID] [int] NULL,
	[RecordID] [bigint] NOT NULL,
	[MachineName] [nvarchar](255) NULL,
	[Name] [nvarchar](255) NULL,
	[UserName] [nvarchar](255) NULL,
	[DomainName] [nvarchar](255) NULL,
	[IpAddress] [nvarchar](255) NULL,
	[ObjectName] [nvarchar](255) NULL,
	[RelativeTargetName] [nvarchar](255) NULL,
	[HandleID] [nvarchar](255) NULL,
	[AccessList] [nvarchar](255) NULL,
	[AccessMask] [nvarchar](255) NULL,
	[ProcessName] [nvarchar](255) NULL,
	[TimeGenerated] [datetime] NULL,
	[Stat] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[fsWatch]    Script Date: 17/01/2020 19:39:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[fsWatch](
	[ID] [uniqueidentifier] NOT NULL,
	[WhenHappened] [datetime] NULL,
	[Name] [nvarchar](127) NULL,
	[FullName] [nvarchar](255) NULL,
	[OldName] [nvarchar](127) NULL,
	[OldFullName] [nvarchar](255) NULL,
	[ChangeType] [int] NULL,
	[UserName] [nvarchar](127) NULL,
	[SourceIp] [nvarchar](127) NULL,
	[Stat] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Index [IX_eWatcher]    Script Date: 17/01/2020 19:39:42 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_eWatcher] ON [dbo].[eWatcher]
(
	[RecordID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_fsWatch]    Script Date: 17/01/2020 19:39:42 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_fsWatch] ON [dbo].[fsWatch]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[SaveEwValue]    Script Date: 17/01/2020 19:39:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[SaveEwValue]
@pEventID int
,@pRecordID bigint
--,@pActivityID nvarchar(255)
--,@pRelatedActivityID nvarchar(255)
,@pMachineName nvarchar(255)
,@pName nvarchar(255)
,@pUserName nvarchar(255)
,@pDomainName nvarchar(255)
,@pIpAddress nvarchar(255)
,@pObjectName nvarchar(255)
,@pRelativeTargetName nvarchar(255)
,@pHandleID nvarchar(255)
,@pAccessList nvarchar(255)
,@pAccessMask nvarchar(255)
,@pProcessName nvarchar(255)
,@pTimeGenerated datetime
,@pStat int
AS
INSERT INTO [dbo].[eWatcher]
           ([EventID]
           ,[RecordID]
		   --,[ActivityID]
		   --,[RelatedActivityID]
           ,[MachineName]
           ,[Name]
           ,[UserName]
           ,[DomainName]
           ,[IpAddress]
           ,[ObjectName]
		   ,[RelativeTargetName]
           ,[HandleID]
           ,[AccessList]
           ,[AccessMask]
           ,[ProcessName]
           ,[TimeGenerated]
           ,[Stat])
     VALUES
           (@pEventID
           ,@pRecordID
		   --,@pActivityID
		   --,@pRelatedActivityID
           ,@pMachineName
           ,@pName
           ,@pUserName
           ,@pDomainName
           ,@pIpAddress
           ,@pObjectName
		   ,@pRelativeTargetName
           ,@pHandleID
           ,@pAccessList
           ,@pAccessMask
           ,@pProcessName
           ,@pTimeGenerated
           ,@pStat)
GO
/****** Object:  StoredProcedure [dbo].[SaveFsValue]    Script Date: 17/01/2020 19:39:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[SaveFsValue]
 @pID nvarchar(63)
,@pWhenHappened datetime
,@pName nvarchar(127)
,@pFullName nvarchar(255)
,@pOldName nvarchar(127)
,@pOldFullName nvarchar(255)
,@pChangeType int
,@pUserName nvarchar(127)
,@pSourceIp nvarchar(127)
,@pStat bit
AS

IF(not exists(select * from fsWatch where ID = @pID))
	BEGIN
		INSERT INTO [dbo].[fsWatch]
				   ([ID]
				   ,[WhenHappened]
				   ,[Name]
				   ,[FullName]
				   ,[OldName]
				   ,[OldFullName]
				   ,[ChangeType]
				   ,[UserName]
				   ,[SourceIp]
				   ,[Stat])
			 VALUES
				   (@pID
				   ,@pWhenHappened
				   ,@pName
				   ,@pFullName
				   ,@pOldName
				   ,@pOldFullName
				   ,@pChangeType
				   ,@pUserName
				   ,@pSourceIp
				   ,@pStat)
	END
ELSE
	BEGIN	
		UPDATE [dbo].[fsWatch]
		   SET 
			   [WhenHappened] = @pWhenHappened
			  ,[Name] = @pName
			  ,[FullName] = @pFullName
			  ,[OldName] = @pOldName
			  ,[OldFullName] = @pOldFullName
			  ,[ChangeType] = @pChangeType
			  ,[UserName] = @pUserName
			  ,[SourceIp] = @pSourceIp
			  ,[Stat] = @pStat
		 WHERE [ID] = @pID
	END
GO
USE [master]
GO
ALTER DATABASE [FileAudit] SET  READ_WRITE 
GO
