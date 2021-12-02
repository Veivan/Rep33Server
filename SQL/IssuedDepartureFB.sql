select case
         when substring(FL.NUM_REIS from 1 for 2) in ('SU', 'D9') then 'SU'
         when substring(FL.NUM_REIS from 1 for 2) in ('RU', 'V8', 'CC', 'P3') then 'BRIDGE'
         else 'OTHER'
       end as AIRLINE,
       case
         when FL.TREIS = 1 then 'DEP'
         when FL.TREIS = 2 then 'ARR'
         else null
       end as FLIGHT_TYPE,
       count(NUM_REIS) as KOL
from FLIGHT FL
left join PERRON_ZAD PZ on PZ.FLIGHT_ID = FL.ID_FLIGHT
left join TYPE_VS TVS on TVS.ID_TVS = FL.TVS_ID
left join GET_COUNT_PERS_ZAD(PZ.ID_PERRON_ZAD) GCP on 1 = 1
where STRIPTIME(FL.DAT_PLN) = @DBEGIN and
      not(PZ.ID_PERRON_ZAD is null) and
      FL.TREIS IN (1, 2)
group by case
         when substring(FL.NUM_REIS from 1 for 2) in ('SU', 'D9') then 'SU'
         when substring(FL.NUM_REIS from 1 for 2) in ('RU', 'V8', 'CC', 'P3') then 'BRIDGE'
         else 'OTHER'
       end,
       case
         when FL.TREIS = 1 then 'DEP'
         when FL.TREIS = 2 then 'ARR'
         else null
       end
