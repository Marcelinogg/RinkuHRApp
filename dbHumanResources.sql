USE [master]
GO
/****** Object:  Database [dbHumanResources]    Script Date: 12/10/2022 12:48:32 a. m. ******/
CREATE DATABASE [dbHumanResources]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'dbHumanResources', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\dbHumanResources.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'dbHumanResources_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\dbHumanResources_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [dbHumanResources] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [dbHumanResources].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [dbHumanResources] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [dbHumanResources] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [dbHumanResources] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [dbHumanResources] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [dbHumanResources] SET ARITHABORT OFF 
GO
ALTER DATABASE [dbHumanResources] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [dbHumanResources] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [dbHumanResources] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [dbHumanResources] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [dbHumanResources] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [dbHumanResources] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [dbHumanResources] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [dbHumanResources] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [dbHumanResources] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [dbHumanResources] SET  DISABLE_BROKER 
GO
ALTER DATABASE [dbHumanResources] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [dbHumanResources] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [dbHumanResources] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [dbHumanResources] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [dbHumanResources] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [dbHumanResources] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [dbHumanResources] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [dbHumanResources] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [dbHumanResources] SET  MULTI_USER 
GO
ALTER DATABASE [dbHumanResources] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [dbHumanResources] SET DB_CHAINING OFF 
GO
ALTER DATABASE [dbHumanResources] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [dbHumanResources] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [dbHumanResources] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [dbHumanResources] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [dbHumanResources] SET QUERY_STORE = OFF
GO
USE [dbHumanResources]
GO
/****** Object:  Schema [Payroll]    Script Date: 12/10/2022 12:48:33 a. m. ******/
CREATE SCHEMA [Payroll]
GO
/****** Object:  UserDefinedFunction [Payroll].[fn_cal_concept_1]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author, Marcelino Guzmán García	>
-- Create date: <Create Date,	October 09th 2022	>
-- Description:	<Description,	Calculate the concept #1 'SALARIO REGULAR'	>
-- Exection:	<Execution,		Select Payroll.fn_cal_concept_1(1000001, 192)		>
-- =============================================
CREATE FUNCTION [Payroll].[fn_cal_concept_1]
(
	@employeeId	int,
	@concept7	int
)
RETURNS money
AS
BEGIN
	-- Return the result of the function
	RETURN (
	Select
			SalaryPerHour * @concept7
	From	dbo.Employees
	Where	Id = @employeeId
	);

END
GO
/****** Object:  UserDefinedFunction [Payroll].[fn_cal_concept_2]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author, Marcelino Guzmán García	>
-- Create date: <Create Date,	October 09th 2022	>
-- Description:	<Description,	Calculate the concept #2 'PAGO POR ENTREGA'	>
-- Exection:	<Execution,		Select Payroll.fn_cal_concept_2(1, 202210, 1000001)		>
-- =============================================
CREATE FUNCTION [Payroll].[fn_cal_concept_2]
(
	@payrollId	int,
	@periodId	int,
	@employeeId	int
)
RETURNS money
AS
BEGIN
	-- Declare the return variable here
	Declare
	@conceptId	int = 2;
	Declare
	@result money = (
	Select
			SUM(amount * times)
	From	Payroll.Transactions
	Where	PayrollId = @payrollId
			And PeriodId = @periodId
			And EmployeeId = @employeeId
			And ConceptId = @conceptId
			And StatusId IN(1,2)
	);
	-- Return the result of the function
	RETURN ISNULL(@result, 0);

END
GO
/****** Object:  UserDefinedFunction [Payroll].[fn_cal_concept_3]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author, Marcelino Guzmán García	>
-- Create date: <Create Date,	October 09th 2022	>
-- Description:	<Description,	Calculate the concept #3 'BONO POR HORA'	>
-- Exection:	<Execution,		Select Payroll.fn_cal_concept_3(1000001, 192)		>
-- =============================================
CREATE FUNCTION [Payroll].[fn_cal_concept_3]
(
	@employeeId	int,
	@Concept7	int
)
RETURNS money
AS
BEGIN
	-- Declare the return variable here
	Declare
	@result money = (
	Select
			
			Case PositionId
				When 1 Then 10
				When 2 Then 5
				Else 0
			End
			*
			@Concept7
	From	dbo.Employees
	Where	Id = @employeeId
	);
	-- Return the result of the function
	RETURN @result;

END
GO
/****** Object:  UserDefinedFunction [Payroll].[fn_cal_concept_4]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author, Marcelino Guzmán García	>
-- Create date: <Create Date,	October 09th 2022	>
-- Description:	<Description,	Calculate the concept #4 'IMPUESTO ISR'	>
-- Exection:	<Execution,		Select Payroll.fn_cal_concept_4(6000, 2000, 0)		>
-- =============================================
CREATE FUNCTION [Payroll].[fn_cal_concept_4]
(
	@Concept1	int,
	@Concept2	int,
	@Concept3	int
)
RETURNS money
AS
BEGIN
	-- Declare the return variable here
	Declare
	@limit	money = 10000.00,
	@salary	money = @Concept1 + @Concept2 + @Concept3

	-- Return the result of the function
	RETURN (Case When @salary > @limit Then 0.12 Else 0.09 End) * @salary;

END
GO
/****** Object:  UserDefinedFunction [Payroll].[fn_cal_concept_5]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author, Marcelino Guzmán García	>
-- Create date: <Create Date,	October 09th 2022	>
-- Description:	<Description,	Calculate the concept #5 'VALES DE DESPENSA'	>
-- Exection:	<Execution,		Select Payroll.fn_cal_concept_5(9000)		>
-- =============================================
CREATE FUNCTION [Payroll].[fn_cal_concept_5]
(
	@concept1	int
)
RETURNS money
AS
BEGIN
	-- Return the result of the function
	RETURN (@concept1 * 0.04);

END
GO
/****** Object:  UserDefinedFunction [Payroll].[fn_cal_concept_7]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author, Marcelino Guzmán García	>
-- Create date: <Create Date,	October 09th 2022	>
-- Description:	<Description,	Calculate the concept #7 'HORAS TRABAJADAS'	>
-- Exection:	<Execution,		Select Payroll.fn_cal_concept_7(1000001)		>
-- =============================================
CREATE FUNCTION [Payroll].[fn_cal_concept_7]
(
	@employeeId	int
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	Declare
	@result money = (
	Select
			HoursPerDay * DaysPerWeek * P.Weeks
	From	dbo.Employees E Inner Join
			dbo.Payrolls P ON E.PayrollId = P.Id
	Where	E.Id = @employeeId
	);
	-- Return the result of the function
	RETURN @result;

END
GO
/****** Object:  Table [dbo].[Positions]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Positions](
	[Id] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
 CONSTRAINT [PK__Position__3214EC071E937993] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Payroll].[Concepts]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Payroll].[Concepts](
	[Id] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[TypConcepteId] [int] NOT NULL,
 CONSTRAINT [PK__Concepts__3214EC077F657353] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Payroll].[PayrollConcepts]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Payroll].[PayrollConcepts](
	[PayrollId] [int] NOT NULL,
	[PeriodId] [int] NOT NULL,
	[ConceptId] [int] NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[amount] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK_PayrollPayrollConcepts] PRIMARY KEY CLUSTERED 
(
	[PayrollId] ASC,
	[PeriodId] ASC,
	[ConceptId] ASC,
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Payroll].[TypesConcepts]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Payroll].[TypesConcepts](
	[Id] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
 CONSTRAINT [PK__TypesCon__3214EC07E21BD65D] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[Id] [int] NOT NULL,
	[FullName] [varchar](255) NOT NULL,
	[PositionId] [int] NOT NULL,
	[SalaryPerHour] [money] NOT NULL,
	[HoursPerDay] [int] NOT NULL,
	[DaysPerWeek] [int] NOT NULL,
	[PayrollId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
 CONSTRAINT [PK__Employee__3214EC074F618EC1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [Payroll].[vwPayrollConcepts]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Script for SelectTopNRows command from SSMS  ******/
