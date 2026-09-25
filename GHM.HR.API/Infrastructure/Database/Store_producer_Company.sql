USE [GHM_HR_New_Demo]
GO
/****** Object:  StoredProcedure [dbo].[spCompany_Code]    Script Date: 25/09/2026 11:20:50 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCompany_Code]
(
	@CompanyId AS VARCHAR(50) = 'thaithinhmedic'
)
AS
BEGIN
	SELECT Code AS CompanyCode FROM Companys
	WHERE Id = @CompanyId
END
GO
/****** Object:  StoredProcedure [dbo].[spCompany_CheckCompanyExist]    Script Date: 25/09/2026 11:20:50 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCompany_CheckCompanyExist]
    @CompanyId NVARCHAR(50)=null,
    @Result BIT OUTPUT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM [dbo].[Users] WHERE CompanyId = @CompanyId)
    BEGIN
        SET @Result = 1
    END
    ELSE
    BEGIN
        SET @Result = 0
    END
END
GO
/****** Object:  StoredProcedure [dbo].[spCompany_CheckExistUser]    Script Date: 25/09/2026 11:20:50 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[spCompany_CheckExistUser](
	@TenantId AS VARCHAR(50) = 'dae13df6-6720-4bda-a61c-61e5e948017e',
	@CompanyId AS VARCHAR(50) = 'thaithinhmedic'
)
AS
BEGIN
	SELECT IIF (EXISTS (SELECT 1 FROM [dbo].[Users]
						WHERE TenantId = @TenantId AND
						CompanyId = @CompanyId), 1, 0)
END
GO
/****** Object:  StoredProcedure [dbo].[spCompany_DeleteByID]    Script Date: 25/09/2026 11:20:50 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Create by : Trần Văn Mạnh
--Create date : 16/01/2025 14:57:38
--Description :
--Output :
--Modify :
--Project : Quản Lý Spa
-----------------------o0o-----------------------

CREATE PROCEDURE [dbo].[spCompany_DeleteByID]
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

			UPDATE [dbo].[Companys]
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
/****** Object:  StoredProcedure [dbo].[spCompany_ForceDeleteByID]    Script Date: 25/09/2026 11:20:50 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Create by : Trần Văn Mạnh
--Create date : 16/01/2025 14:57:47
--Description :
--Output :
--Modify :
--Project : Quản Lý Spa
-----------------------o0o-----------------------

CREATE PROCEDURE [dbo].[spCompany_ForceDeleteByID]
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

			DELETE FROM [dbo].[Companys]
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
/****** Object:  StoredProcedure [dbo].[spCompany_GetConnectionString]    Script Date: 25/09/2026 11:20:50 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Create by : Trần Văn Mạnh
--Create date : 16/01/2025 14:58:01
--Description :
--Output :
--Modify :
--Project : Quản Lý Spa
-----------------------o0o-----------------------

CREATE PROCEDURE [dbo].[spCompany_GetConnectionString]
(
	@CompanyId AS VARCHAR(50) = 'neomedic'
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		ConnectionString
	FROM [dbo].[Companys] WITH (NOLOCK)
	WHERE 
		[Id]=@CompanyId AND 
		[IsDelete] = 0
END 
GO
/****** Object:  StoredProcedure [dbo].[spCompany_GetLogo]    Script Date: 25/09/2026 11:20:50 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCompany_GetLogo](
	@TenantId AS VARCHAR(50)=null,
	@CompanyId AS VARCHAR(50)=null
)
AS
BEGIN
	SELECT [Logo],[LogoFooter]
	FROM [dbo].[Companys] WITH (NOLOCK)
	WHERE [TenantId]=@TenantId AND [Id]=@CompanyId
END
GO
/****** Object:  StoredProcedure [dbo].[spCompany_GetThongTin]    Script Date: 25/09/2026 11:20:50 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spCompany_GetThongTin]
(
    @CompanyId VARCHAR(50)
)
AS
BEGIN
    SET NOCOUNT ON;

    SET @CompanyId = NULLIF(LTRIM(RTRIM(@CompanyId)), '');

    IF @CompanyId IS NULL
        RETURN;

    /* ⚠️ KHÔNG có `@TenantId`, ngược quy tắc "lọc TenantId trước" của repo — ngoại lệ CÓ LÝ DO:
         · `Id` của công ty là khoá **toàn cục** (PK trên `Id`), nên thêm `TenantId` không làm hẹp
           thêm kết quả;
         · hai SP sẵn có cũng chỉ nhận `@CompanyId` (`HRRepository.cs:29-52`) — cùng họ tra cứu,
           và SP này phải thay thế được chúng nên không thể đòi thêm tham số.
       Cách ly tenant nằm ở tầng trên: SP nghiệp vụ bên `KhamBenhAI` lọc `@TenantId` bắt buộc
       (ví dụ `spLichSuKham_SelectDanhSach`), nên `CompanyId` đi tới đây đã qua cửa tenant.

       ⚠️ Vì vậy TUYỆT ĐỐI không dùng SP này để **liệt kê** công ty cho người dùng chọn — chỗ đó
       phải lọc `TenantId`. Nó chỉ dịch một id đã biết thành thông tin của id đó. */
    SELECT TOP (1)
           CompanyId          = [Id],
           MaCoSo             = [Code],
           TenCoSo            = [Name],
           /* Ba cột dưới đây là thứ hai SP cũ đang trả — giữ ĐÚNG TÊN để `CompanyModel` map được
              không cần đổi gì ở C#. */
           [ConnectionString],
           [UrlUpload],
           [UrlViewKQ]
    FROM [dbo].[Companys] WITH (NOLOCK)
    WHERE [Id] = @CompanyId;
    /* KHÔNG lọc `IsDelete` / `IsActive`, hai lý do:
         · Lịch sử khám hiện lượt khám của **quá khứ** — cơ sở đóng cửa năm ngoái vẫn phải ra tên
           cho badge, không được để trống.
         · Và quan trọng hơn: hai SP cũ có lọc hay không thì **tôi không biết**. Không lọc là bản
           rộng hơn ⇒ PHẦN ĐỐI CHIẾU sẽ phát hiện nếu SP cũ hẹp hơn (một cơ sở đã xoá mềm mà SP cũ
           trả rỗng còn SP mới trả có). Thêm lọc mà đoán sai thì ngược lại: mất connection string
           của một phòng khám đang chạy. */
