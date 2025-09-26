CREATE TABLE [dbo].[CornerAlgorithms]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CycleId] INT NOT NULL, 
    [Algorithm] NVARCHAR(200) NOT NULL,
    CONSTRAINT [FK_CornerAlgorithms_CornerCycles] FOREIGN KEY (CycleId) REFERENCES CornerCycles(Id)
)
