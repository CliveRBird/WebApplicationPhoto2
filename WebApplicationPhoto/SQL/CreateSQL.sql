Use [Photo]
go

drop table if exists Photos;

create table Photos
(
PhotoID  		int Identity(1,1) Primary Key,
Title			varchar(255),
[Description]	varchar(MAX),
Photo			varbinary(MAX)
);

truncate table Photos;

insert into [Photos] (Title,[Description], Photo ) 
SELECT 
'Apricot 1', 
'Pic of Apricot',
BulkColumn FROM OPENROWSET(BULK N'c:\PNG\apricot_PNG12647.png', SINGLE_BLOB) image;

insert into [Photos] (Title,[Description], Photo ) 
SELECT 
'Apricot 2', 
'Apricot 2 png',
BulkColumn FROM OPENROWSET(BULK N'c:\PNG\purepng.com-apricotapricotfruitfreshorangeapricotsume-481521304824jpk3y.png', SINGLE_BLOB) image;

insert into [Photos] (Title,[Description], Photo ) 
SELECT 
'Apricot 3', 
'Apricot 3 png',
BulkColumn FROM OPENROWSET(BULK N'c:\PNG\apricot_PNG12657.png', SINGLE_BLOB) image;

select * from Photos

/*
 See
 https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/develop-using-always-encrypted-with-net-framework-data-provider?view=sql-server-ver15

*/


/*

Tip: Run as Admin the SQL Server Management studio. This then creates the certificate on the local computer.
MMC with the certificate snap in. Personal>Certificates folder has the certificate present.
If not run as Admin, the certificate gets lost i.e. the certificate doesn't appear in MMC>Certificates>Personal>Certificates
Thus use the SQL Server Management Tool run as admin to create certificates.
Otherwise you get this error
Failed to decrypt a column encryption key using key store provider: 'MSSQL_CERTIFICATE_STORE'. The last 10 bytes of the encrypted column encryption key are: 'ED-5E-67-6F-1E-C9-73-15-4F-86'. Internal error. Invalid certificate location 'Current User' in certificate path 'Current User/Personal/f2260f28d909d21c642a3d8e0b45a830e79a1420'. Use the following format: //, where is either 'LocalMachine' or 'CurrentUser'. Parameter name: masterKeyPath

*/

CREATE COLUMN MASTER KEY MyCMK  
WITH (  
     KEY_STORE_PROVIDER_NAME = 'MSSQL_CERTIFICATE_STORE',   
     KEY_PATH = 'Current User/Personal/f2260f28d909d21c642a3d8e0b45a830e79a1420'  
   );  
---------------------------------------------  
CREATE COLUMN ENCRYPTION KEY MyCEK   
WITH VALUES  
(  
    COLUMN_MASTER_KEY = MyCMK,   
    ALGORITHM = 'RSA_OAEP',   
    ENCRYPTED_VALUE = 0x01700000016C006F00630061006C006D0061006300680069006E0065002F006D0079002F003200660061006600640038003100320031003400340034006500620031006100320065003000360039003300340038006100350064003400300032003300380065006600620063006300610031006300284FC4316518CF3328A6D9304F65DD2CE387B79D95D077B4156E9ED8683FC0E09FA848275C685373228762B02DF2522AFF6D661782607B4A2275F2F922A5324B392C9D498E4ECFC61B79F0553EE8FB2E5A8635C4DBC0224D5A7F1B136C182DCDE32A00451F1A7AC6B4492067FD0FAC7D3D6F4AB7FC0E86614455DBB2AB37013E0A5B8B5089B180CA36D8B06CDB15E95A7D06E25AACB645D42C85B0B7EA2962BD3080B9A7CDB805C6279FE7DD6941E7EA4C2139E0D4101D8D7891076E70D433A214E82D9030CF1F40C503103075DEEB3D64537D15D244F503C2750CF940B71967F51095BFA51A85D2F764C78704CAB6F015EA87753355367C5C9F66E465C0C66BADEDFDF76FB7E5C21A0D89A2FCCA8595471F8918B1387E055FA0B816E74201CD5C50129D29C015895CD073925B6EA87CAF4A4FAF018C06A3856F5DFB724F42807543F777D82B809232B465D983E6F19DFB572BEA7B61C50154605452A891190FB5A0C4E464862CF5EFAD5E7D91F7D65AA1A78F688E69A1EB098AB42E95C674E234173CD7E0925541AD5AE7CED9A3D12FDFE6EB8EA4F8AAD2629D4F5A18BA3DDCC9CF7F352A892D4BEBDC4A1303F9C683DACD51A237E34B045EBE579A381E26B40DCFBF49EFFA6F65D17F37C6DBA54AA99A65D5573D4EB5BA038E024910A4D36B79A1D4E3C70349DADFF08FD8B4DEE77FDB57F01CB276ED5E676F1EC973154F86  
);  
---------------------------------------------  
DROP TABLE IF EXISTS [dbo].[Patients];
go

