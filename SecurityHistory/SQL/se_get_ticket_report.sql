if exists (select * from sysobjects where name = 'se_get_ticket_report')  --select * from issuer where short_name like '%gener%'
begin
	drop procedure se_get_ticket_report
	print 'PROCEDURE: se_get_ticket_report dropped'    

end
go
create procedure se_get_ticket_report  --se_get_ticket_report 10366,-1,-1,'','01/01/2016','12/01/2019',189,-1

(	
    @account				numeric(10),
	
	@security				numeric(10),

	@issuer                  numeric(10),

	@search                  Varchar(40) = null,

	@trade_date_start			datetime = null,

	@trade_date_end				datetime = null,

	@userId					  	  smallint = 189,

	@m_intAssetCode                numeric(10) = -1,

	@broker                numeric(10) = -1
	

) 

as

	declare @ret_val int,
	@settle_date_start			datetime = null,
	@settle_date_end			datetime = null,
	@broker_id					numeric(10) = null,
	@include_confirmed_tickets 	tinyint = 1,
	@isAccountId                  smallint = 1,
    
	@trader_id 					numeric(10) = 1,
	@display_currency_id 		numeric(10) = null,
	@account_id            numeric(10) = null
	
	
begin

                        set nocount on;

                        declare @ec__errno int;

                        declare @sp_initial_trancount int;

                        declare @sp_trancount int;

-----------------------------------------------------  account ------------------------------------------------------------------
  --   if @account = 'All'
	 --   begin
		--    select @account_id = -1
		--end
  --   else
	 --   begin
		--   select @account_id = account_id from account where short_name  = @account
		--end

-----------------------------------------------------  security ------------------------------------------------------------------

--if @security = ''
--	    begin
--		    select @security_id = -1
--		end
--     else
--	    begin
--		   select @security_id = security_id from security where symbol  = @security
--		end

-----------------------------------------------------  issuer ------------------------------------------------------------------

--if @issuer = ''
--	    begin
--		    select @issuer_id = -1
--		end
--     else
--	    begin
--		   select @issuer_id = issuer_id from issuer where issuer_name  = @issuer  --select * from issuer where issuer_id = 484
--		end

------------------------------------------------------------------------------------------------------------------------




	 if @trade_date_start is null 
				 begin

		execute @ret_val = se_get_ticket_active_report 

	 	                   @trader_id = @trader_id,

	 	                   @display_currency_id = @display_currency_id,

	 	                   @include_confirmed_tickets = @include_confirmed_tickets,

	 	                   @account_id = @account,

	 	                   @security_id = @security,
							
							@search     = @search   ,
							
							@m_intAssetCode = @m_intAssetCode

	 	                   ;

	end else begin

	   	execute @ret_val = se_get_ticket_history_report 

	 	                      @trader_id = @trader_id,

	 	                      @display_currency_id = @display_currency_id,

	 	                      @trade_date_start = @trade_date_start,

	 	                      @trade_date_end = @trade_date_end,

	 	                      @include_confirmed_tickets = @include_confirmed_tickets,

	 	                      @account_id = @account,

							  @isAccountId  = @isAccountId,

							  @userId	 = @userId, 

	 	                      @security_id = @security,

	 	                      @issuer_id  = @issuer  ,

	 	                      @settle_date_start = @settle_date_start,

	 	                      @settle_date_end = @settle_date_end , 
							  
							  @search     = @search    ,
							  
							  @m_intAssetCode = @m_intAssetCode      

	 	                      ;


	end;

	return;	

end
