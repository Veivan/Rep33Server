select case
         when substring(FL.NUM_REIS from 1 for 2) in ('SU', 'D9') then 'SU'
         when substring(FL.NUM_REIS from 1 for 2) in ('RU', 'V8', 'CC', 'P3') then 'BRIDGE'
         else 'OTHER'
       end as AIRLINE,
       round(sum(PR.AUPP_W)/1000) as WEIGHT
from FLIGHT FL
left join C_POST_REIS PR on PR.ID_FLIGHT = FL.ID_FLIGHT
left join GET_POST_REIS_EXP(FL.ID_FLIGHT) GPR on 1 = 1
left join TYPE_VS TVS on TVS.ID_TVS = FL.TVS_ID
left join C_TYPE_CARRIER CTC on CTC.IATA_CODE = FL.AC
left join C_TYPE_VOZVRAT CTV on CTV.ID_TVOZVR = PR.RETURN_TYPE
where STRIPTIME(FL.DAT_SCH) = @DBEGIN and
      (PR.AUPP_N + PR.AUPP_W + PR.AC_N + PR.AC_W + GPR.BULK_N + GPR.ULD_N + GPR.BULK_W + GPR.ULD_W + PR.RETURN_N + PR.RETURN_W + PR.CL_EMS_N + PR.CL_EMS_W + PR.CL_PSV_N + PR.CL_PSV_W) > 0
group by case
         when substring(FL.NUM_REIS from 1 for 2) in ('SU', 'D9') then 'SU'
         when substring(FL.NUM_REIS from 1 for 2) in ('RU', 'V8', 'CC', 'P3') then 'BRIDGE'
         else 'OTHER'
       end