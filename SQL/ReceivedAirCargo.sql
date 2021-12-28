  SELECT ROUND (
              SUM (
                 NVL (RD.WEIGHT, 0))
            / 1000)
            AS weight,
         CASE
            WHEN F.CARRIER = 'SU' THEN 'SU'
            WHEN F.CARRIER = 'RU' THEN 'BRIDGE'
            WHEN F.CARRIER IN ('N4','EO') THEN 'NORD'
            ELSE 'OTHER'
         END
            AS AIRLINE,
         'CARGO'
            AS CARGO_TYPE
    FROM DOCUSR.DOC_AWB a
         INNER JOIN DOCUSR.DOC_AWB_RECEIPT_DISPATCH rd ON a.ID = rd.DOC_ID
         INNER JOIN DOCUSR.DOC_FLIGHT f ON RD.FLIGHT_ID = F.ID
   WHERE     rd.OPERATION_TYPE = 'ARR'
         AND TRUNC (RD.OPERATION_DATE, 'DD') = :DBEGIN
         AND a.TECHNOLOGY <> 'EXP'
GROUP BY CASE
            WHEN F.CARRIER = 'SU' THEN 'SU'
            WHEN F.CARRIER = 'RU' THEN 'BRIDGE'
            WHEN F.CARRIER IN ('N4','EO') THEN 'NORD'
            ELSE 'OTHER'
         END,
         'CARGO'