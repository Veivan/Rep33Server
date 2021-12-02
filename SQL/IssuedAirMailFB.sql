SELECT 'MAIL' as CARGO_TYPE, ID_FLON as AIRLINE, round(sum(WEIGHT)/1000) as WEIGHT FROM (
SELECT case when AC in ('SU','D9') then 'SU'
                when AC in ('RU','V8','CC','P3') then 'BRIDGE' 
                else 'OTHER' end as ID_FLON, COALESCE(u.WEIGTH,0) AS WEIGHT 
FROM C_MANIF_ULD  u
JOIN FLIGHT f ON f.ID_FLIGHT=u.ID_FLIGHT
WHERE u.IS_MAIL > 0 AND f.TREIS = 1
AND cast(f.DAT_PLN AS date) = Cast(@DBEGIN AS date)  
)
GROUP BY  'MAIL', ID_FLON