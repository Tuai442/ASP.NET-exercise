CREATE TABLE Gebruiker (KlantNr int(10) NOT NULL AUTO_INCREMENT, Naam varchar(255) NOT NULL, Email varchar(255) NOT NULL, TelefoonNummer varchar(255) NOT NULL, Locatie varchar(255) NOT NULL, PRIMARY KEY (KlantNr));
CREATE TABLE Reservatie (ReservatieNummer int(10) NOT NULL AUTO_INCREMENT, AantalPlaatsen int(10) NOT NULL, Datum timestamp NULL, TafelNummer int(10) NOT NULL, ContactPersoonKlantNr int(10) NOT NULL, RestaurantId int(10) NOT NULL, PRIMARY KEY (ReservatieNummer));
CREATE TABLE Restaurant (Id int(10) NOT NULL AUTO_INCREMENT, Naam varchar(255) NOT NULL, Locatie varchar(255) NOT NULL, Keuken varchar(255) NOT NULL, ContactGegevens varchar(255) NOT NULL, PRIMARY KEY (Id));
ALTER TABLE Reservatie ADD CONSTRAINT FKReservatie29400 FOREIGN KEY (ContactPersoonKlantNr) REFERENCES Gebruiker (KlantNr);
ALTER TABLE Reservatie ADD CONSTRAINT FKReservatie321892 FOREIGN KEY (RestaurantId) REFERENCES Restaurant (Id);
