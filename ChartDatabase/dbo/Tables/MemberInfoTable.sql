CREATE TABLE [dbo].[MemberInfoTable]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1, 1),
	[EID] NVarChar(20) Not Null,
	[Type] NVarChar(10) Not Null,
	[FirstName] NVarChar(25) Not Null,
	[LastName] NVarChar(25) Not Null,
	[JobLvl] NVarChar(10) Not Null,
	[Location] NVarChar(100) Not Null Default(''),
	[RoleName] NVarChar(50),
	[TeamName] NVarChar(50),
	[ValStrm] NVarChar(50),
	[LT] NVarChar(50)
)
GO
