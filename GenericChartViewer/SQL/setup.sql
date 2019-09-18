go
begin
	declare @ret_val int;
	declare @message_count int;
	declare	@tran_count numeric(10);
	begin
		--execute add_service_library  'Linedata.Server.Widget.AccountQuery.dll', 1;	
		--execute add_service_library  'Linedata.Server.Widget.AccountSummary.dll', 1;
		--execute add_service_library  'Linedata.Server.Widget.PortfolioBreakdown.dll', 1;	
		--execute add_service_library  'Linedata.Server.Widget.Common.dll', 1;
		execute add_service_library  'DashBoard\HierarchyViewerAddIn.Server.AddInDetailServiceTypes.dll', 1;
		execute add_service_library  'DashBoard\HierarchyViewerAddIn.Shared.ServiceContracts.dll', 1;

	end;
end;
go
if @@error = 0 print 'DATA: service_library updated'
else print 'DATA: service_library error on update'
go

select * from service_library