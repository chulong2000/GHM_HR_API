USE [GHM_HR_New_Demo]
GO
/****** Object:  StoredProcedure [dbo].[spPosition_Code]    Script Date: 25/09/2026 11:23:35 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spPosition_Code]
(
	@TenantId AS VARCHAR(50) ='dae13df6-6720-4bda-a61c-61e5e948017e',
	@CompanyId AS VARCHAR(50) = null,
	@PositionIdHIS as VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id FROM Positions
	WHERE TenantId=@TenantId and CompanyId=@CompanyId and PositionId_HIS=@PositionIdHIS
END
GO
/****** Object:  StoredProcedure [dbo].[spPosition_DeleteByID]    Script Date: 25/09/2026 11:23:35 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Create by : Nguyen Dac Quang
--Create date : 20/01/2025 16:18:06
--Description :
--Output :
--Modify :
--Project :Quản lý Nhân Sự
-----------------------o0o-----------------------

CREATE PROCEDURE [dbo].[spPosition_DeleteByID]
(
	@Id AS VARCHAR(50),
	@DeleteUserId AS VARCHAR(50) = NULL,
	@DeleteFullName AS NVARCHAR(300) = NULL
)
AS
BEGIN
	DECLARE @RetryCount INT = 10;
	DECLARE @CurrentAttempt INT = 0;
	DECLARE @Success BIT = 0;
	DECLARE @DelayTime INT = 1;

	WHILE @CurrentAttempt < @RetryCount AND @Success = 0
	BEGIN
		BEGIN TRY
		BEGIN TRANSACTION;

			UPDATE [dbo].[Positions]
			SET
				[IsDelete] = 1,
				[DeleteTime] = GETDATE(),
				[DeleteUserId] = @DeleteUserId,
				[DeleteFullName] = @DeleteFullName
			WHERE 
				[Id]=@Id 

		COMMIT TRANSACTION;
			SET @Success = 1;
		END TRY
		BEGIN CATCH
		IF XACT_STATE() <> 0
		BEGIN
			ROLLBACK TRANSACTION;
		END

		IF ERROR_NUMBER() = 1205 -- Error code for deadlock
		BEGIN
			SET @CurrentAttempt = @CurrentAttempt + 1;

			IF @CurrentAttempt > 7
			BEGIN
				SET @DelayTime = @DelayTime + 1;
			END
			-- Wait for a short time before retrying
			DECLARE @DelayString NVARCHAR(8);
			SET @DelayString = '00:00:' + RIGHT('0' + CAST(@DelayTime AS VARCHAR(2)), 2);
			WAITFOR DELAY @DelayString; -- delay in seconds
		END
		ELSE
		BEGIN
			THROW;
		END
		END CATCH;
	END

	IF @Success = 0
	BEGIN
		RAISERROR('Transaction failed after retrying.', 16, 1);
	END

END 
GO
/****** Object:  StoredProcedure [dbo].[spPosition_ForceDeleteByID]    Script Date: 25/09/2026 11:23:35 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Create by : Nguyen Dac Quang
--Create date : 20/01/2025 16:01:42
--Description :
--Output :
--Modify :
--Project :Quản lý Nhân Sự
-----------------------o0o-----------------------

CREATE PROCEDURE [dbo].[spPosition_ForceDeleteByID]
(
	@CompanyId AS VARCHAR(50),
	@Id AS VARCHAR(50)
)
AS
BEGIN
	DECLARE @RetryCount INT = 10;
	DECLARE @CurrentAttempt INT = 0;
	DECLARE @Success BIT = 0;
	DECLARE @DelayTime INT = 1;

	WHILE @CurrentAttempt < @RetryCount AND @Success = 0
	BEGIN
		BEGIN TRY
		BEGIN TRANSACTION;

			DELETE FROM [dbo].[Positions]
			WHERE 
				[Id]=@Id AND [CompanyId] = @CompanyId

		COMMIT TRANSACTION;
			SET @Success = 1;
		END TRY
		BEGIN CATCH
		IF XACT_STATE() <> 0
		BEGIN
			ROLLBACK TRANSACTION;
		END

		IF ERROR_NUMBER() = 1205 -- Error code for deadlock
		BEGIN
			SET @CurrentAttempt = @CurrentAttempt + 1;

			IF @CurrentAttempt > 7
			BEGIN
				SET @DelayTime = @DelayTime + 1;
			END
			-- Wait for a short time before retrying
			DECLARE @DelayString NVARCHAR(8);
			SET @DelayString = '00:00:' + RIGHT('0' + CAST(@DelayTime AS VARCHAR(2)), 2);
			WAITFOR DELAY @DelayString; -- delay in seconds
		END
		ELSE
		BEGIN
			THROW;
		END
		END CATCH;
	END

	IF @Success = 0
	BEGIN
		RAISERROR('Transaction failed after retrying.', 16, 1);
	END

END 
GO
/****** Object:  StoredProcedure [dbo].[spPosition_Insert]    Script Date: 25/09/2026 11:23:35 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Create by : Nguyen Dac Quang
--Create date : 20/01/2025 15:57:42
--Description :
--Output :
--Modify :
--Project :Quản lý Nhân Sự
-----------------------o0o-----------------------

CREATE PROCEDURE [dbo].[spPosition_Insert]
(
	@Id AS VARCHAR(50),
	@TenantId AS VARCHAR(50) = NULL,
	@CompanyId AS VARCHAR(50) = NULL,
	@Code AS NVARCHAR(50) = NULL,
	@Name AS NVARCHAR(50) = NULL,
	@Description AS NVARCHAR(MAX) = NULL,
	@IsMultiple AS BIT = NULL,
	@IsActive AS BIT = NULL,
	@IsDelete AS BIT = NULL,
	@ConcurrencyStamp AS VARCHAR(50) = NULL,
	@CreateTime AS DATETIME2 = NULL,
	@CreatorId AS VARCHAR(50) = NULL,
	@CreatorFullName AS NVARCHAR(300) = NULL,
	@LastUpdate AS DATETIME2 = NULL,
	@LastUpdatedUserId AS VARCHAR(50) = NULL,
	@LastUpdateFullName AS NVARCHAR(300) = NULL,
	@DeleteTime AS DATETIME2 = NULL,
	@DeleteUserId AS VARCHAR(50) = NULL,
	@DeleteFullName AS NVARCHAR(300) = NULL
)
AS
BEGIN

	DECLARE @RetryCount INT = 10;
	DECLARE @CurrentAttempt INT = 0;
	DECLARE @Success BIT = 0;
	DECLARE @DelayTime INT = 1;

	WHILE @CurrentAttempt < @RetryCount AND @Success = 0
	BEGIN
		BEGIN TRY
		BEGIN TRANSACTION;

			INSERT INTO [dbo].[Positions]
			(
				[Id],
				[TenantId],
				[CompanyId],
				[Code],
				[Name],
				[Description],
				[IsMultiple],
				[IsActive],
				[IsDelete],
				[ConcurrencyStamp],
				[CreateTime],
				[CreatorId],
				[CreatorFullName],
				[LastUpdate],
				[LastUpdatedUserId],
				[LastUpdateFullName],
				[DeleteTime],
				[DeleteUserId],
				[DeleteFullName]
			) 
			VALUES
			(
				@Id,
				@TenantId,
				@CompanyId,
				@Code,
				@Name,
				@Description,
				@IsMultiple,
				@IsActive,
				@IsDelete,
				@ConcurrencyStamp,
				@CreateTime,
				@CreatorId,
				@CreatorFullName,
				@LastUpdate,
				@LastUpdatedUserId,
				@LastUpdateFullName,
				@DeleteTime,
				@DeleteUserId,
				@DeleteFullName
			) 

		COMMIT TRANSACTION;
			SET @Success = 1;
		END TRY
		BEGIN CATCH
		IF XACT_STATE() <> 0
		BEGIN
			ROLLBACK TRANSACTION;
		END

		IF ERROR_NUMBER() = 1205 -- Error code for deadlock
		BEGIN
			SET @CurrentAttempt = @CurrentAttempt + 1;

			IF @CurrentAttempt > 7
			BEGIN
				SET @DelayTime = @DelayTime + 1;
			END
			-- Wait for a short time before retrying
			DECLARE @DelayString NVARCHAR(8);
			SET @DelayString = '00:00:' + RIGHT('0' + CAST(@DelayTime AS VARCHAR(2)), 2);
			WAITFOR DELAY @DelayString; -- delay in seconds
		END
		ELSE
		BEGIN
			THROW;
		END
		END CATCH;
	END

	IF @Success = 0
	BEGIN
		RAISERROR('Transaction failed after retrying.', 16, 1);
	END

END 
GO
/****** Object:  StoredProcedure [dbo].[spPosition_SelectAll]    Script Date: 25/09/2026 11:23:35 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Create by : Nguyen Dac Quang
--Create date : 20/01/2025 16:14:44
--Description :
--Output :
--Modify :
--Project :Quản lý Nhân Sự
-----------------------o0o-----------------------


CREATE PROCEDURE [dbo].[spPosition_SelectAll]
(
	@TenantId AS VARCHAR(50) = 'dae13df6-6720-4bda-a61c-61e5e948017e',
	@CompanyId AS VARCHAR(50) = 'thaithinhmedic'
)
AS
BEGIN
	SELECT [Id], [CompanyId], [Code], [Name], [Description], [IsMultiple], [IsActive],[ConcurrencyStamp]
	FROM [dbo].[Positions] WITH (NOLOCK)
	WHERE [TenantId] = @TenantId AND [IsDelete] = 0 AND [CompanyId]=@CompanyId AND [IsMultiple] = 0
	UNION ALL
	SELECT [Id], [CompanyId], [Code], [Name], [Description], [IsMultiple], [IsActive],[ConcurrencyStamp]
	FROM [dbo].[Positions] WITH (NOLOCK)
	WHERE [TenantId] = @TenantId AND [IsDelete] = 0 AND [IsMultiple]=1
END 
GO
/****** Object:  StoredProcedure [dbo].[spPosition_SelectAll_Active]    Script Date: 25/09/2026 11:23:35 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spPosition_SelectAll_Active]
(
	@TenantId AS VARCHAR(50) = 'dae13df6-6720-4bda-a61c-61e5e948017e',
	@CompanyId AS VARCHAR(50) = 'thaithinhmedic'
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [CompanyId], [Code], [Name], [Description], [IsMultiple], [IsActive],[ConcurrencyStamp]
	FROM [dbo].[Positions] WITH (NOLOCK)
	WHERE [TenantId] = @TenantId AND [IsDelete] = 0 AND [IsActive] = 1 AND [CompanyId]=@CompanyId AND [IsMultiple] = 0
	UNION ALL
	SELECT [Id], [CompanyId], [Code], [Name], [Description], [IsMultiple], [IsActive],[ConcurrencyStamp]
	FROM [dbo].[Positions] WITH (NOLOCK)
	WHERE [TenantId] = @TenantId AND [IsDelete] = 0 AND [IsActive] = 1 AND [IsMultiple]=1
END 
GO
/****** Object:  StoredProcedure [dbo].[spPosition_SelectByID]    Script Date: 25/09/2026 11:23:35 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Create by : Nguyen Dac Quang
--Create date : 20/01/2025 16:12:05
--Description :
--Output :
--Modify :
--Project :Quản lý Nhân Sự
-----------------------o0o-----------------------

CREATE PROCEDURE [dbo].[spPosition_SelectByID]
(
	@Id AS VARCHAR(50)='27d6a897-fea7-47c1-8cd5-92fdafce2b62'
)
AS
BEGIN
	SELECT [Id],[TenantId],[CompanyId],[Code],[Name],[Description],[IsMultiple],[IsActive],[ConcurrencyStamp]
	FROM [dbo].[Positions] WITH (NOLOCK)
	WHERE [Id]=@Id AND [IsDelete] = 0
END 
GO
/****** Object:  StoredProcedure [dbo].[spPosition_Update]    Script Date: 25/09/2026 11:23:35 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Create by : Nguyen Dac Quang
--Create date : 20/01/2025 16:13:14
--Description :
--Output :
--Modify :
--Project :Quản lý Nhân Sự
-----------------------o0o-----------------------

CREATE PROCEDURE [dbo].[spPosition_Update]
(
	@Id AS VARCHAR(50),
	@TenantId AS VARCHAR(50) = NULL,
	@CompanyId AS VARCHAR(50) = NULL,
	@Code AS VARCHAR(50) = null,
	@Name AS NVARCHAR(50) = NULL,
	@Description AS NVARCHAR(MAX) = NULL,
	@IsMultiple AS BIT = NULL,
	@IsActive AS BIT = NULL,
	@IsDelete AS BIT = NULL,
	@ConcurrencyStamp AS VARCHAR(50) = NULL,
	@CreateTime AS DATETIME2 = NULL,
	@CreatorId AS VARCHAR(50) = NULL,
	@CreatorFullName AS NVARCHAR(300) = NULL,
	@LastUpdate AS DATETIME2 = NULL,
	@LastUpdatedUserId AS VARCHAR(50) = NULL,
	@LastUpdateFullName AS NVARCHAR(300) = NULL,
	@DeleteTime AS DATETIME2 = NULL,
	@DeleteUserId AS VARCHAR(50) = NULL,
	@DeleteFullName AS NVARCHAR(300) = NULL
)
AS
BEGIN

	DECLARE @RetryCount INT = 10;
	DECLARE @CurrentAttempt INT = 0;
	DECLARE @Success BIT = 0;
	DECLARE @DelayTime INT = 1;

	WHILE @CurrentAttempt < @RetryCount AND @Success = 0
	BEGIN
		BEGIN TRY
		BEGIN TRANSACTION;

			UPDATE [dbo].[Positions]
			SET
				[TenantId] = COALESCE(@TenantId, TenantId),
				[CompanyId] = COALESCE(@CompanyId, CompanyId),
				[Code] = COALESCE(@Code, Code),
				[Name] = COALESCE(@Name, Name),
				[Description] = COALESCE(@Description, Description),
				[IsMultiple] = COALESCE(@IsMultiple, IsMultiple),
				[IsActive] = COALESCE(@IsActive, IsActive),
				[IsDelete] = COALESCE(@IsDelete, IsDelete),
				[ConcurrencyStamp] = COALESCE(@ConcurrencyStamp, ConcurrencyStamp),
				[CreateTime] = COALESCE(@CreateTime, CreateTime),
				[CreatorId] = COALESCE(@CreatorId, CreatorId),
				[CreatorFullName] = COALESCE(@CreatorFullName, CreatorFullName),
				[LastUpdate] = COALESCE(@LastUpdate, LastUpdate),
				[LastUpdatedUserId] = COALESCE(@LastUpdatedUserId, LastUpdatedUserId),
				[LastUpdateFullName] = COALESCE(@LastUpdateFullName, LastUpdateFullName),
				[DeleteTime] = COALESCE(@DeleteTime, DeleteTime),
				[DeleteUserId] = COALESCE(@DeleteUserId, DeleteUserId),
				[DeleteFullName] = COALESCE(@DeleteFullName, DeleteFullName)
			WHERE 
				[Id] = @Id

		COMMIT TRANSACTION;
			SET @Success = 1;
		END TRY
		BEGIN CATCH
		IF XACT_STATE() <> 0
		BEGIN
			ROLLBACK TRANSACTION;
		END

		IF ERROR_NUMBER() = 1205 -- Error code for deadlock
		BEGIN
			SET @CurrentAttempt = @CurrentAttempt + 1;

			IF @CurrentAttempt > 7
			BEGIN
				SET @DelayTime = @DelayTime + 1;
			END
			-- Wait for a short time before retrying
			DECLARE @DelayString NVARCHAR(8);
			SET @DelayString = '00:00:' + RIGHT('0' + CAST(@DelayTime AS VARCHAR(2)), 2);
			WAITFOR DELAY @DelayString; -- delay in seconds
		END
		ELSE
		BEGIN
			THROW;
		END
		END CATCH;
	END

	IF @Success = 0
	BEGIN
		RAISERROR('Transaction failed after retrying.', 16, 1);
	END

END 
GO
/****** Object:  StoredProcedure [dbo].[spPosition_Update_IsActive]    Script Date: 25/09/2026 11:23:35 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCedure [dbo].[spPosition_Update_IsActive]
(
 @ComanyId AS VARCHAR(50) = NULL,
 @Id AS VARCHAR(50) = NULL,
 @IsActive AS BIT = NULL
)
AS 
BEGIN
	UPDATE [dbo].[Positions] 
	SET [IsActive] = @IsActive
	WHERE [Id] = @Id AND [IsDelete] = 0 AND [CompanyId]=@ComanyId
END
GO
