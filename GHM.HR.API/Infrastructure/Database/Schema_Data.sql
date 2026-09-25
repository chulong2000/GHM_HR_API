USE [GHM_HR_New_Demo]
GO
/****** Object:  Table [dbo].[Companys]    Script Date: 25/09/2026 11:03:39 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companys](
	[Id] [varchar](50) NOT NULL,
	[TenantId] [varchar](50) NOT NULL,
	[Code] [varchar](50) NULL,
	[Name] [nvarchar](2000) NOT NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[Address] [nvarchar](500) NULL,
	[Description] [nvarchar](500) NULL,
	[TaxCode] [varchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[ConcurrencyStamp] [varchar](50) NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[CreatorId] [varchar](50) NULL,
	[CreatorFullName] [nvarchar](150) NULL,
	[LastUpdate] [datetime2](7) NULL,
	[LastUpdateUserId] [varchar](50) NULL,
	[LastUpdateFullName] [nvarchar](150) NULL,
	[IsDelete] [bit] NULL,
	[DeleteTime] [datetime2](7) NULL,
	[DeleteUserId] [varchar](50) NULL,
	[DeleteFullName] [nvarchar](150) NULL,
	[CompanyId_HIS] [int] NULL,
	[ConnectionString] [nvarchar](max) NULL,
	[Logo] [varchar](max) NULL,
	[LogoFooter] [varchar](max) NULL,
	[UrlUpload] [nvarchar](1500) NULL,
	[UrlViewKQ] [nvarchar](1500) NULL,
	[AppIds] [nvarchar](max) NULL,
 CONSTRAINT [PK_Companys] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Departments]    Script Date: 25/09/2026 11:03:39 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [varchar](50) NULL,
	[CompanyId] [varchar](50) NULL,
	[ParentId] [varchar](50) NULL,
	[Code] [varchar](50) NULL,
	[Name] [nvarchar](100) NULL,
	[Description] [nvarchar](1000) NULL,
	[IdPath] [varchar](1000) NULL,
	[NamePath] [nvarchar](2000) NULL,
	[ChildCount] [int] NULL,
	[ConcurrencyStamp] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[IsDelete] [bit] NULL,
	[CreateTime] [datetime2](7) NULL,
	[CreatorId] [varchar](50) NULL,
	[CreatorFullName] [nvarchar](100) NULL,
	[LastUpdate] [datetime2](7) NULL,
	[LastUpdateUserId] [varchar](50) NULL,
	[LastUpdateFullName] [nvarchar](100) NULL,
	[DeleteTime] [datetime2](7) NULL,
	[DeleteUserId] [varchar](50) NULL,
	[DeleteFullName] [nvarchar](100) NULL,
	[DepartmentId_HIS] [varchar](100) NULL,
	[AdvanceLeaveGranted] [int] NULL,
	[CompLeaveGranted] [int] NULL,
	[ExpireDays] [int] NULL,
 CONSTRAINT [PK__Departme__3214EC07952D7215] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Positions]    Script Date: 25/09/2026 11:03:39 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Positions](
	[Id] [varchar](50) NOT NULL,
	[TenantId] [varchar](50) NULL,
	[CompanyId] [varchar](50) NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](100) NULL,
	[Description] [nvarchar](3000) NULL,
	[IsMultiple] [bit] NULL,
	[IsActive] [bit] NULL,
	[IsDelete] [bit] NULL,
	[ConcurrencyStamp] [varchar](50) NULL,
	[CreateTime] [datetime2](7) NULL,
	[CreatorId] [varchar](50) NULL,
	[CreatorFullName] [nvarchar](150) NULL,
	[LastUpdate] [datetime2](7) NULL,
	[LastUpdatedUserId] [varchar](50) NULL,
	[LastUpdateFullName] [nvarchar](150) NULL,
	[DeleteTime] [datetime2](7) NULL,
	[DeleteUserId] [varchar](50) NULL,
	[DeleteFullName] [nvarchar](150) NULL,
	[PositionId_HIS] [varchar](100) NULL,
 CONSTRAINT [PK_Positions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 25/09/2026 11:03:39 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [varchar](50) NOT NULL,
	[Code] [varchar](50) NULL,
	[TenantId] [varchar](50) NULL,
	[CompanyId] [varchar](50) NULL,
	[FullName] [nvarchar](150) NULL,
	[FirstName] [nvarchar](500) NULL,
	[MiddleName] [nvarchar](500) NULL,
	[LastName] [nvarchar](500) NULL,
	[Birthday] [datetime2](7) NULL,
	[Avatar] [varchar](500) NULL,
	[Gender] [int] NULL,
	[UserName] [varchar](30) NULL,
	[CountryId] [varchar](50) NULL,
	[ProvinceId] [varchar](50) NULL,
	[DistrictId] [varchar](50) NULL,
	[Address] [nvarchar](3000) NULL,
	[PermanentAddress] [nvarchar](3000) NULL,
	[TemporaryAddress] [nvarchar](3000) NULL,
	[NationId] [varchar](50) NULL,
	[ReligionId] [varchar](50) NULL,
	[MarriedStatus] [int] NULL,
	[Status] [int] NOT NULL,
	[Month] [int] NULL,
	[OfficalDate] [datetime2](7) NULL,
	[JoinedDate] [datetime2](7) NOT NULL,
	[OutDate] [datetime2](7) NULL,
	[DepartmentId] [int] NULL,
	[DepartmentPath] [varchar](50) NULL,
	[DepartmentName] [nvarchar](2000) NULL,
	[TitleId] [varchar](50) NULL,
	[TitleName] [nvarchar](2000) NULL,
	[PositionId] [varchar](50) NULL,
	[PositionName] [nvarchar](2000) NULL,
	[PhoneNumber] [varchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[ManagerUserId] [varchar](50) NULL,
	[ManagerFullName] [nvarchar](50) NULL,
	[SuccessorUserId] [varchar](50) NULL,
	[SuccessorFullName] [nvarchar](50) NULL,
	[WorkingForm] [int] NOT NULL,
	[TaxCode] [varchar](50) NULL,
	[TimekeepingId] [varchar](50) NULL,
	[Note] [nvarchar](3000) NULL,
	[ContractCode] [varchar](50) NULL,
	[InsuranceCode] [varchar](50) NULL,
	[InsuranceStatus] [int] NOT NULL,
	[InsuranceName] [nvarchar](2000) NULL,
	[IdCardNumber] [nvarchar](30) NULL,
	[IdCardDateOfIssue] [datetime2](7) NULL,
	[IdCardPlaceOfIssue] [nvarchar](100) NULL,
	[IsActive] [bit] NOT NULL,
	[ConcurrencyStamp] [varchar](50) NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[CreatorId] [varchar](50) NULL,
	[CreatorFullName] [nvarchar](150) NULL,
	[LastUpdate] [datetime2](7) NULL,
	[LastUpdateUserId] [varchar](50) NULL,
	[LastUpdateFullName] [nvarchar](150) NULL,
	[IsDelete] [bit] NOT NULL,
	[DeleteTime] [datetime2](7) NULL,
	[DeleteUserId] [varchar](50) NULL,
	[DeleteFullName] [nvarchar](150) NULL,
	[Total] [decimal](18, 2) NULL,
	[TotalUsed] [decimal](18, 2) NULL,
	[TotalUnUsed] [decimal](18, 2) NULL,
	[TotalMinute] [int] NULL,
	[ReferenceId] [varchar](50) NULL,
	[EnrollNumberNeo] [varchar](50) NULL,
	[EnrollNumberTT] [varchar](50) NULL,
	[AdvanceLeaveGranted] [int] NULL,
	[CompLeaveGranted] [int] NULL,
	[ContractExpirationDate] [datetime2](7) NULL,
	[PersonnelStatus] [int] NULL,
	[DoctorCodeTT] [varchar](50) NULL,
	[DoctorCodeNeo] [varchar](50) NULL,
	[ExpireDays] [int] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_EnrollNumberNeo]  DEFAULT ('') FOR [EnrollNumberNeo]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_EnrollNumberTT]  DEFAULT ('') FOR [EnrollNumberTT]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tình trạng hôn nhân' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'MarriedStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ngày vào làm' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'JoinedDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ngày nghỉ việc' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'OutDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mã số thuế' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'TaxCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mã hợp đồng' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'ContractCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tình trạng bảo hiểm' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'InsuranceStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'đơn vị  đóng' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'InsuranceName'
GO

