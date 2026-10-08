
USE [GHM_HR_New_Demo]


CREATE TYPE [dbo].[MultipleCompanyType] AS TABLE(
	[CompanyId] [varchar](50) NULL,
	[DepartmentId] [varchar](50) NULL,
	[PositionId] [varchar](50) NULL,
	[DoctorCode] [varchar](50) NULL
)
GO

GO
/****** Object:  Table [dbo].[MultipleCompanys]    Script Date: 08/10/2026 9:50:24 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MultipleCompanys](
	[Id] [varchar](50) NOT NULL,
	[TenantId] [varchar](50) NULL,
	[UserId] [varchar](50) NULL,
	[CompanyId] [varchar](50) NULL,
	[DepartmentId] [int] NULL,
	[DepartmentName] [nvarchar](2000) NULL,
	[CreateTime] [datetime2](7) NULL,
	[CreatorId] [varchar](50) NULL,
	[CreatorFullName] [nvarchar](150) NULL,
	[PositionId] [varchar](50) NULL,
	[PositionName] [nvarchar](50) NULL,
	[DoctorCode] [varchar](50) NULL,
 CONSTRAINT [PK_MultipleCompanys] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[spMultiCompany_ForceDeleteByUserId]    Script Date: 08/10/2026 9:50:24 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create   PROCEDURE [dbo].[spMultiCompany_ForceDeleteByUserId]
(
	@UserId AS varchar(50) = 'da87490a-5e89-4b79-bc47-54a51d8d5b7e',
	@TenantId AS nvarchar(150) = 'dae13df6-6720-4bda-a61c-61e5e948017e'
)
AS
BEGIN
	Delete dbo.MultipleCompanys 
	where UserId = @UserId 
	and TenantId = @TenantId
END 

GO
/****** Object:  StoredProcedure [dbo].[spMultiCompany_GetAllCompanyOfUser]    Script Date: 08/10/2026 9:50:24 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create   PROCEDURE [dbo].[spMultiCompany_GetAllCompanyOfUser]
(
	@UserId As varchar(50) = '5a423f2e-8ce0-4c51-8227-45daf8882913',
	@TenantId As varchar(50) = 'dae13df6-6720-4bda-a61c-61e5e948017e'
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		m.Id,
		m.UserId,
		m.CompanyId,
		c.Name,
		m.DepartmentId,
		m.DepartmentName,
		m.PositionId,
		m.PositionName,
		m.DoctorCode,
		m.CreateTime
	FROM [dbo].[MultipleCompanys] as m WITH (NOLOCK)
	inner join dbo.Companys as c with (NOLOCK)
 	on m.CompanyId = c.Id
	WHERE 
		[UserId] = @UserId and m.TenantId = @TenantId
END 
GO
/****** Object:  StoredProcedure [dbo].[spMultipleCompanys_InsertByTableType]    Script Date: 08/10/2026 9:50:24 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spMultipleCompanys_InsertByTableType](
	@TenantId AS VARCHAR(50),
	@UserId AS VARCHAR(50),
	@CreatorId AS VARCHAR(50),
	@CreatorFullName AS NVARCHAR(100),
	@List MultipleCompanyType READONLY
)
AS
BEGIN

	SELECT mc.Id,mc.CompanyId,mc.UserId,mc.DoctorCode
	INTO #delete
	FROM MultipleCompanys AS mc
	WHERE NOT EXISTS (
		SELECT 1 FROM @List as l
		WHERE mc.CompanyId=l.CompanyId AND
		ISNULL(l.DepartmentId, -1) = ISNULL(mc.DepartmentId, -1) AND
		mc.PositionId=l.PositionId AND
		ISNULL(l.DoctorCode, -1)=ISNULL(mc.DoctorCode, -1)) AND
	  mc.TenantId=@TenantId AND mc.UserId=@UserId

	Insert into dbo.TempDelete 
	select * from #delete

	SELECT l.CompanyId,l.DepartmentId,l.PositionId,l.DoctorCode
	INTO #insert
	FROM @List AS l
	WHERE NOT EXISTS (SELECT 1 FROM MultipleCompanys AS mc
					  WHERE mc.CompanyId=l.CompanyId AND
						    ISNULL(l.DepartmentId, -1) = ISNULL(mc.DepartmentId, -1) AND
							mc.PositionId=l.PositionId AND
							ISNULL(l.DoctorCode, -1)=ISNULL(mc.DoctorCode, -1) AND 
							mc.TenantId = @TenantId AND mc.UserId = @UserId)

	DELETE FROM MultipleCompanys
	WHERE Id in (SELECT Id FROM #delete)

	DECLARE @CompanyId VARCHAR(50)
	DECLARE company_cursor CURSOR FOR
	SELECT DISTINCT CompanyId FROM #delete

	OPEN company_cursor
	FETCH NEXT FROM company_cursor INTO @CompanyId

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM MultipleCompanys WHERE UserId=@UserId and CompanyId=@CompanyId)
		BEGIN 
			UPDATE Users
			SET ManagerUserId=null,ManagerFullName=null
			WHERE ManagerUserId=@UserId AND CompanyId=@CompanyId
		END
		FETCH NEXT FROM company_cursor INTO @CompanyId;
    END
    CLOSE company_cursor;
    DEALLOCATE company_cursor;

	INSERT INTO MultipleCompanys(Id,TenantId,CompanyId,UserId,DepartmentId,DepartmentName,PositionId,PositionName,CreateTime,CreatorId,CreatorFullName,DoctorCode)
	SELECT LOWER(NEWID()),@TenantId,i.CompanyId,@UserId,DepartmentId,d.Name AS DepartmentName,PositionId,p.Name AS PositionName,GETDATE(),@CreatorId,@CreatorFullName,i.DoctorCode
	FROM #insert AS i
	LEFT JOIN Departments AS d ON i.DepartmentId=d.Id
	INNER JOIN Positions AS p ON i.PositionId=p.Id
	
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nhaan vien kiêm nhiệm' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MultipleCompanys'
GO
