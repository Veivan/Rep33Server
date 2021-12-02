
---//--- Выдано на другие СВХ в Москве
update REPORTER.RPT_QUERIES q
set Q.QUERY_TEXT = 
'select
    CASE 
    	WHEN F.Carrier IN (''SU'', ''D9'') THEN ''SU'' 
    	WHEN F.Carrier IN (''RU'', ''V8'', ''6L'', ''P3'') THEN ''BRIDGE'' 
    	ELSE ''OTHER'' 
    END AS AIRLINE,
    fua.ULD_PROCESSING_TECHNOLOGY as TECHNOLOGY, round(sum(FUA.WEIGHT)/1000) as WEIGHT
from DOCUSR.DOC_FLIGHT f
inner join DOCUSR.DOC_FLIGHT_ULD_TO_AWB fua on F.ID = fua.DOC_ID
where F.FLIGHT_TYPE = ''ARR''
    and fua.ULD_PROCESSING_TECHNOLOGY IN (''RampAgent'', ''WhsAgent''/*, ''WHS2''*/)
    and trunc(F.ST, ''dd'') = :DBEGIN 
group by  
    CASE 
    	WHEN F.Carrier IN (''SU'', ''D9'') THEN ''SU'' 
    	WHEN F.Carrier IN (''RU'', ''V8'', ''6L'', ''P3'') THEN ''BRIDGE'' 
    	ELSE ''OTHER'' 
    END,
    fua.ULD_PROCESSING_TECHNOLOGY'
where Q.QUERY_NAME = 'IssuedSVH';