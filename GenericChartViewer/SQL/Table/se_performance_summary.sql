
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[se_performance_summary](
	[account_id] [numeric](10, 0) NOT NULL,
	[performace_type_id] [numeric](10, 0) NOT NULL,
	[account_performace] [float] NULL,
	[benchmark_performace] [float] NULL
) ON [PRIMARY]

GO


