declare @MInBlockID numeric(10)
declare @create_time datetime
declare @user_id numeric(10)
declare @name varchar(40)

select @MInBlockID = min(blocked_orders.block_id)    
		from blocked_orders where deleted = 0    
  
while @MInBlockID is not null    
	begin  

	select @create_time =  created_time from blocked_orders where block_id = @MInBlockID
	select @user_id =  created_by from blocked_orders where block_id = @MInBlockID
	select @name = name from user_info where user_id = @user_id

	insert into se_journaling values(@MInBlockID,@create_time,'Order Created', @name)

select @MInBlockID = min(blocked_orders.block_id)    
		from blocked_orders where deleted = 0 
		and blocked_orders.block_id > @MInBlockID
	end