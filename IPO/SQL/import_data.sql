--drop table se_bb_Data

truncate table se_bb_Data

bulk insert se_bb_Data
from 'C:\Temp\ensign\BB intergration\bb_analytics.csv'
with (formatfile = 'C:\Temp\ensign\BB intergration\bb_data.fmt')


UPDATE se_bb_Data
SET se_bb_Data.security_id = security.security_id
FROM se_bb_Data
INNER JOIN security 
    ON se_bb_Data.symbol = security.user_id_2