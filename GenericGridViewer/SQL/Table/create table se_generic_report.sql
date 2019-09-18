	
	
	  create table se_generic_report   
(    
  showAccount tinyint,
  showDesk tinyint,
  showSecurity tinyint,
  showStartDate tinyint,
  showEndDate tinyint,
  showBlockID tinyint,
  Report varchar(40)
  
 );  

 truncate table se_generic_report

       insert into se_generic_report values(1,0,0,0,0,0,'Cash Balance')
       insert into se_generic_report values(1,0,0,0,0,0,'Account Compare')
       insert into se_generic_report values(1,1,1,1,1,0,'Security History')
       insert into se_generic_report values(1,0,0,0,0,0,'Top Issuers')
       insert into se_generic_report values(1,0,0,0,0,0,'Top Securities')
	   insert into se_generic_report values(1,0,0,0,0,0,'Wash Sales')
	   insert into se_generic_report values(1,0,0,1,1,0,'Compliance Rules')
	   insert into se_generic_report values(1,0,0,0,0,0,'Restricted Securities')
	   insert into se_generic_report values(1,0,0,0,0,0,'Account Header')
	   insert into se_generic_report values(1,0,0,0,0,0,'Upcoming Corporate Actions')
	   insert into se_generic_report values(1,0,1,0,0,0,'Security Xref')
	   insert into se_generic_report values(1,0,0,0,0,0,'Missing Data')
	   insert into se_generic_report values(1,0,0,1,1,0,'Nav Rule Matrix')
	   insert into se_generic_report values(1,0,0,1,0,0,'Nav Results by Rule')
	   insert into se_generic_report values(1,0,0,1,0,0,'Nav Results Matric')
	   insert into se_generic_report values(1,0,0,1,0,0,'Nav Balances')
	   insert into se_generic_report values(1,0,0,1,0,0,'AsOf Portfolio')
	   insert into se_generic_report values(1,0,1,0,0,0,'Pre-rocess Restrictions')

	   select * from se_generic_report

	 

	 


  delete from se_generic_report
  