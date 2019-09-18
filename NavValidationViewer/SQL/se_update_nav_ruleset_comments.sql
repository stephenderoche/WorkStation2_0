
--exec se_get_nav_comments 12858,1
--update_nav_rule_set_result 10065.00000000, 1, null, 13040.00000000, null, 13041.00000000, 0, null

if exists (select * from sysobjects where name = 'se_update_nav_ruleset_comments')
begin
	drop procedure se_update_nav_ruleset_comments
	print 'PROCEDURE: se_update_nav_ruleset_comments dropped'
end
go
create procedure se_update_nav_ruleset_comments  --exec se_update_nav_comments -1,1,24831,'fff',41
(   
	@first_or_dq_rev_comm_text_id 				numeric(10),
	@comment_type			    bit,
	@nav_res_ruleset_review_id	numeric(10),
	@nav_status_ok				tinyint,
	@comment                    varchar(40),
	@first_or_dq_rev_by			numeric(10),
	@second_rev_by				numeric(10)= 0,
	@second_rev_comment_text_id	numeric(10)= 0,
	@dq_review					tinyint = null,	
	@doc_url					nvarchar(255) = null
	
)
as
	declare @cmpl_long_text_id numeric(10);
	declare @cmpl_long_text_id2 numeric(10);
begin

If(@first_or_dq_rev_comm_text_id = -1)
  begin
  
        execute update_cmpl_long_text2  @cmpl_long_text_id output, @first_or_dq_rev_by
		select @cmpl_long_text_id2 = @cmpl_long_text_id + 1
		--execute update_nav_rule_result @nav_res_rule_result_id, @cmpl_long_text_id, @cmpl_long_text_id2
		execute update_nav_rule_set_result @nav_res_ruleset_review_id, @nav_status_ok, null, @cmpl_long_text_id, null, @cmpl_long_text_id2, 0, null
		if(@comment_type = 1)
		execute add_cmpl_long_text_data2 @cmpl_long_text_id, @comment_type, @comment 
		else
		execute add_cmpl_long_text_data2 @cmpl_long_text_id2, @comment_type, @comment 
  end
else
    begin
	   
		   execute add_cmpl_long_text_data2 @first_or_dq_rev_comm_text_id, @comment_type, @comment 
		
	 end

end
go
if @@error = 0 print 'PROCEDURE: se_update_nav_ruleset_comments created'
else print 'PROCEDURE: se_update_nav_ruleset_comments error on creation'
go
