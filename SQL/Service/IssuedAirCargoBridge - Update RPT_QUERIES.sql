
---//--- Вылетело на воздушных (бриджи, груз)
update REPORTER.RPT_QUERIES q
set Q.QUERY_TEXT = 
'select ROUND(SUM(ARU.WEIGTH - ARU.RET_WEIGTH)/1000) as WEIGHT
from C_MANIF_ULD CMU
	inner join FLIGHT FL on FL.ID_FLIGHT = CMU.ID_FLIGHT
	inner join C_AWB_REIS_ULD ARU on ARU.ID_MANIF_ULD = CMU.ID_MANIF_ULD
	inner join C_AWB AW on AW.ID_AWB = ARU.ID_AWB
where
    FL.TREIS = 1 
    and ARU.ID_AWB is not null 
    and FL.DAT_FCT is not null 
    and STRIPTIME(FL.DAT_SCH) = @DBEGIN
    and AW.TYPE_EXPIMP_AWB = 1
    and (
        (substring(FL.NUM_REIS from 1 for 2) in (''RU'', ''6L'', ''P3''))
        or (substring(FL.NUM_REIS from 1 for 2) = ''V8'' and AW.AWB_AC_PREFIX = ''580'')
    )'
where Q.QUERY_NAME = 'IssuedAirCargoBridge';

