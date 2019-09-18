

/* 
 face value is assumed to be 100
 payment_frquency : 0 as zero coupon,
					1 annual,
					2 semi-annual,
					4- quarterly
*/

create procedure calculate_cf_schedule(@security_id numeric(10), @payment_frequency tinyint) as  -- exec calculate_cf_schedule 8281,2
declare
 @maturity_date date,
 @annual_coupon float,
 @coupon        float,
 @last_coupon_payment date,
 @payment_period_in_months tinyint,
 @date_projected date;
begin

 set @date_projected = getdate();

 select @maturity_date= maturity_date,
        @annual_coupon = coupon 
   from security
  where security_id = @security_id;

  -- initialize
  delete cf_schedule where security_id=@security_id and date_projected=@date_projected;
  
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



