WITH CELL
     AS (SELECT M.TU_INSTANCES.ID
           FROM M.TU_INSTANCES
          WHERE M.TU_INSTANCES.CELL IN
                   (SELECT M.CELLS.ID
                      FROM M.CELLS
                     WHERE M.CELLS.DEPOT IN
                              ('COMPL',
                               'UNCMP',
                               '$LDGEQP',
                               '$LDGREM',
                               'OUTVVL',
                               'OUTBOUND',
                               'OUTCUST')))
  SELECT A.TECH,
         CASE WHEN A.IS_IT_CUSTOM = 'X' THEN 'MVL' ELSE 'VVL' END AS VL,
         ROUND (SUM (CU.WEIGHT) / 1000) AS WEIGHT
    FROM (SELECT AWB_ID,
                 DECODE (WEIGHT,
                         NULL, NVL (ESTIMATED_WEIGHT, 0),
                         0, NVL (ESTIMATED_WEIGHT, 0),
                         WEIGHT)
                    AS WEIGHT
            FROM M.CARGO_UNIT
           WHERE TUNIT NOT IN (SELECT ID FROM CELL) AND BORN_DATE > sysdate-61) CU
         INNER JOIN M.AWB A ON CU.AWB_ID = A.AWB_ID
   WHERE A.TECH <> 'IMP'
GROUP BY CASE WHEN A.IS_IT_CUSTOM = 'X' THEN 'MVL' ELSE 'VVL' END, A.TECH