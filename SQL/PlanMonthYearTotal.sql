SELECT VALUENAME, sum(VALUE) * extract(day from :DBEGIN) AS value
FROM REPORTER.RPT_PLAN_DATA_33B
WHERE DATEREPORT BETWEEN trunc(:DBEGIN, 'MM') 
 AND :DBEGIN
GROUP BY VALUENAME