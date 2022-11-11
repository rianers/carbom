CREATE TABLE [Customer]
(
 [id]         int IDENTITY (1, 1) NOT NULL,
 [name]       varchar(50) NOT NULL,
 [email]      varchar(50) NOT NULL,
 [password]   varchar(50) NOT NULL,
 [createdAt]  datetime NOT NULL,
 [modifiedAt] datetime NOT NULL,


 CONSTRAINT [customer_id_pk] PRIMARY KEY CLUSTERED ([id] ASC)
);
GO

CREATE TABLE [Mechanic]
(
 [id]          int IDENTITY (1, 1) NOT NULL,
 [name]        varchar(50) NOT NULL,
 [description] varchar(100) NOT NULL,
 [distance]    float(5),
 [ranking]     float(3),
 [image]       varbinary(MAX),
 [createdAt]   datetime NOT NULL ,
 [modifiedAt]  datetime NOT NULL ,


 CONSTRAINT [mechanic_id_pk] PRIMARY KEY CLUSTERED ([id] ASC)
);
GO

CREATE TABLE [Address]
(
 [id]                  int IDENTITY (1, 1) NOT NULL,
 [state]               varchar(30),
 [city]                varchar(30),
 [street]              varchar(50),
 [number]              varchar(25),
 [neighbourhood]       varchar(30),
 [zippostalcode]       varchar(20),
 [latitude]            decimal(8,6),
 [longitude]           decimal(9,6),
 [createdAt]           datetime NOT NULL,
 [modifiedAt]          datetime NOT NULL,
 [customer_address_id] int NOT NULL,
 [mechanic_address_id] int NOT NULL,


 CONSTRAINT [address_id_pk] PRIMARY KEY CLUSTERED ([id] ASC),
 CONSTRAINT [customer_address_id] FOREIGN KEY ([customer_address_id])  REFERENCES [Customer]([id]),
 CONSTRAINT [mechanic_address] FOREIGN KEY ([mechanic_address_id])  REFERENCES [Mechanic]([id])
);
GO

CREATE NONCLUSTERED INDEX [mechanic_address_id_fk] ON [Address] 
 (
  [mechanic_address_id] ASC
 )

GO

CREATE NONCLUSTERED INDEX [customer_address_id_fk] ON [Address] 
 (
  [customer_address_id] ASC
 )

GO

CREATE TABLE [Service]
(
 [id]               int IDENTITY (1, 1) NOT NULL,
 [name]             varchar(50) NOT NULL,
 [price]            float(5),
 [image]            varbinary(MAX),
 [createdAt]        datetime NOT NULL,
 [modifiedAt]       datetime NOT NULL,
 [mechanic_service_id] int NOT NULL,


 CONSTRAINT [service_id_pk] PRIMARY KEY CLUSTERED ([id] ASC),
 CONSTRAINT [mechanic_service_id] FOREIGN KEY ([mechanic_service_id])  REFERENCES [Mechanic]([id])
);
GO

CREATE NONCLUSTERED INDEX [mechanic_service_id_fk] ON [Service] 
 (
  [mechanic_service_id] ASC
 )

GO


---------------------------------  Sample Commands ---------------------------------

INSERT INTO Mechanic (name, description, distance, ranking, createdAt, modifiedAt) 
VALUES('Mecânica do Zé', 'Mecânica especializada em serviços gerais e conserto. Entre já em contato!', 5.5, 4.3, GETDATE(), GETDATE());

INSERT INTO Service (name, price, createdAt, modifiedAt, mechanic_service_id)
VALUES ('Reparo Simples', 120, GETDATE(), GETDATE(), 1)

INSERT INTO Customer (name, email, password, createdAt, modifiedAt)
VALUES ('José Ramalho dos Santos', 'joseramalho@hotmail.com', 'zezinho1234', GETDATE(), GETDATE())

INSERT INTO Address (state, city, street, number, neighbourhood, zippostalcode, latitude, longitude, createdAt, modifiedAt, customer_address_id, mechanic_address_id)
VALUES ('SP', 'São Paulo', 'Avenida Paulista', '124 A', 'Bela Vista', '01310-000', -23.498350, -46.674740, GETDATE(), GETDATE(), 1, 1)
