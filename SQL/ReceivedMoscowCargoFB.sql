select case
         when B.IS_CUSTOM = 1 then 'MVL'
         else 'VVL'
       end as VL,
       case
         when substring(B.AWB_FULL_NUM from 1 for 3) = '555' then 'SU'
         else 'OTHER'
       end as AIRLINE,
       round(sum(A.DIM_WEIGTH) / 100000) as WEIGHT
from C_DIM_AWB A, C_AWB B
where A.AWB_ID = B.ID_AWB and
      STRIPTIME(A.DAT) = @DBEGIN and
      B.TYPE_EXPIMP_AWB = 1 and
      A.STATE = 0
group by case
         when B.IS_CUSTOM = 1 then 'MVL'
         else 'VVL'
       end,
       case
         when substring(B.AWB_FULL_NUM from 1 for 3) = '555' then 'SU'
         else 'OTHER'
       end