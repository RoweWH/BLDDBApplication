CREATE TABLE [dbo].[ParityCases]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstEdge] INT NOT NULL, 
    [SecondEdge] INT NOT NULL, 
    [FirstCorner] INT NOT NULL, 
    [SecondCorner] INT NOT NULL,
    [Twist] INT NULL, 
    CONSTRAINT [FK_FirstEdge_EdgePieces] FOREIGN KEY (FirstEdge) REFERENCES EdgePieces(Id), 
    CONSTRAINT [FK_SecondEdge_EdgePieces] FOREIGN KEY (SecondEdge) REFERENCES EdgePieces(Id),
    CONSTRAINT [FK_FirstCorner_CornerPieces] FOREIGN KEY (FirstCorner) REFERENCES CornerPieces(Id),
    CONSTRAINT [FK_SecondCorner_CornerPieces] FOREIGN KEY (SecondCorner) REFERENCES CornerPieces(Id),
    CONSTRAINT [FK_Twist_CornerPieces] FOREIGN KEY (Twist) REFERENCES CornerPieces(Id)
)