CREATE VIEW [Payroll].[vwPayrollConcepts]
AS
SELECT
		E.Id	EmployeeId
		,E.FullName	EmployeeFullName
		,E.PositionId
		,P.Name	PositionName
		,PC.PayrollId
		,PC.PeriodId
		,PC.ConceptId
		,C.Name	ConceptName
		,PC.amount
		,TC.Name	TypeConcept
FROM	[Payroll].[PayrollConcepts] PC INNER JOIN
		[Payroll].[Concepts] C ON PC.ConceptId = C.Id INNER JOIN
		[Payroll].[TypesConcepts] TC ON C.TypConcepteId = TC.Id LEFT JOIN
		[dbo].[Employees] E ON PC.EmployeeId = E.Id AND PC.PayrollId = E.PayrollId LEFT JOIN
		[dbo].[Positions] P ON E.PositionId = P.Id
WHERE	E.StatusId = 1
GO
/****** Object:  Table [dbo].[EmployeeStatus]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeStatus](
	[Id] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
 CONSTRAINT [PK__Employee__3214EC07D2E5D7AD] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payrolls]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payrolls](
	[Id] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Weeks] [int] NOT NULL,
 CONSTRAINT [PK__Payroll__3214EC075BF0C2F6] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Payroll].[Periods]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Payroll].[Periods](
	[PayrollId] [int] NOT NULL,
	[Id] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[PaymentDate] [date] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_PayrollPeriods] PRIMARY KEY CLUSTERED 
(
	[PayrollId] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Payroll].[Transactions]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Payroll].[Transactions](
	[PayrollId] [int] NOT NULL,
	[PeriodId] [int] NOT NULL,
	[ConceptId] [int] NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[Sequence] [int] NOT NULL,
	[times] [int] NOT NULL,
	[amount] [decimal](10, 2) NOT NULL,
	[UserId] [varchar](255) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[StatusId] [int] NOT NULL,
 CONSTRAINT [PK_PayrollTransactions] PRIMARY KEY CLUSTERED 
(
	[PayrollId] ASC,
	[PeriodId] ASC,
	[ConceptId] ASC,
	[EmployeeId] ASC,
	[Sequence] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Payroll].[TransactionsStatus]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Payroll].[TransactionsStatus](
	[Id] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
 CONSTRAINT [PK__Transact__3214EC07BEE823A2] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Employees] ([Id], [FullName], [PositionId], [SalaryPerHour], [HoursPerDay], [DaysPerWeek], [PayrollId], [StatusId]) VALUES (10000001, N'Marcelino Guzmán García', 1, 30.0000, 8, 6, 1, 1)
INSERT [dbo].[Employees] ([Id], [FullName], [PositionId], [SalaryPerHour], [HoursPerDay], [DaysPerWeek], [PayrollId], [StatusId]) VALUES (10000002, N'Luis Pérez', 2, 30.0000, 8, 6, 1, 1)
INSERT [dbo].[Employees] ([Id], [FullName], [PositionId], [SalaryPerHour], [HoursPerDay], [DaysPerWeek], [PayrollId], [StatusId]) VALUES (10000003, N'Juan López', 3, 30.0000, 8, 6, 1, 1)
INSERT [dbo].[Employees] ([Id], [FullName], [PositionId], [SalaryPerHour], [HoursPerDay], [DaysPerWeek], [PayrollId], [StatusId]) VALUES (10000004, N'Mateo Juárez', 3, 30.0000, 8, 6, 1, 1)
GO
INSERT [dbo].[EmployeeStatus] ([Id], [Name]) VALUES (1, N'ACTIVO')
INSERT [dbo].[EmployeeStatus] ([Id], [Name]) VALUES (2, N'BAJA')
GO
INSERT [dbo].[Payrolls] ([Id], [Name], [Weeks]) VALUES (1, N'NOMINA MENSUAL ORDINARIA', 4)
INSERT [dbo].[Payrolls] ([Id], [Name], [Weeks]) VALUES (2, N'NOMINA SEMANAL ORDINARIA', 1)
GO
INSERT [dbo].[Positions] ([Id], [Name]) VALUES (1, N'CHOFER')
INSERT [dbo].[Positions] ([Id], [Name]) VALUES (2, N'CARGADOR')
INSERT [dbo].[Positions] ([Id], [Name]) VALUES (3, N'AUXILIAR')
GO
INSERT [Payroll].[Concepts] ([Id], [Name], [TypConcepteId]) VALUES (1, N'SALARIO REGULAR', 1)
INSERT [Payroll].[Concepts] ([Id], [Name], [TypConcepteId]) VALUES (2, N'PAGO POR ENTREGA', 1)
INSERT [Payroll].[Concepts] ([Id], [Name], [TypConcepteId]) VALUES (3, N'BONO POR HORA', 1)
INSERT [Payroll].[Concepts] ([Id], [Name], [TypConcepteId]) VALUES (4, N'IMPUESTO ISR', 2)
INSERT [Payroll].[Concepts] ([Id], [Name], [TypConcepteId]) VALUES (5, N'VALES DE DESPENSA', 3)
INSERT [Payroll].[Concepts] ([Id], [Name], [TypConcepteId]) VALUES (6, N'SALARIO NETO', 3)
INSERT [Payroll].[Concepts] ([Id], [Name], [TypConcepteId]) VALUES (7, N'HORAS TRABAJADAS', 3)
GO
INSERT [Payroll].[Periods] ([PayrollId], [Id], [Name], [StartDate], [EndDate], [PaymentDate], [Active]) VALUES (1, 20221, N' ENERO 2022', CAST(N'2022-01-01' AS Date), CAST(N'2022-01-31' AS Date), CAST(N'2022-01-31' AS Date), 0)
INSERT [Payroll].[Periods] ([PayrollId], [Id], [Name], [StartDate], [EndDate], [PaymentDate], [Active]) VALUES (1, 20222, N'FEBRERO 2022', CAST(N'2022-02-01' AS Date), CAST(N'2022-02-28' AS Date), CAST(N'2022-02-28' AS Date), 0)
INSERT [Payroll].[Periods] ([PayrollId], [Id], [Name], [StartDate], [EndDate], [PaymentDate], [Active]) VALUES (1, 20223, N'MARZO 2022', CAST(N'2022-03-01' AS Date), CAST(N'2022-03-31' AS Date), CAST(N'2022-03-31' AS Date), 0)
INSERT [Payroll].[Periods] ([PayrollId], [Id], [Name], [StartDate], [EndDate], [PaymentDate], [Active]) VALUES (1, 20224, N'ABRIL 2022', CAST(N'2022-04-01' AS Date), CAST(N'2022-04-30' AS Date), CAST(N'2022-04-30' AS Date), 0)
INSERT [Payroll].[Periods] ([PayrollId], [Id], [Name], [StartDate], [EndDate], [PaymentDate], [Active]) VALUES (1, 20225, N'MAYO 2022', CAST(N'2022-05-01' AS Date), CAST(N'2022-05-31' AS Date), CAST(N'2022-05-31' AS Date), 0)
INSERT [Payroll].[Periods] ([PayrollId], [Id], [Name], [StartDate], [EndDate], [PaymentDate], [Active]) VALUES (1, 20226, N'JUNIO 2022', CAST(N'2022-06-01' AS Date), CAST(N'2022-06-30' AS Date), CAST(N'2022-06-30' AS Date), 0)
INSERT [Payroll].[Periods] ([PayrollId], [Id], [Name], [StartDate], [EndDate], [PaymentDate], [Active]) VALUES (1, 20227, N'JULIO 2022', CAST(N'2022-07-01' AS Date), CAST(N'2022-07-31' AS Date), CAST(N'2022-07-31' AS Date), 0)
INSERT [Payroll].[Periods] ([PayrollId], [Id], [Name], [StartDate], [EndDate], [PaymentDate], [Active]) VALUES (1, 20228, N'AGOSTO 2022', CAST(N'2022-08-01' AS Date), CAST(N'2022-08-31' AS Date), CAST(N'2022-08-31' AS Date), 0)
INSERT [Payroll].[Periods] ([PayrollId], [Id], [Name], [StartDate], [EndDate], [PaymentDate], [Active]) VALUES (1, 20229, N'SEPTIEMBRE 2022', CAST(N'2022-09-01' AS Date), CAST(N'2022-09-30' AS Date), CAST(N'2022-09-30' AS Date), 0)
INSERT [Payroll].[Periods] ([PayrollId], [Id], [Name], [StartDate], [EndDate], [PaymentDate], [Active]) VALUES (1, 202210, N'OCTUBRE 2022', CAST(N'2022-10-01' AS Date), CAST(N'2022-10-31' AS Date), CAST(N'2022-10-31' AS Date), 1)
INSERT [Payroll].[Periods] ([PayrollId], [Id], [Name], [StartDate], [EndDate], [PaymentDate], [Active]) VALUES (1, 202211, N'NOVIEMBRE 2022', CAST(N'2022-11-01' AS Date), CAST(N'2022-11-30' AS Date), CAST(N'2022-11-30' AS Date), 0)
INSERT [Payroll].[Periods] ([PayrollId], [Id], [Name], [StartDate], [EndDate], [PaymentDate], [Active]) VALUES (1, 202212, N'DICIEMBRE 2022', CAST(N'2022-12-01' AS Date), CAST(N'2022-12-31' AS Date), CAST(N'2022-12-31' AS Date), 0)
INSERT [Payroll].[Periods] ([PayrollId], [Id], [Name], [StartDate], [EndDate], [PaymentDate], [Active]) VALUES (2, 20221, N'OCTUBRE SEMANA 1 2022', CAST(N'2022-10-03' AS Date), CAST(N'2022-10-09' AS Date), CAST(N'2022-10-09' AS Date), 0)
INSERT [Payroll].[Periods] ([PayrollId], [Id], [Name], [StartDate], [EndDate], [PaymentDate], [Active]) VALUES (2, 20222, N'OCTUBRE SEMANA 2 2022', CAST(N'2022-10-10' AS Date), CAST(N'2022-10-16' AS Date), CAST(N'2022-10-16' AS Date), 1)
INSERT [Payroll].[Periods] ([PayrollId], [Id], [Name], [StartDate], [EndDate], [PaymentDate], [Active]) VALUES (2, 20223, N'OCTUBRE SEMANA 3 2022', CAST(N'2022-10-17' AS Date), CAST(N'2022-10-23' AS Date), CAST(N'2022-10-23' AS Date), 0)
INSERT [Payroll].[Periods] ([PayrollId], [Id], [Name], [StartDate], [EndDate], [PaymentDate], [Active]) VALUES (2, 20224, N'OCTUBRE SEMANA 4 2022', CAST(N'2022-10-24' AS Date), CAST(N'2022-10-30' AS Date), CAST(N'2022-10-30' AS Date), 0)
GO
INSERT [Payroll].[TransactionsStatus] ([Id], [Name]) VALUES (1, N'DISPONIBLE')
INSERT [Payroll].[TransactionsStatus] ([Id], [Name]) VALUES (2, N'PROCESADO')
INSERT [Payroll].[TransactionsStatus] ([Id], [Name]) VALUES (3, N'CANCELADO')
GO
INSERT [Payroll].[TypesConcepts] ([Id], [Name]) VALUES (1, N'PERCEPCIONES')
INSERT [Payroll].[TypesConcepts] ([Id], [Name]) VALUES (2, N'DEDUCCIONES')
INSERT [Payroll].[TypesConcepts] ([Id], [Name]) VALUES (3, N'INFORMATIVO')
GO
ALTER TABLE [Payroll].[PayrollConcepts] ADD  CONSTRAINT [DF_PayrollConcepts_amount]  DEFAULT ((0)) FOR [amount]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK_Employees_EmployeeStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[EmployeeStatus] ([Id])
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_Employees_EmployeeStatus]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK_Employees_Payrolls] FOREIGN KEY([PayrollId])
REFERENCES [dbo].[Payrolls] ([Id])
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_Employees_Payrolls]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK_Employees_Positions] FOREIGN KEY([PositionId])
REFERENCES [dbo].[Positions] ([Id])
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_Employees_Positions]
GO
ALTER TABLE [Payroll].[Concepts]  WITH CHECK ADD  CONSTRAINT [FK_PayrollConcepts_PayrollTypesConcepts] FOREIGN KEY([TypConcepteId])
REFERENCES [Payroll].[TypesConcepts] ([Id])
GO
ALTER TABLE [Payroll].[Concepts] CHECK CONSTRAINT [FK_PayrollConcepts_PayrollTypesConcepts]
GO
ALTER TABLE [Payroll].[PayrollConcepts]  WITH CHECK ADD  CONSTRAINT [FK_PayrollPayrollConcepts_Employees] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employees] ([Id])
GO
ALTER TABLE [Payroll].[PayrollConcepts] CHECK CONSTRAINT [FK_PayrollPayrollConcepts_Employees]
GO
ALTER TABLE [Payroll].[PayrollConcepts]  WITH CHECK ADD  CONSTRAINT [FK_PayrollPayrollConcepts_PayrollConcepts] FOREIGN KEY([ConceptId])
REFERENCES [Payroll].[Concepts] ([Id])
GO
ALTER TABLE [Payroll].[PayrollConcepts] CHECK CONSTRAINT [FK_PayrollPayrollConcepts_PayrollConcepts]
GO
ALTER TABLE [Payroll].[PayrollConcepts]  WITH CHECK ADD  CONSTRAINT [FK_PayrollPayrollConcepts_PayrollPeriods] FOREIGN KEY([PayrollId], [PeriodId])
REFERENCES [Payroll].[Periods] ([PayrollId], [Id])
GO
ALTER TABLE [Payroll].[PayrollConcepts] CHECK CONSTRAINT [FK_PayrollPayrollConcepts_PayrollPeriods]
GO
ALTER TABLE [Payroll].[PayrollConcepts]  WITH CHECK ADD  CONSTRAINT [FK_PayrollPayrollConcepts_Payrolls] FOREIGN KEY([PayrollId])
REFERENCES [dbo].[Payrolls] ([Id])
GO
ALTER TABLE [Payroll].[PayrollConcepts] CHECK CONSTRAINT [FK_PayrollPayrollConcepts_Payrolls]
GO
ALTER TABLE [Payroll].[Periods]  WITH CHECK ADD  CONSTRAINT [FK_PayrollPeriods_Payrolls] FOREIGN KEY([PayrollId])
REFERENCES [dbo].[Payrolls] ([Id])
GO
ALTER TABLE [Payroll].[Periods] CHECK CONSTRAINT [FK_PayrollPeriods_Payrolls]
GO
ALTER TABLE [Payroll].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_PayrollTransactions_Employees] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employees] ([Id])
GO
ALTER TABLE [Payroll].[Transactions] CHECK CONSTRAINT [FK_PayrollTransactions_Employees]
GO
ALTER TABLE [Payroll].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_PayrollTransactions_PayrollConcepts] FOREIGN KEY([ConceptId])
REFERENCES [Payroll].[Concepts] ([Id])
GO
ALTER TABLE [Payroll].[Transactions] CHECK CONSTRAINT [FK_PayrollTransactions_PayrollConcepts]
GO
ALTER TABLE [Payroll].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_PayrollTransactions_PayrollPeriods] FOREIGN KEY([PayrollId], [PeriodId])
REFERENCES [Payroll].[Periods] ([PayrollId], [Id])
GO
ALTER TABLE [Payroll].[Transactions] CHECK CONSTRAINT [FK_PayrollTransactions_PayrollPeriods]
GO
ALTER TABLE [Payroll].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_PayrollTransactions_Payrolls] FOREIGN KEY([PayrollId])
REFERENCES [dbo].[Payrolls] ([Id])
GO
ALTER TABLE [Payroll].[Transactions] CHECK CONSTRAINT [FK_PayrollTransactions_Payrolls]
GO
ALTER TABLE [Payroll].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_PayrollTransactions_PayrollTransactionsStatus] FOREIGN KEY([StatusId])
REFERENCES [Payroll].[TransactionsStatus] ([Id])
GO
ALTER TABLE [Payroll].[Transactions] CHECK CONSTRAINT [FK_PayrollTransactions_PayrollTransactionsStatus]
GO
/****** Object:  StoredProcedure [Payroll].[CalculateSalary]    Script Date: 12/10/2022 12:48:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author, Marcelino Guzmán García	>
-- Create date: <Create Date,	October 09th 2022	>
-- Description:	<Description,	Calculate the salary	>
-- Execution:	<Execution,				Payroll.CalculateSalary @PayrollId = 1, @PeriodId= 20221										>
-- Execution:	<Execution,				Payroll.CalculateSalary @PayrollId = 1, @PeriodId= 202210,	@EmployeeIds = '1000001'			>
-- =============================================
CREATE PROCEDURE [Payroll].[CalculateSalary]
	@PayrollId		int,
	@PeriodId		int,
	@EmployeeIds	varchar(max) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;
	-- Cursor variables
	Declare
	@employeeId	int;


	-- Iterate each employee
	DECLARE cursorEmployee CURSOR FOR
		Select
				Id
		From	dbo.Employees
		Where	StatusId = 1
				And (
					Id In(Select value From STRING_SPLIT(@EmployeeIds, ','))
					Or
					@EmployeeIds = ''
				)
	OPEN cursorEmployee
	FETCH NEXT FROM cursorEmployee INTO @employeeId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		Declare @concept7	money = (Select Payroll.fn_cal_concept_7(@employeeId))										--	HORAS TRABAJADAS
		Declare	@concept1	money = (Select Payroll.fn_cal_concept_1(@employeeId, @concept7));							--	SALARIO REGULAR	
		Declare @concept2	money = (Select Payroll.fn_cal_concept_2(@payrollId, @PeriodId, @employeeId))				--	PAGO POR ENTREGA
		Declare @concept3	money = (Select Payroll.fn_cal_concept_3(@employeeId, @concept7))							--	BONO POR HORA
		Declare @concept4	money = (Select Payroll.fn_cal_concept_4(@concept1, @concept2, @concept3))					--	IMPUESTO ISR
		Declare @concept5	money = (Select Payroll.fn_cal_concept_5(@concept1))										--	VALES DE DESPENSA
		Declare @concept6	money = ( (@concept1 + @concept2 + @concept3) -  @concept4)									--	SALARIO NETO

		BEGIN TRY
			BEGIN TRANSACTION
			-- Save all changes
			Update
			Payroll.Transactions
			Set StatusId = 2
			Where	PayrollId = @payrollId	And PeriodId = @PeriodId And EmployeeId = @employeeId And StatusId IN(1,2)

			Delete
			Payroll.PayrollConcepts
			Where	PayrollId = @payrollId	And PeriodId = @PeriodId And EmployeeId = @employeeId


			INSERT INTO Payroll.PayrollConcepts
			(	PayrollId,	PeriodId,	ConceptId,	EmployeeId,	amount	)
			VALUES
			(@payrollId,	@PeriodId,	1,	@employeeId,	@concept1),
			(@payrollId,	@PeriodId,	2,	@employeeId,	@concept2),
			(@payrollId,	@PeriodId,	3,	@employeeId,	@concept3),
			(@payrollId,	@PeriodId,	4,	@employeeId,	@concept4),
			(@payrollId,	@PeriodId,	5,	@employeeId,	@concept5),
			(@payrollId,	@PeriodId,	6,	@employeeId,	@concept6),
			(@payrollId,	@PeriodId,	7,	@employeeId,	@concept7);
		COMMIT TRANSACTION
			Print @employeeId
		END TRY
		BEGIN CATCH
			IF @@TRANCOUNT > 0
                ROLLBACK;
            Print CONCAT('Failed (', @employeeId, ') Error: ' ,ERROR_NUMBER(), ' ' ,ERROR_MESSAGE());
		END CATCH

		FETCH NEXT FROM cursorEmployee INTO @employeeId
	END
	CLOSE cursorEmployee
	DEALLOCATE cursorEmployee

END
GO
USE [master]
GO
ALTER DATABASE [dbHumanResources] SET  READ_WRITE 
GO
