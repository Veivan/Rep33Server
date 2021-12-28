  SELECT VALUENAME,
         ROUND(SUM (VALUE) / EXTRACT (DAY FROM LAST_DAY (ADD_MONTHS (:DBEGIN, -1))))
            AS VALUE
    FROM RPT_PREV_DATA_33B
   WHERE     EXTRACT (MONTH FROM DATEREPORT) =
                EXTRACT (MONTH FROM ADD_MONTHS (:DBEGIN, -1))
         AND EXTRACT (YEAR FROM DATEREPORT) =
                EXTRACT (YEAR FROM ADD_MONTHS (:DBEGIN, -1))
GROUP BY VALUENAME