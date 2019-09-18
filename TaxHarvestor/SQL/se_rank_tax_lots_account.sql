if exists (select * from sysobjects where name = 'se_rank_tax_lots_account')
begin
	drop procedure se_rank_tax_lots_account
	print 'PROCEDURE: se_rank_tax_lots_account dropped'
end
go


create procedure  [dbo].[se_rank_tax_lots_account] 
--se_rank_tax_lots_account 206,'Largest Unrealized loss',-123000,'',0
--se_rank_tax_lots_account 10286,'Highest Cost Long Term',0,'ROP',0

(
       @account_id                       numeric(10), 
       @relief_method_decription         varchar(40),
       @Offsetgain                       float,
	   @security_search varchar(40) = '',
       @position_type                    numeric(10)= 0
		
)
as
       declare  @relief_method_code               numeric(10);
       declare       @priority int;
       declare       @tax_lot_id   int;
       declare @latest_price float;
       declare @one_year_in_past datetime;
       declare @ret_val int;
       declare @continue_flag            int;
       declare @cps_rank_tax_lots nvarchar(30);
       declare @cpe_rank_tax_lots nvarchar(30);
       declare @today datetime;
	   declare @DefaultDate datetime;
	   declare @stGL as float;
	   declare @ltgl as float;
	   declare @minLoss varchar(40) = 'Y';

begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;     
       select @continue_flag = 1
       select @today = getdate();
	   select @DefaultDate = dateadd(yy, -2, getdate())


         select  @relief_method_code = tax_lot_relief_method_code from tax_lot_relief_method where description = @relief_method_decription
       

CREATE TABLE #order_tax_lot_rank 
      (
          account_id numeric(10),
          security_id numeric(10),
       tax_lot_id numeric(10),
          trade_date date,
          quantity float,
          proposed_quantity numeric(10),
       rank numeric(10),
       client_tax_lot_id varchar(40),
       lot_number varchar(100),
          unit_cost float,
          price_latest float,
          gain  float,
		  MinLoss varchar(40),
		  IsRestricted varchar(40),
		  IsTaxable varchar(40),
		  WashSale   varchar(40)
       )

create table #sell_tax_lots(

              tax_lot_id                 numeric(10),
              quantity                   float ,
              proposed_quantity    float ,
              rank                       numeric(6) ,
              client_tax_lot_id    varchar(40),
              lot_number                 varchar(100)
       );

	create index idx01 on #sell_tax_lots ( tax_lot_id);
create table #account(account_id numeric(10)); -- Store all child level accounts;

      insert into #account(account_id)
         select ahm.child_id
           from account_hierarchy_map ahm
          where parent_id=@account_id
            and child_type=3;

select @stGL =
sum(Coalesce(account.user_field_15,0))
from account 
join #account on
account.account_id = #account.account_id

select @ltGL =
sum(Coalesce(account.user_field_16,0))
from account 
join #account on
account.account_id = #account.account_id

update statistics #account;

create table #selected_tax_lot(tax_lot_id numeric(10), account_id numeric(10), trade_date datetime, unit_cost float);
insert into #selected_tax_lot(tax_lot_id, account_id, trade_date, unit_cost)
select top (100)

tax_lot.tax_lot_id,
tax_lot.account_id, 
Coalesce(tax_lot.trade_date,@DefaultDate), 
Coalesce(tax_lot.unit_cost,tax_lot.unit_cost_base)
from  tax_lot 
join #account on ( tax_lot.account_id = #account.account_id)
  and tax_lot.position_type_code=@position_type
join security on
security.security_id = tax_lot.security_id
join se_major_asset_selected on
se_major_asset_selected.major_asset_code = security.major_asset_code
--join se_general_accounts on
--se_general_accounts.account_id = tax_lot.account_id

where 
	 (security.symbol like @security_search +'%'   or @security_search = '')
	 and se_major_asset_selected.selected = 1
	--and se_general_accounts.eligible = 1

	-- select * from #selected_tax_lot



       select @priority = 0;
       select @one_year_in_past = dateadd(yy, -1, getdate());

/* My code */

          if ( @relief_method_code is null) 
          begin
              select @relief_method_code = tax_lot_relief_method_code
            from account
                 where account_id=@account_id;
          end;
