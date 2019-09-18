if exists (select * from sysobjects where name = 'se_rebal_sessions')
begin
	drop procedure se_rebal_sessions
	print 'PROCEDURE: se_rebal_sessions dropped'
end
go


create procedure  [dbo].[se_rebal_sessions] --se_rebal_sessions -1
(
 @session_id numeric(10) = -1
)


as
    

	declare @number_of_accounts_proposed numeric(10);
	declare @number_of_accounts_orders numeric(10);
	select @number_of_accounts_proposed = count(distinct account_id) from proposed_orders where proposed_orders.rebal_session_id = @session_id
	  --select @number_of_accounts_orders = count(distinct account_id) from orders where orders.rebal_session_id = @session_id                           

	select 
	CONVERT(varchar, rebal_session_id)+ ' - ' + model_name + ' - ' + CONVERT(varchar, created_time) as session,
	rebal_session.rebal_session_id,
	rebal_session.model_id,
	rebal_session.model_name,
	rebal_session.owner_id,
	user_info.name,
	rebal_status.description,
	rebal_session.created_time as completion_date,
	case when sell_non_model_holdings = 0 then 'N'
	else 'Y' end  as'Sell non model Holdings',
	case when exclude_encumbered = 0 then 'N'
	else 'Y' end  as'Exclude Encumbered',
	case when include_accrued_interest = 0 then 'N'
	else 'Y' end  as'include_accrued_interest',
	case when clear_proposed = 0 then 'N'
	else 'Y' end  as'clear_proposed',
	case when normalize_targets = 0 then 'N'
	else 'Y' end  as'Normalize Targets',
	@number_of_accounts_proposed  as'prevent_over_adjustment',
	case when redistribute_disqualified_mv = 0 then 'N'
	else 'Y' end  as'redistribute_disqualified_mv',
	case when prevent_negative_cash = 1 and ignore_existing_cash =1  then 'Only Sales Proceeds'
	when prevent_negative_cash = 1 and ignore_existing_cash =0  then 'Existing and Sales Cash'
	else 'Allow Negative Cash ' end  as'prevent_negative_cash',
	case when ignore_existing_cash = 0 then 'N'
	else 'Y' end  as'ignore_existing_cash',
	case when sell_off_odd_lots = 0 then 'N'
	else 'Y' end  as'sell_off_odd_lots',
	case when exclude_unclass_for_min = 0 then 'N'
	else 'Y' end  as'exclude_unclass_for_min',
	minimum_quantity as'minimum_quantity',
	rounding_quantity as'rounding_quantity',
	case when restlist_processing  = NULL then 'Ignore List'
	     when restlist_processing  = 1 then 'Exclude Restricted list'
		 when restlist_processing  = 2 then 'Redistribute Restricted list'
	else 'Exclude Acct If Restricted Securities' end  as'Restricted list Processing',
	case when restlist_error_level  = 1 then 'Warning'
	     when restlist_error_level  = 2 then '1st Overdide'
		 when restlist_error_level  = 3 then '2nd Overdide'
	else 'Hard' end  as'Severity Level',
	case when order_direction_code  = 1 then 'Buy Or Sell'
	     when order_direction_code  = 2 then 'Sell only'
		 else 'Buy Only' end  as'Order Direction',
		case when price_field_code  = 0 then 'Latest'
	     when price_field_code  = 1 then 'Bid'
		 else 'Offer' end  as 'Price Type'

	from rebal_session
	join user_info on
	user_info.user_id = rebal_session.owner_id
	join rebal_status on
	rebal_status.rebal_status_code = rebal_session.rebal_status_code
	where (@session_id = -1 or @session_id = rebal_session_id)
	order by rebal_session.created_time desc

go
if @@error = 0 print 'PROCEDURE: se_rebal_sessions created'
else print 'PROCEDURE: se_rebal_sessions error on creation'
go





--select * from rebal_session where rebal_session_id =60