CREATE TABLE [dbo].[EdgeAlgorithms]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CycleId] INT NOT NULL, 
    [Algorithm] NVARCHAR(200) NOT NULL,
    CONSTRAINT [FK_EdgeAlgorithms_EdgeCycles] FOREIGN KEY (CycleId) REFERENCES EdgeCycles(Id)
)
