
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/28/2015 14:19:39
-- Generated from EDMX file: D:\labs\TBD_CP\TBD\Railways\Railways\Model\RailwayDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [D:\labs\TBD_CP\TBD\Railways\Railways\Model\RailwayDB.mdf];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_WagonWagon_Seat]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WagonSeatSet] DROP CONSTRAINT [FK_WagonWagon_Seat];
GO
IF OBJECT_ID(N'[dbo].[FK_SeatWagon_Seat]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WagonSeatSet] DROP CONSTRAINT [FK_SeatWagon_Seat];
GO
IF OBJECT_ID(N'[dbo].[FK_TrainTrain_Wagon]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TrainWagonSet] DROP CONSTRAINT [FK_TrainTrain_Wagon];
GO
IF OBJECT_ID(N'[dbo].[FK_WagonTrain_Wagon]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TrainWagonSet] DROP CONSTRAINT [FK_WagonTrain_Wagon];
GO
IF OBJECT_ID(N'[dbo].[FK_TrainVoyage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VoyageSet] DROP CONSTRAINT [FK_TrainVoyage];
GO
IF OBJECT_ID(N'[dbo].[FK_RouteVoyage_Route]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VoyageRouteSet] DROP CONSTRAINT [FK_RouteVoyage_Route];
GO
IF OBJECT_ID(N'[dbo].[FK_VoyageVoyage_Route]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VoyageRouteSet] DROP CONSTRAINT [FK_VoyageVoyage_Route];
GO
IF OBJECT_ID(N'[dbo].[FK_SeatTicket]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TicketSet] DROP CONSTRAINT [FK_SeatTicket];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientTicket]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TicketSet] DROP CONSTRAINT [FK_ClientTicket];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeTicket]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TicketSet] DROP CONSTRAINT [FK_EmployeeTicket];
GO
IF OBJECT_ID(N'[dbo].[FK_StationRoute]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RouteSet] DROP CONSTRAINT [FK_StationRoute];
GO
IF OBJECT_ID(N'[dbo].[FK_RouteTicket]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TicketSet] DROP CONSTRAINT [FK_RouteTicket];
GO
IF OBJECT_ID(N'[dbo].[FK_RouteTicket1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TicketSet] DROP CONSTRAINT [FK_RouteTicket1];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[EmployeeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeeSet];
GO
IF OBJECT_ID(N'[dbo].[ClientSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClientSet];
GO
IF OBJECT_ID(N'[dbo].[SeatSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SeatSet];
GO
IF OBJECT_ID(N'[dbo].[WagonSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WagonSet];
GO
IF OBJECT_ID(N'[dbo].[TrainSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TrainSet];
GO
IF OBJECT_ID(N'[dbo].[StationSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StationSet];
GO
IF OBJECT_ID(N'[dbo].[WagonSeatSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WagonSeatSet];
GO
IF OBJECT_ID(N'[dbo].[TrainWagonSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TrainWagonSet];
GO
IF OBJECT_ID(N'[dbo].[RouteSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RouteSet];
GO
IF OBJECT_ID(N'[dbo].[VoyageSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VoyageSet];
GO
IF OBJECT_ID(N'[dbo].[VoyageRouteSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VoyageRouteSet];
GO
IF OBJECT_ID(N'[dbo].[TicketSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TicketSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'EmployeeSet'
CREATE TABLE [dbo].[EmployeeSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FullName] nvarchar(max)  NULL,
    [UserRights] tinyint  NULL,
    [Password] nvarchar(max)  NULL
);
GO

-- Creating table 'ClientSet'
CREATE TABLE [dbo].[ClientSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FullName] nvarchar(max)  NULL,
    [PassportNum] nvarchar(max)  NULL
);
GO

-- Creating table 'SeatSet'
CREATE TABLE [dbo].[SeatSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SeatNum] int  NULL
);
GO

-- Creating table 'WagonSet'
CREATE TABLE [dbo].[WagonSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [WagonNum] tinyint  NULL,
    [WagonType] tinyint  NULL
);
GO

-- Creating table 'TrainSet'
CREATE TABLE [dbo].[TrainSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TrainNum] nvarchar(max)  NULL
);
GO

-- Creating table 'StationSet'
CREATE TABLE [dbo].[StationSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StationName] nvarchar(max)  NULL
);
GO

-- Creating table 'WagonSeatSet'
CREATE TABLE [dbo].[WagonSeatSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [WagonId] int  NULL,
    [SeatId] int  NULL
);
GO

-- Creating table 'TrainWagonSet'
CREATE TABLE [dbo].[TrainWagonSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TrainId] int  NULL,
    [WagonId] int  NULL
);
GO

-- Creating table 'RouteSet'
CREATE TABLE [dbo].[RouteSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Distance] float  NULL,
    [DepartureTimeOffset] datetime  NULL,
    [ArrivalTimeOffset] datetime  NULL,
    [StationId] int  NULL
);
GO

-- Creating table 'VoyageSet'
CREATE TABLE [dbo].[VoyageSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Periodicity] tinyint  NULL,
    [DepartureDateTime] datetime  NULL,
    [TrainId] int  NULL
);
GO

-- Creating table 'VoyageRouteSet'
CREATE TABLE [dbo].[VoyageRouteSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RouteId] int  NULL,
    [VoyageId] int  NULL
);
GO

-- Creating table 'TicketSet'
CREATE TABLE [dbo].[TicketSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SeatId] int  NULL,
    [ClientId] int  NULL,
    [EmployeeId] int  NULL,
    [Price] float  NULL,
    [DepartureRouteId] int  NULL,
    [ArrivalRouteId] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'EmployeeSet'
ALTER TABLE [dbo].[EmployeeSet]
ADD CONSTRAINT [PK_EmployeeSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClientSet'
ALTER TABLE [dbo].[ClientSet]
ADD CONSTRAINT [PK_ClientSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SeatSet'
ALTER TABLE [dbo].[SeatSet]
ADD CONSTRAINT [PK_SeatSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'WagonSet'
ALTER TABLE [dbo].[WagonSet]
ADD CONSTRAINT [PK_WagonSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TrainSet'
ALTER TABLE [dbo].[TrainSet]
ADD CONSTRAINT [PK_TrainSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StationSet'
ALTER TABLE [dbo].[StationSet]
ADD CONSTRAINT [PK_StationSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'WagonSeatSet'
ALTER TABLE [dbo].[WagonSeatSet]
ADD CONSTRAINT [PK_WagonSeatSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TrainWagonSet'
ALTER TABLE [dbo].[TrainWagonSet]
ADD CONSTRAINT [PK_TrainWagonSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RouteSet'
ALTER TABLE [dbo].[RouteSet]
ADD CONSTRAINT [PK_RouteSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VoyageSet'
ALTER TABLE [dbo].[VoyageSet]
ADD CONSTRAINT [PK_VoyageSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VoyageRouteSet'
ALTER TABLE [dbo].[VoyageRouteSet]
ADD CONSTRAINT [PK_VoyageRouteSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TicketSet'
ALTER TABLE [dbo].[TicketSet]
ADD CONSTRAINT [PK_TicketSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [WagonId] in table 'WagonSeatSet'
ALTER TABLE [dbo].[WagonSeatSet]
ADD CONSTRAINT [FK_WagonWagon_Seat]
    FOREIGN KEY ([WagonId])
    REFERENCES [dbo].[WagonSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WagonWagon_Seat'
CREATE INDEX [IX_FK_WagonWagon_Seat]
ON [dbo].[WagonSeatSet]
    ([WagonId]);
GO

-- Creating foreign key on [SeatId] in table 'WagonSeatSet'
ALTER TABLE [dbo].[WagonSeatSet]
ADD CONSTRAINT [FK_SeatWagon_Seat]
    FOREIGN KEY ([SeatId])
    REFERENCES [dbo].[SeatSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SeatWagon_Seat'
CREATE INDEX [IX_FK_SeatWagon_Seat]
ON [dbo].[WagonSeatSet]
    ([SeatId]);
GO

-- Creating foreign key on [TrainId] in table 'TrainWagonSet'
ALTER TABLE [dbo].[TrainWagonSet]
ADD CONSTRAINT [FK_TrainTrain_Wagon]
    FOREIGN KEY ([TrainId])
    REFERENCES [dbo].[TrainSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TrainTrain_Wagon'
CREATE INDEX [IX_FK_TrainTrain_Wagon]
ON [dbo].[TrainWagonSet]
    ([TrainId]);
GO

-- Creating foreign key on [WagonId] in table 'TrainWagonSet'
ALTER TABLE [dbo].[TrainWagonSet]
ADD CONSTRAINT [FK_WagonTrain_Wagon]
    FOREIGN KEY ([WagonId])
    REFERENCES [dbo].[WagonSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WagonTrain_Wagon'
CREATE INDEX [IX_FK_WagonTrain_Wagon]
ON [dbo].[TrainWagonSet]
    ([WagonId]);
GO

-- Creating foreign key on [TrainId] in table 'VoyageSet'
ALTER TABLE [dbo].[VoyageSet]
ADD CONSTRAINT [FK_TrainVoyage]
    FOREIGN KEY ([TrainId])
    REFERENCES [dbo].[TrainSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TrainVoyage'
CREATE INDEX [IX_FK_TrainVoyage]
ON [dbo].[VoyageSet]
    ([TrainId]);
GO

-- Creating foreign key on [RouteId] in table 'VoyageRouteSet'
ALTER TABLE [dbo].[VoyageRouteSet]
ADD CONSTRAINT [FK_RouteVoyage_Route]
    FOREIGN KEY ([RouteId])
    REFERENCES [dbo].[RouteSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RouteVoyage_Route'
CREATE INDEX [IX_FK_RouteVoyage_Route]
ON [dbo].[VoyageRouteSet]
    ([RouteId]);
GO

-- Creating foreign key on [VoyageId] in table 'VoyageRouteSet'
ALTER TABLE [dbo].[VoyageRouteSet]
ADD CONSTRAINT [FK_VoyageVoyage_Route]
    FOREIGN KEY ([VoyageId])
    REFERENCES [dbo].[VoyageSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VoyageVoyage_Route'
CREATE INDEX [IX_FK_VoyageVoyage_Route]
ON [dbo].[VoyageRouteSet]
    ([VoyageId]);
GO

-- Creating foreign key on [SeatId] in table 'TicketSet'
ALTER TABLE [dbo].[TicketSet]
ADD CONSTRAINT [FK_SeatTicket]
    FOREIGN KEY ([SeatId])
    REFERENCES [dbo].[SeatSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SeatTicket'
CREATE INDEX [IX_FK_SeatTicket]
ON [dbo].[TicketSet]
    ([SeatId]);
GO

-- Creating foreign key on [ClientId] in table 'TicketSet'
ALTER TABLE [dbo].[TicketSet]
ADD CONSTRAINT [FK_ClientTicket]
    FOREIGN KEY ([ClientId])
    REFERENCES [dbo].[ClientSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientTicket'
CREATE INDEX [IX_FK_ClientTicket]
ON [dbo].[TicketSet]
    ([ClientId]);
GO

-- Creating foreign key on [EmployeeId] in table 'TicketSet'
ALTER TABLE [dbo].[TicketSet]
ADD CONSTRAINT [FK_EmployeeTicket]
    FOREIGN KEY ([EmployeeId])
    REFERENCES [dbo].[EmployeeSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeTicket'
CREATE INDEX [IX_FK_EmployeeTicket]
ON [dbo].[TicketSet]
    ([EmployeeId]);
GO

-- Creating foreign key on [StationId] in table 'RouteSet'
ALTER TABLE [dbo].[RouteSet]
ADD CONSTRAINT [FK_StationRoute]
    FOREIGN KEY ([StationId])
    REFERENCES [dbo].[StationSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StationRoute'
CREATE INDEX [IX_FK_StationRoute]
ON [dbo].[RouteSet]
    ([StationId]);
GO

-- Creating foreign key on [DepartureRouteId] in table 'TicketSet'
ALTER TABLE [dbo].[TicketSet]
ADD CONSTRAINT [FK_RouteTicket]
    FOREIGN KEY ([DepartureRouteId])
    REFERENCES [dbo].[RouteSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RouteTicket'
CREATE INDEX [IX_FK_RouteTicket]
ON [dbo].[TicketSet]
    ([DepartureRouteId]);
GO

-- Creating foreign key on [ArrivalRouteId] in table 'TicketSet'
ALTER TABLE [dbo].[TicketSet]
ADD CONSTRAINT [FK_RouteTicket1]
    FOREIGN KEY ([ArrivalRouteId])
    REFERENCES [dbo].[RouteSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RouteTicket1'
CREATE INDEX [IX_FK_RouteTicket1]
ON [dbo].[TicketSet]
    ([ArrivalRouteId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------