
---//--- Прилетело на воздушных судах (почта)
update REPORTER.RPT_QUERIES q
set Q.QUERY_TEXT = 
'SELECT ID_FLON AS AIRLINE, ROUND (SUM (WEIGHT_PASSED) / 1000) AS WEIGHT
    FROM (SELECT NVL (M1.WEIGHT_PASSED, 0) AS WEIGHT_PASSED,
                 CASE
                    WHEN F.Carrier IN (''SU'', ''D9'') THEN ''SU''
                    WHEN F.Carrier IN (''RU'', ''V8'', ''6L'', ''P3'') THEN ''BRIDGE''
                    WHEN F.Carrier IN (''N4'', ''EO'') THEN ''NORD''
                    ELSE ''OTHER''
                 END
                    AS ID_FLON
            FROM DOCUSR.DOC_FLIGHT_MAIL M1
                 INNER JOIN DOCUSR.DOC_FLIGHT F ON f.ID = m1.DOC_ID
           WHERE     TRUNC (f.FLIGHT_DATE, ''DD'') = :DBEGIN
                 AND (f.STATUS NOT IN (''Draft'', ''Planned''))
                 AND ( (NVL (M1.WEIGHT_PASSED, 0) > 0))
                 AND f.FLIGHT_TYPE = ''ARR'') t1
GROUP BY ID_FLON'
where Q.QUERY_NAME = 'ReceivedAirMail';