END
GO
/****** Object:  StoredProcedure [dbo].[spCompany_GetUrl]    Script Date: 25/09/2026 11:20:50 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Create by : Trần Văn Mạnh
--Create date : 16/01/2025 14:58:01
--Description :
--Output :
--Modify :
--Project : Quản Lý Spa
-----------------------o0o-----------------------

CREATE PROCEDURE [dbo].[spCompany_GetUrl]
(
	@CompanyId AS VARCHAR(50) = 'neomedic'
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		UrlUpload,
		ConnectionString,
		UrlViewKQ
	FROM [dbo].[Companys] WITH (NOLOCK)
	WHERE 
		[Id]=@CompanyId AND 
		[IsDelete] = 0
END 
GO
/****** Object:  StoredProcedure [dbo].[spCompany_GetUrlUpload]    Script Date: 25/09/2026 11:20:50 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Create by : Trần Văn Mạnh
--Create date : 16/01/2025 14:58:01
--Description :
--Output :
--Modify :
--Project : Quản Lý Spa
-----------------------o0o-----------------------

CREATE PROCEDURE [dbo].[spCompany_GetUrlUpload]
(
	@CompanyId AS VARCHAR(50) = 'neomedic'
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		UrlUpload
	FROM [dbo].[Companys] WITH (NOLOCK)
	WHERE 
		[Id]=@CompanyId AND 
		[IsDelete] = 0
END 
GO
/****** Object:  StoredProcedure [dbo].[spCompany_Insert]    Script Date: 25/09/2026 11:20:50 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ---------------------------------------------------------------- spCompany_Insert (+ @AppIds)
CREATE   PROCEDURE [dbo].[spCompany_Insert]
(
    @Id AS VARCHAR(50),
    @TenantId AS VARCHAR(50) = NULL,
    @Code AS VARCHAR(50) = NULL,
    @Name AS NVARCHAR(4000) = NULL,
    @PhoneNumber AS NVARCHAR(100) = NULL,
    @Address AS NVARCHAR(1000) = NULL,
    @Description AS NVARCHAR(1000) = NULL,
    @TaxCode AS VARCHAR(50) = NULL,
    @IsActive AS BIT = NULL,
    @ConcurrencyStamp AS VARCHAR(50) = NULL,
    @CreateTime AS DATETIME2 = NULL,
    @CreatorId AS VARCHAR(50) = NULL,
    @CreatorFullName AS NVARCHAR(300) = NULL,
    @LastUpdate AS DATETIME2 = NULL,
    @LastUpdateUserId AS VARCHAR(50) = NULL,
    @LastUpdateFullName AS NVARCHAR(300) = NULL,
    @IsDelete AS BIT = NULL,
    @DeleteTime AS DATETIME2 = NULL,
    @DeleteUserId AS VARCHAR(50) = NULL,
    @DeleteFullName AS NVARCHAR(300) = NULL,
    @Logo AS VARCHAR(Max) = null,
    @LogoFooter AS VARCHAR(MAX) = null,
    @AppIds AS NVARCHAR(MAX) = NULL          -- JSON array of App.Id the company may access
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

            INSERT INTO [dbo].[Companys]
            (
                [Id],
                [TenantId],
                [Code],
                [Name],
                [PhoneNumber],
                [Address],
                [Description],
                [TaxCode],
                [IsActive],
                [ConcurrencyStamp],
                [CreateTime],
                [CreatorId],
                [CreatorFullName],
                [LastUpdate],
                [LastUpdateUserId],
                [LastUpdateFullName],
                [IsDelete],
                [DeleteTime],
                [DeleteUserId],
                [DeleteFullName],
                [Logo],
                [LogoFooter],
                [AppIds]
            )
            VALUES
            (
                @Id,
                @TenantId,
                @Code,
                @Name,
                @PhoneNumber,
                @Address,
                @Description,
                @TaxCode,
                @IsActive,
                @ConcurrencyStamp,
                @CreateTime,
                @CreatorId,
                @CreatorFullName,
                @LastUpdate,
                @LastUpdateUserId,
                @LastUpdateFullName,
                @IsDelete,
                @DeleteTime,
                @DeleteUserId,
                @DeleteFullName,
                @Logo,
                @LogoFooter,
                @AppIds
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
/****** Object:  StoredProcedure [dbo].[spCompany_SelectAll]    Script Date: 25/09/2026 11:20:50 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCompany_SelectAll]
(
	@TenantId AS VARCHAR(50) = ''
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		[Id],
	    [Code],
		[Name],
		[PhoneNumber],
		[Address],
		[Description],
		[TaxCode],
		[IsActive],
		[CreateTime]
	FROM [dbo].[Companys] WITH (NOLOCK)
	WHERE
		[TenantId] = @TenantId AND IsDelete = 0
	ORDER BY [CreateTime]
END 
GO
/****** Object:  StoredProcedure [dbo].[spCompany_SelectAllByUser]    Script Date: 25/09/2026 11:20:50 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCompany_SelectAllByUser]
(
	@TenantId AS VARCHAR(50) = 'dae13df6-6720-4bda-a61c-61e5e948017e',
	@UserId AS VARCHAR(50) = 'a7302944-0917-4cc3-bcb1-9832a83bfd91'
)
AS
BEGIN
	IF(EXISTS (SELECT 1 FROM dbo.ResourceRoles WHERE RoleId = 'TenantAdmin' AND UserId = @UserId))
	BEGIN
		SELECT
			[Id],
			[Name]
		FROM [dbo].[Companys] WITH (NOLOCK)
		WHERE
			[TenantId] = @TenantId AND [IsDelete] = 0 AND [IsActive] = 1
	END
	ELSE
	BEGIN
		SELECT CompanyId INTO #User FROM [dbo].[Users]
		WHERE TenantId=@TenantId and Id=@UserId
		UNION ALL
		SELECT CompanyId FROM [dbo].[MultipleCompanys]
		WHERE TenantId=@TenantId and UserId=@UserId

		SET NOCOUNT ON;
		SELECT DISTINCT
			#User.CompanyId as [Id],
			[Name]
		FROM [dbo].[Companys] WITH (NOLOCK)
		INNER JOIN #User ON Companys.Id=#User.CompanyId
		WHERE
			[TenantId] = @TenantId AND [IsDelete] = 0 AND [IsActive]=1
	END
END 
GO
/****** Object:  StoredProcedure [dbo].[spCompany_SelectByID]    Script Date: 25/09/2026 11:20:50 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCompany_SelectByID]
(
	@Id AS VARCHAR(50) = 'neomedic'
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[Id],
		[TenantId],
		[Code],
		[Name],
		[PhoneNumber],
		[Address],
		[Description],
		[TaxCode],
		[IsActive],
		[ConcurrencyStamp],
		[CreateTime],
		[CreatorId],
		[CreatorFullName],
		[LastUpdate],
		[LastUpdateUserId],
		[LastUpdateFullName],
		[IsDelete],
		[DeleteTime],
		[DeleteUserId],
		[DeleteFullName],
		[Logo],
		[LogoFooter],
        [AppIds]
	FROM [dbo].[Companys] WITH (NOLOCK)
	WHERE 
		[Id]=@Id AND 
		[IsDelete] = 0
END
GO
/****** Object:  StoredProcedure [dbo].[spCompany_Update]    Script Date: 25/09/2026 11:20:50 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ---------------------------------------------------------------- spCompany_Update (+ @AppIds)
CREATE   PROCEDURE [dbo].[spCompany_Update]
(
    @Id AS VARCHAR(50),
    @TenantId AS VARCHAR(50) = NULL,
    @Code AS VARCHAR(50) = NULL,
    @Name AS NVARCHAR(4000) = NULL,
    @PhoneNumber AS NVARCHAR(100) = NULL,
    @Address AS NVARCHAR(1000) = NULL,
    @Description AS NVARCHAR(1000) = NULL,
    @TaxCode AS VARCHAR(50) = NULL,
    @IsActive AS BIT = NULL,
    @ConcurrencyStamp AS VARCHAR(50) = NULL,
    @CreateTime AS DATETIME2 = NULL,
    @CreatorId AS VARCHAR(50) = NULL,
    @CreatorFullName AS NVARCHAR(300) = NULL,
    @LastUpdate AS DATETIME2 = NULL,
    @LastUpdateUserId AS VARCHAR(50) = NULL,
    @LastUpdateFullName AS NVARCHAR(300) = NULL,
    @IsDelete AS BIT = NULL,
    @DeleteTime AS DATETIME2 = NULL,
    @DeleteUserId AS VARCHAR(50) = NULL,
    @DeleteFullName AS NVARCHAR(300) = NULL,
    @Logo AS VARCHAR(max) = null,
    @LogoFooter AS VARCHAR(max) = null,
    @AppIds AS NVARCHAR(MAX) = NULL          -- NULL keeps existing; '[]' clears; a JSON array replaces
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

            UPDATE [dbo].[Companys]
            SET
                [TenantId] = COALESCE(@TenantId, TenantId),
                [Code] = COALESCE(@Code, Code),
                [Name] = COALESCE(@Name, Name),
                [PhoneNumber] = COALESCE(@PhoneNumber, PhoneNumber),
                [Address] = COALESCE(@Address, Address),
                [Description] = COALESCE(@Description, Description),
                [TaxCode] = COALESCE(@TaxCode, TaxCode),
                [IsActive] = COALESCE(@IsActive, IsActive),
                [ConcurrencyStamp] = COALESCE(@ConcurrencyStamp, ConcurrencyStamp),
                [CreateTime] = COALESCE(@CreateTime, CreateTime),
                [CreatorId] = COALESCE(@CreatorId, CreatorId),
                [CreatorFullName] = COALESCE(@CreatorFullName, CreatorFullName),
                [LastUpdate] = COALESCE(@LastUpdate, LastUpdate),
                [LastUpdateUserId] = COALESCE(@LastUpdateUserId, LastUpdateUserId),
                [LastUpdateFullName] = COALESCE(@LastUpdateFullName, LastUpdateFullName),
                [IsDelete] = COALESCE(@IsDelete, IsDelete),
                [DeleteTime] = COALESCE(@DeleteTime, DeleteTime),
                [DeleteUserId] = COALESCE(@DeleteUserId, DeleteUserId),
                [DeleteFullName] = COALESCE(@DeleteFullName, DeleteFullName),
                [Logo] = COALESCE(@Logo, Logo),
                [LogoFooter] = COALESCE(@LogoFooter, LogoFooter),
                [AppIds] = COALESCE(@AppIds, AppIds)
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
/****** Object:  StoredProcedure [dbo].[spCompanys_SelectByIDTenantId]    Script Date: 25/09/2026 11:20:50 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCompanys_SelectByIDTenantId]
(
	@Id AS VARCHAR(50),
	@TenantId AS VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[Id],
		[TenantId],
		[Code],
		[Name],
		[PhoneNumber],
		[Address],
		[Description],
		[TaxCode],
		[IsActive],
		[ConcurrencyStamp],
		[CreateTime],
		[CreatorId],
		[CreatorFullName],
		[LastUpdate],
		[LastUpdateUserId],
		[LastUpdateFullName],
		[IsDelete],
		[DeleteTime],
		[DeleteUserId],
		[DeleteFullName],
        [AppIds]
	FROM [dbo].[Companys] WITH (NOLOCK)
	WHERE 
		[Id]=@Id AND 
		[TenantId] = @TenantId AND
		[IsDelete] = 0
END
GO
