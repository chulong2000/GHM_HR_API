

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
	select w.WorkDay, s.Name as ShiftName, h.Name as HolidayName, s.StartTime, s.EndTime, Month(w.WorkDay) as month, w.Year from dbo.WorkSchedule as w
	inner join dbo.Shift as s
	on w.ShiftId = s.Id
	left join dbo.Holidays as h
	on w.HolidayId = h.Id
	where w.TenantId = @TenantId 
	and w.CompanyId = @companyId
	and w.UserId = @userId
	and w.Year = @year
	and Month(w.WorkDay) = @month
END 

ALTER PROCEDURE [dbo].[spWorkSchedule_UpdatewhenHolidayCreated]
(
	@FromDay AS Date,
	@ToDay AS Date,
    @HolidayId AS UniqueIdentifier,
	@TenantId AS UniqueIdentifier,
	@CompanyId AS UniqueIdentifier
)
AS
BEGIN
		 UPDATE dbo.WorkSchedule
         SET    HolidayId = @HolidayId
         WHERE  TenantId  = @TenantId
         AND  CompanyId = @CompanyId
         AND  WorkDay between @FromDay and @ToDay;	

		 SELECT @@ROWCOUNT;
END


ALTER   PROCEDURE [dbo].[spWorkSchedule_UpdatewhenHolidayUpdated]
(
	@FromDay AS Date,
	@ToDay AS Date,
    @HolidayId AS UniqueIdentifier,
	@TenantId AS UniqueIdentifier,
	@CompanyId AS UniqueIdentifier
)
AS
BEGIN

	
		 UPDATE dbo.WorkSchedule
           SET    HolidayId = Null
           WHERE  TenantId  = @TenantId
           AND  CompanyId = @CompanyId
           AND  HolidayId = @HolidayId
           AND  (WorkDay < @FromDay OR WorkDay >= DATEADD(DAY, 1, @ToDay));

		 SELECT @@ROWCOUNT;

END



