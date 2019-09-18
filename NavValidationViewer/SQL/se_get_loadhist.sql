if exists (select * from sysobjects where name = 'se_get_loadhist')
begin
	drop procedure se_get_loadhist
	print 'PROCEDURE: se_get_loadhist dropped'
end
go

create procedure se_get_loadhist

as
begin
select * from loadhist_definition where loadhist_definition_type_code = 1
end
go
if @@error = 0 print 'PROCEDURE: se_get_loadhist created'
else print 'PROCEDURE: se_get_loadhist error on creation'
go

