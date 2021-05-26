USE [ipsnews]
GO
/****** Object:  Table [dbo].[event]    Script Date: 1/30/2020 4:17:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[event](
	[row_id] [int] IDENTITY(1,1) NOT NULL,
	[id] [varchar](50) NOT NULL,
	[data] [varchar](max) NULL,
	[date_created] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_event] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[feed]    Script Date: 1/30/2020 4:17:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[feed](
	[row_id] [int] IDENTITY(1,1) NOT NULL,
	[id] [varchar](50) NOT NULL,
	[data] [varchar](max) NULL,
	[date_created] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_feed] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[post]    Script Date: 1/30/2020 4:17:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[post](
	[row_id] [int] IDENTITY(1,1) NOT NULL,
	[id] [varchar](50) NOT NULL,
	[data] [varchar](max) NULL,
	[date_created] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_post] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user]    Script Date: 1/30/2020 4:17:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user](
	[row_id] [int] IDENTITY(1,1) NOT NULL,
	[id] [varchar](50) NOT NULL,
	[data] [varchar](max) NULL,
	[date_created] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[event] ADD  CONSTRAINT [DF_event_date_created]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[feed] ADD  CONSTRAINT [DF_feed_date_created]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[post] ADD  CONSTRAINT [DF_post_date_created]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[user] ADD  CONSTRAINT [DF_user_date_created]  DEFAULT (getdate()) FOR [date_created]
GO
/****** Object:  StoredProcedure [dbo].[delete_json]    Script Date: 1/30/2020 4:17:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[delete_json]
@table varchar(50),
@id varchar(50)
as

if(@table = 'feed')
begin
	delete from [feed] where id = @id
end

if(@table = 'post')
begin
	delete from [post] where id = @id
end

if(@table = 'user')
begin
	delete from [user] where id = @id
end

GO
/****** Object:  StoredProcedure [dbo].[get_json]    Script Date: 1/30/2020 4:17:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[get_json]
@table varchar(50),
@id varchar(50)
as


if(@table = 'feed')
begin
	select id, [data] from [feed] where id = @id
end

if(@table = 'post')
begin
	select id, [data] from [post] where id = @id
end

if(@table = 'user')
begin
	select id, [data] from [user] where id = @id
end

if(@table = 'event')
begin
	select id, [data] from [event] where id = @id
end
GO
/****** Object:  StoredProcedure [dbo].[get_json_all]    Script Date: 1/30/2020 4:17:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[get_json_all]
@table varchar(50),
@startAfter varchar(50) = null
as

declare @results table
(
row_id int,
id varchar(50),
[data] varchar(MAX),
date_created datetimeoffset(7)
)

if(@table = 'feed')
begin
	insert into @results
	select row_id, id, [data], date_created from [feed]
end

if(@table = 'post')
begin
	insert into @results
	select row_id, id, [data], date_created from [post]
end

if(@table = 'user')
begin
	insert into @results
	select row_id, id, [data], date_created from [user]
end

if(@table = 'event')
begin
	insert into @results
	select row_id, id, [data], date_created from [event]
end

declare @startAfterRowId int = 0
if(ISNULL(@startAfter,'') <> '')
	select @startAfterRowId = row_id from @results where id = @startAfter

select row_id as rowId, id, [data], date_created
from @results 
where row_id > @startAfterRowId
order by row_id
GO
/****** Object:  StoredProcedure [dbo].[save_json]    Script Date: 1/30/2020 4:17:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[save_json]
@table varchar(50),
@id varchar(50),
@data varchar(MAX)
as

if(@table = 'feed')
begin
	if (select count(id) from [feed] where id = @id) = 0
		insert into [feed] (id,[data]) values (@id,@data)
	else
		update [feed]
		set [data] = @data
		where id = @id
end

if(@table = 'post')
begin
	if (select count(id) from [post] where id = @id) = 0
		insert into [post] (id,[data]) values (@id,@data)
	else
		update [post]
		set [data] = @data
		where id = @id
end

if(@table = 'user')
begin
	if (select count(id) from [user] where id = @id) = 0
		insert into [user] (id,[data]) values (@id,@data)
	else
		update [user]
		set [data] = @data
		where id = @id
end

if(@table = 'event')
begin
	if (select count(id) from [event] where id = @id) = 0
		insert into [event] (id,[data]) values (@id,@data)
	else
		update [event]
		set [data] = @data
		where id = @id
end

GO
