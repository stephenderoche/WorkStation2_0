--drop table cf_schedule
/*create table cf_schedule(date_projected date default getdate(), security_id numeric(10), cf_year numeric(4), cf_month numeric(2), cf_date date, payment numeric(10,4), payment_to_principal numeric(10,4),
                          payment_to_interest numeric(10,4), outstanding_loan numeric(10,4));
*/
/* 
 face value is assumed to be 100
 payment_frquency : 0 as zero coupon,
					1 annual,
					2 semi-annual,
					4- quarterly,
					12 - monthly
*/

if exists (select * from sysobjects where name = 'calculate_cf_schedule')
begin
	drop procedure calculate_cf_schedule
	print 'PROCEDURE: calculate_cf_schedule dropped'
end
go

create  procedure calculate_cf_schedule(@security_id numeric(10), @payment_frequency tinyint) as
declare
 @maturity_date date,
 @annual_coupon float,
 @coupon        float,
 @period_interest_rate float,

 @period_factor  float,
 @last_coupon_payment date,
 @payment_period_in_months tinyint,
 @minor_asset_code tinyint,
 @number_of_periods numeric(10),
 @issue_date date,
 @last_payment date,
 @next_payment date,
 @outstanding_loan float,
 @payment_to_principal float,
 @payment_to_interest  float,
 @principal_factor float,
 @principal_factor_date date,
 @date_projected date;
begin

 set @date_projected = getdate();

 select @maturity_date    = maturity_date,
        @annual_coupon    = coupon,
		@minor_asset_code = minor_asset_code,
		@issue_date       = issue_date,
		@payment_frequency = payment_frequency,
		@principal_factor   = principal_factor,
		@principal_factor_date = principal_factor_date
		
   from security
  where security_id = @security_id;

  -- initialize
  delete cf_schedule where security_id=@security_id and date_projected=@date_projected;
  if @minor_asset_code = 25  -- Coupon bond
  begin
		  -- insert the face value 
		   insert 
			 into cf_schedule(date_projected, security_id, cf_year, cf_month, cf_date, payment)
			 values(@date_projected, @security_id, DATEPART(year,@maturity_date), datepart(month,@maturity_date),
					@maturity_date, 100);

		  -- insert coupons
		  set @coupon              = @annual_coupon/@payment_frequency;
		  if @payment_frequency >0
			begin
				set @payment_period_in_months = 12/@payment_frequency;
				set @coupon                   = @annual_coupon/@payment_frequency;
			end;
		  set @last_coupon_payment =  @maturity_date;

		  while (@last_coupon_payment >= GETDATE()) and coalesce(@payment_frequency,0) >0 
  
		  begin
     
			 insert 
			   into cf_schedule(date_projected, security_id, cf_year, cf_month, cf_date, payment)
			   values(getdate(),@security_id,datepart(year, @last_coupon_payment), datepart(month,@last_coupon_payment),@last_coupon_payment,@coupon);
	         

			 set @last_coupon_payment = DATEADD(month,-@payment_period_in_months,@last_coupon_payment);
		  end;
   end;

   if (@minor_asset_code = 51)     -- MBS/ABS : Amortized debt

   begin
     -- Outstanding loan is assumed to be $100

     set @period_interest_rate =  @annual_coupon / (@payment_frequency*100);    -- Convert into fraction
	 set @number_of_periods    =  datediff(month,@issue_date,@maturity_date)/(12/@payment_frequency);   -- Number of payments in month; 360 for 30 years home loan
	 -- Payment Factor = r(1+r)^n/( ( 1+r)^n -1)
	 set @outstanding_loan        = 100;
	 set @period_factor        = @outstanding_loan*(@period_interest_rate*power(1+@period_interest_rate,@number_of_periods))/ (  power(1+@period_interest_rate,@number_of_periods) -1);
		
     -- Create schedule 
	 if @principal_factor_date is null 
	    begin
			set @next_payment   =  dateadd(month,12/@payment_frequency,@issue_date);
		end;
	 else
		begin
		   set @next_payment   =  dateadd(month,12/@payment_frequency,@issue_date);
		   while @next_payment  < @principal_factor_date
		      begin
				 set @next_payment = dateadd(month,12/@payment_frequency,@next_payment);
			  end;
		end;


	 set @outstanding_loan = @outstanding_loan*@principal_factor;

	 while @outstanding_loan > 0
	 begin
	     
	    
		 set @payment_to_interest  = @outstanding_loan*@period_interest_rate;
         set @payment_to_principal = @period_factor - @payment_to_interest;
		
	     if @outstanding_loan - @payment_to_principal > 0
		    begin
			 set @outstanding_loan     = @outstanding_loan - @payment_to_principal;
			insert 
				into cf_schedule(date_projected, security_id, cf_year, cf_month, cf_date, payment, payment_to_principal, payment_to_interest, outstanding_loan)
				values(@date_projected, @security_id, DATEPART(year,@next_payment), datepart(month,@next_payment),
					@next_payment, @period_factor, @payment_to_principal, @payment_to_interest, @outstanding_loan  );
			end;
		 else
			  begin 
				 set @payment_to_interest  = 0;
				 set @payment_to_principal = @outstanding_loan;
				 set @outstanding_loan     = 0;

				insert 
					into cf_schedule(date_projected, security_id, cf_year, cf_month, cf_date, payment, payment_to_principal, payment_to_interest, outstanding_loan)
					values(@date_projected, @security_id, DATEPART(year,@next_payment), datepart(month,@next_payment),
							@next_payment, @payment_to_principal, @payment_to_principal, @payment_to_interest, 0  );
				end;
		  

		 set @next_payment = dateadd(month,12/@payment_frequency,@next_payment);
	    
	 end;

   end;

end;

go
if @@error = 0 print 'PROCEDURE: calculate_cf_schedule created'
else print 'PROCEDURE: calculate_cf_schedule error on creation'
go


/*
 test 
 exec calculate_cf_schedule	200534,12


 select security_id,name_1,issue_date,minor_asset_code, maturity_date,payment_frequency, issue_date, maturity_date,datediff(month,issue_date,maturity_date) months, coupon ,
        principal_factor
from security 
where minor_asset_code in ( 25,51) and maturity_date >getdate()
--and payment_frequency =12
and security_id=341995

exec calculate_cf_schedule	9821,12

select * from cf_schedule
where security_id=9821
order by 1,2,3,4
*/

--update security set principal_factor=.6, principal_factor_date=getdate() ,modified_by=1 where  security_id=9821;


--select 
--security_id,name_1,issue_date,minor_asset_code, maturity_date,payment_frequency, issue_date, maturity_date,datediff(month,issue_date,maturity_date) months, coupon , principal_factor,principal_factor_date
-- principal_factor_date 
--from security 
--where minor_asset_code in ( 25,51) and maturity_date >getdate()
--and security_id=9821