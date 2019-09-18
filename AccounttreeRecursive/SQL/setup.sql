

go
begin
	declare @ret_val int;
	declare @message_count int;
	declare	@tran_count numeric(10);
	begin
	
		execute add_service_library  'Linedata.Server.Workstation.Api.dll', 1;
		execute add_service_library  'Dashboard\SalesSharedContracts.Types.dll', 1;
		execute add_service_library  'Dashboard\SalesSharedContracts.dll', 1;

	end;
end;
go
if @@error = 0 print 'DATA: service_library updated'
else print 'DATA: service_library error on update'
go