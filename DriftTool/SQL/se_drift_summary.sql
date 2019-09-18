drop table se_drift_summary

CREATE TABLE [dbo].[se_drift_summary](
	[on_hold] [bit] NOT NULL DEFAULT ((0)),
	[account_id] [numeric](10, 0) NOT NULL,
	[short_name] [nvarchar](40) NOT NULL,
	[model_name] [nvarchar](40) NOT NULL,
	[sector_drift] [nvarchar](11) NULL,
	[security_drift] [nvarchar](11) NULL,
	[cash] [float] NULL,
	[last_rebalance] [datetime] NULL,
	[next_rebalance] [datetime] NULL,
	[current_orders] [numeric](10, 0) NULL,
	[AutoRebalance]  [bit] NOT NULL DEFAULT ((0)),
	[Rebalance_frequency] [nvarchar](40) NOT NULL,
    Duration [float] NULL,
	Convexity [float] NULL,
	equity_percent float null,
	equity_model float null,
	debt_percent float null,
	debt_model float null

) 

GO


