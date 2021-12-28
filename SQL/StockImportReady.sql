WITH CELL
      AS (SELECT M.TU_INSTANCES.ID
            FROM     M.TU_INSTANCES
            WHERE  M.TU_INSTANCES.CELL IN (SELECT M.CELLS.ID
                                                     FROM   M.CELLS
                                                     WHERE  M.CELLS.DEPOT IN ('COMPL', 'UNCMP', 'COLMVL', 'COLVVL', 'DSTMVL', 'DSTVVL', 
                                                                        '$LDGEQP', '$LDGREM', 'OUTVVL', 'OUTBOUND', 'OUTCUST')))
SELECT    CASE WHEN A.IS_IT_CUSTOM = 'X' THEN 'MVL' ELSE 'VVL' END AS VL, ROUND(SUM(CU.WEIGHT) / 1000) AS WEIGHT
FROM        (SELECT AWB_ID, DECODE(WEIGHT,  NULL, NVL(ESTIMATED_WEIGHT, 0),  0, NVL(ESTIMATED_WEIGHT, 0),  WEIGHT) AS WEIGHT
             FROM   M.CARGO_UNIT
             WHERE  TUNIT NOT IN (SELECT ID FROM CELL) AND BORN_DATE > TO_DATE('19.08.2019', 'dd.mm.yyyy')) CU
            INNER JOIN M.AWB A ON CU.AWB_ID = A.AWB_ID
WHERE     A.TECH = 'IMP'
GROUP BY CASE WHEN A.IS_IT_CUSTOM = 'X' THEN 'MVL' ELSE 'VVL' END