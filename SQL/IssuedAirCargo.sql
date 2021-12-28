SELECT -ROUND (SUM (RD.WEIGHT) / 1000)
            AS weight,
         CASE
            WHEN a.AIRLINE_PREFIX = '555' THEN 'SU'
            WHEN a.AIRLINE_PREFIX = '580' THEN 'BRIDGE'
            WHEN a.AIRLINE_PREFIX = '770' or a.AIRLINE_PREFIX = '216' THEN 'NORD'
            ELSE 'OTHER'
         END
            AS AIRLINE,
         'CARGO'
            AS CARGO_TYPE
    FROM DOCUSR.DOC_AWB a
         INNER JOIN DOCUSR.DOC_AWB_RECEIPT_DISPATCH rd ON a.ID = rd.DOC_ID
   WHERE     rd.OPERATION_TYPE = 'EXP'
         AND TRUNC (RD.OPERATION_DATE, 'DD') = :DBEGIN 
         AND (a.AIRLINE_PREFIX <> '580' and a.TECHNOLOGY <> 'IMP' 
            OR a.AIRLINE_PREFIX = '580' AND a.TECHNOLOGY = 'EXP')
GROUP BY CASE
            WHEN a.AIRLINE_PREFIX = '555' THEN 'SU'
            WHEN a.AIRLINE_PREFIX = '580' THEN 'BRIDGE'
            WHEN a.AIRLINE_PREFIX = '770' or a.AIRLINE_PREFIX = '216' THEN 'NORD'
            ELSE 'OTHER'
         END,
         'CARGO'