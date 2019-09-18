if exists (select * from sysobjects where name = 'nq_get_nav_linked_reports')
begin
	drop proc nq_get_nav_linked_reports 
	print 'PROCEDURE: nq_get_nav_linked_reports  dropped'
end

GO


create procedure [dbo].[nq_get_nav_linked_reports]--exec nq_get_nav_linked_reports -1, 1
(	
	@nav_res_rule_result_id numeric(10)
	,@nav_report_order int
)
as
begin

	create table #report_design (nav_report_header_id numeric(10), nav_report_order smallint, column_name varchar(100), column_order smallint, column_name_rpt varchar(100), column_number_db int)
	create table #report_header_ids (id int identity(1, 1), nav_report_header_id numeric(10), nav_report_order int)

	if @nav_res_rule_result_id = -1
	begin
		select '' as nav_report_row_number
			,'' as text_color
			,'' as background_color
			, '' as column_1
		return
	end
	insert into #report_design (nav_report_header_id, nav_report_order, column_name, column_order, column_number_db)
	SELECT nav_report_header_id, nav_report_order, column_name, column_order, replace(replace(column_name, 'column_', ''), '_order', '')
	FROM (SELECT nav_report_header_id, nav_report_order, column_1_order, column_2_order, column_3_order,	column_4_order,	column_5_order,	column_6_order, column_7_order, column_8_order, column_9_order, column_10_order, column_11_order, column_12_order, column_13_order, column_14_order, column_15_order, 
		column_16_order, column_17_order, column_18_order, column_19_order, column_20_order, column_21_order, column_22_order, column_23_order, column_24_order, column_25_order, column_26_order, column_27_order, column_28_order, column_29_order, column_30_order
	from nav_report_header
	where nav_res_rule_result_id = @nav_res_rule_result_id 
		and nav_report_order = @nav_report_order
	) stu
	UNPIVOT
	(column_order FOR column_name IN (column_1_order, column_2_order, column_3_order,	column_4_order,	column_5_order,	column_6_order, column_7_order, column_8_order, column_9_order, column_10_order, column_11_order, column_12_order, column_13_order, column_14_order, column_15_order, 
		column_16_order, column_17_order, column_18_order, column_19_order, column_20_order, column_21_order, column_22_order, column_23_order, column_24_order, column_25_order, column_26_order, column_27_order, column_28_order, column_29_order, column_30_order)
	) AS column_order
	order by nav_report_header_id, nav_report_order	

	;with cte_column_name as (
		SELECT nav_report_header_id, nav_report_order, column_name, replace(replace(column_value, 'column_', ''), '_name', '') as column_number_db
		FROM (SELECT nav_report_header_id, nav_report_order, column_1_name, column_2_name, column_3_name,	column_4_name,	column_5_name,	column_6_name, column_7_name, column_8_name, column_9_name, column_10_name, column_11_name, column_12_name, column_13_name, column_14_name, column_15_name, 
			column_16_name, column_17_name, column_18_name, column_19_name, column_20_name, column_21_name, column_22_name, column_23_name, column_24_name, column_25_name, column_26_name, column_27_name, column_28_name, column_29_name, column_30_name
		from nav_report_header
		where nav_res_rule_result_id = @nav_res_rule_result_id and nav_report_order > 2
		) stu
		UNPIVOT
		(column_name FOR column_value IN (column_1_name, column_2_name, column_3_name,	column_4_name,	column_5_name,	column_6_name, column_7_name, column_8_name, column_9_name, column_10_name, column_11_name, column_12_name, column_13_name, column_14_name, column_15_name, 
			column_16_name, column_17_name, column_18_name, column_19_name, column_20_name, column_21_name, column_22_name, column_23_name, column_24_name, column_25_name, column_26_name, column_27_name, column_28_name, column_29_name, column_30_name)
		) AS column_name
	)

	update r
	set column_name_rpt = c.column_name
	from #report_design r 
		inner join cte_column_name c on c.nav_report_header_id = r.nav_report_header_id and c.column_number_db = r.column_number_db

	insert into #report_header_ids (nav_report_header_id, nav_report_order)
	select distinct nav_report_header_id, nav_report_order 
	from #report_design
	order by nav_report_order, nav_report_header_id

	declare @id as int
		,@current_id as int
		,@report_header_id as numeric(10)
		,@sql_str nvarchar(max)

	select @id = min(id) 
	from #report_header_ids

	while IsNull(@id, 0) > 0
	begin
		
		select @report_header_id = nav_report_header_id
		from #report_header_ids 
		where id = @id 

		;with cte_report as (
			select distinct
				column_number_db
				, column_name_rpt = 'column_' + cast(column_order as varchar(10))
				, header_name = IsNull(column_name_rpt, ' ')
			from #report_design
			where nav_report_header_id = @report_header_id
		)
		select @sql_str =
			(select N'
			select
				nav_report_row_number = 0, text_color = 0, background_color = 14861467, ' + stuff((
				select ', [' + r.column_name_rpt + '] = ''' + r.header_name + ''''
				from cte_report r
				for xml path ('')), 1, 1, '')
			) +
			(select N'
			union all select
				nav_report_row_number, text_color, background_color, ' + stuff((
				select ', [' + r.column_name_rpt + '] = ' + stuff((
					select '+ IsNull(' + c.name + ', '''')'
					from sys.columns c
						cross apply #report_design rd 
					where object_id = OBJECT_ID('dbo.nav_report_row')
						and rd.nav_report_header_id = @report_header_id
						and name like replace(replace(rd.column_name, 'order', '%'), 'column', 'col')
						and rd.column_number_db = r.column_number_db
					for xml path ('')), 1, 1, '')
				from cte_report r 
				for xml path ('')), 1, 1, '') + '
			from nav_report_row 
			where nav_report_header_id = ' + cast(@report_header_id as varchar(100)) + '
			order by nav_report_row_number'
		)

		exec sp_executesql @sql_str

		set @current_id = @id
		
		select @id = min(id) 
		from #report_header_ids
		where id > @current_id

	end



end

go
if exists (select * from sysobjects where name = 'nq_get_nav_linked_reports ')
begin
	print 'PROCEDURE: nq_get_nav_linked_reports  created'
end
else
begin
	print 'PROCEDURE: nq_get_nav_linked_reports  error on creation'
end
GO 
