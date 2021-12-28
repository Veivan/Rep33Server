SELECT  round(sum(nvl(rd.WEIGHT,0)/1000)) AS weight
FROM     DOCUSR.doc_flight f
         INNER JOIN DOCUSR.DOC_AWB_RECEIPT_DISPATCH rd ON rd.FLIGHT_ID = f.ID AND rd.OPERATION_TYPE = 'ARR' AND rd.status = 'Approved'
         INNER JOIN DOCUSR.doc_awb A ON A.ID = rd.DOC_ID
WHERE F.FLIGHT_TYPE='ARR'  
    AND ( F.Carrier in ('RU', '6L','P3') OR (F.Carrier = 'V8' and A.AIRLINE_PREFIX = '580')  ) 
    AND A.TECHNOLOGY = 'IMP' 
    AND trunc(f.at, 'DD') = :DBEGIN    