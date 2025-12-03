CREATE TABLE [dbo].[ParityAlgorithms]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CaseId] INT NOT NULL, 
    [Algorithm] NVARCHAR(200) NOT NULL,
    CONSTRAINT [FK_ParityAlgorithms_ParityCases] FOREIGN KEY ([CaseId]) REFERENCES ParityCases(Id)
)
