CREATE TABLE [dbo].[Match]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstTeamId] BIGINT NOT NULL, 
    [SecondTeamId] BIGINT NOT NULL, 
    [FirstTeamWins] BIT NOT NULL, 
    CONSTRAINT [FK_FirstTeam] FOREIGN KEY ([FirstTeamId]) REFERENCES [dbo].[Team]([Id]), 
    CONSTRAINT [FK_SecondTeam] FOREIGN KEY ([SecondTeamId]) REFERENCES [dbo].[Team]([Id])
)
