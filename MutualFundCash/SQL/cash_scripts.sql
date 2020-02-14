declare 
@account_id numeric(10),
@Sub_Cash_id numeric(10),
@Red_Cash_id numeric(10),
@Sub_cash float,
@Red_cash float
select @Sub_Cash_id = security_id from security where symbol = 'PCASH_SUB'
   select @Red_Cash_id = security_id from security where symbol = 'PCASH_Red'

--fof1
set @account_id = 11333
set @Sub_cash = 1000
exec update_cash_position 0,  @account_id, @Sub_Cash_id , @Sub_cash, 0.00000000, @Sub_cash, @Sub_cash, null, 1.00000000, 1.00000000, 47, 172.00000000
set @red_cash = -1000
exec update_cash_position 0,  @account_id, @Red_Cash_id , @Red_cash, 0.00000000, @Red_cash, @Red_cash, null, 1.00000000, 1.00000000, 47, 172.00000000

--fof2
set @account_id = 11334
set @Sub_cash = 1000
exec update_cash_position 0, @account_id, @Sub_Cash_id , @Sub_cash, 0.00000000, @Sub_cash, @Sub_cash, null, 1.00000000, 1.00000000, 47, 172.00000000
set @red_cash = -1000
exec update_cash_position 0, @account_id, @Red_Cash_id , @Red_cash, 0.00000000, @Red_cash, @Red_cash, null, 1.00000000, 1.00000000, 47, 172.00000000

--fof3
set @account_id = 11335
set @Sub_cash = 1000
exec update_cash_position 0, @account_id, @Sub_Cash_id , @Sub_cash, 0.00000000, @Sub_cash, @Sub_cash, null, 1.00000000, 1.00000000, 47, 172.00000000
set @red_cash = -1000
exec update_cash_position 0, @account_id, @Red_Cash_id , @Red_cash, 0.00000000, @Red_cash, @Red_cash, null, 1.00000000, 1.00000000, 47, 172.00000000

--fof4
set @account_id = 11336
set @Sub_cash = 1000
exec update_cash_position 0, @account_id, @Sub_Cash_id , @Sub_cash, 0.00000000, @Sub_cash, @Sub_cash, null, 1.00000000, 1.00000000, 47, 172.00000000
set @red_cash = -1000
exec update_cash_position 0, @account_id, @Red_Cash_id , @Red_cash, 0.00000000, @Red_cash, @Red_cash, null, 1.00000000, 1.00000000, 47, 172.00000000

--fof5
set @account_id = 11337
set @Sub_cash = 1000
exec update_cash_position 0, @account_id, @Sub_Cash_id , @Sub_cash, 0.00000000, @Sub_cash, @Sub_cash, null, 1.00000000, 1.00000000, 47, 172.00000000
set @red_cash = -1000
exec update_cash_position 0, @account_id, @Red_Cash_id , @Red_cash, 0.00000000, @Red_cash, @Red_cash, null, 1.00000000, 1.00000000, 47, 172.00000000
