Create Table PersnInfo(
	BusinessEntityID int Primary Key,
	PersonType varchar(50) Not Null,
	NameStyle int not null,
	FirstName varchar(100) not null,
	LastName varchar(100) not null,
	EmailPromotion int Not null 
)