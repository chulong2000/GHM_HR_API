ALTER   PROCEDURE [dbo].[spWorkSchedule_DeleteByID]
(
	@Id AS varchar(50),
	@TenantId AS varchar(50) = NULL,
	@CompanyId AS nvarchar(150) = NULL
)
AS
BEGIN
	DELETE FROM dbo.WorkSchedule
           WHERE Id = @Id
           AND TenantId = @TenantId    
           AND CompanyId = @CompanyId;
END


ALTER   PROCEDURE [dbo].[spWorkSchedule_Insert]
(
    @Id AS uniqueidentifier = Null,
    @WorkDay AS DateTime = NULL,
	@ShiftId AS uniqueidentifier  = NULL,
    @UserId AS uniqueidentifier  = NULL,
    @CreateTime AS DateTime = NULL,
    @CreateId As uniqueidentifier  = NULL,
    @CreateFullName AS VARChar(50) = NULL,
    @CompanyId AS uniqueidentifier = NULL, 
	@TenantId as uniqueidentifier = NULL,
	@Year as int = NULL
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

		INSERT INTO [dbo].[WorkSchedule]
           ([Id]
           ,[WorkDay]
           ,[ShiftId]
           ,[UserId]
           ,[CreateTime]
           ,[CreateId]
           ,[CreateFullName]
           ,[CompanyId]
           ,[TenantId]
           ,[Year])
     VALUES
           (@Id
           ,@WorkDay
           ,@ShiftId
           ,@UserId
           ,@CreateTime
           ,@CreateId
           ,@CreateFullName
           ,@CompanyId
           ,@TenantId
           ,@Year)
        
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

ALTER   PROCEDURE [dbo].[spWorkSchedule_SelectWorkDayByMonth]
(
	@TenantId as uniqueidentifier = 'dae13df6-6720-4bda-a61c-61e5e948017e',
	@companyId as uniqueidentifier = '4eab0dbb-9d1d-424d-a28a-fca1405d4401',
	@userId as uniqueidentifier = '6af2ce44-be8d-4f2e-b427-5de92cda59bd',
	@year as int = '2026',
	@month as int = '1'
)
AS
BEGIN
	SET NOCOUNT ON;
	select w.WorkDay, s.Name as ShiftName, s.StartTime, s.EndTime, Month(w.WorkDay) as month, w.Year from dbo.WorkSchedule as w
	inner join dbo.Shift as s
	on w.ShiftId = s.Id
	where w.TenantId = @TenantId 
	and w.CompanyId = @companyId
	and w.UserId = @userId
	and w.Year = @year
	and Month(w.WorkDay) = @month
END 

