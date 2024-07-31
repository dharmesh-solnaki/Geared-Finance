import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { BaseRespons, FundingCategory, FundingEquipmentResponse, FundingEquipmentType,   } from '../Models/common-models';
import { CommonSearch } from '../Models/common-search.model';


@Injectable({
  providedIn: 'root'
})
export class EquipmentService {
  
  private API_URL = `${environment.BASE_URL}/Equipment`
  constructor(private _http:HttpClient) { } 
 
  getFundingCategories():Observable<FundingCategory[]>{
    return this._http.get<FundingCategory[]>(`${this.API_URL}/GetAllCategories`)
  }
  upsertEquipmentType(fundingEquipmentType:FundingEquipmentType):Observable<void>{
   return this._http.post<void>(this.API_URL,fundingEquipmentType)
  }
  getEquipmentTypes(searchModel:CommonSearch):Observable<BaseRespons<FundingEquipmentResponse>>{
  return this._http.post<BaseRespons<FundingEquipmentResponse>>(`${this.API_URL}/GetEquipments`,searchModel);
  }

}
