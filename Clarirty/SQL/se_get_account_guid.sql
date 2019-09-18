if exists (select * from sysobjects where name = 'se_get_account_guid')
begin
	drop procedure se_get_account_guid
	print 'PROCEDURE: se_get_account_guid dropped'
end
go

create procedure [dbo].[se_get_account_guid]--se_get_account_guid 199
(       
 @account_id numeric(10) = -1
 )      
as   
begin

select client_account_id from account_client_map 
where 
source_system_id 
in (select source_system_id from source_system_client_map where client_source_system_id = 'StatPro')
and 
account_id = 208--@account_id

end
go
if @@error = 0 print 'PROCEDURE: se_get_account_guid created'
else print 'PROCEDURE: se_get_account_guid error on creation'
go

--select * from source_system_client_map
--select * from account_client_map where source_system_id = 9

--OBC3TEJOKK	AGG01	35821690-f131-4834-8976-66f7e64b7657
--AC01	All Cap 01	19cd5b7b-0de1-48f9-b718-d7629a8b2bb2
--CLIENT001	Client001	585fd061-a2dc-4430-b3dc-e3accbd55cd0
--5VPB58NRUG	Client030	396fe9a6-9a9e-4097-b820-7c3a8599ed40
--FIAGG01	FI US Agg 01	841fecc3-1577-4060-acec-f1abe5a5ee4a
--3O1D71TSOV	NA Fixed Income	ee4942e6-5e2e-434c-806f-d4192af15481
--KC4J85RW5T	Unicorn Income Portfolio_ Statpro Import	e050686a-64d2-49a0-853d-cb5c3c158d77
--GSANHTNBIP	US Balanced Benchmark	6e7ff061-01a8-451e-8111-6704288fcac8
--R524SUOC0A	US Balanced Portfolio	59b7bfff-6eeb-49bd-940f-c5a884bbc305
--MI4TCRJT8R	US Equity	e4459ef1-cb00-4efd-9240-bce251193b5a
--MI4TCRJT8R	US Equity	8741bd65-4597-48ea-9c2e-a46c67f64fdf



