
---//--- Обслужено рейсов
update REPORTER.RPT_QUERIES q
set Q.QUERY_TEXT = 
'SELECT F.FLIGHT_TYPE,
         CASE
            WHEN F.CARRIER IN (''SU'', ''D9'') THEN ''SU''
            WHEN F.CARRIER IN (''RU'', ''V8'', ''6L'', ''P3'') THEN ''BRIDGE''
            WHEN F.CARRIER IN (''N4'', ''EO'') THEN ''NORD''
            ELSE ''OTHER''
         END
            AS AIRLINE,
         COUNT (F.ID) AS KOL FROM DOCUSR.DOC_FLIGHT f
 LEFT JOIN DOCUSR.DOC_FLIGHT_MAIL fm ON f.id = fm.DOC_ID 
 WHERE trunc(f."AT") = :DBEGIN AND FLIGHT_TYPE = ''ARR'' AND F.IS_SERVICED = 1 --AND COMPL_STARTED IS NOT NULL 
 AND (HAS_PROCESSING_PLAN = 1 OR  WH_DOCUMENTS_RECEIVED_DATE IS NOT NULL OR fm.RECEIVE_DATE IS NOT NULL)
 GROUP BY F.FLIGHT_TYPE,
         CASE
            WHEN F.CARRIER IN (''SU'', ''D9'') THEN ''SU''
            WHEN F.CARRIER IN (''RU'', ''V8'', ''6L'', ''P3'') THEN ''BRIDGE''
            WHEN F.CARRIER IN (''N4'', ''EO'') THEN ''NORD''
            ELSE ''OTHER''
         END         
UNION ALL
  SELECT F.FLIGHT_TYPE,
         CASE
            WHEN F.CARRIER IN (''SU'', ''D9'') THEN ''SU''
            WHEN F.CARRIER IN (''RU'', ''V8'', ''6L'', ''P3'') THEN ''BRIDGE''
            WHEN F.CARRIER IN (''N4'', ''EO'') THEN ''NORD''
            ELSE ''OTHER''
         END
            AS AIRLINE,
         COUNT (F.ID) AS KOL
    FROM DOCUSR.DOC_FLIGHT F
   WHERE     F.ID IN (SELECT DOC_ID
                        FROM DOCUSR.DOC_FLIGHT_CARGO_ULD u
                       WHERE u.DOC_FLIGHT_CARGO_ID IS NOT NULL)
         AND TRUNC (F.AT, ''DD'') = :DBEGIN
         AND F.IS_SERVICED = 1
         AND F.FLIGHT_TYPE = ''DEP''
         AND NOT F.CARRIER IN (''RU'', ''V8'', ''6L'', ''P3'')
GROUP BY F.FLIGHT_TYPE,
         CASE
            WHEN F.CARRIER IN (''SU'', ''D9'') THEN ''SU''
            WHEN F.CARRIER IN (''RU'', ''V8'', ''6L'', ''P3'') THEN ''BRIDGE''
            WHEN F.CARRIER IN (''N4'', ''EO'') THEN ''NORD''
            ELSE ''OTHER''
         END         
UNION ALL
  SELECT F.FLIGHT_TYPE,
         CASE WHEN F.CARRIER IN (''RU'', ''V8'', ''6L'', ''P3'') THEN ''BRIDGE'' END
            AS AIRLINE,
         COUNT (F.ID) AS KOL
    FROM DOCUSR.DOC_FLIGHT F
   WHERE     NOT F.ID IN
                    (  select DOC_ID from (SELECT DOC_ID,
                              SUM (
                                 CASE
                                    WHEN ULD_PROCESSING_TECHNOLOGY = ''NoProcess''
                                    THEN
                                       0
                                    ELSE
                                       1
                                 END)
                                 AS KOL
                         FROM DOCUSR.DOC_FLIGHT_CARGO_ULD where TRUNC (STATUS_MODIFIED, ''DD'') = :DBEGIN
                     GROUP BY DOC_ID
                       HAVING SUM (
                                 CASE
                                    WHEN ULD_PROCESSING_TECHNOLOGY =
                                            ''NoProcess''
                                    THEN
                                       0
                                    ELSE
                                       1
                                 END) = 0))
         AND TRUNC (F.AT, ''DD'') = :DBEGIN
         AND F.IS_SERVICED = 1
         AND F.CARRIER IN (''RU'', ''V8'', ''6L'', ''P3'')
         AND F.FLIGHT_TYPE <> ''ARR''
GROUP BY F.FLIGHT_TYPE,
         CASE WHEN F.CARRIER IN (''RU'', ''V8'', ''6L'', ''P3'') THEN ''BRIDGE'' END'
where Q.QUERY_NAME = 'IssuedDeparture';