CREATE TABLE [dbo].[Patients]
(
[PatientId]     [int] IDENTITY(1,1), 
[SSN]           [char](11) COLLATE Latin1_General_BIN2 ENCRYPTED WITH (ENCRYPTION_TYPE = DETERMINISTIC, ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256', COLUMN_ENCRYPTION_KEY = CEK_Auto1) NOT NULL,
[FirstName]     [nvarchar](50) NULL,
[LastName]      [nvarchar](50) NULL, 
[BirthDate]     [date] ENCRYPTED WITH (ENCRYPTION_TYPE = RANDOMIZED, ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256', COLUMN_ENCRYPTION_KEY = CEK_Auto1) NOT NULL
PRIMARY KEY CLUSTERED ([PatientId] ASC) ON [PRIMARY]
);
GO

DROP TABLE IF EXISTS [dbo].[Person];
GO

CREATE TABLE [dbo].[Person]
(
[PersonID]                  [int] IDENTITY(1,1), 
[NationalInsuranceNumber]   [varchar](50) COLLATE Latin1_General_BIN2 ENCRYPTED WITH (ENCRYPTION_TYPE = DETERMINISTIC, ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256', COLUMN_ENCRYPTION_KEY = CEK_Auto1) NOT NULL,
[FirstName]                 [nvarchar](50) NULL,
[LastName]                  [nvarchar](50) NULL, 
[Photo]                     [varbinary](max) ENCRYPTED WITH (ENCRYPTION_TYPE = RANDOMIZED, ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256', COLUMN_ENCRYPTION_KEY = CEK_Auto1) NULL
PRIMARY KEY CLUSTERED ([PersonID] ASC) ON [PRIMARY]
);
GO

insert into [dbo].[Person] ([NationalInsuranceNumber],[FirstName],[LastName],[Photo]) 
SELECT 
'1234', 
'Clive',
'Thompson',
BulkColumn FROM OPENROWSET(BULK N'c:\PNG\Apple-Fruit-Transparent.png', SINGLE_BLOB) image;
 

/*

TODO
https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/develop-using-always-encrypted-with-net-framework-data-provider?view=sql-server-ver15

*/

CREATE TABLE [dbo].[Person]
(
	[PersonID] [int] IDENTITY(1,1) NOT NULL,
	[NationalInsuranceNumber] [varchar](50) COLLATE Latin1_General_BIN2 ENCRYPTED WITH (COLUMN_ENCRYPTION_KEY = [CEK_Auto1], ENCRYPTION_TYPE = Deterministic, ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256') NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Photo] [varbinary](max) ENCRYPTED WITH (COLUMN_ENCRYPTION_KEY = [CEK_Auto1], ENCRYPTION_TYPE = Randomized, ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256') NULL,
PRIMARY KEY CLUSTERED 
(
[PersonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/*

How to setup the IIS Web server.

1. By publishing the WebApplicationPhoto to IIS the following must be configured first.
a. When publishing use the Method, 'Folder'.
b. Target folder, C:\inetpub\wwwroot
c. Delete all files existing files when a deployment occurs.
d. Using 'Server Manager' perform the installation of, .NET Framework 4.6 Features (3 of 7 installed). Check, 'ASP.NET 4.6 (Installed)' and install it. This install ASP.NET components in the IIS Manager.

Configure the user accounts.
2. In Active Directory add the users 'ORANGE\BI-SQLSERVER' and 'NT AUTHORITY\LOCAL SERVICE'

otherwise you get
{
 Login failed for user 'ORANGE\BI-SQLSERVER$'.
Description: An unhandled exception occurred during the execution of the current web request. Please review the stack trace for more information about the error and where it originated in the code.

Exception Details: System.Data.SqlClient.SqlException: Login failed for user 'ORANGE\BI-SQLSERVER$'.
}

for both users at the root SQL Server security level add two logins for
'ORANGE\BI-SQLSERVER' and 'NT AUTHORITY\LOCAL SERVICE'

In the 'Photo' database add two users
'ORANGE\BI-SQLSERVER' and 'NT AUTHORITY\LOCAL SERVICE'
under Membership
add 'db_reader', 'db_writer' abd 'db_owner'


Configure IIS to allow it the ability to read/write to the encrypted column in SQL Server. 
This requires the ability 
3. In IIS Manager under 'Application Pools' select the 'Default Application Pool', under the identity column ensure the value is set to 'LocalService'.
4. Set the ability for the user 'LOCAL SERVICE' to have read access to the certificate associated with the encrypted column in SQL server.
4a. Open MMC, add the 'Computer Account' for 'Certificates'. Under 'Certificate (Local Computer)' click all tasks and select certificate used to encrypt the columns e.g. 'Always Encrypted Auto Certificate1' click 'Manage Private Keys'.
4b. Add, 'LOCAL SERVICE' user with FULL CONTROL. This allows the LOCAL SERVICE user read access to the certificate. This allows the default Application Pool access to the column certificate.
*/



