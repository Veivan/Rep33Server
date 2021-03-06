select v.TECHNOLOGY,
    CASE WHEN v.IS_CUSTOMS_CONTROLLED = 1 THEN 'MVL' ELSE 'VVL' END AS VL,
    ROUND(sum(nvl(v.WEIGHT_READY_CU, 0)) / 1000) as WEIGHT_READY_CU,
    ROUND(sum(nvl(v.WEIGHT_PROCESS_CU, 0)) / 1000) as WEIGHT_PROCESS_CU,
    ROUND(sum(nvl(v.WEIGHT_OLD_CU, 0)) / 1000) as WEIGHT_OLD_CU,
    ROUND(sum(nvl(v.WEIGHT_READY_OLD_CU, 0)) / 1000) as WEIGHT_READY_OLD_CU,
    ROUND(sum(nvl(v.WEIGHT_CU, 0)) / 1000) as WEIGHT_CU    
from M.V_WHS_AWB_BALANCE v
where v.STATUS not in ('Archive', 'Draft', 'Cancelled')
    and (v.AIRLINE_PREFIX <> '580' or v.AIRLINE_PREFIX = '580' AND v.TECHNOLOGY <> 'TRN')
    and V.WEIGHT_AWB_RD > 0
group by v.TECHNOLOGY, v.IS_CUSTOMS_CONTROLLED