USE [GHM_HR_New_Demo]
GO
/****** Object:  StoredProcedure [dbo].[spShift_Insert]    Script Date: 14/10/2026 11:53:14 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[spShift_Insert]
(
    @Id AS VARCHAR(50),
    @CompanyId AS VARCHAR(50) = NULL,
	@TenantId AS VARCHAR(50) = NULL,
    @Name AS varchar(50) = NULL,
    @StartTime AS Time(7) = NULL,
    @EndTime As Time(7) = NULL,
    @CreateTime AS DateTime = NULL,
    @CreateId AS VARCHAR(50) = NULL, 
	@CreateFullName as varchar(50) = NULL,
	@LastUpdateTime as DateTime = NULL,
	@LastUpdatId as Varchar(50) = NULL,
	@LastUpdateFullName as varchar(50) = NULL
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

            INSERT INTO [dbo].[Shift]
           ([Id]
           ,[CompanyId]
		   ,[TenantId]
           ,[Name]
           ,[StartTime]
           ,[EndTime]
           ,[CreateTime]
           ,[CreateId]
           ,[CreateFullName]
           ,[LastUpdateTime]
           ,[LastUpdatId]
           ,[LastUpdateFullName])
     VALUES (
           @Id,
           @CompanyId,
		   @TenantId,
           @Name,
		   @StartTime,
		   @EndTime,
		   @CreateTime,
		   @CreateId,
		   @CreateFullName,
		   @LastUpdateTime,
		   @LastUpdatId,
		   @LastUpdateFullName
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

ALTER   PROCEDURE [dbo].[spShift_DeleteByID]
(
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

			Delete from [dbo].[Shift]
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

ALTER   PROCEDURE [dbo].[spShift_Insert]
(
    @Id AS VARCHAR(50),
    @CompanyId AS VARCHAR(50) = NULL,
	@TenantId AS VARCHAR(50) = NULL,
    @Name AS varchar(50) = NULL,
    @StartTime AS Time(7) = NULL,
    @EndTime As Time(7) = NULL,
    @CreateTime AS DateTime = NULL,
    @CreateId AS VARCHAR(50) = NULL, 
	@CreateFullName as varchar(50) = NULL,
	@LastUpdateTime as DateTime = NULL,
	@LastUpdatId as Varchar(50) = NULL,
	@LastUpdateFullName as varchar(50) = NULL
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

            INSERT INTO [dbo].[Shift]
           ([Id]
           ,[CompanyId]
		   ,[TenantId]
           ,[Name]
           ,[StartTime]
           ,[EndTime]
           ,[CreateTime]
           ,[CreateId]
           ,[CreateFullName]
           ,[LastUpdateTime]
           ,[LastUpdatId]
           ,[LastUpdateFullName])
     VALUES (
           @Id,
           @CompanyId,
		   @TenantId,
           @Name,
		   @StartTime,
		   @EndTime,
		   @CreateTime,
		   @CreateId,
		   @CreateFullName,
		   @LastUpdateTime,
		   @LastUpdatId,
		   @LastUpdateFullName
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

ALTER   PROCEDURE [dbo].[spShift_SelectAll]
(
	@TenantId AS VARCHAR(50) = 'dae13df6-6720-4bda-a61c-61e5e948017e',
	@CompanyId AS VARCHAR(50) = '4eab0dbb-9d1d-424d-a28a-fca1405d4401'
)
AS
BEGIN
	SET NOCOUNT ON;
	select c.Name as CompanyName, s.Name as ShiftName, s.StartTime, s.EndTime from dbo.Shift as s
	inner join dbo.Companys as c
	on s.CompanyId = c.Id
    where s.CompanyId = @CompanyId 
	and s.TenantId = @TenantId
END 

ALTER   PROCEDURE [dbo].[spShift_SelectByID]
(
	@Id AS VARCHAR(50) = 'neomedic'
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		c.Name as CompanyName,
		s.Name as Name,
		s.StartTime,
		s.EndTime,
		s.CompanyId as CompanyId
	FROM [dbo].[Shift] as s WITH (NOLOCK)
	inner join [dbo].Companys as c
	on s.CompanyId = c.Id
	WHERE 
	   @Id is null or s.[Id] = @Id
END

ALTER   PROCEDURE [dbo].[spShift_Update]
(
	@Id AS VARCHAR(50),
    @Name AS varchar(50) = NULL,
    @StartTime AS Time(7) = NULL,
    @EndTime As Time(7) = NULL,
	@LastUpdatId as Varchar(50) = NULL,
	@LastUpdateFullName as varchar(50) = NULL
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

			UPDATE [dbo].[Shift]
			SET
                 [Name] = @Name
                ,[StartTime] = @StartTime
                ,[EndTime] = @EndTime
                ,[LastUpdateTime] = SYSDATETIME()
                ,[LastUpdatId] = @LastUpdatId
                ,[LastUpdateFullName] = @LastUpdateFullName
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