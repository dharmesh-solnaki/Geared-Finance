import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { BaseResponse } from '../Models/common-models';
import { CommonSearch } from '../Models/common-search.model';
import {
  FundingCategory,
  FundingEquipmentResponse,
  FundingEquipmentType,
} from '../Models/equipmentTypes.model';

@Injectable({
  providedIn: 'root',
})
export class EquipmentService {
  private API_URL = `${environment.BASE_URL}/Equipment`;
  constructor(private _http: HttpClient) {}

  getFundingCategories(): Observable<FundingCategory[]> {
    return this._http.get<FundingCategory[]>(`${this.API_URL}/Categories`);
  }
  upsertEquipmentType(
    fundingEquipmentType: FundingEquipmentType
  ): Observable<void> {
    return this._http.post<void>(this.API_URL, fundingEquipmentType);
  }
  getEquipmentTypes(
    searchModel: CommonSearch
  ): Observable<BaseResponse<FundingEquipmentResponse>> {
    return this._http.post<BaseResponse<FundingEquipmentResponse>>(
      `${this.API_URL}/Equipments`,
      searchModel
    );
  }
  deleteEquipmentType(id: number): Observable<void> {
    return this._http.delete<void>(`${this.API_URL}?id=${id}`);
  }
}
