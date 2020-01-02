  
  --select * from security

  --update security 
  --set symbol = 'PCASH_RED',
  --modified_by =172
  --where security_id = 67433

  --update security 
  --set symbol = 'PCASH_SUB',
  --modified_by =172
  --where security_id = 67434

  --select * from positions where account_id = 11582

if exists (select * from sysobjects where name = 'se_add_internal_mf_cash')
begin
	drop procedure se_add_internal_mf_cash
	print 'PROCEDURE: se_add_internal_mf_cash dropped'
end
go

CREATE procedure [dbo].[se_add_internal_mf_cash]--se_add_internal_mf_cash 'FSMDX',20000
(       
 @short_name      varchar(40),
 @cash_amount float = 1
 
 )      
as  
declare

@Cash_id numeric(10),
@subs numeric(10),
@account_id numeric(10),
@current_cash float,
@total_cash float
begin 

select @account_id = account_id from account where short_name = @short_name 


if (@cash_amount < 0)
   begin
       select @Cash_id = security_id from security where symbol = 'PCASH_RED'
	   select @current_cash = quantity from positions where security_id = @Cash_id and account_id = @account_id 
	   select @total_cash = coalesce(@current_cash,0) + coalesce( @cash_amount,0)
   end
else
  begin
  select @Cash_id = security_id from security where symbol = 'PCASH_SUB'
   select @current_cash = quantity from positions where security_id = @Cash_id and account_id = @account_id 
   select @total_cash = coalesce(@current_cash,0) + coalesce( @cash_amount,0)
  end
            
			--exec clear_added_cash @account_id, 172.00000000, 1, 1
	exec update_cash_position 0, @account_id, @Cash_id , @total_cash, 0.00000000, @total_cash, @total_cash, null, 1.00000000, 1.00000000, 47, 172.00000000
				

end
go
if @@error = 0 print 'PROCEDURE: se_add_internal_mf_cash created'
else print 'PROCEDURE: se_add_internal_mf_cash error on creation'
go


--update se_drift_summary
--set sector_drift = 'N',
--security_drift = 'N'