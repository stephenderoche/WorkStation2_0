drop Table se_yield_curve

create table se_yield_curve ([yield_curve_id] [numeric](10, 0) IDENTITY(1,1) NOT NULL,name varchar(40),one_month float,three_month float,six_month float,one_year float,two_year float,three_year float,five_year float,
seven_year float,ten_year float,twenty_year float, thirty_year float);

insert into se_yield_curve values ('Yield Curve',2.18,2.23,2.40,2.60,2.82,2.90,2.96,3.04,3.09,3.18,3.24)
insert into se_yield_curve values ('2017 Yield Curve',1.28,1.39,1.53,1.76,1.89,1.98,2.20,2.33,2.40,2.58,2.74)
insert into se_yield_curve values ('2016 Yield Curve',0.44,0.51,0.62,0.85,1.20,1.47,1.93,2.25,2.45,2.79,3.06)
insert into se_yield_curve values ('2015 Yield Curve',0.14,0.16,0.49,0.65,1.06,1.31,1.76,2.09,2.27,2.67,3.01)

select * from se_yield_curve