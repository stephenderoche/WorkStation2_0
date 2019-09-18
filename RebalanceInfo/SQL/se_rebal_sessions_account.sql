if exists (select * from sysobjects where name = 'se_rebal_sessions_account')
begin
	drop procedure se_rebal_sessions_account
	print 'PROCEDURE: se_rebal_sessions_account dropped'
end
go


create procedure  [dbo].[se_rebal_sessions_account] --se_rebal_sessions_account 3

(
       @session_id                       numeric(10)
    
		
)
as
    
	 select 
	 account.short_name,
	 hierarchy.name as hierarchy_name,
	 model.name,
	 round(rebal_audit_account.market_value_before,2) mv_before,
	 round(rebal_audit_account.market_value_after,2) mv_after,
	 round(rebal_audit_account.net_cash_change,2) net_cash_change,
	 rebal_audit_account.holdings_qty_after,
	 rebal_audit_account.holdings_qty_before,
	 rebal_audit_account.orders_qty_generated,
	 user_info.name as manager,
	 case when
	      rebal_audit_account.excluded_mv_encumbered_pos = 0 then 'N'
	else 'Y'
	end as excluded_mv_encumbered_pos,

	 case when
	      rebal_audit_account.excluded_mv_exclusion_list = 0 then 'N'
	else 'Y'
	end as excluded_mv_exclusion_list,
	  case when
	      rebal_audit_account.excluded_mv_restricted_sec = 0 then 'N'
	else 'Y'
	end as excluded_mv_restricted_sec,
	   case when
	      rebal_audit_account.is_missing_model_security = 0 then 'N'
	else 'Y'
	end as is_missing_model_security,
	  case when
	      rebal_audit_account.is_no_orders_min_share = 0 then 'N'
	else 'Y'
	end as is_no_orders_min_share,
	 case when
	      rebal_audit_account.is_no_orders_zero_rebal_mv = 0 then 'N'
	else 'Y'
	end as is_no_orders_zero_rebal_mv,
	 case when
	      rebal_audit_account.is_no_orders_restricted = 0 then 'N'
	else 'Y'
	end as is_no_orders_restricted

	 from rebal_audit_account 
	 join account on 
	 account.account_id = rebal_audit_account.account_id
	 join hierarchy on
	 hierarchy.hierarchy_id = rebal_audit_account.hierarchy_id
	 join model on 
	 model.model_id = account.default_model_id
	 join User_info
	 on user_info.user_id = account.manager
	 where rebal_session_id = @session_id

go
if @@error = 0 print 'PROCEDURE: se_rebal_sessions_account created'
else print 'PROCEDURE: se_rebal_sessions_account error on creation'
go