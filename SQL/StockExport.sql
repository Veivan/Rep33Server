SELECT TECH, VL, round(sum(ost_weight)/1000) AS WEIGHT FROM (
 select 
  case aw.type_expimp_awb when 1 then 'EXP' when 3 then 'TRF' else '-' end as TECH,
  case aw.is_custom when 1 then 'MVL' else 'VVL' end as VL,
  coalesce(cargo.weight,0) - coalesce(dep.weight,0) as ost_weight
 from ars_vcargo cargo
 inner join c_Awb aw on cargo.id_awb=aw.id_awb
 left join ars_vdep dep on cargo.id_awb=dep.id_awb
 left join ars_vman mn on cargo.id_awb=mn.id_awb
 left join (select max(ID_PK) as ID_PK,ID_AWB from c_awb_reis_reserv group by ID_AWB) ri on aw.id_awb=ri.ID_Awb
 left join c_awb_reis_reserv rr on rr.id_pk=ri.id_pk
 left join flight fl on fl.id_flight=rr.flight_id
 where aw.type_expimp_awb in (1,3) and  coalesce(aw.awb_comment,'-')<>'Tranzit ULD'
  and (coalesce(cargo.pieces, 0) - coalesce(dep.pieces, 0)) > 0
  AND (coalesce(cargo.weight,0) - coalesce(dep.weight,0)) > 0
  and cargo.dat >= cast(@DBEGIN AS date) - 20
   and aw.state_doc=1 and coalesce(cargo.weight, 0) <> 0
   ) t1
   GROUP BY  TECH, VL