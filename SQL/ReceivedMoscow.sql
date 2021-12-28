  SELECT CASE
            WHEN A.IS_CUSTOMS_CONTROLLED = 0 THEN 'VVL'
            WHEN A.IS_CUSTOMS_CONTROLLED = 1 THEN 'MVL'
            ELSE '?'
         END
            AS VL,
         CASE
            WHEN A.AIRLINE_PREFIX = '555' THEN 'AEROFLOT'
            WHEN A.AIRLINE_PREFIX IN ('216', '770') THEN 'NORD'
            ELSE 'OTHER'
         END
            AS AIRLINE,
         ROUND (SUM (RD.WEIGHT / 1000)) AS WEIGHT
    FROM DOCUSR.DOC_AWB A
         INNER JOIN DOCUSR.DOC_AWB_RECEIPT_DISPATCH RD ON A.ID = RD.DOC_ID
   WHERE     RD.OPERATION_TYPE = 'REC'
         AND TRUNC (RD.OPERATION_DATE, 'DD') = :DBEGIN
GROUP BY CASE
            WHEN A.IS_CUSTOMS_CONTROLLED = 0 THEN 'VVL'
            WHEN A.IS_CUSTOMS_CONTROLLED = 1 THEN 'MVL'
            ELSE '?'
         END,
         CASE
            WHEN A.AIRLINE_PREFIX = '555' THEN 'AEROFLOT'
            WHEN A.AIRLINE_PREFIX IN ('216', '770') THEN 'NORD'
            ELSE 'OTHER'
         END