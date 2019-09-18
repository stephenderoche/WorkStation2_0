
--exec se_get_nav_comments 12858,1


if exists (select * from sysobjects where name = 'se_update_nav_comments')
begin
	drop procedure se_update_nav_comments
	print 'PROCEDURE: se_update_nav_comments dropped'
end
go
create procedure se_update_nav_comments  --exec se_update_nav_comments -1,1,24831,'fff',41
(   @comment_id 				numeric(10),
	@comment_type			    bit,
	@nav_res_rule_result_id     numeric(10),
	@comment                    varchar(40),
	@current_user               numeric(10)
	
)
as
	declare @cmpl_long_text_id numeric(10);
	declare @cmpl_long_text_id2 numeric(10);
begin

If(@comment_id = -1)
  begin
  
        execute update_cmpl_long_text2  @cmpl_long_text_id output, @current_user
		select @cmpl_long_text_id2 = @cmpl_long_text_id + 1
		execute update_nav_rule_result @nav_res_rule_result_id, @cmpl_long_text_id, @cmpl_long_text_id2

		if(@comment_type = 1)
		execute add_cmpl_long_text_data2 @cmpl_long_text_id, @comment_type, @comment 
		else
		execute add_cmpl_long_text_data2 @cmpl_long_text_id2, @comment_type, @comment 
  end
else
    begin
	   
		   execute add_cmpl_long_text_data2 @comment_id, @comment_type, @comment 
		
	 end

end
go
if @@error = 0 print 'PROCEDURE: se_update_nav_comments created'
else print 'PROCEDURE: se_update_nav_comments error on creation'
go
