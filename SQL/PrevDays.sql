SELECT *
  FROM RPT_PREV_DATA_33B
 WHERE TRUNC (DATEREPORT, 'DD') BETWEEN TRUNC (:DBEGIN, 'MM')
                                    AND TRUNC (:DBEGIN, 'DD') - 1