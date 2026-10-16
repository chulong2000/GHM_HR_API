ALTER   procedure [dbo].[spHoliday_CheckExistUser](
	@TenantId AS VARCHAR(50) = 'dae13df6-6720-4bda-a61c-61e5e948017e',
	@CompanyId AS VARCHAR(50) = '4eab0dbb-9d1d-424d-a28a-fca1405d4401',
	@Name AS VARCHAR(50) = 'ngay le',
	@Id AS uniqueidentifier
)
AS
BEGIN
	SELECT IIF (EXISTS (SELECT 1 FROM [dbo].[Holidays]
						WHERE TenantId = @TenantId AND
						CompanyId = @CompanyId AND
						Name = @Name and
						Id <> @Id
						), 1, 0)
END

ALTER   procedure [dbo].[spHoliday_CheckNameExists](
	@TenantId AS VARCHAR(50) = 'dae13df6-6720-4bda-a61c-61e5e948017e',
	@CompanyId AS VARCHAR(50) = '4eab0dbb-9d1d-424d-a28a-fca1405d4401',
	@Name AS VARCHAR(50) = 'ngay le',
	@HolidayId AS uniqueidentifier
)
AS
BEGIN
	SELECT IIF (EXISTS (SELECT 1 FROM [dbo].[Holidays]
						WHERE TenantId = @TenantId AND
						CompanyId = @CompanyId AND
						Name = @Name and
						Id <> @HolidayId
						), 1, 0)
END

ALTER   PROCEDURE [dbo].[spHoliday_DeleteByID]
(
	@Id AS VARCHAR(50),
	@DeleteUserId AS uniqueidentifier,
	@DeleteFullName as varchar(60),
	@DeleteTime as datetime
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

			Update dbo.Holidays
			set IsDelete = 1,
			    DeleteFullName = @DeleteFullName,
				DeleteUserId = @DeleteUserId,
			    DeleteTime = @DeleteTime
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

ALTER   PROCEDURE [dbo].[spHoliday_Insert]
(
    @Id uniqueidentifier,
	@TenantId uniqueidentifier,
	@CompanyId uniqueidentifier,
	@FromDay datetime,
	@ToDay datetime,
	@Name nvarchar(60),
	@Description varchar(80) = NULL,
	@CreateTime datetime,
	@CreateId uniqueidentifier,
	@CreateFullName nvarchar(80),
	@LastUpdateTime datetime = NULL,
	@LastUpdateId uniqueidentifier = NULL,
	@LastUpdateFullName nvarchar(80) = NULL,
	@IsDelete Bit = 0
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

		INSERT INTO [dbo].[Holidays]
           ([Id]
           ,[TenantId]
           ,[CompanyId]
           ,[FromDay]
           ,[ToDay]
           ,[Name]
           ,[Description]
           ,[CreateTime]
           ,[CreateId]
           ,[CreateFullName]
           ,[LastUpdateTime]
           ,[LastUpdateId]
           ,[LastUpdateFullName]
		   ,[IsDelete])
     VALUES
           (@Id
           ,@TenantId
           ,@CompanyId
           ,@FromDay
           ,@ToDay
           ,@Name
           ,@Description
           ,@CreateTime
           ,@CreateId
           ,@CreateFullName
           ,@LastUpdateTime
           ,@LastUpdateId
           ,@LastUpdateFullName
		   ,0)
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

ALTER   PROCEDURE [dbo].[spHoliday_SelectAll]
(
	@TenantId AS VARCHAR(50) = 'dae13df6-6720-4bda-a61c-61e5e948017e',
	@CompanyId AS VARCHAR(50) = '4eab0dbb-9d1d-424d-a28a-fca1405d4401'
)
AS
BEGIN
	SET NOCOUNT ON;
	select c.Name as CompanyName, h.Name as Name, h.FromDay, h.ToDay from dbo.Holidays as h
	inner join dbo.Companys as c
	on h.CompanyId = c.Id
	where h.CompanyId = @CompanyId 
	and h.TenantId = @TenantId
	and h.IsDelete = 0
	and c.IsDelete = 0
END

ALTER PROCEDURE [dbo].[spHoliday_Update]
(
    @Id uniqueidentifier,
    @TenantId uniqueidentifier,
    @CompanyId uniqueidentifier,
    @FromDay datetime,
    @ToDay datetime,
    @Name nvarchar(60),
    @Description varchar(80),
    @LastUpdateTime datetime,
    @LastUpdateId uniqueidentifier,
    @LastUpdateFullName nvarchar(80),
	@IsDelete bit = 0
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

		UPDATE [dbo].[Holidays]
        SET 
           [TenantId] = @TenantId
          ,[CompanyId] = @CompanyId
          ,[FromDay] = @FromDay
          ,[ToDay] = @ToDay
          ,[Name] = @Name
          ,[Description] = @Description
          ,[LastUpdateTime] = @LastUpdateTime
          ,[LastUpdateId] = @LastUpdateId
          ,[LastUpdateFullName] = @LastUpdateFullName
		  ,[IsDelete] = @IsDelete
        WHERE Id = @Id
		
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