select case
         when substring(FL.NUM_REIS from 1 for 2) in ('SU', 'D9') then 'SU'
         when substring(FL.NUM_REIS from 1 for 2) in ('RU', 'V8', '6L', 'P3') then 'BRIDGE'
         when substring(FL.NUM_REIS from 1 for 2) in ('N4', 'EO') then 'NORD'
         else 'OTHER'
       end as AIRLINE,
       case AW.TECH
         when 1 then 'WhsAgent'
         when 2 then 'RampAgent'
         when 3 then 'WhsAgent'
         when 4 then 'RampAgent'
         else null
       end as TECHNOLOGY,
       round(sum(AW.WEIGHT)/1000) as WEIGHT
from FLIGHT FL
inner join ARS_REIS_AWB AW on FL.ID_FLIGHT = AW.ID_FLIGHT
left join C_AWB A on A.ID_AWB = AW.ID_AWB
where FL.TREIS = 2 and
      (AW.SVH_TO > 0 or (AW.TECH = 3 and
      A.SVH_TO > 0)) and
      AW.FLT_TYP = 1 and
      STRIPTIME(FL.DAT_SCH) = @DBEGIN
group by case
         when substring(FL.NUM_REIS from 1 for 2) in ('SU', 'D9') then 'SU'
         when substring(FL.NUM_REIS from 1 for 2) in ('RU', 'V8', '6L', 'P3') then 'BRIDGE'
         when substring(FL.NUM_REIS from 1 for 2) in ('N4', 'EO') then 'NORD'
         else 'OTHER'
         end,
       case AW.TECH
         when 1 then 'WhsAgent'
         when 2 then 'RampAgent'
         when 3 then 'WhsAgent'
         when 4 then 'RampAgent'
         else null
       end
