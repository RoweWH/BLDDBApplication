CREATE TABLE [dbo].[EdgeCycles]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Buffer] INT NOT NULL, 
    [First] INT NOT NULL, 
    [Second] INT NOT NULL,
    CONSTRAINT [FK_Buffer_EdgePieces] FOREIGN KEY (Buffer) REFERENCES EdgePieces(Id), 
    CONSTRAINT [FK_First_EdgePieces] FOREIGN KEY (First) REFERENCES EdgePieces(Id),
    CONSTRAINT [FK_Second_EdgePieces] FOREIGN KEY (Second) REFERENCES EdgePieces(Id)
)
