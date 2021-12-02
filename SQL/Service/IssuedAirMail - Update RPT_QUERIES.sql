
---//--- Вылетело на воздушных судах (почта)
update REPORTER.RPT_QUERIES q
set Q.QUERY_TEXT = 
'SELECT    ''MAIL'' AS CARGO_TYPE,
            ID_FLON AS AIRLINE,
            ROUND(sum(WEIGHT - RETURN_WEIGHT + WEIGHT_TRNMAIL_ULD - WEIGHT_transit_RET) / 1000) AS WEIGHT
FROM        (SELECT NVL(M1.WEIGHT, 0) AS WEIGHT, NVL(M1.RETURN_WEIGHT, 0) AS RETURN_WEIGHT, 
                      NVL(tm.WEIGHT, 0) AS WEIGHT_transit, NVL(tm.TM_WEIGHT_DEP, 0) AS WEIGHT_transit_DEP, NVL(tm.TM_WEIGHT_RET, 0) AS WEIGHT_transit_RET, 
                      NVL(tm_uld.WEIGHT, 0) AS WEIGHT_TRNMAIL_ULD,
                      CASE 
                      	WHEN F.Carrier IN (''SU'', ''D9'') THEN ''SU'' 
                      	WHEN F.Carrier IN (''RU'', ''6L'', ''P3'', ''V8'') THEN ''BRIDGE'' 
                      	WHEN F.Carrier IN (''N4'', ''EO'') THEN ''NORD'' 
                      	ELSE ''OTHER'' 
                      END AS ID_FLON
             FROM   DOCUSR.DOC_FLIGHT F
                      LEFT JOIN DOCUSR.DOC_FLIGHT_MAIL M1 ON f.ID = m1.DOC_ID
                      LEFT JOIN (SELECT     MR.FLIGHT_ID, ABS(SUM(NVL(MR.WEIGHT, 0))) AS WEIGHT,
                                        ABS(SUM(case when MR.OPERATION_TYPE = ''TMDEP'' then NVL(MR.WEIGHT, 0) else 0 end)) as TM_WEIGHT_DEP,
                                        ABS(SUM(case when MR.OPERATION_TYPE = ''TMRET'' then NVL(MR.WEIGHT, 0) else 0 end)) as TM_WEIGHT_RET
                                     FROM      DOCUSR.DOC_TRANSIT_MAIL_RECEIPT mr
                                     GROUP BY MR.FLIGHT_ID) tm ON f.id = tm.FLIGHT_ID
                      LEFT JOIN (SELECT     TMU.DOC_ID, SUM(NVL(TMU.WEIGHT, 0)) AS WEIGHT
                                     FROM      DOCUSR.DOC_FLIGHT_TRNMAIL_ULD TMU
                                     GROUP BY TMU.DOC_ID) TM_ULD ON F.ID = TM_ULD.DOC_ID
             WHERE  TRUNC(f.ET, ''DD'') = :DBEGIN
             AND      (f.STATUS NOT IN (''Draft'', ''Planned''))
             AND      (NVL(M1.WEIGHT, 0) != 0 OR NVL(M1.RETURN_WEIGHT, 0) != 0 OR NVL(tm_uld.WEIGHT, 0) != 0 OR NVL(tm.TM_WEIGHT_RET, 0) != 0)
             AND      f.FLIGHT_TYPE = ''DEP'') t1
GROUP BY ID_FLON'
where Q.QUERY_NAME = 'IssuedAirMail';

