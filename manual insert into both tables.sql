Insert into opskrifter(navn, fremgangsmåde,tid, billede)
values('','',0,'')

INSERT INTO ingredienser(ingredienserId, ing1, ing2, ing3, ing4, ing5,ing6,ing7)
VALUES ((SELECT opskrifterId FROM opskrifter WHERE navn = ),'', '', '', '','', '', ''); 

--ALTER TABLE opskrifter 
--ADD billede varchar(200);

--UPDATE opskrifter
--SET billede = '--Link Til Billede--'
--where opskrifterID = 0