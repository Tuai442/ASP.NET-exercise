drop table locatie;
drop table restaurant_tafel
drop table reservatie;

drop table restaurant 
drop table tafel;
drop table gebruiker;


CREATE TABLE [dbo].[locatie] (
    [id]    INT           IDENTITY (1, 1) NOT NULL,
    [postcode]        VARCHAR (300) NOT NULL,
    [gemeente]       VARCHAR (300) NOT NULL,
    [straat] VARCHAR (300) ,
    [huisNr]     INT,

    PRIMARY KEY (id)
    
);




CREATE TABLE [dbo].[tafel] (
    [id]       INT           IDENTITY (1, 1) NOT NULL,
    [plaatsen] INT NULL,
    
    PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[restaurant] (
    [id]               INT           IDENTITY (1, 1) NOT NULL,
    [naam]             VARCHAR (200) NOT NULL,
    [locatie_id]       INT           NOT NULL,
    [keuken]           VARCHAR (200) NOT NULL,
    [telefoon]		   VARCHAR (200) NOT NULL,
	[email]			   VARCHAR (200) NOT NULL,
    [actief]           BIT           DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Restaurant_Locatie] FOREIGN KEY ([locatie_id]) REFERENCES [dbo].[locatie] ([id])
);


CREATE TABLE [dbo].[gebruiker] (
    [klant_nr]    INT           IDENTITY (1, 1) NOT NULL,
    [naam]        VARCHAR (300) NOT NULL,
    [email]       VARCHAR (300) NOT NULL,
    [telefoon_nr] VARCHAR (300) NOT NULL,
    [locatie_id]    int NOT NULL,
    [actief] Bit DEFAULT 1,

    PRIMARY KEY CLUSTERED ([klant_nr] ASC),
    CONSTRAINT [FK_Gebruiker_Locatie] FOREIGN KEY ([locatie_id]) REFERENCES [dbo].[locatie] ([id]),
);


CREATE TABLE [dbo].[restaurant_tafel] (
    [restaurant_id] INT NOT NULL,
    [tafel_id]      INT NOT NULL,
    CONSTRAINT [FKreservatie619949] FOREIGN KEY ([restaurant_id]) REFERENCES [dbo].[restaurant] ([id]),
    CONSTRAINT [FKreservatie619950] FOREIGN KEY ([tafel_id]) REFERENCES [dbo].[tafel] ([id])
);




CREATE TABLE [dbo].[reservatie] (
    [reservatie_nr]      INT      IDENTITY (1, 1) NOT NULL,
    [aantal_plaatsen]    INT      NOT NULL,
    [datum]              DATETIME NOT NULL,
    [contact_persoon_id] INT      NOT NULL,
    [restaurant_id]      INT      NOT NULL,
    [tafel_id]           INT      NOT NULL,
    PRIMARY KEY CLUSTERED ([reservatie_nr] ASC),
    CONSTRAINT [FKreservatie926936] FOREIGN KEY ([contact_persoon_id]) REFERENCES [dbo].[gebruiker] ([klant_nr]),
    CONSTRAINT [FKreservatie619945] FOREIGN KEY ([restaurant_id]) REFERENCES [dbo].[restaurant] ([id]),
    CONSTRAINT [FKreservatie619948] FOREIGN KEY ([tafel_id]) REFERENCES [dbo].[tafel] ([id])
);




