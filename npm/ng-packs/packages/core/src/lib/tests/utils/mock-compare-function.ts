import { ABP } from "../../models";

 
export const mockCompareFunction = (a: ABP.Route, b: ABP.Route) => {
  const aName = a.name;
  const bName = b.name;
  const aNumber = a.order;
  const bNumber = b.order;
  
  if (!Number.isInteger(aNumber)) return 1;
  if (!Number.isInteger(bNumber)) return -1;

  if (aNumber > bNumber) return 1
  if (aNumber < bNumber) return -1
  
  if ( aName > bName ) return 1;
  if ( aName < bName ) return -1;

  return 0
}