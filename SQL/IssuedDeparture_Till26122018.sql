  SELECT F.FLIGHT_TYPE,
         CASE
            WHEN F.CARRIER IN ('SU', 'D9') THEN 'SU'
            WHEN F.CARRIER IN ('RU', 'V8', 'CC', 'P3') THEN 'BRIDGE'
            ELSE 'OTHER'
         END
            AS AIRLINE,
         COUNT (F.ID) AS KOL
    FROM DOCUSR.DOC_FLIGHT F
   WHERE     F.ID IN
                (SELECT DOC_ID
                   FROM DOCUSR.DOC_FLIGHT_CARGO_ULD u
                  WHERE     u.DOC_FLIGHT_CARGO_ID IS NOT NULL
                        AND (   u.ULD_PROCESSING_TECHNOLOGY = 'MK'
                             OR u.ULD_PROCESSING_TECHNOLOGY_EXT = 'MK'))
         AND TRUNC (F.AT, 'DD') = :DBEGIN
         AND F.IS_SERVICED = 1
         AND F.FLIGHT_TYPE = 'ARR'
         AND NOT F.CARRIER IN ('RU', 'V8', 'CC', 'P3')
GROUP BY F.FLIGHT_TYPE,
         CASE
            WHEN F.CARRIER IN ('SU', 'D9') THEN 'SU'
            WHEN F.CARRIER IN ('RU', 'V8', 'CC', 'P3') THEN 'BRIDGE'
            ELSE 'OTHER'
         END
UNION ALL
  SELECT F.FLIGHT_TYPE,
         CASE
            WHEN F.CARRIER IN ('SU', 'D9') THEN 'SU'
            WHEN F.CARRIER IN ('RU', 'V8', 'CC', 'P3') THEN 'BRIDGE'
            ELSE 'OTHER'
         END
            AS AIRLINE,
         COUNT (F.ID) AS KOL
    FROM DOCUSR.DOC_FLIGHT F
   WHERE     F.ID IN (SELECT DOC_ID
                        FROM DOCUSR.DOC_FLIGHT_CARGO_ULD u
                       WHERE u.DOC_FLIGHT_CARGO_ID IS NOT NULL)
         AND TRUNC (F.AT, 'DD') = :DBEGIN
         AND F.IS_SERVICED = 1
         AND F.FLIGHT_TYPE = 'DEP'
         AND NOT F.CARRIER IN ('RU', 'V8', 'CC', 'P3')
GROUP BY F.FLIGHT_TYPE,
         CASE
            WHEN F.CARRIER IN ('SU', 'D9') THEN 'SU'
            WHEN F.CARRIER IN ('RU', 'V8', 'CC', 'P3') THEN 'BRIDGE'
            ELSE 'OTHER'
         END
UNION ALL
  SELECT F.FLIGHT_TYPE,
         CASE WHEN F.CARRIER IN ('RU', 'V8', 'CC', 'P3') THEN 'BRIDGE' END
            AS AIRLINE,
         COUNT (F.ID) AS KOL
    FROM DOCUSR.DOC_FLIGHT F
   WHERE     NOT F.ID IN
                    (  select DOC_ID from (SELECT DOC_ID,
                              SUM (
                                 CASE
                                    WHEN ULD_PROCESSING_TECHNOLOGY = 'NoProcess'
                                    THEN
                                       0
                                    ELSE
                                       1
                                 END)
                                 AS KOL
                         FROM DOCUSR.DOC_FLIGHT_CARGO_ULD where TRUNC (STATUS_MODIFIED, 'DD') = :DBEGIN
                     GROUP BY DOC_ID
                       HAVING SUM (
                                 CASE
                                    WHEN ULD_PROCESSING_TECHNOLOGY =
                                            'NoProcess'
                                    THEN
                                       0
                                    ELSE
                                       1
                                 END) = 0))
         AND TRUNC (F.AT, 'DD') = :DBEGIN
         AND F.IS_SERVICED = 1
         AND F.CARRIER IN ('RU', 'V8', 'CC', 'P3')
GROUP BY F.FLIGHT_TYPE,
         CASE WHEN F.CARRIER IN ('RU', 'V8', 'CC', 'P3') THEN 'BRIDGE' END