/* My code End */


       if (@relief_method_code = 1)
       begin
              return 0;
       end;
      -- delete from sell_tax_lots_rank where security_id = @security_id and Account_id = @account_id       
       if exists(select 1 from tax_lot_relief_method where tax_lot_relief_method_code = @relief_method_code) begin
              select @tax_lot_id = -1 ;
              WHILE (@tax_lot_id is not null)
              begin
                     select @priority = @priority + 1 ;
                     select @tax_lot_id = NULL ;
                     if (@relief_method_code = 2)
                     begin
select top (1)
                                  @tax_lot_id = tax_lot_id
                           from tax_lot, #account
                           where tax_lot.account_id = #account.account_id
                                  and position_type_code = @position_type
                                --  and security_id = @security_id
                                -- and tax_lot_id not in (select tax_lot_id from #sell_tax_lots)
								and tax_lot_id not in (select tax_lot_id from #sell_tax_lots)
                           order by unit_cost desc, trade_date asc;
                     end;   
                     if (@relief_method_code = 3)
                     begin
 select top (1)
                                  @tax_lot_id = tax_lot_id
                           from #selected_tax_lot 
                           where not exists ( select 1 from #sell_tax_lots where #sell_tax_lots.tax_lot_id = #selected_tax_lot.tax_lot_id)
                           order by trade_date desc, unit_cost desc;
                     end;   
                     if (@relief_method_code = 4)
                     begin
                         select top (1)
                                  @tax_lot_id = #selected_tax_lot.tax_lot_id
                           from #selected_tax_lot 
						    join tax_lot on 
		                     tax_lot.tax_lot_id = #selected_tax_lot.tax_lot_id
						     join price
                            on tax_lot.security_id = price.security_id
                           where not exists ( select 1 from #sell_tax_lots where #sell_tax_lots.tax_lot_id = #selected_tax_lot.tax_lot_id)
                           order by 
						
						  #selected_tax_lot.trade_date asc, #selected_tax_lot.unit_cost desc;
                     end;   
                     if (@relief_method_code = 5)
                     begin
 select top (1)
                                  @tax_lot_id = tax_lot_id
                           from #selected_tax_lot 
                           where not exists ( select 1 from #sell_tax_lots where #sell_tax_lots.tax_lot_id = #selected_tax_lot.tax_lot_id)
                           order by unit_cost asc, trade_date asc;
                     end;   
                     if (@relief_method_code = 6)
                     begin
 select top (1)
                                  @tax_lot_id = tax_lot_id
                           from #selected_tax_lot 
                           where not exists ( select 1 from #sell_tax_lots where #sell_tax_lots.tax_lot_id = #selected_tax_lot.tax_lot_id)
                                  and datediff(dd,  trade_date,  @one_year_in_past) >= 1
                           order by unit_cost asc, trade_date asc;
                     end;   
                     if (@relief_method_code = 7)
                     begin
 select top (1)
                                  @tax_lot_id = tax_lot_id
                           from #selected_tax_lot 
                           where not exists ( select 1 from #sell_tax_lots where #sell_tax_lots.tax_lot_id = #selected_tax_lot.tax_lot_id)
                                  and datediff(dd,  trade_date,  @one_year_in_past) >= 1
                           order by unit_cost desc, trade_date asc;
                     end;   
                     if (@relief_method_code = 8)
                     begin
 select top (1)
                                  @tax_lot_id = tax_lot_id
                           from #selected_tax_lot 
                           where not exists ( select 1 from #sell_tax_lots where #sell_tax_lots.tax_lot_id = #selected_tax_lot.tax_lot_id)
                                  and datediff(dd,  trade_date,  @one_year_in_past) < 1
                           order by unit_cost asc, trade_date asc;
                     end;   
                     if (@relief_method_code = 9)
                     begin
 select top (1)
                                  @tax_lot_id = tax_lot_id
                           from #selected_tax_lot 
                           where not exists ( select 1 from #sell_tax_lots where #sell_tax_lots.tax_lot_id = #selected_tax_lot.tax_lot_id)
                                  and datediff(dd,  trade_date,  @one_year_in_past) < 1
                     order by unit_cost desc, trade_date asc;
                     end;
                     if (@relief_method_code = 11)
         begin

         select top (1)
         @tax_lot_id = #selected_tax_lot.tax_lot_id
         from #selected_tax_lot 
         join account
         on #selected_tax_lot.account_id = account.account_id
              join #account 
               on account.account_id = #account.account_id
		  join tax_lot on 
		  tax_lot.tax_lot_id = #selected_tax_lot.tax_lot_id
         join price
          on tax_lot.security_id = price.security_id
          join security
          on tax_lot.security_id = security.security_id
          join currency
          on security.principal_currency_id = currency.security_id
          where tax_lot.position_type_code = @position_type
         -- and tax_lot.security_id = @security_id
          and not exists ( select 1 from #sell_tax_lots where #sell_tax_lots.tax_lot_id = #selected_tax_lot.tax_lot_id)
         order by case
                     when position_type_code = 0 then (price.latest - coalesce(tax_lot.unit_cost, 0.0))
                     else (coalesce(tax_lot.unit_cost, 0.0) - price.latest)
                     end 
                           
                     * case 
                           when datediff(dd,  #selected_tax_lot.trade_date,  @today) < 365 then coalesce(account.user_field_13, 0.35)
                           else coalesce(account.user_field_14, 0.15)
                       end
                     
end;
              if (@relief_method_code = 12)
         begin

          select top (1)
         @tax_lot_id = #selected_tax_lot.tax_lot_id
         from #selected_tax_lot 
         join account
         on #selected_tax_lot.account_id = account.account_id
              join #account 
               on account.account_id = #account.account_id
		  join tax_lot on 
		  tax_lot.tax_lot_id = #selected_tax_lot.tax_lot_id
         join price
          on tax_lot.security_id = price.security_id
          join security
          on tax_lot.security_id = security.security_id
          join currency
          on security.principal_currency_id = currency.security_id
          where tax_lot.position_type_code = @position_type
         -- and tax_lot.security_id = @security_id
          and not exists ( select 1 from #sell_tax_lots where #sell_tax_lots.tax_lot_id = #selected_tax_lot.tax_lot_id)
         order by 
               
                case
                           when position_type_code = 0 then 
                           
                                     case 
                                                when (coalesce(tax_lot.unit_cost, 0.0) - price.latest) < 1 then 1
                                                   else -1
                                           end
                           else 
                                   case 
                                              when ( price.latest-coalesce(tax_lot.unit_cost, 0.0)) < 1 then 1
                                                else -1

                                         end
                 end,
              datediff(dd,  #selected_tax_lot.trade_date,  @today)asc
end;

                     if (@relief_method_code = 13)
         begin

         select top (1)
         @tax_lot_id = #selected_tax_lot.tax_lot_id
         from #selected_tax_lot 
         join account
         on #selected_tax_lot.account_id = account.account_id
              join #account 
               on account.account_id = #account.account_id
		  join tax_lot on 
		  tax_lot.tax_lot_id = #selected_tax_lot.tax_lot_id
         join price
          on tax_lot.security_id = price.security_id
          join security
          on tax_lot.security_id = security.security_id
          join currency
          on security.principal_currency_id = currency.security_id
          where tax_lot.position_type_code = @position_type
         -- and tax_lot.security_id = @security_id
          and not exists ( select 1 from #sell_tax_lots where #sell_tax_lots.tax_lot_id = #selected_tax_lot.tax_lot_id)
         order by case
                     when position_type_code = 0 then (price.latest - coalesce(tax_lot.unit_cost, 0.0))
                     else (coalesce(tax_lot.unit_cost, 0.0) - price.latest)
                     end 
                           
                     * case 
                           when datediff(dd,  #selected_tax_lot.trade_date,  @today) < 365 then coalesce(account.user_field_13, 0.35)
                           else coalesce(account.user_field_14, 0.15)
                       end desc,
                       datediff(dd,  #selected_tax_lot.trade_date,  @today)asc
					   end
	     if (@relief_method_code = 14)
                     begin
                         select top (1)
                                  @tax_lot_id = #selected_tax_lot.tax_lot_id
                           from #selected_tax_lot 
						    join tax_lot on 
		                     tax_lot.tax_lot_id = #selected_tax_lot.tax_lot_id
						     join price
                            on tax_lot.security_id = price.security_id
                           where not exists ( select 1 from #sell_tax_lots where #sell_tax_lots.tax_lot_id = #selected_tax_lot.tax_lot_id)
                           order by 
						   (quantity*(price.latest - coalesce(tax_lot.unit_cost, 0.0)))
						   --,#selected_tax_lot.trade_date asc, #selected_tax_lot.unit_cost desc;
                     end;   
                     

       

                      insert into #sell_tax_lots
                     (      tax_lot_id,
                           quantity,
                           proposed_quantity,
                           rank,
                           client_tax_lot_id,
                           lot_number
                     )
                     select distinct tax_lot.tax_lot_id, 
                           tax_lot.quantity - coalesce((select      sum(order_tax_lot.quantity - order_tax_lot.quantity_confirmed)
                                                                                         from       order_tax_lot
                                                                                         where       order_tax_lot.tax_lot_id = tax_lot.tax_lot_id
                                                                                         and           order_tax_lot.deleted = 0), 0.0), 
                           null,
                           @priority,
                            tax_lot.client_tax_lot_id,
                           tax_lot.lot_number
                     from tax_lot
                     where tax_lot.tax_lot_id = @tax_lot_id;


                        select @ec__errno = @@error;
                           if @ec__errno != 0
                           begin
                                  if @sp_initial_trancount = 0
                                      rollback transaction;
                          return @ec__errno;
                           end;
           
                           

              

       
--          
       end;
       ----------------------------------------------for taxlot detail to see disposal level-----------------------------------------------

	  
	  
	   insert into #order_tax_lot_rank
       (
          account_id,
          security_id,
          tax_lot_id,
          trade_date,
          quantity,
          proposed_quantity,
          rank,
          client_tax_lot_id,
           lot_number,
          unit_cost,
          price_latest,
          gain,
		  MinLoss,
		  IsRestricted,
		  IsTaxable,
		  WashSale

       )
       select
             tax_lot.account_id,
                tax_lot.security_id,
                tax_lot.tax_lot_id,
                     tax_lot.trade_date,
             tax_lot.quantity,
             0,
             rank,
             tax_lot.client_tax_lot_id,
             tax_lot.lot_number,
                     coalesce(tax_lot.unit_cost,tax_lot.unit_cost_base),
                     price.latest,
                     tax_lot.quantity*(price.latest- coalesce(tax_lot.unit_cost,tax_lot.unit_cost_base)),
		 -- @minLoss
		  case when
		 Sum(tax_lot.quantity* (latest - unit_cost)) * security.principal_factor * pricing_factor/ currency.exchange_rate    < -500  then 'Y'
		 else
		  'N'
		  end as Minloss,
		  case 
				when 
					 list_decimal_value = tax_lot.security_id then 'Y'
					else
					'N'
				end as 'IsRestricted',
	     case 
		      when account.user_field_12 = 'Y' then 'Y'
				else
				 'N'
				 end as 'IsTaxable',
		 'N'
          from #sell_tax_lots, tax_lot, price, security,account,currency
	 
	   join list on
	   list.list_id in (select list_id from list where list_name = 'IPC Sell Restring list')
	   join list_member on
       list.list_id = list_member.list_id
	
       where #sell_tax_lots.tax_lot_id = tax_lot.tax_lot_id
		 
              and tax_lot.security_id=price.security_id
              and tax_lot.security_id = security.security_id
			  and tax_lot.account_id = account.account_id
			  and security.principal_currency_id = currency.security_id 
              and security.major_asset_code <> 0
			  and (security.symbol like @security_search +'%'   or @security_search = '')
	   group by tax_lot.account_id,tax_lot.security_id,tax_lot.tax_lot_id,tax_lot.trade_date,tax_lot.quantity,#sell_tax_lots.rank,tax_lot.client_tax_lot_id,
	   tax_lot.lot_number,tax_lot.unit_cost,tax_lot.unit_cost_base,price.latest,security.principal_factor,
	   security.pricing_factor,currency.exchange_rate,list_member.list_decimal_value,account.user_field_12
         order by rank;

		 --se_rank_tax_lots_account 208,'Largest Unrealized loss',-123000,'',0
   declare @rec_index numeric(10);
   declare @max_index numeric(10);
   declare @amt float(10);
   declare @qty numeric(10);

   print 2001;

if @Offsetgain >0 
begin
  select   @rec_index = min(rank), 
           @max_index = max(rank)
     from #order_tax_lot_rank
       where gain > 0;

   while ( @Offsetgain > 0  and @rec_index <= @max_index )
   begin

   print 2002;

      print @rec_index
       if exists ( select 1 from #order_tax_lot_rank
                                where rank = @rec_index
                                  and gain > 0 )
          begin
                 select @amt = gain,
                             @qty = quantity,
							 @minLoss = 'N'
                     from #order_tax_lot_rank
                     where rank = @rec_index
                       and gain > 0;

                   print 2003;

                 if (@Offsetgain >= @amt )
                              begin
                              update #order_tax_lot_rank
                                    set proposed_quantity = quantity
                                  where rank=@rec_index
                                    and gain > 0
									and @minLoss = 'Y'
									and IsRestricted = 'N'
									and IsTaxable = 'Y'
									and WashSale = 'N';
                                  set @Offsetgain = @Offsetgain - @amt;


                   print 2004;


                                  print @Offsetgain
                             end;
                     
                 else
                             begin
                                  update #order_tax_lot_rank
                                    set proposed_quantity = (@Offsetgain/gain)*quantity
                                  where rank=@rec_index
                                     and gain >0
									 and @minLoss = 'Y' 
									 and IsRestricted = 'N'
									 and IsTaxable = 'Y'
									and WashSale = 'N';;
                                  set  @Offsetgain = 0;


                     print 2005;



                                  print @Offsetgain
                             end;
          end;
       set @rec_index = @rec_index +1
   end;
end
else
begin
                  print 2006;


  select   @rec_index = min(rank), 
           @max_index = max(rank)
     from #order_tax_lot_rank
       where gain < 0;

               print 2007;


while ( @Offsetgain < 0  and @rec_index <= @max_index )

   begin
   
       if exists ( select 1 from #order_tax_lot_rank
                                where rank = @rec_index
                                  and gain < 0 )
              begin
       select @amt = gain,
                 @qty = quantity
            from #order_tax_lot_rank
              where rank = @rec_index
                and gain <0;

          if (@Offsetgain >=  @amt )
              begin
           update #order_tax_lot_rank
                 set proposed_quantity = (@Offsetgain/gain)*quantity
                     where rank=@rec_index
                       and gain < 0
					   and MinLoss = 'Y'
					   and IsRestricted = 'N'
					   and IsTaxable = 'Y'
					   and WashSale = 'N';;
                     set @Offsetgain = 0;
             end;
          else
             begin
                  update #order_tax_lot_rank
                 set proposed_quantity = quantity
                     where rank=@rec_index
                        and gain < 0 
						and MinLoss = 'Y'
						and IsRestricted = 'N'
						and IsTaxable = 'Y'
						and WashSale = 'N';
                     set  @Offsetgain = @Offsetgain + abs(@amt);
                     print @offsetgain;
                end;
          end;
       set @rec_index = @rec_index + 1;

   end;
end;


               print 2008;


   select
       #order_tax_lot_rank.account_id,
          account.short_name,
		  Coalesce(@stgl,0) as 'ST Realized Gain',
		  Coalesce(@ltgl,0) as 'LT Realized Gain',
          #order_tax_lot_rank.security_id,
          security.symbol,
          tax_lot_relief_method.description as method,
          tax_lot_id,
          Coalesce(trade_date,@DefaultDate) as trade_date,
          case when DATEDIFF(d,trade_date,getdate()) > 365 then 'LT'
          else 'ST'
          end term ,

          case when DATEDIFF(d,trade_date,getdate()) > 365 then 0
          else  365-DATEDIFF(d,trade_date,getdate())
          end DTL,

          Coalesce(quantity,0) as available_qty,
          Coalesce(proposed_quantity,0) as selected_qty,
          rank,
          Coalesce(client_tax_lot_id,'')as client_tax_lot_id, 
          Coalesce(lot_number,'')as lot_number,
          Coalesce(unit_cost,0) as unit_cost,
          Coalesce(price_latest,0) as price_latest,
          round(Coalesce(gain,0),2) as gain,
          round( (price_latest/case when coalesce(unit_cost,0) = 0 then price_latest else coalesce(unit_cost,0) end -1)*100, 2)  pct,
		  #order_tax_lot_rank.MinLoss,
		  #order_tax_lot_rank.IsRestricted,
		  #order_tax_lot_rank.IsTaxable,
		  #order_tax_lot_rank.WashSale
   from #order_tax_lot_rank, 
        tax_lot_relief_method,
              account,
              security
   where tax_lot_relief_method.tax_lot_relief_method_code=@relief_method_code
   and #order_tax_lot_rank.account_id = account.account_id
   and #order_tax_lot_rank.security_id = security.security_id
   order by rank;

                  print 2009;
end	
END

go
if @@error = 0 print 'PROCEDURE: se_rank_tax_lots_account created'
else print 'PROCEDURE: se_rank_tax_lots_account error on creation'
go