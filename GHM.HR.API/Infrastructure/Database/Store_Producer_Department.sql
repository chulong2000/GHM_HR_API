USE [GHM_HR_New_Demo]
GO
/****** Object:  StoredProcedure [dbo].[spDepartment_Code]    Script Date: 25/09/2026 11:21:55 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spDepartment_Code]
(
	@TenantId AS VARCHAR(50) ='dae13df6-6720-4bda-a61c-61e5e948017e',
	@CompanyId AS VARCHAR(50) = '54ed7b02-36c3-4f1d-ba7d-a3fff1a23c59',
	@DepartmentIdHIS as VARCHAR(50) = 'BGD'
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id FROM Departments
	WHERE TenantId=@TenantId and CompanyId=@CompanyId and DepartmentId_HIS=@DepartmentIdHIS
END
GO
/****** Object:  StoredProcedure [dbo].[spDepartment_Check_ParentId]    Script Date: 25/09/2026 11:21:55 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spDepartment_Check_ParentId]
(
	@Id AS INT = 18,
	@ParentId AS INT = 22
)
AS
BEGIN
	IF(@ParentId IS NULL) SET @ParentId = 0
	
	DECLARE @ListId AS VARCHAR(2000)
	SELECT @ListId = [dbo].[fnDepartment_GetIdChildren] (@Id)

	SELECT RTRIM(LTRIM(items)) AS Id INTO #A FROM [dbo].[Split](@ListId, ',')

	SELECT IIF (EXISTS (SELECT 1 FROM #A WHERE Id = @ParentId), 1, 0)

END 
GO
/****** Object:  StoredProcedure [dbo].[spDepartment_DeleteByID]    Script Date: 25/09/2026 11:21:55 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spDepartment_DeleteByID]
(
	@Id AS INT,
	@DeleteUserId AS varchar(50) = NULL,
	@DeleteFullName AS nvarchar(150) = NULL
)
AS
BEGIN
	UPDATE [dbo].[Departments]
	SET
		[IsDelete] = 1,
		[DeleteTime] = GETDATE(),
		[DeleteUserId] = @DeleteUserId,
		[DeleteFullName] = @DeleteFullName
	WHERE 
		[Id]=@Id
END 
GO
/****** Object:  StoredProcedure [dbo].[spDepartment_GetIdChildren]    Script Date: 25/09/2026 11:21:55 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[spDepartment_GetIdChildren]
(
	@Id AS INT = 4
)
AS
BEGIN
	
	DECLARE @ListId AS VARCHAR(2000)
	SELECT @ListId = [dbo].[fnDepartment_GetIdChildren] (@Id)

	SELECT RTRIM(LTRIM(items)) AS Id INTO #A FROM [dbo].[Split](@ListId, ',')

SELECT * FROM #A 

END 
GO
/****** Object:  StoredProcedure [dbo].[spDepartment_Insert]    Script Date: 25/09/2026 11:21:55 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spDepartment_Insert]
(
	@Id AS INT OUTPUT,
	@TenantId AS VARCHAR(50) = NULL,
	@CompanyId AS VARCHAR(50) = NULL,
	@ParentId AS INT = NULL,
	@Code AS VARCHAR(50) = NULL,
	@Name AS NVARCHAR(4000) = NULL,
	@Description AS NVARCHAR(MAX) = NULL,
	@IdPath AS VARCHAR(2000) = NULL,
	@NamePath AS NVARCHAR(MAX) = NULL,
	@ChildCount AS INT = NULL,
	@ConcurrencyStamp AS VARCHAR(50) = NULL,
	@IsActive AS BIT = NULL,
	@IsDelete AS BIT = NULL,
	@CreateTime AS DATETIME2 = NULL,
	@CreatorId AS VARCHAR(50) = NULL,
	@CreatorFullName AS NVARCHAR(300) = NULL,
	@LastUpdate AS DATETIME2 = NULL,
	@LastUpdateUserId AS VARCHAR(50) = NULL,
	@LastUpdateFullName AS NVARCHAR(300) = NULL,
	@DeleteTime AS DATETIME2 = NULL,
	@DeleteUserId AS VARCHAR(50) = NULL,
	@DeleteFullName AS NVARCHAR(300) = NULL,
	@AdvanceLeaveGranted AS INT,
	@CompLeaveGranted AS INT,
	@ExpireDays AS INT
)
AS
BEGIN
	INSERT INTO [dbo].[Departments]
	(
		[TenantId],
		[CompanyId],
		[ParentId],
		[Code],
		[Name],
		[Description],
		[IdPath],
		[NamePath],
		[ChildCount],
		[ConcurrencyStamp],
		[IsActive],
		[IsDelete],
		[CreateTime],
		[CreatorId],
		[CreatorFullName],
		[LastUpdate],
		[LastUpdateUserId],
		[LastUpdateFullName],
		[DeleteTime],
		[DeleteUserId],
		[DeleteFullName],
		[AdvanceLeaveGranted],
		[CompLeaveGranted],
		[ExpireDays]
	) 
	VALUES
	(
		@TenantId,
		@CompanyId,
		@ParentId,
		@Code,
		@Name,
		@Description,	
		@IdPath,
		@NamePath,
		@ChildCount,
		@ConcurrencyStamp,
		@IsActive,
		@IsDelete,
		@CreateTime,
		@CreatorId,
		@CreatorFullName,
		@LastUpdate,
		@LastUpdateUserId,
		@LastUpdateFullName,
		@DeleteTime,
		@DeleteUserId,
		@DeleteFullName,
		@AdvanceLeaveGranted,
		@CompLeaveGranted,
		@ExpireDays
	) 
		SELECT @Id=SCOPE_IDENTITY()
END 
GO
/****** Object:  StoredProcedure [dbo].[spDepartment_Search]    Script Date: 25/09/2026 11:21:55 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


  CREATE PROCEDURE [dbo].[spDepartment_Search]
(
	@TenantId AS VARCHAR(50)='dae13df6-6720-4bda-a61c-61e5e948017e',
	@CompanyId AS VARCHAR(50)='thaithinhmedic',
	@Keyword AS NVARCHAR(50),
	@IsActive AS BIT,
	@page AS INT = 1,
	@pageSize AS INT =20
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @SQL NVARCHAR(MAX);
	DECLARE @ParamList NVARCHAR(2000);

	--SELECT @keyword = UPPER([dbo].[StripVietnameseChars](@keyword));

	SELECT @SQL= N'
WITH #Results
AS (SELECT [Id], [CompanyId],[TenantId], [ParentId],[Code], [Name], [Description], [IdPath], [NamePath], [ChildCount], [CreateTime], [IsActive]
FROM [dbo].[Departments] WITH (NOLOCK)
WHERE [TenantId] = '''+ @TenantId + N''' AND [IsDelete] = 0';

IF ISNULL(@keyword, '') <> ''
		SELECT @SQL = @SQL + N'
AND ([Name] LIKE N''%' + @keyword + '%'')';

	IF @IsActive IS NOT NULL
		SELECT @SQL = @SQL + N'
AND ([IsActive] = @IsActive)';

SELECT @SQL= @SQL+ N'
UNION ALL
SELECT [Departments].[Id],[Departments].[CompanyId], [Departments].[TenantId], [Departments].[ParentId],[Departments].[Code], [Departments].[Name], [Departments].[Description],[Departments].[IdPath], [Departments].[NamePath] [Departments].[ChildCount], [Departments].[CreateTime], [Departments].[IsActive]
FROM [dbo].[Departments] WITH (NOLOCK) INNER JOIN #Results  ON #Results.ParentId = Departments.Id
WHERE [Departments].[TenantId] = '''+ @TenantId + N''' AND [Departments].[IsDelete] = 0';

	IF @IsActive IS NOT NULL
		SELECT @SQL = @SQL + N'
AND ([Departments].[IsActive] = @IsActive);';


SELECT @SQL= @SQL+ N'
SELECT DISTINCT * INTO #Departments FROM #Results';

	SELECT @SQL= @SQL+ N'
SELECT ISNULL(COUNT(1),0) AS TotalRows FROM #Departments';

	SELECT @SQL= @SQL+ N'
SELECT [Id],[CompanyId], [TenantId], [ParentId], [Code], [Name], [Description],[IdPath], [NamePath], [ChildCount], [CreateTime], [IsActive]
FROM #Departments
ORDER BY [CreateTime]
OFFSET ' + CAST(@pageSize AS VARCHAR(10)) + N' * (' + CAST(@page AS VARCHAR(10)) + N' - 1) ROWS
FETCH NEXT ' + CAST(@pageSize AS VARCHAR(10)) + N' ROWS ONLY;';

	SELECT @ParamList= N'
@TenantId AS VARCHAR(50),
@CompanyId AS VARCHAR(50),
@Keyword AS NVARCHAR(50),
@IsActive AS BIT,
@page AS INT,
@pageSize AS INT';

	EXECUTE sp_executesql @SQL,@ParamList,@TenantId,@CompanyId,@Keyword,@IsActive,@page,@pageSize;

END; 
GO
/****** Object:  StoredProcedure [dbo].[spDepartment_SelectAll]    Script Date: 25/09/2026 11:21:55 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	CREATE PROCEDURE [dbo].[spDepartment_SelectAll]
(
	@TenantId AS varchar(50) = NULL,
	@CompanyId AS varchar(50) = null
)
AS
BEGIN
	SELECT 
		Id,
		CompanyId,
		ParentId,
		Code,
		Name,
		Description,
		IdPath,
		NamePath,
		ChildCount,
		IsActive,
		CreateTime
	FROM Departments
	WHERE (TenantId = @TenantId Or TenantId is null) AND CompanyId='4eab0dbb-9d1d-424d-a28a-fca1405d4401' AND IsDelete=0
END 
GO
/****** Object:  StoredProcedure [dbo].[spDepartment_SelectAll_Action]    Script Date: 25/09/2026 11:21:55 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spDepartment_SelectAll_Action]
(
	@TenantId AS varchar(50) = null,
	@CompanyId AS varchar(50) = '195ba1ec-d16c-4f23-a118-52dcbfe9e260'
)
AS
BEGIN
	SELECT 
		Id,
		CompanyId,
		ParentId,
		Code,
		Name,
		Description,
		IdPath,
		NamePath,
		ChildCount,
		IsActive,
		CreateTime
	FROM Departments
	WHERE (TenantId = @TenantId Or TenantId is null) and CompanyId=@CompanyId AND IsDelete=0 AND IsActive = 1
END
GO
/****** Object:  StoredProcedure [dbo].[spDepartment_SelectByID]    Script Date: 25/09/2026 11:21:55 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spDepartment_SelectByID]
(
	@Id AS INT=119
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[Id],
		[CompanyId],
		[TenantId],
		[ParentId],
		[Code],
		[Name],
		[Description],
		[IdPath],
		[NamePath],
		[ChildCount],
		[ConcurrencyStamp],
		[IsActive],
		[IsDelete],
		[CreateTime],
		[CreatorId],
		[CreatorFullName],
		[LastUpdate],
		[LastUpdateUserId],
		[LastUpdateFullName],
		[DeleteTime],
		[DeleteUserId],
		[DeleteFullName],
		[AdvanceLeaveGranted],
		[CompLeaveGranted],
		[ExpireDays]
	FROM [dbo].[Departments] WITH (NOLOCK)
	WHERE 
		[Id]=@Id AND
		[IsDelete] = 0
END 
GO
/****** Object:  StoredProcedure [dbo].[spDepartment_SelectByTenantId]    Script Date: 25/09/2026 11:21:55 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spDepartment_SelectByTenantId](
	@TenantId AS VARCHAR(50)='dae13df6-6720-4bda-a61c-61e5e948017e',
	@CompanyId AS VARCHAR(50)='c33b9591-4ab8-443b-8932-55ea3ff2c523',
	@UserId AS VARCHAR(50)='7b82130f-7294-4d98-86de-fe75ea452094'
)
AS
BEGIN
	SELECT DepartmentId
	INTO #Department
	FROM Users
	WHERE [TenantId]=@TenantId AND [CompanyId]=@CompanyId AND [Id]=@UserId AND IsDelete=0 AND IsActive=1
	UNION
	SELECT DepartmentId
	FROM MultipleCompanys
	WHERE [TenantId]=@TenantId AND [CompanyId]=@CompanyId AND [UserId]=@UserId

	IF EXISTS (SELECT 1 FROM #Department WHERE DepartmentId IS NULL)
	BEGIN
		SELECT 
			Id,
			CompanyId,
			ParentId,
			Name,
			Description,
			IdPath,
			NamePath,
			ChildCount,
			IsActive,
			CreateTime
		FROM Departments
		WHERE TenantId=@TenantId AND CompanyId=@CompanyId AND IsDelete=0 AND IsActive=1
	END
	ELSE 
	BEGIN
		SELECT 
			Id,
			CompanyId,
			ParentId,
			Name,
			Description,
			IdPath,
			NamePath,
			ChildCount,
			IsActive,
			CreateTime
		FROM Departments
		WHERE EXISTS (
			SELECT 1
			FROM #Department
			WHERE Departments.Id=#Department.DepartmentId OR
				  IdPath LIKE CAST(#Department.DepartmentId AS VARCHAR(200))+'.%'
			)
	END
END
GO
/****** Object:  StoredProcedure [dbo].[spDepartment_Update]    Script Date: 25/09/2026 11:21:55 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Create by : Nguyen Dac Quang
--Create date : 20/01/2025 14:39:55
--Description :
--Output :
--Modify :
--Project :Quản lý Nhân Sự
-----------------------o0o-----------------------

CREATE PROCEDURE [dbo].[spDepartment_Update]
(
	@Id AS INT,
	@TenantId AS VARCHAR(50) = NULL,
	@CompanyId AS VARCHAR(50) = NULL,
	@ParentId AS VARCHAR(50) = NULL,
	@Code AS VARCHAR(50) = NULL,
	@Name AS NVARCHAR(200) = NULL,
	@Description AS NVARCHAR(2000) = NULL,
	@IdPath AS VARCHAR(1000) = NULL,
	@NamePath AS NVARCHAR(4000) = NULL,
	@ChildCount AS INT = NULL,
	@ConcurrencyStamp AS VARCHAR(50) = NULL,
	@IsActive AS BIT = NULL,
	@IsDelete AS BIT = NULL,
	@CreateTime AS DATETIME2 = NULL,
	@CreatorId AS VARCHAR(50) = NULL,
	@CreatorFullName AS NVARCHAR(200) = NULL,
	@LastUpdate AS DATETIME2 = NULL,
	@LastUpdateUserId AS VARCHAR(50) = NULL,
	@LastUpdateFullName AS NVARCHAR(200) = NULL,
	@DeleteTime AS DATETIME2 = NULL,
	@DeleteUserId AS VARCHAR(50) = NULL,
	@DeleteFullName AS NVARCHAR(200) = NULL,
	@AdvanceLeaveGranted AS INT,
	@CompLeaveGranted AS INT,
	@ExpireDays AS INT  = NULL
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

			UPDATE [dbo].[Departments]
			SET
				[TenantId] = @TenantId,
				[CompanyId] = @CompanyId,
				[ParentId] = @ParentId,
				[Code] = @Code,
				[Name] = @Name,
				[Description] = @Description,
				[IdPath] = @IdPath,
				[NamePath] = @NamePath,
				[ChildCount] =@ChildCount,
				[ConcurrencyStamp] = @ConcurrencyStamp,
				[IsActive] = @IsActive,
				[IsDelete] = @IsDelete,
				[CreateTime] = @CreateTime,
				[CreatorId] = @CreatorId, 
				[CreatorFullName] = @CreatorFullName,
				[LastUpdate] = @LastUpdate,
				[LastUpdateUserId] =@LastUpdateUserId,
				[LastUpdateFullName] = @LastUpdateFullName,
				[DeleteTime] = @DeleteTime, 
				[DeleteUserId] = @DeleteUserId, 
				[DeleteFullName] = @DeleteFullName,
				[AdvanceLeaveGranted] = @AdvanceLeaveGranted,
				[CompLeaveGranted] = @CompLeaveGranted,
				[ExpireDays] = @ExpireDays
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
/****** Object:  StoredProcedure [dbo].[spDepartment_Update_ChildCount]    Script Date: 25/09/2026 11:21:55 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spDepartment_Update_ChildCount]
(
	@Id AS INT = 4
)
AS
BEGIN
	DECLARE @ChildCount INT
	SELECT @ChildCount = [dbo].[fnDepartment_GetChildCount]  (@Id)
	
	UPDATE [dbo].[Departments]
	SET
		[ChildCount] = @ChildCount
	WHERE 
		[Id] = @Id
END 
GO
/****** Object:  StoredProcedure [dbo].[spDepartment_Update_IdPath_NamePath_DepartmentPath]    Script Date: 25/09/2026 11:21:55 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spDepartment_Update_IdPath_NamePath_DepartmentPath]
(
	@Id AS INT = 6
)
AS
BEGIN
	DECLARE @IdPath VARCHAR(2000)
	DECLARE @NamePath NVARCHAR(MAX)

	SELECT @IdPath = [dbo].[fnDepartment_GetIdPath] (@Id)
	SELECT @NamePath = [dbo].[fnDepartment_GetNamePath] (@Id)
	
	SET @IdPath = ISNULL(@IdPath,'')
	SET @NamePath = ISNULL(@NamePath,'')

	UPDATE [dbo].[Departments]
	SET
		[IdPath] = @IdPath,
		[NamePath] = @NamePath
	WHERE 
		[Id] = @Id
END 
GO
/****** Object:  StoredProcedure [dbo].[spDepartment_Update_IsActive]    Script Date: 25/09/2026 11:21:55 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[spDepartment_Update_IsActive]
(
	@Id AS INT = 10,
	@IsActive AS BIT = 1
)
AS
BEGIN
	DECLARE @ListId AS VARCHAR(2000)
	SELECT @ListId = [dbo].[fnDepartment_GetIdChildren] (@Id)
	DECLARE @SQL AS NVARCHAR(max)
	SET @SQL = '' 
	IF(ISNULL(@ListId,'') <> '')
	BEGIN
		SET @SQL = 'UPDATE [dbo].[Departments] SET [IsActive] = ' + CAST(@IsActive AS VARCHAR) + ' WHERE [Id] IN (' + @ListId + ')'
		EXECUTE  SP_EXECUTESQL @SQL
	END

END 
GO
