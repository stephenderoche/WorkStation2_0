
--exec se_get_nav_comments 12858,1


if exists (select * from sysobjects where name = 'se_get_nav_comments')
begin
	drop procedure se_get_nav_comments
	print 'PROCEDURE: se_get_nav_comments dropped'
end
go
create procedure se_get_nav_comments
(   @comment_id 				numeric(10),
	@comment_type			    bit
	
)
as
	
begin

select data from cmpl_long_text_data where cmpl_long_text_id = @comment_id

end
go
if @@error = 0 print 'PROCEDURE: se_get_nav_comments created'
else print 'PROCEDURE: se_get_nav_comments error on creation'
go
