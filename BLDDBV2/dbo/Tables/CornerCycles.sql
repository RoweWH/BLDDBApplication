CREATE TABLE [dbo].[CornerCycles]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Buffer] INT NOT NULL, 
    [First] INT NOT NULL, 
    [Second] INT NOT NULL,
    CONSTRAINT [FK_Buffer_CornerPieces] FOREIGN KEY (Buffer) REFERENCES CornerPieces(Id), 
    CONSTRAINT [FK_First_CornerPieces] FOREIGN KEY (First) REFERENCES CornerPieces(Id),
    CONSTRAINT [FK_Second_CornerPieces] FOREIGN KEY (Second) REFERENCES CornerPieces(Id)
)
